using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200037E RID: 894
public class SaltbakerLevelSaltbaker : LevelProperties.Saltbaker.Entity
{
	// Token: 0x1400004C RID: 76
	// (add) Token: 0x0600275A RID: 10074 RVA: 0x000CB83C File Offset: 0x000C9A3C
	// (remove) Token: 0x0600275B RID: 10075 RVA: 0x000CB874 File Offset: 0x000C9A74
	public event Action OnDeathEvent;

	// Token: 0x0600275C RID: 10076 RVA: 0x000CB8AC File Offset: 0x000C9AAC
	public void Start()
	{
		this.scale = base.transform.localScale.x;
		this.startPos = base.transform.position;
		this.damageReceiver = this.phaseOneCollider.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.doughFXAnimator.transform.parent = null;
		base.animator.SetBool("IntroCubes", Rand.Bool());
	}

	// Token: 0x0600275D RID: 10077 RVA: 0x000CB934 File Offset: 0x000C9B34
	public override void LevelInit(LevelProperties.Saltbaker properties)
	{
		base.LevelInit(properties);
		this.strawberriesSpawnString = new PatternString(properties.CurrentState.strawberries.locationSpawnString, true, true);
		this.strawberriesDelayString = new PatternString(properties.CurrentState.strawberries.bulletDelayString, true, true);
		this.sugarcubesPhaseString = new PatternString(properties.CurrentState.sugarcubes.phaseString, true, true);
		this.sugarcubesDelayString = new PatternString(properties.CurrentState.sugarcubes.bulletDelayString, true, true);
		this.sugarcubesParryString = new PatternString(properties.CurrentState.sugarcubes.parryString, true);
		this.doughSpawnSidePatternString = new PatternString(properties.CurrentState.dough.doughSpawnSideString, true, true);
		this.doughSpawnDelayString = new PatternString(properties.CurrentState.dough.doughDelayString, true, true);
		this.doughSpawnTypeString = new PatternString(properties.CurrentState.dough.doughSpawnTypeString, true, true);
		this.limeHeightString = new PatternString(properties.CurrentState.limes.boomerangHeightString, true, true);
		this.limesDelayString = new PatternString(properties.CurrentState.limes.boomerangDelayString, true, true);
		this.timeToNextAttack[0] = properties.CurrentState.strawberries.startNextAtk;
		this.timeToNextAttack[1] = properties.CurrentState.sugarcubes.startNextAttack;
		this.timeToNextAttack[2] = properties.CurrentState.dough.startNextAttack;
		this.timeToNextAttack[3] = properties.CurrentState.limes.startNextAttack;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x0600275E RID: 10078 RVA: 0x000CBAD8 File Offset: 0x000C9CD8
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		((SaltbakerLevel)Level.Current).SpawnSwoopers();
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.currentAttack = ((!base.animator.GetBool("IntroCubes")) ? SaltbakerLevelSaltbaker.State.Limes : SaltbakerLevelSaltbaker.State.Sugarcubes);
		this.prevAttack = this.currentAttack;
		this.AniEvent_StartProjectiles();
		this.attackCoroutines.Add(base.StartCoroutine(this.pattern_cr()));
		yield break;
	}

