using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000627 RID: 1575
	public abstract class PostProcessingComponentBase
	{
		// Token: 0x060040CB RID: 16587 RVA: 0x00033F8A File Offset: 0x0003218A
		public PostProcessingComponentBase()
		{
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x00033F92 File Offset: 0x00032192
		public virtual DepthTextureMode GetCameraFlags()
		{
			return 0;
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060040CD RID: 16589
		public abstract bool active { get; }

		// Token: 0x060040CE RID: 16590 RVA: 0x00033F95 File Offset: 0x00032195
		public virtual void OnEnable()
		{
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x00033F97 File Offset: 0x00032197
		public virtual void OnDisable()
		{
		}

		// Token: 0x060040D0 RID: 16592
		public abstract PostProcessingModel GetModel();

		// Token: 0x04003383 RID: 13187
		public PostProcessingContext context;
	}
}
