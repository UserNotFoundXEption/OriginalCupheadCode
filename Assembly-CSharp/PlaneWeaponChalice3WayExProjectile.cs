using System;
using UnityEngine;

// Token: 0x02000572 RID: 1394
public class PlaneWeaponChalice3WayExProjectile : AbstractProjectile
{
	// Token: 0x06003A8D RID: 14989 RVA: 0x0010FDC8 File Offset: 0x0010DFC8
	public override void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		base.OnDealDamage(damage, receiver, damageDealer);
		if (this.state == PlaneWeaponChalice3WayExProjectile.State.Idle)
		{
			this.Freeze();
			this.partner.Freeze();
		}
		AudioManager.Play("player_plane_weapon_ex_chomp");
		this.emitAudioFromObject.Add("player_plane_weapon_ex_chomp");
	}

	// Token: 0x06003A8E RID: 14990 RVA: 0x0010FE14 File Offset: 0x0010E014
	public void Freeze()
	{
		this.state = PlaneWeaponChalice3WayExProjectile.State.Frozen;
		this.timeSinceFrozen = 0f;
		this.deathSpark.transform.localScale = new Vector3(0.5f, 0.5f);
		this.deathSpark.flipX = Rand.Bool();
		this.deathSpark.flipY = Rand.Bool();
		this.deathSpark.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		base.animator.Play("Spark", 1, 0f);
	}

	// Token: 0x06003A8F RID: 14991 RVA: 0x0010FEB4 File Offset: 0x0010E0B4
	public void SetArcPosition()
	{
		base.transform.localPosition = new Vector3(Mathf.Sin(EaseUtils.Linear(0.15f, 1f, this.arcTimer) * 3.14159274f) * this.arcX, 10f + EaseUtils.Linear(0f, 1f, this.arcTimer) * 3.14159274f * this.vDirection * this.arcY);
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x0010FF28 File Offset: 0x0010E128
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		this.smokeTimer += CupheadTime.FixedDelta;
		if (this.smokeTimer > this.firstSmokeDelay)
		{
			this.smokeFX.Create(base.transform.position);
			this.smokeTimer -= this.smokeDelay;
		}
		this.sparkleTimer += CupheadTime.FixedDelta;
		if (this.sparkleTimer > this.sparkleDelay)
		{
			this.sparkleFX.Create(base.transform.position + MathUtils.AngleToDirection((float)Random.Range(0, 360)) * Random.Range(0f, this.sparkleRadius));
			this.sparkleTimer -= this.sparkleDelay;
		}
		switch (this.state)
		{
		case PlaneWeaponChalice3WayExProjectile.State.Idle:
			this.SetArcPosition();
			this.arcTimer += this.arcSpeed / 3.14159274f * CupheadTime.FixedDelta;
			base.transform.localScale = new Vector3(Mathf.Lerp(0.5f, 1f, this.arcTimer), Mathf.Lerp(0.5f, 1f, this.arcTimer));
			if (this.arcTimer > 1f)
			{
				this.state = PlaneWeaponChalice3WayExProjectile.State.Paused;
				this.damageDealer.SetDamage(this.damageAfterLaunch);
				this.CollisionDeath.Enemies = true;
				base.transform.localScale = new Vector3(1f, 1f);
			}
			break;
		case PlaneWeaponChalice3WayExProjectile.State.Frozen:
			this.timeSinceFrozen += CupheadTime.FixedDelta;
			if (this.timeSinceFrozen > this.FreezeTime)
			{
				this.state = PlaneWeaponChalice3WayExProjectile.State.Idle;
			}
			break;
		case PlaneWeaponChalice3WayExProjectile.State.Paused:
			this.pauseTime -= CupheadTime.FixedDelta;
			if (this.pauseTime <= 0f)
			{
				this.FindTarget();
				this.state = PlaneWeaponChalice3WayExProjectile.State.Launched;
				Vector3 vector = base.transform.parent.position + Vector3.right * this.xDistanceNoTarget;
				base.transform.parent = null;
				if (this.target != null && this.target.gameObject.activeInHierarchy && this.target.isActiveAndEnabled)
				{
					vector = this.target.transform.position;
					vector.x = Mathf.Clamp(vector.x, base.transform.position.x + this.minXDistance, vector.x);
				}
				this.velocityAfterLaunch = (vector - base.transform.position).normalized;
				this.accelVectorAfterLaunch = this.velocityAfterLaunch * this.accelAfterLaunch;
				this.velocityAfterLaunch *= this.speedAfterLaunch;
			}
			break;
		case PlaneWeaponChalice3WayExProjectile.State.Launched:
			base.transform.position += this.velocityAfterLaunch * CupheadTime.FixedDelta;
			this.velocityAfterLaunch += this.accelVectorAfterLaunch * CupheadTime.FixedDelta;
			if (this.velocityAfterLaunch.x > 0f)
			{
				if (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
				{
					base.animator.Play("Shoot");
					this.shootFX.Create(base.transform.position + Vector3.left * 20f);
				}
				this.magnet.transform.eulerAngles = new Vector3(0f, 0f, MathUtils.DirectionToAngle(this.velocityAfterLaunch));
			}
			break;
		}
	}

	// Token: 0x06003A91 RID: 14993 RVA: 0x0002FA5E File Offset: 0x0002DC5E
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06003A92 RID: 14994 RVA: 0x0002FA6F File Offset: 0x0002DC6F
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x06003A93 RID: 14995 RVA: 0x0002FA8F File Offset: 0x0002DC8F
	public void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06003A94 RID: 14996 RVA: 0x00110334 File Offset: 0x0010E534
	public override void Die()
	{
		base.Die();
		this.magnet.transform.eulerAngles = Vector3.zero;
		this.magnet.flipX = Rand.Bool();
		this.deathSpark.transform.localScale = new Vector3(1f, 1f);
		this.deathSpark.flipX = Rand.Bool();
		this.deathSpark.flipY = Rand.Bool();
		this.deathSpark.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		base.animator.Play("Spark", 1, 0f);
		base.animator.Play((this.ID != 0) ? "DieB" : "DieA");
	}

	// Token: 0x06003A95 RID: 14997 RVA: 0x00110414 File Offset: 0x0010E614
	public void FindTarget()
	{
		if (this.partner != null && this.ID == 1)
		{
			return;
		}
		float num = float.MaxValue;
		Collider2D collider2D = null;
		Vector2 vector = base.transform.parent.position;
		foreach (DamageReceiver damageReceiver in Object.FindObjectsOfType<DamageReceiver>())
		{
			if (damageReceiver.gameObject.activeInHierarchy && damageReceiver.type == DamageReceiver.Type.Enemy && damageReceiver.transform.position.x >= base.transform.position.x)
			{
				foreach (Collider2D collider2D2 in damageReceiver.GetComponents<Collider2D>())
				{
					if (collider2D2.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D2.bounds.center, collider2D2.bounds.size / 2f))
					{
						float num2 = Mathf.Abs(MathUtils.DirectionToAngle(collider2D2.bounds.center - vector));
						if (num2 < num)
						{
							num = num2;
							collider2D = collider2D2;
						}
					}
				}
				foreach (DamageReceiverChild damageReceiverChild in damageReceiver.GetComponentsInChildren<DamageReceiverChild>())
				{
					foreach (Collider2D collider2D3 in damageReceiverChild.GetComponents<Collider2D>())
					{
						if (collider2D3.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D3.bounds.center, collider2D3.bounds.size / 2f))
						{
							float num3 = Mathf.Abs(MathUtils.DirectionToAngle(collider2D3.bounds.center - vector));
							if (num3 < num)
							{
								num = num3;
								collider2D = collider2D3;
							}
						}
					}
				}
			}
		}
		this.target = collider2D;
		if (this.partner != null)
		{
			this.partner.target = collider2D;
		}
	}

	// Token: 0x06003A96 RID: 14998 RVA: 0x0002FA9E File Offset: 0x0002DC9E
	public override void OnLevelEnd()
	{
	}

	// Token: 0x04002EE2 RID: 12002
	public const float Y_OFFSET = 10f;

	// Token: 0x04002EE3 RID: 12003
	public float FreezeTime;

	// Token: 0x04002EE4 RID: 12004
	public PlaneWeaponChalice3WayExProjectile.State state;

	// Token: 0x04002EE5 RID: 12005
	public float timeSinceFrozen;

	// Token: 0x04002EE6 RID: 12006
	public float arcTimer;

	// Token: 0x04002EE7 RID: 12007
	public float arcSpeed = 5f;

	// Token: 0x04002EE8 RID: 12008
	public float arcX = 500f;

	// Token: 0x04002EE9 RID: 12009
	public float arcY = 500f;

	// Token: 0x04002EEA RID: 12010
	public float damageAfterLaunch = 20f;

	// Token: 0x04002EEB RID: 12011
	public float speedAfterLaunch = 3000f;

	// Token: 0x04002EEC RID: 12012
	public float accelAfterLaunch = 100f;

	// Token: 0x04002EED RID: 12013
	public float minXDistance = 500f;

	// Token: 0x04002EEE RID: 12014
	public float xDistanceNoTarget = 500f;

	// Token: 0x04002EEF RID: 12015
	public int ID;

	// Token: 0x04002EF0 RID: 12016
	public PlaneWeaponChalice3WayExProjectile partner;

	// Token: 0x04002EF1 RID: 12017
	public Vector3 accelVectorAfterLaunch;

	// Token: 0x04002EF2 RID: 12018
	public Vector3 velocityAfterLaunch;

	// Token: 0x04002EF3 RID: 12019
	public Collider2D target;

	// Token: 0x04002EF4 RID: 12020
	public float pauseTime = 0.5f;

	// Token: 0x04002EF5 RID: 12021
	public float vDirection = 1f;

	// Token: 0x04002EF6 RID: 12022
	[SerializeField]
	public SpriteRenderer magnet;

	// Token: 0x04002EF7 RID: 12023
	[SerializeField]
	public SpriteRenderer deathSpark;

	// Token: 0x04002EF8 RID: 12024
	[SerializeField]
	public Effect shootFX;

	// Token: 0x04002EF9 RID: 12025
	[SerializeField]
	public Effect smokeFX;

	// Token: 0x04002EFA RID: 12026
	[SerializeField]
	public Effect sparkleFX;

	// Token: 0x04002EFB RID: 12027
	[SerializeField]
	public float firstSmokeDelay = 0.7f;

	// Token: 0x04002EFC RID: 12028
	[SerializeField]
	public float smokeDelay = 0.09f;

	// Token: 0x04002EFD RID: 12029
	[SerializeField]
	public float sparkleDelay = 0.15f;

	// Token: 0x04002EFE RID: 12030
	[SerializeField]
	public float sparkleRadius = 20f;

	// Token: 0x04002EFF RID: 12031
	public float smokeTimer;

	// Token: 0x04002F00 RID: 12032
	public float sparkleTimer;

	// Token: 0x020011F7 RID: 4599
	public enum State
	{
		// Token: 0x04007D11 RID: 32017
		Idle,
		// Token: 0x04007D12 RID: 32018
		Frozen,
		// Token: 0x04007D13 RID: 32019
		Paused,
		// Token: 0x04007D14 RID: 32020
		Launched
	}
}
