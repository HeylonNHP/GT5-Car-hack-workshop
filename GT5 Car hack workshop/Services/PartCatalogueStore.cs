using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GT5_Car_hack_workshop.Models;
using Microsoft.Data.Sqlite;

namespace GT5_Car_hack_workshop.Services
{
    /// <summary>One part a known tune asks for, resolved to a catalogue entry.</summary>
    public sealed class PresetEntry
    {
        /// <summary>The catalogue category (game table id) the tune sets.</summary>
        public int Category { get; init; }

        /// <summary>The entry the tune picks, already resolved to the tune's car where possible.</summary>
        public PartEntry Part { get; init; } = null!;
    }

    /// <summary>A car body code from the game's VARIATION data (the catalogue's <c>Bodies</c> table).</summary>
    public sealed class CarBody
    {
        public int Code { get; init; }

        public string Name { get; init; } = "";

        public override string ToString() => Name;
    }

    /// <summary>
    /// Reads the generated parts catalogue (<c>partscatalogue.db</c>, built from the game's own data
    /// files) that sits next to the executable.
    /// <para>
    /// The catalogue is READ-ONLY at runtime: the connection is opened read-only and nothing here
    /// ever writes. The hand-built <c>partsdatabase.db</c> - the <c>CarParts</c> table that still
    /// backs the Body and Horn drop-downs - is untouched by this class and keeps being saved by
    /// <see cref="PartsDatabaseStore"/>.
    /// </para>
    /// <para>
    /// Everything is loaded once, lazily, on first use (about 116 000 part rows over 30 categories,
    /// 3 000 tune names and 1 150 bodies). A missing or broken catalogue is never fatal: the
    /// drop-downs just come back empty and typing a code by hand keeps working. The tyre drop-downs
    /// are the one exception - their 15 universal grades are built in code, so they work either way.
    /// </para>
    /// </summary>
    public static class PartCatalogueStore
    {
        /// <summary>The catalogue file name, resolved next to the executable.</summary>
        public const string FileName = "partscatalogue.db";

        /// <summary>
        /// The game's 15 tyre grades, in slot order. The save stores a gear slot, not a part key, and
        /// every grade fits every car, so the game's part tables have no rows to read: this list is the
        /// tyre catalogue.
        /// </summary>
        public static readonly IReadOnlyList<string> TyreGrades = new[]
        {
            "Comfort Hard", "Comfort Medium", "Comfort Soft",
            "Sports Hard", "Sports Medium", "Sports Soft", "Sports Super Soft",
            "Racing Hard", "Racing Medium", "Racing Soft", "Racing Super Soft",
            "Racing Intermediate", "Racing Rain",
            "Dirt", "Snow"
        };

        private static bool _loadAttempted;
        private static Dictionary<int, List<PartEntry>> _byCategory = new();
        private static Dictionary<int, Dictionary<string, PartEntry>> _byLabel = new();
        private static readonly List<string> _presetNames = new();
        private static readonly List<CarBody> _bodies = new();
        private static readonly List<TuningSourceCar> _cars = new();

