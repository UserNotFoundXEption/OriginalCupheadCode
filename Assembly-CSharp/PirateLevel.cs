using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class PirateLevel : Level
{
	// Token: 0x06000364 RID: 868 RVA: 0x00066678 File Offset: 0x00064878
	public override void PartialInit()
	{
		this.properties = LevelProperties.Pirate.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000365 RID: 869 RVA: 0x00004B83 File Offset: 0x00002D83
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Pirate;
		}
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000366 RID: 870 RVA: 0x00004B86 File Offset: 0x00002D86
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_pirate;
		}
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000367 RID: 871 RVA: 0x00066710 File Offset: 0x00064910
	// (remove) Token: 0x06000368 RID: 872 RVA: 0x00066748 File Offset: 0x00064948
	public event PirateLevel.WhistleDelegate OnWhistleEvent;

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000369 RID: 873 RVA: 0x00066780 File Offset: 0x00064980
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Pirate.States.Main:
			case LevelProperties.Pirate.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Pirate.States.Boat:
				return this._bossPortraitBoat;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x0600036A RID: 874 RVA: 0x000667F4 File Offset: 0x000649F4
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Pirate.States.Main:
			case LevelProperties.Pirate.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Pirate.States.Boat:
				return this._bossQuoteBoat;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00004B8A File Offset: 0x00002D8A
	public override void Awake()
	{
		base.Awake();
		this.inkOverlay = this.prefabs.inkOverlay.InstantiatePrefab<PirateLevelSquidInkOverlay>();
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00066868 File Offset: 0x00064A68
	public override void Start()
	{
		base.Start();
		this.barrel.LevelInit(this.properties);
		this.inkOverlay.LevelInit(this.properties);
		this.pirate.LevelInit(this.properties);
		this.boat.LevelInit(this.properties);
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00004BA8 File Offset: 0x00002DA8
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.piratePattern_cr());
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00004BB7 File Offset: 0x00002DB7
	public override void OnStateChanged()
	{
		base.OnStateChanged();
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00004BBF File Offset: 0x00002DBF
	public void StartBoat()
	{
		this.StopAllCoroutines();
		this.boat.OnLaunchPirate += this.OnBoatLaunchPirate;
		this.boat.StartTransformation();
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00004BE9 File Offset: 0x00002DE9
	public void OnBoatLaunchPirate()
	{
		base.StartCoroutine(this.launchPirate_cr());
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00004BF8 File Offset: 0x00002DF8
	public void Whistle(PirateLevel.Creature creature)
	{
		if (this.OnWhistleEvent != null)
		{
			this.OnWhistleEvent(creature);
		}
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00004C11 File Offset: 0x00002E11
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.prefabs = null;
		this._bossPortraitBoat = null;
		this._bossPortraitMain = null;
	}

	// Token: 0x06000373 RID: 883 RVA: 0x000668C0 File Offset: 0x00064AC0
	public IEnumerator piratePattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000374 RID: 884 RVA: 0x000668DC File Offset: 0x00064ADC
	public IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.Pirate.Pattern.Shark:
			yield return base.StartCoroutine(this.shark_cr());
			break;
		case LevelProperties.Pirate.Pattern.Squid:
			yield return base.StartCoroutine(this.squid_cr());
			break;
		case LevelProperties.Pirate.Pattern.DogFish:
			yield return base.StartCoroutine(this.dogFish_cr());
			break;
		case LevelProperties.Pirate.Pattern.Peashot:
			yield return base.StartCoroutine(this.peashot_cr());
			break;
		case LevelProperties.Pirate.Pattern.Boat:
			this.StartBoat();
			break;
		default:
			yield return new WaitForSeconds(1f);
			break;
		}
		yield break;
	}

	// Token: 0x06000375 RID: 885 RVA: 0x000668F8 File Offset: 0x00064AF8
	public IEnumerator squid_cr()
	{
		this.Whistle(PirateLevel.Creature.Squid);
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.squid.startDelay);
		PirateLevelSquid squid = this.prefabs.squid.InstantiatePrefab<PirateLevelSquid>();
		squid.LevelInit(this.properties);
		while (squid.state != PirateLevelSquid.State.Exit && squid.state != PirateLevelSquid.State.Die)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, (float)this.properties.CurrentState.squid.endDelay);
		yield break;
	}

	// Token: 0x06000376 RID: 886 RVA: 0x00066914 File Offset: 0x00064B14
	public IEnumerator shark_cr()
	{
		this.Whistle(PirateLevel.Creature.Shark);
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.shark.startDelay);
		PirateLevelShark shark = this.prefabs.shark.InstantiatePrefab<PirateLevelShark>();
		shark.LevelInitWithGroup(this.properties.CurrentState.shark);
		while (shark.state != PirateLevelShark.State.Complete)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.shark.endDelay);
		yield break;
	}

	// Token: 0x06000377 RID: 887 RVA: 0x00066930 File Offset: 0x00064B30
	public IEnumerator dogFish_cr()
	{
		bool secretHitBox = false;
		this.Whistle(PirateLevel.Creature.DogFish);
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.scope.In();
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.dogFish.startDelay);
		LevelProperties.Pirate.DogFish properties = this.properties.CurrentState.dogFish;
		for (int i = 0; i < properties.count; i++)
		{
			secretHitBox = (i == 3);
			PirateLevelDogFish dogFish = this.prefabs.dogFish.InstantiatePrefab<PirateLevelDogFish>();
			dogFish.transform.SetPosition(new float?(0f), new float?(-210f), new float?(0f));
			dogFish.Init(this.properties, secretHitBox);
			yield return CupheadTime.WaitForSeconds(this, properties.nextFishDelay);
		}
		yield return CupheadTime.WaitForSeconds(this, properties.endDelay);
		yield break;
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0006694C File Offset: 0x00064B4C
	public IEnumerator peashot_cr()
	{
		LevelProperties.Pirate.Peashot properties = this.properties.CurrentState.peashot;
		KeyValue[] pattern = KeyValue.ListFromString(properties.patterns[Random.Range(0, properties.patterns.Length)], new char[]
		{
			'P',
			'D'
		});
		this.pirate.StartGun();
		yield return CupheadTime.WaitForSeconds(this, properties.startDelay);
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i].key == "P")
			{
				int p = 0;
				while ((float)p < pattern[i].value)
				{
					yield return CupheadTime.WaitForSeconds(this, properties.shotDelay);
					this.pirate.FireGun(properties);
					p++;
				}
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, pattern[i].value);
			}
			yield return null;
		}
		this.pirate.EndGun();
		yield return CupheadTime.WaitForSeconds(this, (float)properties.endDelay);
		yield break;
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00066968 File Offset: 0x00064B68
	public IEnumerator launchPirate_cr()
	{
		LevelProperties.Pirate.Boat p = this.properties.CurrentState.boat;
		this.deadPirate.Go(p.pirateFallDelay, p.pirateFallTime);
		float t = 0f;
		float time = 1f;
		float speed = 1200f;
		while (t < time)
		{
			float y = speed * CupheadTime.Delta;
			foreach (Transform transform in this.boatParts)
			{
				transform.AddPosition(0f, y, 0f);
			}
			this.pirate.transform.AddPosition(0f, y, 0f);
			t += CupheadTime.Delta;
			yield return null;
		}
		foreach (Transform transform2 in this.boatParts)
		{
			Object.Destroy(transform2.gameObject);
		}
		this.pirate.CleanUp();
		yield break;
	}

	// Token: 0x0400026C RID: 620
	public LevelProperties.Pirate properties;

	// Token: 0x0400026D RID: 621
	public const float WHISTLE_ANIM_TIME = 1.5f;

	// Token: 0x0400026F RID: 623
	[Space(10f)]
	public PirateLevelPirate pirate;

	// Token: 0x04000270 RID: 624
	public PirateLevelPirateDead deadPirate;

	// Token: 0x04000271 RID: 625
	public PirateLevelBoat boat;

	// Token: 0x04000272 RID: 626
	public PirateLevelBarrel barrel;

	// Token: 0x04000273 RID: 627
	public PirateLevelDogFishScope scope;

	// Token: 0x04000274 RID: 628
	public Transform[] boatParts;

	// Token: 0x04000275 RID: 629
	[Space(10f)]
	[SerializeField]
	public PirateLevel.Prefabs prefabs;

	// Token: 0x04000276 RID: 630
	public PirateLevelSquidInkOverlay inkOverlay;

	// Token: 0x04000277 RID: 631
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x04000278 RID: 632
	[SerializeField]
	public Sprite _bossPortraitBoat;

	// Token: 0x04000279 RID: 633
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x0400027A RID: 634
	[SerializeField]
	public string _bossQuoteBoat;

	// Token: 0x02000810 RID: 2064
	public enum Creature
	{
		// Token: 0x04003F76 RID: 16246
		Squid,
		// Token: 0x04003F77 RID: 16247
		Shark,
		// Token: 0x04003F78 RID: 16248
		DogFish
	}

	// Token: 0x02000811 RID: 2065
	// (Invoke) Token: 0x06004EFF RID: 20223
	public delegate void WhistleDelegate(PirateLevel.Creature creature);

	// Token: 0x02000812 RID: 2066
	[Serializable]
	public class Prefabs
	{
		// Token: 0x04003F79 RID: 16249
		public PirateLevelSquid squid;

		// Token: 0x04003F7A RID: 16250
		public PirateLevelShark shark;

		// Token: 0x04003F7B RID: 16251
		public PirateLevelDogFish dogFish;

		// Token: 0x04003F7C RID: 16252
		[Space(10f)]
		public PirateLevelSquidInkOverlay inkOverlay;
	}
}
