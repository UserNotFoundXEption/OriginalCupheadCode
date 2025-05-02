using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class ChessRookLevel : ChessLevel
{
	// Token: 0x0600013E RID: 318 RVA: 0x00060DB0 File Offset: 0x0005EFB0
	public override void PartialInit()
	{
		this.properties = LevelProperties.ChessRook.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600013F RID: 319 RVA: 0x00003C6D File Offset: 0x00001E6D
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessRook;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06000140 RID: 320 RVA: 0x00003C74 File Offset: 0x00001E74
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_rook;
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000141 RID: 321 RVA: 0x00003C78 File Offset: 0x00001E78
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortraitMain;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000142 RID: 322 RVA: 0x00003C80 File Offset: 0x00001E80
	public override string BossQuote
	{
		get
		{
			return this._bossQuoteMain;
		}
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00003C88 File Offset: 0x00001E88
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this.rook = null;
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00003C9E File Offset: 0x00001E9E
	public override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		this.rook.LevelInit(this.properties);
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00003CBD File Offset: 0x00001EBD
	public override void OnLevelStart()
	{
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00003CBF File Offset: 0x00001EBF
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		this.rook.OnPhaseChange();
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00060E48 File Offset: 0x0005F048
	public IEnumerator chessrookPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00060E64 File Offset: 0x0005F064
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.ChessRook.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000105 RID: 261
	public LevelProperties.ChessRook properties;

	// Token: 0x04000106 RID: 262
	[SerializeField]
	public ChessRookLevelRook rook;

	// Token: 0x04000107 RID: 263
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x04000108 RID: 264
	[SerializeField]
	public string _bossQuoteMain;
}
