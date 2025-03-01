using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GT5_Car_hack_workshop_2.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace GT5_Car_hack_workshop_2
{
    [DesignerGenerated]
    public partial class Form1 : Form
    {
        [AccessedThroughProperty("Button1")] private Button _Button1;

        [AccessedThroughProperty("Button10")] private Button _Button10;

        [AccessedThroughProperty("Button11")] private Button _Button11;

        [AccessedThroughProperty("Button12")] private Button _Button12;

        [AccessedThroughProperty("Button13")] private Button _Button13;

        [AccessedThroughProperty("Button14")] private Button _Button14;

        [AccessedThroughProperty("Button15")] private Button _Button15;

        [AccessedThroughProperty("Button16")] private Button _Button16;

        [AccessedThroughProperty("Button17")] private Button _Button17;

        [AccessedThroughProperty("Button18")] private Button _Button18;

        [AccessedThroughProperty("Button19")] private Button _Button19;

        [AccessedThroughProperty("Button2")] private Button _Button2;

        [AccessedThroughProperty("Button20")] private Button _Button20;

        [AccessedThroughProperty("Button21")] private Button _Button21;

        [AccessedThroughProperty("Button22")] private Button _Button22;

        [AccessedThroughProperty("Button23")] private Button _Button23;

        [AccessedThroughProperty("Button3")] private Button _Button3;

        [AccessedThroughProperty("Button4")] private Button _Button4;

        [AccessedThroughProperty("Button5")] private Button _Button5;

        [AccessedThroughProperty("Button6")] private Button _Button6;

        [AccessedThroughProperty("Button7")] private Button _Button7;

        [AccessedThroughProperty("Button8")] private Button _Button8;

        [AccessedThroughProperty("Button9")] private Button _Button9;

        private string _CarName;

        /// <summary>
        ///     Represents an array of car parts, utilized for storing and managing saved car part values.
        /// </summary>
        /// <remarks>
        ///     This array has a capacity of up to 10,000 slots for car part data.
        ///     Users can add and update car part details within this array, which is persistently saved and loaded
        ///     from a database file ("partsdatabase.db") during program execution.
        ///     It is primarily used within the application to display and modify vehicle part information.
        /// </remarks>
        private string[] _CarParts;

        [AccessedThroughProperty("ComboBox1")] private ComboBox _ComboBox1;

        [AccessedThroughProperty("ComboBox2")] private ComboBox _ComboBox2;

        [AccessedThroughProperty("ComboBox3")] private ComboBox _ComboBox3;

        [AccessedThroughProperty("ComboBox4")] private ComboBox _ComboBox4;

        [AccessedThroughProperty("ComboBox5")] private ComboBox _ComboBox5;

        [AccessedThroughProperty("ComboBox6")] private ComboBox _ComboBox6;

        [AccessedThroughProperty("ComboBox7")] private ComboBox _ComboBox7;

        [AccessedThroughProperty("ComboBox8")] private ComboBox _ComboBox8;

        private string[] _ProgramSettings;

        [AccessedThroughProperty("TextBox4")] private TextBox _TorqueSplitTextBox;

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

        [field: AccessedThroughProperty("GroupBox1")]
        private GroupBox GroupBox1
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button1
        {
            get => _Button1;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button1_Click;
                if (_Button1 != null) _Button1.Click -= eventHandler;

                _Button1 = value;
                if (_Button1 != null) _Button1.Click += eventHandler;
            }
        }


        [field: AccessedThroughProperty("Label1")]
        private Label Label1
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox1")]
        private TextBox TextBox1
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button3
        {
            get => _Button3;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button3_Click;
                if (_Button3 != null) _Button3.Click -= eventHandler;

                _Button3 = value;
                if (_Button3 != null) _Button3.Click += eventHandler;
            }
        }

        private Button Button2
        {
            get => _Button2;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button2_Click;
                if (_Button2 != null) _Button2.Click -= eventHandler;

                _Button2 = value;
                if (_Button2 != null) _Button2.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("GroupBox2")]
        private GroupBox GroupBox2
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TabControl1")]
        private TabControl TabControl1
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TabPage1")]
        private TabPage TabPage1
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label2")]
        private Label Label2
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox2")]
        private TextBox TextBox2
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TabControl2")]
        private TabControl TabControl2
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TabPage2")]
        private TabPage TabPage2
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TabPage3")]
        private TabPage TabPage3
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox3")]
        private TextBox EngineCodeTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label3")]
        private Label Label3
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button5
        {
            get => _Button5;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button5_Click;
                if (_Button5 != null) _Button5.Click -= eventHandler;

                _Button5 = value;
                if (_Button5 != null) _Button5.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label5")]
        private Label Label5
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox5")]
        private TextBox TextBox5
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private TextBox TorqueSplitTextBox
        {
            get => _TorqueSplitTextBox;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = TextBox4_TextChanged;
                if (_TorqueSplitTextBox != null) _TorqueSplitTextBox.TextChanged -= eventHandler;

                _TorqueSplitTextBox = value;
                if (_TorqueSplitTextBox != null) _TorqueSplitTextBox.TextChanged += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label4")]
        private Label Label4
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label6")]
        private Label Label6
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox6")]
        private TextBox DrivetrainCodeTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button6
        {
            get => _Button6;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button6_Click;
                if (_Button6 != null) _Button6.Click -= eventHandler;

                _Button6 = value;
                if (_Button6 != null) _Button6.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label7")]
        private Label Label7
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label8")]
        private Label Label8
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("GroupBox3")]
        private GroupBox GroupBox3
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label9")]
        private Label Label9
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button4
        {
            get => _Button4;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button4_Click;
                if (_Button4 != null) _Button4.Click -= eventHandler;

                _Button4 = value;
                if (_Button4 != null) _Button4.Click += eventHandler;
            }
        }

        private Button Button7
        {
            get => _Button7;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button7_Click;
                if (_Button7 != null) _Button7.Click -= eventHandler;

                _Button7 = value;
                if (_Button7 != null) _Button7.Click += eventHandler;
            }
        }

        private Button Button8
        {
            get => _Button8;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button8_Click;
                if (_Button8 != null) _Button8.Click -= eventHandler;

                _Button8 = value;
                if (_Button8 != null) _Button8.Click += eventHandler;
            }
        }

        private Button Button9
        {
            get => _Button9;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button9_Click;
                if (_Button9 != null) _Button9.Click -= eventHandler;

                _Button9 = value;
                if (_Button9 != null) _Button9.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("CheckBox1")]
        private CheckBox Add74ByteCheckBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox8")]
        private TextBox TransmissionCodeTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label11")]
        private Label Label11
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label10")]
        private Label Label10
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox7")]
        private TextBox ChassisCodeTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label12")]
        private Label Label12
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox9")]
        private TextBox RemoveSpoilerCodeTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button10
        {
            get => _Button10;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button10_Click;
                if (_Button10 != null) _Button10.Click -= eventHandler;

                _Button10 = value;
                if (_Button10 != null) _Button10.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label13")]
        private Label Label13
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox10")]
        private TextBox PaintCodesTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label14")]
        private Label Label14
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button11
        {
            get => _Button11;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button11_Click;
                if (_Button11 != null) _Button11.Click -= eventHandler;

                _Button11 = value;
                if (_Button11 != null) _Button11.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("TextBox11")]
        public TextBox TurboModifierTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label15")]
        private Label Label15
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("CheckBox2")]
        private CheckBox SambaLikePerformanceCheckbox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label17")]
        private Label Label17
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox12")]
        private TextBox AeroRearTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox13")]
        private TextBox AeroFrontTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label16")]
        private Label Label16
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label19")]
        private Label Label19
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox15")]
        private TextBox SuspensionHeightRearTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox14")]
        private TextBox SuspensionHeightFrontTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label18")]
        private Label Label18
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox16")]
        private TextBox GripTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label20")]
        private Label Label20
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label22")]
        private Label Label22
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox17")]
        private TextBox SpringRateRearTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox18")]
        private TextBox SpringRateFrontTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label21")]
        private Label Label21
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label24")]
        private Label Label24
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private ComboBox ComboBox2
        {
            get => _ComboBox2;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = ComboBox2_SelectedIndexChanged;
                if (_ComboBox2 != null) _ComboBox2.SelectedIndexChanged -= eventHandler;

                _ComboBox2 = value;
                if (_ComboBox2 != null) _ComboBox2.SelectedIndexChanged += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label23")]
        private Label Label23
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private ComboBox ComboBox1
        {
            get => _ComboBox1;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = ComboBox1_SelectedIndexChanged;
                if (_ComboBox1 != null) _ComboBox1.SelectedIndexChanged -= eventHandler;

                _ComboBox1 = value;
                if (_ComboBox1 != null) _ComboBox1.SelectedIndexChanged += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label28")]
        private Label Label28
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private ComboBox ComboBox6
        {
            get => _ComboBox6;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = ComboBox6_SelectedIndexChanged;
                if (_ComboBox6 != null) _ComboBox6.SelectedIndexChanged -= eventHandler;

                _ComboBox6 = value;
                if (_ComboBox6 != null) _ComboBox6.SelectedIndexChanged += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label27")]
        private Label Label27
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private ComboBox ComboBox5
        {
            get => _ComboBox5;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = ComboBox5_SelectedIndexChanged;
                if (_ComboBox5 != null) _ComboBox5.SelectedIndexChanged -= eventHandler;

                _ComboBox5 = value;
                if (_ComboBox5 != null) _ComboBox5.SelectedIndexChanged += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label26")]
        private Label Label26
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private ComboBox ComboBox4
        {
            get => _ComboBox4;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = ComboBox4_SelectedIndexChanged;
                if (_ComboBox4 != null) _ComboBox4.SelectedIndexChanged -= eventHandler;

                _ComboBox4 = value;
                if (_ComboBox4 != null) _ComboBox4.SelectedIndexChanged += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label25")]
        private Label Label25
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private ComboBox ComboBox3
        {
            get => _ComboBox3;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = ComboBox3_SelectedIndexChanged;
                if (_ComboBox3 != null) _ComboBox3.SelectedIndexChanged -= eventHandler;

                _ComboBox3 = value;
                if (_ComboBox3 != null) _ComboBox3.SelectedIndexChanged += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label30")]
        private Label Label30
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private ComboBox ComboBox8
        {
            get => _ComboBox8;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = ComboBox8_SelectedIndexChanged;
                if (_ComboBox8 != null) _ComboBox8.SelectedIndexChanged -= eventHandler;

                _ComboBox8 = value;
                if (_ComboBox8 != null) _ComboBox8.SelectedIndexChanged += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label29")]
        private Label Label29
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private ComboBox ComboBox7
        {
            get => _ComboBox7;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = ComboBox7_SelectedIndexChanged;
                if (_ComboBox7 != null) _ComboBox7.SelectedIndexChanged -= eventHandler;

                _ComboBox7 = value;
                if (_ComboBox7 != null) _ComboBox7.SelectedIndexChanged += eventHandler;
            }
        }

        private Button Button12
        {
            get => _Button12;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button12_Click;
                if (_Button12 != null) _Button12.Click -= eventHandler;

                _Button12 = value;
                if (_Button12 != null) _Button12.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("TextBox19")]
        public TextBox ExhauseMultiplierTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label31")]
        private Label Label31
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox20")]
        private TextBox CarBodyCodeTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label32")]
        private Label Label32
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox21")]
        private TextBox SuspensionCodeTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label33")]
        private Label Label33
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox22")]
        private TextBox LsdCodeTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label34")]
        private Label Label34
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox23")]
        private TextBox HornCodeTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label35")]
        private Label Label35
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TabPage4")]
        private TabPage TabPage4
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button13
        {
            get => _Button13;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button13_Click;
                if (_Button13 != null) _Button13.Click -= eventHandler;

                _Button13 = value;
                if (_Button13 != null) _Button13.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label36")]
        private Label Label36
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button14
        {
            get => _Button14;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button14_Click;
                if (_Button14 != null) _Button14.Click -= eventHandler;

                _Button14 = value;
                if (_Button14 != null) _Button14.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("CheckBox3")]
        private CheckBox AddLucaBytesCheckBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button15
        {
            get => _Button15;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button15_Click;
                if (_Button15 != null) _Button15.Click -= eventHandler;

                _Button15 = value;
                if (_Button15 != null) _Button15.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("GroupBox5")]
        private GroupBox GroupBox5
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("GroupBox4")]
        private GroupBox GroupBox4
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("GroupBox7")]
        private GroupBox GroupBox7
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("GroupBox6")]
        private GroupBox GroupBox6
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("GroupBox10")]
        private GroupBox GroupBox10
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("GroupBox9")]
        private GroupBox GroupBox9
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("GroupBox8")]
        private GroupBox GroupBox8
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("GroupBox12")]
        private GroupBox GroupBox12
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("GroupBox11")]
        private GroupBox GroupBox11
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button16
        {
            get => _Button16;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button16_Click;
                if (_Button16 != null) _Button16.Click -= eventHandler;

                _Button16 = value;
                if (_Button16 != null) _Button16.Click += eventHandler;
            }
        }

        private Button Button17
        {
            get => _Button17;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button17_Click;
                if (_Button17 != null) _Button17.Click -= eventHandler;

                _Button17 = value;
                if (_Button17 != null) _Button17.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label37")]
        private Label Label37
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("GroupBox13")]
        private GroupBox GroupBox13
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("CheckBox4")]
        private CheckBox CheckBox4
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox24")]
        private TextBox WeightMultiplierTextBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("Label38")]
        private Label Label38
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button18
        {
            get => _Button18;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button18_Click;
                if (_Button18 != null) _Button18.Click -= eventHandler;

                _Button18 = value;
                if (_Button18 != null) _Button18.Click += eventHandler;
            }
        }


        /// <summary>
        ///     Slam car button
        /// </summary>
        private Button Button19
        {
            get => _Button19;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = SlamCar_Button19_Click;
                if (_Button19 != null) _Button19.Click -= eventHandler;

                _Button19 = value;
                if (_Button19 != null) _Button19.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("GroupBox14")]
        private GroupBox GroupBox14
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button21
        {
            get => _Button21;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button21_Click;
                if (_Button21 != null) _Button21.Click -= eventHandler;

                _Button21 = value;
                if (_Button21 != null) _Button21.Click += eventHandler;
            }
        }

        private Button Button20
        {
            get => _Button20;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button20_Click;
                if (_Button20 != null) _Button20.Click -= eventHandler;

                _Button20 = value;
                if (_Button20 != null) _Button20.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Label39")]
        private Label Label39
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button22
        {
            get => _Button22;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button22_Click;
                if (_Button22 != null) _Button22.Click -= eventHandler;

                _Button22 = value;
                if (_Button22 != null) _Button22.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("CheckBox7")]
        private CheckBox RemoveFrontBumperCheckBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("CheckBox6")]
        private CheckBox RemoveRearBumperCheckBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("CheckBox5")]
        private CheckBox RemoveHoodCheckBox
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        private Button Button23
        {
            get => _Button23;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button23_Click;
                if (_Button23 != null) _Button23.Click -= eventHandler;

                _Button23 = value;
                if (_Button23 != null) _Button23.Click += eventHandler;
            }
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

            var suspensionHeightFront = ConvertBytesToUnsignedInt(new[] { Gt5Save[Moff - 33], Gt5Save[Moff - 32] });
            SuspensionHeightFrontTextBox.Text = suspensionHeightFront.ToString();
            var suspensionHeightRear = ConvertBytesToUnsignedInt(new[] { Gt5Save[Moff - 31], Gt5Save[Moff - 30] });
            SuspensionHeightRearTextBox.Text = suspensionHeightRear.ToString();

            GripTextBox.Text = Gt5Save[Moff + 10].ToString();

            SpringRateFrontTextBox.Text = Gt5Save[Moff - 27].ToString();
            SpringRateRearTextBox.Text = Gt5Save[Moff - 26].ToString();

            ExhauseMultiplierTextBox.Text = $"{Gt5Save[Moff - 155]:X2} {Gt5Save[Moff - 154]:X2} {Gt5Save[Moff - 153]:X2} {Gt5Save[Moff - 152]:X2}";

            CarBodyCodeTextBox.Text = $"{Gt5Save[Moff - 262]:X2} {Gt5Save[Moff - 261]:X2}";

            SuspensionCodeTextBox.Text = $"{Gt5Save[Moff - 201]:X2} {Gt5Save[Moff - 200]:X2}";

            LsdCodeTextBox.Text = $"{Gt5Save[Moff - 197]:X2} {Gt5Save[Moff - 196]:X2}";

            HornCodeTextBox.Text = $"{Gt5Save[Moff + 23]:X2} {Gt5Save[Moff + 24]:X2}";

            WeightMultiplierTextBox.Text = $"{Gt5Save[Moff - 191]:X2} {Gt5Save[Moff - 190]:X2} {Gt5Save[Moff - 189]:X2} {Gt5Save[Moff - 188]:X2}";
        }

        private int ConvertBytesToUnsignedInt(byte[] bytes)
        {
            Array.Reverse(bytes); // Reverse the byte order

            var result = 0;
            for (var i = 0; i < bytes.Length; i++) result += bytes[i] << (i * 8);

            return result;
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex)) throw new ArgumentException("Input hex string cannot be null or empty.");

            // Remove any whitespace from the hex string
            hex = hex.Replace(" ", "");

            // Ensure the string contains an even number of characters
            if (hex.Length % 2 != 0) throw new FormatException("Hex string must have an even number of characters.");

            var byteList = new List<byte>();

            for (var i = 0; i < hex.Length; i += 2)
            {
                var byteString = hex.Substring(i, 2);
                byteList.Add(byte.Parse(byteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
            }

            return byteList.ToArray();
        }

        public static byte[] ConvertToByteArray(int value, int outputSize = 0)
        {
            // Throws an exception if the value is negative
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be non-negative.");

            // Create a list to store bytes
            var byteList = new List<byte>();

            // Extract the bytes from the integer
            while (value > 0)
            {
                byteList.Insert(0, (byte)(value & 0xFF)); // Get the least significant byte
                value >>= 8; // Shift right by 8 bits (1 byte)
            }

            // If outputSize is greater than the current size, pad the array with leading zeros
            while (byteList.Count < outputSize) byteList.Insert(0, 0); // Add padding at the beginning

            // Validate if the array exceeds the outputSize and throw an error
            if (outputSize != 0 && byteList.Count > outputSize) throw new ArgumentOutOfRangeException(nameof(outputSize), "Output size is too small to represent the value.");

            // Convert the list to an array and return it
            return byteList.Count > 0 ? byteList.ToArray() : new byte[] { 0 };
        }


        private void SaveData()
        {
            try
            {
                var engineByteValues = HexStringToByteArray(EngineCodeTextBox.Text);

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
                var drivetrainByteValues = HexStringToByteArray(DrivetrainCodeTextBox.Text);

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
                var chassisByteValues = HexStringToByteArray(ChassisCodeTextBox.Text);

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
                var transmissionByteValues = HexStringToByteArray(TransmissionCodeTextBox.Text);

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
                var paintCodes = HexStringToByteArray(PaintCodesTextBox.Text);

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
                var turboModifier = HexStringToByteArray(TurboModifierTextBox.Text);

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
                if (!int.TryParse(SuspensionHeightFrontTextBox.Text, out var suspensionHeightFront)) throw new FormatException("Suspension height front value must be a number.");

                if (suspensionHeightFront < 0 || suspensionHeightFront > (2 ^ 16) - 1) throw new FormatException("Suspension height front value must be between 0 and 65535.");

                var suspensionHeightFrontBytes = ConvertToByteArray(suspensionHeightFront, 2);
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
                if (!int.TryParse(SuspensionHeightRearTextBox.Text, out var suspensionHeightRear)) throw new FormatException("Suspension height rear value must be a number.");

                if (suspensionHeightRear < 0 || suspensionHeightRear > (2 ^ 16) - 1) throw new FormatException("Suspension height rear value must be between 0 and 65535.");

                var suspensionHeightRearBytes = ConvertToByteArray(suspensionHeightRear, 2);
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
                var exhauseMultiplier = HexStringToByteArray(ExhauseMultiplierTextBox.Text);

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
                var carBodyCode = HexStringToByteArray(CarBodyCodeTextBox.Text);

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
                var suspensionCode = HexStringToByteArray(SuspensionCodeTextBox.Text);

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
                var lsdCode = HexStringToByteArray(LsdCodeTextBox.Text);

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
                var hornCode = HexStringToByteArray(HornCodeTextBox.Text);

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
                var weightMultiplier = HexStringToByteArray(WeightMultiplierTextBox.Text);

                Gt5Save[Moff - 191] = weightMultiplier[0];
                Gt5Save[Moff - 190] = weightMultiplier[1];
                Gt5Save[Moff - 189] = weightMultiplier[2];
                Gt5Save[Moff - 188] = weightMultiplier[3];
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

        // Token: 0x06000143 RID: 323 RVA: 0x0000D6D8 File Offset: 0x0000BAD8
        private void Button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Find your GT5.0 file";
            if (ofd.ShowDialog() == DialogResult.OK) TextBox1.Text = ofd.FileName;
        }

        // Token: 0x06000144 RID: 324 RVA: 0x0000D710 File Offset: 0x0000BB10
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!MyProject.Computer.FileSystem.DirectoryExists("Backups")) MyProject.Computer.FileSystem.CreateDirectory("Backups");

            _ProgramSettings = SettingsFileClass.LoadSettings("GT5CHWsettings.ini", 1);
            _CarParts = SettingsFileClass.LoadSettings("partsdatabase.db", 9999);
            TextBox1.Text = _ProgramSettings[0];
            TextBox2.Text = _ProgramSettings[1];
            LoadParts();
        }

        // Token: 0x06000145 RID: 325 RVA: 0x0000D7A4 File Offset: 0x0000BBA4
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _ProgramSettings[0] = TextBox1.Text;
            _ProgramSettings[1] = TextBox2.Text;
            SettingsFileClass.SaveSettings(_ProgramSettings, "GT5CHWsettings.ini");
            SettingsFileClass.SaveSettings(_CarParts, "partsdatabase.db");
        }

        // Token: 0x06000146 RID: 326 RVA: 0x0000D7F8 File Offset: 0x0000BBF8
        private void Button2_Click(object sender, EventArgs e)
        {
            var fi = new FileInfo(TextBox1.Text);
            var tad = DateAndTime.Now.ToString();
            tad = tad.Replace("/", "-");
            tad = tad.Replace(":", ".");
            MyProject.Computer.FileSystem.CopyFile(TextBox1.Text, "Backups\\" + tad + " " + fi.Name);
            ProcessData();
        }

        // Token: 0x06000147 RID: 327 RVA: 0x0000D87C File Offset: 0x0000BC7C
        private void Button3_Click(object sender, EventArgs e)
        {
            SaveData();
            LoadData.Encrypt(TextBox1.Text);
            Interaction.MsgBox("Data saved, immediately goto the GT Auto and change the cars oil to apply the hacks.");
        }

        // Token: 0x06000148 RID: 328 RVA: 0x0000D8A4 File Offset: 0x0000BCA4
        private void Button5_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        // Token: 0x06000149 RID: 329 RVA: 0x0000D8AC File Offset: 0x0000BCAC
        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            TextBox5.Text = Conversions.ToString(100.0 - Conversion.Val(TorqueSplitTextBox.Text));
        }

        // Token: 0x0600014A RID: 330 RVA: 0x0000D8D8 File Offset: 0x0000BCD8
        private void Button6_Click(object sender, EventArgs e)
        {
            TorqueSplitTextBox.Text = Conversions.ToString(50);
            DrivetrainCodeTextBox.Text = "0A 1E";
        }

        // Token: 0x0600014B RID: 331 RVA: 0x0000D8FC File Offset: 0x0000BCFC
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

        // Token: 0x0600014C RID: 332 RVA: 0x0000D9E8 File Offset: 0x0000BDE8
        private void Button7_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "Copy the hex values in this box, this is your current cars engine\r\rIf you paste your copied engine values back in while editing another car, that car will have the engine of the car you copied it from.");
        }

        // Token: 0x0600014D RID: 333 RVA: 0x0000D9F8 File Offset: 0x0000BDF8
        private void Button8_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "This directly controls the torque that is sent to the front wheels, EG: if you typed in 10 the torque split would then be 10:90, if you typed in 50 the split would be 50:50\rYou could be really tricky and type 101, then the split would be 101:-1, which I reccomend in combination with added grip for high HP cars");
        }

        // Token: 0x0600014E RID: 334 RVA: 0x0000DA08 File Offset: 0x0000BE08
        private void Button9_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "This is what gives hackers all the freedom to swap chassis, engine etc. in update 2.14.\rUntick this and you will find that most hacks won't work");
        }

        // Token: 0x0600014F RID: 335 RVA: 0x0000DA18 File Offset: 0x0000BE18
        private void Button10_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "Make sure the highest stage spoiler in GT Auto is installed. EG Type B or Type C (which ever one is the highest available).\rThen increase the value here by 1. EG: 2 to 3 or 3 to 4 etc.");
        }

        // Token: 0x06000150 RID: 336 RVA: 0x0000DA28 File Offset: 0x0000BE28
        private void Button11_Click(object sender, EventArgs e)
        {
            MyProject.Forms.Tranny.Show();
        }

        // Token: 0x06000151 RID: 337 RVA: 0x0000DA3C File Offset: 0x0000BE3C
        /// <summary>
        ///     Add current car to parts database.
        ///     Handles the event triggered when button12 is clicked. This method is used to add the current car details
        ///     to the parts database. It validates for available space in the database and ensures no duplicate entries exist.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs object containing event data.</param>
        private void Button12_Click(object sender, EventArgs e)
        {
            // cpos is used to find the next empty slot in the car parts database to place the newly added car parts
            var cpos = -1;
            for (var i = 0; i <= _CarParts.Length - 1; i++)
                if (_CarParts[i] == " ")
                {
                    cpos = i;
                    break;
                }

            if (cpos == -1)
            {
                MessageBox.Show("No more space in the database, please delete some cars first");
                return;
            }

            _CarName = Interaction.InputBox("Car name:", "", Conversions.ToString(_CarName));

            var linetoadd =
                _CarName + "," +
                EngineCodeTextBox.Text + "," +
                DrivetrainCodeTextBox.Text + "," +
                ChassisCodeTextBox.Text + "," +
                TransmissionCodeTextBox.Text + "," +
                CarBodyCodeTextBox.Text + "," +
                SuspensionCodeTextBox.Text + "," +
                LsdCodeTextBox.Text + "," +
                HornCodeTextBox.Text;

            for (var j = 0; j <= _CarParts.Length - 1; j++)
                if (_CarParts[j].Equals(linetoadd, StringComparison.OrdinalIgnoreCase))
                    MessageBox.Show("Car already exists", "", MessageBoxButtons.OK);

            _CarParts[cpos] = linetoadd;
            LoadParts();
        }

        private void LoadParts()
        {
            // Assuming all ComboBoxes are part of the same container (like the form or a group box)
            foreach (var comboBox in new[] { ComboBox1, ComboBox2, ComboBox3, ComboBox4, ComboBox5, ComboBox6, ComboBox7, ComboBox8 }) comboBox.Items.Clear();

            // Spacer is used so that the hex value data after the comma doesn't show up in the GUI (Yep, terrible) TODO: Fix
            var spacer = "                                                                                                                                            ,";
            for (var i = 0; i <= _CarParts.Length - 1; i++)
                if (_CarParts[i] != " ")
                    try
                    {
                        var sparr = _CarParts[i].Split(',');
                        ComboBox1.Items.Add(sparr[0] + spacer + sparr[1]);
                        ComboBox2.Items.Add(sparr[0] + spacer + sparr[2]);
                        ComboBox3.Items.Add(sparr[0] + spacer + sparr[3]);
                        ComboBox4.Items.Add(sparr[0] + spacer + sparr[4]);
                        ComboBox5.Items.Add(sparr[0] + spacer + sparr[6]);
                        ComboBox6.Items.Add(sparr[0] + spacer + sparr[5]);
                        ComboBox7.Items.Add(sparr[0] + spacer + sparr[7]);
                        ComboBox8.Items.Add(sparr[0] + spacer + sparr[8]);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An issue occured while loading the parts database: {ex.Message}");
                    }
        }

        // Token: 0x06000153 RID: 339 RVA: 0x0000DDF0 File Offset: 0x0000C1F0
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var sparr = ComboBox1.SelectedItem.ToString().Split(',');
                EngineCodeTextBox.Text = sparr[1];
            }
            catch (Exception ex)
            {
            }
        }

        // Token: 0x06000154 RID: 340 RVA: 0x0000DE50 File Offset: 0x0000C250
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var sparr = ComboBox2.SelectedItem.ToString().Split(',');
                DrivetrainCodeTextBox.Text = sparr[1];
            }
            catch (Exception ex)
            {
            }
        }

        // Token: 0x06000155 RID: 341 RVA: 0x0000DEB0 File Offset: 0x0000C2B0
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var sparr = ComboBox3.SelectedItem.ToString().Split(',');
                ChassisCodeTextBox.Text = sparr[1];
            }
            catch (Exception ex)
            {
            }
        }

        // Token: 0x06000156 RID: 342 RVA: 0x0000DF10 File Offset: 0x0000C310
        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var sparr = ComboBox4.SelectedItem.ToString().Split(',');
                TransmissionCodeTextBox.Text = sparr[1];
            }
            catch (Exception ex)
            {
            }
        }

        // Token: 0x06000157 RID: 343 RVA: 0x0000DF70 File Offset: 0x0000C370
        private void ComboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var sparr = ComboBox5.SelectedItem.ToString().Split(',');
                SuspensionCodeTextBox.Text = sparr[1];
            }
            catch (Exception ex)
            {
            }
        }

        // Token: 0x06000158 RID: 344 RVA: 0x0000DFD0 File Offset: 0x0000C3D0
        private void ComboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var sparr = ComboBox6.SelectedItem.ToString().Split(',');
                CarBodyCodeTextBox.Text = sparr[1];
            }
            catch (Exception ex)
            {
            }
        }

        // Token: 0x06000159 RID: 345 RVA: 0x0000E030 File Offset: 0x0000C430
        private void ComboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var sparr = ComboBox7.SelectedItem.ToString().Split(',');
                LsdCodeTextBox.Text = sparr[1];
            }
            catch (Exception ex)
            {
            }
        }

        // Token: 0x0600015A RID: 346 RVA: 0x0000E090 File Offset: 0x0000C490
        private void ComboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var sparr = ComboBox8.SelectedItem.ToString().Split(',');
                HornCodeTextBox.Text = sparr[1];
            }
            catch (Exception ex)
            {
            }
        }

        // Token: 0x0600015B RID: 347 RVA: 0x0000E0F0 File Offset: 0x0000C4F0
        private void Button13_Click(object sender, EventArgs e)
        {
            MyProject.Forms.Customperformance.ShowDialog();
        }

        // Token: 0x0600015C RID: 348 RVA: 0x0000E104 File Offset: 0x0000C504
        private void Button14_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "This will override the 74 byte checkbox\rGT5 Editor 1.6 uses this method instead of the 74 byte, I have no idea whether it's more effective. So I've added in this function for testing purposes.");
        }

        // Token: 0x0600015D RID: 349 RVA: 0x0000E114 File Offset: 0x0000C514
        private void Button15_Click(object sender, EventArgs e)
        {
            Gt5Save[Conversions.ToInteger(Operators.AddObject(Moff, 248))] = byte.MaxValue;
            Interaction.MsgBox(
                "The car is now yours, you can now either hack it, or click encrypt and save then return the data to the PS3");
            SaveData();
        }

        // Token: 0x0600015E RID: 350 RVA: 0x0000E150 File Offset: 0x0000C550
        private void Button16_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "This is the performance multiplier of the exhaust and turbo. Increasing these will increase the effectiveness of these performance parts on the engines performance");
        }

        // Token: 0x0600015F RID: 351 RVA: 0x0000E160 File Offset: 0x0000C560
        private void Button17_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "Samba like performance installs the performance parts out of a samba bus onto your current car.\rThe samba bus parts increase the engines performance a lot more than the performance parts for most other cars.\rEG: a 300hp (When stock) engine with samba bus parts installed can increase to about 1,000 - 1,800hp");
        }

        // Token: 0x06000160 RID: 352 RVA: 0x0000E170 File Offset: 0x0000C570
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

        // Token: 0x06000164 RID: 356 RVA: 0x0000E310 File Offset: 0x0000C710
        private void Button22_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "-----------------GT5 car hack workshop-----------------\rCreated by HeylonNHP\rSpecial thanks to:\rflatz for the pfdtool\raldotools.org for games.conf/global.conf\rTo the guys at http://gt5dragracing.com/ for daring to beta test my first version");
        }

        // Token: 0x06000165 RID: 357 RVA: 0x0000E320 File Offset: 0x0000C720
        private void Button23_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox(
                "When you tick this checkbox, you must buy the Rigidity Improvement right before doing anything after loading the hacked save, the Rigidity Improvement is found under Body/Chassis in the tuning shop. The hack wont take proper effect without buying this upgrade.\r\rIf you have already purchased this upgrade, it will be uninstalled when you apply this hack.");
        }
    }
}