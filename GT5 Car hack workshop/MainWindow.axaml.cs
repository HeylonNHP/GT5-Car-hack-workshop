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

        // The last code each part drop-down stood for, so that a box a platform clears on losing
        // focus cannot blank a save (HexStringToUshort throws on empty input).
        private readonly Dictionary<AutoCompleteBox, ushort> _lastPartCodes = new();

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
            InitializePresetControls();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializePaintSearchBoxes();
            InitializePartComboBoxes();
            InitializePresetControls();
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
            // Every part field is written from PartCatalogue's list: the same two big-endian bytes at
            // the same offsets as the per-field blocks that used to sit here, so the 4WD hack and the
            // other byte-level hacks see exactly the bytes they saw before.
            foreach (var (category, box, _) in PartFieldBindings())
            {
                try
                {
                    var partBytes = ByteUtils.HexStringToByteArray(ResolvePartHex(box, category));
                    Gt5Save[Moff + category.SaveOffset] = partBytes[0];
                    Gt5Save[Moff + category.SaveOffset + 1] = partBytes[1];
                }
                catch (Exception ex)
                {
                    await ShowMessageBox($"Can't save {category.Label.ToLowerInvariant()} code to the save file.\n{ex.Message}");
                    return;
                }
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


            if (SambaLikePerformanceCheckbox.IsChecked == true)
            {
                Gt5Save[Moff - 332] = 56; Gt5Save[Moff - 331] = 9; Gt5Save[Moff - 330] = 8;
                Gt5Save[Moff - 315] = 80; Gt5Save[Moff - 314] = 9;
                Gt5Save[Moff - 179] = 0; Gt5Save[Moff - 178] = 0; Gt5Save[Moff - 177] = 5; Gt5Save[Moff - 176] = 81;
                Gt5Save[Moff - 175] = 0; Gt5Save[Moff - 174] = 0; Gt5Save[Moff - 173] = 14; Gt5Save[Moff - 172] = 242;
                Gt5Save[Moff - 171] = 0; Gt5Save[Moff - 170] = 0; Gt5Save[Moff - 169] = 21; Gt5Save[Moff - 168] = 39;
                // Moff-153/-152 (the muffler code) used to be set here too, but the exhaust field's own
                // write always ran afterwards and overwrote it, so the field keeps writing those bytes.
                Gt5Save[Moff - 155] = 0; Gt5Save[Moff - 154] = 0;
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
                var hornCode = ByteUtils.HexStringToByteArray(ResolvePartHex(HornCodeComboBox, p => p.Horn));
                Gt5Save[Moff + 23] = hornCode[0];
                Gt5Save[Moff + 24] = hornCode[1];
            }
            catch (Exception e)
            {
                await ShowMessageBox($"Can't save Horn code to the save file.\n{e.Message}");
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

        // ---- The generated parts catalogue drives every part field ------------------------------
        //
        // Everything below works from PartCatalogue.Categories: the combo-box wiring, the catalogue
        // load, reading a save and writing a save all walk that one list, so a part field is named in
        // exactly one place instead of in five. Body and Horn are deliberately not in the list: they
        // are not game part keys, so they keep the hand-built CarParts catalogue and their own boxes.

        /// <summary>
        /// The one place where the named controls meet <see cref="PartCatalogue.Categories"/>.
        /// </summary>
        private IEnumerable<(PartCategory Category, AutoCompleteBox Box, TextBlock HexLabel)> PartFieldBindings() => new[]
        {
            (PartCatalogue.Get(13), EngineCodeComboBox, EngineHexLabel),
            (PartCatalogue.Get(7), ChassisCodeComboBox, ChassisHexLabel),
            (PartCatalogue.Get(11), DrivetrainCodeComboBox, DrivetrainHexLabel),
            (PartCatalogue.Get(12), TransmissionCodeComboBox, TransmissionHexLabel),
            (PartCatalogue.Get(4), SuspensionCodeComboBox, SuspensionHexLabel),
            (PartCatalogue.Get(23), LsdCodeComboBox, LsdHexLabel),
            (PartCatalogue.Get(2), BrakeCodeComboBox, BrakeHexLabel),
            (PartCatalogue.Get(3), BrakeControllerCodeComboBox, BrakeControllerHexLabel),
            (PartCatalogue.Get(9), WeightCodeComboBox, WeightHexLabel),
            (PartCatalogue.Get(15), TurboCodeComboBox, TurboHexLabel),
            (PartCatalogue.Get(19), ExhaustCodeComboBox, ExhaustHexLabel),
            (PartCatalogue.Get(20), ClutchCodeComboBox, ClutchHexLabel),
            (PartCatalogue.Get(21), FlywheelCodeComboBox, FlywheelHexLabel),
            (PartCatalogue.Get(22), PropellerShaftCodeComboBox, PropellerShaftHexLabel),
            (PartCatalogue.Get(14), NatuneCodeComboBox, NatuneHexLabel),
            (PartCatalogue.Get(16), DisplacementCodeComboBox, DisplacementHexLabel),
            (PartCatalogue.Get(17), ComputerCodeComboBox, ComputerHexLabel),
            (PartCatalogue.Get(18), IntercoolerCodeComboBox, IntercoolerHexLabel),
            (PartCatalogue.Get(27), SuperchargerCodeComboBox, SuperchargerHexLabel),
            (PartCatalogue.Get(28), IntakeManifoldCodeComboBox, IntakeManifoldHexLabel),
            (PartCatalogue.Get(29), ExhaustManifoldCodeComboBox, ExhaustManifoldHexLabel),
            (PartCatalogue.Get(30), CatalystCodeComboBox, CatalystHexLabel),
            (PartCatalogue.Get(31), AirCleanerCodeComboBox, AirCleanerHexLabel),
            (PartCatalogue.Get(26), NosCodeComboBox, NosHexLabel),
        };

        private void InitializePartComboBoxes()
        {
            foreach (var (category, box, hexLabel) in PartFieldBindings())
                WirePartComboBox(box, hexLabel, category);
        }

        /// <summary>
        /// Turns a part box into a filtered picker. Its list is a whole catalogue category - every
        /// car's own variant of the part, thousands of entries for some - the typed text filters it,
        /// and the small label beside the box always shows the code the box currently stands for.
        /// </summary>
        private void WirePartComboBox(AutoCompleteBox box, TextBlock hexLabel, PartCategory category)
        {
            box.ItemFilter = PartItemFilter;

            // The box's text is the value: it holds either an entry's label (picked from the list) or
            // a raw hex code the user typed, exactly as the old editable combo box did.
            box.PropertyChanged += (_, e) =>
            {
                if (e.Property != AutoCompleteBox.TextProperty) return;
                RememberPartCode(box, category);
                hexLabel.Text = ResolvePartHex(box, category);
            };

            // A box whose text matches no item can be cleared when it loses focus. A blank box aborts
            // the whole save (HexStringToUshort throws on empty input), so put back the code the box
            // last stood for rather than leave the user with a silent failure.
            box.LostFocus += (_, _) =>
            {
                if (!string.IsNullOrWhiteSpace(box.Text)) return;
                if (!_lastPartCodes.TryGetValue(box, out var code)) return;

                box.Text = ByteUtils.UshortToHexString(code);
                hexLabel.Text = ResolvePartHex(box, category);
            };
        }

        /// <summary>Matches typed text against an entry's label, car, upgrade name or hex code.</summary>
        private static bool PartItemFilter(string? search, object? item)
            => item is PartEntry entry && PartCatalogueStore.MatchesSearch(entry, search);

        /// <summary>
        /// Remembers the code a box currently stands for, so a box that gets emptied (see above) can
        /// be put back instead of left blank.
        /// </summary>
        private void RememberPartCode(AutoCompleteBox box, PartCategory category)
        {
            var text = box.Text;
            if (string.IsNullOrWhiteSpace(text)) return;

            var entry = PartCatalogueStore.FindByLabel(category.TableId, text);
            if (entry != null)
            {
                _lastPartCodes[box] = entry.PartKey;
                return;
            }

            try
            {
                _lastPartCodes[box] = ByteUtils.HexStringToUshort(text);
            }
            catch (Exception)
            {
                // Neither a catalogue entry nor a code: remember nothing, so a genuine mistake still
                // surfaces as the save error it always was.
            }
        }

        /// <summary>
        /// Resolves the code a part box currently stands for: the catalogue entry whose label is the
        /// box's text, otherwise the raw text itself, which is a hex code (possibly one the user
        /// deliberately copied from another category). That is what keeps manual hex editing - and the
        /// 4WD hack - working.
        /// </summary>
        private string ResolvePartHex(AutoCompleteBox box, PartCategory category)
        {
            var text = box.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                var entry = PartCatalogueStore.FindByLabel(category.TableId, text);
                return entry?.Hex ?? text;
            }

            // Empty box: fall back to the last code it stood for, so a wiped box cannot blank a save.
            return _lastPartCodes.TryGetValue(box, out var remembered)
                ? ByteUtils.UshortToHexString(remembered)
                : string.Empty;
        }

        /// <summary>
        /// Points a part box at a code: selects the catalogue entry carrying it, preferring the car
        /// the save belongs to so the car's own variant is the one shown, and falls back to the raw
        /// hex code (exactly as the editor always did) when the catalogue has no such entry.
        /// </summary>
        private void SetPartSelection(AutoCompleteBox box, PartCategory category, ushort value, int preferredCarId)
        {
            var match = PartCatalogueStore.Find(category.TableId, value, preferredCarId);
            if (match != null)
            {
                box.SelectedItem = match;
                box.Text = match.Label; // keep the text (the authoritative value) in step
                return;
            }

            SetPartHexText(box, ByteUtils.UshortToHexString(value));
        }

        /// <summary>
        /// Shows a raw hex code in a part box, clearing any selection first so the code - not a
        /// previously chosen entry - is what ResolvePartHex (and therefore the save) uses.
        /// </summary>
        private static void SetPartHexText(AutoCompleteBox box, string hex)
        {
            box.SelectedItem = null;
            box.Text = hex;
        }

        /// <summary>The two bytes of a part field in the save, big-endian, at its declared offset.</summary>
        private ushort ReadPartValue(PartCategory category)
            => ByteUtils.BytesToUshort(Gt5Save[Moff + category.SaveOffset], Gt5Save[Moff + category.SaveOffset + 1]);

        // Fills the parts drop-downs from the loaded save. Each code is looked up in the catalogue so
        // the box shows the part's name and car; when the catalogue has no such entry the raw code is
        // shown as free text, which is what a hand-edited or unknown part needs.
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
            var brake = ByteUtils.BytesToUshort(Gt5Save[Moff -225], Gt5Save[Moff -224]);
            var brakeController = ByteUtils.BytesToUshort(Gt5Save[Moff -221], Gt5Save[Moff -220]);
            var displacement = ByteUtils.BytesToUshort(Gt5Save[Moff -181], Gt5Save[Moff -180]);
            var computer = ByteUtils.BytesToUshort(Gt5Save[Moff -177], Gt5Save[Moff -176]);
            var natune = ByteUtils.BytesToUshort(Gt5Save[Moff -173], Gt5Save[Moff -172]);
            var flywheel = ByteUtils.BytesToUshort(Gt5Save[Moff -165], Gt5Save[Moff -164]);
            var clutch = ByteUtils.BytesToUshort(Gt5Save[Moff -161], Gt5Save[Moff -160]);
            var propellerShaft = ByteUtils.BytesToUshort(Gt5Save[Moff -157], Gt5Save[Moff -156]);
            var intercooler = ByteUtils.BytesToUshort(Gt5Save[Moff -149], Gt5Save[Moff -148]);
            var supercharger = ByteUtils.BytesToUshort(Gt5Save[Moff -133], Gt5Save[Moff -132]);
            var intakeManifold = ByteUtils.BytesToUshort(Gt5Save[Moff -129], Gt5Save[Moff -128]);
            var exhaustManifold = ByteUtils.BytesToUshort(Gt5Save[Moff -125], Gt5Save[Moff -124]);
            var catalyst = ByteUtils.BytesToUshort(Gt5Save[Moff -121], Gt5Save[Moff -120]);
            var airCleaner = ByteUtils.BytesToUshort(Gt5Save[Moff -117], Gt5Save[Moff -116]);
            var nos = ByteUtils.BytesToUshort(Gt5Save[Moff -113], Gt5Save[Moff -112]);

            // Body and Horn keep their old behaviour: prefer a single CarParts entry that matches the
            // whole car, so both boxes agree on it, and show a raw code when nothing matches.
            var wholeMatch = _CarPartsList?.FirstOrDefault(p =>
                p.Engine == engine && p.Drivetrain == drivetrain && p.Chassis == chassis &&
                p.Transmission == transmission && p.Suspension == suspension && p.Body == body &&
                p.Lsd == lsd && p.Horn == horn &&
                p.Turbo == turbo && p.Exhaust == exhaust && p.Weight == weight &&
                p.Brake == brake &&
                p.BrakeController == brakeController &&
                p.Displacement == displacement &&
                p.Computer == computer &&
                p.Natune == natune &&
                p.Flywheel == flywheel &&
                p.Clutch == clutch &&
                p.PropellerShaft == propellerShaft &&
                p.Intercooler == intercooler &&
                p.Supercharger == supercharger &&
                p.IntakeManifold == intakeManifold &&
                p.ExhaustManifold == exhaustManifold &&
                p.Catalyst == catalyst &&
                p.AirCleaner == airCleaner &&
                p.Nos == nos);

            // The catalogue is per car, so find the car this save is: its engine, chassis and drivetrain
            // ids only belong to one car each. That car's own part variants are then shown first.
            var preferredCarId = PartCatalogueStore.FindCarId(engine, chassis, drivetrain);

            foreach (var (category, box, _) in PartFieldBindings())
                SetPartSelection(box, category, ReadPartValue(category), preferredCarId);

            SetPartSelection(BodyCodeComboBox, body, p => p.Body, wholeMatch);
            SetPartSelection(HornCodeComboBox, horn, p => p.Horn, wholeMatch);
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
        /// Points one of the two CarParts-backed boxes (Body, Horn) at a value: selects
        /// <paramref name="preferred"/>, else the first catalogue entry whose matching field equals the
        /// value, so the saved car's name is shown; if nothing matches, displays the raw hex code.
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
        /// Resolves the hex code one of the two CarParts-backed boxes (Body, Horn) currently stands
        /// for: the chosen car's value when an entry is selected, otherwise the raw text the user
        /// typed. If the box is still showing a car name that name is looked up again so the correct
        /// code is still used.
        /// </summary>
        private static string ResolvePartHex(ComboBox comboBox, Func<CarParts, ushort> selector)
        {
            var text = comboBox.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (comboBox.ItemsSource is IEnumerable<CarParts> parts)
                {
                    var named = parts.FirstOrDefault(p => p.Name.Equals(text, StringComparison.OrdinalIgnoreCase));
                    if (named != null)
                        return ByteUtils.UshortToHexString(selector(named));
                }

                return text;
            }

            if (comboBox.SelectedItem is CarParts selected)
                return ByteUtils.UshortToHexString(selector(selected));

            return string.Empty;
        }

        /// <summary>
        /// Shows a raw hex code in one of the two CarParts-backed boxes, clearing any car selection
        /// first so the code, not a previously chosen car, is what ResolvePartHex (and therefore the
        /// save) uses.
        /// </summary>
        private static void SetPartHexText(ComboBox comboBox, string hex)
        {
            comboBox.SelectedItem = null;
            comboBox.Text = hex;
        }

        /// <summary>
        /// Fills every part drop-down from the generated catalogue, and the Body and Horn drop-downs
        /// from the hand-built CarParts catalogue.
        /// </summary>
        private void LoadParts()
        {
            try
            {
                var sortedList = _CarPartsList?.OrderBy(cp => cp.Name).ToList() ?? new List<CarParts>();
                BodyCodeComboBox.ItemsSource = sortedList;
                HornCodeComboBox.ItemsSource = sortedList;
            }
            catch (Exception ex)
            {
                _ = ShowMessageBox($"An issue occurred while loading the parts database: {ex.Message}");
            }

            var total = 0;
            var empty = 0;
            foreach (var (category, box, _) in PartFieldBindings())
            {
                try
                {
                    var entries = PartCatalogueStore.Entries(category.TableId);
                    total += entries.Count;
                    if (entries.Count == 0) empty++;

                    // Replacing the list updates the drop-down but must leave whatever value is
                    // currently in the box untouched.
                    var text = box.Text;
                    box.ItemsSource = entries;
                    if (!string.IsNullOrWhiteSpace(text) && string.IsNullOrWhiteSpace(box.Text)) box.Text = text;

                    // A category the catalogue has no parts for (NOS) would leave the box blank, and a
                    // blank box aborts the whole save, so start it on the "stock/none" code instead.
                    if (entries.Count == 0 && string.IsNullOrWhiteSpace(box.Text))
                        SetPartHexText(box, ByteUtils.UshortToHexString(ushort.MaxValue));
                }
                catch (Exception ex)
                {
                    _ = ShowMessageBox($"An issue occurred while loading the parts catalogue: {ex.Message}");
                }
            }

            PartsCatalogueInfoText.Text = DescribeCatalogue(total, empty);
        }

        /// <summary>A one-line summary of what the generated catalogue supplied, shown on the tab.</summary>
        private static string DescribeCatalogue(int total, int emptyCategories)
        {
            if (!PartCatalogueStore.Exists)
                return "partscatalogue.db was not found next to the program, so the part lists are empty. " +
                       "Typing a hex code into any box still works.";

            var text = $"{total:n0} parts in the drop-downs, over {PartCatalogue.Categories.Count - emptyCategories} part categories.";
            if (emptyCategories > 0)
                text += $" {emptyCategories} part category has no shop parts in the game data (NOS): its box starts on FF FF and takes a typed code.";
            return text;
        }

        // ---- Known tunes: the save file's own preset list (pt_gt5_*) -----------------------------

        /// <summary>Wires the "known tune" picker: a filtered list of the catalogue's preset names.</summary>
        private void InitializePresetControls()
        {
            PresetSearchBox.ItemFilter = (search, item) => item is string name && PartCatalogueStore.MatchesPreset(name, search);
            PresetSearchBox.ItemsSource = PartCatalogueStore.PresetNames;
            PresetSearchBox.SelectionChanged += PresetSearchBox_SelectionChanged;
            ApplyPresetButton.Click += ApplyPresetButton_Click;

            PresetInfoText.Text = PartCatalogueStore.PresetNames.Count == 0
                ? "No known tunes: the catalogue's preset list could not be read."
                : $"{PartCatalogueStore.PresetNames.Count:n0} known tunes, each one a full set of parts for one car. " +
                  "Pick one and press \"Apply tune\".";
        }

        private void PresetSearchBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (PresetSearchBox.SelectedItem is not string name) return;

            var car = PartCatalogueStore.PresetCar(name);
            var count = PartCatalogueStore.ResolvePreset(name).Count;

            PresetInfoText.Text = string.IsNullOrEmpty(car)
                ? $"{name}: sets {count} part fields. Press \"Apply tune\" to fill them in."
                : $"{name}: sets {count} part fields, and is the saved tune of {car}.";
        }

        /// <summary>
        /// Fills in every part drop-down a known tune defines. The tune is a full set of parts for one
        /// car, so a category it sets - including the car's engine, chassis and drivetrain - is applied
        /// to the drop-down; categories it says nothing about are left exactly as they are, and nothing
        /// reaches the save file until the user saves.
        /// </summary>
        private async void ApplyPresetButton_Click(object? sender, RoutedEventArgs e)
        {
            var name = (PresetSearchBox.SelectedItem as string) ?? PresetSearchBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                await ShowMessageBox("Pick a known tune from the list first.");
                return;
            }

            var entries = PartCatalogueStore.ResolvePreset(name);
            if (entries.Count == 0)
            {
                await ShowMessageBox($"\"{name}\" does not set any part this editor has a field for.");
                return;
            }

            var byCategory = new Dictionary<int, PartEntry>();
            foreach (var entry in entries) byCategory[entry.Category] = entry.Part;

            var applied = 0;
            foreach (var (category, box, _) in PartFieldBindings())
            {
                if (!byCategory.TryGetValue(category.TableId, out var part)) continue;
                SetPartSelection(box, category, part.PartKey, part.CarId);
                applied++;
            }

            var car = PartCatalogueStore.PresetCar(name);
            await ShowMessageBox(
                $"Filled in {applied} of the {entries.Count} parts of \"{name}\"" +
                (string.IsNullOrEmpty(car) ? "" : $" (the saved tune of {car})") +
                ".\n\nYour save file is not touched until you save, and part fields the tune does not set are left alone.");
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
                    Engine = ByteUtils.HexStringToUshort(ResolvePartHex(EngineCodeComboBox, PartCatalogue.Get(13))),
                    Drivetrain = ByteUtils.HexStringToUshort(ResolvePartHex(DrivetrainCodeComboBox, PartCatalogue.Get(11))),
                    Chassis = ByteUtils.HexStringToUshort(ResolvePartHex(ChassisCodeComboBox, PartCatalogue.Get(7))),
                    Transmission = ByteUtils.HexStringToUshort(ResolvePartHex(TransmissionCodeComboBox, PartCatalogue.Get(12))),
                    Body = ByteUtils.HexStringToUshort(ResolvePartHex(BodyCodeComboBox, p => p.Body)),
                    Suspension = ByteUtils.HexStringToUshort(ResolvePartHex(SuspensionCodeComboBox, PartCatalogue.Get(4))),
                    Lsd = ByteUtils.HexStringToUshort(ResolvePartHex(LsdCodeComboBox, PartCatalogue.Get(23))),
                    Horn = ByteUtils.HexStringToUshort(ResolvePartHex(HornCodeComboBox, p => p.Horn)),
                    Turbo = ByteUtils.HexStringToUshort(ResolvePartHex(TurboCodeComboBox, PartCatalogue.Get(15))),
                    Exhaust = ByteUtils.HexStringToUshort(ResolvePartHex(ExhaustCodeComboBox, PartCatalogue.Get(19))),
                    Weight = ByteUtils.HexStringToUshort(ResolvePartHex(WeightCodeComboBox, PartCatalogue.Get(9))),
                    Brake = ByteUtils.HexStringToUshort(ResolvePartHex(BrakeCodeComboBox, PartCatalogue.Get(2))),
                    BrakeController = ByteUtils.HexStringToUshort(ResolvePartHex(BrakeControllerCodeComboBox, PartCatalogue.Get(3))),
                    Displacement = ByteUtils.HexStringToUshort(ResolvePartHex(DisplacementCodeComboBox, PartCatalogue.Get(16))),
                    Computer = ByteUtils.HexStringToUshort(ResolvePartHex(ComputerCodeComboBox, PartCatalogue.Get(17))),
                    Natune = ByteUtils.HexStringToUshort(ResolvePartHex(NatuneCodeComboBox, PartCatalogue.Get(14))),
                    Flywheel = ByteUtils.HexStringToUshort(ResolvePartHex(FlywheelCodeComboBox, PartCatalogue.Get(21))),
                    Clutch = ByteUtils.HexStringToUshort(ResolvePartHex(ClutchCodeComboBox, PartCatalogue.Get(20))),
                    PropellerShaft = ByteUtils.HexStringToUshort(ResolvePartHex(PropellerShaftCodeComboBox, PartCatalogue.Get(22))),
                    Intercooler = ByteUtils.HexStringToUshort(ResolvePartHex(IntercoolerCodeComboBox, PartCatalogue.Get(18))),
                    Supercharger = ByteUtils.HexStringToUshort(ResolvePartHex(SuperchargerCodeComboBox, PartCatalogue.Get(27))),
                    IntakeManifold = ByteUtils.HexStringToUshort(ResolvePartHex(IntakeManifoldCodeComboBox, PartCatalogue.Get(28))),
                    ExhaustManifold = ByteUtils.HexStringToUshort(ResolvePartHex(ExhaustManifoldCodeComboBox, PartCatalogue.Get(29))),
                    Catalyst = ByteUtils.HexStringToUshort(ResolvePartHex(CatalystCodeComboBox, PartCatalogue.Get(30))),
                    AirCleaner = ByteUtils.HexStringToUshort(ResolvePartHex(AirCleanerCodeComboBox, PartCatalogue.Get(31))),
                    Nos = ByteUtils.HexStringToUshort(ResolvePartHex(NosCodeComboBox, PartCatalogue.Get(26)))

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

        private async void ManagePaintChipsButton_Click(object sender, RoutedEventArgs e)
        {
            if (Gt5Save == null || Gt5Save.Length == 0)
            {
                await ShowMessageBox("Load a GT5.0 save before managing paint chips.");
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
