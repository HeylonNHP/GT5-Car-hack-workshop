using System;
using System.Collections.Generic;
using System.Linq;

namespace GT5_Car_hack_workshop.Models
{
    /// <summary>
    /// Which way a sort runs. The comparers below always compare ascending; the direction is applied
    /// by the sort operator itself (OrderBy / OrderByDescending), which is how LINQ does it too.
    /// </summary>
    public enum PaintSortDirection
    {
        Ascending,
        Descending,
    }

    /// <summary>
    /// One way of ordering paint colours: a name plus its ascending comparer. Same shape as
    /// PaintFinish, so it binds straight to a combo box and reads the same way everywhere.
    /// </summary>
    public sealed class PaintSortOption
    {
        public PaintSortOption(string name, IComparer<PaintEntry> comparer, bool neutralsLast = false)
        {
            Name = name;
            Comparer = comparer;
            NeutralsLast = neutralsLast;
        }

        public string Name { get; }

        /// <summary>Compares two entries ascending. Ties keep the catalogue's own order.</summary>
        public IComparer<PaintEntry> Comparer { get; }

        /// <summary>
        /// Whether colours with no hue (grey, white, black) are grouped at the end of the list. True
        /// for the sorts that would otherwise pile them all up at one end.
        /// </summary>
        public bool NeutralsLast { get; }

        public override string ToString() => Name;
    }

    /// <summary>
    /// The sorts a paint picker offers, plus the one place that applies them. One ascending comparer
    /// per option is the whole strategy: the option object carries the behaviour, and the list below
    /// is the registry the combo box binds to.
    /// </summary>
    public static class PaintSortOptions
    {
        // Declared before All, because static initialisers run in declaration order.
        private static readonly IComparer<PaintEntry> ByName =
            Comparer<PaintEntry>.Create((a, b) => string.Compare(a.Name, b.Name, StringComparison.CurrentCultureIgnoreCase));

        private static readonly IComparer<PaintEntry> ByFinish =
            Comparer<PaintEntry>.Create((a, b) => a.Category.CompareTo(b.Category));

        private static readonly IComparer<PaintEntry> ByMaker =
            Comparer<PaintEntry>.Create((a, b) => string.Compare(a.MakerName, b.MakerName, StringComparison.CurrentCultureIgnoreCase));

        private static readonly IComparer<PaintEntry> ByHue =
            Comparer<PaintEntry>.Create((a, b) => a.Colour.Hue.CompareTo(b.Colour.Hue));

        private static readonly IComparer<PaintEntry> ByLuminance =
            Comparer<PaintEntry>.Create((a, b) => a.Luminance.CompareTo(b.Luminance));

        private static readonly IComparer<PaintEntry> BySaturation =
            Comparer<PaintEntry>.Create((a, b) => a.Colour.Saturation.CompareTo(b.Colour.Saturation));

        /// <summary>The sorts, in the order a combo box should show them.</summary>
        public static IReadOnlyList<PaintSortOption> All { get; } = new List<PaintSortOption>
        {
            new("Name", ByName),
            new("Finish", ByFinish),
            new("Maker", ByMaker),
            new("Hue", ByHue, neutralsLast: true),
            new("Luminance", ByLuminance),
            new("Saturation", BySaturation, neutralsLast: true),
        };

        /// <summary>Looks a sort up by name, for restoring the one the user last chose.</summary>
        public static PaintSortOption Find(string? name)
            => All.FirstOrDefault(option => string.Equals(option.Name, name?.Trim(), StringComparison.OrdinalIgnoreCase))
               ?? All[0];

        /// <summary>
        /// Orders the colours by the chosen sort and direction. Two things are deliberate:
        /// the ordering goes through LINQ, whose sort is documented to be stable, so equal colours
        /// keep the catalogue's order; and a sort that groups neutrals keeps them last whichever
        /// direction is chosen, so the top of the list is always actual colour.
        /// </summary>
        public static IEnumerable<PaintEntry> Order(
            IEnumerable<PaintEntry> entries, PaintSortOption option, PaintSortDirection direction)
        {
            if (option.NeutralsLast)
            {
                var grouped = entries.OrderBy(entry => entry.Colour.IsNeutral ? 1 : 0);
                return direction == PaintSortDirection.Descending
                    ? grouped.ThenByDescending(entry => entry, option.Comparer)
                    : grouped.ThenBy(entry => entry, option.Comparer);
            }

            return direction == PaintSortDirection.Descending
                ? entries.OrderByDescending(entry => entry, option.Comparer)
                : entries.OrderBy(entry => entry, option.Comparer);
        }
    }
}
