using System;
using UnityEngine;

// Token: 0x0200015E RID: 350
public class BatLevelMiniBat : BasicSineProjectile
{
	// Token: 0x060010D7 RID: 4311 RVA: 0x00091268 File Offset: 0x0008F468
	public BatLevelMiniBat Create(Vector2 pos, float rotation, float velocity, float sinVelocity, float sinSize, float health)
	{
		BatLevelMiniBat batLevelMiniBat = base.Create(pos, rotation, velocity, sinVelocity, sinSize) as BatLevelMiniBat;
		batLevelMiniBat.health = health;
		return batLevelMiniBat;
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x0000E30A File Offset: 0x0000C50A
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x0000E335 File Offset: 0x0000C535
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000DB4 RID: 3508
	public DamageReceiver damageReceiver;

	// Token: 0x04000DB5 RID: 3509
	public float health;
}
