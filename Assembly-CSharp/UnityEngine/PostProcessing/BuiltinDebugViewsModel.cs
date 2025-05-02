using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200061A RID: 1562
	[Serializable]
	public class BuiltinDebugViewsModel : PostProcessingModel
	{
		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06004084 RID: 16516 RVA: 0x00033C5E File Offset: 0x00031E5E
		// (set) Token: 0x06004085 RID: 16517 RVA: 0x00033C66 File Offset: 0x00031E66
		public BuiltinDebugViewsModel.Settings settings
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

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06004086 RID: 16518 RVA: 0x00033C6F File Offset: 0x00031E6F
		public bool willInterrupt
		{
			get
			{
				return !this.IsModeActive(BuiltinDebugViewsModel.Mode.None) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut);
			}
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x00033CAF File Offset: 0x00031EAF
		public override void Reset()
		{
			this.settings = BuiltinDebugViewsModel.Settings.defaultSettings;
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x00033CBC File Offset: 0x00031EBC
		public bool IsModeActive(BuiltinDebugViewsModel.Mode mode)
		{
			return this.m_Settings.mode == mode;
		}

		// Token: 0x04003358 RID: 13144
		[SerializeField]
		public BuiltinDebugViewsModel.Settings m_Settings = BuiltinDebugViewsModel.Settings.defaultSettings;

		// Token: 0x0200128E RID: 4750
		[Serializable]
		public struct DepthSettings
		{
			// Token: 0x17001912 RID: 6418
			// (get) Token: 0x060081D7 RID: 33239 RVA: 0x0029BD58 File Offset: 0x00299F58
			public static BuiltinDebugViewsModel.DepthSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.DepthSettings
					{
						scale = 1f
					};
				}
			}

			// Token: 0x04008016 RID: 32790
			[Range(0f, 1f)]
			[Tooltip("Scales the camera far plane before displaying the depth map.")]
			public float scale;
		}

		// Token: 0x0200128F RID: 4751
		[Serializable]
		public struct MotionVectorsSettings
		{
			// Token: 0x17001913 RID: 6419
			// (get) Token: 0x060081D8 RID: 33240 RVA: 0x0029BD7C File Offset: 0x00299F7C
			public static BuiltinDebugViewsModel.MotionVectorsSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.MotionVectorsSettings
					{
						sourceOpacity = 1f,
						motionImageOpacity = 0f,
						motionImageAmplitude = 16f,
						motionVectorsOpacity = 1f,
						motionVectorsResolution = 24,
						motionVectorsAmplitude = 64f
					};
				}
			}

			// Token: 0x04008017 RID: 32791
			[Range(0f, 1f)]
			[Tooltip("Opacity of the source render.")]
			public float sourceOpacity;

			// Token: 0x04008018 RID: 32792
			[Range(0f, 1f)]
			[Tooltip("Opacity of the per-pixel motion vector colors.")]
			public float motionImageOpacity;

			// Token: 0x04008019 RID: 32793
			[Min(0f)]
			[Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")]
			public float motionImageAmplitude;

			// Token: 0x0400801A RID: 32794
			[Range(0f, 1f)]
			[Tooltip("Opacity for the motion vector arrows.")]
			public float motionVectorsOpacity;

			// Token: 0x0400801B RID: 32795
			[Range(8f, 64f)]
			[Tooltip("The arrow density on screen.")]
			public int motionVectorsResolution;

			// Token: 0x0400801C RID: 32796
			[Min(0f)]
			[Tooltip("Tweaks the arrows length.")]
			public float motionVectorsAmplitude;
		}

		// Token: 0x02001290 RID: 4752
		public enum Mode
		{
			// Token: 0x0400801E RID: 32798
			None,
			// Token: 0x0400801F RID: 32799
			Depth,
			// Token: 0x04008020 RID: 32800
			Normals,
			// Token: 0x04008021 RID: 32801
			MotionVectors,
			// Token: 0x04008022 RID: 32802
			AmbientOcclusion,
			// Token: 0x04008023 RID: 32803
			EyeAdaptation,
			// Token: 0x04008024 RID: 32804
			FocusPlane,
			// Token: 0x04008025 RID: 32805
			PreGradingLog,
			// Token: 0x04008026 RID: 32806
			LogLut,
			// Token: 0x04008027 RID: 32807
			UserLut
		}

		// Token: 0x02001291 RID: 4753
		[Serializable]
		public struct Settings
		{
			// Token: 0x17001914 RID: 6420
			// (get) Token: 0x060081D9 RID: 33241 RVA: 0x0029BDD8 File Offset: 0x00299FD8
			public static BuiltinDebugViewsModel.Settings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.Settings
					{
						mode = BuiltinDebugViewsModel.Mode.None,
						depth = BuiltinDebugViewsModel.DepthSettings.defaultSettings,
						motionVectors = BuiltinDebugViewsModel.MotionVectorsSettings.defaultSettings
					};
				}
			}

			// Token: 0x04008028 RID: 32808
			public BuiltinDebugViewsModel.Mode mode;

			// Token: 0x04008029 RID: 32809
			public BuiltinDebugViewsModel.DepthSettings depth;

			// Token: 0x0400802A RID: 32810
			public BuiltinDebugViewsModel.MotionVectorsSettings motionVectors;
		}
	}
}
