using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class ChessBishopLevel : ChessLevel
{
	// Token: 0x060000B1 RID: 177 RVA: 0x0005F4E4 File Offset: 0x0005D6E4
	public override void PartialInit()
	{
		this.properties = LevelProperties.ChessBishop.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060000B2 RID: 178 RVA: 0x0000380F File Offset: 0x00001A0F
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessBishop;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060000B3 RID: 179 RVA: 0x00003816 File Offset: 0x00001A16
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_bishop;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x060000B4 RID: 180 RVA: 0x0000381A File Offset: 0x00001A1A
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x060000B5 RID: 181 RVA: 0x00003822 File Offset: 0x00001A22
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0000382A File Offset: 0x00001A2A
	public override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		this.bishop.LevelInit(this.properties);
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00003849 File Offset: 0x00001A49
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.bishop = null;
		this._bossPortrait = null;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x0000385F File Offset: 0x00001A5F
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		this.bishop.StartNewPhase();
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x0005F57C File Offset: 0x0005D77C
	public IEnumerator bishopPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x0005F598 File Offset: 0x0005D798
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.ChessBishop.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x040000B1 RID: 177
	public LevelProperties.ChessBishop properties;

	// Token: 0x040000B2 RID: 178
	[SerializeField]
	public ChessBishopLevelBishop bishop;

	// Token: 0x040000B3 RID: 179
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x040000B4 RID: 180
	[SerializeField]
	[Multiline]
	public string _bossQuote;
}
