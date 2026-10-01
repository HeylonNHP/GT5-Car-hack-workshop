using System.ComponentModel;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace GT5_Car_hack_workshop.Models
{
    /// <summary>
    /// One tile in the paint colour browser's grid: a paint colour plus whether it is the tile the
    /// user has currently picked. It is deliberately tiny, because the grid can hold all 3408
    /// colours and is virtualised.
    /// </summary>
    public sealed class PaintTile : INotifyPropertyChanged
    {
        private static readonly IBrush SelectedBorder = new ImmutableSolidColorBrush(Color.FromRgb(0xC0, 0xFF, 0xFF));

        public PaintTile(PaintEntry entry)
        {
            Entry = entry;
        }

        public PaintEntry Entry { get; }

        public string Name => Entry.Name;

        /// <summary>The maker and finish, shown under the name in a smaller, dimmer font.</summary>
        public string Detail => $"{Entry.MakerName} \u00b7 {Entry.CategoryName}";

        /// <summary>A brush for the colour itself, so the tile can show it.</summary>
        public IBrush SwatchBrush => Entry.SwatchBrush;

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectionBrush)));
            }
        }

        /// <summary>The tile's outline: bright when picked, invisible otherwise.</summary>
        public IBrush SelectionBrush => IsSelected ? SelectedBorder : Brushes.Transparent;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
