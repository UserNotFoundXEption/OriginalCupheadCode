using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000336 RID: 822
public class RobotLevelHatchShotbot : AbstractCollidableObject
{
	// Token: 0x060023EB RID: 9195 RVA: 0x000C21D0 File Offset: 0x000C03D0
	public void InitShotbot(int hp, int bulletSpeed, int pinkBulletCount, float shootDelay, int flightSpeed)
	{
		this.speedPCT = 200f / (float)flightSpeed;
		this.health = (float)hp;
		this.flightSpeed = flightSpeed;
		this.bulletSpeed = bulletSpeed;
		this.pinkBulletCount = pinkBulletCount;
		this.shotsFired = 0;
		this.shootDelay = shootDelay;
		this.damageDealer = DamageDealer.NewEnemy();
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.rotate_cr());
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060023EC RID: 9196 RVA: 0x000C2264 File Offset: 0x000C0464
	public RobotLevelHatchShotbot Create()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject);
		gameObject.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(180f));
		return gameObject.GetComponent<RobotLevelHatchShotbot>();
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x000C22AC File Offset: 0x000C04AC
	public IEnumerator intro_cr()
	{
		float rotTime = 0.15f;
		float scale = base.transform.localScale.x;
		base.transform.SetEulerAngles(null, null, new float?(180f));
		yield return CupheadTime.WaitForSeconds(this, 0.5f * this.speedPCT);
		yield return base.StartCoroutine(this.tweenRotation_cr(180f * scale, 270f * scale, rotTime / 3f * this.speedPCT));
		yield return base.StartCoroutine(this.tweenRotation_cr(270f * scale, 180f * scale, rotTime / 3f * this.speedPCT));
		yield return null;
		yield break;
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x000C22C8 File Offset: 0x000C04C8
	public IEnumerator move_cr()
	{
		float scale = base.transform.localScale.x;
		for (;;)
		{
			Vector2 move = base.transform.right * (float)this.flightSpeed * CupheadTime.Delta * scale;
			base.transform.AddPosition(move.x, move.y, 0f);
			yield return null;
			if (base.transform.position.y > 460f)
			{
				this.End();
			}
		}
		yield break;
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x000C22E4 File Offset: 0x000C04E4
	public IEnumerator rotate_cr()
	{
		float rotTime = 0.15f * this.speedPCT;
		float scale = base.transform.localScale.x;
		yield return CupheadTime.WaitForSeconds(this, 1.8f * this.speedPCT);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.time.x * this.speedPCT);
			yield return base.StartCoroutine(this.tweenRotation_cr(180f * scale, 90f * scale, rotTime));
			yield return CupheadTime.WaitForSeconds(this, this.time.y * this.speedPCT);
			yield return base.StartCoroutine(this.tweenRotation_cr(90f * scale, 0f, rotTime));
			yield return CupheadTime.WaitForSeconds(this, this.time.x * this.speedPCT);
			yield return base.StartCoroutine(this.tweenRotation_cr(0f, 90f * scale, rotTime));
			yield return CupheadTime.WaitForSeconds(this, this.time.y * this.speedPCT);
			yield return base.StartCoroutine(this.tweenRotation_cr(90f * scale, 180f * scale, rotTime));
		}
		yield break;
	}

	// Token: 0x060023F0 RID: 9200 RVA: 0x000C2300 File Offset: 0x000C0500
	public IEnumerator tweenRotation_cr(float start, float end, float time)
	{
		base.transform.SetEulerAngles(null, null, new float?(start));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetEulerAngles(null, null, new float?(EaseUtils.Ease(EaseUtils.EaseType.linear, start, end, val)));
			t += CupheadTime.Delta / 3f;
			yield return null;
		}
		base.transform.SetEulerAngles(null, null, new float?(end));
		yield break;
	}

	// Token: 0x060023F1 RID: 9201 RVA: 0x000C2330 File Offset: 0x000C0530
	public IEnumerator fire_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.shootDelay);
			AudioManager.Play("robot_shotbot_shoot");
			this.emitAudioFromObject.Add("robot_shotbot_shoot");
			GameObject proj = Object.Instantiate<GameObject>(this.projectile);
			proj.transform.position = base.transform.position;
			proj.transform.right = (PlayerManager.GetNext().center - base.transform.position).normalized;
			proj.GetComponent<BasicProjectile>().Speed = (float)this.bulletSpeed;
			if (this.shotsFired >= this.pinkBulletCount)
			{
				this.shotsFired = 0;
				proj.GetComponent<SpriteRenderer>().sprite = this.spriteSpecial;
				proj.GetComponent<BasicProjectile>().SetParryable(true);
			}
			else
			{
				this.shotsFired++;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060023F2 RID: 9202 RVA: 0x0001E4EF File Offset: 0x0001C6EF
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Dead();
		}
	}

	// Token: 0x060023F3 RID: 9203 RVA: 0x0001E51A File Offset: 0x0001C71A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060023F4 RID: 9204 RVA: 0x0001E538 File Offset: 0x0001C738
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x0001E550 File Offset: 0x0001C750
	public void Dead()
	{
		AudioManager.Play("robot_shotbot_death");
		this.emitAudioFromObject.Add("robot_shotbot_death");
		this.StopAllCoroutines();
		this.CreateSmoke();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060023F6 RID: 9206 RVA: 0x0001E583 File Offset: 0x0001C783
	public void End()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000C234C File Offset: 0x000C054C
	public void CreateSmoke()
	{
		this.smokeEffect.Create(base.transform.position);
		foreach (SpriteDeathParts spriteDeathParts in this.deathParts)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x04001DC3 RID: 7619
	[SerializeField]
	public Effect smokeEffect;

	// Token: 0x04001DC4 RID: 7620
	[SerializeField]
	public SpriteDeathParts[] deathParts;

	// Token: 0x04001DC5 RID: 7621
	[SerializeField]
	public GameObject projectile;

	// Token: 0x04001DC6 RID: 7622
	[SerializeField]
	public Sprite spriteSpecial;

	// Token: 0x04001DC7 RID: 7623
	[SerializeField]
	public Vector2 time;

	// Token: 0x04001DC8 RID: 7624
	public float speedPCT;

	// Token: 0x04001DC9 RID: 7625
	public float health;

	// Token: 0x04001DCA RID: 7626
	public int flightSpeed;

	// Token: 0x04001DCB RID: 7627
	public int bulletSpeed;

	// Token: 0x04001DCC RID: 7628
	public int pinkBulletCount;

	// Token: 0x04001DCD RID: 7629
	public int shotsFired;

	// Token: 0x04001DCE RID: 7630
	public float shootDelay;

	// Token: 0x04001DCF RID: 7631
	public DamageDealer damageDealer;

	// Token: 0x04001DD0 RID: 7632
	public const int MAX_HEIGHT = 460;
}
