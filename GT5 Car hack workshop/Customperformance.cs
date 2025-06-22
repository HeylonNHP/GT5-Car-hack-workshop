using System;
using System.Windows.Forms;
using GT5_Car_hack_workshop.Services;

namespace GT5_Car_hack_workshop
{
    public partial class Customperformance : Form
    {
        private readonly IFormManager _formManager;

        public Customperformance(IFormManager formManager)
        {
            _formManager = formManager;
            Load += Customperformance_Load;
            InitializeComponent();
        }

        private void DiscardButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (!NoPartInstallCheckBox.Checked)
            {
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 332] = 56;
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 331] = 9;
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 330] = 8;
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 315] = 80;
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 314] = 9;
            }

            // Turbo
            SetCombinedValue(171, 170, 0); // Reset bytes
            if (ushort.TryParse(TurboTextBox.Text, out var number))
            {
                SetCombinedValue(169, 168, number);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the Turbo");
            }

            // Engine
            SetCombinedValue(175, 174, 0); // Reset bytes
            if (ushort.TryParse(EngineTextBox.Text, out var number2))
            {
                SetCombinedValue(173, 172, number2);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the Engine");
            }

            // Exhaust
            SetCombinedValue(155, 154, 0); // Reset bytes
            if (ushort.TryParse(ExhaustTextBox.Text, out var number3))
            {
                SetCombinedValue(153, 152, number3);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the Exhaust Multiplier");
            }

            // Transmission
            SetCombinedValue(179, 178, 0); // Reset bytes
            if (ushort.TryParse(TextBox4.Text, out var number4))
            {
                SetCombinedValue(177, 176, number4);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the ???Transmission???"); // TODO: Fix these
            }

            // Suspension
            SetCombinedValue(131, 130, 0); // Reset bytes
            if (ushort.TryParse(TextBox5.Text, out var number5))
            {
                SetCombinedValue(129, 128, number5);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the ???Suspension???");
            }

            // Drivetrain
            SetCombinedValue(127, 126, 0); // Reset bytes
            if (ushort.TryParse(TextBox6.Text, out var number6))
            {
                SetCombinedValue(125, 124, number6);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the ???Drivetrain???");
            }

            // Brakes
            SetCombinedValue(123, 122, 0); // Reset bytes
            if (ushort.TryParse(TextBox7.Text, out var number7))
            {
                SetCombinedValue(121, 120, number7);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the ???Brakes???");
            }

            // Chassis
            SetCombinedValue(119, 118, 0); // Reset bytes
            if (ushort.TryParse(TextBox8.Text, out var number8))
            {
                SetCombinedValue(117, 116, number8);
            }
            else
            {
                MessageBox.Show(@"Please enter a valid number for the ???Chassis???");
            }

            _formManager.MainForm.TurboModifierTextBox.Text = string.Concat(
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 171].ToString("X2"), " ",
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 170].ToString("X2"), " ",
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 169].ToString("X2"), " ",
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 168].ToString("X2"));

            _formManager.MainForm.ExhauseMultiplierTextBox.Text = string.Concat(
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 155].ToString("X2"), " ",
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 154].ToString("X2"), " ",
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 153].ToString("X2"), " ",
                _formManager.MainForm.Gt5Save[_formManager.MainForm.Moff - 152].ToString("X2"));
            Close();
        }

        private void LoadCurrentCarPerformanceButton_Click(object sender, EventArgs e)
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

        ushort GetCombinedValue(int offsetHigh, int offsetLow)
        {
            var gt5Save = _formManager.MainForm.Gt5Save;
            var moff = _formManager.MainForm.Moff;

            var highByte = gt5Save[moff - offsetHigh];
            var lowByte = gt5Save[moff - offsetLow];

            return ByteUtils.BytesToUshort(highByte, lowByte);
        }

        void SetCombinedValue(int offsetHigh, int offsetLow, ushort value)
        {
            var gt5Save = _formManager.MainForm.Gt5Save;
            var moff = _formManager.MainForm.Moff;

            // Write in big endian format - high byte first
            gt5Save[moff - offsetHigh] = (byte)(value >> 8); // High byte
            gt5Save[moff - offsetLow] = (byte)(value & 0xFF); // Low byte
        }

        private void Customperformance_Load(object sender, EventArgs e)
        {
        }
    }
}