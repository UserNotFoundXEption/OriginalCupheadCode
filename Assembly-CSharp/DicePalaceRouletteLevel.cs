using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class DicePalaceRouletteLevel : AbstractDicePalaceLevel
{
	// Token: 0x06000216 RID: 534 RVA: 0x00062820 File Offset: 0x00060A20
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceRoulette.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06000217 RID: 535 RVA: 0x00004273 File Offset: 0x00002473
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceRoulette;
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x06000218 RID: 536 RVA: 0x0000427A File Offset: 0x0000247A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceRoulette;
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x06000219 RID: 537 RVA: 0x00004281 File Offset: 0x00002481
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_roulette;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x0600021A RID: 538 RVA: 0x00004285 File Offset: 0x00002485
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x0600021B RID: 539 RVA: 0x0000428D File Offset: 0x0000248D
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600021C RID: 540 RVA: 0x000628B8 File Offset: 0x00060AB8
	public override void Start()
	{
		base.Start();
		this.roulette.LevelInit(this.properties);
		foreach (DicePalaceRouletteLevelPlatform dicePalaceRouletteLevelPlatform in this.platforms)
		{
			dicePalaceRouletteLevelPlatform.Init(this.properties.CurrentState.platform);
		}
	}

	// Token: 0x0600021D RID: 541 RVA: 0x00004295 File Offset: 0x00002495
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalaceroulettePattern_cr());
	}

	// Token: 0x0600021E RID: 542 RVA: 0x000042A4 File Offset: 0x000024A4
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x0600021F RID: 543 RVA: 0x00062914 File Offset: 0x00060B14
	public IEnumerator dicepalaceroulettePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000220 RID: 544 RVA: 0x00062930 File Offset: 0x00060B30
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceRoulette.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceRoulette.Pattern.Twirl)
		{
			if (p != LevelProperties.DicePalaceRoulette.Pattern.Marble)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				yield return base.StartCoroutine(this.marble_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.twirl_cr());
		}
		yield break;
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0006294C File Offset: 0x00060B4C
	public IEnumerator twirl_cr()
	{
		while (this.roulette.state != DicePalaceRouletteLevelRoulette.State.Idle)
		{
			yield return null;
		}
		this.roulette.StartTwirl();
		while (this.roulette.state != DicePalaceRouletteLevelRoulette.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000222 RID: 546 RVA: 0x00062968 File Offset: 0x00060B68
	public IEnumerator marble_cr()
	{
		while (this.roulette.state != DicePalaceRouletteLevelRoulette.State.Idle)
		{
			yield return null;
		}
		this.roulette.StartMarbleDrop();
		while (this.roulette.state != DicePalaceRouletteLevelRoulette.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000175 RID: 373
	public LevelProperties.DicePalaceRoulette properties;

	// Token: 0x04000176 RID: 374
	[SerializeField]
	public DicePalaceRouletteLevelRoulette roulette;

	// Token: 0x04000177 RID: 375
	[SerializeField]
	public DicePalaceRouletteLevelPlatform[] platforms;

	// Token: 0x04000178 RID: 376
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000179 RID: 377
	[SerializeField]
	public string _bossQuote;
}
