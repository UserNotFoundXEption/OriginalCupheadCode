using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200026A RID: 618
public class FlyingGenieLevelGenieTransform : LevelProperties.FlyingGenie.Entity
{
	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06001C50 RID: 7248 RVA: 0x00017F94 File Offset: 0x00016194
	// (set) Token: 0x06001C51 RID: 7249 RVA: 0x00017F9C File Offset: 0x0001619C
	public FlyingGenieLevelGenieTransform.State state { get; set; }

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06001C52 RID: 7250 RVA: 0x00017FA5 File Offset: 0x000161A5
	// (set) Token: 0x06001C53 RID: 7251 RVA: 0x00017FAD File Offset: 0x000161AD
	public bool skipMarionette { get; set; }

	// Token: 0x06001C54 RID: 7252 RVA: 0x000ADF20 File Offset: 0x000AC120
	public override void Awake()
	{
		base.Awake();
		this.skipMarionette = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x00017FB6 File Offset: 0x000161B6
	public override void LevelInit(LevelProperties.FlyingGenie properties)
	{
		base.LevelInit(properties);
		this.state = FlyingGenieLevelGenieTransform.State.Intro;
		this.pyramids = new List<FlyingGenieLevelPyramid>();
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x000ADF74 File Offset: 0x000AC174
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (this.skipMarionette && this.state == FlyingGenieLevelGenieTransform.State.Giant && this.transitionHP > 0f)
		{
			float num = this.transitionHP;
			this.transitionHP -= info.damage;
			Level.Current.timeline.DealDamage(Mathf.Clamp(num - this.transitionHP, 0f, num));
		}
		else if (base.properties.CurrentHealth <= 0f && this.state != FlyingGenieLevelGenieTransform.State.Dead)
		{
			this.state = FlyingGenieLevelGenieTransform.State.Dead;
			if (Level.Current.mode == Level.Mode.Easy)
			{
				this.MarionetteDead();
			}
			else
			{
				this.StartDeath();
			}
		}
	}

	// Token: 0x06001C57 RID: 7255 RVA: 0x00017FD1 File Offset: 0x000161D1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x00017FEF File Offset: 0x000161EF
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x00018007 File Offset: 0x00016207
	public void StartMarionette(Vector3 spawnPos, FlyingGenieLevelMeditateFX meditateP1, FlyingGenieLevelMeditateFX meditateP2)
	{
		base.GetComponent<Collider2D>().enabled = true;
		base.transform.position = spawnPos;
		this.meditateP1 = meditateP1;
		this.meditateP2 = meditateP2;
		base.StartCoroutine(this.phase2_intro_cr());
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x000AE044 File Offset: 0x000AC244
	public IEnumerator phase2_intro_cr()
	{
		AudioManager.Play("genie_return");
		this.emitAudioFromObject.Add("genie_return");
		LevelProperties.FlyingGenie.Scan p = base.properties.CurrentState.scan;
		float timer = 0f;
		float P1ShrinkTimer = 0f;
		float P2ShrinkTimer = 0f;
		PlanePlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne) as PlanePlayerController;
		PlanePlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo) as PlanePlayerController;
		bool player2In = player2 != null;
		while (timer < p.scanDuration)
		{
			timer += CupheadTime.Delta;
			if (Level.Current.mode != Level.Mode.Easy)
			{
				if (player.Shrunk)
				{
					P1ShrinkTimer += CupheadTime.Delta;
				}
				if (player2In && player2.Shrunk)
				{
					P2ShrinkTimer += CupheadTime.Delta;
				}
			}
			yield return null;
		}
		if (P1ShrinkTimer >= p.miniDuration)
		{
			if (player2In)
			{
				if (P2ShrinkTimer >= p.miniDuration)
				{
					this.skipMarionette = true;
				}
				else
				{
					this.skipMarionette = false;
				}
			}
			else
			{
				this.skipMarionette = true;
			}
		}
		base.animator.SetTrigger("Continue");
		this.pyramidsGoingClockwise = Rand.Bool();
		if (this.skipMarionette)
		{
			this.transitionHP = p.transitionDamage;
			base.animator.SetBool("IsPuppet", true);
			this.state = FlyingGenieLevelGenieTransform.State.Giant;
			base.StartCoroutine(this.move_up_puppet_cr());
			base.properties.DealDamageToNextNamedState();
		}
		else
		{
			base.animator.SetBool("IsPuppet", false);
			this.state = FlyingGenieLevelGenieTransform.State.Marionette;
			yield return base.animator.WaitForAnimationToEnd(this, "Marionette_Intro", false, true);
			this.startPos = base.transform.position;
			base.StartCoroutine(this.move_cr());
			base.StartCoroutine(this.shoot_cr());
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x0001803C File Offset: 0x0001623C
	public void EndFX()
	{
		if (this.meditateP1 != null)
		{
			this.meditateP1.EndEffect();
		}
		if (this.meditateP2 != null)
		{
			this.meditateP2.EndEffect();
		}
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x000AE060 File Offset: 0x000AC260
	public void SnapPosition()
	{
		this.HandSFX();
		base.transform.position = this.morphRoot.position;
		this.bottomLayer.transform.localPosition = new Vector3(-160f, this.bottomLayer.transform.localPosition.y);
		base.StartCoroutine(this.handle_carpet_fadeout_cr());
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x000AE0C8 File Offset: 0x000AC2C8
	public IEnumerator move_up_puppet_cr()
	{
		float t = 0f;
		float timer = 0.4f;
		float slowTimer = 0.8f;
		float midTimer = 0.5f;
		float midEnd = 300f;
		float slowEnd = 410f;
		float end = 1071f;
		float start = base.transform.position.y;
		yield return base.animator.WaitForAnimationToEnd(this, "Genie_Morph_Puppet", false, true);
		this.tinyMarionette.gameObject.SetActive(true);
		while (t < slowTimer)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, t / slowTimer);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, slowEnd, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		start = base.transform.position.y;
		bool startIntro = false;
		while (t < midTimer)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / midTimer);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, midEnd, val2)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		start = base.transform.position.y;
		while (t < timer)
		{
			float val3 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / timer);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, end, val3)), null);
			if (t / timer > 0.95f && !startIntro)
			{
				this.tinyMarionette.animator.SetTrigger("OnIntro");
				startIntro = true;
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		this.tinyMarionette.transform.parent = null;
		base.properties.DealDamage(base.properties.CurrentState.scan.transitionDamage);
		Vector3 endPos = new Vector3(this.pyramidPivotPoint.position.x, this.pyramidPivotPoint.position.y + 145f);
		this.tinyMarionette.Activate(endPos, base.properties.CurrentState.scan, !this.pyramidsGoingClockwise);
		this.EndMarionette();
		yield return null;
		yield break;
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x000AE0E4 File Offset: 0x000AC2E4
	public IEnumerator handle_carpet_fadeout_cr()
	{
		this.bottomLayer.color = new Color(1f, 1f, 1f, 1f);
		float t = 0f;
		float time = 2f;
		while (t < time)
		{
			this.bottomLayer.color = new Color(1f, 1f, 1f, 1f - t / time);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.Play("Hands_Off");
		this.bottomLayer.color = new Color(1f, 1f, 1f, 1f);
		this.bottomLayer.transform.localPosition = Vector3.zero;
		yield return null;
		yield break;
	}

	// Token: 0x06001C5F RID: 7263 RVA: 0x000AE100 File Offset: 0x000AC300
	public void SpawnTurban()
	{
		if (!this.skipMarionette)
		{
			this.spawner = this.spawnerPrefab.Create(new Vector3(base.transform.position.x, (float)Level.Current.Height + 100f), PlayerManager.GetNext(), base.properties.CurrentState.bullets);
			this.spawner.isDead = false;
		}
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x000AE178 File Offset: 0x000AC378
	public IEnumerator shoot_cr()
	{
		LevelProperties.FlyingGenie.Bullets p = base.properties.CurrentState.bullets;
		int mainShotIndex = Random.Range(0, p.shotCount.Length);
		string[] shotCount = p.shotCount[mainShotIndex].Split(new char[]
		{
			','
		});
		int shotIndex = 0;
		string[] pinkCount = p.pinkString.Split(new char[]
		{
			','
		});
		int pinkIndex = 0;
		while (this.state == FlyingGenieLevelGenieTransform.State.Marionette)
		{
			this.isShooting = false;
			shotCount = p.shotCount[mainShotIndex].Split(new char[]
			{
				','
			});
			yield return CupheadTime.WaitForSeconds(this, p.hesitateRange.RandomFloat());
			this.isShooting = true;
			base.animator.SetBool("IsAttacking", true);
			yield return base.animator.WaitForAnimationToEnd(this, "Marionette_Attack_Start", false, true);
			AudioManager.Play("genie_voice_laugh_reverb");
			AbstractPlayerController player = PlayerManager.GetNext();
			base.animator.Play("Marionette_Spark");
			for (int i = 0; i < shotCount.Length; i++)
			{
				for (int j = 0; j < Parser.IntParse(shotCount[shotIndex]); j++)
				{
					if (player == null || player.IsDead)
					{
						player = PlayerManager.GetNext();
					}
					Vector3 dir = player.transform.position - this.marionetteShootRoot.transform.position;
					if (dir.x > 0f)
					{
						dir.x = 0f;
					}
					if (pinkCount[pinkIndex][0] == 'P')
					{
						this.pinkBullet.Create(this.marionetteShootRoot.transform.position, MathUtils.DirectionToAngle(dir), p.shotSpeed);
						AudioManager.Play("genie_puppet_shoot");
						this.emitAudioFromObject.Add("genie_puppet_shoot");
					}
					else if (pinkCount[pinkIndex][0] == 'R')
					{
						this.shotBullet.Create(this.marionetteShootRoot.transform.position, MathUtils.DirectionToAngle(dir), p.shotSpeed);
						AudioManager.Play("genie_puppet_shoot");
						this.emitAudioFromObject.Add("genie_puppet_shoot");
					}
					yield return this.WaitWhileShooting(p.shotDelay, p.shotSpeed);
					pinkIndex = (pinkIndex + 1) % pinkCount.Length;
				}
				if (player == null || player.IsDead)
				{
					player = PlayerManager.GetNext();
				}
				yield return this.WaitWhileShooting(p.shotDelay, p.shotSpeed);
				if (shotIndex < shotCount.Length - 1)
				{
					shotIndex++;
				}
				else
				{
					mainShotIndex = (mainShotIndex + 1) % p.shotCount.Length;
					shotIndex = 0;
				}
				yield return null;
			}
			yield return null;
			base.animator.SetBool("IsAttacking", false);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x000AE194 File Offset: 0x000AC394
	public IEnumerator WaitWhileShooting(float time, float shootSpeed)
	{
		bool pointingUp = false;
		float timeEsalpsed = 0f;
		float timeSinceSubShot = 0f;
		while (timeEsalpsed <= time)
		{
			if (timeSinceSubShot >= 0.12f)
			{
				this.shootBullet.Create(this.marionetteShootRoot.transform.position, (float)((!pointingUp) ? -100 : 100), shootSpeed);
				pointingUp = !pointingUp;
				timeSinceSubShot = 0f;
			}
			timeEsalpsed += CupheadTime.Delta;
			timeSinceSubShot += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C62 RID: 7266 RVA: 0x000AE1C0 File Offset: 0x000AC3C0
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (this.state == FlyingGenieLevelGenieTransform.State.Marionette)
		{
			if (!this.isShooting)
			{
				if (base.transform.position.x > -this.startPos.x)
				{
					base.transform.AddPosition(-base.properties.CurrentState.bullets.marionetteMoveSpeed * CupheadTime.FixedDelta, 0f, 0f);
				}
			}
			else if (base.transform.position.x < this.startPos.x)
			{
				base.transform.AddPosition(base.properties.CurrentState.bullets.marionetteReturnSpeed * CupheadTime.FixedDelta, 0f, 0f);
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001C63 RID: 7267 RVA: 0x000AE1DC File Offset: 0x000AC3DC
	public void EndMarionette()
	{
		if (!this.skipMarionette)
		{
			AudioManager.Play("genie_puppet_exit");
			this.emitAudioFromObject.Add("genie_puppet_exit");
		}
		if (this.spawner != null)
		{
			this.spawner.isDead = true;
		}
		this.spark.SetActive(false);
		this.StopAllCoroutines();
		this.state = FlyingGenieLevelGenieTransform.State.Giant;
		base.StartCoroutine(this.genie_intro_cr());
	}

	// Token: 0x06001C64 RID: 7268 RVA: 0x00018076 File Offset: 0x00016276
	public void MarionetteDead()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("MarionetteDeath");
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x000AE254 File Offset: 0x000AC454
	public IEnumerator genie_intro_cr()
	{
		float pullSpeed = 700f;
		float size = base.GetComponent<SpriteRenderer>().bounds.size.x;
		float angle = 120f;
		int number = 1;
		if (!this.skipMarionette)
		{
			base.animator.SetTrigger("MarionetteDeath");
			base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		while (base.transform.position.y < 960f)
		{
			base.transform.AddPosition(0f, pullSpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		if (!this.skipMarionette)
		{
			base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		}
		yield return CupheadTime.WaitForSeconds(this, 0.7f);
		base.animator.Play("Giant_Intro");
		base.transform.position = new Vector3(640f + size / 3f, 0f);
		Vector3 startPos = base.transform.position;
		float t = 0f;
		float time = 1f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(startPos, this.giantRoot.position, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = this.giantRoot.position;
		for (int i = 0; i < 3; i++)
		{
			this.SpawnPyramids(angle * 0.0174532924f * (float)i, number);
			number++;
		}
		base.StartCoroutine(this.attack_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x00018094 File Offset: 0x00016294
	public void IntroHands()
	{
		base.StartCoroutine(this.intro_hands_cr());
	}

	// Token: 0x06001C67 RID: 7271 RVA: 0x000AE270 File Offset: 0x000AC470
	public IEnumerator intro_hands_cr()
	{
		Vector3 end = this.handFront.transform.position;
		Vector3 start = this.handFront.transform.position;
		start.y = this.handFront.transform.position.y - 500f;
		this.handFront.transform.position = start;
		this.handBack.transform.position = start;
		base.animator.Play("Giant_Hands");
		float t = 0f;
		float time = 1.25f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			this.handFront.transform.position = Vector2.Lerp(start, end, val);
			this.handBack.transform.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.8f);
		t = 0f;
		while (t < time)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			this.handFront.transform.position = Vector2.Lerp(end, start, val2);
			this.handBack.transform.position = Vector2.Lerp(end, start, val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.Play("Hands_Off");
		base.StartCoroutine(this.gem_stone_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001C68 RID: 7272 RVA: 0x000AE28C File Offset: 0x000AC48C
	public void SpawnPyramids(float startingAngle, int number)
	{
		LevelProperties.FlyingGenie.Pyramids pyramids = base.properties.CurrentState.pyramids;
		FlyingGenieLevelPyramid flyingGenieLevelPyramid = Object.Instantiate<FlyingGenieLevelPyramid>(this.pyramidPrefab);
		flyingGenieLevelPyramid.Init(pyramids, base.transform.position, startingAngle, pyramids.speedRotation, this.pyramidPivotPoint, number, this.pyramidsGoingClockwise);
		flyingGenieLevelPyramid.GetComponent<Collider2D>().enabled = false;
		this.pyramids.Add(flyingGenieLevelPyramid);
	}

	// Token: 0x06001C69 RID: 7273 RVA: 0x000AE2FC File Offset: 0x000AC4FC
	public IEnumerator attack_cr()
	{
		LevelProperties.FlyingGenie.Pyramids p = base.properties.CurrentState.pyramids;
		string[] delayString = p.attackDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] attackString = p.pyramidAttackString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int delayIndex = Random.Range(0, delayString.Length);
		int attackIndex = Random.Range(0, attackString.Length);
		float delay = 0f;
		int numberReceived = 0;
		float t = 0f;
		float time = 2.5f;
		foreach (FlyingGenieLevelPyramid flyingGenieLevelPyramid in this.pyramids)
		{
			flyingGenieLevelPyramid.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
		}
		while (t < time)
		{
			t += CupheadTime.Delta;
			foreach (FlyingGenieLevelPyramid flyingGenieLevelPyramid2 in this.pyramids)
			{
				flyingGenieLevelPyramid2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / time);
			}
			yield return null;
		}
		foreach (FlyingGenieLevelPyramid flyingGenieLevelPyramid3 in this.pyramids)
		{
			flyingGenieLevelPyramid3.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			flyingGenieLevelPyramid3.GetComponent<Collider2D>().enabled = true;
		}
		for (;;)
		{
			for (int i = attackIndex; i < attackString.Length; i++)
			{
				Parser.FloatTryParse(delayString[delayIndex], out delay);
				yield return CupheadTime.WaitForSeconds(this, delay);
				string[] attackOrder = attackString[i].Split(new char[]
				{
					'-'
				});
				foreach (string s in attackOrder)
				{
					Parser.IntTryParse(s, out numberReceived);
					for (int l = 0; l < this.pyramids.Count; l++)
					{
						if (this.pyramids[l].number == numberReceived)
						{
							base.StartCoroutine(this.pyramids[l].beam_cr());
						}
					}
				}
				for (int j = 0; j < this.pyramids.Count; j++)
				{
					if (this.pyramids[j].number == numberReceived)
					{
						while (!this.pyramids[j].finishedATK)
						{
							yield return null;
						}
					}
				}
				attackIndex = 0;
				i %= attackString.Length;
				delayIndex = (delayIndex + 1) % delayString.Length;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C6A RID: 7274 RVA: 0x000AE318 File Offset: 0x000AC518
	public IEnumerator gem_stone_cr()
	{
		LevelProperties.FlyingGenie.GemStone p = base.properties.CurrentState.gemStone;
		string[] attackDelayPattern = p.attackDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int delayIndex = Random.Range(0, attackDelayPattern.Length);
		this.pinkString = p.pinkString.Split(new char[]
		{
			','
		});
		this.pinkIndex = Random.Range(0, this.pinkString.Length);
		float delay = 0f;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, p.warningDuration);
			Parser.FloatTryParse(attackDelayPattern[delayIndex], out delay);
			base.animator.SetTrigger("OnGiantAttack");
			yield return base.animator.WaitForAnimationToEnd(this, "Giant_Attack", false, true);
			yield return CupheadTime.WaitForSeconds(this, delay);
			delayIndex = (delayIndex + 1) % attackDelayPattern.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C6B RID: 7275 RVA: 0x000AE334 File Offset: 0x000AC534
	public void OnRing()
	{
		LevelProperties.FlyingGenie.GemStone gemStone = base.properties.CurrentState.gemStone;
		this.gemStone.LookAt2D(PlayerManager.GetNext().center);
		bool isPink;
		FlyingGenieLevelRing ring;
		if (this.pinkString[this.pinkIndex][0] == 'P')
		{
			isPink = true;
			ring = (this.pinkRingPrefab.Create(this.gemStone.position, this.gemStone.eulerAngles.z, gemStone.bulletSpeed) as FlyingGenieLevelRing);
		}
		else
		{
			isPink = false;
			ring = (this.ringPrefab.Create(this.gemStone.position, this.gemStone.eulerAngles.z, gemStone.bulletSpeed) as FlyingGenieLevelRing);
		}
		base.StartCoroutine(this.ring_cr(ring, isPink));
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkString.Length;
	}

	// Token: 0x06001C6C RID: 7276 RVA: 0x000AE42C File Offset: 0x000AC62C
	public IEnumerator ring_cr(FlyingGenieLevelRing ring, bool isPink)
	{
		ring.isMain = true;
		int frameCount = 0;
		float frameTime = 0f;
		FlyingGenieLevelRing trailRing = (!isPink) ? this.ringPrefab : this.pinkRingPrefab;
		FlyingGenieLevelRing lastRing = null;
		while (ring != null)
		{
			frameTime += CupheadTime.Delta;
			if (frameTime > 0.0416666679f)
			{
				if (frameCount < 3)
				{
					frameCount++;
				}
				else
				{
					frameCount = 0;
					if (lastRing != null)
					{
						lastRing.DisableCollision();
					}
					lastRing = (trailRing.Create(ring.transform.position, this.gemStone.eulerAngles.z, 0.1f) as FlyingGenieLevelRing);
				}
				frameTime -= 0.0416666679f;
				yield return null;
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001C6D RID: 7277 RVA: 0x000180A3 File Offset: 0x000162A3
	public void StartDeath()
	{
		if (this.skipMarionette && this.tinyMarionette != null)
		{
			this.tinyMarionette.Die();
		}
		base.animator.SetTrigger("Death");
	}

	// Token: 0x06001C6E RID: 7278 RVA: 0x000180DC File Offset: 0x000162DC
	public void SpawnPuff()
	{
		this.deathPuffEffect.Create(this.deathPuffRoot.transform.position);
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x000180FA File Offset: 0x000162FA
	public void HandSFX()
	{
		AudioManager.Play("genie_puppet_hand_enter");
		this.emitAudioFromObject.Add("genie_puppet_hand_enter");
	}

	// Token: 0x06001C70 RID: 7280 RVA: 0x00018116 File Offset: 0x00016316
	public void SoundGenieVoiceMorph()
	{
		AudioManager.Play("genie_voice_excited");
		this.emitAudioFromObject.Add("genie_voice_excited");
	}

	// Token: 0x06001C71 RID: 7281 RVA: 0x00018132 File Offset: 0x00016332
	public void SoundPuppetRun()
	{
		AudioManager.Play("genie_puppet_run");
		this.emitAudioFromObject.Add("genie_puppet_run");
	}

	// Token: 0x06001C72 RID: 7282 RVA: 0x0001814E File Offset: 0x0001634E
	public void SoundGenieVoicePhase3Intro()
	{
		AudioManager.Play("genie_voice_phase3_intro");
		this.emitAudioFromObject.Add("genie_voice_phase3_intro");
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x0001816A File Offset: 0x0001636A
	public void SoundGenieMindShoot()
	{
		AudioManager.Play("genie_phase3_mind_shoot");
		this.emitAudioFromObject.Add("genie_phase3_mind_shoot");
	}

	// Token: 0x06001C74 RID: 7284 RVA: 0x00018186 File Offset: 0x00016386
	public void SoundPuppetSmallEnterMobile()
	{
		AudioManager.Play("genie_puppetsmall_enter_mobile");
		this.emitAudioFromObject.Add("genie_puppetsmall_enter_mobile");
	}

	// Token: 0x04001702 RID: 5890
	public const float FRAME_TIME = 0.0416666679f;

	// Token: 0x04001704 RID: 5892
	[SerializeField]
	public Effect deathPuffEffect;

	// Token: 0x04001705 RID: 5893
	[SerializeField]
	public SpriteRenderer bottomLayer;

	// Token: 0x04001706 RID: 5894
	[Space(10f)]
	[SerializeField]
	public FlyingGenieLevelSpawner spawnerPrefab;

	// Token: 0x04001707 RID: 5895
	[SerializeField]
	public Transform marionetteShootRoot;

	// Token: 0x04001708 RID: 5896
	[SerializeField]
	public BasicProjectile shotBullet;

	// Token: 0x04001709 RID: 5897
	[SerializeField]
	public BasicProjectile pinkBullet;

	// Token: 0x0400170A RID: 5898
	[SerializeField]
	public BasicProjectile shootBullet;

	// Token: 0x0400170B RID: 5899
	[SerializeField]
	public BasicProjectile spreadProjectile;

	// Token: 0x0400170C RID: 5900
	[SerializeField]
	public FlyingGenieLevelRing ringPrefab;

	// Token: 0x0400170D RID: 5901
	[SerializeField]
	public FlyingGenieLevelRing pinkRingPrefab;

	// Token: 0x0400170E RID: 5902
	[SerializeField]
	public FlyingGenieLevelPyramid pyramidPrefab;

	// Token: 0x0400170F RID: 5903
	[SerializeField]
	public FlyingGenieLevelTinyMarionette tinyMarionette;

	// Token: 0x04001710 RID: 5904
	[Space(10f)]
	[SerializeField]
	public Transform pyramidPivotPoint;

	// Token: 0x04001711 RID: 5905
	[SerializeField]
	public Transform gemStone;

	// Token: 0x04001712 RID: 5906
	[SerializeField]
	public Transform pipe;

	// Token: 0x04001713 RID: 5907
	[SerializeField]
	public Transform giantRoot;

	// Token: 0x04001714 RID: 5908
	[SerializeField]
	public Transform handFront;

	// Token: 0x04001715 RID: 5909
	[SerializeField]
	public Transform handBack;

	// Token: 0x04001716 RID: 5910
	[SerializeField]
	public Transform deathPuffRoot;

	// Token: 0x04001717 RID: 5911
	[SerializeField]
	public Transform morphRoot;

	// Token: 0x04001718 RID: 5912
	[SerializeField]
	public Transform marionetteRoot;

	// Token: 0x04001719 RID: 5913
	[SerializeField]
	public GameObject spark;

	// Token: 0x0400171A RID: 5914
	public FlyingGenieLevelMeditateFX meditateP1;

	// Token: 0x0400171B RID: 5915
	public FlyingGenieLevelMeditateFX meditateP2;

	// Token: 0x0400171C RID: 5916
	public FlyingGenieLevelSpawner spawner;

	// Token: 0x0400171D RID: 5917
	public List<FlyingGenieLevelBomb> bombs;

	// Token: 0x0400171E RID: 5918
	public List<FlyingGenieLevelPyramid> pyramids;

	// Token: 0x0400171F RID: 5919
	public DamageDealer damageDealer;

	// Token: 0x04001720 RID: 5920
	public DamageReceiver damageReceiver;

	// Token: 0x04001721 RID: 5921
	public Vector3 startPos;

	// Token: 0x04001722 RID: 5922
	public bool pyramidsGoingClockwise;

	// Token: 0x04001723 RID: 5923
	public bool isShooting;

	// Token: 0x04001725 RID: 5925
	public float transitionHP;

	// Token: 0x04001726 RID: 5926
	public int pinkIndex;

	// Token: 0x04001727 RID: 5927
	public string[] pinkString;

	// Token: 0x02000D01 RID: 3329
	public enum State
	{
		// Token: 0x04005E45 RID: 24133
		Intro,
		// Token: 0x04005E46 RID: 24134
		Idle,
		// Token: 0x04005E47 RID: 24135
		Marionette,
		// Token: 0x04005E48 RID: 24136
		Giant,
		// Token: 0x04005E49 RID: 24137
		Dead
	}
}
