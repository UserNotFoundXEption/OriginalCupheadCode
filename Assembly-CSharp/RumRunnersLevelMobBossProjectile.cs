using System;
using UnityEngine;

// Token: 0x0200034A RID: 842
public class RumRunnersLevelMobBossProjectile : BasicProjectile
{
	// Token: 0x060024E9 RID: 9449 RVA: 0x000C5554 File Offset: 0x000C3754
	public override BasicProjectile Create(Vector2 position, float rotation, float speed)
	{
		BasicProjectile basicProjectile = base.Create(position, rotation, speed);
		basicProjectile.CollisionDeath.None();
		basicProjectile.DamagesType.OnlyPlayer();
		return basicProjectile;
	}
}
