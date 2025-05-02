using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000208 RID: 520
public class DicePalaceRouletteLevelRoulette : LevelProperties.DicePalaceRoulette.Entity
{
	// Token: 0x17000289 RID: 649
	// (get) Token: 0x060017DA RID: 6106 RVA: 0x000145AC File Offset: 0x000127AC
	// (set) Token: 0x060017DB RID: 6107 RVA: 0x000145B4 File Offset: 0x000127B4
	public DicePalaceRouletteLevelRoulette.State state { get; set; }

	// Token: 0x060017DC RID: 6108 RVA: 0x000145BD File Offset: 0x000127BD
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x000145F3 File Offset: 0x000127F3
	public void Start()
	{
		this.state = DicePalaceRouletteLevelRoulette.State.Intro;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x00014609 File Offset: 0x00012809
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x00014621 File Offset: 0x00012821
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x000A2954 File Offset: 0x000A0B54
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != DicePalaceRouletteLevelRoulette.State.Death)
		{
			this.state = DicePalaceRouletteLevelRoulette.State.Death;
			this.StartDeath();
		}
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x000A29A0 File Offset: 0x000A0BA0
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		AudioManager.Play("dice_palace_roulette_intro");
		this.emitAudioFromObject.Add("dice_palace_roulette_intro");
		base.animator.Play("Roulette_Intro");
		this.state = DicePalaceRouletteLevelRoulette.State.Idle;
		yield break;
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x0001463F File Offset: 0x0001283F
	public virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x00014660 File Offset: 0x00012860
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.marble = null;
		this.marbleLaunch = null;
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x00014676 File Offset: 0x00012876
	public void StartTwirl()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.twirl_cr());
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x000A29BC File Offset: 0x000A0BBC
	public IEnumerator twirl_cr()
	{
		this.state = DicePalaceRouletteLevelRoulette.State.Twirl;
		base.animator.Play("Roulette_Travel");
		LevelProperties.DicePalaceRoulette.Twirl p = base.properties.CurrentState.twirl;
		string[] amountPattern = p.twirlAmount.GetRandom<string>().Split(new char[]
		{
			','
		});
		base.StartCoroutine(this.twirl_vary_speed_cr());
		float twirlAmount = 0f;
		float stopDist = 200f;
		Parser.FloatTryParse(amountPattern[this.index], out twirlAmount);
		Vector3 pos = base.transform.position;
		int i = 0;
		while ((float)i < twirlAmount)
		{
			if (this.onRight)
			{
				this.slowDown = false;
				float maxPoint = -630f;
				while (base.transform.position.x > maxPoint)
				{
					if (!this.stopTwirl)
					{
						float num = maxPoint - base.transform.position.x;
						num = Mathf.Abs(num);
						pos.x = Mathf.MoveTowards(base.transform.position.x, maxPoint, this.speed * CupheadTime.Delta * this.hitPauseCoefficient());
						if (num < stopDist)
						{
							this.slowDown = true;
						}
						base.transform.position = pos;
					}
					yield return null;
				}
				this.onRight = !this.onRight;
			}
			else
			{
				this.slowDown = false;
				float maxPoint2 = 490f;
				while (base.transform.position.x < maxPoint2)
				{
					if (!this.stopTwirl)
					{
						float num2 = maxPoint2 - base.transform.position.x;
						num2 = Mathf.Abs(num2);
						pos.x = Mathf.MoveTowards(base.transform.position.x, maxPoint2, this.speed * CupheadTime.Delta * this.hitPauseCoefficient());
						if (num2 < stopDist)
						{
							this.slowDown = true;
						}
						base.transform.position = pos;
					}
					yield return null;
				}
				this.onRight = !this.onRight;
			}
			i++;
		}
		twirlAmount = (twirlAmount + 1f) % (float)amountPattern.Length;
		base.StopCoroutine(this.twirl_vary_speed_cr());
		this.state = DicePalaceRouletteLevelRoulette.State.Idle;
		yield break;
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x000146A1 File Offset: 0x000128A1
	public void TwirlStop()
	{
		this.stopTwirl = true;
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x000146AA File Offset: 0x000128AA
	public void TwirlStart()
	{
		this.stopTwirl = false;
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x000A29D8 File Offset: 0x000A0BD8
	public IEnumerator twirl_vary_speed_cr()
	{
		LevelProperties.DicePalaceRoulette.Twirl p = base.properties.CurrentState.twirl;
		float incrementspeed = p.movementSpeed / 50f;
		for (;;)
		{
			if (this.slowDown)
			{
				if (this.speed <= 50f)
				{
					this.slowDown = false;
				}
				else
				{
					this.speed -= incrementspeed;
				}
			}
			else if (this.speed < p.movementSpeed)
			{
				this.speed += incrementspeed;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x000146B3 File Offset: 0x000128B3
	public void StartMarbleDrop()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.marble_drop_cr());
	}

	// Token: 0x060017EA RID: 6122 RVA: 0x000A29F4 File Offset: 0x000A0BF4
	public void SpawnMarble(float xOffset)
	{
		LevelProperties.DicePalaceRoulette.MarbleDrop marbleDrop = base.properties.CurrentState.marbleDrop;
		float rotation = Mathf.Atan2((float)Level.Current.Ground, 0f) * 57.29578f;
		Vector2 position = base.transform.position;
		position.y = 360f;
		position.x = ((!this.onRight) ? (640f - xOffset) : (-640f + xOffset));
		this.marble.Create(position, rotation, marbleDrop.marbleSpeed);
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x000A2A84 File Offset: 0x000A0C84
	public IEnumerator marble_drop_cr()
	{
		LevelProperties.DicePalaceRoulette.MarbleDrop p = base.properties.CurrentState.marbleDrop;
		string[] spawnPattern = p.marblePositionStrings.GetRandom<string>().Split(new char[]
		{
			','
		});
		float waitTime = 0f;
		this.state = DicePalaceRouletteLevelRoulette.State.Marble;
		this.firstLaunch = true;
		this.stopMarbles = false;
		base.animator.Play("Roulette_Attack_Start");
		AudioManager.Play("dice_palace_roulette_attack_start");
		this.emitAudioFromObject.Add("dice_palace_roulette_attack_start");
		yield return base.animator.WaitForAnimationToStart(this, "Roulette_Attack_Loop", false);
		AudioManager.PlayLoop("dice_palace_roulette_attack_loop");
		this.emitAudioFromObject.Add("dice_palace_roulette_attack_loop");
		base.StartCoroutine(this.marble_sound_cr());
		yield return CupheadTime.WaitForSeconds(this, p.marbleInitalDelay);
		for (int i = 0; i < spawnPattern.Length; i++)
		{
			if (spawnPattern[i][0] == 'D')
			{
				Parser.FloatTryParse(spawnPattern[i].Substring(1), out waitTime);
				yield return CupheadTime.WaitForSeconds(this, waitTime);
			}
			else
			{
				string[] array = spawnPattern[i].Split(new char[]
				{
					'-'
				});
				foreach (string s in array)
				{
					float xOffset = 0f;
					Parser.FloatTryParse(s, out xOffset);
					this.SpawnMarble(xOffset);
				}
			}
			i %= spawnPattern.Length;
			yield return CupheadTime.WaitForSeconds(this, p.marbleDelay);
		}
		this.stopMarbles = true;
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		base.animator.SetTrigger("Continue");
		AudioManager.Stop("dice_palace_roulette_attack_loop");
		AudioManager.Play("dice_palace_roulette_attack_end");
		this.emitAudioFromObject.Add("dice_palace_roulette_attack_end");
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.state = DicePalaceRouletteLevelRoulette.State.Idle;
		yield break;
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x000A2AA0 File Offset: 0x000A0CA0
	public void SpawnMarbleAnimation()
	{
		if (this.stopMarbles)
		{
			return;
		}
		DicePalaceRouletteLevelMarblesLaunch dicePalaceRouletteLevelMarblesLaunch = Object.Instantiate<DicePalaceRouletteLevelMarblesLaunch>(this.marbleLaunch, this.marbleRoot, false);
		dicePalaceRouletteLevelMarblesLaunch.IsFirstTime = this.firstLaunch;
		this.firstLaunch = false;
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x000A2AE0 File Offset: 0x000A0CE0
	public IEnumerator marble_sound_cr()
	{
		AudioManager.Play("dice_palace_roulette_balls_start");
		this.emitAudioFromObject.Add("dice_palace_roulette_balls_start");
		AudioManager.PlayLoop("dice_palace_roulette_balls_shoot_loop");
		this.emitAudioFromObject.Add("dice_palace_roulette_balls_shoot_loop");
		while (!this.stopMarbles)
		{
			yield return null;
		}
		AudioManager.Stop("dice_palace_roulette_balls_shoot_loop");
		AudioManager.Play("dice_palace_roulette_balls_end");
		this.emitAudioFromObject.Add("dice_palace_roulette_balls_end");
		yield break;
	}

	// Token: 0x060017EE RID: 6126 RVA: 0x000146DE File Offset: 0x000128DE
	public void StartDeath()
	{
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Roulette_Death");
		AudioManager.Play("dice_palace_roulette_death");
		this.emitAudioFromObject.Add("dice_palace_roulette_death");
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x0001471C File Offset: 0x0001291C
	public void TravelSFX()
	{
		AudioManager.Play("dice_palace_roulette_travel");
		this.emitAudioFromObject.Add("dice_palace_roulette_travel");
	}

	// Token: 0x04001357 RID: 4951
	[SerializeField]
	public BasicProjectile marble;

	// Token: 0x04001358 RID: 4952
	[SerializeField]
	public DicePalaceRouletteLevelMarblesLaunch marbleLaunch;

	// Token: 0x04001359 RID: 4953
	[SerializeField]
	public Transform marbleRoot;

	// Token: 0x0400135A RID: 4954
	public bool onRight = true;

	// Token: 0x0400135B RID: 4955
	public bool slowDown;

	// Token: 0x0400135C RID: 4956
	public bool stopTwirl;

	// Token: 0x0400135D RID: 4957
	public bool firstLaunch = true;

	// Token: 0x0400135E RID: 4958
	public bool stopMarbles;

	// Token: 0x0400135F RID: 4959
	public int index;

	// Token: 0x04001360 RID: 4960
	public float speed;

	// Token: 0x04001361 RID: 4961
	public DamageDealer damageDealer;

	// Token: 0x04001362 RID: 4962
	public DamageReceiver damageReceiver;

	// Token: 0x04001363 RID: 4963
	public Coroutine patternCoroutine;

	// Token: 0x02000BDF RID: 3039
	public enum State
	{
		// Token: 0x04005687 RID: 22151
		Intro,
		// Token: 0x04005688 RID: 22152
		Idle,
		// Token: 0x04005689 RID: 22153
		Twirl,
		// Token: 0x0400568A RID: 22154
		Marble,
		// Token: 0x0400568B RID: 22155
		Death
	}
}
