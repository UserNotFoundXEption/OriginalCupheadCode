using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200028B RID: 651
public class FlyingMermaidLevelMermaid : LevelProperties.FlyingMermaid.Entity
{
	// Token: 0x06001D75 RID: 7541 RVA: 0x00018F9F File Offset: 0x0001719F
	public FlyingMermaidLevelMermaid()
	{
		FlyingMermaidLevelMermaid.FishPossibility[] array = new FlyingMermaidLevelMermaid.FishPossibility[2];
		array[0] = FlyingMermaidLevelMermaid.FishPossibility.Homer;
		this.fishPattern = array;
		this.maxBlinks = 3;
		base..ctor();
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06001D76 RID: 7542 RVA: 0x00018FD2 File Offset: 0x000171D2
	// (set) Token: 0x06001D77 RID: 7543 RVA: 0x00018FDA File Offset: 0x000171DA
	public FlyingMermaidLevelMermaid.State state { get; set; }

	// Token: 0x06001D78 RID: 7544 RVA: 0x000B099C File Offset: 0x000AEB9C
	public override void Awake()
	{
		base.Awake();
		this.summonPattern.Shuffle<FlyingMermaidLevelMermaid.SummonPossibility>();
		this.fishPattern.Shuffle<FlyingMermaidLevelMermaid.FishPossibility>();
		base.StartCoroutine(this.intro_cr());
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		CollisionChild collisionChild = this.blockingColliders.gameObject.AddComponent<CollisionChild>();
		collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x06001D79 RID: 7545 RVA: 0x00018FE3 File Offset: 0x000171E3
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001D7A RID: 7546 RVA: 0x000B0A24 File Offset: 0x000AEC24
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (!this.stopMoving)
		{
			if (this.introEnded && !this.transformationStarting)
			{
				float num = Mathf.Max(PlayerManager.GetNext().center.x, PlayerManager.GetNext().center.x);
				if (num > base.transform.position.x)
				{
					this.Position(true);
				}
				else
				{
					this.Position(false);
				}
			}
			else if (this.transformationStarting)
			{
				this.Position(false);
			}
		}
	}

	// Token: 0x06001D7B RID: 7547 RVA: 0x00018FF6 File Offset: 0x000171F6
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x000B0AD8 File Offset: 0x000AECD8
	public override void LevelInit(LevelProperties.FlyingMermaid properties)
	{
		base.LevelInit(properties);
		this.initFishPatternIndices();
		this.spreadFishPinkPattern = properties.CurrentState.spreadshotFish.spreadshotPinkString.Split(new char[]
		{
			','
		});
		this.spreadFishPinkIndex = Random.Range(0, this.spreadFishPinkPattern.Length);
	}

	// Token: 0x06001D7D RID: 7549 RVA: 0x000B0B2C File Offset: 0x000AED2C
	public void Position(bool closeGap)
	{
		this.walkDuration = (float)((!this.transformationStarting) ? 4 : 2);
		if (closeGap)
		{
			float x = this.walkingPositions[0].position.x;
			float x2 = this.walkingPositions[1].position.x;
			this.Move(x, x2, this.walkDuration, 1);
		}
		else
		{
			float x = this.walkingPositions[1].position.x;
			float x2 = this.walkingPositions[0].position.x;
			this.Move(x, x2, this.walkDuration, -1);
		}
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x000B0BD8 File Offset: 0x000AEDD8
	public void Move(float startPosition, float endPosition, float duration, int direction)
	{
		this.walkTime += CupheadTime.Delta * (float)direction;
		if (direction < 0)
		{
			if (this.walkTime <= 0f)
			{
				this.walkTime = 0f;
			}
		}
		else if (this.walkTime >= duration)
		{
			this.walkTime = duration;
		}
		this.walkPCT = this.walkTime / duration;
		if (this.walkPCT >= 1f)
		{
			this.walkPCT = 1f;
		}
		if (direction < 0)
		{
			this.walkPCT = 1f - this.walkPCT;
		}
		base.transform.SetPosition(new float?(startPosition + (endPosition - startPosition) * this.walkPCT), null, null);
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x00019014 File Offset: 0x00017214
	public void PlayIntroSound()
	{
		AudioManager.Play("level_mermaid_intro");
		this.emitAudioFromObject.Add("level_mermaid_intro");
	}

	// Token: 0x06001D80 RID: 7552 RVA: 0x000B0CAC File Offset: 0x000AEEAC
	public IEnumerator intro_cr()
	{
		float t = 0f;
		base.transform.SetPosition(null, new float?(this.startUnderwaterY), null);
		yield return CupheadTime.WaitForSeconds(this, this.introRiseTime * 0.5f);
		base.StartCoroutine(this.spawn_splash_cr());
		while (t < this.introRiseTime * 0.5f)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(null, new float?(Mathf.Lerp(this.startUnderwaterY, this.regularY, t / (this.introRiseTime * 0.5f))), null);
			yield return null;
		}
		base.transform.SetPosition(null, new float?(this.regularY), null);
		while (!this.introEnded)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.state = FlyingMermaidLevelMermaid.State.Idle;
		yield break;
	}

	// Token: 0x06001D81 RID: 7553 RVA: 0x000B0CC8 File Offset: 0x000AEEC8
	public IEnumerator spawn_splash_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.35f);
		FlyingMermaidLevelSplashManager.Instance.SpawnMegaSplashMedium(base.gameObject, -50f, true, -200f);
		yield return null;
		yield break;
	}

