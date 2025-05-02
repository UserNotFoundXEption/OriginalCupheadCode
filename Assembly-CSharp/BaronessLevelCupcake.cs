using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200014D RID: 333
public class BaronessLevelCupcake : BaronessLevelMiniBossBase
{
	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x0000D7E4 File Offset: 0x0000B9E4
	// (set) Token: 0x06000FE3 RID: 4067 RVA: 0x0000D7EC File Offset: 0x0000B9EC
	public BaronessLevelCupcake.State state { get; set; }

	// Token: 0x06000FE4 RID: 4068 RVA: 0x0008E7BC File Offset: 0x0008C9BC
	public override void Awake()
	{
		base.Awake();
		this.isGoingDown = false;
		this.isGoingRight = false;
		this.xSpeed = this.changeXSpeed;
		this.patternIndex = 0;
		this.fadeTime = 0.3f;
		this.damageDealer = DamageDealer.NewEnemy();
		this.collisionChild.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.collisionChild.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x06000FE5 RID: 4069 RVA: 0x0000D7F5 File Offset: 0x0000B9F5
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000FE6 RID: 4070 RVA: 0x0008E840 File Offset: 0x0008CA40
	public void Init(LevelProperties.Baroness.Cupcake properties, Vector2 pos, float health)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.health = health;
		this.state = BaronessLevelCupcake.State.Moving;
		base.StartCoroutine(this.select_x_speed_cr());
		base.StartCoroutine(this.moving_cr());
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x0008E890 File Offset: 0x0008CA90
	public override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health > 0f)
		{
			base.OnDamageTaken(info);
		}
		this.health -= info.damage;
		if (this.health < 0f && this.state != BaronessLevelCupcake.State.Dying)
		{
			DamageDealer.DamageInfo info2 = new DamageDealer.DamageInfo(this.health, info.direction, info.origin, info.damageSource);
			base.OnDamageTaken(info2);
			this.state = BaronessLevelCupcake.State.Dying;
			this.StartDeath();
		}
	}

	// Token: 0x06000FE8 RID: 4072 RVA: 0x0000D80D File Offset: 0x0000BA0D
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000FE9 RID: 4073 RVA: 0x0000D82B File Offset: 0x0000BA2B
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.splashPrefab = null;
	}

	// Token: 0x06000FEA RID: 4074 RVA: 0x0000D83A File Offset: 0x0000BA3A
	public override float hitPauseCoefficient()
	{
		return (!this.collisionChild.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06000FEB RID: 4075 RVA: 0x0000D860 File Offset: 0x0000BA60
	public void SetLaunchOffset()
	{
		base.transform.position = this.launchOffset.transform.position;
	}

	// Token: 0x06000FEC RID: 4076 RVA: 0x0008E918 File Offset: 0x0008CB18
	public IEnumerator moving_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		bool curlAni = false;
		bool flatAni = false;
		for (;;)
		{
			if (!this.isGoingDown)
			{
				if (flatAni)
				{
					base.StartCoroutine(this.select_x_speed_cr());
					yield return CupheadTime.WaitForSeconds(this, this.properties.hold);
					base.animator.SetTrigger("Continue");
					yield return base.animator.WaitForAnimationToEnd(this, "Slam_Start", false, true);
					flatAni = false;
				}
				this.GoingUp();
				curlAni = true;
			}
			else
			{
				if (curlAni)
				{
					yield return base.animator.WaitForAnimationToEnd(this, "Slam_Curl", false, true);
					curlAni = false;
				}
				this.GoingDown();
				flatAni = true;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x0008E934 File Offset: 0x0008CB34
	public void GoingUp()
	{
		if (base.transform.position.y < 360f - this.offset)
		{
			Vector3 position = base.transform.position;
			if (!this.isGoingRight)
			{
				position.x -= this.changeXSpeed * CupheadTime.FixedDelta * this.hitPauseCoefficient();
			}
			else
			{
				position.x += this.changeXSpeed * CupheadTime.FixedDelta * this.hitPauseCoefficient();
			}
			position.y += this.ySpeedUp * CupheadTime.FixedDelta;
			base.transform.position = position;
			this.BoundaryCheck();
		}
		else
		{
			this.isGoingDown = true;
		}
	}

	// Token: 0x06000FEE RID: 4078 RVA: 0x0008E9FC File Offset: 0x0008CBFC
	public void GoingDown()
	{
		if (base.transform.position.y > (float)Level.Current.Ground + 120f)
		{
			if (this.xSpeed == 0f)
			{
				this.xSpeed = this.changeXSpeed;
			}
			Vector3 position = base.transform.position;
			position.y -= this.ySpeedDown * CupheadTime.FixedDelta * this.hitPauseCoefficient();
			base.transform.position = position;
		}
		else
		{
			Vector3 position2 = base.transform.position;
			position2.y = (float)Level.Current.Ground + 120f;
			base.transform.position = position2;
			this.isGoingDown = false;
		}
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x0008EAC4 File Offset: 0x0008CCC4
	public void BoundaryCheck()
	{
		if (base.transform.position.x < -540f && !this.isGoingRight)
		{
			this.xSpeed = 0f;
			base.transform.SetScale(new float?(-1f), new float?(1f), new float?(1f));
			this.isGoingRight = true;
		}
		else if (base.transform.position.x > 540f && this.isGoingRight)
		{
			this.xSpeed = 0f;
			base.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
			this.isGoingRight = false;
		}
		else
		{
			this.xSpeed = this.changeXSpeed;
		}
	}

	// Token: 0x06000FF0 RID: 4080 RVA: 0x0008EBB0 File Offset: 0x0008CDB0
	public IEnumerator select_x_speed_cr()
	{
		string[] pattern = this.properties.XSpeedString[0].Split(new char[]
		{
			','
		});
		Parser.FloatTryParse(pattern[this.patternIndex], out this.changeXSpeed);
		if (this.patternIndex < pattern.Length - 1)
		{
			this.patternIndex++;
		}
		else
		{
			this.patternIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000FF1 RID: 4081 RVA: 0x0000D87D File Offset: 0x0000BA7D
	public void FireBullets()
	{
		if (this.properties.projectileOn)
		{
			this.StartSplashes();
		}
	}

	// Token: 0x06000FF2 RID: 4082 RVA: 0x0008EBCC File Offset: 0x0008CDCC
	public void StartSplashes()
	{
		base.StartCoroutine(this.splash_cr(true, this.deathRoot.transform.position.x));
		base.StartCoroutine(this.splash_cr(false, this.deathRoot.transform.position.x));
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x0008EC28 File Offset: 0x0008CE28
	public IEnumerator splash_cr(bool onLeft, float posX)
	{
		float originalOffset = (!onLeft) ? (-this.properties.splashOriginalOffset) : this.properties.splashOriginalOffset;
		float offset = (!onLeft) ? (-this.properties.splashOffset) : this.properties.splashOffset;
		float delay = 0.4f;
		int value = 0;
		for (int i = 0; i < 3; i++)
		{
			if (onLeft)
			{
				value = i;
			}
			else if (i == 0)
			{
				value = 2;
			}
			else if (i == 1)
			{
				value = 0;
			}
			else
			{
				value = 1;
			}
			Effect splash = this.splashPrefab.Create(new Vector2(posX + originalOffset + offset * (float)i, (float)Level.Current.Ground));
			float scale = (!onLeft) ? splash.transform.localScale.x : (-splash.transform.localScale.x);
			splash.animator.SetInteger("SplashType", value);
			splash.transform.SetScale(new float?(scale), null, null);
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		yield break;
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x0000D895 File Offset: 0x0000BA95
	public void StartDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x06000FF5 RID: 4085 RVA: 0x0008EC54 File Offset: 0x0008CE54
	public IEnumerator death_cr()
	{
		this.StartExplosions();
		this.collisionChild.GetComponent<Collider2D>().enabled = false;
		base.transform.position = this.deathRoot.transform.position;
		this.isDying = true;
		base.animator.SetTrigger("Death");
		yield return base.animator.WaitForAnimationToEnd(this, "Cupcake_Death", false, true);
		this.EndExplosions();
		this.Die();
		yield break;
	}

	// Token: 0x06000FF6 RID: 4086 RVA: 0x0000D8AA File Offset: 0x0000BAAA
	public void SoundCupcakeJump()
	{
		AudioManager.Play("level_baroness_cupcake_jump");
		this.emitAudioFromObject.Add("level_baroness_cupcake_jump");
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x0000D8C6 File Offset: 0x0000BAC6
	public void SoundCupcakeLand()
	{
		AudioManager.Play("level_baroness_cupcake_land");
		this.emitAudioFromObject.Add("level_baroness_cupcake_land");
	}

	// Token: 0x06000FF8 RID: 4088 RVA: 0x0000D8E2 File Offset: 0x0000BAE2
	public void SoundCupcakeSpin()
	{
		AudioManager.Play("level_baroness_cupcake_spin");
		this.emitAudioFromObject.Add("level_baroness_cupcake_spin");
	}

	// Token: 0x04000CEC RID: 3308
	public LevelProperties.Baroness.Cupcake properties;

	// Token: 0x04000CED RID: 3309
	public DamageDealer damageDealer;

	// Token: 0x04000CEE RID: 3310
	[SerializeField]
	public Effect splashPrefab;

	// Token: 0x04000CEF RID: 3311
	[SerializeField]
	public Transform launchOffset;

	// Token: 0x04000CF0 RID: 3312
	[SerializeField]
	public BasicProjectile cupcakeProjectile;

	// Token: 0x04000CF1 RID: 3313
	[SerializeField]
	public Transform collisionChild;

	// Token: 0x04000CF2 RID: 3314
	[SerializeField]
	public Transform deathRoot;

	// Token: 0x04000CF3 RID: 3315
	public float health;

	// Token: 0x04000CF4 RID: 3316
	public float ySpeedUp = 1800f;

	// Token: 0x04000CF5 RID: 3317
	public float ySpeedDown = 2500f;

	// Token: 0x04000CF6 RID: 3318
	public float xSpeed;

	// Token: 0x04000CF7 RID: 3319
	public float changeXSpeed;

	// Token: 0x04000CF8 RID: 3320
	public float offset = 250f;

	// Token: 0x04000CF9 RID: 3321
	public bool isGoingDown;

	// Token: 0x04000CFA RID: 3322
	public bool isGoingRight;

	// Token: 0x04000CFB RID: 3323
	public int patternIndex;

	// Token: 0x04000CFC RID: 3324
	public int mainPatternIndex;

	// Token: 0x02000A16 RID: 2582
	public enum State
	{
		// Token: 0x04004AAC RID: 19116
		Moving,
		// Token: 0x04004AAD RID: 19117
		Dying
	}
}
