using System;
using UnityEngine;

// Token: 0x02000426 RID: 1062
public class HarbourPlatformingLevelBarnacleShot : BasicProjectile
{
	// Token: 0x06002DFA RID: 11770 RVA: 0x000DE1B8 File Offset: 0x000DC3B8
	public override BasicProjectile Create(Vector2 position, float rotation, float speed)
	{
		HarbourPlatformingLevelBarnacleShot harbourPlatformingLevelBarnacleShot = base.Create(position, rotation, speed) as HarbourPlatformingLevelBarnacleShot;
		harbourPlatformingLevelBarnacleShot.animator.SetFloat("Speed", ((!Rand.Bool()) ? 1f : -1f) * 1f * Random.Range(0.9f, 1.1f));
		return harbourPlatformingLevelBarnacleShot;
	}

	// Token: 0x04002612 RID: 9746
	public const float ProjectileSpeed = 1f;
}
