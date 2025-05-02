using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class AirshipStorkLevel : Level
{
	// Token: 0x06000059 RID: 89 RVA: 0x0005E58C File Offset: 0x0005C78C
	public override void PartialInit()
	{
		this.properties = LevelProperties.AirshipStork.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600005A RID: 90 RVA: 0x00003580 File Offset: 0x00001780
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.AirshipStork;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x0600005B RID: 91 RVA: 0x00003587 File Offset: 0x00001787
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_airship_stork;
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600005C RID: 92 RVA: 0x0000358B File Offset: 0x0000178B
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x0600005D RID: 93 RVA: 0x00003593 File Offset: 0x00001793
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x0000359B File Offset: 0x0000179B
	public override void Start()
	{
		base.Start();
		this.stork.LevelInit(this.properties);
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000035B4 File Offset: 0x000017B4
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.airshipstorkPattern_cr());
	}

	// Token: 0x06000060 RID: 96 RVA: 0x0005E624 File Offset: 0x0005C824
	public IEnumerator airshipstorkPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x0005E640 File Offset: 0x0005C840
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.AirshipStork.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x0400007D RID: 125
	public LevelProperties.AirshipStork properties;

	// Token: 0x0400007E RID: 126
	[SerializeField]
	public AirshipStorkLevelStork stork;

	// Token: 0x0400007F RID: 127
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000080 RID: 128
	[SerializeField]
	[Multiline]
	public string _bossQuote;
}
