using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200062A RID: 1578
	public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x060040DA RID: 16602 RVA: 0x00033FD7 File Offset: 0x000321D7
		public PostProcessingComponentRenderTexture()
		{
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x00033FDF File Offset: 0x000321DF
		public virtual void Prepare(Material material)
		{
		}
	}
}
