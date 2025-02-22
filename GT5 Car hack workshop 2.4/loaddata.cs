using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;

namespace GT5_Car_hack_workshop_2
{
	// Token: 0x0200000A RID: 10
	public class loaddata
	{
		/// <summary>
		/// Unencrypt and load GT5.0 file into byte array which is then returned
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
		/// Encrypt and save GT5.0 file.
		/// </summary>
		/// <param name="path">Path to the GT5.0 file to be encrypted</param>
		public static void Encrypt(object path)
		{
			var gt5File = new FileInfo(Conversions.ToString(path));
			var proc = new Process();
			proc.StartInfo.FileName = "pfdtool";
			proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			proc.StartInfo.Arguments = $"-g BCES00569 -e \"{gt5File.Directory.FullName}\" {gt5File.Name}";
			proc.Start();
			proc.WaitForExit();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00002428 File Offset: 0x00000828
		public static int FindSequence(byte[] list, byte[] value)
		{
			int startIndex = Array.IndexOf<byte>(list, value[0]);
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
