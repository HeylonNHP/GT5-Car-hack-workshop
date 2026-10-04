using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using GT5_Car_hack_workshop.Models;
using GT5_Car_hack_workshop.Services;

namespace GT5_Car_hack_workshop
{
    /// <summary>
    /// Dialog replicating the in-game Tuning Shop: choose a car to borrow parts from, choose a
    /// category, then fit any number of those parts onto the current car for free. Fitting a part
    /// writes its key <em>and</em> sets the game's ownership PurchaseBit for the part's tier (see
    /// <see cref="PartInstaller"/>), which is what makes the game treat the part as bought and fitted.
    /// <para>
    /// Everything is edited in the save held in memory; the main window's save buttons are what write
    /// it to disk. On close the main window's part drop-downs are re-read, so the parts that were just
    /// installed are not reverted by the next save.
    /// </para>
    /// </summary>
    public partial class TuningShopWindow : Window
    {
        private readonly IFormManager? _formManager;

        /// <summary>Status-line colours: ordinary text, and red for anything that went wrong.</summary>
        private static readonly IBrush NormalStatusBrush = new SolidColorBrush(Color.Parse("#C8CDD4"));
        private static readonly IBrush ErrorStatusBrush = new SolidColorBrush(Color.Parse("#FF8A8A"));

        private MainWindow? _mainForm;
        private string? _openError;

        public TuningShopWindow(IFormManager formManager)
        {
            _formManager = formManager;
            InitializeComponent();
            InitializeBoxes();
            Opened += OnOpened;
            Closed += OnClosed;
        }

        // Parameterless constructor for the Avalonia designer.
        public TuningShopWindow()
        {
            InitializeComponent();
            InitializeBoxes();
            Opened += OnOpened;
            Closed += OnClosed;
        }

        /// <summary>
        /// One entry of the category filter: a field, or the "all installable parts" choice when
        /// <see cref="Category"/> is null. <see cref="ToString"/> is what the ComboBox shows.
        /// </summary>
        private sealed class CategoryChoice
        {
            public PartCategory? Category { get; init; }
            public string Label { get; init; } = "";
            public override string ToString() => Label;
        }

        /// <summary>Wires the fixed lists - the categories, and the filter on the source-car box.</summary>
        private void InitializeBoxes()
        {
            SourceCarBox.ItemFilter = SourceCarFilter;
            SourceCarBox.SelectionChanged += (_, _) => RefreshPartList();

            // The category filter lists "all installable parts" first, then one entry per field.
            var choices = new List<CategoryChoice> { new() { Category = null, Label = "All installable parts" } };
            foreach (var category in PartCatalogue.InstallableCategories)
                choices.Add(new CategoryChoice { Category = category, Label = category.Label });

            CategoryBox.ItemsSource = choices;
            CategoryBox.SelectedIndex = 0;
        }

        private void OnOpened(object? sender, EventArgs e)
        {
            _mainForm = _formManager?.MainForm;

            PopulateSourceCars();

            var save = _mainForm?.Gt5Save;
            if (save is null || save.Length == 0)
            {
                RefuseInstall("Load a GT5.0 save in the main window first, then reopen this window.");
                return;
            }

            if (_mainForm!.Moff <= 0)
            {
                RefuseInstall("The current car's data could not be placed in this save, so parts cannot be installed safely.");
                return;
            }

            RefreshPartList();
        }

        /// <summary>Disables the install buttons and shows why, for a save that cannot be edited.</summary>
        private void RefuseInstall(string reason)
        {
            _openError = reason;
            InstallSelectedButton.IsEnabled = false;
            InstallAllButton.IsEnabled = false;
            PartList.ItemsSource = null;
            SetStatus(reason, isError: true);
        }

        /// <summary>
        /// Fills the source-car picker from the catalogue and defaults it to the current car, which is
        /// resolved exactly as the main window resolves it: from the car's own engine / chassis /
        /// drivetrain keys through <see cref="PartCatalogueStore.FindCarId"/>.
        /// </summary>
        private void PopulateSourceCars()
        {
            SourceCarBox.ItemsSource = PartCatalogueStore.Cars;

            var current = PartCatalogueStore.FindCar(_mainForm?.CurrentCarId ?? 0);
            if (current is not null)
            {
                SourceCarBox.SelectedItem = current;
                SourceCarBox.Text = current.Name;
                InstallTargetText.Text = $"Installing onto: {current.Name}";
            }
            else
            {
                InstallTargetText.Text = "Installing onto: the current car";
            }
        }

        private void CategoryBox_SelectionChanged(object? sender, SelectionChangedEventArgs e) => RefreshPartList();

        /// <summary>
        /// Rebuilds the part list from the chosen source car and category, working out each row's
        /// fitted state and ownership bit from the in-memory save.
        /// </summary>
        private void RefreshPartList()
        {
            if (_openError is not null) return;

            var save = _mainForm?.Gt5Save;
            var moff = _mainForm?.Moff ?? 0;
            if (save is null || save.Length == 0 || moff <= 0)
            {
                PartList.ItemsSource = null;
                return;
            }

            if (SourceCarBox.SelectedItem is not TuningSourceCar sourceCar)
            {
                PartList.ItemsSource = null;
                SetStatus("Pick the car to borrow parts from using the box above.", isError: true);
                return;
            }

            var rows = new List<TuningPartRow>();
            foreach (var category in SelectedCategories())
            {
                foreach (var entry in PartCatalogueStore.EntriesForCar(category.TableId, sourceCar.Id))
                    rows.Add(BuildRow(save, moff, category, entry));
            }

            PartList.ItemsSource = rows;

            SetStatus(rows.Count == 0
                ? $"{sourceCar.Name} has no parts in this selection. Try another car or category."
                : $"{rows.Count} part(s) from {sourceCar.Name}. Select parts and click Install selected, or use Install all shown.");
        }

