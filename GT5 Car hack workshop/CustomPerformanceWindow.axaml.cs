using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GT5_Car_hack_workshop.Services;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace GT5_Car_hack_workshop
{
    public partial class CustomPerformanceWindow : Window
    {
        private readonly IFormManager _formManager;

        public CustomPerformanceWindow(IFormManager formManager)
        {
            _formManager = formManager;
            InitializeComponent();
        }

        public CustomPerformanceWindow()
        {
            InitializeComponent();
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!NoPartInstallCheckBox.IsChecked == true)
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
                await ShowMessageBox("Please enter a valid number for the Turbo");
                return;
            }

            // Engine
            SetCombinedValue(175, 174, 0); // Reset bytes
            if (ushort.TryParse(EngineTextBox.Text, out var number2))
            {
                SetCombinedValue(173, 172, number2);
            }
            else
            {
                await ShowMessageBox("Please enter a valid number for the Engine");
                return;
            }

            // Exhaust
            SetCombinedValue(155, 154, 0); // Reset bytes
            if (ushort.TryParse(ExhaustTextBox.Text, out var number3))
            {
                SetCombinedValue(153, 152, number3);
            }
            else
            {
                await ShowMessageBox("Please enter a valid number for the Exhaust Multiplier");
                return;
            }

            // Transmission
            SetCombinedValue(179, 178, 0); // Reset bytes
            if (ushort.TryParse(TextBox4.Text, out var number4))
            {
                SetCombinedValue(177, 176, number4);
            }
            else
            {
                await ShowMessageBox("Please enter a valid number for the ???Transmission???");
                return;
            }

            // Suspension
            SetCombinedValue(131, 130, 0); // Reset bytes
            if (ushort.TryParse(TextBox5.Text, out var number5))
            {
                SetCombinedValue(129, 128, number5);
            }
            else
            {
                await ShowMessageBox("Please enter a valid number for the ???Suspension???");
                return;
            }

            // Drivetrain
            SetCombinedValue(127, 126, 0); // Reset bytes
            if (ushort.TryParse(TextBox6.Text, out var number6))
            {
                SetCombinedValue(125, 124, number6);
            }
            else
            {
                await ShowMessageBox("Please enter a valid number for the ???Drivetrain???");
                return;
            }

            // Brakes
            SetCombinedValue(123, 122, 0); // Reset bytes
            if (ushort.TryParse(TextBox7.Text, out var number7))
            {
                SetCombinedValue(121, 120, number7);
            }
            else
            {
                await ShowMessageBox("Please enter a valid number for the ???Brakes???");
                return;
            }

            // Chassis
            SetCombinedValue(119, 118, 0); // Reset bytes
            if (ushort.TryParse(TextBox8.Text, out var number8))
            {
                SetCombinedValue(117, 116, number8);
            }
            else
            {
                await ShowMessageBox("Please enter a valid number for the ???Chassis???");
                return;
            }

            // Update main form text boxes
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

        private void LoadCurrentCarPerformanceButton_Click(object sender, RoutedEventArgs e)
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

        private ushort GetCombinedValue(int offsetHigh, int offsetLow)
        {
            var gt5Save = _formManager.MainForm.Gt5Save;
            var moff = _formManager.MainForm.Moff;

            var highByte = gt5Save[moff - offsetHigh];
            var lowByte = gt5Save[moff - offsetLow];

            return ByteUtils.BytesToUshort(highByte, lowByte);
        }

        private void SetCombinedValue(int offsetHigh, int offsetLow, ushort value)
        {
            var gt5Save = _formManager.MainForm.Gt5Save;
            var moff = _formManager.MainForm.Moff;

            // Write in big endian format - high byte first
            gt5Save[moff - offsetHigh] = (byte)(value >> 8); // High byte
            gt5Save[moff - offsetLow] = (byte)(value & 0xFF); // Low byte
        }

        private async System.Threading.Tasks.Task ShowMessageBox(string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Custom Performance", message, ButtonEnum.Ok);
            await box.ShowAsync();
        }
    }
}
