using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200056B RID: 1387
public class PlaneWeaponBomb : AbstractPlaneWeapon
{
	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06003A5A RID: 14938 RVA: 0x0002F7F9 File Offset: 0x0002D9F9
	public override bool rapidFire
	{
		get
		{
			return WeaponProperties.PlaneWeaponBomb.Basic.rapidFire;
		}
	}

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06003A5B RID: 14939 RVA: 0x0002F800 File Offset: 0x0002DA00
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.PlaneWeaponBomb.Basic.rapidFireRate;
		}
	}

	// Token: 0x06003A5C RID: 14940 RVA: 0x0010F0A0 File Offset: 0x0010D2A0
	public override AbstractProjectile fireBasic()
	{
		PlaneWeaponBombProjectile planeWeaponBombProjectile = base.fireBasic() as PlaneWeaponBombProjectile;
		planeWeaponBombProjectile.shootsUp = false;
		planeWeaponBombProjectile.velocity = WeaponProperties.PlaneWeaponBomb.Basic.speed * MathUtils.AngleToDirection(planeWeaponBombProjectile.transform.rotation.eulerAngles.z);
		planeWeaponBombProjectile.gravity = WeaponProperties.PlaneWeaponBomb.Basic.gravity;
		planeWeaponBombProjectile.Damage = WeaponProperties.PlaneWeaponBomb.Basic.damage;
		planeWeaponBombProjectile.PlayerId = this.player.id;
		planeWeaponBombProjectile.bulletSize = WeaponProperties.PlaneWeaponBomb.Basic.size;
		planeWeaponBombProjectile.explosionSize = WeaponProperties.PlaneWeaponBomb.Basic.sizeExplosion;
		planeWeaponBombProjectile.SetAnimation(this.player.id);
		if (WeaponProperties.PlaneWeaponBomb.Basic.Up)
		{
			PlaneWeaponBombProjectile planeWeaponBombProjectile2 = base.fireBasic() as PlaneWeaponBombProjectile;
			planeWeaponBombProjectile2.shootsUp = true;
			planeWeaponBombProjectile2.velocity = WeaponProperties.PlaneWeaponBomb.Basic.speed * MathUtils.AngleToDirection(planeWeaponBombProjectile.transform.rotation.eulerAngles.z);
			planeWeaponBombProjectile2.gravity = WeaponProperties.PlaneWeaponBomb.Basic.gravity;
			planeWeaponBombProjectile2.Damage = WeaponProperties.PlaneWeaponBomb.Basic.damage;
			planeWeaponBombProjectile2.PlayerId = this.player.id;
			planeWeaponBombProjectile2.bulletSize = WeaponProperties.PlaneWeaponBomb.Basic.size;
			planeWeaponBombProjectile2.explosionSize = WeaponProperties.PlaneWeaponBomb.Basic.sizeExplosion;
			planeWeaponBombProjectile2.SetAnimation(this.player.id);
		}
		return planeWeaponBombProjectile;
	}

	// Token: 0x06003A5D RID: 14941 RVA: 0x0002F807 File Offset: 0x0002DA07
	public override AbstractProjectile fireEx()
	{
		base.StartCoroutine(this.ex_cr());
		return null;
	}

	// Token: 0x06003A5E RID: 14942 RVA: 0x0010F1DC File Offset: 0x0010D3DC
	public IEnumerator ex_cr()
	{
		for (int wave = 0; wave < WeaponProperties.PlaneWeaponBomb.Ex.counts.Length; wave++)
		{
			int count = WeaponProperties.PlaneWeaponBomb.Ex.counts[wave];
			float angle = WeaponProperties.PlaneWeaponBomb.Ex.angles[wave];
			MeterScoreTracker tracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
			for (int i = 0; i < count; i++)
			{
				float num = Mathf.Lerp(0f, angle, (float)i / (float)count) - 90f;
				PlaneWeaponBombExProjectile planeWeaponBombExProjectile = this.<fireEx>__BaseCallProxy0() as PlaneWeaponBombExProjectile;
				planeWeaponBombExProjectile.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(num));
				planeWeaponBombExProjectile.rotation = num;
				planeWeaponBombExProjectile.speed = WeaponProperties.PlaneWeaponBomb.Ex.speed;
				planeWeaponBombExProjectile.Damage = WeaponProperties.PlaneWeaponBomb.Ex.damage;
				planeWeaponBombExProjectile.PlayerId = this.player.id;
				planeWeaponBombExProjectile.rotationSpeed = WeaponProperties.PlaneWeaponBomb.Ex.rotationSpeed;
				planeWeaponBombExProjectile.rotationSpeedEaseTime = WeaponProperties.PlaneWeaponBomb.Ex.rotationSpeedEaseTime;
				planeWeaponBombExProjectile.timeBeforeEaseRotationSpeed = WeaponProperties.PlaneWeaponBomb.Ex.timeBeforeEaseRotationSpeed;
				tracker.Add(planeWeaponBombExProjectile);
				planeWeaponBombExProjectile.Init();
				planeWeaponBombExProjectile.FindTarget();
			}
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}
}
