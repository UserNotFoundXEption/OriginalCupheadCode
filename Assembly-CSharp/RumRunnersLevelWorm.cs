using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000353 RID: 851
public class RumRunnersLevelWorm : LevelProperties.RumRunners.Entity
{
	// Token: 0x17000316 RID: 790
	// (get) Token: 0x0600256E RID: 9582 RVA: 0x0001F888 File Offset: 0x0001DA88
	// (set) Token: 0x0600256F RID: 9583 RVA: 0x0001F890 File Offset: 0x0001DA90
	public bool introDrop { get; set; }

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06002570 RID: 9584 RVA: 0x0001F899 File Offset: 0x0001DA99
	// (set) Token: 0x06002571 RID: 9585 RVA: 0x0001F8A1 File Offset: 0x0001DAA1
	public bool isDead { get; set; }

	// Token: 0x06002572 RID: 9586 RVA: 0x000C6BE4 File Offset: 0x000C4DE4
	public override void Awake()
	{
		base.Awake();
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.boxCollider.enabled = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002573 RID: 9587 RVA: 0x0001F8AA File Offset: 0x0001DAAA
	public override void LevelInit(LevelProperties.RumRunners properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06002574 RID: 9588 RVA: 0x000C6C40 File Offset: 0x000C4E40
	public void Setup()
	{
		base.gameObject.SetActive(true);
		this.phonographPos = base.transform.position;
		Vector3 position = this.laserGroup1.transform.parent.position;
		Vector3 position2;
		position2..ctor(base.transform.position.x, 720f);
		base.transform.position = position2;
		this.diamond.transform.position = this.phonographPos;
		this.laserGroup1.transform.parent.position = position;
	}

	// Token: 0x06002575 RID: 9589 RVA: 0x000C6CD8 File Offset: 0x000C4ED8
	public void StartWorm(float introDamage)
	{
		this.bossMaxHealth = base.properties.CurrentHealth;
		if (introDamage > 0f)
		{
			base.properties.DealDamage(introDamage * base.properties.CurrentState.worm.introDamageMultiplier);
			this.GetNewSpeed();
		}
		this.diamond.transform.parent = null;
		this.laserGroup1.transform.parent.parent = null;
		this.speed = base.properties.CurrentState.worm.rotationSpeedRange.min;
		base.StartCoroutine(this.bugIntro_cr());
	}

	// Token: 0x06002576 RID: 9590 RVA: 0x000C6D80 File Offset: 0x000C4F80
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (!this.canTakeDamage)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
		this.GetNewSpeed();
		if (Level.Current.mode == Level.Mode.Easy && !this.isDead && base.properties.CurrentHealth <= 0f)
		{
			this.StartDeath();
		}
	}

	// Token: 0x06002577 RID: 9591 RVA: 0x0001F8B3 File Offset: 0x0001DAB3
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06002578 RID: 9592 RVA: 0x0001F8CA File Offset: 0x0001DACA
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002579 RID: 9593 RVA: 0x0001F8E2 File Offset: 0x0001DAE2
	public void StartBarrels()
	{
		this.runnersCoroutine = base.StartCoroutine(this.spawnRunners_cr());
	}

	// Token: 0x0600257A RID: 9594 RVA: 0x0001F8F6 File Offset: 0x0001DAF6
	public void AniEvent_StartBug()
	{
		base.StartCoroutine(this.revealLaser_cr());
		this.lasersChangeCoroutine = base.StartCoroutine(this.lasersChangeDir_cr());
		this.lasersTurnOnCoroutine = base.StartCoroutine(this.lasersTurnOn_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600257B RID: 9595 RVA: 0x000C6DE8 File Offset: 0x000C4FE8
	public IEnumerator bugIntro_cr()
	{
		this.boxCollider.enabled = true;
		this.canTakeDamage = true;
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 startPos = new Vector3(base.transform.position.x, 720f);
		Vector3 endPos = this.phonographPos;
		this.offscreenPos = startPos;
		base.transform.position = startPos;
		base.animator.Play(0, 0, 0.2f);
		float elapsedTime = 0f;
		bool canDrop = false;
		while (!canDrop)
		{
			if (this.introDrop)
			{
				float normalizedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
				canDrop = (normalizedTime >= 0.9f || normalizedTime <= 0.1f);
			}
			elapsedTime += CupheadTime.FixedDelta;
			base.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / 1.65f);
			yield return wait;
		}
		base.animator.SetTrigger("Continue");
		elapsedTime = 0f;
		bool dustSpawned = false;
		float dropPosition = base.transform.position.y;
		while (elapsedTime < 0.6f)
		{
			elapsedTime += CupheadTime.FixedDelta;
			float t = elapsedTime / 0.6f;
			Vector3 position = base.transform.position;
			float startPosition = dropPosition;
			if (t >= 0.363636374f)
			{
				startPosition = (dropPosition - endPos.y) * 0.6f + endPos.y;
			}
			position.y = EaseUtils.EaseOutBounce(startPosition, endPos.y, t);
			base.transform.position = position;
			int shadowIndex = Mathf.Clamp(Mathf.RoundToInt(t * 10f), 0, this.dropShadowSprites.Length - 1);
			this.fakePhonographShadowRenderer.sprite = this.dropShadowSprites[shadowIndex];
			this.fakePhonographShadowRenderer.transform.position = endPos;
			if (!dustSpawned && t >= 0.363636374f)
			{
				this.dropDustEffect.Create(endPos);
				dustSpawned = true;
				CupheadLevelCamera.Current.Shake(10f, 0.3f, false);
				this.diamond.GetComponent<Collider2D>().enabled = true;
			}
			yield return wait;
		}
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForNormalizedTime(this, 1f, "IntroEnd1", 0, true, false, true);
		this.fakePhonographShadowRenderer.sprite = null;
		base.animator.Play("IntroEnd2");
		this.diamond.animator.Play("Slack", 0);
		yield return this.diamond.animator.WaitForNormalizedTime(this, 1f, "Slack", 0, true, false, true);
		this.diamond.animator.Play("Loop", 0);
		this.diamond.animator.Play("Idle", 1);
		yield break;
	}

	// Token: 0x0600257C RID: 9596 RVA: 0x000C6E04 File Offset: 0x000C5004
	public IEnumerator revealLaser_cr()
	{
		this.diamond.StartSparkle();
		this.laserGroup1.Begin();
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.laserGroup2.Begin();
		yield break;
	}

	// Token: 0x0600257D RID: 9597 RVA: 0x000C6E20 File Offset: 0x000C5020
	public IEnumerator lasersChangeDir_cr()
	{
		LevelProperties.RumRunners.Worm p = base.properties.CurrentState.worm;
		string[] directionPattern = p.directionAttackString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int directionIndex = Random.Range(0, directionPattern.Length);
		this.groupSpeed1 = this.speed;
		this.groupSpeed2 = -this.speed;
		base.StartCoroutine(this.lasersRotate_cr());
		while (!this.isDead)
		{
			Parser.IntTryParse(directionPattern[directionIndex], out this.direction);
			if (this.direction == 1)
			{
				this.groupSpeed1 = this.speed;
				this.groupSpeed2 = -this.speed;
			}
			else
			{
				this.groupSpeed1 = -this.speed;
				this.groupSpeed2 = this.speed;
			}
			yield return CupheadTime.WaitForSeconds(this, p.directionTime);
			directionIndex = (directionIndex + 1) % directionPattern.Length;
		}
		yield break;
	}

	// Token: 0x0600257E RID: 9598 RVA: 0x000C6E3C File Offset: 0x000C503C
	public IEnumerator lasersTurnOn_cr()
	{
		LevelProperties.RumRunners.Worm p = base.properties.CurrentState.worm;
		this.MusicSnapshot_StartGreenBeam();
		while (!this.isDead)
		{
			RumRunnersLevelLaser currentLaser = (!Rand.Bool()) ? this.laserGroup2 : this.laserGroup1;
			yield return CupheadTime.WaitForSeconds(this, p.attackOffDurationRange.RandomFloat());
			yield return null;
			if (currentLaser != null)
			{
				currentLaser.Warning();
				this.MusicSnapshot_StartYellowBeam();
			}
			yield return CupheadTime.WaitForSeconds(this, p.warningDuration);
			yield return null;
			if (currentLaser != null)
			{
				this.lasersAttack(currentLaser);
				this.audioWarble.HandleWarble();
				this.MusicSnapshot_StartRedBeam();
			}
			yield return CupheadTime.WaitForSeconds(this, p.attackOnDurationRange.RandomFloat());
			yield return null;
			if (currentLaser != null)
			{
				this.lasersEndAttack(currentLaser);
				this.MusicSnapshot_StartGreenBeam();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600257F RID: 9599 RVA: 0x000C6E58 File Offset: 0x000C5058
	public IEnumerator lasersRotate_cr()
	{
		for (;;)
		{
			if (this.laserGroup1 != null)
			{
				this.laserGroup1.transform.Rotate(Vector3.forward * this.groupSpeed1 * CupheadTime.Delta);
			}
			if (this.laserGroup2 != null)
			{
				this.laserGroup2.transform.Rotate(Vector3.forward * this.groupSpeed2 * CupheadTime.Delta);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002580 RID: 9600 RVA: 0x000C6E74 File Offset: 0x000C5074
	public void GetNewSpeed()
	{
		MinMax rotationSpeedRange = base.properties.CurrentState.worm.rotationSpeedRange;
		float num = base.properties.CurrentHealth / this.bossMaxHealth;
		float num2 = 1f - num;
		this.speed = rotationSpeedRange.min + rotationSpeedRange.max * num2;
		if (this.direction == 1)
		{
			this.groupSpeed1 = this.speed;
			this.groupSpeed2 = -this.speed;
		}
		else
		{
			this.groupSpeed1 = -this.speed;
			this.groupSpeed2 = this.speed;
		}
	}

	// Token: 0x06002581 RID: 9601 RVA: 0x000C6F0C File Offset: 0x000C510C
	public void endLasers()
	{
		base.StopCoroutine(this.lasersTurnOnCoroutine);
		base.StopCoroutine(this.lasersChangeCoroutine);
		this.laserGroup1.CancelWarning();
		this.laserGroup2.CancelWarning();
		this.lasersEndAttack(this.laserGroup1);
		this.lasersEndAttack(this.laserGroup2);
		this.laserGroup1.End();
		this.laserGroup2.End();
		this.diamond.EndSparkle();
	}

	// Token: 0x06002582 RID: 9602 RVA: 0x0001F936 File Offset: 0x0001DB36
	public void lasersAttack(RumRunnersLevelLaser laserGroup)
	{
		laserGroup.Attack();
		this.diamond.SetAttack(true);
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x0001F94A File Offset: 0x0001DB4A
	public void lasersEndAttack(RumRunnersLevelLaser laserGroup)
	{
		laserGroup.EndAttack();
		this.diamond.SetAttack(false);
	}

	// Token: 0x06002584 RID: 9604 RVA: 0x000C6F80 File Offset: 0x000C5180
	public IEnumerator spawnRunners_cr()
	{
		LevelProperties.RumRunners.Barrels p = base.properties.CurrentState.barrels;
		float topDirection = (float)Rand.PosOrNeg();
		bool bottom = false;
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		if (player == null)
		{
			player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		}
		if (player != null)
		{
			Vector3 position = player.transform.position;
			if ((position.x > 0f && topDirection < 0f) || (position.x < 0f && topDirection > 0f))
			{
				bottom = true;
			}
		}
		PatternString barrelDelayPattern = new PatternString(p.barrelDelayString, true);
		PatternString parryString = new PatternString(p.barrelParryString, true);
		while (base.properties.CurrentState.stateName != LevelProperties.RumRunners.States.Anteater)
		{
			bool isCop = (!bottom) ? this.topBarrelCop : this.bottomBarrelCop;
			this.bottomBarrelCop = ((!bottom) ? this.bottomBarrelCop : (!this.bottomBarrelCop));
			this.topBarrelCop = ((!bottom) ? (!this.topBarrelCop) : this.topBarrelCop);
			RumRunnersLevelBarrel r = this.barrelPrefab.InstantiatePrefab<RumRunnersLevelBarrel>();
			bool parryable = !isCop && parryString.PopLetter() == 'P';
			Vector3 spawnPos = (!bottom) ? this.runnerSpawnPointTop.position : this.runnerSpawnPointBottom.position;
			float direction = topDirection * (float)((!bottom) ? 1 : -1);
			spawnPos.x *= direction;
			r.LevelInit(base.properties);
			r.Initialize(direction, spawnPos, this, parryable, isCop);
			bottom = !bottom;
			float delayTime = barrelDelayPattern.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, delayTime);
		}
		yield break;
	}

	// Token: 0x06002585 RID: 9605 RVA: 0x000C6F9C File Offset: 0x000C519C
	public IEnumerator move_cr()
	{
		bool movingOut = true;
		float time = base.properties.CurrentState.worm.moveTime;
		Vector3 startPos = new Vector3(-base.properties.CurrentState.worm.moveDistance / 2f, base.transform.position.y);
		Vector3 endPos = new Vector3(base.properties.CurrentState.worm.moveDistance / 2f, base.transform.position.y);
		float t = time / 2f;
		bool kick = false;
		bool endMove = false;
		this.AnimationEvent_SFX_RUMRUN_BugGirl_Tapdance();
		SpriteRenderer spriteRenderer = base.GetComponent<SpriteRenderer>();
		int initialSortingOrder = spriteRenderer.sortingOrder;
		spriteRenderer.sortingOrder = 10;
		YieldInstruction waitInstruction = new WaitForFixedUpdate();
		bool initialLoop = true;
		while (!endMove)
		{
			float start = (!movingOut) ? endPos.x : startPos.x;
			float end = (!movingOut) ? startPos.x : endPos.x;
			if (initialLoop)
			{
				start = base.transform.position.x;
				t = 0f;
				time /= 2f;
			}
			while (t < time && !this.isDead)
			{
				t += CupheadTime.FixedDelta;
				float val = t / time;
				Vector3 position = base.transform.position;
				position.x = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val);
				position.y = RumRunnersLevel.GroundWalkingPosY(position, this.boxCollider, RumRunnersLevelWorm.PositionYOffset, RumRunnersLevelWorm.PositionYRayLength);
				base.transform.position = position;
				yield return waitInstruction;
				if (!kick && val > 0.7f)
				{
					kick = true;
					string trigger = (end <= 0f) ? "KickLeft" : "KickRight";
					base.animator.SetTrigger(trigger);
				}
			}
			if (this.isDead)
			{
				endMove = true;
				base.StartCoroutine(this.defeat_cr());
			}
			else
			{
				movingOut = !movingOut;
				kick = false;
				t = 0f;
				base.transform.SetPosition(new float?(end), null, null);
				if (initialLoop)
				{
					time *= 2f;
					initialLoop = false;
				}
			}
			yield return null;
		}
		base.StartCoroutine(this.deathMove_cr(initialSortingOrder));
		yield break;
	}

	// Token: 0x06002586 RID: 9606 RVA: 0x000C6FB8 File Offset: 0x000C51B8
	public IEnumerator defeat_cr()
	{
		base.animator.SetBool("EasyMode", Level.Current.mode == Level.Mode.Easy);
		base.animator.Play("Defeat");
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.position.x), 1f);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.diamond.animator.Play("Defeat");
		yield break;
	}

	// Token: 0x06002587 RID: 9607 RVA: 0x000C6FD4 File Offset: 0x000C51D4
	public IEnumerator deathMove_cr(int initialSortingOrder)
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float FALL_SPEED = (Level.Current.mode != Level.Mode.Easy) ? 400f : 200f;
		bool flipped = base.transform.position.x < 0f;
		float POS_X = 612f * Mathf.Sign(base.transform.position.x);
		float fallTime = (Mathf.Abs(POS_X) - Mathf.Abs(base.transform.position.x)) / FALL_SPEED;
		float startPos = base.transform.position.x;
		float endpos = POS_X;
		float elapsedTime = 0f;
		while (elapsedTime < fallTime)
		{
			elapsedTime += CupheadTime.FixedDelta;
			Vector3 position = base.transform.position;
			position.x = Mathf.Lerp(startPos, endpos, elapsedTime / fallTime);
			position.y = RumRunnersLevel.GroundWalkingPosY(position, this.boxCollider, RumRunnersLevelWorm.PositionYOffset, RumRunnersLevelWorm.PositionYRayLength);
			base.transform.position = position;
			yield return wait;
		}
		base.animator.SetTrigger("Continue");
		if (Level.Current.mode == Level.Mode.Easy)
		{
			this.StopAllCoroutines();
			yield break;
		}
		yield return base.animator.WaitForAnimationToEnd(this, "Fall", false, true);
		this.canTakeDamage = false;
		base.GetComponent<HitFlash>().disabled = true;
		base.GetComponent<SpriteRenderer>().sortingOrder = initialSortingOrder;
		elapsedTime = 0f;
		startPos = base.transform.position.x;
		float targetPositionX = (!flipped) ? this.phonographPos.x : -105f;
		base.animator.enabled = false;
		while (elapsedTime < 2f)
		{
			Vector2 position2 = base.transform.position;
			position2.x = Mathf.Lerp(startPos, targetPositionX, elapsedTime / 2f);
			position2.y = RumRunnersLevel.GroundWalkingPosY(position2, this.boxCollider, RumRunnersLevelWorm.PositionYOffset, RumRunnersLevelWorm.PositionYRayLength);
			base.transform.position = position2;
			yield return null;
			base.animator.Update(CupheadTime.Delta);
			elapsedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		}
		base.transform.SetPosition(new float?(targetPositionX), null, null);
		base.animator.enabled = true;
		base.animator.SetBool("Flipped", flipped);
		base.animator.SetTrigger("End");
		string animationName = (!flipped) ? "Jump" : "JumpFlipped";
		yield return base.animator.WaitForNormalizedTime(this, 1f, animationName, 0, true, false, true);
		animationName = ((!flipped) ? "JumpSquish" : "JumpSquishFlipped");
		base.animator.Play(animationName);
		this.diamond.animator.Play((!flipped) ? "DefeatSquish" : "DefeatSquishFlipped");
		yield return base.animator.WaitForNormalizedTime(this, 1f, animationName, 0, true, false, true);
		base.transform.SetPosition(new float?(this.phonographPos.x * Mathf.Sign(base.transform.position.x) + ((!flipped) ? 0f : -6f)), null, null);
		base.animator.Play("Wave");
		this.diamond.GetComponent<Collider2D>().enabled = false;
		elapsedTime = 0f;
		Vector3 start = base.transform.position;
		Vector3 targetPosition = new Vector3(base.transform.position.x, this.offscreenPos.y);
		this.diamond.transform.parent = base.transform;
		base.StartCoroutine(this.exitShadow_cr());
		while (elapsedTime < 0.866f)
		{
			elapsedTime += CupheadTime.FixedDelta;
			base.transform.position = Vector3.Lerp(start, targetPosition, elapsedTime / 0.866f);
			yield return wait;
		}
		this.StopAllCoroutines();
		this.diamond.Die();
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06002588 RID: 9608 RVA: 0x000C6FF8 File Offset: 0x000C51F8
	public IEnumerator exitShadow_cr()
	{
		this.realPhonographShadowRenderer.enabled = false;
		Vector3 position = this.realPhonographShadowRenderer.transform.position;
		float accumulator = 0f;
		int index = 4;
		while (index >= 0)
		{
			this.fakePhonographShadowRenderer.sprite = this.dropShadowSprites[index];
			this.fakePhonographShadowRenderer.transform.position = position;
			yield return null;
			accumulator += CupheadTime.Delta;
			if (accumulator > 0.0416666679f)
			{
				accumulator -= 0.0416666679f;
				index--;
			}
		}
		this.fakePhonographShadowRenderer.sprite = null;
		yield break;
	}

	// Token: 0x06002589 RID: 9609 RVA: 0x000C7014 File Offset: 0x000C5214
	public void StartDeath()
	{
		if (this.isDead)
		{
			return;
		}
		this.AnimationEvent_SFX_RUMRUN_BugGirl_Tapdance_Stop();
		this.SFX_RUMRUN_BugGirl_DieFalltoGround();
		this.MusicSnapshot_RevertToDefault();
		this.isDead = true;
		this.endLasers();
		base.StopCoroutine(this.runnersCoroutine);
		if (Level.Current.mode == Level.Mode.Easy)
		{
			base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		}
	}

	// Token: 0x0600258A RID: 9610 RVA: 0x000C7074 File Offset: 0x000C5274
	public virtual void MusicSnapshot_StartGreenBeam()
	{
		AudioManager.SnapshotTransition(new string[]
		{
			"RumRunners_GreenBeam",
			"Unpaused",
			"Unpaused_1920s"
		}, new float[]
		{
			1f,
			0f,
			0f
		}, 0.5f);
	}

	// Token: 0x0600258B RID: 9611 RVA: 0x000C70CC File Offset: 0x000C52CC
	public virtual void MusicSnapshot_StartYellowBeam()
	{
		AudioManager.SnapshotTransition(new string[]
		{
			"RumRunners_YellowBeam",
			"Unpaused",
			"Unpaused_1920s"
		}, new float[]
		{
			1f,
			0f,
			0f
		}, 0.5f);
	}

	// Token: 0x0600258C RID: 9612 RVA: 0x000C7124 File Offset: 0x000C5324
	public virtual void MusicSnapshot_StartRedBeam()
	{
		AudioManager.SnapshotTransition(new string[]
		{
			"RumRunners_RedBeam",
			"RumRunners_GreenBeam",
			"Unpaused_1920s"
		}, new float[]
		{
			1f,
			0f,
			0f
		}, 0.16f);
	}

	// Token: 0x0600258D RID: 9613 RVA: 0x000C717C File Offset: 0x000C537C
	public virtual void MusicSnapshot_RevertToDefault()
	{
		string[] array = new string[2];
		array[0] = "RumRunners_RedBeam";
		if (SettingsData.Data.vintageAudioEnabled)
		{
			array[1] = "Unpaused_1920s";
		}
		else
		{
			array[1] = "Unpaused";
		}
		AudioManager.SnapshotTransition(array, new float[]
		{
			0f,
			1f
		}, 3f);
	}

	// Token: 0x0600258E RID: 9614 RVA: 0x000C71E0 File Offset: 0x000C53E0
	public IEnumerator StartRedBeamMusicSnapshotWait_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		yield break;
	}

	// Token: 0x0600258F RID: 9615 RVA: 0x0001F95E File Offset: 0x0001DB5E
	public void SFX_RUMRUN_BugGirl_DieFalltoGround()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_DieFalltoGround");
		this.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_DieFalltoGround");
	}

	// Token: 0x06002590 RID: 9616 RVA: 0x0001F97A File Offset: 0x0001DB7A
	public void AnimationEvent_SFX_RUMRUN_BugGirl_DismountJumpLand()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_DismountJumpLand");
		this.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_DismountJumpLand");
	}

	// Token: 0x06002591 RID: 9617 RVA: 0x0001F996 File Offset: 0x0001DB96
	public void AnimationEvent_SFX_RUMRUN_BugGirl_ExitWinch()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_ExitWinch");
		this.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_ExitWinch");
	}

	// Token: 0x06002592 RID: 9618 RVA: 0x0001F9B2 File Offset: 0x0001DBB2
	public void AnimationEvent_SFX_RUMRUN_BugGirl_Tapdance()
	{
		AudioManager.PlayLoop("sfx_dlc_rumrun_p2_buggirl_tapdance");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p2_buggirl_tapdance");
	}

	// Token: 0x06002593 RID: 9619 RVA: 0x0001F9CE File Offset: 0x0001DBCE
	public void AnimationEvent_SFX_RUMRUN_BugGirl_Tapdance_Stop()
	{
		AudioManager.Stop("sfx_dlc_rumrun_p2_buggirl_tapdance");
	}

	// Token: 0x06002594 RID: 9620 RVA: 0x0001F9DA File Offset: 0x0001DBDA
	public void AnimationEvent_SFX_RUMRUN_BugGirl_VocalDismountLaugh()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_VocalDismountLaugh");
		this.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_VocalDismountLaugh");
	}

