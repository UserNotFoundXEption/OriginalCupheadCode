using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200033B RID: 827
public class RumRunnersLevelAnteater : LevelProperties.RumRunners.Entity
{
	// Token: 0x06002417 RID: 9239 RVA: 0x0001E73B File Offset: 0x0001C93B
	public void OnEnable()
	{
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.onDamageTakenEventHandler;
	}

	// Token: 0x06002418 RID: 9240 RVA: 0x0001E754 File Offset: 0x0001C954
	public void OnDisable()
	{
		base.GetComponent<DamageReceiver>().OnDamageTaken -= this.onDamageTakenEventHandler;
	}

	// Token: 0x06002419 RID: 9241 RVA: 0x000C298C File Offset: 0x000C0B8C
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		foreach (CollisionChild collisionChild in this.collChildren)
		{
			collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		}
		this.tongue.OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x0001E76D File Offset: 0x0001C96D
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600241B RID: 9243 RVA: 0x0001E785 File Offset: 0x0001C985
	public void DoDamage(float damage)
	{
		base.properties.DealDamage(damage);
	}

	// Token: 0x0600241C RID: 9244 RVA: 0x0001E793 File Offset: 0x0001C993
	public void onDamageTakenEventHandler(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600241D RID: 9245 RVA: 0x0001E7A6 File Offset: 0x0001C9A6
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x000C29F0 File Offset: 0x000C0BF0
	public override void LevelInit(LevelProperties.RumRunners properties)
	{
		base.LevelInit(properties);
		this.snoutAttackPattern = new PatternString(properties.CurrentState.anteaterSnout.snoutActionArray, true, false);
		this.snoutPositionPattern = new PatternString(properties.CurrentState.anteaterSnout.snoutPosString, true, true);
		this.snout.Setup(properties);
	}

	// Token: 0x0600241F RID: 9247 RVA: 0x0001E7CE File Offset: 0x0001C9CE
	public void StartAnteater()
	{
		base.StartCoroutine(this.snout_cr());
	}

	// Token: 0x06002420 RID: 9248 RVA: 0x000C2A4C File Offset: 0x000C0C4C
	public IEnumerator snout_cr()
	{
		LevelProperties.RumRunners.AnteaterSnout p = base.properties.CurrentState.anteaterSnout;
		for (;;)
		{
			RumRunnersLevelAnteater.AttackIntro attackIntro = (!Rand.Bool()) ? RumRunnersLevelAnteater.AttackIntro.B : RumRunnersLevelAnteater.AttackIntro.A;
			base.animator.SetInteger("AttackIntro", (int)attackIntro);
			int count = this.snoutAttackPattern.SubStringLength();
			for (int i = 0; i < count; i++)
			{
				RumRunnersLevelSnout.AttackType attackType;
				RumRunnersLevelAnteater.AttackIntroLength introLength;
				this.parseAttackString(this.snoutAttackPattern.GetString(), out attackType, out introLength);
				this.snoutAttackPattern.IncrementString();
				RumRunnersLevelSnout.AttackType nextAttackType;
				RumRunnersLevelAnteater.AttackIntroLength nextIntroLength;
				this.parseAttackString(this.snoutAttackPattern.GetString(), out nextAttackType, out nextIntroLength);
				this.queuedAttack = attackType;
				if (p.anticipationBoilDelay > 0f)
				{
					yield return CupheadTime.WaitForSeconds(this, p.anticipationBoilDelay);
				}
				base.animator.SetTrigger("AnticipationComplete");
				while (!this.snout.isAttacking)
				{
					yield return null;
				}
				string endAnimationName;
				if (i == 0)
				{
					endAnimationName = "AttackInitialEnd";
				}
				else
				{
					string str = (attackIntro != RumRunnersLevelAnteater.AttackIntro.A) ? "AttackB." : "AttackA.";
					endAnimationName = str + "AttackEnd";
				}
				bool nextAttackIsFinal = nextAttackType == RumRunnersLevelSnout.AttackType.Tongue || i == count - 1;
				if (!nextAttackIsFinal)
				{
					attackIntro = ((attackIntro != RumRunnersLevelAnteater.AttackIntro.A) ? RumRunnersLevelAnteater.AttackIntro.A : RumRunnersLevelAnteater.AttackIntro.B);
				}
				else
				{
					if (i < count - 1 && nextAttackType == RumRunnersLevelSnout.AttackType.Tongue)
					{
						this.snoutAttackPattern.IncrementString();
					}
					attackIntro = RumRunnersLevelAnteater.AttackIntro.Final;
				}
				base.animator.SetInteger("AttackIntro", (int)attackIntro);
				base.animator.SetBool("LongIntro", nextIntroLength == RumRunnersLevelAnteater.AttackIntroLength.Long);
				while (this.snout.isAttacking)
				{
					yield return null;
				}
				base.animator.SetTrigger("EndAttack");
				yield return base.animator.WaitForAnimationToEnd(this, endAnimationName, false, true);
				if (nextAttackIsFinal)
				{
					break;
				}
			}
			this.queuedAttack = RumRunnersLevelSnout.AttackType.Tongue;
			if (p.anticipationBoilDelay > 0f)
			{
				yield return CupheadTime.WaitForSeconds(this, p.anticipationBoilDelay);
			}
			base.animator.SetTrigger("AnticipationComplete");
			base.StartCoroutine(this.finalAttackEyes_cr());
			while (!this.snout.isAttacking)
			{
				yield return null;
			}
			while (this.snout.isAttacking)
			{
				yield return null;
			}
			base.animator.SetTrigger("EndAttack");
			base.animator.Play("Off", RumRunnersLevelAnteater.EyesAnimationLayer, 0f);
			base.animator.Update(0f);
			yield return base.animator.WaitForAnimationToStart(this, "AttackFinal.AttackEndHold", false);
			yield return CupheadTime.WaitForSeconds(this, p.finalAttackTauntDuration);
			base.animator.SetTrigger("EndTaunt");
			yield return base.animator.WaitForAnimationToEnd(this, "AttackFinal.AttackEnd", false, true);
		}
		yield break;
	}

	// Token: 0x06002421 RID: 9249 RVA: 0x000C2A68 File Offset: 0x000C0C68
	public IEnumerator finalAttackEyes_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "AttackFinal.AttackStart", false, true);
		base.animator.Play("Hold", RumRunnersLevelAnteater.EyesAnimationLayer, 0f);
		base.animator.Update(0f);
		yield break;
	}

	// Token: 0x06002422 RID: 9250 RVA: 0x000C2A84 File Offset: 0x000C0C84
	public void FakeDeathStart()
	{
		this.StopAllCoroutines();
		LevelProperties.RumRunners.Boss boss = base.properties.CurrentState.boss;
		this.mobBoss.Setup(base.properties, this, this.mobBossHelperTransform);
		foreach (SpriteRenderer spriteRenderer in this.flipRenderers)
		{
			spriteRenderer.flipX = false;
		}
		this.snout.Death();
		base.animator.Play("Off", RumRunnersLevelAnteater.EyesAnimationLayer);
		base.animator.Play("Off", RumRunnersLevelAnteater.HandsAnimatorLayer);
		base.animator.Play("Death");
		base.animator.Update(0f);
		base.GetComponent<AnimationHelper>().Speed = 0f;
		this.eyes.transform.localPosition = new Vector3(2f, -62f);
		base.GetComponent<LevelBossDeathExploder>().StartExplosion(true);
	}

	// Token: 0x06002423 RID: 9251 RVA: 0x0001E7DD File Offset: 0x0001C9DD
	public void FakeDeathContinue()
	{
		base.GetComponent<AnimationHelper>().Speed = 1f;
	}

	// Token: 0x06002424 RID: 9252 RVA: 0x000C2B78 File Offset: 0x000C0D78
	public IEnumerator fakeDeathEyes_cr()
	{
		PatternString eyesPattern = new PatternString(base.properties.CurrentState.boss.anteaterEyeClosedOpenString, true);
		for (;;)
		{
			string[] currentPattern = eyesPattern.PopString().Split(new char[]
			{
				':'
			});
			if (currentPattern.Length != 2)
			{
				break;
			}
			int closedCount;
			Parser.IntTryParse(currentPattern[0], out closedCount);
			int openCount;
			Parser.IntTryParse(currentPattern[1], out openCount);
			yield return base.StartCoroutine(this.eyeHandler_cr(closedCount, "DeathLoop"));
			yield return base.animator.WaitForAnimationToStart(this, "DeathLoopEyeOpen", false);
			yield return base.StartCoroutine(this.eyeHandler_cr(openCount, "DeathLoopOpen"));
			yield return base.animator.WaitForAnimationToStart(this, "DeathLoopEyeClose", false);
		}
		throw new Exception("Invalid anteater eye pattern");
		yield break;
	}

	// Token: 0x06002425 RID: 9253 RVA: 0x000C2B94 File Offset: 0x000C0D94
	public IEnumerator eyeHandler_cr(int count, string loopAnimationName)
	{
		if (count == 0)
		{
			base.animator.SetTrigger("DeathLoopEyeChange");
		}
		else
		{
			yield return base.animator.WaitForAnimationToStart(this, loopAnimationName, false);
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < (float)count - 0.5f)
			{
				yield return null;
			}
			base.animator.SetTrigger("DeathLoopEyeChange");
		}
		yield break;
	}

	// Token: 0x06002426 RID: 9254 RVA: 0x0001E7EF File Offset: 0x0001C9EF
	public void RealDeath()
	{
		this.StopAllCoroutines();
		base.animator.Play("ActualDeath", 0, 0.2631579f);
	}

	// Token: 0x06002427 RID: 9255 RVA: 0x000C2BC0 File Offset: 0x000C0DC0
	public void animationEvent_LeftDirt()
	{
		CupheadLevelCamera.Current.Shake(20f, 0.3f, true);
		((RumRunnersLevel)Level.Current).FullscreenDirt(1, new float?(-640f + Random.Range(100f, 200f)), -1f, -1f);
	}

	// Token: 0x06002428 RID: 9256 RVA: 0x000C2C18 File Offset: 0x000C0E18
	public void animationEvent_RightDirt()
	{
		CupheadLevelCamera.Current.Shake(20f, 0.3f, true);
		((RumRunnersLevel)Level.Current).FullscreenDirt(1, new float?(640f - Random.Range(100f, 200f)), -1f, -1f);
	}

	// Token: 0x06002429 RID: 9257 RVA: 0x0001E80D File Offset: 0x0001CA0D
	public void animationEvent_MiddleBridgeDestroy()
	{
		((RumRunnersLevel)Level.Current).DestroyMiddleBridge();
	}

	// Token: 0x0600242A RID: 9258 RVA: 0x0001E81E File Offset: 0x0001CA1E
	public void animationEvent_UpperBridgeDestroy()
	{
		((RumRunnersLevel)Level.Current).DestroyUpperBridge();
	}

	// Token: 0x0600242B RID: 9259 RVA: 0x0001E82F File Offset: 0x0001CA2F
	public void animationEvent_BridgeShatter()
	{
		((RumRunnersLevel)Level.Current).ShatterBridges();
	}

	// Token: 0x0600242C RID: 9260 RVA: 0x000C2C70 File Offset: 0x000C0E70
	public void animationEvent_InitialAttackStarted()
	{
		if (this.firstAttack)
		{
			this.firstAttack = false;
			return;
		}
		this.onLeft = !this.onLeft;
		foreach (SpriteRenderer spriteRenderer in this.flipRenderers)
		{
			spriteRenderer.flipX = !this.onLeft;
		}
	}

	// Token: 0x0600242D RID: 9261 RVA: 0x000C2CD0 File Offset: 0x000C0ED0
	public void animationEvent_SnoutAttack()
	{
		int num = this.snoutPositionPattern.PopInt();
		Vector2 vector;
		vector.x = ((!this.onLeft) ? RumRunnersLevelAnteater.SNOUT_SPAWN_X : (-RumRunnersLevelAnteater.SNOUT_SPAWN_X));
		vector.y = this.spawnPoints[num].position.y;
		this.snout.Attack(vector, this.snoutShadowPositions[num], this.onLeft, this.queuedAttack);
	}

	// Token: 0x0600242E RID: 9262 RVA: 0x000C2D58 File Offset: 0x000C0F58
	public void animationEvent_HandsUp()
	{
		int num;
		if (this.onLeft)
		{
			num = RumRunnersLevelAnteater.HandsUpAnimatorHash;
		}
		else
		{
			num = RumRunnersLevelAnteater.HandsDownAnimatorHash;
		}
		AnimatorStateInfo currentAnimatorStateInfo = base.animator.GetCurrentAnimatorStateInfo(RumRunnersLevelAnteater.HandsAnimatorLayer);
		if (currentAnimatorStateInfo.shortNameHash != num || currentAnimatorStateInfo.normalizedTime >= 1f)
		{
			base.animator.Play(num, RumRunnersLevelAnteater.HandsAnimatorLayer, 0f);
		}
		base.animator.Update(0f);
	}

	// Token: 0x0600242F RID: 9263 RVA: 0x000C2DD8 File Offset: 0x000C0FD8
	public void animationEvent_HandsDown()
	{
		int num;
		if (this.onLeft)
		{
			num = RumRunnersLevelAnteater.HandsDownAnimatorHash;
		}
		else
		{
			num = RumRunnersLevelAnteater.HandsUpAnimatorHash;
		}
		AnimatorStateInfo currentAnimatorStateInfo = base.animator.GetCurrentAnimatorStateInfo(RumRunnersLevelAnteater.HandsAnimatorLayer);
		if (currentAnimatorStateInfo.shortNameHash != num || currentAnimatorStateInfo.normalizedTime >= 1f)
		{
			base.animator.Play(num, RumRunnersLevelAnteater.HandsAnimatorLayer, 0f);
		}
		base.animator.Update(0f);
	}

	// Token: 0x06002430 RID: 9264 RVA: 0x000C2E58 File Offset: 0x000C1058
	public void animationEvent_HandsUpHalfway()
	{
		int num;
		if (this.onLeft)
		{
			num = RumRunnersLevelAnteater.HandsUpAnimatorHash;
		}
		else
		{
			num = RumRunnersLevelAnteater.HandsDownAnimatorHash;
		}
		AnimatorStateInfo currentAnimatorStateInfo = base.animator.GetCurrentAnimatorStateInfo(RumRunnersLevelAnteater.HandsAnimatorLayer);
		base.animator.Play(num, RumRunnersLevelAnteater.HandsAnimatorLayer, 0.5f);
		base.animator.Update(0f);
	}

	// Token: 0x06002431 RID: 9265 RVA: 0x000C2EB8 File Offset: 0x000C10B8
	public void animationEvent_HandsStartTaunt()
	{
		if (this.onLeft)
		{
			base.animator.Play("HandsTaunt", 2, 0f);
		}
		else
		{
			base.animator.Play("HandsTauntRight", 2, 0f);
		}
		base.animator.Update(0f);
	}

	// Token: 0x06002432 RID: 9266 RVA: 0x0001E840 File Offset: 0x0001CA40
	public void animationEvent_HandsEndTaunt()
	{
		base.animator.SetTrigger("HandsEndTaunt");
	}

	// Token: 0x06002433 RID: 9267 RVA: 0x0001E852 File Offset: 0x0001CA52
	public void animationEvent_FalseDeathDust()
	{
		base.animator.Play("DeathDust", RumRunnersLevelAnteater.DeathDustAnimatorLayer);
		CupheadLevelCamera.Current.Shake(35f, 0.5f, false);
	}

	// Token: 0x06002434 RID: 9268 RVA: 0x0001E87E File Offset: 0x0001CA7E
	public void animationEvent_FalseDeathEnded()
	{
		this.mobBoss.Begin();
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		base.StartCoroutine(this.fakeDeathEyes_cr());
	}

	// Token: 0x06002435 RID: 9269 RVA: 0x000C2F14 File Offset: 0x000C1114
	public void parseAttackString(string attackString, out RumRunnersLevelSnout.AttackType attackType, out RumRunnersLevelAnteater.AttackIntroLength introLength)
	{
		char c;
		char c2;
		if (attackString.Length == 2)
		{
			c = attackString[1];
			c2 = attackString[0];
		}
		else
		{
			c = attackString[0];
			c2 = '0';
		}
		if (c == 'Q')
		{
			attackType = RumRunnersLevelSnout.AttackType.Quick;
		}
		else if (c == 'F')
		{
			attackType = RumRunnersLevelSnout.AttackType.Fake;
		}
		else
		{
			if (c != 'T')
			{
				throw new Exception("Invalid attack string: " + attackString);
			}
			attackType = RumRunnersLevelSnout.AttackType.Tongue;
		}
		if (c2 == 'L')
		{
			introLength = RumRunnersLevelAnteater.AttackIntroLength.Long;
		}
		else
		{
			introLength = RumRunnersLevelAnteater.AttackIntroLength.Standard;
		}
	}

	// Token: 0x06002436 RID: 9270 RVA: 0x000C2FA4 File Offset: 0x000C11A4
	public void TriggerEyesTurnaround()
	{
		base.animator.SetTrigger((this.snout.transform.position.y >= 150f) ? "EyesLookUp" : "EyesLookDown");
	}

	// Token: 0x06002437 RID: 9271 RVA: 0x000C2FF0 File Offset: 0x000C11F0
	public void SetEyeSide(bool onLeft)
	{
		this.eyes.transform.localPosition = new Vector3((!onLeft) ? this.eyePositionAttack.x : (-this.eyePositionAttack.x), this.eyePositionAttack.y);
	}

	// Token: 0x06002438 RID: 9272 RVA: 0x0001E8A3 File Offset: 0x0001CAA3
	public void AnimationEvent_SFX_RUMRUN_P3_Anteater_Intro()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_intro");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_intro");
	}

	// Token: 0x06002439 RID: 9273 RVA: 0x0001E8BF File Offset: 0x0001CABF
	public void AnimationEvent_SFX_RUMRUN_P3_Anteater_HandSlamFirst()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_handslamfirst");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_handslamfirst");
	}

	// Token: 0x0600243A RID: 9274 RVA: 0x0001E8DB File Offset: 0x0001CADB
	public void AnimationEvent_SFX_RUMRUN_P3_Anteater_HandSlamSecond()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_handslamsecond");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_handslamsecond");
	}

	// Token: 0x0600243B RID: 9275 RVA: 0x0001E8F7 File Offset: 0x0001CAF7
	public void AnimationEvent_SFX_RUMRUN_P3_Anteater_Intro_Hat_Off()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_intro_hat_off");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_intro_hat_off");
	}

	// Token: 0x0600243C RID: 9276 RVA: 0x0001E913 File Offset: 0x0001CB13
	public void AnimationEvent_SFX_RUMRUN_P3_Anteater_Intro_Hatton()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_intro_hatton");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_intro_hatton");
	}

	// Token: 0x0600243D RID: 9277 RVA: 0x0001E92F File Offset: 0x0001CB2F
	public void AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_Start()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_initial_start");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_initial_start");
	}

	// Token: 0x0600243E RID: 9278 RVA: 0x0001E94B File Offset: 0x0001CB4B
	public void AnimationEvent_SFX_RUMRUN_P4_IntroSnailLaugh()
	{
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_laughing");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_vx_fakeannouncer_laughing");
	}

	// Token: 0x04001DE8 RID: 7656
	public static readonly float SNOUT_SPAWN_X = 0f;

	// Token: 0x04001DE9 RID: 7657
	public static readonly int EyesAnimationLayer = 1;

	// Token: 0x04001DEA RID: 7658
	public static readonly int HandsAnimatorLayer = 2;

	// Token: 0x04001DEB RID: 7659
	public static readonly int DeathDustAnimatorLayer = 3;

	// Token: 0x04001DEC RID: 7660
	public static readonly int HandsUpAnimatorHash = Animator.StringToHash("HandsUp");

	// Token: 0x04001DED RID: 7661
	public static readonly int HandsDownAnimatorHash = Animator.StringToHash("HandsDown");

	// Token: 0x04001DEE RID: 7662
	public Vector2 eyePositionAttack = new Vector2(348f, 145f);

	// Token: 0x04001DEF RID: 7663
	[SerializeField]
	public Transform[] spawnPoints;

	// Token: 0x04001DF0 RID: 7664
	[SerializeField]
	public Vector2[] snoutShadowPositions;

	// Token: 0x04001DF1 RID: 7665
	[SerializeField]
	public RumRunnersLevelSnout snout;

	// Token: 0x04001DF2 RID: 7666
	[SerializeField]
	public RumRunnersLevelMobBoss mobBoss;

	// Token: 0x04001DF3 RID: 7667
	[SerializeField]
	public Transform mobBossHelperTransform;

	// Token: 0x04001DF4 RID: 7668
	[SerializeField]
	public CollisionChild[] collChildren;

	// Token: 0x04001DF5 RID: 7669
	[SerializeField]
	public RumRunnersLevelSnoutTongue tongue;

	// Token: 0x04001DF6 RID: 7670
	[SerializeField]
	public SpriteRenderer[] flipRenderers;

	// Token: 0x04001DF7 RID: 7671
	[SerializeField]
	public float blinkProbability;

	// Token: 0x04001DF8 RID: 7672
	[SerializeField]
	public GameObject eyes;

	// Token: 0x04001DF9 RID: 7673
	public DamageDealer damageDealer;

	// Token: 0x04001DFA RID: 7674
	public PatternString snoutAttackPattern;

	// Token: 0x04001DFB RID: 7675
	public PatternString snoutPositionPattern;

	// Token: 0x04001DFC RID: 7676
	public bool onLeft = true;

	// Token: 0x04001DFD RID: 7677
	public bool firstAttack = true;

	// Token: 0x04001DFE RID: 7678
	public RumRunnersLevelSnout.AttackType queuedAttack;

	// Token: 0x02000E9B RID: 3739
	public enum AttackIntro
	{
		// Token: 0x04006918 RID: 26904
		Initial = -1,
		// Token: 0x04006919 RID: 26905
		A,
		// Token: 0x0400691A RID: 26906
		B,
		// Token: 0x0400691B RID: 26907
		Final
	}

	// Token: 0x02000E9C RID: 3740
	public enum AttackIntroLength
	{
		// Token: 0x0400691D RID: 26909
		Standard,
		// Token: 0x0400691E RID: 26910
		Long
	}
}
