using System.Diagnostics;
using System.IO;

namespace GT5_Car_hack_workshop
{
    public static class LoadData
    {
	    /// <summary>
	    ///     Unencrypt and load GT5.0 file into byte array which is then returned
	    /// </summary>
	    /// <param name="path">Path to GT5.0 file</param>
	    /// <returns>Unencrypted GT5 save file</returns>
	    public static byte[] Load(string path)
        {
            var gt5File = new FileInfo(path);
            var proc = new Process();
            proc.StartInfo.FileName = "pfdtool";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.Arguments = $"-g BCES00569 -d \"{gt5File.Directory.FullName}\" {gt5File.Name}";
            proc.Start();
            proc.WaitForExit();
            return File.ReadAllBytes(gt5File.FullName);
        }

	    /// <summary>
	    ///     Encrypt and save GT5.0 file.
	    /// </summary>
	    /// <param name="path">Path to the GT5.0 file to be encrypted</param>
	    public static void Encrypt(string path)
        {
            var gt5File = new FileInfo(path);
            var proc = new Process();
            proc.StartInfo.FileName = "pfdtool";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.Arguments = $"-g BCES00569 -e \"{gt5File.Directory.FullName}\" {gt5File.Name}";
            proc.Start();
            proc.WaitForExit();
        }

	    /// <summary>
	    ///     Finds the first occurrence of a sequence of bytes within a byte array and returns the starting index.
	    /// </summary>
	    /// <param name="list">The byte array to search within.</param>
	    /// <param name="value">The sequence of bytes to search for.</param>
	    /// <returns>The starting index of the first occurrence of the sequence within the array, or -1 if the sequence is not found.</returns>
	    public static int FindSequence(byte[] list, byte[] value)
        {
            // If the value array is empty or longer than the list, return -1 immediately
            if (value == null || value.Length == 0 || list == null || value.Length > list.Length) return -1;

            // Iterate through the list, ensuring there's enough room for the value array to fit
            for (var i = 0; i <= list.Length - value.Length; i++)
            {
                // Check if the subarray from the current position matches the value array
                var match = true;
                for (var j = 0; j < value.Length; j++)
                    if (list[i + j] != value[j])
                    {
                        match = false;
                        break;
                    }

                // If a complete match is found, return the starting index
                if (match) return i;
            }

            // Return -1 if the sequence is not found
            return -1;
        }
    }
}