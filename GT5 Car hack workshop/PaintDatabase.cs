using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using GT5_Car_hack_workshop.Models;
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

        /// <summary>The colour's RGB value (0xRRGGBB), or null when the game data has no colour.</summary>
        public uint? Rgb { get; set; }

        private PaintColour? _colour;

        /// <summary>This colour as a value type, which is where every colour calculation lives.</summary>
        public PaintColour Colour => _colour ??= PaintColour.FromPacked(Rgb);

        private double? _luminance;

        /// <summary>
        /// How bright the colour looks, for the "Luminance" sort. Cached, because a sort asks for it
        /// far more often than once per colour (a comparer runs about n log n times).
        /// </summary>
        public double Luminance => _luminance ??= Colour.RelativeLuminance;

        private IBrush? _swatchBrush;

        /// <summary>The colour a swatch should show: the game's colour, else a neutral grey.</summary>
        public Color SwatchColor => Rgb is { } rgb
            ? PaintColour.FromPacked(rgb).ToColor()
            : Colors.Gray;

        /// <summary>A cached brush for <see cref="SwatchColor"/>, for the UI swatches.</summary>
        public IBrush SwatchBrush => _swatchBrush ??= new ImmutableSolidColorBrush(SwatchColor);

        /// <summary>The friendly description, so XAML item templates can bind to it.</summary>
        public string Display => ToString();

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
    /// One selectable finish (colour category) in a paint picker's filter, so the list can be
    /// narrowed to a single finish such as Metallic or Chrome.
    /// </summary>
    public sealed class PaintFinish
    {
        public PaintFinish(string name, int? category)
        {
            Name = name;
            Category = category;
        }

        public string Name { get; }

        /// <summary>The category this finish matches, or null for "any finish".</summary>
        public int? Category { get; }

        public override string ToString() => Name;
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

        /// <summary>The finishes a paint picker can be filtered by, "Any finish" first.</summary>
        public static IReadOnlyList<PaintFinish> Finishes { get; } = new List<PaintFinish>
        {
            new("Any finish", null),
            new("Solid", 0),
            new("Metallic", 1),
            new("Pearl", 2),
            new("Iridescent", 3),
            new("Matte", 4),
            new("Chrome", 5),
            new("Other", 99),
        };

        /// <summary>
        /// The catalogue entries of one finish, or every entry when <paramref name="category"/> is
        /// null. Used to narrow a picker's item list to a single finish.
        /// </summary>
        public static IReadOnlyList<PaintEntry> ByFinish(int? category)
            => category is null ? Entries : Entries.Where(e => e.Category == category).ToList();

        /// <summary>Resource name of the embedded SQLite catalogue.</summary>
        private const string ResourceName = "GT5_Car_hack_workshop.Resources.PaintDatabase.db";

        private static List<PaintEntry> Load()
        {
            var entries = new List<PaintEntry>
            {
                // Colour id 0 is the game's default and is also present in the dump; this entry is
                // kept so id 0 always resolves even if the dump is missing. It has no colour of its
                // own, so its swatch stays neutral.
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
                    "SELECT Id, Category, Maker, Name, Rgb FROM PaintColours ORDER BY Ord",
                    reader =>
                    {
                        var rgb = reader.IsDBNull(4) ? -1 : reader.GetInt64(4);
                        entries.Add(new PaintEntry
                        {
                            Id = (uint)reader.GetInt64(0),
                            Category = reader.GetInt32(1),
                            Maker = reader.GetString(2),
                            Name = reader.GetString(3),
                            Rgb = rgb is >= 0 and <= 0xFFFFFF ? (uint)rgb : null
                        });
                    });
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