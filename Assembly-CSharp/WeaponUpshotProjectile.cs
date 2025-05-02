using System;
using UnityEngine;

// Token: 0x02000557 RID: 1367
public class WeaponUpshotProjectile : AbstractProjectile
{
	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x06003930 RID: 14640 RVA: 0x0002E90B File Offset: 0x0002CB0B
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003931 RID: 14641 RVA: 0x0010B140 File Offset: 0x00109340
	public override void Start()
	{
		base.Start();
		this.damageDealer.isDLCWeapon = true;
		AbstractPlayerController player = PlayerManager.GetPlayer(this.PlayerId);
		this.onLeft = (player.transform.localScale.x < 0f);
		this.startAngle = base.transform.eulerAngles.z;
		this.SetAngle();
	}

	// Token: 0x06003932 RID: 14642 RVA: 0x0002E90E File Offset: 0x0002CB0E
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		this.UpdateSpeed();
		this.Move();
	}

	// Token: 0x06003933 RID: 14643 RVA: 0x0010B1AC File Offset: 0x001093AC
	public void UpdateSpeed()
	{
		if (this.time < this.timeToArc)
		{
			this.time += CupheadTime.FixedDelta;
			this.ySpeed = this.ySpeedMinMax.GetFloatAt(this.time / this.timeToArc);
			this.ySpeed = ((!this.onLeft) ? this.ySpeed : (-this.ySpeed));
			this.SetAngle();
		}
	}

	// Token: 0x06003934 RID: 14644 RVA: 0x0010B224 File Offset: 0x00109424
	public void Move()
	{
		if (base.dead)
		{
			return;
		}
		Vector3 vector;
		vector..ctor(this.xSpeed, this.ySpeed);
		Quaternion quaternion = Quaternion.Euler(0f, 0f, this.startAngle);
		vector = quaternion * vector;
		base.transform.position += vector * CupheadTime.FixedDelta;
	}

	// Token: 0x06003935 RID: 14645 RVA: 0x0010B290 File Offset: 0x00109490
	public void SetAngle()
	{
		int num = Mathf.RoundToInt(this.startAngle);
		if (num != 0)
		{
			if (num == 45)
			{
				base.transform.SetEulerAngles(null, null, new float?(this.time * 2f / this.timeToArc * 45f));
				return;
			}
			if (num == 90)
			{
				base.transform.SetEulerAngles(null, null, new float?((float)((!this.onLeft) ? 45 : -225) + this.time * 2f / this.timeToArc * (float)((!this.onLeft) ? 45 : -45)));
				return;
			}
			if (num == 135)
			{
				base.transform.SetEulerAngles(null, null, new float?(180f + this.time * 2f / this.timeToArc * -45f));
				return;
			}
			if (num != 180)
			{
				if (num == 225)
				{
					base.transform.SetEulerAngles(null, null, new float?(270f + this.time * 2f / this.timeToArc * -45f));
					return;
				}
				if (num == 270)
				{
					base.transform.SetEulerAngles(null, null, new float?((float)((!this.onLeft) ? 225 : -45) + this.time * 2f / this.timeToArc * (float)((!this.onLeft) ? 45 : -45)));
					return;
				}
				if (num != 315)
				{
					return;
				}
				base.transform.SetEulerAngles(null, null, new float?(270f + this.time * 2f / this.timeToArc * 45f));
				return;
			}
		}
		base.transform.SetEulerAngles(null, null, new float?((float)((!this.onLeft) ? -45 : 225) + this.time * 2f / this.timeToArc * (float)((!this.onLeft) ? 45 : -45)));
	}

	// Token: 0x06003936 RID: 14646 RVA: 0x0002E922 File Offset: 0x0002CB22
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003937 RID: 14647 RVA: 0x0010B548 File Offset: 0x00109748
	public override void OnCollisionDie(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionDie(hit, phase);
		if (base.tag == "PlayerProjectile" && phase == CollisionPhase.Enter)
		{
			if (hit.GetComponent<DamageReceiver>() && hit.GetComponent<DamageReceiver>().enabled)
			{
				AudioManager.Play("player_shoot_hit_cuphead");
			}
			else
			{
				AudioManager.Play("player_weapon_peashot_miss");
			}
		}
	}

	// Token: 0x06003938 RID: 14648 RVA: 0x0002E93F File Offset: 0x0002CB3F
	public override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x04002E01 RID: 11777
	public MinMax ySpeedMinMax;

	// Token: 0x04002E02 RID: 11778
	public float timeToArc;

	// Token: 0x04002E03 RID: 11779
	public float xSpeed;

	// Token: 0x04002E04 RID: 11780
	public float ySpeed;

	// Token: 0x04002E05 RID: 11781
	public float time;

	// Token: 0x04002E06 RID: 11782
	public bool onLeft;

	// Token: 0x04002E07 RID: 11783
	public float startAngle;
}
