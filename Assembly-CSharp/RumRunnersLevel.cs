using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class RumRunnersLevel : Level
{
	// Token: 0x060003A9 RID: 937 RVA: 0x000673A8 File Offset: 0x000655A8
	public override void PartialInit()
	{
		this.properties = LevelProperties.RumRunners.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x060003AA RID: 938 RVA: 0x00004D26 File Offset: 0x00002F26
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.RumRunners;
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x060003AB RID: 939 RVA: 0x00004D2D File Offset: 0x00002F2D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_rum_runners;
		}
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060003AC RID: 940 RVA: 0x00067440 File Offset: 0x00065640
	// (remove) Token: 0x060003AD RID: 941 RVA: 0x00067478 File Offset: 0x00065678
	public event Action<Rangef> OnUpperBridgeDestroy;

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x060003AE RID: 942 RVA: 0x000674B0 File Offset: 0x000656B0
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.RumRunners.States.Main:
				return this._bossPortraitMain;
			case LevelProperties.RumRunners.States.Worm:
				return this._bossPortraitPhaseTwo;
			case LevelProperties.RumRunners.States.Anteater:
				return this._bossPortraitPhaseThree;
			case LevelProperties.RumRunners.States.MobBoss:
				return this._bossPortraitPhaseFour;
			}
			Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x060003AF RID: 943 RVA: 0x0006753C File Offset: 0x0006573C
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.RumRunners.States.Main:
				return this._bossQuoteMain;
			case LevelProperties.RumRunners.States.Worm:
				return this._bossQuotePhaseTwo;
			case LevelProperties.RumRunners.States.Anteater:
				return this._bossQuotePhaseThree;
			case LevelProperties.RumRunners.States.MobBoss:
				return this._bossQuotePhaseFour;
			}
			Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x000675C8 File Offset: 0x000657C8
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
		this._bossPortraitPhaseFour = null;
		if (CupheadLevelCamera.Current != null)
		{
			CupheadLevelCamera.Current.OnShakeEvent -= this.onShakeEventHandler;
		}
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x00067620 File Offset: 0x00065820
	public override void Start()
	{
		base.Start();
		this.spider.LevelInit(this.properties);
		this.worm.LevelInit(this.properties);
		this.anteater.LevelInit(this.properties);
		CupheadLevelCamera.Current.OnShakeEvent += this.onShakeEventHandler;
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0006767C File Offset: 0x0006587C
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(new Vector3(this.topPlatformEffectRange.minimum, -1000f), new Vector3(this.topPlatformEffectRange.minimum, 1000f));
		Gizmos.DrawLine(new Vector3(this.topPlatformEffectRange.maximum, -1000f), new Vector3(this.topPlatformEffectRange.maximum, 1000f));
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(new Vector3(this.middlePlatformEffectRange.minimum, -1000f), new Vector3(this.middlePlatformEffectRange.minimum, 1000f));
		Gizmos.DrawLine(new Vector3(this.middlePlatformEffectRange.maximum, -1000f), new Vector3(this.middlePlatformEffectRange.maximum, 1000f));
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00067760 File Offset: 0x00065960
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		this.StopAllCoroutines();
		if (this.properties.CurrentState.stateName == LevelProperties.RumRunners.States.Worm)
		{
			base.StartCoroutine(this.worm_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.RumRunners.States.Anteater)
		{
			base.StartCoroutine(this.anteater_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.RumRunners.States.MobBoss)
		{
			base.StartCoroutine(this.winFakeout_cr());
		}
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x000677EC File Offset: 0x000659EC
	public IEnumerator worm_cr()
	{
		this.spider.Die();
		while (this.spider != null)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.worm.Setup();
		this.worm.StartBarrels();
		this.ph2SpiderAnimation.gameObject.SetActive(true);
		yield return this.ph2SpiderAnimation.WaitForAnimationToEnd(this, "Ph2", false, true);
		yield return CupheadTime.WaitForSeconds(this, RumRunnersLevel.SpiderTransitionLowerRopeDuration);
		this.worm.StartWorm(this.mobIntro.bugGirlDamage);
		yield return CupheadTime.WaitForSeconds(this, RumRunnersLevel.SpiderTransitionLowerBugDuration);
		this.ph2SpiderAnimation.SetTrigger("End");
		RumRunnersLevelPh2StartAnimation ph2 = this.ph2SpiderAnimation.GetComponent<RumRunnersLevelPh2StartAnimation>();
		while (!ph2.dropped)
		{
			yield return null;
		}
		this.worm.introDrop = true;
		yield break;
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x00067808 File Offset: 0x00065A08
	public IEnumerator anteater_cr()
	{
		this.worm.StartDeath();
		yield return this.worm.animator.WaitForAnimationToEnd(this, "Fall", false, true);
		while (Mathf.Abs(this.worm.transform.position.x) > RumRunnersLevel.AnteaterIntroWormTriggerDistance)
		{
			yield return null;
		}
		this.anteater.gameObject.SetActive(true);
		yield return this.anteater.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		this.anteater.StartAnteater();
		yield break;
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00004D31 File Offset: 0x00002F31
	public void DestroyMiddleBridge()
	{
		this.destroyPlatforms(this.destroyedPlatformsMiddle, this.destroyedSpritesMiddleA, this.middlePlatformEffectRange);
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00004D4B File Offset: 0x00002F4B
	public void DestroyUpperBridge()
	{
		if (this.OnUpperBridgeDestroy != null)
		{
			this.OnUpperBridgeDestroy(this.topPlatformEffectRange);
		}
		this.destroyPlatforms(this.destroyedPlatformsUpper, this.destroyedSpritesUpperA, this.topPlatformEffectRange);
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x00067824 File Offset: 0x00065A24
	public void ShatterBridges()
	{
		this.destroyPlatforms(null, this.destroyedSpritesMiddleB, default(Rangef));
		this.destroyPlatforms(null, this.destroyedSpritesUpperB, default(Rangef));
		float[] array = new float[]
		{
			0f,
			0.25f,
			0.5f,
			0.75f
		};
		array.Shuffle<float>();
		float num = 1280f / this.camera.zoom;
		for (int i = 0; i < array.Length; i++)
		{
			float value = Random.Range(array[i], array[i] + array[1]);
			value = MathUtilities.LerpMapping(value, 0f, 1f, -num * 0.4f, num * 0.4f, true);
			this.FullscreenDirt(1, new float?(value), 0.15f, 0.3f);
		}
		CupheadLevelCamera.Current.Shake(55f, 0.5f, true);
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x000678FC File Offset: 0x00065AFC
	public void destroyPlatforms(LevelPlatform[] colliders, GameObject[] sprites, Rangef checkRange = default(Rangef))
	{
		if (colliders != null)
		{
			foreach (LevelPlatform levelPlatform in colliders)
			{
				LevelPlatform levelPlatform2 = null;
				int num = Array.IndexOf<LevelPlatform>(this.swapPlatformsMappingBefore, levelPlatform);
				if (num >= 0)
				{
					levelPlatform2 = this.swapPlatformsMappingAfter[num];
				}
				if (levelPlatform.GetComponentInChildren<LevelPlayerController>())
				{
					LevelPlayerController[] componentsInChildren = levelPlatform.GetComponentsInChildren<LevelPlayerController>();
					foreach (LevelPlayerController levelPlayerController in componentsInChildren)
					{
						if (!checkRange.ContainsExclusive(levelPlayerController.transform.position.x) && levelPlatform2 != null)
						{
							levelPlatform2.AddChild(levelPlayerController.transform);
						}
						else
						{
							levelPlayerController.motor.OnTrampolineKnockUp(RumRunnersLevel.BridgeDestroyBounceHeight);
						}
					}
				}
			}
			foreach (LevelPlatform levelPlatform3 in colliders)
			{
				Object.Destroy(levelPlatform3.gameObject);
			}
		}
		if (sprites != null)
		{
			foreach (GameObject gameObject in sprites)
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060003BA RID: 954 RVA: 0x00067A38 File Offset: 0x00065C38
	public IEnumerator winFakeout_cr()
	{
		this.anteater.FakeDeathStart();
		AudioManager.Play("level_announcer_knockout_bell");
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_knockout");
		base.StartCoroutine(this.stingSound_cr());
		Vector3 bannerPosition = this.fakeBannerAnimator.transform.position;
		bannerPosition += CupheadLevelCamera.Current.transform.position;
		this.fakeBannerAnimator.transform.position = bannerPosition;
		this.fakeBannerAnimator.SetTrigger("Banner");
		Coroutine dirtCoroutine = base.StartCoroutine(this.fakeDeathDirt_cr());
		yield return this.fakeBannerAnimator.WaitForAnimationToEnd(this, "Banner", false, true);
		base.StopCoroutine(dirtCoroutine);
		this.anteater.FakeDeathContinue();
		yield break;
	}

	// Token: 0x060003BB RID: 955 RVA: 0x00067A54 File Offset: 0x00065C54
	public IEnumerator fakeDeathDirt_cr()
	{
		float elapsedTime = 0f;
		float delay = 0.4f;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
			elapsedTime += delay;
			delay = Mathf.Lerp(0.4f, 0.8f, elapsedTime / 1.5f);
			this.FullscreenDirt(2, null, -1f, -1f);
		}
		yield break;
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00067A70 File Offset: 0x00065C70
	public IEnumerator stingSound_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		AudioManager.Play("sfx_dlc_rumrun_fake_levelbossdefeatsting");
		yield break;
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00004D81 File Offset: 0x00002F81
	public void FullscreenDirt(int count, float? positionX = null, float customInitialDelay = -1f, float customIntraDelay = -1f)
	{
		base.StartCoroutine(this.dirtFX_cr(count, positionX, customInitialDelay, customIntraDelay));
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00067A8C File Offset: 0x00065C8C
	public IEnumerator dirtFX_cr(int count, float? positionX, float customInitialDelay, float customIntraDelay)
	{
		MinMax[] DirtRandomizationX = new MinMax[]
		{
			new MinMax(-50f, 0f),
			new MinMax(0f, 50f)
		};
		MinMax WaitRange = new MinMax(0.08f, 0.12f);
		MinMax previousSpawnRange = (!Rand.Bool()) ? new MinMax(0.5f, 1f) : new MinMax(0f, 0.5f);
		for (int i = 0; i < count; i++)
		{
			float initialDelay = WaitRange.RandomFloat();
			if (customInitialDelay >= 0f)
			{
				initialDelay = customInitialDelay;
			}
			yield return CupheadTime.WaitForSeconds(this, initialDelay);
			Vector3 position = new Vector3(0f, 360f / this.camera.zoom);
			if (positionX == null)
			{
				MinMax minMax;
				if (previousSpawnRange.min == 0f)
				{
					minMax = new MinMax(0.5f, 1f);
				}
				else
				{
					minMax = new MinMax(0f, 0.5f);
				}
				position.x = 1280f * minMax.RandomFloat() - 640f;
				previousSpawnRange = minMax;
			}
			else
			{
				position.x = positionX.Value;
			}
			DirtRandomizationX.Shuffle<MinMax>();
			for (int spawn = 0; spawn < DirtRandomizationX.Length; spawn++)
			{
				this.fullscreenDirtFX.Create(position + new Vector3(DirtRandomizationX[spawn].RandomFloat(), 0f));
				float intraDelay = Random.Range(0.1f, 0.2f);
				if (customIntraDelay >= 0f)
				{
					intraDelay = Random.Range(customIntraDelay * 0.8f, customIntraDelay * 1.2f);
				}
				yield return CupheadTime.WaitForSeconds(this, intraDelay);
			}
		}
		yield break;
	}

	// Token: 0x060003BF RID: 959 RVA: 0x00067AC4 File Offset: 0x00065CC4
	public void onShakeEventHandler(float amount, float time)
	{
		this.FullscreenDirt(2, null, -1f, -1f);
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x00004D95 File Offset: 0x00002F95
	public override void PlayAnnouncerReady()
	{
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_ready");
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00004DA1 File Offset: 0x00002FA1
	public override void PlayAnnouncerBegin()
	{
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_begin");
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x00067AEC File Offset: 0x00065CEC
	public override IEnumerator knockoutSFX_cr()
	{
		AudioManager.Play("level_announcer_knockout_bell");
		AudioManager.Play("sfx_DLC_RUMRUN_VX_AnnouncerClearThroat");
		yield return CupheadTime.WaitForSeconds(this, 1.4f);
		AudioManager.Play("level_boss_defeat_sting");
		yield break;
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00067B08 File Offset: 0x00065D08
	public static float GroundWalkingPosY(Vector2 position, Collider2D collider, float offset = 0f, float rayLength = 200f)
	{
		int num = 1048576;
		position.x = Mathf.Clamp(position.x, (float)Level.Current.Left, (float)Level.Current.Right);
		Vector3 vector;
		vector..ctor(position.x, position.y);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, Vector3.down, rayLength, num);
		if (raycastHit2D.collider != null)
		{
			float y = raycastHit2D.point.y;
			float num2 = (!collider) ? 0f : (-collider.offset.y + collider.bounds.size.y / 2f);
			return y + num2 + offset;
		}
		return position.y;
	}

	// Token: 0x0400029B RID: 667
	public LevelProperties.RumRunners properties;

	// Token: 0x0400029C RID: 668
	public static readonly float SpiderTransitionLowerRopeDuration = 0.5f;

	// Token: 0x0400029D RID: 669
	public static readonly float SpiderTransitionLowerBugDuration = 0.3f;

	// Token: 0x0400029E RID: 670
	public static readonly float BridgeDestroyBounceHeight = -1f;

	// Token: 0x0400029F RID: 671
	public static readonly float AnteaterIntroWormTriggerDistance = 500f;

	// Token: 0x040002A1 RID: 673
	[SerializeField]
	public Animator ph2SpiderAnimation;

	// Token: 0x040002A2 RID: 674
	[SerializeField]
	public RumRunnersLevelSpider spider;

	// Token: 0x040002A3 RID: 675
	[SerializeField]
	public RumRunnersLevelWorm worm;

	// Token: 0x040002A4 RID: 676
	[SerializeField]
	public RumRunnersLevelAnteater anteater;

	// Token: 0x040002A5 RID: 677
	[SerializeField]
	public RumRunnersLevelMobIntroAnimation mobIntro;

	// Token: 0x040002A6 RID: 678
	[SerializeField]
	public Effect fullscreenDirtFX;

	// Token: 0x040002A7 RID: 679
	[SerializeField]
	public Animator fakeBannerAnimator;

	// Token: 0x040002A8 RID: 680
	[SerializeField]
	public GameObject[] destroyedSpritesMiddleA;

	// Token: 0x040002A9 RID: 681
	[SerializeField]
	public GameObject[] destroyedSpritesMiddleB;

	// Token: 0x040002AA RID: 682
	[SerializeField]
	public GameObject[] destroyedSpritesUpperA;

	// Token: 0x040002AB RID: 683
	[SerializeField]
	public GameObject[] destroyedSpritesUpperB;

	// Token: 0x040002AC RID: 684
	[SerializeField]
	public LevelPlatform[] destroyedPlatformsMiddle;

	// Token: 0x040002AD RID: 685
	[SerializeField]
	public LevelPlatform[] destroyedPlatformsUpper;

	// Token: 0x040002AE RID: 686
	[SerializeField]
	public LevelPlatform[] swapPlatformsMappingBefore;

	// Token: 0x040002AF RID: 687
	[SerializeField]
	public LevelPlatform[] swapPlatformsMappingAfter;

	// Token: 0x040002B0 RID: 688
	[SerializeField]
	public Rangef middlePlatformEffectRange;

	// Token: 0x040002B1 RID: 689
	[SerializeField]
	public Rangef topPlatformEffectRange;

	// Token: 0x040002B2 RID: 690
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040002B3 RID: 691
	[SerializeField]
	public Sprite _bossPortraitPhaseTwo;

	// Token: 0x040002B4 RID: 692
	[SerializeField]
	public Sprite _bossPortraitPhaseThree;

	// Token: 0x040002B5 RID: 693
	[SerializeField]
	public Sprite _bossPortraitPhaseFour;

	// Token: 0x040002B6 RID: 694
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x040002B7 RID: 695
	[SerializeField]
	public string _bossQuotePhaseTwo;

	// Token: 0x040002B8 RID: 696
	[SerializeField]
	public string _bossQuotePhaseThree;

	// Token: 0x040002B9 RID: 697
	[SerializeField]
	public string _bossQuotePhaseFour;
}
