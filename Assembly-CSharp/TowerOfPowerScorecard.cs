using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

// Token: 0x020005BF RID: 1471
public class TowerOfPowerScorecard : AbstractMonoBehaviour
{
	// Token: 0x06003DA2 RID: 15778 RVA: 0x0011951C File Offset: 0x0011771C
	public override void Awake()
	{
		base.Awake();
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
		Cuphead.Init(false);
		if (PlayerManager.Multiplayer)
		{
			if (Localization.language == Localization.Languages.Polish || Localization.language == Localization.Languages.Italian || Localization.language == Localization.Languages.French)
			{
				this.CenterResultTitles(this.japaneseTitleRoot);
			}
		}
		else if (Localization.language == Localization.Languages.Polish || Localization.language == Localization.Languages.Italian || Localization.language == Localization.Languages.French || Localization.language == Localization.Languages.SimplifiedChinese || Localization.language == Localization.Languages.Japanese)
		{
			this.CenterResultTitles(this.playerOneOffCenterTitleRoot);
		}
		base.StartCoroutine(this.main_cr());
		this.continuePrompt.SetActive(false);
		this.input = new CupheadInput.AnyPlayerInput(false);
	}

	// Token: 0x06003DA3 RID: 15779 RVA: 0x00119674 File Offset: 0x00117874
	public void DisableEnglishMDHRSubtitles()
	{
		foreach (SpriteRenderer spriteRenderer in this.studioMHDRSubtitles)
		{
			spriteRenderer.enabled = false;
		}
	}

	// Token: 0x06003DA4 RID: 15780 RVA: 0x001196A8 File Offset: 0x001178A8
	public void CenterResultTitles(Vector3 rootPosition)
	{
		foreach (Transform transform in this.resultsTitles)
		{
			transform.localPosition = rootPosition;
		}
	}

