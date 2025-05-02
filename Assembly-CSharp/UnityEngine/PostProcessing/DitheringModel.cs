using System;
using System.Runtime.InteropServices;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200061E RID: 1566
	[Serializable]
	public class DitheringModel : PostProcessingModel
	{
		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x0600409B RID: 16539 RVA: 0x00033DA9 File Offset: 0x00031FA9
		// (set) Token: 0x0600409C RID: 16540 RVA: 0x00033DB1 File Offset: 0x00031FB1
		public DitheringModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x00033DBA File Offset: 0x00031FBA
		public override void Reset()
		{
			this.m_Settings = DitheringModel.Settings.defaultSettings;
		}

		// Token: 0x0400335E RID: 13150
		[SerializeField]
		public DitheringModel.Settings m_Settings = DitheringModel.Settings.defaultSettings;

		// Token: 0x0200129F RID: 4767
		[Serializable]
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Settings
		{
			// Token: 0x1700191F RID: 6431
			// (get) Token: 0x060081E4 RID: 33252 RVA: 0x0029C374 File Offset: 0x0029A574
			public static DitheringModel.Settings defaultSettings
			{
				get
				{
					return default(DitheringModel.Settings);
				}
			}
		}
	}
}
