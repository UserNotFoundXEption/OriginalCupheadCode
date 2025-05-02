using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class DicePalaceEightBallLevel : AbstractDicePalaceLevel
{
	// Token: 0x060001BF RID: 447 RVA: 0x0006212C File Offset: 0x0006032C
	public override void PartialInit()
	{
		this.properties = LevelProperties.DicePalaceEightBall.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060001C0 RID: 448 RVA: 0x00003FFB File Offset: 0x000021FB
	public override DicePalaceLevels CurrentDicePalaceLevel
	{
		get
		{
			return DicePalaceLevels.DicePalaceEightBall;
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x060001C1 RID: 449 RVA: 0x00004002 File Offset: 0x00002202
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.DicePalaceEightBall;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x060001C2 RID: 450 RVA: 0x00004009 File Offset: 0x00002209
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dice_palace_eight_ball;
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000400D File Offset: 0x0000220D
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x060001C4 RID: 452 RVA: 0x00004015 File Offset: 0x00002215
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000401D File Offset: 0x0000221D
	public override void Start()
	{
		base.Start();
		this.eightBall.LevelInit(this.properties);
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00004036 File Offset: 0x00002236
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.dicepalaceeightballPattern_cr());
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00004045 File Offset: 0x00002245
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortrait = null;
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x000621C4 File Offset: 0x000603C4
	public IEnumerator dicepalaceeightballPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x000621E0 File Offset: 0x000603E0
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.DicePalaceEightBall.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.DicePalaceEightBall.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000155 RID: 341
	public LevelProperties.DicePalaceEightBall properties;

	// Token: 0x04000156 RID: 342
	[SerializeField]
	public DicePalaceEightBallLevelEightBall eightBall;

	// Token: 0x04000157 RID: 343
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortrait;

	// Token: 0x04000158 RID: 344
	[SerializeField]
	public string _bossQuote;
}
