using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class ChessKnightLevel : ChessLevel
{
	// Token: 0x0600010C RID: 268 RVA: 0x000606A8 File Offset: 0x0005E8A8
	public override void PartialInit()
	{
		this.properties = LevelProperties.ChessKnight.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x0600010D RID: 269 RVA: 0x00003AC7 File Offset: 0x00001CC7
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessKnight;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x0600010E RID: 270 RVA: 0x00003ACE File Offset: 0x00001CCE
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_knight;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x0600010F RID: 271 RVA: 0x00003AD2 File Offset: 0x00001CD2
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000110 RID: 272 RVA: 0x00003ADA File Offset: 0x00001CDA
	public override string BossQuote
	{
		get
		{
			return this._bossQuoteMain;
		}
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00003AE2 File Offset: 0x00001CE2
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this.knight = null;
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00060740 File Offset: 0x0005E940
	public override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		this.knight.LevelInit(this.properties);
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.gameObject.layer = 31;
				foreach (Transform transform in levelPlayerController.gameObject.GetComponentsInChildren<Transform>(true))
				{
					transform.gameObject.layer = 31;
				}
			}
		}
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00060808 File Offset: 0x0005EA08
	public override void OnPlayerJoined(PlayerId playerId)
	{
		base.OnPlayerJoined(playerId);
		AbstractPlayerController player = PlayerManager.GetPlayer(playerId);
		if (player)
		{
			foreach (SpriteRenderer spriteRenderer in player.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.gameObject.layer = 31;
			}
		}
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00003AF8 File Offset: 0x00001CF8
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.chessknightPattern_cr());
	}

	// Token: 0x06000115 RID: 277 RVA: 0x0006085C File Offset: 0x0005EA5C
	public IEnumerator chessknightPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00060878 File Offset: 0x0005EA78
	public IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.ChessKnight.Pattern.Long:
			yield return base.StartCoroutine(this.long_cr());
			break;
		case LevelProperties.ChessKnight.Pattern.Short:
			yield return base.StartCoroutine(this.short_cr());
			break;
		case LevelProperties.ChessKnight.Pattern.Up:
			yield return base.StartCoroutine(this.up_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		}
		yield break;
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00060894 File Offset: 0x0005EA94
	public IEnumerator short_cr()
	{
		while (this.knight.state != ChessKnightLevelKnight.State.Idle)
		{
			yield return null;
		}
		this.knight.Short();
		while (this.knight.state != ChessKnightLevelKnight.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000118 RID: 280 RVA: 0x000608B0 File Offset: 0x0005EAB0
	public IEnumerator long_cr()
	{
		while (this.knight.state != ChessKnightLevelKnight.State.Idle)
		{
			yield return null;
		}
		this.knight.Long();
		while (this.knight.state != ChessKnightLevelKnight.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000119 RID: 281 RVA: 0x000608CC File Offset: 0x0005EACC
	public IEnumerator up_cr()
	{
		while (this.knight.state != ChessKnightLevelKnight.State.Idle)
		{
			yield return null;
		}
		this.knight.Up();
		while (this.knight.state != ChessKnightLevelKnight.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040000F2 RID: 242
	public LevelProperties.ChessKnight properties;

	// Token: 0x040000F3 RID: 243
	[SerializeField]
	public ChessKnightLevelKnight knight;

	// Token: 0x040000F4 RID: 244
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040000F5 RID: 245
	[SerializeField]
	public string _bossQuoteMain;
}
