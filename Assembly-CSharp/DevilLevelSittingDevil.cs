using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class DevilLevelSittingDevil : LevelProperties.Devil.Entity
{
	// Token: 0x060014AF RID: 5295 RVA: 0x0009A69C File Offset: 0x0009889C
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.leftWallPositionX = this.leftWall.transform.position.x;
		this.rightWallPositionX = this.rightWall.transform.position.x;
	}

	// Token: 0x060014B0 RID: 5296 RVA: 0x00011864 File Offset: 0x0000FA64
	public void Start()
	{
		this.dragonPos = this.dragonHead.transform.position;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060014B1 RID: 5297 RVA: 0x0009A710 File Offset: 0x00098910
	public override void LevelInit(LevelProperties.Devil properties)
	{
		base.LevelInit(properties);
		this.isSpiderAttackNext = Rand.Bool();
		this.spiderOffsets = properties.CurrentState.spider.positionOffset.Split(new char[]
		{
			','
		});
		this.spiderOffsetIndex = Random.Range(0, this.spiderOffsets.Length);
		this.pitchforkPattern = properties.CurrentState.pitchfork.patternString.RandomChoice<string>().Split(new char[]
		{
			','
		});
		this.pitchforkPatternIndex = Random.Range(0, this.pitchforkPattern.Length);
		this.pitchforkTwoFlameWheelSpawner = new DevilLevelPitchforkProjectileSpawner(2, properties.CurrentState.pitchforkTwoFlameWheel.angleOffset);
		this.pitchforkThreeFlameJumperSpawner = new DevilLevelPitchforkProjectileSpawner(3, properties.CurrentState.pitchforkThreeFlameJumper.angleOffset);
		this.pitchforkFourFlameBouncerSpawner = new DevilLevelPitchforkProjectileSpawner(4, properties.CurrentState.pitchforkFourFlameBouncer.angleOffset);
		this.pitchforkFiveFlameSpinnerSpawner = new DevilLevelPitchforkProjectileSpawner(4, properties.CurrentState.pitchforkFiveFlameSpinner.angleOffset);
		this.pitchforkSixFlameRingSpawner = new DevilLevelPitchforkProjectileSpawner(6, properties.CurrentState.pitchforkSixFlameRing.angleOffset);
		if (Level.CurrentMode == Level.Mode.Easy)
		{
			properties.OnBossDeath += this.DeathEasy;
		}
	}

	// Token: 0x060014B2 RID: 5298 RVA: 0x00011889 File Offset: 0x0000FA89
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.demonPrefab = null;
		this.wheelProjectilePrefab = null;
		this.wheelOrbitingProjectilePrefab = null;
		this.jumpingProjectilePrefab = null;
		this.bouncingProjectilePrefab = null;
		this.spinnerOrbitingProjectilePrefab = null;
		this.spinnerProjectilePrefab = null;
		this.ringProjectilePrefab = null;
	}

	// Token: 0x060014B3 RID: 5299 RVA: 0x0009A850 File Offset: 0x00098A50
	public IEnumerator intro_cr()
	{
		this.state = DevilLevelSittingDevil.State.Intro;
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x060014B4 RID: 5300 RVA: 0x000118C9 File Offset: 0x0000FAC9
	public void StartDemons()
	{
		base.StartCoroutine(this.demon_cr());
	}

	// Token: 0x060014B5 RID: 5301 RVA: 0x0009A86C File Offset: 0x00098A6C
	public IEnumerator demon_cr()
	{
		bool fromLeft = Rand.Bool();
		bool playedFirstSound = false;
		yield return CupheadTime.WaitForSeconds(this, 3f);
		while (!this.endPH1)
		{
			yield return null;
			if (playedFirstSound)
			{
				AudioManager.Play("devil_small_flame_imp_spawn");
				this.emitAudioFromObject.Add("devil_small_flame_imp_spawn");
			}
			else
			{
				AudioManager.Play("devil_small_flame_imp_first_spawn");
				this.emitAudioFromObject.Add("devil_small_flame_imp_first_spawn");
				playedFirstSound = true;
			}
			DevilLevelDemon demon = this.demonPrefab.Create((!fromLeft) ? this.rightDemonPeek.position : this.leftDemonPeek.position, (float)((!fromLeft) ? -1 : 1), base.properties.CurrentState.demons.speed, base.properties.CurrentState.demons.hp, this);
			if (fromLeft)
			{
				demon.JumpRoot = this.leftDemonJumpRoot.position;
				demon.RunRoot = this.leftDemonRunRoot.position;
				demon.PillarDestination = this.leftDemonPillar.position;
				demon.FrontSpawn = this.leftDemonFront.position;
			}
			else
			{
				demon.JumpRoot = this.rightDemonJumpRoot.position;
				demon.RunRoot = this.rightDemonRunRoot.position;
				demon.PillarDestination = this.rightDemonPillar.position;
				demon.FrontSpawn = this.rightDemonFront.position;
			}
			fromLeft = !fromLeft;
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.demons.delay);
		}
		yield break;
	}

	// Token: 0x060014B6 RID: 5302 RVA: 0x000118D8 File Offset: 0x0000FAD8
	public void StartClap()
	{
		this.state = DevilLevelSittingDevil.State.Clap;
		base.StartCoroutine(this.clap_cr());
	}

	// Token: 0x060014B7 RID: 5303 RVA: 0x0009A888 File Offset: 0x00098A88
	public IEnumerator clap_cr()
	{
		LevelProperties.Devil.Clap p = base.properties.CurrentState.clap;
		base.animator.SetBool("StartRam", true);
		yield return base.animator.WaitForAnimationToEnd(this, "Ram_Start", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.delay.RandomFloat());
		foreach (DevilLevelDevilArm devilLevelDevilArm in this.arms)
		{
			devilLevelDevilArm.Attack(p.speed);
		}
		while (this.arms[0].state != DevilLevelDevilArm.State.Idle)
		{
			yield return null;
		}
		base.animator.SetBool("StartRam", false);
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x060014B8 RID: 5304 RVA: 0x0009A8A4 File Offset: 0x00098AA4
	public void StartHead()
	{
		this.state = DevilLevelSittingDevil.State.Head;
		if (this.isSpiderAttackNext)
		{
			base.StartCoroutine(this.spider_cr());
		}
		else
		{
			base.StartCoroutine(this.dragon_cr());
		}
		this.isSpiderAttackNext = !this.isSpiderAttackNext;
	}

	// Token: 0x060014B9 RID: 5305 RVA: 0x0009A8F4 File Offset: 0x00098AF4
	public IEnumerator spider_cr()
	{
		base.animator.SetBool("StartSpider", true);
		yield return base.animator.WaitForAnimationToStart(this, "Spider_Start", false);
		AudioManager.Play("devil_spider_head_intro");
		this.emitAudioFromObject.Add("devil_spider_head_intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Spider_Start", false, true);
		LevelProperties.Devil.Spider p = base.properties.CurrentState.spider;
		int numAttacks = p.numAttacks.RandomInt();
		for (int i = 0; i < numAttacks; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, p.entranceDelay.RandomFloat());
			this.spiderOffsetIndex = (this.spiderOffsetIndex + 1) % this.spiderOffsets.Length;
			float offset = 0f;
			Parser.FloatTryParse(this.spiderOffsets[this.spiderOffsetIndex], out offset);
			this.spiderHead.Attack(Mathf.Clamp(PlayerManager.GetNext().center.x + offset, -620f, 620f), p.downSpeed, p.upSpeed);
			while (this.spiderHead.state != DevilLevelSpiderHead.State.Idle)
			{
				yield return null;
			}
		}
		base.animator.SetBool("StartSpider", false);
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x060014BA RID: 5306 RVA: 0x0009A910 File Offset: 0x00098B10
	public IEnumerator dragon_cr()
	{
		base.animator.SetBool("StartDragon", true);
		bool isLeft = Rand.Bool();
		base.animator.SetBool("IsLeft", isLeft);
		this.dragonHead.Attack(this, isLeft);
		LevelProperties.Devil.Dragon p = base.properties.CurrentState.dragon;
		while (this.dragonHead.state != DevilLevelDragonHead.State.Idle)
		{
			yield return null;
		}
		base.animator.SetBool("StartDragon", false);
		yield return base.animator.WaitForAnimationToEnd(this, "Morph_End", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x060014BB RID: 5307 RVA: 0x000118EE File Offset: 0x0000FAEE
	public void DragonStop()
	{
		base.animator.SetTrigger("Continue");
		this.dragonHead.state = DevilLevelDragonHead.State.Stopped;
	}

	// Token: 0x060014BC RID: 5308 RVA: 0x0001190C File Offset: 0x0000FB0C
	public void DragonReverse()
	{
		base.animator.SetTrigger("OnDragonEnd");
	}

	// Token: 0x060014BD RID: 5309 RVA: 0x0001191E File Offset: 0x0000FB1E
	public void ResetPosition()
	{
		this.dragonHead.SetPosition(this.dragonPos);
	}

	// Token: 0x060014BE RID: 5310 RVA: 0x00011931 File Offset: 0x0000FB31
	public void StartPitchfork()
	{
		this.state = DevilLevelSittingDevil.State.Pitchfork;
		base.animator.SetBool("StartTrident", true);
	}

	// Token: 0x060014BF RID: 5311 RVA: 0x0009A92C File Offset: 0x00098B2C
	public void SpawnProjectiles()
	{
		this.StartTridentHeadSFX();
		this.pitchforkPatternIndex = (this.pitchforkPatternIndex + 1) % this.pitchforkPattern.Length;
		int num = 0;
		Parser.IntTryParse(this.pitchforkPattern[this.pitchforkPatternIndex], out num);
		AudioManager.Play("devil_generic_projectile_start");
		this.emitAudioFromObject.Add("devil_generic_projectile_start");
		switch (num)
		{
		case 2:
			base.StartCoroutine(this.pitchforkTwoFlameWheel_cr());
			break;
		case 3:
			base.StartCoroutine(this.pitchforkThreeFlameJumper_cr());
			break;
		case 4:
			base.StartCoroutine(this.pitchforkFourFlameBouncer_cr());
			break;
		case 5:
			base.StartCoroutine(this.pitchforkFiveFlameSpinner_cr());
			break;
		case 6:
			base.StartCoroutine(this.pitchforkSixFlameRing_cr());
			break;
		}
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x0009AA04 File Offset: 0x00098C04
	public Vector2 getPitchforkFiringPos(float angle)
	{
		return new Vector2(0f, base.properties.CurrentState.pitchfork.spawnCenterY) + MathUtils.AngleToDirection(angle) * base.properties.CurrentState.pitchfork.spawnRadius;
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x0001194B File Offset: 0x0000FB4B
	public void StartParts()
	{
		base.animator.Play("Trident_Body", 2);
		base.animator.Play("Trident_Attack", 3);
	}

	// Token: 0x060014C2 RID: 5314 RVA: 0x0001196F File Offset: 0x0000FB6F
	public void StopParts()
	{
		base.animator.SetBool("StartTrident", false);
	}

	// Token: 0x060014C3 RID: 5315 RVA: 0x0009AA58 File Offset: 0x00098C58
	public IEnumerator pitchforkTwoFlameWheel_cr()
	{
		LevelProperties.Devil.PitchforkTwoFlameWheel p = base.properties.CurrentState.pitchforkTwoFlameWheel;
		List<DevilLevelPitchforkWheelProjectile> projectiles = new List<DevilLevelPitchforkWheelProjectile>();
		bool flipDelays = Rand.Bool();
		foreach (float angle in this.pitchforkTwoFlameWheelSpawner.getSpawnAngles())
		{
			bool flag = projectiles.Count == 0;
			if (flipDelays)
			{
				flag = !flag;
			}
			float attackDelay = (!flag) ? p.secondAttackDelay : p.initialtAttackDelay;
			DevilLevelPitchforkWheelProjectile devilLevelPitchforkWheelProjectile = this.wheelProjectilePrefab.Create(this.getPitchforkFiringPos(angle), attackDelay, p.movementSpeed, this);
			this.wheelOrbitingProjectilePrefab.Create(devilLevelPitchforkWheelProjectile, 90f, (float)Rand.PosOrNeg() * p.rotationSpeed, 100f, this);
			projectiles.Add(devilLevelPitchforkWheelProjectile);
		}
		bool allProjectilesFinished = false;
		while (!allProjectilesFinished)
		{
			allProjectilesFinished = true;
			foreach (DevilLevelPitchforkWheelProjectile devilLevelPitchforkWheelProjectile2 in projectiles)
			{
				if (devilLevelPitchforkWheelProjectile2 != null && devilLevelPitchforkWheelProjectile2.state != DevilLevelPitchforkWheelProjectile.State.Returning)
				{
					allProjectilesFinished = false;
				}
			}
			yield return null;
		}
		AudioManager.Play("devil_generic_projectile_stop");
		this.emitAudioFromObject.Add("devil_generic_projectile_stop");
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x060014C4 RID: 5316 RVA: 0x0009AA74 File Offset: 0x00098C74
	public IEnumerator pitchforkThreeFlameJumper_cr()
	{
		LevelProperties.Devil.PitchforkThreeFlameJumper p = base.properties.CurrentState.pitchforkThreeFlameJumper;
		List<DevilLevelPitchforkJumpingProjectile> projectiles = new List<DevilLevelPitchforkJumpingProjectile>();
		foreach (float angle in this.pitchforkThreeFlameJumperSpawner.getSpawnAngles())
		{
			projectiles.Add(this.jumpingProjectilePrefab.Create(this.getPitchforkFiringPos(angle), p.launchAngle, p.launchSpeed, p.gravity, p.numJumps, this));
		}
		projectiles.Shuffle<DevilLevelPitchforkJumpingProjectile>();
		float delay = p.initialAttackDelay.RandomFloat();
		for (int i = 0; i < p.numJumps; i++)
		{
			foreach (DevilLevelPitchforkJumpingProjectile projectile in projectiles)
			{
				yield return CupheadTime.WaitForSeconds(this, delay);
				projectile.Jump();
				delay = p.jumpDelay;
			}
		}
		AudioManager.Play("devil_generic_projectile_stop");
		this.emitAudioFromObject.Add("devil_generic_projectile_stop");
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x060014C5 RID: 5317 RVA: 0x0009AA90 File Offset: 0x00098C90
	public IEnumerator pitchforkFourFlameBouncer_cr()
	{
		LevelProperties.Devil.PitchforkFourFlameBouncer p = base.properties.CurrentState.pitchforkFourFlameBouncer;
		List<DevilLevelPitchforkBouncingProjectile> projectiles = new List<DevilLevelPitchforkBouncingProjectile>();
		float delay = p.initialAttackDelay.RandomFloat();
		foreach (float angle in this.pitchforkFourFlameBouncerSpawner.getSpawnAngles())
		{
			projectiles.Add(this.bouncingProjectilePrefab.Create(this.getPitchforkFiringPos(angle), delay, p.speed, angle, p.numBounces, this, base.properties.CurrentState.pitchfork.dormantDuration));
		}
		projectiles[Random.Range(0, projectiles.Count)].SetParryable(true);
		bool allProjectilesFinished = false;
		while (!allProjectilesFinished)
		{
			allProjectilesFinished = true;
			foreach (DevilLevelPitchforkBouncingProjectile devilLevelPitchforkBouncingProjectile in projectiles)
			{
				if (devilLevelPitchforkBouncingProjectile.BouncesRemaining > 0)
				{
					allProjectilesFinished = false;
				}
			}
			yield return null;
		}
		AudioManager.Play("devil_generic_projectile_stop");
		this.emitAudioFromObject.Add("devil_generic_projectile_stop");
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x060014C6 RID: 5318 RVA: 0x0009AAAC File Offset: 0x00098CAC
	public IEnumerator pitchforkFiveFlameSpinner_cr()
	{
		LevelProperties.Devil.PitchforkFiveFlameSpinner p = base.properties.CurrentState.pitchforkFiveFlameSpinner;
		DevilLevelPitchforkSpinnerProjectile centerProjectile = this.spinnerProjectilePrefab.Create(new Vector2(0f, base.properties.CurrentState.pitchfork.spawnCenterY), p.maxSpeed, p.acceleration, p.attackDuration, this, base.properties.CurrentState.pitchfork.dormantDuration);
		float rotationSpeed = (float)Rand.PosOrNeg() * p.rotationSpeed;
		foreach (float angle in this.pitchforkFiveFlameSpinnerSpawner.getSpawnAngles())
		{
			this.spinnerOrbitingProjectilePrefab.Create(centerProjectile, angle, rotationSpeed, base.properties.CurrentState.pitchfork.spawnRadius, this, base.properties.CurrentState.pitchfork.dormantDuration);
		}
		AudioManager.Play("devil_generic_projectile_stop");
		this.emitAudioFromObject.Add("devil_generic_projectile_stop");
		yield return CupheadTime.WaitForSeconds(this, p.attackDuration + p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x0009AAC8 File Offset: 0x00098CC8
	public IEnumerator pitchforkSixFlameRing_cr()
	{
		LevelProperties.Devil.PitchforkSixFlameRing p = base.properties.CurrentState.pitchforkSixFlameRing;
		List<DevilLevelPitchforkRingProjectile> projectiles = new List<DevilLevelPitchforkRingProjectile>();
		foreach (float angle in this.pitchforkSixFlameRingSpawner.getSpawnAngles())
		{
			projectiles.Add(this.ringProjectilePrefab.Create(this.getPitchforkFiringPos(angle), p.speed, p.groundDuration, this, base.properties.CurrentState.pitchfork.dormantDuration));
		}
		projectiles[Random.Range(0, projectiles.Count)].SetParryable(true);
		yield return CupheadTime.WaitForSeconds(this, p.initialAttackDelay.RandomFloat());
		projectiles[0].Attack();
		projectiles.RemoveAt(0);
		if (Rand.Bool())
		{
			projectiles.Reverse();
		}
		foreach (DevilLevelPitchforkRingProjectile projectile in projectiles)
		{
			yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
			projectile.Attack();
		}
		AudioManager.Play("devil_generic_projectile_stop");
		this.emitAudioFromObject.Add("devil_generic_projectile_stop");
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x0009AAE4 File Offset: 0x00098CE4
	public void DeathEasy()
	{
		if (Level.Current.mode == Level.Mode.Easy)
		{
			base.properties.OnBossDeath -= this.DeathEasy;
			base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		}
		base.animator.Play("DeathEasy");
	}

	// Token: 0x060014C9 RID: 5321 RVA: 0x00011982 File Offset: 0x0000FB82
	public void StartTransform()
	{
		this.endPH1 = true;
		this.state = DevilLevelSittingDevil.State.EndPhase1;
		base.animator.SetTrigger("OnPhase2");
		base.StartCoroutine(this.on_phase_2_cr());
	}

	// Token: 0x060014CA RID: 5322 RVA: 0x0009AB34 File Offset: 0x00098D34
	public IEnumerator on_phase_2_cr()
	{
		yield return base.animator.WaitForAnimationToStart(this, "Death_Start", false);
		if (this.OnPhase1Death != null)
		{
			this.OnPhase1Death();
		}
		yield return base.animator.WaitForAnimationToStart(this, "Death_Hole", false);
		this.middleGround.SetActive(false);
		base.StartCoroutine(this.move_fire_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060014CB RID: 5323 RVA: 0x0009AB50 File Offset: 0x00098D50
	public IEnumerator move_fire_cr()
	{
		AudioManager.PlayLoop("devil_fire_wall");
		this.emitAudioFromObject.Add("devil_fire_wall");
		while (!this.endFire)
		{
			if (this.leftWall.transform.position.x >= -200f)
			{
				break;
			}
			this.leftWall.transform.position += Vector3.right * base.properties.CurrentState.firewall.firewallSpeed * CupheadTime.FixedDelta;
			if (this.rightWall.transform.position.x <= 200f)
			{
				break;
			}
			this.rightWall.transform.position += Vector3.left * base.properties.CurrentState.firewall.firewallSpeed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x060014CC RID: 5324 RVA: 0x000119AF File Offset: 0x0000FBAF
	public void RemoveFire()
	{
		base.StartCoroutine(this.remove_fire_cr());
	}

	// Token: 0x060014CD RID: 5325 RVA: 0x0009AB6C File Offset: 0x00098D6C
	public IEnumerator remove_fire_cr()
	{
		this.endFire = true;
		float t = 0f;
		float time = 1f;
		float startLeftPos = this.leftWall.transform.position.x;
		float startRightPos = this.rightWall.transform.position.x;
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			this.leftWall.transform.SetPosition(new float?(Mathf.Lerp(startLeftPos, this.leftWallPositionX, val)), null, null);
			this.rightWall.transform.SetPosition(new float?(Mathf.Lerp(startRightPos, this.rightWallPositionX, val)), null, null);
			yield return null;
		}
		this.leftWall.transform.SetPosition(new float?(this.leftWallPositionX), null, null);
		this.rightWall.transform.SetPosition(new float?(this.rightWallPositionX), null, null);
		AudioManager.FadeSFXVolume("devil_fire_wall", 0f, 1f);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AudioManager.Stop("devil_fire_wall");
		yield break;
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x000119BE File Offset: 0x0000FBBE
	public void TridentStartSFX()
	{
		AudioManager.Play("devil_trident_start");
		this.emitAudioFromObject.Add("devil_trident_start");
	}

	// Token: 0x060014CF RID: 5327 RVA: 0x000119DA File Offset: 0x0000FBDA
	public void TridentEndSFX()
	{
		AudioManager.Play("devil_trident_end");
		this.emitAudioFromObject.Add("devil_trident_end");
	}

	// Token: 0x060014D0 RID: 5328 RVA: 0x000119F6 File Offset: 0x0000FBF6
	public void TridentAttackSFX()
	{
		AudioManager.Play("devil_trident_attack");
		this.emitAudioFromObject.Add("devil_trident_attack");
	}

	// Token: 0x060014D1 RID: 5329 RVA: 0x00011A12 File Offset: 0x0000FC12
	public void SpiderMorphEndSFX()
	{
		AudioManager.Play("devil_spider_morph_end");
		this.emitAudioFromObject.Add("devil_spider_morph_end");
	}

	// Token: 0x060014D2 RID: 5330 RVA: 0x00011A2E File Offset: 0x0000FC2E
	public void DevilPhase1DeathSFX()
	{
		AudioManager.Play("devil_phase_1_death_start");
		this.emitAudioFromObject.Add("devil_phase_1_death_start");
	}

	// Token: 0x060014D3 RID: 5331 RVA: 0x00011A4A File Offset: 0x0000FC4A
	public void DragonMorphEndSFX()
	{
		AudioManager.Play("devil_dragon_morph_end");
		this.emitAudioFromObject.Add("devil_dragon_morph_end");
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x00011A66 File Offset: 0x0000FC66
	public void HandclapSnakeSFX()
	{
		AudioManager.Play("devil_dragon_start");
		this.emitAudioFromObject.Add("devil_dragon_start");
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x00011A82 File Offset: 0x0000FC82
	public void IntroPupilsSFX()
	{
		AudioManager.Play("devil_intro_pupils");
		this.emitAudioFromObject.Add("devil_intro_pupils");
	}

	// Token: 0x060014D6 RID: 5334 RVA: 0x00011A9E File Offset: 0x0000FC9E
	public void RamMorphStartSFX()
	{
		AudioManager.Play("devil_ram_morph_start");
		this.emitAudioFromObject.Add("devil_ram_morph_start");
	}

	// Token: 0x060014D7 RID: 5335 RVA: 0x00011ABA File Offset: 0x0000FCBA
	public void RamMorphEndSFX()
	{
		AudioManager.Play("devil_ram_morph_end");
		this.emitAudioFromObject.Add("devil_ram_morph_end");
	}

	// Token: 0x060014D8 RID: 5336 RVA: 0x00011AD6 File Offset: 0x0000FCD6
	public void StartTridentHeadSFX()
	{
		AudioManager.Play("devil_trident_head");
		this.emitAudioFromObject.Add("devil_trident_head");
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x00011AF2 File Offset: 0x0000FCF2
	public void EndTridentHeadSFX()
	{
	}

	// Token: 0x060014DA RID: 5338 RVA: 0x00011AF4 File Offset: 0x0000FCF4
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060014DB RID: 5339 RVA: 0x00011B07 File Offset: 0x0000FD07
	public void ShowGoSign()
	{
		this.holeSign.SetActive(true);
	}

	// Token: 0x040010EC RID: 4332
	public DevilLevelSittingDevil.State state;

	// Token: 0x040010ED RID: 4333
	[SerializeField]
	public GameObject middleGround;

	// Token: 0x040010EE RID: 4334
	[SerializeField]
	public DevilLevelGiantHead giantHead;

	// Token: 0x040010EF RID: 4335
	[SerializeField]
	public DevilLevelDemon demonPrefab;

	// Token: 0x040010F0 RID: 4336
	[SerializeField]
	public Transform leftDemonPeek;

	// Token: 0x040010F1 RID: 4337
	[SerializeField]
	public Transform leftDemonJumpRoot;

	// Token: 0x040010F2 RID: 4338
	[SerializeField]
	public Transform leftDemonRunRoot;

	// Token: 0x040010F3 RID: 4339
	[SerializeField]
	public Transform leftDemonPillar;

	// Token: 0x040010F4 RID: 4340
	[SerializeField]
	public Transform leftDemonFront;

	// Token: 0x040010F5 RID: 4341
	[SerializeField]
	public Transform rightDemonPeek;

	// Token: 0x040010F6 RID: 4342
	[SerializeField]
	public Transform rightDemonJumpRoot;

	// Token: 0x040010F7 RID: 4343
	[SerializeField]
	public Transform rightDemonRunRoot;

	// Token: 0x040010F8 RID: 4344
	[SerializeField]
	public Transform rightDemonPillar;

	// Token: 0x040010F9 RID: 4345
	[SerializeField]
	public Transform rightDemonFront;

	// Token: 0x040010FA RID: 4346
	[SerializeField]
	public DevilLevelDevilArm[] arms;

	// Token: 0x040010FB RID: 4347
	[SerializeField]
	public DevilLevelSpiderHead spiderHead;

	// Token: 0x040010FC RID: 4348
	[SerializeField]
	public DevilLevelDragonHead dragonHead;

	// Token: 0x040010FD RID: 4349
	[SerializeField]
	public Transform leftWall;

	// Token: 0x040010FE RID: 4350
	public float leftWallPositionX;

	// Token: 0x040010FF RID: 4351
	[SerializeField]
	public Transform rightWall;

	// Token: 0x04001100 RID: 4352
	public float rightWallPositionX;

	// Token: 0x04001101 RID: 4353
	[SerializeField]
	public DevilLevelPitchforkWheelProjectile wheelProjectilePrefab;

	// Token: 0x04001102 RID: 4354
	[SerializeField]
	public DevilLevelPitchforkOrbitingProjectile wheelOrbitingProjectilePrefab;

	// Token: 0x04001103 RID: 4355
	[SerializeField]
	public DevilLevelPitchforkJumpingProjectile jumpingProjectilePrefab;

	// Token: 0x04001104 RID: 4356
	[SerializeField]
	public DevilLevelPitchforkBouncingProjectile bouncingProjectilePrefab;

	// Token: 0x04001105 RID: 4357
	[SerializeField]
	public DevilLevelPitchforkSpinnerProjectile spinnerProjectilePrefab;

	// Token: 0x04001106 RID: 4358
	[SerializeField]
	public DevilLevelPitchforkOrbitingProjectile spinnerOrbitingProjectilePrefab;

	// Token: 0x04001107 RID: 4359
	[SerializeField]
	public DevilLevelPitchforkRingProjectile ringProjectilePrefab;

	// Token: 0x04001108 RID: 4360
	[SerializeField]
	public GameObject holeSign;

	// Token: 0x04001109 RID: 4361
	public Vector3 dragonPos;

	// Token: 0x0400110A RID: 4362
	public DamageReceiver damageReceiver;

	// Token: 0x0400110B RID: 4363
	public bool isSpiderAttackNext;

	// Token: 0x0400110C RID: 4364
	public bool endFire;

	// Token: 0x0400110D RID: 4365
	public bool endPH1;

	// Token: 0x0400110E RID: 4366
	public int spiderOffsetIndex;

	// Token: 0x0400110F RID: 4367
	public string[] spiderOffsets;

	// Token: 0x04001110 RID: 4368
	public int pitchforkPatternIndex;

	// Token: 0x04001111 RID: 4369
	public string[] pitchforkPattern;

	// Token: 0x04001112 RID: 4370
	public DevilLevelPitchforkProjectileSpawner pitchforkTwoFlameWheelSpawner;

	// Token: 0x04001113 RID: 4371
	public DevilLevelPitchforkProjectileSpawner pitchforkThreeFlameJumperSpawner;

	// Token: 0x04001114 RID: 4372
	public DevilLevelPitchforkProjectileSpawner pitchforkFourFlameBouncerSpawner;

	// Token: 0x04001115 RID: 4373
	public DevilLevelPitchforkProjectileSpawner pitchforkFiveFlameSpinnerSpawner;

	// Token: 0x04001116 RID: 4374
	public DevilLevelPitchforkProjectileSpawner pitchforkSixFlameRingSpawner;

	// Token: 0x04001117 RID: 4375
	public Action OnPhase1Death;

	// Token: 0x02000B3E RID: 2878
	public enum State
	{
		// Token: 0x04005266 RID: 21094
		Intro,
		// Token: 0x04005267 RID: 21095
		Idle,
		// Token: 0x04005268 RID: 21096
		Clap,
		// Token: 0x04005269 RID: 21097
		Head,
		// Token: 0x0400526A RID: 21098
		Pitchfork,
		// Token: 0x0400526B RID: 21099
		EndPhase1
	}
}
