using System;
using UnityEngine;

// Token: 0x0200056F RID: 1391
public class PlaneWeaponChaliceBomb : AbstractPlaneWeapon
{
	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06003A79 RID: 14969 RVA: 0x0002F996 File Offset: 0x0002DB96
	public override bool rapidFire
	{
		get
		{
			return WeaponProperties.PlaneWeaponChaliceBomb.Basic.rapidFire;
		}
	}

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06003A7A RID: 14970 RVA: 0x0002F99D File Offset: 0x0002DB9D
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.PlaneWeaponChaliceBomb.Basic.rapidFireRate;
		}
	}

	// Token: 0x06003A7B RID: 14971 RVA: 0x0010F908 File Offset: 0x0010DB08
	public override AbstractProjectile fireBasic()
	{
		PlaneWeaponChaliceBombProjectile planeWeaponChaliceBombProjectile = base.fireBasic() as PlaneWeaponChaliceBombProjectile;
		planeWeaponChaliceBombProjectile.transform.Rotate(new Vector3(0f, 0f, Random.Range(-WeaponProperties.PlaneWeaponChaliceBomb.Basic.angleRange, WeaponProperties.PlaneWeaponChaliceBomb.Basic.angleRange)));
		planeWeaponChaliceBombProjectile.velocity = WeaponProperties.PlaneWeaponChaliceBomb.Basic.speed * MathUtils.AngleToDirection(planeWeaponChaliceBombProjectile.transform.rotation.eulerAngles.z);
		planeWeaponChaliceBombProjectile.gravity = WeaponProperties.PlaneWeaponChaliceBomb.Basic.gravity;
		planeWeaponChaliceBombProjectile.Damage = WeaponProperties.PlaneWeaponChaliceBomb.Basic.damage;
		planeWeaponChaliceBombProjectile.size = WeaponProperties.PlaneWeaponChaliceBomb.Basic.size;
		planeWeaponChaliceBombProjectile.damageExplosion = WeaponProperties.PlaneWeaponChaliceBomb.Basic.damageExplosion;
		planeWeaponChaliceBombProjectile.explosionSize = WeaponProperties.PlaneWeaponChaliceBomb.Basic.sizeExplosion;
		planeWeaponChaliceBombProjectile.PlayerId = this.player.id;
		planeWeaponChaliceBombProjectile.SetAnimation(this.isA);
		this.isA = !this.isA;
		return planeWeaponChaliceBombProjectile;
	}

	// Token: 0x06003A7C RID: 14972 RVA: 0x0010F9E0 File Offset: 0x0010DBE0
	public override AbstractProjectile fireEx()
	{
		PlaneWeaponChaliceBombExProjectile planeWeaponChaliceBombExProjectile = base.fireEx() as PlaneWeaponChaliceBombExProjectile;
		planeWeaponChaliceBombExProjectile.FreezeTime = WeaponProperties.PlaneWeaponChaliceBomb.Ex.freezeTime;
		planeWeaponChaliceBombExProjectile.Damage = WeaponProperties.PlaneWeaponChaliceBomb.Ex.damage;
		planeWeaponChaliceBombExProjectile.DamageRate = WeaponProperties.PlaneWeaponChaliceBomb.Ex.damageRate;
		planeWeaponChaliceBombExProjectile.DamageRateIncrease = WeaponProperties.PlaneWeaponChaliceBomb.Ex.damageRateIncrease;
		planeWeaponChaliceBombExProjectile.Gravity = WeaponProperties.PlaneWeaponChaliceBomb.Ex.gravity;
		planeWeaponChaliceBombExProjectile.Velocity = WeaponProperties.PlaneWeaponChaliceBomb.Ex.startSpeed * Vector3.right;
		planeWeaponChaliceBombExProjectile.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(planeWeaponChaliceBombExProjectile);
		return planeWeaponChaliceBombExProjectile;
	}

	// Token: 0x04002ECF RID: 11983
	public bool isA;
}
