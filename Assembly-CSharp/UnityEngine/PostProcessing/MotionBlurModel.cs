using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000622 RID: 1570
	[Serializable]
	public class MotionBlurModel : PostProcessingModel
	{
		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x060040AB RID: 16555 RVA: 0x00033E6D File Offset: 0x0003206D
		// (set) Token: 0x060040AC RID: 16556 RVA: 0x00033E75 File Offset: 0x00032075
		public MotionBlurModel.Settings settings
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

		// Token: 0x060040AD RID: 16557 RVA: 0x00033E7E File Offset: 0x0003207E
		public override void Reset()
		{
			this.m_Settings = MotionBlurModel.Settings.defaultSettings;
		}

		// Token: 0x04003362 RID: 13154
		[SerializeField]
		public MotionBlurModel.Settings m_Settings = MotionBlurModel.Settings.defaultSettings;

		// Token: 0x020012A4 RID: 4772
		[Serializable]
		public struct Settings
		{
			// Token: 0x17001923 RID: 6435
			// (get) Token: 0x060081E8 RID: 33256 RVA: 0x0029C47C File Offset: 0x0029A67C
			public static MotionBlurModel.Settings defaultSettings
			{
				get
				{
					return new MotionBlurModel.Settings
					{
						shutterAngle = 270f,
						sampleCount = 10,
						frameBlending = 0f
					};
				}
			}

			// Token: 0x0400807D RID: 32893
			[Range(0f, 360f)]
			[Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
			public float shutterAngle;

			// Token: 0x0400807E RID: 32894
			[Range(4f, 32f)]
			[Tooltip("The amount of sample points, which affects quality and performances.")]
			public int sampleCount;

			// Token: 0x0400807F RID: 32895
			[Range(0f, 1f)]
			[Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
			public float frameBlending;
		}
	}
}
