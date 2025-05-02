using System;
using UnityEngine;

// Token: 0x02000554 RID: 1364
public class WeaponSpread : AbstractLevelWeapon
{
	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x06003919 RID: 14617 RVA: 0x0002E81E File Offset: 0x0002CA1E
	public override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x0600391A RID: 14618 RVA: 0x0002E821 File Offset: 0x0002CA21
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponSpreadshot.Basic.rapidFireRate;
		}
	}

	// Token: 0x0600391B RID: 14619 RVA: 0x0010A960 File Offset: 0x00108B60
	public override AbstractProjectile fireBasic()
	{
		float[] array = new float[]
		{
			0.5f,
			0.75f
		};
		float damage = WeaponProperties.LevelWeaponSpreadshot.Basic.damage;
		for (int i = 0; i < 2; i++)
		{
			BasicProjectile basicProjectile = base.fireBasicNoEffect() as BasicProjectile;
			basicProjectile.Speed = WeaponProperties.LevelWeaponSpreadshot.Basic.speed * array[i];
			basicProjectile.DestroyDistance = WeaponProperties.LevelWeaponSpreadshot.Basic.distance - 20f * (float)(i + 1);
			basicProjectile.Damage = damage;
			basicProjectile.PlayerId = this.player.id;
			basicProjectile.transform.AddEulerAngles(0f, 0f, 15f * (float)(i + 1));
			Animator component = basicProjectile.GetComponent<Animator>();
			component.SetBool("Large", i == 1);
			BasicProjectile basicProjectile2 = base.fireBasicNoEffect() as BasicProjectile;
			basicProjectile2.Speed = WeaponProperties.LevelWeaponSpreadshot.Basic.speed * array[i];
			basicProjectile2.DestroyDistance = WeaponProperties.LevelWeaponSpreadshot.Basic.distance - 20f * (float)(i + 1);
			basicProjectile2.Damage = damage;
			basicProjectile2.PlayerId = this.player.id;
			basicProjectile2.transform.AddEulerAngles(0f, 0f, -15f * (float)(i + 1));
			Animator component2 = basicProjectile2.GetComponent<Animator>();
			component2.SetBool("Large", i == 1);
		}
		BasicProjectile basicProjectile3 = base.fireBasic() as BasicProjectile;
		basicProjectile3.Speed = WeaponProperties.LevelWeaponSpreadshot.Basic.speed;
		basicProjectile3.Damage = damage;
		basicProjectile3.PlayerId = this.player.id;
		basicProjectile3.DestroyDistance = WeaponProperties.LevelWeaponSpreadshot.Basic.distance;
		return basicProjectile3;
	}

	// Token: 0x0600391C RID: 14620 RVA: 0x0010AAE4 File Offset: 0x00108CE4
	public override AbstractProjectile fireEx()
	{
		AudioManager.Play("player_weapon_exploder_fire");
		PlayerLevelSpreadEx playerLevelSpreadEx = base.fireEx() as PlayerLevelSpreadEx;
		playerLevelSpreadEx.Init(WeaponProperties.LevelWeaponSpreadshot.Ex.speed, WeaponProperties.LevelWeaponSpreadshot.Ex.damage, WeaponProperties.LevelWeaponSpreadshot.Ex.childCount, WeaponProperties.LevelWeaponSpreadshot.Ex.radius);
		return playerLevelSpreadEx;
	}

	// Token: 0x0600391D RID: 14621 RVA: 0x0002E828 File Offset: 0x0002CA28
	public override void BeginBasic()
	{
		base.BeginBasic();
		this.BasicSoundLoop("player_weapon_spread_loop", "player_weapon_spread_loop_p2");
	}

	// Token: 0x0600391E RID: 14622 RVA: 0x0002E840 File Offset: 0x0002CA40
	public override void EndBasic()
	{
		base.EndBasic();
		this.StopLoopSound("player_weapon_spread_loop", "player_weapon_spread_loop_p2");
	}
}
