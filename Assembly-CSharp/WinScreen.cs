using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

// Token: 0x020005C0 RID: 1472
public class WinScreen : AbstractMonoBehaviour
{
	// Token: 0x06003DAA RID: 15786 RVA: 0x001197D8 File Offset: 0x001179D8
	public override void Awake()
	{
		base.Awake();
		this.OnePlayerCuphead.SetActive(false);
		this.TwoPlayerCupheadMugman.SetActive(false);
		Cuphead.Init(false);
		LevelScoringData scoringData = Level.ScoringData;
		if (scoringData != null)
		{
			this.player1IsChalice = scoringData.player1IsChalice;
			this.player2IsChalice = scoringData.player2IsChalice;
		}
		if (!PlayerManager.Multiplayer)
		{
			this.player2IsChalice = false;
		}
		if (Localization.language != Localization.Languages.English)
		{
			this.DisableEnglishMDHRSubtitles();
		}
		if (Localization.language == Localization.Languages.Japanese)
		{
			this.CenterResultTitles(this.japaneseTitleRoot);
		}
		else if (Localization.language == Localization.Languages.Korean)
		{
			this.CenterResultTitles(this.koreanTitleRoot);
		}
		else if (Localization.language == Localization.Languages.SimplifiedChinese || Localization.language == Localization.Languages.German || Localization.language == Localization.Languages.SpanishSpain || Localization.language == Localization.Languages.SpanishAmerica || Localization.language == Localization.Languages.Russian || Localization.language == Localization.Languages.PortugueseBrazil)
		{
			this.CenterResultTitles(this.chineseTitleRoot);
		}
		if (PlayerManager.Multiplayer)
		{
			Animator animator;
			if (PlayerManager.player1IsMugman)
			{
				if (this.player1IsChalice)
				{
					animator = this.TwoPlayerTitleChaliceCuphead;
					this.TwoPlayerChaliceCuphead.SetActive(true);
					this.results.transform.position = this.TwoPlayerChaliceCupheadUIRoot.transform.position;
				}
				else if (this.player2IsChalice)
				{
					animator = this.TwoPlayerTitleMugmanChalice;
					this.TwoPlayerMugmanChalice.SetActive(true);
					this.results.transform.position = this.TwoPlayerMugmanChaliceUIRoot.transform.position;
				}
				else
				{
					animator = this.TwoPlayerTitleMugman;
					this.TwoPlayerMugmanCuphead.SetActive(true);
					this.results.transform.position = this.TwoPlayerMugmanCupheadUIRoot.transform.position;
				}
			}
			else if (this.player1IsChalice)
			{
				animator = this.TwoPlayerTitleChaliceMugman;
				this.TwoPlayerChaliceMugman.SetActive(true);
				this.results.transform.position = this.TwoPlayerChaliceMugmanUIRoot.transform.position;
			}
			else if (this.player2IsChalice)
			{
				animator = this.TwoPlayerTitleCupheadChalice;
				this.TwoPlayerCupheadChalice.SetActive(true);
				this.results.transform.position = this.TwoPlayerCupheadChaliceUIRoot.transform.position;
			}
			else
			{
				animator = this.TwoPlayerTitleCuphead;
				this.TwoPlayerCupheadMugman.SetActive(true);
				this.results.transform.position = this.TwoPlayerCupheadMugmanUIRoot.transform.position;
			}
			if (Localization.language == Localization.Languages.Polish || Localization.language == Localization.Languages.Italian || Localization.language == Localization.Languages.French)
			{
				this.CenterResultTitles(this.japaneseTitleRoot);
			}
			if (Localization.language == Localization.Languages.English)
			{
				animator.SetBool("pickedA", Rand.Bool());
			}
			animator.SetTrigger(this.GetTriggerName(Localization.language));
		}
		else
		{
			Animator animator;
			if (this.player1IsChalice)
			{
				animator = this.OnePlayerTitleChalice;
				this.OnePlayerChalice.SetActive(true);
			}
			else if (PlayerManager.player1IsMugman)
			{
				animator = this.OnePlayerTitleMugman;
				this.OnePlayerMugman.SetActive(true);
			}
			else
			{
				animator = this.OnePlayerTitleCuphead;
				this.OnePlayerCuphead.SetActive(true);
			}
			this.results.transform.position = this.OnePlayerUIRoot.transform.position;
			if (Localization.language == Localization.Languages.Polish || Localization.language == Localization.Languages.Italian || Localization.language == Localization.Languages.French || Localization.language == Localization.Languages.SimplifiedChinese || Localization.language == Localization.Languages.Japanese)
			{
				this.CenterResultTitles(this.playerOneOffCenterTitleRoot);
			}
			if (Localization.language == Localization.Languages.English)
			{
				animator.SetBool("pickedA", Rand.Bool());
			}
			animator.SetTrigger(this.GetTriggerName(Localization.language));
		}
		base.StartCoroutine(this.main_cr());
		this.continuePrompt.SetActive(false);
		this.input = new CupheadInput.AnyPlayerInput(false);
		base.StartCoroutine(this.rotate_bg_cr());
	}

