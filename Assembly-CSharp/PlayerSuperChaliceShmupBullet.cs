using System;

// Token: 0x0200052B RID: 1323
public class PlayerSuperChaliceShmupBullet : BasicProjectile
{
	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x060037C7 RID: 14279 RVA: 0x0002D886 File Offset: 0x0002BA86
	public override float DestroyLifetime
	{
		get
		{
			return this.lifetimeMax;
		}
	}

	// Token: 0x04002CF4 RID: 11508
	public float lifetimeMax = 20f;
}
