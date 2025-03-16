using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GT5_Car_hack_workshop_2.My;
using Microsoft.VisualBasic.CompilerServices;

namespace GT5_Car_hack_workshop_2
{
    [DesignerGenerated]
    public partial class TransmissionEditor : Form
    {
        [AccessedThroughProperty("Button1")] private Button _Button1;

        [AccessedThroughProperty("Timer1")] private Timer _Timer1;

        private decimal lastgearpre;

        private decimal originalFinalDrive;

        public TransmissionEditor()
        {
            Load += TransmissionLoadValues;
            lastgearpre = 0.1m;
            originalFinalDrive = 0.1m;
            InitializeComponent();
        }

        [field: AccessedThroughProperty("Label1")]
        private Label Label1 { get; set; }

        [field: AccessedThroughProperty("Label2")]
        private Label Label2 { get; set; }

        [field: AccessedThroughProperty("Label3")]
        private Label Label3 { get; set; }

        [field: AccessedThroughProperty("Label4")]
        private Label Label4 { get; set; }

        [field: AccessedThroughProperty("Label5")]
        private Label Label5 { get; set; }

        [field: AccessedThroughProperty("Label6")]
        private Label Label6 { get; set; }

        [field: AccessedThroughProperty("Label7")]
        private Label Label7 { get; set; }

        [field: AccessedThroughProperty("Label8")]
        private Label Label8 { get; set; }

        [field: AccessedThroughProperty("Label9")]
        private Label Label9 { get; set; }

        [field: AccessedThroughProperty("Label10")]
        private Label Label10 { get; set; }

        [field: AccessedThroughProperty("Label11")]
        private Label Label11 { get; set; }

        [field: AccessedThroughProperty("TextBox1")]
        private TextBox TextBox1 { get; set; }

        [field: AccessedThroughProperty("TextBox2")]
        private TextBox TextBox2 { get; set; }

        [field: AccessedThroughProperty("TextBox3")]
        private TextBox TextBox3 { get; set; }

        [field: AccessedThroughProperty("TextBox4")]
        private TextBox TextBox4 { get; set; }

        [field: AccessedThroughProperty("TextBox5")]
        private TextBox TextBox5 { get; set; }

        [field: AccessedThroughProperty("TextBox6")]
        private TextBox TextBox6 { get; set; }

        [field: AccessedThroughProperty("TextBox7")]
        private TextBox TextBox7 { get; set; }

        [field: AccessedThroughProperty("TextBox8")]
        private TextBox TextBox8 { get; set; }

        [field: AccessedThroughProperty("TextBox9")]
        private TextBox TextBox9 { get; set; }

        [field: AccessedThroughProperty("TextBox10")]
        private TextBox TextBox10 { get; set; }

        [field: AccessedThroughProperty("TextBox11")]
        private TextBox TextBox11 { get; set; }

        [field: AccessedThroughProperty("TextBox12")]
        private TextBox TextBox12 { get; set; }

        [field: AccessedThroughProperty("Label12")]
        private Label Label12 { get; set; }

        [field: AccessedThroughProperty("GroupBox1")]
        private GroupBox GroupBox1 { get; set; }

        [field: AccessedThroughProperty("TextBox14")]
        private TextBox TextBox14 { get; set; }

        [field: AccessedThroughProperty("Label14")]
        private Label Label14 { get; set; }

        [field: AccessedThroughProperty("TextBox13")]
        private TextBox TextBox13 { get; set; }

        [field: AccessedThroughProperty("Label13")]
        private Label Label13 { get; set; }

        private Timer Timer1
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

        private void TransmissionLoadValues(object sender, EventArgs e)
        {
            // Load gear ratios from save data into text boxes
            // Each gear ratio is stored as 2 bytes in hex format, divided by 1000 for display

            // Gear 1
            var firstByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 73];
            var secondByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 72];
            TextBox1.Text = ((Convert.ToInt32(firstByte.ToString("X2") + secondByte.ToString("X2"), 16)) / 1000.0).ToString();

            // Gear 2
            firstByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 71];
            secondByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 70];
            TextBox2.Text = ((Convert.ToInt32(firstByte.ToString("X2") + secondByte.ToString("X2"), 16)) / 1000.0).ToString();

            // Gear 3
            firstByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 69];
            secondByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 68];
            TextBox3.Text = ((Convert.ToInt32(firstByte.ToString("X2") + secondByte.ToString("X2"), 16)) / 1000.0).ToString();

            // Gear 4
            firstByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 67];
            secondByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 66];
            TextBox4.Text = ((Convert.ToInt32(firstByte.ToString("X2") + secondByte.ToString("X2"), 16)) / 1000.0).ToString();

            // Gear 5
            firstByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 65];
            secondByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 64];
            TextBox5.Text = ((Convert.ToInt32(firstByte.ToString("X2") + secondByte.ToString("X2"), 16)) / 1000.0).ToString();

            // Gear 6
            firstByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 63];
            secondByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 62];
            TextBox6.Text = ((Convert.ToInt32(firstByte.ToString("X2") + secondByte.ToString("X2"), 16)) / 1000.0).ToString();

            // Gear 7
            firstByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 61];
            secondByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 60];
            TextBox7.Text = ((Convert.ToInt32(firstByte.ToString("X2") + secondByte.ToString("X2"), 16)) / 1000.0).ToString();

            // Gear 8
            firstByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 59];
            secondByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 58];
            TextBox8.Text = ((Convert.ToInt32(firstByte.ToString("X2") + secondByte.ToString("X2"), 16)) / 1000.0).ToString();

            // Gear 9
            firstByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 57];
            secondByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 56];
            TextBox9.Text = ((Convert.ToInt32(firstByte.ToString("X2") + secondByte.ToString("X2"), 16)) / 1000.0).ToString();

            // Gear 10
            firstByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 55];
            secondByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 54];
            TextBox10.Text = ((Convert.ToInt32(firstByte.ToString("X2") + secondByte.ToString("X2"), 16)) / 1000.0).ToString();

            // Gear 11
            firstByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 53];
            secondByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 52];
            TextBox11.Text = ((Convert.ToInt32(firstByte.ToString("X2") + secondByte.ToString("X2"), 16)) / 1000.0).ToString();

            // Final Drive
            firstByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 51];
            secondByte = MyProject.Forms.Form1.Gt5Save[MyProject.Forms.Form1.Moff - 50];
            TextBox12.Text = ((Convert.ToInt32(firstByte.ToString("X2") + secondByte.ToString("X2"), 16)) / 1000.0).ToString();

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
                    lastgearpre = parsedHighestGearRatio;
                    break;
                }
            }

            // Cache the final drive ratio
            if (decimal.TryParse(TextBox12.Text, out var parsedFinalDriveRatio))
            {
                originalFinalDrive = parsedFinalDriveRatio;
            }
        }

        private void SaveGearRatioToGt5Save(string textBoxValue, int moffOffset1, int moffOffset2)
        {
            try
            {
                var working = (int)(float.Parse(textBoxValue) * 1000);
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[moffOffset1] =
                        byte.Parse(working.ToString("X4").Substring(0, 2), NumberStyles.HexNumber);
                    MyProject.Forms.Form1.Gt5Save[moffOffset2] =
                        byte.Parse(working.ToString("X4").Substring(2, 2), NumberStyles.HexNumber);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, e.g., log it or display an error message.
            }
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

                TextBox14.Text = Math.Round((float)originalFinalDrive /
                                            float.Parse(TextBox12.Text) *
                                            float.Parse(TextBox13.Text) *
                                            (1f / lastGear * (float)lastgearpre), 1).ToString();
            }
        }
    }
}