using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace GT5_Car_hack_workshop_2.My.Resources
{
	// Token: 0x0200000B RID: 11
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[HideModuleName]
	[DebuggerNonUserCode]
	[StandardModule]
	[CompilerGenerated]
	internal sealed class Resources
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00002494 File Offset: 0x00000894
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager temp = new ResourceManager("GT5_Car_hack_workshop_2.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = temp;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600016B RID: 363 RVA: 0x000024D4 File Offset: 0x000008D4
		// (set) Token: 0x0600016C RID: 364 RVA: 0x000024E8 File Offset: 0x000008E8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x0400009D RID: 157
		private static ResourceManager resourceMan;

		// Token: 0x0400009E RID: 158
		private static CultureInfo resourceCulture;
	}
}
