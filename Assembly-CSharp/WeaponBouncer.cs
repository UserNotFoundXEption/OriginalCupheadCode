using System;
using UnityEngine;

// Token: 0x0200053A RID: 1338
public class WeaponBouncer : AbstractLevelWeapon
{
	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x0600385F RID: 14431 RVA: 0x0002DFD5 File Offset: 0x0002C1D5
	public override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x06003860 RID: 14432 RVA: 0x0002DFD8 File Offset: 0x0002C1D8
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponBouncer.Basic.fireRate;
		}
	}

	// Token: 0x06003861 RID: 14433 RVA: 0x00106F1C File Offset: 0x0010511C
	public override AbstractProjectile fireBasic()
	{
		this.BasicSoundOneShot("player_weapon_bouncer", "player_weapon_bouncer_p2");
		WeaponBouncerProjectile weaponBouncerProjectile = base.fireBasic() as WeaponBouncerProjectile;
		float adjustedAngle = this.getAdjustedAngle(weaponBouncerProjectile.transform.rotation.eulerAngles.z);
		weaponBouncerProjectile.transform.SetEulerAngles(null, null, new float?(0f));
		weaponBouncerProjectile.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		weaponBouncerProjectile.velocity = WeaponProperties.LevelWeaponBouncer.Basic.launchSpeed * MathUtils.AngleToDirection(adjustedAngle);
		weaponBouncerProjectile.gravity = WeaponProperties.LevelWeaponBouncer.Basic.gravity;
		weaponBouncerProjectile.bounceRatio = WeaponProperties.LevelWeaponBouncer.Basic.bounceRatio;
		weaponBouncerProjectile.bounceSpeedDampening = WeaponProperties.LevelWeaponBouncer.Basic.bounceSpeedDampening;
		weaponBouncerProjectile.Damage = WeaponProperties.LevelWeaponBouncer.Basic.damage;
		weaponBouncerProjectile.PlayerId = this.player.id;
		weaponBouncerProjectile.weapon = this;
		return weaponBouncerProjectile;
	}

	// Token: 0x06003862 RID: 14434 RVA: 0x00107014 File Offset: 0x00105214
	public float getAdjustedAngle(float angle)
	{
		int num = Mathf.RoundToInt(angle);
		if (num == 0)
		{
			angle += WeaponProperties.LevelWeaponBouncer.Basic.straightExtraAngle;
		}
		else if (num == 45)
		{
			angle += WeaponProperties.LevelWeaponBouncer.Basic.diagonalUpExtraAngle;
		}
		else if (num == 135)
		{
			angle -= WeaponProperties.LevelWeaponBouncer.Basic.diagonalUpExtraAngle;
		}
		else if (num == 180)
		{
			angle -= WeaponProperties.LevelWeaponBouncer.Basic.straightExtraAngle;
		}
		else if (num == 225)
		{
			angle -= WeaponProperties.LevelWeaponBouncer.Basic.diagonalDownExtraAngle;
		}
		else if (num == 315)
		{
			angle += WeaponProperties.LevelWeaponBouncer.Basic.diagonalDownExtraAngle;
		}
		return angle;
	}

	// Token: 0x06003863 RID: 14435 RVA: 0x001070B4 File Offset: 0x001052B4
	public override AbstractProjectile fireEx()
	{
		WeaponBouncerProjectile weaponBouncerProjectile = base.fireEx() as WeaponBouncerProjectile;
		float adjustedAngle = this.getAdjustedAngle(weaponBouncerProjectile.transform.rotation.eulerAngles.z);
		weaponBouncerProjectile.transform.SetEulerAngles(null, null, new float?(0f));
		weaponBouncerProjectile.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		weaponBouncerProjectile.velocity = WeaponProperties.LevelWeaponBouncer.Basic.launchSpeed * MathUtils.AngleToDirection(adjustedAngle);
		weaponBouncerProjectile.gravity = WeaponProperties.LevelWeaponBouncer.Basic.gravity;
		weaponBouncerProjectile.weapon = this;
		weaponBouncerProjectile.Damage = WeaponProperties.LevelWeaponBouncer.Ex.damage;
		weaponBouncerProjectile.PlayerId = this.player.id;
		return weaponBouncerProjectile;
	}

	// Token: 0x06003864 RID: 14436 RVA: 0x0002DFDF File Offset: 0x0002C1DF
	public override void BeginBasic()
	{
		this.BeginBasicCheckAttenuation("player_weapon_bouncer", "player_weapon_bouncer_p2");
		base.BeginBasic();
	}

	// Token: 0x06003865 RID: 14437 RVA: 0x0002DFF7 File Offset: 0x0002C1F7
	public override void EndBasic()
	{
		this.EndBasicCheckAttenuation("player_weapon_bouncer", "player_weapon_bouncer_p2");
		base.EndBasic();
	}
}