        /// <summary>The categories the filter stands for: all installable ones, or just the chosen field.</summary>
        private IEnumerable<PartCategory> SelectedCategories()
            => CategoryBox.SelectedItem is CategoryChoice { Category: { } chosen }
                ? new[] { chosen }
                : PartCatalogue.InstallableCategories;

        /// <summary>Builds one list row: its tier, the current car's value in the field, and its bit state.</summary>
        private static TuningPartRow BuildRow(byte[] save, int moff, PartCategory category, PartEntry entry)
        {
            ushort currentKey;
            try
            {
                currentKey = PartFieldCodec.Read(save, moff, category);
            }
            catch (Exception)
            {
                currentKey = 0;
            }

            var bits = PartInstaller.PurchaseBitsOf(category, entry);
            var topBit = bits.Count > 0 ? bits[^1] : (int?)null;

            var bitSet = false;
            if (topBit is { } bit)
            {
                var bitByte = moff + PartInstaller.PurchaseBitFromMoff + bit / 8;
                if (bitByte >= 0 && bitByte < save.Length)
                    bitSet = (save[bitByte] & (1 << (bit % 8))) != 0;
            }

            return new TuningPartRow
            {
                Part = entry,
                CategoryLabel = category.Label,
                TierName = TierNameOf(entry),
                CurrentKey = currentKey,
                IsFitted = currentKey == entry.PartKey,
                PurchaseBit = topBit,
                PurchaseBitSet = bitSet
            };
        }

        /// <summary>The tier's name: the game's item name for the level, "Stock" for level 0, or a plain level label.</summary>
        private static string TierNameOf(PartEntry entry)
        {
            if (entry.Level == 0) return "Stock";
            return string.IsNullOrWhiteSpace(entry.PartName) ? $"Level {entry.Level}" : entry.PartName;
        }

        private void InstallSelectedButton_Click(object? sender, RoutedEventArgs e)
        {
            var rows = PartList.SelectedItems?.Cast<TuningPartRow>().ToList();
            if (rows is null || rows.Count == 0)
            {
                SetStatus("Select one or more parts in the list first.", isError: true);
                return;
            }

            InstallRows(rows);
        }

        private void InstallAllButton_Click(object? sender, RoutedEventArgs e)
        {
            var rows = (PartList.ItemsSource as IEnumerable<TuningPartRow>)?.ToList();
            if (rows is null || rows.Count == 0)
            {
                SetStatus("There are no parts shown to install.", isError: true);
                return;
            }

            InstallRows(rows);
        }

        /// <summary>
        /// Installs every row in <paramref name="rows"/> onto the current car. The in-memory save is
        /// mutated in place, so each install is visible to the next. Rows are already in level order,
        /// so an "install all" leaves the highest tier's key in the field while every tier's ownership
        /// bit ends up set.
        /// </summary>
        private void InstallRows(IReadOnlyList<TuningPartRow> rows)
        {
            var save = _mainForm?.Gt5Save;
            var moff = _mainForm?.Moff ?? 0;
            if (save is null || save.Length == 0 || moff <= 0) return;

            var installed = 0;
            var bitsSet = 0;
            var flagsCleared = 0;
            var failed = 0;
            string? firstError = null;

            foreach (var row in rows)
            {
                try
                {
                    var currentKey = PartFieldCodec.Read(save, moff, PartCatalogue.Get(row.Part.Category));
                    var result = PartInstaller.Install(save, moff, row.Part, currentKey);

                    installed++;
                    bitsSet += result.BitsSet.Count;
                    if (result.FlagCleared) flagsCleared++;
                }
                catch (Exception ex)
                {
                    failed++;
                    firstError ??= $"{row.CategoryLabel}: {ex.Message}";
                }
            }

            // Put the freshly written parts into the main window's drop-downs straight away.
            _mainForm!.RefreshPartSelections();
            RefreshPartList();

            var message = $"Installed {installed} part(s); set {bitsSet} ownership bit(s); cleared {flagsCleared} leftover flag(s). "
                          + "Remember to save to write the changes to disk.";
            if (failed > 0)
                message += $" {failed} part(s) could not be installed ({firstError}).";

            SetStatus(message, isError: failed > 0);
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e) => Close();

        private void OnClosed(object? sender, EventArgs e)
        {
            // Without this the main window would save the part fields it last read, silently reverting
            // everything this dialog just installed.
            _mainForm?.RefreshPartSelections();
        }

        /// <summary>Matches typed text against a source car's name.</summary>
        private static bool SourceCarFilter(string? search, object? item)
            => item is TuningSourceCar car && PartCatalogueStore.MatchesCar(car, search);

        /// <summary>Shows a message on the single status line at the bottom of the dialog.</summary>
        private void SetStatus(string message, bool isError = false)
        {
            StatusText.Text = message;
            StatusText.Foreground = isError ? ErrorStatusBrush : NormalStatusBrush;
        }
    }
}
