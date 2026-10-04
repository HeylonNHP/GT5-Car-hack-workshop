using System;
using System.Collections.Generic;
using GT5_Car_hack_workshop.Models;

namespace GT5_Car_hack_workshop.Services
{
    /// <summary>What one <see cref="PartInstaller.Install"/> call actually changed.</summary>
    /// <param name="OldKey">The field's part key before the write.</param>
    /// <param name="NewKey">The field's part key after the write.</param>
    /// <param name="OldFlag">The field's flag half before the write (0 when it was already clean).</param>
    /// <param name="BitsSet">The purchase bits that were clear and are now set.</param>
    /// <param name="BitsAlreadySet">The purchase bits that were already set (an install never clears one).</param>
    public sealed record InstallResult(
        ushort OldKey, ushort NewKey, int OldFlag,
        IReadOnlyList<int> BitsSet, IReadOnlyList<int> BitsAlreadySet)
    {
        /// <summary>Whether the part key in the field actually moved.</summary>
        public bool KeyChanged => OldKey != NewKey;

        /// <summary>Whether a leftover <c>0xFFFF</c> (or any non-zero) flag half was zeroed.</summary>
        public bool FlagCleared => OldFlag != 0;
    }

    /// <summary>
    /// Fits a part onto a car in the save, the way the game's Tuning Shop does: write the part key the
    /// editor has always written, clean the field's flag half, then set the game's PurchaseBit for the
    /// part's tier so the part actually registers as owned.
    /// <para>
    /// This is deliberately UI-free and holds all of the byte-level reasoning. It bounds-checks
    /// everything <em>before</em> writing anything, so a bad offset throws instead of leaving a
    /// half-written record behind, and it never touches a byte outside the four field bytes and the
    /// ownership-bit bytes.
    /// </para>
    /// </summary>
    public static class PartInstaller
    {
        /// <summary>The PurchaseBits mask is this many bytes long.</summary>
        public const int PurchaseBitLength = 64;

        /// <summary>Where the mask starts, relative to <c>Moff</c> (it runs to Moff-273).</summary>
        public const int PurchaseBitFromMoff = -336;

        /// <summary>The SUSPENSION table id, the one family whose catalogue levels are offset.</summary>
        private const int SuspensionTableId = 4;

        /// <summary>The game's purchase-bit base for a family (0 when it owns no bit at all).</summary>
        public static int PurchaseBitBaseOf(PartCategory category) => category.PurchaseBitBase;

        /// <summary>Whether owning the top tier also records every lower tier (LIGHT_WEIGHT, NATUNE).</summary>
        public static bool SetsLowerTiers(PartCategory category) => category.SetsLowerTiers;

        /// <summary>
        /// The game's purchase ordinal for a catalogue level.
        /// <para>
        /// Every family but one uses the catalogue level verbatim. SUSPENSION IS OFF BY ONE: the live
        /// record and 140 <c>carparameter</c> blobs (the game's own three copies per car) show a
        /// catalogue Level 3 part (<c>su_..._d</c>) landing on ordinal <b>4</b> - bit 8+4 = 12 is the
        /// bit actually set - while ordinal 3 is never exercised by any car. So suspension maps
        /// Level 0 -> 0 and Level >= 1 -> Level + 1.
        /// </para>
        /// <para>
        /// This is inferred from limited data: only Level 3 was observed fitted, so the mapping for
        /// Levels 1 and 2 is an extrapolation. It is isolated here so this is the single place to
        /// revisit if a wider sample of suspension-fitted cars ever contradicts it.
        /// </para>
        /// </summary>
        public static int TierOrdinal(PartCategory category, int catalogueLevel)
        {
            if (category.TableId == SuspensionTableId)
                return catalogueLevel <= 0 ? 0 : catalogueLevel + 1;

            return catalogueLevel;
        }

