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

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

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

            // Turbo
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 171] = 0;
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 170] = 0;
            if (ushort.TryParse(TurboTextBox.Text, out var number))
            {
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 169] = (byte)(number >> 8);
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 168] = (byte)(number & 0xFF);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the Turbo");
            }

            // Engine
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 175] = 0;
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 174] = 0;
            if (ushort.TryParse(EngineTextBox.Text, out var number2))
            {
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 173] = (byte)(number2 >> 8);
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 172] = (byte)(number2 & 0xFF);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the Engine");
            }

            // Exhaust
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 155] = 0;
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 154] = 0;
            if (ushort.TryParse(ExhaustTextBox.Text, out var number3))
            {
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 153] = (byte)(number3 >> 8);
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 152] = (byte)(number3 & 0xFF);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the Exhaust Multiplier");
            }

            // Transmission
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 179] = 0;
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 178] = 0;
            if (ushort.TryParse(TextBox4.Text, out var number4))
            {
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 177] = (byte)(number4 >> 8);
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 176] = (byte)(number4 & 0xFF);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the ???Transmission???"); // TODO: Fix these
            }

            // Suspension
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 131] = 0;
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 130] = 0;
            if (ushort.TryParse(TextBox5.Text, out var number5))
            {
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 129] = (byte)(number5 >> 8);
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 128] = (byte)(number5 & 0xFF);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the ???Suspension???");
            }

            // Drivetrain
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 127] = 0;
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 126] = 0;
            if (ushort.TryParse(TextBox6.Text, out var number6))
            {
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 125] = (byte)(number6 >> 8);
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 124] = (byte)(number6 & 0xFF);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the ???Drivetrain???");
            }

            // Brakes
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 123] = 0;
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 122] = 0;
            if (ushort.TryParse(TextBox7.Text, out var number7))
            {
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 121] = (byte)(number7 >> 8);
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 120] = (byte)(number7 & 0xFF);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the ???Brakes???");
            }

            // Chassis
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 119] = 0;
            MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 118] = 0;
            if (ushort.TryParse(TextBox8.Text, out var number8))
            {
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 117] = (byte)(number8 >> 8);
                MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 116] = (byte)(number8 & 0xFF);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the ???Chassis???");
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

        ushort GetCombinedValue(int offsetHigh, int offsetLow)
        {
            var form = MyProject.Forms.Form1;
            // Shift the high byte left by 8 bits and then OR with the low byte.
            return (ushort)((form.Gt5Save[form.Moff - offsetHigh] << 8) |
                            form.Gt5Save[form.Moff - offsetLow]);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            TurboTextBox.Text = GetCombinedValue(169, 168).ToString();
            EngineTextBox.Text = GetCombinedValue(173, 172).ToString();
            ExhaustTextBox.Text = GetCombinedValue(153, 152).ToString();
            TextBox4.Text = GetCombinedValue(177, 176).ToString();
            TextBox5.Text = GetCombinedValue(129, 128).ToString();
            TextBox6.Text = GetCombinedValue(125, 124).ToString();
            TextBox7.Text = GetCombinedValue(121, 120).ToString();
            TextBox8.Text = GetCombinedValue(117, 116).ToString();
        }

        private void Customperformance_Load(object sender, EventArgs e)
        {
        }
    }
}