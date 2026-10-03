using System;
using System.Collections.Generic;
using System.Globalization;
using GT5_Car_hack_workshop.Models;

namespace GT5_Car_hack_workshop.Services
{
    /// <summary>
    /// Reads and writes one part field of the car record, according to how the field is laid out
    /// (see <see cref="PartFieldKind"/>).
    /// <para>
    /// An upgrade part is two big-endian bytes and is written exactly as the editor always wrote it,
    /// bit for bit, because other hacks (the 4WD/drivetrain hack among them) depend on those bytes.
    /// </para>
    /// <para>
    /// A tyre is an 8-byte key whose last byte is the grade slot. The first seven bytes identify the
    /// field (front or rear) and are never moved: when the key is already intact only the slot byte is
    /// written; when it is not intact - a hand-edited save - the whole canonical key is laid down so
    /// the field becomes usable again, and an unknown slot leaves the field's own bytes alone and
    /// varies only the slot, so a hand-typed value round-trips as raw hex like every other category.
    /// Nothing outside the field is ever written.
    /// </para>
    /// </summary>
    public static class PartFieldCodec
    {
        /// <summary>The length of a tyre field: 7 bytes of key plus the slot byte.</summary>
        public const int TyreKeyLength = 8;

        /// <summary>The highest tyre slot the game's grade list names (0 = Comfort Hard .. 14 = Snow).</summary>
        public const ushort MaxTyreSlot = 14;

        /// <summary>The fixed bytes of a canonical key, parsed once per distinct key.</summary>
        private static readonly Dictionary<string, byte[]> CanonicalKeys = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The value a field holds in the save: the 16-bit part key for an upgrade part, or the slot
        /// byte of the 8-byte key for a tyre.
        /// </summary>
        public static ushort Read(byte[] save, int moff, PartCategory category)
        {
            var offset = OffsetOf(save, moff, category);

            return category.FieldKind == PartFieldKind.TyreSlotKey
                ? save[offset + TyreKeyLength - 1] // the slot is the last byte of the key
                : ByteUtils.BytesToUshort(save[offset], save[offset + 1]);
        }

        /// <summary>
        /// Writes <paramref name="value"/> into the field. The field's length and offsets come from
        /// the category, so nothing outside it can be touched.
        /// </summary>
        public static void Write(byte[] save, int moff, PartCategory category, ushort value)
        {
            var offset = OffsetOf(save, moff, category);

            if (category.FieldKind != PartFieldKind.TyreSlotKey)
            {
                // Upgrade parts: the same two big-endian bytes the editor has always written.
                save[offset] = (byte)(value >> 8);
                save[offset + 1] = (byte)(value & 0xFF);
                return;
            }

            ApplyTyreSlot(save.AsSpan(offset, TyreKeyLength), category, value);
        }

        /// <summary>
        /// The hex a field shows beside its box: the two bytes of the part key, or the whole 8-byte
        /// tyre key (<c>00 00 00 27 00 33 00 09</c>) as it will read once <paramref name="value"/> is
        /// written. Works before a save is loaded: the field's own canonical shape is shown then.
        /// </summary>
        public static string DisplayHex(byte[]? save, int moff, PartCategory category, ushort value)
        {
            if (category.FieldKind != PartFieldKind.TyreSlotKey)
                return ByteUtils.UshortToHexString(value);

            var key = new byte[TyreKeyLength];
            var offset = moff + category.SaveOffset;
            if (save is { } bytes && offset >= 0 && offset + TyreKeyLength <= bytes.Length)
                Array.Copy(bytes, offset, key, 0, TyreKeyLength);
            else
                CanonicalKeyBytes(category)?.CopyTo(key, 0); // no save yet: show the field's own key shape

            ApplyTyreSlot(key, category, value);
            return HexOf(key);
        }

