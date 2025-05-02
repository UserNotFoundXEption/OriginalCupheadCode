using System;
using UnityEngine;

// Token: 0x02000555 RID: 1365
public class WeaponUpshot : AbstractLevelWeapon
{
	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x06003920 RID: 14624 RVA: 0x0002E877 File Offset: 0x0002CA77
	public override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x06003921 RID: 14625 RVA: 0x0002E87A File Offset: 0x0002CA7A
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponUpshot.Basic.fireRate;
		}
	}

	// Token: 0x06003922 RID: 14626 RVA: 0x0010AB24 File Offset: 0x00108D24
	public override AbstractProjectile fireBasic()
	{
		this.animationCycleCount++;
		for (int i = 0; i < 3; i++)
		{
			WeaponUpshotProjectile weaponUpshotProjectile = (i != 0) ? (base.fireBasicNoEffect() as WeaponUpshotProjectile) : (base.fireBasic() as WeaponUpshotProjectile);
			if (i == 1)
			{
				weaponUpshotProjectile.GetComponent<SpriteRenderer>().sortingOrder = 1;
			}
			weaponUpshotProjectile.Damage = WeaponProperties.LevelWeaponUpshot.Basic.damage;
			weaponUpshotProjectile.PlayerId = this.player.id;
			weaponUpshotProjectile.DamagesType.PlayerProjectileDefault();
			weaponUpshotProjectile.CollisionDeath.PlayerProjectileDefault();
			weaponUpshotProjectile.CollisionDeath.Other = false;
			weaponUpshotProjectile.xSpeed = WeaponProperties.LevelWeaponUpshot.Basic.xSpeed[i];
			weaponUpshotProjectile.ySpeedMinMax = WeaponProperties.LevelWeaponUpshot.Basic.ySpeed[i];
			weaponUpshotProjectile.timeToArc = WeaponProperties.LevelWeaponUpshot.Basic.timeToMaxSpeed[i];
			weaponUpshotProjectile.animator.Play(((this.animationCycleCount + i) % 3).ToString(), 0, Random.Range(0f, 1f));
		}
		return null;
	}

	// Token: 0x06003923 RID: 14627 RVA: 0x0010AC20 File Offset: 0x00108E20
	public override AbstractProjectile fireEx()
	{
		WeaponUpshotExProjectile weaponUpshotExProjectile = base.fireEx() as WeaponUpshotExProjectile;
		weaponUpshotExProjectile.Damage = WeaponProperties.LevelWeaponUpshot.Ex.damage;
		weaponUpshotExProjectile.DamageRate = WeaponProperties.LevelWeaponUpshot.Ex.damageRate;
		weaponUpshotExProjectile.PlayerId = this.player.id;
		weaponUpshotExProjectile.rotateDir = Mathf.Sign(this.player.gameObject.transform.localScale.x);
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponUpshotExProjectile);
		return weaponUpshotExProjectile;
	}

	// Token: 0x06003924 RID: 14628 RVA: 0x0002E881 File Offset: 0x0002CA81
	public override void BeginBasic()
	{
		base.BeginBasic();
		AudioManager.Play("player_weapon_upshot_start");
		this.emitAudioFromObject.Add("player_weapon_upshot_start");
		this.BasicSoundLoop("player_weapon_upshot_loop_p1", "player_weapon_upshot_loop_p2");
	}

	// Token: 0x06003925 RID: 14629 RVA: 0x0002E8B3 File Offset: 0x0002CAB3
	public override void EndBasic()
	{
		base.EndBasic();
		this.StopLoopSound("player_weapon_upshot_loop_p1", "player_weapon_upshot_loop_p2");
	}

	// Token: 0x04002DEE RID: 11758
	public const int NUM_OF_BULLETS = 3;

	// Token: 0x04002DEF RID: 11759
	public int[] xOffset = new int[]
	{
		-1,
		1,
		0,
		1,
		-1,
		0
	};

	// Token: 0x04002DF0 RID: 11760
	public int xIndex;

	// Token: 0x04002DF1 RID: 11761
	public int animationCycleCount;
}