	// Token: 0x06003DA5 RID: 15781 RVA: 0x001196DC File Offset: 0x001178DC
	public IEnumerator main_cr()
	{
		LevelScoringData data = Level.ScoringData;
		this.done = false;
		if (Localization.language == Localization.Languages.Korean)
		{
			foreach (TextMeshProUGUI textMeshProUGUI in this.scoring.GetComponentsInChildren<TextMeshProUGUI>())
			{
				textMeshProUGUI.fontStyle = FontStyles.Bold;
			}
			this.gradeLabel.fontStyle = FontStyles.Bold;
		}
		if (!Level.IsTowerOfPowerMain && data.difficulty == Level.Mode.Easy && Level.PreviousDifficulty == Level.Mode.Easy && Level.PreviousLevelType == Level.Type.Battle && !Level.IsDicePalace && !Level.IsDicePalaceMain && Level.PreviousLevel != Levels.Devil)
		{
			Localization.Translation translation = Localization.Translate("ResultsTryRegular");
			this.tryRegular.SetActive(true);
			if (translation.image == null && !translation.hasSpriteAtlasImage)
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
				if (Localization.language != Localization.Languages.English)
				{
					this.glowScript.BeginGlow();
				}
			}
			else
			{
				this.tryRegularText.enabled = false;
			}
		}
		this.timeTicker.TargetValue = (int)data.time;
		this.timeTicker.MaxValue = (int)data.goalTime;
		this.hitsTicker.TargetValue = ((data.numTimesHit >= 3) ? 0 : (3 - data.numTimesHit));
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
		this.done = true;
		yield break;
	}

	// Token: 0x06003DA6 RID: 15782 RVA: 0x001196F8 File Offset: 0x001178F8
	public void AlignBannerText(GameObject bannerText)
	{
		bannerText.GetComponent<TextMeshCurveAndJitter>().CurveScale = (float)TowerOfPowerScorecard.TryRegularCurveValues[(int)Localization.language];
		Vector3 localPosition = bannerText.transform.localPosition;
		localPosition.y = (float)TowerOfPowerScorecard.TryRegularCurveOffsets[(int)Localization.language];
		bannerText.transform.localPosition = localPosition;
	}

	// Token: 0x06003DA7 RID: 15783 RVA: 0x00119748 File Offset: 0x00117948
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

	// Token: 0x0400310D RID: 12557
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

	// Token: 0x0400310E RID: 12558
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

	// Token: 0x0400310F RID: 12559
	public const float BOB_FRAME_TIME = 0.0416666679f;

	// Token: 0x04003110 RID: 12560
	[Header("Delays")]
	[SerializeField]
	public float introDelay = 10f;

	// Token: 0x04003111 RID: 12561
	[SerializeField]
	public float talliesDelay = 0.5f;

	// Token: 0x04003112 RID: 12562
	[SerializeField]
	public float gradeDelay = 0.7f;

	// Token: 0x04003113 RID: 12563
	[SerializeField]
	public float advanceDelay = 10f;

	// Token: 0x04003114 RID: 12564
	[SerializeField]
	public WinScreenTicker timeTicker;

	// Token: 0x04003115 RID: 12565
	[SerializeField]
	public WinScreenTicker hitsTicker;

	// Token: 0x04003116 RID: 12566
	[SerializeField]
	public WinScreenTicker parriesTicker;

	// Token: 0x04003117 RID: 12567
	[SerializeField]
	public WinScreenTicker superMeterTicker;

	// Token: 0x04003118 RID: 12568
	[SerializeField]
	public WinScreenTicker difficultyTicker;

	// Token: 0x04003119 RID: 12569
	[SerializeField]
	public LocalizationHelper spiritStockLabelLocalizationHelper;

	// Token: 0x0400311A RID: 12570
	[SerializeField]
	public WinScreenGradeDisplay gradeDisplay;

	// Token: 0x0400311B RID: 12571
	[SerializeField]
	public GameObject continuePrompt;

	// Token: 0x0400311C RID: 12572
	[Header("UI Scoring")]
	[SerializeField]
	public GameObject scoring;

	// Token: 0x0400311D RID: 12573
	[SerializeField]
	public TextMeshProUGUI gradeLabel;

	// Token: 0x0400311E RID: 12574
	[Header("Try Text")]
	[SerializeField]
	public GameObject tryRegular;

	// Token: 0x0400311F RID: 12575
	[SerializeField]
	public TMP_Text tryRegularText;

	// Token: 0x04003120 RID: 12576
	[SerializeField]
	public SpriteRenderer tryRegularEnglishBackground;

	// Token: 0x04003121 RID: 12577
	[Header("Glow effect")]
	[SerializeField]
	public GameObject glowingText;

	// Token: 0x04003122 RID: 12578
	[SerializeField]
	public GlowText glowScript;

	// Token: 0x04003123 RID: 12579
	[SerializeField]
	public PostProcessingBehaviour postProcessingScript;

	// Token: 0x04003124 RID: 12580
	[SerializeField]
	public PostProcessingProfile asianProfile;

	// Token: 0x04003125 RID: 12581
	[SerializeField]
	public GameObject tryExpert;

	// Token: 0x04003126 RID: 12582
	[SerializeField]
	public TMP_Text tryExpertText;

	// Token: 0x04003127 RID: 12583
	[Space(10f)]
	[SerializeField]
	public SpriteRenderer[] studioMHDRSubtitles;

	// Token: 0x04003128 RID: 12584
	[SerializeField]
	public Transform[] resultsTitles;

	// Token: 0x04003129 RID: 12585
	[SerializeField]
	public Vector3 playerOneOffCenterTitleRoot;

	// Token: 0x0400312A RID: 12586
	[SerializeField]
	public Vector3 japaneseTitleRoot;

	// Token: 0x0400312B RID: 12587
	[SerializeField]
	public Vector3 koreanTitleRoot;

	// Token: 0x0400312C RID: 12588
	[SerializeField]
	public Vector3 chineseTitleRoot;

	// Token: 0x0400312D RID: 12589
	[Space(10f)]
	[SerializeField]
	public Canvas results;

	// Token: 0x0400312E RID: 12590
	[Header("BannerCurve")]
	[SerializeField]
	public MinMax textWidthRange;

	// Token: 0x0400312F RID: 12591
	[SerializeField]
	public MinMax curveScaleRange;

	// Token: 0x04003130 RID: 12592
	[SerializeField]
	public float yOffsetDelta;

	// Token: 0x04003131 RID: 12593
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04003132 RID: 12594
	public const float BG_NORMAL_SPEED = 50f;

	// Token: 0x04003133 RID: 12595
	public const float BG_FAST_SPEED = 150f;

	// Token: 0x04003134 RID: 12596
	public bool done;
}
