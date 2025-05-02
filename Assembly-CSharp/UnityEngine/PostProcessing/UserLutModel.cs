using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000624 RID: 1572
	[Serializable]
	public class UserLutModel : PostProcessingModel
	{
		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x060040B3 RID: 16563 RVA: 0x00033ECF File Offset: 0x000320CF
		// (set) Token: 0x060040B4 RID: 16564 RVA: 0x00033ED7 File Offset: 0x000320D7
		public UserLutModel.Settings settings
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

		// Token: 0x060040B5 RID: 16565 RVA: 0x00033EE0 File Offset: 0x000320E0
		public override void Reset()
		{
			this.m_Settings = UserLutModel.Settings.defaultSettings;
		}

		// Token: 0x04003364 RID: 13156
		[SerializeField]
		public UserLutModel.Settings m_Settings = UserLutModel.Settings.defaultSettings;

		// Token: 0x020012AB RID: 4779
		[Serializable]
		public struct Settings
		{
			// Token: 0x17001925 RID: 6437
			// (get) Token: 0x060081EA RID: 33258 RVA: 0x0029C588 File Offset: 0x0029A788
			public static UserLutModel.Settings defaultSettings
			{
				get
				{
					return new UserLutModel.Settings
					{
						lut = null,
						contribution = 1f
					};
				}
			}

			// Token: 0x04008096 RID: 32918
			[Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
			public Texture2D lut;

			// Token: 0x04008097 RID: 32919
			[Range(0f, 1f)]
			[Tooltip("Blending factor.")]
			public float contribution;
		}
	}
}
