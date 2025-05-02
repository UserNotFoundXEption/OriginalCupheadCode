using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000625 RID: 1573
	[Serializable]
	public class VignetteModel : PostProcessingModel
	{
		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x060040B7 RID: 16567 RVA: 0x00033F00 File Offset: 0x00032100
		// (set) Token: 0x060040B8 RID: 16568 RVA: 0x00033F08 File Offset: 0x00032108
		public VignetteModel.Settings settings
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

		// Token: 0x060040B9 RID: 16569 RVA: 0x00033F11 File Offset: 0x00032111
		public override void Reset()
		{
			this.m_Settings = VignetteModel.Settings.defaultSettings;
		}

		// Token: 0x04003365 RID: 13157
		[SerializeField]
		public VignetteModel.Settings m_Settings = VignetteModel.Settings.defaultSettings;

		// Token: 0x020012AC RID: 4780
		public enum Mode
		{
			// Token: 0x04008099 RID: 32921
			Classic,
			// Token: 0x0400809A RID: 32922
			Masked
		}

		// Token: 0x020012AD RID: 4781
		[Serializable]
		public struct Settings
		{
			// Token: 0x17001926 RID: 6438
			// (get) Token: 0x060081EB RID: 33259 RVA: 0x0029C5B4 File Offset: 0x0029A7B4
			public static VignetteModel.Settings defaultSettings
			{
				get
				{
					return new VignetteModel.Settings
					{
						mode = VignetteModel.Mode.Classic,
						color = new Color(0f, 0f, 0f, 1f),
						center = new Vector2(0.5f, 0.5f),
						intensity = 0.45f,
						smoothness = 0.2f,
						roundness = 1f,
						mask = null,
						opacity = 1f,
						rounded = false
					};
				}
			}

			// Token: 0x0400809B RID: 32923
			[Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
			public VignetteModel.Mode mode;

			// Token: 0x0400809C RID: 32924
			[ColorUsage(false)]
			[Tooltip("Vignette color. Use the alpha channel for transparency.")]
			public Color color;

			// Token: 0x0400809D RID: 32925
			[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
			public Vector2 center;

			// Token: 0x0400809E RID: 32926
			[Range(0f, 1f)]
			[Tooltip("Amount of vignetting on screen.")]
			public float intensity;

			// Token: 0x0400809F RID: 32927
			[Range(0.01f, 1f)]
			[Tooltip("Smoothness of the vignette borders.")]
			public float smoothness;

			// Token: 0x040080A0 RID: 32928
			[Range(0f, 1f)]
			[Tooltip("Lower values will make a square-ish vignette.")]
			public float roundness;

			// Token: 0x040080A1 RID: 32929
			[Tooltip("A black and white mask to use as a vignette.")]
			public Texture mask;

			// Token: 0x040080A2 RID: 32930
			[Range(0f, 1f)]
			[Tooltip("Mask opacity.")]
			public float opacity;

			// Token: 0x040080A3 RID: 32931
			[Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
			public bool rounded;
		}
	}
}
