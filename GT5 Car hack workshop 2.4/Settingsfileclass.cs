using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GT5_Car_hack_workshop_2.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace GT5_Car_hack_workshop_2
{
    // Token: 0x0200000E RID: 14
    public class Settingsfileclass
    {
        // Token: 0x06000173 RID: 371 RVA: 0x000025C4 File Offset: 0x000009C4
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void savesettings(string[] settings, string file)
        {
            if (MyProject.Computer.FileSystem.FileExists(file))
            {
                FileSystem.Kill(file);
            }

            StreamWriter swr = new StreamWriter(file);
            int num = 0;
            checked
            {
                int num2 = (int)Math.Round(Conversion.Val(settings.Length - 1));
                for (int i = num; i <= num2; i++)
                {
                    try
                    {
                        swr.WriteLine(settings[i].ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }

                swr.Flush();
                swr.Close();
            }
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
                string fileContent = System.Text.Encoding.UTF8.GetString(fileBytes);
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

        // Token: 0x06000175 RID: 373 RVA: 0x0000282C File Offset: 0x00000C2C
        public static object FindSequence(byte[] list, byte[] value, int startindex1)
        {
            int startIndex = Array.IndexOf<byte>(list, value[0], startindex1);
            checked
            {
                while (startIndex != -1 && list.Length - startIndex >= value.Length)
                {
                    int runLength = 0;
                    int num = 0;
                    int num2 = value.Length - 1;
                    int index = num;
                    while (index <= num2 && value[index] == list[startIndex + index])
                    {
                        runLength++;
                        index++;
                    }

                    if (runLength == value.Length)
                    {
                        return startIndex;
                    }

                    startIndex = Array.IndexOf<byte>(list, value[0], startIndex + runLength);
                }

                return -1;
            }
        }
    }
}