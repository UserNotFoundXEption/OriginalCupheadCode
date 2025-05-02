using System;
using UnityEngine;

// Token: 0x02000221 RID: 545
public class FlowerLevelFlowerDamageRegion : CollisionChild
{
	// Token: 0x06001901 RID: 6401 RVA: 0x000155D1 File Offset: 0x000137D1
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		base.Awake();
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x000155E4 File Offset: 0x000137E4
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001903 RID: 6403 RVA: 0x000155FC File Offset: 0x000137FC
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x04001429 RID: 5161
	public DamageDealer damageDealer;
}
