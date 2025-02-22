using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace GT5_Car_hack_workshop_2
{
    // Token: 0x0200000E RID: 14
    public class Settingsfileclass
    {
        // Token: 0x06000173 RID: 371 RVA: 0x000025C4 File Offset: 0x000009C4
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void savesettings(string[] settings, string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            File.WriteAllLines(file, settings);
        }

        // Token: 0x06000174 RID: 372 RVA: 0x0000264C File Offset: 0x00000A4C
        public static string[] loadsettings(string file, int arrsize)
        {
            if (File.Exists(file))
            {
                var lines = LoadFileToArray(file);
                return lines.Take(arrsize + 1).ToArray();
            }

            return Enumerable.Repeat(" ", arrsize + 1).ToArray();
        }

        /// <summary>
        /// Reads the contents of the specified file into a string array, with each line of the file being an element in the array.
        /// </summary>
        /// <param name="file">The path of the file to load.</param>
        /// <returns>An array of strings where each element represents a line from the file. If the file does not exist, an empty array is returned.</returns>
        static string[] LoadFileToArray(string file)
        {
            var lines = new List<string>();

            if (File.Exists(file))
            {
                byte[] fileBytes = File.ReadAllBytes(file);
                string fileContent = Encoding.UTF8.GetString(fileBytes);
                string[] fileLines = fileContent.Split(new[] { "\r\n" }, StringSplitOptions.None);
                foreach (string line in fileLines)
                {
                    lines.Add(line);
                }
            }
            else
            {
                Console.WriteLine("File not found: " + file);
            }

            return lines.ToArray();
        }
    }
}