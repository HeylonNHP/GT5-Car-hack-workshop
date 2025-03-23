using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GT5_Car_hack_workshop_2.Models;

namespace GT5_Car_hack_workshop_2
{
    public static class SettingsFileClass
    {
        public static void SaveSettings(string[] settings, string file)
        {
            if (File.Exists(file)) File.Delete(file);

            File.WriteAllLines(file, settings);
        }

        public static string[] LoadSettings(string file, int arraySize)
        {
            if (File.Exists(file))
            {
                var lines = LoadFileToArray(file);
                return lines.Take(arraySize + 1).ToArray();
            }

            return Enumerable.Repeat(" ", arraySize + 1).ToArray();
        }

        /// <summary>
        ///     Reads the contents of the specified file into a string array, with each line of the file being an element in the array.
        /// </summary>
        /// <param name="file">The path of the file to load.</param>
        /// <returns>An array of strings where each element represents a line from the file. If the file does not exist, an empty array is returned.</returns>
        private static string[] LoadFileToArray(string file)
        {
            var lines = new List<string>();

            if (File.Exists(file))
            {
                var fileBytes = File.ReadAllBytes(file);
                var fileContent = Encoding.UTF8.GetString(fileBytes);
                var fileLines = fileContent.Split(new[] { "\r\n" }, StringSplitOptions.None);
                foreach (var line in fileLines) lines.Add(line);
            }
            else
            {
                Console.WriteLine("File not found: " + file);
            }

            return lines.ToArray();
        }

        public static List<CarParts> LoadCarParts(string file){
            var carPartsList = new List<CarParts>();
            
            if (!File.Exists(file))
            {
                return carPartsList;
            }

            var lines = LoadFileToArray(file);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line == " ")
                    continue;

                try
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 9) // Ensure we have all required fields
                    {
                        var carPart = new CarParts
                        {
                            Name = parts[0],
                            Engine = ByteUtils.HexStringToUshort(parts[1]),
                            Drivetrain = ByteUtils.HexStringToUshort(parts[2]),
                            Chassis = ByteUtils.HexStringToUshort(parts[3]),
                            Transmission = ByteUtils.HexStringToUshort(parts[4]),
                            Body = ByteUtils.HexStringToUshort(parts[5]),
                            Suspension = ByteUtils.HexStringToUshort(parts[6]),
                            Lsd = ByteUtils.HexStringToUshort(parts[7]),
                            Horn = ByteUtils.HexStringToUshort(parts[8])
                        };
                        carPartsList.Add(carPart);
                    }
                }
                catch (Exception)
                {
                    // Skip invalid entries
                    continue;
                }
            }

            return carPartsList;
        }

        public static void SaveCarParts(List<CarParts> carParts, string file)
        {
            var lines = new List<string>();
            
            foreach (var part in carParts)
            {
                var line = $"{part.Name}," +
                          $"{ByteUtils.UshortToHexString(part.Engine)}," +
                          $"{ByteUtils.UshortToHexString(part.Drivetrain)}," +
                          $"{ByteUtils.UshortToHexString(part.Chassis)}," +
                          $"{ByteUtils.UshortToHexString(part.Transmission)}," +
                          $"{ByteUtils.UshortToHexString(part.Body)}," +
                          $"{ByteUtils.UshortToHexString(part.Suspension)}," +
                          $"{ByteUtils.UshortToHexString(part.Lsd)}," +
                          $"{ByteUtils.UshortToHexString(part.Horn)}";
                lines.Add(line);
            }

            if (File.Exists(file)) File.Delete(file);
            File.WriteAllLines(file, lines);
        }
    }
}