	// Token: 0x0600275F RID: 10079 RVA: 0x0002112E File Offset: 0x0001F32E
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002760 RID: 10080 RVA: 0x000CBAF4 File Offset: 0x000C9CF4
	public IEnumerator pattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		do
		{
			this.currentAttack = (SaltbakerLevelSaltbaker.State)(base.properties.CurrentState.NextPattern + 1);
		}
		while (this.currentAttack == SaltbakerLevelSaltbaker.State.Strawberries || this.currentAttack == this.prevAttack);
		while (!this.phaseOneEnded)
		{
			SaltbakerLevelSaltbaker.State state = this.currentAttack;
			if (state != SaltbakerLevelSaltbaker.State.Sugarcubes)
			{
				if (state != SaltbakerLevelSaltbaker.State.Dough)
				{
					if (state == SaltbakerLevelSaltbaker.State.Limes)
					{
						base.animator.Play("Limes");
					}
				}
				else
				{
					base.animator.Play("Dough");
				}
			}
			else
			{
				base.animator.Play("Sugarcubes");
				this.sugarTextReversed.enabled = (base.transform.localScale.x == -1f);
			}
			this.prevAttack = this.currentAttack;
			while (this.currentAttack != SaltbakerLevelSaltbaker.State.Idle)
			{
				yield return null;
			}
			if (!this.phaseOneEnded)
			{
				this.currentAttack = (SaltbakerLevelSaltbaker.State)(base.properties.CurrentState.NextPattern + 1);
				base.animator.SetBool("NextStrawberries", this.currentAttack == SaltbakerLevelSaltbaker.State.Strawberries);
				float timeToIdle = this.timeToNextAttack[this.prevAttack - SaltbakerLevelSaltbaker.State.Strawberries] - this.postAttackTime[this.prevAttack - SaltbakerLevelSaltbaker.State.Strawberries] - this.preAttackTime[this.currentAttack - SaltbakerLevelSaltbaker.State.Strawberries];
				yield return CupheadTime.WaitForSeconds(this, timeToIdle);
			}
			yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		}
		yield break;
	}

	// Token: 0x06002761 RID: 10081 RVA: 0x000CBB10 File Offset: 0x000C9D10
	public void AniEvent_StartProjectiles()
	{
		if (this.phaseOneEnded)
		{
			return;
		}
		switch (this.currentAttack)
		{
		case SaltbakerLevelSaltbaker.State.Strawberries:
			this.attackCoroutines.Add(base.StartCoroutine(this.strawberries_cr()));
			break;
		case SaltbakerLevelSaltbaker.State.Sugarcubes:
			this.attackCoroutines.Add(base.StartCoroutine(this.sugarcubes_cr()));
			break;
		case SaltbakerLevelSaltbaker.State.Dough:
			this.attackCoroutines.Add(base.StartCoroutine(this.dough_cr()));
			break;
		case SaltbakerLevelSaltbaker.State.Limes:
			this.attackCoroutines.Add(base.StartCoroutine(this.limes_cr()));
			break;
		}
		if (this.currentAttack == SaltbakerLevelSaltbaker.State.Strawberries)
		{
			this.prevAttack = SaltbakerLevelSaltbaker.State.Strawberries;
			if (!this.phaseOneEnded)
			{
				this.currentAttack = (SaltbakerLevelSaltbaker.State)(base.properties.CurrentState.NextPattern + 1);
			}
		}
		else
		{
			this.currentAttack = SaltbakerLevelSaltbaker.State.Idle;
		}
	}

	// Token: 0x06002762 RID: 10082 RVA: 0x000CBC00 File Offset: 0x000C9E00
	public void AniEvent_FinishMove()
	{
		base.transform.localScale = new Vector3(-base.transform.localScale.x, base.transform.localScale.y);
		this.onLeft = !this.onLeft;
	}

	// Token: 0x06002763 RID: 10083 RVA: 0x000CBC54 File Offset: 0x000C9E54
	public IEnumerator strawberries_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		LevelProperties.Saltbaker.Strawberries p = base.properties.CurrentState.strawberries;
		float delay = p.firstDelay;
		float attackTime = 0f;
		float delayTime = 0f;
		int anim = Random.Range(0, 4);
		while (attackTime <= p.diagAtkDuration)
		{
			attackTime += CupheadTime.FixedDelta;
			delayTime += CupheadTime.FixedDelta;
			if (delayTime > delay)
			{
				delayTime -= delay;
				delay = this.strawberriesDelayString.PopFloat();
				this.destroyOnPhaseEnd.Add(this.strawberryPrefab.Create(new Vector3(this.strawberriesSpawnString.PopFloat(), (float)Level.Current.Ceiling + 100f), p.diagAngle + 90f, p.bulletSpeed, anim).gameObject);
				anim = (anim + 1) % 4;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002764 RID: 10084 RVA: 0x00021141 File Offset: 0x0001F341
	public void AniEvent_StrawberryBasketStart()
	{
		this.strawberryBasket.StartRunIn(this.onLeft);
	}

	// Token: 0x06002765 RID: 10085 RVA: 0x00021154 File Offset: 0x0001F354
	public void AniEvent_StrawberryBasketGrab()
	{
		this.strawberryBasket.GetGrabbed();
	}

	// Token: 0x06002766 RID: 10086 RVA: 0x00021161 File Offset: 0x0001F361
	public void AniEvent_StrawberryBasketExit()
	{
		this.strawberryBasket.StartRunOut();
	}

	// Token: 0x06002767 RID: 10087 RVA: 0x000CBC70 File Offset: 0x000C9E70
	public IEnumerator sugarcubes_cr()
	{
		LevelProperties.Saltbaker.Sugarcubes p = base.properties.CurrentState.sugarcubes;
		bool side = this.onLeft;
		float delay = p.firstDelay;
		float phase = this.sugarcubesPhaseString.PopFloat();
		float delayTime = 0f;
		float attackTime = 0f;
		int anim = Random.Range(0, 3);
		YieldInstruction wait = new WaitForFixedUpdate();
		while (attackTime <= p.sineAttackDuration)
		{
			attackTime += CupheadTime.FixedDelta;
			delayTime += CupheadTime.FixedDelta;
			if (delayTime > delay)
			{
				delayTime -= delay;
				delay = this.sugarcubesDelayString.PopFloat();
				phase = this.sugarcubesPhaseString.PopFloat();
				SaltbakerLevelSugarcube saltbakerLevelSugarcube = this.sugarcubePrefab.Spawn<SaltbakerLevelSugarcube>();
				saltbakerLevelSugarcube.Init(new Vector3((float)((!side) ? (Level.Current.Right + 100) : (Level.Current.Left - 100)), p.centerHeight), side, p, phase, this, anim, this.sugarcubesParryString.PopLetter() == 'P');
				this.destroyOnPhaseEnd.Add(saltbakerLevelSugarcube.gameObject);
				anim = (anim + 1) % 3;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002768 RID: 10088 RVA: 0x000CBC8C File Offset: 0x000C9E8C
	public IEnumerator dough_cr()
	{
		LevelProperties.Saltbaker.Dough p = base.properties.CurrentState.dough;
		bool side = this.onLeft;
		float attackTime = 0f;
		float delayTime = 0f;
		float delay = p.firstDelay;
		Vector3 left = new Vector3((float)Level.Current.Left - 100f, -300f);
		Vector3 right = new Vector3((float)Level.Current.Right + 100f, -300f);
		YieldInstruction wait = new WaitForFixedUpdate();
		int count = 0;
		int startAnimalType = Random.Range(0, 3);
		while (attackTime <= p.doughAttackDuration)
		{
			attackTime += CupheadTime.FixedDelta;
			delayTime += CupheadTime.FixedDelta;
			if (delayTime > delay)
			{
				delayTime -= delay;
				delay = this.doughSpawnDelayString.PopFloat();
				int num = this.doughSpawnTypeString.PopInt();
				char c = this.doughSpawnSidePatternString.PopLetter();
				bool flag = (c != 'P') ? (!side) : side;
				SaltbakerLevelDough saltbakerLevelDough = this.doughPrefab.Spawn<SaltbakerLevelDough>();
				saltbakerLevelDough.Init((!flag) ? right : left, (!flag) ? (-p.doughXSpeed[num]) : p.doughXSpeed[num], p.doughYSpeed[num], p.doughGravity[num], p.doughHealth, count, (startAnimalType + count) % 3);
				this.destroyOnPhaseEnd.Add(saltbakerLevelDough.gameObject);
				count++;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002769 RID: 10089 RVA: 0x0002116E File Offset: 0x0001F36E
	public void AniEvent_FinishDough()
	{
		this.doughFXAnimator.transform.localScale = base.transform.localScale;
		this.doughFXAnimator.Play("DoughFX");
	}

	// Token: 0x0600276A RID: 10090 RVA: 0x000CBCA8 File Offset: 0x000C9EA8
	public IEnumerator limes_cr()
	{
		LevelProperties.Saltbaker.Limes p = base.properties.CurrentState.limes;
		bool side = this.onLeft;
		float attackTime = 0f;
		float delayTime = 0f;
		float delay = p.firstDelay;
		int sfxID = 0;
		int anim = Random.Range(0, 4);
		YieldInstruction wait = new WaitForFixedUpdate();
		while (attackTime <= p.boomerangAttackDuration)
		{
			attackTime += CupheadTime.FixedDelta;
			delayTime += CupheadTime.FixedDelta;
			if (delayTime > delay)
			{
				delayTime -= delay;
				delay = this.limesDelayString.PopFloat();
				SaltbakerLevelLime saltbakerLevelLime = this.limePrefab.Spawn<SaltbakerLevelLime>();
				saltbakerLevelLime.Init(new Vector3((float)((!side) ? Level.Current.Right : Level.Current.Left), 0f), side, this.limeHeightString.PopLetter() == 'H', base.properties.CurrentState.limes, sfxID, anim);
				this.destroyOnPhaseEnd.Add(saltbakerLevelLime.gameObject);
				sfxID = (sfxID + 1) % 3;
				anim = (anim + 1) % 4;
			}
			yield return wait;
		}
		yield return wait;
		yield break;
	}

	// Token: 0x0600276B RID: 10091 RVA: 0x000CBCC4 File Offset: 0x000C9EC4
	public IEnumerator phase_one_to_two_cr()
	{
		this.phaseOneEnded = true;
		base.animator.SetTrigger("EndPhaseOne");
		yield return base.animator.WaitForAnimationToStart(this, "PhaseOneToTwo", false);
		this.phaseTwoHPPrediction = (float)((int)(base.properties.CurrentHealth - base.properties.GetNextStateHealthTrigger() * base.properties.TotalHealth));
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.weaponManager.EnableSuper(false);
			}
		}
		yield return base.animator.WaitForAnimationToEnd(this, "PhaseTwoIntro", false, true);
		this.phaseTwoStarted = true;
		this.Phase2SwitchOnPatterns();
		yield break;
	}

	// Token: 0x0600276C RID: 10092 RVA: 0x000CBCE0 File Offset: 0x000C9EE0
	public void AniEvent_HitTable()
	{
		this.phaseOneCollider.enabled = false;
		CupheadLevelCamera.Current.Shake(55f, 0.5f, false);
		base.transform.localScale = new Vector3(1f, 1f);
		AudioManager.StartBGMAlternate(0);
	}

	// Token: 0x0600276D RID: 10093 RVA: 0x0002119B File Offset: 0x0001F39B
	public void AniEvent_KillFires()
	{
		((SaltbakerLevel)Level.Current).KillFires();
	}

	// Token: 0x0600276E RID: 10094 RVA: 0x000CBD30 File Offset: 0x000C9F30
	public void AniEvent_HandsClosed()
	{
		this.ClearPhaseOneObjects();
		((SaltbakerLevel)Level.Current).ClearFires();
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.weaponManager.InterruptSuper();
			}
		}
		foreach (AbstractPlayerController abstractPlayerController2 in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController2 = (LevelPlayerController)abstractPlayerController2;
			if (levelPlayerController2 != null)
			{
				levelPlayerController2.DisableInput();
				levelPlayerController2.motor.ClearBufferedInput();
				Level.Current.SetBounds(new int?(10780), new int?(-9220), new int?(446), new int?(370));
				levelPlayerController2.transform.position = this.playerDefrostPositions[(int)levelPlayerController2.id].position + Vector3.left * 10000f;
			}
		}
		this.transitionCamera.SetActive(true);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		LevelPauseGUI.OnUnpauseEvent += this.SuppressPlayerJoin;
		if (((SaltbakerLevel)Level.Current).playerLost)
		{
			base.animator.speed = 0f;
		}
		else
		{
			base.StartCoroutine(this.scroll_bg_cr());
		}
	}

	// Token: 0x0600276F RID: 10095 RVA: 0x000211AC File Offset: 0x0001F3AC
	public void AniEvent_ShakeScreen()
	{
		CupheadLevelCamera.Current.Shake(55f, 0.5f, false);
	}

	// Token: 0x06002770 RID: 10096 RVA: 0x000211C3 File Offset: 0x0001F3C3
	public void AniEvent_FadeInReflection()
	{
		base.StartCoroutine(this.fade_in_reflection_cr());
	}

	// Token: 0x06002771 RID: 10097 RVA: 0x000CBED8 File Offset: 0x000CA0D8
	public void ClearPhaseOneObjects()
	{
		this.attackCoroutines.RemoveAll((Coroutine c) => c == null);
		foreach (Coroutine coroutine in this.attackCoroutines)
		{
			base.StopCoroutine(coroutine);
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("PlayerProjectile");
		for (int m = 0; m < array.Length; m++)
		{
			Object.Destroy(array[m]);
		}
		Effect[] array2 = (Effect[])Object.FindObjectsOfType(typeof(Effect));
		for (int j = 0; j < array2.Length; j++)
		{
			Object.Destroy(array2[j].gameObject);
		}
		this.destroyOnPhaseEnd.RemoveAll((GameObject i) => i == null);
		for (int k = 0; k < this.destroyOnPhaseEnd.Count; k++)
		{
			Object.Destroy(this.destroyOnPhaseEnd[k]);
		}
		Object.Destroy(this.strawberryBasket.gameObject);
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.weaponManager.AbortEX();
			}
		}
		foreach (PlayerSuperChaliceShieldHeart playerSuperChaliceShieldHeart in Object.FindObjectsOfType<PlayerSuperChaliceShieldHeart>())
		{
			playerSuperChaliceShieldHeart.transform.parent = playerSuperChaliceShieldHeart.player.transform;
		}
	}

	// Token: 0x06002772 RID: 10098 RVA: 0x000CC0D0 File Offset: 0x000CA2D0
	public IEnumerator scroll_bg_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.transitionDelayAfterHandsClose);
		SaltbakerLevel level = Level.Current as SaltbakerLevel;
		float t = 0f;
		YieldInstruction wait = new WaitForFixedUpdate();
		CupheadLevelCamera.Current.Shake(8f, this.transitionDuration, false);
		Vector3 shadowOffset = this.shadow.transform.position - this.table.transform.position;
		while (t < this.transitionDuration)
		{
			level.yScrollPos = EaseUtils.EaseInOut(EaseUtils.EaseType.easeInSine, EaseUtils.EaseType.easeOutBack, 0f, 1f, Mathf.InverseLerp(0f, this.transitionDuration, t));
			this.shadow.transform.position = shadowOffset + this.table.transform.position + Vector3.up * level.yScrollPos * 1500f;
			t += CupheadTime.FixedDelta;
			yield return wait;
		}
		level.yScrollPos = 1f;
		yield break;
	}

	// Token: 0x06002773 RID: 10099 RVA: 0x000CC0EC File Offset: 0x000CA2EC
	public IEnumerator fade_in_reflection_cr()
	{
		this.reflectionCamera.SetActive(true);
		yield return null;
		this.reflectionTexture.SetActive(true);
		float c = 0f;
		while (c < 0.5f)
		{
			c = Mathf.Clamp(c + CupheadTime.Delta * 5f, 0f, 0.5f);
			this.reflectionMaterial.color = new Color(1f, 1f, 1f, c);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002774 RID: 10100 RVA: 0x000211D2 File Offset: 0x0001F3D2
	public void SuppressPlayerJoin()
	{
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
	}

	// Token: 0x06002775 RID: 10101 RVA: 0x000CC108 File Offset: 0x000CA308
	public void AniEvent_HandsOpen()
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.EnableInput();
				levelPlayerController.weaponManager.DisableInput();
				levelPlayerController.transform.position = this.playerDefrostPositions[(int)levelPlayerController.id].position + Vector3.left * 10000f;
				levelPlayerController.motor.DoPostSuperHop();
			}
		}
	}

	// Token: 0x06002776 RID: 10102 RVA: 0x000211DC File Offset: 0x0001F3DC
	public void AniEvent_SFX_MagicDough()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_magiccookie");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_magiccookie");
	}

	// Token: 0x06002777 RID: 10103 RVA: 0x000211F8 File Offset: 0x0001F3F8
	public void AniEvent_SpawnJumpers()
	{
		((SaltbakerLevel)Level.Current).SpawnJumpers();
	}

	// Token: 0x06002778 RID: 10104 RVA: 0x00021209 File Offset: 0x0001F409
	public void AniEvent_ShakeScreenSaltFall()
	{
		CupheadLevelCamera.Current.Shake(20f, 2f, false);
	}

	// Token: 0x06002779 RID: 10105 RVA: 0x000CC1BC File Offset: 0x000CA3BC
	public void AniEvent_RestorePlayers()
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.weaponManager.EnableSuper(true);
				levelPlayerController.weaponManager.EnableInput();
				Level.Current.SetBounds(new int?(780), new int?(780), new int?(446), new int?(370));
				levelPlayerController.transform.position += Vector3.right * 10000f;
			}
		}
		foreach (PlayerSuperChaliceShieldHeart playerSuperChaliceShieldHeart in Object.FindObjectsOfType<PlayerSuperChaliceShieldHeart>())
		{
			playerSuperChaliceShieldHeart.transform.parent = null;
		}
		this.transitionCamera.SetActive(false);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		LevelPauseGUI.OnUnpauseEvent -= this.SuppressPlayerJoin;
	}

	// Token: 0x0600277A RID: 10106 RVA: 0x00021220 File Offset: 0x0001F420
	public void AniEvent_StartHandIdle()
	{
		this.phaseOneRenderer.enabled = false;
		this.handAnimator.Play("Idle");
		this.handAnimator.Update(0f);
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x0600277B RID: 10107 RVA: 0x0002125A File Offset: 0x0001F45A
	public void Phase2SwitchOnPatterns()
	{
		if (base.properties.CurrentState.leaf.leavesOn)
		{
			base.StartCoroutine(this.leaf_fall_cr());
		}
	}

	// Token: 0x0600277C RID: 10108 RVA: 0x00021283 File Offset: 0x0001F483
	public bool PreDamagePhaseTwoAndReturnWhetherDoomed(float damage)
	{
		this.phaseTwoHPPrediction -= damage;
		if (this.phaseTwoHPPrediction < 0f)
		{
			AudioManager.StopBGM();
			AudioManager.StartBGMAlternate(1);
		}
		return this.phaseTwoHPPrediction < 0f;
	}

	// Token: 0x0600277D RID: 10109 RVA: 0x000CC2E4 File Offset: 0x000CA4E4
	public void DamageSaltbaker(float damage, int turretIndex)
	{
		base.properties.DealDamage(damage);
		if (base.properties.CurrentState.stateName != LevelProperties.Saltbaker.States.PhaseThree)
		{
			base.animator.Play(this.turretHitAnimName[turretIndex]);
			base.animator.Update(0f);
			this.handAnimator.Play("Hit", 0, 0f);
			this.mintHandAnimator.Play((turretIndex != 3) ? "HitA" : "HitB", 1, 0f);
			CupheadLevelCamera.Current.Shake(30f, 0.5f, false);
		}
	}

	// Token: 0x0600277E RID: 10110 RVA: 0x000CC388 File Offset: 0x000CA588
	public void AniEvent_SpawnPepperShaker()
	{
		this.turrets[this.turretIndex] = Object.Instantiate<SaltbakerLevelFeistTurret>(this.feistTurretPrefab);
		this.turrets[this.turretIndex].transform.position = this.turretRoots[this.turretIndex].position;
		this.turrets[this.turretIndex].transform.localScale = new Vector3(Mathf.Sign(-this.turrets[this.turretIndex].transform.position.x), 1f);
		this.turrets[this.turretIndex].Setup(base.properties.CurrentState.turrets, this, this.turretIndex);
		this.turretIndex++;
		if (this.turretIndex == 4)
		{
			base.StartCoroutine(this.turret_cr());
		}
	}

	// Token: 0x0600277F RID: 10111 RVA: 0x000CC46C File Offset: 0x000CA66C
	public IEnumerator turret_cr()
	{
		LevelProperties.Saltbaker.Turrets p = base.properties.CurrentState.turrets;
		this.turretIndex = Random.Range(0, 4);
		this.turretFiringDir = Rand.PosOrNeg();
		PatternString bulletTypeString = new PatternString(p.bulletTypeString, true, true);
		yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
		for (;;)
		{
			if (this.turrets != null && this.turrets[this.turretFiringOrder[this.turretIndex]].IsActivated)
			{
				bool isPink = bulletTypeString.PopLetter() == 'P';
				this.turrets[this.turretFiringOrder[this.turretIndex]].Shoot(isPink, p.warningTime);
				yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
			}
			this.turretIndex = (this.turretIndex + this.turretFiringDir + 4) % this.turrets.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002780 RID: 10112 RVA: 0x000CC488 File Offset: 0x000CA688
	public IEnumerator leaf_fall_cr()
	{
		LevelProperties.Saltbaker.Leaf p = base.properties.CurrentState.leaf;
		PatternString leavesCountString = new PatternString(p.leavesCountString, true, true);
		bool animA = Rand.Bool();
		float posX = 0f;
		float posY = (float)Level.Current.Ceiling + 20f;
		for (;;)
		{
			animA = !animA;
			base.animator.SetTrigger((!animA) ? "MintB" : "MintA");
			yield return base.animator.WaitForAnimationToStart(this, (!animA) ? "PhaseTwoMintB" : "PhaseTwoMintA", false);
			this.mintHandAnimator.Play((!animA) ? "MintB" : "MintA");
			yield return this.mintHandAnimator.WaitForAnimationToEnd(this, (!animA) ? "MintB" : "MintA", false, true);
			yield return CupheadTime.WaitForSeconds(this, p.reenterDelay);
			int leavesCount = leavesCountString.PopInt();
			float offset = (float)(Level.Current.Width / leavesCount);
			float extraOffset = p.leavesOffset.RandomFloat();
			List<int> animIDs = new List<int>
			{
				0,
				1,
				2,
				3
			};
			for (int i = 0; i < animIDs.Count; i++)
			{
				int index = Random.Range(0, animIDs.Count);
				int value = animIDs[i];
				animIDs[i] = animIDs[index];
				animIDs[index] = value;
			}
			for (int j = 0; j < leavesCount; j++)
			{
				posX = offset * ((float)j - (float)(leavesCount - 1) / 2f) - p.xDistance / 2f;
				Vector3 pos;
				pos..ctor(posX + extraOffset, posY);
				SaltBakerLevelLeaf saltBakerLevelLeaf = this.leafFallPrefab.Spawn<SaltBakerLevelLeaf>();
				saltBakerLevelLeaf.Init(pos, p.xTime, p.xDistance, p.yConstantSpeed, p.ySpeed, this, animIDs[j % 4]);
			}
			for (int k = 0; k < Random.Range(4, 8); k++)
			{
				SaltbakerLevelBGMint saltbakerLevelBGMint = Object.Instantiate<SaltbakerLevelBGMint>(this.bgMintPrefab, new Vector3((float)Random.Range(Level.Current.Left, 0), (float)(Level.Current.Ceiling + Random.Range(250, 500))), Quaternion.identity, null);
				saltbakerLevelBGMint.StartAnimation(k % 4);
			}
			AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_leafdescend");
			yield return CupheadTime.WaitForSeconds(this, p.leavesDelay - 1.75f);
		}
		yield break;
	}

	// Token: 0x06002781 RID: 10113 RVA: 0x000212BB File Offset: 0x0001F4BB
	public void OnPhaseThree()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.phase_two_to_three_cr());
	}

	// Token: 0x06002782 RID: 10114 RVA: 0x000CC4A4 File Offset: 0x000CA6A4
	public IEnumerator phase_two_to_three_cr()
	{
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		base.animator.Play("PhaseTwoDeath");
		this.handAnimator.Play("Death");
		this.mintHandAnimator.Play("None");
		yield return base.animator.WaitForAnimationToStart(this, "PhaseTwoDeath", false);
		this.transitionFader.gameObject.SetActive(true);
		this.turretIndex = 0;
		while (this.turretIndex < 4)
		{
			this.turrets[this.turretIndex].Die();
			this.turretIndex++;
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.975f)
		{
			this.transitionFader.color = new Color(1f, 1f, 1f, Mathf.InverseLerp(0.8f, 0.975f, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
			CupheadLevelCamera.Current.Shake(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * this.endPhaseTwoShakeAmount, this.startPhaseThreeShakeHoldover, false);
			yield return null;
		}
		CupheadLevelCamera.Current.Shake(this.endPhaseTwoShakeAmount, this.startPhaseThreeShakeHoldover, false);
		foreach (SaltbakerLevelFeistTurret saltbakerLevelFeistTurret in this.turrets)
		{
			Object.Destroy(saltbakerLevelFeistTurret.gameObject);
		}
		this.transitionFader.color = Color.white;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		((SaltbakerLevel)Level.Current).StartPhase3();
		this.BG.SetActive(false);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x000212D0 File Offset: 0x0001F4D0
	public override void OnDestroy()
	{
		Object.Destroy(this.reflectionCamera);
		Object.Destroy(this.reflectionTexture);
		base.OnDestroy();
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x000212EE File Offset: 0x0001F4EE
	public void AnimationEvent_SFX_SALTBAKER_P1_DoughAttack_RollAndKnead()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_doughattack_rollandknead");
	}

	// Token: 0x06002785 RID: 10117 RVA: 0x000212FA File Offset: 0x0001F4FA
	public void AnimationEvent_SFX_SALTBAKER_P1_DoughAttack_RollingPinAppear()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_doughattack_rollingpinappear");
	}

	// Token: 0x06002786 RID: 10118 RVA: 0x00021306 File Offset: 0x0001F506
	public void AnimationEvent_SFX_SALTBAKER_P1_Intro_BowTiePull()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_intro_bowtiepull");
	}

	// Token: 0x06002787 RID: 10119 RVA: 0x00021312 File Offset: 0x0001F512
	public void AnimationEvent_SFX_SALTBAKER_P1_Intro_HandSwipeLimesSugar()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_intro_handswipe_limessugar");
	}

	// Token: 0x06002788 RID: 10120 RVA: 0x0002131E File Offset: 0x0001F51E
	public void AnimationEvent_SFX_SALTBAKER_P1_Limes_Knife_ChopCut()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_limes_knife_chopcut");
	}

	// Token: 0x06002789 RID: 10121 RVA: 0x0002132A File Offset: 0x0001F52A
	public void AnimationEvent_SFX_SALTBAKER_P1_Limes_Knife_Scrape()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_limes_knife_scrape");
	}

	// Token: 0x0600278A RID: 10122 RVA: 0x00021336 File Offset: 0x0001F536
	public void AnimationEvent_SFX_SALTBAKER_P1_Limes_Knife_SliceSwing()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_limes_knife_sliceswing");
	}

	// Token: 0x0600278B RID: 10123 RVA: 0x00021342 File Offset: 0x0001F542
	public void AnimationEvent_SFX_SALTBAKER_P1_StrawberrySqueeze_Attack()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_strawberrysqueeze_attack");
	}

	// Token: 0x0600278C RID: 10124 RVA: 0x0002134E File Offset: 0x0001F54E
	public void AnimationEvent_SFX_SALTBAKER_P1_SugarCube_Blow()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_sugarcube_blow");
	}

	// Token: 0x0600278D RID: 10125 RVA: 0x0002135A File Offset: 0x0001F55A
	public void AnimationEvent_SFX_SALTBAKER_P1_SugarCube_KnockAndBreak()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_sugarcube_knockandbreak");
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x00021366 File Offset: 0x0001F566
	public void AnimationEvent_SFX_SALTBAKER_P1_SugarCube_PlaceOnTable()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_sugarcube_placeontable");
	}

	// Token: 0x0600278F RID: 10127 RVA: 0x00021372 File Offset: 0x0001F572
	public void AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_A_TableSlam()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_a_tableslam");
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x0002137E File Offset: 0x0001F57E
	public void AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_B_HatRemove()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_b_hatremove");
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x0002138A File Offset: 0x0001F58A
	public void AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_C_ShroomInsert()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_c_shroominsert");
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x00021396 File Offset: 0x0001F596
	public void AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_D_BakerPowerup()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_d_bakerpowerup");
	}

	// Token: 0x06002793 RID: 10131 RVA: 0x000213A2 File Offset: 0x0001F5A2
	public void AnimationEvent_SFX_SALTBAKER_P1_to_P2_Transition_E_GrabandRise()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_to_p2_transition_e_grabandrise");
	}

	// Token: 0x06002794 RID: 10132 RVA: 0x000213AE File Offset: 0x0001F5AE
	public void AnimationEvent_SFX_SALTBAKER_P2_MintLeafAttack_LaunchThrow()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_launchthrow");
	}

	// Token: 0x06002795 RID: 10133 RVA: 0x000213BA File Offset: 0x0001F5BA
	public void AnimationEvent_SFX_SALTBAKER_P2_MintLeafAttack_LeafRustle()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_leafrustle");
	}

	// Token: 0x06002796 RID: 10134 RVA: 0x000213C6 File Offset: 0x0001F5C6
	public void AnimationEvent_SFX_SALTBAKER_P2_Intro_Fingersnap()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_intro_fingersnap");
	}

	// Token: 0x06002797 RID: 10135 RVA: 0x000213D2 File Offset: 0x0001F5D2
	public void AnimationEvent_SFX_SALTBAKER_P2_Intro_Fingersnap_Laugh()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_intro_fingersnap_laugh");
	}

	// Token: 0x06002798 RID: 10136 RVA: 0x000213DE File Offset: 0x0001F5DE
	public void AnimationEvent_SFX_SALTBAKER_P2_VocalPain()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_vocal_pain");
	}

	// Token: 0x06002799 RID: 10137 RVA: 0x000213EA File Offset: 0x0001F5EA
	public void AnimationEvent_SFX_SALTBAKER_P2_Death()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_death");
	}

	// Token: 0x0600279A RID: 10138 RVA: 0x000213F6 File Offset: 0x0001F5F6
	public void SFXLeafRustle()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_leafrustle");
	}

	// Token: 0x0600279B RID: 10139 RVA: 0x00021402 File Offset: 0x0001F602
	public void SFXLaunchThrow()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p2_mintleafattack_launchthrow");
	}

	// Token: 0x04002098 RID: 8344
	public const float MOVE_POS_X = 370f;

	// Token: 0x04002099 RID: 8345
	public const float MINT_ANIMATION_LENGTH = 1.75f;

	// Token: 0x0400209A RID: 8346
	public const float PHASE_TWO_REFLECTION_OPACITY = 0.5f;

	// Token: 0x0400209B RID: 8347
	public SaltbakerLevelSaltbaker.State currentAttack;

	// Token: 0x0400209C RID: 8348
	public SaltbakerLevelSaltbaker.State prevAttack;

	// Token: 0x0400209D RID: 8349
	public float[] preAttackTime = new float[]
	{
		1.75f,
		4.54166651f,
		5.20833349f,
		4.20833349f
	};

	// Token: 0x0400209E RID: 8350
	public float[] postAttackTime = new float[]
	{
		1.125f,
		1.58333337f,
		0.333333343f,
		1.91666663f
	};

	// Token: 0x040020A0 RID: 8352
	[SerializeField]
	public SpriteRenderer sugarTextReversed;

	// Token: 0x040020A1 RID: 8353
	[SerializeField]
	public Transform[] playerDefrostPositions;

	// Token: 0x040020A2 RID: 8354
	[SerializeField]
	public GameObject shadow;

	// Token: 0x040020A3 RID: 8355
	[SerializeField]
	public GameObject table;

	// Token: 0x040020A4 RID: 8356
	[Header("Prefabs")]
	[SerializeField]
	public SaltbakerLevelStrawberry strawberryPrefab;

	// Token: 0x040020A5 RID: 8357
	[SerializeField]
	public SaltbakerLevelSugarcube sugarcubePrefab;

	// Token: 0x040020A6 RID: 8358
	[SerializeField]
	public SaltbakerLevelDough doughPrefab;

	// Token: 0x040020A7 RID: 8359
	[SerializeField]
	public SaltbakerLevelLime limePrefab;

	// Token: 0x040020A8 RID: 8360
	[SerializeField]
	public SaltbakerLevelStrawberryBasket strawberryBasket;

	// Token: 0x040020A9 RID: 8361
	[SerializeField]
	public SaltbakerLevelFeistTurret feistTurretPrefab;

	// Token: 0x040020AA RID: 8362
	public SaltbakerLevelFeistTurret[] turrets = new SaltbakerLevelFeistTurret[4];

	// Token: 0x040020AB RID: 8363
	public int turretIndex;

	// Token: 0x040020AC RID: 8364
	public int[] turretFiringOrder = new int[]
	{
		2,
		1,
		3,
		0
	};

	// Token: 0x040020AD RID: 8365
	public int turretFiringDir;

	// Token: 0x040020AE RID: 8366
	public string[] turretHitAnimName = new string[]
	{
		"PhaseTwoHitB",
		"PhaseTwoHitA",
		"PhaseTwoHitD",
		"PhaseTwoHitC"
	};

	// Token: 0x040020AF RID: 8367
	[SerializeField]
	public SaltBakerLevelLeaf leafFallPrefab;

	// Token: 0x040020B0 RID: 8368
	[SerializeField]
	public SaltbakerLevelBGMint bgMintPrefab;

	// Token: 0x040020B1 RID: 8369
	[SerializeField]
	public Transform[] turretRoots;

	// Token: 0x040020B2 RID: 8370
	[SerializeField]
	public Animator handAnimator;

	// Token: 0x040020B3 RID: 8371
	[SerializeField]
	public GameObject transitionCamera;

	// Token: 0x040020B4 RID: 8372
	[SerializeField]
	public float transitionDelayAfterHandsClose;

	// Token: 0x040020B5 RID: 8373
	[SerializeField]
	public float transitionDuration = 2.5f;

	// Token: 0x040020B6 RID: 8374
	[SerializeField]
	public SpriteRenderer transitionFader;

	// Token: 0x040020B7 RID: 8375
	[SerializeField]
	public float endPhaseTwoShakeAmount = 75f;

	// Token: 0x040020B8 RID: 8376
	[SerializeField]
	public float startPhaseThreeShakeHoldover = 4f;

	// Token: 0x040020B9 RID: 8377
	public DamageReceiver damageReceiver;

	// Token: 0x040020BA RID: 8378
	public Vector3 startPos;

	// Token: 0x040020BB RID: 8379
	public bool onLeft;

	// Token: 0x040020BC RID: 8380
	public float scale;

	// Token: 0x040020BD RID: 8381
	public bool phaseOneEnded;

	// Token: 0x040020BE RID: 8382
	public bool phaseTwoStarted;

	// Token: 0x040020BF RID: 8383
	public bool preventAdditionalTurretLaunch;

	// Token: 0x040020C0 RID: 8384
	public float phaseTwoHPPrediction;

	// Token: 0x040020C1 RID: 8385
	public PatternString strawberriesSpawnString;

	// Token: 0x040020C2 RID: 8386
	public PatternString strawberriesDelayString;

	// Token: 0x040020C3 RID: 8387
	public PatternString sugarcubesPhaseString;

	// Token: 0x040020C4 RID: 8388
	public PatternString sugarcubesDelayString;

	// Token: 0x040020C5 RID: 8389
	public PatternString sugarcubesParryString;

	// Token: 0x040020C6 RID: 8390
	public PatternString doughSpawnSidePatternString;

	// Token: 0x040020C7 RID: 8391
	public PatternString doughSpawnTypeString;

	// Token: 0x040020C8 RID: 8392
	public PatternString doughSpawnDelayString;

	// Token: 0x040020C9 RID: 8393
	[SerializeField]
	public Animator doughFXAnimator;

	// Token: 0x040020CA RID: 8394
	public PatternString limeHeightString;

	// Token: 0x040020CB RID: 8395
	public PatternString limesDelayString;

	// Token: 0x040020CC RID: 8396
	[SerializeField]
	public GameObject BG;

	// Token: 0x040020CD RID: 8397
	[SerializeField]
	public Collider2D phaseOneCollider;

	// Token: 0x040020CE RID: 8398
	[SerializeField]
	public SpriteRenderer phaseOneRenderer;

	// Token: 0x040020CF RID: 8399
	[SerializeField]
	public float[] timeToNextAttack = new float[4];

	// Token: 0x040020D0 RID: 8400
	[SerializeField]
	public Animator mintHandAnimator;

	// Token: 0x040020D1 RID: 8401
	public List<GameObject> destroyOnPhaseEnd = new List<GameObject>();

	// Token: 0x040020D2 RID: 8402
	public List<Coroutine> attackCoroutines = new List<Coroutine>();

	// Token: 0x040020D3 RID: 8403
	[SerializeField]
	public GameObject reflectionCamera;

	// Token: 0x040020D4 RID: 8404
	[SerializeField]
	public Material reflectionMaterial;

	// Token: 0x040020D5 RID: 8405
	[SerializeField]
	public GameObject reflectionTexture;

	// Token: 0x02000F43 RID: 3907
	public enum State
	{
		// Token: 0x04006E09 RID: 28169
		Idle,
		// Token: 0x04006E0A RID: 28170
		Strawberries,
		// Token: 0x04006E0B RID: 28171
		Sugarcubes,
		// Token: 0x04006E0C RID: 28172
		Dough,
		// Token: 0x04006E0D RID: 28173
		Limes
	}
}
