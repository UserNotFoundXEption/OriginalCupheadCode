using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000629 RID: 1577
	public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x060040D6 RID: 16598 RVA: 0x00033FCF File Offset: 0x000321CF
		public PostProcessingComponentCommandBuffer()
		{
		}

		// Token: 0x060040D7 RID: 16599
		public abstract CameraEvent GetCameraEvent();

		// Token: 0x060040D8 RID: 16600
		public abstract string GetName();

		// Token: 0x060040D9 RID: 16601
		public abstract void PopulateCommandBuffer(CommandBuffer cb);
	}
}
