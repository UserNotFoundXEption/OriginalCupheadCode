using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000618 RID: 1560
	[Serializable]
	public class AntialiasingModel : PostProcessingModel
	{
		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600407C RID: 16508 RVA: 0x00033BFC File Offset: 0x00031DFC
		// (set) Token: 0x0600407D RID: 16509 RVA: 0x00033C04 File Offset: 0x00031E04
		public AntialiasingModel.Settings settings
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

		// Token: 0x0600407E RID: 16510 RVA: 0x00033C0D File Offset: 0x00031E0D
		public override void Reset()
		{
			this.m_Settings = AntialiasingModel.Settings.defaultSettings;
		}

		// Token: 0x04003356 RID: 13142
		[SerializeField]
		public AntialiasingModel.Settings m_Settings = AntialiasingModel.Settings.defaultSettings;

		// Token: 0x02001284 RID: 4740
		public enum Method
		{
			// Token: 0x04007FF4 RID: 32756
			Fxaa,
			// Token: 0x04007FF5 RID: 32757
			Taa
		}

		// Token: 0x02001285 RID: 4741
		public enum FxaaPreset
		{
			// Token: 0x04007FF7 RID: 32759
			ExtremePerformance,
			// Token: 0x04007FF8 RID: 32760
			Performance,
			// Token: 0x04007FF9 RID: 32761
			Default,
			// Token: 0x04007FFA RID: 32762
			Quality,
			// Token: 0x04007FFB RID: 32763
			ExtremeQuality
		}

		// Token: 0x02001286 RID: 4742
		[Serializable]
		public struct FxaaQualitySettings
		{
			// Token: 0x04007FFC RID: 32764
			[Tooltip("The amount of desired sub-pixel aliasing removal. Effects the sharpeness of the output.")]
			[Range(0f, 1f)]
			public float subpixelAliasingRemovalAmount;

			// Token: 0x04007FFD RID: 32765
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.063f, 0.333f)]
			public float edgeDetectionThreshold;

			// Token: 0x04007FFE RID: 32766
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0f, 0.0833f)]
			public float minimumRequiredLuminance;

			// Token: 0x04007FFF RID: 32767
			public static AntialiasingModel.FxaaQualitySettings[] presets = new AntialiasingModel.FxaaQualitySettings[]
			{
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0f,
					edgeDetectionThreshold = 0.333f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.25f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.75f,
					edgeDetectionThreshold = 0.166f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.0625f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.063f,
					minimumRequiredLuminance = 0.0312f
				}
			};
		}

		// Token: 0x02001287 RID: 4743
		[Serializable]
		public struct FxaaConsoleSettings
		{
			// Token: 0x04008000 RID: 32768
			[Tooltip("The amount of spread applied to the sampling coordinates while sampling for subpixel information.")]
			[Range(0.33f, 0.5f)]
			public float subpixelSpreadAmount;

			// Token: 0x04008001 RID: 32769
			[Tooltip("This value dictates how sharp the edges in the image are kept; a higher value implies sharper edges.")]
			[Range(2f, 8f)]
			public float edgeSharpnessAmount;

			// Token: 0x04008002 RID: 32770
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.125f, 0.25f)]
			public float edgeDetectionThreshold;

			// Token: 0x04008003 RID: 32771
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0.04f, 0.06f)]
			public float minimumRequiredLuminance;

			// Token: 0x04008004 RID: 32772
			public static AntialiasingModel.FxaaConsoleSettings[] presets = new AntialiasingModel.FxaaConsoleSettings[]
			{
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.05f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 4f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 2f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				}
			};
		}

		// Token: 0x02001288 RID: 4744
		[Serializable]
		public struct FxaaSettings
		{
			// Token: 0x1700190B RID: 6411
			// (get) Token: 0x060081CF RID: 33231 RVA: 0x0029BC0C File Offset: 0x00299E0C
			public static AntialiasingModel.FxaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.FxaaSettings
					{
						preset = AntialiasingModel.FxaaPreset.Default
					};
				}
			}

			// Token: 0x04008005 RID: 32773
			public AntialiasingModel.FxaaPreset preset;
		}

		// Token: 0x02001289 RID: 4745
		[Serializable]
		public struct TaaSettings
		{
			// Token: 0x1700190C RID: 6412
			// (get) Token: 0x060081D0 RID: 33232 RVA: 0x0029BC2C File Offset: 0x00299E2C
			public static AntialiasingModel.TaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.TaaSettings
					{
						jitterSpread = 0.75f,
						sharpen = 0.3f,
						stationaryBlending = 0.95f,
						motionBlending = 0.85f
					};
				}
			}

			// Token: 0x04008006 RID: 32774
			[Tooltip("The diameter (in texels) inside which jitter samples are spread. Smaller values result in crisper but more aliased output, while larger values result in more stable but blurrier output.")]
			[Range(0.1f, 1f)]
			public float jitterSpread;

			// Token: 0x04008007 RID: 32775
			[Tooltip("Controls the amount of sharpening applied to the color buffer.")]
			[Range(0f, 3f)]
			public float sharpen;

			// Token: 0x04008008 RID: 32776
			[Tooltip("The blend coefficient for a stationary fragment. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float stationaryBlending;

			// Token: 0x04008009 RID: 32777
			[Tooltip("The blend coefficient for a fragment with significant motion. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float motionBlending;
		}

		// Token: 0x0200128A RID: 4746
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700190D RID: 6413
			// (get) Token: 0x060081D1 RID: 33233 RVA: 0x0029BC74 File Offset: 0x00299E74
			public static AntialiasingModel.Settings defaultSettings
			{
				get
				{
					return new AntialiasingModel.Settings
					{
						method = AntialiasingModel.Method.Fxaa,
						fxaaSettings = AntialiasingModel.FxaaSettings.defaultSettings,
						taaSettings = AntialiasingModel.TaaSettings.defaultSettings
					};
				}
			}

			// Token: 0x0400800A RID: 32778
			public AntialiasingModel.Method method;

			// Token: 0x0400800B RID: 32779
			public AntialiasingModel.FxaaSettings fxaaSettings;

			// Token: 0x0400800C RID: 32780
			public AntialiasingModel.TaaSettings taaSettings;
		}
	}
}
