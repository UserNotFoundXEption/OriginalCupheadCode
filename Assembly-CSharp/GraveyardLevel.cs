using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class GraveyardLevel : Level
{
	// Token: 0x060002E2 RID: 738 RVA: 0x000648A0 File Offset: 0x00062AA0
	public override void PartialInit()
	{
		this.properties = LevelProperties.Graveyard.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000476C File Offset: 0x0000296C
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Graveyard;
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x060002E4 RID: 740 RVA: 0x00004773 File Offset: 0x00002973
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_graveyard;
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x060002E5 RID: 741 RVA: 0x00004777 File Offset: 0x00002977
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortraitMain;
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000477F File Offset: 0x0000297F
	public override string BossQuote
	{
		get
		{
			return this._bossQuoteMain;
		}
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x00004787 File Offset: 0x00002987
	public override void Awake()
	{
		this.originalMode = Level.CurrentMode;
		Level.SetCurrentMode(Level.Mode.Normal);
		base.Awake();
		Level.IsGraveyard = true;
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x00064938 File Offset: 0x00062B38
	public override void Start()
	{
		base.Start();
		for (int i = 0; i < this.splitDevil.Length; i++)
		{
			this.splitDevil[i].LevelInit(this.properties);
		}
		this.attackCounterString = new PatternString(this.properties.CurrentState.splitDevilBeam.attacksBeforeBeamString, true);
		this.attackCounter = this.attackCounterString.PopInt();
		AudioManager.PlayLoop("sfx_dlc_graveyard_amb_loop");
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x000047A6 File Offset: 0x000029A6
	public override void PlayAnnouncerReady()
	{
		AudioManager.Play("level_announcer_ready_ghostly");
	}

	// Token: 0x060002EA RID: 746 RVA: 0x000047B2 File Offset: 0x000029B2
	public override void PlayAnnouncerBegin()
	{
		AudioManager.Play("level_announcer_begin_ghostly");
	}

	// Token: 0x060002EB RID: 747 RVA: 0x000047BE File Offset: 0x000029BE
	public bool CheckForBeamAttack()
	{
		this.attackCounter--;
		if (this.attackCounter == -1)
		{
			this.attackCounter = this.attackCounterString.PopInt();
			return true;
		}
		return false;
	}

	// Token: 0x060002EC RID: 748 RVA: 0x000649B4 File Offset: 0x00062BB4
	public override void OnLevelStart()
	{
		for (int i = 0; i < this.splitDevil.Length; i++)
		{
			this.splitDevil[i].NextPattern();
		}
	}

	// Token: 0x060002ED RID: 749 RVA: 0x000649E8 File Offset: 0x00062BE8
	public override void OnWin()
	{
		base.OnWin();
		for (int i = 0; i < this.splitDevil.Length; i++)
		{
			this.splitDevil[i].Die();
		}
	}

	// Token: 0x060002EE RID: 750 RVA: 0x000047EE File Offset: 0x000029EE
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		Level.SetCurrentMode(this.originalMode);
		this.splitDevil = null;
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00064A24 File Offset: 0x00062C24
	public IEnumerator devilPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00064A40 File Offset: 0x00062C40
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.Graveyard.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x040001F7 RID: 503
	public LevelProperties.Graveyard properties;

	// Token: 0x040001F8 RID: 504
	[SerializeField]
	public GraveyardLevelSplitDevil[] splitDevil;

	// Token: 0x040001F9 RID: 505
	public PatternString attackCounterString;

	// Token: 0x040001FA RID: 506
	public int attackCounter;

	// Token: 0x040001FB RID: 507
	public Level.Mode originalMode;

	// Token: 0x040001FC RID: 508
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040001FD RID: 509
	[SerializeField]
	public string _bossQuoteMain;
}
