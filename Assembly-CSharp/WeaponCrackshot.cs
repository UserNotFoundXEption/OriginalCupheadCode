using System;
using UnityEngine;

// Token: 0x0200053F RID: 1343
public class WeaponCrackshot : AbstractLevelWeapon
{
	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x06003884 RID: 14468 RVA: 0x0002E1A0 File Offset: 0x0002C3A0
	public override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x06003885 RID: 14469 RVA: 0x0002E1A3 File Offset: 0x0002C3A3
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponCrackshot.Basic.fireRate;
		}
	}

	// Token: 0x06003886 RID: 14470 RVA: 0x00107B08 File Offset: 0x00105D08
	public override AbstractProjectile fireBasic()
	{
		WeaponCrackshotProjectile weaponCrackshotProjectile = base.fireBasic() as WeaponCrackshotProjectile;
		weaponCrackshotProjectile.Speed = WeaponProperties.LevelWeaponCrackshot.Basic.initialSpeed;
		weaponCrackshotProjectile.Damage = WeaponProperties.LevelWeaponCrackshot.Basic.initialDamage;
		weaponCrackshotProjectile.PlayerId = this.player.id;
		weaponCrackshotProjectile.DamagesType.PlayerProjectileDefault();
		weaponCrackshotProjectile.CollisionDeath.PlayerProjectileDefault();
		weaponCrackshotProjectile.maxAngleRange = ((!WeaponProperties.LevelWeaponCrackshot.Basic.enableMaxAngle) ? 180f : WeaponProperties.LevelWeaponCrackshot.Basic.maxAngle);
		weaponCrackshotProjectile.variant = this.variantString.PopInt();
		weaponCrackshotProjectile.useBComet = this.useBComet;
		this.useBComet = !this.useBComet;
		float y = this.yPositions[this.currentY];
		this.currentY++;
		if (this.currentY >= this.yPositions.Length)
		{
			this.currentY = 0;
		}
		weaponCrackshotProjectile.transform.AddPosition(0f, y, 0f);
		return weaponCrackshotProjectile;
	}

	// Token: 0x06003887 RID: 14471 RVA: 0x00107BF8 File Offset: 0x00105DF8
	public override AbstractProjectile fireEx()
	{
		if (this.activeEX)
		{
			this.activeEX.GetReplaced();
		}
		WeaponCrackshotExProjectile weaponCrackshotExProjectile = base.fireEx() as WeaponCrackshotExProjectile;
		weaponCrackshotExProjectile.Damage = WeaponProperties.LevelWeaponCrackshot.Ex.collideDamage;
		weaponCrackshotExProjectile.DamageRate = 0f;
		weaponCrackshotExProjectile.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponCrackshotExProjectile);
		this.activeEX = weaponCrackshotExProjectile;
		return weaponCrackshotExProjectile;
	}

	// Token: 0x06003888 RID: 14472 RVA: 0x0002E1AA File Offset: 0x0002C3AA
	public override void BeginBasic()
	{
		AudioManager.Play("player_weapon_crackshot_shoot_start");
		this.emitAudioFromObject.Add("player_weapon_crackshot_shoot_start");
		this.variantString = new PatternString("0,1,0,2,1,2,0,1,2", true);
		base.BeginBasic();
	}

	// Token: 0x06003889 RID: 14473 RVA: 0x0002E1DD File Offset: 0x0002C3DD
	public override void EndBasic()
	{
		this.ActivateCooldown();
		base.EndBasic();
	}

	// Token: 0x04002D6D RID: 11629
	public const float Y_POS = 20f;

	// Token: 0x04002D6E RID: 11630
	public const float ROTATION_OFFSET = 3f;

	// Token: 0x04002D6F RID: 11631
	public float[] yPositions = new float[]
	{
		0f,
		20f,
		40f,
		20f
	};

	// Token: 0x04002D70 RID: 11632
	public int currentY;

	// Token: 0x04002D71 RID: 11633
	[SerializeField]
	public PatternString variantString;

	// Token: 0x04002D72 RID: 11634
	public bool useBComet;

	// Token: 0x04002D73 RID: 11635
	public WeaponCrackshotExProjectile activeEX;
}
