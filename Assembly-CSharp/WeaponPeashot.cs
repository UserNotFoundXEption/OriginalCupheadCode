using System;

// Token: 0x0200054B RID: 1355
public class WeaponPeashot : AbstractLevelWeapon
{
	// Token: 0x17000470 RID: 1136
	// (get) Token: 0x060038E9 RID: 14569 RVA: 0x0002E61A File Offset: 0x0002C81A
	public override bool rapidFire
	{
		get
		{
			return WeaponProperties.LevelWeaponPeashot.Basic.rapidFire;
		}
	}

	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x060038EA RID: 14570 RVA: 0x0002E621 File Offset: 0x0002C821
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponPeashot.Basic.rapidFireRate;
		}
	}

	// Token: 0x060038EB RID: 14571 RVA: 0x0010A090 File Offset: 0x00108290
	public override AbstractProjectile fireBasic()
	{
		BasicProjectile basicProjectile = base.fireBasic() as BasicProjectile;
		basicProjectile.Speed = WeaponProperties.LevelWeaponPeashot.Basic.speed;
		basicProjectile.Damage = WeaponProperties.LevelWeaponPeashot.Basic.damage;
		basicProjectile.PlayerId = this.player.id;
		basicProjectile.DamagesType.PlayerProjectileDefault();
		basicProjectile.CollisionDeath.PlayerProjectileDefault();
		float y = this.yPositions[this.currentY];
		this.currentY++;
		if (this.currentY >= this.yPositions.Length)
		{
			this.currentY = 0;
		}
		basicProjectile.transform.AddPosition(0f, y, 0f);
		return basicProjectile;
	}

	// Token: 0x060038EC RID: 14572 RVA: 0x0010A134 File Offset: 0x00108334
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

	// Token: 0x060038ED RID: 14573 RVA: 0x0002E628 File Offset: 0x0002C828
	public override void BeginBasic()
	{
		this.OneShotCooldown("player_default_fire_start");
		this.BasicSoundLoop("player_default_fire_loop", "player_default_fire_loop_p2");
		base.BeginBasic();
	}

	// Token: 0x060038EE RID: 14574 RVA: 0x0002E64B File Offset: 0x0002C84B
	public override void EndBasic()
	{
		this.ActivateCooldown();
		base.EndBasic();
		this.StopLoopSound("player_default_fire_loop", "player_default_fire_loop_p2");
	}

	// Token: 0x04002DC8 RID: 11720
	public const float Y_POS = 20f;

	// Token: 0x04002DC9 RID: 11721
	public const float ROTATION_OFFSET = 3f;

	// Token: 0x04002DCA RID: 11722
	public float[] yPositions = new float[]
	{
		0f,
		20f,
		40f,
		20f
	};

	// Token: 0x04002DCB RID: 11723
	public int currentY;
}
