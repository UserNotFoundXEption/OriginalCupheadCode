using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class DicePalaceMainLevel : AbstractDicePalaceLevel
{
	// Token: 0x060001EE RID: 494 RVA: 0x0006246C File Offset: 0x0006066C
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceMain.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060001EF RID: 495 RVA: 0x00004170 File Offset: 0x00002370
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceMain;
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060001F0 RID: 496 RVA: 0x00004177 File Offset: 0x00002377
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceMain;
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000417E File Offset: 0x0000237E
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_main;
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060001F2 RID: 498 RVA: 0x00004182 File Offset: 0x00002382
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000418A File Offset: 0x0000238A
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060001F4 RID: 500 RVA: 0x00004192 File Offset: 0x00002392
	public DicePalaceMainLevelGameManager GameManager
	{
		get
		{
			return this.gameManager;
		}
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x00062504 File Offset: 0x00060704
	public override void Start()
	{
		base.Start();
		this.gameManager.LevelInit(this.properties);
		this.kingDice.LevelInit(this.properties);
		if (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice)
		{
			DicePalaceMainLevelGameInfo.CHALICE_PLAYER = 0;
		}
		else if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice)
		{
			DicePalaceMainLevelGameInfo.CHALICE_PLAYER = 1;
		}
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0000419A File Offset: 0x0000239A
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacemainPattern_cr());
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x000041A9 File Offset: 0x000023A9
	public override void CheckIfInABossesHub()
	{
		base.CheckIfInABossesHub();
		if (!this.isTowerOfPower)
		{
			Level.IsDicePalaceMain = true;
		}
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x000041C2 File Offset: 0x000023C2
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x00062588 File Offset: 0x00060788
	public IEnumerator dicepalacemainPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001FA RID: 506 RVA: 0x000625A4 File Offset: 0x000607A4
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceMain.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000166 RID: 358
	public LevelProperties.DicePalaceMain properties;

	// Token: 0x04000167 RID: 359
	[SerializeField]
	public DicePalaceMainLevelGameManager gameManager;

	// Token: 0x04000168 RID: 360
	[SerializeField]
	public DicePalaceMainLevelKingDice kingDice;

	// Token: 0x04000169 RID: 361
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x0400016A RID: 362
	[SerializeField]
	public string _bossQuote;
}
