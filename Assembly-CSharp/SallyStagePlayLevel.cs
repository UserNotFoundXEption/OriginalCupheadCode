using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class SallyStagePlayLevel : Level
{
	// Token: 0x060003C6 RID: 966 RVA: 0x00067BEC File Offset: 0x00065DEC
	public override void PartialInit()
	{
		this.properties = LevelProperties.SallyStagePlay.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x060003C7 RID: 967 RVA: 0x00004DDF File Offset: 0x00002FDF
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.SallyStagePlay;
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x060003C8 RID: 968 RVA: 0x00004DE6 File Offset: 0x00002FE6
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_sally_stage_play;
		}
	}

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x060003C9 RID: 969 RVA: 0x00067C84 File Offset: 0x00065E84
	// (remove) Token: 0x060003CA RID: 970 RVA: 0x00067CBC File Offset: 0x00065EBC
	public event Action OnPhase3;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x060003CB RID: 971 RVA: 0x00067CF4 File Offset: 0x00065EF4
	// (remove) Token: 0x060003CC RID: 972 RVA: 0x00067D2C File Offset: 0x00065F2C
	public event Action OnPhase2;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x060003CD RID: 973 RVA: 0x00067D64 File Offset: 0x00065F64
	// (remove) Token: 0x060003CE RID: 974 RVA: 0x00067D9C File Offset: 0x00065F9C
	public event Action OnPhase4;

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x060003CF RID: 975 RVA: 0x00067DD4 File Offset: 0x00065FD4
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.SallyStagePlay.States.Main:
			case LevelProperties.SallyStagePlay.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.SallyStagePlay.States.House:
				return this._bossPortraitHouse;
			case LevelProperties.SallyStagePlay.States.Angel:
				return this._bossPortraitAngel;
			case LevelProperties.SallyStagePlay.States.Final:
				return this._bossPortraitFinal;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x060003D0 RID: 976 RVA: 0x00067E60 File Offset: 0x00066060
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.SallyStagePlay.States.Main:
			case LevelProperties.SallyStagePlay.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.SallyStagePlay.States.House:
				return this._bossQuoteHouse;
			case LevelProperties.SallyStagePlay.States.Angel:
				return this._bossQuoteAngel;
			case LevelProperties.SallyStagePlay.States.Final:
				return this._bossQuoteFinal;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x00067EEC File Offset: 0x000660EC
	public override void Start()
	{
		base.Start();
		this.sally.LevelInit(this.properties);
		this.sally.GetParent(this);
		this.angel.LevelInit(this.properties);
		this.backgroundHandler.GetProperties(this.properties, this);
		this.husband.LevelInit(this.properties);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00067F60 File Offset: 0x00066160
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.SallyStagePlay.States.House)
		{
			base.StartCoroutine(this.residence_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.SallyStagePlay.States.Angel)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.angel_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.SallyStagePlay.States.Final)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.final_cr());
		}
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x00004DEA File Offset: 0x00002FEA
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.sallystageplayPattern_cr());
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x00004DF9 File Offset: 0x00002FF9
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitAngel = null;
		this._bossPortraitFinal = null;
		this._bossPortraitHouse = null;
		this._bossPortraitMain = null;
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x00067FF4 File Offset: 0x000661F4
	public IEnumerator sallystageplayPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			if (this.sally.state != SallyStagePlayLevelSally.State.Transition)
			{
				yield return base.StartCoroutine(this.nextPattern_cr());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x00068010 File Offset: 0x00066210
	public IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.SallyStagePlay.Pattern.Jump:
			base.StartCoroutine(this.jump_cr());
			break;
		case LevelProperties.SallyStagePlay.Pattern.Umbrella:
			base.StartCoroutine(this.umbrella_cr());
			break;
		case LevelProperties.SallyStagePlay.Pattern.Kiss:
			base.StartCoroutine(this.kiss_cr());
			break;
		case LevelProperties.SallyStagePlay.Pattern.Teleport:
			base.StartCoroutine(this.teleport_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		}
		yield break;
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0006802C File Offset: 0x0006622C
	public IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2.2f);
		this.backgroundHandler.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Church);
		yield return null;
		yield break;
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x00068048 File Offset: 0x00066248
	public IEnumerator jump_cr()
	{
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		this.sally.OnJumpAttack();
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x00068064 File Offset: 0x00066264
	public IEnumerator umbrella_cr()
	{
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		this.sally.OnUmbrellaAttack();
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060003DA RID: 986 RVA: 0x00068080 File Offset: 0x00066280
	public IEnumerator kiss_cr()
	{
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		this.sally.OnKissAttack();
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060003DB RID: 987 RVA: 0x0006809C File Offset: 0x0006629C
	public IEnumerator teleport_cr()
	{
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		this.sally.OnTeleportAttack();
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060003DC RID: 988 RVA: 0x000680B8 File Offset: 0x000662B8
	public IEnumerator residence_cr()
	{
		this.backgroundHandler.RollUpCupids();
		this.sally.PrePhase2();
		this.secretTriggered = SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE;
		while (this.sally.state != SallyStagePlayLevelSally.State.Idle)
		{
			yield return null;
		}
		if (this.OnPhase2 != null)
		{
			this.OnPhase2();
			this.StopAllCoroutines();
			base.StartCoroutine(this.sallystageplayPattern_cr());
		}
		yield return null;
		yield break;
	}

	// Token: 0x060003DD RID: 989 RVA: 0x000680D4 File Offset: 0x000662D4
	public IEnumerator angel_cr()
	{
		if (this.OnPhase3 != null)
		{
			this.OnPhase3();
		}
		this.sally.OnPhase3(SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE);
		yield return null;
		yield break;
	}

	// Token: 0x060003DE RID: 990 RVA: 0x000680F0 File Offset: 0x000662F0
	public IEnumerator final_cr()
	{
		this.angel.OnPhase4();
		if (this.OnPhase4 != null)
		{
			this.OnPhase4();
		}
		yield return null;
		AudioManager.PlayLoop("sally_audience_applause_ph4_loop");
		yield break;
	}

	// Token: 0x040002BA RID: 698
	public LevelProperties.SallyStagePlay properties;

	// Token: 0x040002BE RID: 702
	[SerializeField]
	public SallyStagePlayLevelBackgroundHandler backgroundHandler;

	// Token: 0x040002BF RID: 703
	[SerializeField]
	public SallyStagePlayLevelAngel angel;

	// Token: 0x040002C0 RID: 704
	[SerializeField]
	public SallyStagePlayLevelSally sally;

	// Token: 0x040002C1 RID: 705
	[SerializeField]
	public SallyStagePlayLevelFianceDeity husband;

	// Token: 0x040002C2 RID: 706
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040002C3 RID: 707
	[SerializeField]
	public Sprite _bossPortraitHouse;

	// Token: 0x040002C4 RID: 708
	[SerializeField]
	public Sprite _bossPortraitAngel;

	// Token: 0x040002C5 RID: 709
	[SerializeField]
	public Sprite _bossPortraitFinal;

	// Token: 0x040002C6 RID: 710
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x040002C7 RID: 711
	[SerializeField]
	public string _bossQuoteHouse;

	// Token: 0x040002C8 RID: 712
	[SerializeField]
	public string _bossQuoteAngel;

	// Token: 0x040002C9 RID: 713
	[SerializeField]
	public string _bossQuoteFinal;
}
