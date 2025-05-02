using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000536 RID: 1334
public class WeaponArcProjectile : AbstractProjectile
{
	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x06003837 RID: 14391 RVA: 0x0002DE5B File Offset: 0x0002C05B
	public override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x06003838 RID: 14392 RVA: 0x0002DE62 File Offset: 0x0002C062
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06003839 RID: 14393 RVA: 0x00106450 File Offset: 0x00104650
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		WeaponArcProjectile.State state = this._state;
		if (state != WeaponArcProjectile.State.InAir)
		{
			if (state == WeaponArcProjectile.State.OnGround)
			{
				this.UpdateOnGround();
			}
		}
		else
		{
			this.UpdateInAir();
		}
		if (!this.isEx)
		{
			this.UpdateDamageState();
			if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(150f, 1000f)))
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x0600383A RID: 14394 RVA: 0x001064E8 File Offset: 0x001046E8
	public void UpdateInAir()
	{
		this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
		base.transform.position += this.velocity * CupheadTime.FixedDelta;
	}

	// Token: 0x0600383B RID: 14395 RVA: 0x0002DE6A File Offset: 0x0002C06A
	public void UpdateOnGround()
	{
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x00106540 File Offset: 0x00104740
	public void UpdateDamageState()
	{
		if (base.lifetime < WeaponProperties.LevelWeaponArc.Basic.timeStateTwo)
		{
			this.Damage = WeaponProperties.LevelWeaponArc.Basic.baseDamage;
			base.transform.SetScale(new float?(1f), new float?(1f), null);
		}
		else if (base.lifetime < WeaponProperties.LevelWeaponArc.Basic.timeStateThree)
		{
			this.Damage = WeaponProperties.LevelWeaponArc.Basic.damageStateTwo;
			base.transform.SetScale(new float?(1.5f), new float?(1.5f), null);
		}
		else
		{
			this.Damage = WeaponProperties.LevelWeaponArc.Basic.damageStateThree;
			base.transform.SetScale(new float?(2.5f), new float?(2.5f), null);
		}
	}

	// Token: 0x0600383D RID: 14397 RVA: 0x00106610 File Offset: 0x00104810
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		LevelPlatform component = hit.GetComponent<LevelPlatform>();
		if (this._state == WeaponArcProjectile.State.InAir && (component == null || (!component.canFallThrough && this.velocity.y < 0f)))
		{
			this.HitGround(hit);
		}
	}

	// Token: 0x0600383E RID: 14398 RVA: 0x0010666C File Offset: 0x0010486C
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		LevelPlatform component = hit.GetComponent<LevelPlatform>();
		if (this._state == WeaponArcProjectile.State.InAir && component != null && !component.canFallThrough && this.velocity.y < 0f)
		{
			this.HitGround(hit);
		}
	}

	// Token: 0x0600383F RID: 14399 RVA: 0x0002DE6C File Offset: 0x0002C06C
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (!this.isEx)
		{
			this.damageDealer.SetDamage(this.Damage);
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06003840 RID: 14400 RVA: 0x001066C8 File Offset: 0x001048C8
	public void HitGround(GameObject hit)
	{
		this._state = WeaponArcProjectile.State.OnGround;
		if (!this.isEx)
		{
			this.weapon.projectilesOnGround.Add(this);
			if (this.weapon.projectilesOnGround.Count > WeaponProperties.LevelWeaponArc.Basic.maxNumMines)
			{
				WeaponArcProjectile weaponArcProjectile = this.weapon.projectilesOnGround[0];
				this.weapon.projectilesOnGround.RemoveAt(0);
				weaponArcProjectile.Die();
			}
		}
		else
		{
			base.StartCoroutine(this.timedExplode_cr());
		}
		base.transform.SetParent(hit.transform);
	}

	// Token: 0x06003841 RID: 14401 RVA: 0x00106760 File Offset: 0x00104960
	public IEnumerator timedExplode_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, WeaponProperties.LevelWeaponArc.Ex.explodeDelay);
		this.Die();
		yield break;
	}

	// Token: 0x06003842 RID: 14402 RVA: 0x0010677C File Offset: 0x0010497C
	public override void Die()
	{
		base.Die();
		if (this.isEx)
		{
			this.exExplosion.Create(base.transform.position, this.Damage, base.DamageMultiplier, this.PlayerId);
			AudioManager.Play("player_weapon_arc_ex_explosion");
			this.emitAudioFromObject.Add("player_weapon_arc_ex_explosion");
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003843 RID: 14403 RVA: 0x0002DE9F File Offset: 0x0002C09F
	public override void OnDestroy()
	{
		if (this.weapon.projectilesOnGround.Contains(this))
		{
			this.weapon.projectilesOnGround.Remove(this);
		}
		base.OnDestroy();
	}

	// Token: 0x04002D35 RID: 11573
	[SerializeField]
	public bool isEx;

	// Token: 0x04002D36 RID: 11574
	[SerializeField]
	public WeaponArcProjectileExplosion exExplosion;

	// Token: 0x04002D37 RID: 11575
	public float chargeTime;

	// Token: 0x04002D38 RID: 11576
	public float gravity;

	// Token: 0x04002D39 RID: 11577
	public Vector2 velocity;

	// Token: 0x04002D3A RID: 11578
	public WeaponArc weapon;

	// Token: 0x04002D3B RID: 11579
	public WeaponArcProjectile.State _state;

	// Token: 0x020011BE RID: 4542
	public enum State
	{
		// Token: 0x04007BFD RID: 31741
		InAir,
		// Token: 0x04007BFE RID: 31742
		OnGround
	}
}
