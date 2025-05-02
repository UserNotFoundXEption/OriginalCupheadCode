using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class FlyingGenieLevel : Level
{
	// Token: 0x06000291 RID: 657 RVA: 0x00063CC4 File Offset: 0x00061EC4
	public override void PartialInit()
	{
		this.properties = LevelProperties.FlyingGenie.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06000292 RID: 658 RVA: 0x00004582 File Offset: 0x00002782
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.FlyingGenie;
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000293 RID: 659 RVA: 0x00004589 File Offset: 0x00002789
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flying_genie;
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000294 RID: 660 RVA: 0x00063D5C File Offset: 0x00061F5C
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingGenie.States.Main:
			case LevelProperties.FlyingGenie.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.FlyingGenie.States.Giant:
				return this._bossPortraitGiant;
			case LevelProperties.FlyingGenie.States.Marionette:
				return this._bossPortraitMarionette;
			case LevelProperties.FlyingGenie.States.Disappear:
				return (this.genie.state != FlyingGenieLevelGenie.State.Disappear) ? this._bossPortraitCoffin : this._bossPortraitDisappear;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000295 RID: 661 RVA: 0x00063E04 File Offset: 0x00062004
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingGenie.States.Main:
			case LevelProperties.FlyingGenie.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.FlyingGenie.States.Giant:
				if (PlayerData.Data.DjimmiActivatedBaseGame())
				{
					return this._bossQuoteGameDjimmi;
				}
				return this._bossQuoteGiant;
			case LevelProperties.FlyingGenie.States.Marionette:
				return this._bossQuoteMarionette;
			case LevelProperties.FlyingGenie.States.Disappear:
				return (this.genie.state != FlyingGenieLevelGenie.State.Disappear) ? this._bossQuoteCoffin : this._bossQuoteDisappear;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0000458D File Offset: 0x0000278D
	public override void Awake()
	{
		base.Awake();
		base.StartCoroutine(FlyingGenieLevel.timer_cr());
	}

	// Token: 0x06000297 RID: 663 RVA: 0x000045A1 File Offset: 0x000027A1
	public override void Start()
	{
		base.Start();
		this.genie.LevelInit(this.properties);
		this.genieTransformed.LevelInit(this.properties);
		this.goop.LevelInit(this.properties);
	}

	// Token: 0x06000298 RID: 664 RVA: 0x000045DC File Offset: 0x000027DC
	public override void OnLevelStart()
	{
	}

	// Token: 0x06000299 RID: 665 RVA: 0x00063EC0 File Offset: 0x000620C0
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.FlyingGenie.States.Marionette)
		{
			base.StartCoroutine(this.phase2_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingGenie.States.Giant)
		{
			base.StartCoroutine(this.phase3_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingGenie.States.Disappear)
		{
			base.StartCoroutine(this.pillar_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingGenie.States.Generic)
		{
			base.StartCoroutine(this.treasure_cr());
		}
	}

	// Token: 0x0600029A RID: 666 RVA: 0x000045DE File Offset: 0x000027DE
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitCoffin = null;
		this._bossPortraitDisappear = null;
		this._bossPortraitGiant = null;
		this._bossPortraitMain = null;
		this._bossPortraitMarionette = null;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00063F70 File Offset: 0x00062170
	public IEnumerator flyinggeniePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600029C RID: 668 RVA: 0x00063F8C File Offset: 0x0006218C
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.FlyingGenie.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x0600029D RID: 669 RVA: 0x00063FA8 File Offset: 0x000621A8
	public IEnumerator phase2_cr()
	{
		this.genie.HitTrigger();
		while (this.genie.state != FlyingGenieLevelGenie.State.Idle)
		{
			yield return null;
		}
		this.genie.animator.SetTrigger("ToPhase2");
		yield break;
	}

	// Token: 0x0600029E RID: 670 RVA: 0x00063FC4 File Offset: 0x000621C4
	public IEnumerator phase3_cr()
	{
		if (!this.genieTransformed.skipMarionette)
		{
			this.genieTransformed.EndMarionette();
		}
		else
		{
			this.secretTriggered = true;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00063FE0 File Offset: 0x000621E0
	public IEnumerator pillar_cr()
	{
		this.genie.HitTrigger();
		while (this.genie.state != FlyingGenieLevelGenie.State.Idle)
		{
			yield return null;
		}
		this.genie.StartObelisk();
		while (this.genie.state != FlyingGenieLevelGenie.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x00063FFC File Offset: 0x000621FC
	public IEnumerator treasure_cr()
	{
		this.genie.HitTrigger();
		while (this.genie.state != FlyingGenieLevelGenie.State.Idle)
		{
			yield return null;
		}
		this.genie.StartTreasure();
		while (this.genie.state != FlyingGenieLevelGenie.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00064018 File Offset: 0x00062218
	public static IEnumerator timer_cr()
	{
		FlyingGenieLevel.mainTimer = 9.25f;
		for (;;)
		{
			FlyingGenieLevel.mainTimer += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x040001C5 RID: 453
	public LevelProperties.FlyingGenie properties;

	// Token: 0x040001C6 RID: 454
	public const float SHADE_PERIOD = 12f;

	// Token: 0x040001C7 RID: 455
	public const float SHADE_START_TIME = 9.25f;

	// Token: 0x040001C8 RID: 456
	public static float mainTimer;

	// Token: 0x040001C9 RID: 457
	[SerializeField]
	public FlyingGenieLevelGoop goop;

	// Token: 0x040001CA RID: 458
	[SerializeField]
	public FlyingGenieLevelGenie genie;

	// Token: 0x040001CB RID: 459
	[SerializeField]
	public FlyingGenieLevelGenieTransform genieTransformed;

	// Token: 0x040001CC RID: 460
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040001CD RID: 461
	[SerializeField]
	public Sprite _bossPortraitDisappear;

	// Token: 0x040001CE RID: 462
	[SerializeField]
	public Sprite _bossPortraitCoffin;

	// Token: 0x040001CF RID: 463
	[SerializeField]
	public Sprite _bossPortraitMarionette;

	// Token: 0x040001D0 RID: 464
	[SerializeField]
	public Sprite _bossPortraitGiant;

	// Token: 0x040001D1 RID: 465
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x040001D2 RID: 466
	[SerializeField]
	public string _bossQuoteDisappear;

	// Token: 0x040001D3 RID: 467
	[SerializeField]
	public string _bossQuoteCoffin;

	// Token: 0x040001D4 RID: 468
	[SerializeField]
	public string _bossQuoteMarionette;

	// Token: 0x040001D5 RID: 469
	[SerializeField]
	public string _bossQuoteGiant;

	// Token: 0x040001D6 RID: 470
	[SerializeField]
	public string _bossQuoteGameDjimmi;
}
