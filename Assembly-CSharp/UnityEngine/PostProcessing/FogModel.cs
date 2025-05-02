using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000620 RID: 1568
	[Serializable]
	public class FogModel : PostProcessingModel
	{
		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x060040A3 RID: 16547 RVA: 0x00033E0B File Offset: 0x0003200B
		// (set) Token: 0x060040A4 RID: 16548 RVA: 0x00033E13 File Offset: 0x00032013
		public FogModel.Settings settings
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

		// Token: 0x060040A5 RID: 16549 RVA: 0x00033E1C File Offset: 0x0003201C
		public override void Reset()
		{
			this.m_Settings = FogModel.Settings.defaultSettings;
		}

		// Token: 0x04003360 RID: 13152
		[SerializeField]
		public FogModel.Settings m_Settings = FogModel.Settings.defaultSettings;

		// Token: 0x020012A2 RID: 4770
		[Serializable]
		public struct Settings
		{
			// Token: 0x17001921 RID: 6433
			// (get) Token: 0x060081E6 RID: 33254 RVA: 0x0029C418 File Offset: 0x0029A618
			public static FogModel.Settings defaultSettings
			{
				get
				{
					return new FogModel.Settings
					{
						excludeSkybox = true
					};
				}
			}

			// Token: 0x04008078 RID: 32888
			[Tooltip("Should the fog affect the skybox?")]
			public bool excludeSkybox;
		}
	}
}
