using System;
using UnityEngine;

// Token: 0x020003EB RID: 1003
public class ForestPlatformingLevelLobberProjectileExplosion : Effect
{
	// Token: 0x06002C26 RID: 11302 RVA: 0x00024FBA File Offset: 0x000231BA
	public override void Awake()
	{
		base.Awake();
		AudioManager.Play("level_lobber_projectile_explosion");
		this.emitAudioFromObject.Add("level_lobber_projectile_explosion");
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x00024FE7 File Offset: 0x000231E7
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x00024FFF File Offset: 0x000231FF
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0400247F RID: 9343
	public DamageDealer damageDealer;
}
