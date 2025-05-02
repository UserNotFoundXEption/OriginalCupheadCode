using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class DicePalaceFlyingMemoryLevel : AbstractDicePalaceLevel
{
	// Token: 0x060001D7 RID: 471 RVA: 0x000622CC File Offset: 0x000604CC
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceFlyingMemory.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x060001D8 RID: 472 RVA: 0x000040BD File Offset: 0x000022BD
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceFlyingMemory;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x060001D9 RID: 473 RVA: 0x000040C4 File Offset: 0x000022C4
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceFlyingMemory;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x060001DA RID: 474 RVA: 0x000040CB File Offset: 0x000022CB
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_flying_memory;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x060001DB RID: 475 RVA: 0x000040CF File Offset: 0x000022CF
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x060001DC RID: 476 RVA: 0x000040D7 File Offset: 0x000022D7
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060001DD RID: 477 RVA: 0x000040DF File Offset: 0x000022DF
	public override void Start()
	{
		base.Start();
		this.gameManager.LevelInit(this.properties);
		this.stuffedToy.LevelInit(this.properties);
	}

	// Token: 0x060001DE RID: 478 RVA: 0x00004109 File Offset: 0x00002309
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalaceflyingmemoryPattern_cr());
	}

	// Token: 0x060001DF RID: 479 RVA: 0x00004118 File Offset: 0x00002318
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x00062364 File Offset: 0x00060564
	public IEnumerator dicepalaceflyingmemoryPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x00062380 File Offset: 0x00060580
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceFlyingMemory.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceFlyingMemory.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400015D RID: 349
	public LevelProperties.DicePalaceFlyingMemory properties;

	// Token: 0x0400015E RID: 350
	[SerializeField]
	public DicePalaceFlyingMemoryLevelStuffedToy stuffedToy;

	// Token: 0x0400015F RID: 351
	[SerializeField]
	public DicePalaceFlyingMemoryLevelGameManager gameManager;

	// Token: 0x04000160 RID: 352
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000161 RID: 353
	[SerializeField]
	public string _bossQuote;
}
