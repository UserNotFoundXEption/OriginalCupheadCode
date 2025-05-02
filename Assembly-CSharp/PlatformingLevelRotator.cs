using System;
using UnityEngine;

// Token: 0x02000464 RID: 1124
public class PlatformingLevelRotator : AbstractCollidableObject
{
	// Token: 0x06002FDA RID: 12250 RVA: 0x00027D9F File Offset: 0x00025F9F
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002FDB RID: 12251 RVA: 0x00027DB2 File Offset: 0x00025FB2
	public void Update()
	{
		base.transform.AddEulerAngles(0f, 0f, -this.speed * CupheadTime.Delta);
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002FDC RID: 12252 RVA: 0x00027DF1 File Offset: 0x00025FF1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0400279E RID: 10142
	public float speed = 180f;

	// Token: 0x0400279F RID: 10143
	public DamageDealer damageDealer;
}
