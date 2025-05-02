using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class FlyingBlimpLevel : Level
{
	// Token: 0x0600026D RID: 621 RVA: 0x000634E0 File Offset: 0x000616E0
	public override void PartialInit()
	{
		this.properties = LevelProperties.FlyingBlimp.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x0600026E RID: 622 RVA: 0x000044A0 File Offset: 0x000026A0
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.FlyingBlimp;
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x0600026F RID: 623 RVA: 0x000044A7 File Offset: 0x000026A7
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flying_blimp;
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000270 RID: 624 RVA: 0x00063578 File Offset: 0x00061778
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingBlimp.States.Main:
			case LevelProperties.FlyingBlimp.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.FlyingBlimp.States.Moon:
				return this._bossPortraitMoon;
			case LevelProperties.FlyingBlimp.States.Sagittarius:
			case LevelProperties.FlyingBlimp.States.Taurus:
			case LevelProperties.FlyingBlimp.States.Gemini:
			case LevelProperties.FlyingBlimp.States.SagOrGem:
				return this._bossPortraitMagical;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000271 RID: 625 RVA: 0x00063604 File Offset: 0x00061804
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingBlimp.States.Main:
			case LevelProperties.FlyingBlimp.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.FlyingBlimp.States.Moon:
				return this._bossQuoteMoon;
			case LevelProperties.FlyingBlimp.States.Sagittarius:
			case LevelProperties.FlyingBlimp.States.Taurus:
			case LevelProperties.FlyingBlimp.States.Gemini:
			case LevelProperties.FlyingBlimp.States.SagOrGem:
				return this._bossQuoteMagical;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000272 RID: 626 RVA: 0x000044AB File Offset: 0x000026AB
	public override void Start()
	{
		base.Start();
		this.blimpLady.LevelInit(this.properties);
		this.moonLady.LevelInit(this.properties);
	}

	// Token: 0x06000273 RID: 627 RVA: 0x000044D5 File Offset: 0x000026D5
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.flyingblimpPattern_cr());
		base.StartCoroutine(this.enemies_cr());
	}

	// Token: 0x06000274 RID: 628 RVA: 0x00063690 File Offset: 0x00061890
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		this.changingStates = true;
		this.StopAllCoroutines();
		base.StopCoroutine(this.blimpLady.spawnEnemy_cr());
		base.StartCoroutine(this.enemies_cr());
		if (this.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Moon)
		{
			base.StartCoroutine(this.morph_to_moon_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Sagittarius)
		{
			base.StartCoroutine(this.sagittarius_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Taurus)
		{
			base.StartCoroutine(this.taurus_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.Gemini)
		{
			base.StartCoroutine(this.gemini_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingBlimp.States.SagOrGem)
		{
			if (Rand.Bool())
			{
				base.StartCoroutine(this.sagittarius_cr());
			}
			else
			{
				base.StartCoroutine(this.gemini_cr());
			}
		}
		else
		{
			base.StartCoroutine(this.flyingblimpPattern_cr());
		}
	}

	// Token: 0x06000275 RID: 629 RVA: 0x000044F1 File Offset: 0x000026F1
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMagical = null;
		this._bossPortraitMain = null;
		this._bossPortraitMoon = null;
	}

	// Token: 0x06000276 RID: 630 RVA: 0x000637C0 File Offset: 0x000619C0
	public IEnumerator flyingblimpPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x000637DC File Offset: 0x000619DC
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.FlyingBlimp.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.FlyingBlimp.Pattern.Tornado)
		{
			if (p != LevelProperties.FlyingBlimp.Pattern.Shoot)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				yield return base.StartCoroutine(this.shoot_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.tornado_cr());
		}
		yield break;
	}

	// Token: 0x06000278 RID: 632 RVA: 0x000637F8 File Offset: 0x000619F8
	public IEnumerator tornado_cr()
	{
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		this.blimpLady.StartTornado();
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000279 RID: 633 RVA: 0x00063814 File Offset: 0x00061A14
	public IEnumerator sagittarius_cr()
	{
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		this.blimpLady.StartSagittarius();
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600027A RID: 634 RVA: 0x00063830 File Offset: 0x00061A30
	public IEnumerator taurus_cr()
	{
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		this.blimpLady.StartTaurus();
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0006384C File Offset: 0x00061A4C
	public IEnumerator gemini_cr()
	{
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		this.blimpLady.StartGemini();
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00063868 File Offset: 0x00061A68
	public IEnumerator shoot_cr()
	{
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		this.blimpLady.StartShoot();
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00063884 File Offset: 0x00061A84
	public IEnumerator enemies_cr()
	{
		if (!this.properties.CurrentState.enemy.active)
		{
			yield return null;
		}
		else
		{
			base.StartCoroutine(this.blimpLady.spawnEnemy_cr());
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600027E RID: 638 RVA: 0x000638A0 File Offset: 0x00061AA0
	public IEnumerator morph_to_moon_cr()
	{
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		this.blimpLady.SpawnMoonLady();
		while (this.blimpLady.state != FlyingBlimpLevelBlimpLady.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040001AB RID: 427
	public LevelProperties.FlyingBlimp properties;

	// Token: 0x040001AC RID: 428
	[Space(10f)]
	[SerializeField]
	public FlyingBlimpLevelBlimpLady blimpLady;

	// Token: 0x040001AD RID: 429
	[SerializeField]
	public FlyingBlimpLevelMoonLady moonLady;

	// Token: 0x040001AE RID: 430
	public bool changingStates;

	// Token: 0x040001AF RID: 431
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040001B0 RID: 432
	[SerializeField]
	public Sprite _bossPortraitMagical;

	// Token: 0x040001B1 RID: 433
	[SerializeField]
	public Sprite _bossPortraitMoon;

	// Token: 0x040001B2 RID: 434
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x040001B3 RID: 435
	[SerializeField]
	public string _bossQuoteMagical;

	// Token: 0x040001B4 RID: 436
	[SerializeField]
	public string _bossQuoteMoon;
}
