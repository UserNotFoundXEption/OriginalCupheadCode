using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000042 RID: 66
public class TrainLevel : Level
{
	// Token: 0x0600044A RID: 1098 RVA: 0x00069654 File Offset: 0x00067854
	public override void PartialInit()
	{
		this.properties = LevelProperties.Train.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x0600044B RID: 1099 RVA: 0x0000512B File Offset: 0x0000332B
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Train;
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x0600044C RID: 1100 RVA: 0x0000512E File Offset: 0x0000332E
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_train;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x0600044D RID: 1101 RVA: 0x000696EC File Offset: 0x000678EC
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.currentPhase)
			{
			case 1:
				return this._bossPortraitSpecter;
			case 2:
				return this._bossPortraitSkeleton;
			case 3:
				return this._bossPortraitLollipop;
			case 4:
				return this._bossPortraitEngine;
			default:
				Debug.LogError("Couldn't find portrait for phase " + this.currentPhase + ". Using Main.", null);
				return this._bossPortraitEngine;
			}
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x0600044E RID: 1102 RVA: 0x00069760 File Offset: 0x00067960
	public override string BossQuote
	{
		get
		{
			switch (this.currentPhase)
			{
			case 1:
				return this._bossQuoteSpecter;
			case 2:
				return this._bossQuoteSkeleton;
			case 3:
				return this._bossQuoteLollipop;
			case 4:
				return this._bossQuoteEngine;
			default:
				Debug.LogError("Couldn't find portrait for phase " + this.currentPhase + ". Using Main.", null);
				return this._bossQuoteEngine;
			}
		}
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x000697D4 File Offset: 0x000679D4
	public override void Start()
	{
		base.Start();
		this.properties.OnBossDamaged -= base.timeline.DealDamage;
		base.timeline = new Level.Timeline();
		base.timeline.health = 0f;
		base.timeline.health += (float)this.properties.CurrentState.blindSpecter.health;
		base.timeline.health += this.properties.CurrentState.skeleton.health;
		base.timeline.health += this.properties.CurrentState.lollipopGhouls.health * 2f;
		base.timeline.health += this.properties.CurrentState.engine.health;
		base.timeline.AddEvent(new Level.Timeline.Event("Skeleton", 1f - (float)this.properties.CurrentState.blindSpecter.health / base.timeline.health));
		base.timeline.AddEvent(new Level.Timeline.Event("Lollipop Ghouls", 1f - ((float)this.properties.CurrentState.blindSpecter.health + this.properties.CurrentState.skeleton.health) / base.timeline.health));
		base.timeline.AddEvent(new Level.Timeline.Event("Engine", 1f - ((float)this.properties.CurrentState.blindSpecter.health + this.properties.CurrentState.skeleton.health + this.properties.CurrentState.lollipopGhouls.health * 2f) / base.timeline.health));
		this.train.LevelInit(this.properties);
		this.blindSpecter.LevelInit(this.properties);
		this.skeleton.LevelInit(this.properties);
		this.ghouls.LevelInit(this.properties);
		this.engine.LevelInit(this.properties);
		this.blindSpecter.OnDeathEvent += this.OnBlindSpecterDeath;
		this.skeleton.OnDeathEvent += this.OnSkeletonDeath;
		this.ghouls.OnDeathEvent += this.OnLollipopsDeath;
		this.engine.OnDeathEvent += this.OnEngineDeath;
		this.blindSpecter.OnDamageTakenEvent += base.timeline.DealDamage;
		this.skeleton.OnDamageTakenEvent += base.timeline.DealDamage;
		this.ghouls.OnDamageTakenEvent += base.timeline.DealDamage;
		this.engine.OnDamageTakenEvent += base.timeline.DealDamage;
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x00005132 File Offset: 0x00003332
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.pumpkinPrefab = null;
		this._bossPortraitEngine = null;
		this._bossPortraitLollipop = null;
		this._bossPortraitSkeleton = null;
		this._bossPortraitSpecter = null;
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x0000515D File Offset: 0x0000335D
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.pumpkins_cr());
		base.StartCoroutine(this.trainPattern_cr());
		this.setPhase(1);
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00005180 File Offset: 0x00003380
	public void OnBlindSpecterDeath()
	{
		this.train.OnBlindSpectreDeath();
		this.setPhase(2);
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00005194 File Offset: 0x00003394
	public void OnSkeletonDeath()
	{
		this.train.OnSkeletonDeath();
		this.setPhase(3);
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x000051A8 File Offset: 0x000033A8
	public void OnLollipopsDeath()
	{
		if (Level.Current.mode == Level.Mode.Easy)
		{
			this.properties.WinInstantly();
		}
		else
		{
			this.train.OnLollipopsDeath();
			this.setPhase(4);
		}
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x000051DB File Offset: 0x000033DB
	public void OnEngineDeath()
	{
		this.properties.WinInstantly();
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00069AE4 File Offset: 0x00067CE4
	public void setPhase(int phase)
	{
		this.currentPhase = phase;
		foreach (string s in this.properties.CurrentState.pumpkins.bossPhaseOn.Split(new char[]
		{
			','
		}))
		{
			int num = 0;
			Parser.IntTryParse(s, out num);
			if (num == phase)
			{
				this.pumpkinsEnabled = true;
				return;
			}
		}
		this.pumpkinsEnabled = false;
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00069B58 File Offset: 0x00067D58
	public IEnumerator trainPattern_cr()
	{
		yield return new WaitForSeconds(1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x00069B74 File Offset: 0x00067D74
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.Train.Pattern p = this.properties.CurrentState.NextPattern;
		yield return new WaitForSeconds(1f);
		yield break;
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x00069B90 File Offset: 0x00067D90
	public IEnumerator pumpkins_cr()
	{
		int dir = (!Rand.Bool()) ? -1 : 1;
		Transform target = this.rightValve;
		LevelProperties.Train.Pumpkins p = this.properties.CurrentState.pumpkins;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, p.delay);
			if (this.pumpkinsEnabled)
			{
				this.pumpkinPrefab.Create(new Vector2((float)(840 * -(float)dir), 280f), dir, p.speed, p.health, p.fallTime, target);
				dir *= -1;
				if (this.train.state != TrainLevelTrain.State.BlindSpecter)
				{
					if (dir < 0)
					{
						target = this.rightValve;
					}
					else
					{
						target = this.leftValve;
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x0400031D RID: 797
	public LevelProperties.Train properties;

	// Token: 0x0400031E RID: 798
	[SerializeField]
	public TrainLevelTrain train;

	// Token: 0x0400031F RID: 799
	[Space(10f)]
	[SerializeField]
	public TrainLevelPumpkin pumpkinPrefab;

	// Token: 0x04000320 RID: 800
	[SerializeField]
	public Transform leftValve;

	// Token: 0x04000321 RID: 801
	[SerializeField]
	public Transform rightValve;

	// Token: 0x04000322 RID: 802
	[Space(10f)]
	[SerializeField]
	public TrainLevelBlindSpecter blindSpecter;

	// Token: 0x04000323 RID: 803
	[SerializeField]
	public TrainLevelSkeleton skeleton;

	// Token: 0x04000324 RID: 804
	[SerializeField]
	public TrainLevelLollipopGhoulsManager ghouls;

	// Token: 0x04000325 RID: 805
	[SerializeField]
	public TrainLevelEngineBoss engine;

	// Token: 0x04000326 RID: 806
	public Collider2D handCarCollider;

	// Token: 0x04000327 RID: 807
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitSpecter;

	// Token: 0x04000328 RID: 808
	[SerializeField]
	public Sprite _bossPortraitSkeleton;

	// Token: 0x04000329 RID: 809
	[SerializeField]
	public Sprite _bossPortraitLollipop;

	// Token: 0x0400032A RID: 810
	[SerializeField]
	public Sprite _bossPortraitEngine;

	// Token: 0x0400032B RID: 811
	[SerializeField]
	public string _bossQuoteSpecter;

	// Token: 0x0400032C RID: 812
	[SerializeField]
	public string _bossQuoteSkeleton;

	// Token: 0x0400032D RID: 813
	[SerializeField]
	public string _bossQuoteLollipop;

	// Token: 0x0400032E RID: 814
	[SerializeField]
	public string _bossQuoteEngine;

	// Token: 0x0400032F RID: 815
	public bool pumpkinsEnabled;

	// Token: 0x04000330 RID: 816
	public int currentPhase;

	// Token: 0x0200085C RID: 2140
	[Serializable]
	public class Prefabs
	{
	}
}
