using System;
using UnityEngine;

// Token: 0x020000D3 RID: 211
[RequireComponent(typeof(Collider2D))]
public class BasicDamageDealingObject : AbstractCollidableObject
{
	// Token: 0x060009F6 RID: 2550 RVA: 0x00009218 File Offset: 0x00007418
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageDealer.SetRate(this.damageRate);
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x0000923C File Offset: 0x0000743C
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x0000925A File Offset: 0x0000745A
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0400079B RID: 1947
	[SerializeField]
	public float damageRate = 0.2f;

	// Token: 0x0400079C RID: 1948
	public DamageDealer damageDealer;
}
