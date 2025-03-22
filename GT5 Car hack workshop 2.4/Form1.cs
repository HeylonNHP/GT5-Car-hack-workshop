using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GT5_Car_hack_workshop_2.Models;
using GT5_Car_hack_workshop_2.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace GT5_Car_hack_workshop_2
{
    [DesignerGenerated]
    public partial class Form1 : Form
    {
        private const string PARTS_DATABASE_FILENAME = "partsdatabase.db";
        private string _CarName;

        private List<CarParts> _CarPartsList; // New car parts list

        private string[] _ProgramSettings;

        public byte[] Gt5Save;

        public int Moff;

        public Form1()
        {
            Load += Form1_Load;
            FormClosing += Form1_FormClosing;
            Moff = 0;
            _CarName = "";
            InitializeComponent();
        }

        private void ProcessData()
        {
            if (!File.Exists(TextBox1.Text))
            {
                MessageBox.Show("GT5.0 file doesn't exist!");
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBox2.Text))
            {
                MessageBox.Show("You left the PSN name empty! We need this to load up your data!");
                return;
            }

            Gt5Save = LoadData.Load(TextBox1.Text);

            var psnUserCharacterArray = TextBox2.Text.ToCharArray();
            var psnUserCharacterByteArray = psnUserCharacterArray.Select(c => (byte)c).ToArray();

            Moff = LoadData.FindSequence(Gt5Save, psnUserCharacterByteArray) - 306;
            if (Moff < 1)
            {
                MessageBox.Show("PSN name is incorrect or data is corrupt\rRemember, the PSN name is CASE sensitive.");
                return;
            }

            // Grab current car's name from PARAM.SFO
            try
            {
                var gt50File = new FileInfo(TextBox1.Text);

                var paramSfoBytes = File.ReadAllBytes(gt50File.DirectoryName + "\\PARAM.SFO");
                var currentCar = "Current Car: ";
                var currentCarBytes = currentCar.ToCharArray().Select(c => (byte)c).ToArray();
                var currentCarIndex = LoadData.FindSequence(paramSfoBytes, currentCarBytes) + currentCarBytes.Length;

                // Find the index where the car's name string ends. 
                var endIndex = 0;
                for (var i = currentCarIndex; i < paramSfoBytes.Length; i++)
                    if (paramSfoBytes[i] == 0)
                    {
                        endIndex = i;
                        break;
                    }

                var currentCarNameBytes = paramSfoBytes.Skip(currentCarIndex).Take(endIndex - currentCarIndex).ToArray();
                var currentCarString = new string(currentCarNameBytes.Select(s => (char)s).ToArray());
                Label7.Text = currentCar + currentCarString;
                _CarName = currentCarString;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Can't get param.sfo for loading the current car's name.\n{ex.Message}");
            }

            EngineCodeTextBox.Text = Gt5Save[Moff - 213].ToString("X2") + " " + Gt5Save[Moff - 212].ToString("X2");
            TorqueSplitTextBox.Text = Conversion.Val("&H" + Gt5Save[Moff - 46].ToString("X2")).ToString(CultureInfo.CurrentCulture);
            DrivetrainCodeTextBox.Text = Gt5Save[Moff - 209].ToString("X2") + " " + Gt5Save[Moff - 208].ToString("X2");
            ChassisCodeTextBox.Text = Gt5Save[Moff - 217].ToString("X2") + " " + Gt5Save[Moff - 216].ToString("X2");
            TransmissionCodeTextBox.Text = Gt5Save[Moff - 205].ToString("X2") + " " + Gt5Save[Moff - 204].ToString("X2");
            RemoveSpoilerCodeTextBox.Text = Gt5Save[Moff - 88].ToString();

            PaintCodesTextBox.Text = $"{Gt5Save[Moff - 344]:X2} {Gt5Save[Moff - 343]:X2} {Gt5Save[Moff - 342]:X2} {Gt5Save[Moff - 341]:X2}";

            TurboModifierTextBox.Text = $"{Gt5Save[Moff - 171]:X2} {Gt5Save[Moff - 170]:X2} {Gt5Save[Moff - 169]:X2} {Gt5Save[Moff - 168]:X2}";

            AeroFrontTextBox.Text = Gt5Save[Moff - 43].ToString();
            AeroRearTextBox.Text = Gt5Save[Moff - 42].ToString();

            var suspensionHeightFront = ByteUtils.ConvertBytesToUnsignedInt(new[] { Gt5Save[Moff - 33], Gt5Save[Moff - 32] });
            SuspensionHeightFrontTextBox.Text = suspensionHeightFront.ToString();
            var suspensionHeightRear = ByteUtils.ConvertBytesToUnsignedInt(new[] { Gt5Save[Moff - 31], Gt5Save[Moff - 30] });
            SuspensionHeightRearTextBox.Text = suspensionHeightRear.ToString();

            GripTextBox.Text = Gt5Save[Moff + 10].ToString();

            SpringRateFrontTextBox.Text = Gt5Save[Moff - 27].ToString();
            SpringRateRearTextBox.Text = Gt5Save[Moff - 26].ToString();

            ExhauseMultiplierTextBox.Text = $"{Gt5Save[Moff - 155]:X2} {Gt5Save[Moff - 154]:X2} {Gt5Save[Moff - 153]:X2} {Gt5Save[Moff - 152]:X2}";

            CarBodyCodeTextBox.Text = $"{Gt5Save[Moff - 262]:X2} {Gt5Save[Moff - 261]:X2}";

            SuspensionCodeTextBox.Text = $"{Gt5Save[Moff - 201]:X2} {Gt5Save[Moff - 200]:X2}";

            LsdCodeTextBox.Text = $"{Gt5Save[Moff - 197]:X2} {Gt5Save[Moff - 196]:X2}";

            HornCodeTextBox.Text = $"{Gt5Save[Moff + 23]:X2} {Gt5Save[Moff + 24]:X2}";

            WeightMultiplierTextBox.Text = ByteUtils.ConvertBytesToUnsignedInt(new Byte[] { Gt5Save[Moff - 191], Gt5Save[Moff - 190], Gt5Save[Moff - 189], Gt5Save[Moff - 188] }).ToString();
        }

        private void SaveData()
        {
            try
            {
                var engineByteValues = ByteUtils.HexStringToByteArray(EngineCodeTextBox.Text);

                Gt5Save[Moff - 213] = engineByteValues[0];
                Gt5Save[Moff - 212] = engineByteValues[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Can't save engine code to the save file.\n{ex.Message}");
                return;
            }

            try
            {
                if (!int.TryParse(TorqueSplitTextBox.Text, out var value)) throw new FormatException("Torque split value must be a number.");

                if (value < 0 || value > 255) throw new FormatException("Torque split value must be between 0 and 255.");

                Gt5Save[Moff - 46] = (byte)value;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save Torque split to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var drivetrainByteValues = ByteUtils.HexStringToByteArray(DrivetrainCodeTextBox.Text);

                Gt5Save[Moff - 209] = drivetrainByteValues[0];
                Gt5Save[Moff - 208] = drivetrainByteValues[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Can't save drivetrain code to the save file.\n{ex.Message}");
                return;
            }

            try
            {
                var chassisByteValues = ByteUtils.HexStringToByteArray(ChassisCodeTextBox.Text);

                Gt5Save[Moff - 217] = chassisByteValues[0];
                Gt5Save[Moff - 216] = chassisByteValues[1];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save chassis code to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var transmissionByteValues = ByteUtils.HexStringToByteArray(TransmissionCodeTextBox.Text);

                Gt5Save[Moff - 205] = transmissionByteValues[0];
                Gt5Save[Moff - 204] = transmissionByteValues[1];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save transmission code to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!int.TryParse(RemoveSpoilerCodeTextBox.Text, out var value)) throw new FormatException("Remove spoiler value must be a number.");

                if (value < 0 || value > 255) throw new FormatException("Remove spoiler value must be between 0 and 255.");

                Gt5Save[Moff - 88] = (byte)value;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save remove spoiler to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var paintCodes = ByteUtils.HexStringToByteArray(PaintCodesTextBox.Text);

                Gt5Save[Moff - 344] = paintCodes[0];
                Gt5Save[Moff - 343] = paintCodes[1];
                Gt5Save[Moff - 342] = paintCodes[2];
                Gt5Save[Moff - 341] = paintCodes[3];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Can't save paint codes to the save file.\n{ex.Message}");
                return;
            }

            try
            {
                var turboModifier = ByteUtils.HexStringToByteArray(TurboModifierTextBox.Text);

                Gt5Save[Moff - 171] = turboModifier[0];
                Gt5Save[Moff - 170] = turboModifier[1];
                Gt5Save[Moff - 169] = turboModifier[2];
                Gt5Save[Moff - 168] = turboModifier[3];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save turbo modifier to the save file.\n{e.Message}");
                return;
            }

            if (SambaLikePerformanceCheckbox.Checked)
            {
                Gt5Save[Moff - 332] = 56;
                Gt5Save[Moff - 331] = 9;
                Gt5Save[Moff - 330] = 8;
                Gt5Save[Moff - 315] = 80;
                Gt5Save[Moff - 314] = 9;
                Gt5Save[Moff - 179] = 0;
                Gt5Save[Moff - 178] = 0;
                Gt5Save[Moff - 177] = 5;
                Gt5Save[Moff - 176] = 81;
                Gt5Save[Moff - 175] = 0;
                Gt5Save[Moff - 174] = 0;
                Gt5Save[Moff - 173] = 14;
                Gt5Save[Moff - 172] = 242;
                Gt5Save[Moff - 171] = 0;
                Gt5Save[Moff - 170] = 0;
                Gt5Save[Moff - 169] = 21;
                Gt5Save[Moff - 168] = 39;
                Gt5Save[Moff - 155] = 0;
                Gt5Save[Moff - 154] = 0;
                Gt5Save[Moff - 153] = 20;
                Gt5Save[Moff - 152] = 60;
                Gt5Save[Moff - 131] = 0;
                Gt5Save[Moff - 130] = 0;
                Gt5Save[Moff - 129] = 3;
                Gt5Save[Moff - 128] = 88;
                Gt5Save[Moff - 127] = 0;
                Gt5Save[Moff - 126] = 0;
                Gt5Save[Moff - 125] = 3;
                Gt5Save[Moff - 124] = 88;
                Gt5Save[Moff - 123] = 0;
                Gt5Save[Moff - 122] = 0;
                Gt5Save[Moff - 121] = 3;
                Gt5Save[Moff - 120] = 56;
                Gt5Save[Moff - 119] = 0;
                Gt5Save[Moff - 118] = 0;
                Gt5Save[Moff - 117] = 3;
                Gt5Save[Moff - 116] = 61;
            }

            try
            {
                if (!byte.TryParse(AeroFrontTextBox.Text, out var AeroFront)) throw new FormatException("Aero front value must be a byte value (0-255).");

                Gt5Save[Moff - 43] = AeroFront;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save aero front to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!byte.TryParse(AeroRearTextBox.Text, out var AeroRear)) throw new FormatException("Aero rear value must be a byte value (0-255).");

                Gt5Save[Moff - 42] = AeroRear;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save aero rear to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!ushort.TryParse(SuspensionHeightFrontTextBox.Text, out var suspensionHeightFront)) throw new FormatException("Suspension height front value must be a number.");

                var suspensionHeightFrontBytes = ByteUtils.UshortToByteArray(suspensionHeightFront);
                Gt5Save[Moff - 33] = suspensionHeightFrontBytes[0];
                Gt5Save[Moff - 32] = suspensionHeightFrontBytes[1];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save suspension height front to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!ushort.TryParse(SuspensionHeightRearTextBox.Text, out var suspensionHeightRear)) throw new FormatException("Suspension height rear value must be a number.");

                var suspensionHeightRearBytes = ByteUtils.UshortToByteArray(suspensionHeightRear);
                Gt5Save[Moff - 31] = suspensionHeightRearBytes[0];
                Gt5Save[Moff - 30] = suspensionHeightRearBytes[1];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save suspension height rear to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!byte.TryParse(GripTextBox.Text, out var grip)) throw new FormatException("Grip value must be a byte value (0-255).");

                Gt5Save[Moff + 10] = grip;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save grip to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!byte.TryParse(SpringRateFrontTextBox.Text, out var springRateFront)) throw new FormatException("Spring rate front value must be a byte value (0-255).");

                Gt5Save[Moff - 27] = springRateFront;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save spring rate front to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!byte.TryParse(SpringRateRearTextBox.Text, out var springRateRear)) throw new FormatException("Spring rate front value must be a byte value (0-255).");

                Gt5Save[Moff - 26] = springRateRear;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save spring rate rear to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var exhauseMultiplier = ByteUtils.HexStringToByteArray(ExhauseMultiplierTextBox.Text);

                Gt5Save[Moff - 155] = exhauseMultiplier[0];
                Gt5Save[Moff - 154] = exhauseMultiplier[1];
                Gt5Save[Moff - 153] = exhauseMultiplier[2];
                Gt5Save[Moff - 152] = exhauseMultiplier[3];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save exhause multiplier to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var carBodyCode = ByteUtils.HexStringToByteArray(CarBodyCodeTextBox.Text);

                Gt5Save[Moff - 262] = carBodyCode[0];
                Gt5Save[Moff - 261] = carBodyCode[1];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save car body code to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var suspensionCode = ByteUtils.HexStringToByteArray(SuspensionCodeTextBox.Text);

                Gt5Save[Moff - 201] = suspensionCode[0];
                Gt5Save[Moff - 200] = suspensionCode[1];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save suspension code to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var lsdCode = ByteUtils.HexStringToByteArray(LsdCodeTextBox.Text);

                Gt5Save[Moff - 197] = lsdCode[0];
                Gt5Save[Moff - 196] = lsdCode[1];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save LSD code to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var hornCode = ByteUtils.HexStringToByteArray(HornCodeTextBox.Text);

                Gt5Save[Moff + 23] = hornCode[0];
                Gt5Save[Moff + 24] = hornCode[1];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save Horn code to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (uint.TryParse(WeightMultiplierTextBox.Text, out var weightMultiplierInt))
                {
                    var weightMultiplierBytes = ByteUtils.ConvertToByteArray(weightMultiplierInt, 4);
                    Gt5Save[Moff - 191] = weightMultiplierBytes[0];
                    Gt5Save[Moff - 190] = weightMultiplierBytes[1];
                    Gt5Save[Moff - 189] = weightMultiplierBytes[2];
                    Gt5Save[Moff - 188] = weightMultiplierBytes[3];
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't save weight multiplier to the save file.\n{e.Message}");
                return;
            }

            if (Add74ByteCheckBox.Checked) Gt5Save[Moff - 260] = 116;

            if (AddLucaBytesCheckBox.Checked)
            {
                Gt5Save[Moff - 260] = 76;
                Gt5Save[Moff - 259] = 85;
                Gt5Save[Moff - 258] = 67;
                Gt5Save[Moff - 257] = 65;
            }

            if (RemoveHoodCheckBox.Checked)
            {
                Gt5Save[Moff - 313] = byte.MaxValue;
                Gt5Save[Moff - 107] = 0;
                Gt5Save[Moff - 106] = 52;
                Gt5Save[Moff - 105] = 0;
                Gt5Save[Moff - 104] = 3;
                Gt5Save[Moff - 311] = 0;
                Gt5Save[Moff - 83] = byte.MaxValue;
                Gt5Save[Moff - 82] = byte.MaxValue;
                Gt5Save[Moff - 81] = byte.MaxValue;
                Gt5Save[Moff - 80] = byte.MaxValue;
            }

            if (RemoveFrontBumperCheckBox.Checked)
            {
                Gt5Save[Moff - 367] = 0;
                Gt5Save[Moff - 103] = 0;
                Gt5Save[Moff - 102] = 52;
                Gt5Save[Moff - 101] = 0;
                Gt5Save[Moff - 100] = 2;
                Gt5Save[Moff - 311] = 0;
                Gt5Save[Moff - 83] = byte.MaxValue;
                Gt5Save[Moff - 82] = byte.MaxValue;
                Gt5Save[Moff - 81] = byte.MaxValue;
                Gt5Save[Moff - 80] = byte.MaxValue;
            }

            if (RemoveRearBumperCheckBox.Checked)
            {
                Gt5Save[Moff - 367] = 0;
                Gt5Save[Moff - 99] = 0;
                Gt5Save[Moff - 98] = 52;
                Gt5Save[Moff - 97] = 0;
                Gt5Save[Moff - 96] = 2;
                Gt5Save[Moff - 311] = 0;
                Gt5Save[Moff - 83] = byte.MaxValue;
                Gt5Save[Moff - 82] = byte.MaxValue;
                Gt5Save[Moff - 81] = byte.MaxValue;
                Gt5Save[Moff - 80] = byte.MaxValue;
            }

            File.WriteAllBytes(TextBox1.Text, Gt5Save);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Find your GT5.0 file";
            if (ofd.ShowDialog() == DialogResult.OK) TextBox1.Text = ofd.FileName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!MyProject.Computer.FileSystem.DirectoryExists("Backups")) MyProject.Computer.FileSystem.CreateDirectory("Backups");

            _ProgramSettings = SettingsFileClass.LoadSettings("GT5CHWsettings.ini", 1);
            // New car parts list
            _CarPartsList = SettingsFileClass.LoadCarParts(PARTS_DATABASE_FILENAME);
            TextBox1.Text = _ProgramSettings[0];
            TextBox2.Text = _ProgramSettings[1];
            LoadParts();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _ProgramSettings[0] = TextBox1.Text;
            _ProgramSettings[1] = TextBox2.Text;
            SettingsFileClass.SaveSettings(_ProgramSettings, "GT5CHWsettings.ini");
            SettingsFileClass.SaveCarParts(_CarPartsList, PARTS_DATABASE_FILENAME);
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            var fileInfo = new FileInfo(TextBox1.Text);
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss");
            File.Copy(TextBox1.Text, $"Backups\\{currentDate} {fileInfo.Name}");
            ProcessData();
        }

        private void SaveAndEncrypt_Click(object sender, EventArgs e)
        {
            SaveData();
            LoadData.Encrypt(TextBox1.Text);
            Interaction.MsgBox("Data saved, immediately goto the GT Auto and change the cars oil to apply the hacks.");
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            TextBox5.Text = Conversions.ToString(100.0 - Conversion.Val(TorqueSplitTextBox.Text));
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            TorqueSplitTextBox.Text = Conversions.ToString(50);
            DrivetrainCodeTextBox.Text = "0A 1E";
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 355))] = 0;
            Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 333))] = 88;
            Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 211))] = 0;
            Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 210))] = 0;
            Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 209))] = 10;
            Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 208))] = 84;
            Interaction.MsgBox("Torque split editor installed, don't forget to save");
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "Copy the hex values in this box, this is your current cars engine\r\rIf you paste your copied engine values back in while editing another car, that car will have the engine of the car you copied it from.");
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "This directly controls the torque that is sent to the front wheels, EG: if you typed in 10 the torque split would then be 10:90, if you typed in 50 the split would be 50:50\rYou could be really tricky and type 101, then the split would be 101:-1, which I reccomend in combination with added grip for high HP cars");
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "This is what gives hackers all the freedom to swap chassis, engine etc. in update 2.14.\rUntick this and you will find that most hacks won't work");
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "Make sure the highest stage spoiler in GT Auto is installed. EG Type B or Type C (which ever one is the highest available).\rThen increase the value here by 1. EG: 2 to 3 or 3 to 4 etc.");
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            MyProject.Forms.TransmissionEditor.Show();
        }

        /// <summary>
        ///     Add current car to parts database.
        ///     Handles the event triggered when button12 is clicked. This method is used to add the current car details
        ///     to the parts database. It validates for available space in the database and ensures no duplicate entries exist.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs object containing event data.</param>
        private void Button12_Click(object sender, EventArgs e)
        {
            _CarName = Interaction.InputBox("Car name:", "", Conversions.ToString(_CarName));
            if (string.IsNullOrWhiteSpace(_CarName))
            {
                MessageBox.Show("Car name cannot be empty");
                return;
            }

            try
            {
                // Create new CarParts object with current car data
                var newCarParts = new CarParts
                {
                    Name = _CarName,
                    Engine = ByteUtils.HexStringToUshort(EngineCodeTextBox.Text),
                    Drivetrain = ByteUtils.HexStringToUshort(DrivetrainCodeTextBox.Text),
                    Chassis = ByteUtils.HexStringToUshort(ChassisCodeTextBox.Text),
                    Transmission = ByteUtils.HexStringToUshort(TransmissionCodeTextBox.Text),
                    Body = ByteUtils.HexStringToUshort(CarBodyCodeTextBox.Text),
                    Suspension = ByteUtils.HexStringToUshort(SuspensionCodeTextBox.Text),
                    Lsd = ByteUtils.HexStringToUshort(LsdCodeTextBox.Text),
                    Horn = ByteUtils.HexStringToUshort(HornCodeTextBox.Text)
                };

                // Check for duplicates
                if (_CarPartsList.Any(cp => cp.Name.Equals(_CarName, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Car already exists", "", MessageBoxButtons.OK);
                    return;
                }

                // Add to list and save
                _CarPartsList.Add(newCarParts);
                SettingsFileClass.SaveCarParts(_CarPartsList, PARTS_DATABASE_FILENAME);
                LoadParts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding car to database: {ex.Message}");
            }
        }

        private void LoadParts()
        {
            // Clear all ComboBoxes
            foreach (var comboBox in new[] { EngineCodeComboBox, DrivetrainCodeComboBox, ChassisCodeComboBox, TransmissionCodeComboBox, SuspensionCodeComboBox, BodyCodeComboBox, LsdCodeComboBox, HornCodeComboBox }) 
            {
                comboBox.Items.Clear();
                comboBox.SelectedIndex = -1;
            }

            // Set up ComboBox data sources
            if (_CarPartsList != null && _CarPartsList.Count > 0)
            {
                try
                {
                    // Create sorted lists from the car parts list
                    var engineList = new List<CarParts>(_CarPartsList.OrderBy(cp => cp.Name));
                    var drivetrainList = new List<CarParts>(_CarPartsList.OrderBy(cp => cp.Name));
                    var chassisList = new List<CarParts>(_CarPartsList.OrderBy(cp => cp.Name));
                    var transmissionList = new List<CarParts>(_CarPartsList.OrderBy(cp => cp.Name));
                    var suspensionList = new List<CarParts>(_CarPartsList.OrderBy(cp => cp.Name));
                    var bodyList = new List<CarParts>(_CarPartsList.OrderBy(cp => cp.Name));
                    var lsdList = new List<CarParts>(_CarPartsList.OrderBy(cp => cp.Name));
                    var hornList = new List<CarParts>(_CarPartsList.OrderBy(cp => cp.Name));
                    
                    // Insert a null item at the beginning of each sorted list
                    engineList.Insert(0, new CarParts() {Name = "Select Engine"});
                    drivetrainList.Insert(0, new CarParts() {Name = "Select Drivetrain"});
                    chassisList.Insert(0, new CarParts() {Name = "Select Chassis"});
                    transmissionList.Insert(0, new CarParts() {Name = "Select Transmission"});
                    suspensionList.Insert(0, new CarParts() {Name = "Select Suspension"});
                    bodyList.Insert(0, new CarParts() {Name = "Select Body"});
                    lsdList.Insert(0, new CarParts() {Name = "Select LSD"});
                    hornList.Insert(0, new CarParts() {Name = "Select Horn"});
                    
                    // Configure each ComboBox to display the car name but store the entire CarParts object
                    EngineCodeComboBox.DisplayMember = "Name";
                    EngineCodeComboBox.ValueMember = "Engine";
                    EngineCodeComboBox.DataSource = new BindingSource(engineList, null);
                    EngineCodeComboBox.SelectedItem = engineList[0];
                    
                    DrivetrainCodeComboBox.DisplayMember = "Name";
                    DrivetrainCodeComboBox.ValueMember = "Drivetrain";
                    DrivetrainCodeComboBox.DataSource = new BindingSource(drivetrainList, null);
                    DrivetrainCodeComboBox.SelectedItem = drivetrainList[0];
                    
                    ChassisCodeComboBox.DisplayMember = "Name";
                    ChassisCodeComboBox.ValueMember = "Chassis";
                    ChassisCodeComboBox.DataSource = new BindingSource(chassisList, null);
                    ChassisCodeComboBox.SelectedItem = chassisList[0];
                    
                    TransmissionCodeComboBox.DisplayMember = "Name";
                    TransmissionCodeComboBox.ValueMember = "Transmission";
                    TransmissionCodeComboBox.DataSource = new BindingSource(transmissionList, null);
                    TransmissionCodeComboBox.SelectedItem = transmissionList[0];
                    
                    SuspensionCodeComboBox.DisplayMember = "Name";
                    SuspensionCodeComboBox.ValueMember = "Suspension";
                    SuspensionCodeComboBox.DataSource = new BindingSource(suspensionList, null);
                    SuspensionCodeComboBox.SelectedItem = suspensionList[0];
                    
                    BodyCodeComboBox.DisplayMember = "Name";
                    BodyCodeComboBox.ValueMember = "Body";
                    BodyCodeComboBox.DataSource = new BindingSource(bodyList, null);
                    BodyCodeComboBox.SelectedItem = bodyList[0];
                    
                    LsdCodeComboBox.DisplayMember = "Name";
                    LsdCodeComboBox.ValueMember = "Lsd";
                    LsdCodeComboBox.DataSource = new BindingSource(lsdList, null);
                    LsdCodeComboBox.SelectedItem = lsdList[0];
                    
                    HornCodeComboBox.DisplayMember = "Name";
                    HornCodeComboBox.ValueMember = "Horn";
                    HornCodeComboBox.DataSource = new BindingSource(hornList, null);
                    HornCodeComboBox.SelectedItem = hornList[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An issue occurred while loading the parts database: {ex.Message}");
                }
            }
        }

        private void EngineCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (EngineCodeComboBox.SelectedItem is CarParts selectedParts && selectedParts.Name != "Select Engine")
                {
                    EngineCodeTextBox.Text = ByteUtils.UshortToHexString(selectedParts.Engine);
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception if needed
            }
        }

        private void DrivetrainCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DrivetrainCodeComboBox.SelectedItem is CarParts selectedParts && selectedParts.Name != "Select Drivetrain")
                {
                    DrivetrainCodeTextBox.Text = ByteUtils.UshortToHexString(selectedParts.Drivetrain);
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception if needed
            }
        }

        private void ChassisCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChassisCodeComboBox.SelectedItem is CarParts selectedParts && selectedParts.Name != "Select Chassis")
                {
                    ChassisCodeTextBox.Text = ByteUtils.UshortToHexString(selectedParts.Chassis);
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception if needed
            }
        }

        private void TransmissionCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (TransmissionCodeComboBox.SelectedItem is CarParts selectedParts && selectedParts.Name != "Select Transmission")
                {
                    TransmissionCodeTextBox.Text = ByteUtils.UshortToHexString(selectedParts.Transmission);
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception if needed
            }
        }

        private void SuspensionCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (SuspensionCodeComboBox.SelectedItem is CarParts selectedParts && selectedParts.Name != "Select Suspension")
                {
                    SuspensionCodeTextBox.Text = ByteUtils.UshortToHexString(selectedParts.Suspension);
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception if needed
            }
        }

        private void BodyCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (BodyCodeComboBox.SelectedItem is CarParts selectedParts && selectedParts.Name != "Select Body")
                {
                    CarBodyCodeTextBox.Text = ByteUtils.UshortToHexString(selectedParts.Body);
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception if needed
            }
        }

        private void LsdCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (LsdCodeComboBox.SelectedItem is CarParts selectedParts && selectedParts.Name != "Select LSD")
                {
                    LsdCodeTextBox.Text = ByteUtils.UshortToHexString(selectedParts.Lsd);
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception if needed
            }
        }

        private void HornCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (HornCodeComboBox.SelectedItem is CarParts selectedParts && selectedParts.Name != "Select Horn")
                {
                    HornCodeTextBox.Text = ByteUtils.UshortToHexString(selectedParts.Horn);
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception if needed
            }
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            MyProject.Forms.Customperformance.ShowDialog();
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "This will override the 74 byte checkbox\rGT5 Editor 1.6 uses this method instead of the 74 byte, I have no idea whether it's more effective. So I've added in this function for testing purposes.");
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            Gt5Save[Conversions.ToInteger(Operators.AddObject(Moff, 248))] = byte.MaxValue;
            Interaction.MsgBox(
                "The car is now yours, you can now either hack it, or click encrypt and save then return the data to the PS3");
            SaveData();
        }

        private void Button16_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "This is the performance multiplier of the exhaust and turbo. Increasing these will increase the effectiveness of these performance parts on the engines performance");
        }

        private void Button17_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "Samba like performance installs the performance parts out of a samba bus onto your current car.\rThe samba bus parts increase the engines performance a lot more than the performance parts for most other cars.\rEG: a 300hp (When stock) engine with samba bus parts installed can increase to about 1,000 - 1,800hp");
        }

        private void Button18_Click(object sender, EventArgs e)
        {
            Gt5Save[Moff + 2282] = byte.MaxValue;
            Gt5Save[Moff + 2283] = byte.MaxValue;
            Gt5Save[Moff + 2284] = byte.MaxValue;
            Gt5Save[Moff + 2285] = byte.MaxValue;
            Interaction.MsgBox("You now have 4,294,967,295 cr");
            SaveData();
        }

        private void SlamCar_Button19_Click(object sender, EventArgs e)
        {
            Gt5Save[Moff - 25] = 0;
            Gt5Save[Moff - 24] = 0;
        }

        private void Button20_Click(object sender, EventArgs e)
        {
            Gt5Save[Moff - 355] = 0;
            Gt5Save[Moff - 332] = 2;
        }

        private void Button21_Click(object sender, EventArgs e)
        {
            Gt5Save[Moff - 355] = 1;
            Gt5Save[Moff - 332] = 0;
        }

        private void Button22_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "-----------------GT5 car hack workshop-----------------\rCreated by HeylonNHP\rSpecial thanks to:\rflatz for the pfdtool\raldotools.org for games.conf/global.conf\rTo the guys at http://gt5dragracing.com/ for daring to beta test my first version");
        }

        private void Button23_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "When you tick this checkbox, you must buy the Rigidity Improvement right before doing anything after loading the hacked save, the Rigidity Improvement is found under Body/Chassis in the tuning shop. The hack wont take proper effect without buying this upgrade.\r\rIf you have already purchased this upgrade, it will be uninstalled when you apply this hack.");
        }
    }
}