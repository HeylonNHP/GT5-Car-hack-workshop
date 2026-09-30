using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using GT5_Car_hack_workshop.Services;

namespace GT5_Car_hack_workshop
{
    /// <summary>
    /// A single entry of GT5's internal paint (colour) database.
    /// </summary>
    public class PaintEntry
    {
        public uint Id { get; set; }
        public int Category { get; set; }
        public string Name { get; set; } = "";
        public string Maker { get; set; } = "";

        public string CategoryName => Category switch
        {
            0 => "Solid",
            1 => "Metallic",
            2 => "Pearl",
            3 => "Iridescent",
            4 => "Matte",
            5 => "Chrome",
            _ => "Paint"
        };

        public string MakerName => Maker switch
        {
            "ac" => "AC", "acura" => "Acura", "alfaromeo" => "Alfa Romeo", "alpine" => "Alpine",
            "amuse" => "Amuse", "asl" => "ASL", "astonmartin" => "Aston Martin", "audi" => "Audi",
            "autobianchi" => "Autobianchi", "bmw" => "BMW", "buick" => "Buick", "callaway" => "Callaway",
            "caterham" => "Caterham", "chevrolet" => "Chevrolet", "chrysler" => "Chrysler", "citroen" => "Citroën",
            "cizeta" => "Cizeta", "daihatsu" => "Daihatsu", "dodge" => "Dodge", "eagle" => "Eagle",
            "ferrari" => "Ferrari", "fiat" => "Fiat", "ford" => "Ford", "ginetta" => "Ginetta",
            "holden" => "Holden", "hommell" => "Hommell", "honda" => "Honda", "hyundai" => "Hyundai",
            "infiniti" => "Infiniti", "isuzu" => "Isuzu", "jaguar" => "Jaguar", "jensen" => "Jensen",
            "lamborghini" => "Lamborghini", "lancia" => "Lancia", "lexus" => "Lexus", "lotus" => "Lotus",
            "marcos" => "Marcos", "maserati" => "Maserati", "mazda" => "Mazda", "mercedes" => "Mercedes-Benz",
            "mercury" => "Mercury", "mg_mini" => "MG", "mines" => "Mines", "mini" => "MINI",
            "mitsubishi" => "Mitsubishi", "nismo" => "Nismo", "nissan" => "Nissan", "opel" => "Opel",
            "pagani" => "Pagani", "peugeot" => "Peugeot", "plymouth" => "Plymouth", "polyphony" => "Polyphony Digital",
            "pontiac" => "Pontiac", "proto" => "Proto", "renault" => "Renault", "saleen" => "Saleen",
            "scion" => "Scion", "seat" => "SEAT", "shelby" => "Shelby", "spoon" => "Spoon",
            "spyker" => "Spyker", "subaru" => "Subaru", "suzuki" => "Suzuki", "tesla_motors" => "Tesla",
            "tommykaira" => "TommyKaira", "toms" => "TOM'S", "toyota" => "Toyota", "trd" => "TRD",
            "triumph" => "Triumph", "tvr" => "TVR", "vauxhall" => "Vauxhall", "volkswagen" => "Volkswagen",
            "volvo" => "Volvo",
            _ => Maker
        };

        public override string ToString() => $"{Name} ({MakerName}, {CategoryName}) - {Id:X4}";
    }

    /// <summary>
    /// Provides access to GT5's paint colour database. The catalogue is stored as an embedded
    /// SQLite database (converted from the community dump - the t_colour table of the GT5 Garage
    /// Editor 1.3.1 colour database, via the gt5GarageEditor project). Colour IDs index the game's
    /// internal paint database.
    /// </summary>
    public static class PaintDatabase
    {
        private static List<PaintEntry>? _entries;
        private static Dictionary<uint, PaintEntry>? _byId;

        public static IReadOnlyList<PaintEntry> Entries => _entries ??= Load();

