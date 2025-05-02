using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000619 RID: 1561
	[Serializable]
	public class BloomModel : PostProcessingModel
	{
		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06004080 RID: 16512 RVA: 0x00033C2D File Offset: 0x00031E2D
		// (set) Token: 0x06004081 RID: 16513 RVA: 0x00033C35 File Offset: 0x00031E35
		public BloomModel.Settings settings
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

		// Token: 0x06004082 RID: 16514 RVA: 0x00033C3E File Offset: 0x00031E3E
		public override void Reset()
		{
			this.m_Settings = BloomModel.Settings.defaultSettings;
		}

		// Token: 0x04003357 RID: 13143
		[SerializeField]
		public BloomModel.Settings m_Settings = BloomModel.Settings.defaultSettings;

		// Token: 0x0200128B RID: 4747
		[Serializable]
		public struct BloomSettings
		{
			// Token: 0x1700190E RID: 6414
			// (get) Token: 0x060081D3 RID: 33235 RVA: 0x00056830 File Offset: 0x00054A30
			// (set) Token: 0x060081D2 RID: 33234 RVA: 0x00056822 File Offset: 0x00054A22
			public float thresholdLinear
			{
				get
				{
					return Mathf.GammaToLinearSpace(this.threshold);
				}
				set
				{
					this.threshold = Mathf.LinearToGammaSpace(value);
				}
			}

			// Token: 0x1700190F RID: 6415
			// (get) Token: 0x060081D4 RID: 33236 RVA: 0x0029BCAC File Offset: 0x00299EAC
			public static BloomModel.BloomSettings defaultSettings
			{
				get
				{
					return new BloomModel.BloomSettings
					{
						intensity = 0.5f,
						threshold = 1.1f,
						softKnee = 0.5f,
						radius = 4f,
						antiFlicker = false
					};
				}
			}

			// Token: 0x0400800D RID: 32781
			[Min(0f)]
			[Tooltip("Strength of the bloom filter.")]
			public float intensity;

			// Token: 0x0400800E RID: 32782
			[Min(0f)]
			[Tooltip("Filters out pixels under this level of brightness.")]
			public float threshold;

			// Token: 0x0400800F RID: 32783
			[Range(0f, 1f)]
			[Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
			public float softKnee;

			// Token: 0x04008010 RID: 32784
			[Range(1f, 7f)]
			[Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
			public float radius;

			// Token: 0x04008011 RID: 32785
			[Tooltip("Reduces flashing noise with an additional filter.")]
			public bool antiFlicker;
		}

		// Token: 0x0200128C RID: 4748
		[Serializable]
		public struct LensDirtSettings
		{
			// Token: 0x17001910 RID: 6416
			// (get) Token: 0x060081D5 RID: 33237 RVA: 0x0029BCFC File Offset: 0x00299EFC
			public static BloomModel.LensDirtSettings defaultSettings
			{
				get
				{
					return new BloomModel.LensDirtSettings
					{
						texture = null,
						intensity = 3f
					};
				}
			}

			// Token: 0x04008012 RID: 32786
			[Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
			public Texture texture;

			// Token: 0x04008013 RID: 32787
			[Min(0f)]
			[Tooltip("Amount of lens dirtiness.")]
			public float intensity;
		}

		// Token: 0x0200128D RID: 4749
		[Serializable]
		public struct Settings
		{
			// Token: 0x17001911 RID: 6417
			// (get) Token: 0x060081D6 RID: 33238 RVA: 0x0029BD28 File Offset: 0x00299F28
			public static BloomModel.Settings defaultSettings
			{
				get
				{
					return new BloomModel.Settings
					{
						bloom = BloomModel.BloomSettings.defaultSettings,
						lensDirt = BloomModel.LensDirtSettings.defaultSettings
					};
				}
			}

			// Token: 0x04008014 RID: 32788
			public BloomModel.BloomSettings bloom;

			// Token: 0x04008015 RID: 32789
			public BloomModel.LensDirtSettings lensDirt;
		}
	}
}
