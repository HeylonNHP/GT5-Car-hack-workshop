using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
    }
}