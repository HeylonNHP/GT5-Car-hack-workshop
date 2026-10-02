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
        Car
    }

    /// <summary>
    /// One car-part field the editor edits: which game table its value indexes (and therefore which
    /// <c>Parts</c> category of the generated catalogue it lists), where its two bytes live in the
    /// save, and how its entries should be labelled.
    /// </summary>
    /// <param name="TableId">The game table / catalogue category id (the <c>Parts.Category</c> value).</param>
    /// <param name="Label">The field's name, used in the UI and in save error messages.</param>
    /// <param name="SaveOffset">
    /// Offset of the FIRST (high) byte of the big-endian 16-bit value, relative to the car record
    /// start (<c>Moff</c>). These are the offsets the editor has always used, so they are fixed.
    /// </param>
    /// <param name="Mode">Whether the entry is labelled by its car or by its upgrade name.</param>
    public sealed record PartCategory(int TableId, string Label, int SaveOffset, PartLabelMode Mode);

    /// <summary>
    /// THE list of part fields the editor edits. Combo-box wiring, catalogue loading, the save-file
    /// read and the save-file write are all driven from here, so a part is declared in exactly one
    /// place instead of being repeated wherever it is used.
    /// <para>
    /// Body and Horn are deliberately NOT here: they are not game part keys, they keep the hand-built
    /// <c>CarParts</c> catalogue and their own combo behaviour.
    /// </para>
    /// </summary>
    public static class PartCatalogue
    {
        /// <summary>Every part field, in the order the editor shows and writes them.</summary>
        public static readonly IReadOnlyList<PartCategory> Categories = new List<PartCategory>
        {
            new(13, "Engine",           -213, PartLabelMode.Car),  // ENGINE      (which engine the car has / donor)
            new(7,  "Chassis",          -217, PartLabelMode.Car),  // CHASSIS
            new(11, "Drivetrain",       -209, PartLabelMode.Car),  // DRIVETRAIN
            new(12, "Transmission",     -205, PartLabelMode.Item), // GEAR
            new(4,  "Suspension",       -201, PartLabelMode.Item), // SUSPENSION
            new(23, "LSD",              -197, PartLabelMode.Item), // LSD
            new(2,  "Brake",            -225, PartLabelMode.Item), // BRAKE
            new(3,  "Brake controller", -221, PartLabelMode.Item), // BRAKECONTROLLER
            new(9,  "Weight reduction", -189, PartLabelMode.Item), // LIGHTWEIGHT
            new(15, "Turbo kit",        -169, PartLabelMode.Item), // TURBINEKIT
            new(19, "Exhaust",          -153, PartLabelMode.Item), // MUFFLER
            new(20, "Clutch",           -161, PartLabelMode.Item), // CLUTCH
            new(21, "Flywheel",         -165, PartLabelMode.Item), // FLYWHEEL
            new(22, "Propeller shaft",  -157, PartLabelMode.Item), // PROPELLERSHAFT
            new(14, "NA tune",          -173, PartLabelMode.Item), // NATUNE
            new(16, "Displacement",     -181, PartLabelMode.Item), // DISPLACEMENT
            new(17, "Computer (ECU)",   -177, PartLabelMode.Item), // COMPUTER
            new(18, "Intercooler",      -149, PartLabelMode.Item), // INTERCOOLER
            new(27, "Supercharger",     -133, PartLabelMode.Item), // SUPERCHARGER
            new(28, "Intake manifold",  -129, PartLabelMode.Item), // INTAKE_MANIFOLD
            new(29, "Exhaust manifold", -125, PartLabelMode.Item), // EXHAUST_MANIFOLD
            new(30, "Catalyst",         -121, PartLabelMode.Item), // CATALYST
            new(31, "Air cleaner",      -117, PartLabelMode.Item), // AIR_CLEANER
            new(26, "NOS",              -113, PartLabelMode.Item), // NOS - no shop parts exist in the catalogue
        };

        /// <summary>The descriptor for a game table / catalogue category, or null when unknown.</summary>
        public static PartCategory? Find(int tableId) => Categories.FirstOrDefault(c => c.TableId == tableId);

        /// <summary>
        /// The descriptor for a game table, throwing when it is not declared. Used by the wiring code
        /// so a typo in a table id fails loudly at start-up instead of silently dropping a part.
        /// </summary>
        public static PartCategory Get(int tableId) =>
            Find(tableId) ?? throw new InvalidOperationException($"Part category {tableId} is not declared in PartCatalogue.Categories.");
    }
}
