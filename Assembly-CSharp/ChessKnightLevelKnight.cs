using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class ChessKnightLevelKnight : LevelProperties.ChessKnight.Entity
{
	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06001278 RID: 4728 RVA: 0x0000F92C File Offset: 0x0000DB2C
	// (set) Token: 0x06001279 RID: 4729 RVA: 0x0000F934 File Offset: 0x0000DB34
	public ChessKnightLevelKnight.State state { get; set; }

	// Token: 0x0600127A RID: 4730 RVA: 0x000951A4 File Offset: 0x000933A4
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		float num = -640f + this.positionBoundaryInset.minimum;
		Gizmos.DrawLine(new Vector3(num, -500f), new Vector3(num, 500f));
		Gizmos.color = Color.green;
		num = -640f + this.positionBoundaryInset.maximum;
		Gizmos.DrawLine(new Vector3(num, -500f), new Vector3(num, 500f));
		Gizmos.color = Color.red;
		num = 640f - this.positionBoundaryInset.minimum;
		Gizmos.DrawLine(new Vector3(num, -500f), new Vector3(num, 500f));
		Gizmos.color = Color.green;
		num = 640f - this.positionBoundaryInset.maximum;
		Gizmos.DrawLine(new Vector3(num, -500f), new Vector3(num, 500f));
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x00095294 File Offset: 0x00093494
	public override void LevelInit(LevelProperties.ChessKnight properties)
	{
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.onIntroEventHandler;
		LevelProperties.ChessKnight.Knight knight = properties.CurrentState.knight;
		this.attackIntervalPattern = new PatternString(knight.attackIntervalString, true, true);
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 != null && !player2.IsDead)
		{
			this.targetPlayer = ((!Rand.Bool()) ? player2 : player);
		}
		this.battleStartPosition = base.transform.position.x;
		this.movementPattern = new PatternString(properties.CurrentState.movement.movementString, true, true);
		this.numberTauntString = new PatternString(properties.CurrentState.tauntAttack.numberTauntString, true);
		this.numberTauntString.SetSubStringIndex(-1);
		this.tauntAttackCounter = this.numberTauntString.PopInt();
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x00095388 File Offset: 0x00093588
	public override void Awake()
	{
		base.Awake();
		Vector3 position = base.transform.position;
		position.x = 640f - (this.positionBoundaryInset.maximum + this.positionBoundaryInset.minimum) * 0.5f;
		base.transform.position = position;
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x000953E0 File Offset: 0x000935E0
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.pink.OnActivate += this.GotParried;
		this.swordHitbox.OnPlayerCollision += this.OnCollisionPlayer;
		this.upHitbox.OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x0000F93D File Offset: 0x0000DB3D
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x0000F955 File Offset: 0x0000DB55
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x00095440 File Offset: 0x00093640
	public void GotParried()
	{
		base.properties.DealDamage((!PlayerManager.BothPlayersActive()) ? 10f : ChessKingLevelKing.multiplayerDamageNerf);
		this.hitFlash.Flash(0.7f);
		base.StartCoroutine(this.parry_timer_cr());
		if (base.properties.CurrentHealth <= 0f && this.state != ChessKnightLevelKnight.State.Death)
		{
			this.state = ChessKnightLevelKnight.State.Death;
			this.death();
		}
	}

	// Token: 0x06001281 RID: 4737 RVA: 0x000954BC File Offset: 0x000936BC
	public IEnumerator parry_timer_cr()
	{
		this.pink.gameObject.SetActive(false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.knight.parryCooldown);
		this.pink.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x06001282 RID: 4738 RVA: 0x0000F973 File Offset: 0x0000DB73
	public void onIntroEventHandler()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001283 RID: 4739 RVA: 0x000954D8 File Offset: 0x000936D8
	public IEnumerator intro_cr()
	{
		base.animator.SetTrigger("Intro");
		yield return base.animator.WaitForNormalizedTime(this, 1f, "Intro", 0, false, false, true);
		this.EndAttack();
		yield break;
	}

	// Token: 0x06001284 RID: 4740 RVA: 0x0000F982 File Offset: 0x0000DB82
	public void EndAttack()
	{
		this.isTauntAttack = false;
		this.SFX_KOG_KNIGHT_RecoverFoley();
		this.state = ChessKnightLevelKnight.State.Move;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x000954F4 File Offset: 0x000936F4
	public IEnumerator move_cr()
	{
		if (this.tauntAttackCounter > 0)
		{
			LevelProperties.ChessKnight.Movement p = base.properties.CurrentState.movement;
			float idleTime = this.attackIntervalPattern.PopFloat();
			float idleT = 0f;
			float moveT = 0f;
			YieldInstruction wait = new WaitForFixedUpdate();
			float startPosition = base.transform.position.x;
			float movementAmount = this.movementPattern.PopFloat();
			this.goingLeft = this.chooseGoingLeft(movementAmount);
			float endPosition = this.getWalkingEndPosition(movementAmount);
			float moveTime = Mathf.Abs(endPosition - startPosition) / p.movementSpeed;
			base.animator.SetBool("FacingLeft", this.facingLeft);
			base.animator.SetBool("Walking", true);
			for (;;)
			{
				idleT += CupheadTime.FixedDelta;
				Vector3 previousPosition = base.transform.position;
				if (moveT < moveTime)
				{
					moveT += CupheadTime.FixedDelta;
					if (p.hasEasing)
					{
						float num = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, moveT / moveTime);
						base.transform.SetPosition(new float?(Mathf.Lerp(startPosition, endPosition, num)), null, null);
					}
					else if (this.goingLeft && base.transform.position.x > endPosition)
					{
						base.transform.position += Vector3.left * p.movementSpeed * CupheadTime.FixedDelta;
					}
					else if (!this.goingLeft && base.transform.position.x < endPosition)
					{
						base.transform.position += Vector3.right * p.movementSpeed * CupheadTime.FixedDelta;
					}
				}
				else
				{
					if (idleT > idleTime)
					{
						break;
					}
					this.goingLeft = !this.goingLeft;
					base.transform.SetPosition(new float?(endPosition), null, null);
					startPosition = endPosition;
					endPosition = this.getWalkingEndPosition(this.movementPattern.PopFloat());
					moveTime = Mathf.Abs(endPosition - startPosition) / p.movementSpeed;
					moveT = 0f;
				}
				this.updateAnimatorSpeed(base.transform.position, previousPosition);
				yield return wait;
			}
			this.goingLeft = !this.goingLeft;
		}
		this.CheckTaunt();
		yield break;
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x00095510 File Offset: 0x00093710
	public bool chooseGoingLeft(float movementAmount)
	{
		bool flag = Rand.Bool();
		float num;
		if (this.facingLeft)
		{
			num = ((!flag) ? (640f - this.positionBoundaryInset.minimum) : (640f - this.positionBoundaryInset.maximum));
		}
		else
		{
			num = ((!flag) ? (-640f + this.positionBoundaryInset.maximum) : (-640f + this.positionBoundaryInset.minimum));
		}
		float num2 = num - base.transform.position.x;
		if (Mathf.Abs(num2 / movementAmount) < 0.5f)
		{
			flag = !flag;
		}
		return flag;
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x000955C0 File Offset: 0x000937C0
	public float getWalkingEndPosition(float movementAmount)
	{
		float endPosition = base.transform.position.x + ((!this.goingLeft) ? movementAmount : (-movementAmount)) * 2f;
		return this.clampEndPosition(endPosition, this.facingLeft);
	}

	// Token: 0x06001288 RID: 4744 RVA: 0x0009560C File Offset: 0x0009380C
	public void CheckTaunt()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		float tauntDistance = base.properties.CurrentState.taunt.tauntDistance;
		float num = Mathf.Abs(player.transform.position.x - base.transform.position.x);
		float num2 = (!(player2 != null) || player2.IsDead) ? num : Mathf.Abs(player2.transform.position.x - base.transform.position.x);
		if (num > tauntDistance && num2 > tauntDistance)
		{
			if (this.tauntAttackCounter <= 0)
			{
				this.isTauntAttack = true;
				base.StartCoroutine(this.long_cr());
			}
			else
			{
				base.StartCoroutine(this.taunt_cr());
			}
		}
		else
		{
			this.state = ChessKnightLevelKnight.State.Idle;
		}
	}

	// Token: 0x06001289 RID: 4745 RVA: 0x00095708 File Offset: 0x00093908
	public IEnumerator taunt_cr()
	{
		this.state = ChessKnightLevelKnight.State.Taunt;
		base.animator.SetBool("Taunting", true);
		base.animator.SetBool("Walking", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.taunt.tauntDuration);
		base.animator.SetBool("Taunting", false);
		if (this.shouldBackDash())
		{
			base.animator.SetTrigger("BackDash");
			yield return base.animator.WaitForNormalizedTime(this, 1f, "Taunt.Exit", 0, false, false, true);
			yield return base.StartCoroutine(this.backDash_cr());
		}
		else if (this.shouldTurn())
		{
			base.animator.SetTrigger("Turn");
			yield return base.animator.WaitForNormalizedTime(this, 1f, "Taunt.Exit", 0, false, false, true);
			yield return base.animator.WaitForNormalizedTime(this, 1f, "Turn", 0, false, false, true);
			this.turn();
		}
		else
		{
			yield return base.animator.WaitForNormalizedTime(this, 1f, "Taunt.Exit", 0, false, false, true);
		}
		this.tauntAttackCounter--;
		this.EndAttack();
		yield break;
	}

	// Token: 0x0600128A RID: 4746 RVA: 0x0000F9A5 File Offset: 0x0000DBA5
	public void Short()
	{
		this.state = ChessKnightLevelKnight.State.Short;
		base.StartCoroutine(this.short_cr());
	}

	// Token: 0x0600128B RID: 4747 RVA: 0x00095724 File Offset: 0x00093924
	public IEnumerator short_cr()
	{
		this.tauntAttackCounter = this.numberTauntString.PopInt();
		LevelProperties.ChessKnight.ShortAttack p = base.properties.CurrentState.shortAttack;
		base.animator.SetTrigger("RegularAttack");
		base.animator.SetBool("Walking", false);
		yield return CupheadTime.WaitForSeconds(this, p.shortAntiDuration);
		base.animator.SetTrigger("OnAttack");
		yield return CupheadTime.WaitForSeconds(this, p.shortAttackDuration);
		base.animator.SetTrigger("OnAttackEnd");
		yield return base.animator.WaitForAnimationToStart(this, "RegularAttack.RecoveryHold", false);
		yield return CupheadTime.WaitForSeconds(this, p.shortRecoveryDuration);
		if (this.shouldBackDash())
		{
			base.animator.SetTrigger("BackDash");
			yield return base.StartCoroutine(this.backDash_cr());
		}
		else
		{
			base.animator.SetTrigger("ExitRecovery");
			yield return base.animator.WaitForNormalizedTime(this, 1f, "RegularAttack.RecoveryExit", 0, false, false, true);
		}
		this.EndAttack();
		yield break;
	}

	// Token: 0x0600128C RID: 4748 RVA: 0x0000F9BB File Offset: 0x0000DBBB
	public void Long()
	{
		this.isTauntAttack = false;
		this.state = ChessKnightLevelKnight.State.Long;
		base.StartCoroutine(this.long_cr());
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x00095740 File Offset: 0x00093940
	public IEnumerator long_cr()
	{
		this.tauntAttackCounter = this.numberTauntString.PopInt();
		float antiDuration = (!this.isTauntAttack) ? base.properties.CurrentState.longAttack.longAntiDuration : base.properties.CurrentState.tauntAttack.tauntAttackAntiDuration;
		float attackTime = (!this.isTauntAttack) ? base.properties.CurrentState.longAttack.longAttackTime : base.properties.CurrentState.tauntAttack.tauntAttackTime;
		float attackDist = (!this.isTauntAttack) ? base.properties.CurrentState.longAttack.longAttackDist : base.properties.CurrentState.tauntAttack.tauntAttackDist;
		float attackRecovery = (!this.isTauntAttack) ? base.properties.CurrentState.longAttack.longRecoveryDuration : base.properties.CurrentState.tauntAttack.tauntAttackRecoveryDuration;
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		base.animator.SetBool("Walking", false);
		if (this.isTauntAttack)
		{
			base.animator.Play("DashAttack.Anticipation");
			base.animator.SetBool("Taunting", false);
		}
		else
		{
			base.animator.SetTrigger("DashAttack");
		}
		if (antiDuration > 0.7f)
		{
			yield return CupheadTime.WaitForSeconds(this, antiDuration);
			base.animator.SetTrigger("OnAttack");
		}
		else
		{
			if (!this.isTauntAttack)
			{
				yield return base.animator.WaitForAnimationToStart(this, "DashAttack.Anticipation", false);
			}
			yield return CupheadTime.WaitForSeconds(this, Mathf.Max(antiDuration, 0.208333328f));
			base.animator.Play("DashAttack.Attack");
			base.animator.Update(0f);
		}
		float t = 0f;
		float time = attackTime;
		float startPosition = base.transform.position.x;
		float endPosition = (!this.facingLeft) ? (base.transform.position.x + attackDist) : (base.transform.position.x - attackDist);
		endPosition = this.clampEndPosition(endPosition, !this.facingLeft);
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(new float?(Mathf.Lerp(startPosition, endPosition, t / time)), null, null);
			yield return wait;
		}
		base.animator.SetTrigger("OnAttackEnd");
		this.recovery(attackRecovery);
		this.SFX_KOG_KNIGHT_RecoverFoley();
		yield break;
	}

	// Token: 0x0600128E RID: 4750 RVA: 0x0000F9D8 File Offset: 0x0000DBD8
	public void Up()
	{
		this.state = ChessKnightLevelKnight.State.Up;
		base.StartCoroutine(this.up_cr());
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x0009575C File Offset: 0x0009395C
	public IEnumerator up_cr()
	{
		this.tauntAttackCounter = this.numberTauntString.PopInt();
		LevelProperties.ChessKnight.UpAttack p = base.properties.CurrentState.upAttack;
		base.animator.SetTrigger("MoonAttack");
		base.animator.SetBool("Walking", false);
		yield return CupheadTime.WaitForSeconds(this, p.upAntiDuration);
		base.animator.SetTrigger("OnAttack");
		yield return base.animator.WaitForAnimationToStart(this, "Recovery", false);
		this.recovery(p.upRecoveryDuration);
		this.SFX_KOG_KNIGHT_RecoverFoley();
		yield break;
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x0000F9EE File Offset: 0x0000DBEE
	public void recovery(float duration)
	{
		base.StartCoroutine(this.recovery_cr(duration));
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x00095778 File Offset: 0x00093978
	public IEnumerator recovery_cr(float duration)
	{
		this.SFX_KOG_KNIGHT_MoonSlash_Panting();
		yield return CupheadTime.WaitForSeconds(this, duration);
		if (this.shouldBackDash())
		{
			base.animator.SetTrigger("BackDash");
			yield return base.StartCoroutine(this.backDash_cr());
		}
		else if (this.shouldTurn())
		{
			base.animator.SetTrigger("Turn");
			yield return base.animator.WaitForNormalizedTime(this, 1f, "Turn", 0, false, false, true);
			this.turn();
		}
		else
		{
			base.animator.SetTrigger("ExitRecovery");
			yield return base.animator.WaitForNormalizedTime(this, 1f, "RecoveryExit", 0, false, false, true);
		}
		this.SFX_KOG_KNIGHT_MoonSlash_PantingStop();
		this.EndAttack();
		yield break;
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x0009579C File Offset: 0x0009399C
	public bool shouldFaceLeft()
	{
		if (this.targetPlayer == null || this.targetPlayer.IsDead)
		{
			this.targetPlayer = PlayerManager.GetNext();
		}
		return this.targetPlayer.transform.position.x < base.transform.position.x;
	}

	// Token: 0x06001293 RID: 4755 RVA: 0x0000F9FE File Offset: 0x0000DBFE
	public bool shouldTurn()
	{
		return this.shouldFaceLeft() != this.facingLeft;
	}

	// Token: 0x06001294 RID: 4756 RVA: 0x00095804 File Offset: 0x00093A04
	public void turn()
	{
		this.facingLeft = !this.facingLeft;
		base.transform.SetScale(new float?((float)((!this.facingLeft) ? -1 : 1)), null, null);
	}

	// Token: 0x06001295 RID: 4757 RVA: 0x00095858 File Offset: 0x00093A58
	public bool shouldBackDash()
	{
		bool flag = this.shouldFaceLeft();
		float num = (float)((!flag) ? 640 : -640);
		float num2 = 640f;
		float num3 = Mathf.Abs(num - base.transform.position.x);
		bool flag2 = false;
		if (PlayerManager.BothPlayersActive())
		{
			AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			if (Mathf.Sign(base.transform.position.x - player.transform.position.x) != Mathf.Sign(base.transform.position.x - player2.transform.position.x))
			{
				flag2 = true;
			}
		}
		return num3 < num2 && !flag2;
	}

	// Token: 0x06001296 RID: 4758 RVA: 0x00095938 File Offset: 0x00093B38
	public IEnumerator backDash_cr()
	{
		float returnSpeed = (!this.isTauntAttack) ? base.properties.CurrentState.longAttack.longReturnSpeed : base.properties.CurrentState.tauntAttack.tauntAttackReturnSpeed;
		this.facingLeft = this.shouldFaceLeft();
		base.transform.SetScale(new float?((float)((!this.facingLeft) ? -1 : 1)), null, null);
		float startPosition = base.transform.position.x;
		float endPosition = (!this.facingLeft) ? (-640f + this.positionBoundaryInset.minimum) : (640f - this.positionBoundaryInset.minimum);
		float time = Mathf.Abs(endPosition - base.transform.position.x) / returnSpeed;
		float t = 0f;
		base.StartCoroutine(this.backDashAnimation_cr());
		Effect smoke = this.backDashSmoke.Spawn(this.smokeSpawnPoint.position);
		smoke.transform.SetScale(new float?((float)((!this.facingLeft) ? -1 : 1)), null, null);
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			Vector3 previousPosition = base.transform.position;
			t += CupheadTime.FixedDelta;
			if (base.properties.CurrentState.movement.hasEasing)
			{
				float num = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
				base.transform.SetPosition(new float?(Mathf.Lerp(startPosition, endPosition, num)), null, null);
			}
			else if (this.goingLeft && base.transform.position.x > endPosition)
			{
				base.transform.position += Vector3.left * returnSpeed * CupheadTime.FixedDelta;
			}
			else if (!this.goingLeft && base.transform.position.x < endPosition)
			{
				base.transform.position += Vector3.right * returnSpeed * CupheadTime.FixedDelta;
			}
			this.updateAnimatorSpeed(base.transform.position, previousPosition);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001297 RID: 4759 RVA: 0x00095954 File Offset: 0x00093B54
	public IEnumerator backDashAnimation_cr()
	{
		yield return base.animator.WaitForNormalizedTime(this, 1f, "BackDash.Dash", 0, false, false, true);
		this.SFX_KOG_KNIGHT_RecoverFoley();
		base.animator.SetBool("FacingLeft", this.facingLeft);
		base.animator.SetBool("Walking", true);
		yield break;
	}

	// Token: 0x06001298 RID: 4760 RVA: 0x00095970 File Offset: 0x00093B70
	public void death()
	{
		this.StopAllCoroutines();
		this.SFX_KOG_KNIGHT_Die();
		base.animator.SetBool("Walking", false);
		base.animator.SetTrigger("Death");
		for (int i = 0; i < this.deathArmor.Length; i++)
		{
			SpriteDeathPartsDLC spriteDeathPartsDLC = Object.Instantiate<SpriteDeathPartsDLC>(this.deathArmor[i], this.deathArmorSpawns[i].position, Quaternion.identity);
			spriteDeathPartsDLC.transform.localScale = new Vector3(-base.transform.localScale.x, 1f);
			spriteDeathPartsDLC.transform.parent = base.transform;
			spriteDeathPartsDLC.SetVelocity(new Vector3(spriteDeathPartsDLC.transform.localPosition.x * 3f * base.transform.localScale.x, spriteDeathPartsDLC.transform.localPosition.y * 6f + 800f));
		}
	}

	// Token: 0x06001299 RID: 4761 RVA: 0x00095A78 File Offset: 0x00093C78
	public void updateAnimatorSpeed(Vector3 currentPosition, Vector3 previousPosition)
	{
		if (CupheadTime.IsPaused())
		{
			return;
		}
		Vector3 vector = (currentPosition - previousPosition) / CupheadTime.FixedDelta;
		base.animator.SetFloat("XSpeed", vector.x);
		float num = MathUtilities.LerpMapping(Mathf.Abs(vector.x), 0f, this.maximumLegVelocity, this.legSpeedMultiplierRange.minimum, this.legSpeedMultiplierRange.maximum, true);
		base.animator.SetFloat("LegSpeed", num);
	}

	// Token: 0x0600129A RID: 4762 RVA: 0x00095B00 File Offset: 0x00093D00
	public float clampEndPosition(float endPosition, bool isRightSide)
	{
		if (isRightSide)
		{
			float num = 640f - this.positionBoundaryInset.maximum;
			float num2 = 640f - this.positionBoundaryInset.minimum;
			endPosition = Mathf.Clamp(endPosition, num, num2);
		}
		else
		{
			float num3 = -640f + this.positionBoundaryInset.minimum;
			float num4 = -640f + this.positionBoundaryInset.maximum;
			endPosition = Mathf.Clamp(endPosition, num3, num4);
		}
		return endPosition;
	}

	// Token: 0x0600129B RID: 4763 RVA: 0x0000FA11 File Offset: 0x0000DC11
	public void AnimationEvent_SFX_KOG_KNIGHT_AttackUpwards_Stab()
	{
		AudioManager.Play("sfx_dlc_kog_knight_attackupwards_stab");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_attackupwards_stab");
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x0000FA2D File Offset: 0x0000DC2D
	public void AnimationEvent_SFX_KOG_KNIGHT_AttackUpwards_Start()
	{
		AudioManager.Play("sfx_dlc_kog_knight_attackupwards_start");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_attackupwards_start");
	}

	// Token: 0x0600129D RID: 4765 RVA: 0x0000FA49 File Offset: 0x0000DC49
	public void SFX_KOG_KNIGHT_Die()
	{
		AudioManager.Play("sfx_dlc_kog_knight_die");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_die");
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x0000FA65 File Offset: 0x0000DC65
	public void AnimationEvent_SFX_KOG_KNIGHT_Foley_Walk()
	{
		AudioManager.Play("sfx_dlc_kog_knight_foley_walk");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_foley_walk");
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x0000FA81 File Offset: 0x0000DC81
	public void AnimationEvent_SFX_KOG_KNIGHT_Intro_ShieldBash()
	{
		AudioManager.Play("sfx_dlc_kog_knight_intro_shieldbash");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_intro_shieldbash");
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x0000FA9D File Offset: 0x0000DC9D
	public void AnimationEvent_SFX_KOG_KNIGHT_Intro_Visor()
	{
		AudioManager.Play("sfx_dlc_kog_knight_intro_visor");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_intro_visor");
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x0000FAB9 File Offset: 0x0000DCB9
	public void AnimationEvent_SFX_KOG_KNIGHT_MoonSlash_End()
	{
		AudioManager.Stop("sfx_dlc_kog_knight_moonslash_panting");
		AudioManager.Play("sfx_dlc_kog_knight_moonslash_end");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_moonslash_end");
	}

	// Token: 0x060012A2 RID: 4770 RVA: 0x0000FADF File Offset: 0x0000DCDF
	public void SFX_KOG_KNIGHT_MoonSlash_Panting()
	{
		AudioManager.Play("sfx_dlc_kog_knight_moonslash_panting");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_moonslash_panting");
	}

	// Token: 0x060012A3 RID: 4771 RVA: 0x0000FAFB File Offset: 0x0000DCFB
	public void SFX_KOG_KNIGHT_MoonSlash_PantingStop()
	{
		AudioManager.Stop("sfx_dlc_kog_knight_moonslash_panting");
	}

	// Token: 0x060012A4 RID: 4772 RVA: 0x0000FB07 File Offset: 0x0000DD07
	public void AnimationEvent_SFX_KOG_KNIGHT_MoonSlash_Start()
	{
		AudioManager.Play("sfx_dlc_kog_knight_moonslash_start");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_moonslash_start");
	}

	// Token: 0x060012A5 RID: 4773 RVA: 0x0000FB23 File Offset: 0x0000DD23
	public void AnimationEvent_SFX_KOG_KNIGHT_MoonSlash_Swing()
	{
		AudioManager.Play("sfx_dlc_kog_knight_moonslash_swing");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_moonslash_swing");
	}

	// Token: 0x060012A6 RID: 4774 RVA: 0x0000FB3F File Offset: 0x0000DD3F
	public void AnimationEvent_SFX_KOG_KNIGHT_TauntHand()
	{
		AudioManager.Play("sfx_dlc_kog_knight_taunthand");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_taunthand");
	}

	// Token: 0x060012A7 RID: 4775 RVA: 0x0000FB5B File Offset: 0x0000DD5B
	public void AnimationEvent_SFX_KOG_KNIGHT_Dash_End()
	{
		AudioManager.Play("sfx_dlc_kog_knight_dash_end");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_dash_end");
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x0000FB77 File Offset: 0x0000DD77
	public void SFX_KOG_KNIGHT_RecoverFoley()
	{
		AudioManager.Play("sfx_dlc_kog_knight_recoverfoley");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_recoverfoley");
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x0000FB93 File Offset: 0x0000DD93
	public void AnimationEvent_SFX_KOG_KNIGHT_Dash_Start()
	{
		AudioManager.Play("sfx_dlc_kog_knight_dash_start");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_dash_start");
	}

	// Token: 0x060012AA RID: 4778 RVA: 0x0000FBAF File Offset: 0x0000DDAF
	public void AnimationEvent_SFX_KOG_KNIGHT_Dash_Attack()
	{
		AudioManager.Play("sfx_dlc_kog_knight_dash_attack");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_dash_attack");
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x0000FBCB File Offset: 0x0000DDCB
	public void AnimationEvent_SFX_KOG_KNIGHT_Vocal_Attack()
	{
		AudioManager.Play("sfx_dlc_kog_knight_vocal_attack");
		this.emitAudioFromObject.Add("sfx_dlc_kog_knight_vocal_attack");
	}

	// Token: 0x04000EE9 RID: 3817
	[SerializeField]
	public ParrySwitch pink;

	// Token: 0x04000EEA RID: 3818
	[SerializeField]
	public CollisionChild swordHitbox;

	// Token: 0x04000EEB RID: 3819
	[SerializeField]
	public CollisionChild upHitbox;

	// Token: 0x04000EEC RID: 3820
	[SerializeField]
	public Rangef positionBoundaryInset;

	// Token: 0x04000EED RID: 3821
	[SerializeField]
	public Transform smokeSpawnPoint;

	// Token: 0x04000EEE RID: 3822
	[SerializeField]
	public Effect backDashSmoke;

	// Token: 0x04000EEF RID: 3823
	[SerializeField]
	public Rangef legSpeedMultiplierRange;

	// Token: 0x04000EF0 RID: 3824
	[SerializeField]
	public float maximumLegVelocity;

	// Token: 0x04000EF1 RID: 3825
	[SerializeField]
	public SpriteDeathPartsDLC[] deathArmor;

	// Token: 0x04000EF2 RID: 3826
	[SerializeField]
	public Transform[] deathArmorSpawns;

	// Token: 0x04000EF3 RID: 3827
	[SerializeField]
	public HitFlash hitFlash;

	// Token: 0x04000EF4 RID: 3828
	public DamageDealer damageDealer;

	// Token: 0x04000EF5 RID: 3829
	public float battleStartPosition;

	// Token: 0x04000EF6 RID: 3830
	public bool goingLeft = true;

	// Token: 0x04000EF7 RID: 3831
	public bool facingLeft = true;

	// Token: 0x04000EF8 RID: 3832
	public AbstractPlayerController targetPlayer;

	// Token: 0x04000EF9 RID: 3833
	public PatternString attackIntervalPattern;

	// Token: 0x04000EFA RID: 3834
	public PatternString movementPattern;

	// Token: 0x04000EFB RID: 3835
	public PatternString numberTauntString;

	// Token: 0x04000EFC RID: 3836
	public int tauntAttackCounter;

	// Token: 0x04000EFD RID: 3837
	public bool isTauntAttack;

	// Token: 0x02000ABC RID: 2748
	public enum State
	{
		// Token: 0x04004ECE RID: 20174
		Intro,
		// Token: 0x04004ECF RID: 20175
		Move,
		// Token: 0x04004ED0 RID: 20176
		Idle,
		// Token: 0x04004ED1 RID: 20177
		Short,
		// Token: 0x04004ED2 RID: 20178
		Long,
		// Token: 0x04004ED3 RID: 20179
		Up,
		// Token: 0x04004ED4 RID: 20180
		Taunt,
		// Token: 0x04004ED5 RID: 20181
		Death
	}
}
