using System;
using System.Collections;
using System.IO;
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
		public static object loadsettings(string file, int arrsize)
		{
			ArrayList lines = new ArrayList();
			checked
			{
				if (MyProject.Computer.FileSystem.FileExists(file))
				{
					byte[] f = File.ReadAllBytes(file);
					int i = 0;
					byte[] array = new byte[f.Length - 1 + 2 + 1];
					int num = 0;
					int num2 = f.Length - 1;
					for (int j = num; j <= num2; j++)
					{
						array[j] = f[j];
					}
					array[array.Length - 2] = 13;
					array[array.Length - 1] = 10;
					while (i < f.Length - 3)
					{
						int num3 = Conversions.ToInteger(Operators.SubtractObject(Settingsfileclass.FindSequence(array, new byte[] { 13, 10 }, i), 1));
						string text = "";
						int num4 = i;
						int num5 = num3;
						object obj;
						for (int k = num4; k <= num5; k++)
						{
							obj = Operators.AddObject(Operators.AddObject(obj, array[k].ToString()), ",");
							text += Conversions.ToString(Strings.Chr((int)array[k]));
						}
						obj = obj.ToString().Replace(Conversions.ToString(10), "");
						text = text.ToString().Replace("\n", "");
						lines.Add(text);
						i = num3 + 2;
					}
					string[] array2 = new string[arrsize + 1];
					int num6 = 0;
					int num7 = lines.Count - 1;
					for (int l = num6; l <= num7; l++)
					{
						try
						{
							array2[l] = Conversions.ToString(lines[l]);
						}
						catch (Exception ex)
						{
							array2[l] = " ";
						}
					}
					return array2;
				}
				string[] settings = new string[arrsize + 1];
				for (int m = 0; m <= arrsize; m++)
				{
					settings[m] = " ";
				}
				return settings;
			}
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
