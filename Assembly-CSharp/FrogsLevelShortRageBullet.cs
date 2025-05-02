using System;

// Token: 0x020002A6 RID: 678
public class FrogsLevelShortRageBullet : BasicProjectile
{
	// Token: 0x06001E7D RID: 7805 RVA: 0x00019BF8 File Offset: 0x00017DF8
	public override void Die()
	{
		if (!base.CanParry)
		{
			return;
		}
		base.Die();
		this.move = true;
	}
}
