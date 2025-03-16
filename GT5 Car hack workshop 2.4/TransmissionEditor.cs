using System;
using System.Globalization;
using System.Windows.Forms;
using GT5_Car_hack_workshop_2.My;

namespace GT5_Car_hack_workshop_2
{
    public partial class TransmissionEditor : Form
    {
        private readonly Timer _Timer1;

        private decimal _Lastgearpre;

        private decimal _OriginalFinalDrive;

        public TransmissionEditor()
        {
            Load += TransmissionLoadValues;
            _Lastgearpre = 0.1m;
            _OriginalFinalDrive = 0.1m;
            InitializeComponent();

            _Timer1 = new Timer();
            _Timer1.Tick += Timer1_Tick;
            _Timer1.Interval = 100;
            _Timer1.Enabled = true;
            _Timer1.Start();

            FormClosing += (s, e) =>
            {
                _Timer1.Stop();
                _Timer1.Dispose();
            };
        }

        private string LoadGearRatioToGt5Save(int moffOffset1, int moffOffset2)
        {
            var firstByte = MyProject.Forms.Form1.Gt5Save[moffOffset1];
            var secondByte = MyProject.Forms.Form1.Gt5Save[moffOffset2];
            return (((firstByte << 8) | secondByte) / 1000.0f).ToString(CultureInfo.CurrentCulture);
        }

        private void TransmissionLoadValues(object sender, EventArgs e)
        {
            // Load gear ratios from save data into text boxes
            // Each gear ratio is stored as 2 bytes in hex format, divided by 1000 for display

            // Gear 1
            TextBox1.Text = LoadGearRatioToGt5Save(MyProject.Forms.Form1.Moff - 73, MyProject.Forms.Form1.Moff - 72);

            // Gear 2
            TextBox2.Text = LoadGearRatioToGt5Save(MyProject.Forms.Form1.Moff - 71, MyProject.Forms.Form1.Moff - 70);

            // Gear 3
            TextBox3.Text = LoadGearRatioToGt5Save(MyProject.Forms.Form1.Moff - 69, MyProject.Forms.Form1.Moff - 68);

            // Gear 4
            TextBox4.Text = LoadGearRatioToGt5Save(MyProject.Forms.Form1.Moff - 67, MyProject.Forms.Form1.Moff - 66);

            // Gear 5
            TextBox5.Text = LoadGearRatioToGt5Save(MyProject.Forms.Form1.Moff - 65, MyProject.Forms.Form1.Moff - 64);

            // Gear 6
            TextBox6.Text = LoadGearRatioToGt5Save(MyProject.Forms.Form1.Moff - 63, MyProject.Forms.Form1.Moff - 62);

            // Gear 7
            TextBox7.Text = LoadGearRatioToGt5Save(MyProject.Forms.Form1.Moff - 61, MyProject.Forms.Form1.Moff - 60);

            // Gear 8
            TextBox8.Text = LoadGearRatioToGt5Save(MyProject.Forms.Form1.Moff - 59, MyProject.Forms.Form1.Moff - 58);

            // Gear 9
            TextBox9.Text = LoadGearRatioToGt5Save(MyProject.Forms.Form1.Moff - 57, MyProject.Forms.Form1.Moff - 56);

            // Gear 10
            TextBox10.Text = LoadGearRatioToGt5Save(MyProject.Forms.Form1.Moff - 55, MyProject.Forms.Form1.Moff - 54);

            // Gear 11
            TextBox11.Text = LoadGearRatioToGt5Save(MyProject.Forms.Form1.Moff - 53, MyProject.Forms.Form1.Moff - 52);

            // Final Drive
            TextBox12.Text = LoadGearRatioToGt5Save(MyProject.Forms.Form1.Moff - 51, MyProject.Forms.Form1.Moff - 50);

            // Cache the gear ratio of the highest gear
            var textBoxes = new[] { TextBox11, TextBox10, TextBox9, TextBox8, TextBox7, TextBox6, TextBox5, TextBox4, TextBox3, TextBox2, TextBox1 };
            foreach (var textBox in textBoxes)
            {
                if (!decimal.TryParse(textBox.Text, out var parsedHighestGearRatio))
                {
                    continue;
                }

                if (parsedHighestGearRatio > 0.0m)
                {
                    _Lastgearpre = parsedHighestGearRatio;
                    break;
                }
            }

            // Cache the final drive ratio
            if (decimal.TryParse(TextBox12.Text, out var parsedFinalDriveRatio))
            {
                _OriginalFinalDrive = parsedFinalDriveRatio;
            }
        }

