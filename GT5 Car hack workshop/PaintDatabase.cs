using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

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
    /// Provides access to GT5's paint colour database, embedded from the community dump
    /// (t_colour table of the GT5 Garage Editor 1.3.1 colour database, via the
    /// gt5GarageEditor project). Colour IDs index the game's internal paint database.
    /// </summary>
    public static class PaintDatabase
    {
        private static List<PaintEntry>? _entries;

        public static IReadOnlyList<PaintEntry> Entries => _entries ??= Load();

        private static List<PaintEntry> Load()
        {
            var entries = new List<PaintEntry>
            {
                // Colour ID 0 is the game's default (black) colour and is not part of the dump.
                new PaintEntry { Id = 0, Category = 0, Name = "Default Colour (Black)", Maker = "polyphony" }
            };

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("GT5_Car_hack_workshop.Resources.PaintDatabase.tsv");
            if (stream == null) return entries; // Database missing - text boxes still work as normal

            using var reader = new StreamReader(stream);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length == 0 || line[0] == '#') continue;

                var parts = line.Split('\t');
                if (parts.Length < 3 || !uint.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var id)) continue;

                entries.Add(new PaintEntry
                {
                    Id = id,
                    Category = int.TryParse(parts[1], out var category) ? category : 0,
                    Maker = parts.Length > 2 ? parts[2] : "",
                    Name = parts.Length > 3 ? parts[3] : ""
                });
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
            var index = IndexOf(id);
            return index >= 0 ? Entries[index] : null;
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