using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GT5_Car_hack_workshop_2.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace GT5_Car_hack_workshop_2
{
    [DesignerGenerated]
    public partial class Tranny : Form
    {
        // Token: 0x040000BB RID: 187
        [AccessedThroughProperty("Button1")] private Button _Button1;

        // Token: 0x040000BC RID: 188

        // Token: 0x040000A3 RID: 163

        // Token: 0x040000AC RID: 172

        // Token: 0x040000AD RID: 173

        // Token: 0x040000BA RID: 186

        // Token: 0x040000C0 RID: 192

        // Token: 0x040000BE RID: 190

        // Token: 0x040000A4 RID: 164

        // Token: 0x040000A5 RID: 165

        // Token: 0x040000A6 RID: 166

        // Token: 0x040000A7 RID: 167

        // Token: 0x040000A8 RID: 168

        // Token: 0x040000A9 RID: 169

        // Token: 0x040000AA RID: 170

        // Token: 0x040000AB RID: 171

        // Token: 0x040000AE RID: 174

        // Token: 0x040000B7 RID: 183

        // Token: 0x040000B8 RID: 184

        // Token: 0x040000B9 RID: 185

        // Token: 0x040000BF RID: 191

        // Token: 0x040000BD RID: 189

        // Token: 0x040000AF RID: 175

        // Token: 0x040000B0 RID: 176

        // Token: 0x040000B1 RID: 177

        // Token: 0x040000B2 RID: 178

        // Token: 0x040000B3 RID: 179

        // Token: 0x040000B4 RID: 180

        // Token: 0x040000B5 RID: 181

        // Token: 0x040000B6 RID: 182

        // Token: 0x040000C1 RID: 193
        [AccessedThroughProperty("Timer1")] private Timer _Timer1;

        // Token: 0x040000C2 RID: 194
        private decimal lastgearpre;

        // Token: 0x040000C3 RID: 195
        private decimal origfg;

        public Tranny()
        {
            Load += Tranny_Load;
            lastgearpre = 0.1m;
            origfg = 0.1m;
            InitializeComponent();
        }

        // Token: 0x17000099 RID: 153
        // (get) Token: 0x06000179 RID: 377 RVA: 0x0000FABC File Offset: 0x0000DEBC
        // (set) Token: 0x0600017A RID: 378 RVA: 0x0000FAD0 File Offset: 0x0000DED0
        [field: AccessedThroughProperty("Label1")]
        internal virtual Label Label1
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x1700009A RID: 154
        // (get) Token: 0x0600017B RID: 379 RVA: 0x0000FADC File Offset: 0x0000DEDC
        // (set) Token: 0x0600017C RID: 380 RVA: 0x0000FAF0 File Offset: 0x0000DEF0
        [field: AccessedThroughProperty("Label2")]
        internal virtual Label Label2
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x1700009B RID: 155
        // (get) Token: 0x0600017D RID: 381 RVA: 0x0000FAFC File Offset: 0x0000DEFC
        // (set) Token: 0x0600017E RID: 382 RVA: 0x0000FB10 File Offset: 0x0000DF10
        [field: AccessedThroughProperty("Label3")]
        internal virtual Label Label3
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x1700009C RID: 156
        // (get) Token: 0x0600017F RID: 383 RVA: 0x0000FB1C File Offset: 0x0000DF1C
        // (set) Token: 0x06000180 RID: 384 RVA: 0x0000FB30 File Offset: 0x0000DF30
        [field: AccessedThroughProperty("Label4")]
        internal virtual Label Label4
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x1700009D RID: 157
        // (get) Token: 0x06000181 RID: 385 RVA: 0x0000FB3C File Offset: 0x0000DF3C
        // (set) Token: 0x06000182 RID: 386 RVA: 0x0000FB50 File Offset: 0x0000DF50
        [field: AccessedThroughProperty("Label5")]
        internal virtual Label Label5
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x1700009E RID: 158
        // (get) Token: 0x06000183 RID: 387 RVA: 0x0000FB5C File Offset: 0x0000DF5C
        // (set) Token: 0x06000184 RID: 388 RVA: 0x0000FB70 File Offset: 0x0000DF70
        [field: AccessedThroughProperty("Label6")]
        internal virtual Label Label6
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x1700009F RID: 159
        // (get) Token: 0x06000185 RID: 389 RVA: 0x0000FB7C File Offset: 0x0000DF7C
        // (set) Token: 0x06000186 RID: 390 RVA: 0x0000FB90 File Offset: 0x0000DF90
        [field: AccessedThroughProperty("Label7")]
        internal virtual Label Label7
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000A0 RID: 160
        // (get) Token: 0x06000187 RID: 391 RVA: 0x0000FB9C File Offset: 0x0000DF9C
        // (set) Token: 0x06000188 RID: 392 RVA: 0x0000FBB0 File Offset: 0x0000DFB0
        [field: AccessedThroughProperty("Label8")]
        internal virtual Label Label8
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000A1 RID: 161
        // (get) Token: 0x06000189 RID: 393 RVA: 0x0000FBBC File Offset: 0x0000DFBC
        // (set) Token: 0x0600018A RID: 394 RVA: 0x0000FBD0 File Offset: 0x0000DFD0
        [field: AccessedThroughProperty("Label9")]
        internal virtual Label Label9
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000A2 RID: 162
        // (get) Token: 0x0600018B RID: 395 RVA: 0x0000FBDC File Offset: 0x0000DFDC
        // (set) Token: 0x0600018C RID: 396 RVA: 0x0000FBF0 File Offset: 0x0000DFF0
        [field: AccessedThroughProperty("Label10")]
        internal virtual Label Label10
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000A3 RID: 163
        // (get) Token: 0x0600018D RID: 397 RVA: 0x0000FBFC File Offset: 0x0000DFFC
        // (set) Token: 0x0600018E RID: 398 RVA: 0x0000FC10 File Offset: 0x0000E010
        [field: AccessedThroughProperty("Label11")]
        internal virtual Label Label11
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000A4 RID: 164
        // (get) Token: 0x0600018F RID: 399 RVA: 0x0000FC1C File Offset: 0x0000E01C
        // (set) Token: 0x06000190 RID: 400 RVA: 0x0000FC30 File Offset: 0x0000E030
        [field: AccessedThroughProperty("TextBox1")]
        internal virtual TextBox TextBox1
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000A5 RID: 165
        // (get) Token: 0x06000191 RID: 401 RVA: 0x0000FC3C File Offset: 0x0000E03C
        // (set) Token: 0x06000192 RID: 402 RVA: 0x0000FC50 File Offset: 0x0000E050
        [field: AccessedThroughProperty("TextBox2")]
        internal virtual TextBox TextBox2
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000A6 RID: 166
        // (get) Token: 0x06000193 RID: 403 RVA: 0x0000FC5C File Offset: 0x0000E05C
        // (set) Token: 0x06000194 RID: 404 RVA: 0x0000FC70 File Offset: 0x0000E070
        [field: AccessedThroughProperty("TextBox3")]
        internal virtual TextBox TextBox3
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000A7 RID: 167
        // (get) Token: 0x06000195 RID: 405 RVA: 0x0000FC7C File Offset: 0x0000E07C
        // (set) Token: 0x06000196 RID: 406 RVA: 0x0000FC90 File Offset: 0x0000E090
        [field: AccessedThroughProperty("TextBox4")]
        internal virtual TextBox TextBox4
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000A8 RID: 168
        // (get) Token: 0x06000197 RID: 407 RVA: 0x0000FC9C File Offset: 0x0000E09C
        // (set) Token: 0x06000198 RID: 408 RVA: 0x0000FCB0 File Offset: 0x0000E0B0
        [field: AccessedThroughProperty("TextBox5")]
        internal virtual TextBox TextBox5
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000A9 RID: 169
        // (get) Token: 0x06000199 RID: 409 RVA: 0x0000FCBC File Offset: 0x0000E0BC
        // (set) Token: 0x0600019A RID: 410 RVA: 0x0000FCD0 File Offset: 0x0000E0D0
        [field: AccessedThroughProperty("TextBox6")]
        internal virtual TextBox TextBox6
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000AA RID: 170
        // (get) Token: 0x0600019B RID: 411 RVA: 0x0000FCDC File Offset: 0x0000E0DC
        // (set) Token: 0x0600019C RID: 412 RVA: 0x0000FCF0 File Offset: 0x0000E0F0
        [field: AccessedThroughProperty("TextBox7")]
        internal virtual TextBox TextBox7
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000AB RID: 171
        // (get) Token: 0x0600019D RID: 413 RVA: 0x0000FCFC File Offset: 0x0000E0FC
        // (set) Token: 0x0600019E RID: 414 RVA: 0x0000FD10 File Offset: 0x0000E110
        [field: AccessedThroughProperty("TextBox8")]
        internal virtual TextBox TextBox8
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000AC RID: 172
        // (get) Token: 0x0600019F RID: 415 RVA: 0x0000FD1C File Offset: 0x0000E11C
        // (set) Token: 0x060001A0 RID: 416 RVA: 0x0000FD30 File Offset: 0x0000E130
        [field: AccessedThroughProperty("TextBox9")]
        internal virtual TextBox TextBox9
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000AD RID: 173
        // (get) Token: 0x060001A1 RID: 417 RVA: 0x0000FD3C File Offset: 0x0000E13C
        // (set) Token: 0x060001A2 RID: 418 RVA: 0x0000FD50 File Offset: 0x0000E150
        [field: AccessedThroughProperty("TextBox10")]
        internal virtual TextBox TextBox10
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000AE RID: 174
        // (get) Token: 0x060001A3 RID: 419 RVA: 0x0000FD5C File Offset: 0x0000E15C
        // (set) Token: 0x060001A4 RID: 420 RVA: 0x0000FD70 File Offset: 0x0000E170
        [field: AccessedThroughProperty("TextBox11")]
        internal virtual TextBox TextBox11
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000AF RID: 175
        // (get) Token: 0x060001A5 RID: 421 RVA: 0x0000FD7C File Offset: 0x0000E17C
        // (set) Token: 0x060001A6 RID: 422 RVA: 0x0000FD90 File Offset: 0x0000E190
        [field: AccessedThroughProperty("TextBox12")]
        internal virtual TextBox TextBox12
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000B0 RID: 176
        // (get) Token: 0x060001A7 RID: 423 RVA: 0x0000FD9C File Offset: 0x0000E19C
        // (set) Token: 0x060001A8 RID: 424 RVA: 0x0000FDB0 File Offset: 0x0000E1B0
        [field: AccessedThroughProperty("Label12")]
        internal virtual Label Label12
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000B1 RID: 177
        // (get) Token: 0x060001A9 RID: 425 RVA: 0x0000FDBC File Offset: 0x0000E1BC
        // (set) Token: 0x060001AA RID: 426 RVA: 0x0000FDD0 File Offset: 0x0000E1D0
        internal virtual Button Button1
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

        // Token: 0x170000B2 RID: 178
        // (get) Token: 0x060001AB RID: 427 RVA: 0x0000FE1C File Offset: 0x0000E21C
        // (set) Token: 0x060001AC RID: 428 RVA: 0x0000FE30 File Offset: 0x0000E230
        [field: AccessedThroughProperty("GroupBox1")]
        internal virtual GroupBox GroupBox1
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000B3 RID: 179
        // (get) Token: 0x060001AD RID: 429 RVA: 0x0000FE3C File Offset: 0x0000E23C
        // (set) Token: 0x060001AE RID: 430 RVA: 0x0000FE50 File Offset: 0x0000E250
        [field: AccessedThroughProperty("TextBox14")]
        internal virtual TextBox TextBox14
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000B4 RID: 180
        // (get) Token: 0x060001AF RID: 431 RVA: 0x0000FE5C File Offset: 0x0000E25C
        // (set) Token: 0x060001B0 RID: 432 RVA: 0x0000FE70 File Offset: 0x0000E270
        [field: AccessedThroughProperty("Label14")]
        internal virtual Label Label14
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000B5 RID: 181
        // (get) Token: 0x060001B1 RID: 433 RVA: 0x0000FE7C File Offset: 0x0000E27C
        // (set) Token: 0x060001B2 RID: 434 RVA: 0x0000FE90 File Offset: 0x0000E290
        [field: AccessedThroughProperty("TextBox13")]
        internal virtual TextBox TextBox13
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000B6 RID: 182
        // (get) Token: 0x060001B3 RID: 435 RVA: 0x0000FE9C File Offset: 0x0000E29C
        // (set) Token: 0x060001B4 RID: 436 RVA: 0x0000FEB0 File Offset: 0x0000E2B0
        [field: AccessedThroughProperty("Label13")]
        internal virtual Label Label13
        {
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        // Token: 0x170000B7 RID: 183
        // (get) Token: 0x060001B5 RID: 437 RVA: 0x0000FEBC File Offset: 0x0000E2BC
        // (set) Token: 0x060001B6 RID: 438 RVA: 0x0000FED0 File Offset: 0x0000E2D0
        internal virtual Timer Timer1
        {
            get => _Timer1;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = Timer1_Tick;
                if (_Timer1 != null) _Timer1.Tick -= eventHandler;

                _Timer1 = value;
                if (_Timer1 != null) _Timer1.Tick += eventHandler;
            }
        }

        // Token: 0x060001B7 RID: 439 RVA: 0x0000FF1C File Offset: 0x0000E31C
        private void Tranny_Load(object sender, EventArgs e)
        {
            var textBox = TextBox1;
            var text = "&H";
            var b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 73))];
            var text2 = b.ToString("X2");
            var text3 = " ";
            var b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 72))];
            textBox.Text = Conversions.ToString(Conversion.Val(text + text2 + text3 + b2.ToString("X2")) / 1000.0);
            var textBox2 = TextBox2;
            var text4 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 71))];
            var text5 = b2.ToString("X2");
            var text6 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 70))];
            textBox2.Text = Conversions.ToString(Conversion.Val(text4 + text5 + text6 + b.ToString("X2")) / 1000.0);
            var textBox3 = TextBox3;
            var text7 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 69))];
            var text8 = b2.ToString("X2");
            var text9 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 68))];
            textBox3.Text = Conversions.ToString(Conversion.Val(text7 + text8 + text9 + b.ToString("X2")) / 1000.0);
            var textBox4 = TextBox4;
            var text10 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 67))];
            var text11 = b2.ToString("X2");
            var text12 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 66))];
            textBox4.Text = Conversions.ToString(Conversion.Val(text10 + text11 + text12 + b.ToString("X2")) / 1000.0);
            var textBox5 = TextBox5;
            var text13 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 65))];
            var text14 = b2.ToString("X2");
            var text15 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 64))];
            textBox5.Text = Conversions.ToString(Conversion.Val(text13 + text14 + text15 + b.ToString("X2")) / 1000.0);
            var textBox6 = TextBox6;
            var text16 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 63))];
            var text17 = b2.ToString("X2");
            var text18 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 62))];
            textBox6.Text = Conversions.ToString(Conversion.Val(text16 + text17 + text18 + b.ToString("X2")) / 1000.0);
            var textBox7 = TextBox7;
            var text19 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 61))];
            var text20 = b2.ToString("X2");
            var text21 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 60))];
            textBox7.Text = Conversions.ToString(Conversion.Val(text19 + text20 + text21 + b.ToString("X2")) / 1000.0);
            var textBox8 = TextBox8;
            var text22 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 59))];
            var text23 = b2.ToString("X2");
            var text24 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 58))];
            textBox8.Text = Conversions.ToString(Conversion.Val(text22 + text23 + text24 + b.ToString("X2")) / 1000.0);
            var textBox9 = TextBox9;
            var text25 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 57))];
            var text26 = b2.ToString("X2");
            var text27 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 56))];
            textBox9.Text = Conversions.ToString(Conversion.Val(text25 + text26 + text27 + b.ToString("X2")) / 1000.0);
            var textBox10 = TextBox10;
            var text28 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 55))];
            var text29 = b2.ToString("X2");
            var text30 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 54))];
            textBox10.Text = Conversions.ToString(Conversion.Val(text28 + text29 + text30 + b.ToString("X2")) / 1000.0);
            var textBox11 = TextBox11;
            var text31 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 53))];
            var text32 = b2.ToString("X2");
            var text33 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 52))];
            textBox11.Text = Conversions.ToString(Conversion.Val(text31 + text32 + text33 + b.ToString("X2")) / 1000.0);
            var textBox12 = TextBox12;
            var text34 = "&H";
            b2 = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 51))];
            var text35 = b2.ToString("X2");
            var text36 = " ";
            b = MyProject.Forms.Form1.Gt5Save[
                Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 50))];
            textBox12.Text = Conversions.ToString(Conversion.Val(text34 + text35 + text36 + b.ToString("X2")) / 1000.0);
            try
            {
                if (Conversion.Val(TextBox11.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox11.Text));
                else if (Conversion.Val(TextBox10.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox10.Text));
                else if (Conversion.Val(TextBox9.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox9.Text));
                else if (Conversion.Val(TextBox8.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox8.Text));
                else if (Conversion.Val(TextBox7.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox7.Text));
                else if (Conversion.Val(TextBox6.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox6.Text));
                else if (Conversion.Val(TextBox5.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox5.Text));
                else if (Conversion.Val(TextBox4.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox4.Text));
                else if (Conversion.Val(TextBox3.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox3.Text));
                else if (Conversion.Val(TextBox2.Text) > 0.0)
                    lastgearpre = new decimal(Conversion.Val(TextBox2.Text));
                else if (Conversion.Val(TextBox1.Text) > 0.0) lastgearpre = new decimal(Conversion.Val(TextBox1.Text));
            }
            catch (Exception ex)
            {
            }

            origfg = new decimal(Conversion.Val(TextBox12.Text));
        }

        // Token: 0x060001B8 RID: 440 RVA: 0x000109D0 File Offset: 0x0000EDD0
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var working = TextBox1.Text;
                working = Conversions.ToString(Conversion.Val(working) * 1000.0);
                var @int = Conversions.ToInteger(Conversion.Int(working));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 73))] =
                        (byte)Math.Round(Conversion.Val("&H" + @int.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 72))] =
                        (byte)Math.Round(Conversion.Val("&H" + @int.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex)
            {
            }

            try
            {
                var working2 = TextBox2.Text;
                working2 = Conversions.ToString(Conversion.Val(working2) * 1000.0);
                var int2 = Conversions.ToInteger(Conversion.Int(working2));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 71))] =
                        (byte)Math.Round(Conversion.Val("&H" + int2.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 70))] =
                        (byte)Math.Round(Conversion.Val("&H" + int2.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex2)
            {
            }

            try
            {
                var working3 = TextBox3.Text;
                working3 = Conversions.ToString(Conversion.Val(working3) * 1000.0);
                var int3 = Conversions.ToInteger(Conversion.Int(working3));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 69))] =
                        (byte)Math.Round(Conversion.Val("&H" + int3.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 68))] =
                        (byte)Math.Round(Conversion.Val("&H" + int3.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex3)
            {
            }

            try
            {
                var working4 = TextBox4.Text;
                working4 = Conversions.ToString(Conversion.Val(working4) * 1000.0);
                var int4 = Conversions.ToInteger(Conversion.Int(working4));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 67))] =
                        (byte)Math.Round(Conversion.Val("&H" + int4.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 66))] =
                        (byte)Math.Round(Conversion.Val("&H" + int4.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex4)
            {
            }

            try
            {
                var working5 = TextBox5.Text;
                working5 = Conversions.ToString(Conversion.Val(working5) * 1000.0);
                var int5 = Conversions.ToInteger(Conversion.Int(working5));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 65))] =
                        (byte)Math.Round(Conversion.Val("&H" + int5.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 64))] =
                        (byte)Math.Round(Conversion.Val("&H" + int5.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex5)
            {
            }

            try
            {
                var working6 = TextBox6.Text;
                working6 = Conversions.ToString(Conversion.Val(working6) * 1000.0);
                var int6 = Conversions.ToInteger(Conversion.Int(working6));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 63))] =
                        (byte)Math.Round(Conversion.Val("&H" + int6.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 62))] =
                        (byte)Math.Round(Conversion.Val("&H" + int6.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex6)
            {
            }

            try
            {
                var working7 = TextBox7.Text;
                working7 = Conversions.ToString(Conversion.Val(working7) * 1000.0);
                var int7 = Conversions.ToInteger(Conversion.Int(working7));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 61))] =
                        (byte)Math.Round(Conversion.Val("&H" + int7.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 60))] =
                        (byte)Math.Round(Conversion.Val("&H" + int7.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex7)
            {
            }

            try
            {
                var working8 = TextBox8.Text;
                working8 = Conversions.ToString(Conversion.Val(working8) * 1000.0);
                var int8 = Conversions.ToInteger(Conversion.Int(working8));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 59))] =
                        (byte)Math.Round(Conversion.Val("&H" + int8.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 58))] =
                        (byte)Math.Round(Conversion.Val("&H" + int8.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex8)
            {
            }

            try
            {
                var working9 = TextBox9.Text;
                working9 = Conversions.ToString(Conversion.Val(working9) * 1000.0);
                var int9 = Conversions.ToInteger(Conversion.Int(working9));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 57))] =
                        (byte)Math.Round(Conversion.Val("&H" + int9.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 56))] =
                        (byte)Math.Round(Conversion.Val("&H" + int9.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex9)
            {
            }

            try
            {
                var working10 = TextBox10.Text;
                working10 = Conversions.ToString(Conversion.Val(working10) * 1000.0);
                var int10 = Conversions.ToInteger(Conversion.Int(working10));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 55))] =
                        (byte)Math.Round(Conversion.Val("&H" + int10.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 54))] =
                        (byte)Math.Round(Conversion.Val("&H" + int10.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex10)
            {
            }

            try
            {
                var working11 = TextBox11.Text;
                working11 = Conversions.ToString(Conversion.Val(working11) * 1000.0);
                var int11 = Conversions.ToInteger(Conversion.Int(working11));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 53))] =
                        (byte)Math.Round(Conversion.Val("&H" + int11.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 52))] =
                        (byte)Math.Round(Conversion.Val("&H" + int11.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex11)
            {
            }

            try
            {
                var working12 = TextBox12.Text;
                working12 = Conversions.ToString(Conversion.Val(working12) * 1000.0);
                var int12 = Conversions.ToInteger(Conversion.Int(working12));
                checked
                {
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 51))] =
                        (byte)Math.Round(Conversion.Val("&H" + int12.ToString("X4").Substring(0, 2)));
                    MyProject.Forms.Form1.Gt5Save[
                            Conversions.ToInteger(Operators.SubtractObject(MyProject.Forms.Form1.Moff, 50))] =
                        (byte)Math.Round(Conversion.Val("&H" + int12.ToString("X4").Substring(2, 2)));
                }
            }
            catch (Exception ex12)
            {
            }

            Close();
        }

        // Token: 0x060001B9 RID: 441 RVA: 0x000115E0 File Offset: 0x0000F9E0
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox13.Text)) TextBox13.Text = "0";

            var omax = Conversion.Val(TextBox13.Text);
            if (omax != 0.0)
            {
                var lastgear = 0.1m;

                // Group the TextBoxes in an array for iteration
                TextBox[] textBoxes =
                {
                    TextBox11, TextBox10, TextBox9, TextBox8, TextBox7,
                    TextBox6, TextBox5, TextBox4, TextBox3, TextBox2, TextBox1
                };

                // Find the first TextBox with a value > 0
                foreach (var textBox in textBoxes)
                    if (Conversion.Val(textBox.Text) > 0.0)
                    {
                        lastgear = new decimal(Conversion.Val(textBox.Text));
                        break;
                    }

                TextBox14.Text = Conversions.ToString(
                    Convert.ToDouble(origfg) /
                    Conversion.Val(TextBox12.Text) *
                    Conversion.Val(TextBox13.Text) *
                    Convert.ToDouble(decimal.Multiply(
                        decimal.Divide(1m, lastgear), lastgearpre
                    ))
                );
            }
        }
    }
}