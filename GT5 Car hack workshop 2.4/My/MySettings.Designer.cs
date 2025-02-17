using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace GT5_Car_hack_workshop_2.My
{
	// Token: 0x0200000C RID: 12
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[CompilerGenerated]
	internal sealed partial class MySettings : ApplicationSettingsBase
	{
		// Token: 0x0600016F RID: 367 RVA: 0x00002520 File Offset: 0x00000920
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DebuggerNonUserCode]
		private static void AutoSaveSettings(object sender, EventArgs e)
		{
			if (MyProject.Application.SaveMySettingsOnExit)
			{
				MySettingsProperty.Settings.Save();
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00002538 File Offset: 0x00000938
		public static MySettings Default
		{
			get
			{
				if (!MySettings.addedHandler)
				{
					object obj = MySettings.addedHandlerLockObject;
					ObjectFlowControl.CheckForSyncLockOnValueType(obj);
					lock (obj)
					{
						if (!MySettings.addedHandler)
						{
							MyProject.Application.Shutdown += MySettings.AutoSaveSettings;
							MySettings.addedHandler = true;
						}
					}
				}
				return MySettings.defaultInstance;
			}
		}

		// Token: 0x0400009F RID: 159
		private static MySettings defaultInstance = (MySettings)SettingsBase.Synchronized(new MySettings());

		// Token: 0x040000A0 RID: 160
		private static bool addedHandler;

		// Token: 0x040000A1 RID: 161
		private static object addedHandlerLockObject = RuntimeHelpers.GetObjectValue(new object());
	}
}
