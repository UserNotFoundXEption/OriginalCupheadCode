using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200061D RID: 1565
	[Serializable]
	public class DepthOfFieldModel : PostProcessingModel
	{
		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06004097 RID: 16535 RVA: 0x00033D78 File Offset: 0x00031F78
		// (set) Token: 0x06004098 RID: 16536 RVA: 0x00033D80 File Offset: 0x00031F80
		public DepthOfFieldModel.Settings settings
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

		// Token: 0x06004099 RID: 16537 RVA: 0x00033D89 File Offset: 0x00031F89
		public override void Reset()
		{
			this.m_Settings = DepthOfFieldModel.Settings.defaultSettings;
		}

		// Token: 0x0400335D RID: 13149
		[SerializeField]
		public DepthOfFieldModel.Settings m_Settings = DepthOfFieldModel.Settings.defaultSettings;

		// Token: 0x0200129D RID: 4765
		public enum KernelSize
		{
			// Token: 0x04008061 RID: 32865
			Small,
			// Token: 0x04008062 RID: 32866
			Medium,
			// Token: 0x04008063 RID: 32867
			Large,
			// Token: 0x04008064 RID: 32868
			VeryLarge
		}

		// Token: 0x0200129E RID: 4766
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700191E RID: 6430
			// (get) Token: 0x060081E3 RID: 33251 RVA: 0x0029C328 File Offset: 0x0029A528
			public static DepthOfFieldModel.Settings defaultSettings
			{
				get
				{
					return new DepthOfFieldModel.Settings
					{
						focusDistance = 10f,
						aperture = 5.6f,
						focalLength = 50f,
						useCameraFov = false,
						kernelSize = DepthOfFieldModel.KernelSize.Medium
					};
				}
			}

			// Token: 0x04008065 RID: 32869
			[Min(0.1f)]
			[Tooltip("Distance to the point of focus.")]
			public float focusDistance;

			// Token: 0x04008066 RID: 32870
			[Range(0.05f, 32f)]
			[Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
			public float aperture;

			// Token: 0x04008067 RID: 32871
			[Range(1f, 300f)]
			[Tooltip("Distance between the lens and the film. The larger the value is, the shallower the depth of field is.")]
			public float focalLength;

			// Token: 0x04008068 RID: 32872
			[Tooltip("Calculate the focal length automatically from the field-of-view value set on the camera. Using this setting isn't recommended.")]
			public bool useCameraFov;

			// Token: 0x04008069 RID: 32873
			[Tooltip("Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).")]
			public DepthOfFieldModel.KernelSize kernelSize;
		}
	}
}
