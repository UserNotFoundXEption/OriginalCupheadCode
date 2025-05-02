using System;

// Token: 0x02000587 RID: 1415
public interface PlmInterface
{
	// Token: 0x140000B8 RID: 184
	// (add) Token: 0x06003B99 RID: 15257
	// (remove) Token: 0x06003B9A RID: 15258
	event OnSuspendHandler OnSuspend;

	// Token: 0x140000B9 RID: 185
	// (add) Token: 0x06003B9B RID: 15259
	// (remove) Token: 0x06003B9C RID: 15260
	event OnResumeHandler OnResume;

	// Token: 0x140000BA RID: 186
	// (add) Token: 0x06003B9D RID: 15261
	// (remove) Token: 0x06003B9E RID: 15262
	event OnConstrainedHandler OnConstrained;

	// Token: 0x140000BB RID: 187
	// (add) Token: 0x06003B9F RID: 15263
	// (remove) Token: 0x06003BA0 RID: 15264
	event OnUnconstrainedHandler OnUnconstrained;

	// Token: 0x06003BA1 RID: 15265
	void Init();

	// Token: 0x06003BA2 RID: 15266
	bool IsConstrained();
}
