using System;
using UnityEngine;

// Token: 0x02000546 RID: 1350
public class WeaponFirecracker : AbstractLevelWeapon
{
	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x060038BB RID: 14523 RVA: 0x0002E3CF File Offset: 0x0002C5CF
	public override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x060038BC RID: 14524 RVA: 0x0002E3D2 File Offset: 0x0002C5D2
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponFirecracker.Basic.fireRate;
		}
	}

	// Token: 0x060038BD RID: 14525 RVA: 0x0002E3D9 File Offset: 0x0002C5D9
	public void Start()
	{
		this.CreateDummyObject();
		this.explosionAngles = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionAngleString.Split(new char[]
		{
			','
		});
	}

	// Token: 0x060038BE RID: 14526 RVA: 0x00108F70 File Offset: 0x00107170
	public override AbstractProjectile fireBasic()
	{
		WeaponFirecrackerProjectile weaponFirecrackerProjectile = base.fireBasic() as WeaponFirecrackerProjectile;
		if (this.isTypeB)
		{
			weaponFirecrackerProjectile.explosionRadiusSize = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionsRadiusSize;
			float explosionAngle = 0f;
			Parser.FloatTryParse(this.explosionAngles[this.explosionAngleIndex], out explosionAngle);
			weaponFirecrackerProjectile.explosionAngle = explosionAngle;
			this.explosionAngleIndex = (this.explosionAngleIndex + 1) % this.explosionAngles.Length;
			weaponFirecrackerProjectile.Speed = WeaponProperties.LevelWeaponFirecrackerB.Basic.bulletSpeed;
			weaponFirecrackerProjectile.Damage = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionDamage;
			weaponFirecrackerProjectile.bulletLife = WeaponProperties.LevelWeaponFirecrackerB.Basic.bulletLife;
			weaponFirecrackerProjectile.explosionSize = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionSize;
			weaponFirecrackerProjectile.explosionDuration = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionDuration;
		}
		else
		{
			weaponFirecrackerProjectile.Speed = WeaponProperties.LevelWeaponFirecracker.Basic.bulletSpeed;
			weaponFirecrackerProjectile.Damage = WeaponProperties.LevelWeaponFirecracker.Basic.explosionDamage;
			weaponFirecrackerProjectile.bulletLife = WeaponProperties.LevelWeaponFirecracker.Basic.bulletLife;
			weaponFirecrackerProjectile.explosionSize = WeaponProperties.LevelWeaponFirecracker.Basic.explosionSize;
			weaponFirecrackerProjectile.explosionDuration = WeaponProperties.LevelWeaponFirecracker.Basic.explosionDuration;
		}
		weaponFirecrackerProjectile.collider.enabled = false;
		weaponFirecrackerProjectile.PlayerId = this.player.id;
		weaponFirecrackerProjectile.DamagesType.PlayerProjectileDefault();
		weaponFirecrackerProjectile.CollisionDeath.PlayerProjectileDefault();
		this.dummyObject.transform.eulerAngles = this.player.transform.eulerAngles;
		this.dummyObject.transform.localScale = this.player.transform.localScale;
		weaponFirecrackerProjectile.transform.parent = this.dummyObject.transform;
		weaponFirecrackerProjectile.SetupFirecracker(this.dummyObject.transform, this.player, this.isTypeB);
		return weaponFirecrackerProjectile;
	}

	// Token: 0x060038BF RID: 14527 RVA: 0x001090F4 File Offset: 0x001072F4
	public override AbstractProjectile fireEx()
	{
		WeaponFirecrackerEXProjectile weaponFirecrackerEXProjectile = base.fireEx() as WeaponFirecrackerEXProjectile;
		if (this.isTypeB)
		{
			weaponFirecrackerEXProjectile.Speed = WeaponProperties.LevelWeaponFirecrackerB.Ex.exSpeed;
			weaponFirecrackerEXProjectile.bulletLife = WeaponProperties.LevelWeaponFirecrackerB.Ex.exLife;
			weaponFirecrackerEXProjectile.explosionSize = WeaponProperties.LevelWeaponFirecrackerB.Ex.explosionRadius;
			weaponFirecrackerEXProjectile.DamageRate = WeaponProperties.LevelWeaponFirecrackerB.Ex.damageRate;
			weaponFirecrackerEXProjectile.Damage = WeaponProperties.LevelWeaponFirecrackerB.Ex.explosionDamage;
			weaponFirecrackerEXProjectile.explosionDuration = WeaponProperties.LevelWeaponFirecrackerB.Ex.explosionTime;
		}
		else
		{
			weaponFirecrackerEXProjectile.Speed = WeaponProperties.LevelWeaponFirecracker.Ex.exSpeed;
			weaponFirecrackerEXProjectile.bulletLife = WeaponProperties.LevelWeaponFirecracker.Ex.exLife;
			weaponFirecrackerEXProjectile.explosionSize = WeaponProperties.LevelWeaponFirecracker.Ex.explosionRadius;
			weaponFirecrackerEXProjectile.DamageRate = WeaponProperties.LevelWeaponFirecracker.Ex.damageRate;
			weaponFirecrackerEXProjectile.Damage = WeaponProperties.LevelWeaponFirecracker.Ex.explosionDamage;
			weaponFirecrackerEXProjectile.explosionDuration = WeaponProperties.LevelWeaponFirecracker.Ex.explosionTime;
		}
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponFirecrackerEXProjectile);
		return weaponFirecrackerEXProjectile;
	}

	// Token: 0x060038C0 RID: 14528 RVA: 0x0002E3FC File Offset: 0x0002C5FC
	public new void Update()
	{
		this.dummyObject.transform.position = this.player.transform.position;
	}

	// Token: 0x060038C1 RID: 14529 RVA: 0x0002E41E File Offset: 0x0002C61E
	public void CreateDummyObject()
	{
		this.dummyObject = new GameObject();
		this.dummyObject.name = "FirecrackerDummyObj";
	}

	// Token: 0x04002D93 RID: 11667
	public bool isTypeB;

	// Token: 0x04002D94 RID: 11668
	public GameObject dummyObject;

	// Token: 0x04002D95 RID: 11669
	public string[] explosionAngles;

	// Token: 0x04002D96 RID: 11670
	public int explosionAngleIndex;
}
