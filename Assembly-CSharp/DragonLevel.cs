using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class DragonLevel : Level
{
	// Token: 0x0600022F RID: 559 RVA: 0x00062A54 File Offset: 0x00060C54
	public override void PartialInit()
	{
		this.properties = LevelProperties.Dragon.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x06000230 RID: 560 RVA: 0x0000430E File Offset: 0x0000250E
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Dragon;
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06000231 RID: 561 RVA: 0x00004315 File Offset: 0x00002515
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_dragon;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06000232 RID: 562 RVA: 0x00004319 File Offset: 0x00002519
	// (set) Token: 0x06000233 RID: 563 RVA: 0x00004320 File Offset: 0x00002520
	public static float SPEED { get; set; }

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x06000234 RID: 564 RVA: 0x00062AEC File Offset: 0x00060CEC
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Dragon.States.Main:
			case LevelProperties.Dragon.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Dragon.States.ThreeHeads:
				return this._bossPortraitThreeHeads;
			case LevelProperties.Dragon.States.FireMarchers:
				return this._bossPortraitFireMarchers;
			default:
				Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossPortraitMain;
			}
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x06000235 RID: 565 RVA: 0x00062B6C File Offset: 0x00060D6C
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Dragon.States.Main:
			case LevelProperties.Dragon.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Dragon.States.ThreeHeads:
				return this._bossQuoteThreeHeads;
			case LevelProperties.Dragon.States.FireMarchers:
				return this._bossQuoteFireMarchers;
			default:
				Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
				return this._bossQuoteMain;
			}
		}
	}

	// Token: 0x06000236 RID: 566 RVA: 0x00004328 File Offset: 0x00002528
	public override void Awake()
	{
		base.Awake();
		DragonLevel.SPEED = 0f;
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0000433A File Offset: 0x0000253A
	public override void Start()
	{
		base.Start();
		this.dragon.LevelInit(this.properties);
		this.tail.LevelInit(this.properties);
		this.leftSideDragon.LevelInit(this.properties);
	}

	// Token: 0x06000238 RID: 568 RVA: 0x00062BEC File Offset: 0x00060DEC
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.speed_cr());
		base.StartCoroutine(this.tail_cr());
		base.StartCoroutine(this.dragonPattern_cr());
		this.manager.Init(this.properties.CurrentState.clouds);
		this.SetPlatformVariables(true);
		this.cloudsSameDir = this.properties.CurrentState.clouds.movingRight;
	}

	// Token: 0x06000239 RID: 569 RVA: 0x00062C60 File Offset: 0x00060E60
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		this.manager.UpdateProperties(this.properties.CurrentState.clouds);
		this.SetPlatformVariables(false);
		if (this.properties.CurrentState.clouds.movingRight != this.cloudsSameDir)
		{
			base.StartCoroutine(this.speed_cr());
			this.cloudsSameDir = this.properties.CurrentState.clouds.movingRight;
		}
		if (this.properties.CurrentState.stateName == LevelProperties.Dragon.States.FireMarchers)
		{
			this.StopAllCoroutines();
			this.dragon.Leave();
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Dragon.States.ThreeHeads)
		{
			this.StopAllCoroutines();
			this.leftSideDragon.StartThreeHeads();
			base.StartCoroutine(this.phase3ColorTransition());
		}
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00062D40 File Offset: 0x00060F40
	public void SetPlatformVariables(bool firstTime)
	{
		foreach (DragonLevelCloudPlatform dragonLevelCloudPlatform in this.platforms)
		{
			dragonLevelCloudPlatform.GetProperties(this.properties.CurrentState.clouds, firstTime);
		}
	}

	// Token: 0x0600023B RID: 571 RVA: 0x00004375 File Offset: 0x00002575
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitThreeHeads = null;
		this._bossPortraitFireMarchers = null;
	}

	// Token: 0x0600023C RID: 572 RVA: 0x00062D84 File Offset: 0x00060F84
	public IEnumerator dragonPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600023D RID: 573 RVA: 0x00062DA0 File Offset: 0x00060FA0
	public IEnumerator nextPattern_cr()
	{
		LevelProperties.Dragon.Pattern p = this.properties.CurrentState.NextPattern;
		if (p != LevelProperties.Dragon.Pattern.Meteor)
		{
			if (p != LevelProperties.Dragon.Pattern.Peashot)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			else
			{
				yield return base.StartCoroutine(this.peashot_cr());
			}
		}
		else
		{
			yield return base.StartCoroutine(this.meteor_cr());
		}
		yield break;
	}

	// Token: 0x0600023E RID: 574 RVA: 0x00062DBC File Offset: 0x00060FBC
	public IEnumerator meteor_cr()
	{
		while (this.dragon.state != DragonLevelDragon.State.Idle)
		{
			yield return null;
		}
		this.dragon.StartMeteor();
		while (this.dragon.state != DragonLevelDragon.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600023F RID: 575 RVA: 0x00062DD8 File Offset: 0x00060FD8
	public IEnumerator peashot_cr()
	{
		while (this.dragon.state != DragonLevelDragon.State.Idle)
		{
			yield return null;
		}
		this.dragon.StartPeashot();
		while (this.dragon.state != DragonLevelDragon.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000240 RID: 576 RVA: 0x00062DF4 File Offset: 0x00060FF4
	public IEnumerator speed_cr()
	{
		float t = 0f;
		while (t < 3f)
		{
			DragonLevel.SPEED = t / 3f;
			t += CupheadTime.Delta;
			yield return null;
		}
		DragonLevel.SPEED = 1f;
		yield break;
	}

	// Token: 0x06000241 RID: 577 RVA: 0x00062E08 File Offset: 0x00061008
	public IEnumerator tail_cr()
	{
		while (this.dragon.state != DragonLevelDragon.State.Idle)
		{
			yield return null;
		}
		for (;;)
		{
			while (!this.properties.CurrentState.tail.active)
			{
				yield return null;
			}
			LevelProperties.Dragon.Tail tailProperties = this.properties.CurrentState.tail;
			this.tail.TailStart(tailProperties.warningTime, tailProperties.inTime, tailProperties.holdTime, tailProperties.outTime);
			while (this.tail.state != DragonLevelTail.State.Idle)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, tailProperties.attackDelay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x06000242 RID: 578 RVA: 0x00062E24 File Offset: 0x00061024
	public IEnumerator phase3ColorTransition()
	{
		while (this.leftSideDragon.state != DragonLevelLeftSideDragon.State.ThreeHeads)
		{
			yield return null;
		}
		base.StartCoroutine(this.lightning_cr());
		float t = 0f;
		float fadeTime = 6f;
		DragonLevel.LightningState lastLightningState = this.lightningState;
		HitFlash dragonHitFlash = this.leftSideDragon.GetComponentInChildren<HitFlash>();
		this.dragonMaterial = this.leftSideDragon.GetComponent<SpriteRenderer>().material;
		for (;;)
		{
			LevelPlayerController playerOne = PlayerManager.GetPlayer<LevelPlayerController>(PlayerId.PlayerOne);
			LevelPlayerController playerTwo = PlayerManager.GetPlayer<LevelPlayerController>(PlayerId.PlayerTwo);
			float ratio = Mathf.Min(1f, t / fadeTime);
			Color playerColor;
			Color projectileColor;
			Color dragonColor;
			Color darkSpireColor;
			Color platformColor;
			if (this.lightningState == DragonLevel.LightningState.FirstFlash)
			{
				playerColor = ColorUtils.HexToColor("333333");
				projectileColor = ColorUtils.HexToColor("333333");
				dragonColor = ColorUtils.HexToColor("333333");
				darkSpireColor = new Color(0.2f, 0.2f, 0.2f, this.darkSpire.color.a);
				platformColor = ColorUtils.HexToColor("191919");
			}
			else if (this.lightningState == DragonLevel.LightningState.SecondFlash)
			{
				playerColor = ColorUtils.HexToColor("191919");
				projectileColor = ColorUtils.HexToColor("191919");
				dragonColor = ColorUtils.HexToColor("191919");
				darkSpireColor = new Color(0.1f, 0.1f, 0.1f, this.darkSpire.color.a);
				platformColor = ColorUtils.HexToColor("0c0c0c");
			}
			else
			{
				playerColor = Color.Lerp(Color.white, ColorUtils.HexToColor("d8d8d8"), ratio);
				projectileColor = Color.Lerp(Color.white, ColorUtils.HexToColor("d8d8d8"), ratio);
				dragonColor = Color.black;
				darkSpireColor = new Color(1f, 1f, 1f, this.darkSpire.color.a);
				platformColor = Color.Lerp(Color.white, ColorUtils.HexToColor("9c9da63"), ratio);
			}
			if (playerOne != null)
			{
				playerOne.animationController.SetColor(playerColor);
			}
			if (playerTwo != null)
			{
				playerTwo.animationController.SetColor(playerColor);
			}
			this.darkSpire.color = darkSpireColor;
			if (this.lightningState != lastLightningState)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("PlayerProjectile");
				foreach (GameObject gameObject in array)
				{
					SpriteRenderer component = gameObject.GetComponent<SpriteRenderer>();
					if (component != null)
					{
						component.color = projectileColor;
					}
				}
			}
			if (!dragonHitFlash.flashing)
			{
				foreach (SpriteRenderer spriteRenderer in this.leftSideDragon.GetComponentsInChildren<SpriteRenderer>())
				{
					spriteRenderer.material = ((this.lightningState != DragonLevel.LightningState.Off) ? this.dragonFlashMaterial : this.dragonMaterial);
					spriteRenderer.color = dragonColor;
				}
			}
			foreach (DragonLevelCloudPlatform dragonLevelCloudPlatform in this.manager.platforms)
			{
				dragonLevelCloudPlatform.GetComponent<SpriteRenderer>().color = platformColor;
				dragonLevelCloudPlatform.top.color = platformColor;
			}
			for (int k = 0; k < this.lightningFlashes.Length; k++)
			{
				if (this.lightningState == DragonLevel.LightningState.FirstFlash)
				{
					this.lightningFlashes[k].SetFlash1();
				}
				else if (this.lightningState == DragonLevel.LightningState.SecondFlash)
				{
					this.lightningFlashes[k].SetFlash2();
				}
				else if (this.lightningState == DragonLevel.LightningState.Off)
				{
					this.lightningFlashes[k].SetNormal();
				}
			}
			t += CupheadTime.Delta;
			lastLightningState = this.lightningState;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000243 RID: 579 RVA: 0x00062E40 File Offset: 0x00061040
	public IEnumerator lightning_cr()
	{
		for (;;)
		{
			this.lightningState = DragonLevel.LightningState.Off;
			yield return CupheadTime.WaitForSeconds(this, MathUtils.ExpRandom(2f) + 1f);
			this.lightningStrikes.PlayLightning();
			float rand = Random.value;
			if (rand < 0.25f)
			{
				this.lightningState = DragonLevel.LightningState.FirstFlash;
				yield return CupheadTime.WaitForSeconds(this, 0.041f);
			}
			else if (rand < 0.5f)
			{
				this.lightningState = DragonLevel.LightningState.SecondFlash;
				yield return CupheadTime.WaitForSeconds(this, 0.041f);
			}
			else
			{
				this.lightningState = DragonLevel.LightningState.FirstFlash;
				yield return CupheadTime.WaitForSeconds(this, 0.041f);
				this.lightningState = DragonLevel.LightningState.Off;
				yield return CupheadTime.WaitForSeconds(this, 0.041f);
				this.lightningState = DragonLevel.LightningState.SecondFlash;
				yield return CupheadTime.WaitForSeconds(this, 0.041f);
			}
		}
		yield break;
	}

	// Token: 0x0400017E RID: 382
	public LevelProperties.Dragon properties;

	// Token: 0x0400017F RID: 383
	public const float Flash1Probability = 0.25f;

	// Token: 0x04000180 RID: 384
	public const float Flash2Probability = 0.25f;

	// Token: 0x04000182 RID: 386
	[SerializeField]
	public DragonLevelBackgroundFlash[] lightningFlashes;

	// Token: 0x04000183 RID: 387
	[SerializeField]
	public DragonLevelCloudPlatform[] platforms;

	// Token: 0x04000184 RID: 388
	[SerializeField]
	public SpriteRenderer spire;

	// Token: 0x04000185 RID: 389
	[SerializeField]
	public SpriteRenderer darkSpire;

	// Token: 0x04000186 RID: 390
	[SerializeField]
	public DragonLevelDragon dragon;

	// Token: 0x04000187 RID: 391
	[SerializeField]
	public DragonLevelLeftSideDragon leftSideDragon;

	// Token: 0x04000188 RID: 392
	[SerializeField]
	public DragonLevelTail tail;

	// Token: 0x04000189 RID: 393
	[SerializeField]
	public DragonLevelPlatformManager manager;

	// Token: 0x0400018A RID: 394
	[SerializeField]
	public DragonLevelLightning lightningStrikes;

	// Token: 0x0400018B RID: 395
	[SerializeField]
	public Material dragonFlashMaterial;

	// Token: 0x0400018C RID: 396
	public DragonLevel.LightningState lightningState;

	// Token: 0x0400018D RID: 397
	[SerializeField]
	public SpriteRenderer[] backgroundClouds;

	// Token: 0x0400018E RID: 398
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x0400018F RID: 399
	[SerializeField]
	public Sprite _bossPortraitFireMarchers;

	// Token: 0x04000190 RID: 400
	[SerializeField]
	public Sprite _bossPortraitThreeHeads;

	// Token: 0x04000191 RID: 401
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x04000192 RID: 402
	[SerializeField]
	public string _bossQuoteFireMarchers;

	// Token: 0x04000193 RID: 403
	[SerializeField]
	public string _bossQuoteThreeHeads;

	// Token: 0x04000194 RID: 404
	public bool cloudsSameDir;

	// Token: 0x04000195 RID: 405
	public Material dragonMaterial;

	// Token: 0x0200079C RID: 1948
	public enum LightningState
	{
		// Token: 0x04003D33 RID: 15667
		Off,
		// Token: 0x04003D34 RID: 15668
		FirstFlash,
		// Token: 0x04003D35 RID: 15669
		SecondFlash
	}

	// Token: 0x0200079D RID: 1949
	[Serializable]
	public class Prefabs
	{
	}
}
