using System;

// Token: 0x0200054F RID: 1359
public class WeaponSplitter : AbstractLevelWeapon
{
	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x060038FD RID: 14589 RVA: 0x0002E6CB File Offset: 0x0002C8CB
	public override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x060038FE RID: 14590 RVA: 0x0002E6CE File Offset: 0x0002C8CE
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponSplitter.Basic.fireRate;
		}
	}

	// Token: 0x060038FF RID: 14591 RVA: 0x0010A504 File Offset: 0x00108704
	public override AbstractProjectile fireBasic()
	{
		WeaponSplitterProjectile weaponSplitterProjectile = base.fireBasic() as WeaponSplitterProjectile;
		weaponSplitterProjectile.Speed = WeaponProperties.LevelWeaponSplitter.Basic.speed;
		weaponSplitterProjectile.Damage = WeaponProperties.LevelWeaponSplitter.Basic.bulletDamage;
		weaponSplitterProjectile.isMain = true;
		weaponSplitterProjectile.nextDistance = WeaponProperties.LevelWeaponSplitter.Basic.splitDistanceA;
		weaponSplitterProjectile.PlayerId = this.player.id;
		weaponSplitterProjectile.DamagesType.PlayerProjectileDefault();
		weaponSplitterProjectile.CollisionDeath.PlayerProjectileDefault();
		return weaponSplitterProjectile;
	}

	// Token: 0x06003900 RID: 14592 RVA: 0x0010A570 File Offset: 0x00108770
	public override AbstractProjectile fireEx()
	{
		WeaponPeashotExProjectile weaponPeashotExProjectile = base.fireEx() as WeaponPeashotExProjectile;
		weaponPeashotExProjectile.moveSpeed = WeaponProperties.LevelWeaponPeashot.Ex.speed;
		weaponPeashotExProjectile.Damage = WeaponProperties.LevelWeaponPeashot.Ex.damage;
		weaponPeashotExProjectile.hitFreezeTime = WeaponProperties.LevelWeaponPeashot.Ex.freezeTime;
		weaponPeashotExProjectile.DamageRate = weaponPeashotExProjectile.hitFreezeTime + WeaponProperties.LevelWeaponPeashot.Ex.damageDistance / weaponPeashotExProjectile.moveSpeed;
		weaponPeashotExProjectile.maxDamage = WeaponProperties.LevelWeaponPeashot.Ex.maxDamage;
		weaponPeashotExProjectile.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponPeashotExProjectile);
		return weaponPeashotExProjectile;
	}

	// Token: 0x06003901 RID: 14593 RVA: 0x0002E6D5 File Offset: 0x0002C8D5
	public override void BeginBasic()
	{
		this.OneShotCooldown("player_default_fire_start");
		this.BasicSoundLoop("player_default_fire_loop", "player_default_fire_loop_p2");
		base.BeginBasic();
	}

	// Token: 0x06003902 RID: 14594 RVA: 0x0002E6F8 File Offset: 0x0002C8F8
	public override void EndBasic()
	{
		this.ActivateCooldown();
		base.EndBasic();
		this.StopLoopSound("player_default_fire_loop", "player_default_fire_loop_p2");
	}

	// Token: 0x04002DE3 RID: 11747
	public const float ROTATION_OFFSET = 3f;
}
