using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using GT5_Car_hack_workshop.Models;
using GT5_Car_hack_workshop.Services;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace GT5_Car_hack_workshop
{
    public partial class MainWindow : Window
    {
        private const string PARTS_DATABASE_FILENAME = "partsdatabase.db";
        private string _CarName;
        private List<CarParts> _CarPartsList;
        private string[] _ProgramSettings;
        private readonly IFormManager _formManager;

        // Guards against paint field <-> combo box sync loops
        private bool _syncingPaintFields;

        public byte[] Gt5Save;
        public int Moff;

        // Avalonia's source generator will automatically create properties for x:Name controls

        public MainWindow(IFormManager formManager)
        {
            _formManager = formManager;
            Moff = 0;
            _CarName = "";
            InitializeComponent();
            InitializePaintSearchBoxes();
            InitializePartComboBoxes();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializePaintSearchBoxes();
            InitializePartComboBoxes();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void InitializePaintSearchBoxes()
        {
            BodyPaintAutoCompleteBox.ItemsSource = PaintDatabase.Entries;
            BodyPaintAutoCompleteBox.ItemFilter = PaintItemFilter;
            BodyPaintAutoCompleteBox.TextSelector = PaintTextSelector;

            WheelsPaintAutoCompleteBox.ItemsSource = PaintDatabase.Entries;
            WheelsPaintAutoCompleteBox.ItemFilter = PaintItemFilter;
            WheelsPaintAutoCompleteBox.TextSelector = PaintTextSelector;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var backupPath = Path.Combine(AppContext.BaseDirectory, "Backups");
            if (!Directory.Exists(backupPath)) Directory.CreateDirectory(backupPath);

            _ProgramSettings = SettingsFileClass.LoadSettings("GT5CHWsettings.ini", 1);
            _CarPartsList = SettingsFileClass.LoadCarParts(PARTS_DATABASE_FILENAME);

            // Safely access settings with bounds checking
            if (_ProgramSettings != null && _ProgramSettings.Length > 0)
                TextBox1.Text = _ProgramSettings[0];
            if (_ProgramSettings != null && _ProgramSettings.Length > 1)
                TextBox2.Text = _ProgramSettings[1];

            LoadParts();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _ProgramSettings[0] = TextBox1.Text;
            _ProgramSettings[1] = TextBox2.Text;
            SettingsFileClass.SaveSettings(_ProgramSettings, "GT5CHWsettings.ini");
            SettingsFileClass.SaveCarParts(_CarPartsList, PARTS_DATABASE_FILENAME);
        }

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Find your GT5.0 file",
                AllowMultiple = false
            });

            if (files.Count > 0)
            {
                TextBox1.Text = files[0].Path.LocalPath;
            }
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var filePath = TextBox1.Text?.Trim();
            if (string.IsNullOrWhiteSpace(filePath))
            {
                await ShowMessageBox("Please select a GT5.0 file first.");
                return;
            }

            if (!File.Exists(filePath))
            {
                await ShowMessageBox($"The file does not exist:\n{filePath}");
                return;
            }

            try
            {
                var fileInfo = new FileInfo(filePath);
                var currentDate = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss");
                var backupPath = Path.Combine(AppContext.BaseDirectory, "Backups", $"{currentDate} {fileInfo.Name}");
                File.Copy(filePath, backupPath);
                await ProcessData();
            }
            catch (Exception ex)
            {
                await ShowMessageBox($"Error creating backup:\n{ex.Message}");
            }
        }

        private async void SaveAndEncrypt_Click(object sender, RoutedEventArgs e)
        {
            await SaveData();
            LoadData.Encrypt(TextBox1.Text);
            await ShowMessageBox("Data saved, immediately goto the GT Auto and change the cars oil to apply the hacks.");
        }

        private async void Button5_Click(object sender, RoutedEventArgs e)
        {
            await SaveData();
        }

        // ---- Paint database search box <-> hex field syncing ----

        private void BodyPaintAutoCompleteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_syncingPaintFields) return;
            if (BodyPaintAutoCompleteBox.SelectedItem is PaintEntry entry)
                BodyPaintTextBox.Text = entry.Id.ToString("X4");
        }

        private void WheelsPaintAutoCompleteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_syncingPaintFields) return;
            if (WheelsPaintAutoCompleteBox.SelectedItem is PaintEntry entry)
                WheelsPaintTextBox.Text = entry.Id.ToString("X4");
        }

        private void BodyPaintTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_syncingPaintFields) return;
            SyncPaintSearchBox(BodyPaintAutoCompleteBox, BodyPaintTextBox.Text);
        }

        private void WheelsPaintTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_syncingPaintFields) return;
            SyncPaintSearchBox(WheelsPaintAutoCompleteBox, WheelsPaintTextBox.Text);
        }

        /// <summary>
        /// Keeps a paint search box in step with the authoritative hex field: shows the matching
        /// colour's name when the ID is known, or clears it for custom/unknown values.
        /// </summary>
        private void SyncPaintSearchBox(AutoCompleteBox searchBox, string? text)
        {
            var id = uint.TryParse(text?.Replace(" ", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value)
                ? value
                : (uint?)null;

            _syncingPaintFields = true;
            searchBox.Text = id.HasValue ? PaintDatabase.Find(id.Value)?.ToString() ?? "" : "";
            _syncingPaintFields = false;
        }

        /// <summary>Matches typed text against a colour's name, maker or hex ID.</summary>
        private static bool PaintItemFilter(string? search, object? item)
        {
            if (item is not PaintEntry entry) return false;
            if (string.IsNullOrWhiteSpace(search)) return true; // no filter yet - show the whole list
            var s = search.Trim().ToLowerInvariant();
            return s.Length != 0 && (
                entry.Name.ToLowerInvariant().Contains(s) ||
                entry.MakerName.ToLowerInvariant().Contains(s) ||
                entry.Id.ToString("X4").ToLowerInvariant().Contains(s));
        }

        /// <summary>Shows the friendly colour description when an entry is picked.</summary>
        private static string? PaintTextSelector(string? text, object? item)
            => item is PaintEntry entry ? entry.ToString() : text;

        private void TorqueSplitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(TorqueSplitTextBox.Text, out var torqueValue))
            {
                torqueValue = 0.0;
            }
            TextBox5.Text = (100.0 - torqueValue).ToString(CultureInfo.InvariantCulture);
        }

        private async System.Threading.Tasks.Task ProcessData()
        {
            if (!File.Exists(TextBox1.Text))
            {
                await ShowMessageBox("GT5.0 file doesn't exist!");
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBox2.Text))
            {
                await ShowMessageBox("You left the PSN name empty! We need this to load up your data!");
                return;
            }

            Gt5Save = LoadData.Load(TextBox1.Text);

            var psnUserCharacterArray = TextBox2.Text.ToCharArray();
            var psnUserCharacterByteArray = psnUserCharacterArray.Select(c => (byte)c).ToArray();

            Moff = LoadData.FindSequence(Gt5Save, psnUserCharacterByteArray) - 306;
            if (Moff < 1)
            {
                await ShowMessageBox("PSN name is incorrect or data is corrupt\nRemember, the PSN name is CASE sensitive.");
                return;
            }

            // Grab current car's name from PARAM.SFO
            try
            {
                var gt50File = new FileInfo(TextBox1.Text);
                var paramSfoBytes = File.ReadAllBytes(Path.Combine(gt50File.DirectoryName, "PARAM.SFO"));
                var currentCar = "Current Car: ";
                var currentCarBytes = currentCar.ToCharArray().Select(c => (byte)c).ToArray();
                var currentCarIndex = LoadData.FindSequence(paramSfoBytes, currentCarBytes) + currentCarBytes.Length;

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
                await ShowMessageBox($"Can't get param.sfo for loading the current car's name.\n{ex.Message}");
            }

            EngineCodeComboBox.Text = Gt5Save[Moff - 213].ToString("X2") + " " + Gt5Save[Moff - 212].ToString("X2");
            TorqueSplitTextBox.Text = Gt5Save[Moff - 46].ToString();
            DrivetrainCodeComboBox.Text = Gt5Save[Moff - 209].ToString("X2") + " " + Gt5Save[Moff - 208].ToString("X2");
            ChassisCodeComboBox.Text = Gt5Save[Moff - 217].ToString("X2") + " " + Gt5Save[Moff - 216].ToString("X2");
            TransmissionCodeComboBox.Text = Gt5Save[Moff - 205].ToString("X2") + " " + Gt5Save[Moff - 204].ToString("X2");
            RemoveSpoilerCodeTextBox.Text = Gt5Save[Moff - 88].ToString();

            // Paint is stored as a single big-endian 32-bit value: (body colour ID << 13) | wheel colour ID,
            // where each ID indexes the game's internal paint database (0x1FFF = unset/default).
            var paintValue = ByteUtils.ConvertBytesToUnsignedInt(new[] { Gt5Save[Moff - 344], Gt5Save[Moff - 343], Gt5Save[Moff - 342], Gt5Save[Moff - 341] }) & 0x03FFFFFF;
            _syncingPaintFields = true;
            BodyPaintTextBox.Text = (paintValue >> 13).ToString("X4");
            WheelsPaintTextBox.Text = (paintValue & 0x1FFF).ToString("X4");
            BodyPaintAutoCompleteBox.Text = PaintDatabase.Find(paintValue >> 13)?.ToString() ?? "";
            WheelsPaintAutoCompleteBox.Text = PaintDatabase.Find(paintValue & 0x1FFF)?.ToString() ?? "";
            _syncingPaintFields = false;

            TurboModifierTextBox.Text = $"{Gt5Save[Moff - 171]:X2} {Gt5Save[Moff - 170]:X2} {Gt5Save[Moff - 169]:X2} {Gt5Save[Moff - 168]:X2}";
            HorsepowerMultiplierText.Text = Gt5Save[Moff + 1].ToString();

            AeroFrontTextBox.Text = Gt5Save[Moff - 43].ToString();
            AeroRearTextBox.Text = Gt5Save[Moff - 42].ToString();

            var suspensionHeightFront = ByteUtils.ConvertBytesToUnsignedInt(new[] { Gt5Save[Moff - 33], Gt5Save[Moff - 32] });
            SuspensionHeightFrontTextBox.Text = suspensionHeightFront.ToString();
            var suspensionHeightRear = ByteUtils.ConvertBytesToUnsignedInt(new[] { Gt5Save[Moff - 31], Gt5Save[Moff - 30] });
            SuspensionHeightRearTextBox.Text = suspensionHeightRear.ToString();

            GripTextBox.Text = Gt5Save[Moff + 10].ToString();

            // Driven kilometres of the current car. This is the Odometer field of the car's
            // MCarParameter condition block (MCarParameter base = Moff - 396, so the odometer
            // sits at Moff - 384). It is a big-endian 32-bit unsigned value stored in metres,
            // i.e. km = raw / 1000.
            var odometerMetres = ByteUtils.ConvertBytesToUnsignedInt(new[]
                { Gt5Save[Moff - 384], Gt5Save[Moff - 383], Gt5Save[Moff - 382], Gt5Save[Moff - 381] });
            OdometerTextBox.Text = (odometerMetres / 1000.0).ToString("0.0", CultureInfo.InvariantCulture);

            SpringRateFrontTextBox.Text = Gt5Save[Moff - 27].ToString();
            SpringRateRearTextBox.Text = Gt5Save[Moff - 26].ToString();

            ExhauseMultiplierTextBox.Text = $"{Gt5Save[Moff - 155]:X2} {Gt5Save[Moff - 154]:X2} {Gt5Save[Moff - 153]:X2} {Gt5Save[Moff - 152]:X2}";
            BodyCodeComboBox.Text = $"{Gt5Save[Moff - 262]:X2} {Gt5Save[Moff - 261]:X2}";
            SuspensionCodeComboBox.Text = $"{Gt5Save[Moff - 201]:X2} {Gt5Save[Moff - 200]:X2}";
            LsdCodeComboBox.Text = $"{Gt5Save[Moff - 197]:X2} {Gt5Save[Moff - 196]:X2}";
            HornCodeComboBox.Text = $"{Gt5Save[Moff + 23]:X2} {Gt5Save[Moff + 24]:X2}";

            WeightMultiplierTextBox.Text = ByteUtils.ConvertBytesToUnsignedInt(new Byte[]
                { Gt5Save[Moff - 191], Gt5Save[Moff - 190], Gt5Save[Moff - 189], Gt5Save[Moff - 188] }).ToString();
        }

        private async System.Threading.Tasks.Task SaveData()
        {
            try
            {
                var engineByteValues = ByteUtils.HexStringToByteArray(ResolvePartHex(EngineCodeComboBox, p => p.Engine));
                Gt5Save[Moff - 213] = engineByteValues[0];
                Gt5Save[Moff - 212] = engineByteValues[1];
            }
            catch (Exception ex)
            {
                await ShowMessageBox($"Can't save engine code to the save file. {ex.Message}");
                return;
            }

            try
            {
                if (!int.TryParse(TorqueSplitTextBox.Text, out var value))
                    throw new FormatException("Torque split value must be a number.");

                if (value < 0 || value > 255)
                    throw new FormatException("Torque split value must be between 0 and 255.");

                Gt5Save[Moff - 46] = (byte)value;
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save Torque split to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var drivetrainByteValues = ByteUtils.HexStringToByteArray(ResolvePartHex(DrivetrainCodeComboBox, p => p.Drivetrain));
                Gt5Save[Moff - 209] = drivetrainByteValues[0];
                Gt5Save[Moff - 208] = drivetrainByteValues[1];
            }
            catch (Exception ex)
            {
                await ShowMessageBox($"Can't save drivetrain code to the save file.\n{ex.Message}");
                return;
            }

            try
            {
                var chassisByteValues = ByteUtils.HexStringToByteArray(ResolvePartHex(ChassisCodeComboBox, p => p.Chassis));
                Gt5Save[Moff - 217] = chassisByteValues[0];
                Gt5Save[Moff - 216] = chassisByteValues[1];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save chassis code to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var transmissionByteValues = ByteUtils.HexStringToByteArray(ResolvePartHex(TransmissionCodeComboBox, p => p.Transmission));
                Gt5Save[Moff - 205] = transmissionByteValues[0];
                Gt5Save[Moff - 204] = transmissionByteValues[1];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save transmission code to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!int.TryParse(RemoveSpoilerCodeTextBox.Text, out var value))
                    throw new FormatException("Remove spoiler value must be a number.");

                if (value < 0 || value > 255)
                    throw new FormatException("Remove spoiler value must be between 0 and 255.");

                Gt5Save[Moff - 88] = (byte)value;
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save remove spoiler to the save file.\n{e.Message}");
                return;
            }

            try
            {
                // Paint is stored as a single 32-bit value: (body colour ID << 13) | wheel colour ID.
                // Each ID indexes the game's internal colour database and must be 0x1FFF or below
                // (0x1FFF = unset/default). Community paint codes such as 018BBFFF are exactly this
                // format: body ID = code >> 13 (C5D), wheel ID = code & 1FFF (1FFF).
                var bodyPaintId = ByteUtils.HexStringToUint(BodyPaintTextBox.Text);
                var wheelsPaintId = ByteUtils.HexStringToUint(WheelsPaintTextBox.Text);

                if (bodyPaintId > 0x1FFF)
                    throw new FormatException("Body paint ID must be between 0 and 1FFF. Tip: a full community paint code (e.g. 018BBFFF) packs both IDs - body = code >> 13 (C5D), wheels = code & 1FFF (1FFF).");

                if (wheelsPaintId > 0x1FFF)
                    throw new FormatException("Wheels paint ID must be between 0 and 1FFF.");

                var paintBytes = ByteUtils.UintToByteArray((bodyPaintId << 13) | wheelsPaintId);
                Gt5Save[Moff - 344] = paintBytes[0];
                Gt5Save[Moff - 343] = paintBytes[1];
                Gt5Save[Moff - 342] = paintBytes[2];
                Gt5Save[Moff - 341] = paintBytes[3];
            }
            catch (Exception ex)
            {
                await ShowMessageBox($"Can't save paint codes to the save file.\n{ex.Message}");
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
                await ShowMessageBox($"Can't save turbo modifier to the save file.\n{e.Message}");
                return;
            }

            if (SambaLikePerformanceCheckbox.IsChecked == true)
            {
                Gt5Save[Moff - 332] = 56; Gt5Save[Moff - 331] = 9; Gt5Save[Moff - 330] = 8;
                Gt5Save[Moff - 315] = 80; Gt5Save[Moff - 314] = 9;
                Gt5Save[Moff - 179] = 0; Gt5Save[Moff - 178] = 0; Gt5Save[Moff - 177] = 5; Gt5Save[Moff - 176] = 81;
                Gt5Save[Moff - 175] = 0; Gt5Save[Moff - 174] = 0; Gt5Save[Moff - 173] = 14; Gt5Save[Moff - 172] = 242;
                Gt5Save[Moff - 171] = 0; Gt5Save[Moff - 170] = 0; Gt5Save[Moff - 169] = 21; Gt5Save[Moff - 168] = 39;
                Gt5Save[Moff - 155] = 0; Gt5Save[Moff - 154] = 0; Gt5Save[Moff - 153] = 20; Gt5Save[Moff - 152] = 60;
                Gt5Save[Moff - 131] = 0; Gt5Save[Moff - 130] = 0; Gt5Save[Moff - 129] = 3; Gt5Save[Moff - 128] = 88;
                Gt5Save[Moff - 127] = 0; Gt5Save[Moff - 126] = 0; Gt5Save[Moff - 125] = 3; Gt5Save[Moff - 124] = 88;
                Gt5Save[Moff - 123] = 0; Gt5Save[Moff - 122] = 0; Gt5Save[Moff - 121] = 3; Gt5Save[Moff - 120] = 56;
                Gt5Save[Moff - 119] = 0; Gt5Save[Moff - 118] = 0; Gt5Save[Moff - 117] = 3; Gt5Save[Moff - 116] = 61;
            }

            if (!byte.TryParse(HorsepowerMultiplierText.Text, out var horsepowerMultiplier))
                throw new FormatException("Horsepower multiplier value must be a byte value (0-255).");
            Gt5Save[Moff + 1] = horsepowerMultiplier;

            try
            {
                if (!byte.TryParse(AeroFrontTextBox.Text, out var AeroFront))
                    throw new FormatException("Aero front value must be a byte value (0-255).");
                Gt5Save[Moff - 43] = AeroFront;
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save aero front to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!byte.TryParse(AeroRearTextBox.Text, out var AeroRear))
                    throw new FormatException("Aero rear value must be a byte value (0-255).");
                Gt5Save[Moff - 42] = AeroRear;
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save aero rear to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!ushort.TryParse(SuspensionHeightFrontTextBox.Text, out var suspensionHeightFront))
                    throw new FormatException("Suspension height front value must be a number.");
                var suspensionHeightFrontBytes = ByteUtils.UshortToByteArray(suspensionHeightFront);
                Gt5Save[Moff - 33] = suspensionHeightFrontBytes[0];
                Gt5Save[Moff - 32] = suspensionHeightFrontBytes[1];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save suspension height front to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!ushort.TryParse(SuspensionHeightRearTextBox.Text, out var suspensionHeightRear))
                    throw new FormatException("Suspension height rear value must be a number.");
                var suspensionHeightRearBytes = ByteUtils.UshortToByteArray(suspensionHeightRear);
                Gt5Save[Moff - 31] = suspensionHeightRearBytes[0];
                Gt5Save[Moff - 30] = suspensionHeightRearBytes[1];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save suspension height rear to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!byte.TryParse(GripTextBox.Text, out var grip))
                    throw new FormatException("Grip value must be a byte value (0-255).");
                Gt5Save[Moff + 10] = grip;
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save grip to the save file.\n{e.Message}");
                return;
            }

            try
            {
                // Driven kilometres (odometer) - big-endian 32-bit value at Moff - 384, stored in metres.
                if (!double.TryParse(OdometerTextBox.Text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var odometerKm))
                    throw new FormatException("Driven km must be a number.");

                if (odometerKm < 0 || odometerKm > 4294967.295)
                    throw new FormatException("Driven km must be between 0 and 4,294,967 km.");

                var odometerBytes = ByteUtils.UintToByteArray((uint)Math.Round(odometerKm * 1000.0));
                Gt5Save[Moff - 384] = odometerBytes[0];
                Gt5Save[Moff - 383] = odometerBytes[1];
                Gt5Save[Moff - 382] = odometerBytes[2];
                Gt5Save[Moff - 381] = odometerBytes[3];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save driven km to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!byte.TryParse(SpringRateFrontTextBox.Text, out var springRateFront))
                    throw new FormatException("Spring rate front value must be a byte value (0-255).");
                Gt5Save[Moff - 27] = springRateFront;
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save spring rate front to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (!byte.TryParse(SpringRateRearTextBox.Text, out var springRateRear))
                    throw new FormatException("Spring rate front value must be a byte value (0-255).");
                Gt5Save[Moff - 26] = springRateRear;
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save spring rate rear to the save file.\n{e.Message}");
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
                await ShowMessageBox($"Can't save exhause multiplier to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var carBodyCode = ByteUtils.HexStringToByteArray(ResolvePartHex(BodyCodeComboBox, p => p.Body));
                Gt5Save[Moff - 262] = carBodyCode[0];
                Gt5Save[Moff - 261] = carBodyCode[1];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save car body code to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var suspensionCode = ByteUtils.HexStringToByteArray(ResolvePartHex(SuspensionCodeComboBox, p => p.Suspension));
                Gt5Save[Moff - 201] = suspensionCode[0];
                Gt5Save[Moff - 200] = suspensionCode[1];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save suspension code to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var lsdCode = ByteUtils.HexStringToByteArray(ResolvePartHex(LsdCodeComboBox, p => p.Lsd));
                Gt5Save[Moff - 197] = lsdCode[0];
                Gt5Save[Moff - 196] = lsdCode[1];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save LSD code to the save file.\n{e.Message}");
                return;
            }

            try
            {
                var hornCode = ByteUtils.HexStringToByteArray(ResolvePartHex(HornCodeComboBox, p => p.Horn));
                Gt5Save[Moff + 23] = hornCode[0];
                Gt5Save[Moff + 24] = hornCode[1];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save Horn code to the save file.\n{e.Message}");
                return;
            }

            try
            {
                if (uint.TryParse(WeightMultiplierTextBox.Text, out var weightMultiplierInt))
                {
                    var weightMultiplierBytes = ByteUtils.UintToByteArray(weightMultiplierInt);
                    Gt5Save[Moff - 191] = weightMultiplierBytes[0];
                    Gt5Save[Moff - 190] = weightMultiplierBytes[1];
                    Gt5Save[Moff - 189] = weightMultiplierBytes[2];
                    Gt5Save[Moff - 188] = weightMultiplierBytes[3];
                }
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save weight multiplier to the save file.\n{e.Message}");
                return;
            }

            if (Add74ByteCheckBox.IsChecked == true) Gt5Save[Moff - 260] = 116;

            if (AddLucaBytesCheckBox.IsChecked == true)
            {
                Gt5Save[Moff - 260] = 76;
                Gt5Save[Moff - 259] = 85;
                Gt5Save[Moff - 258] = 67;
                Gt5Save[Moff - 257] = 65;
            }

            if (RemoveHoodCheckBox.IsChecked == true)
            {
                Gt5Save[Moff - 313] = byte.MaxValue; Gt5Save[Moff - 107] = 0; Gt5Save[Moff - 106] = 52;
                Gt5Save[Moff - 105] = 0; Gt5Save[Moff - 104] = 3; Gt5Save[Moff - 311] = 0;
                Gt5Save[Moff - 83] = byte.MaxValue; Gt5Save[Moff - 82] = byte.MaxValue;
                Gt5Save[Moff - 81] = byte.MaxValue; Gt5Save[Moff - 80] = byte.MaxValue;
            }

            if (RemoveFrontBumperCheckBox.IsChecked == true)
            {
                Gt5Save[Moff - 367] = 0; Gt5Save[Moff - 103] = 0; Gt5Save[Moff - 102] = 52;
                Gt5Save[Moff - 101] = 0; Gt5Save[Moff - 100] = 2; Gt5Save[Moff - 311] = 0;
                Gt5Save[Moff - 83] = byte.MaxValue; Gt5Save[Moff - 82] = byte.MaxValue;
                Gt5Save[Moff - 81] = byte.MaxValue; Gt5Save[Moff - 80] = byte.MaxValue;
            }

            if (RemoveRearBumperCheckBox.IsChecked == true)
            {
                Gt5Save[Moff - 367] = 0; Gt5Save[Moff - 99] = 0; Gt5Save[Moff - 98] = 52;
                Gt5Save[Moff - 97] = 0; Gt5Save[Moff - 96] = 2; Gt5Save[Moff - 311] = 0;
                Gt5Save[Moff - 83] = byte.MaxValue; Gt5Save[Moff - 82] = byte.MaxValue;
                Gt5Save[Moff - 81] = byte.MaxValue; Gt5Save[Moff - 80] = byte.MaxValue;
            }

            File.WriteAllBytes(TextBox1.Text, Gt5Save);
        }

        // Wires up the parts-database ComboBoxes. Each one is now the single source of truth for its
        // car-part code: pick a saved car from the drop-down to load that car's value, or type a hex
        // code directly. The small label beside each box always shows the resolved hex.
        private void InitializePartComboBoxes()
        {
            WirePartComboBox(EngineCodeComboBox, EngineHexLabel, p => p.Engine);
            WirePartComboBox(DrivetrainCodeComboBox, DrivetrainHexLabel, p => p.Drivetrain);
            WirePartComboBox(ChassisCodeComboBox, ChassisHexLabel, p => p.Chassis);
            WirePartComboBox(TransmissionCodeComboBox, TransmissionHexLabel, p => p.Transmission);
            WirePartComboBox(SuspensionCodeComboBox, SuspensionHexLabel, p => p.Suspension);
            WirePartComboBox(BodyCodeComboBox, BodyHexLabel, p => p.Body);
            WirePartComboBox(LsdCodeComboBox, LsdHexLabel, p => p.Lsd);
            WirePartComboBox(HornCodeComboBox, HornHexLabel, p => p.Horn);
        }

        private static void WirePartComboBox(ComboBox comboBox, TextBlock hexLabel, Func<CarParts, ushort> selector)
        {
            // Picking a car sets Text to that car's name, and typing changes Text directly, so
            // watching Text covers both. ResolvePartHex turns either one into the hex to display.
            comboBox.PropertyChanged += (_, e) =>
            {
                if (e.Property == ComboBox.TextProperty)
                    hexLabel.Text = ResolvePartHex(comboBox, selector);
            };
        }

        /// <summary>
        /// Resolves the hex code a parts ComboBox currently represents: the chosen car's value when
        /// an entry is selected, otherwise the raw text the user typed. If the box is still showing
        /// a car name (e.g. just after the drop-down list was reloaded and the selection cleared)
        /// that name is looked up again so the correct code is still used.
        /// </summary>
        private static string ResolvePartHex(ComboBox comboBox, Func<CarParts, ushort> selector)
        {
            if (comboBox.SelectedItem is CarParts selected)
                return ByteUtils.UshortToHexString(selector(selected));

            var text = comboBox.Text;
            if (!string.IsNullOrWhiteSpace(text) && comboBox.ItemsSource is IEnumerable<CarParts> parts)
            {
                var named = parts.FirstOrDefault(p => p.Name.Equals(text, StringComparison.OrdinalIgnoreCase));
                if (named != null)
                    return ByteUtils.UshortToHexString(selector(named));
            }

            return text ?? string.Empty;
        }

        private void LoadParts()
        {
            var sortedList = _CarPartsList?.OrderBy(cp => cp.Name).ToList() ?? new List<CarParts>();

            foreach (var comboBox in new[] { EngineCodeComboBox, DrivetrainCodeComboBox, ChassisCodeComboBox,
                TransmissionCodeComboBox, SuspensionCodeComboBox, BodyCodeComboBox, LsdCodeComboBox, HornCodeComboBox })
            {
                try
                {
                    // Refreshing ItemsSource updates the drop-down list while leaving whatever value
                    // is currently in the box untouched.
                    comboBox.ItemsSource = sortedList;
                }
                catch (Exception ex)
                {
                    _ = ShowMessageBox($"An issue occurred while loading the parts database: {ex.Message}");
                }
            }
        }

        private async void Button12_Click(object sender, RoutedEventArgs e)
        {
            var carName = await InputDialog.Show(this, "Enter car name:", "Add Car to Database", _CarName ?? "");

            if (string.IsNullOrWhiteSpace(carName))
            {
                return; // User cancelled or entered empty name
            }

            try
            {
                var newCarParts = new CarParts
                {
                    Name = carName,
                    Engine = ByteUtils.HexStringToUshort(ResolvePartHex(EngineCodeComboBox, p => p.Engine)),
                    Drivetrain = ByteUtils.HexStringToUshort(ResolvePartHex(DrivetrainCodeComboBox, p => p.Drivetrain)),
                    Chassis = ByteUtils.HexStringToUshort(ResolvePartHex(ChassisCodeComboBox, p => p.Chassis)),
                    Transmission = ByteUtils.HexStringToUshort(ResolvePartHex(TransmissionCodeComboBox, p => p.Transmission)),
                    Body = ByteUtils.HexStringToUshort(ResolvePartHex(BodyCodeComboBox, p => p.Body)),
                    Suspension = ByteUtils.HexStringToUshort(ResolvePartHex(SuspensionCodeComboBox, p => p.Suspension)),
                    Lsd = ByteUtils.HexStringToUshort(ResolvePartHex(LsdCodeComboBox, p => p.Lsd)),
                    Horn = ByteUtils.HexStringToUshort(ResolvePartHex(HornCodeComboBox, p => p.Horn))
                };

                if (_CarPartsList.Any(cp => cp.Name.Equals(carName, StringComparison.OrdinalIgnoreCase)))
                {
                    await ShowMessageBox("Car already exists");
                    return;
                }

                _CarPartsList.Add(newCarParts);
                SettingsFileClass.SaveCarParts(_CarPartsList, PARTS_DATABASE_FILENAME);
                LoadParts();
                await ShowMessageBox($"Successfully added {carName} to the database");
            }
            catch (Exception ex)
            {
                await ShowMessageBox($"Error adding car to database: {ex.Message}");
            }
        }

        private async void Button13_Click(object sender, RoutedEventArgs e)
        {
            var customPerformanceWindow = new CustomPerformanceWindow(_formManager);
            await customPerformanceWindow.ShowDialog(this);
        }

        private async void Button6_Click(object sender, RoutedEventArgs e)
        {
            TorqueSplitTextBox.Text = "30";
            DrivetrainCodeComboBox.Text = "0C E2";
        }

        private async void Button4_Click(object sender, RoutedEventArgs e)
        {
            Gt5Save[Moff - 355] = 0; Gt5Save[Moff - 333] = 88; Gt5Save[Moff - 211] = 0; Gt5Save[Moff - 210] = 0;
            Gt5Save[Moff - 209] = 10; Gt5Save[Moff - 208] = 84;
            await ShowMessageBox("Torque split editor installed, don't forget to save");
        }

        private async void Button7_Click(object sender, RoutedEventArgs e)
        {
            await ShowMessageBox("This is your current car's engine code. Copy the hex value from the Engine box (or read it off the label beside it).\n\nIf you paste your copied engine values back in while editing another car, that car will have the engine of the car you copied it from.");
        }

        private async void Button8_Click(object sender, RoutedEventArgs e)
        {
            await ShowMessageBox("This directly controls the torque that is sent to the front wheels, EG: if you typed in 10 the torque split would then be 10:90, if you typed in 50 the split would be 50:50\nYou could be really tricky and type 101, then the split would be 101:-1, which I reccomend in combination with added grip for high HP cars");
        }

        private async void Button9_Click(object sender, RoutedEventArgs e)
        {
            await ShowMessageBox("This is what gives hackers all the freedom to swap chassis, engine etc. in update 2.14.\nUntick this and you will find that most hacks won't work");
        }

        private async void Button10_Click(object sender, RoutedEventArgs e)
        {
            await ShowMessageBox("Make sure the highest stage spoiler in GT Auto is installed. EG Type B or Type C (which ever one is the highest available).\nThen increase the value here by 1. EG: 2 to 3 or 3 to 4 etc.");
        }

        private async void Button11_Click(object sender, RoutedEventArgs e)
        {
            var transmissionEditorWindow = new TransmissionEditorWindow(_formManager);
            await transmissionEditorWindow.ShowDialog(this);
        }

        private async void Button14_Click(object sender, RoutedEventArgs e)
        {
            await ShowMessageBox("This will override the 74 byte checkbox\nGT5 Editor 1.6 uses this method instead of the 74 byte, I have no idea whether it's more effective. So I've added in this function for testing purposes.");
        }

        private async void Button15_Click(object sender, RoutedEventArgs e)
        {
            Gt5Save[Moff + 248] = byte.MaxValue;
            await ShowMessageBox("The car is now yours, you can now either hack it, or click encrypt and save then return the data to the PS3");
            await SaveData();
        }

        private async void Button16_Click(object sender, RoutedEventArgs e)
        {
            await ShowMessageBox("This is the performance multiplier of the exhaust and turbo. Increasing these will increase the effectiveness of these performance parts on the engines performance");
        }

        private async void Button17_Click(object sender, RoutedEventArgs e)
        {
            await ShowMessageBox("Samba like performance installs the performance parts out of a samba bus onto your current car.\nThe samba bus parts increase the engines performance a lot more than the performance parts for most other cars.\nEG: a 300hp (When stock) engine with samba bus parts installed can increase to about 1,000 - 1,800hp");
        }

        private async void Button18_Click(object sender, RoutedEventArgs e)
        {
            Gt5Save[Moff + 2282] = byte.MaxValue; Gt5Save[Moff + 2283] = byte.MaxValue;
            Gt5Save[Moff + 2284] = byte.MaxValue; Gt5Save[Moff + 2285] = byte.MaxValue;
            await ShowMessageBox("You now have 4,294,967,295 cr");
            await SaveData();
        }

        private async void Button19_Click(object sender, RoutedEventArgs e)
        {
            Gt5Save[Moff - 25] = 0;
            Gt5Save[Moff - 24] = 0;
        }

        private async void Button20_Click(object sender, RoutedEventArgs e)
        {
            Gt5Save[Moff - 355] = 0;
            Gt5Save[Moff - 332] = 2;
        }

        private async void Button21_Click(object sender, RoutedEventArgs e)
        {
            Gt5Save[Moff - 355] = 1;
            Gt5Save[Moff - 332] = 0;
        }

        private async void Button22_Click(object sender, RoutedEventArgs e)
        {
            await ShowMessageBox("-----------------GT5 car hack workshop-----------------\nCreated by HeylonNHP\nSpecial thanks to:\nflatz for the pfdtool\naldotools.org for games.conf/global.conf\nTo the guys at http://gt5dragracing.com/ for daring to beta test my first version");
        }

        private async void Button23_Click(object sender, RoutedEventArgs e)
        {
            await ShowMessageBox("When you tick this checkbox, you must buy the Rigidity Improvement right before doing anything after loading the hacked save, the Rigidity Improvement is found under Body/Chassis in the tuning shop. The hack wont take proper effect without buying this upgrade.\n\nIf you have already purchased this upgrade, it will be uninstalled when you apply this hack.");
        }

        private async void Button24_Click(object sender, RoutedEventArgs e)
        {
            await ShowMessageBox("This is the total distance the current car has been driven (its odometer).\nEnter a value in kilometres, e.g. 12345.6.\n\nSet it to 0 for a brand-new car. Note: engine/body wear and the 'needs oil' warning are stored in separate fields and are not reset by this; use 'Set good oil' if you also want the car to look freshly serviced.");
        }

        private async void BadOilBtn_Click(object sender, RoutedEventArgs e)
        {
            var bytes = ByteUtils.UintToByteArray(1251513984);
            Gt5Save[Moff - 376] = bytes[0]; Gt5Save[Moff - 375] = bytes[1];
            Gt5Save[Moff - 374] = bytes[2]; Gt5Save[Moff - 373] = bytes[3];
        }

        private async void GoodOilBtn_Click(object sender, RoutedEventArgs e)
        {
            var bytes = ByteUtils.UintToByteArray(3365043200);
            Gt5Save[Moff - 376] = bytes[0]; Gt5Save[Moff - 375] = bytes[1];
            Gt5Save[Moff - 374] = bytes[2]; Gt5Save[Moff - 373] = bytes[3];
        }

        private async System.Threading.Tasks.Task ShowMessageBox(string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("GT5 Car hack workshop", message, ButtonEnum.Ok);
            await box.ShowAsync();
        }
    }
}
