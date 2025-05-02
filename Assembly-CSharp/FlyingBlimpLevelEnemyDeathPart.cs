using System;
using UnityEngine;

// Token: 0x0200023F RID: 575
public class FlyingBlimpLevelEnemyDeathPart : AbstractProjectile
{
	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06001A7B RID: 6779 RVA: 0x000168CF File Offset: 0x00014ACF
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x000A8CF0 File Offset: 0x000A6EF0
	public FlyingBlimpLevelEnemyDeathPart CreatePart(Vector3 position, LevelProperties.FlyingBlimp.Gear properties)
	{
		FlyingBlimpLevelEnemyDeathPart flyingBlimpLevelEnemyDeathPart = this.InstantiatePrefab<FlyingBlimpLevelEnemyDeathPart>();
		flyingBlimpLevelEnemyDeathPart.transform.position = position;
		flyingBlimpLevelEnemyDeathPart.properties = properties;
		return flyingBlimpLevelEnemyDeathPart;
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x000A8D18 File Offset: 0x000A6F18
	public override void Start()
	{
		base.Start();
		if (!this.gear)
		{
			this.velocity = new Vector2(Random.Range(-500f, 500f), Random.Range(500f, 1200f));
		}
		else
		{
			this.velocity = new Vector2(-500f, this.properties.bounceHeight);
		}
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x000A8D80 File Offset: 0x000A6F80
	public override void FixedUpdate()
	{
		base.Update();
		base.transform.position += (this.velocity + new Vector2(-this.properties.bounceSpeed, this.accumulatedGravity)) * Time.fixedDeltaTime;
		this.accumulatedGravity += -100f;
		if (base.transform.position.y < -360f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x000A8E14 File Offset: 0x000A7014
	public override void OnParry(AbstractPlayerController player)
	{
		if (!this.getNewWeapon)
		{
			if (this.parryCounter < (float)this.properties.parryCount)
			{
				this.parryCounter += 1f;
				this.accumulatedGravity = 0f;
			}
			else
			{
				base.GetComponent<SpriteRenderer>().color = ColorUtils.HexToColor("FF00EDFF");
				base.FrameDelayedCallback(new Action(this.SetWeapon), 5);
				this.accumulatedGravity = 0f;
			}
		}
		else
		{
			this.parriedIt = true;
			this.Die();
		}
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x000A8EAC File Offset: 0x000A70AC
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.getNewWeapon && !this.parriedIt)
		{
			AbstractPlayerController next = PlayerManager.GetNext();
			PlanePlayerController planePlayerController = next as PlanePlayerController;
			planePlayerController.weaponManager.SwitchToWeapon(Weapon.plane_weapon_laser);
			this.Die();
		}
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x000168D6 File Offset: 0x00014AD6
	public void SetWeapon()
	{
		this.getNewWeapon = true;
	}

	// Token: 0x06001A82 RID: 6786 RVA: 0x000168DF File Offset: 0x00014ADF
	public override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x0400153C RID: 5436
	[SerializeField]
	public bool gear;

	// Token: 0x0400153D RID: 5437
	public LevelProperties.FlyingBlimp.Gear properties;

	// Token: 0x0400153E RID: 5438
	public const float VELOCITY_X_MIN = -500f;

	// Token: 0x0400153F RID: 5439
	public const float VELOCITY_X_MAX = 500f;

	// Token: 0x04001540 RID: 5440
	public const float VELOCITY_Y_MIN = 500f;

	// Token: 0x04001541 RID: 5441
	public const float VELOCITY_Y_MAX = 1200f;

	// Token: 0x04001542 RID: 5442
	public const float GRAVITY = -100f;

	// Token: 0x04001543 RID: 5443
	public Vector2 velocity;

	// Token: 0x04001544 RID: 5444
	public float accumulatedGravity;

	// Token: 0x04001545 RID: 5445
	public float parryCounter;

	// Token: 0x04001546 RID: 5446
	public bool getNewWeapon;

	// Token: 0x04001547 RID: 5447
	public bool parriedIt;
}
