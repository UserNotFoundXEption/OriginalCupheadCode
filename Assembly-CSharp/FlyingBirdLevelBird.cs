using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200022B RID: 555
public class FlyingBirdLevelBird : LevelProperties.FlyingBird.Entity
{
	// Token: 0x17000290 RID: 656
	// (get) Token: 0x0600196A RID: 6506 RVA: 0x00015AED File Offset: 0x00013CED
	// (set) Token: 0x0600196B RID: 6507 RVA: 0x00015AF5 File Offset: 0x00013CF5
	public FlyingBirdLevelBird.State state { get; set; }

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x0600196C RID: 6508 RVA: 0x00015AFE File Offset: 0x00013CFE
	// (set) Token: 0x0600196D RID: 6509 RVA: 0x00015B06 File Offset: 0x00013D06
	public bool floating { get; set; }

	// Token: 0x0600196E RID: 6510 RVA: 0x000A6328 File Offset: 0x000A4528
	public override void Awake()
	{
		base.Awake();
		this.state = FlyingBirdLevelBird.State.Init;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.heart.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		this.heart.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.houseCollider = new GameObject("House Collider").transform;
		BoxCollider2D boxCollider2D = this.houseCollider.gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.isTrigger = true;
		boxCollider2D.offset = new Vector2(60f, -50f);
		boxCollider2D.size = new Vector2(400f, 300f);
		CollisionChild collisionChild = this.houseCollider.gameObject.AddComponent<CollisionChild>();
		collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x00015B0F File Offset: 0x00013D0F
	public void Start()
	{
		this.featherPrefab.CreatePool(150);
		this.heart.gameObject.SetActive(false);
		this.feathersFirstTime = true;
	}

	// Token: 0x06001970 RID: 6512 RVA: 0x00015B39 File Offset: 0x00013D39
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.damageReceiver != null)
		{
			this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		}
		this.featherPrefab = null;
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x000A6424 File Offset: 0x000A4624
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.state == FlyingBirdLevelBird.State.Dead || this.state == FlyingBirdLevelBird.State.Dying)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f)
		{
			this.BirdDie();
		}
	}

	// Token: 0x06001972 RID: 6514 RVA: 0x00015B70 File Offset: 0x00013D70
	public void Update()
	{
		if (this.houseCollider != null)
		{
			this.houseCollider.position = base.transform.position;
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x00015BAF File Offset: 0x00013DAF
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x000A6478 File Offset: 0x000A4678
	public override void LevelInit(LevelProperties.FlyingBird properties)
	{
		base.LevelInit(properties);
		properties.OnStateChange += this.OnStateChange;
		this.floating = false;
		base.StartCoroutine(this.float_cr());
		this.garbageIndex = Random.Range(0, properties.CurrentState.garbage.garbageTypeString.Length);
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x000A64D0 File Offset: 0x000A46D0
	public void OnStateChange()
	{
		if (base.properties.CurrentState.stateName == LevelProperties.FlyingBird.States.Whistle)
		{
			base.StartCoroutine(this.whistle_cr());
			if (this.patternCoroutine != null)
			{
				base.StopCoroutine(this.patternCoroutine);
			}
			this.patternCoroutine = base.StartCoroutine(this.whistle_cr());
		}
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x000A652C File Offset: 0x000A472C
	public void IntroContinue()
	{
		Animator component = base.GetComponent<Animator>();
		component.SetTrigger("Continue");
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x00015BD8 File Offset: 0x00013DD8
	public void SfxIntroA()
	{
		AudioManager.Play("level_flying_bird_intro_a");
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x00015BE4 File Offset: 0x00013DE4
	public void SfxIntroB()
	{
		AudioManager.Play("level_flying_bird_intro_b");
	}

	// Token: 0x06001979 RID: 6521 RVA: 0x00015BF0 File Offset: 0x00013DF0
	public void OnIntroAnimComplete()
	{
		this.introEnded = true;
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x000A6558 File Offset: 0x000A4758
	public IEnumerator intro_cr()
	{
		while (!this.introEnded)
		{
			yield return null;
		}
		this.floating = true;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.floating.attackInitialDelayRange.RandomFloat());
		this.state = FlyingBirdLevelBird.State.Idle;
		yield break;
	}

	// Token: 0x0600197B RID: 6523 RVA: 0x00015BF9 File Offset: 0x00013DF9
	public void IdleNoBlink()
	{
		this.blinks++;
		if (this.blinks >= this.maxBlinks)
		{
			base.animator.SetBool("Blink", true);
		}
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x00015C2B File Offset: 0x00013E2B
	public void Blink()
	{
		this.blinks = 0;
		this.maxBlinks = Random.Range(2, 5);
		base.animator.SetBool("Blink", false);
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x000A6574 File Offset: 0x000A4774
	public IEnumerator whistle_cr()
	{
		this.state = FlyingBirdLevelBird.State.Whistle;
		this.floating = false;
		Animator animator = base.GetComponent<Animator>();
		animator.Play("Whistle");
		AudioManager.Play("level_flying_bird_whistle");
		yield return animator.WaitForAnimationToEnd(this, "Whistle", false, true);
		this.state = FlyingBirdLevelBird.State.Idle;
		this.floating = true;
		yield break;
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x000A6590 File Offset: 0x000A4790
	public IEnumerator float_cr()
	{
		bool goUp = Rand.Bool();
		for (;;)
		{
			if (goUp)
			{
				yield return base.StartCoroutine(this.floatTo_cr(base.properties.CurrentState.floating.top, base.properties.CurrentState.floating.time));
			}
			else
			{
				yield return base.StartCoroutine(this.floatTo_cr(base.properties.CurrentState.floating.bottom, base.properties.CurrentState.floating.time));
			}
			goUp = !goUp;
		}
		yield break;
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x000A65AC File Offset: 0x000A47AC
	public IEnumerator floatTo_cr(float end, float time)
	{
		float t = 0f;
		float start = base.transform.position.y;
		while (t < time)
		{
			if (!this.floating)
			{
				while (!this.floating)
				{
					yield return null;
				}
			}
			float val = t / time;
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(null, new float?(end), null);
		yield break;
	}

	// Token: 0x06001980 RID: 6528 RVA: 0x00015C52 File Offset: 0x00013E52
	public void StartFeathers()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.feathers_cr());
	}

	// Token: 0x06001981 RID: 6529 RVA: 0x000A65D8 File Offset: 0x000A47D8
	public void FireFeathers(int count, float offset, bool parryable)
	{
		parryable = false;
		for (int i = 0; i < count; i++)
		{
			float num = 360f * ((float)i / (float)count);
			this.featherPrefab.Spawn(base.transform.position, Quaternion.Euler(new Vector3(0f, 0f, offset + num - 180f))).Init(base.properties.CurrentState.feathers.speed).SetParryable(parryable);
		}
	}

	// Token: 0x06001982 RID: 6530 RVA: 0x000A665C File Offset: 0x000A485C
	public IEnumerator feathers_cr()
	{
		this.state = FlyingBirdLevelBird.State.Feathers;
		this.floating = false;
		Animator animator = base.GetComponent<Animator>();
		animator.Play("Feathers_Start");
		AudioManager.Play("level_flyingbird_feathers_start");
		this.emitAudioFromObject.Add("level_flyingbird_feathers_start");
		yield return animator.WaitForAnimationToEnd(this, "Feathers_Start", false, true);
		LevelProperties.FlyingBird.Feathers featherProperties = base.properties.CurrentState.feathers;
		KeyValue[] pattern = KeyValue.ListFromString(featherProperties.pattern[Random.Range(0, featherProperties.pattern.Length)], new char[]
		{
			'P',
			'D'
		});
		AudioManager.PlayLoop("level_flyingbird_feathers_loop");
		this.emitAudioFromObject.Add("level_flyingbird_feathers_loop");
		for (int i = 0; i < pattern.Length; i++)
		{
			float offset = 0f;
			bool parryable = false;
			if (pattern[i].key == "P")
			{
				int p = 0;
				while ((float)p < pattern[i].value)
				{
					this.FireFeathers(featherProperties.count, offset, parryable);
					parryable = !parryable;
					offset += featherProperties.offset;
					yield return CupheadTime.WaitForSeconds(this, this.feathersFirstTime ? featherProperties.initalShotDelay : featherProperties.shotDelay);
					this.feathersFirstTime = false;
					p++;
				}
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, pattern[i].value);
			}
			yield return null;
		}
		AudioManager.Stop("level_flyingbird_feathers_loop");
		this.floating = true;
		animator.Play("Feathers_End");
		AudioManager.Play("level_flyingbird_feathers_hesitate");
		this.emitAudioFromObject.Add("level_flyingbird_feathers_hesitate");
		yield return CupheadTime.WaitForSeconds(this, featherProperties.hesitate);
		animator.Play("Feathers_Hesitate_End");
		AudioManager.Stop("level_flyingbird_feathers_hesitate");
		yield return animator.WaitForAnimationToEnd(this, "Feathers_Hesitate_End", false, true);
		this.state = FlyingBirdLevelBird.State.Idle;
		yield break;
	}

	// Token: 0x06001983 RID: 6531 RVA: 0x00015C7D File Offset: 0x00013E7D
	public void StartEggs()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.eggs_cr());
	}

	// Token: 0x06001984 RID: 6532 RVA: 0x00015CA8 File Offset: 0x00013EA8
	public void FireEgg()
	{
		this.eggPrefab.Create(base.properties.CurrentState.feathers.speed, this.eggRoot.position);
	}

	// Token: 0x06001985 RID: 6533 RVA: 0x00015CDB File Offset: 0x00013EDB
	public void SoundFireEggThroaty()
	{
		AudioManager.Play("level_flying_bird_spit_throaty");
		this.emitAudioFromObject.Add("level_flying_bird_spit_throaty");
	}

	// Token: 0x06001986 RID: 6534 RVA: 0x00015CF7 File Offset: 0x00013EF7
	public void SoundFireEggProjectile()
	{
		AudioManager.Play("level_flying_bird_spit");
		this.emitAudioFromObject.Add("level_flying_bird_spit");
	}

	// Token: 0x06001987 RID: 6535 RVA: 0x000A6678 File Offset: 0x000A4878
	public IEnumerator eggs_cr()
	{
		this.floating = true;
		this.state = FlyingBirdLevelBird.State.Eggs;
		Animator animator = base.GetComponent<Animator>();
		LevelProperties.FlyingBird.Eggs eggProperties = base.properties.CurrentState.eggs;
		KeyValue[] pattern = KeyValue.ListFromString(eggProperties.pattern[Random.Range(0, eggProperties.pattern.Length)], new char[]
		{
			'P',
			'D'
		});
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i].key == "P")
			{
				int p = 0;
				while ((float)p < pattern[i].value)
				{
					yield return CupheadTime.WaitForSeconds(this, eggProperties.shotDelay);
					animator.Play("Spit");
					p++;
				}
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, pattern[i].value);
			}
			yield return null;
		}
		yield return animator.WaitForAnimationToEnd(this, "Spit", false, true);
		yield return CupheadTime.WaitForSeconds(this, eggProperties.hesitate);
		this.state = FlyingBirdLevelBird.State.Idle;
		yield break;
	}

	// Token: 0x06001988 RID: 6536 RVA: 0x00015D13 File Offset: 0x00013F13
	public void StartLasers()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.lasers_cr());
	}

	// Token: 0x06001989 RID: 6537 RVA: 0x000A6694 File Offset: 0x000A4894
	public void FireLasers()
	{
		AudioManager.Play("level_flyingbird_bird_laser_fire");
		this.emitAudioFromObject.Add("level_flyingbird_bird_laser_fire");
		this.laserEffect.Create(this.laserRoots[0].position);
		foreach (Transform transform in this.laserRoots)
		{
			this.laserPrefab.Create(transform.position, -transform.eulerAngles.z, -base.properties.CurrentState.lasers.speed);
		}
	}

	// Token: 0x0600198A RID: 6538 RVA: 0x00015D3E File Offset: 0x00013F3E
	public void LasersAnimEnded()
	{
		this.state = FlyingBirdLevelBird.State.LasersEnding;
	}

	// Token: 0x0600198B RID: 6539 RVA: 0x000A6730 File Offset: 0x000A4930
	public IEnumerator lasers_cr()
	{
		Animator animator = base.GetComponent<Animator>();
		this.state = FlyingBirdLevelBird.State.Lasers;
		this.floating = false;
		LevelProperties.FlyingBird.Lasers properties = base.properties.CurrentState.lasers;
		animator.SetTrigger("StartLasers");
		while (this.state == FlyingBirdLevelBird.State.Lasers)
		{
			yield return null;
		}
		this.floating = true;
		yield return CupheadTime.WaitForSeconds(this, properties.hesitate);
		this.state = FlyingBirdLevelBird.State.Idle;
		yield break;
	}

	// Token: 0x0600198C RID: 6540 RVA: 0x00015D47 File Offset: 0x00013F47
	public void LasersSFX()
	{
		AudioManager.Play("level_flyingbird_bird_lasers");
		this.emitAudioFromObject.Add("level_flyingbird_bird_lasers");
	}

	// Token: 0x0600198D RID: 6541 RVA: 0x000A674C File Offset: 0x000A494C
	public void BirdFall()
	{
		this.state = FlyingBirdLevelBird.State.Dying;
		this.houseCollider.gameObject.SetActive(false);
		AudioManager.Stop("level_flyingbird_feathers_loop");
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		base.GetComponent<CircleCollider2D>().enabled = false;
		this.StopAllCoroutines();
		base.animator.Play("Death");
		base.StartCoroutine(this.die_cr());
		this.nurses.Die();
	}

	// Token: 0x0600198E RID: 6542 RVA: 0x000A67C0 File Offset: 0x000A49C0
	public void BirdDie()
	{
		this.nurses.Die();
		this.nurses.nurses[0].gameObject.SetActive(false);
		this.nurses.nurses[1].gameObject.SetActive(false);
		base.GetComponent<Collider2D>().enabled = false;
		base.StopCoroutine(this.checkHeart_cr());
		base.StopCoroutine(this.garbage_cr());
		this.nurses.animator.SetTrigger("Die");
		base.animator.Play("Stretcher_Death");
		AudioManager.PlayLoop("level_flyingbird_stretcher_death");
		this.emitAudioFromObject.Add("level_flyingbird_stretcher_death");
		foreach (BoxCollider2D boxCollider2D in base.GetComponentsInChildren<BoxCollider2D>())
		{
			boxCollider2D.enabled = false;
		}
		foreach (CircleCollider2D circleCollider2D in base.GetComponentsInChildren<CircleCollider2D>())
		{
			circleCollider2D.enabled = false;
		}
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x00015D63 File Offset: 0x00013F63
	public void OnDeathComplete()
	{
		this.StopAllCoroutines();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x00015D77 File Offset: 0x00013F77
	public void DeathSfx()
	{
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x00015D79 File Offset: 0x00013F79
	public void OnDeathExploded()
	{
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		this.smallBird.StartPattern(base.transform.position);
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x000A68C0 File Offset: 0x000A4AC0
	public IEnumerator die_cr()
	{
		Animator animator = base.GetComponent<Animator>();
		while (base.transform.position.y > 100f || base.transform.position.y < 0f)
		{
			yield return null;
		}
		this.floating = false;
		animator.Play("Death");
		this.deathEffectFront.Create(this.deathEffectsRoot.position);
		this.deathEffectBack.Create(this.deathEffectsRoot.position);
		yield break;
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x000A68DC File Offset: 0x000A4ADC
	public void OnBossRevival()
	{
		this.state = FlyingBirdLevelBird.State.Reviving;
		this.houseCollider.gameObject.SetActive(true);
		Object.Destroy(this.deathParts);
		base.gameObject.SetActive(true);
		base.animator.Play("Revived");
		this.nurses.animator.SetTrigger("StartNurses");
		base.GetComponent<CircleCollider2D>().enabled = true;
		base.GetComponent<HitFlash>().StopAllCoroutines();
		base.GetComponent<HitFlash>().SetColor(0f);
		base.transform.SetPosition(new float?((float)Level.Current.Right + 250f), new float?((float)(Level.Current.Ground - 150)), null);
		base.StartCoroutine(this.revival_cr());
		this.heart.InitHeart(base.properties);
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x000A69C4 File Offset: 0x000A4BC4
	public IEnumerator revival_cr()
	{
		float end = (float)Level.Current.Ground + 250f;
		yield return base.StartCoroutine(this.move_to_position_cr(base.transform.position.y, end, base.properties.CurrentState.floating.time, EaseUtils.EaseType.easeInOutSine));
		this.state = FlyingBirdLevelBird.State.Revived;
		this.nurses.InitNurse(base.properties.CurrentState.nurses);
		yield break;
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x000A69E0 File Offset: 0x000A4BE0
	public IEnumerator move_to_position_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		base.transform.SetPosition(null, new float?(start), null);
		float startX = base.transform.position.x;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetPosition(new float?(EaseUtils.Ease(ease, startX, 0f, val)), new float?(EaseUtils.Ease(ease, start, end, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(new float?(0f), new float?(end), null);
		yield return null;
		base.StartCoroutine(this.stretcherMove_cr());
		yield break;
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x000A6A18 File Offset: 0x000A4C18
	public IEnumerator stretcherMove_cr()
	{
		bool movingRight = Rand.Bool();
		float time = base.properties.CurrentState.bigBird.speedXTime;
		float end = 0f;
		do
		{
			if (this.state != FlyingBirdLevelBird.State.Heart)
			{
				float t = 0f;
				float start = base.transform.position.x;
				if (movingRight)
				{
					end = 290f;
				}
				else
				{
					end = -240f;
				}
				while (t < time)
				{
					if (this.state != FlyingBirdLevelBird.State.Heart)
					{
						float value = t / time;
						base.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, value)), null, null);
						t += CupheadTime.Delta;
					}
					yield return null;
				}
				base.transform.SetPosition(new float?(end), null, null);
				movingRight = !movingRight;
			}
			yield return null;
		}
		while (base.properties.CurrentHealth > 0f);
		yield break;
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x00015DA1 File Offset: 0x00013FA1
	public void StartGarbageOne()
	{
		base.StartCoroutine(this.garbage_cr());
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x000A6A34 File Offset: 0x000A4C34
	public IEnumerator garbage_cr()
	{
		this.state = FlyingBirdLevelBird.State.Garbage;
		base.animator.SetBool("OnGarbage", true);
		yield return base.animator.WaitForAnimationToStart(this, "Garbage_Start", false);
		AudioManager.Play("level_flyingbird_stretcher_garbage_start");
		this.emitAudioFromObject.Add("level_flyingbird_stretcher_garbage_start");
		float garbageSpeed = base.properties.CurrentState.garbage.speedX;
		float garbageCounter = 0f;
		GameObject chosenPrefab = null;
		yield return base.animator.WaitForAnimationToEnd(this, "Garbage_Start", false, true);
		int maxShotIndex = Random.Range(0, base.properties.CurrentState.garbage.shotCount.Split(new char[]
		{
			','
		}).Length);
		int maxShot = Parser.IntParse(base.properties.CurrentState.garbage.shotCount.Split(new char[]
		{
			','
		})[maxShotIndex]);
		while (garbageCounter < (float)maxShot)
		{
			string[] garbageTypes = base.properties.CurrentState.garbage.garbageTypeString[this.garbageIndex].Split(new char[]
			{
				','
			});
			if (garbageTypes[this.typeIndex][0] == 'P')
			{
				chosenPrefab = this.bootPinkPrefab;
			}
			else if (garbageTypes[this.typeIndex][0] == 'B')
			{
				chosenPrefab = this.bootPrefab;
			}
			else if (garbageTypes[this.typeIndex][0] == 'F')
			{
				chosenPrefab = this.fishPrefab;
			}
			else if (garbageTypes[this.typeIndex][0] == 'A')
			{
				chosenPrefab = this.applePrefab;
			}
			else
			{
				Debug.LogError("Invalid garbage type string.", null);
			}
			yield return base.animator.WaitForAnimationToEnd(this, "Garbage", false, true);
			AudioManager.Play("level_flyingbird_stretcher_garbage");
			this.emitAudioFromObject.Add("level_flyingbird_stretcher_garbage");
			GameObject garbage = Object.Instantiate<GameObject>(chosenPrefab, this.garbageRoot.transform.position, Quaternion.identity);
			garbage.GetComponent<BasicProjectile>().Speed = 1f;
			garbage.transform.localScale = Vector3.one * base.properties.CurrentState.garbage.shotSize;
			base.StartCoroutine(this.multiShotGarbage_cr(garbageSpeed, garbage));
			garbageSpeed += base.properties.CurrentState.garbage.speedXIncreaser;
			garbageCounter += 1f;
			if (this.typeIndex < garbageTypes.Length - 1)
			{
				this.typeIndex++;
			}
			else
			{
				this.garbageIndex = (this.garbageIndex + 1) % base.properties.CurrentState.garbage.garbageTypeString.Length;
				this.typeIndex = 0;
			}
			if (garbageCounter < (float)maxShot)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.garbage.shotDelay);
			}
		}
		garbageCounter = 0f;
		base.animator.SetTrigger("Continue");
		base.animator.SetBool("OnGarbage", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.garbage.hesitate.RandomFloat());
		this.state = FlyingBirdLevelBird.State.Revived;
		yield break;
	}

	// Token: 0x06001999 RID: 6553 RVA: 0x000A6A50 File Offset: 0x000A4C50
	public IEnumerator multiShotGarbage_cr(float speedX, GameObject proj)
	{
		bool isFalling = false;
		float pct = 1f;
		Vector3 velocity = new Vector3(-speedX, base.properties.CurrentState.garbage.speedY);
		while (proj != null)
		{
			if (proj.transform.position.y > (float)Level.Current.Ground - 200f)
			{
				if (isFalling)
				{
					pct -= CupheadTime.Delta * 4f;
					if (pct < -1f)
					{
						pct = -1f;
					}
				}
				velocity.y = base.properties.CurrentState.garbage.speedY * pct;
				proj.transform.position += velocity * CupheadTime.FixedDelta;
				if (proj.transform.position.y >= base.properties.CurrentState.garbage.maxHeight)
				{
					isFalling = true;
				}
				yield return null;
			}
			yield return null;
		}
		Object.Destroy(proj);
		yield break;
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x00015DB0 File Offset: 0x00013FB0
	public void StartHeartAttack()
	{
		this.state = FlyingBirdLevelBird.State.HeartTrans;
		base.animator.SetBool("OnRegurgitate", true);
		AudioManager.Play("level_flyingbird_stretcher_regurgitate_start");
		this.emitAudioFromObject.Add("level_flyingbird_stretcher_regurgitate_start");
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x00015DE5 File Offset: 0x00013FE5
	public void OpenHeart()
	{
		this.state = FlyingBirdLevelBird.State.Heart;
		this.heart.StartHeartAttack();
		base.GetComponent<DamageReceiver>().enabled = false;
		this.heartSpitFX.SetActive(true);
		base.StartCoroutine(this.checkHeart_cr());
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x000A6A7C File Offset: 0x000A4C7C
	public IEnumerator checkHeart_cr()
	{
		while (this.heart.gameObject.activeSelf)
		{
			yield return null;
		}
		base.animator.SetBool("OnRegurgitate", false);
		AudioManager.Play("level_flyingbird_stretcher_regurgitate_end");
		this.emitAudioFromObject.Add("level_flyingbird_stretcher_regurgitate_end");
		base.GetComponent<DamageReceiver>().enabled = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Regurgitate_End", false, true);
		this.state = FlyingBirdLevelBird.State.HeartTrans;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.heart.hesitate.RandomFloat());
		this.heartSpitFX.SetActive(false);
		this.state = FlyingBirdLevelBird.State.Revived;
		yield break;
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x000A6A98 File Offset: 0x000A4C98
	public void NursesHeartHeight()
	{
		foreach (Transform transform in this.nurses.nurses)
		{
			transform.transform.localPosition = new Vector3(transform.transform.localPosition.x, transform.transform.localPosition.y + 8f);
		}
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x000A6B08 File Offset: 0x000A4D08
	public void NursesGarbageHeight()
	{
		foreach (Transform transform in this.nurses.nurses)
		{
			transform.transform.localPosition = new Vector3(transform.transform.localPosition.x, transform.transform.localPosition.y + 6f);
		}
	}

	// Token: 0x0600199F RID: 6559 RVA: 0x000A6B78 File Offset: 0x000A4D78
	public void NursesReset()
	{
		foreach (Transform transform in this.nurses.nurses)
		{
			transform.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x0400147D RID: 5245
	[SerializeField]
	public GameObject bootPrefab;

	// Token: 0x0400147E RID: 5246
	[SerializeField]
	public GameObject bootPinkPrefab;

	// Token: 0x0400147F RID: 5247
	[SerializeField]
	public GameObject fishPrefab;

	// Token: 0x04001480 RID: 5248
	[SerializeField]
	public GameObject applePrefab;

	// Token: 0x04001481 RID: 5249
	[Space(10f)]
	[SerializeField]
	public FlyingBirdLevelSmallBird smallBird;

	// Token: 0x04001482 RID: 5250
	[Space(10f)]
	[SerializeField]
	public FlyingBirdLevelBirdFeather featherPrefab;

	// Token: 0x04001483 RID: 5251
	[Space(10f)]
	[SerializeField]
	public Transform eggRoot;

	// Token: 0x04001484 RID: 5252
	[SerializeField]
	public FlyingBirdLevelBirdEgg eggPrefab;

	// Token: 0x04001485 RID: 5253
	[SerializeField]
	public GameObject deathParts;

	// Token: 0x04001486 RID: 5254
	[Space(10f)]
	[SerializeField]
	public Transform nurse1Root;

	// Token: 0x04001487 RID: 5255
	[SerializeField]
	public Transform nurse2Root;

	// Token: 0x04001488 RID: 5256
	[SerializeField]
	public Transform garbageRoot;

	// Token: 0x04001489 RID: 5257
	[SerializeField]
	public FlyingBirdLevelHeart heart;

	// Token: 0x0400148A RID: 5258
	[SerializeField]
	public GameObject heartSpitFX;

	// Token: 0x0400148B RID: 5259
	[SerializeField]
	public FlyingBirdLevelNurses nurses;

	// Token: 0x0400148C RID: 5260
	[SerializeField]
	public GameObject head;

	// Token: 0x0400148D RID: 5261
	public DamageDealer damageDealer;

	// Token: 0x0400148E RID: 5262
	public DamageReceiver damageReceiver;

	// Token: 0x0400148F RID: 5263
	public bool introEnded;

	// Token: 0x04001490 RID: 5264
	public bool feathersFirstTime;

	// Token: 0x04001491 RID: 5265
	public Transform houseCollider;

	// Token: 0x04001492 RID: 5266
	public Coroutine patternCoroutine;

	// Token: 0x04001493 RID: 5267
	public int garbageIndex;

	// Token: 0x04001494 RID: 5268
	public int typeIndex;

	// Token: 0x04001495 RID: 5269
	public int blinks;

	// Token: 0x04001496 RID: 5270
	public int maxBlinks = 6;

	// Token: 0x04001497 RID: 5271
	[Space(10f)]
	[SerializeField]
	public Transform[] laserRoots;

	// Token: 0x04001498 RID: 5272
	[SerializeField]
	public BasicProjectile laserPrefab;

	// Token: 0x04001499 RID: 5273
	[SerializeField]
	public Effect laserEffect;

	// Token: 0x0400149A RID: 5274
	[Space(10f)]
	[SerializeField]
	public Transform deathEffectsRoot;

	// Token: 0x0400149B RID: 5275
	[SerializeField]
	public Effect deathEffectFront;

	// Token: 0x0400149C RID: 5276
	[SerializeField]
	public Effect deathEffectBack;

	// Token: 0x02000C3A RID: 3130
	public enum State
	{
		// Token: 0x04005891 RID: 22673
		Init,
		// Token: 0x04005892 RID: 22674
		Idle,
		// Token: 0x04005893 RID: 22675
		Feathers,
		// Token: 0x04005894 RID: 22676
		Eggs,
		// Token: 0x04005895 RID: 22677
		Dying,
		// Token: 0x04005896 RID: 22678
		Dead,
		// Token: 0x04005897 RID: 22679
		Whistle,
		// Token: 0x04005898 RID: 22680
		Lasers,
		// Token: 0x04005899 RID: 22681
		LasersEnding,
		// Token: 0x0400589A RID: 22682
		Reviving,
		// Token: 0x0400589B RID: 22683
		Revived,
		// Token: 0x0400589C RID: 22684
		Garbage,
		// Token: 0x0400589D RID: 22685
		Heart,
		// Token: 0x0400589E RID: 22686
		HeartTrans
	}
}
