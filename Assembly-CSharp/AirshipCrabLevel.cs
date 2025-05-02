using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class AirshipCrabLevel : Level
{
	// Token: 0x06000045 RID: 69 RVA: 0x0005E3EC File Offset: 0x0005C5EC
	public override void PartialInit()
	{
		this.properties = LevelProperties.AirshipCrab.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000046 RID: 70 RVA: 0x000034FB File Offset: 0x000016FB
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.AirshipCrab;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000047 RID: 71 RVA: 0x00003502 File Offset: 0x00001702
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_airship_crab;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000048 RID: 72 RVA: 0x00003506 File Offset: 0x00001706
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000049 RID: 73 RVA: 0x0000350E File Offset: 0x0000170E
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00003516 File Offset: 0x00001716
	public override void Start()
	{
		base.Start();
		this.crab.LevelInit(this.properties);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x0000352F File Offset: 0x0000172F
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.airshipcrabPattern_cr());
	}

	// Token: 0x0600004C RID: 76 RVA: 0x0005E484 File Offset: 0x0005C684
	public IEnumerator airshipcrabPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x0005E4A0 File Offset: 0x0005C6A0
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.AirshipCrab.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000075 RID: 117
	public LevelProperties.AirshipCrab properties;

	// Token: 0x04000076 RID: 118
	[SerializeField]
	public AirshipCrabLevelCrab crab;

	// Token: 0x04000077 RID: 119
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000078 RID: 120
	[SerializeField]
	[Multiline]
	public string _bossQuote;
}
