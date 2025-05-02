using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000628 RID: 1576
	public abstract class PostProcessingComponent<T> : PostProcessingComponentBase where T : PostProcessingModel
	{
		// Token: 0x060040D1 RID: 16593 RVA: 0x00033F99 File Offset: 0x00032199
		public PostProcessingComponent()
		{
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060040D2 RID: 16594 RVA: 0x00033FA1 File Offset: 0x000321A1
		// (set) Token: 0x060040D3 RID: 16595 RVA: 0x00033FA9 File Offset: 0x000321A9
		public T model { get; set; }

		// Token: 0x060040D4 RID: 16596 RVA: 0x00033FB2 File Offset: 0x000321B2
		public virtual void Init(PostProcessingContext pcontext, T pmodel)
		{
			this.context = pcontext;
			this.model = pmodel;
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x00033FC2 File Offset: 0x000321C2
		public override PostProcessingModel GetModel()
		{
			return this.model;
		}
	}
}
