using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000036 RID: 54
public class OldManLevel : Level
{
	// Token: 0x06000346 RID: 838 RVA: 0x00065E38 File Offset: 0x00064038
	public override void PartialInit()
	{
		this.properties = LevelProperties.OldMan.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06000347 RID: 839 RVA: 0x00004AF3 File Offset: 0x00002CF3
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.OldMan;
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000348 RID: 840 RVA: 0x00004AFA File Offset: 0x00002CFA
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_old_man;
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000349 RID: 841 RVA: 0x00065ED0 File Offset: 0x000640D0
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.OldMan.States.Main:
				return this._bossPortraitMain;
			case LevelProperties.OldMan.States.SockPuppet:
				return this._bossPortraitPhaseTwo;
			case LevelProperties.OldMan.States.GnomeLeader:
				return this._bossPortraitPhaseThree;
			}
			Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x0600034A RID: 842 RVA: 0x00065F50 File Offset: 0x00064150
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.OldMan.States.Main:
				return this._bossQuoteMain;
			case LevelProperties.OldMan.States.SockPuppet:
				return this._bossQuotePhaseTwo;
			case LevelProperties.OldMan.States.GnomeLeader:
				return this._bossQuotePhaseThree;
			}
			Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x0600034B RID: 843 RVA: 0x00004AFE File Offset: 0x00002CFE
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00065FD0 File Offset: 0x000641D0
	public override void Start()
	{
		base.Start();
		this.firstAttack = true;
		this.platformManager.LevelInit(this.properties);
		this.oldMan.LevelInit(this.properties);
		this.sockPuppet.LevelInit(this.properties);
		this.gnomeLeader.LevelInit(this.properties);
		this.climberPosString = new PatternString(this.properties.CurrentState.climberGnomes.gnomePositionStrings, true, true);
		for (int i = 0; i < this.spikes.Length; i++)
		{
			this.spikes[i].SetProperties(this.properties);
			this.spikes[i].SetID(i);
		}
		this.gnomeLeader.gameObject.SetActive(false);
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_stomachacid_amb_loop", 0f, 0f);
	}

	// Token: 0x0600034D RID: 845 RVA: 0x000660B0 File Offset: 0x000642B0
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.OldMan.States.SockPuppet)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.phase_2_trans_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.OldMan.States.GnomeLeader)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.phase_3_trans_cr());
		}
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00004B21 File Offset: 0x00002D21
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.oldmanPattern_cr());
		base.StartCoroutine(this.gnome_turrets_cr());
		base.StartCoroutine(this.climbers_cr());
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00004B4A File Offset: 0x00002D4A
	public override void OnPreWin()
	{
		if (Level.Current.mode == Level.Mode.Easy)
		{
			this.sockPuppet.OnPhase3();
		}
	}

	// Token: 0x06000350 RID: 848 RVA: 0x0006611C File Offset: 0x0006431C
	public void CreateFX(Vector3 pos, bool isSparkle, bool isPink)
	{
		Effect effect = null;
		List<Effect> list = (!isSparkle) ? this.smokeFXPool : this.sparkleFXPool;
		for (int i = 0; i < list.Count; i++)
		{
			if (!list[i].inUse)
			{
				effect = list[i];
				break;
			}
		}
		if (effect == null)
		{
			effect = ((!isSparkle) ? this.smokePrefab.Create(pos) : this.sparklePrefab.Create(pos));
			list.Add(effect);
		}
		effect.Initialize(pos);
		effect.animator.Play((!isPink) ? this.EffectReset : this.EffectResetPink);
		effect.inUse = true;
	}

	// Token: 0x06000351 RID: 849 RVA: 0x000661E0 File Offset: 0x000643E0
	public void ClearFX(List<Effect> pool)
	{
		for (int i = 0; i < pool.Count; i++)
		{
			if (pool[i].inUse)
			{
				pool[i].removeOnEnd = true;
			}
			else
			{
				Object.Destroy(pool[i].gameObject);
			}
		}
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00066238 File Offset: 0x00064438
	public IEnumerator oldmanPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00066254 File Offset: 0x00064454
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.OldMan.Pattern p = this.properties.CurrentState.NextPattern;
		while (p == LevelProperties.OldMan.Pattern.Camel && this.firstAttack)
		{
			p = this.properties.CurrentState.NextPattern;
		}
		this.firstAttack = false;
		switch (p)
		{
		case LevelProperties.OldMan.Pattern.Spit:
			yield return base.StartCoroutine(this.spit_cr());
			break;
		case LevelProperties.OldMan.Pattern.Duck:
			yield return base.StartCoroutine(this.duck_cr());
			break;
		case LevelProperties.OldMan.Pattern.Camel:
			yield return base.StartCoroutine(this.camel_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		}
		yield break;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00066270 File Offset: 0x00064470
	public IEnumerator spit_cr()
	{
		while (this.oldMan.state != OldManLevelOldMan.State.Idle)
		{
			yield return null;
		}
		this.oldMan.Spit();
		while (this.oldMan.state != OldManLevelOldMan.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000355 RID: 853 RVA: 0x0006628C File Offset: 0x0006448C
	public IEnumerator duck_cr()
	{
		while (this.oldMan.state != OldManLevelOldMan.State.Idle)
		{
			yield return null;
		}
		this.oldMan.Goose();
		while (this.oldMan.state != OldManLevelOldMan.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x000662A8 File Offset: 0x000644A8
	public IEnumerator camel_cr()
	{
		while (this.oldMan.state != OldManLevelOldMan.State.Idle)
		{
			yield return null;
		}
		this.oldMan.Bear();
		while (this.oldMan.state != OldManLevelOldMan.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000357 RID: 855 RVA: 0x000662C4 File Offset: 0x000644C4
	public IEnumerator gnome_turrets_cr()
	{
		LevelProperties.OldMan.Turret p = this.properties.CurrentState.turret;
		this.gnomesSpawned = new List<OldManLevelSpikeFloor>();
		PatternString appearString = new PatternString(p.appearOrder, true, true);
		for (;;)
		{
			while (!p.gnomesOn)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, p.appearDelayRange.RandomFloat());
			this.gnomesSpawned.RemoveAll((OldManLevelSpikeFloor g) => g.spikeState != OldManLevelSpikeFloor.SpikeState.Gnomed);
			if (this.gnomesSpawned.Count < p.maxCount)
			{
				int appearOrder = 0;
				do
				{
					appearOrder = appearString.PopInt();
					yield return null;
				}
				while (this.spikes[appearOrder].spikeState != OldManLevelSpikeFloor.SpikeState.Idle);
				this.spikes[appearOrder].SpawnGnome();
				this.gnomesSpawned.Add(this.spikes[appearOrder]);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000358 RID: 856 RVA: 0x000662E0 File Offset: 0x000644E0
	public IEnumerator climbers_cr()
	{
		LevelProperties.OldMan.ClimberGnomes p = this.properties.CurrentState.climberGnomes;
		for (;;)
		{
			yield return null;
			int pos = this.climberPosString.PopInt();
			int platform = 4 - pos / 2;
			if (!this.platformManager.PlatformRemoved(platform))
			{
				OldManLevelGnomeClimber oldManLevelGnomeClimber = this.gnomeClimberPrefab.Spawn<OldManLevelGnomeClimber>();
				float facing = (float)((pos % 2 != 0) ? 1 : -1);
				Transform platform2 = this.platformManager.GetPlatform(platform);
				oldManLevelGnomeClimber.Init(this.climberXPosition[pos], facing, platform2, p);
				this.platformManager.AttachGnome(platform, oldManLevelGnomeClimber);
			}
			yield return CupheadTime.WaitForSeconds(this, p.spawnDelayRange.RandomFloat());
		}
		yield break;
	}

	// Token: 0x06000359 RID: 857 RVA: 0x000662FC File Offset: 0x000644FC
	public void ActivatePhase2Beard()
	{
		foreach (GameObject gameObject in this.hairObjects)
		{
			gameObject.SetActive(true);
		}
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00066330 File Offset: 0x00064530
	public IEnumerator phase_2_trans_cr()
	{
		this.oldMan.EndPhase1();
		this.ClearFX(this.sparkleFXPool);
		this.ClearFX(this.smokeFXPool);
		yield return this.oldMan.animator.WaitForAnimationToStart(this, "Phase_Trans", false);
		this.oldMan.StopAllCoroutines();
		yield return null;
		foreach (OldManLevelSpikeFloor oldManLevelSpikeFloor in this.spikes)
		{
			oldManLevelSpikeFloor.Exit();
		}
		this.platformManager.EndPhase();
		while (this.oldMan.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.684210539f)
		{
			yield return null;
		}
		Level.Current.SetBounds(new int?(1249), new int?(331), null, null);
		CupheadLevelCamera.Current.ChangeHorizontalBounds(1002, 85);
		Vector3 cameraEndPos = new Vector3(-460f, 0f, 0f);
		base.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraEndPos, 3f));
		base.StartCoroutine(this.move_clouds_cr(3f));
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.oldMan.OnPhase2();
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.bleachers.SetActive(true);
		yield return null;
		yield break;
	}

	// Token: 0x0600035B RID: 859 RVA: 0x0006634C File Offset: 0x0006454C
	public IEnumerator move_clouds_cr(float time)
	{
		float leftStartPos = this.cloudLeft.transform.localPosition.x;
		float rightStartPos = this.cloudRight.transform.localPosition.x;
		float t = 0f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.cloudLeft.transform.localPosition = new Vector3(EaseUtils.EaseInOutSine(leftStartPos, -720f, t / time), this.cloudLeft.transform.localPosition.y);
			this.cloudRight.transform.localPosition = new Vector3(EaseUtils.EaseInOutSine(rightStartPos, 420f, t / time), this.cloudRight.transform.localPosition.y);
			yield return null;
		}
		this.cloudLeft.transform.localPosition = new Vector3(-720f, this.cloudLeft.transform.localPosition.y);
		this.cloudRight.transform.localPosition = new Vector3(420f, this.cloudRight.transform.localPosition.y);
		yield break;
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00004B66 File Offset: 0x00002D66
	public bool InPhase2()
	{
		return this.properties.CurrentState.stateName == LevelProperties.OldMan.States.SockPuppet;
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00066370 File Offset: 0x00064570
	public IEnumerator phase_3_trans_cr()
	{
		Object.Destroy(this.platformManager.gameObject);
		this.sockPuppet.OnPhase3();
		AbstractPlayerController p = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		LevelPlayerWeaponManager weaponManagerP = p.GetComponent<LevelPlayerWeaponManager>();
		LevelPlayerMotor motorP = p.GetComponent<LevelPlayerMotor>();
		weaponManagerP.InterruptSuper();
		LevelPlayerWeaponManager weaponManagerP2 = null;
		LevelPlayerMotor motorP2 = null;
		AbstractPlayerController p2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (p2 != null)
		{
			motorP2 = p2.GetComponent<LevelPlayerMotor>();
			weaponManagerP2 = p2.GetComponent<LevelPlayerWeaponManager>();
			weaponManagerP2.InterruptSuper();
		}
		yield return new WaitForEndOfFrame();
		while (this.sockPuppet.transState != OldManLevelSockPuppetHandler.TransitionState.PlatformDestroyed)
		{
			yield return null;
		}
		this.mainPlatform.SetActive(false);
		this.phaseTransitionTrigger.gameObject.SetActive(true);
		this.mainPit.SetActive(false);
		if (motorP)
		{
			motorP.OnTrampolineKnockUp(-2.3f);
		}
		if (motorP2)
		{
			motorP2.OnTrampolineKnockUp(-2.3f);
		}
		bool readyToGo = false;
		while (!readyToGo)
		{
			if ((p.IsDead || this.phaseTransitionTrigger.bounds.Contains(p.transform.position + Vector3.down * 10f)) && (PlayerManager.GetPlayer(PlayerId.PlayerTwo) == null || PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead || this.phaseTransitionTrigger.bounds.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position + Vector3.down * 10f)))
			{
				readyToGo = true;
				this.sockPuppet.SwallowedPlayers();
			}
			if (p.IsDead || this.phaseTransitionTrigger.bounds.Contains(p.transform.position + Vector3.down * 10f))
			{
				p.gameObject.SetActive(false);
			}
			if ((PlayerManager.GetPlayer(PlayerId.PlayerTwo) == null || PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead || this.phaseTransitionTrigger.bounds.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position + Vector3.down * 10f)) && PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
			{
				PlayerManager.GetPlayer(PlayerId.PlayerTwo).gameObject.SetActive(false);
			}
			yield return null;
		}
		PlayerDeathEffect[] ghosts = Object.FindObjectsOfType(typeof(PlayerDeathEffect)) as PlayerDeathEffect[];
		foreach (PlayerDeathEffect playerDeathEffect in ghosts)
		{
			playerDeathEffect.transform.position += Vector3.up * 5000f;
		}
		PlayerSuperGhost[] superGhosts = Object.FindObjectsOfType(typeof(PlayerSuperGhost)) as PlayerSuperGhost[];
		foreach (PlayerSuperGhost playerSuperGhost in superGhosts)
		{
			Object.Destroy(playerSuperGhost.gameObject);
		}
		PlayerSuperGhostHeart[] superGhostHearts = Object.FindObjectsOfType(typeof(PlayerSuperGhostHeart)) as PlayerSuperGhostHeart[];
		foreach (PlayerSuperGhostHeart playerSuperGhostHeart in superGhostHearts)
		{
			Object.Destroy(playerSuperGhostHeart);
		}
		while (this.sockPuppet.transState != OldManLevelSockPuppetHandler.TransitionState.InStomach)
		{
			yield return null;
		}
		yield return base.StartCoroutine(this.iris_cr());
		if (!p.IsDead)
		{
			motorP.EnableInput();
		}
		p2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (p2 != null && !p2.IsDead)
		{
			motorP2.EnableInput();
		}
		base.StartCoroutine(this.scuba_gnomes_cr());
		yield return null;
		yield break;
	}

	// Token: 0x0600035E RID: 862 RVA: 0x0006638C File Offset: 0x0006458C
	public IEnumerator iris_cr()
	{
		LevelPauseGUI pauseGUI = GameObject.Find("Level_UI").GetComponentInChildren<LevelPauseGUI>();
		pauseGUI.ForceDisablePause(true);
		Animator faderAni = this.fader.GetComponent<Animator>();
		Color c = this.fader.color;
		c.a = 1f;
		this.fader.color = c;
		faderAni.SetTrigger("Iris_In");
		yield return faderAni.WaitForAnimationToEnd(this, "Iris_In", false, true);
		yield return new WaitForSeconds(0.9f);
		this.SetupStomach();
		faderAni.SetTrigger("Iris_Out");
		this.gnomeLeader.StartGnomeLeader();
		yield return faderAni.WaitForAnimationToEnd(this, "Iris_Out", false, true);
		pauseGUI.ForceDisablePause(false);
		c = this.fader.color;
		c.a = 0f;
		this.fader.color = c;
		yield break;
	}

	// Token: 0x0600035F RID: 863 RVA: 0x000663A8 File Offset: 0x000645A8
	public void SetupStomach()
	{
		Level.Current.SetBounds(new int?(1249), new int?(331), null, null);
		this.gnomeLeader.gameObject.SetActive(true);
		this.phaseTransitionTrigger.gameObject.SetActive(false);
		this.mountainBG.SetActive(false);
		this.stomachBG.SetActive(true);
		this.sockPuppet.FinishPuppet();
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		if (!player.IsDead)
		{
			player.gameObject.SetActive(true);
			LevelPlayerMotor component = player.GetComponent<LevelPlayerMotor>();
			component.ClearBufferedInput();
			component.ForceLooking(new Trilean2(1, 1));
			player.GetComponent<LevelPlayerAnimationController>().ResetMoveX();
			component.OnRevive(this.gnomeLeader.platformPositions[1].position + Vector3.up * 1000f);
			component.CancelReviveBounce();
			component.EnableInput();
		}
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 != null && !player2.IsDead)
		{
			player2.gameObject.SetActive(true);
			LevelPlayerMotor component2 = player2.GetComponent<LevelPlayerMotor>();
			component2 = player2.GetComponent<LevelPlayerMotor>();
			component2.ClearBufferedInput();
			component2.ForceLooking(new Trilean2(1, 1));
			player2.GetComponent<LevelPlayerAnimationController>().ResetMoveX();
			component2.OnRevive(this.gnomeLeader.platformPositions[3].position + Vector3.up * 1000f);
			component2.CancelReviveBounce();
			component2.EnableInput();
		}
		this.SFX_StomachLoop();
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00066540 File Offset: 0x00064740
	public IEnumerator scuba_gnomes_cr()
	{
		bool onLeft = Rand.Bool();
		LevelProperties.OldMan.ScubaGnomes p = this.properties.CurrentState.scubaGnomes;
		PatternString scubaTypeString = new PatternString(p.scubaTypeString, true, true);
		PatternString spawnDelayString = new PatternString(p.spawnDelayString, true, true);
		PatternString dartParryableString = new PatternString(p.dartParryableString, true);
		float offset = 50f;
		for (;;)
		{
			float xPos = (!onLeft) ? (CupheadLevelCamera.Current.Bounds.xMax - offset) : (CupheadLevelCamera.Current.Bounds.xMin + offset);
			OldManLevelScubaGnome scubaGnome = this.scubaGnomePrefab.Spawn<OldManLevelScubaGnome>();
			scubaGnome.Init(new Vector3(xPos, CupheadLevelCamera.Current.Bounds.yMin), PlayerManager.GetNext(), scubaTypeString.PopLetter() == 'A', onLeft, dartParryableString.PopLetter() == 'P', p, this.gnomeLeader);
			yield return CupheadTime.WaitForSeconds(this, spawnDelayString.PopFloat());
			onLeft = !onLeft;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000361 RID: 865 RVA: 0x0006655C File Offset: 0x0006475C
	public void SFX_StomachLoop()
	{
		base.transform.position = this.stomachBG.transform.position;
		AudioManager.PlayLoop("sfx_dlc_omm_p3_stomachacid_amb_loop");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_stomachacid_amb_loop");
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_stomachacid_amb_loop", 1f, 1f);
	}

	// Token: 0x06000362 RID: 866 RVA: 0x000665B4 File Offset: 0x000647B4
	public void WORKAROUND_NullifyFields()
	{
		this.platformManager = null;
		this.fader = null;
		this.hairObjects = null;
		this.scubaGnomePrefab = null;
		this.mainPlatform = null;
		this.oldMan = null;
		this.sockPuppet = null;
		this.gnomeLeader = null;
		this.gnomeClimberPrefab = null;
		this.spikes = null;
		this.mountainBG = null;
		this.cloudLeft = null;
		this.cloudRight = null;
		this.stomachBG = null;
		this.phaseTransitionTrigger = null;
		this.mainPit = null;
		this.bleachers = null;
		this.gnomesSpawned = null;
		this.climberPosString = null;
		this.climberXPosition = null;
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
		this._bossQuoteMain = null;
		this._bossQuotePhaseTwo = null;
		this._bossQuotePhaseThree = null;
	}

	// Token: 0x04000242 RID: 578
	public LevelProperties.OldMan properties;

	// Token: 0x04000243 RID: 579
	public int EffectReset = Animator.StringToHash("Reset");

	// Token: 0x04000244 RID: 580
	public int EffectResetPink = Animator.StringToHash("ResetPink");

	// Token: 0x04000245 RID: 581
	public const float CAM_END_POS_X = -460f;

	// Token: 0x04000246 RID: 582
	public const float CAM_MOVE_TIME = 3f;

	// Token: 0x04000247 RID: 583
	public const int CAM_PHASE2_BOUNDS_LEFT = 1002;

	// Token: 0x04000248 RID: 584
	public const int CAM_PHASE2_BOUNDS_RIGHT = 85;

	// Token: 0x04000249 RID: 585
	public const int PHASE2_BOUNDS_LEFT = 1249;

	// Token: 0x0400024A RID: 586
	public const int PHASE2_BOUNDS_RIGHT = 331;

	// Token: 0x0400024B RID: 587
	public const float IRIS_TIME = 0.9f;

	// Token: 0x0400024C RID: 588
	[SerializeField]
	public Image fader;

	// Token: 0x0400024D RID: 589
	[SerializeField]
	public GameObject[] hairObjects;

	// Token: 0x0400024E RID: 590
	[SerializeField]
	public OldManLevelScubaGnome scubaGnomePrefab;

	// Token: 0x0400024F RID: 591
	[SerializeField]
	public GameObject mainPlatform;

	// Token: 0x04000250 RID: 592
	public OldManLevelPlatformManager platformManager;

	// Token: 0x04000251 RID: 593
	[SerializeField]
	public OldManLevelOldMan oldMan;

	// Token: 0x04000252 RID: 594
	[SerializeField]
	public OldManLevelSockPuppetHandler sockPuppet;

	// Token: 0x04000253 RID: 595
	[SerializeField]
	public OldManLevelGnomeLeader gnomeLeader;

	// Token: 0x04000254 RID: 596
	[SerializeField]
	public OldManLevelGnomeClimber gnomeClimberPrefab;

	// Token: 0x04000255 RID: 597
	[SerializeField]
	public OldManLevelSpikeFloor[] spikes;

	// Token: 0x04000256 RID: 598
	[SerializeField]
	public GameObject mountainBG;

	// Token: 0x04000257 RID: 599
	[SerializeField]
	public GameObject cloudLeft;

	// Token: 0x04000258 RID: 600
	[SerializeField]
	public GameObject cloudRight;

	// Token: 0x04000259 RID: 601
	[SerializeField]
	public GameObject stomachBG;

	// Token: 0x0400025A RID: 602
	[SerializeField]
	public Collider2D phaseTransitionTrigger;

	// Token: 0x0400025B RID: 603
	[SerializeField]
	public GameObject mainPit;

	// Token: 0x0400025C RID: 604
	[SerializeField]
	public GameObject bleachers;

	// Token: 0x0400025D RID: 605
	public List<OldManLevelSpikeFloor> gnomesSpawned;

	// Token: 0x0400025E RID: 606
	public PatternString climberPosString;

	// Token: 0x0400025F RID: 607
	public bool playedFirstSpikeSound;

	// Token: 0x04000260 RID: 608
	public bool firstAttack;

	// Token: 0x04000261 RID: 609
	public float[] climberXPosition = new float[]
	{
		-1006f,
		-766f,
		-792f,
		-562.2f,
		-590.8f,
		-357f,
		-377f,
		-147.7f,
		-163.2f,
		63.2f
	};

	// Token: 0x04000262 RID: 610
	public List<Effect> smokeFXPool = new List<Effect>();

	// Token: 0x04000263 RID: 611
	[SerializeField]
	public Effect smokePrefab;

	// Token: 0x04000264 RID: 612
	public List<Effect> sparkleFXPool = new List<Effect>();

	// Token: 0x04000265 RID: 613
	[SerializeField]
	public Effect sparklePrefab;

	// Token: 0x04000266 RID: 614
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x04000267 RID: 615
	[SerializeField]
	public Sprite _bossPortraitPhaseTwo;

	// Token: 0x04000268 RID: 616
	[SerializeField]
	public Sprite _bossPortraitPhaseThree;

	// Token: 0x04000269 RID: 617
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x0400026A RID: 618
	[SerializeField]
	public string _bossQuotePhaseTwo;

	// Token: 0x0400026B RID: 619
	[SerializeField]
	public string _bossQuotePhaseThree;
}