        /// <summary>
        /// Every purchase bit a part of this family and tier owns. Empty for a family with no bit
        /// (engine's sentinel, chassis, NOS, tyres) and for a stock part of a progressive family; the
        /// progressive families own <c>base+1 .. base+ordinal</c>, every other family owns only
        /// <c>base+ordinal</c>.
        /// </summary>
        public static IReadOnlyList<int> PurchaseBitsOf(PartCategory category, int catalogueLevel)
        {
            if (!category.HasPurchaseBit) return Array.Empty<int>();

            var ordinal = TierOrdinal(category, catalogueLevel);

            if (category.SetsLowerTiers)
            {
                if (ordinal <= 0) return Array.Empty<int>();

                var bundled = new int[ordinal];
                for (var i = 0; i < ordinal; i++) bundled[i] = category.PurchaseBitBase + 1 + i;
                return bundled;
            }

            return new[] { category.PurchaseBitBase + Math.Max(0, ordinal) };
        }

        /// <summary>
        /// Fits <paramref name="part"/> onto the car whose record starts at <paramref name="moff"/>.
        /// The save array is mutated in place (the caller holds the same array the UI edits).
        /// </summary>
        /// <exception cref="InvalidOperationException">The category is a tyre, or is not declared.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The field or the mask does not fit the save.</exception>
        public static InstallResult Install(byte[] save, int moff, PartEntry part, ushort currentKey)
        {
            ArgumentNullException.ThrowIfNull(save);
            ArgumentNullException.ThrowIfNull(part);

            var category = PartCatalogue.Get(part.Category);

            // Tyres are out of scope: their stock reads as ordinal 2 and one car owns 2, 9, 11 and 12,
            // so their ownership is a per-axle purchase ordinal this build does not understand. A
            // guessed bit would quietly mark the wrong tyre grade owned, so refuse rather than guess.
            if (category.FieldKind == PartFieldKind.TyreSlotKey)
                throw new InvalidOperationException(
                    $"\"{category.Label}\" cannot be installed here: tyre ownership uses a per-axle " +
                    "purchase ordinal this build does not understand, so setting a bit could mark the wrong tyre owned. " +
                    "Use the tyre boxes on the main window instead.");

            // Bounds-check everything before writing anything, so a bad offset cannot leave a
            // half-written record behind.
            var fieldOffset = moff + category.SaveOffset;
            if (fieldOffset < 0 || fieldOffset + 4 > save.Length)
                throw new ArgumentOutOfRangeException(nameof(moff),
                    $"The {category.Label} field does not fit in the save at Moff{category.SaveOffset:+0;-0}.");

            var bits = PurchaseBitsOf(category, part.Level);
            var bitBase = moff + PurchaseBitFromMoff;
            if (bits.Count > 0 && (bitBase < 0 || bitBase + PurchaseBitLength > save.Length))
                throw new ArgumentOutOfRangeException(nameof(moff),
                    "The purchase-bit mask (Moff-336 ... Moff-273) does not fit in this save.");

            var oldKey = ByteUtils.BytesToUshort(save[fieldOffset], save[fieldOffset + 1]);
            var oldFlag = (save[fieldOffset + 2] << 8) | save[fieldOffset + 3];

            // 1. The key: exactly the two big-endian bytes PartFieldCodec has always written.
            PartFieldCodec.Write(save, moff, category, part.PartKey);

            // 2. The flag half, which past editor writes left at 0xFFFF. It sits at SaveOffset+2..+3,
            //    still inside the same four-byte field, so zeroing it cannot touch a neighbour.
            PartFieldCodec.ClearFlag(save, moff, category);

            // 3. The ownership bits: OR only, never clear one, and never set the sentinel bit 0.
            var justSet = new List<int>(bits.Count);
            var alreadySet = new List<int>(bits.Count);
            foreach (var bit in bits)
            {
                if (bit <= 0) continue; // bit 0 is ENGINE's sentinel and must never be set

                var byteIndex = bitBase + bit / 8;
                var mask = (byte)(1 << (bit % 8));
                if ((save[byteIndex] & mask) != 0)
                    alreadySet.Add(bit);
                else
                {
                    save[byteIndex] |= mask;
                    justSet.Add(bit);
                }
            }

            return new InstallResult(oldKey, part.PartKey, oldFlag, justSet, alreadySet);
        }
    }
}
