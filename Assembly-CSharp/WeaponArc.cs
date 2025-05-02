using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000535 RID: 1333
public class WeaponArc : AbstractLevelWeapon
{
	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x06003832 RID: 14386 RVA: 0x0002DE45 File Offset: 0x0002C045
	public override bool rapidFire
	{
		get
		{
			return WeaponProperties.LevelWeaponArc.Basic.rapidFire;
		}
	}

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x06003833 RID: 14387 RVA: 0x0002DE4C File Offset: 0x0002C04C
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponArc.Basic.fireRate;
		}
	}

	// Token: 0x06003834 RID: 14388 RVA: 0x00106230 File Offset: 0x00104430
	public override AbstractProjectile fireBasic()
	{
		AudioManager.Play("player_weapon_arc");
		this.emitAudioFromObject.Add("player_weapon_arc");
		WeaponArcProjectile weaponArcProjectile = base.fireBasic() as WeaponArcProjectile;
		weaponArcProjectile.PlayerId = this.player.id;
		float num = weaponArcProjectile.transform.rotation.eulerAngles.z;
		if (num == 0f)
		{
			num += WeaponProperties.LevelWeaponArc.Basic.straightShotAngle;
			this.isDiagonal = false;
		}
		else if (num == 180f)
		{
			num -= WeaponProperties.LevelWeaponArc.Basic.straightShotAngle;
			this.isDiagonal = false;
		}
		else if (Mathf.Approximately(num, 45f) || Mathf.Approximately(num, 135f))
		{
			num += WeaponProperties.LevelWeaponArc.Basic.diagShotAngle;
			this.isDiagonal = true;
		}
		else
		{
			this.isDiagonal = false;
		}
		weaponArcProjectile.transform.SetEulerAngles(null, null, new float?(num));
		if (this.isDiagonal)
		{
			weaponArcProjectile.velocity = WeaponProperties.LevelWeaponArc.Basic.diagLaunchSpeed * MathUtils.AngleToDirection(weaponArcProjectile.transform.rotation.eulerAngles.z);
			weaponArcProjectile.gravity = WeaponProperties.LevelWeaponArc.Basic.diagGravity;
		}
		else
		{
			weaponArcProjectile.velocity = WeaponProperties.LevelWeaponArc.Basic.launchSpeed * MathUtils.AngleToDirection(weaponArcProjectile.transform.rotation.eulerAngles.z);
			weaponArcProjectile.gravity = WeaponProperties.LevelWeaponArc.Basic.gravity;
		}
		weaponArcProjectile.weapon = this;
		return weaponArcProjectile;
	}

	// Token: 0x06003835 RID: 14389 RVA: 0x001063C0 File Offset: 0x001045C0
	public override AbstractProjectile fireEx()
	{
		AudioManager.Play("player_weapon_peashot_ex");
		WeaponArcProjectile weaponArcProjectile = base.fireEx() as WeaponArcProjectile;
		weaponArcProjectile.velocity = WeaponProperties.LevelWeaponArc.Basic.launchSpeed * MathUtils.AngleToDirection(weaponArcProjectile.transform.rotation.eulerAngles.z);
		weaponArcProjectile.gravity = WeaponProperties.LevelWeaponArc.Basic.gravity;
		weaponArcProjectile.weapon = this;
		weaponArcProjectile.Damage = WeaponProperties.LevelWeaponArc.Ex.damage;
		weaponArcProjectile.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponArcProjectile);
		return weaponArcProjectile;
	}

	// Token: 0x04002D33 RID: 11571
	public List<WeaponArcProjectile> projectilesOnGround = new List<WeaponArcProjectile>();

	// Token: 0x04002D34 RID: 11572
	public bool isDiagonal;
}
