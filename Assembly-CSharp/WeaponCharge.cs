using System;
using UnityEngine;

// Token: 0x0200053C RID: 1340
public class WeaponCharge : AbstractLevelWeapon
{
	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x06003872 RID: 14450 RVA: 0x0002E056 File Offset: 0x0002C256
	public override bool rapidFire
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x06003873 RID: 14451 RVA: 0x0002E059 File Offset: 0x0002C259
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponCharge.Basic.fireRate;
		}
	}

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x06003874 RID: 14452 RVA: 0x0002E060 File Offset: 0x0002C260
	public override bool isChargeWeapon
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003875 RID: 14453 RVA: 0x00107660 File Offset: 0x00105860
	public override void StartCharging()
	{
		base.StartCharging();
		this.BasicSoundOneShot("player_weapon_charge_start", "player_weapon_charge_start_p2");
		if (this.chargeEffect != null)
		{
			Object.Destroy(this.chargeEffect.gameObject);
			this.chargeEffect = null;
		}
		this.chargeEffect = this.chargeEffectPrefab.Create(base.transform.position);
		this.chargeEffect.transform.parent = this.player.transform;
		this.timeCharged = 0f;
	}

	// Token: 0x06003876 RID: 14454 RVA: 0x0002E063 File Offset: 0x0002C263
	public override void StopCharging()
	{
		base.StopCharging();
		if (this.chargeEffect != null)
		{
			Object.Destroy(this.chargeEffect.gameObject);
			this.chargeEffect = null;
			this.timeCharged = 0f;
		}
	}

	// Token: 0x06003877 RID: 14455 RVA: 0x001076F4 File Offset: 0x001058F4
	public void FixedUpdate()
	{
		if (this.chargeEffect == null)
		{
			this.fullyCharged = false;
			this.damage = WeaponProperties.LevelWeaponCharge.Basic.baseDamage;
		}
		else
		{
			this.chargeEffect.transform.position = this.player.weaponManager.GetBulletPosition();
			this.timeCharged += CupheadTime.FixedDelta;
			if (this.timeCharged > WeaponProperties.LevelWeaponCharge.Basic.timeStateThree)
			{
				this.fullyCharged = true;
				if (this.AllowChargeSound)
				{
					AudioManager.Play("player_weapon_charge_ready");
					this.AllowChargeSound = false;
				}
				this.chargeEffect.animator.SetTrigger("IsFull");
				this.damage = WeaponProperties.LevelWeaponCharge.Basic.damageStateThree;
			}
			else
			{
				this.fullyCharged = false;
				this.damage = WeaponProperties.LevelWeaponCharge.Basic.baseDamage;
			}
		}
	}

	// Token: 0x06003878 RID: 14456 RVA: 0x001077CC File Offset: 0x001059CC
	public override AbstractProjectile fireBasic()
	{
		WeaponChargeProjectile weaponChargeProjectile;
		if (this.fullyCharged)
		{
			Effect basicEffectPrefab = this.basicEffectPrefab;
			this.basicEffectPrefab = null;
			weaponChargeProjectile = (base.fireBasic() as WeaponChargeProjectile);
			this.basicEffectPrefab = basicEffectPrefab;
		}
		else
		{
			weaponChargeProjectile = (base.fireBasic() as WeaponChargeProjectile);
		}
		weaponChargeProjectile.Speed = ((!this.fullyCharged) ? WeaponProperties.LevelWeaponCharge.Basic.speed : WeaponProperties.LevelWeaponCharge.Basic.speedStateTwo);
		weaponChargeProjectile.Damage = this.damage;
		weaponChargeProjectile.PlayerId = this.player.id;
		weaponChargeProjectile.DamagesType.PlayerProjectileDefault();
		weaponChargeProjectile.CollisionDeath.PlayerProjectileDefault();
		if (this.fullyCharged && this.player.motor.Ducking)
		{
			weaponChargeProjectile.CollisionDeath.Ground = false;
			weaponChargeProjectile.CollisionDeath.Walls = false;
			weaponChargeProjectile.CollisionDeath.Other = false;
		}
		weaponChargeProjectile.fullyCharged = this.fullyCharged;
		weaponChargeProjectile.animator.SetBool("FullCharge", this.fullyCharged);
		if (this.chargeEffect != null)
		{
			Object.Destroy(this.chargeEffect.gameObject);
			this.chargeEffect = null;
			this.timeCharged = 0f;
		}
		if (this.fullyCharged)
		{
			Effect effect = this.fullChargeFx.Create(weaponChargeProjectile.transform.position);
			effect.transform.eulerAngles = new Vector3(0f, 0f, this.weaponManager.GetBulletRotation());
			this.BasicSoundOneShot("player_weapon_charge_full_fireball", "player_weapon_charge_full_fireball_p2");
			this.AllowChargeSound = true;
		}
		else
		{
			this.BasicSoundOneShot("player_weapon_charge_fire_small", "player_weapon_charge_fire_small_p2");
		}
		return weaponChargeProjectile;
	}

	// Token: 0x06003879 RID: 14457 RVA: 0x00107978 File Offset: 0x00105B78
	public override AbstractProjectile fireEx()
	{
		WeaponChargeExBurst weaponChargeExBurst = base.fireEx() as WeaponChargeExBurst;
		Vector2 vector = 125f * MathUtils.AngleToDirection(weaponChargeExBurst.transform.eulerAngles.z);
		weaponChargeExBurst.transform.AddPosition(vector.x, vector.y, 0f);
		weaponChargeExBurst.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		weaponChargeExBurst.transform.SetScale(new float?((float)((!Rand.Bool()) ? 1 : -1)), null, null);
		weaponChargeExBurst.PlayerId = this.player.id;
		weaponChargeExBurst.Damage = WeaponProperties.LevelWeaponCharge.Ex.damage;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponChargeExBurst);
		return weaponChargeExBurst;
	}

	// Token: 0x0600387A RID: 14458 RVA: 0x0002E09E File Offset: 0x0002C29E
	public override void BeginBasic()
	{
		if (this.fullyCharged)
		{
			this.BeginBasicCheckAttenuation("player_weapon_charge_full_fireball", "player_weapon_charge_full_fireball_p2");
		}
		else
		{
			this.BeginBasicCheckAttenuation("player_weapon_charge_fire_small", "player_weapon_charge_fire_small_p2");
		}
		base.BeginBasic();
	}

	// Token: 0x0600387B RID: 14459 RVA: 0x0002E0D6 File Offset: 0x0002C2D6
	public override void EndBasic()
	{
		if (this.fullyCharged)
		{
			this.EndBasicCheckAttenuation("player_weapon_charge_full_fireball", "player_weapon_charge_full_fireball_p2");
		}
		else
		{
			this.EndBasicCheckAttenuation("player_weapon_charge_fire_small", "player_weapon_charge_fire_small_p2");
		}
		base.EndBasic();
	}

	// Token: 0x0600387C RID: 14460 RVA: 0x0002E10E File Offset: 0x0002C30E
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.chargeEffectPrefab = null;
		this.fullChargeFx = null;
	}

	// Token: 0x04002D63 RID: 11619
	[SerializeField]
	public WeaponChargeChargingEffect chargeEffectPrefab;

	// Token: 0x04002D64 RID: 11620
	[SerializeField]
	public Effect fullChargeFx;

	// Token: 0x04002D65 RID: 11621
	public WeaponChargeChargingEffect chargeEffect;

	// Token: 0x04002D66 RID: 11622
	public bool fullyCharged;

	// Token: 0x04002D67 RID: 11623
	public float timeCharged;

	// Token: 0x04002D68 RID: 11624
	public int damageState;

	// Token: 0x04002D69 RID: 11625
	public float damage;

	// Token: 0x04002D6A RID: 11626
	public bool AllowChargeSound = true;
}
