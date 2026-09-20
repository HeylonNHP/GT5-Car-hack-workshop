using System;
using System.Collections.Generic;
using System.Text;

namespace GT5_Car_hack_workshop.Services
{
    /// <summary>
    /// Reads and writes the player's owned paint chips - the "Color Paint" items shown in the
    /// garage - which GT5 keeps as rows of the <c>t_itembox_user</c> table in the SQLite
    /// database embedded inside the save.
    /// <para>
    /// Everything happens in memory: the database is loaded from the save byte array, changed,
    /// and handed back as a new byte array. Nothing is written to disk here - the caller writes
    /// the returned array out (through the normal save/encrypt path) only when the user asks.
    /// </para>
    /// </summary>
    public sealed class PaintChipStore : IDisposable
    {
        // Item taxonomy used by the save's item tables (see t_item_type / t_item_category).
        private const int GtAutoTypeId = 6;
        private const int ColorPaintCategoryId = 601;
        private const int PaintItemId = 101;        // "Paint Item" - solid, metallic, pearl, iridescent
        private const int SpecialPaintItemId = 103; // "Special Paint Item" - matte, chrome
        private const int VisibleStatus = 1;

        /// <summary>The number of visible items the engine will allow before it stops adding more.</summary>
        public const int MaxVisibleItems = 7000;

        private const int SqliteHeaderLength = 100;
        private const int PageSize = EmbeddedSqliteDb.PageSize;
        private static readonly byte[] SqliteMagic = Encoding.ASCII.GetBytes("SQLite format 3\0");

        private readonly EmbeddedSqliteDb _database;
        private readonly byte[] _trailingBytes;

        /// <summary>Offset within the save at which the embedded database starts.</summary>
        public int DatabaseOffset { get; }

        private PaintChipStore(EmbeddedSqliteDb database, int databaseOffset, byte[] trailingBytes)
        {
            _database = database;
            DatabaseOffset = databaseOffset;
            _trailingBytes = trailingBytes;
        }

