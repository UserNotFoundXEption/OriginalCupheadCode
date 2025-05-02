using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class FlyingMermaidLevel : Level
{
	// Token: 0x060002A4 RID: 676 RVA: 0x0006402C File Offset: 0x0006222C
	public override void PartialInit()
	{
		this.properties = LevelProperties.FlyingMermaid.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060002A5 RID: 677 RVA: 0x00004613 File Offset: 0x00002813
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.FlyingMermaid;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000461A File Offset: 0x0000281A
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flying_mermaid;
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000461E File Offset: 0x0000281E
	// (set) Token: 0x060002A8 RID: 680 RVA: 0x00004626 File Offset: 0x00002826
	public bool MerdusaTransformStarted { get; set; }

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x060002A9 RID: 681 RVA: 0x000640C4 File Offset: 0x000622C4
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingMermaid.States.Main:
			case LevelProperties.FlyingMermaid.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.FlyingMermaid.States.Merdusa:
				return this._bossPortraitMerdusa;
			case LevelProperties.FlyingMermaid.States.Head:
				return this._bossPortraitHead;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x060002AA RID: 682 RVA: 0x00064144 File Offset: 0x00062344
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingMermaid.States.Main:
			case LevelProperties.FlyingMermaid.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.FlyingMermaid.States.Merdusa:
				return this._bossQuoteMerdusa;
			case LevelProperties.FlyingMermaid.States.Head:
				return this._bossQuoteHead;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x060002AB RID: 683 RVA: 0x000641C4 File Offset: 0x000623C4
	public override void Start()
	{
		base.Start();
		this.mermaid.LevelInit(this.properties);
		this.merdusa.LevelInit(this.properties);
		this.merdusaHead.LevelInit(this.properties);
		this.MerdusaTransformStarted = false;
	}

	// Token: 0x060002AC RID: 684 RVA: 0x0000462F File Offset: 0x0000282F
	public override void OnLevelStart()
	{
		this.mermaid.IntroContinue();
		base.StartCoroutine(this.mermaidPattern_cr());
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00064214 File Offset: 0x00062414
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.FlyingMermaid.States.Merdusa)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.transform_to_merdusa_cr());
		}
		if (this.properties.CurrentState.stateName == LevelProperties.FlyingMermaid.States.Head)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.mermaidPattern_cr());
			base.StartCoroutine(this.transform_to_head_cr());
		}
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00004649 File Offset: 0x00002849
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitHead = null;
		this._bossPortraitMain = null;
		this._bossPortraitMerdusa = null;
	}

	// Token: 0x060002AF RID: 687 RVA: 0x00064288 File Offset: 0x00062488
	public IEnumerator mermaidPattern_cr()
	{
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x000642A4 File Offset: 0x000624A4
	public IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.FlyingMermaid.Pattern.Yell:
			yield return base.StartCoroutine(this.yell_cr());
			break;
		case LevelProperties.FlyingMermaid.Pattern.Summon:
			yield return base.StartCoroutine(this.summon_cr());
			break;
		case LevelProperties.FlyingMermaid.Pattern.Fish:
			yield return base.StartCoroutine(this.fish_cr());
			break;
		case LevelProperties.FlyingMermaid.Pattern.Zap:
			yield return base.StartCoroutine(this.zap_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		case LevelProperties.FlyingMermaid.Pattern.Bubble:
			yield return base.StartCoroutine(this.bubble_cr());
			break;
		case LevelProperties.FlyingMermaid.Pattern.HeadBlast:
			yield return base.StartCoroutine(this.head_blast_cr());
			break;
		case LevelProperties.FlyingMermaid.Pattern.BubbleHeadBlast:
			yield return base.StartCoroutine(this.bubble_head_blast_cr());
			break;
		}
		yield break;
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x000642C0 File Offset: 0x000624C0
	public IEnumerator yell_cr()
	{
		while (this.mermaid.state != FlyingMermaidLevelMermaid.State.Idle)
		{
			yield return null;
		}
		this.mermaid.StartYell();
		while (this.mermaid.state != FlyingMermaidLevelMermaid.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x000642DC File Offset: 0x000624DC
	public IEnumerator transform_to_merdusa_cr()
	{
		this.mermaid.StartTransform();
		while (this.merdusa.state != FlyingMermaidLevelMerdusa.State.Idle)
		{
			yield return null;
		}
		base.StartCoroutine(this.mermaidPattern_cr());
		yield break;
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x000642F8 File Offset: 0x000624F8
	public IEnumerator transform_to_head_cr()
	{
		this.merdusa.StartTransform();
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00064314 File Offset: 0x00062514
	public IEnumerator summon_cr()
	{
		while (this.mermaid.state != FlyingMermaidLevelMermaid.State.Idle)
		{
			yield return null;
		}
		this.mermaid.StartSummon();
		while (this.mermaid.state != FlyingMermaidLevelMermaid.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00064330 File Offset: 0x00062530
	public IEnumerator fish_cr()
	{
		while (this.mermaid.state != FlyingMermaidLevelMermaid.State.Idle)
		{
			yield return null;
		}
		this.mermaid.StartFish();
		while (this.mermaid.state != FlyingMermaidLevelMermaid.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x0006434C File Offset: 0x0006254C
	public IEnumerator zap_cr()
	{
		while (this.merdusa.state != FlyingMermaidLevelMerdusa.State.Idle)
		{
			yield return null;
		}
		this.merdusa.StartZap();
		while (this.merdusa.state != FlyingMermaidLevelMerdusa.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00064368 File Offset: 0x00062568
	public IEnumerator bubble_cr()
	{
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		this.merdusaHead.StartBubble();
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00064384 File Offset: 0x00062584
	public IEnumerator head_blast_cr()
	{
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		this.merdusaHead.StartHeadBlast();
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x000643A0 File Offset: 0x000625A0
	public IEnumerator bubble_head_blast_cr()
	{
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		this.merdusaHead.StartHeadBubble();
		while (this.merdusaHead.state != FlyingMermaidLevelMerdusaHead.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040001D7 RID: 471
	public LevelProperties.FlyingMermaid properties;

	// Token: 0x040001D8 RID: 472
	[SerializeField]
	public FlyingMermaidLevelMermaid mermaid;

	// Token: 0x040001D9 RID: 473
	[Header("FlyingMermaidLevel")]
	[SerializeField]
	public FlyingMermaidLevel.Prefabs prefabs;

	// Token: 0x040001DA RID: 474
	[SerializeField]
	public FlyingMermaidLevelMerdusa merdusa;

	// Token: 0x040001DB RID: 475
	[SerializeField]
	public FlyingMermaidLevelMerdusaHead merdusaHead;

	// Token: 0x040001DD RID: 477
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040001DE RID: 478
	[SerializeField]
	public Sprite _bossPortraitMerdusa;

	// Token: 0x040001DF RID: 479
	[SerializeField]
	public Sprite _bossPortraitHead;

	// Token: 0x040001E0 RID: 480
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x040001E1 RID: 481
	[SerializeField]
	public string _bossQuoteMerdusa;

	// Token: 0x040001E2 RID: 482
	[SerializeField]
	public string _bossQuoteHead;

	// Token: 0x020007CE RID: 1998
	[Serializable]
	public class Prefabs
	{
	}
}
