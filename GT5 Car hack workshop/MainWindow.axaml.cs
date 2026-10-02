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
            BodyPaintAutoCompleteBox.ItemFilter = PaintItemFilter;
            WheelsPaintAutoCompleteBox.ItemFilter = PaintItemFilter;
            // Deliberately no TextSelector: Avalonia hands a text selector the already-formatted
            // string rather than the entry, and the default formatting already shows the colour's
            // friendly description (PaintEntry.ToString()).

            // Finish filter: narrows each picker to one finish (Metallic, Chrome, ...).
            BodyFinishComboBox.ItemsSource = PaintDatabase.Finishes;
            WheelsFinishComboBox.ItemsSource = PaintDatabase.Finishes;
            BodyFinishComboBox.SelectedIndex = 0;
            WheelsFinishComboBox.SelectedIndex = 0;
            RefreshPaintFinish(BodyPaintAutoCompleteBox, BodyFinishComboBox);
            RefreshPaintFinish(WheelsPaintAutoCompleteBox, WheelsFinishComboBox);
        }

        private void BodyFinishComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => RefreshPaintFinish(BodyPaintAutoCompleteBox, BodyFinishComboBox);

        private void WheelsFinishComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => RefreshPaintFinish(WheelsPaintAutoCompleteBox, WheelsFinishComboBox);

        /// <summary>
        /// Points a paint picker at the colours of the finish chosen in its filter (every colour for
        /// "Any finish"). Replacing ItemsSource is what makes the drop-down, and its search, show
        /// only that finish.
        /// </summary>
        private static void RefreshPaintFinish(AutoCompleteBox searchBox, ComboBox finishBox)
            => searchBox.ItemsSource = PaintDatabase.ByFinish((finishBox.SelectedItem as PaintFinish)?.Category);

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var backupPath = Path.Combine(AppContext.BaseDirectory, "Backups");
            if (!Directory.Exists(backupPath)) Directory.CreateDirectory(backupPath);

            _ProgramSettings = SettingsFileClass.LoadSettings("GT5CHWsettings.ini", 1);
            _CarPartsList = PartsDatabaseStore.LoadAll();

            // Safely access settings with bounds checking
            if (_ProgramSettings != null && _ProgramSettings.Length > 0)
                TextBox1.Text = _ProgramSettings[0];

            LoadParts();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Save line by line, so settings this window does not own (the paint browser's sort
            // choice, on later lines) are left exactly as they are. Line 2 is no longer used: it
            // held the PSN name.
            SettingsFileClass.SaveSetting("GT5CHWsettings.ini", 0, TextBox1.Text ?? " ");
            PartsDatabaseStore.SaveAll(_CarPartsList);
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
        /// Shows a paint colour in one of the paint search boxes: the colour's description when the
        /// id is in the catalogue, otherwise the raw id, so the box always reflects what the car
        /// currently has rather than going blank. That matters because the "unset" marker
        /// (0x1FFF) has no catalogue entry, and it is what most cars store for their paint.
        /// </summary>
        private static void SetPaintSearchBox(AutoCompleteBox searchBox, uint id)
        {
            // Find returns the very instance held in the box's ItemsSource, which is what makes
            // this a genuine selection rather than just some text.
            var entry = PaintDatabase.Find(id);
            searchBox.SelectedItem = entry;
            searchBox.Text = entry?.ToString() ?? id.ToString("X4");
        }

        /// <summary>
        /// Keeps a paint search box in step with the authoritative hex field: shows the matching
        /// colour's description when the ID is known, or the raw id for custom/unknown values.
        /// </summary>
        private void SyncPaintSearchBox(AutoCompleteBox searchBox, string? text)
        {
            var id = uint.TryParse(text?.Replace(" ", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value)
                ? value
                : (uint?)null;

            _syncingPaintFields = true;
            if (id.HasValue)
            {
                SetPaintSearchBox(searchBox, id.Value);
            }
            else
            {
                searchBox.SelectedItem = null;
                searchBox.Text = "";
            }
            _syncingPaintFields = false;
        }

        /// <summary>Matches typed text against a colour's name, maker or hex ID.</summary>
        private static bool PaintItemFilter(string? search, object? item)
            => item is PaintEntry entry && PaintDatabase.MatchesSearch(entry, search);

        private async void BodyBrowseButton_Click(object? sender, RoutedEventArgs e)
            => await BrowseForColour(BodyPaintTextBox);

        private async void WheelsBrowseButton_Click(object? sender, RoutedEventArgs e)
            => await BrowseForColour(WheelsPaintTextBox);

        /// <summary>
        /// Opens the colour browser and, if a colour is chosen, puts it in the given hex field. The
        /// hex field is what SaveData writes and what the paint search box follows, so setting it
        /// keeps the box, the swatch and the save in step. The browser itself only returns a colour.
        /// </summary>
        private async System.Threading.Tasks.Task BrowseForColour(TextBox hexBox)
        {
            var chosen = await new PaintBrowserWindow().ShowDialog<PaintEntry?>(this);
            if (chosen is null) return;

            hexBox.Text = chosen.Id.ToString("X4");
        }

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
            PsnNameTextBox.Text = "";

            if (!File.Exists(TextBox1.Text))
            {
                await ShowMessageBox("GT5.0 file doesn't exist!");
                return;
            }

            Gt5Save = LoadData.Load(TextBox1.Text);

            // Find the current car from the save's own structure. No PSN name is needed (and it could
            // never be relied on anyway - it appears three times in every save).
            var anchor = SaveAnchor.Locate(Gt5Save);
            if (!anchor.Found)
            {
                await ShowMessageBox(
                    $"Couldn't find the current car's data in this save:\n{anchor.FailureReason}\n\n" +
                    "Nothing was changed, because writing at the wrong place would corrupt the save.");
                return;
            }

            Moff = anchor.Moff;

            // The player's name is part of the same record, so it can simply be read and shown.
            PsnNameTextBox.Text = SaveAnchor.ReadPlayerName(Gt5Save, Moff) ?? "";

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

            TorqueSplitTextBox.Text = Gt5Save[Moff - 46].ToString();
            RemoveSpoilerCodeTextBox.Text = Gt5Save[Moff - 88].ToString();
            LoadPartsFromSave();

            // Paint is stored as a single big-endian 32-bit value: (body colour ID << 13) | wheel colour ID,
            // where each ID indexes the game's internal paint database (0x1FFF = unset/default).
            var paintValue = ByteUtils.ConvertBytesToUnsignedInt(new[] { Gt5Save[Moff - 344], Gt5Save[Moff - 343], Gt5Save[Moff - 342], Gt5Save[Moff - 341] }) & 0x03FFFFFF;
            // Show the car's real paints, so clear any finish filter left over from browsing.
            BodyFinishComboBox.SelectedIndex = 0;
            WheelsFinishComboBox.SelectedIndex = 0;
            _syncingPaintFields = true;
            BodyPaintTextBox.Text = (paintValue >> 13).ToString("X4");
            WheelsPaintTextBox.Text = (paintValue & 0x1FFF).ToString("X4");
            SetPaintSearchBox(BodyPaintAutoCompleteBox, paintValue >> 13);
            SetPaintSearchBox(WheelsPaintAutoCompleteBox, paintValue & 0x1FFF);
            _syncingPaintFields = false;

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

            // Credits, a big-endian 32-bit value.
            var credits = ByteUtils.ConvertBytesToUnsignedInt(new[]
                { Gt5Save[Moff + 2282], Gt5Save[Moff + 2283], Gt5Save[Moff + 2284], Gt5Save[Moff + 2285] });
            CreditsTextBox.Text = credits.ToString(CultureInfo.InvariantCulture);

            SpringRateFrontTextBox.Text = Gt5Save[Moff - 27].ToString();
            SpringRateRearTextBox.Text = Gt5Save[Moff - 26].ToString();

            // Turbo/Exhaust/Weight are set by LoadPartsFromSave() via their parts-database drop-downs.
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
                var turboCode = ByteUtils.HexStringToByteArray(ResolvePartHex(TurboCodeComboBox, p => p.Turbo));
                Gt5Save[Moff - 169] = turboCode[0];
                Gt5Save[Moff - 168] = turboCode[1];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save turbo (turbine kit) code to the save file.\n{e.Message}");
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
                var exhaustCode = ByteUtils.HexStringToByteArray(ResolvePartHex(ExhaustCodeComboBox, p => p.Exhaust));
                Gt5Save[Moff - 153] = exhaustCode[0];
                Gt5Save[Moff - 152] = exhaustCode[1];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save exhaust (muffler) code to the save file.\n{e.Message}");
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
                var weightCode = ByteUtils.HexStringToByteArray(ResolvePartHex(WeightCodeComboBox, p => p.Weight));
                Gt5Save[Moff - 189] = weightCode[0];
                Gt5Save[Moff - 188] = weightCode[1];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save weight (lightweight) code to the save file.\n{e.Message}");
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
            WirePartComboBox(TurboCodeComboBox, TurboHexLabel, p => p.Turbo);
            WirePartComboBox(ExhaustCodeComboBox, ExhaustHexLabel, p => p.Exhaust);
            WirePartComboBox(WeightCodeComboBox, WeightHexLabel, p => p.Weight);
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
            // The box text is the value: it holds either a car name (chosen from the list) or a raw
            // hex code the user typed, and SetPartSelection keeps it in step with any selection.
            // Resolve from the text and fall back to the selected entry only when the box is empty,
            // so this no longer relies on Avalonia clearing SelectedItem when Text is set.
            var text = comboBox.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                // A car name in the box means "use that car's code".
                if (comboBox.ItemsSource is IEnumerable<CarParts> parts)
                {
                    var named = parts.FirstOrDefault(p => p.Name.Equals(text, StringComparison.OrdinalIgnoreCase));
                    if (named != null)
                        return ByteUtils.UshortToHexString(selector(named));
                }

                // Otherwise the box holds the code itself.
                return text;
            }

            // Empty box: fall back to whatever entry is selected (e.g. right after a list reload).
            if (comboBox.SelectedItem is CarParts selected)
                return ByteUtils.UshortToHexString(selector(selected));

            return string.Empty;
        }

        // Fills the parts drop-downs from the loaded save. When a code matches an existing database
        // entry (or the whole car matches one) the drop-down selects that entry so it shows the
        // saved name; otherwise the raw hex code is shown as free text.
        private void LoadPartsFromSave()
        {
            var engine = ByteUtils.BytesToUshort(Gt5Save[Moff - 213], Gt5Save[Moff - 212]);
            var drivetrain = ByteUtils.BytesToUshort(Gt5Save[Moff - 209], Gt5Save[Moff - 208]);
            var chassis = ByteUtils.BytesToUshort(Gt5Save[Moff - 217], Gt5Save[Moff - 216]);
            var transmission = ByteUtils.BytesToUshort(Gt5Save[Moff - 205], Gt5Save[Moff - 204]);
            var suspension = ByteUtils.BytesToUshort(Gt5Save[Moff - 201], Gt5Save[Moff - 200]);
            var body = ByteUtils.BytesToUshort(Gt5Save[Moff - 262], Gt5Save[Moff - 261]);
            var lsd = ByteUtils.BytesToUshort(Gt5Save[Moff - 197], Gt5Save[Moff - 196]);
            var horn = ByteUtils.BytesToUshort(Gt5Save[Moff + 23], Gt5Save[Moff + 24]);
            // Turbo = TurbineKit part id (low 2 bytes of the int32 at Moff-171),
            // Exhaust = Muffler (Moff-155), Weight = Lightweight (Moff-191).
            var turbo = ByteUtils.BytesToUshort(Gt5Save[Moff - 169], Gt5Save[Moff - 168]);
            var exhaust = ByteUtils.BytesToUshort(Gt5Save[Moff - 153], Gt5Save[Moff - 152]);
            var weight = ByteUtils.BytesToUshort(Gt5Save[Moff - 189], Gt5Save[Moff - 188]);

            // Prefer a single entry that matches the whole car so every drop-down agrees on it.
            var wholeMatch = _CarPartsList?.FirstOrDefault(p =>
                p.Engine == engine && p.Drivetrain == drivetrain && p.Chassis == chassis &&
                p.Transmission == transmission && p.Suspension == suspension && p.Body == body &&
                p.Lsd == lsd && p.Horn == horn &&
                p.Turbo == turbo && p.Exhaust == exhaust && p.Weight == weight);

            SetPartSelection(EngineCodeComboBox, engine, p => p.Engine, wholeMatch);
            SetPartSelection(DrivetrainCodeComboBox, drivetrain, p => p.Drivetrain, wholeMatch);
            SetPartSelection(ChassisCodeComboBox, chassis, p => p.Chassis, wholeMatch);
            SetPartSelection(TransmissionCodeComboBox, transmission, p => p.Transmission, wholeMatch);
            SetPartSelection(SuspensionCodeComboBox, suspension, p => p.Suspension, wholeMatch);
            SetPartSelection(BodyCodeComboBox, body, p => p.Body, wholeMatch);
            SetPartSelection(LsdCodeComboBox, lsd, p => p.Lsd, wholeMatch);
            SetPartSelection(HornCodeComboBox, horn, p => p.Horn, wholeMatch);
            SetPartSelection(TurboCodeComboBox, turbo, p => p.Turbo, wholeMatch);
            SetPartSelection(ExhaustCodeComboBox, exhaust, p => p.Exhaust, wholeMatch);
            SetPartSelection(WeightCodeComboBox, weight, p => p.Weight, wholeMatch);
        }

        /// <summary>
        /// Re-reads the part-id fields from the in-memory save and re-points the parts drop-downs
        /// at the matching values, so a child dialog that edited the save (e.g. Custom
        /// Performance) leaves the drop-downs showing what the car now actually has.
        /// </summary>
        public void RefreshPartSelections()
        {
            if (Gt5Save == null || Gt5Save.Length == 0) return;
            LoadPartsFromSave();
        }

        /// <summary>
        /// Points a parts ComboBox at the given value: selects <paramref name="preferred"/>, else the
        /// first database entry whose matching field equals the value, so the saved car's name is
        /// shown; if nothing matches, displays the raw hex code instead.
        /// </summary>
        private static void SetPartSelection(ComboBox comboBox, ushort value, Func<CarParts, ushort> selector, CarParts? preferred)
        {
            var match = preferred ?? (comboBox.ItemsSource as IEnumerable<CarParts>)?.FirstOrDefault(p => selector(p) == value);
            if (match != null)
            {
                comboBox.SelectedItem = match;
                comboBox.Text = match.Name; // keep the text (the authoritative value) in step
            }
            else
            {
                SetPartHexText(comboBox, ByteUtils.UshortToHexString(value));
            }
        }

        /// <summary>
        /// Shows a raw hex code in a parts combo box, clearing any car selection first so the code -
        /// not a previously chosen car - is what ResolvePartHex (and therefore the save) uses.
        /// </summary>
        private static void SetPartHexText(ComboBox comboBox, string hex)
        {
            comboBox.SelectedItem = null;
            comboBox.Text = hex;
        }

        private void LoadParts()
        {
            var sortedList = _CarPartsList?.OrderBy(cp => cp.Name).ToList() ?? new List<CarParts>();

            foreach (var comboBox in new[] { EngineCodeComboBox, DrivetrainCodeComboBox, ChassisCodeComboBox,
                TransmissionCodeComboBox, SuspensionCodeComboBox, BodyCodeComboBox, LsdCodeComboBox, HornCodeComboBox,
                TurboCodeComboBox, ExhaustCodeComboBox, WeightCodeComboBox })
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
                    Horn = ByteUtils.HexStringToUshort(ResolvePartHex(HornCodeComboBox, p => p.Horn)),
                    Turbo = ByteUtils.HexStringToUshort(ResolvePartHex(TurboCodeComboBox, p => p.Turbo)),
                    Exhaust = ByteUtils.HexStringToUshort(ResolvePartHex(ExhaustCodeComboBox, p => p.Exhaust)),
                    Weight = ByteUtils.HexStringToUshort(ResolvePartHex(WeightCodeComboBox, p => p.Weight))
                };

                if (_CarPartsList.Any(cp => cp.Name.Equals(carName, StringComparison.OrdinalIgnoreCase)))
                {
                    await ShowMessageBox("Car already exists");
                    return;
                }

                _CarPartsList.Add(newCarParts);
                PartsDatabaseStore.Upsert(newCarParts);
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
            // Clear any selected car first: a stale selection would otherwise shadow the code and
            // the 4WD hack would silently not be applied.
            SetPartHexText(DrivetrainCodeComboBox, "0C E2");
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

        private async void AddPaintChipsButton_Click(object sender, RoutedEventArgs e)
        {
            if (Gt5Save == null || Gt5Save.Length == 0)
            {
                await ShowMessageBox("Load a GT5.0 save before adding paint chips.");
                return;
            }

            var paintChipWindow = new PaintChipWindow(_formManager);
            await paintChipWindow.ShowDialog(this);
        }

        private async void Button15_Click(object sender, RoutedEventArgs e)
        {
            Gt5Save[Moff + 248] = byte.MaxValue;
            await ShowMessageBox("The car is now yours, you can now either hack it, or click encrypt and save then return the data to the PS3");
            await SaveData();
        }

        private async void Button17_Click(object sender, RoutedEventArgs e)
        {
            await ShowMessageBox("Samba like performance installs the performance parts out of a samba bus onto your current car.\nThe samba bus parts increase the engines performance a lot more than the performance parts for most other cars.\nEG: a 300hp (When stock) engine with samba bus parts installed can increase to about 1,000 - 1,800hp");
        }

        /// <summary>
        /// Credits are a big-endian 32-bit value. Whatever is typed here is written straight into the
        /// save in memory, like the other fields, so use Save only or Save and encrypt to persist it.
        /// </summary>
        private void CreditsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Gt5Save == null || Moff < 1) return;

            var text = (CreditsTextBox.Text ?? string.Empty).Replace(",", string.Empty).Replace(" ", string.Empty);
            if (!uint.TryParse(text, out var credits)) return;

            // Avalonia raises TextChanged when the box is filled in from a save too, so skip when the
            // save already holds this value rather than writing it back.
            var current = ByteUtils.ConvertBytesToUnsignedInt(new[]
                { Gt5Save[Moff + 2282], Gt5Save[Moff + 2283], Gt5Save[Moff + 2284], Gt5Save[Moff + 2285] });
            if (current == credits) return;

            Gt5Save[Moff + 2282] = (byte)(credits >> 24);
            Gt5Save[Moff + 2283] = (byte)(credits >> 16);
            Gt5Save[Moff + 2284] = (byte)(credits >> 8);
            Gt5Save[Moff + 2285] = (byte)credits;
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
