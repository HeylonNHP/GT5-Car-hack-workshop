namespace GT5_Car_hack_workshop.Models
{
    /// <summary>
    /// One row of the tuning shop's installable-part list: a catalogue <see cref="PartEntry"/> dressed
    /// with everything the list and the install need - the current car's value in the same field, so
    /// the row can say whether the part is already fitted, and the ownership bit the install will set.
    /// </summary>
    public sealed class TuningPartRow
    {
        /// <summary>The catalogue entry this row installs.</summary>
        public required PartEntry Part { get; init; }

        /// <summary>The field's friendly name, e.g. <c>Turbo kit</c>.</summary>
        public string CategoryLabel { get; init; } = "";

        /// <summary>The tier's name (the game's item name for the level, or "Stock").</summary>
        public string TierName { get; init; } = "";

        /// <summary>The part key as the editor's hex fields show it, e.g. <c>0C E2</c>.</summary>
        public string Hex => Part.Hex;

        /// <summary>The current car's value in this field, so the row can tell fitted from not.</summary>
        public ushort CurrentKey { get; init; }

        /// <summary>Whether the field already holds exactly this part's key.</summary>
        public bool IsFitted { get; init; }

        /// <summary>
        /// The ownership bit the install will set, or null for a family with no bit (engine, chassis,
        /// NOS). For a progressive family this is the highest of its bundled bits.
        /// </summary>
        public int? PurchaseBit { get; init; }

        /// <summary>Whether that ownership bit is already set in the save.</summary>
        public bool PurchaseBitSet { get; init; }

        /// <summary>A short phrase for the list's fitted column.</summary>
        public string FittedText
        {
            get
            {
                var fitted = IsFitted ? "Fitted" : "Not fitted";
                return PurchaseBit is { } bit
                    ? $"{fitted}, {(PurchaseBitSet ? "owned" : "not owned")} (bit {bit})"
                    : $"{fitted}, no ownership bit";
            }
        }

        /// <summary>The whole row as one line, used by the list template and by tooltips.</summary>
        public string Display => $"{CategoryLabel}: {TierName} ({Hex}) - {FittedText}";

        public override string ToString() => Display;
    }
}
