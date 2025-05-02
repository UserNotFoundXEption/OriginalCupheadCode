using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class ChessQueenLevel : ChessLevel
{
	// Token: 0x0600012E RID: 302 RVA: 0x00060BBC File Offset: 0x0005EDBC
	public override void PartialInit()
	{
		this.properties = LevelProperties.ChessQueen.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x0600012F RID: 303 RVA: 0x00003BEC File Offset: 0x00001DEC
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessQueen;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000130 RID: 304 RVA: 0x00003BF3 File Offset: 0x00001DF3
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_queen;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000131 RID: 305 RVA: 0x00003BF7 File Offset: 0x00001DF7
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortraitMain;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x06000132 RID: 306 RVA: 0x00003BFF File Offset: 0x00001DFF
	public override string BossQuote
	{
		get
		{
			return this._bossQuoteMain;
		}
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00003C07 File Offset: 0x00001E07
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this.queen = null;
		this.mouseAnimator = null;
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00003C24 File Offset: 0x00001E24
	public override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		this.queen.LevelInit(this.properties);
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00060C54 File Offset: 0x0005EE54
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		float num = Random.Range(0f, 1f);
		this.mouseAnimator[0].Play("Win", 0, num);
		this.mouseAnimator[1].Play("Win", 0, num + 0.33f);
		this.mouseAnimator[2].Play("Win", 0, num + 0.66f);
		this.mouseAnimator[0].Play("Idle", 1, 0f);
		this.mouseAnimator[1].Play("Idle", 1, 0.33f);
		this.mouseAnimator[2].Play("Idle", 1, 0.66f);
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00003C43 File Offset: 0x00001E43
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		this.queen.StateChanged();
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00003C56 File Offset: 0x00001E56
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.chessQueenPattern_cr());
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00060D08 File Offset: 0x0005EF08
	public IEnumerator chessQueenPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00060D24 File Offset: 0x0005EF24
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.ChessQueen.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.ChessQueen.Pattern.Lightning)
		{
			if (p != LevelProperties.ChessQueen.Pattern.Egg)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				yield return base.StartCoroutine(this.egg_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.lightning_cr());
		}
		yield break;
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00060D40 File Offset: 0x0005EF40
	public bool NextPatternIsEgg()
	{
		if (this.properties.CurrentState.PeekNextPattern == LevelProperties.ChessQueen.Pattern.Egg)
		{
			LevelProperties.ChessQueen.Pattern nextPattern = this.properties.CurrentState.NextPattern;
			return true;
		}
		return false;
	}

	// Token: 0x0600013B RID: 315 RVA: 0x00060D78 File Offset: 0x0005EF78
	public IEnumerator lightning_cr()
	{
		while (this.queen.state != ChessQueenLevelQueen.States.Idle)
		{
			yield return null;
		}
		this.queen.StartLightning();
		while (this.queen.state != ChessQueenLevelQueen.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00060D94 File Offset: 0x0005EF94
	public IEnumerator egg_cr()
	{
		while (this.queen.state != ChessQueenLevelQueen.States.Idle)
		{
			yield return null;
		}
		this.queen.StartEgg();
		while (this.queen.state != ChessQueenLevelQueen.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040000FF RID: 255
	public LevelProperties.ChessQueen properties;

	// Token: 0x04000100 RID: 256
	[SerializeField]
	public ChessQueenLevelQueen queen;

	// Token: 0x04000101 RID: 257
	[SerializeField]
	public Animator[] mouseAnimator;

	// Token: 0x04000102 RID: 258
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x04000103 RID: 259
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x04000104 RID: 260
	public bool cannonBlastFXVariant;
}
