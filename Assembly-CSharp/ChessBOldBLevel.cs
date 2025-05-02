using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class ChessBOldBLevel : Level
{
	// Token: 0x060000C7 RID: 199 RVA: 0x0005F778 File Offset: 0x0005D978
	public override void PartialInit()
	{
		this.properties = LevelProperties.ChessBOldB.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x060000C8 RID: 200 RVA: 0x000038AE File Offset: 0x00001AAE
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessBOldB;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060000C9 RID: 201 RVA: 0x000038B5 File Offset: 0x00001AB5
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_boldb;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060000CA RID: 202 RVA: 0x000038B9 File Offset: 0x00001AB9
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060000CB RID: 203 RVA: 0x000038C1 File Offset: 0x00001AC1
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060000CC RID: 204 RVA: 0x000038C9 File Offset: 0x00001AC9
	public override void Start()
	{
		base.Start();
		this.boss.LevelInit(this.properties);
		this.gameManager.SetupGameManager(this.properties);
	}

	// Token: 0x060000CD RID: 205 RVA: 0x000038F3 File Offset: 0x00001AF3
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		this.gameManager.OnStateChanged();
		this.boss.OnStateChanged();
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00003911 File Offset: 0x00001B11
	public override void OnLevelStart()
	{
	}

	// Token: 0x060000CF RID: 207 RVA: 0x0005F810 File Offset: 0x0005DA10
	public IEnumerator ChessBOldBPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x0005F82C File Offset: 0x0005DA2C
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.ChessBOldB.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x040000BB RID: 187
	public LevelProperties.ChessBOldB properties;

	// Token: 0x040000BC RID: 188
	[SerializeField]
	public ChessBOldBLevelBoss boss;

	// Token: 0x040000BD RID: 189
	[SerializeField]
	public ChessBOldBLevelGameManager gameManager;

	// Token: 0x040000BE RID: 190
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x040000BF RID: 191
	[SerializeField]
	[Multiline]
	public string _bossQuote;
}
