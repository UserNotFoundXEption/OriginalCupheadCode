using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class ChessCastleLevel : Level
{
	// Token: 0x060000D2 RID: 210 RVA: 0x0005F848 File Offset: 0x0005DA48
	public override void PartialInit()
	{
		this.properties = LevelProperties.ChessCastle.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060000D3 RID: 211 RVA: 0x0000391B File Offset: 0x00001B1B
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessCastle;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003922 File Offset: 0x00001B22
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_castle;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x060000D5 RID: 213 RVA: 0x00003926 File Offset: 0x00001B26
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x060000D6 RID: 214 RVA: 0x0000392E File Offset: 0x00001B2E
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x060000D7 RID: 215 RVA: 0x00003936 File Offset: 0x00001B36
	// (set) Token: 0x060000D8 RID: 216 RVA: 0x0000393E File Offset: 0x00001B3E
	public bool rotating { get; set; }

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x060000D9 RID: 217 RVA: 0x00003947 File Offset: 0x00001B47
	public float rotationMultiplier
	{
		get
		{
			return this._rotationMultiplier;
		}
	}

	// Token: 0x060000DA RID: 218 RVA: 0x0005F8E0 File Offset: 0x0005DAE0
	public override void OnEnable()
	{
		base.OnEnable();
		SceneLoader.OnFadeOutStartEvent += this.onFadeOutStartEventHandler;
		base.OnIntroEvent += this.onIntroEventHandler;
		Dialoguer.events.onStarted += this.onDialogueStartedHandler;
		Dialoguer.events.onMessageEvent += this.onDialogueMessageHandler;
		Dialoguer.events.onEnded += this.onDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.onDialogueEndedHandler;
		Dialoguer.events.onTextPhase += this.onDialogueAdvancedHandler;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x0005F984 File Offset: 0x0005DB84
	public override void OnDisable()
	{
		base.OnDisable();
		SceneLoader.OnFadeOutStartEvent -= this.onFadeOutStartEventHandler;
		base.OnIntroEvent -= this.onIntroEventHandler;
		Dialoguer.events.onStarted -= this.onDialogueStartedHandler;
		Dialoguer.events.onMessageEvent -= this.onDialogueMessageHandler;
		Dialoguer.events.onEnded -= this.onDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.onDialogueEndedHandler;
		Dialoguer.events.onTextPhase -= this.onDialogueAdvancedHandler;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x0005FA28 File Offset: 0x0005DC28
	public override void Awake()
	{
		this.previousLevel = Level.PreviousLevel;
		this.previouslyWon = Level.Won;
		if (this.previouslyWon)
		{
			this.attemptsToBeat = PlayerData.Data.chessBossAttemptCounter;
			PlayerData.Data.ResetKingOfGamesCounter();
			PlayerData.SaveCurrentFile();
		}
		base.Awake();
	}

	// Token: 0x060000DD RID: 221 RVA: 0x0005FA7C File Offset: 0x0005DC7C
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.startEntity = (this.exitEntity = null);
		this.playerStartLevelEffects = null;
		this.dialogueInteractionPoint = null;
		this.speechBubble = null;
		this.coinPrefab = null;
		this.kingAnimator = null;
		this.castleAnimator = null;
		this.platformAnimator = null;
		this.cloudPrefab = null;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x0005FAD8 File Offset: 0x0005DCD8
	public override void Start()
	{
		base.Start();
		this.speechBubbleBasePosition = this.speechBubble.basePosition;
		this.updateDialogueState();
		bool flag = PlayerData.Data.CountLevelsCompleted(Level.kingOfGamesLevels) == Level.kingOfGamesLevels.Length;
		base.StartCoroutine(this.cloudSpawn_cr());
		AudioManager.PlayLoop("sfx_dlc_kog_castle_amb_wind_loop");
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_castle_amb_wind_loop", 0.4f, 1f);
		Levels levels;
		if (this.previouslyWon && SceneLoader.CurrentContext is GauntletContext && ((GauntletContext)SceneLoader.CurrentContext).complete)
		{
			levels = Levels.ChessQueen;
			this.movePlayersToDialoguePositions();
			this.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGamesVictory_WDLC;
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, -3f);
		}
		else if (this.previouslyWon && (!flag || Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) != (float)ChessCastleLevel.KingOfGamesFinalDialogueState))
		{
			this.movePlayersToDialoguePositions();
			this.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGamesVictory_WDLC;
			levels = this.calculatePreviousLevel();
			if (flag)
			{
				levels = Levels.ChessQueen;
				Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, -2f);
			}
			else if (this.attemptsToBeat < ChessCastleLevel.MaxAttemptsToContinue)
			{
				int num = (int)Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerCountIndex);
				num++;
				Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerCountIndex, (float)num);
				Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, 0f);
			}
			else
			{
				Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, -1f);
			}
		}
		else
		{
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, -2f);
			if (flag)
			{
				if (Array.Exists<Levels>(Level.kingOfGamesLevels, (Levels level) => level == this.previousLevel))
				{
					levels = this.previousLevel;
					this.movePlayersToDialoguePositions();
				}
				else
				{
					levels = Levels.ChessPawn;
				}
			}
			else
			{
				levels = this.calculateCurrentLevel();
			}
		}
		if (Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) == 0f)
		{
			this.firstEntry = true;
		}
		if (!this.previouslyWon)
		{
			this.introCameraMovementCoroutine = base.StartCoroutine(this.panCamera_cr());
		}
		if (this.firstEntry || this.previouslyWon)
		{
			string text = ChessCastleLevel.LevelPrefixes[Array.IndexOf<Levels>(Level.kingOfGamesLevels, levels)];
			this.castleAnimator.Play(text + "Idle", ChessCastleLevel.CastleBaseLayer);
			this.castleAnimator.Play(text + "Open", ChessCastleLevel.CastleDoorLayer, 1f);
			if (levels == Levels.ChessBishop || levels == Levels.ChessQueen)
			{
				this.castleAnimator.Play(text, ChessCastleLevel.CastleFlairLayer, 1f);
			}
			this.platformAnimator.Play("Stop", ChessCastleLevel.PlatformBaseLayer, 1f);
			this.setTargetLevel(levels, false);
		}
		else
		{
			this.rotate(Levels.ChessPawn, levels, false, true);
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x0005FDA8 File Offset: 0x0005DFA8
	public override void Update()
	{
		base.Update();
		if (this.introCameraMovementCoroutine == null)
		{
			this.cameraSineAccumulator += CupheadTime.Delta;
			Vector3 manualFloat;
			manualFloat..ctor(0f, this.sineAmplitude * Mathf.Sin(this.cameraSineAccumulator / this.sinePeriod * 2f * 3.14159274f + 1.57079637f));
			CupheadLevelCamera.Current.SetManualFloat(manualFloat);
		}
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x0000394F File Offset: 0x00001B4F
	public override void OnLevelStart()
	{
		if (this.firstEntry)
		{
			base.StartCoroutine(this.firstEntry_cr());
		}
		else if (this.dialogueInteractionPoint.dialogueInteraction == DialoguerDialogues.KingOfGamesVictory_WDLC)
		{
			base.StartCoroutine(this.postWinEntry_cr());
		}
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x0005FE20 File Offset: 0x0005E020
	public override void OnTransitionInComplete()
	{
		bool flag = PlayerData.Data.CountLevelsCompleted(Level.kingOfGamesLevels) == Level.kingOfGamesLevels.Length;
		base.OnTransitionInComplete();
		if (flag)
		{
			AudioManager.StartBGMAlternate(1);
		}
		else if (Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) == 0f)
		{
			AudioManager.PlayBGM();
		}
		else
		{
			AudioManager.StartBGMAlternate(0);
		}
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0005FE84 File Offset: 0x0005E084
	public IEnumerator panCamera_cr()
	{
		CupheadLevelCamera.Current.SetManualFloat(new Vector3(0f, -this.introPanAmount));
		while (!this.beginIntroPan)
		{
			yield return null;
		}
		float elapsedTime = 0f;
		while (elapsedTime < this.introPanDuration)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			CupheadLevelCamera.Current.SetManualFloat(new Vector3(0f, EaseUtils.EaseOutCubic(-this.introPanAmount, this.sineAmplitude, elapsedTime / this.introPanDuration)));
		}
		this.introCameraMovementCoroutine = null;
		yield break;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x0005FEA0 File Offset: 0x0005E0A0
	public IEnumerator firstEntry_cr()
	{
		this.castleAnimator.Play(ChessCastleLevel.LevelPrefixes[0] + "Close", ChessCastleLevel.CastleDoorLayer, 1f);
		this.startEntity.enabled = false;
		AudioSource castleIntroMusic = GameObject.Find("MUS_CastleIntro").GetComponent<AudioSource>();
		while (castleIntroMusic.isPlaying)
		{
			yield return null;
		}
		AudioManager.StartBGMAlternate(0);
		yield break;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x0005FEBC File Offset: 0x0005E0BC
	public IEnumerator postWinEntry_cr()
	{
		AudioManager.StopBGM();
		Behaviour behaviour = this.startEntity;
		bool flag = false;
		this.dialogueInteractionPoint.enabled = flag;
		flag = flag;
		this.exitEntity.enabled = flag;
		behaviour.enabled = flag;
		yield return CupheadTime.WaitForSeconds(this, 0.35f);
		if (Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) != 0f)
		{
			AudioManager.StartBGMAlternate(0);
		}
		this.dialogueInteractionPoint.BeginDialogue();
		yield return CupheadTime.WaitForSeconds(this, 2f);
		Behaviour behaviour2 = this.startEntity;
		flag = true;
		this.dialogueInteractionPoint.enabled = flag;
		flag = flag;
		this.exitEntity.enabled = flag;
		behaviour2.enabled = flag;
		yield break;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x0000398D File Offset: 0x00001B8D
	public void rotate(Levels startLevel, Levels endLevel, bool gauntlet, bool intro)
	{
		if (this.rotationCoroutine != null)
		{
			return;
		}
		this.rotationCoroutine = base.StartCoroutine(this.rotate_cr(startLevel, endLevel, gauntlet, intro));
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x0005FED8 File Offset: 0x0005E0D8
	public IEnumerator rotate_cr(Levels startLevel, Levels endLevel, bool gauntlet, bool intro)
	{
		if (this.gauntletSparklesCoroutine != null)
		{
			base.StopCoroutine(this.gauntletSparklesCoroutine);
			this.gauntletSparklesCoroutine = null;
		}
		int startIndex = Array.IndexOf<Levels>(Level.kingOfGamesLevels, startLevel);
		int destinationIndex = Array.IndexOf<Levels>(Level.kingOfGamesLevels, endLevel);
		string startPrefix = ChessCastleLevel.LevelPrefixes[startIndex];
		string endPrefix = ChessCastleLevel.LevelPrefixes[destinationIndex];
		Behaviour behaviour = this.startEntity;
		bool flag = false;
		this.dialogueInteractionPoint.enabled = flag;
		flag = flag;
		this.exitEntity.enabled = flag;
		behaviour.enabled = flag;
		if (intro)
		{
			float num = (float)destinationIndex / (float)Level.kingOfGamesLevels.Length - 0.25f;
			this.castleAnimator.Play("FullRotation", ChessCastleLevel.CastleBaseLayer, num);
			this.castleAnimator.Play("Off", ChessCastleLevel.CastleDoorLayer);
			this.kingAnimator.Play("LeverPull", 0, 0.2f);
		}
		else
		{
			this.castleAnimator.Play(startPrefix + "Close", ChessCastleLevel.CastleDoorLayer);
			AudioManager.Play(ChessCastleLevel.DoorSounds[startIndex] + "_close");
			this.kingAnimator.SetTrigger("PullLever");
			this.kingAnimator.SetBool("Talking", false);
			yield return this.kingAnimator.WaitForNormalizedTime(this, 0.6101695f, "LeverPull", 0, false, false, true);
			yield return this.castleAnimator.WaitForNormalizedTimeLooping(this, 0.9166667f, startPrefix + "Idle", ChessCastleLevel.CastleBaseLayer, true, false, true);
			this.castleAnimator.Play(startPrefix + "Start");
		}
		this.castleAnimator.SetInteger("Destination", destinationIndex);
		this.castleAnimator.SetBool("Gauntlet", gauntlet);
		this.castleAnimator.Play("Off", ChessCastleLevel.CastleFlairLayer);
		this.platformAnimator.Play("Start", ChessCastleLevel.PlatformBaseLayer);
		this.rotating = true;
		CupheadLevelCamera.Current.StartShake(2f);
		if (destinationIndex - startIndex != 1)
		{
			AudioManager.PlayLoop("sfx_dlc_kog_castle_kog_rotate_loop");
			AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_castle_kog_rotate_loop", 0f, 0.2f, 0.2f);
		}
		else
		{
			AudioManager.Play("sfx_dlc_kog_castle_kog_rotate");
		}
		yield return this.castleAnimator.WaitForAnimationToStart(this, endPrefix + "Stop", ChessCastleLevel.CastleBaseLayer, false);
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_castle_kog_rotate_loop", 0f, 0.2f);
		AudioManager.Play("sfx_dlc_kog_castle_kog_roateend");
		yield return this.castleAnimator.WaitForNormalizedTime(this, 1f, endPrefix + "Stop", ChessCastleLevel.CastleBaseLayer, true, false, true);
		this.rotating = false;
		CupheadLevelCamera.Current.EndShake(0.2f);
		this.castleAnimator.Play(endPrefix + "Idle", ChessCastleLevel.CastleBaseLayer);
		this.castleAnimator.Play(endPrefix + "Open", ChessCastleLevel.CastleDoorLayer);
		AudioManager.Play(ChessCastleLevel.DoorSounds[destinationIndex] + "_open");
		if (endLevel == Levels.ChessPawn || endLevel == Levels.ChessBishop || endLevel == Levels.ChessRook || endLevel == Levels.ChessQueen)
		{
			this.castleAnimator.Play(endPrefix, ChessCastleLevel.CastleFlairLayer);
		}
		this.platformAnimator.Play("Stop", ChessCastleLevel.PlatformBaseLayer);
		this.setTargetLevel(endLevel, gauntlet);
		Behaviour behaviour2 = this.startEntity;
		flag = true;
		this.dialogueInteractionPoint.enabled = flag;
		flag = flag;
		this.exitEntity.enabled = flag;
		behaviour2.enabled = flag;
		this.rotationCoroutine = null;
		if (gauntlet)
		{
			this.gauntletSparklesCoroutine = base.StartCoroutine(this.gauntletSparkles_cr());
		}
		yield break;
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x000039B2 File Offset: 0x00001BB2
	public void StartChessLevel()
	{
		base.StartCoroutine(this.startChessLevel_cr());
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0005FF10 File Offset: 0x0005E110
	public IEnumerator startChessLevel_cr()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.playerStartLevelEffects[0].gameObject.SetActive(true);
		this.playerStartLevelEffects[0].transform.position = player.transform.position;
		player.gameObject.SetActive(false);
		this.playerStartLevelEffects[0].animator.SetTrigger("OnStartTutorial");
		player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null)
		{
			this.playerStartLevelEffects[1].gameObject.SetActive(true);
			this.playerStartLevelEffects[1].transform.position = player.transform.position;
			player.gameObject.SetActive(false);
			this.playerStartLevelEffects[1].animator.SetTrigger("OnStartTutorial");
		}
		AudioManager.Play("sfx_dlc_kog_castle_kog_entercastle");
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		GauntletContext context = (!this.currentIsGauntlet) ? null : new GauntletContext(false);
		Levels level = this.currentTargetLevel;
		SceneLoader.Transition transitionStart = SceneLoader.Transition.Iris;
		GauntletContext context2 = context;
		SceneLoader.LoadLevel(level, transitionStart, SceneLoader.Icon.Hourglass, context2);
		yield break;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x0005FF2C File Offset: 0x0005E12C
	public void Exit()
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null))
			{
				levelPlayerController.DisableInput();
			}
		}
		SceneLoader.LoadScene(Scenes.scene_map_world_DLC, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x060000EA RID: 234 RVA: 0x0005FFA8 File Offset: 0x0005E1A8
	public IEnumerator cloudSpawn_cr()
	{
		MinMax cloudSpawnTime = new MinMax(0.8f, 1.4f);
		for (;;)
		{
			float elapsedTime = 0f;
			float duration = cloudSpawnTime.RandomFloat();
			while (elapsedTime < ((!this.rotating) ? duration : (duration / this.rotationMultiplier)))
			{
				yield return null;
				elapsedTime += CupheadTime.Delta;
			}
			GameObject obj = Object.Instantiate<GameObject>(this.cloudPrefab);
			obj.GetComponent<ChessCastleLevelCloud>().Initialize(this);
		}
		yield break;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x000039C1 File Offset: 0x00001BC1
	public void AnimateCoins(int count)
	{
		base.StartCoroutine(this.coin_cr(count));
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0005FFC4 File Offset: 0x0005E1C4
	public IEnumerator coin_cr(int count)
	{
		for (int i = 0; i < count; i++)
		{
			string animationName = (i % 2 != 0) ? "CoinB" : "CoinA";
			this.kingAnimator.Play(animationName, 1);
			yield return this.kingAnimator.WaitForAnimationToEnd(this, animationName, 1, false, true);
			AudioManager.Play("sfx_coin_pickup");
			GameObject coinSpark = Object.Instantiate<GameObject>(this.coinPrefab, this.coinSparkSpawnPoint.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
			Renderer coinSparkRenderer = coinSpark.GetComponent<Renderer>();
			coinSparkRenderer.sortingLayerName = "Effects";
			coinSparkRenderer.sortingOrder = 50;
			coinSpark.GetComponent<Animator>().Play("anim_level_coin_death");
		}
		yield break;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x0005FFE8 File Offset: 0x0005E1E8
	public IEnumerator gauntletSparkles_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.35f);
			Vector3 position = this.sparklesCenter.position + Random.insideUnitCircle * 70f;
			Effect effect = this.sparkleEffect.Create(position);
			effect.GetComponent<AnimationHelper>().Speed = 0.333333343f;
			effect.GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";
		}
		yield break;
	}

	// Token: 0x060000EE RID: 238 RVA: 0x000039D1 File Offset: 0x00001BD1
	public void onDialogueStartedHandler()
	{
		base.Ending = true;
		AudioManager.Play("sfx_dlc_kog_castle_kingvoice");
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00060004 File Offset: 0x0005E204
	public void onDialogueMessageHandler(string message, string metadata)
	{
		if (message == "GiveCoins")
		{
			string[] array;
			if (ChessCastleLevel.Coins.TryGetValue(this.currentTargetLevel, out array))
			{
				this.AnimateCoins(array.Length);
			}
		}
		else if (message == "SetupChooseLevel")
		{
			this.setupChooseLevel();
		}
		else if (message == "ChooseLevel")
		{
			this.revertChooseLevel();
			if (metadata == "-1")
			{
				return;
			}
			int num;
			if (Parser.IntTryParse(metadata, out num))
			{
				if (num == 5)
				{
					this.rotate(this.currentTargetLevel, Levels.ChessPawn, true, false);
				}
				else
				{
					Levels endLevel = Level.kingOfGamesLevels[num];
					this.rotate(this.currentTargetLevel, endLevel, false, false);
				}
			}
		}
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x000600CC File Offset: 0x0005E2CC
	public void onDialogueEndedHandler()
	{
		base.Ending = false;
		this.stopTalk();
		bool flag = SceneLoader.CurrentContext is GauntletContext && ((GauntletContext)SceneLoader.CurrentContext).complete;
		if (flag)
		{
			OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatKOGGauntlet);
			this.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGames_WDLC;
		}
		else if (this.dialogueInteractionPoint.dialogueInteraction == DialoguerDialogues.KingOfGamesVictory_WDLC)
		{
			PlayerData.SaveCurrentFile();
			this.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGames_WDLC;
			if (this.attemptsToBeat < ChessCastleLevel.MaxAttemptsToContinue && Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) != (float)ChessCastleLevel.KingOfGamesFinalDialogueState)
			{
				this.rotate(this.currentTargetLevel, this.calculateCurrentLevel(), false, false);
			}
			else
			{
				this.Exit();
			}
		}
		else if (this.firstEntry)
		{
			PlayerData.SaveCurrentFile();
			if (!this.startEntity.enabled)
			{
				AudioManager.Play("sfx_dlc_kog_castle_door_wooddoor_open");
			}
			this.castleAnimator.Play(ChessCastleLevel.LevelPrefixes[0] + "Open", ChessCastleLevel.CastleDoorLayer);
			this.startEntity.enabled = true;
		}
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00060200 File Offset: 0x0005E400
	public void onDialogueAdvancedHandler(DialoguerTextData data)
	{
		if (!this.kingAnimator.GetCurrentAnimatorStateInfo(0).IsName("Talk"))
		{
			this.kingAnimator.SetTrigger("Talk");
		}
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x000039E4 File Offset: 0x00001BE4
	public void StartTalkAnimation()
	{
		this.kingAnimator.SetBool("Talking", true);
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x000039F7 File Offset: 0x00001BF7
	public void EndTalkAnimation()
	{
		this.kingAnimator.SetBool("Talking", false);
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00003A0A File Offset: 0x00001C0A
	public void onIntroEventHandler()
	{
		this.beginIntroPan = true;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00003A13 File Offset: 0x00001C13
	public void onFadeOutStartEventHandler(float time)
	{
		this.beginIntroPan = true;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00003A1C File Offset: 0x00001C1C
	public void stopTalk()
	{
		this.kingAnimator.SetBool("Talking", false);
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x0006023C File Offset: 0x0005E43C
	public void updateDialogueState()
	{
		int num = PlayerData.Data.CountLevelsCompleted(Level.kingOfGamesLevels);
		if (num == 1)
		{
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, 2f);
		}
		else if (num == 2)
		{
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, 3f);
		}
		else if (num == 3)
		{
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, 4f);
		}
		else if (num == 4)
		{
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, 5f);
		}
		else if (Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) == 7f)
		{
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, (float)ChessCastleLevel.KingOfGamesFinalDialogueState);
		}
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x000602EC File Offset: 0x0005E4EC
	public Levels calculateCurrentLevel()
	{
		foreach (Levels levels in Level.kingOfGamesLevels)
		{
			if (!PlayerData.Data.CheckLevelCompleted(levels))
			{
				return levels;
			}
		}
		return Level.kingOfGamesLevels.GetLast<Levels>();
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00060334 File Offset: 0x0005E534
	public Levels calculatePreviousLevel()
	{
		Levels[] kingOfGamesLevels = Level.kingOfGamesLevels;
		if (!PlayerData.Data.CheckLevelCompleted(kingOfGamesLevels[0]))
		{
			return Levels.Test;
		}
		for (int i = 1; i < Level.kingOfGamesLevels.Length; i++)
		{
			if (!PlayerData.Data.CheckLevelCompleted(kingOfGamesLevels[i]))
			{
				return kingOfGamesLevels[i - 1];
			}
		}
		return Levels.Test;
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00003A2F File Offset: 0x00001C2F
	public void setTargetLevel(Levels level, bool gauntlet)
	{
		this.currentTargetLevel = level;
		this.currentIsGauntlet = gauntlet;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x0006038C File Offset: 0x0005E58C
	public void setupChooseLevel()
	{
		int i = Array.IndexOf<Levels>(Level.kingOfGamesLevels, this.currentTargetLevel);
		this.speechBubble.HideOptionByIndex(i);
		Vector2 basePosition = this.speechBubbleBasePosition;
		basePosition.y -= 130f;
		this.speechBubble.basePosition = basePosition;
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00003A3F File Offset: 0x00001C3F
	public void revertChooseLevel()
	{
		this.speechBubble.basePosition = this.speechBubbleBasePosition;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x000603DC File Offset: 0x0005E5DC
	public void movePlayersToDialoguePositions()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		if (player != null)
		{
			Vector3 position = player.transform.position;
			position.x = this.dialogueInteractionPoint.playerOneDialoguePosition.x;
			player.transform.position = position;
		}
		player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null)
		{
			Vector3 position2 = player.transform.position;
			position2.x = this.dialogueInteractionPoint.playerTwoDialoguePosition.x;
			player.transform.position = position2;
		}
	}

	// Token: 0x040000C0 RID: 192
	public LevelProperties.ChessCastle properties;

	// Token: 0x040000C1 RID: 193
	public static readonly int MaxAttemptsToContinue = 5;

	// Token: 0x040000C2 RID: 194
	public static readonly int KingOfGamesDialoguerStateIndex = 36;

	// Token: 0x040000C3 RID: 195
	public static readonly int KingOfGamesVictoryDialoguerCountIndex = 37;

	// Token: 0x040000C4 RID: 196
	public static readonly int KingOfGamesVictoryDialoguerStateIndex = 42;

	// Token: 0x040000C5 RID: 197
	public static readonly int KingOfGamesFinalDialogueState = 6;

	// Token: 0x040000C6 RID: 198
	public static readonly int CastleBaseLayer = 0;

	// Token: 0x040000C7 RID: 199
	public static readonly int CastleDoorLayer = 1;

	// Token: 0x040000C8 RID: 200
	public static readonly int CastleFlairLayer = 2;

	// Token: 0x040000C9 RID: 201
	public static readonly string[] LevelPrefixes = new string[]
	{
		"Pawn",
		"Knight",
		"Bishop",
		"Rook",
		"Queen"
	};

	// Token: 0x040000CA RID: 202
	public static readonly int PlatformBaseLayer = 0;

	// Token: 0x040000CB RID: 203
	public static readonly Dictionary<Levels, string[]> Coins = new Dictionary<Levels, string[]>
	{
		{
			Levels.ChessPawn,
			new string[]
			{
				"a37b3d37-a32e-4b88-a583-34489496494d",
				"25f15554-d229-4330-96cc-ac8a13c18ea0"
			}
		},
		{
			Levels.ChessKnight,
			new string[]
			{
				"eacf4228-e200-4839-9d79-3439cfcc5824",
				"47f7edb1-b5c5-4afb-9acb-a46f5e6df557"
			}
		},
		{
			Levels.ChessBishop,
			new string[]
			{
				"3826615a-498b-4158-af7b-0d01acbc18c8",
				"d52b1cc6-414c-4a7c-9f8a-250316566d58"
			}
		},
		{
			Levels.ChessRook,
			new string[]
			{
				"fc2c48cd-5dec-472a-ae18-dccfc94232c6",
				"16732bc8-7230-467a-a9ac-ff9c62ab7657"
			}
		},
		{
			Levels.ChessQueen,
			new string[]
			{
				"e0c6e8bc-0c56-4e52-a9a1-c53887f5ca4c",
				"19090606-09e8-4e56-92ac-e08200926b94",
				"39bfe6d8-0dbc-4886-9998-52c67b57969e"
			}
		}
	};

	// Token: 0x040000CC RID: 204
	public static readonly string[] DoorSounds = new string[]
	{
		"sfx_dlc_kog_castle_door_wooddoor",
		"sfx_dlc_kog_castle_door_drawbridge",
		"sfx_dlc_kog_castle_door_tall",
		"sfx_dlc_kog_castle_door_portcullis",
		"sfx_dlc_kog_castle_door_queen"
	};

	// Token: 0x040000CD RID: 205
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x040000CE RID: 206
	[SerializeField]
	[Multiline]
	public string _bossQuote;

	// Token: 0x040000CF RID: 207
	[SerializeField]
	public AbstractLevelInteractiveEntity startEntity;

	// Token: 0x040000D0 RID: 208
	[SerializeField]
	public AbstractLevelInteractiveEntity exitEntity;

	// Token: 0x040000D1 RID: 209
	[SerializeField]
	public PlayerDeathEffect[] playerStartLevelEffects;

	// Token: 0x040000D2 RID: 210
	[SerializeField]
	public ChessCastleLevelKingInteractionPoint dialogueInteractionPoint;

	// Token: 0x040000D3 RID: 211
	[SerializeField]
	public SpeechBubble speechBubble;

	// Token: 0x040000D4 RID: 212
	[SerializeField]
	public Animator castleAnimator;

	// Token: 0x040000D5 RID: 213
	[SerializeField]
	public Animator platformAnimator;

	// Token: 0x040000D6 RID: 214
	[SerializeField]
	public GameObject cloudPrefab;

	// Token: 0x040000D7 RID: 215
	[SerializeField]
	public GameObject coinPrefab;

	// Token: 0x040000D8 RID: 216
	[SerializeField]
	public Transform coinSparkSpawnPoint;

	// Token: 0x040000D9 RID: 217
	[SerializeField]
	public Animator kingAnimator;

	// Token: 0x040000DA RID: 218
	[SerializeField]
	public Effect sparkleEffect;

	// Token: 0x040000DB RID: 219
	[SerializeField]
	public Transform sparklesCenter;

	// Token: 0x040000DC RID: 220
	[SerializeField]
	public float sinePeriod;

	// Token: 0x040000DD RID: 221
	[SerializeField]
	public float sineAmplitude;

	// Token: 0x040000DE RID: 222
	[SerializeField]
	public float _rotationMultiplier;

	// Token: 0x040000DF RID: 223
	[SerializeField]
	public float introPanAmount;

	// Token: 0x040000E0 RID: 224
	[SerializeField]
	public float introPanDuration;

	// Token: 0x040000E1 RID: 225
	public bool firstEntry;

	// Token: 0x040000E2 RID: 226
	public Levels previousLevel;

	// Token: 0x040000E3 RID: 227
	public bool previouslyWon;

	// Token: 0x040000E4 RID: 228
	public int attemptsToBeat;

	// Token: 0x040000E5 RID: 229
	public Levels currentTargetLevel;

	// Token: 0x040000E6 RID: 230
	public bool currentIsGauntlet;

	// Token: 0x040000E7 RID: 231
	public Coroutine rotationCoroutine;

	// Token: 0x040000E8 RID: 232
	public Coroutine gauntletSparklesCoroutine;

	// Token: 0x040000E9 RID: 233
	public Vector2 speechBubbleBasePosition;

	// Token: 0x040000EA RID: 234
	public float cameraSineAccumulator;

	// Token: 0x040000EB RID: 235
	public Coroutine introCameraMovementCoroutine;

	// Token: 0x040000EC RID: 236
	public bool beginIntroPan;
}
