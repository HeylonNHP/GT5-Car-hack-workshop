using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GT5_Car_hack_workshop_2.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace GT5_Car_hack_workshop_2
{
	// Token: 0x02000008 RID: 8
	[DesignerGenerated]
	public partial class Customperformance : Form
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002D08 File Offset: 0x00001108
		public Customperformance()
		{
			base.Load += this.Customperformance_Load;
			this.InitializeComponent();
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00003864 File Offset: 0x00001C64
		// (set) Token: 0x06000025 RID: 37 RVA: 0x00003878 File Offset: 0x00001C78
		internal virtual Label Label1
		{
			get
			{
				return this._Label1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label1 = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00003884 File Offset: 0x00001C84
		// (set) Token: 0x06000027 RID: 39 RVA: 0x00003898 File Offset: 0x00001C98
		internal virtual Label Label2
		{
			get
			{
				return this._Label2;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label2 = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000038A4 File Offset: 0x00001CA4
		// (set) Token: 0x06000029 RID: 41 RVA: 0x000038B8 File Offset: 0x00001CB8
		internal virtual Label Label3
		{
			get
			{
				return this._Label3;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label3 = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000038C4 File Offset: 0x00001CC4
		// (set) Token: 0x0600002B RID: 43 RVA: 0x000038D8 File Offset: 0x00001CD8
		internal virtual TextBox TextBox1
		{
			get
			{
				return this._TextBox1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox1 = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000038E4 File Offset: 0x00001CE4
		// (set) Token: 0x0600002D RID: 45 RVA: 0x000038F8 File Offset: 0x00001CF8
		internal virtual TextBox TextBox2
		{
			get
			{
				return this._TextBox2;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox2 = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00003904 File Offset: 0x00001D04
		// (set) Token: 0x0600002F RID: 47 RVA: 0x00003918 File Offset: 0x00001D18
		internal virtual TextBox TextBox3
		{
			get
			{
				return this._TextBox3;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox3 = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00003924 File Offset: 0x00001D24
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00003938 File Offset: 0x00001D38
		internal virtual TextBox TextBox4
		{
			get
			{
				return this._TextBox4;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox4 = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00003944 File Offset: 0x00001D44
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00003958 File Offset: 0x00001D58
		internal virtual TextBox TextBox5
		{
			get
			{
				return this._TextBox5;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox5 = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00003964 File Offset: 0x00001D64
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00003978 File Offset: 0x00001D78
		internal virtual Label Label4
		{
			get
			{
				return this._Label4;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label4 = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00003984 File Offset: 0x00001D84
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00003998 File Offset: 0x00001D98
		internal virtual Button Button1
		{
			get
			{
				return this._Button1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.Button1_Click);
				if (this._Button1 != null)
				{
					this._Button1.Click -= eventHandler;
				}
				this._Button1 = value;
				if (this._Button1 != null)
				{
					this._Button1.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000039E4 File Offset: 0x00001DE4
		// (set) Token: 0x06000039 RID: 57 RVA: 0x000039F8 File Offset: 0x00001DF8
		internal virtual Button Button2
		{
			get
			{
				return this._Button2;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.Button2_Click);
				if (this._Button2 != null)
				{
					this._Button2.Click -= eventHandler;
				}
				this._Button2 = value;
				if (this._Button2 != null)
				{
					this._Button2.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00003A44 File Offset: 0x00001E44
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00003A58 File Offset: 0x00001E58
		internal virtual Button Button3
		{
			get
			{
				return this._Button3;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler eventHandler = new EventHandler(this.Button3_Click);
				if (this._Button3 != null)
				{
					this._Button3.Click -= eventHandler;
				}
				this._Button3 = value;
				if (this._Button3 != null)
				{
					this._Button3.Click += eventHandler;
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003AA4 File Offset: 0x00001EA4
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00003AB8 File Offset: 0x00001EB8
		internal virtual Label Label5
		{
			get
			{
				return this._Label5;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label5 = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00003AC4 File Offset: 0x00001EC4
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00003AD8 File Offset: 0x00001ED8
		internal virtual TextBox TextBox6
		{
			get
			{
				return this._TextBox6;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox6 = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003AE4 File Offset: 0x00001EE4
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00003AF8 File Offset: 0x00001EF8
		internal virtual TextBox TextBox7
		{
			get
			{
				return this._TextBox7;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox7 = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00003B04 File Offset: 0x00001F04
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00003B18 File Offset: 0x00001F18
		internal virtual TextBox TextBox8
		{
			get
			{
				return this._TextBox8;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox8 = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003B24 File Offset: 0x00001F24
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00003B38 File Offset: 0x00001F38
		internal virtual Label Label6
		{
			get
			{
				return this._Label6;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label6 = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003B44 File Offset: 0x00001F44
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00003B58 File Offset: 0x00001F58
		internal virtual CheckBox CheckBox1
		{
			get
			{
				return this._CheckBox1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._CheckBox1 = value;
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003B64 File Offset: 0x00001F64
		private void Button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003B6C File Offset: 0x00001F6C
		private void Button1_Click(object sender, EventArgs e)
		{
			if (!this.CheckBox1.Checked)
			{
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 332))] = 56;
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 331))] = 9;
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 330))] = 8;
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 315))] = 80;
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 314))] = 9;
			}
			MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 171))] = 0;
			MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 170))] = 0;
			checked
			{
				try
				{
					string working = this.TextBox1.Text;
					working = Conversions.ToString(Conversion.Val(working));
					int @int = Conversions.ToInteger(Conversion.Int(working));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 169))] = (byte)Math.Round(Conversion.Val("&H" + @int.ToString("X4").Substring(0, 2)));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 168))] = (byte)Math.Round(Conversion.Val("&H" + @int.ToString("X4").Substring(2, 2)));
				}
				catch (Exception ex)
				{
				}
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 175))] = 0;
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 174))] = 0;
				try
				{
					string working2 = this.TextBox2.Text;
					working2 = Conversions.ToString(Conversion.Val(working2));
					int int2 = Conversions.ToInteger(Conversion.Int(working2));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 173))] = (byte)Math.Round(Conversion.Val("&H" + int2.ToString("X4").Substring(0, 2)));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 172))] = (byte)Math.Round(Conversion.Val("&H" + int2.ToString("X4").Substring(2, 2)));
				}
				catch (Exception ex2)
				{
				}
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 155))] = 0;
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 154))] = 0;
				try
				{
					string working3 = this.TextBox3.Text;
					working3 = Conversions.ToString(Conversion.Val(working3));
					int int3 = Conversions.ToInteger(Conversion.Int(working3));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 153))] = (byte)Math.Round(Conversion.Val("&H" + int3.ToString("X4").Substring(0, 2)));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 152))] = (byte)Math.Round(Conversion.Val("&H" + int3.ToString("X4").Substring(2, 2)));
				}
				catch (Exception ex3)
				{
				}
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 179))] = 0;
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 178))] = 0;
				try
				{
					string working4 = this.TextBox4.Text;
					working4 = Conversions.ToString(Conversion.Val(working4));
					int int4 = Conversions.ToInteger(Conversion.Int(working4));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 177))] = (byte)Math.Round(Conversion.Val("&H" + int4.ToString("X4").Substring(0, 2)));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 176))] = (byte)Math.Round(Conversion.Val("&H" + int4.ToString("X4").Substring(2, 2)));
				}
				catch (Exception ex4)
				{
				}
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 131))] = 0;
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 130))] = 0;
				try
				{
					string working5 = this.TextBox5.Text;
					working5 = Conversions.ToString(Conversion.Val(working5));
					int int5 = Conversions.ToInteger(Conversion.Int(working5));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 129))] = (byte)Math.Round(Conversion.Val("&H" + int5.ToString("X4").Substring(0, 2)));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 128))] = (byte)Math.Round(Conversion.Val("&H" + int5.ToString("X4").Substring(2, 2)));
				}
				catch (Exception ex5)
				{
				}
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 127))] = 0;
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 126))] = 0;
				try
				{
					string working6 = this.TextBox6.Text;
					working6 = Conversions.ToString(Conversion.Val(working6));
					int int6 = Conversions.ToInteger(Conversion.Int(working6));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 125))] = (byte)Math.Round(Conversion.Val("&H" + int6.ToString("X4").Substring(0, 2)));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 124))] = (byte)Math.Round(Conversion.Val("&H" + int6.ToString("X4").Substring(2, 2)));
				}
				catch (Exception ex6)
				{
				}
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 123))] = 0;
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 122))] = 0;
				try
				{
					string working7 = this.TextBox7.Text;
					working7 = Conversions.ToString(Conversion.Val(working7));
					int int7 = Conversions.ToInteger(Conversion.Int(working7));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 121))] = (byte)Math.Round(Conversion.Val("&H" + int7.ToString("X4").Substring(0, 2)));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 120))] = (byte)Math.Round(Conversion.Val("&H" + int7.ToString("X4").Substring(2, 2)));
				}
				catch (Exception ex7)
				{
				}
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 119))] = 0;
				MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 118))] = 0;
				try
				{
					string working8 = this.TextBox8.Text;
					working8 = Conversions.ToString(Conversion.Val(working8));
					int int8 = Conversions.ToInteger(Conversion.Int(working8));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 117))] = (byte)Math.Round(Conversion.Val("&H" + int8.ToString("X4").Substring(0, 2)));
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 116))] = (byte)Math.Round(Conversion.Val("&H" + int8.ToString("X4").Substring(2, 2)));
				}
				catch (Exception ex8)
				{
				}
				MyProject.Forms.Form1.TurboModifierTextBox.Text = string.Concat(new string[]
				{
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 171))].ToString("X2"),
					" ",
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 170))].ToString("X2"),
					" ",
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 169))].ToString("X2"),
					" ",
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 168))].ToString("X2")
				});
				MyProject.Forms.Form1.ExhauseMultiplierTextBox.Text = string.Concat(new string[]
				{
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 155))].ToString("X2"),
					" ",
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 154))].ToString("X2"),
					" ",
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 153))].ToString("X2"),
					" ",
					MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 152))].ToString("X2")
				});
				this.Close();
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004A34 File Offset: 0x00002E34
		private void Button3_Click(object sender, EventArgs e)
		{
			TextBox textBox = this.TextBox1;
			string text = "&H";
			byte b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 169))];
			string text2 = b.ToString("X2");
			string text3 = " ";
			byte b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 168))];
			textBox.Text = Conversions.ToString(Conversion.Val(text + text2 + text3 + b2.ToString("X2")));
			TextBox textBox2 = this.TextBox2;
			string text4 = "&H";
			b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 173))];
			string text5 = b2.ToString("X2");
			string text6 = " ";
			b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 172))];
			textBox2.Text = Conversions.ToString(Conversion.Val(text4 + text5 + text6 + b.ToString("X2")));
			TextBox textBox3 = this.TextBox3;
			string text7 = "&H";
			b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 153))];
			string text8 = b2.ToString("X2");
			string text9 = " ";
			b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 152))];
			textBox3.Text = Conversions.ToString(Conversion.Val(text7 + text8 + text9 + b.ToString("X2")));
			TextBox textBox4 = this.TextBox4;
			string text10 = "&H";
			b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 177))];
			string text11 = b2.ToString("X2");
			string text12 = " ";
			b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 176))];
			textBox4.Text = Conversions.ToString(Conversion.Val(text10 + text11 + text12 + b.ToString("X2")));
			TextBox textBox5 = this.TextBox5;
			string text13 = "&H";
			b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 129))];
			string text14 = b2.ToString("X2");
			string text15 = " ";
			b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 128))];
			textBox5.Text = Conversions.ToString(Conversion.Val(text13 + text14 + text15 + b.ToString("X2")));
			TextBox textBox6 = this.TextBox6;
			string text16 = "&H";
			b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 125))];
			string text17 = b2.ToString("X2");
			string text18 = " ";
			b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 124))];
			textBox6.Text = Conversions.ToString(Conversion.Val(text16 + text17 + text18 + b.ToString("X2")));
			TextBox textBox7 = this.TextBox7;
			string text19 = "&H";
			b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 121))];
			string text20 = b2.ToString("X2");
			string text21 = " ";
			b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 120))];
			textBox7.Text = Conversions.ToString(Conversion.Val(text19 + text20 + text21 + b.ToString("X2")));
			TextBox textBox8 = this.TextBox8;
			string text22 = "&H";
			b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 117))];
			string text23 = b2.ToString("X2");
			string text24 = " ";
			b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 116))];
			textBox8.Text = Conversions.ToString(Conversion.Val(text22 + text23 + text24 + b.ToString("X2")));
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004F50 File Offset: 0x00003350
		private void Customperformance_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x0400000C RID: 12
		[AccessedThroughProperty("Label1")]
		private Label _Label1;

		// Token: 0x0400000D RID: 13
		[AccessedThroughProperty("Label2")]
		private Label _Label2;

		// Token: 0x0400000E RID: 14
		[AccessedThroughProperty("Label3")]
		private Label _Label3;

		// Token: 0x0400000F RID: 15
		[AccessedThroughProperty("TextBox1")]
		private TextBox _TextBox1;

		// Token: 0x04000010 RID: 16
		[AccessedThroughProperty("TextBox2")]
		private TextBox _TextBox2;

		// Token: 0x04000011 RID: 17
		[AccessedThroughProperty("TextBox3")]
		private TextBox _TextBox3;

		// Token: 0x04000012 RID: 18
		[AccessedThroughProperty("TextBox4")]
		private TextBox _TextBox4;

		// Token: 0x04000013 RID: 19
		[AccessedThroughProperty("TextBox5")]
		private TextBox _TextBox5;

		// Token: 0x04000014 RID: 20
		[AccessedThroughProperty("Label4")]
		private Label _Label4;

		// Token: 0x04000015 RID: 21
		[AccessedThroughProperty("Button1")]
		private Button _Button1;

		// Token: 0x04000016 RID: 22
		[AccessedThroughProperty("Button2")]
		private Button _Button2;

		// Token: 0x04000017 RID: 23
		[AccessedThroughProperty("Button3")]
		private Button _Button3;

		// Token: 0x04000018 RID: 24
		[AccessedThroughProperty("Label5")]
		private Label _Label5;

		// Token: 0x04000019 RID: 25
		[AccessedThroughProperty("TextBox6")]
		private TextBox _TextBox6;

		// Token: 0x0400001A RID: 26
		[AccessedThroughProperty("TextBox7")]
		private TextBox _TextBox7;

		// Token: 0x0400001B RID: 27
		[AccessedThroughProperty("TextBox8")]
		private TextBox _TextBox8;

		// Token: 0x0400001C RID: 28
		[AccessedThroughProperty("Label6")]
		private Label _Label6;

		// Token: 0x0400001D RID: 29
		[AccessedThroughProperty("CheckBox1")]
		private CheckBox _CheckBox1;
	}
}
