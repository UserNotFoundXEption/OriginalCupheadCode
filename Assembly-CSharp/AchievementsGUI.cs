using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x020000E7 RID: 231
public class AchievementsGUI : AbstractMonoBehaviour
{
	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x00009CAC File Offset: 0x00007EAC
	// (set) Token: 0x06000AE7 RID: 2791 RVA: 0x00009CB4 File Offset: 0x00007EB4
	public bool achievementsMenuOpen { get; set; }

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00009CBD File Offset: 0x00007EBD
	// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x00009CC5 File Offset: 0x00007EC5
	public bool inputEnabled { get; set; }

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06000AEA RID: 2794 RVA: 0x00009CCE File Offset: 0x00007ECE
	// (set) Token: 0x06000AEB RID: 2795 RVA: 0x00009CD6 File Offset: 0x00007ED6
	public bool justClosed { get; set; }

	// Token: 0x06000AEC RID: 2796 RVA: 0x0007C4E8 File Offset: 0x0007A6E8
	public override void Awake()
	{
		base.Awake();
		this.defaultAtlas = AssetLoader<SpriteAtlas>.GetCachedAsset("Achievements");
		this.stringBuilder = new StringBuilder();
		this.background.sprite = this.defaultAtlas.GetSprite("cheev_bg");
		this.unearnedBackground.sprite = this.defaultAtlas.GetSprite("cheev_card_unearned");
		this.achievementsMenuOpen = false;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00009CDF File Offset: 0x00007EDF
	public void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x0007C570 File Offset: 0x0007A770
	public void Update()
	{
		this.justClosed = false;
		this.timeSinceStart += Time.deltaTime;
		if (this.timeSinceStart < 0.25f)
		{
			return;
		}
		if (this.activeNavigationButton != CupheadButton.None && this.input.GetButtonUp(this.activeNavigationButton))
		{
			this.activeNavigationButton = CupheadButton.None;
		}
		if (!this.inputEnabled)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.Cancel))
		{
			AudioManager.Play("level_menu_select");
			this.HideAchievements();
		}
		if (this.activeNavigationButton == CupheadButton.None)
		{
			if (this.GetButtonDown(CupheadButton.MenuUp))
			{
				if (this.achievementIndex.y == 0)
				{
					this.rowOffset = this.currentGridSize.y - AchievementsGUI.VisualGridSize.y;
					this.cursorIndex.y = AchievementsGUI.VisualGridSize.y - 1;
					this.achievementIndex.y = this.currentGridSize.y - 1;
				}
				else
				{
					if (this.cursorIndex.y == 0)
					{
						this.rowOffset--;
					}
					this.cursorIndex.y = Mathf.Max(this.cursorIndex.y - 1, 0);
					this.achievementIndex.y = Mathf.Max(this.achievementIndex.y - 1, 0);
				}
				this.refreshIcons();
				this.updateSelection();
			}
			else if (this.GetButtonDown(CupheadButton.MenuDown))
			{
				if (this.achievementIndex.y == this.currentGridSize.y - 1)
				{
					this.rowOffset = 0;
					this.cursorIndex.y = 0;
					this.achievementIndex.y = 0;
				}
				else
				{
					if (this.cursorIndex.y == AchievementsGUI.VisualGridSize.y - 1)
					{
						this.rowOffset = Mathf.Min(this.rowOffset + 1, this.currentGridSize.y - AchievementsGUI.VisualGridSize.y);
					}
					this.cursorIndex.y = Mathf.Min(this.cursorIndex.y + 1, AchievementsGUI.VisualGridSize.y - 1);
					this.achievementIndex.y = Mathf.Min(this.achievementIndex.y + 1, this.currentGridSize.y - 1);
				}
				this.refreshIcons();
				this.updateSelection();
			}
			else if (this.GetButtonDown(CupheadButton.MenuLeft))
			{
				this.cursorIndex.x = this.cursorIndex.x - 1;
				if (this.cursorIndex.x < 0)
				{
					this.cursorIndex.x = AchievementsGUI.VisualGridSize.x - 1;
				}
				this.achievementIndex.x = this.achievementIndex.x - 1;
				if (this.achievementIndex.x < 0)
				{
					this.achievementIndex.x = this.currentGridSize.x - 1;
				}
				this.updateSelection();
			}
			else if (this.GetButtonDown(CupheadButton.MenuRight))
			{
				this.cursorIndex.x = this.cursorIndex.x + 1;
				if (this.cursorIndex.x >= AchievementsGUI.VisualGridSize.x)
				{
					this.cursorIndex.x = 0;
				}
				this.achievementIndex.x = this.achievementIndex.x + 1;
				if (this.achievementIndex.x >= this.currentGridSize.x)
				{
					this.achievementIndex.x = 0;
				}
				this.updateSelection();
			}
		}
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x0007C910 File Offset: 0x0007AB10
	public void ShowAchievements()
	{
		this.handleDLCStatus();
		this.cursorIndex = Vector2Int.zero;
		this.achievementIndex = Vector2Int.zero;
		this.rowOffset = 0;
		this.refreshIcons();
		this.updateSelection();
		if (this.dlcEnabled)
		{
			this.arrowCoroutine = base.StartCoroutine(this.arrow_cr());
		}
		this.timeSinceStart = 0f;
		this.achievementsMenuOpen = true;
		this.canvasGroup.alpha = 1f;
		base.FrameDelayedCallback(new Action(this.interactable), 1);
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x0007C9A0 File Offset: 0x0007ABA0
	public void HideAchievements()
	{
		if (this.dlcEnabled)
		{
			base.StopCoroutine(this.arrowCoroutine);
			this.arrowCoroutine = null;
		}
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		this.inputEnabled = false;
		this.achievementsMenuOpen = false;
		this.justClosed = true;
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x00009CED File Offset: 0x00007EED
	public void interactable()
	{
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		this.inputEnabled = true;
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x0007CA08 File Offset: 0x0007AC08
	public void updateSelection()
	{
		AchievementIcon achievementIcon = this.iconRows[this.cursorIndex.y].achievementIcons[this.cursorIndex.x];
		this.cursor.position = achievementIcon.transform.position;
		int num = this.achievementIndex.y * this.currentGridSize.x + this.achievementIndex.x;
		LocalAchievementsManager.Achievement achievement = (LocalAchievementsManager.Achievement)num;
		string text = achievement.ToString();
		bool flag = LocalAchievementsManager.IsAchievementUnlocked(achievement);
		bool flag2 = LocalAchievementsManager.IsHiddenAchievement(achievement);
		if (flag || !flag2)
		{
			string key = "Achievement" + text + "Title";
			this.titleLocalization.ApplyTranslation(Localization.Find(key), null);
			string key2 = "Achievement" + text + "Desc";
			this.descriptionLocalization.ApplyTranslation(Localization.Find(key2), null);
		}
		else
		{
			this.titleText.text = AchievementsGUI.TitleHidden;
			this.titleText.font = FontLoader.GetFont(AchievementsGUI.TitleHiddenFont);
			this.descriptionText.text = AchievementsGUI.DescriptionHidden;
			this.descriptionText.font = FontLoader.GetFont(AchievementsGUI.DescriptionHiddenFont);
		}
		string text2 = text;
		if (flag)
		{
			text2 += "_earned";
		}
		Sprite achievementSprite = this.getAchievementSprite(text2, achievement);
		this.largeIcon.sprite = achievementSprite;
		this.titleText.color = ((!flag) ? AchievementsGUI.LockedTextColor : AchievementsGUI.UnlockedTextColor);
		this.descriptionText.color = ((!flag) ? AchievementsGUI.LockedTextColor : AchievementsGUI.UnlockedTextColor);
		this.unearnedBackground.enabled = !flag;
		this.noise.sprite = this.getSprite((!flag) ? "cheev_card_noise_unearned" : "cheev_card_noise_earned", this.defaultAtlas);
		AudioManager.Play("level_menu_move");
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x00009D0E File Offset: 0x00007F0E
	public bool GetButtonDown(CupheadButton button)
	{
		if (this.input.GetButtonDown(button))
		{
			this.activeNavigationButton = button;
			return true;
		}
		return false;
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x0007CBF4 File Offset: 0x0007ADF4
	public void handleDLCStatus()
	{
		this.dlcEnabled = DLCManager.DLCEnabled();
		this.currentGridSize = ((!this.dlcEnabled) ? AchievementsGUI.VisualGridSize : AchievementsGUI.GridSize);
		if (this.dlcEnabled && this.dlcAtlas == null)
		{
			this.dlcAtlas = AssetLoader<SpriteAtlas>.GetCachedAsset("Achievements_DLC");
		}
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x0007CC58 File Offset: 0x0007AE58
	public IEnumerator arrow_cr()
	{
		int index = 0;
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.0833333358f, true);
		for (;;)
		{
			this.topArrow.sprite = this.arrowSprites[index];
			index = MathUtilities.NextIndex(index, this.arrowSprites.Length);
			this.bottomArrow.sprite = this.arrowSprites[MathUtilities.NextIndex(index, this.arrowSprites.Length)];
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x0007CC74 File Offset: 0x0007AE74
	public void refreshIcons()
	{
		if (this.dlcEnabled)
		{
			this.topArrow.enabled = (this.rowOffset != 0);
			this.bottomArrow.enabled = (this.rowOffset != 2);
		}
		else
		{
			Behaviour behaviour = this.topArrow;
			bool enabled = false;
			this.bottomArrow.enabled = enabled;
			behaviour.enabled = enabled;
		}
		int num = this.rowOffset * this.currentGridSize.x;
		foreach (AchievementsGUI.IconRow iconRow in this.iconRows)
		{
			foreach (AchievementIcon achievementIcon in iconRow.achievementIcons)
			{
				this.stringBuilder.Length = 0;
				LocalAchievementsManager.Achievement achievement = (LocalAchievementsManager.Achievement)num;
				this.stringBuilder.Append(AchievementsGUI.AchievementNames[num]);
				if (LocalAchievementsManager.IsAchievementUnlocked(achievement))
				{
					this.stringBuilder.Append("_earned");
				}
				this.stringBuilder.Append("_sm");
				Sprite achievementSprite = this.getAchievementSprite(this.stringBuilder.ToString(), achievement);
				achievementIcon.SetIcon(achievementSprite);
				num++;
			}
		}
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x0007CDAC File Offset: 0x0007AFAC
	public Sprite getSprite(string spriteName, SpriteAtlas atlas)
	{
		Sprite sprite;
		if (!this.spriteCache.TryGetValue(spriteName, out sprite))
		{
			sprite = atlas.GetSprite(spriteName);
			this.spriteCache.Add(spriteName, sprite);
		}
		return sprite;
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0007CDE4 File Offset: 0x0007AFE4
	public Sprite getAchievementSprite(string spriteName, LocalAchievementsManager.Achievement achievement)
	{
		SpriteAtlas atlas;
		if (Array.IndexOf<LocalAchievementsManager.Achievement>(LocalAchievementsManager.DLCAchievements, achievement) >= 0)
		{
			atlas = this.dlcAtlas;
		}
		else
		{
			atlas = this.defaultAtlas;
		}
		return this.getSprite(spriteName, atlas);
	}

	// Token: 0x04000867 RID: 2151
	public static readonly string[] AchievementNames = Enum.GetNames(typeof(LocalAchievementsManager.Achievement));

	// Token: 0x04000868 RID: 2152
	public static readonly Color UnlockedTextColor = new Color(0.9098039f, 0.8235294f, 0.68235296f);

	// Token: 0x04000869 RID: 2153
	public static readonly Color LockedTextColor = new Color(0.270588249f, 0.266666681f, 0.2627451f);

	// Token: 0x0400086A RID: 2154
	public static readonly string TitleHidden = "? ? ? ? ? ? ?";

	// Token: 0x0400086B RID: 2155
	public static readonly FontLoader.FontType TitleHiddenFont = FontLoader.FontType.CupheadMemphis_Medium_merged;

	// Token: 0x0400086C RID: 2156
	public static readonly string DescriptionHidden = "?  ?  ?  ?  ?  ?";

	// Token: 0x0400086D RID: 2157
	public static readonly FontLoader.FontType DescriptionHiddenFont = FontLoader.FontType.CupheadVogue_Bold_merged;

	// Token: 0x0400086E RID: 2158
	public static readonly Vector2Int GridSize = new Vector2Int(7, 6);

	// Token: 0x0400086F RID: 2159
	public static readonly Vector2Int VisualGridSize = new Vector2Int(7, 4);

	// Token: 0x04000870 RID: 2160
	[SerializeField]
	public AchievementsGUI.IconRow[] iconRows;

	// Token: 0x04000871 RID: 2161
	[SerializeField]
	public RectTransform cursor;

	// Token: 0x04000872 RID: 2162
	[SerializeField]
	public Image topArrow;

	// Token: 0x04000873 RID: 2163
	[SerializeField]
	public Image bottomArrow;

	// Token: 0x04000874 RID: 2164
	[SerializeField]
	public Image background;

	// Token: 0x04000875 RID: 2165
	[SerializeField]
	public Image unearnedBackground;

	// Token: 0x04000876 RID: 2166
	[SerializeField]
	public Text titleText;

	// Token: 0x04000877 RID: 2167
	[SerializeField]
	public Text descriptionText;

	// Token: 0x04000878 RID: 2168
	[SerializeField]
	public LocalizationHelper titleLocalization;

	// Token: 0x04000879 RID: 2169
	[SerializeField]
	public LocalizationHelper descriptionLocalization;

	// Token: 0x0400087A RID: 2170
	[SerializeField]
	public Image largeIcon;

	// Token: 0x0400087B RID: 2171
	[SerializeField]
	public Image noise;

	// Token: 0x0400087C RID: 2172
	[SerializeField]
	public Sprite[] arrowSprites;

	// Token: 0x0400087D RID: 2173
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x0400087E RID: 2174
	public float timeSinceStart;

	// Token: 0x0400087F RID: 2175
	public Vector2Int achievementIndex;

	// Token: 0x04000880 RID: 2176
	public Vector2Int cursorIndex;

	// Token: 0x04000881 RID: 2177
	public int rowOffset;

	// Token: 0x04000882 RID: 2178
	public SpriteAtlas defaultAtlas;

	// Token: 0x04000883 RID: 2179
	public SpriteAtlas dlcAtlas;

	// Token: 0x04000884 RID: 2180
	public Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

	// Token: 0x04000885 RID: 2181
	public CupheadButton activeNavigationButton = CupheadButton.None;

	// Token: 0x04000886 RID: 2182
	public StringBuilder stringBuilder;

	// Token: 0x04000887 RID: 2183
	public Coroutine arrowCoroutine;

	// Token: 0x04000888 RID: 2184
	public bool dlcEnabled;

	// Token: 0x04000889 RID: 2185
	public Vector2Int currentGridSize;

	// Token: 0x0400088A RID: 2186
	public CanvasGroup canvasGroup;

	// Token: 0x0200095F RID: 2399
	[Serializable]
	public class IconRow
	{
		// Token: 0x04004653 RID: 18003
		public AchievementIcon[] achievementIcons;
	}
}
