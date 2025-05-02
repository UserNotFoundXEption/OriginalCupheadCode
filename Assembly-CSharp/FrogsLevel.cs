using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class FrogsLevel : Level
{
	// Token: 0x060002C5 RID: 709 RVA: 0x0006448C File Offset: 0x0006268C
	public override void PartialInit()
	{
		this.properties = LevelProperties.Frogs.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x060002C6 RID: 710 RVA: 0x000046A8 File Offset: 0x000028A8
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Frogs;
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x060002C7 RID: 711 RVA: 0x000046AB File Offset: 0x000028AB
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_frogs;
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x060002C8 RID: 712 RVA: 0x000046AF File Offset: 0x000028AF
	// (set) Token: 0x060002C9 RID: 713 RVA: 0x000046B6 File Offset: 0x000028B6
	public static bool FINAL_FORM { get; set; }

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x060002CA RID: 714 RVA: 0x000046BE File Offset: 0x000028BE
	// (set) Token: 0x060002CB RID: 715 RVA: 0x000046C5 File Offset: 0x000028C5
	public static bool DEMON_TRIGGERED { get; set; }

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x060002CC RID: 716 RVA: 0x00064524 File Offset: 0x00062724
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Frogs.States.Main:
			case LevelProperties.Frogs.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Frogs.States.Roll:
				return this._bossPortraitRoll;
			case LevelProperties.Frogs.States.Morph:
				return this._bossPortraitMorph;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x060002CD RID: 717 RVA: 0x000645A4 File Offset: 0x000627A4
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Frogs.States.Main:
			case LevelProperties.Frogs.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Frogs.States.Roll:
				return this._bossQuoteRoll;
			case LevelProperties.Frogs.States.Morph:
				return this._bossQuoteMorph;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x060002CE RID: 718 RVA: 0x000046CD File Offset: 0x000028CD
	public override void Start()
	{
		base.Start();
		this.tall.LevelInit(this.properties);
		this.small.LevelInit(this.properties);
		this.morphed.LevelInit(this.properties);
	}

	// Token: 0x060002CF RID: 719 RVA: 0x00004708 File Offset: 0x00002908
	public override void OnLevelStart()
	{
		FrogsLevel.FINAL_FORM = false;
		this.StartState(LevelProperties.Frogs.States.Main);
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00064624 File Offset: 0x00062824
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		LevelProperties.Frogs.States stateName = this.properties.CurrentState.stateName;
		if (stateName == LevelProperties.Frogs.States.Morph)
		{
			FrogsLevel.FINAL_FORM = true;
		}
		this.StartState(stateName);
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x0006465C File Offset: 0x0006285C
	public override void CreatePlayers()
	{
		base.CreatePlayers();
		if (PlayerManager.Multiplayer && this.allowMultiplayer)
		{
			this.tall.AddFanForce(this.players[0]);
			this.tall.AddFanForce(this.players[1]);
		}
		else
		{
			this.tall.AddFanForce(this.players[0]);
		}
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00004717 File Offset: 0x00002917
	public override void CreatePlayerTwoOnJoin()
	{
		base.CreatePlayerTwoOnJoin();
		if (PlayerManager.Multiplayer && this.allowMultiplayer)
		{
			this.tall.AddFanForce(this.players[1]);
		}
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00004747 File Offset: 0x00002947
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitMorph = null;
		this._bossPortraitRoll = null;
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x000646C4 File Offset: 0x000628C4
	public void StartState(LevelProperties.Frogs.States state)
	{
		if (state != LevelProperties.Frogs.States.Generic)
		{
			if (this.checkCoroutine != null)
			{
				base.StopCoroutine(this.checkCoroutine);
			}
			this.checkCoroutine = null;
			if (this.stateCoroutine != null)
			{
				base.StopCoroutine(this.stateCoroutine);
			}
			this.stateCoroutine = null;
			if (this.fanCoroutine != null)
			{
				base.StopCoroutine(this.fanCoroutine);
			}
			this.fanCoroutine = null;
		}
		this.checkCoroutine = base.StartCoroutine(this.startState_cr(state));
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00064748 File Offset: 0x00062948
	public IEnumerator startState_cr(LevelProperties.Frogs.States state)
	{
		switch (state)
		{
		default:
			this.stateCoroutine = base.StartCoroutine(this.mainState_cr());
			break;
		case LevelProperties.Frogs.States.Roll:
			this.wantsToRoll = true;
			yield return base.StartCoroutine(this.waitForFrogs_cr());
			this.small.StartRoll();
			yield return base.StartCoroutine(this.waitForShort_cr());
			this.stateCoroutine = base.StartCoroutine(this.rollState_cr());
			break;
		case LevelProperties.Frogs.States.Morph:
			yield return base.StartCoroutine(this.waitForFrogs_cr());
			FrogsLevel.DEMON_TRIGGERED = this.demonTrigger.getTrigger();
			this.tall.StartMorph();
			this.small.StartMorph();
			break;
		}
		yield break;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0006476C File Offset: 0x0006296C
	public IEnumerator mainState_cr()
	{
		for (;;)
		{
			switch (this.properties.CurrentState.NextPattern)
			{
			case LevelProperties.Frogs.Pattern.TallFan:
				yield return base.StartCoroutine(this.tallFan_cr());
				break;
			case LevelProperties.Frogs.Pattern.ShortRage:
				yield return base.StartCoroutine(this.shortRage_cr());
				break;
			case LevelProperties.Frogs.Pattern.TallFireflies:
				yield return base.StartCoroutine(this.tallFireflies_cr());
				break;
			case LevelProperties.Frogs.Pattern.ShortClap:
				yield return base.StartCoroutine(this.shortClap_cr());
				break;
			case LevelProperties.Frogs.Pattern.Morph:
				goto IL_181;
			case LevelProperties.Frogs.Pattern.RagePlusFireflies:
				yield return base.StartCoroutine(this.ragePlusFireflies_cr());
				break;
			default:
				goto IL_181;
			}
			continue;
			IL_181:
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00064788 File Offset: 0x00062988
	public IEnumerator rollState_cr()
	{
		if (this.fanCoroutine != null)
		{
			base.StopCoroutine(this.fanCoroutine);
		}
		this.fanCoroutine = base.StartCoroutine(this.rollFan_cr());
		for (;;)
		{
			LevelProperties.Frogs.Pattern p = this.properties.CurrentState.NextPattern;
			if (p != LevelProperties.Frogs.Pattern.ShortClap)
			{
				if (p != LevelProperties.Frogs.Pattern.ShortRage)
				{
					yield return new WaitForSeconds(1f);
				}
				else
				{
					yield return base.StartCoroutine(this.shortRage_cr());
				}
			}
			else
			{
				yield return base.StartCoroutine(this.shortClap_cr());
			}
		}
		yield break;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x000647A4 File Offset: 0x000629A4
	public IEnumerator rollFan_cr()
	{
		float hesitate = (float)this.properties.CurrentState.tallFan.hesitate;
		yield return base.StartCoroutine(this.waitForShort_cr());
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			this.tall.StartFan();
			yield return base.StartCoroutine(this.waitForTall_cr());
			yield return CupheadTime.WaitForSeconds(this, hesitate);
		}
		yield break;
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x000647C0 File Offset: 0x000629C0
	public IEnumerator waitForFrogs_cr()
	{
		while ((this.tall.state != FrogsLevelTall.State.Complete && this.tall.state != FrogsLevelTall.State.Morphed && this.tall.state != FrogsLevelTall.State.Idle) || (this.small.state != FrogsLevelShort.State.Complete && this.small.state != FrogsLevelShort.State.Morphed && this.small.state != FrogsLevelShort.State.Idle))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002DA RID: 730 RVA: 0x000647DC File Offset: 0x000629DC
	public IEnumerator waitForTall_cr()
	{
		while (this.tall.state != FrogsLevelTall.State.Complete && this.tall.state != FrogsLevelTall.State.Morphed)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002DB RID: 731 RVA: 0x000647F8 File Offset: 0x000629F8
	public IEnumerator waitForShort_cr()
	{
		while (this.small.state != FrogsLevelShort.State.Complete && this.small.state != FrogsLevelShort.State.Morphed)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00064814 File Offset: 0x00062A14
	public IEnumerator tallFan_cr()
	{
		this.tall.StartFan();
		yield return base.StartCoroutine(this.waitForTall_cr());
		yield break;
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00064830 File Offset: 0x00062A30
	public IEnumerator tallFireflies_cr()
	{
		this.tall.StartFireflies();
		yield return base.StartCoroutine(this.waitForTall_cr());
		yield break;
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0006484C File Offset: 0x00062A4C
	public IEnumerator shortRage_cr()
	{
		this.small.StartRage();
		yield return base.StartCoroutine(this.waitForShort_cr());
		yield break;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00064868 File Offset: 0x00062A68
	public IEnumerator ragePlusFireflies_cr()
	{
		this.tall.StartFireflies();
		this.small.StartRage();
		while (!this.wantsToRoll)
		{
			if (this.tall.state == FrogsLevelTall.State.Complete)
			{
				this.tall.StartFireflies();
			}
			if (this.small.state == FrogsLevelShort.State.Complete)
			{
				this.small.StartRage();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x00064884 File Offset: 0x00062A84
	public IEnumerator shortClap_cr()
	{
		this.small.StartClap();
		yield return base.StartCoroutine(this.waitForShort_cr());
		yield break;
	}

	// Token: 0x040001E6 RID: 486
	public LevelProperties.Frogs properties;

	// Token: 0x040001E9 RID: 489
	[SerializeField]
	public FrogsLevelTall tall;

	// Token: 0x040001EA RID: 490
	[SerializeField]
	public FrogsLevelShort small;

	// Token: 0x040001EB RID: 491
	[SerializeField]
	public FrogsLevelMorphed morphed;

	// Token: 0x040001EC RID: 492
	[SerializeField]
	public FrogsLevelDemonTrigger demonTrigger;

	// Token: 0x040001ED RID: 493
	public bool wantsToRoll;

	// Token: 0x040001EE RID: 494
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040001EF RID: 495
	[SerializeField]
	public Sprite _bossPortraitRoll;

	// Token: 0x040001F0 RID: 496
	[SerializeField]
	public Sprite _bossPortraitMorph;

	// Token: 0x040001F1 RID: 497
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x040001F2 RID: 498
	[SerializeField]
	public string _bossQuoteRoll;

	// Token: 0x040001F3 RID: 499
	[SerializeField]
	public string _bossQuoteMorph;

	// Token: 0x040001F4 RID: 500
	public Coroutine checkCoroutine;

	// Token: 0x040001F5 RID: 501
	public Coroutine stateCoroutine;

	// Token: 0x040001F6 RID: 502
	public Coroutine fanCoroutine;

	// Token: 0x020007DD RID: 2013
	[Serializable]
	public class Prefabs
	{
	}
}
