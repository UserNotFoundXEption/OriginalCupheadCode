using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004C9 RID: 1225
public class MapEquipUIChecklistItem : AbstractMonoBehaviour
{
	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x060032C7 RID: 12999 RVA: 0x000EFD28 File Offset: 0x000EDF28
	public float lineWidth
	{
		get
		{
			return this.descriptionText.rectTransform.sizeDelta.x;
		}
	}

	// Token: 0x060032C8 RID: 13000 RVA: 0x0002A1ED File Offset: 0x000283ED
	public override void Awake()
	{
		base.Awake();
		this.originalFontSize = this.descriptionText.fontSize;
	}

	// Token: 0x060032C9 RID: 13001 RVA: 0x000EFD50 File Offset: 0x000EDF50
	public bool EnableCheckbox(bool enabled)
	{
		if (this.checkBox != null)
		{
			this.checkBox.enabled = enabled;
			return enabled;
		}
		return false;
	}

	// Token: 0x060032CA RID: 13002 RVA: 0x000EFD80 File Offset: 0x000EDF80
	public void SetDescription(Levels selectedLevel, string levelName, bool isFinale)
	{
		PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(selectedLevel);
		Localization.Translation translation = Localization.Translate(selectedLevel.ToString());
		string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
		levelName = translation.text.Replace("\\n", newValue);
		if (levelData.played)
		{
			this.descriptionText.text = levelName;
			this.descriptionText.font = Localization.Instance.fonts[(int)Localization.language][15].fontAsset;
		}
		else
		{
			this.descriptionText.text = this.unknown;
			this.descriptionText.font = Localization.Instance.fonts[0][15].fontAsset;
		}
		if (this.isDicePalaceMiniBoss)
		{
			this.descriptionText.fontSize = ((translation.fonts.fontSize <= 0) ? this.originalFontSize : ((float)translation.fonts.fontSize));
		}
		if (!this.isDicePalaceMiniBoss)
		{
			float num = this.originalFontSize;
			while (this.lineWidth - this.descriptionText.preferredWidth < 0f && this.originalFontSize > 0f)
			{
				num -= 1f;
				this.descriptionText.fontSize = num;
			}
			this.SetLeaderDots(levelName, isFinale);
		}
		if (levelData.played)
		{
			if (!this.isDicePalaceMiniBoss && levelData.completed)
			{
				this.gradeText.text = this.grades[(int)levelData.grade];
				this.timeText.text = this.SecondsToMinutes(levelData.bestTime);
				if (levelData.difficultyBeaten == Level.Mode.Normal && this.checkBox != null && this.checkBox.enabled)
				{
					this.checkMark.enabled = true;
					this.checkMarkHard.enabled = false;
				}
				if (levelData.difficultyBeaten == Level.Mode.Hard && this.checkBox != null && this.checkBox.enabled)
				{
					this.checkMark.enabled = false;
					this.checkMarkHard.enabled = true;
				}
			}
		}
		else
		{
			this.ClearDescription(isFinale);
		}
	}

	// Token: 0x060032CB RID: 13003 RVA: 0x000EFFE0 File Offset: 0x000EE1E0
	public string SecondsToMinutes(float seconds)
	{
		if (seconds == 3.40282347E+38f)
		{
			return "6:66";
		}
		int num = (int)seconds / 60;
		int num2 = (int)seconds % 60;
		return string.Format("{0}:{1:00}", num, num2);
	}

	// Token: 0x060032CC RID: 13004 RVA: 0x000F0024 File Offset: 0x000EE224
	public void ClearDescription(bool isFinale)
	{
		if (!this.isDicePalaceMiniBoss)
		{
			this.gradeText.text = "?";
			this.timeText.text = "?";
			this.descriptionText.text = this.unknown;
			if (isFinale)
			{
				this.SetLeaderDots(this.unknown, isFinale);
			}
			else
			{
				this.SetLeaderDots(this.unknown, isFinale);
			}
		}
		else
		{
			this.descriptionText.text = this.unknown;
		}
		if (this.checkMark != null)
		{
			this.checkMark.enabled = false;
		}
		if (this.checkMarkHard != null)
		{
			this.checkMarkHard.enabled = false;
		}
	}

	// Token: 0x060032CD RID: 13005 RVA: 0x000F00E4 File Offset: 0x000EE2E4
	public void SetLeaderDots(string name, bool isFinale)
	{
		this.leaderDotText.text = this.dots;
		float num = this.lineWidth - this.descriptionText.preferredWidth - this.dotsPadding;
		if (num < 0f)
		{
			this.leaderDotText.text = string.Empty;
			return;
		}
		int num2 = 100000;
		while (this.leaderDotText.text.Length > 2 && this.leaderDotText.preferredWidth > num && num2 > 0)
		{
			num2--;
			this.leaderDotText.text = this.leaderDotText.text.Substring(0, this.leaderDotText.text.Length - 2);
		}
	}

	// Token: 0x040029A7 RID: 10663
	[Header("Text")]
	public TextMeshProUGUI descriptionText;

	// Token: 0x040029A8 RID: 10664
	public Text leaderDotText;

	// Token: 0x040029A9 RID: 10665
	public Text gradeText;

	// Token: 0x040029AA RID: 10666
	public Text timeText;

	// Token: 0x040029AB RID: 10667
	[Header("Images")]
	public Image checkBox;

	// Token: 0x040029AC RID: 10668
	public Image checkMark;

	// Token: 0x040029AD RID: 10669
	public Image checkMarkHard;

	// Token: 0x040029AE RID: 10670
	public readonly string[] grades = new string[]
	{
		"D-",
		"D",
		"D+",
		"C-",
		"C",
		"C+",
		"B-",
		"B",
		"B+",
		"A-",
		"A",
		"A+",
		"S",
		"P"
	};

	// Token: 0x040029AF RID: 10671
	public readonly string unknown = "?????";

	// Token: 0x040029B0 RID: 10672
	public readonly string dots = ". . . . . . . . . . . . . . . . . . . . . . .";

	// Token: 0x040029B1 RID: 10673
	public bool isDicePalaceMiniBoss;

	// Token: 0x040029B2 RID: 10674
	public float dotsPadding = 5f;

	// Token: 0x040029B3 RID: 10675
	public float originalFontSize;
}
