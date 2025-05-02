using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005C2 RID: 1474
public class WinScreenGradeDisplay : AbstractMonoBehaviour
{
	// Token: 0x1700050B RID: 1291
	// (get) Token: 0x06003DB6 RID: 15798 RVA: 0x00031B90 File Offset: 0x0002FD90
	// (set) Token: 0x06003DB7 RID: 15799 RVA: 0x00031B98 File Offset: 0x0002FD98
	public LevelScoringData.Grade Grade { get; set; }

	// Token: 0x1700050C RID: 1292
	// (get) Token: 0x06003DB8 RID: 15800 RVA: 0x00031BA1 File Offset: 0x0002FDA1
	// (set) Token: 0x06003DB9 RID: 15801 RVA: 0x00031BA9 File Offset: 0x0002FDA9
	public Level.Mode Difficulty { get; set; }

	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x06003DBA RID: 15802 RVA: 0x00031BB2 File Offset: 0x0002FDB2
	// (set) Token: 0x06003DBB RID: 15803 RVA: 0x00031BBA File Offset: 0x0002FDBA
	public bool Celebration { get; set; }

	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x06003DBC RID: 15804 RVA: 0x00031BC3 File Offset: 0x0002FDC3
	// (set) Token: 0x06003DBD RID: 15805 RVA: 0x00031BCB File Offset: 0x0002FDCB
	public bool FinishedGrading { get; set; }

	// Token: 0x06003DBE RID: 15806 RVA: 0x00031BD4 File Offset: 0x0002FDD4
	public override void Awake()
	{
		base.Awake();
		this.input = new CupheadInput.AnyPlayerInput(false);
	}

	// Token: 0x06003DBF RID: 15807 RVA: 0x00119FCC File Offset: 0x001181CC
	public void Start()
	{
		this.Celebration = false;
		if (Level.PreviouslyWon)
		{
			this.topGradeLabel.fontStyle = ((Localization.language != Localization.Languages.Korean) ? this.topGradeLabel.fontStyle : FontStyles.Bold);
			this.topGradeValue.text = " " + this.grades[(int)Level.PreviousGrade];
		}
	}

	// Token: 0x06003DC0 RID: 15808 RVA: 0x00031BE8 File Offset: 0x0002FDE8
	public void Show()
	{
		base.StartCoroutine(this.grade_tally_up_cr());
	}

