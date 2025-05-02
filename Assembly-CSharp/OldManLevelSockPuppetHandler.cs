using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002E7 RID: 743
public class OldManLevelSockPuppetHandler : LevelProperties.OldMan.Entity
{
	// Token: 0x06002105 RID: 8453 RVA: 0x0001C382 File Offset: 0x0001A582
	public void Start()
	{
		this.transState = OldManLevelSockPuppetHandler.TransitionState.None;
		this.dwarvesObject.gameObject.SetActive(false);
	}

	// Token: 0x06002106 RID: 8454 RVA: 0x0001C39C File Offset: 0x0001A59C
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06002107 RID: 8455 RVA: 0x000B93F0 File Offset: 0x000B75F0
	public void StartPhase2()
	{
		this.dwarvesObject.gameObject.SetActive(true);
		this.sockPuppetLeft.gameObject.SetActive(true);
		this.sockPuppetRight.gameObject.SetActive(true);
		this.damageDealer = DamageDealer.NewEnemy();
		this.sockPuppetRight.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		this.sockPuppetLeft.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		this.sockPuppetRight.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.sockPuppetLeft.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.bounce_ball_cr());
		base.StartCoroutine(this.dwarves_arc_cr());
		((LevelPlayerController)PlayerManager.GetPlayer(PlayerId.PlayerOne)).motor.OnHitEvent += this.TriggerLaugh;
		if (PlayerManager.Multiplayer)
		{
			((LevelPlayerController)PlayerManager.GetPlayer(PlayerId.PlayerTwo)).motor.OnHitEvent += this.TriggerLaugh;
		}
	}

	// Token: 0x06002108 RID: 8456 RVA: 0x0001C3AA File Offset: 0x0001A5AA
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002109 RID: 8457 RVA: 0x0001C3C2 File Offset: 0x0001A5C2
	public override void LevelInit(LevelProperties.OldMan properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x0001C3CB File Offset: 0x0001A5CB
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x0001C3DE File Offset: 0x0001A5DE
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x000B9514 File Offset: 0x000B7714
	public IEnumerator dwarves_arc_cr()
	{
		LevelProperties.OldMan.Dwarf p = base.properties.CurrentState.dwarf;
		PatternString arcAttackDelay = new PatternString(p.arcAttackDelayString, true, true);
		PatternString arcAttackPos = new PatternString(p.arcAttackPosString, true, true);
		PatternString arcShootHeight = new PatternString(p.arcShootHeightString, true, true);
		PatternString parryableString = new PatternString(p.parryString, true);
		int posIndex = 0;
		bool typeA = Rand.Bool();
		yield return CupheadTime.WaitForSeconds(this, 2f);
		for (;;)
		{
			posIndex = arcAttackPos.PopInt();
			if (this.dwarves[posIndex].inPlace)
			{
				this.dwarves[posIndex].ShootInArc(arcShootHeight.PopFloat(), p.arcApex, p.arcHealth, typeA, parryableString.PopLetter() == 'P', p.arcAttackWarningTime);
				typeA = !typeA;
				yield return CupheadTime.WaitForSeconds(this, arcAttackDelay.PopFloat());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x000B9530 File Offset: 0x000B7730
	public IEnumerator bounce_ball_cr()
	{
		LevelProperties.OldMan.Hands p = base.properties.CurrentState.hands;
		PatternString leftHandPosString = new PatternString(p.leftHandPosString, true, true);
		PatternString rightHandPosString = new PatternString(p.rightHandPosString, true, true);
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		this.fromLeft = Rand.Bool();
		if (this.fromLeft)
		{
			this.sockPuppetLeft.AnIEvent_HoldingBall();
		}
		else
		{
			this.sockPuppetRight.AnIEvent_HoldingBall();
		}
		this.sockPuppetLeft.MoveToPos(this.KDpuppetYPositions[this.leftHandPos].position.y, 1f);
		base.StartCoroutine(this.move_level_borders_time_cr(1060, Level.Current.Right, 0.5f));
		yield return CupheadTime.WaitForSeconds(this, 0.45f);
		base.animator.Play("Ph2_Enter");
		yield return base.animator.WaitForAnimationToStart(this, "LookUpLeftAndBack", false);
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		this.sockPuppetRight.MoveToPos(this.DpuppetYPositions[this.rightHandPos].position.y, 1f);
		base.StartCoroutine(this.move_level_borders_time_cr(-Level.Current.Left, 152, 0.5f));
		yield return CupheadTime.WaitForSeconds(this, 0.7f);
		base.animator.Play("LookUpRightAndBack");
		yield return null;
		base.animator.SetTrigger("EndIntroLook");
		bool first = true;
		base.StartCoroutine(this.animate_face_cr());
		for (;;)
		{
			if (!first)
			{
				this.rightHandPosOld = this.rightHandPos;
				this.leftHandPosOld = this.leftHandPos;
				this.rightHandPos = rightHandPosString.PopInt();
				this.leftHandPos = leftHandPosString.PopInt();
				this.sockPuppetLeft.MoveToPos(this.KDpuppetYPositions[this.leftHandPos].position.y, (float)Mathf.Abs(this.leftHandPosOld - this.leftHandPos));
				this.sockPuppetRight.MoveToPos(this.DpuppetYPositions[this.rightHandPos].position.y, (float)Mathf.Abs(this.rightHandPosOld - this.rightHandPos));
			}
			first = false;
			this.sockPuppetRight.animator.SetBool("CanTaunt", this.fromLeft);
			this.sockPuppetLeft.animator.SetBool("CanTaunt", !this.fromLeft);
			while (!this.sockPuppetLeft.ready || !this.sockPuppetRight.ready)
			{
				yield return null;
			}
			this.sockPuppetRight.animator.SetBool("IsCatching", this.fromLeft);
			this.sockPuppetLeft.animator.SetBool("IsCatching", !this.fromLeft);
			yield return CupheadTime.WaitForSeconds(this, p.throwDelay);
			OldManLevelSockPuppet throwingPuppet = (!this.fromLeft) ? this.sockPuppetRight : this.sockPuppetLeft;
			throwingPuppet.animator.SetTrigger("IsThrowing");
			this.sockPuppetRight.animator.SetBool("CanTaunt", false);
			this.sockPuppetLeft.animator.SetBool("CanTaunt", false);
			this.sockPuppetRight.StopTaunt();
			this.sockPuppetLeft.StopTaunt();
			yield return throwingPuppet.animator.WaitForAnimationToEnd(this, "Throw_Start", false, true);
			yield return CupheadTime.WaitForSeconds(this, p.throwWarningTime);
			throwingPuppet.animator.Play("Throw");
			yield return null;
			while (throwingPuppet.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
			{
				yield return null;
			}
			Vector3 startPos = (!this.fromLeft) ? this.sockPuppetRight.throwPosition() : this.sockPuppetLeft.throwPosition();
			Vector3 endPos = (!this.fromLeft) ? this.sockPuppetLeft.catchPosition() : this.sockPuppetRight.catchPosition();
			Vector3 pos = new Vector3(this.mainPlatformCollider.transform.position.x + (float)(this.leftHandPos - this.rightHandPos) * p.bouncePositionSpacing, this.mainPlatformCollider.bounds.max.y);
			this.puppetBall = this.puppetBallPrefab.Spawn<OldManLevelPuppetBall>();
			this.puppetBall.Init(startPos, pos, endPos, p);
			throwingPuppet.animator.Play("Throw_End");
			throwingPuppet.animator.Update(0f);
			throwingPuppet.AnIEvent_NotHoldingBall();
			while (!this.puppetBall.readyToCatch)
			{
				yield return null;
			}
			if (this.fromLeft)
			{
				this.sockPuppetRight.animator.Play("Catch");
				this.sockPuppetRight.animator.Update(0f);
				this.sockPuppetRight.animator.SetBool("IsCatching", false);
			}
			else
			{
				this.sockPuppetLeft.animator.Play("Catch");
				this.sockPuppetLeft.animator.Update(0f);
				this.sockPuppetLeft.animator.SetBool("IsCatching", false);
			}
			this.fromLeft = !this.fromLeft;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x000B954C File Offset: 0x000B774C
	public IEnumerator animate_face_cr()
	{
		float t = 0f;
		float waitTime = 0f;
		bool lookLeft = Rand.Bool();
		PatternString laughString = new PatternString("L,N,N,N,N,N,N,N,L,N,N,N,N,L,N,N,N,N,N,N", true);
		for (;;)
		{
			yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
			if (this.triggerLaugh || laughString.PopLetter() == 'L')
			{
				base.animator.Play("Laugh");
				this.triggerLaugh = false;
				yield return null;
			}
			else
			{
				waitTime = this.idleHoldRange.RandomFloat();
				t = 0f;
				while (t < waitTime && !this.triggerLaugh)
				{
					t += CupheadTime.Delta;
					yield return null;
				}
				int curLook = (!lookLeft) ? this.rightHandPos : this.leftHandPos;
				if (!lookLeft && !this.sockPuppetRight.ready && this.rightHandPos == 2)
				{
					curLook = 1;
				}
				base.animator.Play((!lookLeft) ? ((curLook <= 0) ? "LookRight" : ((curLook <= 1) ? "LookMidRight" : "LookUpRight")) : ((curLook <= 0) ? "LookLeft" : "LookUpLeft"));
				yield return base.animator.WaitForAnimationToStart(this, (!lookLeft) ? ((curLook <= 0) ? "LookRightHold" : ((curLook <= 1) ? "LookMidRightHold" : "LookUpRightHold")) : ((curLook <= 0) ? "LookLeftHold" : "LookUpLeftHold"), false);
				waitTime = this.lookHoldRange.RandomFloat();
				t = 0f;
				while (t < waitTime && (t < this.lookHoldRange.min || ((!lookLeft) ? this.sockPuppetRight.ready : this.sockPuppetLeft.ready)) && !this.triggerLaugh)
				{
					t += CupheadTime.Delta;
					yield return null;
				}
				base.animator.SetTrigger("Continue");
				if (Random.Range(0f, 1f) < this.chanceToSwitchLookSides)
				{
					lookLeft = !lookLeft;
				}
			}
		}
		yield break;
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x000B9568 File Offset: 0x000B7768
	public void TriggerLaugh()
	{
		if (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Laugh"))
		{
			this.triggerLaugh = true;
		}
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x0001C3FC File Offset: 0x0001A5FC
	public void CatchBall()
	{
		this.puppetBall.GetCaught();
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x0001C409 File Offset: 0x0001A609
	public void OnPhase3()
	{
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		this.StopAllCoroutines();
		base.StartCoroutine(this.deathAnimation_cr());
	}

	// Token: 0x06002112 RID: 8466 RVA: 0x000B959C File Offset: 0x000B779C
	public IEnumerator deathAnimation_cr()
	{
		((LevelPlayerController)PlayerManager.GetPlayer(PlayerId.PlayerOne)).motor.OnHitEvent -= this.TriggerLaugh;
		if (PlayerManager.Multiplayer)
		{
			((LevelPlayerController)PlayerManager.GetPlayer(PlayerId.PlayerTwo)).motor.OnHitEvent -= this.TriggerLaugh;
		}
		this.sockPuppetLeft.Die();
		this.sockPuppetRight.Die();
		if (this.puppetBall == null)
		{
			this.puppetBall = this.puppetBallPrefab.Spawn<OldManLevelPuppetBall>();
			this.puppetBall.transform.position = ((!this.fromLeft) ? this.sockPuppetRight.throwPosition() : this.sockPuppetLeft.throwPosition());
		}
		this.puppetBall.Explode();
		foreach (OldManLevelDwarf oldManLevelDwarf in this.dwarves)
		{
			oldManLevelDwarf.Death(true);
		}
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		base.animator.Play("Angry");
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 startPos = this.oldManAngry.localPosition;
		Vector3 endPos = new Vector3(this.oldManAngry.localPosition.x, 200f);
		Vector3 sockPuppetLeftStart = this.sockPuppetLeft.rootPosition;
		Vector3 sockPuppetRightStart = this.sockPuppetRight.rootPosition;
		Vector3 sockPuppetLeftEnd = (Level.Current.mode != Level.Mode.Easy) ? new Vector3(this.sockPuppetLeft.rootPosition.x - 300f, -1100f) : new Vector3(this.sockPuppetLeft.rootPosition.x, this.KDpuppetYPositions[1].position.y);
		Vector3 sockPuppetRightEnd = (Level.Current.mode != Level.Mode.Easy) ? new Vector3(this.sockPuppetRight.rootPosition.x + 300f, -1100f) : new Vector3(this.sockPuppetRight.rootPosition.x, this.DpuppetYPositions[1].position.y);
		yield return CupheadTime.WaitForSeconds(this, (Level.Current.mode != Level.Mode.Easy) ? 2f : 0.1f);
		float t = 0f;
		float time = (Level.Current.mode != Level.Mode.Easy) ? base.properties.CurrentState.hands.endSlideUpTime : 2f;
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			if (Level.Current.mode != Level.Mode.Easy)
			{
				this.oldManAngry.SetPosition(null, new float?(Mathf.Lerp(startPos.y, endPos.y, t / time)), null);
				this.oldManAngryNoseShadow.localPosition = new Vector3(this.oldManAngryNoseShadow.localPosition.x, Mathf.Lerp(0f, 10f, t / time));
			}
			this.sockPuppetLeft.rootPosition = new Vector3(Mathf.Lerp(sockPuppetLeftStart.x, sockPuppetLeftEnd.x, EaseUtils.EaseInSine(0f, 1f, t / time)), Mathf.Lerp(sockPuppetLeftStart.y, sockPuppetLeftEnd.y, EaseUtils.EaseInSine(0f, 1f, t / time)));
			this.sockPuppetRight.rootPosition = new Vector3(Mathf.Lerp(sockPuppetRightStart.x, sockPuppetRightEnd.x, EaseUtils.EaseInSine(0f, 1f, t / time)), Mathf.Lerp(sockPuppetRightStart.y, sockPuppetRightEnd.y, EaseUtils.EaseInSine(0f, 1f, t / time)));
			if (t <= 0.5f && t + CupheadTime.FixedDelta > 0.5f)
			{
				this.sockPuppetLeft.GetComponent<LevelBossDeathExploder>().StopExplosions();
				this.sockPuppetRight.GetComponent<LevelBossDeathExploder>().StopExplosions();
			}
			yield return wait;
		}
		Object.Destroy(this.dwarvesObject.gameObject);
		if (Level.Current.mode != Level.Mode.Easy)
		{
			base.animator.SetTrigger("ContinueDeath");
			while (this.transState != OldManLevelSockPuppetHandler.TransitionState.PlatformDestroyed)
			{
				yield return null;
			}
			yield return base.StartCoroutine(this.move_level_borders_anim_sync_cr(925, 93, 56f));
		}
		yield break;
	}

	// Token: 0x06002113 RID: 8467 RVA: 0x000B95B8 File Offset: 0x000B77B8
	public IEnumerator move_level_borders_time_cr(int left, int right, float time)
	{
		float t = 0f;
		float startLeft = (float)(-(float)Level.Current.Left);
		float startRight = (float)Level.Current.Right;
		while (t < time)
		{
			t += CupheadTime.Delta;
			float tm = Mathf.InverseLerp(0f, time, t);
			Level.Current.SetBounds(new int?((int)Mathf.Lerp(startLeft, (float)left, tm)), new int?((int)Mathf.Lerp(startRight, (float)right, tm)), null, null);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002114 RID: 8468 RVA: 0x000B95E4 File Offset: 0x000B77E4
	public IEnumerator move_level_borders_anim_sync_cr(int left, int right, float endFrame)
	{
		float startTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		float startLeft = (float)(-(float)Level.Current.Left);
		float startRight = (float)Level.Current.Right;
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < (endFrame + 1f) / 79f)
		{
			float tm = Mathf.InverseLerp(startTime, endFrame / 79f, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			Level.Current.SetBounds(new int?((int)Mathf.Lerp(startLeft, (float)left, tm)), new int?((int)Mathf.Lerp(startRight, (float)right, tm)), null, null);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002115 RID: 8469 RVA: 0x000B9614 File Offset: 0x000B7814
	public IEnumerator shake_platform_cr()
	{
		float amount = 1.5f;
		while (this.transState != OldManLevelSockPuppetHandler.TransitionState.PlatformDestroyed)
		{
			this.handsParent.transform.localPosition = new Vector3(Random.Range(-amount, amount), Random.Range(-amount, amount));
			amount += 0.25f;
			yield return CupheadTime.WaitForSeconds(this, 0.0166666675f);
		}
		yield break;
	}

	// Token: 0x06002116 RID: 8470 RVA: 0x000B9630 File Offset: 0x000B7830
	public void AniEvent_HandsGrip()
	{
		CupheadLevelCamera.Current.Shake(5f, 0.2f, false);
		this.beardObject.transform.parent = this.handsParent.transform;
		this.rocksUnderBeardObject.transform.parent = this.handsParent.transform;
		base.StartCoroutine(this.shake_platform_cr());
	}

	// Token: 0x06002117 RID: 8471 RVA: 0x000B9698 File Offset: 0x000B7898
	public void AniEvent_PlatformDestroyed()
	{
		this.beardObject.transform.parent = this.BGParent.transform;
		this.rocksUnderBeardObject.transform.parent = this.BGParent.transform;
		this.transState = OldManLevelSockPuppetHandler.TransitionState.PlatformDestroyed;
		CupheadLevelCamera.Current.Shake(30f, 0.7f, false);
	}

	// Token: 0x06002118 RID: 8472 RVA: 0x0001C434 File Offset: 0x0001A634
	public void AniEvent_FinishedSwallow()
	{
		this.transState = OldManLevelSockPuppetHandler.TransitionState.InStomach;
	}

	// Token: 0x06002119 RID: 8473 RVA: 0x0001C43D File Offset: 0x0001A63D
	public void SwallowedPlayers()
	{
		base.animator.SetTrigger("SwallowedPlayers");
	}

	// Token: 0x0600211A RID: 8474 RVA: 0x0001C44F File Offset: 0x0001A64F
	public void FinishPuppet()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600211B RID: 8475 RVA: 0x0001C45C File Offset: 0x0001A65C
	public void AnimationEvent_SFX_OMM_P2_EndBreakPlatformEat()
	{
		AudioManager.Play("sfx_dlc_omm_p2_end_breakplatformeat");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_end_breakplatformeat");
	}

	// Token: 0x0600211C RID: 8476 RVA: 0x0001C478 File Offset: 0x0001A678
	public void AnimationEvent_SFX_OMM_P2_EndBurp()
	{
		AudioManager.Play("sfx_dlc_omm_p2_end_burp");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_end_burp");
	}

	// Token: 0x0600211D RID: 8477 RVA: 0x000B96F8 File Offset: 0x000B78F8
	public void WORKAROUND_NullifyFields()
	{
		this.OnDeathEvent = null;
		this.idleHoldRange = null;
		this.lookHoldRange = null;
		this.oldManAngry = null;
		this.oldManAngryNoseShadow = null;
		this.mainPlatformCollider = null;
		this.puppetBallPrefab = null;
		this.sockPuppetLeft = null;
		this.sockPuppetRight = null;
		this.platformManager = null;
		this.KDpuppetYPositions = null;
		this.DpuppetYPositions = null;
		this.dwarves = null;
		this.dwarvesObject = null;
		this.handsParent = null;
		this.BGParent = null;
		this.beardObject = null;
		this.rocksUnderBeardObject = null;
		this.damageDealer = null;
		this.puppetBall = null;
	}

	// Token: 0x04001B29 RID: 6953
	public const float POST_DEATH_PRE_MOVE_TIME = 2f;

	// Token: 0x04001B2A RID: 6954
	[SerializeField]
	public MinMax idleHoldRange = new MinMax(0.01f, 0.1f);

	// Token: 0x04001B2B RID: 6955
	[SerializeField]
	public MinMax lookHoldRange = new MinMax(0.75f, 1.5f);

	// Token: 0x04001B2C RID: 6956
	[SerializeField]
	public float chanceToSwitchLookSides = 0.75f;

	// Token: 0x04001B2D RID: 6957
	[SerializeField]
	public float chanceToLaugh = 0.25f;

	// Token: 0x04001B2E RID: 6958
	public OldManLevelSockPuppetHandler.TransitionState transState;

	// Token: 0x04001B2F RID: 6959
	[SerializeField]
	public Transform oldManAngry;

	// Token: 0x04001B30 RID: 6960
	[SerializeField]
	public Transform oldManAngryNoseShadow;

	// Token: 0x04001B31 RID: 6961
	[SerializeField]
	public Collider2D mainPlatformCollider;

	// Token: 0x04001B32 RID: 6962
	[SerializeField]
	public OldManLevelPuppetBall puppetBallPrefab;

	// Token: 0x04001B33 RID: 6963
	[SerializeField]
	public OldManLevelSockPuppet sockPuppetLeft;

	// Token: 0x04001B34 RID: 6964
	[SerializeField]
	public OldManLevelSockPuppet sockPuppetRight;

	// Token: 0x04001B35 RID: 6965
	[SerializeField]
	public OldManLevelPlatformManager platformManager;

	// Token: 0x04001B36 RID: 6966
	[SerializeField]
	public Transform[] KDpuppetYPositions;

	// Token: 0x04001B37 RID: 6967
	[SerializeField]
	public Transform[] DpuppetYPositions;

	// Token: 0x04001B38 RID: 6968
	[SerializeField]
	public OldManLevelDwarf[] dwarves;

	// Token: 0x04001B39 RID: 6969
	[SerializeField]
	public GameObject dwarvesObject;

	// Token: 0x04001B3A RID: 6970
	[SerializeField]
	public GameObject handsParent;

	// Token: 0x04001B3B RID: 6971
	[SerializeField]
	public GameObject BGParent;

	// Token: 0x04001B3C RID: 6972
	[SerializeField]
	public GameObject beardObject;

	// Token: 0x04001B3D RID: 6973
	[SerializeField]
	public GameObject rocksUnderBeardObject;

	// Token: 0x04001B3E RID: 6974
	public DamageDealer damageDealer;

	// Token: 0x04001B3F RID: 6975
	public Action OnDeathEvent;

	// Token: 0x04001B40 RID: 6976
	public OldManLevelPuppetBall puppetBall;

	// Token: 0x04001B41 RID: 6977
	public int leftHandPos = 1;

	// Token: 0x04001B42 RID: 6978
	public int rightHandPos = 1;

	// Token: 0x04001B43 RID: 6979
	public int rightHandPosOld;

	// Token: 0x04001B44 RID: 6980
	public int leftHandPosOld;

	// Token: 0x04001B45 RID: 6981
	public bool triggerLaugh;

	// Token: 0x04001B46 RID: 6982
	public bool fromLeft;

	// Token: 0x02000DF2 RID: 3570
	public enum TransitionState
	{
		// Token: 0x04006517 RID: 25879
		None,
		// Token: 0x04006518 RID: 25880
		PlatformDestroyed,
		// Token: 0x04006519 RID: 25881
		InStomach
	}
}
