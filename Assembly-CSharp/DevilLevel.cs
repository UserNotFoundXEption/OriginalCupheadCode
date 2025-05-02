using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class DevilLevel : Level
{
	// Token: 0x0600015A RID: 346 RVA: 0x000611D8 File Offset: 0x0005F3D8
	public override void PartialInit()
	{
		this.properties = LevelProperties.Devil.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x0600015B RID: 347 RVA: 0x00003D20 File Offset: 0x00001F20
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Devil;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x0600015C RID: 348 RVA: 0x00003D27 File Offset: 0x00001F27
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_devil;
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x0600015D RID: 349 RVA: 0x00061270 File Offset: 0x0005F470
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Devil.States.Main:
			case LevelProperties.Devil.States.Generic:
				return this._bossPortraitMain;
			case LevelProperties.Devil.States.GiantHead:
				return this._bossPortraitPhaseTwo;
			case LevelProperties.Devil.States.Hands:
			case LevelProperties.Devil.States.Tears:
				return this._bossPortraitPhaseThree;
			}
			Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600015E RID: 350 RVA: 0x000612F8 File Offset: 0x0005F4F8
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Devil.States.Main:
			case LevelProperties.Devil.States.Generic:
				return this._bossQuoteMain;
			case LevelProperties.Devil.States.GiantHead:
				return this._bossQuotePhaseTwo;
			case LevelProperties.Devil.States.Hands:
			case LevelProperties.Devil.States.Tears:
				return this._bossQuotePhaseThree;
			}
			Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00003D2B File Offset: 0x00001F2B
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00003D33 File Offset: 0x00001F33
	public override void Start()
	{
		base.Start();
		this.isDevil = true;
		this.sittingDevil.LevelInit(this.properties);
		this.giantHead.LevelInit(this.properties);
		base.StartCoroutine(this.DelayedStart());
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00061380 File Offset: 0x0005F580
	public IEnumerator DelayedStart()
	{
		yield return null;
		this.phase2Background.SetActive(false);
		yield break;
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00003D71 File Offset: 0x00001F71
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.devilPattern_cr());
		this.sittingDevil.StartDemons();
	}

	// Token: 0x06000163 RID: 355 RVA: 0x0006139C File Offset: 0x0005F59C
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Devil.States.Split)
		{
			this.StopAllCoroutines();
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Devil.States.GiantHead)
		{
			this.StopAllCoroutines();
			this.sittingDevil.StartTransform();
			base.StartCoroutine(this.phase_1_end_trans());
			base.StartCoroutine(this.devilPattern_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Devil.States.Hands)
		{
			this.StopAllCoroutines();
			this.giantHead.StartHands();
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Devil.States.Tears)
		{
			this.StopAllCoroutines();
			this.giantHead.StartTears();
		}
	}

	// Token: 0x06000164 RID: 356 RVA: 0x00003D8B File Offset: 0x00001F8B
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseThree = null;
		this._bossPortraitPhaseTwo = null;
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0006146C File Offset: 0x0005F66C
	public IEnumerator phase_1_end_trans()
	{
		foreach (DevilLevelEffectSpawner devilLevelEffectSpawner in this.smokeSpawners)
		{
			devilLevelEffectSpawner.KillSmoke();
		}
		while (!DevilLevelHole.PHASE_1_COMPLETE)
		{
			yield return null;
		}
		this.groundHandler.SetActive(false);
		bool startZoomout = false;
		float t = 0f;
		float cameraSlideUpTime = 1f;
		float time = 3.3f;
		float endCameraTime = 2f;
		Vector3 phase1Start = this.phase1Scroll.transform.position;
		Vector3 phase1End = Vector3.zero;
		Vector3 cameraStart = CupheadLevelCamera.Current.transform.position;
		Vector3 cameraEffectEnd = new Vector3(CupheadLevelCamera.Current.transform.position.x, 50f);
		Vector3 cameraOffsetEnd = new Vector3(CupheadLevelCamera.Current.transform.position.x, 600f);
		foreach (ParallaxLayer parallaxLayer in this.parallax)
		{
			parallaxLayer.enabled = false;
		}
		this.sittingDevil.RemoveFire();
		yield return base.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraEffectEnd, cameraSlideUpTime));
		base.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraOffsetEnd, time));
		while (t < time)
		{
			if (t >= 2f && !startZoomout)
			{
				this.ZoomOut(cameraStart, endCameraTime);
				startZoomout = true;
			}
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			this.phase1Scroll.transform.position = Vector3.Lerp(phase1Start, phase1End, val);
			Color c = this.phase1Foreground.color;
			c.a = Mathf.Clamp(1f - t * 2f, 0f, 1f);
			this.phase1Foreground.color = c;
			yield return null;
		}
		this.phase1Scroll.transform.position = phase1End;
		this.giantHead.transform.parent = null;
		this.giantHead.StartIntroTransform();
		base.StartCoroutine(this.phase2BackgroundFade_cr(2f));
		AudioManager.FadeBGMVolume(0f, 0.5f, true);
		AudioManager.PlayBGMPlaylistManually(false);
		AudioManager.Play("transition_sting");
		yield return CupheadTime.WaitForSeconds(this, endCameraTime);
		this.phase1Scroll.gameObject.SetActive(false);
		Object.Destroy(this.sittingDevil.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00061488 File Offset: 0x0005F688
	public IEnumerator phase2BackgroundFade_cr(float time)
	{
		SpriteRenderer[] sprites = this.phase2Background.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < sprites.Length; i++)
		{
			Color color = sprites[i].color;
			color.a = 0f;
			sprites[i].color = color;
		}
		this.phase2Background.SetActive(true);
		float t = 0f;
		while (t < time)
		{
			for (int j = 0; j < sprites.Length; j++)
			{
				Color color2 = sprites[j].color;
				color2.a = t / time;
				sprites[j].color = color2;
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		for (int k = 0; k < sprites.Length; k++)
		{
			Color color3 = sprites[k].color;
			color3.a = 1f;
			sprites[k].color = color3;
		}
		yield break;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x000614AC File Offset: 0x0005F6AC
	public void ZoomOut(Vector3 cameraStart, float endCameraTime)
	{
		AudioManager.FadeBGMVolume(0f, 1f, true);
		Level.Current.SetBounds(new int?(932), new int?(932), new int?(460), new int?(306));
		base.StartCoroutine(CupheadLevelCamera.Current.change_zoom_cr(0.811f, 10f));
		base.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraStart, endCameraTime));
		this.phase3Platforms.SetActive(true);
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		player.transform.SetScale(new float?(1f), null, null);
		player.transform.position = this.Phase2P1spawn.position;
		player.GetComponent<LevelPlayerMotor>().ForceLooking(new Trilean2(1, 0));
		if (player.stats.isChalice)
		{
			player.GetComponent<LevelPlayerMotor>().DashComplete();
			player.GetComponent<LevelPlayerAnimationController>().ScaredChalice(false);
		}
		else
		{
			player.GetComponent<LevelPlayerAnimationController>().PlayIntro();
		}
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 != null)
		{
			player2.transform.SetScale(new float?(1f), null, null);
			player2.transform.position = this.Phase2P2spawn.position;
			player2.GetComponent<LevelPlayerMotor>().ForceLooking(new Trilean2(1, 0));
			if (player2.stats.isChalice)
			{
				player2.GetComponent<LevelPlayerMotor>().DashComplete();
				player2.GetComponent<LevelPlayerAnimationController>().ScaredChalice(false);
			}
			else
			{
				player2.GetComponent<LevelPlayerAnimationController>().PlayIntro();
			}
		}
		base.StartCoroutine(this.disable_input_cr());
		this.pit.SetActive(true);
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00061674 File Offset: 0x0005F874
	public IEnumerator disable_input_cr()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		LevelPlayerMotor motorP = player.GetComponent<LevelPlayerMotor>();
		LevelPlayerWeaponManager weaponManagerP = player.GetComponent<LevelPlayerWeaponManager>();
		motorP.DisableInput();
		if (player.stats.isChalice)
		{
			motorP.ForceLooking(new Trilean2(0, 0));
		}
		weaponManagerP.DisableInput();
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 != null)
		{
			player2.GetComponent<LevelPlayerMotor>().DisableInput();
			if (player2.stats.isChalice)
			{
				player2.GetComponent<LevelPlayerMotor>().ForceLooking(new Trilean2(0, 0));
			}
			player2.GetComponent<LevelPlayerWeaponManager>().DisableInput();
		}
		if (!player.GetComponent<LevelPlayerController>().IsDead)
		{
			yield return motorP.animator.WaitForAnimationToEnd(this, (!player.stats.isChalice) ? "Intro_Scared" : "Intro_Chalice_Scared", (!player.stats.isChalice) ? 0 : 3, false, true);
		}
		else if (player2 != null && !player2.GetComponent<LevelPlayerController>().IsDead)
		{
			yield return player2.GetComponent<LevelPlayerMotor>().animator.WaitForAnimationToEnd(this, (!player2.stats.isChalice) ? "Intro_Scared" : "Intro_Chalice_Scared", (!player2.stats.isChalice) ? 0 : 3, false, true);
		}
		motorP.ClearBufferedInput();
		motorP.EnableInput();
		weaponManagerP.EnableInput();
		if (player.stats.isChalice)
		{
			motorP.ForceLooking(new Trilean2(1, 0));
		}
		player.GetComponent<LevelPlayerAnimationController>().ResetMoveX();
		player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 != null)
		{
			player2.GetComponent<LevelPlayerMotor>().ClearBufferedInput();
			player2.GetComponent<LevelPlayerMotor>().EnableInput();
			player2.GetComponent<LevelPlayerWeaponManager>().EnableInput();
			player2.GetComponent<LevelPlayerAnimationController>().ResetMoveX();
			if (player2.stats.isChalice)
			{
				player2.GetComponent<LevelPlayerMotor>().ForceLooking(new Trilean2(1, 0));
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00061690 File Offset: 0x0005F890
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(this.Phase2P1spawn.position, 30f);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(this.Phase2P2spawn.position, 30f);
	}

	// Token: 0x0600016A RID: 362 RVA: 0x000616E4 File Offset: 0x0005F8E4
	public IEnumerator devilPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00061700 File Offset: 0x0005F900
	public IEnumerator nextPattern_cr()
	{
		switch (this.properties.CurrentState.NextPattern)
		{
		case LevelProperties.Devil.Pattern.Clap:
			yield return base.StartCoroutine(this.clap_cr());
			break;
		case LevelProperties.Devil.Pattern.Head:
			yield return base.StartCoroutine(this.head_cr());
			break;
		case LevelProperties.Devil.Pattern.Pitchfork:
			yield return base.StartCoroutine(this.pitchfork_cr());
			break;
		case LevelProperties.Devil.Pattern.BombEye:
			yield return base.StartCoroutine(this.bombEye_cr());
			break;
		case LevelProperties.Devil.Pattern.SkullEye:
			yield return base.StartCoroutine(this.skullEye_cr());
			break;
		default:
			yield return CupheadTime.WaitForSeconds(this, 1f);
			break;
		}
		yield break;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0006171C File Offset: 0x0005F91C
	public IEnumerator clap_cr()
	{
		while (this.sittingDevil.state != DevilLevelSittingDevil.State.Idle)
		{
			yield return null;
		}
		this.sittingDevil.StartClap();
		while (this.sittingDevil.state != DevilLevelSittingDevil.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00061738 File Offset: 0x0005F938
	public IEnumerator head_cr()
	{
		while (this.sittingDevil.state != DevilLevelSittingDevil.State.Idle)
		{
			yield return null;
		}
		this.sittingDevil.StartHead();
		while (this.sittingDevil.state != DevilLevelSittingDevil.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00061754 File Offset: 0x0005F954
	public IEnumerator pitchfork_cr()
	{
		while (this.sittingDevil.state != DevilLevelSittingDevil.State.Idle)
		{
			yield return null;
		}
		this.sittingDevil.StartPitchfork();
		while (this.sittingDevil.state != DevilLevelSittingDevil.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00061770 File Offset: 0x0005F970
	public IEnumerator bombEye_cr()
	{
		while (this.giantHead.state != DevilLevelGiantHead.State.Idle)
		{
			yield return null;
		}
		this.giantHead.StartBombEye();
		while (this.giantHead.state != DevilLevelGiantHead.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0006178C File Offset: 0x0005F98C
	public IEnumerator skullEye_cr()
	{
		while (this.giantHead.state != DevilLevelGiantHead.State.Idle)
		{
			yield return null;
		}
		this.giantHead.StartSkullEye();
		while (this.giantHead.state != DevilLevelGiantHead.State.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000117 RID: 279
	public LevelProperties.Devil properties;

	// Token: 0x04000118 RID: 280
	public const float Phase2FadeInTime = 2f;

	// Token: 0x04000119 RID: 281
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x0400011A RID: 282
	[SerializeField]
	public Sprite _bossPortraitPhaseTwo;

	// Token: 0x0400011B RID: 283
	[SerializeField]
	public Sprite _bossPortraitPhaseThree;

	// Token: 0x0400011C RID: 284
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x0400011D RID: 285
	[SerializeField]
	public string _bossQuotePhaseTwo;

	// Token: 0x0400011E RID: 286
	[SerializeField]
	public string _bossQuotePhaseThree;

	// Token: 0x0400011F RID: 287
	[SerializeField]
	public GameObject groundHandler;

	// Token: 0x04000120 RID: 288
	[SerializeField]
	public ParallaxLayer[] parallax;

	// Token: 0x04000121 RID: 289
	[SerializeField]
	public GameObject pit;

	// Token: 0x04000122 RID: 290
	[SerializeField]
	public GameObject middlePiece;

	// Token: 0x04000123 RID: 291
	[SerializeField]
	public Transform phase1Scroll;

	// Token: 0x04000124 RID: 292
	[SerializeField]
	public SpriteRenderer phase1Foreground;

	// Token: 0x04000125 RID: 293
	[SerializeField]
	public GameObject phase2Background;

	// Token: 0x04000126 RID: 294
	[SerializeField]
	public GameObject phase3Platforms;

	// Token: 0x04000127 RID: 295
	[SerializeField]
	public SpriteRenderer phase1Fade;

	// Token: 0x04000128 RID: 296
	[SerializeField]
	public DevilLevelSittingDevil sittingDevil;

	// Token: 0x04000129 RID: 297
	[SerializeField]
	public DevilLevelGiantHead giantHead;

	// Token: 0x0400012A RID: 298
	[SerializeField]
	public DevilLevelEffectSpawner[] smokeSpawners;

	// Token: 0x0400012B RID: 299
	[SerializeField]
	public Transform Phase2P1spawn;

	// Token: 0x0400012C RID: 300
	[SerializeField]
	public Transform Phase2P2spawn;
}
