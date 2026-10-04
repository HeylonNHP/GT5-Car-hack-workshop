using System;
using System.Collections.Generic;
using System.Linq;

namespace GT5_Car_hack_workshop.Models
{
    /// <summary>
    /// How a catalogue entry is described in a part drop-down.
    /// </summary>
    public enum PartLabelMode
    {
        /// <summary>
        /// The part is an upgrade, so the game's own item name leads and the car follows:
        /// <c>Titanium Racing Exhaust (Ferrari 512BB 76)</c>. The "Items" table supplies the name.
        /// </summary>
        Item,

        /// <summary>
        /// The part identifies a whole car (its engine, chassis or drivetrain), so the car's name is
        /// the label. The game's item name would be the same for every car and would only mislead.
        /// </summary>
        Car,

        /// <summary>
        /// The part is a tyre grade. Every grade fits every car, so a car suffix would be a lie: the
        /// label is just the grade's plain name, e.g. <c>Racing Soft</c>.
        /// </summary>
        Tyre
    }

    /// <summary>
    /// How a field's value is laid out in the save, which decides how the editor reads and writes it.
    /// </summary>
    public enum PartFieldKind
    {
        /// <summary>
        /// A 16-bit part key: two big-endian bytes at <see cref="PartCategory.SaveOffset"/>. The
        /// default, and what all the upgrade parts use.
        /// </summary>
        UshortKey,

        /// <summary>
        /// A tyre grade: an 8-byte big-endian key (<c>00 00 00 27 00 33 00 &lt;slot&gt;</c>) whose last
        /// byte is the grade slot (0..14). The first seven bytes say which tyre field it is (front or
        /// rear) and never move. The parts of the key that identify the field are what
        /// <see cref="PartCategory.CanonicalKey"/> holds.
        /// </summary>
        TyreSlotKey
    }

    /// <summary>
    /// One car-part field the editor edits: which game table its value indexes (and therefore which
    /// <c>Parts</c> category of the generated catalogue it lists), where its bytes live in the save,
    /// how they are laid out, and how its entries should be labelled.
    /// </summary>
    /// <param name="TableId">The game table / catalogue category id (the <c>Parts.Category</c> value).</param>
    /// <param name="Label">The field's name, used in the UI and in save error messages.</param>
    /// <param name="SaveOffset">
    /// Offset of the FIRST (high) byte of the field, relative to the car record start (<c>Moff</c>).
    /// These are the offsets the editor has always used, so they are fixed.
    /// </param>
    /// <param name="Mode">Whether the entry is labelled by its car or by its upgrade name.</param>
    /// <param name="FieldKind">How the field is laid out in the save (a 16-bit key, or a tyre slot key).</param>
    /// <param name="CanonicalKey">
    /// For a tyre field, the seven fixed bytes of its key - the slot byte is not part of it - given as
    /// hex (<c>00 00 00 27 00 33 00</c>). It is the shape the field is restored to when it is not
    /// already intact. Null for every other field.
    /// </param>
    /// <param name="PurchaseBitBase">
    /// The game's <c>PartsTableToPurchaseBitGT5</c> base for this table: the lowest PurchaseBit a part
    /// of this family can own. Zero means "no ownership bit at all". ENGINE is the deliberate sentinel
    /// here: its base is 0 and 0 is also the value that would name bit 0, which must never be set, so a
    /// base of 0 is treated as "this family has no bit". NOS, CHASSIS and the tyres likewise have no
    /// usable bit (tyres use a per-axle purchase ordinal that is not understood).
    /// </param>
    /// <param name="SetsLowerTiers">
    /// Whether owning tier L also records ownership of every lower tier, i.e. whether bits
    /// <c>base+1 .. base+ordinal</c> are all set. True only for the two progressive families,
    /// LIGHT_WEIGHT and NATUNE, whose records bundle their earlier stages. Every other family records
    /// only the tier actually fitted.
    /// </param>
    public sealed record PartCategory(int TableId, string Label, int SaveOffset, PartLabelMode Mode,
        PartFieldKind FieldKind = PartFieldKind.UshortKey, string? CanonicalKey = null,
        int PurchaseBitBase = 0, bool SetsLowerTiers = false)
    {
        /// <summary>
        /// Whether this family can own a PurchaseBit at all. Engine, chassis and NOS have no bit in
        /// GT5, and the tyres are out of scope, so all four report false.
        /// </summary>
        public bool HasPurchaseBit => PurchaseBitBase > 0;

        /// <summary>
        /// Whether the tuning shop may fit this field. Tyres are refused: their ownership uses a
        /// per-axle purchase ordinal this build does not understand, and a guessed bit would mark the
        /// wrong tyre owned. Everything else (upgrades, and engine/chassis/drivetrain, which simply
        /// carry no bit) can be fitted as a plain part key.
        /// </summary>
        public bool Installable => FieldKind != PartFieldKind.TyreSlotKey;
    }

