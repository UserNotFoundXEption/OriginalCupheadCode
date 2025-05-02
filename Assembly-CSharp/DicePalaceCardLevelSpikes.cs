using System;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class DicePalaceCardLevelSpikes : AbstractCollidableObject
{
	// Token: 0x060015DE RID: 5598 RVA: 0x0001288C File Offset: 0x00010A8C
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x0001289F File Offset: 0x00010A9F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060015E0 RID: 5600 RVA: 0x000128BD File Offset: 0x00010ABD
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x040011D5 RID: 4565
	public DamageDealer damageDealer;
}