	// Token: 0x06002595 RID: 9621 RVA: 0x0001F9F6 File Offset: 0x0001DBF6
	public void AnimationEvent_SFX_RUMRUN_BugGirl_VocalExcited()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_VocalExcited");
		this.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_VocalExcited");
	}

	// Token: 0x04001EFF RID: 7935
	public static readonly float PositionYOffset = 20f;

	// Token: 0x04001F00 RID: 7936
	public static readonly float PositionYRayLength = 250f;

	// Token: 0x04001F01 RID: 7937
	[SerializeField]
	public Sprite[] dropShadowSprites;

	// Token: 0x04001F02 RID: 7938
	[SerializeField]
	public SpriteRenderer fakePhonographShadowRenderer;

	// Token: 0x04001F03 RID: 7939
	[SerializeField]
	public SpriteRenderer realPhonographShadowRenderer;

	// Token: 0x04001F04 RID: 7940
	[SerializeField]
	public Effect dropDustEffect;

	// Token: 0x04001F05 RID: 7941
	[SerializeField]
	public RumRunnersLevelLaser laserGroup1;

	// Token: 0x04001F06 RID: 7942
	[SerializeField]
	public RumRunnersLevelLaser laserGroup2;

	// Token: 0x04001F07 RID: 7943
	[SerializeField]
	public RumRunnersLevelDiamond diamond;

	// Token: 0x04001F08 RID: 7944
	[SerializeField]
	public RumRunnersLevelBarrel barrelPrefab;

	// Token: 0x04001F09 RID: 7945
	[SerializeField]
	public Transform runnerSpawnPointTop;

	// Token: 0x04001F0A RID: 7946
	[SerializeField]
	public Transform runnerSpawnPointBottom;

	// Token: 0x04001F0B RID: 7947
	[SerializeField]
	public AudioWarble audioWarble;

	// Token: 0x04001F0C RID: 7948
	public DamageDealer damageDealer;

	// Token: 0x04001F0D RID: 7949
	public DamageReceiver damageReceiver;

	// Token: 0x04001F0E RID: 7950
	public bool canTakeDamage;

	// Token: 0x04001F0F RID: 7951
	public Vector3 phonographPos;

	// Token: 0x04001F10 RID: 7952
	public Vector3 offscreenPos;

	// Token: 0x04001F11 RID: 7953
	public float speed;

	// Token: 0x04001F12 RID: 7954
	public float groupSpeed1;

	// Token: 0x04001F13 RID: 7955
	public float groupSpeed2;

	// Token: 0x04001F14 RID: 7956
	public float bossMaxHealth;

	// Token: 0x04001F15 RID: 7957
	public bool topBarrelCop;

	// Token: 0x04001F16 RID: 7958
	public bool bottomBarrelCop;

	// Token: 0x04001F17 RID: 7959
	public int direction;

	// Token: 0x04001F18 RID: 7960
	public Coroutine runnersCoroutine;

	// Token: 0x04001F19 RID: 7961
	public Coroutine lasersChangeCoroutine;

	// Token: 0x04001F1A RID: 7962
	public Coroutine lasersTurnOnCoroutine;

	// Token: 0x04001F1B RID: 7963
	public BoxCollider2D boxCollider;
}
