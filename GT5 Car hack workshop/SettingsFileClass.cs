using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GT5_Car_hack_workshop.Models;

namespace GT5_Car_hack_workshop
{
    public static class SettingsFileClass
    {
        public static void SaveSettings(string[] settings, string file)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, file);
            if (File.Exists(filePath)) File.Delete(filePath);

            File.WriteAllLines(filePath, settings);
        }

        public static string[] LoadSettings(string file, int arraySize)
        {
            var requiredSize = arraySize + 1;
            var filePath = Path.Combine(AppContext.BaseDirectory, file);

            if (File.Exists(filePath))
            {
                var lines = LoadFileToArray(filePath);
                // Ensure we always return the required size by padding with spaces if needed
                if (lines.Length < requiredSize)
                {
                    var paddedArray = new string[requiredSize];
                    for (int i = 0; i < requiredSize; i++)
                    {
                        paddedArray[i] = i < lines.Length ? lines[i] : " ";
                    }
                    return paddedArray;
                }
                return lines.Take(requiredSize).ToArray();
            }

            return Enumerable.Repeat(" ", requiredSize).ToArray();
        }

        /// <summary>
        ///     Reads the contents of the specified file into a string array, with each line of the file being an element in the array.
        ///     Handles both Windows (\r\n) and Unix (\n) line endings.
        /// </summary>
        /// <param name="file">The path of the file to load.</param>
        /// <returns>An array of strings where each element represents a line from the file. If the file does not exist, an empty array is returned.</returns>
        private static string[] LoadFileToArray(string file)
        {
            var lines = new List<string>();

            if (File.Exists(file))
            {
                // Read all text and handle both Windows and Unix line endings
                var fileContent = File.ReadAllText(file);
                // Split by both \r\n and \n, removing empty entries only if they result from the split
                var fileLines = fileContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                foreach (var line in fileLines) 
                    lines.Add(line);
            }
            else
            {
                Console.WriteLine("File not found: " + file);
            }

            return lines.ToArray();
        }

        public static List<CarParts> LoadCarParts(string file){
            var carPartsList = new List<CarParts>();
            var filePath = Path.Combine(AppContext.BaseDirectory, file);

            if (!File.Exists(filePath))
            {
                return carPartsList;
            }

            var lines = LoadFileToArray(filePath);
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

            var filePath = Path.Combine(AppContext.BaseDirectory, file);
            if (File.Exists(filePath)) File.Delete(filePath);
            File.WriteAllLines(filePath, lines);
        }
    }
}