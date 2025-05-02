using System;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class DevilLevelSplitDevilProjectile : BasicProjectile
{
	// Token: 0x0600155D RID: 5469 RVA: 0x0009C060 File Offset: 0x0009A260
	public DevilLevelSplitDevilProjectile Create(Vector2 position, float rotation, float speed, DevilLevelSplitDevil devil)
	{
		DevilLevelSplitDevilProjectile devilLevelSplitDevilProjectile = base.Create(position, rotation, speed) as DevilLevelSplitDevilProjectile;
		devilLevelSplitDevilProjectile.devil = devil;
		return devilLevelSplitDevilProjectile;
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x00012260 File Offset: 0x00010460
	public override void Update()
	{
		base.Update();
		if (base.dead)
		{
			return;
		}
		if (this.devil == null)
		{
			this.Die();
			return;
		}
		this.UpdateColor();
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x00012292 File Offset: 0x00010492
	public void UpdateColor()
	{
	}

	// Token: 0x06001560 RID: 5472 RVA: 0x00012294 File Offset: 0x00010494
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001179 RID: 4473
	public DevilLevelSplitDevil devil;
}
