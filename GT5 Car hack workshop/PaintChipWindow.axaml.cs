using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using GT5_Car_hack_workshop.Models;
using GT5_Car_hack_workshop.Services;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace GT5_Car_hack_workshop
{
    /// <summary>
    /// Dialog for managing paint chips ("Color Paint" items): hand out chips in any colour from
    /// the paint database, and right-click a colour already owned to add more of it or delete it.
    /// Chips are edited in the save that is held in memory; the main window's save buttons are
    /// what eventually write (and encrypt) it to disk.
    /// </summary>
    public partial class PaintChipWindow : Window
    {
        private readonly IFormManager? _formManager;

        /// <summary>Status-line colours: ordinary text, and red for anything that went wrong.</summary>
        private static readonly IBrush NormalStatusBrush = new SolidColorBrush(Color.Parse("#C8CDD4"));
        private static readonly IBrush ErrorStatusBrush = new SolidColorBrush(Color.Parse("#FF8A8A"));

        private MainWindow? _mainForm;
        private PaintChipStore? _store;
        private string? _openError;

        /// <summary>The chip row the right-click menu was opened on, if any.</summary>
        private OwnedPaintChipRow? _contextRow;

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
            PaintSearchBox.ItemFilter = PaintItemFilter;
            // Deliberately no TextSelector: Avalonia hands a text selector the already-formatted
            // string rather than the entry, and the default formatting already shows the colour's
            // friendly description (PaintEntry.ToString()).

            // Finish filter: narrows the list to one finish (Metallic, Chrome, ...).
            PaintFinishBox.ItemsSource = PaintDatabase.Finishes;
            PaintFinishBox.SelectedIndex = 0;
            RefreshPaintList();
        }

        private void PaintFinishBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => RefreshPaintList();

        /// <summary>
        /// Points the search box at the colours of the chosen finish (every colour for "Any
        /// finish"). Replacing ItemsSource is what makes the drop-down, and its search, show only
        /// that finish.
        /// </summary>
        private void RefreshPaintList()
            => PaintSearchBox.ItemsSource = PaintDatabase.ByFinish((PaintFinishBox.SelectedItem as PaintFinish)?.Category);

        /// <summary>
        /// Opens the colour browser and, if a colour is chosen, shows it in the search box ready to
        /// be added. The browser only returns a colour; this dialog decides what it is for.
        /// </summary>
        private async void PaintBrowseButton_Click(object? sender, RoutedEventArgs e)
        {
            var chosen = await new PaintBrowserWindow().ShowDialog<PaintEntry?>(this);
            if (chosen is null) return;

            // Make sure the colour is in the finish-filtered list before selecting it, then show it
            // (the same order SelectOwnedChip uses).
            var finish = PaintDatabase.Finishes.FirstOrDefault(f => f.Category == chosen.Category);
            if (finish is not null) PaintFinishBox.SelectedItem = finish;

            PaintSearchBox.SelectedItem = chosen;
            PaintSearchBox.Text = chosen.ToString();
        }

        private void OnOpened(object? sender, EventArgs e)
        {
            _mainForm = _formManager?.MainForm;
            var save = _mainForm?.Gt5Save;

            if (save is null || save.Length == 0)
            {
                _openError = "Load a GT5.0 save in the main window first, then reopen this dialog.";
                AddButton.IsEnabled = false;
                SetStatus(_openError, isError: true);
                UpdateOwnedChipsList();
                return;
            }

            if (!TryOpenStore(save, out var error))
            {
                _openError = "Could not open the save's item database: " + error;
                AddButton.IsEnabled = false;
                SetStatus(_openError, isError: true);
                UpdateOwnedChipsList();
                return;
            }

            ShowOwnedSummary();
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
                SetStatus(_openError, isError: true);
                return;
            }

            if (_store is null || _mainForm is null) return;

            var entry = PaintSearchBox.SelectedItem as PaintEntry ?? PaintDatabase.Resolve(PaintSearchBox.Text);
            if (entry is null)
            {
                SetStatus("Pick a paint colour from the search list first.", isError: true);
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
                SetStatus("Could not add the chips: " + ex.Message, isError: true);
            }
        }

        /// <summary>
        /// Prepares the right-click menu for whichever chip row the pointer is over. The menu is
        /// only meaningful with a row under it and a database open, so the items are enabled or
        /// disabled to match, and the headers pick up the current quantities.
        /// </summary>
        private void OwnedChipsList_ContextRequested(object? sender, ContextRequestedEventArgs e)
        {
            // The list box also raises this for its own background and its scroll bar, so only act
            // when the click really landed inside a row (i.e. on a ListBoxItem).
            var row = (e.Source as Visual)?
                .GetSelfAndVisualAncestors()
                .OfType<ListBoxItem>()
                .Select(item => item.DataContext)
                .OfType<OwnedPaintChipRow>()
                .FirstOrDefault();

            // A menu opened from the keyboard (Shift+F10) has no pointer, so fall back to the
            // current selection.
            PrepareChipMenu(row ?? OwnedChipsList.SelectedItem as OwnedPaintChipRow);
        }

        /// <summary>
        /// Points the right-click menu at <paramref name="row"/>: enables only what that row
        /// allows, and fills in the quantities it will act on. Kept separate from the event
        /// plumbing so it can be exercised without a pointer.
        /// </summary>
        private void PrepareChipMenu(OwnedPaintChipRow? row)
        {
            _contextRow = row;

            var canEdit = row is not null && _store is not null && _mainForm is not null && _openError is null;

            // A colour this build's paint catalogue does not know about cannot be added again,
            // since the insert needs the paint's category - but it can still be deleted.
            var canAdd = canEdit && PaintDatabase.Find(row!.ColourId) is not null;

            AddOneMenuItem.IsEnabled = canAdd;
            AddMoreMenuItem.IsEnabled = canAdd;
            DeleteOneMenuItem.IsEnabled = canEdit;
            DeleteColourMenuItem.IsEnabled = canEdit;

            if (!canEdit) return;

            var quantity = (int)Math.Clamp(QuantityBox.Value ?? 1m, 1m, 1000m);
            AddMoreMenuItem.Header = $"Add {quantity} chip(s) (uses Quantity above)";
            DeleteColourMenuItem.Header = row!.Quantity == 1
                ? "Delete this colour…"
                : $"Delete all {row.Quantity} chips of this colour…";

            OwnedChipsList.SelectedItem = row;
        }

        private void AddOneMenuItem_Click(object? sender, RoutedEventArgs e) => AddMoreOfContextColour(1);

        private void AddMoreMenuItem_Click(object? sender, RoutedEventArgs e) =>
            AddMoreOfContextColour((int)Math.Clamp(QuantityBox.Value ?? 1m, 1m, 1000m));

        private void DeleteOneMenuItem_Click(object? sender, RoutedEventArgs e) => DeleteContextColour(1);

        private async void DeleteColourMenuItem_Click(object? sender, RoutedEventArgs e)
        {
            if (_contextRow is null) return;

            var confirmed = await ConfirmAsync(
                $"Delete all {_contextRow.Quantity} chip(s) of \"{_contextRow.Display}\"?\n\n" +
                "The save held in memory changes straight away; it reaches disk when you use Save only " +
                "or Save and encrypt on the main window. There is no undo here.");

            if (confirmed) DeleteContextColour(null);
        }

        /// <summary>
        /// Adds <paramref name="quantity"/> more chips of the right-clicked colour, in the same
        /// order the Add button uses: edit the database, write it back into the in-memory save,
        /// then report. On failure the store is reopened from the unchanged save, so an edit that
        /// could not be written cannot resurface later.
        /// </summary>
        private void AddMoreOfContextColour(int quantity)
        {
            if (_contextRow is null || _store is null || _mainForm is null) return;

            var entry = PaintDatabase.Find(_contextRow.ColourId);
            if (entry is null)
            {
                SetStatus($"\"{_contextRow.Display}\" is not in this build's paint catalogue, so more chips of it cannot be added.", isError: true);
                return;
            }

            try
            {
                var added = _store.AddChips(entry.Id, entry.Category, entry.Maker, entry.Name, quantity);

                // Immediately write the edited database back into the in-memory save.
                _mainForm.Gt5Save = _store.WriteInto(_mainForm.Gt5Save);

                var ownedNow = _store.GetOwnedCount(entry.Id);
                SetStatus($"Added {added} more chip(s) of \"{entry.Name}\" ({entry.MakerName}, {entry.CategoryName}) - id {entry.Id:X4}. " +
                          $"This colour now has {ownedNow} chip(s); {UpdateOwnedChipsList()}. " +
                          "Remember to save to write the changes to disk.");
            }
            catch (Exception ex)
            {
                TryOpenStore(_mainForm.Gt5Save, out _);
                UpdateOwnedChipsList();
                SetStatus("Could not add the chips: " + ex.Message, isError: true);
            }
        }

        /// <summary>
        /// Deletes <paramref name="count"/> chips of the right-clicked colour, or every chip of it
        /// when <paramref name="count"/> is null.
        /// </summary>
        private void DeleteContextColour(int? count)
        {
            if (_contextRow is null || _store is null || _mainForm is null) return;

            var colourId = _contextRow.ColourId;
            var description = _contextRow.Display;

            try
            {
                var removed = count is null
                    ? _store.RemoveAllOfColour(colourId)
                    : _store.RemoveChips(colourId, count.Value);

                _mainForm.Gt5Save = _store.WriteInto(_mainForm.Gt5Save);

                var ownedNow = _store.GetOwnedCount(colourId);
                var outcome = ownedNow == 0
                    ? $"Removed the last {removed} chip(s) of \"{description}\" - this colour is no longer owned."
                    : $"Removed {removed} chip(s) of \"{description}\"; it now has {ownedNow} chip(s).";

                SetStatus($"{outcome} {UpdateOwnedChipsList()}. Remember to save to write the changes to disk.");
            }
            catch (Exception ex)
            {
                TryOpenStore(_mainForm.Gt5Save, out _);
                UpdateOwnedChipsList();
                SetStatus("Could not delete the chips: " + ex.Message, isError: true);
            }
        }

        /// <summary>Asks a yes/no question, returning whether the user chose yes.</summary>
        private async Task<bool> ConfirmAsync(string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Manage Paint Chips", message, ButtonEnum.YesNo);
            return await box.ShowAsync() == ButtonResult.Yes;
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
                return "nothing owned";
            }

            var rows = new List<OwnedPaintChipRow>();
            var total = 0;
            foreach (var chip in _store.GetOwnedChips())
            {
                var entry = PaintDatabase.Find(chip.ColourId);
                rows.Add(new OwnedPaintChipRow
                {
                    ColourId = chip.ColourId,
                    Name = entry?.Name ?? $"(unknown colour {chip.ColourId:X4})",
                    Maker = entry?.MakerName ?? string.Empty,
                    Quantity = chip.Quantity,
                    // Grey when the catalogue has no colour for this id, so the column stays aligned.
                    SwatchBrush = entry?.SwatchBrush ?? Brushes.Gray
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
            return $"{rows.Count} colour(s), {total} chip(s) in total";
        }

        /// <summary>
        /// Puts the double-clicked colour into the "Paint colour" search box so the user can set a
        /// quantity and press Add to get more of the same chip. A double-click never adds chips by
        /// itself.
        /// </summary>
        private void OwnedChipsList_DoubleTapped(object? sender, TappedEventArgs e)
        {
            // The list box also raises this for its own background and for its scroll bar, so only
            // act when the double-click really landed inside a row (i.e. on a ListBoxItem).
            var row = (e.Source as Visual)?
                .GetSelfAndVisualAncestors()
                .OfType<ListBoxItem>()
                .Select(item => item.DataContext)
                .OfType<OwnedPaintChipRow>()
                .FirstOrDefault();

            if (row is null) return;

            if (SelectOwnedChip(row))
                e.Handled = true;
        }

        /// <summary>
        /// Selects <paramref name="row"/>'s colour in the "Paint colour" search box and moves focus
        /// to the quantity box so a number can be typed straight away. Nothing is added here.
        /// Returns whether a colour was actually selected.
        /// </summary>
        private bool SelectOwnedChip(OwnedPaintChipRow row)
        {
            var entry = PaintDatabase.Find(row.ColourId);
            if (entry is null)
            {
                // The save owns a colour this build's catalogue does not know about, so there is no
                // PaintEntry to hand to the search box.
                SetStatus($"\"{row.Name}\" is not in the paint catalogue, so it cannot be selected here.", isError: true);
                return false;
            }

            // Make sure the entry is in the current (finish-filtered) list before selecting it.
            var finish = PaintDatabase.Finishes.FirstOrDefault(f => f.Category == entry.Category);
            if (finish is not null) PaintFinishBox.SelectedItem = finish;

            // Find returns the very instance held in PaintSearchBox.ItemsSource, so this is a real
            // selection rather than just some text. The text is set explicitly too, because setting
            // the selection programmatically does not update the box's text on its own.
            PaintSearchBox.SelectedItem = entry;
            PaintSearchBox.Text = entry.ToString();
            QuantityBox.Focus();
            SetStatus($"Selected \"{entry.Name}\" ({entry.MakerName}, {entry.CategoryName}) - id {entry.Id:X4}. " +
                      "Set a quantity and press Add to get more of this chip.");
            return true;
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e) => Close();

        /// <summary>Matches typed text against a colour's name, maker or hex id.</summary>
        private static bool PaintItemFilter(string? search, object? item)
            => item is PaintEntry entry && PaintDatabase.MatchesSearch(entry, search);

        /// <summary>
        /// Shows <paramref name="message"/> on the single status line at the bottom of the dialog.
        /// Each message replaces the last one, so the newest - including any failure - is always
        /// the one on screen. <see cref="ShowOwnedSummary"/> puts the owned totals back.
        /// </summary>
        private void SetStatus(string message, bool isError = false)
        {
            StatusText.Text = message;
            StatusText.Foreground = isError ? ErrorStatusBrush : NormalStatusBrush;
        }

        /// <summary>Shows the owned totals on the status line, or a hint when nothing is owned.</summary>
        private void ShowOwnedSummary()
        {
            var summary = UpdateOwnedChipsList();

            SetStatus(summary == "nothing owned" || summary.StartsWith("0 ")
                ? "No paint chips owned yet - pick a colour above and click Add."
                : $"Owned: {summary}.");
        }
    }
}
