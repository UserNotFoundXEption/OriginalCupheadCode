using System;

// Token: 0x020003D3 RID: 979
public class VeggiesLevelSpit : BasicProjectile
{
	// Token: 0x06002B30 RID: 11056 RVA: 0x0002442D File Offset: 0x0002262D
	public override void Die()
	{
		if (base.CanParry)
		{
			AudioManager.Play("level_veggies_potato_worm_explode");
		}
		base.Die();
	}
}
