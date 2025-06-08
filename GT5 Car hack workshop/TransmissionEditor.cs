using System;
using System.Globalization;
using System.Windows.Forms;
using GT5_Car_hack_workshop.Services;

namespace GT5_Car_hack_workshop
{
    public partial class TransmissionEditor : Form
    {
        private readonly Timer _timer1;
        private readonly IFormManager _formManager;
        private decimal _lastgearpre;
        private decimal _originalFinalDrive;

        public TransmissionEditor(IFormManager formManager)
        {
            _formManager = formManager;
            InitializeComponent();
            
            Load += TransmissionLoadValues;
            _lastgearpre = 0.1m;
            _originalFinalDrive = 0.1m;

            _timer1 = new Timer
            {
                Interval = 100,
                Enabled = true
            };
            _timer1.Tick += Timer1_Tick;
            _timer1.Start();

            FormClosing += (s, e) =>
            {
                _timer1.Stop();
                _timer1.Dispose();
            };
        }

        private string LoadGearRatioToGt5Save(int moffOffset1, int moffOffset2)
        {
            var firstByte = _formManager.MainForm.Gt5Save[moffOffset1];
            var secondByte = _formManager.MainForm.Gt5Save[moffOffset2];
            return (((firstByte << 8) | secondByte) / 1000.0f).ToString(CultureInfo.CurrentCulture);
        }

        private void TransmissionLoadValues(object sender, EventArgs e)
        {
            // Load gear ratios from save data into text boxes
            // Each gear ratio is stored as 2 bytes in hex format, divided by 1000 for display

            // Gear 1
            TextBox1.Text = LoadGearRatioToGt5Save(_formManager.MainForm.Moff - 73, _formManager.MainForm.Moff - 72);

            // Gear 2
            TextBox2.Text = LoadGearRatioToGt5Save(_formManager.MainForm.Moff - 71, _formManager.MainForm.Moff - 70);

            // Gear 3
            TextBox3.Text = LoadGearRatioToGt5Save(_formManager.MainForm.Moff - 69, _formManager.MainForm.Moff - 68);

            // Gear 4
            TextBox4.Text = LoadGearRatioToGt5Save(_formManager.MainForm.Moff - 67, _formManager.MainForm.Moff - 66);

            // Gear 5
            TextBox5.Text = LoadGearRatioToGt5Save(_formManager.MainForm.Moff - 65, _formManager.MainForm.Moff - 64);

            // Gear 6
            TextBox6.Text = LoadGearRatioToGt5Save(_formManager.MainForm.Moff - 63, _formManager.MainForm.Moff - 62);

            // Gear 7
            TextBox7.Text = LoadGearRatioToGt5Save(_formManager.MainForm.Moff - 61, _formManager.MainForm.Moff - 60);

            // Gear 8
            TextBox8.Text = LoadGearRatioToGt5Save(_formManager.MainForm.Moff - 59, _formManager.MainForm.Moff - 58);

            // Gear 9
            TextBox9.Text = LoadGearRatioToGt5Save(_formManager.MainForm.Moff - 57, _formManager.MainForm.Moff - 56);

            // Gear 10
            TextBox10.Text = LoadGearRatioToGt5Save(_formManager.MainForm.Moff - 55, _formManager.MainForm.Moff - 54);

            // Gear 11
            TextBox11.Text = LoadGearRatioToGt5Save(_formManager.MainForm.Moff - 53, _formManager.MainForm.Moff - 52);

            // Final Drive
            TextBox12.Text = LoadGearRatioToGt5Save(_formManager.MainForm.Moff - 51, _formManager.MainForm.Moff - 50);

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
                    _lastgearpre = parsedHighestGearRatio;
                    break;
                }
            }

            // Cache the final drive ratio
            if (decimal.TryParse(TextBox12.Text, out var parsedFinalDriveRatio))
            {
                _originalFinalDrive = parsedFinalDriveRatio;
            }
        }

        private void SaveGearRatioToGt5Save(string textBoxValue, int moffOffset1, int moffOffset2)
        {
            if (!decimal.TryParse(textBoxValue, out var parsedRatio))
            {
                return;
            }

            var intValue = (int)(parsedRatio * 1000);

            _formManager.MainForm.Gt5Save[moffOffset1] =
                byte.Parse(intValue.ToString("X4").Substring(0, 2), NumberStyles.HexNumber);
            _formManager.MainForm.Gt5Save[moffOffset2] =
                byte.Parse(intValue.ToString("X4").Substring(2, 2), NumberStyles.HexNumber);
        }

        private void OkayButtonClick(object sender, EventArgs e)
        {
            // Gear 1
            SaveGearRatioToGt5Save(TextBox1.Text, _formManager.MainForm.Moff - 73, _formManager.MainForm.Moff - 72);

            // Gear 2
            SaveGearRatioToGt5Save(TextBox2.Text, _formManager.MainForm.Moff - 71, _formManager.MainForm.Moff - 70);

            // Gear 3
            SaveGearRatioToGt5Save(TextBox3.Text, _formManager.MainForm.Moff - 69, _formManager.MainForm.Moff - 68);

            // Gear 4
            SaveGearRatioToGt5Save(TextBox4.Text, _formManager.MainForm.Moff - 67, _formManager.MainForm.Moff - 66);

            // Gear 5
            SaveGearRatioToGt5Save(TextBox5.Text, _formManager.MainForm.Moff - 65, _formManager.MainForm.Moff - 64);

            // Gear 6
            SaveGearRatioToGt5Save(TextBox6.Text, _formManager.MainForm.Moff - 63, _formManager.MainForm.Moff - 62);

            // Gear 7
            SaveGearRatioToGt5Save(TextBox7.Text, _formManager.MainForm.Moff - 61, _formManager.MainForm.Moff - 60);

            // Gear 8
            SaveGearRatioToGt5Save(TextBox8.Text, _formManager.MainForm.Moff - 59, _formManager.MainForm.Moff - 58);

            // Gear 9
            SaveGearRatioToGt5Save(TextBox9.Text, _formManager.MainForm.Moff - 57, _formManager.MainForm.Moff - 56);

            // Gear 10
            SaveGearRatioToGt5Save(TextBox10.Text, _formManager.MainForm.Moff - 55, _formManager.MainForm.Moff - 54);

            // Gear 11
            SaveGearRatioToGt5Save(TextBox11.Text, _formManager.MainForm.Moff - 53, _formManager.MainForm.Moff - 52);

            // Final drive
            SaveGearRatioToGt5Save(TextBox12.Text, _formManager.MainForm.Moff - 51, _formManager.MainForm.Moff - 50);

            Close();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox13.Text))
            {
                TextBox13.Text = "0";
            }

            if (float.TryParse(TextBox13.Text, out float maxSpeed) && maxSpeed != 0f)
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

                // Calculate the adjusted ratio based on original final drive, current final drive,
                // max speed, and last gear ratio
                float finalDriveRatio = float.Parse(TextBox12.Text);
                float adjustedRatio = (float)_originalFinalDrive / finalDriveRatio *
                                     maxSpeed *
                                     (1f / lastGear * (float)_lastgearpre);

                // Round to 1 decimal place and display
                TextBox14.Text = Math.Round(adjustedRatio, 1).ToString();
            }
        }
    }
}