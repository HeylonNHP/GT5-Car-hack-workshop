using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using GT5_Car_hack_workshop.Models;

namespace GT5_Car_hack_workshop
{
    /// <summary>
    /// A dialog for choosing a single paint colour.
    ///
    /// It deliberately knows nothing about body or wheels paint: it lists the catalogue (narrowed by
    /// the search box and the finish filter) and hands the PaintEntry the user picked back to
    /// whoever opened it as the ShowDialog result. The caller decides what that colour is for.
    ///
    /// The tiles are laid out as rows of a virtualising list rather than in a wrap panel: Avalonia
    /// 12.1.2 has no virtualising wrap/uniform panel, and laying out all 3408 tiles at once measured
    /// at over a second, versus about 30 ms this way.
    /// </summary>
    public partial class PaintBrowserWindow : Window
    {
        /// <summary>The tile width in the grid, which has to match PaintTileTemplate.</summary>
        private const double TileWidth = 206;

        /// <summary>The tile's outer margin plus a little slack, used when working out the column count.</summary>
        private const double TileGap = 8;

        // Where this dialog remembers its sort in the app's settings file (the save path and PSN
        // own the first two lines, so they are left alone).
        private const string SettingsName = "GT5CHWsettings.ini";
        private const int SortSettingIndex = 2;
        private const int DirectionSettingIndex = 3;

        private readonly List<PaintTile> _tiles = new();
        private PaintEntry? _selectedEntry;
        private int _columns;
        private bool _loadingSettings;

        public PaintBrowserWindow()
        {
            InitializeComponent();

            _loadingSettings = true;

            BrowserFinishBox.ItemsSource = PaintDatabase.Finishes;
            BrowserFinishBox.SelectedIndex = 0;

            // Start on the sort the user last chose.
            var settings = SettingsFileClass.LoadSettings(SettingsName, DirectionSettingIndex);
            BrowserSortBox.ItemsSource = PaintSortOptions.All;
            BrowserSortBox.SelectedItem = PaintSortOptions.Find(settings[SortSettingIndex]);
            BrowserDescendingCheckBox.IsChecked =
                string.Equals(settings[DirectionSettingIndex].Trim(), "Descending", StringComparison.OrdinalIgnoreCase);

            _loadingSettings = false;

            ColourList.SizeChanged += (_, _) => UpdateColumns();
            Closed += (_, _) => SaveSort();

            Loaded += (_, _) => BrowserSearchBox.Focus();
            Refresh();
        }

        /// <summary>The colour the user chose, or null if they closed the dialog without choosing.</summary>
        public PaintEntry? SelectedColour { get; private set; }

        private void BrowserSearchBox_TextChanged(object? sender, TextChangedEventArgs e) => Refresh();

        private void BrowserFinishBox_SelectionChanged(object? sender, SelectionChangedEventArgs e) => Refresh();

        private void BrowserSortBox_SelectionChanged(object? sender, SelectionChangedEventArgs e) => Refresh();

        private void BrowserDescendingCheckBox_IsCheckedChanged(object? sender, RoutedEventArgs e) => Refresh();

        /// <summary>Remembers the chosen sort for next time.</summary>
        private void SaveSort()
        {
            if (BrowserSortBox.SelectedItem is not PaintSortOption sort) return;

            SettingsFileClass.SaveSetting(SettingsName, SortSettingIndex, sort.Name);
            SettingsFileClass.SaveSetting(SettingsName, DirectionSettingIndex,
                BrowserDescendingCheckBox.IsChecked == true ? "Descending" : "Ascending");
        }

        /// <summary>How many tiles fit across the grid at the current width.</summary>
        private int ColumnsForWidth()
        {
            var width = ColourList.Bounds.Width;
            if (width <= 0) width = Math.Max(TileWidth, Width - 46); // before the first layout pass
            return Math.Max(1, (int)(width / (TileWidth + TileGap)));
        }

        /// <summary>Re-flows the grid when a resize changes how many tiles fit per row.</summary>
        private void UpdateColumns()
        {
            if (ColumnsForWidth() == _columns) return;
            Refresh();
        }

        /// <summary>
        /// Rebuilds the grid from the catalogue: the chosen finish first, then the search text, then
        /// the chosen sort. The tiles are chunked into rows so the list virtualises and only the
        /// visible rows are built.
        /// </summary>
        private void Refresh()
        {
            if (_loadingSettings) return;

            var finish = (BrowserFinishBox.SelectedItem as PaintFinish)?.Category;
            var search = BrowserSearchBox.Text;
            var sort = BrowserSortBox.SelectedItem as PaintSortOption ?? PaintSortOptions.All[0];
            var direction = BrowserDescendingCheckBox.IsChecked == true
                ? PaintSortDirection.Descending
                : PaintSortDirection.Ascending;

            var entries = PaintSortOptions.Order(
                    PaintDatabase.ByFinish(finish)
                        .Where(entry => PaintDatabase.MatchesSearch(entry, search)),
                    sort, direction)
                .ToList();

            _columns = ColumnsForWidth();

            _tiles.Clear();
            foreach (var entry in entries)
                _tiles.Add(new PaintTile(entry) { IsSelected = ReferenceEquals(entry, _selectedEntry) });

            var rows = new List<PaintRow>();
            for (var i = 0; i < _tiles.Count; i += _columns)
                rows.Add(new PaintRow { Tiles = _tiles.GetRange(i, Math.Min(_columns, _tiles.Count - i)) });

            ColourList.ItemsSource = rows;

            var count = entries.Count == 0
                ? "No colours match."
                : $"{entries.Count} of {PaintDatabase.Entries.Count} colours";
            SelectionSummaryText.Text = _selectedEntry is not null
                ? $"{count} - selected: {_selectedEntry}"
                : $"{count} - click one to select it.";
        }

        private void ColourTile_PointerPressed(object? sender, PointerPressedEventArgs e) => SelectFrom(sender);

        private void ColourTile_DoubleTapped(object? sender, TappedEventArgs e)
        {
            if (SelectFrom(sender)) Confirm();
        }

        /// <summary>Marks the tile under <paramref name="sender"/> as picked. Returns whether it was a tile.</summary>
        private bool SelectFrom(object? sender)
        {
            // The row template can be asked for a null item while the list recycles containers.
            if (sender is not Control { DataContext: PaintTile tile }) return false;

            foreach (var other in _tiles)
                if (other.IsSelected && !ReferenceEquals(other, tile))
                    other.IsSelected = false;

            tile.IsSelected = true;
            _selectedEntry = tile.Entry;
            SelectedColour = tile.Entry;
            UseColourButton.IsEnabled = true;
            SelectionSummaryText.Text = tile.Entry.ToString();
            return true;
        }

        private void UseColourButton_Click(object? sender, RoutedEventArgs e) => Confirm();

        private void Confirm()
        {
            if (SelectedColour is null) return;
            Close(SelectedColour);
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e) => Close((PaintEntry?)null);
    }
}
