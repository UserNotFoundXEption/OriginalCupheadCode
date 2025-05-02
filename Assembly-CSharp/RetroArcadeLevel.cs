using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class RetroArcadeLevel : Level
{
	// Token: 0x0600037B RID: 891 RVA: 0x00066984 File Offset: 0x00064B84
	public override void PartialInit()
	{
		this.properties = LevelProperties.RetroArcade.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x0600037C RID: 892 RVA: 0x00004C36 File Offset: 0x00002E36
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.RetroArcade;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x0600037D RID: 893 RVA: 0x00004C3D File Offset: 0x00002E3D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_retro_arcade;
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x0600037E RID: 894 RVA: 0x00004C41 File Offset: 0x00002E41
	// (set) Token: 0x0600037F RID: 895 RVA: 0x00004C48 File Offset: 0x00002E48
	public static float ACCURACY_BONUS { get; set; }

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000380 RID: 896 RVA: 0x00004C50 File Offset: 0x00002E50
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000381 RID: 897 RVA: 0x00004C58 File Offset: 0x00002E58
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000382 RID: 898 RVA: 0x00066A1C File Offset: 0x00064C1C
	public override void Start()
	{
		base.Start();
		this.alienManager.LevelInit(this.properties);
		this.caterpillarManager.LevelInit(this.properties);
		this.robotManager.LevelInit(this.properties);
		this.paddleShip.LevelInit(this.properties);
		this.qShip.LevelInit(this.properties);
		this.ufo.LevelInit(this.properties);
		this.toadManager.LevelInit(this.properties);
		this.worm.LevelInit(this.properties);
		this.bouncyManager.LevelInit(this.properties);
		this.missileMan.LevelInit(this.properties);
		this.chaserManager.LevelInit(this.properties);
		this.sheriffManager.LevelInit(this.properties);
		this.snakeManager.LevelInit(this.properties);
		this.tentacleManager.LevelInit(this.properties);
		this.trafficManager.LevelInit(this.properties);
		RetroArcadeLevel.ACCURACY_BONUS = this.properties.CurrentState.general.accuracyBonus;
		this.bigCuphead.Init(PlayerManager.GetPlayer(PlayerId.PlayerOne) as ArcadePlayerController);
		this.bigMugman.Init(PlayerManager.GetPlayer(PlayerId.PlayerTwo) as ArcadePlayerController);
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00004C60 File Offset: 0x00002E60
	public override void CreatePlayers()
	{
		base.CreatePlayers();
	}

	// Token: 0x06000384 RID: 900 RVA: 0x00004C68 File Offset: 0x00002E68
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x06000385 RID: 901 RVA: 0x00004C70 File Offset: 0x00002E70
	public override void OnLevelStart()
	{
		this.bigCuphead.LevelStart();
		this.bigMugman.LevelStart();
		this.StartStateCoroutine();
	}

	// Token: 0x06000386 RID: 902 RVA: 0x00004C8E File Offset: 0x00002E8E
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		this.bigCuphead.OnVictory();
		this.bigMugman.OnVictory();
		this.StartStateCoroutine();
	}

	// Token: 0x06000387 RID: 903 RVA: 0x00066B74 File Offset: 0x00064D74
	public void StartStateCoroutine()
	{
		switch (this.properties.CurrentState.stateName)
		{
		case LevelProperties.RetroArcade.States.Main:
		case LevelProperties.RetroArcade.States.MissileMan:
			base.StartCoroutine(this.startMissile_cr());
			break;
		case LevelProperties.RetroArcade.States.Caterpillar:
			base.StartCoroutine(this.startCaterpillars_cr());
			break;
		case LevelProperties.RetroArcade.States.Robots:
			base.StartCoroutine(this.startRobots_cr());
			break;
		case LevelProperties.RetroArcade.States.PaddleShip:
			base.StartCoroutine(this.startPaddleShip_cr());
			break;
		case LevelProperties.RetroArcade.States.QShip:
			base.StartCoroutine(this.startQShip_cr());
			break;
		case LevelProperties.RetroArcade.States.UFO:
			base.StartCoroutine(this.startUFO_cr());
			break;
		case LevelProperties.RetroArcade.States.Toad:
			base.StartCoroutine(this.startToad_cr());
			break;
		case LevelProperties.RetroArcade.States.Worm:
			base.StartCoroutine(this.startWorm_cr());
			break;
		case LevelProperties.RetroArcade.States.Aliens:
			base.StartCoroutine(this.startAliens_cr());
			break;
		case LevelProperties.RetroArcade.States.Bouncy:
			base.StartCoroutine(this.startBouncy_cr());
			break;
		case LevelProperties.RetroArcade.States.Chaser:
			base.StartCoroutine(this.startChaser_cr());
			break;
		case LevelProperties.RetroArcade.States.Sheriff:
			base.StartCoroutine(this.startSheriff_cr());
			break;
		case LevelProperties.RetroArcade.States.Snake:
			base.StartCoroutine(this.startSnake_cr());
			break;
		case LevelProperties.RetroArcade.States.Tentacle:
			base.StartCoroutine(this.startTentacle_cr());
			break;
		case LevelProperties.RetroArcade.States.Traffic:
			base.StartCoroutine(this.startTrafficUFO_cr());
			break;
		case LevelProperties.RetroArcade.States.JetpackTest:
			base.StartCoroutine(this.switchToJetpack_cr());
			break;
		}
	}

	// Token: 0x06000388 RID: 904 RVA: 0x00004CB2 File Offset: 0x00002EB2
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.bigCuphead.OnVictory();
		this.bigMugman.OnVictory();
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00066D08 File Offset: 0x00064F08
	public IEnumerator startAliens_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.alienManager.StartAliens();
		yield break;
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00066D24 File Offset: 0x00064F24
	public IEnumerator startCaterpillars_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.caterpillarManager.StartCaterpillar();
		yield break;
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00066D40 File Offset: 0x00064F40
	public IEnumerator startRobots_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.robotManager.StartRobots();
		yield break;
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00066D5C File Offset: 0x00064F5C
	public IEnumerator startPaddleShip_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.paddleShip.StartPaddleShip();
		yield break;
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00066D78 File Offset: 0x00064F78
	public IEnumerator startQShip_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.qShip.StartQShip();
		yield break;
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00066D94 File Offset: 0x00064F94
	public IEnumerator startUFO_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.ufo.StartUFO();
		yield break;
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00066DB0 File Offset: 0x00064FB0
	public IEnumerator startToad_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.toadManager.StartToad();
		yield break;
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00066DCC File Offset: 0x00064FCC
	public IEnumerator startWorm_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.worm.StartWorm();
		yield break;
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00066DE8 File Offset: 0x00064FE8
	public IEnumerator startBouncy_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.bouncyManager.StartBouncy();
		yield break;
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00066E04 File Offset: 0x00065004
	public IEnumerator startMissile_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.missileMan.StartMissile();
		yield break;
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00066E20 File Offset: 0x00065020
	public IEnumerator switchToRocket_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		player.ChangeToRocket();
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			ArcadePlayerController player2 = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerTwo);
			player2.ChangeToRocket();
		}
		yield break;
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00066E3C File Offset: 0x0006503C
	public IEnumerator startChaser_cr()
	{
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		if (player.controlScheme != ArcadePlayerController.ControlScheme.Rocket)
		{
			yield return base.StartCoroutine(this.switchToRocket_cr());
		}
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.chaserManager.StartChasers();
		yield break;
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00066E58 File Offset: 0x00065058
	public IEnumerator startSheriff_cr()
	{
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		if (player.controlScheme != ArcadePlayerController.ControlScheme.Rocket)
		{
			yield return base.StartCoroutine(this.switchToRocket_cr());
		}
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.sheriffManager.StartSheriff();
		yield break;
	}

	// Token: 0x06000396 RID: 918 RVA: 0x00066E74 File Offset: 0x00065074
	public IEnumerator startSnake_cr()
	{
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		if (player.controlScheme != ArcadePlayerController.ControlScheme.Rocket)
		{
			yield return base.StartCoroutine(this.switchToRocket_cr());
		}
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.snakeManager.StartSnake();
		yield break;
	}

	// Token: 0x06000397 RID: 919 RVA: 0x00066E90 File Offset: 0x00065090
	public IEnumerator startTentacle_cr()
	{
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		if (player.controlScheme != ArcadePlayerController.ControlScheme.Rocket)
		{
			yield return base.StartCoroutine(this.switchToRocket_cr());
		}
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.tentacleManager.StartTentacle();
		yield break;
	}

	// Token: 0x06000398 RID: 920 RVA: 0x00066EAC File Offset: 0x000650AC
	public IEnumerator startTrafficUFO_cr()
	{
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		if (player.controlScheme != ArcadePlayerController.ControlScheme.Rocket)
		{
			yield return base.StartCoroutine(this.switchToRocket_cr());
		}
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.trafficManager.StartTraffic();
		yield break;
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00066EC8 File Offset: 0x000650C8
	public IEnumerator switchToJetpack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		ArcadePlayerController player = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerOne);
		player.ChangeToJetpack();
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			ArcadePlayerController player2 = PlayerManager.GetPlayer<ArcadePlayerController>(PlayerId.PlayerTwo);
			player2.ChangeToJetpack();
		}
		yield return null;
		yield break;
	}

	// Token: 0x0400027B RID: 635
	public LevelProperties.RetroArcade properties;

	// Token: 0x0400027C RID: 636
	public static float TOTAL_POINTS;

	// Token: 0x0400027E RID: 638
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x0400027F RID: 639
	[SerializeField]
	[Multiline]
	public string _bossQuote;

	// Token: 0x04000280 RID: 640
	[SerializeField]
	public RetroArcadeTrafficManager trafficManager;

	// Token: 0x04000281 RID: 641
	[SerializeField]
	public RetroArcadeTentacleManager tentacleManager;

	// Token: 0x04000282 RID: 642
	[SerializeField]
	public RetroArcadeSnakeManager snakeManager;

	// Token: 0x04000283 RID: 643
	[SerializeField]
	public RetroArcadeSheriffManager sheriffManager;

	// Token: 0x04000284 RID: 644
	[SerializeField]
	public RetroArcadeChaserManager chaserManager;

	// Token: 0x04000285 RID: 645
	[SerializeField]
	public RetroArcadeBouncyManager bouncyManager;

	// Token: 0x04000286 RID: 646
	[SerializeField]
	public RetroArcadeAlienManager alienManager;

	// Token: 0x04000287 RID: 647
	[SerializeField]
	public RetroArcadeCaterpillarManager caterpillarManager;

	// Token: 0x04000288 RID: 648
	[SerializeField]
	public RetroArcadeRobotManager robotManager;

	// Token: 0x04000289 RID: 649
	[SerializeField]
	public RetroArcadePaddleShip paddleShip;

	// Token: 0x0400028A RID: 650
	[SerializeField]
	public RetroArcadeQShip qShip;

	// Token: 0x0400028B RID: 651
	[SerializeField]
	public RetroArcadeUFO ufo;

	// Token: 0x0400028C RID: 652
	[SerializeField]
	public RetroArcadeToadManager toadManager;

	// Token: 0x0400028D RID: 653
	[SerializeField]
	public RetroArcadeMissileMan missileMan;

	// Token: 0x0400028E RID: 654
	[SerializeField]
	public RetroArcadeWorm worm;

	// Token: 0x0400028F RID: 655
	[SerializeField]
	public RetroArcadeBigPlayer bigCuphead;

	// Token: 0x04000290 RID: 656
	[SerializeField]
	public RetroArcadeBigPlayer bigMugman;

	// Token: 0x04000291 RID: 657
	public ArcadePlayerController playerPrefab;
}
