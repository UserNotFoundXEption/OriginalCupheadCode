using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200033F RID: 831
public class RumRunnersLevelCopBall : AbstractProjectile
{
	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06002467 RID: 9319 RVA: 0x0001EC14 File Offset: 0x0001CE14
	// (set) Token: 0x06002468 RID: 9320 RVA: 0x0001EC1C File Offset: 0x0001CE1C
	public bool leaveScreen { get; set; }

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06002469 RID: 9321 RVA: 0x0001EC25 File Offset: 0x0001CE25
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x0001EC2C File Offset: 0x0001CE2C
	public override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600246B RID: 9323 RVA: 0x000C3B68 File Offset: 0x000C1D68
	public void Init(Vector3 position, Vector3 velocity, float speed, float health, LevelProperties.RumRunners.CopBall properties, Transform snoutPos)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		this.properties = properties;
		this.health = health;
		this.velocity = velocity;
		this.offset = base.GetComponent<Collider2D>().bounds.size.x / 2f;
		this.leaveScreen = false;
		this.circleCollider.enabled = false;
		this.launched = false;
		this.snoutPos = snoutPos;
		if (properties.constSpeed)
		{
			this.speed = speed;
		}
		else
		{
			base.StartCoroutine(this.gradualSpeed_cr());
		}
		RumRunnersLevelCopBall.LastSortingIndex--;
		if (RumRunnersLevelCopBall.LastSortingIndex < 10)
		{
			RumRunnersLevelCopBall.LastSortingIndex = 15;
		}
		base.GetComponent<SpriteRenderer>().sortingOrder = RumRunnersLevelCopBall.LastSortingIndex;
		this.audioLoopNumber = RumRunnersLevelCopBall.CurrentAudioLoopIndex + 1;
		RumRunnersLevelCopBall.CurrentAudioLoopIndex = MathUtilities.NextIndex(RumRunnersLevelCopBall.CurrentAudioLoopIndex, RumRunnersLevelCopBall.AudioLoopCount);
		this.SFX_RUMRUN_P3_BallCop_VocalShouts_Loop(this.audioLoopNumber);
	}

	// Token: 0x0600246C RID: 9324 RVA: 0x0001EC57 File Offset: 0x0001CE57
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600246D RID: 9325 RVA: 0x0001EC75 File Offset: 0x0001CE75
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Death(true);
		}
	}

	// Token: 0x0600246E RID: 9326 RVA: 0x000C3C70 File Offset: 0x000C1E70
	public void Launch()
	{
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.shoot_cr());
		base.StartCoroutine(this.checkToDie_cr());
		this.circleCollider.enabled = true;
		base.GetComponent<SpriteRenderer>().sortingLayerName = "Projectiles";
		this.launched = true;
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x0001ECAB File Offset: 0x0001CEAB
	public void LateUpdate()
	{
		if (!this.launched)
		{
			base.transform.position = this.snoutPos.position;
		}
	}

	// Token: 0x06002470 RID: 9328 RVA: 0x000C3CC8 File Offset: 0x000C1EC8
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			base.transform.position += this.velocity * this.speed * CupheadTime.FixedDelta;
			this.CheckBounds();
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002471 RID: 9329 RVA: 0x000C3CE4 File Offset: 0x000C1EE4
	public void CheckBounds()
	{
		bool flag = this.properties.sideWallBounce && !this.leaveScreen;
		Quaternion? quaternion = null;
		Vector3 zero = Vector3.zero;
		if (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMax - this.offset && this.velocity.y > 0f)
		{
			this.velocity.y = -Mathf.Abs(this.velocity.y);
			quaternion = new Quaternion?(Quaternion.identity);
			zero..ctor(0f, this.offset);
			this.SFX_RUMRUN_P3_BallCop_Bounce();
		}
		if (base.transform.position.y < (float)Level.Current.Ground + this.offset && this.velocity.y < 0f)
		{
			this.velocity.y = Mathf.Abs(this.velocity.y);
			quaternion = new Quaternion?(Quaternion.Euler(0f, 0f, 180f));
			zero..ctor(0f, -this.offset);
			this.SFX_RUMRUN_P3_BallCop_Bounce();
		}
		if (flag && base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax - this.offset && this.velocity.x > 0f)
		{
			this.velocity.x = -Mathf.Abs(this.velocity.x);
			quaternion = new Quaternion?(Quaternion.Euler(0f, 0f, 270f));
			zero..ctor(this.offset, 0f);
			this.SFX_RUMRUN_P3_BallCop_Bounce();
		}
		if (flag && base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin + this.offset && this.velocity.x < 0f)
		{
			this.velocity.x = Mathf.Abs(this.velocity.x);
			quaternion = new Quaternion?(Quaternion.Euler(0f, 0f, 90f));
			zero..ctor(-this.offset, 0f);
			this.SFX_RUMRUN_P3_BallCop_Bounce();
		}
		if (quaternion != null)
		{
			Effect effect = this.dustEffect.Create(base.transform.position + zero);
			effect.transform.rotation = quaternion.Value;
			if (quaternion.Value == Quaternion.identity)
			{
				effect.transform.Find("Dirt").gameObject.SetActive(true);
				effect.animator.SetInteger("DirtEffect", Random.Range(0, 3));
			}
		}
	}

	// Token: 0x06002472 RID: 9330 RVA: 0x000C3FF4 File Offset: 0x000C21F4
	public IEnumerator shoot_cr()
	{
		int copBallBulletAngleStringMainIndex = Random.Range(0, this.properties.copBallBulletAngleString.Length);
		string[] copBallBulletAngleString = this.properties.copBallBulletAngleString[copBallBulletAngleStringMainIndex].Split(new char[]
		{
			','
		});
		int copBallBulletAngleStringIndex = Random.Range(0, copBallBulletAngleString.Length);
		int copBallBulletTypeStringMainIndex = Random.Range(0, this.properties.copBallBulletTypeString.Length);
		string[] copBallBulletTypeString = this.properties.copBallBulletTypeString[copBallBulletTypeStringMainIndex].Split(new char[]
		{
			','
		});
		int copBallBulletTypeStringIndex = Random.Range(0, copBallBulletTypeString.Length);
		yield return CupheadTime.WaitForSeconds(this, this.properties.copBallShootHesitate);
		for (;;)
		{
			BasicProjectile bullet;
			if (copBallBulletTypeString[copBallBulletTypeStringIndex][0] == 'P')
			{
				bullet = this.copBulletPink;
			}
			else
			{
				bullet = this.copBullet;
			}
			float angle = 0f;
			Parser.FloatTryParse(copBallBulletAngleString[copBallBulletAngleStringIndex], out angle);
			bullet.Create(base.transform.position, angle, this.properties.copBallBulletSpeed);
			yield return CupheadTime.WaitForSeconds(this, this.properties.copBallShootDelay);
			if (copBallBulletAngleStringIndex < copBallBulletAngleString.Length - 1)
			{
				copBallBulletAngleStringIndex++;
			}
			else
			{
				copBallBulletAngleStringMainIndex = (copBallBulletAngleStringMainIndex + 1) % this.properties.copBallBulletAngleString.Length;
				copBallBulletAngleStringIndex = 0;
			}
			if (copBallBulletTypeStringIndex < copBallBulletTypeString.Length - 1)
			{
				copBallBulletTypeStringIndex++;
			}
			else
			{
				copBallBulletTypeStringMainIndex = (copBallBulletTypeStringMainIndex + 1) % this.properties.copBallBulletTypeString.Length;
				copBallBulletTypeStringIndex = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002473 RID: 9331 RVA: 0x0001ECCE File Offset: 0x0001CECE
	public void Death(bool playAudio)
	{
		this.SFX_RUMRUN_P3_BallCop_Die(this.audioLoopNumber, playAudio);
		this.deathEffect.Create(base.transform.position);
		this.StopAllCoroutines();
		this.Recycle<RumRunnersLevelCopBall>();
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x000C4010 File Offset: 0x000C2210
	public IEnumerator checkToDie_cr()
	{
		while (base.transform.position.x >= -1140f && base.transform.position.x <= 1140f)
		{
			yield return null;
		}
		this.Death(false);
		yield break;
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x000C402C File Offset: 0x000C222C
	public IEnumerator gradualSpeed_cr()
	{
		float t = 0f;
		float time = this.properties.gradualSpeedTime;
		float val = 1f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.speed = this.properties.gradualSpeed.GetFloatAt(val - t / time);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x0001ED00 File Offset: 0x0001CF00
	public void SFX_RUMRUN_P3_BallCop_Bounce()
	{
		AudioManager.Play("sfx_dlc_rumrun_copball_bounce");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_copball_bounce");
	}

	// Token: 0x06002477 RID: 9335 RVA: 0x000C4048 File Offset: 0x000C2248
	public void SFX_RUMRUN_P3_BallCop_VocalShouts_Loop(int loopNumber)
	{
		string key = "sfx_dlc_rumrun_p3_ballcop_vocalshouts_loop_" + loopNumber;
		AudioManager.PlayLoop(key);
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_ballcop_vocalshouts_loop");
	}

	// Token: 0x06002478 RID: 9336 RVA: 0x000C407C File Offset: 0x000C227C
	public void SFX_RUMRUN_P3_BallCop_Die(int loopNumber, bool playAudio)
	{
		string key = "sfx_dlc_rumrun_p3_ballcop_vocalshouts_loop_" + loopNumber;
		AudioManager.Stop(key);
		if (playAudio)
		{
			AudioManager.Play("sfx_dlc_rumrun_copball_bounce");
			this.emitAudioFromObject.Add("sfx_dlc_rumrun_copball_bounce");
		}
	}

	// Token: 0x04001E1F RID: 7711
	public const float DIE_OFFSET_X = 500f;

	// Token: 0x04001E20 RID: 7712
	public static readonly int AudioLoopCount = 6;

	// Token: 0x04001E21 RID: 7713
	public static int CurrentAudioLoopIndex;

	// Token: 0x04001E22 RID: 7714
	public static int LastSortingIndex;

	// Token: 0x04001E23 RID: 7715
	[SerializeField]
	public BasicProjectile copBullet;

	// Token: 0x04001E24 RID: 7716
	[SerializeField]
	public BasicProjectile copBulletPink;

	// Token: 0x04001E25 RID: 7717
	[SerializeField]
	public Effect dustEffect;

	// Token: 0x04001E26 RID: 7718
	[SerializeField]
	public Effect deathEffect;

	// Token: 0x04001E27 RID: 7719
	public LevelProperties.RumRunners.CopBall properties;

	// Token: 0x04001E28 RID: 7720
	public Vector3 velocity;

	// Token: 0x04001E29 RID: 7721
	public DamageReceiver damageReceiver;

	// Token: 0x04001E2A RID: 7722
	[SerializeField]
	public CircleCollider2D circleCollider;

	// Token: 0x04001E2B RID: 7723
	public float health;

	// Token: 0x04001E2C RID: 7724
	public float speed;

	// Token: 0x04001E2D RID: 7725
	public float offset;

	// Token: 0x04001E2E RID: 7726
	public int audioLoopNumber;

	// Token: 0x04001E2F RID: 7727
	public bool launched;

	// Token: 0x04001E30 RID: 7728
	public Transform snoutPos;
}
