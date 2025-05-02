using System;

// Token: 0x02000503 RID: 1283
public class ArcadeWeaponPeashot : AbstractArcadeWeapon
{
	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x060035A9 RID: 13737 RVA: 0x0002C048 File Offset: 0x0002A248
	public override bool rapidFire
	{
		get
		{
			return WeaponProperties.ArcadeWeaponPeashot.Basic.rapidFire;
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x060035AA RID: 13738 RVA: 0x0002C04F File Offset: 0x0002A24F
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.ArcadeWeaponPeashot.Basic.rapidFireRate;
		}
	}

	// Token: 0x060035AB RID: 13739 RVA: 0x000FA944 File Offset: 0x000F8B44
	public override AbstractProjectile fireBasic()
	{
		if (this.p != null && !this.p.dead)
		{
			return null;
		}
		this.p = (base.fireBasic() as ArcadeWeaponBullet);
		this.p.Speed = WeaponProperties.ArcadeWeaponPeashot.Basic.speed;
		this.p.Damage = WeaponProperties.ArcadeWeaponPeashot.Basic.damage;
		this.p.PlayerId = this.player.id;
		this.p.DamagesType.PlayerProjectileDefault();
		this.p.CollisionDeath.PlayerProjectileDefault();
		return this.p;
	}

	// Token: 0x060035AC RID: 13740 RVA: 0x0002C056 File Offset: 0x0002A256
	public override AbstractProjectile fireEx()
	{
		return null;
	}

	// Token: 0x04002B96 RID: 11158
	public const float Y_POS = 20f;

	// Token: 0x04002B97 RID: 11159
	public const float ROTATION_OFFSET = 3f;

	// Token: 0x04002B98 RID: 11160
	public ArcadeWeaponBullet p;
}
