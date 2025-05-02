using System;
using UnityEngine;

// Token: 0x0200055A RID: 1370
public class WeaponWideShotProjectile : BasicProjectile
{
	// Token: 0x06003948 RID: 14664 RVA: 0x0002EA7B File Offset: 0x0002CC7B
	public override void Start()
	{
		base.Start();
		this.damageDealer.isDLCWeapon = true;
		base.GetComponent<SpriteRenderer>().flipY = Rand.Bool();
	}

	// Token: 0x06003949 RID: 14665 RVA: 0x0002EA9F File Offset: 0x0002CC9F
	public override void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		base.OnDealDamage(damage, receiver, damageDealer);
		this.hitSpark.Create(base.transform.position + base.transform.right * 100f);
	}

	// Token: 0x0600394A RID: 14666 RVA: 0x0010B890 File Offset: 0x00109A90
	public override void OnCollisionDie(GameObject hit, CollisionPhase phase)
	{
		this.hitSpark.Create(base.transform.position + base.transform.right * 100f);
		base.OnCollisionDie(hit, phase);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04002E0F RID: 11791
	public const float HITSPARK_OFFSET = 100f;

	// Token: 0x04002E10 RID: 11792
	[SerializeField]
	public Effect hitSpark;
}