	// Token: 0x06003DAB RID: 15787 RVA: 0x00119BDC File Offset: 0x00117DDC
	public void DisableEnglishMDHRSubtitles()
	{
		foreach (SpriteRenderer spriteRenderer in this.studioMHDRSubtitles)
		{
			spriteRenderer.enabled = false;
		}
	}

	// Token: 0x06003DAC RID: 15788 RVA: 0x00119C10 File Offset: 0x00117E10
	public void CenterResultTitles(Vector3 rootPosition)
	{
		if (this.player1IsChalice)
		{
			rootPosition += ((!PlayerManager.Multiplayer) ? this.chaliceTitleOffset1P : this.chaliceTitleOffset2P);
		}
		foreach (Transform transform in this.resultsTitles)
		{
			transform.localPosition = rootPosition;
		}
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x00119C74 File Offset: 0x00117E74
	public IEnumerator main_cr()
	{
		LevelScoringData data = Level.ScoringData;
		if (Localization.language == Localization.Languages.Korean)
		{
			foreach (TextMeshProUGUI textMeshProUGUI in this.scoring.GetComponentsInChildren<TextMeshProUGUI>())
			{
				textMeshProUGUI.fontStyle = FontStyles.Bold;
			}
			this.gradeLabel.fontStyle = FontStyles.Bold;
		}
		if (data.difficulty == Level.Mode.Easy && Level.PreviousDifficulty == Level.Mode.Easy && Level.PreviousLevelType == Level.Type.Battle && !Level.IsDicePalace && !Level.IsDicePalaceMain && Level.PreviousLevel != Levels.Devil && Level.PreviousLevel != Levels.Saltbaker)
		{
			if (Array.IndexOf<Levels>(Level.worldDLCBossLevels, Level.PreviousLevel) >= 0)
			{
				this.isDLCLevel = true;
			}
			else
			{
				this.isDLCLevel = false;
			}
			Localization.Translation translation = (!this.isDLCLevel) ? Localization.Translate("ResultsTryRegular") : Localization.Translate("WinScreen_Tooltip_SimpleIngredient");
			this.tryRegular.SetActive(true);
			if ((translation.image == null && !translation.hasSpriteAtlasImage) || this.isDLCLevel)
			{
				this.tryRegular.GetComponent<SpriteRenderer>().enabled = false;
				this.tryRegularEnglishBackground.enabled = false;
				this.tryRegularText.text = translation.text;
				this.tryRegularText.font = translation.fonts.fontAsset;
				this.tryRegularText.fontSize = ((translation.fonts.fontAssetSize != 0f) ? translation.fonts.fontAssetSize : this.tryRegularText.fontSize);
				this.tryRegularText.outlineWidth = ((Localization.language != Localization.Languages.Korean) ? this.tryRegularText.outlineWidth : 0.07f);
				this.AlignBannerText(this.tryRegularText.gameObject);
				if (Localization.language == Localization.Languages.Korean || Localization.language == Localization.Languages.Japanese)
				{
					this.postProcessingScript.profile = this.asianProfile;
				}
				this.AlignBannerText(this.glowingText);
				this.glowScript.InitTMPText(new MaskableGraphic[]
				{
					this.tryRegularText
				});
				if (Localization.language != Localization.Languages.English || this.isDLCLevel)
				{
					this.glowScript.BeginGlow();
				}
			}
			else
			{
				this.tryRegularText.enabled = false;
			}
		}
		if (data == null)
		{
			yield break;
		}
		this.timeTicker.TargetValue = (int)data.time;
		this.timeTicker.MaxValue = (int)data.goalTime;
		this.hitsTicker.TargetValue = Mathf.Clamp(data.finalHP, 0, 3);
		this.hitsTicker.MaxValue = 3;
		this.parriesTicker.TargetValue = Mathf.Min(data.numParries, (int)Cuphead.Current.ScoringProperties.parriesForHighestGrade);
		this.parriesTicker.MaxValue = (int)Cuphead.Current.ScoringProperties.parriesForHighestGrade;
		this.superMeterTicker.TargetValue = Mathf.Min(data.superMeterUsed, (int)Cuphead.Current.ScoringProperties.superMeterUsageForHighestGrade);
		this.superMeterTicker.MaxValue = (int)Cuphead.Current.ScoringProperties.superMeterUsageForHighestGrade;
		if (data.useCoinsInsteadOfSuperMeter)
		{
			this.superMeterTicker.TargetValue = data.coinsCollected;
			this.superMeterTicker.MaxValue = 5;
			this.spiritStockLabelLocalizationHelper.currentID = Localization.Find("ResultsMenuCoins").id;
		}
		this.difficultyTicker.TargetValue = ((data.difficulty != Level.Mode.Easy) ? ((data.difficulty != Level.Mode.Normal) ? 2 : 1) : 0);
		this.gradeDisplay.Grade = Level.Grade;
		this.gradeDisplay.Difficulty = data.difficulty;
		yield return new WaitForSeconds(this.introDelay);
		WinScreenTicker[] tickers = new WinScreenTicker[]
		{
			this.timeTicker,
			this.hitsTicker,
			this.parriesTicker,
			this.superMeterTicker,
			this.difficultyTicker
		};
		foreach (WinScreenTicker ticker in tickers)
		{
			ticker.StartCounting();
			while (!ticker.FinishedCounting)
			{
				yield return null;
			}
			if (ticker.TargetValue != 0)
			{
				yield return new WaitForSeconds(this.talliesDelay);
			}
		}
		InterruptingPrompt.SetCanInterrupt(true);
		float timer = 0f;
		while (timer < this.gradeDelay)
		{
			if (this.input.GetAnyButtonDown())
			{
				break;
			}
			if (!InterruptingPrompt.IsInterrupting())
			{
				timer += Time.deltaTime;
			}
			yield return null;
		}
		this.gradeDisplay.Show();
		while (!this.gradeDisplay.FinishedGrading)
		{
			yield return null;
		}
		timer = 0f;
		this.continuePrompt.SetActive(true);
		while (timer < this.advanceDelay)
		{
			if (this.input.GetActionButtonDown())
			{
				break;
			}
			if (!InterruptingPrompt.IsInterrupting())
			{
				timer += Time.deltaTime;
			}
			yield return null;
		}
		if (Level.PreviousLevel == Levels.Devil)
		{
			Cutscene.Load(Scenes.scene_title, Scenes.scene_cutscene_outro, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass);
		}
		else if (Level.PreviousLevel == Levels.Saltbaker)
		{
			Cutscene.Load(Scenes.scene_map_world_DLC, Scenes.scene_cutscene_dlc_ending, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass);
		}
		else
		{
			SceneLoader.LoadLastMap();
		}
		yield break;
	}

	// Token: 0x06003DAE RID: 15790 RVA: 0x00119C90 File Offset: 0x00117E90
	public void AlignBannerText(GameObject bannerText)
	{
		bannerText.GetComponent<TextMeshCurveAndJitter>().CurveScale = (float)((!this.isDLCLevel) ? WinScreen.TryRegularCurveValues[(int)Localization.language] : WinScreen.TryRegularCurveValuesDLC[(int)Localization.language]);
		Vector3 localPosition = bannerText.transform.localPosition;
		localPosition.y = (float)((!this.isDLCLevel) ? WinScreen.TryRegularCurveOffsets[(int)Localization.language] : WinScreen.TryRegularCurveOffsetsDLC[(int)Localization.language]);
		bannerText.transform.localPosition = localPosition;
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x00119D18 File Offset: 0x00117F18
	public IEnumerator rotate_bg_cr()
	{
		float frameTime = 0f;
		float normalTime = 0f;
		float speed = 50f;
		for (;;)
		{
			frameTime += CupheadTime.Delta;
			while (frameTime > 0.0416666679f)
			{
				frameTime -= 0.0416666679f;
				this.Background.Rotate(0f, 0f, speed * CupheadTime.Delta);
				yield return null;
			}
			if (this.gradeDisplay.Celebration && speed < 150f)
			{
				normalTime += CupheadTime.Delta;
				speed = Mathf.Lerp(50f, 150f, normalTime / 0.5f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003DB0 RID: 15792 RVA: 0x00119D34 File Offset: 0x00117F34
	public string GetTriggerName(Localization.Languages language)
	{
		switch (language)
		{
		case Localization.Languages.French:
			return "useFrench";
		case Localization.Languages.Italian:
			return "useItalian";
		case Localization.Languages.German:
			return "useGerman";
		case Localization.Languages.SpanishSpain:
			return "useSpanishSpain";
		case Localization.Languages.SpanishAmerica:
			return "useSpanishAmerica";
		case Localization.Languages.Korean:
			return "useKorean";
		case Localization.Languages.Russian:
			return "useRussian";
		case Localization.Languages.Polish:
			return "usePolish";
		case Localization.Languages.PortugueseBrazil:
			return "usePortuguese";
		case Localization.Languages.Japanese:
			return "useJapanese";
		case Localization.Languages.SimplifiedChinese:
			return "useChinese";
		default:
			return "useEnglish";
		}
	}

	// Token: 0x04003135 RID: 12597
	public static readonly int[] TryRegularCurveValues = new int[]
	{
		0,
		62,
		48,
		57,
		65,
		74,
		58,
		36,
		54,
		72,
		83,
		27
	};

	// Token: 0x04003136 RID: 12598
	public static readonly int[] TryRegularCurveOffsets = new int[]
	{
		0,
		32,
		18,
		27,
		35,
		45,
		28,
		7,
		24,
		42,
		50,
		-3
	};

	// Token: 0x04003137 RID: 12599
	public static readonly int[] TryRegularCurveValuesDLC = new int[]
	{
		62,
		62,
		48,
		57,
		65,
		74,
		58,
		36,
		54,
		72,
		83,
		27
	};

	// Token: 0x04003138 RID: 12600
	public static readonly int[] TryRegularCurveOffsetsDLC = new int[]
	{
		32,
		32,
		18,
		27,
		35,
		45,
		28,
		7,
		24,
		42,
		50,
		-3
	};

	// Token: 0x04003139 RID: 12601
	public const float BOB_FRAME_TIME = 0.0416666679f;

	// Token: 0x0400313A RID: 12602
	[Header("Delays")]
	[SerializeField]
	public float introDelay = 10f;

	// Token: 0x0400313B RID: 12603
	[SerializeField]
	public float talliesDelay = 0.5f;

	// Token: 0x0400313C RID: 12604
	[SerializeField]
	public float gradeDelay = 0.7f;

	// Token: 0x0400313D RID: 12605
	[SerializeField]
	public float advanceDelay = 10f;

	// Token: 0x0400313E RID: 12606
	[SerializeField]
	public WinScreenTicker timeTicker;

	// Token: 0x0400313F RID: 12607
	[SerializeField]
	public WinScreenTicker hitsTicker;

	// Token: 0x04003140 RID: 12608
	[SerializeField]
	public WinScreenTicker parriesTicker;

	// Token: 0x04003141 RID: 12609
	[SerializeField]
	public WinScreenTicker superMeterTicker;

	// Token: 0x04003142 RID: 12610
	[SerializeField]
	public WinScreenTicker difficultyTicker;

	// Token: 0x04003143 RID: 12611
	[SerializeField]
	public LocalizationHelper spiritStockLabelLocalizationHelper;

	// Token: 0x04003144 RID: 12612
	[SerializeField]
	public WinScreenGradeDisplay gradeDisplay;

	// Token: 0x04003145 RID: 12613
	[SerializeField]
	public GameObject continuePrompt;

	// Token: 0x04003146 RID: 12614
	public bool player1IsChalice;

	// Token: 0x04003147 RID: 12615
	public bool player2IsChalice;

	// Token: 0x04003148 RID: 12616
	[Header("UI Scoring")]
	[SerializeField]
	public GameObject scoring;

	// Token: 0x04003149 RID: 12617
	[SerializeField]
	public TextMeshProUGUI gradeLabel;

	// Token: 0x0400314A RID: 12618
	[Header("Try Text")]
	[SerializeField]
	public GameObject tryRegular;

	// Token: 0x0400314B RID: 12619
	[SerializeField]
	public TMP_Text tryRegularText;

	// Token: 0x0400314C RID: 12620
	[SerializeField]
	public SpriteRenderer tryRegularEnglishBackground;

	// Token: 0x0400314D RID: 12621
	[Header("Glow effect")]
	[SerializeField]
	public GameObject glowingText;

	// Token: 0x0400314E RID: 12622
	[SerializeField]
	public GlowText glowScript;

	// Token: 0x0400314F RID: 12623
	[SerializeField]
	public PostProcessingBehaviour postProcessingScript;

	// Token: 0x04003150 RID: 12624
	[SerializeField]
	public PostProcessingProfile asianProfile;

	// Token: 0x04003151 RID: 12625
	[SerializeField]
	public GameObject tryExpert;

	// Token: 0x04003152 RID: 12626
	[SerializeField]
	public TMP_Text tryExpertText;

	// Token: 0x04003153 RID: 12627
	[Header("Background")]
	[SerializeField]
	public Transform Background;

	// Token: 0x04003154 RID: 12628
	[Header("DifferentLayouts")]
	[SerializeField]
	public GameObject OnePlayerCuphead;

	// Token: 0x04003155 RID: 12629
	[SerializeField]
	public GameObject OnePlayerMugman;

	// Token: 0x04003156 RID: 12630
	[SerializeField]
	public Transform OnePlayerUIRoot;

	// Token: 0x04003157 RID: 12631
	[SerializeField]
	public Animator OnePlayerTitleCuphead;

	// Token: 0x04003158 RID: 12632
	[SerializeField]
	public Animator OnePlayerTitleMugman;

	// Token: 0x04003159 RID: 12633
	[Space(10f)]
	[SerializeField]
	public GameObject TwoPlayerCupheadMugman;

	// Token: 0x0400315A RID: 12634
	[SerializeField]
	public GameObject TwoPlayerMugmanCuphead;

	// Token: 0x0400315B RID: 12635
	[SerializeField]
	public Transform TwoPlayerCupheadMugmanUIRoot;

	// Token: 0x0400315C RID: 12636
	[SerializeField]
	public Transform TwoPlayerMugmanCupheadUIRoot;

	// Token: 0x0400315D RID: 12637
	[SerializeField]
	public Animator TwoPlayerTitleCuphead;

	// Token: 0x0400315E RID: 12638
	[SerializeField]
	public Animator TwoPlayerTitleMugman;

	// Token: 0x0400315F RID: 12639
	[Space(10f)]
	[SerializeField]
	public GameObject OnePlayerChalice;

	// Token: 0x04003160 RID: 12640
	[SerializeField]
	public GameObject TwoPlayerChaliceCuphead;

	// Token: 0x04003161 RID: 12641
	[SerializeField]
	public GameObject TwoPlayerCupheadChalice;

	// Token: 0x04003162 RID: 12642
	[SerializeField]
	public GameObject TwoPlayerMugmanChalice;

	// Token: 0x04003163 RID: 12643
	[SerializeField]
	public GameObject TwoPlayerChaliceMugman;

	// Token: 0x04003164 RID: 12644
	[SerializeField]
	public Transform OnePlayerChaliceUIRoot;

	// Token: 0x04003165 RID: 12645
	[SerializeField]
	public Transform TwoPlayerChaliceCupheadUIRoot;

	// Token: 0x04003166 RID: 12646
	[SerializeField]
	public Transform TwoPlayerCupheadChaliceUIRoot;

	// Token: 0x04003167 RID: 12647
	[SerializeField]
	public Transform TwoPlayerMugmanChaliceUIRoot;

	// Token: 0x04003168 RID: 12648
	[SerializeField]
	public Transform TwoPlayerChaliceMugmanUIRoot;

	// Token: 0x04003169 RID: 12649
	[SerializeField]
	public Animator OnePlayerTitleChalice;

	// Token: 0x0400316A RID: 12650
	[SerializeField]
	public Animator TwoPlayerTitleChaliceCuphead;

	// Token: 0x0400316B RID: 12651
	[SerializeField]
	public Animator TwoPlayerTitleCupheadChalice;

	// Token: 0x0400316C RID: 12652
	[SerializeField]
	public Animator TwoPlayerTitleMugmanChalice;

	// Token: 0x0400316D RID: 12653
	[SerializeField]
	public Animator TwoPlayerTitleChaliceMugman;

	// Token: 0x0400316E RID: 12654
	[Space(10f)]
	[SerializeField]
	public SpriteRenderer[] studioMHDRSubtitles;

	// Token: 0x0400316F RID: 12655
	[SerializeField]
	public Transform[] resultsTitles;

	// Token: 0x04003170 RID: 12656
	[SerializeField]
	public Vector3 playerOneOffCenterTitleRoot;

	// Token: 0x04003171 RID: 12657
	[SerializeField]
	public Vector3 japaneseTitleRoot;

	// Token: 0x04003172 RID: 12658
	[SerializeField]
	public Vector3 koreanTitleRoot;

	// Token: 0x04003173 RID: 12659
	[SerializeField]
	public Vector3 chineseTitleRoot;

	// Token: 0x04003174 RID: 12660
	[Space(10f)]
	[SerializeField]
	public Vector3 chaliceTitleOffset1P;

	// Token: 0x04003175 RID: 12661
	[SerializeField]
	public Vector3 chaliceTitleOffset2P;

	// Token: 0x04003176 RID: 12662
	[Space(10f)]
	[SerializeField]
	public Canvas results;

	// Token: 0x04003177 RID: 12663
	[Header("BannerCurve")]
	[SerializeField]
	public MinMax textWidthRange;

	// Token: 0x04003178 RID: 12664
	[SerializeField]
	public MinMax curveScaleRange;

	// Token: 0x04003179 RID: 12665
	[SerializeField]
	public float yOffsetDelta;

	// Token: 0x0400317A RID: 12666
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x0400317B RID: 12667
	public const float BG_NORMAL_SPEED = 50f;

	// Token: 0x0400317C RID: 12668
	public const float BG_FAST_SPEED = 150f;

	// Token: 0x0400317D RID: 12669
	public bool isDLCLevel;
}
