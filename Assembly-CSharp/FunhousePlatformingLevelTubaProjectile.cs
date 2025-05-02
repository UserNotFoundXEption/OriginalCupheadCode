using System;

// Token: 0x02000423 RID: 1059
public class FunhousePlatformingLevelTubaProjectile : BasicProjectile
{
	// Token: 0x1700035E RID: 862
	// (get) Token: 0x06002DE2 RID: 11746 RVA: 0x00026449 File Offset: 0x00024649
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x06002DE3 RID: 11747 RVA: 0x0002644C File Offset: 0x0002464C
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002DE4 RID: 11748 RVA: 0x00026453 File Offset: 0x00024653
	public override void Awake()
	{
		base.Awake();
		this.DestroyDistance = 0f;
	}
}
