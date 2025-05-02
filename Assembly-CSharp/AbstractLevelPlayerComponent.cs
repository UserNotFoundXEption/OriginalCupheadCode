using System;

// Token: 0x0200050B RID: 1291
public abstract class AbstractLevelPlayerComponent : AbstractPlayerComponent
{
	// Token: 0x060035E3 RID: 13795 RVA: 0x0002C23C File Offset: 0x0002A43C
	public AbstractLevelPlayerComponent()
	{
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x060035E4 RID: 13796 RVA: 0x0002C244 File Offset: 0x0002A444
	// (set) Token: 0x060035E5 RID: 13797 RVA: 0x0002C24C File Offset: 0x0002A44C
	public LevelPlayerController player { get; set; }

	// Token: 0x060035E6 RID: 13798 RVA: 0x0002C255 File Offset: 0x0002A455
	public override void OnAwake()
	{
		base.OnAwake();
		this.player = (base.basePlayer as LevelPlayerController);
	}
}
