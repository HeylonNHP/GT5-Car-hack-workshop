namespace GT5_Car_hack_workshop.Models
{
    /// <summary>
    /// One selectable part in a drop-down: a single <c>Parts</c> row of the generated catalogue,
    /// which is to say one car's own variant of one part.
    /// <para>
    /// There is deliberately no de-duplication: two cars with the same upgrade fitted have two
    /// entries, because fitting another car's part is the whole point of the editor. The label
    /// therefore always names the part's car, and the 16-bit code (what the save actually stores)
    /// is the entry's <see cref="PartKey"/>.
    /// </para>
    /// </summary>
    public sealed class PartEntry
    {
        /// <summary>The catalogue category (game table id) this part belongs to.</summary>
        public int Category { get; init; }

        /// <summary>The 16-bit part key the save stores for this field.</summary>
        public ushort PartKey { get; init; }

        /// <summary>The part's tier (0 = what the car came with, 1..n = fitted upgrade levels).</summary>
        public int Level { get; init; }

        /// <summary>
        /// The part's real game type - the 'category' byte the SpecDB part tables carry per row
        /// (the published <c>PARTS_*</c> enum values, e.g. <c>PARTS_CATALYST {SPORTS=1, RACING=2}</c>),
        /// copied into the catalogue's Parts rows by <c>tools/patch_part_types.py</c>. Null when
        /// the catalogue cannot resolve a row: the families whose SpecDB tables this build does
        /// not ship (brake controller, displacement, intercooler), NOS, and every unresolvable
        /// key. Never 0-vs-null ambiguous - 0 is a real, resolvable type (engine and chassis are
        /// typed 0), while null strictly means "no type known, fall back to Level".
        /// </summary>
        public int? PartType { get; init; }

        /// <summary>The car this variant belongs to (0 for a catalogue row no car references).</summary>
        public int CarId { get; init; }

        /// <summary>The name the game's item list gives this level, when it names it at all.</summary>
        public string PartName { get; init; } = "";

        /// <summary>The name of the car this variant belongs to.</summary>
        public string CarName { get; init; } = "";

        /// <summary>
        /// The display text - what the drop-down shows and the user types. Unique within its
        /// category, because the box resolves the typed text back to a code through it.
        /// </summary>
        public string Label { get; set; } = "";

        /// <summary>The part key as the editor's hex fields show it, e.g. <c>0C E2</c>.</summary>
        public string Hex => ByteUtils.UshortToHexString(PartKey);

        /// <summary>The text shown when hovering an entry.</summary>
        public string Tooltip => $"{Label} - code {Hex}";

        public override string ToString() => Label;
    }
}
