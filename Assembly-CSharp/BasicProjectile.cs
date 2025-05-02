using System;
using UnityEngine;

// Token: 0x0200058C RID: 1420
public class BasicProjectile : AbstractProjectile
{
	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x06003BFD RID: 15357 RVA: 0x000309A1 File Offset: 0x0002EBA1
	public virtual Vector3 Direction
	{
		get
		{
			return base.transform.right;
		}
	}

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x06003BFE RID: 15358 RVA: 0x000309AE File Offset: 0x0002EBAE
	public override float DestroyLifetime
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x06003BFF RID: 15359 RVA: 0x000309B5 File Offset: 0x0002EBB5
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x06003C00 RID: 15360 RVA: 0x000309B8 File Offset: 0x0002EBB8
	public virtual string projectileMissImpactSFX
	{
		get
		{
			return "player_weapon_peashot_miss";
		}
	}

	// Token: 0x06003C01 RID: 15361 RVA: 0x001147E8 File Offset: 0x001129E8
	public virtual BasicProjectile Create(Vector2 position, float rotation, float speed)
	{
		BasicProjectile basicProjectile = this.Create(position, rotation) as BasicProjectile;
		basicProjectile.Speed = speed;
		return basicProjectile;
	}

	// Token: 0x06003C02 RID: 15362 RVA: 0x0011480C File Offset: 0x00112A0C
	public virtual BasicProjectile Create(Vector2 position, float rotation, Vector2 scale, float speed)
	{
		BasicProjectile basicProjectile = this.Create(position, rotation, scale) as BasicProjectile;
		basicProjectile.Speed = speed;
		return basicProjectile;
	}

	// Token: 0x06003C03 RID: 15363 RVA: 0x000309BF File Offset: 0x0002EBBF
	public override void Awake()
	{
		base.Awake();
		if (base.CompareTag("EnemyProjectile"))
		{
			this.DamagesType.Player = true;
		}
	}

	// Token: 0x06003C04 RID: 15364 RVA: 0x000309E3 File Offset: 0x0002EBE3
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06003C05 RID: 15365 RVA: 0x000309EB File Offset: 0x0002EBEB
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
	}

	// Token: 0x06003C06 RID: 15366 RVA: 0x000309F5 File Offset: 0x0002EBF5
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x06003C07 RID: 15367 RVA: 0x00030A15 File Offset: 0x0002EC15
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06003C08 RID: 15368 RVA: 0x00030A2D File Offset: 0x0002EC2D
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.DealDamage(hit);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06003C09 RID: 15369 RVA: 0x00114834 File Offset: 0x00112A34
	public override void OnCollisionDie(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionDie(hit, phase);
		if (base.tag == "PlayerProjectile" && phase == CollisionPhase.Enter)
		{
			if ((hit.GetComponent<DamageReceiver>() && hit.GetComponent<DamageReceiver>().enabled) || (hit.GetComponent<DamageReceiverChild>() && hit.GetComponent<DamageReceiverChild>().enabled))
			{
				AudioManager.Play("player_shoot_hit_cuphead");
			}
			else
			{
				AudioManager.Play(this.projectileMissImpactSFX);
			}
		}
	}

	// Token: 0x06003C0A RID: 15370 RVA: 0x00030A45 File Offset: 0x0002EC45
	public void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06003C0B RID: 15371 RVA: 0x001148C0 File Offset: 0x00112AC0
	public override void Die()
	{
		this.move = false;
		EffectSpawner component = base.GetComponent<EffectSpawner>();
		if (component != null)
		{
			Object.Destroy(component);
		}
		base.Die();
	}

	// Token: 0x06003C0C RID: 15372 RVA: 0x00030A54 File Offset: 0x0002EC54
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.move)
		{
			this.Move();
		}
	}

	// Token: 0x06003C0D RID: 15373 RVA: 0x001148F4 File Offset: 0x00112AF4
	public virtual void Move()
	{
		base.transform.position += this.Direction * this.Speed * CupheadTime.FixedDelta - new Vector3(0f, this._accumulativeGravity * CupheadTime.FixedDelta, 0f);
		this._accumulativeGravity += this.Gravity * CupheadTime.FixedDelta;
	}

	// Token: 0x04002FA3 RID: 12195
	[Space(10f)]
	public float Speed;

	// Token: 0x04002FA4 RID: 12196
	public float Gravity;

	// Token: 0x04002FA5 RID: 12197
	[Space(10f)]
	public Sfx SfxOnDeath;

	// Token: 0x04002FA6 RID: 12198
	public bool move = true;

	// Token: 0x04002FA7 RID: 12199
	public float _accumulativeGravity;
}
