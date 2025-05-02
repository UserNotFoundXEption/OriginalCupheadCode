using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class FlyingBirdLevel : Level
{
	// Token: 0x06000257 RID: 599 RVA: 0x000630D8 File Offset: 0x000612D8
	public override void PartialInit()
	{
		this.properties = LevelProperties.FlyingBird.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000258 RID: 600 RVA: 0x00004406 File Offset: 0x00002606
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.FlyingBird;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000259 RID: 601 RVA: 0x0000440D File Offset: 0x0000260D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_flying_bird;
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x0600025A RID: 602 RVA: 0x00063170 File Offset: 0x00061370
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingBird.States.Main:
			case LevelProperties.FlyingBird.States.Generic:
			case LevelProperties.FlyingBird.States.Whistle:
				return this._bossPortraitMain;
			case LevelProperties.FlyingBird.States.HouseDeath:
				return this._bossPortraitHouseDeath;
			case LevelProperties.FlyingBird.States.BirdRevival:
				return this._bossPortraitBirdRevival;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x0600025B RID: 603 RVA: 0x000631F4 File Offset: 0x000613F4
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.FlyingBird.States.Main:
			case LevelProperties.FlyingBird.States.Generic:
			case LevelProperties.FlyingBird.States.Whistle:
				return this._bossQuoteMain;
			case LevelProperties.FlyingBird.States.HouseDeath:
				return this._bossQuoteHouseDeath;
			case LevelProperties.FlyingBird.States.BirdRevival:
				return this._bossQuoteBirdRevival;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x0600025C RID: 604 RVA: 0x00004411 File Offset: 0x00002611
	public override void Start()
	{
		base.Start();
		this.bird.LevelInit(this.properties);
		this.smallBird.LevelInit(this.properties);
		this.skybirdPattern = this.skybirdPattern_cr();
	}

	// Token: 0x0600025D RID: 605 RVA: 0x00004447 File Offset: 0x00002647
	public override void OnLevelStart()
	{
		this.bird.IntroContinue();
		base.StartCoroutine(this.skybirdPattern_cr());
		base.StartCoroutine(this.enemies_cr());
		base.StartCoroutine(this.turrets_cr());
	}

	// Token: 0x0600025E RID: 606 RVA: 0x00063278 File Offset: 0x00061478
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.FlyingBird.States.HouseDeath)
		{
			base.StopCoroutine(this.skybirdPattern);
			base.StartCoroutine(this.houseDie_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.FlyingBird.States.BirdRevival)
		{
			base.StopCoroutine(this.skybirdPattern);
			base.StartCoroutine(this.birdHouseRevival_cr());
		}
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000447B File Offset: 0x0000267B
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitBirdRevival = null;
		this._bossPortraitHouseDeath = null;
		this._bossPortraitMain = null;
	}

	// Token: 0x06000260 RID: 608 RVA: 0x000632F0 File Offset: 0x000614F0
	public IEnumerator skybirdPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0006330C File Offset: 0x0006150C
	public IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.FlyingBird.Pattern.Feathers:
			yield return base.StartCoroutine(this.feathers_cr());
			break;
		case LevelProperties.FlyingBird.Pattern.Eggs:
			yield return base.StartCoroutine(this.eggs_cr());
			break;
		case LevelProperties.FlyingBird.Pattern.Lasers:
			yield return base.StartCoroutine(this.lasers_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		case LevelProperties.FlyingBird.Pattern.Garbage:
			yield return base.StartCoroutine(this.garbage_cr());
			break;
		case LevelProperties.FlyingBird.Pattern.Heart:
			yield return base.StartCoroutine(this.heartAttack_cr());
			break;
		}
		yield break;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x00063328 File Offset: 0x00061528
	public IEnumerator feathers_cr()
	{
		while (this.bird.state != FlyingBirdLevelBird.State.Idle)
		{
			yield return null;
		}
		this.bird.StartFeathers();
		while (this.bird.state != FlyingBirdLevelBird.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000263 RID: 611 RVA: 0x00063344 File Offset: 0x00061544
	public IEnumerator eggs_cr()
	{
		while (this.bird.state != FlyingBirdLevelBird.State.Idle)
		{
			yield return null;
		}
		this.bird.StartEggs();
		while (this.bird.state != FlyingBirdLevelBird.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000264 RID: 612 RVA: 0x00063360 File Offset: 0x00061560
	public IEnumerator lasers_cr()
	{
		while (this.bird.state != FlyingBirdLevelBird.State.Idle)
		{
			yield return null;
		}
		this.bird.StartLasers();
		while (this.bird.state != FlyingBirdLevelBird.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0006337C File Offset: 0x0006157C
	public IEnumerator houseDie_cr()
	{
		this.bird.BirdFall();
		yield return null;
		yield break;
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00063398 File Offset: 0x00061598
	public IEnumerator enemies_cr()
	{
		bool firstTime = true;
		AbstractPlayerController target = PlayerManager.GetNext();
		int r = 1;
		for (;;)
		{
			if (!this.properties.CurrentState.enemies.active)
			{
				firstTime = true;
				while (!this.properties.CurrentState.enemies.active)
				{
					yield return null;
				}
			}
			LevelProperties.FlyingBird.Enemies properties = this.properties.CurrentState.enemies;
			int i = 0;
			FlyingBirdLevelEnemy.Properties p = new FlyingBirdLevelEnemy.Properties(properties.health, properties.speed, properties.floatRange, properties.floatTime, properties.projectileHeight, properties.projectileFallTime, properties.projectileDelay);
			target = PlayerManager.GetNext();
			Vector2 pos = this.enemyRoot.position;
			if (!this.properties.CurrentState.enemies.aim)
			{
				pos.y *= (float)r;
				r *= -1;
			}
			else
			{
				pos.y = target.center.y;
			}
			while (i < properties.count)
			{
				yield return CupheadTime.WaitForSeconds(this, properties.delay);
				bool parryable = i == properties.count - 1;
				this.prefabs.formationBird.Create(pos, p).SetParryable(parryable);
				i++;
			}
			yield return CupheadTime.WaitForSeconds(this, firstTime ? properties.initalGroupDelay : properties.groupDelay);
			firstTime = false;
		}
		yield break;
	}

	// Token: 0x06000267 RID: 615 RVA: 0x000633B4 File Offset: 0x000615B4
	public IEnumerator turrets_cr()
	{
		FlyingBirdLevelTurret top = null;
		FlyingBirdLevelTurret bottom = null;
		for (;;)
		{
			if (!this.properties.CurrentState.turrets.active)
			{
				while (!this.properties.CurrentState.turrets.active)
				{
					yield return null;
				}
			}
			if (top == null || top.transform == null || top.state == FlyingBirdLevelTurret.State.Respawn)
			{
				top = this.CreateTurret(this.turretRootTop.position);
			}
			if (bottom == null || bottom.transform == null || bottom.state == FlyingBirdLevelTurret.State.Respawn)
			{
				bottom = this.CreateTurret(this.turretRootBottom.position);
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.turrets.respawnDelay);
		}
		yield break;
	}

	// Token: 0x06000268 RID: 616 RVA: 0x000633D0 File Offset: 0x000615D0
	public IEnumerator birdHouseRevival_cr()
	{
		while (this.smallBird.isActiveAndEnabled)
		{
			yield return null;
		}
		this.bird.OnBossRevival();
		while (this.bird.state == FlyingBirdLevelBird.State.Reviving)
		{
			yield return null;
		}
		base.StartCoroutine(this.skybirdPattern_cr());
		yield break;
	}

	// Token: 0x06000269 RID: 617 RVA: 0x000633EC File Offset: 0x000615EC
	public IEnumerator garbage_cr()
	{
		while (this.bird.state != FlyingBirdLevelBird.State.Revived)
		{
			yield return null;
		}
		this.bird.StartGarbageOne();
		while (this.bird.state != FlyingBirdLevelBird.State.Revived)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00063408 File Offset: 0x00061608
	public IEnumerator heartAttack_cr()
	{
		while (this.bird.state != FlyingBirdLevelBird.State.Revived)
		{
			yield return null;
		}
		this.bird.StartHeartAttack();
		while (this.bird.state != FlyingBirdLevelBird.State.Revived)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00063424 File Offset: 0x00061624
	public FlyingBirdLevelTurret CreateTurret(Vector2 pos)
	{
		FlyingBirdLevelTurret.Properties properties = new FlyingBirdLevelTurret.Properties((float)this.properties.CurrentState.turrets.health, this.properties.CurrentState.turrets.inTime, pos.x, this.properties.CurrentState.turrets.bulletSpeed, this.properties.CurrentState.turrets.bulletDelay, this.properties.CurrentState.turrets.floatRange, this.properties.CurrentState.turrets.floatTime);
		return this.prefabs.turretBird.Create(new Vector2(690f, pos.y), properties);
	}

	// Token: 0x0400019D RID: 413
	public LevelProperties.FlyingBird properties;

	// Token: 0x0400019E RID: 414
	[SerializeField]
	public FlyingBirdLevelBird bird;

	// Token: 0x0400019F RID: 415
	[SerializeField]
	public FlyingBirdLevelSmallBird smallBird;

	// Token: 0x040001A0 RID: 416
	[Space(10f)]
	[SerializeField]
	public Transform enemyRoot;

	// Token: 0x040001A1 RID: 417
	[Space(10f)]
	[SerializeField]
	public Transform turretRootTop;

	// Token: 0x040001A2 RID: 418
	[SerializeField]
	public Transform turretRootBottom;

	// Token: 0x040001A3 RID: 419
	[Space(10f)]
	[SerializeField]
	public FlyingBirdLevel.Prefabs prefabs;

	// Token: 0x040001A4 RID: 420
	public IEnumerator skybirdPattern;

	// Token: 0x040001A5 RID: 421
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040001A6 RID: 422
	[SerializeField]
	public Sprite _bossPortraitHouseDeath;

	// Token: 0x040001A7 RID: 423
	[SerializeField]
	public Sprite _bossPortraitBirdRevival;

	// Token: 0x040001A8 RID: 424
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x040001A9 RID: 425
	[SerializeField]
	public string _bossQuoteHouseDeath;

	// Token: 0x040001AA RID: 426
	[SerializeField]
	public string _bossQuoteBirdRevival;

	// Token: 0x020007AC RID: 1964
	[Serializable]
	public class Prefabs
	{
		// Token: 0x04003D7B RID: 15739
		public FlyingBirdLevelEnemy formationBird;

		// Token: 0x04003D7C RID: 15740
		public FlyingBirdLevelTurret turretBird;
	}
}