        // Resolving a tune or a save's car touches the database, so remember the answers.
        private static readonly Dictionary<string, List<PresetEntry>> PresetCache = new(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, string> PresetCarCache = new(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<(int, int, int), int> CarIdCache = new();

        /// <summary>The full path of the catalogue next to the executable.</summary>
        public static string DatabasePath => System.IO.Path.Combine(AppContext.BaseDirectory, FileName);

        /// <summary>True when a catalogue file was found where the app expects it.</summary>
        public static bool Exists => File.Exists(DatabasePath);

        /// <summary>
        /// Every part of a category, ordered by level and then by car name. Empty when the catalogue
        /// is missing or the category has no parts (NOS is the one real example).
        /// </summary>
        public static IReadOnlyList<PartEntry> Entries(int category)
        {
            EnsureLoaded();
            return _byCategory.TryGetValue(category, out var entries) ? entries : Array.Empty<PartEntry>();
        }

        /// <summary>How many parts a category has, without keeping the list.</summary>
        public static int Count(int category) => Entries(category).Count;

        /// <summary>
        /// The entry a saved code refers to. Several cars can share the same code (the game stores
        /// one generic row for them), so the car the save belongs to is preferred when it is known.
        /// </summary>
        public static PartEntry? Find(int category, ushort partKey, int preferredCarId = 0)
        {
            EnsureLoaded();
            if (!_byCategory.TryGetValue(category, out var entries)) return null;

            PartEntry? first = null;
            foreach (var entry in entries)
            {
                if (entry.PartKey != partKey) continue;
                if (preferredCarId != 0 && entry.CarId == preferredCarId) return entry;
                first ??= entry;
            }

            return first;
        }

        /// <summary>
        /// The entry whose label is exactly the text given, which is how a drop-down turns what the
        /// user typed or picked back into a code. Null means "not a catalogue entry", i.e. the text
        /// is a raw hex code and is used as-is.
        /// </summary>
        public static PartEntry? FindByLabel(int category, string? label)
        {
            if (string.IsNullOrWhiteSpace(label)) return null;
            EnsureLoaded();
            return _byLabel.TryGetValue(category, out var labels) && labels.TryGetValue(label.Trim(), out var entry)
                ? entry
                : null;
        }

        /// <summary>Every known tune name (the save file's preset list), ordered by name.</summary>
        public static IReadOnlyList<string> PresetNames
        {
            get { EnsureLoaded(); return _presetNames; }
        }

        /// <summary>The car body codes of the game's VARIATION data (reserved for a body-swap feature).</summary>
        public static IReadOnlyList<CarBody> Bodies
        {
            get { EnsureLoaded(); return _bodies; }
        }

        /// <summary>
        /// Every car the catalogue knows (its <c>Cars</c> table), ordered by name. This is the tuning
        /// shop's "parts from car" picker: borrowing another car's parts is the point of that dialog,
        /// so it lists all of them rather than only the current car.
        /// </summary>
        public static IReadOnlyList<TuningSourceCar> Cars
        {
            get { EnsureLoaded(); return _cars; }
        }

        /// <summary>The catalogue car with the given id, or null when the catalogue has none.</summary>
        public static TuningSourceCar? FindCar(int id)
        {
            EnsureLoaded();
            foreach (var car in _cars)
                if (car.Id == id)
                    return car;

            return null;
        }

        /// <summary>Matches typed text against a car's name, for the source-car picker.</summary>
        public static bool MatchesCar(TuningSourceCar car, string? search)
            => string.IsNullOrWhiteSpace(search)
               || car.Name.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// The parts of one category that belong to one car: the car's own variants, which is what the
        /// tuning shop lists for its chosen source car. Empty when the catalogue has no such car or the
        /// car has no variant of that part (e.g. it has no supercharger option), so the dialog can say
        /// so honestly rather than show another car's parts.
        /// </summary>
        public static IReadOnlyList<PartEntry> EntriesForCar(int category, int carId)
        {
            EnsureLoaded();
            if (carId == 0 || !_byCategory.TryGetValue(category, out var entries))
                return Array.Empty<PartEntry>();

            var matches = new List<PartEntry>();
            foreach (var entry in entries)
                if (entry.CarId == carId)
                    matches.Add(entry);

            return matches;
        }

        /// <summary>The car a known tune belongs to, or null when it cannot be worked out.</summary>
        public static string? PresetCar(string name)
        {
            ResolvePreset(name); // fills the cache
            return PresetCarCache.TryGetValue(name, out var car) ? car : null;
        }

        /// <summary>
        /// The parts a known tune sets, one entry per category it defines, each resolved to the tune's
        /// own car where the catalogue has that car's variant. Categories the tune says nothing about
        /// (and codes the catalogue has no row for, such as the game's wheel entries) are left out.
        /// </summary>
        public static IReadOnlyList<PresetEntry> ResolvePreset(string name)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(name)) return Array.Empty<PresetEntry>();
            if (PresetCache.TryGetValue(name, out var cached)) return cached;

            var rows = new List<(int Category, int PartKey)>();
            try
            {
                using var connection = Open();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT Category, PartKey FROM Presets WHERE Name = @name ORDER BY Category";
                command.Parameters.AddWithValue("@name", name);
                using var reader = command.ExecuteReader();
                while (reader.Read()) rows.Add((reader.GetInt32(0), reader.GetInt32(1)));
            }
            catch (Exception)
            {
                // No tune list: return an empty result rather than failing the caller.
            }

            var carId = PreferredCar(rows);
            var carName = carId == 0 ? "" : CarNameOf(carId);

            var parts = new List<PresetEntry>(rows.Count);
            foreach (var (category, partKey) in rows)
            {
                var part = Find(category, (ushort)partKey, carId);
                if (part == null) continue; // category 39 (wheels) and the tyre codes have no part row
                parts.Add(new PresetEntry { Category = category, Part = part });
            }

            PresetCache[name] = parts;
            PresetCarCache[name] = carName;
            return parts;
        }

