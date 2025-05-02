using System;

// Token: 0x02000504 RID: 1284
public class ArcadeWeaponRocketPeashot : AbstractArcadeWeapon
{
	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x060035AE RID: 13742 RVA: 0x0002C061 File Offset: 0x0002A261
	public override bool rapidFire
	{
		get
		{
			return WeaponProperties.ArcadeWeaponRocketPeashot.Basic.rapidFire;
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x060035AF RID: 13743 RVA: 0x0002C068 File Offset: 0x0002A268
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.ArcadeWeaponRocketPeashot.Basic.rapidFireRate;
		}
	}

	// Token: 0x060035B0 RID: 13744 RVA: 0x000FA9E4 File Offset: 0x000F8BE4
	public override AbstractProjectile fireBasic()
	{
		if (this.p != null && !this.p.dead)
		{
			return null;
		}
		this.p = (base.fireBasic() as ArcadeWeaponBullet);
		this.p.Speed = WeaponProperties.ArcadeWeaponRocketPeashot.Basic.speed;
		this.p.Damage = WeaponProperties.ArcadeWeaponRocketPeashot.Basic.damage;
		this.p.PlayerId = this.player.id;
		this.p.DamagesType.PlayerProjectileDefault();
		this.p.CollisionDeath.PlayerProjectileDefault();
		return this.p;
	}

	// Token: 0x04002B99 RID: 11161
	public ArcadeWeaponBullet p;
}
