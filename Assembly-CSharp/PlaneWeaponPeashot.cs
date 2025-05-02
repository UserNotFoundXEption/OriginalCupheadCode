using System;
using UnityEngine;

// Token: 0x02000575 RID: 1397
public class PlaneWeaponPeashot : AbstractPlaneWeapon
{
	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06003AA5 RID: 15013 RVA: 0x0002FB52 File Offset: 0x0002DD52
	public override bool rapidFire
	{
		get
		{
			return WeaponProperties.PlaneWeaponPeashot.Basic.rapidFire;
		}
	}

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x06003AA6 RID: 15014 RVA: 0x0002FB59 File Offset: 0x0002DD59
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.PlaneWeaponPeashot.Basic.rapidFireRate;
		}
	}

	// Token: 0x06003AA7 RID: 15015 RVA: 0x00110A48 File Offset: 0x0010EC48
	public override AbstractProjectile fireBasic()
	{
		if ((this.player.id == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (this.player.id == PlayerId.PlayerTwo && PlayerManager.player1IsMugman))
		{
			if (!AudioManager.CheckIfPlaying("player_plane_weapon_fire_loop_cuphead"))
			{
				AudioManager.PlayLoop("player_plane_weapon_fire_loop_cuphead");
			}
		}
		else if (!AudioManager.CheckIfPlaying("player_plane_weapon_fire_loop_mugman"))
		{
			AudioManager.PlayLoop("player_plane_weapon_fire_loop_mugman");
		}
		this.emitAudioFromObject.Add("player_plane_weapon_fire_loop_cuphead");
		this.emitAudioFromObject.Add("player_plane_weapon_fire_loop_mugman");
		BasicProjectile basicProjectile = base.fireBasic() as BasicProjectile;
		basicProjectile.Speed = WeaponProperties.PlaneWeaponPeashot.Basic.speed;
		basicProjectile.Damage = WeaponProperties.PlaneWeaponPeashot.Basic.damage;
		basicProjectile.PlayerId = this.player.id;
		float num = this.yPositions[this.currentY];
		this.currentY++;
		if (this.currentY >= this.yPositions.Length)
		{
			this.currentY = 0;
		}
		basicProjectile.transform.AddPosition(0f, num, 0f);
		Animator component = basicProjectile.GetComponent<Animator>();
		component.SetInteger("Variant", Random.Range(0, component.GetInteger("MaxVariants")));
		component.SetBool("isCH", (basicProjectile.PlayerId == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (basicProjectile.PlayerId == PlayerId.PlayerTwo && PlayerManager.player1IsMugman));
		if (this.player.Shrunk)
		{
			basicProjectile.Damage *= this.shrunkDamageMultiplier;
			basicProjectile.transform.AddPosition(0f, num * -0.5f, 0f);
			basicProjectile.DestroyDistance = (float)Random.Range(200, 350);
			basicProjectile.DestroyDistanceAnimated = true;
			basicProjectile.DamageSource = DamageDealer.DamageSource.SmallPlane;
		}
		return basicProjectile;
	}

	// Token: 0x06003AA8 RID: 15016 RVA: 0x00110C24 File Offset: 0x0010EE24
	public override AbstractProjectile fireEx()
	{
		PlaneWeaponPeashotExProjectile planeWeaponPeashotExProjectile = base.fireEx() as PlaneWeaponPeashotExProjectile;
		planeWeaponPeashotExProjectile.MaxSpeed = WeaponProperties.PlaneWeaponPeashot.Ex.maxSpeed;
		planeWeaponPeashotExProjectile.Acceleration = WeaponProperties.PlaneWeaponPeashot.Ex.acceleration;
		planeWeaponPeashotExProjectile.FreezeTime = WeaponProperties.PlaneWeaponPeashot.Ex.freezeTime;
		planeWeaponPeashotExProjectile.Damage = WeaponProperties.PlaneWeaponPeashot.Ex.damage;
		planeWeaponPeashotExProjectile.DamageRate = WeaponProperties.PlaneWeaponPeashot.Ex.freezeTime + WeaponProperties.PlaneWeaponPeashot.Ex.damageDistance / planeWeaponPeashotExProjectile.MaxSpeed;
		planeWeaponPeashotExProjectile.PlayerId = this.player.id;
		planeWeaponPeashotExProjectile.speed = Mathf.Clamp(this.player.motor.Velocity.x, 0f, planeWeaponPeashotExProjectile.MaxSpeed);
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(planeWeaponPeashotExProjectile);
		planeWeaponPeashotExProjectile.Init();
		return planeWeaponPeashotExProjectile;
	}

	// Token: 0x04002F06 RID: 12038
	public const float Y_POS = 20f;

	// Token: 0x04002F07 RID: 12039
	public float[] yPositions = new float[]
	{
		20f,
		-20f
	};

	// Token: 0x04002F08 RID: 12040
	public int currentY;
}
