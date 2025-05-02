using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200061B RID: 1563
	[Serializable]
	public class ChromaticAberrationModel : PostProcessingModel
	{
		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x0600408A RID: 16522 RVA: 0x00033CDF File Offset: 0x00031EDF
		// (set) Token: 0x0600408B RID: 16523 RVA: 0x00033CE7 File Offset: 0x00031EE7
		public ChromaticAberrationModel.Settings settings
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

		// Token: 0x0600408C RID: 16524 RVA: 0x00033CF0 File Offset: 0x00031EF0
		public override void Reset()
		{
			this.m_Settings = ChromaticAberrationModel.Settings.defaultSettings;
		}

		// Token: 0x04003359 RID: 13145
		[SerializeField]
		public ChromaticAberrationModel.Settings m_Settings = ChromaticAberrationModel.Settings.defaultSettings;

		// Token: 0x02001292 RID: 4754
		[Serializable]
		public struct Settings
		{
			// Token: 0x17001915 RID: 6421
			// (get) Token: 0x060081DA RID: 33242 RVA: 0x0029BE10 File Offset: 0x0029A010
			public static ChromaticAberrationModel.Settings defaultSettings
			{
				get
				{
					return new ChromaticAberrationModel.Settings
					{
						spectralTexture = null,
						intensity = 0.1f
					};
				}
			}

			// Token: 0x0400802B RID: 32811
			[Tooltip("Shift the hue of chromatic aberrations.")]
			public Texture2D spectralTexture;

			// Token: 0x0400802C RID: 32812
			[Range(0f, 1f)]
			[Tooltip("Amount of tangential distortion.")]
			public float intensity;
		}
	}
}
