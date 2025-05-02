using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003A0 RID: 928
public class SnowCultLevelWizard : LevelProperties.SnowCult.Entity
{
	// Token: 0x1400004D RID: 77
	// (add) Token: 0x060028CD RID: 10445 RVA: 0x000CFD98 File Offset: 0x000CDF98
	// (remove) Token: 0x060028CE RID: 10446 RVA: 0x000CFDD0 File Offset: 0x000CDFD0
	public event Action OnDeathEvent;

	// Token: 0x060028CF RID: 10447 RVA: 0x000224E6 File Offset: 0x000206E6
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060028D0 RID: 10448 RVA: 0x0002251C File Offset: 0x0002071C
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060028D1 RID: 10449 RVA: 0x00022534 File Offset: 0x00020734
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060028D2 RID: 10450 RVA: 0x000CFE08 File Offset: 0x000CE008
	public override void LevelInit(LevelProperties.SnowCult properties)
	{
		base.LevelInit(properties);
		this.state = SnowCultLevelWizard.States.Idle;
		this.wizardHesitationString = new PatternString(properties.CurrentState.wizard.wizardHesitationString, true, true);
		this.attackLocationString = new PatternString(properties.CurrentState.quadShot.attackLocationString, true, true);
		this.quadShotBallDelayString = new PatternString(properties.CurrentState.quadShot.ballDelayString, true, true);
		this.hazardDirectionString = new PatternString(properties.CurrentState.quadShot.hazardDirectionString, true, true);
		this.seriesShotCountString = new PatternString(properties.CurrentState.seriesShot.seriesShotCountString, true, true);
		this.seriesShotParryString = new PatternString(properties.CurrentState.seriesShot.parryString, true);
		this.quadShotBallDelayString.SetSubStringIndex(-1);
		this.hazardDirectionString.SetSubStringIndex(-1);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060028D3 RID: 10451 RVA: 0x00022547 File Offset: 0x00020747
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060028D4 RID: 10452 RVA: 0x00022565 File Offset: 0x00020765
	public void PlayerHitByWhale(GameObject hit, CollisionPhase phase)
	{
		this.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060028D5 RID: 10453 RVA: 0x000CFEF8 File Offset: 0x000CE0F8
	public IEnumerator intro_cr()
	{
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		float t = 0f;
		Vector3 startPos = base.transform.position;
		Vector3 endPos = new Vector3(this.pivotPoint.position.x + 540f, this.pivotPoint.transform.position.y);
		base.animator.SetBool("Turn", true);
		while (t < 1f)
		{
			float easedT = EaseUtils.EaseInOutSine(0f, 1f, t);
			base.transform.position = new Vector3(Mathf.Lerp(startPos.x, endPos.x, easedT), EaseUtils.EaseInSine(startPos.y, endPos.y, easedT));
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		base.StartCoroutine(this.move_cr());
		yield break;
	}

	// Token: 0x060028D6 RID: 10454 RVA: 0x0002256F File Offset: 0x0002076F
	public void AniEvent_StartTurn()
	{
		this.turnAnimationPlaying = true;
	}

	// Token: 0x060028D7 RID: 10455 RVA: 0x000CFF14 File Offset: 0x000CE114
	public void AniEvent_CompleteTurn()
	{
		if (!this.dead && this.turnAnimationPlaying)
		{
			bool flag = this.goingLeft;
			if (this.currentPosition > 0.9f)
			{
				flag = !flag;
			}
			base.transform.localScale = new Vector3((float)((!flag) ? 1 : -1), base.transform.localScale.y);
			this.turnAnimationPlaying = false;
		}
	}

	// Token: 0x060028D8 RID: 10456 RVA: 0x000CFF8C File Offset: 0x000CE18C
	public void AniEvent_AlignForOutro()
	{
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.position.x - Camera.main.transform.position.x), base.transform.localScale.y);
		this.outroWobbling = true;
	}

	// Token: 0x060028D9 RID: 10457 RVA: 0x00022578 File Offset: 0x00020778
	public bool Turning()
	{
		return base.animator.GetBool("Turn");
	}

	// Token: 0x060028DA RID: 10458 RVA: 0x000CFFF4 File Offset: 0x000CE1F4
	public IEnumerator move_cr()
	{
		this.goingLeft = true;
		LevelProperties.SnowCult.Movement p = base.properties.CurrentState.movement;
		float startAngle = 1.57079637f;
		float endAngle = -1.57079637f;
		float angle = endAngle;
		float loopSizeX = 540f;
		float loopSizeY = p.dipAmount;
		float loopSpeed = p.speed;
		float startSpeed = p.speed;
		float endSpeed = p.easing;
		bool easeIn = true;
		Vector3 handleRotationX = Vector3.zero;
		Vector3 handleRotationY = Vector3.zero;
		base.transform.SetPosition(new float?(this.pivotPoint.position.x + loopSizeX), null, null);
		float t = 1f;
		float time = 1f;
		this.isMoving = true;
		for (;;)
		{
			while (!this.isMoving)
			{
				yield return null;
			}
			angle += loopSpeed * CupheadTime.FixedDelta * this.postWhalePositionLerpTimer * ((!this.dead) ? 1f : 1.5f);
			if ((angle < endAngle && !this.goingLeft) || (angle > startAngle && this.goingLeft))
			{
				this.reachedApex = true;
				this.notReachedMid = true;
				loopSpeed = -loopSpeed;
				this.goingLeft = !this.goingLeft;
				t = 0f;
				startSpeed = ((!easeIn) ? ((!this.goingLeft) ? (-p.easing) : p.easing) : ((!this.goingLeft) ? (-p.speed) : p.speed));
				endSpeed = ((!easeIn) ? ((!this.goingLeft) ? (-p.speed) : p.speed) : ((!this.goingLeft) ? (-p.easing) : p.easing));
				easeIn = true;
			}
			else
			{
				this.reachedApex = false;
			}
			if ((angle > startAngle - 1.5f && this.goingLeft && easeIn) || (angle < endAngle + 1.5f && !this.goingLeft && easeIn))
			{
				t = 0f;
				startSpeed = ((!easeIn) ? ((!this.goingLeft) ? (-p.easing) : p.easing) : ((!this.goingLeft) ? (-p.speed) : p.speed));
				endSpeed = ((!easeIn) ? ((!this.goingLeft) ? (-p.speed) : p.speed) : ((!this.goingLeft) ? (-p.easing) : p.easing));
				easeIn = false;
			}
			if (((this.goingLeft && base.transform.position.x < 0f) || (!this.goingLeft && base.transform.position.x > 0f)) && this.notReachedMid)
			{
				this.notReachedMid = false;
			}
			if (t < time)
			{
				t += CupheadTime.FixedDelta;
				loopSpeed = Mathf.Lerp(startSpeed, endSpeed, t / time);
			}
			Vector3 handleRotation = new Vector3(-Mathf.Sin(angle) * loopSizeX, -Mathf.Cos(angle) * loopSizeY, 0f);
			Vector3 destinationPos = this.pivotPoint.position + handleRotation;
			this.lastPos = base.transform.position;
			base.transform.position = new Vector3(destinationPos.x, Mathf.Lerp(base.transform.position.y, destinationPos.y, this.postWhalePositionLerpTimer));
			this.postWhalePositionLerpTimer = Mathf.Clamp(this.postWhalePositionLerpTimer + CupheadTime.FixedDelta * 2.5f, 0f, 1f);
			this.currentPosition = Mathf.InverseLerp(startAngle, endAngle, angle);
			if (this.goingLeft)
			{
				this.currentPosition = 1f - this.currentPosition;
			}
			bool goingLeftOrientation = this.goingLeft;
			if (this.currentPosition > 0.9f)
			{
				goingLeftOrientation = !goingLeftOrientation;
			}
			if (!this.dead)
			{
				base.animator.SetBool("Turn", (int)base.transform.localScale.x != ((!goingLeftOrientation) ? 1 : -1) && !this.seriesShotActive);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x060028DB RID: 10459 RVA: 0x0002258A File Offset: 0x0002078A
	public void StartQuadAttack()
	{
		base.StartCoroutine(this.quad_cr());
	}

	// Token: 0x060028DC RID: 10460 RVA: 0x000D0010 File Offset: 0x000CE210
	public IEnumerator quad_cr()
	{
		this.state = SnowCultLevelWizard.States.Quad;
		LevelProperties.SnowCult.QuadShot p = base.properties.CurrentState.quadShot;
		float targetPosX = this.attackLocationString.PopFloat();
		bool inAttackPos = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (!inAttackPos)
		{
			if (this.dead)
			{
				yield break;
			}
			if (Mathf.Abs(targetPosX - base.transform.position.x) < p.distToAttack && !this.turnAnimationPlaying)
			{
				inAttackPos = true;
			}
			yield return wait;
		}
		int curIdleFrame = Mathf.RoundToInt(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 23f);
		if (curIdleFrame >= 14 && curIdleFrame <= 22)
		{
			base.animator.Play("QuadshotIntro", 0, 0.2857143f);
		}
		else if (curIdleFrame >= 2 && curIdleFrame <= 10)
		{
			base.animator.Play("QuadshotIntro", 0, 0.142857149f);
		}
		else
		{
			base.animator.Play("QuadshotIntro");
		}
		this.SFX_SNOWCULT_WizardQuadshotAttack();
		this.isMoving = false;
		List<SnowCultLevelQuadShot> quadShots = new List<SnowCultLevelQuadShot>();
		float downAmount = 0f;
		yield return CupheadTime.WaitForSeconds(this, p.preattackDelay);
		base.animator.Play("QuadshotContinue");
		yield return null;
		yield return base.animator.WaitForAnimationToEnd(this, "QuadshotContinue", false, true);
		this.quadshotMask.enabled = true;
		for (int i = 0; i < 4; i++)
		{
			downAmount = ((i <= 0 || i >= 3) ? 0f : p.distanceDown);
			Vector3 startPos;
			startPos..ctor(base.transform.position.x - p.distanceBetween * 0.8f * 2f + p.distanceBetween * 0.8f * 0.5f + p.distanceBetween * 0.8f * (float)i, base.transform.position.y - downAmount);
			Vector3 destPos;
			destPos..ctor(base.transform.position.x - p.distanceBetween * 2f + p.distanceBetween / 2f + p.distanceBetween * (float)i, base.transform.position.y - downAmount);
			SnowCultLevelQuadShot snowCultLevelQuadShot = this.quadShotProjectile.Spawn<SnowCultLevelQuadShot>();
			float delay = this.quadShotBallDelayString.PopFloat() / 4f * p.ballDelay;
			snowCultLevelQuadShot.Init(startPos, destPos, p.shotVelocity, this.hazardDirectionString.PopString(), p, i, delay, p.distanceBetween, PlayerManager.GetNext());
			quadShots.Add(snowCultLevelQuadShot);
		}
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		this.quadshotMask.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, p.attackDelay - 0.25f);
		base.animator.Play("QuadshotEnd");
		yield return null;
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.272727281f)
		{
			yield return null;
		}
		AbstractPlayerController player = PlayerManager.GetNext();
		float first = 1000f;
		float second = 1000f;
		SnowCultLevelQuadShot shotQuadChosen = null;
		SnowCultLevelQuadShot shotQuadChosen2 = null;
		for (int j = 0; j < 4; j++)
		{
			float num = Mathf.Abs(quadShots[j].transform.position.x - player.transform.position.x);
			if (num < first)
			{
				second = first;
				first = num;
				shotQuadChosen2 = shotQuadChosen;
				shotQuadChosen = quadShots[j];
			}
			else if (num < second && num != first)
			{
				second = num;
				shotQuadChosen2 = quadShots[j];
			}
		}
		float offset = Random.Range(0f, p.maxOffset);
		offset = ((!Rand.Bool()) ? (-offset) : offset);
		SnowCultLevelQuadShot shotQuadChosen3 = (!Rand.Bool()) ? shotQuadChosen2 : shotQuadChosen;
		Vector3 endPos = new Vector3(player.transform.position.x, (float)Level.Current.Ground);
		Vector3 direction = endPos - shotQuadChosen3.transform.position;
		Vector3 finalDirection = new Vector3(direction.x + offset, direction.y);
		this.lineStartPos = shotQuadChosen3.transform.position;
		this.lineEndPos = new Vector3(player.transform.position.x + offset, endPos.y);
		bool startWithRight = Rand.Bool();
		int rightIndex = 3;
		for (int k = 0; k < 4; k++)
		{
			int index = (!startWithRight) ? k : (rightIndex - k);
			quadShots[index].Shoot(MathUtils.DirectionToAngle(finalDirection));
		}
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		this.isMoving = true;
		yield return CupheadTime.WaitForSeconds(this, this.wizardHesitationString.PopFloat());
		this.state = SnowCultLevelWizard.States.Idle;
		yield break;
	}

	// Token: 0x060028DD RID: 10461 RVA: 0x00022599 File Offset: 0x00020799
	public void Whale()
	{
		base.StartCoroutine(this.whale_cr());
	}

	// Token: 0x060028DE RID: 10462 RVA: 0x000D002C File Offset: 0x000CE22C
	public IEnumerator whale_cr()
	{
		this.state = SnowCultLevelWizard.States.Whale;
		LevelProperties.SnowCult.Whale p = base.properties.CurrentState.whale;
		this.dropAttackComplete = false;
		bool drop = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		AbstractPlayerController player = PlayerManager.GetNext();
		float lastPlayerOffset = base.transform.position.x - Mathf.Clamp(player.transform.position.x, -445f, 445f);
		while (!drop)
		{
			float playerClampedX = Mathf.Clamp(player.transform.position.x, -445f, 445f);
			if (Mathf.Abs(playerClampedX - base.transform.position.x) < p.distToDrop || Mathf.Sign(lastPlayerOffset) != Mathf.Sign(base.transform.position.x - playerClampedX))
			{
				drop = true;
			}
			lastPlayerOffset = base.transform.position.x - playerClampedX;
			yield return wait;
		}
		this.isMoving = false;
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		float currentAnimatorTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		base.animator.Play((currentAnimatorTime <= 0.0833333358f || currentAnimatorTime >= 0.5833333f) ? "WhaleDrop_IntroAlt" : "WhaleDrop_Intro");
		this.SFX_SNOWCULT_WizardWhalesmashAttack();
		float t = 0f;
		float val = 0f;
		Vector3 startPos = base.transform.position;
		Vector3 endPos = new Vector3(startPos.x, 200f);
		while (t < 0.22f)
		{
			t += CupheadTime.Delta;
			val = Mathf.InverseLerp(0f, 0.22f, t);
			base.transform.position = Vector3.Lerp(startPos, endPos, EaseUtils.EaseInSine(0f, 1f, val));
			yield return null;
		}
		base.transform.position = endPos;
		t = 0f;
		while (t < p.attackDelay)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("DropWhale");
		while (!this.dropAttackComplete)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.0833333358f);
		this.postWhalePositionLerpTimer = 0f;
		this.isMoving = true;
		yield return CupheadTime.WaitForSeconds(this, p.recoveryDelay);
		yield return CupheadTime.WaitForSeconds(this, this.wizardHesitationString.PopFloat());
		this.state = SnowCultLevelWizard.States.Idle;
		yield break;
	}

	// Token: 0x060028DF RID: 10463 RVA: 0x000225A8 File Offset: 0x000207A8
	public void WhaleAttackImpact()
	{
		CupheadLevelCamera.Current.Shake(55f, 0.5f, false);
	}

	// Token: 0x060028E0 RID: 10464 RVA: 0x000D0048 File Offset: 0x000CE248
	public void WhaleAttackComplete()
	{
		this.whaleDropFX.transform.position = new Vector3(base.transform.position.x, this.whaleDropFX.transform.position.y);
		this.whaleDropFX.gameObject.SetActive(true);
		this.whaleDropFX.Play("Main");
		this.dropAttackComplete = true;
	}

	// Token: 0x060028E1 RID: 10465 RVA: 0x000225BF File Offset: 0x000207BF
	public void SeriesShot()
	{
		base.StartCoroutine(this.series_shot_cr());
	}

	// Token: 0x060028E2 RID: 10466 RVA: 0x000D00C0 File Offset: 0x000CE2C0
	public IEnumerator series_shot_cr()
	{
		this.seriesShotCanExit = false;
		this.seriesShotActive = true;
		this.state = SnowCultLevelWizard.States.SeriesShot;
		LevelProperties.SnowCult.SeriesShot p = base.properties.CurrentState.seriesShot;
		int shotCount = this.seriesShotCountString.PopInt();
		float t = 0f;
		base.animator.SetTrigger("StartPeashot");
		yield return base.animator.WaitForAnimationToStart(this, "Peashot_Intro", false);
		this.table.Intro(base.transform.position - this.lastPos);
		for (int i = 0; i < shotCount; i++)
		{
			while (t < p.seriesShotWarningTime && !this.dead)
			{
				t += CupheadTime.Delta;
				yield return null;
			}
			if (!this.dead)
			{
				base.animator.SetTrigger("OnShoot");
				while (!this.seriesShotFired)
				{
					yield return null;
				}
				this.SFX_SNOWCULT_WizardTarotCardAttackLaunch();
				this.seriesShotFired = false;
			}
			t = 0f;
			while (t < p.betweenShotDelay && !this.dead)
			{
				t += CupheadTime.Delta;
				yield return null;
			}
			if (this.dead)
			{
				break;
			}
		}
		this.seriesShotCanExit = true;
		while (this.seriesShotActive)
		{
			yield return null;
		}
		if (!this.dead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.wizardHesitationString.PopFloat());
		}
		this.state = SnowCultLevelWizard.States.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x060028E3 RID: 10467 RVA: 0x000225CE File Offset: 0x000207CE
	public void CreatePeashot()
	{
		base.StartCoroutine(this.create_peashot());
	}

	// Token: 0x060028E4 RID: 10468 RVA: 0x000D00DC File Offset: 0x000CE2DC
	public IEnumerator create_peashot()
	{
		this.shootFX.Play("ShootFX");
		this.shootFX.Update(0f);
		yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
		AbstractPlayerController player = PlayerManager.GetNext();
		Vector3 dir = player.transform.position - this.shootFX.transform.position;
		BasicProjectile proj = this.seriesShot.Create(this.shootFX.transform.position, MathUtils.DirectionToAngle(dir) + 90f, base.properties.CurrentState.seriesShot.bulletSpeed);
		proj.transform.position += dir.normalized * 25f;
		proj.SetParryable(this.seriesShotParryString.PopLetter() == 'P');
		this.seriesShotFired = true;
		while (this.shootFX.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
		{
			Effect sparkle = this.cardSparkle.Create(this.shootFX.transform.position + MathUtils.AngleToDirection((float)Random.Range(0, 360)) * this.shootFX.GetCurrentAnimatorStateInfo(0).normalizedTime * 200f);
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.005f, 0.01f));
		}
		yield break;
	}

