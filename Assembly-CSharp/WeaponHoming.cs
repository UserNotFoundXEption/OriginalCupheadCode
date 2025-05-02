using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000549 RID: 1353
public class WeaponHoming : AbstractLevelWeapon
{
	// Token: 0x1700046D RID: 1133
	// (get) Token: 0x060038D0 RID: 14544 RVA: 0x0002E4D1 File Offset: 0x0002C6D1
	public override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x060038D1 RID: 14545 RVA: 0x0002E4D4 File Offset: 0x0002C6D4
	public override float rapidFireRate
	{
		get
		{
			return this.fireRate;
		}
	}

	// Token: 0x060038D2 RID: 14546 RVA: 0x00109380 File Offset: 0x00107580
	public void FixedUpdate()
	{
		if (this.player.motor.Locked)
		{
			this.fireRateLockRatio = Mathf.Clamp01(this.fireRateLockRatio + CupheadTime.FixedDelta / WeaponProperties.LevelWeaponHoming.Basic.lockedShotAccelerationTime);
		}
		else
		{
			this.fireRateLockRatio = 0f;
		}
		this.fireRate = WeaponProperties.LevelWeaponHoming.Basic.fireRate.GetFloatAt(1f - this.fireRateLockRatio);
	}

	// Token: 0x060038D3 RID: 14547 RVA: 0x001093EC File Offset: 0x001075EC
	public override AbstractProjectile fireBasic()
	{
		WeaponHomingProjectile weaponHomingProjectile = base.fireBasic() as WeaponHomingProjectile;
		weaponHomingProjectile.rotation = weaponHomingProjectile.transform.rotation.eulerAngles.z + Random.Range(-WeaponProperties.LevelWeaponHoming.Basic.angleVariation, WeaponProperties.LevelWeaponHoming.Basic.angleVariation);
		weaponHomingProjectile.speed = WeaponProperties.LevelWeaponHoming.Basic.speed + Random.Range(-WeaponProperties.LevelWeaponHoming.Basic.speedVariation, WeaponProperties.LevelWeaponHoming.Basic.speedVariation);
		weaponHomingProjectile.rotationSpeed = WeaponProperties.LevelWeaponHoming.Basic.rotationSpeed;
		weaponHomingProjectile.rotationSpeedEaseTime = WeaponProperties.LevelWeaponHoming.Basic.rotationSpeedEaseTime;
		weaponHomingProjectile.timeBeforeEaseRotationSpeed = WeaponProperties.LevelWeaponHoming.Basic.timeBeforeEaseRotationSpeed;
		weaponHomingProjectile.Damage = WeaponProperties.LevelWeaponHoming.Basic.damage;
		weaponHomingProjectile.PlayerId = this.player.id;
		weaponHomingProjectile.DamagesType.PlayerProjectileDefault();
		weaponHomingProjectile.CollisionDeath.PlayerProjectileDefault();
		weaponHomingProjectile.trailFollowFrames = Mathf.Clamp(WeaponProperties.LevelWeaponHoming.Basic.trailFrameDelay, 1, 10);
		if (Random.Range(0, 4) == 0)
		{
			weaponHomingProjectile.transform.SetScale(new float?(0.8f), new float?(0.8f), null);
		}
		if (MathUtils.RandomBool())
		{
			weaponHomingProjectile.transform.SetScale(new float?(-weaponHomingProjectile.transform.localScale.x), null, null);
		}
		weaponHomingProjectile.FindTarget();
		return weaponHomingProjectile;
	}

	// Token: 0x060038D4 RID: 14548 RVA: 0x00109538 File Offset: 0x00107738
	public override AbstractProjectile fireEx()
	{
		foreach (WeaponHomingProjectile weaponHomingProjectile in this.swirlingProjectiles)
		{
			if (weaponHomingProjectile != null)
			{
				weaponHomingProjectile.StopSwirling();
			}
		}
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		this.swirlingProjectiles.Clear();
		for (int i = 0; i < WeaponProperties.LevelWeaponHoming.Ex.bulletCount; i++)
		{
			WeaponHomingProjectile weaponHomingProjectile2 = base.fireEx() as WeaponHomingProjectile;
			weaponHomingProjectile2.speed = WeaponProperties.LevelWeaponHoming.Ex.speed;
			weaponHomingProjectile2.rotationSpeed = WeaponProperties.LevelWeaponHoming.Basic.rotationSpeed;
			weaponHomingProjectile2.rotationSpeedEaseTime = WeaponProperties.LevelWeaponHoming.Basic.rotationSpeedEaseTime;
			weaponHomingProjectile2.timeBeforeEaseRotationSpeed = WeaponProperties.LevelWeaponHoming.Basic.timeBeforeEaseRotationSpeed;
			weaponHomingProjectile2.Damage = WeaponProperties.LevelWeaponHoming.Ex.damage;
			weaponHomingProjectile2.PlayerId = this.player.id;
			weaponHomingProjectile2.DamagesType.PlayerProjectileDefault();
			weaponHomingProjectile2.CollisionDeath.PlayerProjectileDefault();
			weaponHomingProjectile2.CollisionDeath.SetBounds(false);
			weaponHomingProjectile2.swirlDistance = WeaponProperties.LevelWeaponHoming.Ex.swirlDistance;
			weaponHomingProjectile2.swirlEaseTime = WeaponProperties.LevelWeaponHoming.Ex.swirlEaseTime;
			weaponHomingProjectile2.trailFollowFrames = Mathf.Clamp(WeaponProperties.LevelWeaponHoming.Ex.trailFrameDelay, 1, 10);
			weaponHomingProjectile2.StartSwirling(i, WeaponProperties.LevelWeaponHoming.Ex.bulletCount, WeaponProperties.LevelWeaponHoming.Ex.spread, this.player);
			weaponHomingProjectile2.isEx = true;
			this.swirlingProjectiles.Add(weaponHomingProjectile2);
			meterScoreTracker.Add(weaponHomingProjectile2);
		}
		return this.swirlingProjectiles[0];
	}

	// Token: 0x060038D5 RID: 14549 RVA: 0x0002E4DC File Offset: 0x0002C6DC
	public override void BeginBasic()
	{
		base.BeginBasic();
		AudioManager.Play("player_weapon_homing_fire_start");
		this.emitAudioFromObject.Add("player_weapon_homing_fire_start");
		this.BasicSoundLoop("player_weapon_homing_loop", "player_weapon_homing_loop_p2");
	}

	// Token: 0x060038D6 RID: 14550 RVA: 0x0002E50E File Offset: 0x0002C70E
	public override void EndBasic()
	{
		base.EndBasic();
		this.StopLoopSound("player_weapon_homing_loop", "player_weapon_homing_loop_p2");
	}

	// Token: 0x04002DA9 RID: 11689
	public float fireRate = 1f;

	// Token: 0x04002DAA RID: 11690
	public float fireRateLockRatio;

	// Token: 0x04002DAB RID: 11691
	public static Transform target;

	// Token: 0x04002DAC RID: 11692
	public List<WeaponHomingProjectile> swirlingProjectiles = new List<WeaponHomingProjectile>();
}