        private void SaveGearRatioToGt5Save(string textBoxValue, int moffOffset1, int moffOffset2)
        {
            if (!decimal.TryParse(textBoxValue, out var parsedRatio))
            {
                return;
            }

            var intValue = (int)(parsedRatio * 1000);

            MyProject.Forms.Form1.Gt5Save[moffOffset1] =
                byte.Parse(intValue.ToString("X4").Substring(0, 2), NumberStyles.HexNumber);
            MyProject.Forms.Form1.Gt5Save[moffOffset2] =
                byte.Parse(intValue.ToString("X4").Substring(2, 2), NumberStyles.HexNumber);
        }


        private void OkayButtonClick(object sender, EventArgs e)
        {
            // Gear 1
            SaveGearRatioToGt5Save(TextBox1.Text, MyProject.Forms.Form1.Moff - 73, MyProject.Forms.Form1.Moff - 72);

            // Gear 2
            SaveGearRatioToGt5Save(TextBox2.Text, MyProject.Forms.Form1.Moff - 71, MyProject.Forms.Form1.Moff - 70);

            // Gear 3
            SaveGearRatioToGt5Save(TextBox3.Text, MyProject.Forms.Form1.Moff - 69, MyProject.Forms.Form1.Moff - 68);

            // Gear 4
            SaveGearRatioToGt5Save(TextBox4.Text, MyProject.Forms.Form1.Moff - 67, MyProject.Forms.Form1.Moff - 66);

            // Gear 5
            SaveGearRatioToGt5Save(TextBox5.Text, MyProject.Forms.Form1.Moff - 65, MyProject.Forms.Form1.Moff - 64);

            // Gear 6
            SaveGearRatioToGt5Save(TextBox6.Text, MyProject.Forms.Form1.Moff - 63, MyProject.Forms.Form1.Moff - 62);

            // Gear 7
            SaveGearRatioToGt5Save(TextBox7.Text, MyProject.Forms.Form1.Moff - 61, MyProject.Forms.Form1.Moff - 60);

            // Gear 8
            SaveGearRatioToGt5Save(TextBox8.Text, MyProject.Forms.Form1.Moff - 59, MyProject.Forms.Form1.Moff - 58);

            // Gear 9
            SaveGearRatioToGt5Save(TextBox9.Text, MyProject.Forms.Form1.Moff - 57, MyProject.Forms.Form1.Moff - 56);

            // Gear 10
            SaveGearRatioToGt5Save(TextBox10.Text, MyProject.Forms.Form1.Moff - 55, MyProject.Forms.Form1.Moff - 54);

            // Gear 11
            SaveGearRatioToGt5Save(TextBox11.Text, MyProject.Forms.Form1.Moff - 53, MyProject.Forms.Form1.Moff - 52);

            // Final drive
            SaveGearRatioToGt5Save(TextBox12.Text, MyProject.Forms.Form1.Moff - 51, MyProject.Forms.Form1.Moff - 50);

            Close();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox13.Text)) TextBox13.Text = "0";

            var omax = float.Parse(TextBox13.Text);
            if (omax != 0f)
            {
                var lastGear = 0.1f;

                TextBox[] textBoxes =
                {
                    TextBox11, TextBox10, TextBox9, TextBox8, TextBox7,
                    TextBox6, TextBox5, TextBox4, TextBox3, TextBox2, TextBox1
                };

                // Find the first TextBox with a value > 0
                foreach (var textBox in textBoxes)
                {
                    if (float.Parse(textBox.Text) > 0f)
                    {
                        lastGear = float.Parse(textBox.Text);
                        break;
                    }
                }

                TextBox14.Text = Math.Round((float)_OriginalFinalDrive /
                                            float.Parse(TextBox12.Text) *
                                            float.Parse(TextBox13.Text) *
                                            (1f / lastGear * (float)_Lastgearpre), 1).ToString();
            }
        }
    }
}