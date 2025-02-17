using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;

namespace GT5_Car_hack_workshop_2
{
	// Token: 0x0200000A RID: 10
	public class loaddata
	{
		// Token: 0x06000167 RID: 359 RVA: 0x00002338 File Offset: 0x00000738
		public static object load(string file1)
		{
			FileInfo gt5file = new FileInfo(file1);
			Process proc = new Process();
			proc.StartInfo.FileName = "pfdtool";
			proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			proc.StartInfo.Arguments = "-g BCES00569 -d \"" + gt5file.Directory.FullName + "\" " + gt5file.Name;
			proc.Start();
			proc.WaitForExit();
			return File.ReadAllBytes(gt5file.FullName);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000023B4 File Offset: 0x000007B4
		public static void encrypt(object file1)
		{
			FileInfo gt5file = new FileInfo(Conversions.ToString(file1));
			Process proc = new Process();
			proc.StartInfo.FileName = "pfdtool";
			proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			proc.StartInfo.Arguments = "-g BCES00569 -e \"" + gt5file.Directory.FullName + "\" " + gt5file.Name;
			proc.Start();
			proc.WaitForExit();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00002428 File Offset: 0x00000828
		public static object FindSequence(byte[] list, byte[] value)
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