    /// <summary>
    /// THE list of part fields the editor edits. Combo-box wiring, catalogue loading, the save-file
    /// read and the save-file write are all driven from here, so a part is declared in exactly one
    /// place instead of being repeated wherever it is used.
    /// <para>
    /// Body and Horn are deliberately NOT here: they are not game part keys, they keep the hand-built
    /// <c>CarParts</c> catalogue and their own combo behaviour.
    /// </para>
    /// <para>
    /// The <see cref="PartCategory.PurchaseBitBase"/> values are the game's own
    /// <c>PartsTableToPurchaseBitGT5</c> table (verified against the live save and 140
    /// <c>carparameter</c> blobs). They must be given exactly: two families genuinely share bit ranges
    /// (AIR_CLEANER catalogue tier 2 lands on BOOST_CONTROLLER's bit 179), so an off-by-one would
    /// corrupt a neighbouring family's ownership rather than fail loudly.
    /// </para>
    /// </summary>
    public static class PartCatalogue
    {
        /// <summary>Every part field, in the order the editor shows and writes them.</summary>
        public static readonly IReadOnlyList<PartCategory> Categories = new List<PartCategory>
        {
            new(13, "Engine",           -213, PartLabelMode.Car),  // ENGINE      (which engine the car has / donor); base 0 = sentinel, never set bit 0
            new(7,  "Chassis",          -217, PartLabelMode.Car),  // CHASSIS     - no purchase bit exists
            new(11, "Drivetrain",       -209, PartLabelMode.Car,   PartFieldKind.UshortKey, null, 27),  // DRIVETRAIN
            new(12, "Transmission",     -205, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 30),  // GEAR
            new(4,  "Suspension",       -201, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 8),   // SUSPENSION  (offsets shifted one - see PartInstaller.TierOrdinal)
            new(23, "LSD",              -197, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 72),  // LSD
            new(2,  "Brake",            -225, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 1),   // BRAKE
            new(3,  "Brake controller", -221, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 6),   // BRAKECONTROLLER
            new(9,  "Weight reduction", -189, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 18, true),  // LIGHTWEIGHT - progressive
            new(15, "Turbo kit",        -169, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 40),  // TURBINEKIT
            new(19, "Exhaust",          -153, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 58),  // MUFFLER
            new(20, "Clutch",           -161, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 62),  // CLUTCH
            new(21, "Flywheel",         -165, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 66),  // FLYWHEEL
            new(22, "Propeller shaft",  -157, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 70),  // PROPELLERSHAFT
            new(14, "NA tune",          -173, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 34, true),  // NATUNE - progressive
            new(16, "Displacement",     -181, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 46),  // DISPLACEMENT
            new(17, "Computer (ECU)",   -177, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 50),  // COMPUTER
            new(18, "Intercooler",      -149, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 53),  // INTERCOOLER
            new(27, "Supercharger",     -133, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 169), // SUPERCHARGER
            new(28, "Intake manifold",  -129, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 171), // INTAKE_MANIFOLD
            new(29, "Exhaust manifold", -125, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 173), // EXHAUST_MANIFOLD
            new(30, "Catalyst",         -121, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 175), // CATALYST
            new(31, "Air cleaner",      -117, PartLabelMode.Item,  PartFieldKind.UshortKey, null, 177), // AIR_CLEANER
            new(26, "NOS",              -113, PartLabelMode.Item),  // NOS - no shop parts exist in the catalogue, and no purchase bit exists

            // Tyres are not game part keys: the save holds an 8-byte key whose last byte is the grade
            // slot. The 15 grades fit every car, so their entries are built in code (PartCatalogueStore)
            // rather than read from the game's part tables, which have no rows for them. Their
            // ownership bits use a per-axle purchase ordinal that is not understood, so they keep
            // base 0 and the tuning shop refuses them.
            new(51, "Front tyres",     -243, PartLabelMode.Tyre, PartFieldKind.TyreSlotKey, "00 00 00 27 00 33 00"), // TYRE_FRONT
            new(52, "Rear tyres",      -235, PartLabelMode.Tyre, PartFieldKind.TyreSlotKey, "00 00 00 27 00 34 00"), // TYRE_REAR
        };

        /// <summary>The descriptor for a game table / catalogue category, or null when unknown.</summary>
        public static PartCategory? Find(int tableId) => Categories.FirstOrDefault(c => c.TableId == tableId);

        /// <summary>
        /// The descriptor for a game table, throwing when it is not declared. Used by the wiring code
        /// so a typo in a table id fails loudly at start-up instead of silently dropping a part.
        /// </summary>
        public static PartCategory Get(int tableId) =>
            Find(tableId) ?? throw new InvalidOperationException($"Part category {tableId} is not declared in PartCatalogue.Categories.");

        /// <summary>Every field the tuning shop may fit: the installable ones, in catalogue order.</summary>
        public static IEnumerable<PartCategory> InstallableCategories => Categories.Where(c => c.Installable);
    }
}
