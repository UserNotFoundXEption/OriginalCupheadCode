using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000540 RID: 1344
public class WeaponCrackshotExProjectile : BasicProjectile
{
	// Token: 0x0600388B RID: 14475 RVA: 0x0002E1FA File Offset: 0x0002C3FA
	public override void OnDieDistance()
	{
	}

	// Token: 0x0600388C RID: 14476 RVA: 0x0002E1FC File Offset: 0x0002C3FC
	public override void OnDieLifetime()
	{
	}

	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x0600388D RID: 14477 RVA: 0x0002E1FE File Offset: 0x0002C3FE
	public override float ParryMeterMultiplier
	{
		get
		{
			return (this.parryTimeOut != 0f) ? 0.1f : 1f;
		}
	}

	// Token: 0x0600388E RID: 14478 RVA: 0x00107C6C File Offset: 0x00105E6C
	public override void Start()
	{
		base.Start();
		this._countParryTowardsScore = false;
		this.move = false;
		this.shotNumber = WeaponProperties.LevelWeaponCrackshot.Ex.shotNumber;
		base.transform.position += base.transform.right * 120f;
		this.angle = base.transform.eulerAngles.z;
		base.transform.eulerAngles = Vector3.zero;
		base.transform.localScale = new Vector3(Mathf.Sign(MathUtils.AngleToDirection(this.angle).x), 1f);
		this.basePos = base.transform.position;
		this.startPos = base.transform.position;
		this.damageDealer.SetDamage(WeaponProperties.LevelWeaponCrackshot.Ex.collideDamage);
		this.damageDealer.isDLCWeapon = true;
		this.SetParryable(WeaponProperties.LevelWeaponCrackshot.Ex.isPink);
		AudioManager.FadeSFXVolume("player_weapon_crackshot_turret_loop", 0.0001f, 0.0001f);
	}

	// Token: 0x0600388F RID: 14479 RVA: 0x0002E21F File Offset: 0x0002C41F
	public void GetReplaced()
	{
		this.LaunchAtTarget();
	}

	// Token: 0x06003890 RID: 14480 RVA: 0x00107D78 File Offset: 0x00105F78
	public void AniEvent_StartSpinSFX()
	{
		AudioManager.Play("player_weapon_crackshot_turret_loop_start");
		this.emitAudioFromObject.Add("player_weapon_crackshot_turret_loop_start");
		AudioManager.PlayLoop("player_weapon_crackshot_turret_loop");
		this.emitAudioFromObject.Add("player_weapon_crackshot_turret_loop");
		AudioManager.FadeSFXVolumeLinear("player_weapon_crackshot_turret_loop", 0.3f, 1f);
	}

	// Token: 0x06003891 RID: 14481 RVA: 0x0002E227 File Offset: 0x0002C427
	public override void OnParry(AbstractPlayerController player)
	{
		if (!this.parried)
		{
			this.LaunchAtTarget();
		}
		else
		{
			this.Die();
		}
	}

