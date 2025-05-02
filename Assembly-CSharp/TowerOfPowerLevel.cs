using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class TowerOfPowerLevel : Level
{
	// Token: 0x06000438 RID: 1080 RVA: 0x000694D8 File Offset: 0x000676D8
	public override void PartialInit()
	{
		this.properties = LevelProperties.TowerOfPower.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000439 RID: 1081 RVA: 0x00005070 File Offset: 0x00003270
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.TowerOfPower;
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x0600043A RID: 1082 RVA: 0x00005077 File Offset: 0x00003277
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_tower_of_power;
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x0600043B RID: 1083 RVA: 0x0000507B File Offset: 0x0000327B
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x0600043C RID: 1084 RVA: 0x00005083 File Offset: 0x00003283
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x0600043D RID: 1085 RVA: 0x0000508B File Offset: 0x0000328B
	public override float LevelIntroTime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x0600043E RID: 1086 RVA: 0x00005092 File Offset: 0x00003292
	public TowerOfPowerLevelGameManager GameManager
	{
		get
		{
			return this.gameManager;
		}
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00069570 File Offset: 0x00067770
	public override void Start()
	{
		base.Start();
		this.gameManager.LevelInit(this.properties);
		foreach (AbstractPlayerController abstractPlayerController in this.players)
		{
			if (abstractPlayerController != null)
			{
				abstractPlayerController.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x000695CC File Offset: 0x000677CC
	public override void Awake()
	{
		base.Awake();
		if (TowerOfPowerLevelGameInfo.GameInfo != null)
		{
			Level.Current.OnLoseEvent += TowerOfPowerLevelGameInfo.GameInfo.CleanUp;
		}
		base.OnLoseEvent += this.ResetScore;
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x0000509A File Offset: 0x0000329A
	public override void OnPlayerJoined(PlayerId playerId)
	{
		TowerOfPowerLevelGameInfo.InitAddedPlayer(playerId, this.properties.CurrentState.slotMachine.DefaultStartingToken);
		base.OnPlayerJoined(playerId);
		TowerOfPowerLevelGameInfo.InitEquipment(playerId);
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x000050C4 File Offset: 0x000032C4
	public override void OnDestroy()
	{
		base.OnDestroy();
		Level.IsTowerOfPowerMain = false;
		base.OnLoseEvent -= this.ResetScore;
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x000050E4 File Offset: 0x000032E4
	public void ResetScore()
	{
		base.OnLoseEvent -= this.ResetScore;
		base.CleanUpScore();
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x000050FE File Offset: 0x000032FE
	public override void CheckIfInABossesHub()
	{
		base.CheckIfInABossesHub();
		Level.IsTowerOfPower = true;
		Level.IsTowerOfPowerMain = true;
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x00005112 File Offset: 0x00003312
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.towerofpowerPattern_cr());
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x0006961C File Offset: 0x0006781C
	public IEnumerator towerofpowerPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x00069638 File Offset: 0x00067838
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.TowerOfPower.Pattern p = this.properties.CurrentState.NextPattern;
		yield return null;
		yield break;
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x00005121 File Offset: 0x00003321
	public void OnGUI()
	{
	}

	// Token: 0x04000319 RID: 793
	public LevelProperties.TowerOfPower properties;

	// Token: 0x0400031A RID: 794
	[SerializeField]
	public TowerOfPowerLevelGameManager gameManager;

	// Token: 0x0400031B RID: 795
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x0400031C RID: 796
	[SerializeField]
	[Multiline]
	public string _bossQuote;
}
