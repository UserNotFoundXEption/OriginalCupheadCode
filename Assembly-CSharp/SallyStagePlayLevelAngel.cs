using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000356 RID: 854
public class SallyStagePlayLevelAngel : LevelProperties.SallyStagePlay.Entity
{
	// Token: 0x17000318 RID: 792
	// (get) Token: 0x060025A2 RID: 9634 RVA: 0x0001FAB4 File Offset: 0x0001DCB4
	// (set) Token: 0x060025A3 RID: 9635 RVA: 0x0001FABC File Offset: 0x0001DCBC
	public SallyStagePlayLevelAngel.State state { get; set; }

	// Token: 0x060025A4 RID: 9636 RVA: 0x000C7384 File Offset: 0x000C5584
	public override void Awake()
	{
		base.Awake();
		this.signStart = this.sign.transform.position;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x060025A5 RID: 9637 RVA: 0x000C73E8 File Offset: 0x000C55E8
	public override void LevelInit(LevelProperties.SallyStagePlay properties)
	{
		base.LevelInit(properties);
		LevelProperties.SallyStagePlay.Lightning lightning = properties.CurrentState.lightning;
		SallyStagePlayLevelAngel.extraHP = properties.CurrentState.husband.deityHP;
		this.lightningMax = Random.Range((int)lightning.lightningDelayRange.min, (int)lightning.lightningDelayRange.max);
		this.lightningShotIndex = Random.Range(0, lightning.lightningShotCount.Split(new char[]
		{
			','
		}).Length);
		this.lightningAngleIndex = Random.Range(0, lightning.lightningAngleString.Split(new char[]
		{
			','
		}).Length);
		this.lightningSpawnIndex = Random.Range(0, lightning.lightningSpawnString.Split(new char[]
		{
			','
		}).Length);
		this.meteorSpawnIndex = Random.Range(0, properties.CurrentState.meteor.meteorSpawnString.Split(new char[]
		{
			','
		}).Length);
		this.meteors = new List<SallyStagePlayLevelMeteor>();
		if (Level.Current.mode == Level.Mode.Easy)
		{
			Level.Current.OnWinEvent += this.OnEasyDeath;
		}
		else
		{
			Level.Current.OnWinEvent += this.OnDeath;
		}
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x000C7528 File Offset: 0x000C5728
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.killedHusband && SallyStagePlayLevelAngel.extraHP > 0f)
		{
			SallyStagePlayLevelAngel.extraHP -= info.damage;
		}
		else
		{
			base.properties.DealDamage(info.damage);
		}
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x0001FAC5 File Offset: 0x0001DCC5
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x0001FADD File Offset: 0x0001DCDD
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060025A9 RID: 9641 RVA: 0x0001FAFB File Offset: 0x0001DCFB
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.meteorPrefab = null;
		this.lightningPrefab = null;
		this.umbrellaPrefab = null;
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x0001FB18 File Offset: 0x0001DD18
	public void StartPhase3(bool killedHusband)
	{
		this.killedHusband = killedHusband;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060025AB RID: 9643 RVA: 0x000C7578 File Offset: 0x000C5778
	public IEnumerator intro_cr()
	{
		float t = 0f;
		float time = 3f;
		Vector3 endPos = new Vector3(base.transform.position.x, this.phase3Root.position.y);
		Vector2 start = base.transform.position;
		base.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.sally_angel_intro_sound_cr());
		if (this.killedHusband)
		{
			base.StartCoroutine(this.spawn_husband_cr());
		}
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = endPos;
		this.nextAttack = 1;
		base.StartCoroutine(this.sign_slide_cr());
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.StartCoroutine(this.main_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060025AC RID: 9644 RVA: 0x000C7594 File Offset: 0x000C5794
	public IEnumerator sally_angel_intro_sound_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 4f);
		AudioManager.Play("sally_vox_maniacal");
		this.emitAudioFromObject.Add("sally_vox_maniacal");
		yield break;
	}

	// Token: 0x060025AD RID: 9645 RVA: 0x000C75B0 File Offset: 0x000C57B0
	public IEnumerator spawn_husband_cr()
	{
		this.husband.gameObject.SetActive(true);
		this.husband.GetComponent<Collider2D>().enabled = true;
		float t = 0f;
		float time = 3.5f;
		Vector3 endPos = new Vector3(this.phase3Root.transform.position.x, this.husband.transform.position.y);
		Vector2 start = this.husband.transform.position;
		bool soundTriggered = false;
		while (t < time)
		{
			if (t / time >= 0.3f && !soundTriggered)
			{
				AudioManager.Play("sally_fiance_enter");
				this.emitAudioFromObject.Add("sally_fiance_enter");
				soundTriggered = true;
			}
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, 0f, 1f, t / time);
			this.husband.transform.position = Vector2.Lerp(start, endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.husband.Attack();
		yield return null;
		yield break;
	}

	// Token: 0x060025AE RID: 9646 RVA: 0x000C75CC File Offset: 0x000C57CC
	public IEnumerator sign_slide_cr()
	{
		string attackName = string.Empty;
		int num = this.nextAttack;
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					attackName = "Wave";
				}
			}
			else
			{
				attackName = "Meteor";
			}
		}
		else
		{
			attackName = "Lightning";
		}
		this.sign.Play(attackName);
		float t = 0f;
		float time = 0.1f;
		Vector3 start = this.sign.transform.position;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.sign.transform.position = Vector3.Lerp(start, new Vector3(this.signStart.x, this.signStart.y - 100f), t / time);
			yield return null;
		}
		t = 0f;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		start = this.sign.transform.position;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.sign.transform.position = Vector3.Lerp(start, this.signStart, t / time);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060025AF RID: 9647 RVA: 0x000C75E8 File Offset: 0x000C57E8
	public IEnumerator slide_out_cr()
	{
		float t = 0f;
		float time = 0.1f;
		Vector3 start = this.sign.transform.position;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.sign.transform.position = Vector3.Lerp(start, this.signStart, t / time);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060025B0 RID: 9648 RVA: 0x000C7604 File Offset: 0x000C5804
	public IEnumerator main_cr()
	{
		LevelProperties.SallyStagePlay.General p = base.properties.CurrentState.general;
		string[] main = p.attackString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int mainIndex = Random.Range(0, main.Length);
		this.nextAttack = mainIndex;
		while (main[mainIndex] != "M")
		{
			mainIndex = (mainIndex + 1) % main.Length;
			yield return null;
		}
		for (;;)
		{
			while (this.state != SallyStagePlayLevelAngel.State.Idle)
			{
				yield return null;
			}
			base.animator.SetBool("OnPh3Attack", true);
			base.StartCoroutine(this.sign_slide_cr());
			yield return base.animator.WaitForAnimationToStart(this, "Phase3_Attack_Start", false);
			string text = main[mainIndex];
			if (text != null)
			{
				if (!(text == "L"))
				{
					if (!(text == "M"))
					{
						if (text == "T")
						{
							base.StartCoroutine(this.tidal_wave_cr());
						}
					}
					else
					{
						base.StartCoroutine(this.meteor_cr());
					}
				}
				else
				{
					base.StartCoroutine(this.lightning_cr());
				}
			}
			mainIndex = (mainIndex + 1) % main.Length;
			this.GetNextAttack(main[mainIndex]);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060025B1 RID: 9649 RVA: 0x000C7620 File Offset: 0x000C5820
	public void GetNextAttack(string main)
	{
		if (main != null)
		{
			if (!(main == "L"))
			{
				if (!(main == "M"))
				{
					if (main == "T")
					{
						this.nextAttack = 2;
					}
				}
				else
				{
					this.nextAttack = 1;
				}
			}
			else
			{
				this.nextAttack = 0;
			}
		}
	}

	// Token: 0x060025B2 RID: 9650 RVA: 0x000C768C File Offset: 0x000C588C
	public IEnumerator lightning_cr()
	{
		this.state = SallyStagePlayLevelAngel.State.Lightning;
		LevelProperties.SallyStagePlay.Lightning p = base.properties.CurrentState.lightning;
		string[] shotString = p.lightningShotCount.Split(new char[]
		{
			','
		});
		string[] angleString = p.lightningAngleString.Split(new char[]
		{
			','
		});
		string[] spawnString = p.lightningSpawnString.Split(new char[]
		{
			','
		});
		float angle = 0f;
		float spawn = 0f;
		float rotation = 0f;
		int shotCount = 0;
		Parser.IntTryParse(shotString[this.lightningShotIndex], out shotCount);
		for (int i = 0; i < shotCount; i++)
		{
			Parser.FloatTryParse(spawnString[this.lightningSpawnIndex], out spawn);
			bool aimAtPlayer;
			if (this.lightningMaxCounter >= this.lightningMax)
			{
				aimAtPlayer = true;
				this.lightningMaxCounter = 0;
			}
			else
			{
				aimAtPlayer = false;
				if (this.lightningMaxCounter == 0)
				{
					this.lightningMax = Random.Range((int)p.lightningDirectAimRange.min, (int)p.lightningDirectAimRange.max);
				}
				Parser.FloatTryParse(angleString[this.lightningAngleIndex], out angle);
				this.lightningAngleIndex = (this.lightningAngleIndex + 1) % angleString.Length;
				this.lightningMaxCounter++;
			}
			Vector3 pos = new Vector3(-640f + spawn, 460f);
			if (aimAtPlayer)
			{
				AbstractPlayerController next = PlayerManager.GetNext();
				Vector3 vector = next.transform.position - pos;
				rotation = MathUtils.DirectionToAngle(vector);
			}
			else
			{
				rotation = angle;
			}
			this.lightningPrefab.Create(pos, rotation, p.lightningSpeed, i == shotCount - 1);
			this.lightningSpawnIndex = (this.lightningSpawnIndex + 1) % spawnString.Length;
			yield return CupheadTime.WaitForSeconds(this, p.lightningDelayRange.RandomFloat());
		}
		base.animator.SetBool("OnPh3Attack", false);
		this.lightningShotIndex = (this.lightningShotIndex + 1) % shotString.Length;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.general.attackDelayRange.RandomFloat());
		this.state = SallyStagePlayLevelAngel.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x060025B3 RID: 9651 RVA: 0x000C76A8 File Offset: 0x000C58A8
	public IEnumerator meteor_cr()
	{
		this.state = SallyStagePlayLevelAngel.State.Meteor;
		LevelProperties.SallyStagePlay.Meteor p = base.properties.CurrentState.meteor;
		string[] meteorSpawnString = p.meteorSpawnString.Split(new char[]
		{
			','
		});
		int index = 0;
		float spawn = 0f;
		Parser.FloatTryParse(meteorSpawnString[this.meteorSpawnIndex], out spawn);
		bool lockedPosition = false;
		for (int i = 0; i < this.meteors.Count; i++)
		{
			if (this.meteors[i].state == SallyStagePlayLevelMeteor.State.Leaving)
			{
				this.meteors.Remove(this.meteors[i]);
				i++;
			}
		}
		yield return null;
		for (int j = 0; j < this.meteors.Count; j++)
		{
			if (spawn == this.meteors[j].spawnPosition)
			{
				index = j;
				lockedPosition = true;
				break;
			}
		}
		bool positionTaken = false;
		int meteorCounter = 0;
		int spawnStringCounter = 0;
		while (lockedPosition)
		{
			while (meteorCounter < this.meteors.Count)
			{
				meteorCounter++;
				if (spawn == this.meteors[index].spawnPosition)
				{
					positionTaken = true;
				}
				index = (index + 1) % this.meteors.Count;
			}
			if (!positionTaken)
			{
				lockedPosition = false;
				break;
			}
			this.meteorSpawnIndex = (this.meteorSpawnIndex + 1) % meteorSpawnString.Length;
			Parser.FloatTryParse(meteorSpawnString[this.meteorSpawnIndex], out spawn);
			spawnStringCounter++;
			if (spawnStringCounter >= meteorSpawnString.Length)
			{
				break;
			}
			meteorCounter = 0;
			positionTaken = false;
			yield return null;
		}
		if (this.meteors.Count <= 0)
		{
			lockedPosition = false;
		}
		if (!lockedPosition)
		{
			this.meteors.Add(this.meteorPrefab.Create(spawn, (float)p.meteorHP, p));
			this.meteorSpawnIndex = (this.meteorSpawnIndex + 1) % meteorSpawnString.Length;
		}
		base.animator.SetBool("OnPh3Attack", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.general.attackDelayRange.RandomFloat());
		this.state = SallyStagePlayLevelAngel.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x060025B4 RID: 9652 RVA: 0x000C76C4 File Offset: 0x000C58C4
	public IEnumerator tidal_wave_cr()
	{
		this.state = SallyStagePlayLevelAngel.State.Wave;
		LevelProperties.SallyStagePlay.Tidal p = base.properties.CurrentState.tidal;
		this.wave.StartWave(p);
		while (this.wave.isMoving)
		{
			yield return null;
		}
		base.animator.SetBool("OnPh3Attack", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.tidal.tidalHesitate);
		yield return base.animator.WaitForAnimationToEnd(this, "Phase3_Attack", false, false);
		this.state = SallyStagePlayLevelAngel.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x060025B5 RID: 9653 RVA: 0x000C76E0 File Offset: 0x000C58E0
	public void OnPhase4()
	{
		AudioManager.Stop("sally_sally_lightning_move_loop");
		this.StopAllCoroutines();
		base.StartCoroutine(this.slide_out_cr());
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		base.animator.SetTrigger("OnPh3Death");
		base.StartCoroutine(this.start_phase_4_cr());
	}

	// Token: 0x060025B6 RID: 9654 RVA: 0x000C7734 File Offset: 0x000C5934
	public IEnumerator start_phase_4_cr()
	{
		base.GetComponent<SpriteRenderer>().material = this.phase4Material;
		for (int i = 0; i < this.meteors.Count; i++)
		{
			if (this.meteors[i] != null)
			{
				this.meteors[i].MeteorChangePhase();
			}
		}
		float t = 0f;
		float time = 2.5f;
		Vector3 endPos = new Vector3(base.transform.position.x, 860f);
		Vector2 start = base.transform.position;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		if (this.killedHusband)
		{
			this.husband.Dead();
			base.StartCoroutine(this.husband.move_cr());
		}
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		foreach (GameObject gameObject in this.shadows)
		{
			gameObject.SetActive(false);
		}
		base.animator.Play("Phase4_Idle");
		yield return CupheadTime.WaitForSeconds(this, 1f);
		t = 0f;
		time = 1f;
		Vector3 pos = base.transform.position;
		pos.x = -640f + base.transform.GetComponent<Renderer>().bounds.size.x / 2f;
		base.transform.position = pos;
		endPos = new Vector3(base.transform.position.x, this.phase4Root.position.y);
		start = base.transform.position;
		while (t < time)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, endPos, val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.spawn_roses_cr());
		this.SpawnUmbrella();
		yield return null;
		yield break;
	}

	// Token: 0x060025B7 RID: 9655 RVA: 0x000C7750 File Offset: 0x000C5950
	public void SpawnUmbrella()
	{
		SallyStagePlayLevelUmbrella sallyStagePlayLevelUmbrella = Object.Instantiate<SallyStagePlayLevelUmbrella>(this.umbrellaPrefab);
		sallyStagePlayLevelUmbrella.GetProperties(base.properties);
		sallyStagePlayLevelUmbrella.EnableHoming = false;
		float num = (!Rand.Bool()) ? 140f : -140f;
		sallyStagePlayLevelUmbrella.transform.position = new Vector2(num, 460f);
		base.StartCoroutine(this.umbrella_cr(sallyStagePlayLevelUmbrella));
	}

	// Token: 0x060025B8 RID: 9656 RVA: 0x000C77C0 File Offset: 0x000C59C0
	public IEnumerator umbrella_cr(SallyStagePlayLevelUmbrella umbrella)
	{
		for (;;)
		{
			umbrella.TrackingPlayer = PlayerManager.GetNext();
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.umbrella.homingUntilSwitchPlayer);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060025B9 RID: 9657 RVA: 0x000C77E4 File Offset: 0x000C59E4
	public IEnumerator move_cr()
	{
		float t = 0f;
		float time = base.properties.CurrentState.general.finalMovementSpeed;
		float sizeX = base.transform.GetComponent<Renderer>().bounds.size.x / 2f;
		EaseUtils.EaseType ease = EaseUtils.EaseType.easeInOutSine;
		float start = -640f + sizeX;
		float end = 640f - sizeX;
		for (;;)
		{
			t = 0f;
			while (t < time)
			{
				float val = t / time;
				base.transform.SetPosition(new float?(EaseUtils.Ease(ease, start, end, val)), null, null);
				t += CupheadTime.Delta;
				yield return null;
			}
			base.transform.SetPosition(new float?(end), null, null);
			t = 0f;
			while (t < time)
			{
				float val2 = t / time;
				base.transform.SetPosition(new float?(EaseUtils.Ease(ease, end, start, val2)), null, null);
				t += CupheadTime.Delta;
				yield return null;
			}
			base.transform.SetPosition(new float?(start), null, null);
		}
		yield break;
	}

	// Token: 0x060025BA RID: 9658 RVA: 0x000C7800 File Offset: 0x000C5A00
	public IEnumerator spawn_roses_cr()
	{
		LevelProperties.SallyStagePlay.Roses p = base.properties.CurrentState.roses;
		string[] roseString = p.spawnString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int roseIndex = Random.Range(0, roseString.Length);
		float yCoord = 460f;
		float xCoord = 0f;
		int maxCount = p.playerAimRange.RandomInt();
		int counter = 0;
		for (;;)
		{
			if (counter < maxCount)
			{
				Parser.FloatTryParse(roseString[roseIndex], out xCoord);
				counter++;
			}
			else
			{
				AbstractPlayerController next = PlayerManager.GetNext();
				xCoord = next.transform.position.x;
				counter = 0;
				maxCount = p.playerAimRange.RandomInt();
			}
			Vector3 position = new Vector3(-640f + xCoord, yCoord);
			this.applauseHandler.ThrowRose(position, p);
			roseIndex = (roseIndex + 1) % roseString.Length;
			yield return CupheadTime.WaitForSeconds(this, p.spawnDelayRange.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060025BB RID: 9659 RVA: 0x000C781C File Offset: 0x000C5A1C
	public void OnEasyDeath()
	{
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		if (this.killedHusband)
		{
			this.husband.GetComponent<Animator>().SetTrigger("OnDeath");
		}
		base.animator.SetTrigger("OnPh3Death");
	}

	// Token: 0x060025BC RID: 9660 RVA: 0x0001FB2E File Offset: 0x0001DD2E
	public void OnDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.sally_angel_death_sound_cr());
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnPh4Death");
		base.StartCoroutine(this.birds_death_cr());
	}

	// Token: 0x060025BD RID: 9661 RVA: 0x000C786C File Offset: 0x000C5A6C
	public IEnumerator sally_angel_death_sound_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		AudioManager.Play("sally_p4_angel_death_vox");
		yield break;
	}

	// Token: 0x060025BE RID: 9662 RVA: 0x000C7888 File Offset: 0x000C5A88
	public IEnumerator birds_death_cr()
	{
		float t = 0f;
		float time = 2f;
		Vector3 pos = this.birdsDeath.transform.position;
		this.birdsDeath.SetActive(true);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			pos.y = Mathf.Lerp(pos.y, this.birdRoot.transform.position.y, val);
			this.birdsDeath.transform.position = pos;
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060025BF RID: 9663 RVA: 0x0001FB6C File Offset: 0x0001DD6C
	public void SoundAngelIdle()
	{
		AudioManager.Play("sally_angel_idle");
		this.emitAudioFromObject.Add("sally_angel_idle");
	}

	// Token: 0x060025C0 RID: 9664 RVA: 0x0001FB88 File Offset: 0x0001DD88
	public void SoundAngelDeath()
	{
		AudioManager.Play("sally_angel_death");
		this.emitAudioFromObject.Add("sally_angel_death");
	}

	// Token: 0x04001F28 RID: 7976
	public static float extraHP;

	// Token: 0x04001F29 RID: 7977
	[SerializeField]
	public Material phase4Material;

	// Token: 0x04001F2A RID: 7978
	[SerializeField]
	public SallyStagePlayApplauseHandler applauseHandler;

	// Token: 0x04001F2B RID: 7979
	[SerializeField]
	public Animator sign;

	// Token: 0x04001F2C RID: 7980
	[SerializeField]
	public SallyStagePlayLevelWave wave;

	// Token: 0x04001F2D RID: 7981
	[SerializeField]
	public SallyStagePlayLevelMeteor meteorPrefab;

	// Token: 0x04001F2E RID: 7982
	[SerializeField]
	public SallyStagePlayLevelLightning lightningPrefab;

	// Token: 0x04001F2F RID: 7983
	[SerializeField]
	public SallyStagePlayLevelUmbrella umbrellaPrefab;

	// Token: 0x04001F30 RID: 7984
	[SerializeField]
	public GameObject birdsDeath;

	// Token: 0x04001F31 RID: 7985
	[SerializeField]
	public SallyStagePlayLevelFianceDeity husband;

	// Token: 0x04001F32 RID: 7986
	[SerializeField]
	public GameObject[] shadows;

	// Token: 0x04001F33 RID: 7987
	[Space(10f)]
	[SerializeField]
	public Transform phase4Root;

	// Token: 0x04001F34 RID: 7988
	[SerializeField]
	public Transform birdRoot;

	// Token: 0x04001F35 RID: 7989
	[SerializeField]
	public Transform phase3Root;

	// Token: 0x04001F37 RID: 7991
	public List<SallyStagePlayLevelMeteor> meteors;

	// Token: 0x04001F38 RID: 7992
	public DamageDealer damageDealer;

	// Token: 0x04001F39 RID: 7993
	public DamageReceiver damageReceiver;

	// Token: 0x04001F3A RID: 7994
	public Vector3 signStart;

	// Token: 0x04001F3B RID: 7995
	public bool killedHusband;

	// Token: 0x04001F3C RID: 7996
	public int nextAttack;

	// Token: 0x04001F3D RID: 7997
	public int lightningShotIndex;

	// Token: 0x04001F3E RID: 7998
	public int lightningAngleIndex;

	// Token: 0x04001F3F RID: 7999
	public int lightningSpawnIndex;

	// Token: 0x04001F40 RID: 8000
	public int lightningMax;

	// Token: 0x04001F41 RID: 8001
	public int lightningMaxCounter;

	// Token: 0x04001F42 RID: 8002
	public int meteorSpawnIndex;

	// Token: 0x02000ED9 RID: 3801
	public enum State
	{
		// Token: 0x04006AF8 RID: 27384
		Idle,
		// Token: 0x04006AF9 RID: 27385
		Lightning,
		// Token: 0x04006AFA RID: 27386
		Wave,
		// Token: 0x04006AFB RID: 27387
		Meteor
	}
}
