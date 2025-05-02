using System;
using UnityEngine;

// Token: 0x0200029D RID: 669
public class FrogsLevelMorphedCoin : BasicProjectile
{
	// Token: 0x06001E33 RID: 7731 RVA: 0x000B2808 File Offset: 0x000B0A08
	public FrogsLevelMorphedCoin CreateCoin(Vector2 pos, float speed, float rotation)
	{
		FrogsLevelMorphedCoin frogsLevelMorphedCoin = base.Create(pos, rotation, speed) as FrogsLevelMorphedCoin;
		frogsLevelMorphedCoin.CollisionDeath.None();
		frogsLevelMorphedCoin.DamagesType.OnlyPlayer();
		return frogsLevelMorphedCoin;
	}
}
