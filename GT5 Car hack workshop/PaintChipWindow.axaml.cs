using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GT5_Car_hack_workshop.Models;
using GT5_Car_hack_workshop.Services;

namespace GT5_Car_hack_workshop
{
    /// <summary>
    /// Dialog that lets the user hand out paint chips ("Color Paint" items) in any colour from
    /// the paint database. Chips are added to the save that is held in memory; the main window's
    /// save buttons are what eventually write (and encrypt) it to disk.
    /// </summary>
    public partial class PaintChipWindow : Window
    {
        private readonly IFormManager? _formManager;
        private readonly StringBuilder _log = new();

        private MainWindow? _mainForm;
        private PaintChipStore? _store;
        private string? _openError;

        public PaintChipWindow(IFormManager formManager)
        {
            _formManager = formManager;
            InitializeComponent();
            InitializeSearchBox();
            Opened += OnOpened;
            Closed += OnClosed;
        }

        // Parameterless constructor for the Avalonia designer.
        public PaintChipWindow()
        {
            InitializeComponent();
            InitializeSearchBox();
            Opened += OnOpened;
            Closed += OnClosed;
        }

        private void InitializeSearchBox()
        {
            PaintSearchBox.ItemsSource = PaintDatabase.Entries;
            PaintSearchBox.ItemFilter = PaintItemFilter;
            PaintSearchBox.TextSelector = PaintTextSelector;
        }

        private void OnOpened(object? sender, EventArgs e)
        {
            _mainForm = _formManager?.MainForm;
            var save = _mainForm?.Gt5Save;

            if (save is null || save.Length == 0)
            {
                _openError = "Load a GT5.0 save in the main window first, then reopen this dialog.";
                AddButton.IsEnabled = false;
                SetStatus(_openError);
                UpdateOwnedChipsList();
                return;
            }

            if (!TryOpenStore(save, out var error))
            {
                _openError = "Could not open the save's item database: " + error;
                AddButton.IsEnabled = false;
                SetStatus(_openError);
                UpdateOwnedChipsList();
                return;
            }

            SetStatus($"Save loaded. Owned: {UpdateOwnedChipsList()}.");
        }

        /// <summary>
        /// (Re)opens the item database from the given save, disposing any previous handle so the
        /// dialog can never leak one or act on a stale copy.
        /// </summary>
        private bool TryOpenStore(byte[] save, out string error)
        {
            _store?.Dispose();
            _store = null;
            _openError = null;

            if (!PaintChipStore.TryOpen(save, out var store, out error) || store is null)
            {
                AddButton.IsEnabled = false;
                return false;
            }

            _store = store;
            AddButton.IsEnabled = true;
            return true;
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            _store?.Dispose();
            _store = null;
        }

        private void AddButton_Click(object? sender, RoutedEventArgs e)
        {
            if (_openError is not null)
            {
                SetStatus(_openError);
                return;
            }

            if (_store is null || _mainForm is null) return;

            var entry = PaintSearchBox.SelectedItem as PaintEntry ?? PaintDatabase.Resolve(PaintSearchBox.Text);
            if (entry is null)
            {
                SetStatus("Pick a paint colour from the search list first.");
                return;
            }

            var quantity = (int)Math.Clamp(QuantityBox.Value ?? 1m, 1m, 1000m);

            try
            {
                var added = _store.AddChips(entry.Id, entry.Category, entry.Maker, entry.Name, quantity);

                // Immediately write the edited database back into the in-memory save.
                _mainForm.Gt5Save = _store.WriteInto(_mainForm.Gt5Save);

                var ownedNow = _store.GetOwnedCount(entry.Id);
                SetStatus($"Added {added} x \"{entry.Name}\" ({entry.MakerName}, {entry.CategoryName}) - id {entry.Id:X4}. " +
                          $"This colour now has {ownedNow} chip(s); {UpdateOwnedChipsList()}. " +
                          "Remember to save to write the changes to disk.");
            }
            catch (Exception ex)
            {
                // Keep the store in step with the save: discard the un-written changes and reopen
                // from the (unchanged) in-memory save so a later Add cannot silently resurface them.
                TryOpenStore(_mainForm.Gt5Save, out _);
                UpdateOwnedChipsList();
                SetStatus("Could not add the chips: " + ex.Message);
            }
        }

        /// <summary>
        /// Rebuilds the "owned paint chips" list from the save, one row per colour with its
        /// quantity, and returns a short summary of what is owned.
        /// </summary>
        private string UpdateOwnedChipsList()
        {
            if (_store is null)
            {
                OwnedChipsList.ItemsSource = null;
                OwnedSummaryText.Text = "Owned paint chips";
                return "nothing owned";
            }

            var rows = new List<OwnedPaintChipRow>();
            var total = 0;
            foreach (var chip in _store.GetOwnedChips())
            {
                var entry = PaintDatabase.Find(chip.ColourId);
                rows.Add(new OwnedPaintChipRow
                {
                    Name = entry?.Name ?? $"(unknown colour {chip.ColourId:X4})",
                    Maker = entry?.MakerName ?? string.Empty,
                    Quantity = chip.Quantity
                });
                total += chip.Quantity;
            }

            rows.Sort((a, b) =>
            {
                var byName = string.Compare(a.Name, b.Name, StringComparison.CurrentCultureIgnoreCase);
                return byName != 0
                    ? byName
                    : string.Compare(a.Maker, b.Maker, StringComparison.CurrentCultureIgnoreCase);
            });
            OwnedChipsList.ItemsSource = rows;
            OwnedSummaryText.Text = $"Owned paint chips — {rows.Count} colour(s), {total} chip(s)";
            return $"{rows.Count} colour(s), {total} chip(s) in total";
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e) => Close();

        /// <summary>Matches typed text against a colour's name, maker or hex id.</summary>
        private static bool PaintItemFilter(string? search, object? item)
            => item is PaintEntry entry && PaintDatabase.MatchesSearch(entry, search);

        /// <summary>Shows the friendly colour description when an entry is picked.</summary>
        private static string? PaintTextSelector(string? text, object? item)
            => item is PaintEntry entry ? entry.ToString() : text;

        /// <summary>Shows <paramref name="message"/> at the top of the status log.</summary>
        private void SetStatus(string message)
        {
            _log.Insert(0, message + Environment.NewLine);
            StatusText.Text = _log.ToString().TrimEnd();
        }
    }
}
