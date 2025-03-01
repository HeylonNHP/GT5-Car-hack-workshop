using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace GT5_Car_hack_workshop_2.My
{
    // Token: 0x02000002 RID: 2
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GeneratedCode("MyTemplate", "10.0.0.0")]
    internal class MyApplication : WindowsFormsApplicationBase
    {
        // Token: 0x06000002 RID: 2 RVA: 0x000028D0 File Offset: 0x00000CD0
        [DebuggerStepThrough]
        public MyApplication()
            : base(AuthenticationMode.Windows)
        {
            IsSingleInstance = false;
            EnableVisualStyles = true;
            SaveMySettingsOnExit = true;
            ShutdownStyle = ShutdownMode.AfterMainFormCloses;
        }

        // Token: 0x06000001 RID: 1 RVA: 0x00002898 File Offset: 0x00000C98
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DebuggerHidden]
        [STAThread]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static void Main(string[] Args)
        {
            Application.SetCompatibleTextRenderingDefault(UseCompatibleTextRendering);
            MyProject.Application.Run(Args);
        }

        // Token: 0x06000003 RID: 3 RVA: 0x000028F8 File Offset: 0x00000CF8
        [DebuggerStepThrough]
        protected override void OnCreateMainForm()
        {
            MainForm = MyProject.Forms.Form1;
        }
    }
}