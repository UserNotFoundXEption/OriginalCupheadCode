using System;
using UnityEngine;

// Token: 0x020001B8 RID: 440
[RequireComponent(typeof(Collider2D))]
public class DevilLevelSplitDevilVisual : LevelProperties.Devil.Entity
{
	// Token: 0x060014EC RID: 5356 RVA: 0x00011BF5 File Offset: 0x0000FDF5
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += base.GetComponentInParent<DevilLevelSplitDevil>().OnDamageTaken;
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x00011C30 File Offset: 0x0000FE30
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x00011C48 File Offset: 0x0000FE48
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x04001125 RID: 4389
	public DamageDealer damageDealer;

	// Token: 0x04001126 RID: 4390
	public DamageReceiver damageReceiver;
}