	// Token: 0x060028E5 RID: 10469 RVA: 0x000225DD File Offset: 0x000207DD
	public void CanExitPeashotLoop()
	{
		if (this.seriesShotCanExit)
		{
			base.animator.Play("Peashot_Outro_A");
			this.table.Outro();
		}
	}

	// Token: 0x060028E6 RID: 10470 RVA: 0x00022605 File Offset: 0x00020805
	public void EndPeashotLoop()
	{
		this.seriesShotActive = false;
	}

	// Token: 0x060028E7 RID: 10471 RVA: 0x0002260E File Offset: 0x0002080E
	public void ToOutro(SnowCultLevelYeti yeti)
	{
		this.dead = true;
		base.StartCoroutine(this.outro_cr(yeti));
	}

	// Token: 0x060028E8 RID: 10472 RVA: 0x00022625 File Offset: 0x00020825
	public void AniEvent_CultistsSummon()
	{
		((SnowCultLevel)Level.Current).CultistsSummon();
	}

	// Token: 0x060028E9 RID: 10473 RVA: 0x000D00F8 File Offset: 0x000CE2F8
	public IEnumerator outro_cr(SnowCultLevelYeti yeti)
	{
		while (!this.reachedApex)
		{
			yield return null;
		}
		if (base.transform.localScale.x != Mathf.Sign(base.transform.position.x - Camera.main.transform.position.x))
		{
			base.animator.SetBool("Turn", true);
		}
		this.state = SnowCultLevelWizard.States.Idle;
		this.isMoving = false;
		base.animator.SetTrigger("OnOutro");
		float t = 0f;
		Vector3 startPos = base.transform.position;
		if (base.transform.position.x < this.pivotPoint.position.x)
		{
			this.outroPos.position = new Vector3(this.pivotPoint.position.x + (this.pivotPoint.position.x - this.outroPos.position.x), this.outroPos.position.y);
			yeti.StartOnLeft(this.pivotPoint.position);
		}
		while (t < 0.5f)
		{
			base.transform.position = new Vector3(EaseUtils.EaseOutSine(startPos.x, this.outroPos.position.x, t * 2f), EaseUtils.EaseOutBack(startPos.y, this.outroPos.position.y, t * 2f));
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != Animator.StringToHash(base.animator.GetLayerName(0) + ".OutroLoop"))
		{
			base.transform.position = this.outroPos.position;
			yield return null;
		}
		yeti.gameObject.SetActive(true);
		yeti.StartYeti();
		while (!yeti.introRibcageClosed)
		{
			base.transform.position = this.outroPos.position;
			yield return null;
		}
		this.OnDeath();
		yield break;
	}

