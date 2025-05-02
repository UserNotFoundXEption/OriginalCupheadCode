using System;

// Token: 0x020004F9 RID: 1273
public abstract class AbstractArcadePlayerComponent : AbstractPlayerComponent
{
	// Token: 0x06003496 RID: 13462 RVA: 0x0002B239 File Offset: 0x00029439
	public AbstractArcadePlayerComponent()
	{
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x06003497 RID: 13463 RVA: 0x0002B241 File Offset: 0x00029441
	// (set) Token: 0x06003498 RID: 13464 RVA: 0x0002B249 File Offset: 0x00029449
	public ArcadePlayerController player { get; set; }

	// Token: 0x06003499 RID: 13465 RVA: 0x0002B252 File Offset: 0x00029452
	public override void OnAwake()
	{
		base.OnAwake();
		this.player = (base.basePlayer as ArcadePlayerController);
	}
}
