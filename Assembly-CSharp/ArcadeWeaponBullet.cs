using System;
using UnityEngine;

// Token: 0x02000502 RID: 1282
public class ArcadeWeaponBullet : BasicProjectile
{
	// Token: 0x060035A7 RID: 13735 RVA: 0x000FA8D8 File Offset: 0x000F8AD8
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<RetroArcadeEnemy>())
		{
			ArcadeWeaponBullet.IN_COMBO = true;
			ArcadeWeaponBullet.POINTS_BONUS_ACCURACY += RetroArcadeLevel.ACCURACY_BONUS;
		}
		else if (ArcadeWeaponBullet.IN_COMBO)
		{
			RetroArcadeLevel.TOTAL_POINTS += ArcadeWeaponBullet.POINTS_BONUS_ACCURACY;
			ArcadeWeaponBullet.POINTS_BONUS_ACCURACY = 0f;
			ArcadeWeaponBullet.IN_COMBO = false;
		}
	}

	// Token: 0x04002B94 RID: 11156
	public static float POINTS_BONUS_ACCURACY;

	// Token: 0x04002B95 RID: 11157
	public static bool IN_COMBO;
}