	// Token: 0x060028EA RID: 10474 RVA: 0x000D011C File Offset: 0x000CE31C
	public void LateUpdate()
	{
		if (!this.outroWobbling)
		{
			return;
		}
		this.outroWobbleTime += CupheadTime.FixedDelta * 1.5f;
		base.transform.position += new Vector3(Mathf.Sin(this.outroWobbleTime * 3f) * 1f, Mathf.Cos(this.outroWobbleTime * 2f) * 5f);
	}

	// Token: 0x060028EB RID: 10475 RVA: 0x00022636 File Offset: 0x00020836
	public void OnDeath()
	{
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060028EC RID: 10476 RVA: 0x0002265F File Offset: 0x0002085F
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(this.lineStartPos, this.lineEndPos);
	}

	// Token: 0x060028ED RID: 10477 RVA: 0x00022678 File Offset: 0x00020878
	public void AnimationEvent_SFX_SNOWCULT_WizardIntro()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_intro");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_intro");
	}

	// Token: 0x060028EE RID: 10478 RVA: 0x00022694 File Offset: 0x00020894
	public void AnimationEvent_SFX_SNOWCULT_WizardQuadshot_Attack()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_quadshot_attack");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_quadshot_attack");
	}

	// Token: 0x060028EF RID: 10479 RVA: 0x000226B0 File Offset: 0x000208B0
	public void SFX_SNOWCULT_WizardWhalesmashAttack()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_whalesmash_attack");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_whalesmash_attack");
	}

	// Token: 0x060028F0 RID: 10480 RVA: 0x000226CC File Offset: 0x000208CC
	public void SFX_SNOWCULT_WizardTarotCardAttackLaunch()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_tarotcardattack_launch");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_tarotcardattack_launch");
	}

	// Token: 0x060028F1 RID: 10481 RVA: 0x000226E8 File Offset: 0x000208E8
	public void SFX_SNOWCULT_WizardQuadshotAttack()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_quadshot_attack");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_quadshot_attack");
	}

	// Token: 0x060028F2 RID: 10482 RVA: 0x00022704 File Offset: 0x00020904
	public void AnimationEvent_SFX_SNOWCULT_WizardYetiIntroBellComesToLife()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_intro_bell_comestolife");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_intro_bell_comestolife");
	}

	// Token: 0x060028F3 RID: 10483 RVA: 0x00022720 File Offset: 0x00020920
	public void AnimationEvent_SFX_SNOWCULT_WizardVoiceEffortLarge()
	{
		AudioManager.Stop("sfx_dlc_snowcult_wizard_voice_laugh");
		AudioManager.Play("sfx_dlc_snowcult_wizard_voice_effort_large");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_wizard_voice_effort_large");
	}

	// Token: 0x060028F4 RID: 10484 RVA: 0x00022746 File Offset: 0x00020946
	public void AnimationEvent_SFX_SNOWCULT_WizardVoiceLaugh()
	{
		AudioManager.Stop("sfx_dlc_snowcult_wizard_voice_effort_large");
		AudioManager.Stop("sfx_dlc_snowcult_wizard_voice_laugh");
		AudioManager.Play("sfx_dlc_snowcult_wizard_voice_laugh");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_wizard_voice_laugh");
	}

	// Token: 0x060028F5 RID: 10485 RVA: 0x00022776 File Offset: 0x00020976
	public void AnimationEvent_SFX_SNOWCULT_WizardVoiceWhee()
	{
		AudioManager.Play("sfx_dlc_snowcult_wizard_voice_whee");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_wizard_voice_whee");
	}

	// Token: 0x04002214 RID: 8724
	public SnowCultLevelWizard.States state;

	// Token: 0x04002216 RID: 8726
	public const int NUM_OF_SLAM_SLOTS = 4;

	// Token: 0x04002217 RID: 8727
	public const int NUM_OF_QUAD_SHOTS = 4;

	// Token: 0x04002218 RID: 8728
	public const float QUAD_SHOT_START_SPACING_MULTIPLIER = 0.8f;

	// Token: 0x04002219 RID: 8729
	public const float WIZARD_SLAM_OFFSET = 230f;

	// Token: 0x0400221A RID: 8730
	public const float WHALE_ATTACK_HEIGHT = 200f;

	// Token: 0x0400221B RID: 8731
	public const float WHALE_ATTACK_MOVE_DELAY = 0.22f;

	// Token: 0x0400221C RID: 8732
	public const float WHALE_POSTATTACK_MOVE_DELAY = 0.4f;

	// Token: 0x0400221D RID: 8733
	public const float WHALE_RANGE = 195f;

	// Token: 0x0400221E RID: 8734
	public DamageDealer damageDealer;

	// Token: 0x0400221F RID: 8735
	public DamageReceiver damageReceiver;

	// Token: 0x04002220 RID: 8736
	[SerializeField]
	public BasicProjectile seriesShot;

	// Token: 0x04002221 RID: 8737
	[SerializeField]
	public Animator whaleDropFX;

	// Token: 0x04002222 RID: 8738
	[SerializeField]
	public SnowCultLevelTable table;

	// Token: 0x04002223 RID: 8739
	[SerializeField]
	public Animator shootFX;

	// Token: 0x04002224 RID: 8740
	[SerializeField]
	public SnowCultLevelQuadShot quadShotProjectile;

	// Token: 0x04002225 RID: 8741
	[SerializeField]
	public Transform pivotPoint;

	// Token: 0x04002226 RID: 8742
	[SerializeField]
	public Transform outroPos;

	// Token: 0x04002227 RID: 8743
	[SerializeField]
	public SpriteMask quadshotMask;

	// Token: 0x04002228 RID: 8744
	[SerializeField]
	public Effect cardSparkle;

	// Token: 0x04002229 RID: 8745
	[SerializeField]
	public SpriteRenderer introWizRend;

	// Token: 0x0400222A RID: 8746
	public Vector3 lineStartPos;

	// Token: 0x0400222B RID: 8747
	public Vector3 lineEndPos;

	// Token: 0x0400222C RID: 8748
	public bool goingLeft;

	// Token: 0x0400222D RID: 8749
	public bool isMoving;

	// Token: 0x0400222E RID: 8750
	public bool reachedApex;

	// Token: 0x0400222F RID: 8751
	public bool notReachedMid;

	// Token: 0x04002230 RID: 8752
	public Vector3 lastPos = Vector3.zero;

	// Token: 0x04002231 RID: 8753
	public PatternString wizardHesitationString;

	// Token: 0x04002232 RID: 8754
	public PatternString attackLocationString;

	// Token: 0x04002233 RID: 8755
	public PatternString hazardDirectionString;

	// Token: 0x04002234 RID: 8756
	public PatternString iceSummonString;

	// Token: 0x04002235 RID: 8757
	public PatternString seriesShotCountString;

	// Token: 0x04002236 RID: 8758
	public PatternString quadShotBallDelayString;

	// Token: 0x04002237 RID: 8759
	public bool seriesShotFired;

	// Token: 0x04002238 RID: 8760
	public bool seriesShotCanExit = true;

	// Token: 0x04002239 RID: 8761
	public bool seriesShotActive;

	// Token: 0x0400223A RID: 8762
	public PatternString seriesShotParryString;

	// Token: 0x0400223B RID: 8763
	public bool dropAttackComplete;

	// Token: 0x0400223C RID: 8764
	public float postWhalePositionLerpTimer = 1f;

	// Token: 0x0400223D RID: 8765
	public bool dead;

	// Token: 0x0400223E RID: 8766
	public bool turnAnimationPlaying;

	// Token: 0x0400223F RID: 8767
	public float currentPosition = 1f;

	// Token: 0x04002240 RID: 8768
	public bool outroWobbling;

	// Token: 0x04002241 RID: 8769
	public float outroWobbleTime;

	// Token: 0x02000F7B RID: 3963
	public enum States
	{
		// Token: 0x04006F9F RID: 28575
		Idle,
		// Token: 0x04006FA0 RID: 28576
		Quad,
		// Token: 0x04006FA1 RID: 28577
		Whale,
		// Token: 0x04006FA2 RID: 28578
		Slam,
		// Token: 0x04006FA3 RID: 28579
		Wind,
		// Token: 0x04006FA4 RID: 28580
		SeriesShot
	}
}
