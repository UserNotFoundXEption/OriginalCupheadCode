using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class ClownLevel : Level
{
	// Token: 0x0600014A RID: 330 RVA: 0x00060E80 File Offset: 0x0005F080
	public override void PartialInit()
	{
		this.properties = LevelProperties.Clown.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x0600014B RID: 331 RVA: 0x00003CDA File Offset: 0x00001EDA
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Clown;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x0600014C RID: 332 RVA: 0x00003CE1 File Offset: 0x00001EE1
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_clown;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600014D RID: 333 RVA: 0x00060F18 File Offset: 0x0005F118
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Clown.States.Main:
			case LevelProperties.Clown.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Clown.States.HeliumTank:
				return this._bossPortraitHeliumTank;
			case LevelProperties.Clown.States.CarouselHorse:
				return this._bossPortraitCarouselHorse;
			case LevelProperties.Clown.States.Swing:
				return this._bossPortraitSwing;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x0600014E RID: 334 RVA: 0x00060FA4 File Offset: 0x0005F1A4
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Clown.States.Main:
			case LevelProperties.Clown.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Clown.States.HeliumTank:
				return this._bossQuoteHeliumTank;
			case LevelProperties.Clown.States.CarouselHorse:
				return this._bossQuoteCarouselHorse;
			case LevelProperties.Clown.States.Swing:
				return this._bossQuoteSwing;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00061030 File Offset: 0x0005F230
	public override void Start()
	{
		base.Start();
		this.coasterHandler.LevelInit(this.properties);
		this.clown.LevelInit(this.properties);
		this.clownHelium.LevelInit(this.properties);
		this.clownHorse.LevelInit(this.properties);
		this.clownSwing.LevelInit(this.properties);
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00003CE5 File Offset: 0x00001EE5
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.clownPattern_cr());
	}

	// Token: 0x06000151 RID: 337 RVA: 0x00061098 File Offset: 0x0005F298
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Clown.States.HeliumTank)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.helium_tank_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Clown.States.CarouselHorse)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.carousel_horse_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Clown.States.Swing)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.swing_cr());
		}
	}

	// Token: 0x06000152 RID: 338 RVA: 0x00003CF4 File Offset: 0x00001EF4
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitCarouselHorse = null;
		this._bossPortraitHeliumTank = null;
		this._bossPortraitMain = null;
		this._bossPortraitSwing = null;
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00061130 File Offset: 0x0005F330
	public IEnumerator clownPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000154 RID: 340 RVA: 0x0006114C File Offset: 0x0005F34C
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.Clown.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Clown.Pattern.Default)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		else
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00061168 File Offset: 0x0005F368
	public IEnumerator bumper_car_cr()
	{
		this.clown.StartBumperCar();
		yield return null;
		yield break;
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00061184 File Offset: 0x0005F384
	public IEnumerator helium_tank_cr()
	{
		this.clown.EndBumperCar();
		if (this.coasterHandler.isRunning)
		{
			this.coasterHandler.finalRun = true;
		}
		if (this.properties.CurrentState.heliumClown.coasterOn)
		{
			while (this.coasterHandler.finalRun)
			{
				yield return null;
			}
			this.coasterHandler.StartCoaster();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x000611A0 File Offset: 0x0005F3A0
	public IEnumerator carousel_horse_cr()
	{
		this.clownHelium.StartDeath();
		if (this.coasterHandler.isRunning)
		{
			this.coasterHandler.finalRun = true;
		}
		if (this.properties.CurrentState.horse.coasterOn)
		{
			while (this.coasterHandler.finalRun)
			{
				yield return null;
			}
			this.coasterHandler.StartCoaster();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000158 RID: 344 RVA: 0x000611BC File Offset: 0x0005F3BC
	public IEnumerator swing_cr()
	{
		this.clownHorse.StartDeath();
		if (this.coasterHandler.isRunning)
		{
			this.coasterHandler.finalRun = true;
		}
		while (this.coasterHandler.finalRun)
		{
			yield return null;
		}
		this.coasterHandler.StartCoaster();
		yield return null;
		yield break;
	}

	// Token: 0x04000109 RID: 265
	public LevelProperties.Clown properties;

	// Token: 0x0400010A RID: 266
	[SerializeField]
	public ClownLevelClown clown;

	// Token: 0x0400010B RID: 267
	[SerializeField]
	public ClownLevelClownHelium clownHelium;

	// Token: 0x0400010C RID: 268
	[SerializeField]
	public ClownLevelClownHorse clownHorse;

	// Token: 0x0400010D RID: 269
	[SerializeField]
	public ClownLevelClownSwing clownSwing;

	// Token: 0x0400010E RID: 270
	[SerializeField]
	public ClownLevelCoasterHandler coasterHandler;

	// Token: 0x0400010F RID: 271
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x04000110 RID: 272
	[SerializeField]
	public Sprite _bossPortraitHeliumTank;

	// Token: 0x04000111 RID: 273
	[SerializeField]
	public Sprite _bossPortraitCarouselHorse;

	// Token: 0x04000112 RID: 274
	[SerializeField]
	public Sprite _bossPortraitSwing;

	// Token: 0x04000113 RID: 275
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x04000114 RID: 276
	[SerializeField]
	public string _bossQuoteHeliumTank;

	// Token: 0x04000115 RID: 277
	[SerializeField]
	public string _bossQuoteCarouselHorse;

	// Token: 0x04000116 RID: 278
	[SerializeField]
	public string _bossQuoteSwing;
}
