using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class DicePalaceLightLevel : AbstractDicePalaceLevel
{
	// Token: 0x060001E3 RID: 483 RVA: 0x0006239C File Offset: 0x0006059C
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceLight.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000412F File Offset: 0x0000232F
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceLight;
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x060001E5 RID: 485 RVA: 0x00004136 File Offset: 0x00002336
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceLight;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000413D File Offset: 0x0000233D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_light;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x060001E7 RID: 487 RVA: 0x00004141 File Offset: 0x00002341
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060001E8 RID: 488 RVA: 0x00004149 File Offset: 0x00002349
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x00004151 File Offset: 0x00002351
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x060001EA RID: 490 RVA: 0x00004159 File Offset: 0x00002359
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacelightPattern_cr());
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00062434 File Offset: 0x00060634
	public IEnumerator dicepalacelightPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00062450 File Offset: 0x00060650
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceLight.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x04000162 RID: 354
	public LevelProperties.DicePalaceLight properties;

	// Token: 0x04000163 RID: 355
	[SerializeField]
	public RumRunnersLevelWorm lightBoss;

	// Token: 0x04000164 RID: 356
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000165 RID: 357
	[SerializeField]
	public string _bossQuote;
}
