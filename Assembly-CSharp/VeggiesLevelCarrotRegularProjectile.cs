using System;
using UnityEngine;

// Token: 0x020003CC RID: 972
public class VeggiesLevelCarrotRegularProjectile : AbstractProjectile
{
	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06002ACB RID: 10955 RVA: 0x00023F3C File Offset: 0x0002213C
	public override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x06002ACC RID: 10956 RVA: 0x000D4D24 File Offset: 0x000D2F24
	public VeggiesLevelCarrotRegularProjectile Create(VeggiesLevelCarrot parent, Vector2 pos, float speed, float rotation)
	{
		VeggiesLevelCarrotRegularProjectile veggiesLevelCarrotRegularProjectile = this.Create() as VeggiesLevelCarrotRegularProjectile;
		veggiesLevelCarrotRegularProjectile.CollisionDeath.None();
		veggiesLevelCarrotRegularProjectile.DamagesType.OnlyPlayer();
		veggiesLevelCarrotRegularProjectile.Init(parent, pos, speed, rotation);
		return veggiesLevelCarrotRegularProjectile;
	}

	// Token: 0x06002ACD RID: 10957 RVA: 0x000D4D60 File Offset: 0x000D2F60
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		base.transform.position += base.transform.right * (this.speed * CupheadTime.FixedDelta);
	}

	// Token: 0x06002ACE RID: 10958 RVA: 0x00023F43 File Offset: 0x00022143
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.parent.OnDeathEvent -= this.Die;
	}

	// Token: 0x06002ACF RID: 10959 RVA: 0x00023F63 File Offset: 0x00022163
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002AD0 RID: 10960 RVA: 0x000D4DB4 File Offset: 0x000D2FB4
	public void Init(VeggiesLevelCarrot parent, Vector2 pos, float speed, float rotation)
	{
		this.parent = parent;
		this.speed = speed;
		parent.OnDeathEvent += this.Die;
		base.transform.position = pos;
		base.transform.SetLocalEulerAngles(new float?(0f), new float?(0f), new float?(rotation));
	}

	// Token: 0x06002AD1 RID: 10961 RVA: 0x00023F81 File Offset: 0x00022181
	public override void Die()
	{
		AudioManager.Play("level_veggies_carrot_projectile_death");
		base.Die();
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
	}

	// Token: 0x040023A4 RID: 9124
	public float speed;

	// Token: 0x040023A5 RID: 9125
	public VeggiesLevelCarrot parent;
}
