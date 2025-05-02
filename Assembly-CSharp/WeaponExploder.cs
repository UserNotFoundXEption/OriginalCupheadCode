using System;
using System.Collections.Generic;

// Token: 0x02000543 RID: 1347
public class WeaponExploder : AbstractLevelWeapon
{
	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x060038A8 RID: 14504 RVA: 0x0002E2EF File Offset: 0x0002C4EF
	public override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x060038A9 RID: 14505 RVA: 0x0002E2F2 File Offset: 0x0002C4F2
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponExploder.Basic.fireRate;
		}
	}

	// Token: 0x060038AA RID: 14506 RVA: 0x00108C24 File Offset: 0x00106E24
	public override AbstractProjectile fireBasic()
	{
		WeaponExploderProjectile weaponExploderProjectile = base.fireBasic() as WeaponExploderProjectile;
		weaponExploderProjectile.Speed = WeaponProperties.LevelWeaponExploder.Basic.speed;
		weaponExploderProjectile.PlayerId = this.player.id;
		weaponExploderProjectile.DamagesType.SetAll(false);
		weaponExploderProjectile.CollisionDeath.PlayerProjectileDefault();
		weaponExploderProjectile.weapon = this;
		weaponExploderProjectile.minMaxSpeed = WeaponProperties.LevelWeaponExploder.Basic.easeSpeed;
		weaponExploderProjectile.easeTime = WeaponProperties.LevelWeaponExploder.Basic.easeTime;
		if (WeaponProperties.LevelWeaponExploder.Basic.easing)
		{
			weaponExploderProjectile.EaseSpeed();
		}
		return weaponExploderProjectile;
	}

	// Token: 0x060038AB RID: 14507 RVA: 0x00108CA0 File Offset: 0x00106EA0
	public override AbstractProjectile fireEx()
	{
		WeaponExploderProjectile weaponExploderProjectile = base.fireEx() as WeaponExploderProjectile;
		weaponExploderProjectile.Speed = WeaponProperties.LevelWeaponExploder.Ex.speed;
		weaponExploderProjectile.Damage = WeaponProperties.LevelWeaponExploder.Ex.damage;
		weaponExploderProjectile.explodeRadius = WeaponProperties.LevelWeaponExploder.Ex.explodeRadius;
		weaponExploderProjectile.PlayerId = this.player.id;
		weaponExploderProjectile.DamagesType.SetAll(false);
		weaponExploderProjectile.CollisionDeath.PlayerProjectileDefault();
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponExploderProjectile);
		return weaponExploderProjectile;
	}

	// Token: 0x04002D87 RID: 11655
	public List<WeaponArcProjectile> projectilesOnGround = new List<WeaponArcProjectile>();
}
