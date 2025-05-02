using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class DicePalaceCardLevel : AbstractDicePalaceLevel
{
	// Token: 0x0600018D RID: 397 RVA: 0x00061D98 File Offset: 0x0005FF98
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceCard.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x0600018E RID: 398 RVA: 0x00003E45 File Offset: 0x00002045
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceCard;
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x0600018F RID: 399 RVA: 0x00003E4C File Offset: 0x0000204C
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceCard;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000190 RID: 400 RVA: 0x00003E53 File Offset: 0x00002053
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_card;
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000191 RID: 401 RVA: 0x00003E57 File Offset: 0x00002057
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000192 RID: 402 RVA: 0x00003E5F File Offset: 0x0000205F
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000193 RID: 403 RVA: 0x00003E67 File Offset: 0x00002067
	public override void Start()
	{
		base.Start();
		this.card.LevelInit(this.properties);
		this.gameManager.GameSetup(this.properties);
	}

	// Token: 0x06000194 RID: 404 RVA: 0x00003E91 File Offset: 0x00002091
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacecardPattern_cr());
		base.StartCoroutine(this.gameManager.start_game_cr());
	}

	// Token: 0x06000195 RID: 405 RVA: 0x00061E30 File Offset: 0x00060030
	public IEnumerator dicepalacecardPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00061E4C File Offset: 0x0006004C
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceCard.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000142 RID: 322
	public LevelProperties.DicePalaceCard properties;

	// Token: 0x04000143 RID: 323
	[SerializeField]
	public DicePalaceCardGameManager gameManager;

	// Token: 0x04000144 RID: 324
	[SerializeField]
	public DicePalaceCardLevelCard card;

	// Token: 0x04000145 RID: 325
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000146 RID: 326
	[SerializeField]
	public string _bossQuote;
}