	// Token: 0x06001D82 RID: 7554 RVA: 0x000B0CE4 File Offset: 0x000AEEE4
	public void IntroContinue()
	{
		Animator component = base.GetComponent<Animator>();
		component.SetTrigger("Continue");
		this.state = FlyingMermaidLevelMermaid.State.Intro;
	}

	// Token: 0x06001D83 RID: 7555 RVA: 0x00019030 File Offset: 0x00017230
	public void OnIntroAnimComplete()
	{
		this.introEnded = true;
	}

	// Token: 0x06001D84 RID: 7556 RVA: 0x000B0D0C File Offset: 0x000AEF0C
	public void BlinkMaybe()
	{
		this.blinks++;
		if (this.blinks >= this.maxBlinks)
		{
			this.blinks = 0;
			this.maxBlinks = Random.Range(2, 5);
			this.blinkOverlaySprite.enabled = true;
		}
		else
		{
			this.blinkOverlaySprite.enabled = false;
		}
	}

	// Token: 0x06001D85 RID: 7557 RVA: 0x00019039 File Offset: 0x00017239
	public void StartYell()
	{
		this.state = FlyingMermaidLevelMermaid.State.Yell;
		base.StartCoroutine(this.yell_cr());
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x000B0D6C File Offset: 0x000AEF6C
	public IEnumerator yell_cr()
	{
		LevelProperties.FlyingMermaid.Yell p = base.properties.CurrentState.yell;
		string[] pattern = p.patternString.GetRandom<string>().Split(new char[]
		{
			','
		});
		base.animator.SetTrigger("StartYell");
		base.animator.SetBool("Repeat", true);
		yield return base.animator.WaitForAnimationToEnd(this, "Yell_Start", false, true);
		float waitTime = p.anticipateInitialHold;
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i][0] == 'D')
			{
				Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
			}
			else
			{
				int repeatTimes = 0;
				Parser.IntTryParse(pattern[i].Substring(1), out repeatTimes);
				for (int j = 0; j < repeatTimes; j++)
				{
					yield return CupheadTime.WaitForSeconds(this, waitTime);
					base.animator.SetTrigger("Continue");
					yield return base.animator.WaitForAnimationToEnd(this, "Yell_Anticipation_End", false, true);
					this.FireProjectiles();
					this.yellEffect.Create(this.yellFxRoot.position);
					yield return CupheadTime.WaitForSeconds(this, p.mouthHold);
					base.animator.SetTrigger("Continue");
					waitTime = p.anticipateHold;
					if (i < pattern.Length - 1 || j < repeatTimes - 1)
					{
						yield return base.animator.WaitForAnimationToEnd(this, "Yell_Back", false, true);
					}
				}
			}
		}
		base.animator.SetBool("Repeat", false);
		yield return base.animator.WaitForAnimationToEnd(this, "Yell_End", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttack);
		this.state = FlyingMermaidLevelMermaid.State.Idle;
		yield break;
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x000B0D88 File Offset: 0x000AEF88
	public void FireProjectiles()
	{
		LevelProperties.FlyingMermaid.Yell yell = base.properties.CurrentState.yell;
		AbstractPlayerController next = PlayerManager.GetNext();
		for (int i = 0; i < yell.numBullets; i++)
		{
			float floatAt = yell.spreadAngle.GetFloatAt((float)i / ((float)yell.numBullets - 1f));
			FlyingMermaidLevelYellProjectile flyingMermaidLevelYellProjectile = this.yellProjectilePrefab.Create(this.projectileRoot.position, yell.bulletSpeed, floatAt, next);
			flyingMermaidLevelYellProjectile.animator.SetInteger("Variant", i);
		}
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x0001904F File Offset: 0x0001724F
	public void StartSummon()
	{
		this.state = FlyingMermaidLevelMermaid.State.Summon;
		base.StartCoroutine(this.summon_cr());
	}

	// Token: 0x06001D89 RID: 7561 RVA: 0x000B0E18 File Offset: 0x000AF018
	public IEnumerator summon_cr()
	{
		LevelProperties.FlyingMermaid.Summon p = base.properties.CurrentState.summon;
		base.animator.SetBool("Summon", true);
		yield return base.animator.WaitForAnimationToEnd(this, "Summon_Start", false, true);
		AudioManager.Play("level_mermaid_summon_loop_start");
		yield return CupheadTime.WaitForSeconds(this, p.holdBeforeCreature);
		FlyingMermaidLevelMermaid.SummonPossibility summon = this.nextSummon();
		AudioManager.Play("level_mermaid_summon_loop");
		if (summon != FlyingMermaidLevelMermaid.SummonPossibility.Seahorse)
		{
			if (summon != FlyingMermaidLevelMermaid.SummonPossibility.Pufferfish)
			{
				if (summon == FlyingMermaidLevelMermaid.SummonPossibility.Turtle)
				{
					this.SummonTurtle();
				}
			}
			else
			{
				AudioManager.Play("level_mermaid_merdusa_puffer_fish_bubble_up");
				base.StartCoroutine(this.summonPufferFish_cr());
			}
		}
		else
		{
			this.SummonSeahorse();
		}
		yield return CupheadTime.WaitForSeconds(this, p.holdAfterCreature);
		AudioManager.Stop("level_mermaid_summon_loop");
		AudioManager.Play("level_mermaid_summon_loop_end");
		base.animator.SetBool("Summon", false);
		yield return base.animator.WaitForAnimationToEnd(this, "Summon_End", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttack);
		this.state = FlyingMermaidLevelMermaid.State.Idle;
		yield break;
	}

	// Token: 0x06001D8A RID: 7562 RVA: 0x00019065 File Offset: 0x00017265
	public FlyingMermaidLevelMermaid.SummonPossibility nextSummon()
	{
		this.summonIndex = (this.summonIndex + 1) % this.summonPattern.Length;
		return this.summonPattern[this.summonIndex];
	}

	// Token: 0x06001D8B RID: 7563 RVA: 0x000B0E34 File Offset: 0x000AF034
	public IEnumerator summonPufferFish_cr()
	{
		LevelProperties.FlyingMermaid.Pufferfish p = base.properties.CurrentState.pufferfish;
		string[] pattern = p.spawnString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int i = Random.Range(0, pattern.Length);
		float t = 0f;
		float waitTime = 0f;
		int spawnsUntilPinkPufferfish = p.pinkPufferSpawnRange.RandomInt();
		while (t < p.spawnDuration && !this.stopPufferfish)
		{
			if (pattern[i][0] == 'D')
			{
				Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
			}
			else
			{
				if (waitTime > 0f)
				{
					yield return CupheadTime.WaitForSeconds(this, waitTime);
					t += waitTime;
				}
				string[] spawnLocations = pattern[i].Split(new char[]
				{
					'-'
				});
				foreach (string s in spawnLocations)
				{
					float x = 0f;
					Parser.FloatTryParse(s, out x);
					spawnsUntilPinkPufferfish--;
					FlyingMermaidLevelPufferfish prefab;
					if (spawnsUntilPinkPufferfish == 0)
					{
						spawnsUntilPinkPufferfish = p.pinkPufferSpawnRange.RandomInt();
						prefab = this.pinkPufferfishPrefab;
					}
					else
					{
						prefab = this.pufferfishPrefabs[Random.Range(0, this.pufferfishPrefabs.Length)];
					}
					base.StartCoroutine(this.summon_pufferfish_cr(prefab, x));
				}
				waitTime = p.delay;
			}
			i = (i + 1) % pattern.Length;
		}
		yield break;
	}

	// Token: 0x06001D8C RID: 7564 RVA: 0x000B0E50 File Offset: 0x000AF050
	public void SummonSeahorse()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		FlyingMermaidLevelSeahorse flyingMermaidLevelSeahorse = Object.Instantiate<FlyingMermaidLevelSeahorse>(this.seahorsePrefab);
		Vector2 vector = flyingMermaidLevelSeahorse.transform.position;
		vector.x = next.transform.position.x;
		flyingMermaidLevelSeahorse.transform.position = vector;
		flyingMermaidLevelSeahorse.Init(base.properties.CurrentState.seahorse);
		GroundHomingMovement component = flyingMermaidLevelSeahorse.GetComponent<GroundHomingMovement>();
		component.TrackingPlayer = next;
	}

	// Token: 0x06001D8D RID: 7565 RVA: 0x000B0ED4 File Offset: 0x000AF0D4
	public void SummonTurtle()
	{
		FlyingMermaidLevelTurtle flyingMermaidLevelTurtle = Object.Instantiate<FlyingMermaidLevelTurtle>(this.turtlePrefab);
		Vector2 vector = flyingMermaidLevelTurtle.transform.position;
		vector.x = (float)Level.Current.Left + base.properties.CurrentState.turtle.appearPosition.RandomFloat();
		flyingMermaidLevelTurtle.transform.position = vector;
		flyingMermaidLevelTurtle.Init(base.properties.CurrentState.turtle);
	}

	// Token: 0x06001D8E RID: 7566 RVA: 0x000B0F54 File Offset: 0x000AF154
	public IEnumerator summon_pufferfish_cr(FlyingMermaidLevelPufferfish prefab, float x)
	{
		yield return CupheadTime.WaitForSeconds(this, Random.Range(0f, 0.15f));
		FlyingMermaidLevelPufferfish pufferfish = Object.Instantiate<FlyingMermaidLevelPufferfish>(prefab);
		Vector2 position = pufferfish.transform.position;
		position.x = x + (float)Level.Current.Left;
		pufferfish.transform.position = position;
		pufferfish.Init(base.properties.CurrentState.pufferfish);
		yield break;
	}

	// Token: 0x06001D8F RID: 7567 RVA: 0x0001908B File Offset: 0x0001728B
	public void StartFish()
	{
		base.StartCoroutine(this.fish_cr());
	}

	// Token: 0x06001D90 RID: 7568 RVA: 0x0001909A File Offset: 0x0001729A
	public void PlayMermaidTuckdownSound()
	{
		AudioManager.Play("level_mermaid_tuckdown_laugh");
		this.emitAudioFromObject.Add("level_mermaid_tuckdown_laugh");
	}

	// Token: 0x06001D91 RID: 7569 RVA: 0x000B0F80 File Offset: 0x000AF180
	public IEnumerator fish_cr()
	{
		this.state = FlyingMermaidLevelMermaid.State.Fish;
		base.animator.SetTrigger("StartFish");
		yield return base.animator.WaitForAnimationToEnd(this, "Tuckdown_Start", false, true);
		float t = 0f;
		FlyingMermaidLevelSplashManager.Instance.SpawnMegaSplashLarge(base.gameObject, 0f, false, 0f);
		while (t < this.tuckdownMoveTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(null, new float?(Mathf.Lerp(this.regularY, this.fishUnderwaterY, t / this.tuckdownMoveTime)), null);
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.tuckdownWaitTime);
		this.fish = this.nextFish();
		this.spreadshotFishSprite.enabled = (this.fish == FlyingMermaidLevelMermaid.FishPossibility.Spreadshot);
		this.spreadshotFishOverlaySprite.enabled = (this.fish == FlyingMermaidLevelMermaid.FishPossibility.Spreadshot);
		this.spinnerFishSprite.enabled = (this.fish == FlyingMermaidLevelMermaid.FishPossibility.Spinner);
		this.spinnerFishOverlaySprite.enabled = (this.fish == FlyingMermaidLevelMermaid.FishPossibility.Spinner);
		this.homerFishSprite.enabled = (this.fish == FlyingMermaidLevelMermaid.FishPossibility.Homer);
		this.homerFishOverlaySprite.enabled = (this.fish == FlyingMermaidLevelMermaid.FishPossibility.Homer);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Tuckdown_Loop", false, true);
		t = 0f;
		FlyingMermaidLevelSplashManager.Instance.SpawnMegaSplashLarge(base.gameObject, 50f, true, 0f);
		while (t < this.tuckdownRiseTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(null, new float?(Mathf.Lerp(this.fishUnderwaterY, this.regularY, t / this.tuckdownRiseTime)), null);
			yield return null;
		}
		base.animator.SetBool("Repeat", true);
		string[] pattern = this.nextFishPatternString().Split(new char[]
		{
			','
		});
		float waitTime = base.properties.CurrentState.fish.delayBeforeFirstAttack;
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i][0] == 'D')
			{
				Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, waitTime);
				base.animator.SetTrigger("Continue");
				yield return base.animator.WaitForAnimationToEnd(this, "Fish_Attack_Start", false, true);
				this.doFishAttack(pattern[i]);
				if (i < pattern.Length - 1)
				{
					yield return base.animator.WaitForAnimationToEnd(this, "Fish_Attack_Repeat", false, true);
					waitTime = this.waitTimeBetweenFishAttacks();
				}
			}
		}
		base.animator.SetBool("Repeat", false);
		yield return base.animator.WaitForAnimationToEnd(this, "Fish_Attack", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.fish.delayBeforeFly);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Fish_Launch", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.fish.hesitateAfterAttack);
		this.state = FlyingMermaidLevelMermaid.State.Idle;
		yield break;
	}

	// Token: 0x06001D92 RID: 7570 RVA: 0x000190B6 File Offset: 0x000172B6
	public FlyingMermaidLevelMermaid.FishPossibility nextFish()
	{
		this.fishIndex = (this.fishIndex + 1) % this.fishPattern.Length;
		return this.fishPattern[this.fishIndex];
	}

	// Token: 0x06001D93 RID: 7571 RVA: 0x000B0F9C File Offset: 0x000AF19C
	public void initFishPatternIndices()
	{
		this.spreadshotPatternIndex = Random.Range(0, base.properties.CurrentState.spreadshotFish.shootString.Length);
		this.spinnerPatternIndex = Random.Range(0, base.properties.CurrentState.spinnerFish.shootString.Length);
		this.homerPatternIndex = Random.Range(0, base.properties.CurrentState.homerFish.shootString.Length);
	}

	// Token: 0x06001D94 RID: 7572 RVA: 0x000B1014 File Offset: 0x000AF214
	public string nextFishPatternString()
	{
		switch (this.fish)
		{
		case FlyingMermaidLevelMermaid.FishPossibility.Spreadshot:
			this.spreadshotPatternIndex = (this.spreadshotPatternIndex + 1) % base.properties.CurrentState.spreadshotFish.shootString.Length;
			return base.properties.CurrentState.spreadshotFish.shootString[this.spreadshotPatternIndex];
		case FlyingMermaidLevelMermaid.FishPossibility.Spinner:
			this.spinnerPatternIndex = (this.spinnerPatternIndex + 1) % base.properties.CurrentState.spinnerFish.shootString.Length;
			return base.properties.CurrentState.spinnerFish.shootString[this.spinnerPatternIndex];
		case FlyingMermaidLevelMermaid.FishPossibility.Homer:
			this.homerPatternIndex = (this.homerPatternIndex + 1) % base.properties.CurrentState.homerFish.shootString.Length;
			return base.properties.CurrentState.homerFish.shootString[this.homerPatternIndex];
		default:
			return string.Empty;
		}
	}

	// Token: 0x06001D95 RID: 7573 RVA: 0x000B1110 File Offset: 0x000AF310
	public float waitTimeBetweenFishAttacks()
	{
		switch (this.fish)
		{
		case FlyingMermaidLevelMermaid.FishPossibility.Spreadshot:
			return base.properties.CurrentState.spreadshotFish.attackDelay;
		case FlyingMermaidLevelMermaid.FishPossibility.Spinner:
			return base.properties.CurrentState.spinnerFish.attackDelay;
		case FlyingMermaidLevelMermaid.FishPossibility.Homer:
			return base.properties.CurrentState.homerFish.attackDelay;
		default:
			return 0f;
		}
	}

	// Token: 0x06001D96 RID: 7574 RVA: 0x000B1184 File Offset: 0x000AF384
	public void doFishAttack(string attackString)
	{
		AudioManager.Play("level_mermaid_fish_attack");
		this.emitAudioFromObject.Add("level_mermaid_fish_attack");
		FlyingMermaidLevelMermaid.FishPossibility fishPossibility = this.fish;
		if (fishPossibility != FlyingMermaidLevelMermaid.FishPossibility.Spreadshot)
		{
			if (fishPossibility != FlyingMermaidLevelMermaid.FishPossibility.Spinner)
			{
				if (fishPossibility == FlyingMermaidLevelMermaid.FishPossibility.Homer)
				{
					this.fishHomer();
				}
			}
			else
			{
				this.fishSpinner();
			}
		}
		else
		{
			this.fishSpreadshot(attackString);
		}
	}

	// Token: 0x06001D97 RID: 7575 RVA: 0x000B11F0 File Offset: 0x000AF3F0
	public void fishSpreadshot(string attackString)
	{
		int num = 0;
		Parser.IntTryParse(attackString.Substring(1), out num);
		num--;
		string[] array = base.properties.CurrentState.spreadshotFish.spreadVariableGroups[num].Split(new char[]
		{
			','
		});
		float speed = 0f;
		int num2 = 0;
		MinMax minMax = new MinMax(0f, 0f);
		foreach (string text in array)
		{
			if (text[0] == 'S')
			{
				Parser.FloatTryParse(text.Substring(1), out speed);
			}
			else if (text[0] == 'N')
			{
				Parser.IntTryParse(text.Substring(1), out num2);
			}
			else
			{
				string[] array3 = text.Split(new char[]
				{
					'-'
				});
				Parser.FloatTryParse(array3[0], out minMax.min);
				Parser.FloatTryParse(array3[1], out minMax.max);
			}
		}
		for (int j = 0; j < num2; j++)
		{
			float floatAt = minMax.GetFloatAt((float)j / ((float)num2 - 1f));
			BasicProjectile basicProjectile = this.fishSpreadshotBulletPrefab.Create(this.fishProjectileRoot.position, floatAt, speed);
			basicProjectile.animator.SetInteger("Variant", j % 2);
			basicProjectile.SetParryable(this.spreadFishPinkPattern[this.spreadFishPinkIndex][0] == 'P');
			this.spreadFishPinkIndex = (this.spreadFishPinkIndex + 1) % this.spreadFishPinkPattern.Length;
		}
	}

	// Token: 0x06001D98 RID: 7576 RVA: 0x000B138C File Offset: 0x000AF58C
	public void fishSpinner()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector2 direction = next.transform.position - this.fishProjectileRoot.position;
		direction.Normalize();
		if (next.transform.position.x > this.fishProjectileRoot.transform.position.x)
		{
			direction = MathUtils.AngleToDirection(90f);
		}
		this.fishSpinnerBulletPrefab.Create(this.fishProjectileRoot.position, direction, base.properties.CurrentState.spinnerFish);
	}

	// Token: 0x06001D99 RID: 7577 RVA: 0x000B1430 File Offset: 0x000AF630
	public void fishHomer()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector2 direction = next.transform.position - this.fishProjectileRoot.position;
		float rotation = MathUtils.DirectionToAngle(direction) + Random.Range(-15f, 15f);
		LevelProperties.FlyingMermaid.HomerFish homerFish = base.properties.CurrentState.homerFish;
		if (next.transform.position.x > this.fishProjectileRoot.transform.position.x)
		{
			rotation = 90f;
		}
		this.fishHomerBulletPrefab.Create(this.fishProjectileRoot.position, rotation, next, homerFish);
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x000B14E0 File Offset: 0x000AF6E0
	public void LaunchFish()
	{
		FlyingMermaidLevelFish flyingMermaidLevelFish = null;
		FlyingMermaidLevelMermaid.FishPossibility fishPossibility = this.fish;
		if (fishPossibility != FlyingMermaidLevelMermaid.FishPossibility.Spreadshot)
		{
			if (fishPossibility != FlyingMermaidLevelMermaid.FishPossibility.Spinner)
			{
				if (fishPossibility == FlyingMermaidLevelMermaid.FishPossibility.Homer)
				{
					flyingMermaidLevelFish = this.homerFishPrefab;
				}
			}
			else
			{
				flyingMermaidLevelFish = this.spinnerFishPrefab;
			}
		}
		else
		{
			flyingMermaidLevelFish = this.spreadshotFishPrefab;
		}
		flyingMermaidLevelFish.Create(this.fishLaunchRoot.position, base.properties.CurrentState.fish);
	}

	// Token: 0x06001D9B RID: 7579 RVA: 0x000190DC File Offset: 0x000172DC
	public void OnFishSpitFx()
	{
		this.FishSpitEffectPrefab.Create(this.fishProjectileRoot.position);
	}

	// Token: 0x06001D9C RID: 7580 RVA: 0x000190F5 File Offset: 0x000172F5
	public void StartTransform()
	{
		this.transformationStarting = true;
		base.StartCoroutine(this.transform_cr());
	}

	// Token: 0x06001D9D RID: 7581 RVA: 0x000B155C File Offset: 0x000AF75C
	public IEnumerator transform_cr()
	{
		while (base.transform.position.x != this.walkingPositions[0].position.x)
		{
			yield return null;
		}
		this.stopMoving = true;
		float startX = base.transform.position.x;
		float t = 0f;
		while (t < this.transformMoveTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(new float?(Mathf.Lerp(startX, startX - this.transformMoveX, t / this.transformMoveTime)), null, null);
			yield return null;
		}
		base.animator.SetTrigger("Transform");
		if (this.state == FlyingMermaidLevelMermaid.State.Summon)
		{
			yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
			this.stopPufferfish = true;
		}
		if (this.state == FlyingMermaidLevelMermaid.State.Idle)
		{
			this.stopPufferfish = true;
		}
		this.state = FlyingMermaidLevelMermaid.State.Transform;
		yield return base.animator.WaitForAnimationToStart(this, "Transform", false);
		AudioManager.Play("level_mermaid_transform");
		((FlyingMermaidLevel)Level.Current).MerdusaTransformStarted = true;
		this.stopPufferfish = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Transform", false, true);
		t = 0f;
		while (t < this.eelSinkTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(null, new float?(Mathf.Lerp(this.regularY, this.eelUnderwaterY, t / this.eelSinkTime)), null);
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001D9E RID: 7582 RVA: 0x000B1578 File Offset: 0x000AF778
	public void DisableColliders()
	{
		Collider2D[] components = base.GetComponents<Collider2D>();
		foreach (Collider2D collider2D in components)
		{
			collider2D.enabled = false;
		}
		this.blockingColliders.gameObject.SetActive(false);
	}

	// Token: 0x06001D9F RID: 7583 RVA: 0x0001910B File Offset: 0x0001730B
	public void SpawnMerdusa()
	{
		this.merdusa.StartIntro(base.transform.position);
	}

	// Token: 0x06001DA0 RID: 7584 RVA: 0x00019128 File Offset: 0x00017328
	public void RightSplash()
	{
		this.splashRight.Create(this.splashRoot.transform.position);
	}

	// Token: 0x06001DA1 RID: 7585 RVA: 0x00019146 File Offset: 0x00017346
	public void LeftSplash()
	{
		this.splashLeft.Create(this.splashRoot.transform.position);
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x00019164 File Offset: 0x00017364
	public void SoundMermaidFishLaunch()
	{
		AudioManager.Play("level_mermaid_fish_launch");
		this.emitAudioFromObject.Add("level_mermaid_fish_launch");
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x00019180 File Offset: 0x00017380
	public void SoundMermaidAttackYellStart()
	{
		AudioManager.Play("level_mermaid_yell_start");
		this.emitAudioFromObject.Add("level_mermaid_yell_start");
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x0001919C File Offset: 0x0001739C
	public void SoundMermaidAttackYell()
	{
		AudioManager.Play("level_mermaid_yell_attack");
		this.emitAudioFromObject.Add("level_mermaid_yell_attack");
	}

	// Token: 0x0400180E RID: 6158
	[SerializeField]
	public Transform[] walkingPositions;

	// Token: 0x0400180F RID: 6159
	[SerializeField]
	public float introRiseTime;

	// Token: 0x04001810 RID: 6160
	[SerializeField]
	public float tuckdownMoveTime;

	// Token: 0x04001811 RID: 6161
	[SerializeField]
	public float tuckdownWaitTime;

	// Token: 0x04001812 RID: 6162
	[SerializeField]
	public float tuckdownRiseTime;

	// Token: 0x04001813 RID: 6163
	[SerializeField]
	public float regularY;

	// Token: 0x04001814 RID: 6164
	[SerializeField]
	public float startUnderwaterY;

	// Token: 0x04001815 RID: 6165
	[SerializeField]
	public float fishUnderwaterY;

	// Token: 0x04001816 RID: 6166
	[SerializeField]
	public float transformMoveTime;

	// Token: 0x04001817 RID: 6167
	[SerializeField]
	public float transformMoveX;

	// Token: 0x04001818 RID: 6168
	[SerializeField]
	public float eelSinkTime;

	// Token: 0x04001819 RID: 6169
	[SerializeField]
	public float eelUnderwaterY;

	// Token: 0x0400181A RID: 6170
	[SerializeField]
	public FlyingMermaidLevelYellProjectile yellProjectilePrefab;

	// Token: 0x0400181B RID: 6171
	[SerializeField]
	public FlyingMermaidLevelSeahorse seahorsePrefab;

	// Token: 0x0400181C RID: 6172
	[SerializeField]
	public Effect FishSpitEffectPrefab;

	// Token: 0x0400181D RID: 6173
	public bool introEnded;

	// Token: 0x0400181E RID: 6174
	public DamageDealer damageDealer;

	// Token: 0x0400181F RID: 6175
	public DamageReceiver damageReceiver;

	// Token: 0x04001820 RID: 6176
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04001821 RID: 6177
	[SerializeField]
	public Transform yellFxRoot;

	// Token: 0x04001822 RID: 6178
	[SerializeField]
	public FlyingMermaidLevelPufferfish[] pufferfishPrefabs;

	// Token: 0x04001823 RID: 6179
	[SerializeField]
	public FlyingMermaidLevelPufferfish pinkPufferfishPrefab;

	// Token: 0x04001824 RID: 6180
	[SerializeField]
	public FlyingMermaidLevelTurtle turtlePrefab;

	// Token: 0x04001825 RID: 6181
	[SerializeField]
	public SpriteRenderer blinkOverlaySprite;

	// Token: 0x04001826 RID: 6182
	[SerializeField]
	public SpriteRenderer spreadshotFishSprite;

	// Token: 0x04001827 RID: 6183
	[SerializeField]
	public SpriteRenderer spinnerFishSprite;

	// Token: 0x04001828 RID: 6184
	[SerializeField]
	public SpriteRenderer homerFishSprite;

	// Token: 0x04001829 RID: 6185
	[SerializeField]
	public SpriteRenderer spreadshotFishOverlaySprite;

	// Token: 0x0400182A RID: 6186
	[SerializeField]
	public SpriteRenderer spinnerFishOverlaySprite;

	// Token: 0x0400182B RID: 6187
	[SerializeField]
	public SpriteRenderer homerFishOverlaySprite;

	// Token: 0x0400182C RID: 6188
	[SerializeField]
	public FlyingMermaidLevelFish spreadshotFishPrefab;

	// Token: 0x0400182D RID: 6189
	[SerializeField]
	public FlyingMermaidLevelFish spinnerFishPrefab;

	// Token: 0x0400182E RID: 6190
	[SerializeField]
	public FlyingMermaidLevelFish homerFishPrefab;

	// Token: 0x0400182F RID: 6191
	[SerializeField]
	public BasicProjectile fishSpreadshotBulletPrefab;

	// Token: 0x04001830 RID: 6192
	[SerializeField]
	public FlyingMermaidLevelFishSpinner fishSpinnerBulletPrefab;

	// Token: 0x04001831 RID: 6193
	[SerializeField]
	public FlyingMermaidLevelHomingProjectile fishHomerBulletPrefab;

	// Token: 0x04001832 RID: 6194
	[SerializeField]
	public Transform fishLaunchRoot;

	// Token: 0x04001833 RID: 6195
	[SerializeField]
	public Transform fishProjectileRoot;

	// Token: 0x04001834 RID: 6196
	[SerializeField]
	public FlyingMermaidLevelMerdusa merdusa;

	// Token: 0x04001835 RID: 6197
	[SerializeField]
	public Transform blockingColliders;

	// Token: 0x04001836 RID: 6198
	[SerializeField]
	public Effect splashRight;

	// Token: 0x04001837 RID: 6199
	[SerializeField]
	public Effect splashLeft;

	// Token: 0x04001838 RID: 6200
	[SerializeField]
	public Transform splashRoot;

	// Token: 0x04001839 RID: 6201
	[SerializeField]
	public Effect yellEffect;

	// Token: 0x0400183A RID: 6202
	public FlyingMermaidLevelMermaid.SummonPossibility[] summonPattern = new FlyingMermaidLevelMermaid.SummonPossibility[]
	{
		FlyingMermaidLevelMermaid.SummonPossibility.Seahorse,
		FlyingMermaidLevelMermaid.SummonPossibility.Pufferfish,
		FlyingMermaidLevelMermaid.SummonPossibility.Turtle
	};

	// Token: 0x0400183B RID: 6203
	public FlyingMermaidLevelMermaid.FishPossibility[] fishPattern;

	// Token: 0x0400183C RID: 6204
	public int summonIndex;

	// Token: 0x0400183D RID: 6205
	public int fishIndex;

	// Token: 0x0400183E RID: 6206
	public int spreadshotPatternIndex;

	// Token: 0x0400183F RID: 6207
	public int spinnerPatternIndex;

	// Token: 0x04001840 RID: 6208
	public int homerPatternIndex;

	// Token: 0x04001841 RID: 6209
	public bool stopPufferfish;

	// Token: 0x04001842 RID: 6210
	public bool transformationStarting;

	// Token: 0x04001843 RID: 6211
	public bool stopMoving;

	// Token: 0x04001844 RID: 6212
	public float walkPCT;

	// Token: 0x04001845 RID: 6213
	public float walkTime;

	// Token: 0x04001846 RID: 6214
	public float walkDuration;

	// Token: 0x04001847 RID: 6215
	public string[] spreadFishPinkPattern;

	// Token: 0x04001848 RID: 6216
	public int spreadFishPinkIndex;

	// Token: 0x04001849 RID: 6217
	public int blinks;

	// Token: 0x0400184A RID: 6218
	public int maxBlinks;

	// Token: 0x0400184B RID: 6219
	public FlyingMermaidLevelMermaid.FishPossibility fish;

	// Token: 0x02000D41 RID: 3393
	public enum State
	{
		// Token: 0x0400601C RID: 24604
		Intro,
		// Token: 0x0400601D RID: 24605
		Idle,
		// Token: 0x0400601E RID: 24606
		Yell,
		// Token: 0x0400601F RID: 24607
		Summon,
		// Token: 0x04006020 RID: 24608
		Fish,
		// Token: 0x04006021 RID: 24609
		Transform
	}

	// Token: 0x02000D42 RID: 3394
	public enum SummonPossibility
	{
		// Token: 0x04006023 RID: 24611
		Seahorse,
		// Token: 0x04006024 RID: 24612
		Pufferfish,
		// Token: 0x04006025 RID: 24613
		Turtle
	}

	// Token: 0x02000D43 RID: 3395
	public enum FishPossibility
	{
		// Token: 0x04006027 RID: 24615
		Spreadshot,
		// Token: 0x04006028 RID: 24616
		Spinner,
		// Token: 0x04006029 RID: 24617
		Homer
	}
}
