using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class DicePalaceRabbitLevel : AbstractDicePalaceLevel
{
	// Token: 0x06000208 RID: 520 RVA: 0x00062718 File Offset: 0x00060918
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceRabbit.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06000209 RID: 521 RVA: 0x00004212 File Offset: 0x00002412
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceRabbit;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x0600020A RID: 522 RVA: 0x00004219 File Offset: 0x00002419
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceRabbit;
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x0600020B RID: 523 RVA: 0x00004220 File Offset: 0x00002420
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_rabbit;
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x0600020C RID: 524 RVA: 0x00004224 File Offset: 0x00002424
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x0600020D RID: 525 RVA: 0x0000422C File Offset: 0x0000242C
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x0600020E RID: 526 RVA: 0x00004234 File Offset: 0x00002434
	public override void Start()
	{
		base.Start();
		this.rabbit.LevelInit(this.properties);
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000424D File Offset: 0x0000244D
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacerabbitPattern_cr());
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000425C File Offset: 0x0000245C
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x06000211 RID: 529 RVA: 0x000627B0 File Offset: 0x000609B0
	public IEnumerator dicepalacerabbitPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000212 RID: 530 RVA: 0x000627CC File Offset: 0x000609CC
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceRabbit.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceRabbit.Pattern.MagicWand)
		{
			if (p != LevelProperties.DicePalaceRabbit.Pattern.MagicParry)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				yield return base.StartCoroutine(this.magicparry_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.magicwand_cr());
		}
		yield break;
	}

	// Token: 0x06000213 RID: 531 RVA: 0x000627E8 File Offset: 0x000609E8
	public IEnumerator magicwand_cr()
	{
		while (this.rabbit.state != DicePalaceRabbitLevelRabbit.State.Idle)
		{
			yield return null;
		}
		this.rabbit.OnMagicWand();
		while (this.rabbit.state != DicePalaceRabbitLevelRabbit.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000214 RID: 532 RVA: 0x00062804 File Offset: 0x00060A04
	public IEnumerator magicparry_cr()
	{
		while (this.rabbit.state != DicePalaceRabbitLevelRabbit.State.Idle)
		{
			yield return null;
		}
		this.rabbit.OnMagicParry();
		while (this.rabbit.state != DicePalaceRabbitLevelRabbit.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000171 RID: 369
	public LevelProperties.DicePalaceRabbit properties;

	// Token: 0x04000172 RID: 370
	[SerializeField]
	public DicePalaceRabbitLevelRabbit rabbit;

	// Token: 0x04000173 RID: 371
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000174 RID: 372
	[SerializeField]
	public string _bossQuote;
}