        /// <summary>
        /// The car a save belongs to, identified by the parts that belong to one car only: its engine,
        /// chassis and drivetrain codes. Used to show a car's own part variants when a save is loaded.
        /// Returns 0 when no car matches (e.g. a hand-edited save).
        /// </summary>
        public static int FindCarId(ushort engine, ushort chassis, ushort drivetrain)
        {
            EnsureLoaded();
            var key = ((int)engine, (int)chassis, (int)drivetrain);
            if (CarIdCache.TryGetValue(key, out var cached)) return cached;

            var carId = 0;
            try
            {
                using var connection = Open();
                using var command = connection.CreateCommand();
                command.CommandText =
                    "SELECT CarId FROM Parts WHERE CarId <> 0 AND (Category = 13 AND PartKey = @engine " +
                    "OR Category = 7 AND PartKey = @chassis OR Category = 11 AND PartKey = @drivetrain) " +
                    "GROUP BY CarId ORDER BY COUNT(*) DESC, CarId LIMIT 1";
                command.Parameters.AddWithValue("@engine", (int)engine);
                command.Parameters.AddWithValue("@chassis", (int)chassis);
                command.Parameters.AddWithValue("@drivetrain", (int)drivetrain);
                if (command.ExecuteScalar() is long id) carId = (int)id;
            }
            catch (Exception)
            {
                // No catalogue: no preferred car, which is fine.
            }

            CarIdCache[key] = carId;
            return carId;
        }