        /// <summary>
        /// Whether an 8-byte key still holds the seven bytes that identify its field, i.e. whether it
        /// needs no repair. Only the slot byte may differ.
        /// </summary>
        public static bool IsCanonicalTyreKey(ReadOnlySpan<byte> key, PartCategory category)
        {
            var canonical = CanonicalKeyBytes(category);
            if (canonical == null || key.Length != TyreKeyLength) return false;

            for (var i = 0; i < TyreKeyLength - 1; i++)
                if (key[i] != canonical[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Turns the text a part box stands for into the value to write: the catalogue entry's key, or
        /// the hex the user typed. A tyre box holds a slot, so a short code (<c>14</c>, which is slot
        /// 20) is read as that slot byte; anything longer is the 16-bit key of a whole field.
        /// </summary>
        public static ushort ParseCode(PartCategory category, string? code)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                var packed = code.Replace(" ", string.Empty);
                if (category.FieldKind == PartFieldKind.TyreSlotKey && packed.Length <= 2)
                    return byte.Parse(packed, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return ByteUtils.HexStringToUshort(code ?? string.Empty);
        }

        /// <summary>
        /// The text a box shows for a bare value: <c>0C E2</c> for an upgrade part, or just the slot
        /// (<c>0E</c>) for a tyre, so a slot the grade list does not name round-trips as raw hex.
        /// </summary>
        public static string FormatCode(PartCategory category, ushort value)
            => category.FieldKind == PartFieldKind.TyreSlotKey
                ? (value & 0xFF).ToString("X2", CultureInfo.InvariantCulture)
                : ByteUtils.UshortToHexString(value);

        /// <summary>
        /// Puts a slot into a tyre key, writing as little as the key's state allows (see the class
        /// remarks). The span is the field itself; nothing beyond it is ever written.
        /// </summary>
        private static void ApplyTyreSlot(Span<byte> key, PartCategory category, ushort value)
        {
            var canonical = CanonicalKeyBytes(category);

            if (canonical != null && IsCanonicalTyreKey(key, category))
            {
                // Intact key: only the slot moves, so the bytes that identify the field are untouched.
                key[TyreKeyLength - 1] = (byte)value;
                return;
            }

            if (canonical != null && value <= MaxTyreSlot)
            {
                // A known grade on a key that is not intact: lay down the canonical key so the field
                // reads back as a real tyre again.
                canonical.CopyTo(key);
                key[TyreKeyLength - 1] = (byte)value;
                return;
            }

            // Either the field has no canonical shape, or the slot is one the grade list does not
            // name (a hand-typed value): vary the slot only, keeping the field's own bytes.
            key[TyreKeyLength - 1] = (byte)value;
        }

        /// <summary>The seven fixed bytes of a tyre field's key, or null when it has none.</summary>
        private static byte[]? CanonicalKeyBytes(PartCategory category)
        {
            if (string.IsNullOrWhiteSpace(category.CanonicalKey)) return null;
            if (CanonicalKeys.TryGetValue(category.CanonicalKey, out var cached)) return cached;

            var bytes = ByteUtils.HexStringToByteArray(category.CanonicalKey);
            if (bytes.Length != TyreKeyLength - 1) return null;

            CanonicalKeys[category.CanonicalKey] = bytes;
            return bytes;
        }

        /// <summary>
        /// The file offset of the field, checked against the save's length so a field that does not fit
        /// says so instead of throwing an index error from the middle of a write.
        /// </summary>
        private static int OffsetOf(byte[] save, int moff, PartCategory category)
        {
            var offset = moff + category.SaveOffset;
            var length = category.FieldKind == PartFieldKind.TyreSlotKey ? TyreKeyLength : 2;

            if (save == null || offset < 0 || offset + length > save.Length)
                throw new ArgumentOutOfRangeException(nameof(category),
                    $"The {category.Label} field does not fit in the save at Moff{category.SaveOffset:+0;-0}.");

            return offset;
        }

        /// <summary>Spaced uppercase hex, e.g. <c>00 00 00 27 00 33 00 09</c>.</summary>
        private static string HexOf(ReadOnlySpan<byte> bytes)
        {
            var text = new char[bytes.Length * 3 - 1];
            for (var i = 0; i < bytes.Length; i++)
            {
                var pair = bytes[i].ToString("X2", CultureInfo.InvariantCulture);
                text[i * 3] = pair[0];
                text[i * 3 + 1] = pair[1];
                if (i < bytes.Length - 1) text[i * 3 + 2] = ' ';
            }

            return new string(text);
        }
    }
}
