namespace GT5_Car_hack_workshop.Models
{
    /// <summary>
    /// A row of the "owned paint chips" list: one colour from the paint database and how many
    /// chips of that colour the save currently holds.
    /// </summary>
    public class OwnedPaintChipRow
    {
        /// <summary>The colour's id in the paint database, used to select it again in a search box.</summary>
        public uint ColourId { get; init; }

        public string Name { get; init; } = "";

        public string Maker { get; init; } = "";

        public int Quantity { get; init; }

        /// <summary>The colour's name, with its maker when known, for display in the list.</summary>
        public string Display => string.IsNullOrEmpty(Maker) ? Name : $"{Name} ({Maker})";

        /// <summary>The quantity as text, so the list column can bind directly to it.</summary>
        public string QuantityText => Quantity.ToString();

        public override string ToString() => Display;
    }
}
