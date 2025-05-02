using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class ChessBOldALevel : Level
{
	// Token: 0x060000BC RID: 188 RVA: 0x0005F5B4 File Offset: 0x0005D7B4
	public override void PartialInit()
	{
		this.properties = LevelProperties.ChessBOldA.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x060000BD RID: 189 RVA: 0x0000387A File Offset: 0x00001A7A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessBOldA;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x060000BE RID: 190 RVA: 0x00003881 File Offset: 0x00001A81
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_bolda;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x060000BF RID: 191 RVA: 0x00003885 File Offset: 0x00001A85
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000388D File Offset: 0x00001A8D
	public override string BossQuote
	{
		get
		{
			return this._bossQuoteMain;
		}
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00003895 File Offset: 0x00001A95
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x0005F64C File Offset: 0x0005D84C
	public override void Start()
	{
		Level.IsChessBoss = true;
		base.Start();
		this.bishop.LevelInit(this.properties);
		foreach (Transform transform in this.topPlatforms)
		{
			transform.transform.SetPosition(null, new float?(-360f + this.properties.CurrentState.stage.platformHeight * 2f), null);
		}
		foreach (Transform transform2 in this.bottomPlatforms)
		{
			transform2.transform.SetPosition(null, new float?(-360f + this.properties.CurrentState.stage.platformHeight), null);
		}
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x000038A4 File Offset: 0x00001AA4
	public override void OnLevelStart()
	{
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x0005F740 File Offset: 0x0005D940
	public IEnumerator chessbishopPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x0005F75C File Offset: 0x0005D95C
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.ChessBOldA.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x040000B5 RID: 181
	public LevelProperties.ChessBOldA properties;

	// Token: 0x040000B6 RID: 182
	[SerializeField]
	public ChessBOldALevelBishop bishop;

	// Token: 0x040000B7 RID: 183
	[SerializeField]
	public Transform[] topPlatforms;

	// Token: 0x040000B8 RID: 184
	[SerializeField]
	public Transform[] bottomPlatforms;

	// Token: 0x040000B9 RID: 185
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040000BA RID: 186
	[SerializeField]
	public string _bossQuoteMain;
}
