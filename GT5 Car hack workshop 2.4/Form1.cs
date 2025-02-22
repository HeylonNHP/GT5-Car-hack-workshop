using System;
using System.IO;
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
        public Form1()
        {
            Load += Form1_Load;
            FormClosing += Form1_FormClosing;
            Moff = 0;
            Carname = "";
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
                if (_Button1 != null)
                {
                    _Button1.Click -= eventHandler;
                }

                _Button1 = value;
                if (_Button1 != null)
                {
                    _Button1.Click += eventHandler;
                }
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
                if (_Button3 != null)
                {
                    _Button3.Click -= eventHandler;
                }

                _Button3 = value;
                if (_Button3 != null)
                {
                    _Button3.Click += eventHandler;
                }
            }
        }

        private Button Button2
        {
            get => _Button2;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button2_Click;
                if (_Button2 != null)
                {
                    _Button2.Click -= eventHandler;
                }

                _Button2 = value;
                if (_Button2 != null)
                {
                    _Button2.Click += eventHandler;
                }
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
        private TextBox TextBox3
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
                if (_Button5 != null)
                {
                    _Button5.Click -= eventHandler;
                }

                _Button5 = value;
                if (_Button5 != null)
                {
                    _Button5.Click += eventHandler;
                }
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

        private TextBox TextBox4
        {
            get => _TextBox4;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = TextBox4_TextChanged;
                if (_TextBox4 != null)
                {
                    _TextBox4.TextChanged -= eventHandler;
                }

                _TextBox4 = value;
                if (_TextBox4 != null)
                {
                    _TextBox4.TextChanged += eventHandler;
                }
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
        private TextBox TextBox6
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
                if (_Button6 != null)
                {
                    _Button6.Click -= eventHandler;
                }

                _Button6 = value;
                if (_Button6 != null)
                {
                    _Button6.Click += eventHandler;
                }
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
                if (_Button4 != null)
                {
                    _Button4.Click -= eventHandler;
                }

                _Button4 = value;
                if (_Button4 != null)
                {
                    _Button4.Click += eventHandler;
                }
            }
        }

        private Button Button7
        {
            get => _Button7;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button7_Click;
                if (_Button7 != null)
                {
                    _Button7.Click -= eventHandler;
                }

                _Button7 = value;
                if (_Button7 != null)
                {
                    _Button7.Click += eventHandler;
                }
            }
        }

        private Button Button8
        {
            get => _Button8;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button8_Click;
                if (_Button8 != null)
                {
                    _Button8.Click -= eventHandler;
                }

                _Button8 = value;
                if (_Button8 != null)
                {
                    _Button8.Click += eventHandler;
                }
            }
        }

        private Button Button9
        {
            get => _Button9;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button9_Click;
                if (_Button9 != null)
                {
                    _Button9.Click -= eventHandler;
                }

                _Button9 = value;
                if (_Button9 != null)
                {
                    _Button9.Click += eventHandler;
                }
            }
        }

        [field: AccessedThroughProperty("CheckBox1")]
        private CheckBox CheckBox1
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox8")]
        private TextBox TextBox8
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
        private TextBox TextBox7
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
        private TextBox TextBox9
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
                if (_Button10 != null)
                {
                    _Button10.Click -= eventHandler;
                }

                _Button10 = value;
                if (_Button10 != null)
                {
                    _Button10.Click += eventHandler;
                }
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
        private TextBox TextBox10
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
                if (_Button11 != null)
                {
                    _Button11.Click -= eventHandler;
                }

                _Button11 = value;
                if (_Button11 != null)
                {
                    _Button11.Click += eventHandler;
                }
            }
        }

        [field: AccessedThroughProperty("TextBox11")]
        public TextBox TextBox11
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
        private CheckBox CheckBox2
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
        private TextBox TextBox12
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox13")]
        private TextBox TextBox13
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
        private TextBox TextBox15
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox14")]
        private TextBox TextBox14
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
        private TextBox TextBox16
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
        private TextBox TextBox17
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("TextBox18")]
        private TextBox TextBox18
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
                if (_ComboBox2 != null)
                {
                    _ComboBox2.SelectedIndexChanged -= eventHandler;
                }

                _ComboBox2 = value;
                if (_ComboBox2 != null)
                {
                    _ComboBox2.SelectedIndexChanged += eventHandler;
                }
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
                if (_ComboBox1 != null)
                {
                    _ComboBox1.SelectedIndexChanged -= eventHandler;
                }

                _ComboBox1 = value;
                if (_ComboBox1 != null)
                {
                    _ComboBox1.SelectedIndexChanged += eventHandler;
                }
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
                if (_ComboBox6 != null)
                {
                    _ComboBox6.SelectedIndexChanged -= eventHandler;
                }

                _ComboBox6 = value;
                if (_ComboBox6 != null)
                {
                    _ComboBox6.SelectedIndexChanged += eventHandler;
                }
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
                if (_ComboBox5 != null)
                {
                    _ComboBox5.SelectedIndexChanged -= eventHandler;
                }

                _ComboBox5 = value;
                if (_ComboBox5 != null)
                {
                    _ComboBox5.SelectedIndexChanged += eventHandler;
                }
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
                if (_ComboBox4 != null)
                {
                    _ComboBox4.SelectedIndexChanged -= eventHandler;
                }

                _ComboBox4 = value;
                if (_ComboBox4 != null)
                {
                    _ComboBox4.SelectedIndexChanged += eventHandler;
                }
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
                if (_ComboBox3 != null)
                {
                    _ComboBox3.SelectedIndexChanged -= eventHandler;
                }

                _ComboBox3 = value;
                if (_ComboBox3 != null)
                {
                    _ComboBox3.SelectedIndexChanged += eventHandler;
                }
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
                if (_ComboBox8 != null)
                {
                    _ComboBox8.SelectedIndexChanged -= eventHandler;
                }

                _ComboBox8 = value;
                if (_ComboBox8 != null)
                {
                    _ComboBox8.SelectedIndexChanged += eventHandler;
                }
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
                if (_ComboBox7 != null)
                {
                    _ComboBox7.SelectedIndexChanged -= eventHandler;
                }

                _ComboBox7 = value;
                if (_ComboBox7 != null)
                {
                    _ComboBox7.SelectedIndexChanged += eventHandler;
                }
            }
        }

        private Button Button12
        {
            get => _Button12;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button12_Click;
                if (_Button12 != null)
                {
                    _Button12.Click -= eventHandler;
                }

                _Button12 = value;
                if (_Button12 != null)
                {
                    _Button12.Click += eventHandler;
                }
            }
        }

        [field: AccessedThroughProperty("TextBox19")]
        public TextBox TextBox19
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
        private TextBox TextBox20
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
        private TextBox TextBox21
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
        private TextBox TextBox22
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
        private TextBox TextBox23
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
                if (_Button13 != null)
                {
                    _Button13.Click -= eventHandler;
                }

                _Button13 = value;
                if (_Button13 != null)
                {
                    _Button13.Click += eventHandler;
                }
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
                if (_Button14 != null)
                {
                    _Button14.Click -= eventHandler;
                }

                _Button14 = value;
                if (_Button14 != null)
                {
                    _Button14.Click += eventHandler;
                }
            }
        }

        [field: AccessedThroughProperty("CheckBox3")]
        private CheckBox CheckBox3
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
                if (_Button15 != null)
                {
                    _Button15.Click -= eventHandler;
                }

                _Button15 = value;
                if (_Button15 != null)
                {
                    _Button15.Click += eventHandler;
                }
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
                if (_Button16 != null)
                {
                    _Button16.Click -= eventHandler;
                }

                _Button16 = value;
                if (_Button16 != null)
                {
                    _Button16.Click += eventHandler;
                }
            }
        }

        private Button Button17
        {
            get => _Button17;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button17_Click;
                if (_Button17 != null)
                {
                    _Button17.Click -= eventHandler;
                }

                _Button17 = value;
                if (_Button17 != null)
                {
                    _Button17.Click += eventHandler;
                }
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
        private TextBox TextBox24
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
                if (_Button18 != null)
                {
                    _Button18.Click -= eventHandler;
                }

                _Button18 = value;
                if (_Button18 != null)
                {
                    _Button18.Click += eventHandler;
                }
            }
        }


        /// <summary>
        /// Slam car button
        /// </summary>
        private Button Button19
        {
            get => _Button19;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = SlamCar_Button19_Click;
                if (_Button19 != null)
                {
                    _Button19.Click -= eventHandler;
                }

                _Button19 = value;
                if (_Button19 != null)
                {
                    _Button19.Click += eventHandler;
                }
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
                if (_Button21 != null)
                {
                    _Button21.Click -= eventHandler;
                }

                _Button21 = value;
                if (_Button21 != null)
                {
                    _Button21.Click += eventHandler;
                }
            }
        }

        private Button Button20
        {
            get => _Button20;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Button20_Click;
                if (_Button20 != null)
                {
                    _Button20.Click -= eventHandler;
                }

                _Button20 = value;
                if (_Button20 != null)
                {
                    _Button20.Click += eventHandler;
                }
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
                if (_Button22 != null)
                {
                    _Button22.Click -= eventHandler;
                }

                _Button22 = value;
                if (_Button22 != null)
                {
                    _Button22.Click += eventHandler;
                }
            }
        }

        [field: AccessedThroughProperty("CheckBox7")]
        private CheckBox CheckBox7
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("CheckBox6")]
        private CheckBox CheckBox6
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        [field: AccessedThroughProperty("CheckBox5")]
        private CheckBox CheckBox5
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
                if (_Button23 != null)
                {
                    _Button23.Click -= eventHandler;
                }

                _Button23 = value;
                if (_Button23 != null)
                {
                    _Button23.Click += eventHandler;
                }
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

            Gt5Save = (byte[])loaddata.load(TextBox1.Text);
            object[] chararr;
            byte[] charbyte;
            int num4;
            int j;
            checked
            {
                chararr = new object[TextBox2.Text.Length - 1 + 1];
                int num = 0;
                int num2 = TextBox2.Text.Length - 1;
                for (int i = num; i <= num2; i++)
                {
                    chararr[i] = TextBox2.Text.Substring(i, 1);
                }

                charbyte = new byte[chararr.Length - 1 + 1];
                int num3 = 0;
                num4 = chararr.Length - 1;
                j = num3;
            }

            checked
            {
                while (j <= num4)
                {
                    int k = 0;
                    while (!chararr[j].Equals((char)k))
                    {
                        k++;
                        if (k > 255)
                        {
                            j++;
                        }
                    }

                    charbyte[j] = (byte)k;
                    j++;
                }

                Moff = loaddata.FindSequence(Gt5Save, charbyte) - 306;
                if (Operators.ConditionalCompareObjectLess(Moff, 1, false))
                {
                    Interaction.MsgBox(
                        "PSN name is incorrect or data is corrupt\rRemember, the PSN name is CASE sensitive.");
                    return;
                }

                try
                {
                    FileInfo gt50file = new FileInfo(TextBox1.Text);
                    RichTextBox rtf = new RichTextBox();
                    rtf.LoadFile(gt50file.Directory.FullName + "\\PARAM.SFO", RichTextBoxStreamType.PlainText);
                    Label7.Text = rtf.Lines[4];
                    Carname = Label7.Text.Substring(13, Label7.Text.Length - 13);
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox("Can't get param.sfo for loading car name");
                }

                TextBox3.Text =
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 213))].ToString("X2") + " " +
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 212))].ToString("X2");
                TextBox4.Text = Conversions.ToString(Conversion.Val("&H" +
                                                                    Gt5Save[
                                                                            Conversions.ToInteger(
                                                                                Operators.SubtractObject(Moff, 46))]
                                                                        .ToString("X2")));
                TextBox6.Text =
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 209))].ToString("X2") + " " +
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 208))].ToString("X2");
                TextBox7.Text =
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 217))].ToString("X2") + " " +
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 216))].ToString("X2");
                TextBox8.Text =
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 205))].ToString("X2") + " " +
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 204))].ToString("X2");
                TextBox9.Text =
                    Conversions.ToString(Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 88))]);
                TextBox10.Text = string.Concat(
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 344))].ToString("X2"), " ",
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 343))].ToString("X2"), " ",
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 342))].ToString("X2"), " ",
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 341))].ToString("X2"));
                TextBox11.Text = string.Concat(
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 171))].ToString("X2"), " ",
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 170))].ToString("X2"), " ",
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 169))].ToString("X2"), " ",
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 168))].ToString("X2"));
                TextBox13.Text =
                    Conversions.ToString(Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 43))]);
                TextBox12.Text =
                    Conversions.ToString(Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 42))]);
                TextBox textBox = TextBox14;
                string text = "&H";
                byte b = Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 33))];
                string text2 = b.ToString("X2");
                string text3 = " ";
                byte b2 = Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 32))];
                textBox.Text = Conversions.ToString(Conversion.Val(text + text2 + text3 + b2.ToString("X2")));
                TextBox textBox2 = TextBox15;
                string text4 = "&H";
                b2 = Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 31))];
                string text5 = b2.ToString("X2");
                string text6 = " ";
                b = Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 30))];
                textBox2.Text = Conversions.ToString(Conversion.Val(text4 + text5 + text6 + b.ToString("X2")));
                TextBox16.Text =
                    Conversions.ToString(Gt5Save[Conversions.ToInteger(Operators.AddObject(Moff, 10))]);
                TextBox18.Text =
                    Conversions.ToString(Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 27))]);
                TextBox17.Text =
                    Conversions.ToString(Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 26))]);
                TextBox19.Text = string.Concat(
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 155))].ToString("X2"), " ",
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 154))].ToString("X2"), " ",
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 153))].ToString("X2"), " ",
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 152))].ToString("X2"));
                TextBox20.Text =
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 262))].ToString("X2") + " " +
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 261))].ToString("X2");
                TextBox21.Text =
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 201))].ToString("X2") + " " +
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 200))].ToString("X2");
                TextBox22.Text =
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 197))].ToString("X2") + " " +
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 196))].ToString("X2");
                TextBox23.Text =
                    Gt5Save[Conversions.ToInteger(Operators.AddObject(Moff, 23))].ToString("X2") + " " +
                    Gt5Save[Conversions.ToInteger(Operators.AddObject(Moff, 24))].ToString("X2");
                TextBox24.Text = string.Concat(
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 191))].ToString("X2"), " ",
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 190))].ToString("X2"), " ",
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 189))].ToString("X2"), " ",
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 188))].ToString("X2"));
            }
        }

        // Token: 0x06000142 RID: 322 RVA: 0x0000BC04 File Offset: 0x0000A004
        private void savedata()
        {
            checked
            {
                try
                {
                    string working = TextBox3.Text;
                    working = working.Replace(" ", "");
                    object[] bytes = new object[2];
                    int num = 0;
                    int num2 = bytes.Length - 1;
                    for (int i = num; i <= num2; i++)
                    {
                        bytes[i] = working.Substring(i * 2, 2);
                        bytes[i] = Conversion.Val(Operators.ConcatenateObject("&H", bytes[i]));
                    }

                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 213))] =
                        Conversions.ToByte(bytes[0]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 212))] =
                        Conversions.ToByte(bytes[1]);
                }
                catch (Exception ex)
                {
                }

                if ((Conversion.Val(TextBox4.Text) > 255.0) | (Conversion.Val(TextBox4.Text) < 0.0))
                {
                    TextBox4.Text = Conversions.ToString(255);
                    Interaction.MsgBox("Torque split cannot be higher than 255 or lower than 0. Reverting back to 255");
                }

                try
                {
                    string working2 = TextBox4.Text;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 46))] =
                        Conversions.ToByte(working2);
                }
                catch (Exception ex2)
                {
                }

                try
                {
                    string working3 = TextBox6.Text;
                    working3 = working3.Replace(" ", "");
                    object[] bytes2 = new object[2];
                    int num3 = 0;
                    int num4 = bytes2.Length - 1;
                    for (int j = num3; j <= num4; j++)
                    {
                        bytes2[j] = working3.Substring(j * 2, 2);
                        bytes2[j] = Conversion.Val(Operators.ConcatenateObject("&H", bytes2[j]));
                    }

                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 209))] =
                        Conversions.ToByte(bytes2[0]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 208))] =
                        Conversions.ToByte(bytes2[1]);
                }
                catch (Exception ex3)
                {
                }

                try
                {
                    string working4 = TextBox7.Text;
                    working4 = working4.Replace(" ", "");
                    object[] bytes3 = new object[2];
                    int num5 = 0;
                    int num6 = bytes3.Length - 1;
                    for (int k = num5; k <= num6; k++)
                    {
                        bytes3[k] = working4.Substring(k * 2, 2);
                        bytes3[k] = Conversion.Val(Operators.ConcatenateObject("&H", bytes3[k]));
                    }

                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 217))] =
                        Conversions.ToByte(bytes3[0]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 216))] =
                        Conversions.ToByte(bytes3[1]);
                }
                catch (Exception ex4)
                {
                }

                try
                {
                    string working5 = TextBox8.Text;
                    working5 = working5.Replace(" ", "");
                    object[] bytes4 = new object[2];
                    int num7 = 0;
                    int num8 = bytes4.Length - 1;
                    for (int l = num7; l <= num8; l++)
                    {
                        bytes4[l] = working5.Substring(l * 2, 2);
                        bytes4[l] = Conversion.Val(Operators.ConcatenateObject("&H", bytes4[l]));
                    }

                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 205))] =
                        Conversions.ToByte(bytes4[0]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 204))] =
                        Conversions.ToByte(bytes4[1]);
                }
                catch (Exception ex5)
                {
                }

                if ((Conversion.Val(TextBox9.Text) > 255.0) | (Conversion.Val(TextBox9.Text) < 0.0))
                {
                    TextBox9.Text = Conversions.ToString(0);
                    Interaction.MsgBox(
                        "Remove wing value cannot be higher than 255 or lower than 0. Reverting back to 0");
                }

                Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 88))] =
                    Conversions.ToByte(TextBox9.Text);
                try
                {
                    string working6 = TextBox10.Text;
                    working6 = working6.Replace(" ", "");
                    object[] bytes5 = new object[4];
                    int num9 = 0;
                    int num10 = bytes5.Length - 1;
                    for (int m = num9; m <= num10; m++)
                    {
                        bytes5[m] = working6.Substring(m * 2, 2);
                        bytes5[m] = Conversion.Val(Operators.ConcatenateObject("&H", bytes5[m]));
                    }

                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 344))] =
                        Conversions.ToByte(bytes5[0]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 343))] =
                        Conversions.ToByte(bytes5[1]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 342))] =
                        Conversions.ToByte(bytes5[2]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 341))] =
                        Conversions.ToByte(bytes5[3]);
                }
                catch (Exception ex6)
                {
                }

                try
                {
                    string working7 = TextBox11.Text;
                    working7 = working7.Replace(" ", "");
                    object[] bytes6 = new object[4];
                    int num11 = 0;
                    int num12 = bytes6.Length - 1;
                    for (int n = num11; n <= num12; n++)
                    {
                        bytes6[n] = working7.Substring(n * 2, 2);
                        bytes6[n] = Conversion.Val(Operators.ConcatenateObject("&H", bytes6[n]));
                    }

                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 171))] =
                        Conversions.ToByte(bytes6[0]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 170))] =
                        Conversions.ToByte(bytes6[1]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 169))] =
                        Conversions.ToByte(bytes6[2]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 168))] =
                        Conversions.ToByte(bytes6[3]);
                }
                catch (Exception ex7)
                {
                }

                if (CheckBox2.Checked)
                {
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 332))] = 56;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 331))] = 9;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 330))] = 8;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 315))] = 80;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 314))] = 9;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 179))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 178))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 177))] = 5;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 176))] = 81;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 175))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 174))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 173))] = 14;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 172))] = 242;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 171))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 170))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 169))] = 21;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 168))] = 39;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 155))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 154))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 153))] = 20;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 152))] = 60;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 131))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 130))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 129))] = 3;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 128))] = 88;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 127))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 126))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 125))] = 3;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 124))] = 88;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 123))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 122))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 121))] = 3;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 120))] = 56;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 119))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 118))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 117))] = 3;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 116))] = 61;
                }

                if ((Conversion.Val(TextBox13.Text) > 255.0) | (Conversion.Val(TextBox13.Text) < 0.0))
                {
                    TextBox13.Text = Conversions.ToString(0);
                    Interaction.MsgBox("Front Aero cannot be higher than 255 or lower than 0. Reverting back to 0");
                }

                if ((Conversion.Val(TextBox12.Text) > 255.0) | (Conversion.Val(TextBox12.Text) < 0.0))
                {
                    TextBox12.Text = Conversions.ToString(0);
                    Interaction.MsgBox("Rear Aero cannot be higher than 255 or lower than 0. Reverting back to 0");
                }

                Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 43))] =
                    Conversions.ToByte(TextBox13.Text);
                Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 42))] =
                    Conversions.ToByte(TextBox12.Text);
                try
                {
                    string working8 = TextBox14.Text;
                    working8 = Conversions.ToString(Conversion.Val(working8));
                    int @int = Conversions.ToInteger(Conversion.Int(working8));
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 33))] =
                        (byte)Math.Round(Conversion.Val("&H" + @int.ToString("X4").Substring(0, 2)));
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 32))] =
                        (byte)Math.Round(Conversion.Val("&H" + @int.ToString("X4").Substring(2, 2)));
                }
                catch (Exception ex8)
                {
                }

                try
                {
                    string working9 = TextBox15.Text;
                    working9 = Conversions.ToString(Conversion.Val(working9));
                    int int2 = Conversions.ToInteger(Conversion.Int(working9));
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 31))] =
                        (byte)Math.Round(Conversion.Val("&H" + int2.ToString("X4").Substring(0, 2)));
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 30))] =
                        (byte)Math.Round(Conversion.Val("&H" + int2.ToString("X4").Substring(2, 2)));
                }
                catch (Exception ex9)
                {
                }

                Gt5Save[Conversions.ToInteger(Operators.AddObject(Moff, 10))] =
                    Conversions.ToByte(TextBox16.Text);
                Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 27))] =
                    Conversions.ToByte(TextBox18.Text);
                Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 26))] =
                    Conversions.ToByte(TextBox17.Text);
                try
                {
                    string working10 = TextBox19.Text;
                    working10 = working10.Replace(" ", "");
                    object[] bytes7 = new object[4];
                    int num13 = 0;
                    int num14 = bytes7.Length - 1;
                    for (int i2 = num13; i2 <= num14; i2++)
                    {
                        bytes7[i2] = working10.Substring(i2 * 2, 2);
                        bytes7[i2] = Conversion.Val(Operators.ConcatenateObject("&H", bytes7[i2]));
                    }

                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 155))] =
                        Conversions.ToByte(bytes7[0]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 154))] =
                        Conversions.ToByte(bytes7[1]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 153))] =
                        Conversions.ToByte(bytes7[2]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 152))] =
                        Conversions.ToByte(bytes7[3]);
                }
                catch (Exception ex10)
                {
                }

                try
                {
                    string working11 = TextBox20.Text;
                    working11 = working11.Replace(" ", "");
                    object[] bytes8 = new object[2];
                    int num15 = 0;
                    int num16 = bytes8.Length - 1;
                    for (int i3 = num15; i3 <= num16; i3++)
                    {
                        bytes8[i3] = working11.Substring(i3 * 2, 2);
                        bytes8[i3] = Conversion.Val(Operators.ConcatenateObject("&H", bytes8[i3]));
                    }

                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 262))] =
                        Conversions.ToByte(bytes8[0]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 261))] =
                        Conversions.ToByte(bytes8[1]);
                }
                catch (Exception ex11)
                {
                }

                try
                {
                    string working12 = TextBox21.Text;
                    working12 = working12.Replace(" ", "");
                    object[] bytes9 = new object[2];
                    int num17 = 0;
                    int num18 = bytes9.Length - 1;
                    for (int i4 = num17; i4 <= num18; i4++)
                    {
                        bytes9[i4] = working12.Substring(i4 * 2, 2);
                        bytes9[i4] = Conversion.Val(Operators.ConcatenateObject("&H", bytes9[i4]));
                    }

                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 201))] =
                        Conversions.ToByte(bytes9[0]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 200))] =
                        Conversions.ToByte(bytes9[1]);
                }
                catch (Exception ex12)
                {
                }

                try
                {
                    string working13 = TextBox22.Text;
                    working13 = working13.Replace(" ", "");
                    object[] bytes10 = new object[2];
                    int num19 = 0;
                    int num20 = bytes10.Length - 1;
                    for (int i5 = num19; i5 <= num20; i5++)
                    {
                        bytes10[i5] = working13.Substring(i5 * 2, 2);
                        bytes10[i5] = Conversion.Val(Operators.ConcatenateObject("&H", bytes10[i5]));
                    }

                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 197))] =
                        Conversions.ToByte(bytes10[0]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 196))] =
                        Conversions.ToByte(bytes10[1]);
                }
                catch (Exception ex13)
                {
                }

                try
                {
                    string working14 = TextBox23.Text;
                    working14 = working14.Replace(" ", "");
                    object[] bytes11 = new object[2];
                    int num21 = 0;
                    int num22 = bytes11.Length - 1;
                    for (int i6 = num21; i6 <= num22; i6++)
                    {
                        bytes11[i6] = working14.Substring(i6 * 2, 2);
                        bytes11[i6] = Conversion.Val(Operators.ConcatenateObject("&H", bytes11[i6]));
                    }

                    Gt5Save[Conversions.ToInteger(Operators.AddObject(Moff, 23))] =
                        Conversions.ToByte(bytes11[0]);
                    Gt5Save[Conversions.ToInteger(Operators.AddObject(Moff, 24))] =
                        Conversions.ToByte(bytes11[1]);
                }
                catch (Exception ex14)
                {
                }

                try
                {
                    string working15 = TextBox24.Text;
                    working15 = working15.Replace(" ", "");
                    object[] bytes12 = new object[4];
                    int num23 = 0;
                    int num24 = bytes12.Length - 1;
                    for (int i7 = num23; i7 <= num24; i7++)
                    {
                        bytes12[i7] = working15.Substring(i7 * 2, 2);
                        bytes12[i7] = Conversion.Val(Operators.ConcatenateObject("&H", bytes12[i7]));
                    }

                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 191))] =
                        Conversions.ToByte(bytes12[0]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 190))] =
                        Conversions.ToByte(bytes12[1]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 189))] =
                        Conversions.ToByte(bytes12[2]);
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 188))] =
                        Conversions.ToByte(bytes12[3]);
                }
                catch (Exception ex15)
                {
                }

                if (CheckBox1.Checked)
                {
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 260))] = 116;
                }

                if (CheckBox3.Checked)
                {
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 260))] = 76;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 259))] = 85;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 258))] = 67;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 257))] = 65;
                }

                if (CheckBox5.Checked)
                {
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 313))] = byte.MaxValue;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 107))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 106))] = 52;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 105))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 104))] = 3;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 311))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 83))] = byte.MaxValue;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 82))] = byte.MaxValue;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 81))] = byte.MaxValue;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 80))] = byte.MaxValue;
                }

                if (CheckBox7.Checked)
                {
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 367))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 103))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 102))] = 52;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 101))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 100))] = 2;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 311))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 83))] = byte.MaxValue;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 82))] = byte.MaxValue;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 81))] = byte.MaxValue;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 80))] = byte.MaxValue;
                }

                if (CheckBox6.Checked)
                {
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 367))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 99))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 98))] = 52;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 97))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 96))] = 2;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 311))] = 0;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 83))] = byte.MaxValue;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 82))] = byte.MaxValue;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 81))] = byte.MaxValue;
                    Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 80))] = byte.MaxValue;
                }

                File.WriteAllBytes(TextBox1.Text, Gt5Save);
            }
        }

        // Token: 0x06000143 RID: 323 RVA: 0x0000D6D8 File Offset: 0x0000BAD8
        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Find your GT5.0 file";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                TextBox1.Text = ofd.FileName;
            }
        }

        // Token: 0x06000144 RID: 324 RVA: 0x0000D710 File Offset: 0x0000BB10
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!MyProject.Computer.FileSystem.DirectoryExists("Backups"))
            {
                MyProject.Computer.FileSystem.CreateDirectory("Backups");
            }

            Progsettings = Settingsfileclass.loadsettings("GT5CHWsettings.ini", 1);
            Carparts = Settingsfileclass.loadsettings("partsdatabase.db", 9999);
            TextBox1.Text = Progsettings[0];
            TextBox2.Text = Progsettings[1];
            LoadParts();
        }

        // Token: 0x06000145 RID: 325 RVA: 0x0000D7A4 File Offset: 0x0000BBA4
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Progsettings[0] = TextBox1.Text;
            Progsettings[1] = TextBox2.Text;
            Settingsfileclass.savesettings(Progsettings, "GT5CHWsettings.ini");
            Settingsfileclass.savesettings(Carparts, "partsdatabase.db");
        }

        // Token: 0x06000146 RID: 326 RVA: 0x0000D7F8 File Offset: 0x0000BBF8
        private void Button2_Click(object sender, EventArgs e)
        {
            FileInfo fi = new FileInfo(TextBox1.Text);
            string tad = DateAndTime.Now.ToString();
            tad = tad.Replace("/", "-");
            tad = tad.Replace(":", ".");
            MyProject.Computer.FileSystem.CopyFile(TextBox1.Text, "Backups\\" + tad + " " + fi.Name);
            ProcessData();
        }

        // Token: 0x06000147 RID: 327 RVA: 0x0000D87C File Offset: 0x0000BC7C
        private void Button3_Click(object sender, EventArgs e)
        {
            savedata();
            loaddata.encrypt(TextBox1.Text);
            Interaction.MsgBox("Data saved, immediately goto the GT Auto and change the cars oil to apply the hacks.");
        }

        // Token: 0x06000148 RID: 328 RVA: 0x0000D8A4 File Offset: 0x0000BCA4
        private void Button5_Click(object sender, EventArgs e)
        {
            savedata();
        }

        // Token: 0x06000149 RID: 329 RVA: 0x0000D8AC File Offset: 0x0000BCAC
        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            TextBox5.Text = Conversions.ToString(100.0 - Conversion.Val(TextBox4.Text));
        }

        // Token: 0x0600014A RID: 330 RVA: 0x0000D8D8 File Offset: 0x0000BCD8
        private void Button6_Click(object sender, EventArgs e)
        {
            TextBox4.Text = Conversions.ToString(50);
            TextBox6.Text = "0A 1E";
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
        /// Add current car to parts database.
        /// Handles the event triggered when button12 is clicked. This method is used to add the current car details
        /// to the parts database. It validates for available space in the database and ensures no duplicate entries exist.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs object containing event data.</param>
        private void Button12_Click(object sender, EventArgs e)
        {
            // cpos is used to find the next empty slot in the car parts database to place the newly added car parts
            int cpos = -1;
            for (int i = 0; i <= Carparts.Length - 1; i++)
            {
                if (Carparts[i] == " ")
                {
                    cpos = i;
                    break;
                }
            }

            if (cpos == -1)
            {
                MessageBox.Show("No more space in the database, please delete some cars first");
                return;
            }

            Carname = Interaction.InputBox("Car name:", "", Conversions.ToString(Carname));

            string linetoadd =
                Carname + "," +
                TextBox3.Text + "," +
                TextBox6.Text + "," +
                TextBox7.Text + "," +
                TextBox8.Text + "," +
                TextBox20.Text + "," +
                TextBox21.Text + "," +
                TextBox22.Text + "," +
                TextBox23.Text;

            for (int j = 0; j <= Carparts.Length - 1; j++)
            {
                if (Carparts[j].Equals(linetoadd, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Car already exists", "", MessageBoxButtons.OK);
                }
            }

            Carparts[cpos] = linetoadd;
            LoadParts();
        }

        private void LoadParts()
        {
            // Assuming all ComboBoxes are part of the same container (like the form or a group box)
            foreach (var comboBox in new[] { ComboBox1, ComboBox2, ComboBox3, ComboBox4, ComboBox5, ComboBox6, ComboBox7, ComboBox8 })
            {
                comboBox.Items.Clear();
            }

            // Spacer is used so that the hex value data after the comma doesn't show up in the GUI (Yep, terrible) TODO: Fix
            var spacer = "                                                                                                                                            ,";
            for (var i = 0; i <= Carparts.Length - 1; i++)
            {
                if (Carparts[i] != " ")
                {
                    try
                    {
                        var sparr = Carparts[i].Split(',');
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
            }
        }

        // Token: 0x06000153 RID: 339 RVA: 0x0000DDF0 File Offset: 0x0000C1F0
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string[] sparr = ComboBox1.SelectedItem.ToString().Split(',');
                TextBox3.Text = sparr[1];
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
                string[] sparr = ComboBox2.SelectedItem.ToString().Split(',');
                TextBox6.Text = sparr[1];
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
                string[] sparr = ComboBox3.SelectedItem.ToString().Split(',');
                TextBox7.Text = sparr[1];
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
                string[] sparr = ComboBox4.SelectedItem.ToString().Split(',');
                TextBox8.Text = sparr[1];
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
                string[] sparr = ComboBox5.SelectedItem.ToString().Split(',');
                TextBox21.Text = sparr[1];
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
                string[] sparr = ComboBox6.SelectedItem.ToString().Split(',');
                TextBox20.Text = sparr[1];
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
                string[] sparr = ComboBox7.SelectedItem.ToString().Split(',');
                TextBox22.Text = sparr[1];
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
                string[] sparr = ComboBox8.SelectedItem.ToString().Split(',');
                TextBox23.Text = sparr[1];
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
            savedata();
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
            savedata();
        }

        // Token: 0x06000161 RID: 353 RVA: 0x0000E228 File Offset: 0x0000C628
        private void SlamCar_Button19_Click(object sender, EventArgs e)
        {
            Gt5Save[Moff - 25] = 0;
            Gt5Save[Moff - 24] = 0;
        }

        // Token: 0x06000162 RID: 354 RVA: 0x0000E268 File Offset: 0x0000C668
        private void Button20_Click(object sender, EventArgs e)
        {
            Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 355))] = 0;
            Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 332))] = 2;
        }

        // Token: 0x06000163 RID: 355 RVA: 0x0000E2BC File Offset: 0x0000C6BC
        private void Button21_Click(object sender, EventArgs e)
        {
            Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 355))] = 1;
            Gt5Save[Conversions.ToInteger(Operators.SubtractObject(Moff, 332))] = 0;
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

        // Token: 0x0400001F RID: 31

        // Token: 0x04000020 RID: 32
        [AccessedThroughProperty("Button1")] private Button _Button1;

        // Token: 0x04000022 RID: 34

        // Token: 0x04000023 RID: 35
        [AccessedThroughProperty("Button3")] private Button _Button3;

        // Token: 0x04000024 RID: 36
        [AccessedThroughProperty("Button2")] private Button _Button2;

        // Token: 0x04000025 RID: 37

        // Token: 0x04000026 RID: 38

        // Token: 0x04000027 RID: 39

        // Token: 0x04000028 RID: 40

        // Token: 0x04000029 RID: 41

        // Token: 0x0400002A RID: 42

        // Token: 0x0400002B RID: 43

        // Token: 0x0400002C RID: 44

        // Token: 0x0400002D RID: 45

        // Token: 0x0400002E RID: 46

        // Token: 0x0400002F RID: 47
        [AccessedThroughProperty("Button5")] private Button _Button5;

        // Token: 0x04000030 RID: 48

        // Token: 0x04000031 RID: 49

        // Token: 0x04000032 RID: 50
        [AccessedThroughProperty("TextBox4")] private TextBox _TextBox4;

        // Token: 0x04000033 RID: 51

        // Token: 0x04000034 RID: 52

        // Token: 0x04000035 RID: 53

        // Token: 0x04000036 RID: 54
        [AccessedThroughProperty("Button6")] private Button _Button6;

        // Token: 0x04000037 RID: 55

        // Token: 0x04000038 RID: 56

        // Token: 0x04000039 RID: 57

        // Token: 0x0400003A RID: 58

        // Token: 0x0400003B RID: 59
        [AccessedThroughProperty("Button4")] private Button _Button4;

        // Token: 0x0400003C RID: 60
        [AccessedThroughProperty("Button7")] private Button _Button7;

        // Token: 0x0400003D RID: 61
        [AccessedThroughProperty("Button8")] private Button _Button8;

        // Token: 0x0400003E RID: 62
        [AccessedThroughProperty("Button9")] private Button _Button9;

        // Token: 0x0400003F RID: 63

        // Token: 0x04000040 RID: 64

        // Token: 0x04000041 RID: 65

        // Token: 0x04000042 RID: 66

        // Token: 0x04000043 RID: 67

        // Token: 0x04000044 RID: 68

        // Token: 0x04000045 RID: 69

        // Token: 0x04000046 RID: 70
        [AccessedThroughProperty("Button10")] private Button _Button10;

        // Token: 0x04000047 RID: 71

        // Token: 0x04000048 RID: 72

        // Token: 0x04000049 RID: 73

        // Token: 0x0400004A RID: 74
        [AccessedThroughProperty("Button11")] private Button _Button11;

        // Token: 0x0400004B RID: 75

        // Token: 0x0400004C RID: 76

        // Token: 0x0400004D RID: 77

        // Token: 0x0400004E RID: 78

        // Token: 0x0400004F RID: 79

        // Token: 0x04000050 RID: 80

        // Token: 0x04000051 RID: 81

        // Token: 0x04000052 RID: 82

        // Token: 0x04000053 RID: 83

        // Token: 0x04000054 RID: 84

        // Token: 0x04000055 RID: 85

        // Token: 0x04000056 RID: 86

        // Token: 0x04000057 RID: 87

        // Token: 0x04000058 RID: 88

        // Token: 0x04000059 RID: 89

        // Token: 0x0400005A RID: 90

        // Token: 0x0400005B RID: 91

        // Token: 0x0400005C RID: 92

        // Token: 0x0400005D RID: 93
        [AccessedThroughProperty("ComboBox2")] private ComboBox _ComboBox2;

        // Token: 0x0400005E RID: 94

        // Token: 0x0400005F RID: 95
        [AccessedThroughProperty("ComboBox1")] private ComboBox _ComboBox1;

        // Token: 0x04000060 RID: 96

        // Token: 0x04000061 RID: 97
        [AccessedThroughProperty("ComboBox6")] private ComboBox _ComboBox6;

        // Token: 0x04000062 RID: 98

        // Token: 0x04000063 RID: 99
        [AccessedThroughProperty("ComboBox5")] private ComboBox _ComboBox5;

        // Token: 0x04000064 RID: 100

        // Token: 0x04000065 RID: 101
        [AccessedThroughProperty("ComboBox4")] private ComboBox _ComboBox4;

        // Token: 0x04000066 RID: 102

        // Token: 0x04000067 RID: 103
        [AccessedThroughProperty("ComboBox3")] private ComboBox _ComboBox3;

        // Token: 0x04000068 RID: 104

        // Token: 0x04000069 RID: 105
        [AccessedThroughProperty("ComboBox8")] private ComboBox _ComboBox8;

        // Token: 0x0400006A RID: 106

        // Token: 0x0400006B RID: 107
        [AccessedThroughProperty("ComboBox7")] private ComboBox _ComboBox7;

        // Token: 0x0400006C RID: 108
        [AccessedThroughProperty("Button12")] private Button _Button12;

        // Token: 0x0400006D RID: 109

        // Token: 0x0400006E RID: 110

        // Token: 0x0400006F RID: 111

        // Token: 0x04000070 RID: 112

        // Token: 0x04000071 RID: 113

        // Token: 0x04000072 RID: 114

        // Token: 0x04000073 RID: 115

        // Token: 0x04000074 RID: 116

        // Token: 0x04000075 RID: 117

        // Token: 0x04000076 RID: 118

        // Token: 0x04000077 RID: 119

        // Token: 0x04000078 RID: 120
        [AccessedThroughProperty("Button13")] private Button _Button13;

        // Token: 0x04000079 RID: 121

        // Token: 0x0400007A RID: 122
        [AccessedThroughProperty("Button14")] private Button _Button14;

        // Token: 0x0400007B RID: 123

        // Token: 0x0400007C RID: 124
        [AccessedThroughProperty("Button15")] private Button _Button15;

        // Token: 0x0400007D RID: 125

        // Token: 0x0400007E RID: 126

        // Token: 0x0400007F RID: 127

        // Token: 0x04000080 RID: 128

        // Token: 0x04000081 RID: 129

        // Token: 0x04000082 RID: 130

        // Token: 0x04000083 RID: 131

        // Token: 0x04000084 RID: 132

        // Token: 0x04000085 RID: 133

        // Token: 0x04000086 RID: 134
        [AccessedThroughProperty("Button16")] private Button _Button16;

        // Token: 0x04000087 RID: 135
        [AccessedThroughProperty("Button17")] private Button _Button17;

        // Token: 0x04000088 RID: 136

        // Token: 0x04000089 RID: 137

        // Token: 0x0400008A RID: 138

        // Token: 0x0400008B RID: 139

        // Token: 0x0400008C RID: 140

        // Token: 0x0400008D RID: 141
        [AccessedThroughProperty("Button18")] private Button _Button18;

        // Token: 0x0400008E RID: 142
        [AccessedThroughProperty("Button19")] private Button _Button19;

        // Token: 0x0400008F RID: 143

        // Token: 0x04000090 RID: 144
        [AccessedThroughProperty("Button21")] private Button _Button21;

        // Token: 0x04000091 RID: 145
        [AccessedThroughProperty("Button20")] private Button _Button20;

        // Token: 0x04000092 RID: 146

        // Token: 0x04000093 RID: 147
        [AccessedThroughProperty("Button22")] private Button _Button22;

        // Token: 0x04000094 RID: 148

        // Token: 0x04000095 RID: 149

        // Token: 0x04000096 RID: 150

        // Token: 0x04000097 RID: 151
        [AccessedThroughProperty("Button23")] private Button _Button23;

        // Token: 0x04000098 RID: 152
        /// <summary>
        /// Represents an array of car parts, utilized for storing and managing saved car part values.
        /// </summary>
        /// <remarks>
        /// This array has a capacity of up to 10,000 slots for car part data.
        /// Users can add and update car part details within this array, which is persistently saved and loaded
        /// from a database file ("partsdatabase.db") during program execution.
        /// It is primarily used within the application to display and modify vehicle part information.
        /// </remarks>
        public string[] Carparts;

        // Token: 0x04000099 RID: 153
        public string[] Progsettings;

        // Token: 0x0400009A RID: 154
        public byte[] Gt5Save;

        // Token: 0x0400009B RID: 155
        public int Moff;

        // Token: 0x0400009C RID: 156
        public string Carname;
    }
}