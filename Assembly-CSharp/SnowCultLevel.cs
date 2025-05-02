using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class SnowCultLevel : Level
{
	// Token: 0x06000410 RID: 1040 RVA: 0x00068FAC File Offset: 0x000671AC
	public override void PartialInit()
	{
		this.properties = LevelProperties.SnowCult.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000411 RID: 1041 RVA: 0x00004F6D File Offset: 0x0000316D
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.SnowCult;
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000412 RID: 1042 RVA: 0x00004F74 File Offset: 0x00003174
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_snow_cult;
		}
	}

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000413 RID: 1043 RVA: 0x00069044 File Offset: 0x00067244
	// (remove) Token: 0x06000414 RID: 1044 RVA: 0x0006907C File Offset: 0x0006727C
	public event Action OnYetiHitGround;

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06000415 RID: 1045 RVA: 0x000690B4 File Offset: 0x000672B4
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.SnowCult.States.Main:
				return this._bossPortraitMain;
			case LevelProperties.SnowCult.States.JackFrost:
				return this._bossPortraitPhaseThree;
			case LevelProperties.SnowCult.States.Yeti:
			case LevelProperties.SnowCult.States.EasyYeti:
				return this._bossPortraitPhaseTwo;
			}
			Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06000416 RID: 1046 RVA: 0x00069138 File Offset: 0x00067338
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.SnowCult.States.Main:
				return this._bossQuoteMain;
			case LevelProperties.SnowCult.States.JackFrost:
				return this._bossQuotePhaseThree;
			case LevelProperties.SnowCult.States.Yeti:
			case LevelProperties.SnowCult.States.EasyYeti:
				return this._bossQuotePhaseTwo;
			}
			Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00004F78 File Offset: 0x00003178
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x00004F95 File Offset: 0x00003195
	public override void Start()
	{
		base.Start();
		this.yeti.LevelInit(this.properties);
		this.jackFrost.LevelInit(this.properties);
		this.wizard.LevelInit(this.properties);
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x00004FD0 File Offset: 0x000031D0
	public override void OnLevelStart()
	{
		base.StartCoroutine(this.snowcultPattern_cr());
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x000691BC File Offset: 0x000673BC
	public IEnumerator snowcultPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x000691D8 File Offset: 0x000673D8
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.SnowCult.States.Yeti)
		{
			base.StartCoroutine(this.to_phase_2_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.SnowCult.States.JackFrost)
		{
			base.StartCoroutine(this.to_phase_3_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.SnowCult.States.EasyYeti)
		{
			base.StartCoroutine(this.to_phase_3_easy_cr());
		}
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00069260 File Offset: 0x00067460
	public IEnumerator nextPattern_cr()
	{
		while (this.wizard != null && (this.wizard.Turning() || this.wizard.dead))
		{
			yield return null;
		}
		LevelProperties.SnowCult.Pattern p = this.properties.CurrentState.NextPattern;
		if (this.firstAttack)
		{
			while (p != LevelProperties.SnowCult.Pattern.Quad)
			{
				p = this.properties.CurrentState.NextPattern;
			}
			this.firstAttack = false;
		}
		switch (p)
		{
		case LevelProperties.SnowCult.Pattern.Switch:
			yield return base.StartCoroutine(this.switch_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.Eye:
			yield return base.StartCoroutine(this.eye_attack_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.Shard:
			yield return base.StartCoroutine(this.shard_attack_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.Mouth:
			yield return base.StartCoroutine(this.mouth_shot_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.Quad:
			yield return base.StartCoroutine(this.quad_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.Block:
			yield return base.StartCoroutine(this.ice_block_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.SeriesShot:
			yield return base.StartCoroutine(this.series_shot_cr());
			goto IL_2E6;
		case LevelProperties.SnowCult.Pattern.Yeti:
			goto IL_2E6;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		IL_2E6:
		yield break;
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0006927C File Offset: 0x0006747C
	public IEnumerator to_phase_2_cr()
	{
		this.firstAttack = false;
		this.wizard.ToOutro(this.yeti);
		yield return null;
		yield break;
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x00004FDF File Offset: 0x000031DF
	public void CultistsSummon()
	{
		this.cultists.SetTrigger("Summon");
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x00004FF1 File Offset: 0x000031F1
	public void YetiHitGround()
	{
		if (this.OnYetiHitGround != null)
		{
			this.OnYetiHitGround();
		}
		this.cultists.SetTrigger("Summon");
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x00069298 File Offset: 0x00067498
	public IEnumerator to_phase_3_easy_cr()
	{
		this.yeti.ToEasyPhaseThree();
		yield return null;
		yield break;
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x000692B4 File Offset: 0x000674B4
	public IEnumerator to_phase_3_cr()
	{
		this.yeti.ForceOutroToStart();
		while (this.yeti.state != SnowCultLevelYeti.States.Idle || this.yeti.inBallForm)
		{
			yield return null;
		}
		this.cultists.SetTrigger("Summon");
		this.yeti.OnDeath();
		this.jackFrost.Intro();
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.yeti.timeToPlatforms);
		this.jackFrost.CreatePlatforms();
		base.StartCoroutine(this.SFX_SNOWCULT_IcePlatformAppear_cr());
		base.StartCoroutine(this.SFX_SNOWCULT_P2_to_P3_Transition_cr());
		for (int i = 0; i < 5; i++)
		{
			this.jackFrost.CreateAscendingPlatform(i);
			if (i < 4)
			{
				yield return CupheadTime.WaitForSeconds(this, 0.2f);
			}
		}
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		LevelPlayerMotor p1Motor = player.GetComponent<LevelPlayerMotor>();
		LevelPlayerMotor p2Motor = null;
		bool hasStarted = false;
		while (!hasStarted)
		{
			if (player2 != null && !player2.IsDead)
			{
				if (p2Motor == null)
				{
					p2Motor = player2.GetComponent<LevelPlayerMotor>();
				}
				if ((player.transform.position.y > -80f && p1Motor.Grounded) || (player2.transform.position.y > -80f && p2Motor.Grounded))
				{
					hasStarted = true;
				}
			}
			else if (player.transform.position.y > -80f && p1Motor.Grounded)
			{
				hasStarted = true;
			}
			yield return null;
		}
		Vector3 cameraEndPos = new Vector3(0f, 950f, 0f);
		float time = this.properties.CurrentState.yeti.timeForCameraMove;
		CupheadLevelCamera.Current.ChangeVerticalBounds(1290, 675);
		this.pit.SetActive(true);
		float cameraStartPos = CupheadLevelCamera.Current.transform.position.y;
		base.StartCoroutine(CupheadLevelCamera.Current.slide_camera_cr(cameraEndPos, time));
		time = 0f;
		while (time < 0.5f)
		{
			time = Mathf.InverseLerp(cameraStartPos, cameraEndPos.y, CupheadLevelCamera.Current.transform.position.y);
			yield return null;
		}
		Level.Current.SetBounds(new int?(640), new int?(640), new int?(1290), new int?(675));
		while (time < 0.75f)
		{
			time = Mathf.InverseLerp(cameraStartPos, cameraEndPos.y, CupheadLevelCamera.Current.transform.position.y);
			yield return null;
		}
		this.jackFrost.StartPhase3();
		this.pit.transform.parent = null;
		while (time < 0.95f)
		{
			time = Mathf.InverseLerp(cameraStartPos, cameraEndPos.y, CupheadLevelCamera.Current.transform.position.y);
			this.pit.transform.localPosition = CupheadLevelCamera.Current.transform.position + Vector3.down * 500f;
			yield return null;
		}
		this.pit.transform.localPosition = cameraEndPos + Vector3.down * 500f;
		yield break;
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x000692D0 File Offset: 0x000674D0
	public IEnumerator quad_cr()
	{
		while (this.wizard.state != SnowCultLevelWizard.States.Idle)
		{
			yield return null;
		}
		this.wizard.StartQuadAttack();
		while (this.wizard.state != SnowCultLevelWizard.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x000692EC File Offset: 0x000674EC
	public IEnumerator ice_block_cr()
	{
		while (this.wizard.state != SnowCultLevelWizard.States.Idle)
		{
			yield return null;
		}
		this.wizard.Whale();
		while (this.wizard.state != SnowCultLevelWizard.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00069308 File Offset: 0x00067508
	public IEnumerator series_shot_cr()
	{
		while (this.wizard.state != SnowCultLevelWizard.States.Idle)
		{
			yield return null;
		}
		this.wizard.SeriesShot();
		while (this.wizard.state != SnowCultLevelWizard.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x00069324 File Offset: 0x00067524
	public IEnumerator switch_cr()
	{
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		this.jackFrost.StartSwitch();
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x00069340 File Offset: 0x00067540
	public IEnumerator eye_attack_cr()
	{
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		this.jackFrost.StartEyeAttack();
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x0006935C File Offset: 0x0006755C
	public IEnumerator shard_attack_cr()
	{
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		this.jackFrost.StartShardAttack();
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00069378 File Offset: 0x00067578
	public IEnumerator mouth_shot_cr()
	{
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		this.jackFrost.StartMouthShot();
		while (this.jackFrost.state != SnowCultLevelJackFrost.States.Idle)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x00069394 File Offset: 0x00067594
	public IEnumerator SFX_SNOWCULT_IcePlatformAppear_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		AudioManager.Play("sfx_dlc_snowcult_p2_iceplatform_appear");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_iceplatform_appear");
		yield break;
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x000693B0 File Offset: 0x000675B0
	public IEnumerator SFX_SNOWCULT_P2_to_P3_Transition_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		AudioManager.Play("sfx_dlc_snowcult_p2_snow_cultists_wave_hands_transition");
		yield break;
	}

	// Token: 0x04000304 RID: 772
	public LevelProperties.SnowCult properties;

	// Token: 0x04000305 RID: 773
	public const float CLIMBING_PLATFORMS_INTERAPPEAR_DELAY = 0.2f;

	// Token: 0x04000306 RID: 774
	public const float HEIGHT_TO_START_PHASE_THREE = -80f;

	// Token: 0x04000307 RID: 775
	public const float PHASE_THREE_CAMERA_POS = 950f;

	// Token: 0x04000309 RID: 777
	[SerializeField]
	public SnowCultLevelWizard wizard;

	// Token: 0x0400030A RID: 778
	[SerializeField]
	public SnowCultLevelYeti yeti;

	// Token: 0x0400030B RID: 779
	[SerializeField]
	public SnowCultLevelJackFrost jackFrost;

	// Token: 0x0400030C RID: 780
	[SerializeField]
	public Animator cultists;

	// Token: 0x0400030D RID: 781
	[SerializeField]
	public GameObject pit;

	// Token: 0x0400030E RID: 782
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x0400030F RID: 783
	[SerializeField]
	public Sprite _bossPortraitPhaseTwo;

	// Token: 0x04000310 RID: 784
	[SerializeField]
	public Sprite _bossPortraitPhaseThree;

	// Token: 0x04000311 RID: 785
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x04000312 RID: 786
	[SerializeField]
	public string _bossQuotePhaseTwo;

	// Token: 0x04000313 RID: 787
	[SerializeField]
	public string _bossQuotePhaseThree;

	// Token: 0x04000314 RID: 788
	public bool firstAttack = true;
}