        /// <summary>
        /// Attempts to locate and open the SQLite database embedded in <paramref name="save"/>.
        /// Returns <c>false</c> (with a friendly <paramref name="error"/>) when the save is empty,
        /// not a GT5 save, or its database cannot be read.
        /// </summary>
        public static bool TryOpen(byte[]? save, out PaintChipStore? store, out string error)
        {
            store = null;
            error = string.Empty;

            if (save is null || save.Length == 0)
            {
                error = "No save data is loaded.";
                return false;
            }

            var offset = FindSqliteDatabase(save);
            if (offset < 0)
            {
                error = "No embedded item database was found in this save.";
                return false;
            }

            var pageSize = ReadPageSize(save, offset);
            if (pageSize != PageSize)
            {
                error = $"The embedded database uses an unsupported page size ({pageSize}).";
                return false;
            }

            // Layout: [binary section][whole number of pages][optional trailing bytes].
            var remaining = save.Length - offset;
            var databaseLength = remaining / pageSize * pageSize;
            if (databaseLength < pageSize)
            {
                error = "The embedded database is not page-aligned.";
                return false;
            }

            var databaseBytes = new byte[databaseLength];
            Array.Copy(save, offset, databaseBytes, 0, databaseLength);

            var trailingLength = remaining - databaseLength;
            var trailingBytes = new byte[trailingLength];
            Array.Copy(save, offset + databaseLength, trailingBytes, 0, trailingLength);

            try
            {
                var database = EmbeddedSqliteDb.OpenFromBytes(databaseBytes);
                try
                {
                    // Deserialization is lazy, so prove the database is usable before relying on it.
                    database.ScalarInt64("SELECT COUNT(*) FROM t_itembox_user");
                }
                catch
                {
                    database.Dispose();
                    throw;
                }

                store = new PaintChipStore(database, offset, trailingBytes);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>Returns the set of colour ids (see <see cref="PaintDatabase"/>) the player already owns.</summary>
        public HashSet<uint> GetOwnedColourIds()
        {
            var owned = new HashSet<uint>();
            _database.Query(
                $"SELECT DISTINCT argument1 FROM t_itembox_user WHERE type_id = {GtAutoTypeId} AND category_id = {ColorPaintCategoryId} AND argument1 IS NOT NULL",
                reader =>
                {
                    if (!reader.IsDBNull(0)) owned.Add((uint)reader.GetInt64(0));
                });
            return owned;
        }

        /// <summary>Returns how many chips of the given colour are already owned.</summary>
        public int GetOwnedCount(uint colourId) =>
            (int)_database.ScalarInt64(
                $"SELECT COUNT(*) FROM t_itembox_user WHERE type_id = {GtAutoTypeId} AND category_id = {ColorPaintCategoryId} AND argument1 = @colour",
                bind => bind.AddWithValue("@colour", (long)colourId));

        /// <summary>The total number of visible items in the item box (used for the engine's 7000-item cap).</summary>
        public int GetVisibleItemCount() =>
            (int)_database.ScalarInt64($"SELECT COUNT(*) FROM t_itembox_user WHERE status = {VisibleStatus}");

        /// <summary>
        /// Adds <paramref name="count"/> paint chips of the given colour to the item box and
        /// returns how many were inserted. The colour id and category come straight from a
        /// <see cref="PaintEntry"/>, so the chip matches the in-game paint exactly.
        /// </summary>
        /// <exception cref="InvalidOperationException">Adding the chips would exceed the item-box cap.</exception>
        public int AddChips(uint colourId, int category, string maker, string name, int count)
        {
            if (count <= 0) return 0;

            var visible = GetVisibleItemCount();
            if ((long)visible + count > MaxVisibleItems)
                throw new InvalidOperationException(
                    $"Adding {count} chip(s) would exceed the game's {MaxVisibleItems}-item limit (the item box already holds {visible} items).");

            // Match the conventions the game itself uses when it awards items: ids and
            // timestamps carry on from the current maximum, and the game day is the latest one.
            var nextId = _database.ScalarInt64("SELECT COALESCE(MAX(itembox_id), 0) FROM t_itembox_user");
            var nextDate = _database.ScalarInt64("SELECT COALESCE(MAX(create_date), 0) FROM t_itembox_user");
            var gameday = _database.ScalarInt64("SELECT COALESCE(MAX(gameday), 1) FROM t_itembox_user");
            var gameItemId = category is 4 or 5 ? SpecialPaintItemId : PaintItemId;

            // argument3 is a paint-specific value the game assigns per colour that is not part of
            // the colour catalogue we ship. When the colour is already owned we reuse its value for
            // fidelity; otherwise we leave it 0, which is what the community save editor writes and
            // is accepted by the game.
            var argument3 = _database.ScalarInt64(
                $"SELECT COALESCE(MAX(argument3), 0) FROM t_itembox_user WHERE type_id = {GtAutoTypeId} AND category_id = {ColorPaintCategoryId} AND argument1 = @colour",
                bind => bind.AddWithValue("@colour", (long)colourId));

            var inserted = 0;
            _database.RunInTransaction(() =>
            {
                for (var i = 0; i < count; i++)
                {
                    _database.Execute(
                        "INSERT INTO t_itembox_user (" +
                        "itembox_id, create_date, gameday, status, type_id, category_id, gameitem_id, " +
                        "exp_threshold, item_name, presentbox_itembox_id, sender, comment, gameday_start, " +
                        "gameday_end, argument1, argument2, argument3, argument4, code, blob, present_flag) " +
                        "VALUES (@id, @date, @gameday, @status, @type, @category, @gameItem, " +
                        "0, @name, NULL, NULL, NULL, NULL, NULL, @colour, @paintCategory, @argument3, 0, @maker, '', 1)",
                        bind =>
                        {
                            bind.AddWithValue("@id", nextId + 1 + i);
                            bind.AddWithValue("@date", nextDate + 1 + i);
                            bind.AddWithValue("@gameday", gameday);
                            bind.AddWithValue("@status", VisibleStatus);
                            bind.AddWithValue("@type", GtAutoTypeId);
                            bind.AddWithValue("@category", ColorPaintCategoryId);
                            bind.AddWithValue("@gameItem", gameItemId);
                            bind.AddWithValue("@name", name ?? string.Empty);
                            bind.AddWithValue("@colour", (long)colourId);
                            bind.AddWithValue("@paintCategory", category);
                            bind.AddWithValue("@argument3", argument3);
                            bind.AddWithValue("@maker", maker ?? string.Empty);
                        });
                    inserted++;
                }
            });

            return inserted;
        }

        /// <summary>
        /// Returns a copy of <paramref name="save"/> with the edited database written back into
        /// it. The binary section is preserved and the three size fields the container keeps for
        /// the database are updated for the new page count.
        /// </summary>
        public byte[] WriteInto(byte[] save)
        {
            ArgumentNullException.ThrowIfNull(save);

            if (DatabaseOffset <= 8 || save.Length < DatabaseOffset)
                throw new InvalidOperationException("The save layout is not supported.");

            var database = _database.Serialize();
            if (database.Length % PageSize != 0)
                throw new InvalidOperationException("The edited database is not page-aligned.");

            var result = new byte[DatabaseOffset + database.Length + _trailingBytes.Length];
            Array.Copy(save, 0, result, 0, DatabaseOffset);
            Array.Copy(database, 0, result, DatabaseOffset, database.Length);
            if (_trailingBytes.Length > 0)
                Array.Copy(_trailingBytes, 0, result, DatabaseOffset + database.Length, _trailingBytes.Length);

            // The container records the database size in three big-endian 32-bit fields. The
            // "+11"/"+10" constants are the values the game uses alongside its single trailing
            // byte (which every GT5.0 save has).
            WriteUInt32BigEndian(result, 0x0C, (uint)(database.Length + 11));
            WriteUInt32BigEndian(result, DatabaseOffset - 9, (uint)(database.Length + 10));
            WriteUInt32BigEndian(result, DatabaseOffset - 4, (uint)database.Length);

            return result;
        }

        /// <summary>
        /// Finds the first plausible SQLite database header in the save. The magic string alone
        /// is not enough - a text or blob value could contain it - so the header's page size must
        /// also be a sane power of two and the format version bytes must be 1.
        /// </summary>
        private static int FindSqliteDatabase(byte[] save)
        {
            var start = 0;
            while (true)
            {
                var offset = IndexOf(save, SqliteMagic, start);
                if (offset < 0) return -1;

                if (offset + SqliteHeaderLength <= save.Length)
                {
                    var pageSize = ReadPageSize(save, offset);
                    var isPowerOfTwo = pageSize >= 512 && pageSize <= 65536 && (pageSize & (pageSize - 1)) == 0;
                    if (isPowerOfTwo && save[offset + 18] == 1 && save[offset + 19] == 1)
                        return offset;
                }

                start = offset + 1;
            }
        }

        /// <summary>Reads the SQLite header's page size (big-endian; 1 encodes 65536).</summary>
        private static int ReadPageSize(byte[] save, int offset)
        {
            var raw = (save[offset + 16] << 8) | save[offset + 17];
            return raw == 1 ? 65536 : raw;
        }

        private static void WriteUInt32BigEndian(byte[] buffer, int offset, uint value)
        {
            buffer[offset] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)value;
        }

        private static int IndexOf(byte[] haystack, byte[] needle, int start)
        {
            var last = haystack.Length - needle.Length;
            for (var i = Math.Max(0, start); i <= last; i++)
            {
                var match = true;
                for (var j = 0; j < needle.Length; j++)
                {
                    if (haystack[i + j] == needle[j]) continue;
                    match = false;
                    break;
                }

                if (match) return i;
            }

            return -1;
        }

        public void Dispose() => _database.Dispose();
    }
}
