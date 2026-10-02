using System;

namespace GT5_Car_hack_workshop.Services
{
    /// <summary>Where the current car's data begins in a decrypted save, or why it could not be found.</summary>
    public sealed class SaveAnchorResult
    {
        private SaveAnchorResult(bool found, int moff, string? failureReason)
        {
            Found = found;
            Moff = moff;
            FailureReason = failureReason;
        }

        /// <summary>Whether <see cref="Moff"/> is usable.</summary>
        public bool Found { get; }

        /// <summary>The offset every other field is measured from.</summary>
        public int Moff { get; }

        /// <summary>Why the save could not be placed, when <see cref="Found"/> is false.</summary>
        public string? FailureReason { get; }

        internal static SaveAnchorResult Success(int moff) => new(true, moff, null);

        internal static SaveAnchorResult Failure(string reason) => new(false, 0, reason);
    }

    /// <summary>
    /// Finds the current car's data in a decrypted GT5 save.
    ///
    /// The old way was to search the save for the player's PSN name and count back from it, which
    /// needed the name to be typed in - and the name is not even unique (it appears three times per
    /// save). This locates the car from the save's own structure instead, so nothing has to be typed.
    ///
    /// The car's <c>MCarParameter</c> record self-identifies (version 110, header size 0x3C), but it
    /// appears once per car in the file - including the database's own copies - so the signature
    /// alone is ambiguous. The current car is the last such record before the embedded database, and
    /// because it sits in front of the database the two are a fixed distance apart.
    ///
    /// Everything checked here is structure, never a value the player (or this editor) can change:
    /// a check on editable data - such as the odometer - can be broken by using the editor normally.
    /// </summary>
    public static class SaveAnchor
    {
        /// <summary>
        /// The header of the save's embedded car database. It occurs exactly once, so it makes an
        /// unambiguous landmark.
        /// </summary>
        private static readonly byte[] DatabaseHeader = "SQLite format 3\0"u8.ToArray();

        /// <summary>
        /// The start of an <c>MCarParameter</c> record: ParameterVersion 110 then MainHeaderSize 0x3C.
        /// One per car, so this is a shape to search for rather than a unique marker.
        /// </summary>
        private static readonly byte[] ParameterSignature = { 0x00, 0x00, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x3C };

        /// <summary>A serialisation tag that sits just before the current car's record.</summary>
        private static readonly byte[] CarTag = { 0x07, 0x81, 0x8D };

        /// <summary>How far into the car's record the anchor sits (it lands near the end of it).</summary>
        private const int MoffFromBlock = 396;

        /// <summary>Where <see cref="CarTag"/> sits relative to the anchor.</summary>
        private const int CarTagFromMoff = -419;

        /// <summary>Where the database header sits relative to the anchor.</summary>
        private const int DatabaseHeaderFromMoff = 11851;

        /// <summary>
        /// Locates the current car in <paramref name="save"/>, or explains why it could not be.
        /// The two cross-checks must both hold; if either fails the save is reported as unplaceable
        /// rather than edited, because writing at a wrong offset would corrupt the save.
        /// </summary>
        public static SaveAnchorResult Locate(byte[]? save)
        {
            if (save is null || save.Length == 0)
                return SaveAnchorResult.Failure("the save is empty.");

            var database = IndexOf(save, DatabaseHeader, 0);
            if (database < 0)
                return SaveAnchorResult.Failure("its car database was not found, so it may not be a GT5 save.");
            if (IndexOf(save, DatabaseHeader, database + 1) >= 0)
                return SaveAnchorResult.Failure("it holds more than one car database, so the car cannot be placed reliably.");

            var block = LastIndexOfBefore(save, ParameterSignature, database);
            if (block < 0)
                return SaveAnchorResult.Failure("there is no car record in front of its car database.");

            var moff = block + MoffFromBlock;

            if (!MatchesAt(save, moff + CarTagFromMoff, CarTag))
                return SaveAnchorResult.Failure("the car record is not where the save's structure says it should be (tag check).");
            if (database - moff != DatabaseHeaderFromMoff)
                return SaveAnchorResult.Failure("the car record is not the expected distance from the car database.");

            return SaveAnchorResult.Success(moff);
        }

        /// <summary>The first index at or after <paramref name="from"/> holding <paramref name="pattern"/>, or -1.</summary>
        private static int IndexOf(byte[] haystack, byte[] pattern, int from)
        {
            for (var i = Math.Max(0, from); i <= haystack.Length - pattern.Length; i++)
                if (MatchesAt(haystack, i, pattern))
                    return i;

            return -1;
        }

        /// <summary>The last index below <paramref name="limit"/> holding <paramref name="pattern"/>, or -1.</summary>
        private static int LastIndexOfBefore(byte[] haystack, byte[] pattern, int limit)
        {
            var last = Math.Min(limit, haystack.Length - pattern.Length + 1) - 1;
            for (var i = last; i >= 0; i--)
                if (MatchesAt(haystack, i, pattern))
                    return i;

            return -1;
        }

        /// <summary>Whether <paramref name="pattern"/> sits at exactly <paramref name="index"/>.</summary>
        private static bool MatchesAt(byte[] haystack, int index, byte[] pattern)
        {
            if (index < 0 || index + pattern.Length > haystack.Length) return false;

            for (var i = 0; i < pattern.Length; i++)
                if (haystack[index + i] != pattern[i])
                    return false;

            return true;
        }
    }
}