        /// <summary>Resource name of the embedded SQLite catalogue.</summary>
        private const string ResourceName = "GT5_Car_hack_workshop.Resources.PaintDatabase.db";

        private static List<PaintEntry> Load()
        {
            var entries = new List<PaintEntry>
            {
                // Colour ID 0 is the game's default (black) colour and is not part of the dump.
                new PaintEntry { Id = 0, Category = 0, Name = "Default Colour (Black)", Maker = "polyphony" }
            };

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using var stream = assembly.GetManifestResourceStream(ResourceName);
                if (stream == null) return entries; // Database missing - text boxes still work as normal

                var database = new byte[stream.Length];
                stream.ReadExactly(database);

                // Read the colours in their stored order (Ord), which matches the order the old
                // text dump listed them in, so the palette and its search ordering are unchanged.
                using var db = EmbeddedSqliteDb.OpenFromBytes(database);
                db.Query(
                    "SELECT Id, Category, Maker, Name FROM PaintColours ORDER BY Ord",
                    reader => entries.Add(new PaintEntry
                    {
                        Id = (uint)reader.GetInt64(0),
                        Category = reader.GetInt32(1),
                        Maker = reader.GetString(2),
                        Name = reader.GetString(3)
                    }));
            }
            catch (Exception)
            {
                // Never let a bad catalogue stop the app from starting: fall back to just the
                // default colour. Painting by raw id in the text boxes still works.
            }

            return entries;
        }

        /// <summary>Returns the list index of the entry with the given colour ID, or -1 if unknown.</summary>
        public static int IndexOf(uint id)
        {
            for (var i = 0; i < Entries.Count; i++)
                if (Entries[i].Id == id) return i;
            return -1;
        }

        /// <summary>Returns the colour entry with the given ID, or null if unknown.</summary>
        public static PaintEntry? Find(uint id)
        {
            // The catalogue holds thousands of rows and Find is called once per owned colour when
            // the owned-chips list refreshes, so look the entry up rather than scanning each time.
            _byId ??= BuildIdLookup();
            return _byId.TryGetValue(id, out var entry) ? entry : null;
        }

        private static Dictionary<uint, PaintEntry> BuildIdLookup()
        {
            var lookup = new Dictionary<uint, PaintEntry>(Entries.Count);
            foreach (var entry in Entries)
                if (!lookup.ContainsKey(entry.Id)) // first entry wins, matching IndexOf
                    lookup.Add(entry.Id, entry);
            return lookup;
        }

        /// <summary>
        /// True when a colour matches the text typed into a paint search box. An empty search
        /// matches everything; otherwise the colour's name, maker or hex id is compared.
        /// </summary>
        public static bool MatchesSearch(PaintEntry entry, string? search)
        {
            if (string.IsNullOrWhiteSpace(search)) return true;
            var s = search.Trim().ToLowerInvariant();
            return entry.Name.ToLowerInvariant().Contains(s)
                   || entry.MakerName.ToLowerInvariant().Contains(s)
                   || entry.Id.ToString("X4").ToLowerInvariant().Contains(s);
        }

        /// <summary>
        /// Resolves text a user typed into a paint search box to a colour entry: a hex id
        /// (e.g. "0D1B"), a full description ("White (Daihatsu, Solid) - 08D6") or an exact
        /// colour name. Returns null when nothing matches.
        /// </summary>
        public static PaintEntry? Resolve(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            var trimmed = text.Trim();
            var hex = trimmed.Replace(" ", string.Empty).Replace("0x", string.Empty).Replace("0X", string.Empty);
            if (hex.Length is > 0 and <= 4
                && uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var id))
            {
                var byId = Find(id);
                if (byId is not null) return byId;
            }

            return Entries.FirstOrDefault(e => string.Equals(e.ToString(), trimmed, StringComparison.OrdinalIgnoreCase))
                   ?? Entries.FirstOrDefault(e => string.Equals(e.Name, trimmed, StringComparison.OrdinalIgnoreCase));
        }
    }
}