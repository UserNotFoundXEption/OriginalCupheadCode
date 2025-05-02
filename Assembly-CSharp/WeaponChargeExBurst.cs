using System;
using UnityEngine;

// Token: 0x0200053D RID: 1341
public class WeaponChargeExBurst : AbstractProjectile
{
	// Token: 0x0600387E RID: 14462 RVA: 0x0002E12C File Offset: 0x0002C32C
	public override void Start()
	{
		base.Start();
		base.GetComponent<SpriteRenderer>().flipX = Rand.Bool();
	}

	// Token: 0x0600387F RID: 14463 RVA: 0x0002E144 File Offset: 0x0002C344
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter && this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003880 RID: 14464 RVA: 0x0002E16C File Offset: 0x0002C36C
	public void OnEffectComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04002D6B RID: 11627
	public const float Offset = 125f;
}
