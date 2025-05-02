using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200032F RID: 815
public class RobotLevelRobotChest : RobotLevelRobotBodyPart
{
	// Token: 0x0600238C RID: 9100 RVA: 0x000C09FC File Offset: 0x000BEBFC
	public override void InitBodyPart(RobotLevelRobot parent, LevelProperties.Robot properties, int primaryHP = 0, int secondaryHP = 1, float attackDelayMinus = 0f)
	{
		this.primaryAttackDelay = properties.CurrentState.orb.orbInitialSpawnDelay.RandomFloat();
		this.secondaryAttackDelay = properties.CurrentState.arms.attackDelayRange.RandomFloat();
		primaryHP = properties.CurrentState.orb.chestHP;
		secondaryHP = properties.CurrentState.heart.heartHP;
		attackDelayMinus = properties.CurrentState.orb.orbSpawnDelayMinus;
		this.armsTypeIndex = Random.Range(0, properties.CurrentState.arms.attackString.Split(new char[]
		{
			','
		}).Length);
		this.twistyArmsPositionIndex = Random.Range(0, properties.CurrentState.twistyArms.twistyPositionString.Split(new char[]
		{
			','
		}).Length);
		base.InitBodyPart(parent, properties, primaryHP, secondaryHP, attackDelayMinus);
		base.animator.Play("Porthole_Idle", 0, 0.75f);
		base.animator.Play("Off_Idle", 1, 0.75f);
		base.StartCoroutine(this.panicArmsLoop_cr());
		base.StartCoroutine(this.close_porthole_cr());
		this.StartPrimary();
		this.damageEffectRenderer = this.damageEffect.GetComponent<SpriteRenderer>();
	}

