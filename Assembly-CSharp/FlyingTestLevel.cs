using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class FlyingTestLevel : Level
{
	// Token: 0x060002BB RID: 699 RVA: 0x000643BC File Offset: 0x000625BC
	public override void PartialInit()
	{
		this.properties = LevelProperties.FlyingTest.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x060002BC RID: 700 RVA: 0x0000466E File Offset: 0x0000286E
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.FlyingTest;
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x060002BD RID: 701 RVA: 0x00004675 File Offset: 0x00002875
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flying_test;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060002BE RID: 702 RVA: 0x00004679 File Offset: 0x00002879
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x060002BF RID: 703 RVA: 0x00004681 File Offset: 0x00002881
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00004689 File Offset: 0x00002889
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00004691 File Offset: 0x00002891
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.flyingtestPattern_cr());
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00064454 File Offset: 0x00062654
	public IEnumerator flyingtestPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00064470 File Offset: 0x00062670
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.FlyingTest.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x040001E3 RID: 483
	public LevelProperties.FlyingTest properties;

	// Token: 0x040001E4 RID: 484
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x040001E5 RID: 485
	[SerializeField]
	[Multiline]
	public string _bossQuote;

	// Token: 0x020007DA RID: 2010
	[Serializable]
	public class Prefabs
	{
	}
}
