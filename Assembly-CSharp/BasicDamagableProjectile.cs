using System;
using UnityEngine;

// Token: 0x0200058B RID: 1419
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DamageReceiver))]
public class BasicDamagableProjectile : BasicProjectile
{
	// Token: 0x06003BF6 RID: 15350 RVA: 0x00114798 File Offset: 0x00112998
	public virtual BasicDamagableProjectile Create(Vector2 position, float rotation, float speed, float health)
	{
		BasicDamagableProjectile basicDamagableProjectile = this.Create(position, rotation, speed) as BasicDamagableProjectile;
		basicDamagableProjectile.health = health;
		return basicDamagableProjectile;
	}

	// Token: 0x06003BF7 RID: 15351 RVA: 0x001147C0 File Offset: 0x001129C0
	public virtual BasicDamagableProjectile Create(Vector2 position, float rotation, Vector2 scale, float speed, float health)
	{
		BasicDamagableProjectile basicDamagableProjectile = this.Create(position, rotation, scale, speed) as BasicDamagableProjectile;
		basicDamagableProjectile.health = health;
		return basicDamagableProjectile;
	}

	// Token: 0x06003BF8 RID: 15352 RVA: 0x000308FF File Offset: 0x0002EAFF
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06003BF9 RID: 15353 RVA: 0x0003092A File Offset: 0x0002EB2A
	public override void OnDestroy()
	{
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		base.OnDestroy();
	}

	// Token: 0x06003BFA RID: 15354 RVA: 0x00030949 File Offset: 0x0002EB49
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003BFB RID: 15355 RVA: 0x00030967 File Offset: 0x0002EB67
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x04002FA1 RID: 12193
	public float health = 10f;

	// Token: 0x04002FA2 RID: 12194
	public DamageReceiver damageReceiver;
}
