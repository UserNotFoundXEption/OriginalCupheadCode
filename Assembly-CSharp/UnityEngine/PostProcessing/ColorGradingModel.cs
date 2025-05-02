using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200061C RID: 1564
	[Serializable]
	public class ColorGradingModel : PostProcessingModel
	{
		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x0600408E RID: 16526 RVA: 0x00033D10 File Offset: 0x00031F10
		// (set) Token: 0x0600408F RID: 16527 RVA: 0x00033D18 File Offset: 0x00031F18
		public ColorGradingModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
				this.OnValidate();
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06004090 RID: 16528 RVA: 0x00033D27 File Offset: 0x00031F27
		// (set) Token: 0x06004091 RID: 16529 RVA: 0x00033D2F File Offset: 0x00031F2F
		public bool isDirty { get; set; }

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06004092 RID: 16530 RVA: 0x00033D38 File Offset: 0x00031F38
		// (set) Token: 0x06004093 RID: 16531 RVA: 0x00033D40 File Offset: 0x00031F40
		public RenderTexture bakedLut { get; set; }

		// Token: 0x06004094 RID: 16532 RVA: 0x00033D49 File Offset: 0x00031F49
		public override void Reset()
		{
			this.m_Settings = ColorGradingModel.Settings.defaultSettings;
			this.OnValidate();
		}

		// Token: 0x06004095 RID: 16533 RVA: 0x00033D5C File Offset: 0x00031F5C
		public override void OnValidate()
		{
			this.isDirty = true;
		}

		// Token: 0x0400335A RID: 13146
		[SerializeField]
		public ColorGradingModel.Settings m_Settings = ColorGradingModel.Settings.defaultSettings;

		// Token: 0x02001293 RID: 4755
		public enum Tonemapper
		{
			// Token: 0x0400802E RID: 32814
			None,
			// Token: 0x0400802F RID: 32815
			ACES,
			// Token: 0x04008030 RID: 32816
			Neutral
		}

		// Token: 0x02001294 RID: 4756
		[Serializable]
		public struct TonemappingSettings
		{
			// Token: 0x17001916 RID: 6422
			// (get) Token: 0x060081DB RID: 33243 RVA: 0x0029BE3C File Offset: 0x0029A03C
			public static ColorGradingModel.TonemappingSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.TonemappingSettings
					{
						tonemapper = ColorGradingModel.Tonemapper.Neutral,
						neutralBlackIn = 0.02f,
						neutralWhiteIn = 10f,
						neutralBlackOut = 0f,
						neutralWhiteOut = 10f,
						neutralWhiteLevel = 5.3f,
						neutralWhiteClip = 10f
					};
				}
			}

			// Token: 0x04008031 RID: 32817
			[Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use \"Neutral\" if you need a customizable tonemapper or \"Filmic\" to give a standard filmic look to your scenes.")]
			public ColorGradingModel.Tonemapper tonemapper;

			// Token: 0x04008032 RID: 32818
			[Range(-0.1f, 0.1f)]
			public float neutralBlackIn;

			// Token: 0x04008033 RID: 32819
			[Range(1f, 20f)]
			public float neutralWhiteIn;

			// Token: 0x04008034 RID: 32820
			[Range(-0.09f, 0.1f)]
			public float neutralBlackOut;

			// Token: 0x04008035 RID: 32821
			[Range(1f, 19f)]
			public float neutralWhiteOut;

			// Token: 0x04008036 RID: 32822
			[Range(0.1f, 20f)]
			public float neutralWhiteLevel;

			// Token: 0x04008037 RID: 32823
			[Range(1f, 10f)]
			public float neutralWhiteClip;
		}

		// Token: 0x02001295 RID: 4757
		[Serializable]
		public struct BasicSettings
		{
			// Token: 0x17001917 RID: 6423
			// (get) Token: 0x060081DC RID: 33244 RVA: 0x0029BEA4 File Offset: 0x0029A0A4
			public static ColorGradingModel.BasicSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.BasicSettings
					{
						postExposure = 0f,
						temperature = 0f,
						tint = 0f,
						hueShift = 0f,
						saturation = 1f,
						contrast = 1f
					};
				}
			}

			// Token: 0x04008038 RID: 32824
			[Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
			public float postExposure;

			// Token: 0x04008039 RID: 32825
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to a custom color temperature.")]
			public float temperature;

			// Token: 0x0400803A RID: 32826
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
			public float tint;

			// Token: 0x0400803B RID: 32827
			[Range(-180f, 180f)]
			[Tooltip("Shift the hue of all colors.")]
			public float hueShift;

			// Token: 0x0400803C RID: 32828
			[Range(0f, 2f)]
			[Tooltip("Pushes the intensity of all colors.")]
			public float saturation;

			// Token: 0x0400803D RID: 32829
			[Range(0f, 2f)]
			[Tooltip("Expands or shrinks the overall range of tonal values.")]
			public float contrast;
		}

		// Token: 0x02001296 RID: 4758
		[Serializable]
		public struct ChannelMixerSettings
		{
			// Token: 0x17001918 RID: 6424
			// (get) Token: 0x060081DD RID: 33245 RVA: 0x0029BF04 File Offset: 0x0029A104
			public static ColorGradingModel.ChannelMixerSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ChannelMixerSettings
					{
						red = new Vector3(1f, 0f, 0f),
						green = new Vector3(0f, 1f, 0f),
						blue = new Vector3(0f, 0f, 1f),
						currentEditingChannel = 0
					};
				}
			}

			// Token: 0x0400803E RID: 32830
			public Vector3 red;

			// Token: 0x0400803F RID: 32831
			public Vector3 green;

			// Token: 0x04008040 RID: 32832
			public Vector3 blue;

			// Token: 0x04008041 RID: 32833
			[HideInInspector]
			public int currentEditingChannel;
		}

		// Token: 0x02001297 RID: 4759
		[Serializable]
		public struct LogWheelsSettings
		{
			// Token: 0x17001919 RID: 6425
			// (get) Token: 0x060081DE RID: 33246 RVA: 0x0029BF74 File Offset: 0x0029A174
			public static ColorGradingModel.LogWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LogWheelsSettings
					{
						slope = Color.clear,
						power = Color.clear,
						offset = Color.clear
					};
				}
			}

			// Token: 0x04008042 RID: 32834
			[Trackball("GetSlopeValue")]
			public Color slope;

			// Token: 0x04008043 RID: 32835
			[Trackball("GetPowerValue")]
			public Color power;

			// Token: 0x04008044 RID: 32836
			[Trackball("GetOffsetValue")]
			public Color offset;
		}

		// Token: 0x02001298 RID: 4760
		[Serializable]
		public struct LinearWheelsSettings
		{
			// Token: 0x1700191A RID: 6426
			// (get) Token: 0x060081DF RID: 33247 RVA: 0x0029BFB0 File Offset: 0x0029A1B0
			public static ColorGradingModel.LinearWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LinearWheelsSettings
					{
						lift = Color.clear,
						gamma = Color.clear,
						gain = Color.clear
					};
				}
			}

			// Token: 0x04008045 RID: 32837
			[Trackball("GetLiftValue")]
			public Color lift;

			// Token: 0x04008046 RID: 32838
			[Trackball("GetGammaValue")]
			public Color gamma;

			// Token: 0x04008047 RID: 32839
			[Trackball("GetGainValue")]
			public Color gain;
		}

		// Token: 0x02001299 RID: 4761
		public enum ColorWheelMode
		{
			// Token: 0x04008049 RID: 32841
			Linear,
			// Token: 0x0400804A RID: 32842
			Log
		}

		// Token: 0x0200129A RID: 4762
		[Serializable]
		public struct ColorWheelsSettings
		{
			// Token: 0x1700191B RID: 6427
			// (get) Token: 0x060081E0 RID: 33248 RVA: 0x0029BFEC File Offset: 0x0029A1EC
			public static ColorGradingModel.ColorWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ColorWheelsSettings
					{
						mode = ColorGradingModel.ColorWheelMode.Log,
						log = ColorGradingModel.LogWheelsSettings.defaultSettings,
						linear = ColorGradingModel.LinearWheelsSettings.defaultSettings
					};
				}
			}

			// Token: 0x0400804B RID: 32843
			public ColorGradingModel.ColorWheelMode mode;

			// Token: 0x0400804C RID: 32844
			[TrackballGroup]
			public ColorGradingModel.LogWheelsSettings log;

			// Token: 0x0400804D RID: 32845
			[TrackballGroup]
			public ColorGradingModel.LinearWheelsSettings linear;
		}

		// Token: 0x0200129B RID: 4763
		[Serializable]
		public struct CurvesSettings
		{
			// Token: 0x1700191C RID: 6428
			// (get) Token: 0x060081E1 RID: 33249 RVA: 0x0029C024 File Offset: 0x0029A224
			public static ColorGradingModel.CurvesSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.CurvesSettings
					{
						master = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						red = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						green = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						blue = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						hueVShue = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						hueVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						satVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						lumVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						e_CurrentEditingCurve = 0,
						e_CurveY = true,
						e_CurveR = false,
						e_CurveG = false,
						e_CurveB = false
					};
				}
			}

			// Token: 0x0400804E RID: 32846
			public ColorGradingCurve master;

			// Token: 0x0400804F RID: 32847
			public ColorGradingCurve red;

			// Token: 0x04008050 RID: 32848
			public ColorGradingCurve green;

			// Token: 0x04008051 RID: 32849
			public ColorGradingCurve blue;

			// Token: 0x04008052 RID: 32850
			public ColorGradingCurve hueVShue;

			// Token: 0x04008053 RID: 32851
			public ColorGradingCurve hueVSsat;

			// Token: 0x04008054 RID: 32852
			public ColorGradingCurve satVSsat;

			// Token: 0x04008055 RID: 32853
			public ColorGradingCurve lumVSsat;

			// Token: 0x04008056 RID: 32854
			[HideInInspector]
			public int e_CurrentEditingCurve;

			// Token: 0x04008057 RID: 32855
			[HideInInspector]
			public bool e_CurveY;

			// Token: 0x04008058 RID: 32856
			[HideInInspector]
			public bool e_CurveR;

			// Token: 0x04008059 RID: 32857
			[HideInInspector]
			public bool e_CurveG;

			// Token: 0x0400805A RID: 32858
			[HideInInspector]
			public bool e_CurveB;
		}

		// Token: 0x0200129C RID: 4764
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700191D RID: 6429
			// (get) Token: 0x060081E2 RID: 33250 RVA: 0x0029C2D4 File Offset: 0x0029A4D4
			public static ColorGradingModel.Settings defaultSettings
			{
				get
				{
					return new ColorGradingModel.Settings
					{
						tonemapping = ColorGradingModel.TonemappingSettings.defaultSettings,
						basic = ColorGradingModel.BasicSettings.defaultSettings,
						channelMixer = ColorGradingModel.ChannelMixerSettings.defaultSettings,
						colorWheels = ColorGradingModel.ColorWheelsSettings.defaultSettings,
						curves = ColorGradingModel.CurvesSettings.defaultSettings
					};
				}
			}

			// Token: 0x0400805B RID: 32859
			public ColorGradingModel.TonemappingSettings tonemapping;

			// Token: 0x0400805C RID: 32860
			public ColorGradingModel.BasicSettings basic;

			// Token: 0x0400805D RID: 32861
			public ColorGradingModel.ChannelMixerSettings channelMixer;

			// Token: 0x0400805E RID: 32862
			public ColorGradingModel.ColorWheelsSettings colorWheels;

			// Token: 0x0400805F RID: 32863
			public ColorGradingModel.CurvesSettings curves;
		}
	}
}
