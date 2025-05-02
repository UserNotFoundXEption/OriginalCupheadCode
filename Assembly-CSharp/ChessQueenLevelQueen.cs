using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class ChessQueenLevelQueen : LevelProperties.ChessQueen.Entity
{
	// Token: 0x17000256 RID: 598
	// (get) Token: 0x0600130F RID: 4879 RVA: 0x000100D5 File Offset: 0x0000E2D5
	// (set) Token: 0x06001310 RID: 4880 RVA: 0x000100DD File Offset: 0x0000E2DD
	public ChessQueenLevelQueen.States state { get; set; }

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001311 RID: 4881 RVA: 0x000100E6 File Offset: 0x0000E2E6
	// (set) Token: 0x06001312 RID: 4882 RVA: 0x000100EE File Offset: 0x0000E2EE
	public ChessQueenLevelLightning activeLightning { get; set; }

	// Token: 0x06001313 RID: 4883 RVA: 0x00096B9C File Offset: 0x00094D9C
	public override void LevelInit(LevelProperties.ChessQueen properties)
	{
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.onIntroEventHandler;
		LevelProperties.ChessQueen.Turret turret = properties.CurrentState.turret;
		this.cannons = new List<ChessQueenLevelCannon>();
		this.cannonLeft.SetProperties(turret.leftTurretRange.min, turret.leftTurretRange.max, turret.leftTurretRotationTime, ChessQueenLevelCannon.CannonPosition.Side, turret, this);
		this.cannons.Add(this.cannonLeft);
		this.cannonMiddle.SetProperties(turret.middleTurretRange.min, turret.middleTurretRange.max, turret.middleTurretRotationTime, ChessQueenLevelCannon.CannonPosition.Center, turret, this);
		this.cannons.Add(this.cannonMiddle);
		this.cannonRight.SetProperties(turret.rightTurretRange.min, turret.rightTurretRange.max, turret.rightTurretRotationTime, ChessQueenLevelCannon.CannonPosition.Side, turret, this);
		this.cannons.Add(this.cannonRight);
		this.cannonCycleDirection = MathUtils.PlusOrMinus();
		this.activeCannonIndex = ((this.cannonCycleDirection != -1) ? 0 : 2);
		this.cannons[this.activeCannonIndex].IsActive = true;
		base.StartCoroutine(this.check_cannons_cr());
		this.delayPattern = new PatternString(properties.CurrentState.queen.queenAttackDelayString, true, true);
		this.lightningPositionPattern = new PatternString(properties.CurrentState.lightning.lightningPositionString, true, true);
		this.flipPositionString = Rand.Bool();
		this.positionPattern = new PatternString(properties.CurrentState.movement.queenPositionString, true);
		this.SFX_KOG_QUEEN_IntroTypeWriter();
	}

	// Token: 0x06001314 RID: 4884 RVA: 0x00096D40 File Offset: 0x00094F40
	public override void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemyProjectile(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			ChessQueenLevelCannonball component = hit.GetComponent<ChessQueenLevelCannonball>();
			if (component)
			{
				this.receiveDamage();
				this.SFX_KOG_QUEEN_CannonHitQueenDing();
				component.HitQueen();
			}
		}
	}

	// Token: 0x06001315 RID: 4885 RVA: 0x00096D80 File Offset: 0x00094F80
	public void receiveDamage()
	{
		base.properties.DealDamage((!PlayerManager.BothPlayersActive()) ? 10f : ChessKingLevelKing.multiplayerDamageNerf);
		this.hitFlash.Flash(0.7f);
		if (base.properties.CurrentHealth <= 0f)
		{
			this.die();
		}
		else
		{
			this.mouse.HitQueen();
		}
		if (!base.animator.GetBool("OnLightning"))
		{
			this.headWobbleCurrentAmplitude = this.headWobbleAmplitude;
			this.headWobbleTimer = 0f;
		}
	}

	// Token: 0x06001316 RID: 4886 RVA: 0x00096E18 File Offset: 0x00095018
	public void StateChanged()
	{
		this.delayPattern = new PatternString(base.properties.CurrentState.queen.queenAttackDelayString, true, true);
		this.lightningPositionPattern = new PatternString(base.properties.CurrentState.lightning.lightningPositionString, true, true);
		this.positionPattern = new PatternString(base.properties.CurrentState.movement.queenPositionString, true);
	}

	// Token: 0x06001317 RID: 4887 RVA: 0x00096E8C File Offset: 0x0009508C
	public void LateUpdate()
	{
		foreach (SpriteRenderer spriteRenderer in this.dressRenderers)
		{
			spriteRenderer.enabled = (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || base.animator.GetCurrentAnimatorStateInfo(0).IsTag("Egg"));
		}
		this.head.localPosition = new Vector3(Mathf.Sin(this.headWobbleTimer) * this.headWobbleCurrentAmplitude, 0f);
		this.headWobbleTimer += CupheadTime.Delta * this.headWobbleSpeed;
		if (this.headWobbleCurrentAmplitude > 0f)
		{
			this.headWobbleCurrentAmplitude -= CupheadTime.Delta * this.headWobbleDecay;
			if (this.headWobbleCurrentAmplitude < 0f)
			{
				this.headWobbleCurrentAmplitude = 0f;
			}
		}
	}

	// Token: 0x06001318 RID: 4888 RVA: 0x000100F7 File Offset: 0x0000E2F7
	public void onIntroEventHandler()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x00096F88 File Offset: 0x00095188
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.4f);
		base.animator.SetTrigger("Intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro.Start", false, true);
		base.StartCoroutine(this.moving_cr());
		yield break;
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x00096FA4 File Offset: 0x000951A4
	public IEnumerator check_cannons_cr()
	{
		for (;;)
		{
			while (this.cannons[this.activeCannonIndex].IsActive)
			{
				yield return null;
			}
			this.activeCannonIndex = ((this.cannonCycleDirection <= 0) ? MathUtilities.PreviousIndex(this.activeCannonIndex, this.cannons.Count) : MathUtilities.NextIndex(this.activeCannonIndex, this.cannons.Count));
			this.cannons[this.activeCannonIndex].SetActive(true);
		}
		yield break;
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x00096FC0 File Offset: 0x000951C0
	public IEnumerator moving_cr()
	{
		LevelProperties.ChessQueen.Queen p = base.properties.CurrentState.queen;
		float moveSpeed = 0f;
		this.lastXPos = base.transform.position.x;
		base.animator.Play("MoveSlow", 1, 0f);
		base.animator.Update(0f);
		this.isMoving = true;
		bool firstMove = true;
		bool inLightning = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			while (!this.isMoving)
			{
				yield return null;
			}
			float elapsed = 0f;
			float startX = base.transform.position.x;
			float endX = Mathf.Lerp((float)Level.Current.Left + 150f, (float)Level.Current.Right - 150f, (this.positionPattern.PopFloat() * (float)((!this.flipPositionString) ? 1 : -1) + 1f) / 2f);
			if (firstMove && endX > 0f)
			{
				endX = -endX;
			}
			firstMove = false;
			this.movingLeft = (endX < startX);
			float distance = Mathf.Abs(endX - startX);
			float time = distance / p.queenMovementSpeed;
			while (elapsed <= time)
			{
				if (!this.isMoving)
				{
					inLightning = true;
				}
				if (inLightning && this.isMoving)
				{
					inLightning = false;
					startX = base.transform.position.x;
					elapsed = 0f;
					distance = Mathf.Abs(endX - startX);
					int num = 0;
					while (distance < this.minMoveDistanceAfterLightning && num < this.positionPattern.SubStringLength())
					{
						endX = Mathf.Lerp((float)Level.Current.Left + 150f, (float)Level.Current.Right - 150f, (this.positionPattern.PopFloat() * (float)((!this.flipPositionString) ? 1 : -1) + 1f) / 2f);
						distance = Mathf.Abs(endX - startX);
						num++;
					}
					this.movingLeft = (endX < startX);
					time = distance / p.queenMovementSpeed;
					moveSpeed = 0f;
				}
				if ((base.animator.GetCurrentAnimatorStateInfo(1).IsName("MoveSlow") && base.animator.GetCurrentAnimatorStateInfo(1).normalizedTime % 1f < 0.166666672f) || base.animator.GetCurrentAnimatorStateInfo(1).IsName("MoveEaseOut"))
				{
					foreach (SpriteRenderer spriteRenderer in this.dressRenderers)
					{
						spriteRenderer.flipX = !this.movingLeft;
					}
				}
				base.animator.SetBool("Fast", this.isMoving && Mathf.Abs(this.lastXPos - base.transform.position.x) > this.speedThresholdForFastAnimation && this.dressRenderers[0].flipX != this.lastXPos - base.transform.position.x > 0f);
				moveSpeed = Mathf.Clamp(moveSpeed + CupheadTime.FixedDelta * ((!this.isMoving) ? (-this.attackDecel) : this.accel), 0f, 1f);
				float t = elapsed / time;
				float val = (!this.useSineEasing) ? EaseUtils.EaseInOutArbitraryCoefficient(startX, endX, t, this.easeCoefficient) : EaseUtils.EaseInOutSine(startX, endX, t);
				this.lastXPos = base.transform.position.x;
				base.transform.SetPosition(new float?(val), null, null);
				elapsed += CupheadTime.FixedDelta * moveSpeed;
				yield return wait;
			}
		}
		yield break;
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x00010106 File Offset: 0x0000E306
	public void StartLightning()
	{
		this.state = ChessQueenLevelQueen.States.Lightning;
		base.StartCoroutine(this.SFX_KOG_QUEEN_VocalAttack_cr());
		base.StartCoroutine(this.lightning_cr());
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x00096FDC File Offset: 0x000951DC
	public IEnumerator lightning_cr()
	{
		LevelProperties.ChessQueen.Lightning p = base.properties.CurrentState.lightning;
		this.isMoving = false;
		base.animator.SetBool("Fast", false);
		base.animator.SetBool("OnLightning", true);
		this.headWobbleDecay *= 2f;
		yield return CupheadTime.WaitForSeconds(this, p.lightningAnticipationTime);
		base.animator.SetTrigger("OnAttack");
		while (this.activeLightning != null && !this.activeLightning.isGone)
		{
			yield return null;
		}
		base.animator.SetBool("OnLightning", false);
		base.animator.Play("MoveEaseOutHold", 1, 0f);
		base.animator.Update(0f);
		yield return base.animator.WaitForAnimationToEnd(this, "Lightning.Exit", false, true);
		base.animator.Play("MoveSlow", 1, 0f);
		base.animator.Update(0f);
		this.isMoving = true;
		this.headWobbleDecay *= 0.5f;
		yield return CupheadTime.WaitForSeconds(this, this.delayPattern.PopFloat());
		this.state = ChessQueenLevelQueen.States.Idle;
		yield break;
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x00096FF8 File Offset: 0x000951F8
	public void AniEvent_CreateLightning()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		this.lightningPositionPattern.IncrementString();
		this.activeLightning = this.lightningPrefab.Spawn<ChessQueenLevelLightning>();
		this.activeLightning.Create((this.lightningPositionPattern.GetString()[0] != 'P') ? this.lightningPositionPattern.GetFloat() : next.transform.position.x, base.properties.CurrentState.lightning);
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x00010129 File Offset: 0x0000E329
	public void StartEgg()
	{
		this.state = ChessQueenLevelQueen.States.Egg;
		base.animator.SetBool("Egg", true);
		base.StartCoroutine(this.egg_cr());
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x00097080 File Offset: 0x00095280
	public IEnumerator egg_cr()
	{
		LevelProperties.ChessQueen.Egg p = base.properties.CurrentState.egg;
		yield return base.animator.WaitForAnimationToStart(this, "Egg.AttackLoop", false);
		this.SFX_KOG_QUEEN_FabergeEggLoop();
		this.SFX_KOG_QUEEN_FabergeEggTeethLoop();
		float rateTime = 0f;
		float attackTime = 0f;
		float attackDuration = p.eggAttackDuration.RandomFloat();
		this.eggRootLeft.SetPosition(null, null, new float?(5E-07f));
		this.eggRootRight.SetPosition(null, null, new float?(0f));
		while (attackTime < attackDuration)
		{
			attackTime += CupheadTime.Delta;
			if (rateTime > p.eggFireRate)
			{
				this.fireProjectiles();
				rateTime = 0f;
			}
			else
			{
				rateTime += CupheadTime.Delta;
			}
			yield return null;
		}
		float delay = this.delayPattern.PopFloat();
		if (p.eggCooldownDuration + delay < this.maxTimeToHoldForTwoEggAttacks && ((ChessQueenLevel)Level.Current).NextPatternIsEgg())
		{
			if (p.eggCooldownDuration + delay > this.maxTimeToStayOpenForTwoEggAttacks)
			{
				base.animator.SetTrigger("ResetEgg");
				base.animator.SetTrigger("EndAttack");
			}
			yield return CupheadTime.WaitForSeconds(this, p.eggCooldownDuration + delay);
			this.StartEgg();
			yield break;
		}
		base.animator.SetTrigger("EndAttack");
		this.SFX_KOG_QUEEN_FabergeEggLoopStopShort();
		yield return CupheadTime.WaitForSeconds(this, p.eggCooldownDuration);
		base.animator.SetBool("Egg", false);
		this.SFX_KOG_QUEEN_FabergeEggClose();
		this.SFX_KOG_QUEEN_FabergeEggLoopStopEnd();
		this.SFX_KOG_QUEEN_FabergeEggTeethLoopStop();
		yield return base.animator.WaitForAnimationToStart(this, "Egg.End", false);
		yield return CupheadTime.WaitForSeconds(this, delay);
		this.state = ChessQueenLevelQueen.States.Idle;
		yield break;
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x0009709C File Offset: 0x0009529C
	public void fireProjectiles()
	{
		LevelProperties.ChessQueen.Egg egg = base.properties.CurrentState.egg;
		Vector2 zero = Vector2.zero;
		float num = (float)((!this.movingLeft) ? -200 : 200);
		zero.y = egg.eggVelocityY.RandomFloat();
		zero.x = egg.eggVelocityX.RandomFloat() + num;
		this.eggRootLeft.transform.position += Vector3.forward * 1E-06f;
		ChessQueenLevelEgg chessQueenLevelEgg = this.eggPrefab.Spawn<ChessQueenLevelEgg>();
		chessQueenLevelEgg.Create(this.eggRootLeft.position, zero, egg.eggGravity, egg.eggSpawnCollisionTimer);
		zero.y = egg.eggVelocityY.RandomFloat();
		zero.x = egg.eggVelocityX.RandomFloat() + num;
		this.eggRootLeft.transform.position += Vector3.forward * 1E-06f;
		ChessQueenLevelEgg chessQueenLevelEgg2 = this.eggPrefab.Spawn<ChessQueenLevelEgg>();
		chessQueenLevelEgg2.Create(this.eggRootRight.position, zero, egg.eggGravity, egg.eggSpawnCollisionTimer);
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x000971E0 File Offset: 0x000953E0
	public void die()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.mouse.Win();
		this.headWobbleCurrentAmplitude = 0f;
		this.StopAllCoroutines();
		if (base.transform.position.x > 0f)
		{
			base.transform.SetScale(new float?(-1f), null, null);
		}
		LevelBossDeathExploder component = base.GetComponent<LevelBossDeathExploder>();
		component.offset.x = component.offset.x * base.transform.localScale.x;
		base.animator.Play("Death");
		base.StartCoroutine(this.SFX_KOG_QUEEN_Death_cr());
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x00010150 File Offset: 0x0000E350
	public void SFX_KOG_QUEEN_IntroTypeWriter()
	{
		AudioManager.Play("sfx_dlc_kog_queen_introtypewriter");
	}

	// Token: 0x06001324 RID: 4900 RVA: 0x0001015C File Offset: 0x0000E35C
	public void AnimationEvent_SFX_KOG_QUEEN_IntroTableFlip()
	{
		AudioManager.Play("sfx_dlc_kog_queen_introtableflip");
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x00010168 File Offset: 0x0000E368
	public void AnimationEvent_SFX_KOG_QUEEN_FabergeEggOpen()
	{
		AudioManager.Play("sfx_dlc_kog_queen_fabergeegg_open");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_fabergeegg_open");
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x00010184 File Offset: 0x0000E384
	public void SFX_KOG_QUEEN_FabergeEggClose()
	{
		AudioManager.Play("sfx_dlc_kog_queen_fabergeegg_close");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_fabergeegg_close");
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x000101A0 File Offset: 0x0000E3A0
	public void SFX_KOG_QUEEN_FabergeEggTeethLoop()
	{
		AudioManager.Play("sfx_dlc_kog_queen_fabergeeggteeth_loop");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_fabergeeggteeth_loop");
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x000101BC File Offset: 0x0000E3BC
	public void SFX_KOG_QUEEN_FabergeEggTeethLoopStop()
	{
		AudioManager.Stop("sfx_dlc_kog_queen_fabergeeggteeth_loop");
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x000101C8 File Offset: 0x0000E3C8
	public void SFX_KOG_QUEEN_FabergeEggLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_kog_queen_fabergeegg_loop");
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_queen_fabergeegg_loop", 0.7f, 2f);
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_fabergeegg_loop");
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x000101F8 File Offset: 0x0000E3F8
	public void SFX_KOG_QUEEN_FabergeEggLoopStopShort()
	{
		AudioManager.Stop("sfx_dlc_kog_queen_fabergeegg_loop");
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x00010204 File Offset: 0x0000E404
	public void SFX_KOG_QUEEN_FabergeEggLoopStopEnd()
	{
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_queen_fabergeegg_loop", 0f, 1f);
	}

	// Token: 0x0600132C RID: 4908 RVA: 0x0001021A File Offset: 0x0000E41A
	public void AnimationEvent_SFX_KOG_QUEEN_SpawnChessPieces()
	{
		AudioManager.Play("sfx_dlc_kog_queen_spawnchesspieces");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_spawnchesspieces");
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x000972A8 File Offset: 0x000954A8
	public IEnumerator SFX_KOG_QUEEN_Death_cr()
	{
		this.SFX_KOG_QUEEN_FabergeEggLoopStopShort();
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		AudioManager.Play("sfx_dlc_kog_queen_death");
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("sfx_dlc_kog_queen_vocal_death");
		yield return CupheadTime.WaitForSeconds(this, 0.7f);
		AudioManager.PlayLoop("sfx_dlc_kog_queen_deathcrownspin_loop");
		yield break;
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x000972C4 File Offset: 0x000954C4
	public IEnumerator SFX_KOG_QUEEN_VocalAttack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0f);
		AudioManager.Play("sfx_dlc_kog_queen_vocal_attack");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_vocal_attack");
		yield break;
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x00010236 File Offset: 0x0000E436
	public void AnimationEvent_SFX_KOG_QUEEN_VocalHurt()
	{
		AudioManager.Play("sfx_dlc_kog_queen_vocal_hurt");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_vocal_hurt");
	}

	// Token: 0x06001330 RID: 4912 RVA: 0x00010252 File Offset: 0x0000E452
	public void AnimationEvent_SFX_KOG_QUEEN_VocalLaughLrg()
	{
		AudioManager.Play("sfx_dlc_kog_queen_vocal_laughlrg");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_vocal_laughlrg");
	}

	// Token: 0x06001331 RID: 4913 RVA: 0x0001026E File Offset: 0x0000E46E
	public void AnimationEvent_SFX_KOG_QUEEN_VocalLaughSml()
	{
		AudioManager.Play("sfx_dlc_kog_queen_vocal_laughSml");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_vocal_laughSml");
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x0001028A File Offset: 0x0000E48A
	public void SFX_KOG_QUEEN_CannonHitQueenDing()
	{
		AudioManager.Play("sfx_dlc_kog_queen_cannonhitqueending");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_cannonhitqueending");
	}

	// Token: 0x04000F64 RID: 3940
	[SerializeField]
	public SpriteRenderer[] dressRenderers;

	// Token: 0x04000F65 RID: 3941
	[SerializeField]
	public float easeCoefficient;

	// Token: 0x04000F66 RID: 3942
	[SerializeField]
	public float accel = 2f;

	// Token: 0x04000F67 RID: 3943
	[SerializeField]
	public float attackDecel = 2f;

	// Token: 0x04000F68 RID: 3944
	[SerializeField]
	public bool useSineEasing;

	// Token: 0x04000F69 RID: 3945
	[SerializeField]
	public float minMoveDistanceAfterLightning = 100f;

	// Token: 0x04000F6A RID: 3946
	public bool flipPositionString;

	// Token: 0x04000F6B RID: 3947
	[SerializeField]
	public HitFlash hitFlash;

	// Token: 0x04000F6C RID: 3948
	[SerializeField]
	public ChessQueenLevelCannon cannonLeft;

	// Token: 0x04000F6D RID: 3949
	[SerializeField]
	public ChessQueenLevelCannon cannonMiddle;

	// Token: 0x04000F6E RID: 3950
	[SerializeField]
	public ChessQueenLevelCannon cannonRight;

	// Token: 0x04000F6F RID: 3951
	[SerializeField]
	public Transform head;

	// Token: 0x04000F70 RID: 3952
	[SerializeField]
	public ChessQueenLevelLooseMouse mouse;

	// Token: 0x04000F71 RID: 3953
	public float headWobbleCurrentAmplitude;

	// Token: 0x04000F72 RID: 3954
	public float headWobbleTimer;

	// Token: 0x04000F73 RID: 3955
	[SerializeField]
	public float headWobbleSpeed = 50f;

	// Token: 0x04000F74 RID: 3956
	[SerializeField]
	public float headWobbleAmplitude = 25f;

	// Token: 0x04000F75 RID: 3957
	[SerializeField]
	public float headWobbleDecay = 50f;

	// Token: 0x04000F76 RID: 3958
	[Header("Egg")]
	[SerializeField]
	public ChessQueenLevelEgg eggPrefab;

	// Token: 0x04000F77 RID: 3959
	[SerializeField]
	public Transform eggRootRight;

	// Token: 0x04000F78 RID: 3960
	[SerializeField]
	public Transform eggRootLeft;

	// Token: 0x04000F79 RID: 3961
	[SerializeField]
	public float maxTimeToHoldForTwoEggAttacks;

	// Token: 0x04000F7A RID: 3962
	[SerializeField]
	public float maxTimeToStayOpenForTwoEggAttacks = 0.7f;

	// Token: 0x04000F7B RID: 3963
	[Header("Lightning")]
	[SerializeField]
	public ChessQueenLevelLightning lightningPrefab;

	// Token: 0x04000F7C RID: 3964
	[SerializeField]
	public float lightningDisableRange = 150f;

	// Token: 0x04000F7D RID: 3965
	public float lastXPos;

	// Token: 0x04000F7E RID: 3966
	public int cannonCycleDirection;

	// Token: 0x04000F81 RID: 3969
	public List<ChessQueenLevelCannon> cannons;

	// Token: 0x04000F82 RID: 3970
	public int activeCannonIndex;

	// Token: 0x04000F83 RID: 3971
	public PatternString delayPattern;

	// Token: 0x04000F84 RID: 3972
	public PatternString lightningPositionPattern;

	// Token: 0x04000F85 RID: 3973
	public PatternString positionPattern;

	// Token: 0x04000F86 RID: 3974
	public bool movingLeft;

	// Token: 0x04000F87 RID: 3975
	public bool isMoving;

	// Token: 0x04000F88 RID: 3976
	public bool dead;

	// Token: 0x04000F89 RID: 3977
	public float speedThresholdForFastAnimation;

	// Token: 0x02000ADA RID: 2778
	public enum States
	{
		// Token: 0x04004F97 RID: 20375
		Idle,
		// Token: 0x04004F98 RID: 20376
		Lightning,
		// Token: 0x04004F99 RID: 20377
		Egg
	}
}
