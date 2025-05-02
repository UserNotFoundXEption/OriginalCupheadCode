using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class DicePalaceFlyingHorseLevel : AbstractDicePalaceLevel
{
	// Token: 0x060001CB RID: 459 RVA: 0x000621FC File Offset: 0x000603FC
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceFlyingHorse.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x060001CC RID: 460 RVA: 0x0000405C File Offset: 0x0000225C
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceFlyingHorse;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060001CD RID: 461 RVA: 0x00004063 File Offset: 0x00002263
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceFlyingHorse;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060001CE RID: 462 RVA: 0x0000406A File Offset: 0x0000226A
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_flying_horse;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x060001CF RID: 463 RVA: 0x0000406E File Offset: 0x0000226E
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x060001D0 RID: 464 RVA: 0x00004076 File Offset: 0x00002276
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0000407E File Offset: 0x0000227E
	public override void Start()
	{
		base.Start();
		this.horse.LevelInit(this.properties);
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00004097 File Offset: 0x00002297
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalaceflyinghorsePattern_cr());
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x000040A6 File Offset: 0x000022A6
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00062294 File Offset: 0x00060494
	public IEnumerator dicepalaceflyinghorsePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x000622B0 File Offset: 0x000604B0
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceFlyingHorse.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceFlyingHorse.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000159 RID: 345
	public LevelProperties.DicePalaceFlyingHorse properties;

	// Token: 0x0400015A RID: 346
	[SerializeField]
	public DicePalaceFlyingHorseLevelHorse horse;

	// Token: 0x0400015B RID: 347
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x0400015C RID: 348
	[SerializeField]
	public string _bossQuote;
}
