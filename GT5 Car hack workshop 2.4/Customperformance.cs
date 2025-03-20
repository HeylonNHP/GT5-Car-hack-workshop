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

        // Token: 0x0600004A RID: 74 RVA: 0x00004A34 File Offset: 0x00002E34
        private void Button3_Click(object sender, EventArgs e)
        {
            var textBox = TextBox1;
            var text = "&H";
            var b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 169))];
            var text2 = b.ToString("X2");
            var text3 = " ";
            var b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 168))];
            textBox.Text = Conversions.ToString(Conversion.Val(text + text2 + text3 + b2.ToString("X2")));
            var textBox2 = TextBox2;
            var text4 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 173))];
            var text5 = b2.ToString("X2");
            var text6 = " ";
            b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 172))];
            textBox2.Text = Conversions.ToString(Conversion.Val(text4 + text5 + text6 + b.ToString("X2")));
            var textBox3 = TextBox3;
            var text7 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 153))];
            var text8 = b2.ToString("X2");
            var text9 = " ";
            b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 152))];
            textBox3.Text = Conversions.ToString(Conversion.Val(text7 + text8 + text9 + b.ToString("X2")));
            var textBox4 = TextBox4;
            var text10 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 177))];
            var text11 = b2.ToString("X2");
            var text12 = " ";
            b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 176))];
            textBox4.Text = Conversions.ToString(Conversion.Val(text10 + text11 + text12 + b.ToString("X2")));
            var textBox5 = TextBox5;
            var text13 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 129))];
            var text14 = b2.ToString("X2");
            var text15 = " ";
            b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 128))];
            textBox5.Text = Conversions.ToString(Conversion.Val(text13 + text14 + text15 + b.ToString("X2")));
            var textBox6 = TextBox6;
            var text16 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 125))];
            var text17 = b2.ToString("X2");
            var text18 = " ";
            b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 124))];
            textBox6.Text = Conversions.ToString(Conversion.Val(text16 + text17 + text18 + b.ToString("X2")));
            var textBox7 = TextBox7;
            var text19 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 121))];
            var text20 = b2.ToString("X2");
            var text21 = " ";
            b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 120))];
            textBox7.Text = Conversions.ToString(Conversion.Val(text19 + text20 + text21 + b.ToString("X2")));
            var textBox8 = TextBox8;
            var text22 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 117))];
            var text23 = b2.ToString("X2");
            var text24 = " ";
            b = MyProject.Forms.Form1.Gt5Save[Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 116))];
            textBox8.Text = Conversions.ToString(Conversion.Val(text22 + text23 + text24 + b.ToString("X2")));
        }

        // Token: 0x0600004B RID: 75 RVA: 0x00004F50 File Offset: 0x00003350
        private void Customperformance_Load(object sender, EventArgs e)
        {
        }
    }
}