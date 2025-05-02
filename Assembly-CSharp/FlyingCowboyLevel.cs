using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class FlyingCowboyLevel : Level
{
	// Token: 0x06000280 RID: 640 RVA: 0x000638BC File Offset: 0x00061ABC
	public override void PartialInit()
	{
		this.properties = LevelProperties.FlyingCowboy.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06000281 RID: 641 RVA: 0x00004522 File Offset: 0x00002722
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.FlyingCowboy;
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06000282 RID: 642 RVA: 0x00004529 File Offset: 0x00002729
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flying_cowboy;
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06000283 RID: 643 RVA: 0x00063954 File Offset: 0x00061B54
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingCowboy.States.Main:
				return this._bossPortraitMain;
			case LevelProperties.FlyingCowboy.States.Vacuum:
				return this._bossPortraitPhaseTwo;
			case LevelProperties.FlyingCowboy.States.Sausage:
				return this._bossPortraitPhaseFour;
			case LevelProperties.FlyingCowboy.States.Meatball:
				return this._bossPortraitPhaseThree;
			}
			Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000284 RID: 644 RVA: 0x000639E0 File Offset: 0x00061BE0
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingCowboy.States.Main:
				return this._bossQuoteMain;
			case LevelProperties.FlyingCowboy.States.Vacuum:
				return this._bossQuotePhaseTwo;
			case LevelProperties.FlyingCowboy.States.Sausage:
				return this._bossQuotePhaseFour;
			case LevelProperties.FlyingCowboy.States.Meatball:
				return this._bossQuotePhaseThree;
			}
			Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x06000285 RID: 645 RVA: 0x0000452D File Offset: 0x0000272D
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
		this._bossPortraitPhaseFour = null;
	}

	// Token: 0x06000286 RID: 646 RVA: 0x00063A6C File Offset: 0x00061C6C
	public override void Start()
	{
		base.Start();
		this.cowboy.LevelInit(this.properties);
		this.meat.LevelInit(this.properties);
		this.playerDusts[0] = Object.Instantiate<PlanePlayerDust>(this.playerDust);
		this.playerDusts[1] = Object.Instantiate<PlanePlayerDust>(this.playerDust);
		this.playerDusts[0].Initialize(this.players[0], this.playerDustSmallTrigger, this.playerDustLargeTrigger);
		this.playerDusts[1].Initialize(this.players[1], this.playerDustSmallTrigger, this.playerDustLargeTrigger);
	}

	// Token: 0x06000287 RID: 647 RVA: 0x00004551 File Offset: 0x00002751
	public override void CreatePlayerTwoOnJoin()
	{
		base.CreatePlayerTwoOnJoin();
		this.playerDusts[1].Initialize(this.players[1], this.playerDustSmallTrigger, this.playerDustLargeTrigger);
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00063B0C File Offset: 0x00061D0C
	public override void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(new Vector3(-1000f, this.playerDustSmallTrigger), Vector3.right * 2000f);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(new Vector3(-1000f, this.playerDustLargeTrigger), Vector3.right * 2000f);
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00063B78 File Offset: 0x00061D78
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.FlyingCowboy.States.Vacuum)
		{
			base.StartCoroutine(this.toPhase2_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingCowboy.States.Meatball)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.toPhase3_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingCowboy.States.Sausage)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.toPhase4_cr());
		}
	}

	// Token: 0x0600028A RID: 650 RVA: 0x00063C0C File Offset: 0x00061E0C
	public IEnumerator toPhase2_cr()
	{
		LevelProperties.FlyingCowboy.Pattern pattern = this.properties.CurrentState.PeekNextPattern;
		this.cowboy.OnPhase2(pattern);
		yield return this.cowboy.animator.WaitForAnimationToStart(this, "Ph1_To_Ph2", false);
		while (this.cowboy.state == FlyingCowboyLevelCowboy.State.PhaseTrans)
		{
			yield return null;
		}
		base.StartCoroutine(this.phase2Loop_cr());
		yield break;
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00063C28 File Offset: 0x00061E28
	public IEnumerator phase2Loop_cr()
	{
		bool initial = true;
		for (;;)
		{
			LevelProperties.FlyingCowboy.Pattern p = this.properties.CurrentState.NextPattern;
			if (p == LevelProperties.FlyingCowboy.Pattern.Vacuum)
			{
				yield return base.StartCoroutine(this.vacuum_cr(initial));
			}
			else
			{
				yield return base.StartCoroutine(this.ricochet_cr(initial));
			}
			initial = false;
		}
		yield break;
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00063C44 File Offset: 0x00061E44
	public IEnumerator vacuum_cr(bool initial)
	{
		while (this.cowboy.state != FlyingCowboyLevelCowboy.State.Idle)
		{
			yield return null;
		}
		this.cowboy.Vacuum(initial, LevelProperties.FlyingCowboy.Pattern.Default);
		while (this.cowboy.state != FlyingCowboyLevelCowboy.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00063C68 File Offset: 0x00061E68
	public IEnumerator ricochet_cr(bool initial)
	{
		if (initial && this.properties.CurrentState.ricochet.useRicochet)
		{
			this.cowboy.animator.SetBool("OnRicochet", true);
		}
		while (this.cowboy.state != FlyingCowboyLevelCowboy.State.Idle)
		{
			yield return null;
		}
		if (this.properties.CurrentState.ricochet.useRicochet)
		{
			this.cowboy.Ricochet();
		}
		while (this.cowboy.state != FlyingCowboyLevelCowboy.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600028E RID: 654 RVA: 0x00063C8C File Offset: 0x00061E8C
	public IEnumerator toPhase3_cr()
	{
		this.background.BeginTransition();
		if (this.cowboy != null)
		{
			this.cowboy.Death();
		}
		while (!this.cowboy.IsDead)
		{
			yield return null;
		}
		this.meat.SelectPhase(FlyingCowboyLevelMeat.MeatPhase.Sausage);
		yield break;
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00063CA8 File Offset: 0x00061EA8
	public IEnumerator toPhase4_cr()
	{
		while (this.cowboy.state != FlyingCowboyLevelCowboy.State.Idle)
		{
			yield return null;
		}
		this.meat.SelectPhase(FlyingCowboyLevelMeat.MeatPhase.Can);
		yield break;
	}

	// Token: 0x040001B5 RID: 437
	public LevelProperties.FlyingCowboy properties;

	// Token: 0x040001B6 RID: 438
	[SerializeField]
	public FlyingCowboyLevelCowboy cowboy;

	// Token: 0x040001B7 RID: 439
	[SerializeField]
	public FlyingCowboyLevelMeat meat;

	// Token: 0x040001B8 RID: 440
	[SerializeField]
	public FlyingCowboyLevelBackground background;

	// Token: 0x040001B9 RID: 441
	[SerializeField]
	public PlanePlayerDust playerDust;

	// Token: 0x040001BA RID: 442
	[SerializeField]
	public float playerDustSmallTrigger;

	// Token: 0x040001BB RID: 443
	[SerializeField]
	public float playerDustLargeTrigger;

	// Token: 0x040001BC RID: 444
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040001BD RID: 445
	[SerializeField]
	public Sprite _bossPortraitPhaseTwo;

	// Token: 0x040001BE RID: 446
	[SerializeField]
	public Sprite _bossPortraitPhaseThree;

	// Token: 0x040001BF RID: 447
	[SerializeField]
	public Sprite _bossPortraitPhaseFour;

	// Token: 0x040001C0 RID: 448
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x040001C1 RID: 449
	[SerializeField]
	public string _bossQuotePhaseTwo;

	// Token: 0x040001C2 RID: 450
	[SerializeField]
	public string _bossQuotePhaseThree;

	// Token: 0x040001C3 RID: 451
	[SerializeField]
	public string _bossQuotePhaseFour;

	// Token: 0x040001C4 RID: 452
	public PlanePlayerDust[] playerDusts = new PlanePlayerDust[2];
}
