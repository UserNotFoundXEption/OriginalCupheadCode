using System;
using UnityEngine;

// Token: 0x02000538 RID: 1336
public class WeaponBoomerang : AbstractLevelWeapon
{
	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x0600384B RID: 14411 RVA: 0x0002DF44 File Offset: 0x0002C144
	public override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x0600384C RID: 14412 RVA: 0x0002DF47 File Offset: 0x0002C147
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponBoomerang.Basic.fireRate;
		}
	}

	// Token: 0x0600384D RID: 14413 RVA: 0x0010684C File Offset: 0x00104A4C
	public override void Awake()
	{
		base.Awake();
		string[] array = WeaponProperties.LevelWeaponBoomerang.Basic.xDistanceString.Split(new char[]
		{
			','
		});
		string[] array2 = WeaponProperties.LevelWeaponBoomerang.Basic.yDistanceString.Split(new char[]
		{
			','
		});
		this.distances = new Vector2[Mathf.Min(array.Length, array2.Length)];
		for (int i = 0; i < this.distances.Length; i++)
		{
			Parser.FloatTryParse(array[i], out this.distances[i].x);
			Parser.FloatTryParse(array2[i], out this.distances[i].y);
		}
		this.distanceIndex = Random.Range(0, this.distances.Length);
	}

	// Token: 0x0600384E RID: 14414 RVA: 0x0002DF4E File Offset: 0x0002C14E
	public override void BeginBasic()
	{
		this.BeginBasicCheckAttenuation("player_weapon_boomerang", "player_weapon_boomerang_p2");
		base.BeginBasic();
	}

	// Token: 0x0600384F RID: 14415 RVA: 0x00106904 File Offset: 0x00104B04
	public override AbstractProjectile fireBasic()
	{
		this.BasicSoundOneShot("player_weapon_boomerang", "player_weapon_boomerang_p2");
		WeaponBoomerangProjectile weaponBoomerangProjectile = base.fireBasic() as WeaponBoomerangProjectile;
		weaponBoomerangProjectile.Speed = WeaponProperties.LevelWeaponBoomerang.Basic.speed;
		weaponBoomerangProjectile.Damage = WeaponProperties.LevelWeaponBoomerang.Basic.damage;
		weaponBoomerangProjectile.PlayerId = this.player.id;
		weaponBoomerangProjectile.DamagesType.PlayerProjectileDefault();
		weaponBoomerangProjectile.CollisionDeath.PlayerProjectileDefault();
		weaponBoomerangProjectile.CollisionDeath.Other = false;
		weaponBoomerangProjectile.player = this.player;
		this.distanceIndex = (this.distanceIndex + 1) % this.distances.Length;
		weaponBoomerangProjectile.forwardDistance = this.distances[this.distanceIndex].x;
		weaponBoomerangProjectile.lateralDistance = this.distances[this.distanceIndex].y;
		return weaponBoomerangProjectile;
	}

	// Token: 0x06003850 RID: 14416 RVA: 0x0002DF66 File Offset: 0x0002C166
	public override void EndBasic()
	{
		base.EndBasic();
		this.EndBasicCheckAttenuation("player_weapon_boomerang", "player_weapon_boomerang_p2");
	}

	// Token: 0x06003851 RID: 14417 RVA: 0x001069D4 File Offset: 0x00104BD4
	public override AbstractProjectile fireEx()
	{
		WeaponBoomerangProjectile weaponBoomerangProjectile = base.fireEx() as WeaponBoomerangProjectile;
		weaponBoomerangProjectile.Speed = WeaponProperties.LevelWeaponBoomerang.Ex.speed;
		weaponBoomerangProjectile.Damage = WeaponProperties.LevelWeaponBoomerang.Ex.damage;
		weaponBoomerangProjectile.maxDamage = WeaponProperties.LevelWeaponBoomerang.Ex.maxDamage * PlayerManager.DamageMultiplier;
		weaponBoomerangProjectile.PlayerId = this.player.id;
		weaponBoomerangProjectile.hitFreezeTime = WeaponProperties.LevelWeaponBoomerang.Ex.hitFreezeTime;
		weaponBoomerangProjectile.DamageRate = WeaponProperties.LevelWeaponBoomerang.Ex.damageRate + weaponBoomerangProjectile.hitFreezeTime;
		weaponBoomerangProjectile.DamagesType.PlayerProjectileDefault();
		weaponBoomerangProjectile.forwardDistance = WeaponProperties.LevelWeaponBoomerang.Ex.xDistance;
		weaponBoomerangProjectile.lateralDistance = WeaponProperties.LevelWeaponBoomerang.Ex.yDistance;
		weaponBoomerangProjectile.player = this.player;
		weaponBoomerangProjectile.CollisionDeath.Other = false;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponBoomerangProjectile);
		return weaponBoomerangProjectile;
	}

	// Token: 0x04002D3D RID: 11581
	public int distanceIndex;

	// Token: 0x04002D3E RID: 11582
	public Vector2[] distances;
}
