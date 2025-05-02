using System;
using UnityEngine;

// Token: 0x0200020E RID: 526
public class DragonLevelFire : AbstractCollidableObject
{
	// Token: 0x06001821 RID: 6177 RVA: 0x00014A3C File Offset: 0x00012C3C
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001822 RID: 6178 RVA: 0x00014A4F File Offset: 0x00012C4F
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x00014A67 File Offset: 0x00012C67
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001824 RID: 6180 RVA: 0x000A2FB8 File Offset: 0x000A11B8
	public void SetColliderEnabled(bool enabled)
	{
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = enabled;
		}
	}

	// Token: 0x04001389 RID: 5001
	public DamageDealer damageDealer;

	// Token: 0x0400138A RID: 5002
	public Vector3 localPosition;

	// Token: 0x0400138B RID: 5003
	public Vector3 localScale;
}
