using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000161 RID: 353
public class BeeLevelAirplane : LevelProperties.Bee.Entity
{
	// Token: 0x1700023C RID: 572
	// (get) Token: 0x060010EA RID: 4330 RVA: 0x0000E3E7 File Offset: 0x0000C5E7
	// (set) Token: 0x060010EB RID: 4331 RVA: 0x0000E3EF File Offset: 0x0000C5EF
	public BeeLevelAirplane.State state { get; set; }

	// Token: 0x060010EC RID: 4332 RVA: 0x000915DC File Offset: 0x0008F7DC
	public override void Awake()
	{
		base.Awake();
		this.state = BeeLevelAirplane.State.Unspawned;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.midLayer.GetComponentInChildren<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		this.topLayer.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x00091660 File Offset: 0x0008F860
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != BeeLevelAirplane.State.Dead)
		{
			this.state = BeeLevelAirplane.State.Dead;
			this.Dead();
		}
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x0000E3F8 File Offset: 0x0000C5F8
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x0000E416 File Offset: 0x0000C616
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060010F0 RID: 4336 RVA: 0x0000E42E File Offset: 0x0000C62E
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.bullet = null;
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x0000E43D File Offset: 0x0000C63D
	public void StartIntro()
	{
		this.state = BeeLevelAirplane.State.Intro;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x000916AC File Offset: 0x0008F8AC
	public IEnumerator intro_cr()
	{
		float speed = 400f;
		this.countPattern = base.properties.CurrentState.wingSwipe.attackCount.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.countIndex = Random.Range(0, this.countPattern.Length);
		while (base.transform.position.y < -60f)
		{
			base.transform.AddPosition(0f, speed * CupheadTime.Delta, 0f);
			yield return null;
		}
		this.state = BeeLevelAirplane.State.Idle;
		base.StartCoroutine(this.move_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x0000E453 File Offset: 0x0000C653
	public void IdleCount()
	{
		this.blinkCount++;
		if (this.blinkCount >= this.blinkCountMax)
		{
			this.blinkOne = Rand.Bool();
		}
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x000916C8 File Offset: 0x0008F8C8
	public void Blink_One()
	{
		if (this.blinkCount >= this.blinkCountMax && this.blinkOne)
		{
			this.topLayer.enabled = true;
			this.blinkCount = 0;
			this.blinkCountMax = Random.Range(3, 7);
		}
		else
		{
			this.topLayer.enabled = false;
		}
	}

	// Token: 0x060010F5 RID: 4341 RVA: 0x00091724 File Offset: 0x0008F924
	public void Blink_Two()
	{
		if (this.blinkCount >= this.blinkCountMax && !this.blinkOne)
		{
			this.topLayer.enabled = true;
			this.blinkCount = 0;
			this.blinkCountMax = Random.Range(3, 7);
		}
		else
		{
			this.topLayer.enabled = false;
		}
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x00091780 File Offset: 0x0008F980
	public IEnumerator move_cr()
	{
		bool isLooping = false;
		LevelProperties.Bee.General p = base.properties.CurrentState.general;
		this.movingRight = false;
		this.isMoving = true;
		this.offset = p.movementOffset;
		this.speed = p.movementSpeed;
		for (;;)
		{
			if (this.isMoving)
			{
				if (this.movingRight)
				{
					while (base.transform.position.x < 640f - this.offset && this.movingRight)
					{
						base.transform.AddPosition(this.speed * CupheadTime.Delta * this.hitPauseCoefficient(), 0f, 0f);
						yield return null;
					}
					if (this.state != BeeLevelAirplane.State.Wing)
					{
						this.movingRight = !this.movingRight;
					}
				}
				else
				{
					while (base.transform.position.x > -640f + this.offset && !this.movingRight)
					{
						base.transform.AddPosition(-this.speed * CupheadTime.Delta * this.hitPauseCoefficient(), 0f, 0f);
						yield return null;
					}
					if (this.state != BeeLevelAirplane.State.Wing)
					{
						this.movingRight = !this.movingRight;
					}
				}
				if (!isLooping)
				{
					AudioManager.PlayLoop("bee_airplane_idle_loop");
					this.emitAudioFromObject.Add("bee_airplane_idle_loop");
					isLooping = true;
				}
			}
			else if (isLooping)
			{
				AudioManager.Stop("bee_airplane_idle_loop");
				isLooping = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060010F7 RID: 4343 RVA: 0x0000E47F File Offset: 0x0000C67F
	public float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x060010F8 RID: 4344 RVA: 0x0000E4A0 File Offset: 0x0000C6A0
	public void StartTurbine()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.turbine_cr());
	}

	// Token: 0x060010F9 RID: 4345 RVA: 0x0009179C File Offset: 0x0008F99C
	public IEnumerator turbine_cr()
	{
		this.state = BeeLevelAirplane.State.Turbine;
		LevelProperties.Bee.TurbineBlasters p = base.properties.CurrentState.turbineBlasters;
		string[] bulletPattern = p.attackDirectionString.GetRandom<string>().Split(new char[]
		{
			','
		});
		for (int i = 0; i < bulletPattern.Length; i++)
		{
			if (bulletPattern[i][0] == 'R')
			{
				base.animator.Play("Right_Pylon");
			}
			else if (bulletPattern[i][0] == 'L')
			{
				base.animator.Play("Left_Pylon");
			}
			else if (bulletPattern[i][0] == 'B')
			{
				base.animator.Play("Right_Pylon");
				base.animator.Play("Left_Pylon");
			}
			yield return CupheadTime.WaitForSeconds(this, p.repeatDealy);
		}
		yield return CupheadTime.WaitForSeconds(this, p.hesitateRange.RandomFloat());
		this.state = BeeLevelAirplane.State.Idle;
		yield break;
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x000917B8 File Offset: 0x0008F9B8
	public void ShootBulletRight()
	{
		AudioManager.Play("bee_airplane_pylon");
		this.emitAudioFromObject.Add("bee_airplane_pylon");
		Vector3 vector = new Vector3(0f, 360f, 0f) - new Vector3(0f, base.transform.position.y, 0f);
		float rotation = MathUtils.DirectionToAngle(vector);
		this.bullet.Create(this.rightShootRoot.transform.position, rotation, true, base.properties.CurrentState.turbineBlasters);
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x0009185C File Offset: 0x0008FA5C
	public void ShootBulletLeft()
	{
		AudioManager.Play("bee_airplane_pylon");
		this.emitAudioFromObject.Add("bee_airplane_pylon");
		Vector3 vector = new Vector3(0f, 360f, 0f) - new Vector3(0f, base.transform.position.y, 0f);
		float rotation = MathUtils.DirectionToAngle(vector);
		this.bullet.Create(this.leftShootRoot.transform.position, rotation, false, base.properties.CurrentState.turbineBlasters);
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x0000E4CB File Offset: 0x0000C6CB
	public void StartWing()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.wing_cr());
	}

	// Token: 0x060010FD RID: 4349 RVA: 0x00091900 File Offset: 0x0008FB00
	public IEnumerator wing_cr()
	{
		this.state = BeeLevelAirplane.State.Wing;
		LevelProperties.Bee.WingSwipe p = base.properties.CurrentState.wingSwipe;
		AbstractPlayerController player = PlayerManager.GetNext();
		this.attackOnRight = (player.transform.position.x >= 0f);
		Vector3 startPos = Vector3.zero;
		int count = 0;
		Parser.IntTryParse(this.countPattern[this.countIndex], out count);
		for (int i = 0; i < count; i++)
		{
			base.animator.SetTrigger("OnSaw");
			yield return base.animator.WaitForAnimationToEnd(this, "Saw_Start", false, true);
			this.movingRight = this.attackOnRight;
			this.offset = p.warningMaxDistance;
			this.speed = p.warningMovementSpeed;
			if (this.attackOnRight)
			{
				while (base.transform.position.x < 640f - p.warningMaxDistance)
				{
					yield return null;
				}
			}
			else
			{
				while (base.transform.position.x > -640f + p.warningMaxDistance)
				{
					yield return null;
				}
			}
			this.isMoving = false;
			yield return CupheadTime.WaitForSeconds(this, p.warningDuration);
			base.animator.SetTrigger("Continue");
			this.isMoving = true;
			this.offset = p.maxDistance;
			this.movingRight = !this.movingRight;
			this.speed = p.movementSpeed;
			if (!this.attackOnRight)
			{
				while (base.transform.position.x < 640f - p.maxDistance)
				{
					yield return null;
				}
			}
			else
			{
				while (base.transform.position.x > -640f + p.maxDistance)
				{
					yield return null;
				}
			}
			this.isMoving = false;
			yield return CupheadTime.WaitForSeconds(this, p.attackDuration);
			base.animator.SetTrigger("End");
			this.isMoving = true;
			this.offset = base.properties.CurrentState.general.movementOffset;
			this.attackOnRight = !this.attackOnRight;
			this.speed = base.properties.CurrentState.general.movementSpeed;
		}
		this.state = BeeLevelAirplane.State.EndWing;
		this.countIndex = (this.countIndex + 1) % this.countPattern.Length;
		yield return CupheadTime.WaitForSeconds(this, p.hesitateRange.RandomFloat());
		this.state = BeeLevelAirplane.State.Idle;
		yield break;
	}

	// Token: 0x060010FE RID: 4350 RVA: 0x0000E4F6 File Offset: 0x0000C6F6
	public void SawLoopSFX()
	{
		this.SawStartSFX();
		AudioManager.PlayLoop("bee_airplane_saw_loop");
		this.emitAudioFromObject.Add("bee_airplane_saw_loop");
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x0000E518 File Offset: 0x0000C718
	public void SawStartSFX()
	{
		AudioManager.Play("bee_airplane_saw_start");
		this.emitAudioFromObject.Add("bee_airplane_saw_start");
	}

	// Token: 0x06001100 RID: 4352 RVA: 0x0000E534 File Offset: 0x0000C734
	public void SawEndSFX()
	{
		AudioManager.Stop("bee_airplane_saw_loop");
		AudioManager.Play("bee_airplane_saw_end");
		this.emitAudioFromObject.Add("bee_airplane_saw_end");
	}

	// Token: 0x06001101 RID: 4353 RVA: 0x0000E55A File Offset: 0x0000C75A
	public void DeathHeadSFX()
	{
		AudioManager.Play("bee_airplane_death_head");
		this.emitAudioFromObject.Add("bee_airplane_death_head");
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x0000E576 File Offset: 0x0000C776
	public void SawContinueSFX()
	{
		if (!this.isPreSFXPlaying)
		{
			AudioManager.Play("bee_airplane_saw_continue");
			this.emitAudioFromObject.Add("bee_airplane_saw_continue");
			this.isPreSFXPlaying = true;
		}
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x0000E5A4 File Offset: 0x0000C7A4
	public void SawContinueSFXEnd()
	{
		this.isPreSFXPlaying = false;
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x0009191C File Offset: 0x0008FB1C
	public void Flip()
	{
		if (this.attackOnRight)
		{
			base.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		}
		else
		{
			base.transform.SetScale(new float?(-1f), new float?(1f), new float?(1f));
		}
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x0000E5AD File Offset: 0x0000C7AD
	public void Dead()
	{
		this.StopAllCoroutines();
		AudioManager.Stop("bee_airplane_saw_loop");
		AudioManager.Play("bee_airplane_death");
		this.emitAudioFromObject.Add("bee_airplane_death");
		base.animator.SetTrigger("Death");
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x0000E5E9 File Offset: 0x0000C7E9
	public void DeadHead()
	{
		base.animator.Play("Death_Head");
	}

	// Token: 0x04000DBE RID: 3518
	[SerializeField]
	public Transform rightShootRoot;

	// Token: 0x04000DBF RID: 3519
	[SerializeField]
	public Transform leftShootRoot;

	// Token: 0x04000DC0 RID: 3520
	[SerializeField]
	public BeeLevelTurbineBullet bullet;

	// Token: 0x04000DC1 RID: 3521
	[SerializeField]
	public SpriteRenderer topLayer;

	// Token: 0x04000DC2 RID: 3522
	[SerializeField]
	public SpriteRenderer midLayer;

	// Token: 0x04000DC4 RID: 3524
	public string[] countPattern;

	// Token: 0x04000DC5 RID: 3525
	public int countIndex;

	// Token: 0x04000DC6 RID: 3526
	public int blinkCount;

	// Token: 0x04000DC7 RID: 3527
	public int blinkCountMax;

	// Token: 0x04000DC8 RID: 3528
	public bool blinkOne;

	// Token: 0x04000DC9 RID: 3529
	public bool attackOnRight;

	// Token: 0x04000DCA RID: 3530
	public bool movingRight;

	// Token: 0x04000DCB RID: 3531
	public bool isMoving;

	// Token: 0x04000DCC RID: 3532
	public bool isPreSFXPlaying;

	// Token: 0x04000DCD RID: 3533
	public float offset;

	// Token: 0x04000DCE RID: 3534
	public float speed;

	// Token: 0x04000DCF RID: 3535
	public DamageDealer damageDealer;

	// Token: 0x04000DD0 RID: 3536
	public DamageReceiver damageReceiver;

	// Token: 0x04000DD1 RID: 3537
	public Coroutine patternCoroutine;

	// Token: 0x02000A62 RID: 2658
	public enum State
	{
		// Token: 0x04004C62 RID: 19554
		Unspawned,
		// Token: 0x04004C63 RID: 19555
		Intro,
		// Token: 0x04004C64 RID: 19556
		Idle,
		// Token: 0x04004C65 RID: 19557
		Wing,
		// Token: 0x04004C66 RID: 19558
		EndWing,
		// Token: 0x04004C67 RID: 19559
		Turbine,
		// Token: 0x04004C68 RID: 19560
		Dead
	}
}
