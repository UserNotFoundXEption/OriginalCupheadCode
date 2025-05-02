using System;
using System.Collections;

// Token: 0x0200054D RID: 1357
public class WeaponPushback : AbstractLevelWeapon
{
	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x060038F4 RID: 14580 RVA: 0x0002E6B0 File Offset: 0x0002C8B0
	public override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x060038F5 RID: 14581 RVA: 0x0002E6B3 File Offset: 0x0002C8B3
	public override float rapidFireRate
	{
		get
		{
			return this.bulletFireRate;
		}
	}

	// Token: 0x060038F6 RID: 14582 RVA: 0x0010A2F0 File Offset: 0x001084F0
	public void Start()
	{
		this.speedTime = WeaponProperties.LevelWeaponPushback.Basic.speedTime;
		base.StartCoroutine(this.determine_speed_cr());
		this.forceAmount = WeaponProperties.LevelWeaponPushback.Basic.pushbackSpeed;
		this.forceLeft = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, this.forceAmount);
		this.forceRight = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, -this.forceAmount);
	}

	// Token: 0x060038F7 RID: 14583 RVA: 0x0010A348 File Offset: 0x00108548
	public override AbstractProjectile fireBasic()
	{
		BasicProjectile basicProjectile = base.fireBasic() as BasicProjectile;
		basicProjectile.Speed = this.bulletSpeed;
		basicProjectile.Damage = WeaponProperties.LevelWeaponPushback.Basic.damage;
		basicProjectile.PlayerId = this.player.id;
		float y = this.yPositions[this.currentY];
		this.currentY++;
		if (this.currentY >= this.yPositions.Length)
		{
			this.currentY = 0;
		}
		basicProjectile.transform.AddPosition(0f, y, 0f);
		bool flag = this.player.transform.localScale.x < 0f;
		if (!this.hasForce)
		{
			this.AddHorizontalForce(flag);
		}
		return basicProjectile;
	}

	// Token: 0x060038F8 RID: 14584 RVA: 0x0010A408 File Offset: 0x00108608
	public new void Update()
	{
		this.facingLeft = (this.player.transform.localScale.x < 0f);
		if ((this.hasForce && !this.holdingShoot) || this.forceIsLeft != this.facingLeft)
		{
			this.player.motor.RemoveForce(this.forceLeft);
			this.player.motor.RemoveForce(this.forceRight);
			this.hasForce = false;
		}
	}

	// Token: 0x060038F9 RID: 14585 RVA: 0x0010A494 File Offset: 0x00108694
	public void AddHorizontalForce(bool facingLeft)
	{
		this.hasForce = true;
		this.forceIsLeft = facingLeft;
		if (facingLeft)
		{
			this.player.motor.AddForce(this.forceLeft);
		}
		else
		{
			this.player.motor.AddForce(this.forceRight);
		}
	}

	// Token: 0x060038FA RID: 14586 RVA: 0x0010A4E8 File Offset: 0x001086E8
	public IEnumerator determine_speed_cr()
	{
		float t = 0f;
		float speedVal = 0f;
		float fireVal = 0f;
		for (;;)
		{
			if (this.holdingShoot)
			{
				if (speedVal < 1f)
				{
					speedVal = t / this.speedTime;
					fireVal = 1f - t / this.speedTime;
					t += CupheadTime.Delta;
				}
				else
				{
					speedVal = 1f;
					t = 1f;
				}
			}
			else if (speedVal > 0f)
			{
				speedVal = t / this.speedTime;
				fireVal = 1f - t / this.speedTime;
				t -= CupheadTime.Delta;
			}
			else
			{
				speedVal = 0f;
				t = 0f;
			}
			this.holdingShoot = this.player.input.actions.GetButton(3);
			this.bulletSpeed = WeaponProperties.LevelWeaponPushback.Basic.speed.GetFloatAt(speedVal);
			this.bulletFireRate = WeaponProperties.LevelWeaponPushback.Basic.fireRate.GetFloatAt(fireVal);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002DD4 RID: 11732
	public const float ONE = 1f;

	// Token: 0x04002DD5 RID: 11733
	public const float Y_POS = 20f;

	// Token: 0x04002DD6 RID: 11734
	public const float ROTATION_OFFSET = 3f;

	// Token: 0x04002DD7 RID: 11735
	public int currentY;

	// Token: 0x04002DD8 RID: 11736
	public float[] yPositions = new float[]
	{
		0f,
		20f,
		40f,
		20f
	};

	// Token: 0x04002DD9 RID: 11737
	public float bulletSpeed;

	// Token: 0x04002DDA RID: 11738
	public float bulletFireRate;

	// Token: 0x04002DDB RID: 11739
	public float speedTime;

	// Token: 0x04002DDC RID: 11740
	public float forceAmount;

	// Token: 0x04002DDD RID: 11741
	public bool holdingShoot;

	// Token: 0x04002DDE RID: 11742
	public bool hasForce;

	// Token: 0x04002DDF RID: 11743
	public bool facingLeft;

	// Token: 0x04002DE0 RID: 11744
	public bool forceIsLeft;

	// Token: 0x04002DE1 RID: 11745
	public LevelPlayerMotor.VelocityManager.Force forceLeft;

	// Token: 0x04002DE2 RID: 11746
	public LevelPlayerMotor.VelocityManager.Force forceRight;
}