	// Token: 0x06003892 RID: 14482 RVA: 0x00107DD0 File Offset: 0x00105FD0
	public void LaunchAtTarget()
	{
		base.animator.Play("Launch");
		AudioManager.Stop("player_weapon_crackshot_turret_loop");
		AudioManager.Play("player_weapon_crackshot_turret_parry");
		this.emitAudioFromObject.Add("player_weapon_crackshot_turret_parry");
		Collider2D collider2D = this.FindTarget();
		if (collider2D)
		{
			this.angle = MathUtils.DirectionToAngle(collider2D.bounds.center - base.transform.position);
		}
		Effect effect = this.launchFXPrefab.Create(base.transform.position);
		effect.transform.eulerAngles = new Vector3(0f, 0f, this.angle);
		effect.transform.localScale = new Vector3(-1f, (float)MathUtils.PlusOrMinus());
		this.parried = true;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.angle));
		base.transform.localScale = new Vector3(-1f, 1f);
		this.Speed = WeaponProperties.LevelWeaponCrackshot.Ex.parryBulletSpeed;
		this.damageDealer.SetDamage(WeaponProperties.LevelWeaponCrackshot.Ex.parryBulletDamage);
		this.SetParryable(false);
		this.parryTimeOut = WeaponProperties.LevelWeaponCrackshot.Ex.parryTimeOut;
		this.move = true;
	}

	// Token: 0x06003893 RID: 14483 RVA: 0x00107F24 File Offset: 0x00106124
	public override void Die()
	{
		base.animator.Play("Explode");
		AudioManager.Stop("player_weapon_crackshot_turret_parry");
		AudioManager.Play("player_weapon_crackshot_turret_parryexplode");
		this.emitAudioFromObject.Add("player_weapon_crackshot_turret_parryexplode");
		base.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		base.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), (float)MathUtils.PlusOrMinus());
		this.move = false;
		this.coll.enabled = false;
	}

	// Token: 0x06003894 RID: 14484 RVA: 0x0002E245 File Offset: 0x0002C445
	public void _OnDieAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06003895 RID: 14485 RVA: 0x0002E252 File Offset: 0x0002C452
	public override void OnDestroy()
	{
		base.OnDestroy();
		AudioManager.FadeSFXVolume("player_weapon_crackshot_turret_loop", 0.0001f, 0.25f);
	}

	// Token: 0x06003896 RID: 14486 RVA: 0x00107FBC File Offset: 0x001061BC
	public void HandleShot()
	{
		this.shootTimer -= CupheadTime.FixedDelta;
		if (this.shootTimer <= 0f)
		{
			Collider2D collider2D = this.FindTarget();
			if (collider2D)
			{
				MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
				BasicProjectile projectile = this.childPrefab.Create(base.transform.position, MathUtils.DirectionToAngle(collider2D.bounds.center - base.transform.position), WeaponProperties.LevelWeaponCrackshot.Ex.bulletSpeed);
				this.childPrefab.Damage = WeaponProperties.LevelWeaponCrackshot.Ex.bulletDamage;
				this.childPrefab.Speed = WeaponProperties.LevelWeaponCrackshot.Ex.bulletSpeed;
				this.childPrefab.PlayerId = this.PlayerId;
				meterScoreTracker.Add(projectile);
				base.StartCoroutine(this.shoot_stretch_squash_cr());
				this.shootFXPrefab.Create(base.transform.position + (collider2D.bounds.center - base.transform.position).normalized * 25f);
				AudioManager.Play("player_weapon_crackshot_turret_shoot");
				this.emitAudioFromObject.Add("player_weapon_crackshot_turret_shoot");
			}
			this.shotNumber--;
			if (this.shotNumber == 0)
			{
				base.animator.SetTrigger("Disappear");
				this.coll.enabled = false;
			}
			else
			{
				this.shootTimer += WeaponProperties.LevelWeaponCrackshot.Ex.shootDelay;
			}
		}
	}

	// Token: 0x06003897 RID: 14487 RVA: 0x00108148 File Offset: 0x00106348
	public IEnumerator shoot_stretch_squash_cr()
	{
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.localScale.x) * 1.2f, Mathf.Sign(base.transform.localScale.y) * 1.2f);
		yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.localScale.x) * 1.25f, Mathf.Sign(base.transform.localScale.y) * 1.25f);
		yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.localScale.x) * 1.22f, Mathf.Sign(base.transform.localScale.y) * 1.22f);
		yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.localScale.x) * 1.16f, Mathf.Sign(base.transform.localScale.y) * 1.16f);
		yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.localScale.x), Mathf.Sign(base.transform.localScale.y));
		yield break;
	}

	// Token: 0x06003898 RID: 14488 RVA: 0x00108164 File Offset: 0x00106364
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.parried)
		{
			if (this.parryTimeOut > 0f)
			{
				this.parryTimeOut -= CupheadTime.FixedDelta;
				if (this.parryTimeOut <= 0f)
				{
					this.parryTimeOut = 0f;
					this.SetParryable(true);
				}
			}
			return;
		}
		if (base.lifetime < WeaponProperties.LevelWeaponCrackshot.Ex.timeToHoverPoint)
		{
			this.basePos = Vector3.Lerp(this.startPos, this.startPos + MathUtils.AngleToDirection(this.angle) * WeaponProperties.LevelWeaponCrackshot.Ex.launchDistance, EaseUtils.EaseOutSine(0f, 1f, base.lifetime / WeaponProperties.LevelWeaponCrackshot.Ex.timeToHoverPoint));
		}
		else
		{
			if (!this.timerSet)
			{
				this.timerSet = true;
				this.basePos = this.startPos + MathUtils.AngleToDirection(this.angle) * WeaponProperties.LevelWeaponCrackshot.Ex.launchDistance;
				this.shootTimer = WeaponProperties.LevelWeaponCrackshot.Ex.shootDelay;
			}
			this.basePos += Vector3.up * WeaponProperties.LevelWeaponCrackshot.Ex.riseSpeed * CupheadTime.FixedDelta;
			this.HandleShot();
		}
		if (this.shotNumber > 0)
		{
			float num = base.lifetime * WeaponProperties.LevelWeaponCrackshot.Ex.hoverSpeed;
			base.transform.position = this.basePos + new Vector3(Mathf.Cos(num + 1.57079637f) * WeaponProperties.LevelWeaponCrackshot.Ex.hoverWidth * -Mathf.Sign(MathUtils.AngleToDirection(this.angle).x), Mathf.Sin(num * 2f) * WeaponProperties.LevelWeaponCrackshot.Ex.hoverHeight);
		}
	}

	// Token: 0x06003899 RID: 14489 RVA: 0x0002E26E File Offset: 0x0002C46E
	public Collider2D FindTarget()
	{
		return this.findBestTarget(AbstractProjectile.FindOverlapScreenDamageReceivers());
	}

	// Token: 0x0600389A RID: 14490 RVA: 0x00108318 File Offset: 0x00106518
	public Collider2D findBestTarget(IEnumerable<DamageReceiver> damageReceivers)
	{
		float num = float.MaxValue;
		Collider2D result = null;
		Vector2 vector = base.transform.position;
		foreach (DamageReceiver damageReceiver in damageReceivers)
		{
			if (damageReceiver.gameObject.activeInHierarchy && damageReceiver.enabled && damageReceiver.type == DamageReceiver.Type.Enemy)
			{
				foreach (Collider2D collider2D in damageReceiver.GetComponents<Collider2D>())
				{
					if (collider2D.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D.bounds.center, collider2D.bounds.size / 2f))
					{
						float sqrMagnitude = (vector - collider2D.bounds.center).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							result = collider2D;
						}
					}
				}
				foreach (DamageReceiverChild damageReceiverChild in damageReceiver.GetComponentsInChildren<DamageReceiverChild>())
				{
					foreach (Collider2D collider2D2 in damageReceiverChild.GetComponents<Collider2D>())
					{
						if (collider2D2.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D2.bounds.center, collider2D2.bounds.size / 2f))
						{
							float sqrMagnitude2 = (vector - collider2D2.bounds.center).sqrMagnitude;
							if (sqrMagnitude2 < num)
							{
								num = sqrMagnitude2;
								result = collider2D2;
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x04002D74 RID: 11636
	[SerializeField]
	public WeaponCrackshotExProjectileChild childPrefab;

	// Token: 0x04002D75 RID: 11637
	[SerializeField]
	public Effect shootFXPrefab;

	// Token: 0x04002D76 RID: 11638
	[SerializeField]
	public Effect launchFXPrefab;

	// Token: 0x04002D77 RID: 11639
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04002D78 RID: 11640
	public Vector3 basePos;

	// Token: 0x04002D79 RID: 11641
	public Vector3 startPos;

	// Token: 0x04002D7A RID: 11642
	public int shotNumber = 5;

	// Token: 0x04002D7B RID: 11643
	public float shootTimer;

	// Token: 0x04002D7C RID: 11644
	public bool timerSet;

	// Token: 0x04002D7D RID: 11645
	public bool parried;

	// Token: 0x04002D7E RID: 11646
	public float parryTimeOut;

	// Token: 0x04002D7F RID: 11647
	public float angle;
}
