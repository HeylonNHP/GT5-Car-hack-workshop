using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GT5_Car_hack_workshop_2.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace GT5_Car_hack_workshop_2
{
    [DesignerGenerated]
    public partial class TransmissionEditor : Form
    {
        [AccessedThroughProperty("Button1")] private Button _Button1;

        [AccessedThroughProperty("Timer1")] private Timer _Timer1;

        private decimal lastgearpre;

        private decimal origfg;

        public TransmissionEditor()
        {
            Load += Tranny_Load;
            lastgearpre = 0.1m;
            origfg = 0.1m;
            InitializeComponent();
        }

        [field: AccessedThroughProperty("Label1")]
        internal virtual Label Label1
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label2")]
        internal virtual Label Label2
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label3")]
        internal virtual Label Label3
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label4")]
        internal virtual Label Label4
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label5")]
        internal virtual Label Label5
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label6")]
        internal virtual Label Label6
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label7")]
        internal virtual Label Label7
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label8")]
        internal virtual Label Label8
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label9")]
        internal virtual Label Label9
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label10")]
        internal virtual Label Label10
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label11")]
        internal virtual Label Label11
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox1")]
        internal virtual TextBox TextBox1
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox2")]
        internal virtual TextBox TextBox2
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox3")]
        internal virtual TextBox TextBox3
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox4")]
        internal virtual TextBox TextBox4
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox5")]
        internal virtual TextBox TextBox5
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox6")]
        internal virtual TextBox TextBox6
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox7")]
        internal virtual TextBox TextBox7
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox8")]
        internal virtual TextBox TextBox8
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox9")]
        internal virtual TextBox TextBox9
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox10")]
        internal virtual TextBox TextBox10
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox11")]
        internal virtual TextBox TextBox11
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox12")]
        internal virtual TextBox TextBox12
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label12")]
        internal virtual Label Label12
        {
            get;
            set;
        }

        internal virtual Button Button1
        {
            get => _Button1;
            set
            {
                EventHandler eventHandler = Button1_Click;
                if (_Button1 != null) _Button1.Click -= eventHandler;

                _Button1 = value;
                if (_Button1 != null) _Button1.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("GroupBox1")]
        internal virtual GroupBox GroupBox1
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox14")]
        internal virtual TextBox TextBox14
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label14")]
        internal virtual Label Label14
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("TextBox13")]
        internal virtual TextBox TextBox13
        {
            get;
            set;
        }

        [field: AccessedThroughProperty("Label13")]
        internal virtual Label Label13
        {
            get;
            set;
        }

        internal virtual Timer Timer1
        {
            get => _Timer1;
            set
            {
                EventHandler eventHandler = Timer1_Tick;
                if (_Timer1 != null) _Timer1.Tick -= eventHandler;

                _Timer1 = value;
                if (_Timer1 != null) _Timer1.Tick += eventHandler;
            }
        }

        private void Tranny_Load(object sender, EventArgs e)
        {
            var textBox = TextBox1;
            var text = "&H";
            var b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 73))];
            var text2 = b.ToString("X2");
            var text3 = " ";
            var b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 72))];
            textBox.Text = Conversions.ToString(Conversion.Val(text + text2 + text3 + b2.ToString("X2")) / 1000.0);
            var textBox2 = TextBox2;
            var text4 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 71))];
            var text5 = b2.ToString("X2");
            var text6 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 70))];
            textBox2.Text = Conversions.ToString(Conversion.Val(text4 + text5 + text6 + b.ToString("X2")) / 1000.0);
            var textBox3 = TextBox3;
            var text7 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 69))];
            var text8 = b2.ToString("X2");
            var text9 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 68))];
            textBox3.Text = Conversions.ToString(Conversion.Val(text7 + text8 + text9 + b.ToString("X2")) / 1000.0);
            var textBox4 = TextBox4;
            var text10 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 67))];
            var text11 = b2.ToString("X2");
            var text12 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 66))];
            textBox4.Text = Conversions.ToString(Conversion.Val(text10 + text11 + text12 + b.ToString("X2")) / 1000.0);
            var textBox5 = TextBox5;
            var text13 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 65))];
            var text14 = b2.ToString("X2");
            var text15 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 64))];
            textBox5.Text = Conversions.ToString(Conversion.Val(text13 + text14 + text15 + b.ToString("X2")) / 1000.0);
            var textBox6 = TextBox6;
            var text16 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 63))];
            var text17 = b2.ToString("X2");
            var text18 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 62))];
            textBox6.Text = Conversions.ToString(Conversion.Val(text16 + text17 + text18 + b.ToString("X2")) / 1000.0);
            var textBox7 = TextBox7;
            var text19 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 61))];
            var text20 = b2.ToString("X2");
            var text21 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 60))];
            textBox7.Text = Conversions.ToString(Conversion.Val(text19 + text20 + text21 + b.ToString("X2")) / 1000.0);
            var textBox8 = TextBox8;
            var text22 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 59))];
            var text23 = b2.ToString("X2");
            var text24 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 58))];
            textBox8.Text = Conversions.ToString(Conversion.Val(text22 + text23 + text24 + b.ToString("X2")) / 1000.0);
            var textBox9 = TextBox9;
            var text25 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 57))];
            var text26 = b2.ToString("X2");
            var text27 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 56))];
            textBox9.Text = Conversions.ToString(Conversion.Val(text25 + text26 + text27 + b.ToString("X2")) / 1000.0);
            var textBox10 = TextBox10;
            var text28 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 55))];
            var text29 = b2.ToString("X2");
            var text30 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 54))];
            textBox10.Text = Conversions.ToString(Conversion.Val(text28 + text29 + text30 + b.ToString("X2")) / 1000.0);
            var textBox11 = TextBox11;
            var text31 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 53))];
            var text32 = b2.ToString("X2");
            var text33 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 52))];
            textBox11.Text = Conversions.ToString(Conversion.Val(text31 + text32 + text33 + b.ToString("X2")) / 1000.0);
            var textBox12 = TextBox12;
            var text34 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 51))];
            var text35 = b2.ToString("X2");
            var text36 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 50))];
            textBox12.Text = Conversions.ToString(Conversion.Val(text34 + text35 + text36 + b.ToString("X2")) / 1000.0);
            try
            {
                if (Conversion.Val(TextBox11.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox11.Text));
                else if (Conversion.Val(TextBox10.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox10.Text));
                else if (Conversion.Val(TextBox9.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox9.Text));
                else if (Conversion.Val(TextBox8.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox8.Text));
                else if (Conversion.Val(TextBox7.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox7.Text));
                else if (Conversion.Val(TextBox6.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox6.Text));
                else if (Conversion.Val(TextBox5.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox5.Text));
                else if (Conversion.Val(TextBox4.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox4.Text));
                else if (Conversion.Val(TextBox3.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox3.Text));
                else if (Conversion.Val(TextBox2.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox2.Text));
                else if (Conversion.Val(TextBox1.Text) > 0.0) lastgearpre = new decimal(Conversion.Val(TextBox1.Text));
            }
            catch (Exception ex)
            {
            }

            origfg = new decimal(Conversion.Val(TextBox12.Text));
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var working = TextBox1.Text;
                working = Conversions.ToString(Conversion.Val(working) * 1000.0);
                var @int = Conversions.ToInteger(Conversion.Int(working));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 73))] =
                        (byte)Math.Round(Conversion.Val("&H" + @int.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 72))] =
                        (byte)Math.Round(Conversion.Val("&H" + @int.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex)
            {
            }

            try
            {
                var working2 = TextBox2.Text;
                working2 = Conversions.ToString(Conversion.Val(working2) * 1000.0);
                var int2 = Conversions.ToInteger(Conversion.Int(working2));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 71))] =
                        (byte)Math.Round(Conversion.Val("&H" + int2.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 70))] =
                        (byte)Math.Round(Conversion.Val("&H" + int2.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex2)
            {
            }

            try
            {
                var working3 = TextBox3.Text;
                working3 = Conversions.ToString(Conversion.Val(working3) * 1000.0);
                var int3 = Conversions.ToInteger(Conversion.Int(working3));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 69))] =
                        (byte)Math.Round(Conversion.Val("&H" + int3.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 68))] =
                        (byte)Math.Round(Conversion.Val("&H" + int3.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex3)
            {
            }

            try
            {
                var working4 = TextBox4.Text;
                working4 = Conversions.ToString(Conversion.Val(working4) * 1000.0);
                var int4 = Conversions.ToInteger(Conversion.Int(working4));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 67))] =
                        (byte)Math.Round(Conversion.Val("&H" + int4.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 66))] =
                        (byte)Math.Round(Conversion.Val("&H" + int4.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex4)
            {
            }

            try
            {
                var working5 = TextBox5.Text;
                working5 = Conversions.ToString(Conversion.Val(working5) * 1000.0);
                var int5 = Conversions.ToInteger(Conversion.Int(working5));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 65))] =
                        (byte)Math.Round(Conversion.Val("&H" + int5.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 64))] =
                        (byte)Math.Round(Conversion.Val("&H" + int5.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex5)
            {
            }

            try
            {
                var working6 = TextBox6.Text;
                working6 = Conversions.ToString(Conversion.Val(working6) * 1000.0);
                var int6 = Conversions.ToInteger(Conversion.Int(working6));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 63))] =
                        (byte)Math.Round(Conversion.Val("&H" + int6.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 62))] =
                        (byte)Math.Round(Conversion.Val("&H" + int6.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex6)
            {
            }

            try
            {
                var working7 = TextBox7.Text;
                working7 = Conversions.ToString(Conversion.Val(working7) * 1000.0);
                var int7 = Conversions.ToInteger(Conversion.Int(working7));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 61))] =
                        (byte)Math.Round(Conversion.Val("&H" + int7.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 60))] =
                        (byte)Math.Round(Conversion.Val("&H" + int7.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex7)
            {
            }

            try
            {
                var working8 = TextBox8.Text;
                working8 = Conversions.ToString(Conversion.Val(working8) * 1000.0);
                var int8 = Conversions.ToInteger(Conversion.Int(working8));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 59))] =
                        (byte)Math.Round(Conversion.Val("&H" + int8.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 58))] =
                        (byte)Math.Round(Conversion.Val("&H" + int8.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex8)
            {
            }

            try
            {
                var working9 = TextBox9.Text;
                working9 = Conversions.ToString(Conversion.Val(working9) * 1000.0);
                var int9 = Conversions.ToInteger(Conversion.Int(working9));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 57))] =
                        (byte)Math.Round(Conversion.Val("&H" + int9.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 56))] =
                        (byte)Math.Round(Conversion.Val("&H" + int9.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex9)
            {
            }

            try
            {
                var working10 = TextBox10.Text;
                working10 = Conversions.ToString(Conversion.Val(working10) * 1000.0);
                var int10 = Conversions.ToInteger(Conversion.Int(working10));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 55))] =
                        (byte)Math.Round(Conversion.Val("&H" + int10.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 54))] =
                        (byte)Math.Round(Conversion.Val("&H" + int10.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex10)
            {
            }

            try
            {
                var working11 = TextBox11.Text;
                working11 = Conversions.ToString(Conversion.Val(working11) * 1000.0);
                var int11 = Conversions.ToInteger(Conversion.Int(working11));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 53))] =
                        (byte)Math.Round(Conversion.Val("&H" + int11.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 52))] =
                        (byte)Math.Round(Conversion.Val("&H" + int11.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex11)
            {
            }

            try
            {
                var working12 = TextBox12.Text;
                working12 = Conversions.ToString(Conversion.Val(working12) * 1000.0);
                var int12 = Conversions.ToInteger(Conversion.Int(working12));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 51))] =
                        (byte)Math.Round(Conversion.Val("&H" + int12.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 50))] =
                        (byte)Math.Round(Conversion.Val("&H" + int12.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex12)
            {
            }

            Close();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox13.Text)) TextBox13.Text = "0";

            var omax = Conversion.Val(TextBox13.Text);
            if (omax != 0.0)
            {
                var lastgear = 0.1m;

                // Group the TextBoxes in an array for iteration
                TextBox[] textBoxes =
                {
                    TextBox11, TextBox10, TextBox9, TextBox8, TextBox7,
                    TextBox6, TextBox5, TextBox4, TextBox3, TextBox2, TextBox1
                };

                // Find the first TextBox with a value > 0
                foreach (var textBox in textBoxes)
                    if (Conversion.Val(textBox.Text) > 0.0)
                    {
                        lastgear = new decimal(Conversion.Val(textBox.Text));
                        break;
                    }

                TextBox14.Text = Conversions.ToString(
                    Convert.ToDouble(origfg) /
                    Conversion.Val(TextBox12.Text) *
                    Conversion.Val(TextBox13.Text) *
                    Convert.ToDouble(decimal.Multiply(
                        decimal.Divide(1m, lastgear), lastgearpre
                    ))
                );
            }
        }
    }
}