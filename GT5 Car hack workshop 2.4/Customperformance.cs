using System;
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
        public Customperformance()
        {
            Load += Customperformance_Load;
            InitializeComponent();
        }

        // Token: 0x06000048 RID: 72 RVA: 0x00003B64 File Offset: 0x00001F64
        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Token: 0x06000049 RID: 73 RVA: 0x00003B6C File Offset: 0x00001F6C
        private void Button1_Click(object sender, EventArgs e)
        {
            if (!NoPartInstallCheckBox.Checked)
            {
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 332] = 56;
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 331] = 9;
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 330] = 8;
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 315] = 80;
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 314] = 9;
            }

            MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 171))] = 0;
            MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 170))] = 0;
            checked
            {
                try
                {
                    var working = TextBox1.Text;
                    working = Conversions.ToString(Conversion.Val(working));
                    var @int = Conversions.ToInteger(Conversion.Int(working));
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
                    var working2 = TextBox2.Text;
                    working2 = Conversions.ToString(Conversion.Val(working2));
                    var int2 = Conversions.ToInteger(Conversion.Int(working2));
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
                    var working3 = TextBox3.Text;
                    working3 = Conversions.ToString(Conversion.Val(working3));
                    var int3 = Conversions.ToInteger(Conversion.Int(working3));
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
                    var working4 = TextBox4.Text;
                    working4 = Conversions.ToString(Conversion.Val(working4));
                    var int4 = Conversions.ToInteger(Conversion.Int(working4));
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
                    var working5 = TextBox5.Text;
                    working5 = Conversions.ToString(Conversion.Val(working5));
                    var int5 = Conversions.ToInteger(Conversion.Int(working5));
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
                    var working6 = TextBox6.Text;
                    working6 = Conversions.ToString(Conversion.Val(working6));
                    var int6 = Conversions.ToInteger(Conversion.Int(working6));
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
                    var working7 = TextBox7.Text;
                    working7 = Conversions.ToString(Conversion.Val(working7));
                    var int7 = Conversions.ToInteger(Conversion.Int(working7));
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
                    var working8 = TextBox8.Text;
                    working8 = Conversions.ToString(Conversion.Val(working8));
                    var int8 = Conversions.ToInteger(Conversion.Int(working8));
                    MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 117))] = (byte)Math.Round(Conversion.Val("&H" + int8.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 116))] = (byte)Math.Round(Conversion.Val("&H" + int8.ToString("X4").Substring(2, 2)));
                }
                catch (Exception ex8)
                {
                }

                MyProject.Forms.Form1.TurboModifierTextBox.Text = string.Concat(MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 171))].ToString("X2"), " ",
                    MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 170))].ToString("X2"), " ",
                    MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 169))].ToString("X2"), " ",
                    MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 168))].ToString("X2"));
                MyProject.Forms.Form1.ExhauseMultiplierTextBox.Text = string.Concat(MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 155))].ToString("X2"), " ",
                    MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 154))].ToString("X2"), " ",
                    MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 153))].ToString("X2"), " ",
                    MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 152))].ToString("X2"));
                Close();
            }
        }

        ushort GetCombinedValue(int offsetHigh, int offsetLow)
        {
            var form = MyProject.Forms.Form1;
            // Shift the high byte left by 8 bits and then OR with the low byte.
            return (ushort)((form.Gt5Save[form.Moff - offsetHigh] << 8) |
                            form.Gt5Save[form.Moff - offsetLow]);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            TextBox1.Text = GetCombinedValue(169, 168).ToString();
            TextBox2.Text = GetCombinedValue(173, 172).ToString();
            TextBox3.Text = GetCombinedValue(153, 152).ToString();
            TextBox4.Text = GetCombinedValue(177, 176).ToString();
            TextBox5.Text = GetCombinedValue(129, 128).ToString();
            TextBox6.Text = GetCombinedValue(125, 124).ToString();
            TextBox7.Text = GetCombinedValue(121, 120).ToString();
            TextBox8.Text = GetCombinedValue(117, 116).ToString();
        }

        // Token: 0x0600004B RID: 75 RVA: 0x00004F50 File Offset: 0x00003350
        private void Customperformance_Load(object sender, EventArgs e)
        {
        }
    }
}