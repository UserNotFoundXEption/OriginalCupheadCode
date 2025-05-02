using System;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class BaronessLevelBaronessProjectile : AbstractProjectile
{
	// Token: 0x06000F5B RID: 3931 RVA: 0x0008D5A0 File Offset: 0x0008B7A0
	public override void Start()
	{
		base.Start();
		this.health = (float)base.GetComponentInParent<BaronessLevelBaronessProjectileBunch>().properties.projectileHP;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06000F5C RID: 3932 RVA: 0x0000D08A File Offset: 0x0000B28A
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x0000D0A8 File Offset: 0x0000B2A8
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x0000D0C6 File Offset: 0x0000B2C6
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x0008D5F0 File Offset: 0x0008B7F0
	public override void Die()
	{
		this.deathFX.Create(base.transform.position);
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.FX.SetActive(false);
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x04000C8B RID: 3211
	[SerializeField]
	public Effect deathFX;

	// Token: 0x04000C8C RID: 3212
	[SerializeField]
	public GameObject FX;

	// Token: 0x04000C8D RID: 3213
	public DamageReceiver damageReceiver;

	// Token: 0x04000C8E RID: 3214
	public float health;
}
