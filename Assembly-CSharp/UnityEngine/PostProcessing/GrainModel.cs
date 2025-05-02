using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000621 RID: 1569
	[Serializable]
	public class GrainModel : PostProcessingModel
	{
		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x060040A7 RID: 16551 RVA: 0x00033E3C File Offset: 0x0003203C
		// (set) Token: 0x060040A8 RID: 16552 RVA: 0x00033E44 File Offset: 0x00032044
		public GrainModel.Settings settings
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

		// Token: 0x060040A9 RID: 16553 RVA: 0x00033E4D File Offset: 0x0003204D
		public override void Reset()
		{
			this.m_Settings = GrainModel.Settings.defaultSettings;
		}

		// Token: 0x04003361 RID: 13153
		[SerializeField]
		public GrainModel.Settings m_Settings = GrainModel.Settings.defaultSettings;

		// Token: 0x020012A3 RID: 4771
		[Serializable]
		public struct Settings
		{
			// Token: 0x17001922 RID: 6434
			// (get) Token: 0x060081E7 RID: 33255 RVA: 0x0029C438 File Offset: 0x0029A638
			public static GrainModel.Settings defaultSettings
			{
				get
				{
					return new GrainModel.Settings
					{
						colored = true,
						intensity = 0.5f,
						size = 1f,
						luminanceContribution = 0.8f
					};
				}
			}

			// Token: 0x04008079 RID: 32889
			[Tooltip("Enable the use of colored grain.")]
			public bool colored;

			// Token: 0x0400807A RID: 32890
			[Range(0f, 1f)]
			[Tooltip("Grain strength. Higher means more visible grain.")]
			public float intensity;

			// Token: 0x0400807B RID: 32891
			[Range(0.3f, 3f)]
			[Tooltip("Grain particle size.")]
			public float size;

			// Token: 0x0400807C RID: 32892
			[Range(0f, 1f)]
			[Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
			public float luminanceContribution;
		}
	}
}