        /// <summary>
        /// True when an entry matches the text typed into a part drop-down. An empty search matches
        /// everything; otherwise the entry's label, car, part name and hex code are compared, so a
        /// user can type a part name, a car name or a code.
        /// </summary>
        public static bool MatchesSearch(PartEntry entry, string? search)
        {
            if (string.IsNullOrWhiteSpace(search)) return true;

            var text = search.Trim();
            if (entry.Label.Contains(text, StringComparison.OrdinalIgnoreCase)) return true;
            if (entry.CarName.Contains(text, StringComparison.OrdinalIgnoreCase)) return true;
            if (entry.PartName.Contains(text, StringComparison.OrdinalIgnoreCase)) return true;

            var packed = text.Replace(" ", string.Empty).Replace("0x", string.Empty, StringComparison.OrdinalIgnoreCase);
            return packed.Length > 0
                   && entry.PartKey.ToString("X4").Contains(packed, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Matches typed text against a known tune's name.</summary>
        public static bool MatchesPreset(string name, string? search)
            => string.IsNullOrWhiteSpace(search) || name.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase);

        private static string CarNameOf(int carId)
        {
            try
            {
                using var connection = Open();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT Name FROM Cars WHERE Id = @id";
                command.Parameters.AddWithValue("@id", carId);
                return command.ExecuteScalar() as string ?? "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Picks the car a tune belongs to: the car that owns the most of the tune's parts. The
        /// engine/chassis/drivetrain entries belong to exactly one car each and settle it; the shared
        /// rows that a whole run of cars owns do not skew the vote because they are counted per car.
        /// </summary>
        private static int PreferredCar(List<(int Category, int PartKey)> rows)
        {
            var votes = new Dictionary<int, int>();
            foreach (var (category, partKey) in rows)
            {
                if (!_byCategory.TryGetValue(category, out var entries)) continue;
                foreach (var entry in entries)
                {
                    if (entry.PartKey != (ushort)partKey || entry.CarId == 0) continue;
                    votes[entry.CarId] = votes.TryGetValue(entry.CarId, out var count) ? count + 1 : 1;
                }
            }

            var best = 0;
            var bestVotes = 0;
            foreach (var (carId, count) in votes.OrderBy(v => v.Key)) // the lowest car id wins a tie
            {
                if (count <= bestVotes) continue;
                best = carId;
                bestVotes = count;
            }

            return best;
        }

        /// <summary>Opens the catalogue read-only. The caller owns the connection and disposes it.</summary>
        private static SqliteConnection Open()
        {
            var connection = new SqliteConnection(new SqliteConnectionStringBuilder
            {
                DataSource = DatabasePath,
                Mode = SqliteOpenMode.ReadOnly
            }.ConnectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Whether a table has a named column, read through the schema: the catalogue gains
        /// columns over time (PartType, added by tools/patch_part_types.py) and an older file
        /// must still load rather than fail the parts SELECT.
        /// </summary>
        private static bool HasColumn(SqliteConnection connection, string table, string column)
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"PRAGMA table_info({table})";
            using var reader = command.ExecuteReader();
            while (reader.Read())
                if (string.Equals(reader.GetString(1), column, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }

        private static void EnsureLoaded()
        {
            if (_loadAttempted) return;
            _loadAttempted = true;

            try
            {
                Load();
            }
            catch (Exception)
            {
                // A missing or unreadable catalogue must never stop the editor from starting. The
                // drop-downs stay empty (the caller pre-fills those fields with the "stock/none"
                // code) and every bit of manual hex editing keeps working.
            }

            // Tyres need nothing from the catalogue file: their grades are universal and built here, so
            // they are added whatever happened above.
            AddTyreGrades();
        }

        /// <summary>
        /// Adds the game's 15 tyre grades to each tyre category as catalogue-style entries. They are
        /// built rather than read because the save stores a grade slot in an 8-byte key instead of a
        /// part id, and because no car owns a grade: the label is the grade's plain name.
        /// </summary>
        private static void AddTyreGrades()
        {
            foreach (var category in PartCatalogue.Categories)
            {
                if (category.FieldKind != PartFieldKind.TyreSlotKey) continue;

                var entries = new List<PartEntry>(TyreGrades.Count);
                var labels = new Dictionary<string, PartEntry>(StringComparer.OrdinalIgnoreCase);

                for (var slot = 0; slot < TyreGrades.Count; slot++)
                {
                    var entry = new PartEntry
                    {
                        Category = category.TableId,
                        PartKey = (ushort)slot,
                        Level = slot,
                        CarId = 0,     // tyres are car-independent, so no car is preferred
                        CarName = "",
                        PartName = TyreGrades[slot]
                    };

                    entry.Label = BuildLabel(category, entry, new Dictionary<(int, int), string>(),
                        new Dictionary<int, int>());
                    entries.Add(entry);
                    labels[entry.Label] = entry;
                }

                _byCategory[category.TableId] = entries;
                _byLabel[category.TableId] = labels;
            }
        }

        private static void Load()
        {
            if (!File.Exists(DatabasePath)) return;

            using var connection = Open();

            // The game's item list names an upgrade by its level; it is used to label entries.
            var items = new Dictionary<(int, int), string>();
            var firstLevel = new Dictionary<int, int>();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Category, Level, Name FROM Items";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var category = reader.GetInt32(0);
                    var level = reader.GetInt32(1);
                    items[(category, level)] = reader.GetString(2);
                    if (!firstLevel.TryGetValue(category, out var first) || level < first)
                        firstLevel[category] = level;
                }
            }

            _byCategory = new Dictionary<int, List<PartEntry>>();
            using (var command = connection.CreateCommand())
            {
                // PartType is the part's real SpecDB type byte (see PartEntry). A stale, unpatched
                // catalogue must still load, so the column is checked through the schema here and a
                // constant NULL takes its slot when the column is absent - those rows then read
                // exactly like the pre-type catalogue (the installer falls back to Level).
                var partTypeColumn = HasColumn(connection, "Parts", "PartType") ? "p.PartType" : "NULL";
                command.CommandText =
                    $"SELECT p.Category, p.PartKey, p.Level, p.CarId, {partTypeColumn}, COALESCE(c.Name, ''), COALESCE(i.Name, '') " +
                    "FROM Parts p " +
                    "LEFT JOIN Cars c ON c.Id = p.CarId " +
                    "LEFT JOIN Items i ON i.Category = p.Category AND i.Level = p.Level " +
                    "ORDER BY p.Category, p.Level, c.Name, p.PartKey";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var category = reader.GetInt32(0);
                    if (!_byCategory.TryGetValue(category, out var entries))
                        _byCategory[category] = entries = new List<PartEntry>();

                    entries.Add(new PartEntry
                    {
                        Category = category,
                        PartKey = (ushort)reader.GetInt32(1),
                        Level = reader.GetInt32(2),
                        CarId = reader.GetInt32(3),
                        PartType = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                        CarName = reader.GetString(5),
                        PartName = reader.GetString(6)
                    });
                }
            }

            // Label the entries, then make each category's labels unique: the label is what the user
            // types and what the box turns back into a code, so two entries must never share one. A
            // clash (two cars of the same name, say) is broken with the code, which is honest anyway.
            _byLabel = new Dictionary<int, Dictionary<string, PartEntry>>();
            foreach (var (categoryId, entries) in _byCategory)
            {
                var category = PartCatalogue.Find(categoryId);
                var labels = new Dictionary<string, PartEntry>(StringComparer.OrdinalIgnoreCase);

                foreach (var entry in entries)
                {
                    var label = BuildLabel(category, entry, items, firstLevel);
                    if (labels.ContainsKey(label)) label = $"{label} [{entry.Hex}]";
                    entry.Label = label;
                    if (!labels.ContainsKey(label)) labels.Add(label, entry);
                }

                _byLabel[categoryId] = labels;
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT DISTINCT Name FROM Presets ORDER BY Name";
                using var reader = command.ExecuteReader();
                while (reader.Read()) _presetNames.Add(reader.GetString(0));
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Code, Name FROM Bodies ORDER BY Name";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                    _bodies.Add(new CarBody { Code = reader.GetInt32(0), Name = reader.GetString(1) });
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Id, Name FROM Cars ORDER BY Name";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                    _cars.Add(new TuningSourceCar { Id = reader.GetInt32(0), Name = reader.GetString(1) });
            }
        }

        /// <summary>
        /// The display text of one entry: the game's upgrade name followed by the part's car for an
        /// upgrade, or just the car's name for a part that is the car's own identity. A catalogue row
        /// no car references (CarId 0) is called "generic". A tyre is neither: it carries no car at
        /// all, so its label is the grade's plain name.
        /// </summary>
        private static string BuildLabel(PartCategory? category, PartEntry entry,
            Dictionary<(int, int), string> items, Dictionary<int, int> firstLevel)
        {
            if (category is { Mode: PartLabelMode.Tyre })
                return string.IsNullOrWhiteSpace(entry.PartName) ? $"{category.Label} {entry.PartKey}" : entry.PartName;

            var car = string.IsNullOrWhiteSpace(entry.CarName) ? "generic" : entry.CarName;

            if (category is { Mode: PartLabelMode.Car }) return car;

            var name = PartNameOf(entry.Category, entry.Level, items, firstLevel)
                       ?? $"{category?.Label ?? "Part"} L{entry.Level}";
            return $"{name} ({car})";
        }

        /// <summary>
        /// The name to show for a level of a category. Level 0 is the part the car came with and the
        /// game's item list does not name it (only a few categories name a level 0 at all), so it is
        /// called "Stock" rather than borrowing the name of the first real upgrade, which would read
        /// as if the car already had that upgrade fitted. Anything the item list names is used as is.
        /// </summary>
        private static string? PartNameOf(int category, int level,
            Dictionary<(int, int), string> items, Dictionary<int, int> firstLevel)
        {
            if (items.TryGetValue((category, level), out var name)) return name;
            if (level == 0) return "Stock";
            if (firstLevel.TryGetValue(category, out var first) && items.TryGetValue((category, first), out name)) return name;
            return null;
        }
    }
}
