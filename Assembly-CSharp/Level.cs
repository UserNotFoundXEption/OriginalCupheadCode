using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010F RID: 271
public abstract class Level : AbstractPausableComponent
{
	// Token: 0x06000C93 RID: 3219 RVA: 0x000840C4 File Offset: 0x000822C4
	public Level()
	{
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0000AFCB File Offset: 0x000091CB
	// (set) Token: 0x06000C95 RID: 3221 RVA: 0x0000AFD2 File Offset: 0x000091D2
	public static Level Current { get; set; }

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000C96 RID: 3222 RVA: 0x0000AFDA File Offset: 0x000091DA
	// (set) Token: 0x06000C97 RID: 3223 RVA: 0x0000AFE1 File Offset: 0x000091E1
	public static Level.Mode CurrentMode { get; set; } = Level.Mode.Normal;

	// Token: 0x06000C98 RID: 3224 RVA: 0x0000AFE9 File Offset: 0x000091E9
	public static void SetCurrentMode(Level.Mode mode)
	{
		Level.CurrentMode = mode;
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0000AFF1 File Offset: 0x000091F1
	// (set) Token: 0x06000C9A RID: 3226 RVA: 0x0000AFF8 File Offset: 0x000091F8
	public static bool PreviouslyWon { get; set; }

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0000B000 File Offset: 0x00009200
	// (set) Token: 0x06000C9C RID: 3228 RVA: 0x0000B007 File Offset: 0x00009207
	public static bool Won { get; set; }

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06000C9D RID: 3229 RVA: 0x0000B00F File Offset: 0x0000920F
	// (set) Token: 0x06000C9E RID: 3230 RVA: 0x0000B016 File Offset: 0x00009216
	public static LevelScoringData.Grade Grade { get; set; }

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06000C9F RID: 3231 RVA: 0x0000B01E File Offset: 0x0000921E
	// (set) Token: 0x06000CA0 RID: 3232 RVA: 0x0000B025 File Offset: 0x00009225
	public static LevelScoringData.Grade PreviousGrade { get; set; }

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x0000B02D File Offset: 0x0000922D
	// (set) Token: 0x06000CA2 RID: 3234 RVA: 0x0000B034 File Offset: 0x00009234
	public static Level.Mode Difficulty { get; set; }

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x0000B03C File Offset: 0x0000923C
	// (set) Token: 0x06000CA4 RID: 3236 RVA: 0x0000B043 File Offset: 0x00009243
	public static Level.Mode PreviousDifficulty { get; set; }

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x0000B04B File Offset: 0x0000924B
	// (set) Token: 0x06000CA6 RID: 3238 RVA: 0x0000B052 File Offset: 0x00009252
	public static LevelScoringData ScoringData { get; set; }

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0000B05A File Offset: 0x0000925A
	// (set) Token: 0x06000CA8 RID: 3240 RVA: 0x0000B061 File Offset: 0x00009261
	public static Levels PreviousLevel { get; set; }

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x0000B069 File Offset: 0x00009269
	// (set) Token: 0x06000CAA RID: 3242 RVA: 0x0000B070 File Offset: 0x00009270
	public static Level.Type PreviousLevelType { get; set; }

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06000CAB RID: 3243 RVA: 0x0000B078 File Offset: 0x00009278
	// (set) Token: 0x06000CAC RID: 3244 RVA: 0x0000B07F File Offset: 0x0000927F
	public static bool IsDicePalace { get; set; }

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x06000CAD RID: 3245 RVA: 0x0000B087 File Offset: 0x00009287
	// (set) Token: 0x06000CAE RID: 3246 RVA: 0x0000B08E File Offset: 0x0000928E
	public static bool IsDicePalaceMain { get; set; }

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x06000CAF RID: 3247 RVA: 0x0000B096 File Offset: 0x00009296
	// (set) Token: 0x06000CB0 RID: 3248 RVA: 0x0000B09D File Offset: 0x0000929D
	public static bool SuperUnlocked { get; set; }

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x0000B0A5 File Offset: 0x000092A5
	// (set) Token: 0x06000CB2 RID: 3250 RVA: 0x0000B0AC File Offset: 0x000092AC
	public static bool OverrideDifficulty { get; set; }

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x0000B0B4 File Offset: 0x000092B4
	// (set) Token: 0x06000CB4 RID: 3252 RVA: 0x0000B0BB File Offset: 0x000092BB
	public static bool IsChessBoss { get; set; }

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0000B0C3 File Offset: 0x000092C3
	// (set) Token: 0x06000CB6 RID: 3254 RVA: 0x0000B0CA File Offset: 0x000092CA
	public static bool IsTowerOfPower { get; set; }

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0000B0D2 File Offset: 0x000092D2
	// (set) Token: 0x06000CB8 RID: 3256 RVA: 0x0000B0D9 File Offset: 0x000092D9
	public static bool IsTowerOfPowerMain { get; set; }

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x0000B0E1 File Offset: 0x000092E1
	// (set) Token: 0x06000CBA RID: 3258 RVA: 0x0000B0E8 File Offset: 0x000092E8
	public static bool IsGraveyard { get; set; }

	// Token: 0x06000CBB RID: 3259 RVA: 0x0000B0F0 File Offset: 0x000092F0
	public static void ResetPreviousLevelInfo()
	{
		Level.Won = false;
		Level.SuperUnlocked = false;
		PlayerManager.playerWasChalice[0] = false;
		PlayerManager.playerWasChalice[1] = false;
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x00084148 File Offset: 0x00082348
	public static Levels GetEnumByName(string levelName)
	{
		return (Levels)Enum.Parse(typeof(Levels), levelName);
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x0008416C File Offset: 0x0008236C
	public static string GetLevelName(Levels level)
	{
		return Localization.Translate(level.ToString()).text;
	}

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06000CBE RID: 3262 RVA: 0x0000B10E File Offset: 0x0000930E
	// (set) Token: 0x06000CBF RID: 3263 RVA: 0x0000B116 File Offset: 0x00009316
	public Level.Mode mode { get; set; }

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x0000B11F File Offset: 0x0000931F
	// (set) Token: 0x06000CC1 RID: 3265 RVA: 0x0000B127 File Offset: 0x00009327
	public bool defeatedMinion { get; set; }

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x0000B130 File Offset: 0x00009330
	// (set) Token: 0x06000CC3 RID: 3267 RVA: 0x0000B138 File Offset: 0x00009338
	public bool PlayersCreated { get; set; }

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0000B141 File Offset: 0x00009341
	// (set) Token: 0x06000CC5 RID: 3269 RVA: 0x0000B149 File Offset: 0x00009349
	public bool Initialized { get; set; }

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x0000B152 File Offset: 0x00009352
	// (set) Token: 0x06000CC7 RID: 3271 RVA: 0x0000B15A File Offset: 0x0000935A
	public bool Started { get; set; }

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x0000B163 File Offset: 0x00009363
	// (set) Token: 0x06000CC9 RID: 3273 RVA: 0x0000B16B File Offset: 0x0000936B
	public bool[] BlockChaliceCharm { get; set; }

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06000CCA RID: 3274 RVA: 0x0000B174 File Offset: 0x00009374
	// (set) Token: 0x06000CCB RID: 3275 RVA: 0x0000B17C File Offset: 0x0000937C
	public float LevelTime { get; set; }

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06000CCC RID: 3276 RVA: 0x0000B185 File Offset: 0x00009385
	public int Ground
	{
		get
		{
			return -this.bounds.bottom;
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0000B193 File Offset: 0x00009393
	public int Ceiling
	{
		get
		{
			return this.bounds.top;
		}
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0000B1A0 File Offset: 0x000093A0
	public int Left
	{
		get
		{
			return -this.bounds.left;
		}
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0000B1AE File Offset: 0x000093AE
	public int Right
	{
		get
		{
			return this.bounds.right;
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0000B1BB File Offset: 0x000093BB
	public int Width
	{
		get
		{
			return this.bounds.left + this.bounds.right;
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0000B1D4 File Offset: 0x000093D4
	public int Height
	{
		get
		{
			return this.bounds.top + this.bounds.bottom;
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0000B1ED File Offset: 0x000093ED
	public Level.Type LevelType
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x0000B1F5 File Offset: 0x000093F5
	// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x0000B1FD File Offset: 0x000093FD
	public bool CameraRotates { get; set; }

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x0000B206 File Offset: 0x00009406
	public bool IntroComplete
	{
		get
		{
			return this.intro.introComplete;
		}
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0000B213 File Offset: 0x00009413
	// (set) Token: 0x06000CD7 RID: 3287 RVA: 0x0000B21B File Offset: 0x0000941B
	public Level.Timeline timeline { get; set; }

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06000CD8 RID: 3288
	public abstract Levels CurrentLevel { get; }

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06000CD9 RID: 3289
	public abstract Scenes CurrentScene { get; }

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000CDA RID: 3290 RVA: 0x0000B224 File Offset: 0x00009424
	public Level.Camera CameraSettings
	{
		get
		{
			return this.camera;
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000CDB RID: 3291
	public abstract Sprite BossPortrait { get; }

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06000CDC RID: 3292
	public abstract string BossQuote { get; }

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06000CDD RID: 3293 RVA: 0x0000B22C File Offset: 0x0000942C
	// (set) Token: 0x06000CDE RID: 3294 RVA: 0x0000B234 File Offset: 0x00009434
	public bool Ending { get; set; }

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06000CDF RID: 3295 RVA: 0x0000B23D File Offset: 0x0000943D
	public static bool IsInBossesHub
	{
		get
		{
			return Level.IsDicePalace || Level.IsDicePalaceMain || Level.IsTowerOfPower;
		}
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x0000B25B File Offset: 0x0000945B
	public static PlayersStatsBossesHub GetPlayerStats(PlayerId playerId)
	{
		if (Level.IsTowerOfPower)
		{
			return TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId];
		}
		if (Level.IsDicePalace || Level.IsDicePalaceMain)
		{
			return (playerId != PlayerId.PlayerOne) ? DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS : DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS;
		}
		return null;
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x0000B29A File Offset: 0x0000949A
	public virtual void OnEnable()
	{
		EventManager.Instance.AddListener<PlayerStatsManager.DeathEvent>(new EventManager.EventDelegate<PlayerStatsManager.DeathEvent>(this.OnPlayerDeath));
		EventManager.Instance.AddListener<PlayerStatsManager.ReviveEvent>(new EventManager.EventDelegate<PlayerStatsManager.ReviveEvent>(this.OnPlayerRevive));
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x0000B2C8 File Offset: 0x000094C8
	public virtual void OnDisable()
	{
		EventManager.Instance.RemoveListener<PlayerStatsManager.DeathEvent>(new EventManager.EventDelegate<PlayerStatsManager.DeathEvent>(this.OnPlayerDeath));
		EventManager.Instance.RemoveListener<PlayerStatsManager.ReviveEvent>(new EventManager.EventDelegate<PlayerStatsManager.ReviveEvent>(this.OnPlayerRevive));
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x00084194 File Offset: 0x00082394
	public override void Awake()
	{
		base.Awake();
		this.CheckIfInABossesHub();
		Cuphead.Init(false);
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeave;
		DamageDealer.didDamageWithNonSmallPlaneWeapon = false;
		Levels currentLevel = this.CurrentLevel;
		switch (currentLevel)
		{
		case Levels.Platforming_Level_1_1:
		case Levels.Platforming_Level_1_2:
		case Levels.Platforming_Level_3_1:
		case Levels.Platforming_Level_3_2:
			break;
		default:
			if (currentLevel != Levels.Platforming_Level_2_2 && currentLevel != Levels.Platforming_Level_2_1)
			{
				this.mode = Level.CurrentMode;
				goto IL_95;
			}
			break;
		}
		this.mode = Level.Mode.Normal;
		IL_95:
		Level.Current = this;
		PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(this.CurrentLevel);
		Level.Won = false;
		this.BGMPlaylistCurrent = levelData.bgmPlayListCurrent;
		Level.PreviousLevel = this.CurrentLevel;
		Level.PreviousLevelType = this.type;
		Level.PreviouslyWon = levelData.completed;
		Level.PreviousGrade = levelData.grade;
		Level.PreviousDifficulty = levelData.difficultyBeaten;
		Level.SuperUnlocked = false;
		Level.IsChessBoss = false;
		Level.IsGraveyard = false;
		this.Ending = false;
		this.PartialInit();
		Application.targetFrameRate = 60;
		this.CreateUI();
		this.CreateHUD();
		LevelCoin.OnLevelStart();
		SceneLoader.SetCurrentLevel(this.CurrentLevel);
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x0000B2F6 File Offset: 0x000094F6
	public virtual bool AllowDjimmi()
	{
		return !this.isMausoleum && this.type != Level.Type.Tutorial && !(SceneLoader.CurrentContext is GauntletContext);
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x0000B322 File Offset: 0x00009522
	public virtual void CheckIfInABossesHub()
	{
		Level.IsDicePalace = false;
		Level.IsDicePalaceMain = false;
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x0000B330 File Offset: 0x00009530
	public static void ResetBossesHub()
	{
		Level.IsDicePalace = false;
		Level.IsDicePalaceMain = false;
		if (Level.IsTowerOfPower)
		{
			TowerOfPowerLevelGameInfo.GameInfo.CleanUp();
			Level.IsTowerOfPower = false;
			Level.IsTowerOfPowerMain = false;
		}
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x000842D8 File Offset: 0x000824D8
	public virtual void PartialInit()
	{
		if (Level.ScoringData == null || ((this.type == Level.Type.Battle || this.type == Level.Type.Platforming) && (!Level.IsDicePalace || (Level.IsDicePalaceMain && DicePalaceMainLevelGameInfo.TURN_COUNTER == 0)) && (!Level.IsTowerOfPowerMain || TowerOfPowerLevelGameInfo.TURN_COUNTER == 0)))
		{
			Level.ScoringData = new LevelScoringData();
			Level.ScoringData.goalTime = ((this.mode != Level.Mode.Easy) ? ((this.mode != Level.Mode.Normal) ? this.goalTimes.hard : this.goalTimes.normal) : this.goalTimes.easy);
		}
		if ((Level.IsDicePalace && !Level.IsDicePalaceMain) || (Level.IsTowerOfPower && !Level.IsTowerOfPowerMain))
		{
			Level.ScoringData.goalTime += ((this.mode != Level.Mode.Easy) ? ((this.mode != Level.Mode.Normal) ? this.goalTimes.hard : this.goalTimes.normal) : this.goalTimes.easy);
		}
		Level.ScoringData.difficulty = this.mode;
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0008441C File Offset: 0x0008261C
	public virtual void Start()
	{
		CupheadTime.SetAll(1f);
		switch (this.type)
		{
		default:
			base.StartCoroutine(this.startBattle_cr());
			break;
		case Level.Type.Tutorial:
			base.StartCoroutine(this.startNonBattle_cr());
			break;
		case Level.Type.Platforming:
			base.StartCoroutine(this.startPlatforming_cr());
			break;
		}
		this.CreateAudio();
		this.CreateColliders();
		this.CreatePlayers();
		this.CreateCamera();
		this.gui.LevelInit();
		this.hud.LevelInit();
		this.SetRichPresence();
		this.Initialized = true;
		if (this.playerMode != PlayerMode.Plane && this.CurrentLevel != Levels.Devil && this.CurrentLevel != Levels.Saltbaker && this.type != Level.Type.Platforming && this.type != Level.Type.Tutorial)
		{
			base.StartCoroutine(this.check_intros_cr());
		}
		if (this.CurrentLevel == Levels.Devil || this.CurrentLevel == Levels.Saltbaker)
		{
			this.CheckIntros();
		}
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x0008453C File Offset: 0x0008273C
	public virtual void Update()
	{
		if (!this.Started)
		{
			this.CheckPlayerHoldingButtons();
		}
		this.LevelTime += CupheadTime.Delta;
		if (this.playerIsDead)
		{
			this.playerDeathDelayFrames++;
			if (this.playerDeathDelayFrames < 5)
			{
				if (PlayerManager.Multiplayer)
				{
					if (this.players[0].IsDead && this.players[1].IsDead)
					{
						this._OnLose();
					}
				}
				else
				{
					this._OnLose();
				}
				this.playerIsDead = false;
				this.playerDeathDelayFrames = 0;
			}
		}
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x000845E4 File Offset: 0x000827E4
	public override void OnDestroy()
	{
		base.OnDestroy();
		PlayerManager.ClearPlayers();
		Level.Current = null;
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeave;
		this.LevelResources = null;
		this.players = null;
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x00084634 File Offset: 0x00082834
	public void SetBounds(int? left, int? right, int? top, int? bottom)
	{
		if (left != null)
		{
			this.bounds.left = left.Value;
		}
		if (right != null)
		{
			this.bounds.right = right.Value;
		}
		if (top != null)
		{
			this.bounds.top = top.Value;
		}
		if (bottom != null)
		{
			this.bounds.bottom = bottom.Value;
		}
		this.bounds.SetColliderPositions();
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x0000B35E File Offset: 0x0000955E
	public void CleanUpScore()
	{
		Level.ScoringData = null;
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x0000B366 File Offset: 0x00009566
	public void RegisterMinionKilled()
	{
		if (!this.defeatedMinion)
		{
		}
		this.defeatedMinion = true;
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x0000B37A File Offset: 0x0000957A
	public void CreateAudio()
	{
		LevelAudio.Create();
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x000846C4 File Offset: 0x000828C4
	public virtual void CreatePlayers()
	{
		this.PlayersCreated = true;
		foreach (AbstractPlayerController abstractPlayerController in Object.FindObjectsOfType<AbstractPlayerController>())
		{
			Object.Destroy(abstractPlayerController.gameObject);
		}
		this.players = new AbstractPlayerController[2];
		this.BlockChaliceCharm = new bool[2];
		this.BlockChaliceCharm[0] = this.blockChalice;
		this.BlockChaliceCharm[1] = this.blockChalice;
		if (this.playerMode == PlayerMode.Custom)
		{
			return;
		}
		if (PlayerManager.Multiplayer && this.allowMultiplayer)
		{
			if (PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).charm == Charm.charm_chalice && PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm == Charm.charm_chalice)
			{
				this.BlockChaliceCharm[(!Rand.Bool()) ? 1 : 0] = true;
			}
			if (this.isMausoleum)
			{
				this.BlockChaliceCharm[0] = true;
				this.BlockChaliceCharm[1] = true;
			}
			Vector3 vector = (this.playerMode != PlayerMode.Plane) ? this.spawns.playerOne : this.player1PlaneSpawnPos;
			this.players[0] = AbstractPlayerController.Create(PlayerId.PlayerOne, vector, this.playerMode);
			Vector3 vector2 = (this.playerMode != PlayerMode.Plane) ? this.spawns.playerTwo : this.player2PlaneSpawnPos;
			this.players[1] = AbstractPlayerController.Create(PlayerId.PlayerTwo, vector2, this.playerMode);
		}
		else
		{
			Vector3 vector3 = (this.playerMode != PlayerMode.Plane) ? this.spawns.playerOneSingle : this.player1PlaneSpawnPos;
			this.players[0] = AbstractPlayerController.Create(PlayerId.PlayerOne, vector3, this.playerMode);
		}
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x000848A0 File Offset: 0x00082AA0
	public void CheckPlayerCharacters()
	{
		Level.ScoringData.player1IsChalice = this.players[0].stats.isChalice;
		if (PlayerManager.Multiplayer && this.allowMultiplayer)
		{
			Level.ScoringData.player2IsChalice = this.players[1].stats.isChalice;
		}
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x000848FC File Offset: 0x00082AFC
	public void CheckIntros()
	{
		LevelPlayerAnimationController component = this.players[0].GetComponent<LevelPlayerAnimationController>();
		if (component != null)
		{
			if (this.players[0].stats.Loadout.charm == Charm.charm_chalice)
			{
				if (this.players[1] != null && this.players[1].stats.isChalice && this.CurrentLevel != Levels.Devil && this.CurrentLevel != Levels.Saltbaker && (!Level.IsDicePalace || DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY))
				{
					component.CookieFail();
				}
				if (this.players[0].stats.isChalice && (this.CurrentLevel == Levels.Devil || this.CurrentLevel == Levels.Saltbaker))
				{
					component.ScaredChalice(this.CurrentLevel == Levels.Devil);
				}
			}
			else if (this.CurrentLevel != Levels.Devil && this.CurrentLevel != Levels.Saltbaker)
			{
				if (this.player1HeldJump && !this.player1HeldSuper)
				{
					component.IsIntroB();
				}
				else if (!this.player1HeldJump && !this.player1HeldSuper && Rand.Bool())
				{
					component.IsIntroB();
				}
			}
		}
		if (this.players.Length >= 2 && this.players[1] != null)
		{
			LevelPlayerAnimationController component2 = this.players[1].GetComponent<LevelPlayerAnimationController>();
			if (component2 != null)
			{
				if (this.players[1].stats.Loadout.charm == Charm.charm_chalice)
				{
					if (this.players[0].stats.isChalice && this.CurrentLevel != Levels.Devil && this.CurrentLevel != Levels.Saltbaker && (!Level.IsDicePalace || DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY))
					{
						component2.CookieFail();
					}
					if (this.players[1].stats.isChalice && (this.CurrentLevel == Levels.Devil || this.CurrentLevel == Levels.Saltbaker))
					{
						component2.ScaredChalice(this.CurrentLevel == Levels.Devil);
					}
				}
				else if (PlayerManager.Multiplayer && this.CurrentLevel != Levels.Devil && this.CurrentLevel != Levels.Saltbaker)
				{
					if (this.player2HeldJump && !this.player2HeldSuper)
					{
						component2.IsIntroB();
					}
					else if (!this.player2HeldJump && !this.player2HeldSuper && Rand.Bool())
					{
						component2.IsIntroB();
					}
				}
			}
		}
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x00084BCC File Offset: 0x00082DCC
	public virtual void CreatePlayerTwoOnJoin()
	{
		if (PlayerManager.Multiplayer && this.allowMultiplayer)
		{
			if (this.players[0].stats.isChalice || this.blockChalice)
			{
				this.BlockChaliceCharm[1] = true;
			}
			this.players[1] = AbstractPlayerController.Create(PlayerId.PlayerTwo, this.players[0].center, this.playerMode);
			this.players[1].LevelJoin(this.players[0].center);
		}
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x00084C5C File Offset: 0x00082E5C
	public void CreateCamera()
	{
		if (this.players == null)
		{
			Debug.LogError("Level.CreateCamera() must be called AFTER Level.CreatePlayers()", null);
		}
		CupheadLevelCamera cupheadLevelCamera = Object.FindObjectOfType<CupheadLevelCamera>();
		cupheadLevelCamera.Init(this.camera);
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x0000B382 File Offset: 0x00009582
	public void CreateUI()
	{
		this.gui = Object.FindObjectOfType<LevelGUI>();
		if (this.gui == null)
		{
			this.gui = this.LevelResources.levelGUI.InstantiatePrefab<LevelGUI>();
		}
	}

	// Token: 0x06000CF5 RID: 3317 RVA: 0x0000B3BB File Offset: 0x000095BB
	public void CreateHUD()
	{
		this.hud = Object.FindObjectOfType<LevelHUD>();
		if (this.hud == null)
		{
			this.hud = this.LevelResources.levelHUD.InstantiatePrefab<LevelHUD>();
		}
	}

	// Token: 0x06000CF6 RID: 3318 RVA: 0x00084C94 File Offset: 0x00082E94
	public void CreateColliders()
	{
		if (this.playerMode == PlayerMode.Plane)
		{
			return;
		}
		this.collidersRoot = new GameObject("Colliders").transform;
		this.collidersRoot.parent = base.transform;
		this.collidersRoot.ResetLocalTransforms();
		this.SetupCollider(Level.Bounds.Side.Left);
		this.SetupCollider(Level.Bounds.Side.Right);
		this.SetupCollider(Level.Bounds.Side.Top);
		this.SetupCollider(Level.Bounds.Side.Bottom);
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x00084D00 File Offset: 0x00082F00
	public Transform SetupCollider(Level.Bounds.Side side)
	{
		string text = string.Empty;
		string tag = string.Empty;
		int layer = 0;
		int num = 0;
		Vector2 zero = Vector2.zero;
		switch (side)
		{
		case Level.Bounds.Side.Left:
			text = "Level_Wall_Left";
			tag = "Wall";
			layer = LayerMask.NameToLayer(Layers.Bounds_Walls.ToString());
			num = 90;
			break;
		case Level.Bounds.Side.Right:
			text = "Level_Wall_Right";
			tag = "Wall";
			layer = LayerMask.NameToLayer(Layers.Bounds_Walls.ToString());
			num = -90;
			break;
		case Level.Bounds.Side.Top:
			text = "Level_Ceiling";
			tag = "Ceiling";
			layer = LayerMask.NameToLayer(Layers.Bounds_Ceiling.ToString());
			break;
		case Level.Bounds.Side.Bottom:
			text = "Level_Ground";
			tag = "Ground";
			layer = LayerMask.NameToLayer(Layers.Bounds_Ground.ToString());
			num = 180;
			break;
		}
		GameObject gameObject = new GameObject(text);
		gameObject.tag = tag;
		gameObject.layer = layer;
		gameObject.transform.ResetLocalTransforms();
		gameObject.transform.SetPosition(new float?(zero.x), new float?(zero.y), null);
		gameObject.transform.SetEulerAngles(null, null, new float?((float)num));
		gameObject.transform.parent = this.collidersRoot;
		BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.isTrigger = true;
		boxCollider2D.size = new Vector2(10000f, 400f);
		Rigidbody2D rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
		rigidbody2D.gravityScale = 0f;
		rigidbody2D.drag = 0f;
		rigidbody2D.angularDrag = 0f;
		rigidbody2D.isKinematic = true;
		this.bounds.colliders.Add(side, boxCollider2D);
		this.bounds.SetColliderPositions();
		gameObject.SetActive(this.bounds.GetEnabled(side));
		return gameObject.transform;
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x00084F0C File Offset: 0x0008310C
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(this.spawns.playerOne, 20f);
		Gizmos.DrawSphere(this.spawns.playerTwo, 20f);
		Gizmos.DrawSphere(this.spawns.playerOneSingle, 30f);
		Gizmos.color = Color.red;
		Gizmos.DrawCube(this.spawns.playerOneSingle, new Vector3(20f, 20f, 20f));
		Gizmos.DrawWireSphere(this.spawns.playerOne, 20f);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.spawns.playerTwo, 20f);
		Gizmos.color = Color.white;
		if (this.camera.bounds.topEnabled)
		{
			Gizmos.DrawLine(new Vector3((float)this.camera.bounds.right, (float)this.camera.bounds.top, 0f), new Vector3((float)(-(float)this.camera.bounds.left), (float)this.camera.bounds.top, 0f));
		}
		if (this.camera.bounds.bottomEnabled)
		{
			Gizmos.DrawLine(new Vector3((float)this.camera.bounds.right, (float)(-(float)this.camera.bounds.bottom), 0f), new Vector3((float)(-(float)this.camera.bounds.left), (float)(-(float)this.camera.bounds.bottom), 0f));
		}
		if (this.camera.bounds.leftEnabled)
		{
			Gizmos.DrawLine(new Vector3((float)(-(float)this.camera.bounds.left), (float)this.camera.bounds.top, 0f), new Vector3((float)(-(float)this.camera.bounds.left), (float)(-(float)this.camera.bounds.bottom), 0f));
		}
		if (this.camera.bounds.rightEnabled)
		{
			Gizmos.DrawLine(new Vector3((float)this.camera.bounds.right, (float)this.camera.bounds.top, 0f), new Vector3((float)this.camera.bounds.right, (float)(-(float)this.camera.bounds.bottom), 0f));
		}
		if (this.bounds.topEnabled)
		{
			Gizmos.color = Color.blue;
		}
		else
		{
			Gizmos.color = Color.black;
		}
		Gizmos.DrawLine(new Vector3((float)this.bounds.right, (float)this.bounds.top, 0f), new Vector3((float)(-(float)this.bounds.left), (float)this.bounds.top, 0f));
		if (this.bounds.bottomEnabled)
		{
			Gizmos.color = Color.green;
		}
		else
		{
			Gizmos.color = Color.black;
		}
		Gizmos.DrawLine(new Vector3((float)this.bounds.right, (float)(-(float)this.bounds.bottom), 0f), new Vector3((float)(-(float)this.bounds.left), (float)(-(float)this.bounds.bottom), 0f));
		if (this.bounds.leftEnabled)
		{
			Gizmos.color = Color.red;
		}
		else
		{
			Gizmos.color = Color.black;
		}
		Gizmos.DrawLine(new Vector3((float)(-(float)this.bounds.left), (float)this.bounds.top, 0f), new Vector3((float)(-(float)this.bounds.left), (float)(-(float)this.bounds.bottom), 0f));
		if (this.bounds.rightEnabled)
		{
			Gizmos.color = Color.red;
		}
		else
		{
			Gizmos.color = Color.black;
		}
		Gizmos.DrawLine(new Vector3((float)this.bounds.right, (float)this.bounds.top, 0f), new Vector3((float)this.bounds.right, (float)(-(float)this.bounds.bottom), 0f));
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x0000B3F4 File Offset: 0x000095F4
	public virtual void OnPlayerJoined(PlayerId playerId)
	{
		LevelNewPlayerGUI.Current.Init();
		this.CreatePlayerTwoOnJoin();
		this.SetRichPresence();
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x00085394 File Offset: 0x00083594
	public void OnPlayerLeave(PlayerId playerId)
	{
		if (playerId == PlayerId.PlayerTwo)
		{
			AbstractPlayerController player = PlayerManager.GetPlayer(playerId);
			if (player != null)
			{
				player.OnLeave(playerId);
			}
			if (PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead)
			{
				this._OnLose();
			}
		}
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x000853D8 File Offset: 0x000835D8
	public void SetRichPresence()
	{
		if (this.CurrentLevel == Levels.Mausoleum)
		{
			OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Mausoleum", true);
		}
		else if (this.CurrentLevel == Levels.Tutorial || this.CurrentLevel == Levels.House)
		{
			OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Tutorial", true);
		}
		else if (this.CurrentLevel == Levels.ShmupTutorial)
		{
			OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Blueprint", true);
		}
		else
		{
			Level.Type type = this.type;
			if (type != Level.Type.Battle)
			{
				if (type == Level.Type.Platforming)
				{
					OnlineManager.Instance.Interface.SetStat(PlayerId.Any, "PlatformingLevel", SceneLoader.SceneName);
					OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Playing", true);
				}
			}
			else
			{
				OnlineManager.Instance.Interface.SetStat(PlayerId.Any, "Boss", SceneLoader.SceneName);
				OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "Fighting", true);
			}
		}
	}

	// Token: 0x06000CFC RID: 3324 RVA: 0x0000B40C File Offset: 0x0000960C
	public void OnPlayerDeath(PlayerStatsManager.DeathEvent e)
	{
		if (this.timeline != null && this.LevelType != Level.Type.Platforming)
		{
			this.timeline.OnPlayerDeath(e.playerId);
		}
		this.playerIsDead = true;
	}

	// Token: 0x06000CFD RID: 3325 RVA: 0x0000B43D File Offset: 0x0000963D
	public void OnPlayerRevive(PlayerStatsManager.ReviveEvent e)
	{
		this.timeline.OnPlayerRevive(e.playerId);
	}

	// Token: 0x06000CFE RID: 3326 RVA: 0x00085510 File Offset: 0x00083710
	public void CheckPlayerHoldingButtons()
	{
		if (PlayerManager.GetPlayerInput(PlayerId.PlayerOne).GetButton(2) && !this.player1HeldJump)
		{
			this.player1HeldJump = true;
		}
		if (PlayerManager.GetPlayerInput(PlayerId.PlayerOne).GetButton(4) && !this.player1HeldSuper)
		{
			this.player1HeldSuper = true;
		}
		if (PlayerManager.Multiplayer)
		{
			if (PlayerManager.GetPlayerInput(PlayerId.PlayerTwo).GetButton(2) && !this.player2HeldJump)
			{
				this.player2HeldJump = true;
			}
			if (PlayerManager.GetPlayerInput(PlayerId.PlayerTwo).GetButton(4) && !this.player2HeldSuper)
			{
				this.player2HeldSuper = true;
			}
		}
	}

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x06000CFF RID: 3327 RVA: 0x000855B4 File Offset: 0x000837B4
	// (remove) Token: 0x06000D00 RID: 3328 RVA: 0x000855EC File Offset: 0x000837EC
	public event Action OnLevelStartEvent;

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x06000D01 RID: 3329 RVA: 0x00085624 File Offset: 0x00083824
	// (remove) Token: 0x06000D02 RID: 3330 RVA: 0x0008565C File Offset: 0x0008385C
	public event Action OnLevelEndEvent;

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x06000D03 RID: 3331 RVA: 0x00085694 File Offset: 0x00083894
	// (remove) Token: 0x06000D04 RID: 3332 RVA: 0x000856CC File Offset: 0x000838CC
	public event Action OnPlatformingLevelAwakeEvent;

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x06000D05 RID: 3333 RVA: 0x00085704 File Offset: 0x00083904
	// (remove) Token: 0x06000D06 RID: 3334 RVA: 0x0008573C File Offset: 0x0008393C
	public event Action OnStateChangedEvent;

	// Token: 0x14000035 RID: 53
	// (add) Token: 0x06000D07 RID: 3335 RVA: 0x00085774 File Offset: 0x00083974
	// (remove) Token: 0x06000D08 RID: 3336 RVA: 0x000857AC File Offset: 0x000839AC
	public event Action OnWinEvent;

	// Token: 0x14000036 RID: 54
	// (add) Token: 0x06000D09 RID: 3337 RVA: 0x000857E4 File Offset: 0x000839E4
	// (remove) Token: 0x06000D0A RID: 3338 RVA: 0x0008581C File Offset: 0x00083A1C
	public event Action OnPreWinEvent;

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06000D0B RID: 3339 RVA: 0x00085854 File Offset: 0x00083A54
	// (remove) Token: 0x06000D0C RID: 3340 RVA: 0x0008588C File Offset: 0x00083A8C
	public event Action OnLoseEvent;

	// Token: 0x14000038 RID: 56
	// (add) Token: 0x06000D0D RID: 3341 RVA: 0x000858C4 File Offset: 0x00083AC4
	// (remove) Token: 0x06000D0E RID: 3342 RVA: 0x000858FC File Offset: 0x00083AFC
	public event Action OnPreLoseEvent;

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x06000D0F RID: 3343 RVA: 0x00085934 File Offset: 0x00083B34
	// (remove) Token: 0x06000D10 RID: 3344 RVA: 0x0008596C File Offset: 0x00083B6C
	public event Action OnTransitionInCompleteEvent;

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x06000D11 RID: 3345 RVA: 0x000859A4 File Offset: 0x00083BA4
	// (remove) Token: 0x06000D12 RID: 3346 RVA: 0x000859DC File Offset: 0x00083BDC
	public event Action OnIntroEvent;

	// Token: 0x1400003B RID: 59
	// (add) Token: 0x06000D13 RID: 3347 RVA: 0x00085A14 File Offset: 0x00083C14
	// (remove) Token: 0x06000D14 RID: 3348 RVA: 0x00085A4C File Offset: 0x00083C4C
	public event Action OnBossDeathExplosionsEvent;

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x06000D15 RID: 3349 RVA: 0x00085A84 File Offset: 0x00083C84
	// (remove) Token: 0x06000D16 RID: 3350 RVA: 0x00085ABC File Offset: 0x00083CBC
	public event Action OnBossDeathExplosionsEndEvent;

	// Token: 0x1400003D RID: 61
	// (add) Token: 0x06000D17 RID: 3351 RVA: 0x00085AF4 File Offset: 0x00083CF4
	// (remove) Token: 0x06000D18 RID: 3352 RVA: 0x00085B2C File Offset: 0x00083D2C
	public event Action OnBossDeathExplosionsFalloffEvent;

	// Token: 0x06000D19 RID: 3353 RVA: 0x00085B64 File Offset: 0x00083D64
	public void _OnLevelStart()
	{
		this.OnLevelStart();
		if (this.OnLevelStartEvent != null)
		{
			this.OnLevelStartEvent();
		}
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		InterruptingPrompt.SetCanInterrupt(true);
		PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(this.CurrentLevel);
		if (levelData != null && !Level.IsTowerOfPower)
		{
			levelData.played = true;
		}
	}

	// Token: 0x06000D1A RID: 3354 RVA: 0x0000B450 File Offset: 0x00009650
	public void _OnLevelEnd()
	{
		this.Ending = true;
		this.OnLevelEnd();
		if (this.OnLevelEndEvent != null)
		{
			this.OnLevelEndEvent();
		}
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		PlayerManager.ClearJoinPrompt();
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x0000B482 File Offset: 0x00009682
	public void zHack_OnStateChanged()
	{
		this.OnStateChanged();
		if (this.OnStateChangedEvent != null)
		{
			this.OnStateChangedEvent();
		}
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x00085BC4 File Offset: 0x00083DC4
	public void zHack_OnWin()
	{
		PlayerManager.playerWasChalice[0] = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice;
		PlayerManager.playerWasChalice[1] = (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice);
		this.CheckPlayerCharacters();
		Level.Won = true;
		Level.Difficulty = this.mode;
		PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(this.CurrentLevel);
		Level.ScoringData.finalHP = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.Health;
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			Level.ScoringData.finalHP = Mathf.Max(Level.ScoringData.finalHP, PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.Health);
		}
		Level.ScoringData.finalHP = Mathf.Min(Level.ScoringData.finalHP, (int)Cuphead.Current.ScoringProperties.hitsForNoScore);
		Level.ScoringData.usedDjimmi = (PlayerData.Data.DjimmiActivatedCurrentRegion() && this.AllowDjimmi() && this.mode != Level.Mode.Hard);
		if (Level.ScoringData.usedDjimmi && (!Level.IsDicePalace || Level.IsDicePalaceMain))
		{
			PlayerData.Data.DeactivateDjimmi();
		}
		if (!Level.IsTowerOfPower)
		{
			levelData.completed = true;
			if (PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).charm == Charm.charm_chalice)
			{
				levelData.completedAsChaliceP1 = true;
			}
			if (PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm == Charm.charm_chalice)
			{
				levelData.completedAsChaliceP2 = true;
			}
		}
		Level.ScoringData.time += this.LevelTime;
		if ((this.type == Level.Type.Battle || this.type == Level.Type.Platforming) && (!Level.IsDicePalace || Level.IsDicePalaceMain))
		{
			Level.Grade = Level.ScoringData.CalculateGrade();
			float time = Level.ScoringData.time;
			if (!Level.IsTowerOfPower)
			{
				if (Level.Difficulty > Level.PreviousDifficulty || !Level.PreviouslyWon)
				{
					levelData.difficultyBeaten = Level.Difficulty;
				}
				if (Level.Grade > Level.PreviousGrade || !Level.PreviouslyWon)
				{
					levelData.grade = Level.Grade;
					levelData.bestTime = time;
				}
				else if (Level.Grade == Level.PreviousGrade && time < levelData.bestTime)
				{
					levelData.bestTime = time;
				}
				if (this.CurrentLevel == Levels.Devil)
				{
					PlayerData.Data.IsHardModeAvailable = true;
				}
				if (this.CurrentLevel == Levels.Saltbaker)
				{
					PlayerData.Data.IsHardModeAvailableDLC = true;
				}
			}
		}
		if (Level.IsChessBoss)
		{
			if (PlayerData.Data.currentChessBossZone != MapCastleZones.Zone.None)
			{
				MapCastleZones.Zone currentChessBossZone = PlayerData.Data.currentChessBossZone;
				PlayerData.Data.currentChessBossZone = MapCastleZones.Zone.None;
				List<MapCastleZones.Zone> usedChessBossZones = PlayerData.Data.usedChessBossZones;
				if (!usedChessBossZones.Contains(currentChessBossZone))
				{
					usedChessBossZones.Add(currentChessBossZone);
				}
			}
			string[] array;
			if (ChessCastleLevel.Coins.TryGetValue(this.CurrentLevel, out array))
			{
				foreach (string coinID in array)
				{
					if (!PlayerData.Data.coinManager.GetCoinCollected(coinID))
					{
						PlayerData.Data.coinManager.SetCoinValue(coinID, true, PlayerId.Any);
						PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1);
						PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1);
					}
				}
			}
		}
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		if (Level.Difficulty != Level.Mode.Easy && player != null && player.stats.Loadout.charm == Charm.charm_curse && CharmCurse.CalculateLevel(PlayerId.PlayerOne) >= 0)
		{
			levelData.curseCharmP1 = true;
		}
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (Level.Difficulty != Level.Mode.Easy && player2 != null && player2.stats.Loadout.charm == Charm.charm_curse && CharmCurse.CalculateLevel(PlayerId.PlayerTwo) >= 0)
		{
			levelData.curseCharmP2 = true;
		}
		this._OnLevelEnd();
		this._OnPreWin();
		if (this.LevelType == Level.Type.Battle)
		{
			base.StartCoroutine(this.bossDeath_cr());
		}
		this.OnWin();
		if (this.OnWinEvent != null)
		{
			this.OnWinEvent();
		}
		if (!Level.IsTowerOfPower)
		{
			PlayerData.SaveCurrentFile();
		}
		if (!Level.IsTowerOfPower)
		{
			Levels[] levels = null;
			Levels[] levels2 = null;
			string str = null;
			Scenes currentMap = PlayerData.Data.CurrentMap;
			bool flag = Array.Exists<Levels>(Level.kingOfGamesLevels, (Levels level) => this.CurrentLevel == level);
			switch (currentMap)
			{
			case Scenes.scene_map_world_1:
				levels2 = (levels = Level.world1BossLevels);
				str = "World1";
				break;
			case Scenes.scene_map_world_2:
				levels2 = (levels = Level.world2BossLevels);
				str = "World2";
				break;
			case Scenes.scene_map_world_3:
				levels2 = (levels = Level.world3BossLevels);
				str = "World3";
				break;
			case Scenes.scene_map_world_4:
				levels2 = (levels = Level.world4BossLevels);
				str = "World4";
				break;
			default:
				if (currentMap == Scenes.scene_map_world_DLC)
				{
					levels = Level.worldDLCBossLevels;
					levels2 = Level.worldDLCBossLevelsWithSaltbaker;
					str = "WorldDLC";
				}
				break;
			}
			if (currentMap == Scenes.scene_map_world_4)
			{
				if (this.CurrentLevel == Levels.DicePalaceMain)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "CompleteDicePalace");
				}
				else if (this.CurrentLevel == Levels.Devil)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "CompleteDevil");
				}
			}
			else if (this.type == Level.Type.Battle && PlayerData.Data.CheckLevelsCompleted(levels))
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "Complete" + str);
			}
			if (this.CurrentLevel == Levels.Saltbaker)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatSaltbaker);
			}
			if (this.type == Level.Type.Battle && Level.Difficulty == Level.Mode.Hard && PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world1BossLevels, Level.Mode.Hard) && PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world2BossLevels, Level.Mode.Hard) && PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world3BossLevels, Level.Mode.Hard) && PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world4BossLevels, Level.Mode.Hard))
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "NewGamePlus");
			}
			if (this.type == Level.Type.Battle && Level.Grade >= LevelScoringData.Grade.AMinus && PlayerData.Data.CheckLevelsHaveMinGrade(levels2, LevelScoringData.Grade.AMinus))
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "ARank" + str);
			}
			if (this.type == Level.Type.Platforming && !this.isMausoleum && Level.ScoringData.pacifistRun && PlayerData.Data.CheckLevelsHaveMinGrade(Level.platformingLevels, LevelScoringData.Grade.P))
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "PacifistRun");
			}
			if ((this.type == Level.Type.Battle || this.type == Level.Type.Platforming) && (!Level.IsDicePalace || Level.IsDicePalaceMain) && !this.isMausoleum && !flag && Level.ScoringData.numTimesHit == 0)
			{
				if (Level.IsDicePalaceMain)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "NoHitsTakenDicePalace");
				}
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "NoHitsTaken");
			}
			if (this.type == Level.Type.Battle)
			{
				if (DamageDealer.lastPlayerDamageSource == DamageDealer.DamageSource.Super)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(DamageDealer.lastPlayer, "SuperWin");
					AbstractPlayerController abstractPlayerController = (DamageDealer.lastPlayer != PlayerId.PlayerOne) ? player2 : player;
					if (abstractPlayerController != null && abstractPlayerController.stats.isChalice)
					{
						OnlineManager.Instance.Interface.UnlockAchievement(DamageDealer.lastPlayer, OnlineAchievementData.DLC.ChaliceSuperWin);
					}
				}
				if (DamageDealer.lastPlayerDamageSource == DamageDealer.DamageSource.Ex)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(DamageDealer.lastPlayer, "ExWin");
				}
				if (this.playerMode == PlayerMode.Plane && !DamageDealer.didDamageWithNonSmallPlaneWeapon)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "SmallPlaneOnlyWin");
				}
				if (DamageDealer.lastDamageWasDLCWeapon)
				{
					OnlineManager.Instance.Interface.UnlockAchievement(DamageDealer.lastPlayer, OnlineAchievementData.DLC.DefeatBossDLCWeapon);
				}
				if (player != null && player.stats.Loadout.charm == Charm.charm_curse && CharmCurse.IsMaxLevel(PlayerId.PlayerOne))
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerOne, OnlineAchievementData.DLC.Paladin);
				}
				if (player2 != null && player2.stats.Loadout.charm == Charm.charm_curse && CharmCurse.IsMaxLevel(PlayerId.PlayerTwo))
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerTwo, OnlineAchievementData.DLC.Paladin);
				}
			}
			int num = 0;
			int num2 = 0;
			List<Levels> list = new List<Levels>(Level.world1BossLevels);
			list.AddRange(Level.world2BossLevels);
			list.AddRange(Level.world3BossLevels);
			foreach (Levels levelID in list)
			{
				PlayerData.PlayerLevelDataObject levelData2 = PlayerData.Data.GetLevelData(levelID);
				if (levelData2.completed && levelData2.difficultyBeaten >= Level.Mode.Normal)
				{
					num2++;
				}
			}
			List<Levels> list2 = new List<Levels>(Level.world1BossLevels);
			list2.AddRange(Level.world2BossLevels);
			list2.AddRange(Level.world3BossLevels);
			list2.AddRange(Level.world4BossLevels);
			list2.AddRange(Level.platformingLevels);
			foreach (Levels levelID2 in list2)
			{
				PlayerData.PlayerLevelDataObject levelData3 = PlayerData.Data.GetLevelData(levelID2);
				if (levelData3.completed && levelData3.grade >= LevelScoringData.Grade.AMinus)
				{
					num++;
				}
			}
			if (this.type == Level.Type.Battle && this.CurrentLevel != Levels.Mausoleum)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "DefeatBoss");
			}
			if (this.type == Level.Type.Battle && Array.Exists<Levels>(Level.chaliceLevels, (Levels level) => this.CurrentLevel == level))
			{
				bool flag2 = false;
				if (player != null && player.stats.isChalice)
				{
					flag2 = true;
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerOne, OnlineAchievementData.DLC.DefeatBossAsChalice);
				}
				if (player2 != null && player2.stats.isChalice)
				{
					flag2 = true;
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerTwo, OnlineAchievementData.DLC.DefeatBossAsChalice);
				}
				if (flag2)
				{
					if (PlayerData.Data.CountLevelsChaliceCompleted(Level.chaliceLevels, PlayerId.PlayerOne) >= OnlineAchievementData.DLC.Triggers.DefeatXBossesAsChaliceTrigger)
					{
						OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerOne, OnlineAchievementData.DLC.DefeatXBossesAsChalice);
					}
					if (PlayerData.Data.CountLevelsChaliceCompleted(Level.chaliceLevels, PlayerId.PlayerTwo) >= OnlineAchievementData.DLC.Triggers.DefeatXBossesAsChaliceTrigger)
					{
						OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerTwo, OnlineAchievementData.DLC.DefeatXBossesAsChalice);
					}
				}
			}
			if (this.type == Level.Type.Battle && this.CurrentLevel == Levels.Graveyard)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatDevilPhase2);
			}
			if (Level.Grade == LevelScoringData.Grade.S && this.CurrentLevel != Levels.Mausoleum && !flag)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "SRank");
				if (Array.Exists<Levels>(Level.worldDLCBossLevelsWithSaltbaker, (Levels level) => this.CurrentLevel == level))
				{
					OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.SRankAnyDLC);
				}
			}
			if (Array.Exists<Levels>(Level.worldDLCBossLevels, (Levels level) => this.CurrentLevel == level) && !this.defeatedMinion)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatBossNoMinions);
			}
			if (flag && PlayerData.Data.CheckLevelsCompleted(Level.kingOfGamesLevels))
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatAllKOG);
			}
			OnlineManager.Instance.Interface.SetStat(PlayerId.Any, "ARanks", num);
			OnlineManager.Instance.Interface.SetStat(PlayerId.Any, "BossesDefeatedNormal", num2);
			OnlineManager.Instance.Interface.SyncAchievementsAndStats();
		}
		if (!this.isMausoleum)
		{
			InterruptingPrompt.SetCanInterrupt(false);
		}
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x0000B4A0 File Offset: 0x000096A0
	public void _OnPreWin()
	{
		this.OnPreWin();
		if (this.OnPreWinEvent != null)
		{
			this.OnPreWinEvent();
		}
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x000868EC File Offset: 0x00084AEC
	public void _OnLose()
	{
		this._OnLevelEnd();
		this._OnPreLose();
		this.OnLose();
		if (this.OnLoseEvent != null)
		{
			this.OnLoseEvent();
		}
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		LevelEnd.Lose(this.isMausoleum, this.secretTriggered);
		if (!Level.IsTowerOfPower)
		{
			PlayerData.SaveCurrentFile();
		}
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x0000B4BE File Offset: 0x000096BE
	public void _OnPreLose()
	{
		this.OnPreLose();
		if (this.OnPreLoseEvent != null)
		{
			this.OnPreLoseEvent();
		}
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x0000B4DC File Offset: 0x000096DC
	public void _OnTransitionInComplete()
	{
		this.OnTransitionInComplete();
		if (this.OnTransitionInCompleteEvent != null)
		{
			this.OnTransitionInCompleteEvent();
		}
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x0000B4FA File Offset: 0x000096FA
	public void OnStartExplosions()
	{
		if (this.OnBossDeathExplosionsEvent != null)
		{
			this.OnBossDeathExplosionsEvent();
		}
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x0000B512 File Offset: 0x00009712
	public void OnEndExplosions()
	{
		if (this.OnBossDeathExplosionsEndEvent != null)
		{
			this.OnBossDeathExplosionsEndEvent();
		}
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x0000B52A File Offset: 0x0000972A
	public void OnFalloffExplosions()
	{
		if (this.OnBossDeathExplosionsFalloffEvent != null)
		{
			this.OnBossDeathExplosionsFalloffEvent();
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06000D24 RID: 3364 RVA: 0x0000B542 File Offset: 0x00009742
	public virtual float LevelIntroTime
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06000D25 RID: 3365 RVA: 0x0000B549 File Offset: 0x00009749
	public virtual float BossDeathTime
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x06000D26 RID: 3366 RVA: 0x0000B550 File Offset: 0x00009750
	public virtual void PlayAnnouncerReady()
	{
		if (!this.isMausoleum)
		{
			AudioManager.Play("level_announcer_ready");
		}
		else
		{
			AudioManager.Play("level_announcer_opening_line");
		}
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x0000B576 File Offset: 0x00009776
	public virtual void PlayAnnouncerBegin()
	{
		AudioManager.Play("level_announcer_begin");
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x0000B582 File Offset: 0x00009782
	public virtual LevelIntroAnimation CreateLevelIntro(Action callback)
	{
		return LevelIntroAnimation.Create(callback);
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x0000B58A File Offset: 0x0000978A
	public virtual void OnLevelStart()
	{
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x0000B58C File Offset: 0x0000978C
	public virtual void OnStateChanged()
	{
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x0000B58E File Offset: 0x0000978E
	public virtual void OnWin()
	{
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x0000B590 File Offset: 0x00009790
	public virtual void OnPreWin()
	{
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x0000B592 File Offset: 0x00009792
	public virtual void OnLose()
	{
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x0000B594 File Offset: 0x00009794
	public virtual void OnPreLose()
	{
	}

	// Token: 0x06000D2F RID: 3375 RVA: 0x0000B596 File Offset: 0x00009796
	public virtual void OnTransitionInComplete()
	{
	}

	// Token: 0x06000D30 RID: 3376 RVA: 0x0008694C File Offset: 0x00084B4C
	public virtual IEnumerator knockoutSFX_cr()
	{
		if (!this.isMausoleum)
		{
			AudioManager.Play("level_announcer_knockout_bell");
			AudioManager.Play("level_announcer_knockout");
			yield return CupheadTime.WaitForSeconds(this, 1.4f);
			if (!Level.IsChessBoss && this.CurrentLevel != Levels.Saltbaker && this.CurrentLevel != Levels.Graveyard)
			{
				AudioManager.Play("level_boss_defeat_sting");
			}
		}
		else
		{
			AudioManager.Play("level_announcer_victory");
		}
		yield break;
	}

	// Token: 0x06000D31 RID: 3377 RVA: 0x0000B598 File Offset: 0x00009798
	public virtual void OnBossDeath()
	{
	}

	// Token: 0x06000D32 RID: 3378 RVA: 0x00086968 File Offset: 0x00084B68
	public IEnumerator check_intros_cr()
	{
		yield return new WaitForSeconds(0.25f);
		this.CheckIntros();
		yield return null;
		yield break;
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x00086984 File Offset: 0x00084B84
	public virtual IEnumerator startBattle_cr()
	{
		LevelIntroAnimation introAnim = this.CreateLevelIntro(new Action(this.intro.OnReadyAnimComplete));
		yield return new WaitForSeconds(0.4f + SceneLoader.EndTransitionDelay);
		if (!Level.IsDicePalaceMain && !Level.IsTowerOfPowerMain)
		{
			this.PlayAnnouncerReady();
			AudioManager.Play("level_bell_intro");
		}
		yield return new WaitForSeconds(0.25f);
		if (this.players[0] != null)
		{
			this.players[0].PlayIntro();
		}
		if (this.players[1] != null)
		{
			if (!this.players[1].stats.isChalice)
			{
				yield return CupheadTime.WaitForSeconds(this, 0.7f);
			}
			this.players[1].PlayIntro();
		}
		yield return new WaitForSeconds(0.25f);
		this._OnTransitionInComplete();
		if (this.OnIntroEvent != null)
		{
			this.OnIntroEvent();
		}
		this.OnIntroEvent = null;
		yield return new WaitForSeconds(this.LevelIntroTime);
		if (!Level.IsDicePalaceMain && !Level.IsTowerOfPowerMain)
		{
			introAnim.Play();
			while (!this.intro.readyComplete)
			{
				yield return null;
			}
			this.PlayAnnouncerBegin();
		}
		else if (!Level.IsTowerOfPowerMain)
		{
			yield return CupheadTime.WaitForSeconds(this, 1.5f);
		}
		foreach (AbstractPlayerController abstractPlayerController in this.players)
		{
			if (!(abstractPlayerController == null))
			{
				abstractPlayerController.LevelStart();
			}
		}
		this.Started = true;
		this._OnLevelStart();
		yield break;
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x000869A0 File Offset: 0x00084BA0
	public virtual IEnumerator startPlatforming_cr()
	{
		PlatformingLevelIntroAnimation introAnim = PlatformingLevelIntroAnimation.Create(new Action(this.intro.OnReadyAnimComplete));
		yield return new WaitForEndOfFrame();
		if (this.players[0] != null)
		{
			this.players[0].OnPlatformingLevelAwake();
		}
		if (this.players[1] != null)
		{
			this.players[1].OnPlatformingLevelAwake();
		}
		yield return new WaitForSeconds(0.4f + SceneLoader.EndTransitionDelay);
		this._OnTransitionInComplete();
		if (this.OnIntroEvent != null)
		{
			this.OnIntroEvent();
		}
		this.OnIntroEvent = null;
		introAnim.Play();
		AudioManager.Play("level_announcer_begin");
		while (!this.intro.readyComplete)
		{
			yield return null;
		}
		foreach (AbstractPlayerController abstractPlayerController in this.players)
		{
			if (!(abstractPlayerController == null))
			{
				abstractPlayerController.LevelStart();
			}
		}
		this.Started = true;
		this._OnLevelStart();
		yield break;
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x000869BC File Offset: 0x00084BBC
	public virtual IEnumerator startNonBattle_cr()
	{
		yield return new WaitForSeconds(0.4f + SceneLoader.EndTransitionDelay - 0.25f);
		if (this.playerMode == PlayerMode.Plane)
		{
			yield return new WaitForSeconds(0.5f);
			if (this.players[0] != null)
			{
				this.players[0].PlayIntro();
			}
			if (this.players[1] != null)
			{
				yield return CupheadTime.WaitForSeconds(this, 0.7f);
				this.players[1].PlayIntro();
			}
			yield return new WaitForSeconds(0.25f);
		}
		this._OnTransitionInComplete();
		if (this.OnIntroEvent != null)
		{
			this.OnIntroEvent();
		}
		this.OnIntroEvent = null;
		if (this.playerMode == PlayerMode.Plane)
		{
			yield return new WaitForSeconds(1.25f);
		}
		foreach (AbstractPlayerController abstractPlayerController in this.players)
		{
			if (!(abstractPlayerController == null))
			{
				abstractPlayerController.LevelStart();
			}
		}
		this.Started = true;
		this._OnLevelStart();
		yield break;
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x000869D8 File Offset: 0x00084BD8
	public virtual IEnumerator bossDeath_cr()
	{
		LevelEnd.Win(this.knockoutSFX_cr(), new Action(this.OnBossDeath), new Action(this.OnStartExplosions), new Action(this.OnFalloffExplosions), new Action(this.OnEndExplosions), this.players, this.BossDeathTime, (this.type == Level.Type.Battle || this.type == Level.Type.Platforming) && (!Level.IsDicePalace || Level.IsDicePalaceMain) && !Level.IsTowerOfPower && !this.isMausoleum && !Level.IsGraveyard && !Level.IsChessBoss, this.isMausoleum, this.isDevil, this.isTowerOfPower);
		yield return null;
		yield break;
	}

	// Token: 0x04000A03 RID: 2563
	public const int BOUND_COLLIDER_SIZE = 400;

	// Token: 0x04000A04 RID: 2564
	public const float IRIS_NO_INTRO_DELAY = 0.4f;

	// Token: 0x04000A05 RID: 2565
	public const float IRIS_OPEN_DELAY = 1f;

	// Token: 0x04000A06 RID: 2566
	public const int PLAYER_DEATH_DELAY = 5;

	// Token: 0x04000A07 RID: 2567
	public const string GENERIC_STATE_NAME = "Generic";

	// Token: 0x04000A1B RID: 2587
	public static readonly Levels[] world1BossLevels = new Levels[]
	{
		Levels.Veggies,
		Levels.Slime,
		Levels.FlyingBlimp,
		Levels.Flower,
		Levels.Frogs
	};

	// Token: 0x04000A1C RID: 2588
	public static readonly Levels[] world2BossLevels = new Levels[]
	{
		Levels.Baroness,
		Levels.Clown,
		Levels.FlyingGenie,
		Levels.Dragon,
		Levels.FlyingBird
	};

	// Token: 0x04000A1D RID: 2589
	public static readonly Levels[] world3BossLevels = new Levels[]
	{
		Levels.Bee,
		Levels.Pirate,
		Levels.SallyStagePlay,
		Levels.Mouse,
		Levels.Robot,
		Levels.FlyingMermaid,
		Levels.Train
	};

	// Token: 0x04000A1E RID: 2590
	public static readonly Levels[] world4BossLevels = new Levels[]
	{
		Levels.DicePalaceMain,
		Levels.Devil
	};

	// Token: 0x04000A1F RID: 2591
	public static readonly Levels[] world4MiniBossLevels = new Levels[]
	{
		Levels.DicePalaceBooze,
		Levels.DicePalaceChips,
		Levels.DicePalaceCigar,
		Levels.DicePalaceDomino,
		Levels.DicePalaceEightBall,
		Levels.DicePalaceFlyingHorse,
		Levels.DicePalaceFlyingMemory,
		Levels.DicePalaceRabbit,
		Levels.DicePalaceRoulette
	};

	// Token: 0x04000A20 RID: 2592
	public static readonly Levels[] worldDLCBossLevels = new Levels[]
	{
		Levels.Airplane,
		Levels.FlyingCowboy,
		Levels.OldMan,
		Levels.RumRunners,
		Levels.SnowCult
	};

	// Token: 0x04000A21 RID: 2593
	public static readonly Levels[] worldDLCBossLevelsWithSaltbaker = new Levels[]
	{
		Levels.Airplane,
		Levels.FlyingCowboy,
		Levels.OldMan,
		Levels.RumRunners,
		Levels.SnowCult,
		Levels.Saltbaker
	};

	// Token: 0x04000A22 RID: 2594
	public static readonly Levels[] platformingLevels = new Levels[]
	{
		Levels.Platforming_Level_1_1,
		Levels.Platforming_Level_1_2,
		Levels.Platforming_Level_2_1,
		Levels.Platforming_Level_2_2,
		Levels.Platforming_Level_3_1,
		Levels.Platforming_Level_3_2
	};

	// Token: 0x04000A23 RID: 2595
	public static readonly Levels[] kingOfGamesLevels = new Levels[]
	{
		Levels.ChessPawn,
		Levels.ChessKnight,
		Levels.ChessBishop,
		Levels.ChessRook,
		Levels.ChessQueen
	};

	// Token: 0x04000A24 RID: 2596
	public static readonly Levels[] kingOfGamesLevelsWithCastle = new Levels[]
	{
		Levels.ChessPawn,
		Levels.ChessKnight,
		Levels.ChessBishop,
		Levels.ChessRook,
		Levels.ChessQueen,
		Levels.ChessCastle
	};

	// Token: 0x04000A25 RID: 2597
	public static readonly Levels[] chaliceLevels = new Levels[]
	{
		Levels.Veggies,
		Levels.Slime,
		Levels.FlyingBlimp,
		Levels.Flower,
		Levels.Frogs,
		Levels.Baroness,
		Levels.Clown,
		Levels.FlyingGenie,
		Levels.Dragon,
		Levels.FlyingBird,
		Levels.Bee,
		Levels.Pirate,
		Levels.SallyStagePlay,
		Levels.Mouse,
		Levels.Robot,
		Levels.FlyingMermaid,
		Levels.Train,
		Levels.DicePalaceMain,
		Levels.Devil,
		Levels.Airplane,
		Levels.FlyingCowboy,
		Levels.OldMan,
		Levels.RumRunners,
		Levels.SnowCult,
		Levels.Saltbaker
	};

	// Token: 0x04000A26 RID: 2598
	public LevelResources LevelResources;

	// Token: 0x04000A27 RID: 2599
	[SerializeField]
	public Level.Type type;

	// Token: 0x04000A28 RID: 2600
	[SerializeField]
	public PlayerMode playerMode;

	// Token: 0x04000A29 RID: 2601
	[SerializeField]
	public bool allowMultiplayer = true;

	// Token: 0x04000A2A RID: 2602
	[SerializeField]
	public bool blockChalice;

	// Token: 0x04000A2C RID: 2604
	[SerializeField]
	public Level.IntroProperties intro;

	// Token: 0x04000A2D RID: 2605
	[SerializeField]
	public Level.Spawns spawns;

	// Token: 0x04000A2E RID: 2606
	[SerializeField]
	public Level.Bounds bounds = new Level.Bounds(640, 640, 360, 200);

	// Token: 0x04000A2F RID: 2607
	public int playerShadowSortingOrder;

	// Token: 0x04000A30 RID: 2608
	[SerializeField]
	public Level.Camera camera = new Level.Camera(CupheadLevelCamera.Mode.Lerp, 640, 640, 360, 360);

	// Token: 0x04000A31 RID: 2609
	public LevelGUI gui;

	// Token: 0x04000A32 RID: 2610
	public LevelHUD hud;

	// Token: 0x04000A33 RID: 2611
	public AbstractPlayerController[] players;

	// Token: 0x04000A34 RID: 2612
	public Transform collidersRoot;

	// Token: 0x04000A35 RID: 2613
	public Level.GoalTimes goalTimes;

	// Token: 0x04000A36 RID: 2614
	public bool waitingForPlayerJoin;

	// Token: 0x04000A37 RID: 2615
	public bool isMausoleum;

	// Token: 0x04000A38 RID: 2616
	public bool isDevil;

	// Token: 0x04000A39 RID: 2617
	public bool isTowerOfPower;

	// Token: 0x04000A3A RID: 2618
	public bool secretTriggered;

	// Token: 0x04000A44 RID: 2628
	public int BGMPlaylistCurrent;

	// Token: 0x04000A45 RID: 2629
	public readonly Vector3 player1PlaneSpawnPos = new Vector3(-550f, 74.3f);

	// Token: 0x04000A46 RID: 2630
	public readonly Vector3 player2PlaneSpawnPos = new Vector3(-450f, -79.8f);

	// Token: 0x04000A47 RID: 2631
	public int playerDeathDelayFrames;

	// Token: 0x04000A48 RID: 2632
	public bool playerIsDead;

	// Token: 0x04000A49 RID: 2633
	public bool player1HeldJump;

	// Token: 0x04000A4A RID: 2634
	public bool player2HeldJump;

	// Token: 0x04000A4B RID: 2635
	public bool player1HeldSuper;

	// Token: 0x04000A4C RID: 2636
	public bool player2HeldSuper;

	// Token: 0x0200098D RID: 2445
	public enum Type
	{
		// Token: 0x0400474A RID: 18250
		Battle,
		// Token: 0x0400474B RID: 18251
		Tutorial,
		// Token: 0x0400474C RID: 18252
		Platforming
	}

	// Token: 0x0200098E RID: 2446
	public enum Mode
	{
		// Token: 0x0400474E RID: 18254
		Easy,
		// Token: 0x0400474F RID: 18255
		Normal,
		// Token: 0x04004750 RID: 18256
		Hard
	}

	// Token: 0x0200098F RID: 2447
	[Serializable]
	public class Bounds
	{
		// Token: 0x0600557A RID: 21882 RVA: 0x001C6B84 File Offset: 0x001C4D84
		public Bounds()
		{
			this.left = 0;
			this.right = 0;
			this.top = 0;
			this.bottom = 0;
		}

		// Token: 0x0600557B RID: 21883 RVA: 0x001C6BDC File Offset: 0x001C4DDC
		public Bounds(int left, int right, int top, int bottom)
		{
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}

		// Token: 0x0600557C RID: 21884 RVA: 0x001C6C34 File Offset: 0x001C4E34
		public void SetColliderPositions()
		{
			Rect rect = default(Rect);
			rect.xMin = (float)(-(float)this.left);
			rect.xMax = (float)this.right;
			rect.yMin = (float)(-(float)this.bottom);
			rect.yMax = (float)this.top;
			if (this.colliders.ContainsKey(Level.Bounds.Side.Left) && this.colliders[Level.Bounds.Side.Left] != null)
			{
				this.colliders[Level.Bounds.Side.Left].transform.position = new Vector2((float)(-(float)this.left - 200), rect.center.y);
			}
			if (this.colliders.ContainsKey(Level.Bounds.Side.Right) && this.colliders[Level.Bounds.Side.Right] != null)
			{
				this.colliders[Level.Bounds.Side.Right].transform.position = new Vector2((float)(this.right + 200), rect.center.y);
			}
			if (this.colliders.ContainsKey(Level.Bounds.Side.Top) && this.colliders[Level.Bounds.Side.Top] != null)
			{
				this.colliders[Level.Bounds.Side.Top].transform.position = new Vector2(rect.center.x, (float)(this.top + 200));
			}
			if (this.colliders.ContainsKey(Level.Bounds.Side.Bottom) && this.colliders[Level.Bounds.Side.Bottom] != null)
			{
				this.colliders[Level.Bounds.Side.Bottom].transform.position = new Vector2(rect.center.x, (float)(-(float)this.bottom - 200));
			}
		}

		// Token: 0x0600557D RID: 21885 RVA: 0x00040853 File Offset: 0x0003EA53
		public int GetValue(Level.Bounds.Side side)
		{
			switch (side)
			{
			case Level.Bounds.Side.Left:
				return this.left;
			case Level.Bounds.Side.Right:
				return this.right;
			case Level.Bounds.Side.Top:
				return this.top;
			default:
				return this.bottom;
			}
		}

		// Token: 0x0600557E RID: 21886 RVA: 0x001C6E18 File Offset: 0x001C5018
		public void SetValue(Level.Bounds.Side side, int value)
		{
			switch (side)
			{
			case Level.Bounds.Side.Left:
				this.left = value;
				break;
			case Level.Bounds.Side.Right:
				this.right = value;
				break;
			case Level.Bounds.Side.Top:
				this.top = value;
				break;
			default:
				this.bottom = value;
				break;
			}
		}

		// Token: 0x0600557F RID: 21887 RVA: 0x0004088B File Offset: 0x0003EA8B
		public bool GetEnabled(Level.Bounds.Side side)
		{
			switch (side)
			{
			case Level.Bounds.Side.Left:
				return this.leftEnabled;
			case Level.Bounds.Side.Right:
				return this.rightEnabled;
			case Level.Bounds.Side.Top:
				return this.topEnabled;
			default:
				return this.bottomEnabled;
			}
		}

		// Token: 0x06005580 RID: 21888 RVA: 0x001C6E70 File Offset: 0x001C5070
		public void SetEnabled(Level.Bounds.Side side, bool value)
		{
			switch (side)
			{
			case Level.Bounds.Side.Left:
				this.leftEnabled = value;
				break;
			case Level.Bounds.Side.Right:
				this.rightEnabled = value;
				break;
			case Level.Bounds.Side.Top:
				this.topEnabled = value;
				break;
			default:
				this.bottomEnabled = value;
				break;
			}
		}

		// Token: 0x06005581 RID: 21889 RVA: 0x000408C3 File Offset: 0x0003EAC3
		public Level.Bounds Copy()
		{
			return base.MemberwiseClone() as Level.Bounds;
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06005582 RID: 21890 RVA: 0x000408D0 File Offset: 0x0003EAD0
		public int Width
		{
			get
			{
				return this.left + this.right;
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06005583 RID: 21891 RVA: 0x000408DF File Offset: 0x0003EADF
		public int Height
		{
			get
			{
				return this.top + this.bottom;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06005584 RID: 21892 RVA: 0x001C6EC8 File Offset: 0x001C50C8
		public Vector2 Center
		{
			get
			{
				return new Vector2((float)(this.right - this.left), (float)(this.top - this.bottom)) / 2f;
			}
		}

		// Token: 0x04004751 RID: 18257
		public int left;

		// Token: 0x04004752 RID: 18258
		public int right;

		// Token: 0x04004753 RID: 18259
		public int top;

		// Token: 0x04004754 RID: 18260
		public int bottom;

		// Token: 0x04004755 RID: 18261
		public bool topEnabled = true;

		// Token: 0x04004756 RID: 18262
		public bool bottomEnabled = true;

		// Token: 0x04004757 RID: 18263
		public bool leftEnabled = true;

		// Token: 0x04004758 RID: 18264
		public bool rightEnabled = true;

		// Token: 0x04004759 RID: 18265
		public Dictionary<Level.Bounds.Side, BoxCollider2D> colliders = new Dictionary<Level.Bounds.Side, BoxCollider2D>();

		// Token: 0x020015D7 RID: 5591
		public enum Side
		{
			// Token: 0x040091C4 RID: 37316
			Left,
			// Token: 0x040091C5 RID: 37317
			Right,
			// Token: 0x040091C6 RID: 37318
			Top,
			// Token: 0x040091C7 RID: 37319
			Bottom
		}
	}

	// Token: 0x02000990 RID: 2448
	[Serializable]
	public class Spawns
	{
		// Token: 0x17000AC6 RID: 2758
		public Vector2 this[int i]
		{
			get
			{
				if (i == 0)
				{
					return this.playerOne;
				}
				if (i == 1)
				{
					return this.playerTwo;
				}
				if (i == 2)
				{
					return this.playerOneSingle;
				}
				Debug.LogError("Spawn index '" + i + "' not in range", null);
				return Vector2.zero;
			}
		}

		// Token: 0x0400475A RID: 18266
		public Vector2 playerOne = new Vector2(-460f, 0f);

		// Token: 0x0400475B RID: 18267
		public Vector2 playerTwo = new Vector2(-580f, 0f);

		// Token: 0x0400475C RID: 18268
		public Vector2 playerOneSingle = new Vector2(-520f, 0f);
	}

	// Token: 0x02000991 RID: 2449
	[Serializable]
	public class Camera
	{
		// Token: 0x06005587 RID: 21895 RVA: 0x001C6FB0 File Offset: 0x001C51B0
		public Camera(CupheadLevelCamera.Mode mode, int left, int right, int top, int bottom)
		{
			this.mode = mode;
			this.bounds = new Level.Bounds(left, right, top, bottom);
		}

		// Token: 0x0400475D RID: 18269
		public CupheadLevelCamera.Mode mode = CupheadLevelCamera.Mode.Relative;

		// Token: 0x0400475E RID: 18270
		[Space(10f)]
		[Range(0.5f, 2f)]
		public float zoom = 1f;

		// Token: 0x0400475F RID: 18271
		[Space(10f)]
		public bool moveX;

		// Token: 0x04004760 RID: 18272
		public bool moveY;

		// Token: 0x04004761 RID: 18273
		public bool stabilizeY;

		// Token: 0x04004762 RID: 18274
		public float stabilizePaddingTop = 50f;

		// Token: 0x04004763 RID: 18275
		public float stabilizePaddingBottom = 100f;

		// Token: 0x04004764 RID: 18276
		[Space(10f)]
		public bool colliders;

		// Token: 0x04004765 RID: 18277
		[Space(10f)]
		public Level.Bounds bounds;

		// Token: 0x04004766 RID: 18278
		[HideInInspector]
		public VectorPath path;

		// Token: 0x04004767 RID: 18279
		public bool pathMovesOnlyForward;
	}

	// Token: 0x02000992 RID: 2450
	public class GoalTimes
	{
		// Token: 0x06005588 RID: 21896 RVA: 0x000408EE File Offset: 0x0003EAEE
		public GoalTimes(float easy, float normal, float hard)
		{
			this.easy = easy;
			this.normal = normal;
			this.hard = hard;
		}

		// Token: 0x04004768 RID: 18280
		public readonly float easy;

		// Token: 0x04004769 RID: 18281
		public readonly float normal;

		// Token: 0x0400476A RID: 18282
		public readonly float hard;
	}

	// Token: 0x02000993 RID: 2451
	[Serializable]
	public class IntroProperties
	{
		// Token: 0x0600558A RID: 21898 RVA: 0x00040913 File Offset: 0x0003EB13
		public void OnIntroAnimComplete()
		{
			this.introComplete = true;
		}

		// Token: 0x0600558B RID: 21899 RVA: 0x0004091C File Offset: 0x0003EB1C
		public void OnReadyAnimComplete()
		{
			this.readyComplete = true;
		}

		// Token: 0x0400476B RID: 18283
		[NonSerialized]
		public bool introComplete;

		// Token: 0x0400476C RID: 18284
		[NonSerialized]
		public bool readyComplete;
	}

	// Token: 0x02000994 RID: 2452
	public class Timeline
	{
		// Token: 0x0600558C RID: 21900 RVA: 0x00040925 File Offset: 0x0003EB25
		public Timeline()
		{
			this.health = 0f;
			this.damage = 0f;
			this.cuphead = -1f;
			this.mugman = -1f;
			this.events = new List<Level.Timeline.Event>();
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x0600558D RID: 21901 RVA: 0x00040964 File Offset: 0x0003EB64
		// (set) Token: 0x0600558E RID: 21902 RVA: 0x0004096C File Offset: 0x0003EB6C
		public float damage { get; set; }

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x0600558F RID: 21903 RVA: 0x00040975 File Offset: 0x0003EB75
		// (set) Token: 0x06005590 RID: 21904 RVA: 0x0004097D File Offset: 0x0003EB7D
		public List<Level.Timeline.Event> events { get; set; }

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06005591 RID: 21905 RVA: 0x00040986 File Offset: 0x0003EB86
		// (set) Token: 0x06005592 RID: 21906 RVA: 0x0004098E File Offset: 0x0003EB8E
		public float cuphead { get; set; }

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06005593 RID: 21907 RVA: 0x00040997 File Offset: 0x0003EB97
		// (set) Token: 0x06005594 RID: 21908 RVA: 0x0004099F File Offset: 0x0003EB9F
		public float mugman { get; set; }

		// Token: 0x06005595 RID: 21909 RVA: 0x001C7004 File Offset: 0x001C5204
		public int GetHealthOfLastEvent()
		{
			float num = 1f;
			for (int i = 0; i < this.events.Count; i++)
			{
				if (this.events[i].percentage < num)
				{
					num = this.events[i].percentage;
				}
			}
			return (int)(this.health * (1f - num));
		}

		// Token: 0x06005596 RID: 21910 RVA: 0x000409A8 File Offset: 0x0003EBA8
		public void DealDamage(float damage)
		{
			this.damage += damage;
		}

		// Token: 0x06005597 RID: 21911 RVA: 0x001C7074 File Offset: 0x001C5274
		public void OnPlayerDeath(PlayerId playerId)
		{
			if (playerId == PlayerId.PlayerOne || playerId != PlayerId.PlayerTwo)
			{
				if (PlayerManager.player1IsMugman)
				{
					this.mugman = this.damage;
				}
				else
				{
					this.cuphead = this.damage;
				}
			}
			else if (PlayerManager.player1IsMugman)
			{
				this.cuphead = this.damage;
			}
			else
			{
				this.mugman = this.damage;
			}
		}

		// Token: 0x06005598 RID: 21912 RVA: 0x001C70EC File Offset: 0x001C52EC
		public void OnPlayerRevive(PlayerId playerId)
		{
			if (playerId == PlayerId.PlayerOne || playerId != PlayerId.PlayerTwo)
			{
				if (PlayerManager.player1IsMugman)
				{
					this.mugman = -1f;
				}
				else
				{
					this.cuphead = -1f;
				}
			}
			else if (PlayerManager.player1IsMugman)
			{
				this.cuphead = -1f;
			}
			else
			{
				this.mugman = -1f;
			}
		}

		// Token: 0x06005599 RID: 21913 RVA: 0x001C7160 File Offset: 0x001C5360
		public void SetPlayerDamage(PlayerId playerId, float value)
		{
			if (playerId == PlayerId.PlayerOne || playerId != PlayerId.PlayerTwo)
			{
				if (PlayerManager.player1IsMugman)
				{
					this.mugman = value;
				}
				else
				{
					this.cuphead = value;
				}
			}
			else if (PlayerManager.player1IsMugman)
			{
				this.cuphead = value;
			}
			else
			{
				this.mugman = value;
			}
		}

		// Token: 0x0600559A RID: 21914 RVA: 0x000409B8 File Offset: 0x0003EBB8
		public void AddEvent(Level.Timeline.Event e)
		{
			this.events.Add(e);
		}

		// Token: 0x0600559B RID: 21915 RVA: 0x001C71C4 File Offset: 0x001C53C4
		public void AddEventAtHealth(string eventName, int targetHealth)
		{
			float percentage = 1f - (float)targetHealth / this.health;
			this.AddEvent(new Level.Timeline.Event(eventName, percentage));
		}

		// Token: 0x0400476D RID: 18285
		public float health;

		// Token: 0x020015D8 RID: 5592
		public class Event
		{
			// Token: 0x0600875C RID: 34652 RVA: 0x0005BA30 File Offset: 0x00059C30
			public Event(string name, float percentage)
			{
				this.name = name;
				this.percentage = percentage;
			}

			// Token: 0x17001A3A RID: 6714
			// (get) Token: 0x0600875D RID: 34653 RVA: 0x0005BA46 File Offset: 0x00059C46
			// (set) Token: 0x0600875E RID: 34654 RVA: 0x0005BA4E File Offset: 0x00059C4E
			public string name { get; set; }

			// Token: 0x17001A3B RID: 6715
			// (get) Token: 0x0600875F RID: 34655 RVA: 0x0005BA57 File Offset: 0x00059C57
			// (set) Token: 0x06008760 RID: 34656 RVA: 0x0005BA5F File Offset: 0x00059C5F
			public float percentage { get; set; }
		}
	}
}
