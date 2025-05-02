using System;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class ClownLevelBomb : AbstractCollidableObject
{
	// Token: 0x06001376 RID: 4982 RVA: 0x00010516 File Offset: 0x0000E716
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x00010529 File Offset: 0x0000E729
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x00010541 File Offset: 0x0000E741
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x04000FD0 RID: 4048
	public DamageDealer damageDealer;
}