	// Token: 0x06003DC1 RID: 15809 RVA: 0x0011A034 File Offset: 0x00118234
	public IEnumerator grade_tally_up_cr()
	{
		bool isTallying = true;
		float t = 0f;
		int counter = 0;
		this.text.text = this.grades[this.grades.Length - 1].Substring(0, 1) + " ";
		while (counter <= (int)this.Grade && isTallying)
		{
			if (counter >= (int)this.Grade)
			{
				break;
			}
			AudioManager.Play("win_score_tick");
			counter++;
			this.text.text = this.grades[counter].Substring(0, 1) + " ";
			while (t < 0.02f)
			{
				if (this.input.GetButtonDown(CupheadButton.Accept))
				{
					isTallying = false;
					break;
				}
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
		}
		AudioManager.Play("win_grade_chalk");
		this.circle.SetTrigger("Circle");
		this.text.GetComponent<Animator>().SetTrigger("MakeBig");
		this.text.text = this.grades[(int)this.Grade];
		if (counter == this.grades.Length - 1)
		{
			this.text.color = ColorUtils.HexToColor("FCC93D");
		}
		LevelScoringData.Grade PerfectGrade = (this.Difficulty != Level.Mode.Hard) ? LevelScoringData.Grade.APlus : LevelScoringData.Grade.S;
		bool english = Localization.language == Localization.Languages.English;
		if (!english)
		{
			this.AlignBannerText();
		}
		if (!Level.IsTowerOfPower)
		{
			if (this.Grade == PerfectGrade)
			{
				base.StartCoroutine(this.fade_text_cr());
				yield return CupheadTime.WaitForSeconds(this, 0.16f);
				this.gollyBanner.SetTrigger("OnBanner");
				this.Celebration = true;
				this.LanguageUpdate(english);
				this.gollyBannerEnglish.enabled = english;
				this.gollyBannerOther.enabled = !english;
				yield return this.gollyBanner.WaitForAnimationToEnd(this, "Golly", false, true);
			}
			else if (this.Grade > Level.PreviousGrade || !Level.PreviouslyWon)
			{
				base.StartCoroutine(this.fade_text_cr());
				yield return CupheadTime.WaitForSeconds(this, 0.16f);
				this.recordBanner.SetTrigger("OnBanner");
				this.Celebration = true;
				this.LanguageUpdate(english);
				this.recordBannerEnglish.enabled = english;
				this.recordBannerOther.enabled = !english;
				yield return this.recordBanner.WaitForAnimationToEnd(this, "Record", false, true);
			}
		}
		if (Level.IsTowerOfPower && this.Grade >= (LevelScoringData.Grade)TowerOfPowerLevelGameInfo.MIN_RANK_NEED_TO_GET_TOKEN)
		{
			TowerOfPowerLevelGameInfo.AddToken();
		}
		this.FinishedGrading = true;
		yield return null;
		yield break;
	}

	// Token: 0x06003DC2 RID: 15810 RVA: 0x0011A050 File Offset: 0x00118250
	public void AlignBannerText()
	{
		for (int i = 0; i < this.normalBannerTexts.Length; i++)
		{
			this.normalBannerTexts[i].GetComponent<TextMeshCurveAndJitter>().CurveScale = (float)WinScreenGradeDisplay.NormalCurveValues[(int)Localization.language];
			Vector3 localPosition = this.normalBannerTexts[i].transform.localPosition;
			localPosition.y = (float)(-(float)WinScreenGradeDisplay.NormalCurveOffsets[(int)Localization.language]);
			if (i == this.normalBannerTexts.Length - 1)
			{
				localPosition.y += 2f;
			}
			this.normalBannerTexts[i].transform.localPosition = localPosition;
		}
		for (int j = 0; j < this.topScoreBannerTexts.Length; j++)
		{
			this.topScoreBannerTexts[j].GetComponent<TextMeshCurveAndJitter>().CurveScale = (float)WinScreenGradeDisplay.GollyCurveValues[(int)Localization.language];
			Vector3 localPosition2 = this.topScoreBannerTexts[j].transform.localPosition;
			localPosition2.y = (float)(-(float)WinScreenGradeDisplay.GollyCurveOffsets[(int)Localization.language]);
			if (j == this.topScoreBannerTexts.Length - 1)
			{
				localPosition2.y -= 2f;
			}
			this.topScoreBannerTexts[j].transform.localPosition = localPosition2;
		}
	}

	// Token: 0x06003DC3 RID: 15811 RVA: 0x0011A188 File Offset: 0x00118388
	public void LanguageUpdate(bool english)
	{
		for (int i = 0; i < this.recordEnglish.Length; i++)
		{
			this.recordEnglish[i].SetActive(english);
		}
		for (int j = 0; j < this.gollyEnglish.Length; j++)
		{
			this.gollyEnglish[j].SetActive(english);
		}
		for (int k = 0; k < this.recordOther.Length; k++)
		{
			this.recordOther[k].SetActive(!english);
		}
		for (int l = 0; l < this.gollyOther.Length; l++)
		{
			this.gollyOther[l].SetActive(!english);
		}
	}

	// Token: 0x06003DC4 RID: 15812 RVA: 0x0011A238 File Offset: 0x00118438
	public IEnumerator fade_text_cr()
	{
		float t = 0f;
		float fadeTime = 0.29f;
		Color topGradeLabelColor = this.topGradeLabel.color;
		Color topGradeValColor = this.topGradeValue.color;
		while (t < fadeTime)
		{
			t += CupheadTime.Delta;
			this.topGradeLabel.color = new Color(topGradeLabelColor.r, topGradeLabelColor.g, topGradeLabelColor.b, 1f - t / fadeTime);
			this.topGradeValue.color = new Color(topGradeValColor.r, topGradeValColor.g, topGradeValColor.b, 1f - t / fadeTime);
			if (this.tryExpert.gameObject.activeSelf)
			{
				foreach (SpriteRenderer spriteRenderer in this.tryExpert.GetComponentsInChildren<SpriteRenderer>())
				{
					spriteRenderer.color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
				}
				foreach (RawImage rawImage in this.tryExpert.GetComponentsInChildren<RawImage>())
				{
					rawImage.color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
				}
				foreach (TextMeshCurveAndJitter textMeshCurveAndJitter in this.tryExpert.GetComponentsInChildren<TextMeshCurveAndJitter>())
				{
					float value = Mathf.Clamp(255f - t / fadeTime * 255f, 0f, 255f);
					textMeshCurveAndJitter.AlphaValue = Convert.ToByte(value);
				}
			}
			if (this.tryRegular.gameObject.activeSelf)
			{
				foreach (SpriteRenderer spriteRenderer2 in this.tryRegular.GetComponentsInChildren<SpriteRenderer>())
				{
					spriteRenderer2.color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
				}
				foreach (RawImage rawImage2 in this.tryRegular.GetComponentsInChildren<RawImage>())
				{
					rawImage2.color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
				}
				foreach (TextMeshCurveAndJitter textMeshCurveAndJitter2 in this.tryRegular.GetComponentsInChildren<TextMeshCurveAndJitter>())
				{
					float value2 = Mathf.Clamp(255f - t / fadeTime * 255f, 0f, 255f);
					textMeshCurveAndJitter2.AlphaValue = Convert.ToByte(value2);
				}
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x04003187 RID: 12679
	public static readonly int[] NormalCurveValues = new int[]
	{
		0,
		38,
		28,
		65,
		25,
		36,
		8,
		28,
		26,
		40,
		5,
		5
	};

	// Token: 0x04003188 RID: 12680
	public static readonly int[] NormalCurveOffsets = new int[]
	{
		0,
		21,
		16,
		37,
		17,
		23,
		6,
		17,
		15,
		22,
		6,
		6
	};

	// Token: 0x04003189 RID: 12681
	public static readonly int[] GollyCurveValues = new int[]
	{
		0,
		53,
		47,
		51,
		54,
		54,
		20,
		51,
		49,
		49,
		51,
		26
	};

	// Token: 0x0400318A RID: 12682
	public static readonly int[] GollyCurveOffsets = new int[]
	{
		0,
		30,
		28,
		31,
		31,
		31,
		16,
		29,
		29,
		29,
		29,
		16
	};

	// Token: 0x0400318B RID: 12683
	[SerializeField]
	public Text text;

	// Token: 0x0400318C RID: 12684
	[SerializeField]
	public TextMeshProUGUI topGradeLabel;

	// Token: 0x0400318D RID: 12685
	[SerializeField]
	public Text topGradeValue;

	// Token: 0x0400318E RID: 12686
	[SerializeField]
	public string[] grades;

	// Token: 0x0400318F RID: 12687
	[SerializeField]
	public Animator circle;

	// Token: 0x04003190 RID: 12688
	[SerializeField]
	public Animator recordBanner;

	// Token: 0x04003191 RID: 12689
	[SerializeField]
	public GameObject[] recordEnglish;

	// Token: 0x04003192 RID: 12690
	[SerializeField]
	public GameObject[] recordOther;

	// Token: 0x04003193 RID: 12691
	[SerializeField]
	public Image recordBannerEnglish;

	// Token: 0x04003194 RID: 12692
	[SerializeField]
	public Image recordBannerOther;

	// Token: 0x04003195 RID: 12693
	[SerializeField]
	public Animator gollyBanner;

	// Token: 0x04003196 RID: 12694
	[SerializeField]
	public GameObject[] gollyEnglish;

	// Token: 0x04003197 RID: 12695
	[SerializeField]
	public GameObject[] gollyOther;

	// Token: 0x04003198 RID: 12696
	[SerializeField]
	public Image gollyBannerEnglish;

	// Token: 0x04003199 RID: 12697
	[SerializeField]
	public Image gollyBannerOther;

	// Token: 0x0400319A RID: 12698
	[SerializeField]
	public SpriteRenderer tryRegular;

	// Token: 0x0400319B RID: 12699
	[SerializeField]
	public SpriteRenderer tryExpert;

	// Token: 0x0400319C RID: 12700
	[SerializeField]
	public GameObject[] normalBannerTexts;

	// Token: 0x0400319D RID: 12701
	[SerializeField]
	public GameObject[] topScoreBannerTexts;

	// Token: 0x040031A0 RID: 12704
	public const float COUNTER_TIME = 0.02f;

	// Token: 0x040031A1 RID: 12705
	public const float BANNER_FLASH_Y_OFFSET = 2f;

	// Token: 0x040031A4 RID: 12708
	public CupheadInput.AnyPlayerInput input;
}
