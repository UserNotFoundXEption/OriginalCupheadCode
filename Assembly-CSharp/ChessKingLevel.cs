using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class ChessKingLevel : Level
{
	// Token: 0x06000101 RID: 257 RVA: 0x000605D8 File Offset: 0x0005E7D8
	public override void PartialInit()
	{
		this.properties = LevelProperties.ChessKing.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000102 RID: 258 RVA: 0x00003A65 File Offset: 0x00001C65
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessKing;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000103 RID: 259 RVA: 0x00003A6C File Offset: 0x00001C6C
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_king;
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000104 RID: 260 RVA: 0x00003A70 File Offset: 0x00001C70
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000105 RID: 261 RVA: 0x00003A78 File Offset: 0x00001C78
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00003A80 File Offset: 0x00001C80
	public override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		this.king.LevelInit(this.properties);
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00003A9F File Offset: 0x00001C9F
	public override void OnLevelStart()
	{
		this.king.StartGame();
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00003AAC File Offset: 0x00001CAC
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		this.king.StateChange();
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00060670 File Offset: 0x0005E870
	public IEnumerator chesskingPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x0006068C File Offset: 0x0005E88C
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.ChessKing.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x040000EE RID: 238
	public LevelProperties.ChessKing properties;

	// Token: 0x040000EF RID: 239
	[SerializeField]
	public ChessKingLevelKing king;

	// Token: 0x040000F0 RID: 240
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x040000F1 RID: 241
	[SerializeField]
	[Multiline]
	public string _bossQuote;
}
