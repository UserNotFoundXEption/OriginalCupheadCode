using System;

// Token: 0x02000533 RID: 1331
public class WeaponAccuracy : AbstractLevelWeapon
{
	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06003820 RID: 14368 RVA: 0x0002DC6E File Offset: 0x0002BE6E
	public override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06003821 RID: 14369 RVA: 0x0002DC71 File Offset: 0x0002BE71
	public override float rapidFireRate
	{
		get
		{
			return this.fireRate;
		}
	}

	// Token: 0x06003822 RID: 14370 RVA: 0x0002DC79 File Offset: 0x0002BE79
	public void Start()
	{
		this.level = WeaponAccuracy.Levels.One;
		this.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneSpeed;
		this.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneFireRate;
		this.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneSize;
		this.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneDamage;
	}

	// Token: 0x06003823 RID: 14371 RVA: 0x0010608C File Offset: 0x0010428C
	public override AbstractProjectile fireBasic()
	{
		WeaponAccuracyProjectile weaponAccuracyProjectile = base.fireBasic() as WeaponAccuracyProjectile;
		weaponAccuracyProjectile.Speed = this.speed;
		weaponAccuracyProjectile.PlayerId = this.player.id;
		weaponAccuracyProjectile.CollisionDeath.PlayerProjectileDefault();
		weaponAccuracyProjectile.EnemyDeath = new WeaponAccuracyProjectile.OnEnemyDeath(this.EnemyHit);
		weaponAccuracyProjectile.Damage = this.damage;
		weaponAccuracyProjectile.SetSize(this.size);
		return weaponAccuracyProjectile;
	}

	// Token: 0x06003824 RID: 14372 RVA: 0x001060F8 File Offset: 0x001042F8
	public override AbstractProjectile fireEx()
	{
		WeaponAccuracyProjectile weaponAccuracyProjectile = base.fireEx() as WeaponAccuracyProjectile;
		weaponAccuracyProjectile.Speed = WeaponProperties.LevelWeaponAccuracy.Ex.exSpeed;
		weaponAccuracyProjectile.Damage = WeaponProperties.LevelWeaponAccuracy.Ex.exDamage;
		weaponAccuracyProjectile.SetSize(WeaponProperties.LevelWeaponAccuracy.Ex.exShotSize);
		weaponAccuracyProjectile.CollisionDeath.PlayerProjectileDefault();
		weaponAccuracyProjectile.PlayerId = this.player.id;
		weaponAccuracyProjectile.EnemyDeath = new WeaponAccuracyProjectile.OnEnemyDeath(this.EXEnemyHit);
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponAccuracyProjectile);
		return weaponAccuracyProjectile;
	}

	// Token: 0x06003825 RID: 14373 RVA: 0x0002DCAE File Offset: 0x0002BEAE
	public void EXEnemyHit(bool exEnemyHit)
	{
		if (exEnemyHit)
		{
			this.shotCounter += WeaponProperties.LevelWeaponAccuracy.Ex.exShotEquivalent;
			this.CheckLevels();
		}
		else
		{
			this.shotCounter = 0;
			this.LevelOne();
		}
	}

	// Token: 0x06003826 RID: 14374 RVA: 0x0002DCE0 File Offset: 0x0002BEE0
	public void EnemyHit(bool enemyHit)
	{
		if (enemyHit)
		{
			this.shotCounter++;
			this.CheckLevels();
		}
		else
		{
			this.shotCounter = 0;
			this.LevelOne();
		}
	}

	// Token: 0x06003827 RID: 14375 RVA: 0x00106170 File Offset: 0x00104370
	public void CheckLevels()
	{
		switch (this.level)
		{
		case WeaponAccuracy.Levels.One:
			if (this.shotCounter >= WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoCounter)
			{
				this.LevelTwo();
			}
			break;
		case WeaponAccuracy.Levels.Two:
			if (this.shotCounter >= WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeCounter)
			{
				this.LevelThree();
			}
			break;
		case WeaponAccuracy.Levels.Three:
			if (this.shotCounter >= WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourCounter)
			{
				this.LevelFour();
			}
			break;
		case WeaponAccuracy.Levels.Four:
			break;
		default:
			this.LevelOne();
			break;
		}
	}

	// Token: 0x06003828 RID: 14376 RVA: 0x0002DD0E File Offset: 0x0002BF0E
	public void LevelOne()
	{
		this.level = WeaponAccuracy.Levels.One;
		this.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneSpeed;
		this.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneFireRate;
		this.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneSize;
		this.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneDamage;
	}

	// Token: 0x06003829 RID: 14377 RVA: 0x0002DD43 File Offset: 0x0002BF43
	public void LevelTwo()
	{
		this.level = WeaponAccuracy.Levels.Two;
		this.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoSpeed;
		this.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoFireRate;
		this.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoSize;
		this.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoDamage;
	}

	// Token: 0x0600382A RID: 14378 RVA: 0x0002DD78 File Offset: 0x0002BF78
	public void LevelThree()
	{
		this.level = WeaponAccuracy.Levels.Three;
		this.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeSpeed;
		this.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeFireRate;
		this.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeSize;
		this.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeDamage;
	}

	// Token: 0x0600382B RID: 14379 RVA: 0x0002DDAD File Offset: 0x0002BFAD
	public void LevelFour()
	{
		this.level = WeaponAccuracy.Levels.Four;
		this.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourSpeed;
		this.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourFireRate;
		this.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourSize;
		this.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourDamage;
	}

	// Token: 0x04002D2B RID: 11563
	public int shotCounter;

	// Token: 0x04002D2C RID: 11564
	public WeaponAccuracy.Levels level;

	// Token: 0x04002D2D RID: 11565
	public float speed;

	// Token: 0x04002D2E RID: 11566
	public float fireRate;

	// Token: 0x04002D2F RID: 11567
	public float size;

	// Token: 0x04002D30 RID: 11568
	public float damage;

	// Token: 0x020011BC RID: 4540
	public enum Levels
	{
		// Token: 0x04007BF8 RID: 31736
		One,
		// Token: 0x04007BF9 RID: 31737
		Two,
		// Token: 0x04007BFA RID: 31738
		Three,
		// Token: 0x04007BFB RID: 31739
		Four
	}
}
