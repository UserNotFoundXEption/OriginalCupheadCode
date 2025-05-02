using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class DicePalaceDominoLevel : AbstractDicePalaceLevel
{
	// Token: 0x060001B1 RID: 433 RVA: 0x00062024 File Offset: 0x00060224
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceDomino.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x060001B2 RID: 434 RVA: 0x00003F9A File Offset: 0x0000219A
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceDomino;
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x060001B3 RID: 435 RVA: 0x00003FA1 File Offset: 0x000021A1
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceDomino;
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060001B4 RID: 436 RVA: 0x00003FA8 File Offset: 0x000021A8
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_domino;
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060001B5 RID: 437 RVA: 0x00003FAC File Offset: 0x000021AC
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060001B6 RID: 438 RVA: 0x00003FB4 File Offset: 0x000021B4
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00003FBC File Offset: 0x000021BC
	public override void Start()
	{
		base.Start();
		this.domino.LevelInit(this.properties);
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x00003FD5 File Offset: 0x000021D5
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalacedominoPattern_cr());
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00003FE4 File Offset: 0x000021E4
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x060001BA RID: 442 RVA: 0x000620BC File Offset: 0x000602BC
	public IEnumerator dicepalacedominoPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001BB RID: 443 RVA: 0x000620D8 File Offset: 0x000602D8
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceDomino.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceDomino.Pattern.Boomerang)
		{
			if (p != LevelProperties.DicePalaceDomino.Pattern.BouncyBall)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				yield return base.StartCoroutine(this.bouncyball_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.boomerang_cr());
		}
		yield break;
	}

	// Token: 0x060001BC RID: 444 RVA: 0x000620F4 File Offset: 0x000602F4
	public IEnumerator boomerang_cr()
	{
		while (this.domino.state != DicePalaceDominoLevelDomino.State.Idle)
		{
			yield return null;
		}
		this.domino.OnBoomerang();
		while (this.domino.state != DicePalaceDominoLevelDomino.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00062110 File Offset: 0x00060310
	public IEnumerator bouncyball_cr()
	{
		while (this.domino.state != DicePalaceDominoLevelDomino.State.Idle)
		{
			yield return null;
		}
		this.domino.OnBouncyBall();
		while (this.domino.state != DicePalaceDominoLevelDomino.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000151 RID: 337
	public LevelProperties.DicePalaceDomino properties;

	// Token: 0x04000152 RID: 338
	[SerializeField]
	public DicePalaceDominoLevelDomino domino;

	// Token: 0x04000153 RID: 339
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000154 RID: 340
	[SerializeField]
	public string _bossQuote;
}
