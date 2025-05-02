using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x0200003C RID: 60
public class SaltbakerLevel : Level
{
	// Token: 0x060003E0 RID: 992 RVA: 0x0006810C File Offset: 0x0006630C
	public override void PartialInit()
	{
		this.properties = LevelProperties.Saltbaker.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x060003E1 RID: 993 RVA: 0x00004E46 File Offset: 0x00003046
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Saltbaker;
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x060003E2 RID: 994 RVA: 0x00004E4D File Offset: 0x0000304D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_saltbaker;
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x060003E3 RID: 995 RVA: 0x000681A4 File Offset: 0x000663A4
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Saltbaker.States.Main:
				return this._bossPortraitMain;
			case LevelProperties.Saltbaker.States.PhaseTwo:
				return this._bossPortraitPhaseTwo;
			case LevelProperties.Saltbaker.States.PhaseThree:
				return this._bossPortraitPhaseThree;
			case LevelProperties.Saltbaker.States.PhaseFour:
				return this._bossPortraitPhaseFour;
			}
			Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x060003E4 RID: 996 RVA: 0x00068230 File Offset: 0x00066430
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Saltbaker.States.Main:
				return this._bossQuoteMain;
			case LevelProperties.Saltbaker.States.PhaseTwo:
				return this._bossQuotePhaseTwo;
			case LevelProperties.Saltbaker.States.PhaseThree:
				return this._bossQuotePhaseThree;
			case LevelProperties.Saltbaker.States.PhaseFour:
				return this._bossQuotePhaseFour;
			}
			Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x060003E5 RID: 997 RVA: 0x00004E51 File Offset: 0x00003051
	// (set) Token: 0x060003E6 RID: 998 RVA: 0x00004E59 File Offset: 0x00003059
	public bool playerLost { get; set; }

	// Token: 0x060003E7 RID: 999 RVA: 0x000682BC File Offset: 0x000664BC
	public override void Start()
	{
		base.Start();
		this.trappedCharacter.Setup();
		this.trappedCharacterPhaseThree.Setup();
		this.saltbaker.LevelInit(this.properties);
		this.saltbakerBouncer.LevelInit(this.properties);
		this.saltbakerPillarHandler.LevelInit(this.properties);
		this.fires = new List<SaltbakerLevelJumper>();
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x00004E62 File Offset: 0x00003062
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
		this._bossPortraitPhaseFour = null;
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x00004E86 File Offset: 0x00003086
	public override void OnLose()
	{
		this.playerLost = true;
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x00068324 File Offset: 0x00066524
	public override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Saltbaker.States.PhaseTwo)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.saltbaker.phase_one_to_two_cr());
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Saltbaker.States.PhaseThree)
		{
			this.StopAllCoroutines();
			this.KillFires();
			this.saltbaker.OnPhaseThree();
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Saltbaker.States.PhaseFour)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.phase_three_to_four_cr());
		}
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x000683C8 File Offset: 0x000665C8
	public void StartPhase3()
	{
		this.ClearFires();
		this.phase3BG.SetActive(true);
		this.bounds.bottomEnabled = false;
		GameObject.Find("Level_Ground").SetActive(false);
		Level.Current.SetBounds(null, null, new int?(500), null);
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController != null)
			{
				levelPlayerController.transform.position = new Vector3(levelPlayerController.transform.position.x, (float)Level.Current.Ceiling);
				levelPlayerController.motor.DoPostSuperHop();
			}
		}
		this.SpawnCutters();
		base.StartCoroutine(this.phase_two_to_three_cr());
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x000684D8 File Offset: 0x000666D8
	public IEnumerator phase_two_to_three_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float t = 0f;
		while (t < 1f)
		{
			t += CupheadTime.FixedDelta;
			this.transitionFader.color = new Color(1f, 1f, 1f, Mathf.InverseLerp(1f, 0f, t));
			yield return wait;
		}
		this.saltbakerBouncer.gameObject.SetActive(true);
		this.saltbakerBouncer.StartBouncer(new Vector3(0f, 700f));
		this.transitionFader.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x000684F4 File Offset: 0x000666F4
	public IEnumerator phase_three_to_four_cr()
	{
		this.DestroyRunners();
		this.saltbakerBouncer.EndBouncer();
		this.groundCrack.Play("Start");
		base.StartCoroutine(this.flash_sky_cr());
		this.tornadoActivator.enabled = true;
		this.phase3to4Transition.StartSaltman();
		while (this.saltbakerBouncer != null)
		{
			yield return null;
		}
		this.saltbakerPillarHandler.gameObject.SetActive(true);
		this.saltbakerPillarHandler.StartPlatforms();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.saltbakerPillarHandler.suppressCenterPlatforms = false;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.groundCrack.SetTrigger("Continue");
		this.tornadoActivator.SetTrigger("Continue");
		this.saltbakerPillarHandler.StartPillarOfDoom();
		while (this.phase3to4Transition.enabled)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.saltbakerPillarHandler.StartHeart();
		base.StartCoroutine(this.bg_salt_spillage_cr());
		UnityEngine.Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer("Renderer");
		this.phaseFourBlurCamera.SetActive(true);
		this.phaseFourBlurTexture.gameObject.SetActive(true);
		float t = 0f;
		while (t < this.phaseFourBlurDimTime)
		{
			t += CupheadTime.Delta;
			float tNormalized = t / this.phaseFourBlurDimTime;
			this.phaseFourBlurController.blurSize = Mathf.Lerp(0f, this.phaseFourBlurAmount, tNormalized);
			this.phaseFourBlurTexture.material.color = Color.Lerp(Color.white, new Color(this.phaseFourDimAmount, this.phaseFourDimAmount, this.phaseFourDimAmount, 1f), tNormalized);
			yield return null;
		}
		this.phaseFourBlurController.blurSize = this.phaseFourBlurAmount;
		this.phaseFourBlurTexture.material.color = new Color(this.phaseFourDimAmount, this.phaseFourDimAmount, this.phaseFourDimAmount, 1f);
		yield break;
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x00068510 File Offset: 0x00066710
	public IEnumerator flash_sky_cr()
	{
		for (;;)
		{
			float dimRate = Random.Range(2f, 4f);
			this.skyFront.color = Color.white;
			while (this.skyFront.color.r > 0f)
			{
				float c = this.skyFront.color.r - 0.0416666679f * dimRate;
				this.skyFront.color = new Color(c, c, c, 1f);
				yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
			}
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.1f, 4f));
		}
		yield break;
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0006852C File Offset: 0x0006672C
	public IEnumerator bg_salt_spillage_cr()
	{
		this.saltSpillageDelay = new PatternString(this.saltSpillageDelayString, true);
		this.saltSpillageOrder = new PatternString(this.saltSpillageOrderString, true);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.saltSpillageDelay.PopFloat());
			this.groundCrack.Play("Spill", this.saltSpillageOrder.PopInt(), 0f);
		}
		yield break;
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x00068548 File Offset: 0x00066748
	public void SpawnJumpers()
	{
		int numberFireJumpers = this.properties.CurrentState.jumper.numberFireJumpers;
		if (numberFireJumpers == 0)
		{
			return;
		}
		float num = (float)Level.Current.Ceiling;
		float num2 = (float)Level.Current.Left;
		float num3 = (float)(Level.Current.Width / numberFireJumpers);
		float num4 = num2 + num3 / 2f;
		float jumpDelay = this.properties.CurrentState.jumper.jumpDelay;
		for (int i = 0; i < numberFireJumpers; i++)
		{
			float num5 = num4 + num3 * (float)i;
			Vector3 position;
			position..ctor(num5, num);
			SaltbakerLevelJumper saltbakerLevelJumper = this.jumperPrefab.Create(position, this, this.properties.CurrentState.swooper, this.properties.CurrentState.jumper, jumpDelay * (float)i, false);
			saltbakerLevelJumper.GetComponent<SpriteRenderer>().sortingOrder = i + -5;
			this.fires.Add(saltbakerLevelJumper);
		}
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0006863C File Offset: 0x0006683C
	public void SpawnSwoopers()
	{
		float[] array = new float[]
		{
			(float)Level.Current.Right / 2f,
			(float)Level.Current.Left / 2f
		};
		int num = this.properties.CurrentState.swooper.numberFireSwoopers;
		if (num > 2)
		{
			Debug.Break();
			num = 2;
		}
		if (num == 0)
		{
			return;
		}
		float num2 = (float)(Level.Current.Left + Level.Current.Right) / 2f;
		float jumpDelay = this.properties.CurrentState.swooper.jumpDelay;
		for (int i = 0; i < num; i++)
		{
			SaltbakerLevelJumper saltbakerLevelJumper = this.jumperPrefab.Create(new Vector3(array[i], (float)Level.Current.Ceiling), this, this.properties.CurrentState.swooper, this.properties.CurrentState.jumper, jumpDelay * (float)i, true);
			saltbakerLevelJumper.GetComponent<SpriteRenderer>().sortingOrder = 3 + i;
			this.fires.Add(saltbakerLevelJumper);
		}
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x00068754 File Offset: 0x00066954
	public void KillFires()
	{
		foreach (SaltbakerLevelJumper saltbakerLevelJumper in this.fires)
		{
			saltbakerLevelJumper.Die();
		}
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x000687B0 File Offset: 0x000669B0
	public void ClearFires()
	{
		foreach (SaltbakerLevelJumper saltbakerLevelJumper in this.fires)
		{
			if (saltbakerLevelJumper != null)
			{
				Object.Destroy(saltbakerLevelJumper.gameObject);
			}
		}
		this.fires.Clear();
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x00068828 File Offset: 0x00066A28
	public bool IsPositionAvailable(Vector3 pos, SaltbakerLevelJumper fire)
	{
		float num = fire.GetComponent<Collider2D>().bounds.size.x * 1f;
		for (int i = 0; i < this.fires.Count; i++)
		{
			if (this.fires[i] != fire)
			{
				float num2 = pos.x + num;
				float num3 = pos.x - num;
				if (num2 > this.fires[i].GetAimPos().x - num && num3 < this.fires[i].GetAimPos().x + num)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x000688EC File Offset: 0x00066AEC
	public void SpawnCutters()
	{
		LevelProperties.Saltbaker.Cutter cutter = this.properties.CurrentState.cutter;
		if (cutter.cutterCount <= 0)
		{
			return;
		}
		this.cutters = new List<SaltbakerLevelCutter>();
		AbstractPlayerController next = PlayerManager.GetNext();
		float num = 50f;
		float minDistance = 50f;
		float[] array = new float[2];
		bool flag = Rand.Bool();
		List<Vector2> list = new List<Vector2>();
		float num2 = Mathf.Min(PlayerManager.GetNext().transform.position.x, PlayerManager.GetNext().transform.position.x);
		float num3 = Mathf.Max(PlayerManager.GetNext().transform.position.x, PlayerManager.GetNext().transform.position.x);
		list.Add(new Vector2((float)Level.Current.Left + num, num2));
		list.Add(new Vector2(num2, num3));
		list.Add(new Vector2(num3, (float)Level.Current.Right - num));
		list.RemoveAll((Vector2 s) => Mathf.Abs(s.x - s.y) < minDistance * 2f);
		list.Sort((Vector2 s1, Vector2 s2) => Mathf.Abs(s1.x - s1.y).CompareTo(Mathf.Abs(s2.x - s2.y)));
		if (list.Count == 3)
		{
			list.RemoveAt(0);
		}
		if (list.Count == 2)
		{
			array[0] = Mathf.Lerp(list[0].x, list[0].y, 0.5f);
			array[1] = Mathf.Lerp(list[1].x, list[1].y, 0.5f);
		}
		if (list.Count == 1)
		{
			array[0] = Mathf.Lerp(list[0].x, list[0].y, 0.333f);
			array[1] = Mathf.Lerp(list[0].x, list[0].y, 0.667f);
		}
		for (int i = 0; i < cutter.cutterCount; i++)
		{
			Vector3 position;
			position..ctor(array[i], (float)Level.Current.Ground + 26f);
			SaltbakerLevelCutter item = this.cutterPrefab.Create(position, cutter.cutterSpeed, flag, i);
			flag = !flag;
			this.cutters.Add(item);
		}
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x00068B98 File Offset: 0x00066D98
	public void DestroyRunners()
	{
		if (this.cutters == null)
		{
			return;
		}
		foreach (SaltbakerLevelCutter saltbakerLevelCutter in this.cutters)
		{
			saltbakerLevelCutter.Sink();
		}
		this.cutters.Clear();
	}

	// Token: 0x040002CA RID: 714
	public LevelProperties.Saltbaker properties;

	// Token: 0x040002CB RID: 715
	public const float FIRE_OFFSET_MODIFIER = 1f;

	// Token: 0x040002CC RID: 716
	[SerializeField]
	public GameObject phase3BG;

	// Token: 0x040002CD RID: 717
	[SerializeField]
	public GameObject[] cracksBG;

	// Token: 0x040002CE RID: 718
	[SerializeField]
	public SaltbakerLevelCutter cutterPrefab;

	// Token: 0x040002CF RID: 719
	public List<SaltbakerLevelCutter> cutters;

	// Token: 0x040002D0 RID: 720
	[SerializeField]
	public SaltbakerLevelJumper jumperPrefab;

	// Token: 0x040002D1 RID: 721
	public List<SaltbakerLevelJumper> fires;

	// Token: 0x040002D2 RID: 722
	[SerializeField]
	public SaltbakerLevelSaltbaker saltbaker;

	// Token: 0x040002D3 RID: 723
	[SerializeField]
	public SaltbakerLevelBouncer saltbakerBouncer;

	// Token: 0x040002D4 RID: 724
	[SerializeField]
	public SaltbakerLevelPillarHandler saltbakerPillarHandler;

	// Token: 0x040002D5 RID: 725
	[SerializeField]
	public SpriteRenderer skyFront;

	// Token: 0x040002D6 RID: 726
	[SerializeField]
	public SpriteRenderer transitionFader;

	// Token: 0x040002D7 RID: 727
	[SerializeField]
	public SaltbakerLevelPhaseThreeToFourTransition phase3to4Transition;

	// Token: 0x040002D8 RID: 728
	[SerializeField]
	public string saltSpillageOrderString;

	// Token: 0x040002D9 RID: 729
	public PatternString saltSpillageOrder;

	// Token: 0x040002DA RID: 730
	[SerializeField]
	public string saltSpillageDelayString;

	// Token: 0x040002DB RID: 731
	public PatternString saltSpillageDelay;

	// Token: 0x040002DC RID: 732
	[SerializeField]
	public Animator groundCrack;

	// Token: 0x040002DD RID: 733
	[SerializeField]
	public Animator tornadoActivator;

	// Token: 0x040002DE RID: 734
	[SerializeField]
	public GameObject phaseFourBlurCamera;

	// Token: 0x040002DF RID: 735
	[SerializeField]
	public MeshRenderer phaseFourBlurTexture;

	// Token: 0x040002E0 RID: 736
	[SerializeField]
	public float phaseFourBlurAmount = 3f;

	// Token: 0x040002E1 RID: 737
	[SerializeField]
	public float phaseFourDimAmount = 0.8f;

	// Token: 0x040002E2 RID: 738
	[SerializeField]
	public float phaseFourBlurDimTime = 1f;

	// Token: 0x040002E3 RID: 739
	[SerializeField]
	public BlurOptimized phaseFourBlurController;

	// Token: 0x040002E4 RID: 740
	[Header("Boss Info")]
	[SerializeField]
	public Sprite _bossPortraitMain;

	// Token: 0x040002E5 RID: 741
	[SerializeField]
	public Sprite _bossPortraitPhaseTwo;

	// Token: 0x040002E6 RID: 742
	[SerializeField]
	public Sprite _bossPortraitPhaseThree;

	// Token: 0x040002E7 RID: 743
	[SerializeField]
	public Sprite _bossPortraitPhaseFour;

	// Token: 0x040002E8 RID: 744
	[SerializeField]
	public string _bossQuoteMain;

	// Token: 0x040002E9 RID: 745
	[SerializeField]
	public string _bossQuotePhaseTwo;

	// Token: 0x040002EA RID: 746
	[SerializeField]
	public string _bossQuotePhaseThree;

	// Token: 0x040002EB RID: 747
	[SerializeField]
	public string _bossQuotePhaseFour;

	// Token: 0x040002EC RID: 748
	public int crackOn;

	// Token: 0x040002ED RID: 749
	public float yScrollPos;

	// Token: 0x040002EE RID: 750
	[SerializeField]
	public SaltbakerLevelBGTrappedCharacter trappedCharacter;

	// Token: 0x040002EF RID: 751
	[SerializeField]
	public SaltbakerLevelBGTrappedCharacter trappedCharacterPhaseThree;
}