	// Token: 0x0600238D RID: 9101 RVA: 0x0001E102 File Offset: 0x0001C302
	public override void OnPrimaryAttack()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			base.animator.SetTrigger("OnPortOpen");
			base.OnPrimaryAttack();
		}
	}

	// Token: 0x0600238E RID: 9102 RVA: 0x0001E125 File Offset: 0x0001C325
	public void SpawnOrb()
	{
		if (this.current != RobotLevelRobotBodyPart.state.primary)
		{
			return;
		}
		base.StartCoroutine(this.spawn_orb_cr());
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000C0B3C File Offset: 0x000BED3C
	public IEnumerator spawn_orb_cr()
	{
		RobotLevelOrb orb = this.primary.GetComponent<RobotLevelOrb>().Create(this.portholeRoot.transform.position, this.portholeOffsetRoot.position);
		this.primaryAttackDelay = this.properties.CurrentState.orb.orbSpawnDelay;
		orb.InitOrb(this.properties);
		this.isAttacking = false;
		base.animator.SetTrigger("OnFirstClose");
		yield return null;
		yield break;
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x000C0B58 File Offset: 0x000BED58
	public IEnumerator close_porthole_cr()
	{
		base.animator.SetTrigger("OnFirstClose");
		yield return null;
		yield break;
	}

	// Token: 0x06002391 RID: 9105 RVA: 0x000C0B74 File Offset: 0x000BED74
	public override void OnPrimaryDeath()
	{
		if (this.current != RobotLevelRobotBodyPart.state.secondary && this.currentHealth[0] <= 0f)
		{
			AudioManager.Play("robot_upper_chest_port_destroyed");
			this.emitAudioFromObject.Add("robot_upper_chest_port_destroyed");
			this.torsoTop.enabled = false;
			this.StartSecondary();
			base.GetComponent<Collider2D>().enabled = false;
			this.DeathEffect();
			this.panicArmsLoop = true;
			this.parent.animator.Play("Transition_Arms");
			foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.enabled = false;
			}
			foreach (GameObject gameObject in this.damagedPortholes)
			{
				gameObject.SetActive(true);
				gameObject.GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		base.OnPrimaryDeath();
	}

	// Token: 0x06002392 RID: 9106 RVA: 0x000C0C64 File Offset: 0x000BEE64
	public void EnablePorthole()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.enabled = true;
			}
		}
	}

	// Token: 0x06002393 RID: 9107 RVA: 0x000C0CA8 File Offset: 0x000BEEA8
	public void DisablePorthole()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.enabled = false;
			}
		}
	}

	// Token: 0x06002394 RID: 9108 RVA: 0x0001E140 File Offset: 0x0001C340
	public override void StartSecondary()
	{
		this.secondary = Object.Instantiate<GameObject>(this.secondary);
		this.secondaryAnimator = this.secondary.GetComponent<Animator>();
		base.StartCoroutine(this.secondaryStart_cr());
	}

	// Token: 0x06002395 RID: 9109 RVA: 0x000C0CEC File Offset: 0x000BEEEC
	public IEnumerator secondaryStart_cr()
	{
		yield return this.parent.animator.WaitForAnimationToEnd(this.parent, "Extend", 5, true, true);
		this.<StartSecondary>__BaseCallProxy0();
		yield break;
	}

	// Token: 0x06002396 RID: 9110 RVA: 0x000C0D08 File Offset: 0x000BEF08
	public override void OnSecondaryAttack()
	{
		if (!this.armsActive)
		{
			if ((float)Random.Range(0, 100) <= 25f && !AudioManager.CheckIfPlaying("robot_vocals_laugh"))
			{
				AudioManager.Play("robot_vocals_laugh");
				this.emitAudioFromObject.Add("robot_vocals_laugh");
			}
			this.parent.animator.Play("Fast", 5, this.parent.animator.GetCurrentAnimatorStateInfo(5).normalizedTime % 1f);
			this.secondaryAttackDelay = 0f;
			this.armsActive = true;
			char c = this.properties.CurrentState.arms.attackString.Split(new char[]
			{
				','
			})[this.armsTypeIndex][0];
			if (c != 'M')
			{
				if (c == 'T')
				{
					base.StartCoroutine(this.twistyArmsEnter_cr());
				}
			}
			else
			{
				base.StartCoroutine(this.magnetArmsIntro_cr());
			}
		}
		base.OnSecondaryAttack();
	}

	// Token: 0x06002397 RID: 9111 RVA: 0x000C0E1C File Offset: 0x000BF01C
	public IEnumerator twistyArmsEnter_cr()
	{
		this.secondary.GetComponent<RobotLevelSecondaryArms>().InitHelper(this.properties);
		this.secondaryAnimator.Play("Twisty_Arms_Enter", 0, 0f);
		AudioManager.Play("robot_arms_extend_appear");
		yield return null;
		this.spriteBounds = this.secondary.GetComponent<SpriteRenderer>().bounds.size;
		this.secondary.transform.position = new Vector3(-1248f, (float)(Level.Current.Ground + Parser.IntParse(this.properties.CurrentState.twistyArms.twistyPositionString.Split(new char[]
		{
			','
		})[this.twistyArmsPositionIndex])), 0f);
		this.secondary.transform.rotation = Quaternion.identity;
		while (this.secondary.transform.position.x < -1248f + this.properties.CurrentState.twistyArms.warningArmsMoveAmount)
		{
			this.secondary.transform.position += Vector3.right * this.properties.CurrentState.twistyArms.twistyMoveSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.twistyArms.warningDuration);
		this.secondary.GetComponent<BoxCollider2D>().enabled = true;
		AudioManager.Play("robot_arms_extend_across");
		this.emitAudioFromObject.Add("robot_arms_extend_across");
		while (this.secondary.transform.position.x < -1248f + this.spriteBounds.x / 8f * 6f)
		{
			this.secondary.transform.position += Vector3.right * this.properties.CurrentState.twistyArms.twistyMoveSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.twistyArms.twistyArmsStayDuration);
		AudioManager.Play("robot_arms_extend_back");
		this.emitAudioFromObject.Add("robot_arms_extend_back");
		base.StartCoroutine(this.twistyArmsExit_cr(false));
		yield break;
	}

	// Token: 0x06002398 RID: 9112 RVA: 0x000C0E38 File Offset: 0x000BF038
	public IEnumerator twistyArmsExit_cr(bool isPermaDeath = false)
	{
		float speedMultiplier = 1f;
		if (isPermaDeath)
		{
			speedMultiplier = 2f;
		}
		float nt = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
		this.secondaryAnimator.Play("Twisty_Arms_Exit", -1, nt);
		while (this.secondary.transform.position.x > -1248f)
		{
			this.secondary.transform.position += Vector3.left * this.properties.CurrentState.twistyArms.twistyMoveSpeed * speedMultiplier * CupheadTime.Delta;
			yield return null;
		}
		this.secondaryAnimator.Play("Twisty_Arms_Enter");
		this.twistyArmsPositionIndex++;
		if (this.twistyArmsPositionIndex >= this.properties.CurrentState.twistyArms.twistyPositionString.Split(new char[]
		{
			','
		}).Length)
		{
			this.twistyArmsPositionIndex = 0;
		}
		this.secondaryAttackDelay = this.properties.CurrentState.arms.attackDelayRange.RandomFloat();
		yield return null;
		this.secondary.GetComponent<BoxCollider2D>().enabled = false;
		this.armsActive = false;
		this.armsTypeIndex++;
		if (this.armsTypeIndex >= this.properties.CurrentState.arms.attackString.Split(new char[]
		{
			','
		}).Length)
		{
			this.armsTypeIndex = 0;
		}
		this.parent.animator.Play("Slow", 5, this.parent.animator.GetCurrentAnimatorStateInfo(5).normalizedTime % 1f);
		yield break;
	}

	// Token: 0x06002399 RID: 9113 RVA: 0x000C0E5C File Offset: 0x000BF05C
	public IEnumerator magnetArmsIntro_cr()
	{
		this.secondaryAnimator.Play("Magnet_Arms", -1, 0f);
		yield return null;
		this.spriteBounds = this.secondary.GetComponent<SpriteRenderer>().bounds.size;
		this.secondary.transform.position = this.magnetStartRoot.transform.position;
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.magnetArms.magnetStartDelay);
		AudioManager.Play("robot_magnet_arms_start");
		while (AudioManager.CheckIfPlaying("robot_magnet_arms_start"))
		{
			yield return null;
		}
		AudioManager.PlayLoop("robot_magnet_arms_loop");
		float t = 0f;
		float time = 1.8f;
		float deltaRotation = 0f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			deltaRotation = Mathf.Lerp(60f, 0f, t / time);
			this.secondary.transform.position = Vector2.Lerp(this.magnetStartRoot.transform.position, this.magnetEndRoot.transform.position, t / time);
			this.secondary.transform.SetEulerAngles(null, null, new float?(deltaRotation));
			yield return null;
		}
		time = 0f;
		while (time < this.properties.CurrentState.magnetArms.magnetStayDelay)
		{
			time += CupheadTime.Delta;
			foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
			{
				PlanePlayerController player = (PlanePlayerController)abstractPlayerController;
				if (!(player == null))
				{
					Vector2 playerForce = (PlayerManager.GetNext().center - this.secondary.transform.GetChild(1).transform.position).normalized * this.properties.CurrentState.magnetArms.magnetForce * 0.5f;
					this.force = new PlanePlayerMotor.Force(playerForce, true);
					player.motor.AddForce(this.force);
					yield return null;
					time += CupheadTime.Delta;
					if (player.motor != null)
					{
						player.motor.RemoveForce(this.force);
					}
				}
			}
			if (this.current == RobotLevelRobotBodyPart.state.none)
			{
				time = this.properties.CurrentState.magnetArms.magnetStayDelay;
			}
			yield return null;
		}
		AudioManager.Stop("robot_magnet_arms_loop");
		AudioManager.Play("robot_magnet_arms_end");
		base.StartCoroutine(this.magnetArmsExit_cr(false));
		yield break;
	}

	// Token: 0x0600239A RID: 9114 RVA: 0x000C0E78 File Offset: 0x000BF078
	public IEnumerator magnetArmsExit_cr(bool isPermaDeath = false)
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			PlanePlayerController planePlayerController = (PlanePlayerController)abstractPlayerController;
			if (!(planePlayerController == null))
			{
				planePlayerController.motor.RemoveForce(this.force);
			}
		}
		float delay = (1f - this.secondaryAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime % 1f) * this.secondaryAnimator.GetCurrentAnimatorStateInfo(1).length;
		this.secondaryAnimator.Play("Magnet_Arms", -1, 0f);
		yield return CupheadTime.WaitForSeconds(this, delay);
		float t = 0f;
		float time = 1.8f;
		float deltaRotation = 0f;
		Vector3 root = (!isPermaDeath) ? this.magnetEndRoot.transform.position : this.secondary.transform.position;
		while (t < time)
		{
			t += CupheadTime.Delta;
			deltaRotation = Mathf.Lerp(0f, 60f, t / time);
			this.secondary.transform.position = Vector2.Lerp(root, this.magnetStartRoot.transform.position, t / time);
			this.secondary.transform.SetEulerAngles(null, null, new float?(deltaRotation));
			yield return null;
		}
		this.secondaryAttackDelay = this.properties.CurrentState.arms.attackDelayRange.RandomFloat();
		yield return null;
		this.armsActive = false;
		this.armsTypeIndex++;
		if (this.armsTypeIndex >= this.properties.CurrentState.arms.attackString.Split(new char[]
		{
			','
		}).Length)
		{
			this.armsTypeIndex = 0;
		}
		this.parent.animator.Play("Slow", 5, this.parent.animator.GetCurrentAnimatorStateInfo(5).normalizedTime % 1f);
		yield break;
	}

	// Token: 0x0600239B RID: 9115 RVA: 0x000C0E9C File Offset: 0x000BF09C
	public IEnumerator panicArmsLoop_cr()
	{
		for (;;)
		{
			float normalizedTime = this.parent.animator.GetCurrentAnimatorStateInfo(7).normalizedTime;
			normalizedTime %= 1f;
			int currentFrame = (int)(normalizedTime * 24f);
			if (this.panicArmsLoop)
			{
				this.frontArm.transform.position = this.panicArmsPath[currentFrame].position;
				int num = currentFrame + 10;
				if (num >= 24)
				{
					num -= 24;
				}
				this.backArm.transform.position = this.panicArmsPath[num].position;
			}
			currentFrame++;
			if (currentFrame >= 24)
			{
				currentFrame = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600239C RID: 9116 RVA: 0x0001E171 File Offset: 0x0001C371
	public override void OnSecondaryDeath()
	{
		base.StartCoroutine(this.heart_cr());
		base.OnSecondaryDeath();
	}

	// Token: 0x0600239D RID: 9117 RVA: 0x0001E186 File Offset: 0x0001C386
	public void HeartIntroSFX()
	{
		AudioManager.Play("robot_heart_spring_out");
		this.emitAudioFromObject.Add("robot_heart_spring_out");
	}

	// Token: 0x0600239E RID: 9118 RVA: 0x000C0EB8 File Offset: 0x000BF0B8
	public IEnumerator heart_cr()
	{
		base.GetComponent<SpriteRenderer>().enabled = true;
		float waitDuration = this.parent.animator.GetCurrentAnimatorStateInfo(2).length;
		yield return CupheadTime.WaitForSeconds(this, waitDuration);
		base.animator.SetTrigger("OnHeartActive");
		base.GetComponent<Collider2D>().enabled = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", 1, true, true);
		yield break;
	}

	// Token: 0x0600239F RID: 9119 RVA: 0x000C0ED4 File Offset: 0x000BF0D4
	public override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			base.OnDamageTaken(info);
			if (this.damageEffectRoutine != null)
			{
				base.StopCoroutine(this.damageEffectRoutine);
			}
			this.damageEffectRoutine = this.damageEffect_cr();
			base.StartCoroutine(this.damageEffectRoutine);
		}
		else
		{
			float num = this.currentHealth[1];
			this.currentHealth[1] -= info.damage;
			if (this.currentHealth[1] / (float)this.properties.CurrentState.heart.heartHP * 100f <= (float)this.properties.CurrentState.heart.heartDamageChangePercentage)
			{
				if (!this.playedCrackedSound)
				{
					AudioManager.Play("robot_heart_spring_cracked");
					this.emitAudioFromObject.Add("robot_heart_spring_cracked");
					this.playedCrackedSound = true;
				}
				base.animator.Play("Damaged Loop", 1, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f);
			}
			if (this.currentHealth[1] <= 0f && !this.destroyed)
			{
				AudioManager.Stop("robot_magnet_arms_loop");
				AudioManager.Play("robot_heart_spring_destroyed");
				this.emitAudioFromObject.Add("robot_heart_spring_destroyed");
				this.properties.DealDamageToNextNamedState();
				this.destroyed = true;
			}
			if (num > 0f)
			{
				Level.Current.timeline.DealDamage(Mathf.Clamp(num - this.currentHealth[1], 0f, num));
			}
		}
	}

	// Token: 0x060023A0 RID: 9120 RVA: 0x000C1060 File Offset: 0x000BF260
	public override void ExitCurrentAttacks()
	{
		this.StopAllCoroutines();
		bool isPermaDeath = false;
		if (Level.Current.mode == Level.Mode.Easy)
		{
			isPermaDeath = true;
			this.secondary.GetComponent<RobotLevelSecondaryArms>().BossAlive = false;
		}
		if (this.armsActive)
		{
			char c = this.properties.CurrentState.arms.attackString.Split(new char[]
			{
				','
			})[this.armsTypeIndex][0];
			if (c != 'T')
			{
				if (c == 'M')
				{
					base.StartCoroutine(this.magnetArmsExit_cr(isPermaDeath));
				}
			}
			else
			{
				base.StartCoroutine(this.twistyArmsExit_cr(isPermaDeath));
			}
		}
		base.ExitCurrentAttacks();
	}

	// Token: 0x060023A1 RID: 9121 RVA: 0x0001E1A2 File Offset: 0x0001C3A2
	public void InitAnims()
	{
		base.animator.SetTrigger("OnRobotIntro");
	}

	// Token: 0x060023A2 RID: 9122 RVA: 0x000C111C File Offset: 0x000BF31C
	public override void Die()
	{
		if (this.damageEffectRoutine != null)
		{
			base.StopCoroutine(this.damageEffectRoutine);
		}
		this.damageEffect.SetActive(false);
		foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.enabled = false;
		}
		base.Die();
	}

	// Token: 0x060023A3 RID: 9123 RVA: 0x000C1180 File Offset: 0x000BF380
	public IEnumerator damageEffect_cr()
	{
		for (int i = 0; i < 3; i++)
		{
			this.damageEffectRenderer.enabled = true;
			this.damageEffect.SetActive(true);
			yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
			this.damageEffect.SetActive(false);
			yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
		}
		yield break;
	}

	// Token: 0x04001D7B RID: 7547
	public const int PANIC_ARMS_ANIM_FRAME_COUNT = 24;

	// Token: 0x04001D7C RID: 7548
	public int frameCount;

	// Token: 0x04001D7D RID: 7549
	public bool playedCrackedSound;

	// Token: 0x04001D7E RID: 7550
	public bool armsActive;

	// Token: 0x04001D7F RID: 7551
	public bool panicArmsLoop;

	// Token: 0x04001D80 RID: 7552
	public int armsTypeIndex;

	// Token: 0x04001D81 RID: 7553
	public int twistyArmsPositionIndex;

	// Token: 0x04001D82 RID: 7554
	public Vector3 spriteBounds;

	// Token: 0x04001D83 RID: 7555
	public Animator secondaryAnimator;

	// Token: 0x04001D84 RID: 7556
	public PlanePlayerMotor.Force force;

	// Token: 0x04001D85 RID: 7557
	public bool destroyed;

	// Token: 0x04001D86 RID: 7558
	[SerializeField]
	public SpriteRenderer torsoTop;

	// Token: 0x04001D87 RID: 7559
	[SerializeField]
	public GameObject[] damagedPortholes;

	// Token: 0x04001D88 RID: 7560
	[SerializeField]
	public GameObject frontArm;

	// Token: 0x04001D89 RID: 7561
	[SerializeField]
	public GameObject backArm;

	// Token: 0x04001D8A RID: 7562
	[SerializeField]
	public Transform portholeRoot;

	// Token: 0x04001D8B RID: 7563
	[SerializeField]
	public Transform portholeOffsetRoot;

	// Token: 0x04001D8C RID: 7564
	[SerializeField]
	public Transform[] panicArmsPath;

	// Token: 0x04001D8D RID: 7565
	[SerializeField]
	public Transform magnetStartRoot;

	// Token: 0x04001D8E RID: 7566
	[SerializeField]
	public Transform magnetEndRoot;

	// Token: 0x04001D8F RID: 7567
	[SerializeField]
	public GameObject damageEffect;

	// Token: 0x04001D90 RID: 7568
	public IEnumerator damageEffectRoutine;

	// Token: 0x04001D91 RID: 7569
	public SpriteRenderer damageEffectRenderer;

	// Token: 0x02000E75 RID: 3701
	public enum AnimationLayers
	{
		// Token: 0x0400683A RID: 26682
		Main,
		// Token: 0x0400683B RID: 26683
		Pilot,
		// Token: 0x0400683C RID: 26684
		Head,
		// Token: 0x0400683D RID: 26685
		Hose,
		// Token: 0x0400683E RID: 26686
		Waist,
		// Token: 0x0400683F RID: 26687
		BackArm,
		// Token: 0x04006840 RID: 26688
		FrontArm,
		// Token: 0x04006841 RID: 26689
		Torso
	}
}
