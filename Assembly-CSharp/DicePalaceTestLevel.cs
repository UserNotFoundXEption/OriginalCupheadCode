using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class DicePalaceTestLevel : AbstractDicePalaceLevel
{
	// Token: 0x06000224 RID: 548 RVA: 0x00062984 File Offset: 0x00060B84
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceTest.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06000225 RID: 549 RVA: 0x000042BB File Offset: 0x000024BB
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceTest;
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06000226 RID: 550 RVA: 0x000042C2 File Offset: 0x000024C2
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceTest;
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06000227 RID: 551 RVA: 0x000042C9 File Offset: 0x000024C9
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_test;
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000228 RID: 552 RVA: 0x000042CD File Offset: 0x000024CD
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x06000229 RID: 553 RVA: 0x000042D5 File Offset: 0x000024D5
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600022A RID: 554 RVA: 0x000042DD File Offset: 0x000024DD
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x0600022B RID: 555 RVA: 0x000042E5 File Offset: 0x000024E5
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacetestPattern_cr());
		base.StartCoroutine(this.test.start_it_cr());
	}

	// Token: 0x0600022C RID: 556 RVA: 0x00062A1C File Offset: 0x00060C1C
	public IEnumerator dicepalacetestPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600022D RID: 557 RVA: 0x00062A38 File Offset: 0x00060C38
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceTest.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x0400017A RID: 378
	public LevelProperties.DicePalaceTest properties;

	// Token: 0x0400017B RID: 379
	[SerializeField]
	public DicePalaceTestLevelTest test;

	// Token: 0x0400017C RID: 380
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x0400017D RID: 381
	[SerializeField]
	public string _bossQuote;
}
