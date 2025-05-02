using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000617 RID: 1559
	[Serializable]
	public class AmbientOcclusionModel : PostProcessingModel
	{
		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06004078 RID: 16504 RVA: 0x00033BCB File Offset: 0x00031DCB
		// (set) Token: 0x06004079 RID: 16505 RVA: 0x00033BD3 File Offset: 0x00031DD3
		public AmbientOcclusionModel.Settings settings
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

		// Token: 0x0600407A RID: 16506 RVA: 0x00033BDC File Offset: 0x00031DDC
		public override void Reset()
		{
			this.m_Settings = AmbientOcclusionModel.Settings.defaultSettings;
		}

		// Token: 0x04003355 RID: 13141
		[SerializeField]
		public AmbientOcclusionModel.Settings m_Settings = AmbientOcclusionModel.Settings.defaultSettings;

		// Token: 0x02001282 RID: 4738
		public enum SampleCount
		{
			// Token: 0x04007FE8 RID: 32744
			Lowest = 3,
			// Token: 0x04007FE9 RID: 32745
			Low = 6,
			// Token: 0x04007FEA RID: 32746
			Medium = 10,
			// Token: 0x04007FEB RID: 32747
			High = 16
		}

		// Token: 0x02001283 RID: 4739
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700190A RID: 6410
			// (get) Token: 0x060081CC RID: 33228 RVA: 0x0029B908 File Offset: 0x00299B08
			public static AmbientOcclusionModel.Settings defaultSettings
			{
				get
				{
					return new AmbientOcclusionModel.Settings
					{
						intensity = 1f,
						radius = 0.3f,
						sampleCount = AmbientOcclusionModel.SampleCount.Medium,
						downsampling = true,
						forceForwardCompatibility = false,
						ambientOnly = false,
						highPrecision = false
					};
				}
			}

			// Token: 0x04007FEC RID: 32748
			[Range(0f, 4f)]
			[Tooltip("Degree of darkness produced by the effect.")]
			public float intensity;

			// Token: 0x04007FED RID: 32749
			[Min(0.0001f)]
			[Tooltip("Radius of sample points, which affects extent of darkened areas.")]
			public float radius;

			// Token: 0x04007FEE RID: 32750
			[Tooltip("Number of sample points, which affects quality and performance.")]
			public AmbientOcclusionModel.SampleCount sampleCount;

			// Token: 0x04007FEF RID: 32751
			[Tooltip("Halves the resolution of the effect to increase performance at the cost of visual quality.")]
			public bool downsampling;

			// Token: 0x04007FF0 RID: 32752
			[Tooltip("Forces compatibility with Forward rendered objects when working with the Deferred rendering path.")]
			public bool forceForwardCompatibility;

			// Token: 0x04007FF1 RID: 32753
			[Tooltip("Enables the ambient-only mode in that the effect only affects ambient lighting. This mode is only available with the Deferred rendering path and HDR rendering.")]
			public bool ambientOnly;

			// Token: 0x04007FF2 RID: 32754
			[Tooltip("Toggles the use of a higher precision depth texture with the forward rendering path (may impact performances). Has no effect with the deferred rendering path.")]
			public bool highPrecision;
		}
	}
}
