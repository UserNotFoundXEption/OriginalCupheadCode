using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class DicePalaceCigarLevel : AbstractDicePalaceLevel
{
	// Token: 0x060001A5 RID: 421 RVA: 0x00061F54 File Offset: 0x00060154
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceCigar.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x060001A6 RID: 422 RVA: 0x00003F39 File Offset: 0x00002139
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceCigar;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x060001A7 RID: 423 RVA: 0x00003F40 File Offset: 0x00002140
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceCigar;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060001A8 RID: 424 RVA: 0x00003F47 File Offset: 0x00002147
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_cigar;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060001A9 RID: 425 RVA: 0x00003F4B File Offset: 0x0000214B
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060001AA RID: 426 RVA: 0x00003F53 File Offset: 0x00002153
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060001AB RID: 427 RVA: 0x00003F5B File Offset: 0x0000215B
	public override void Start()
	{
		base.Start();
		this.cigar.LevelInit(this.properties);
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00003F74 File Offset: 0x00002174
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacecigarPattern_cr());
	}

	// Token: 0x060001AD RID: 429 RVA: 0x00003F83 File Offset: 0x00002183
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x00061FEC File Offset: 0x000601EC
	public IEnumerator dicepalacecigarPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00062008 File Offset: 0x00060208
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceCigar.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceCigar.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400014C RID: 332
	public LevelProperties.DicePalaceCigar properties;

	// Token: 0x0400014D RID: 333
	[SerializeField]
	public DicePalaceCigarLevelCigar cigar;

	// Token: 0x0400014E RID: 334
	[SerializeField]
	public Animator fire;

	// Token: 0x0400014F RID: 335
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000150 RID: 336
	[SerializeField]
	public string _bossQuote;
}
