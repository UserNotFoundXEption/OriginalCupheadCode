using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004CE RID: 1230
public class MapDifficultySelectStartUI : AbstractMapSceneStartUI
{
	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x060032EC RID: 13036 RVA: 0x0002A443 File Offset: 0x00028643
	// (set) Token: 0x060032ED RID: 13037 RVA: 0x0002A44A File Offset: 0x0002864A
	public static MapDifficultySelectStartUI Current { get; set; }

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x060032EE RID: 13038 RVA: 0x0002A452 File Offset: 0x00028652
	// (set) Token: 0x060032EF RID: 13039 RVA: 0x0002A459 File Offset: 0x00028659
	public static Level.Mode Mode { get; set; }

	// Token: 0x060032F0 RID: 13040 RVA: 0x000F0514 File Offset: 0x000EE714
	public override void Awake()
	{
		base.Awake();
		MapDifficultySelectStartUI.Current = this;
		switch (Level.CurrentMode)
		{
		case Level.Mode.Easy:
			this.index = 0;
			break;
		case Level.Mode.Normal:
			this.index = 1;
			break;
		case Level.Mode.Hard:
			this.index = 2;
			break;
		}
		this.options = new Level.Mode[]
		{
			Level.Mode.Easy,
			Level.Mode.Normal,
			Level.Mode.Hard
		};
		this.SetDifficultyAvailability();
		this.difficulyTexts = new TMP_Text[3];
		this.difficulyTexts[0] = this.easy.GetComponent<TMP_Text>();
		this.difficulyTexts[1] = this.normal.GetComponent<TMP_Text>();
		this.difficulyTexts[2] = this.hard.GetComponent<TMP_Text>();
		if (this.bossImage != null && this.bossImage.textComponent != null)
		{
			this.initialMaxFontSize = this.bossImage.textComponent.resizeTextMaxSize;
		}
		this.initialinImagePosX = this.inAnimated.rectTransform.offsetMin;
		this.initialinImagePosY = this.inAnimated.rectTransform.offsetMax;
		this.initialinDifficultyPos = this.difficultyImage.rectTransform.anchoredPosition;
		this.initialDifficultyPos = this.difficultySelectionText.rectTransform.anchoredPosition;
		this.initialBossNamePos = this.bossNameImage.rectTransform.anchoredPosition;
	}

	// Token: 0x060032F1 RID: 13041 RVA: 0x000F0680 File Offset: 0x000EE880
	public void SetDifficultyAvailability()
	{
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_4)
		{
			if (!PlayerData.Data.IsHardModeAvailable)
			{
				this.options = new Level.Mode[]
				{
					Level.Mode.Normal
				};
				this.hard.gameObject.SetActive(false);
				this.hardSeparator.gameObject.SetActive(false);
			}
			else
			{
				this.options = new Level.Mode[]
				{
					Level.Mode.Normal,
					Level.Mode.Hard
				};
			}
			this.index = Mathf.Max(0, this.index - 1);
			this.easy.gameObject.SetActive(false);
			this.normalSeparator.gameObject.SetActive(false);
		}
		else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
		{
			if (this.level == "Saltbaker")
			{
				if (!PlayerData.Data.IsHardModeAvailableDLC)
				{
					this.options = new Level.Mode[]
					{
						Level.Mode.Normal
					};
				}
				else
				{
					this.options = new Level.Mode[]
					{
						Level.Mode.Normal,
						Level.Mode.Hard
					};
				}
			}
			else if (!PlayerData.Data.IsHardModeAvailableDLC)
			{
				this.options = new Level.Mode[]
				{
					Level.Mode.Easy,
					Level.Mode.Normal
				};
			}
			else
			{
				this.options = new Level.Mode[]
				{
					Level.Mode.Easy,
					Level.Mode.Normal,
					Level.Mode.Hard
				};
			}
			this.easy.gameObject.SetActive(this.level != "Saltbaker");
			this.normalSeparator.gameObject.SetActive(this.level != "Saltbaker");
			this.hard.gameObject.SetActive(PlayerData.Data.IsHardModeAvailableDLC);
			this.hardSeparator.gameObject.SetActive(PlayerData.Data.IsHardModeAvailableDLC);
		}
		else
		{
			if (!PlayerData.Data.IsHardModeAvailable)
			{
				this.options = new Level.Mode[]
				{
					Level.Mode.Easy,
					Level.Mode.Normal
				};
			}
			this.hard.gameObject.SetActive(PlayerData.Data.IsHardModeAvailable);
			this.hardSeparator.gameObject.SetActive(PlayerData.Data.IsHardModeAvailable);
		}
	}

	// Token: 0x060032F2 RID: 13042 RVA: 0x000F089C File Offset: 0x000EEA9C
	public new void In(MapPlayerController playerController)
	{
		base.In(playerController);
		if (Level.CurrentMode == Level.Mode.Easy && PlayerData.Data.CurrentMap == Scenes.scene_map_world_4)
		{
			Level.SetCurrentMode(Level.Mode.Normal);
			switch (Level.CurrentMode)
			{
			case Level.Mode.Easy:
				this.index = 0;
				break;
			case Level.Mode.Normal:
				this.index = 1;
				break;
			case Level.Mode.Hard:
				this.index = 2;
				break;
			}
		}
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
		{
			this.SetDifficultyAvailability();
			if (this.level == "Saltbaker" && Level.CurrentMode == Level.Mode.Easy)
			{
				Level.SetCurrentMode(Level.Mode.Normal);
			}
		}
		if (base.animator != null)
		{
			base.animator.SetTrigger("ZoomIn");
			AudioManager.Play("world_map_level_menu_open");
		}
		this.InWordSetup();
		this.difficultyImage.enabled = (Localization.language == Localization.Languages.Japanese);
		this.difficultyImage.rectTransform.anchoredPosition = this.initialinDifficultyPos;
		for (int i = 0; i < this.separatorsAnimated.Length; i++)
		{
			this.separatorsAnimated[i].sprite = this.separatorsSprites[Random.Range(0, this.separatorsSprites.Length)];
		}
		bool flag = Localization.language == Localization.Languages.Korean || Localization.language == Localization.Languages.SimplifiedChinese || Localization.language == Localization.Languages.Japanese;
		this.bossTitleImage.enabled = (Localization.language == Localization.Languages.English || flag || PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC);
		this.glowScript.StopGlow();
		this.glowScript.DisableTMPText();
		this.glowScript.DisableImages();
		if (Localization.language == Localization.Languages.SimplifiedChinese)
		{
			this.difficultySelectionText.rectTransform.anchoredPosition = new Vector2(this.difficultySelectionText.rectTransform.anchoredPosition.x, -70f);
		}
		else
		{
			this.difficultySelectionText.rectTransform.anchoredPosition = this.initialDifficultyPos;
		}
		TranslationElement translationElement = Localization.Find(this.level + "Selection");
		if (this.bossImage != null && translationElement != null)
		{
			this.bossImage.ApplyTranslation(translationElement, null);
			if (this.bossImage.textComponent != null)
			{
				if (Localization.language == Localization.Languages.Korean)
				{
					this.bossImage.textComponent.resizeTextMaxSize = 100;
				}
				else
				{
					this.bossImage.textComponent.resizeTextMaxSize = this.initialMaxFontSize;
				}
			}
			if (flag)
			{
				this.SetupAsianBossCard(translationElement, this.bossTitleImage);
			}
			else
			{
				this.bossImage.transform.localScale = Vector3.one;
				this.bossImage.transform.localPosition = Vector3.zero;
				this.bossTitleImage.rectTransform.offsetMax = new Vector2(this.bossTitleImage.rectTransform.offsetMax.x, 0.5f);
				this.bossTitleImage.rectTransform.offsetMin = new Vector2(this.bossTitleImage.rectTransform.offsetMin.x, 0.5f);
				this.inAnimated.rectTransform.offsetMin = this.initialinImagePosX;
				this.inAnimated.rectTransform.offsetMax = this.initialinImagePosY;
				this.inText.fontStyle = 2;
			}
		}
		TranslationElement translationElement2 = Localization.Find(this.level + "WorldMap");
		if (translationElement2 != null)
		{
			this.bossName.ApplyTranslation(translationElement2, null);
			if (this.bossName.textComponent != null && this.bossName.textComponent.enabled)
			{
				this.bossName.textComponent.font = FontLoader.GetFont(FontLoader.FontType.CupheadHenriette_A_merged);
			}
			this.bossNameImage.transform.localScale = Vector3.one;
			this.bossNameImage.rectTransform.anchoredPosition = this.initialBossNamePos;
			if (flag)
			{
				this.bossNameImage.material = this.bossCardWhiteMaterial;
				if (Localization.language == Localization.Languages.Korean || Localization.language == Localization.Languages.Japanese)
				{
					this.bossNameImage.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
					if (Localization.language == Localization.Languages.Japanese)
					{
						this.bossNameImage.rectTransform.anchoredPosition = new Vector2(0f, 214.2f);
					}
				}
			}
			this.bossName.gameObject.SetActive(Localization.language != Localization.Languages.English && !flag && PlayerData.Data.CurrentMap != Scenes.scene_map_world_DLC);
		}
		TranslationElement translationElement3 = Localization.Find(this.level + "Glow");
		if (Localization.language != Localization.Languages.English)
		{
			if (translationElement3 != null && flag)
			{
				this.bossGlow.ApplyTranslation(translationElement3, null);
			}
			else
			{
				this.glowScript.InitTMPText(new MaskableGraphic[]
				{
					this.bossImage.textMeshProComponent,
					this.bossName.textComponent
				});
				this.glowScript.BeginGlow();
			}
		}
		this.bossGlow.gameObject.SetActive(flag && PlayerData.Data.CurrentMap != Scenes.scene_map_world_DLC);
		for (int j = 0; j < this.difficulyTexts.Length; j++)
		{
			this.difficulyTexts[j].color = this.unselectedColor;
		}
		this.difficulyTexts[(int)Level.CurrentMode].color = this.selectedColor;
	}

	// Token: 0x060032F3 RID: 13043 RVA: 0x0002A461 File Offset: 0x00028661
	public new void OnDestroy()
	{
		this.bossNameImage.sprite = null;
		this.bossTitleImage.sprite = null;
		this.asianGlow.sprite = null;
		if (MapDifficultySelectStartUI.Current == this)
		{
			MapDifficultySelectStartUI.Current = null;
		}
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x0002A49D File Offset: 0x0002869D
	public void Update()
	{
		this.UpdateCursor();
		if (base.CurrentState == AbstractMapSceneStartUI.State.Active)
		{
			this.CheckInput();
		}
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x000F0E54 File Offset: 0x000EF054
	public void CheckInput()
	{
		if (!base.Able)
		{
			return;
		}
		if (base.GetButtonDown(CupheadButton.MenuLeft))
		{
			this.Next(-1);
		}
		if (base.GetButtonDown(CupheadButton.MenuRight))
		{
			this.Next(1);
		}
		if (base.GetButtonDown(CupheadButton.Cancel))
		{
			base.Out();
		}
		if (base.GetButtonDown(CupheadButton.Accept))
		{
			base.LoadLevel();
		}
	}

	// Token: 0x060032F6 RID: 13046 RVA: 0x000F0EBC File Offset: 0x000EF0BC
	public void Next(int direction)
	{
		if ((this.index != this.options.Length - 1 && direction != -1) || (this.index != 0 && direction != 1))
		{
			AudioManager.Play("world_map_level_difficulty_hover");
		}
		this.index = Mathf.Clamp(this.index + direction, 0, this.options.Length - 1);
		Level.SetCurrentMode(this.options[this.index]);
		this.UpdateCursor();
		for (int i = 0; i < this.difficulyTexts.Length; i++)
		{
			this.difficulyTexts[i].color = this.unselectedColor;
		}
		this.difficulyTexts[(int)Level.CurrentMode].color = this.selectedColor;
	}

	// Token: 0x060032F7 RID: 13047 RVA: 0x000F0F7C File Offset: 0x000EF17C
	public void UpdateCursor()
	{
		Vector3 position = this.cursor.transform.position;
		position.y = this.normal.position.y;
		Level.Mode mode = Level.CurrentMode;
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_4 && mode == Level.Mode.Easy)
		{
			mode = Level.Mode.Normal;
		}
		switch (mode)
		{
		case Level.Mode.Easy:
			position.x = this.easy.position.x;
			this.cursor.sizeDelta = new Vector2(this.easy.sizeDelta.x + 30f, this.easy.sizeDelta.y + 20f);
			break;
		case Level.Mode.Normal:
			position.x = this.normal.position.x;
			this.cursor.sizeDelta = new Vector2(this.normal.sizeDelta.x + 30f, this.normal.sizeDelta.y + 20f);
			break;
		case Level.Mode.Hard:
			position.x = this.hard.position.x;
			this.cursor.sizeDelta = new Vector2(this.hard.sizeDelta.x + 30f, this.hard.sizeDelta.y + 20f);
			break;
		}
		this.cursor.transform.position = position;
	}

	// Token: 0x060032F8 RID: 13048 RVA: 0x000F112C File Offset: 0x000EF32C
	public void SetupAsianBossCard(TranslationElement translation, Image image)
	{
		image.material = this.bossCardWhiteMaterial;
		image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, 0f);
		image.rectTransform.offsetMin = new Vector2(image.rectTransform.offsetMin.x, 0f);
		this.SetupAsianDifficulty();
		image.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
		if (Localization.language == Localization.Languages.Korean)
		{
			if (PlayerData.Data.CurrentMap != Scenes.scene_map_world_DLC)
			{
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, 40f);
				image.rectTransform.offsetMin = new Vector2(image.rectTransform.offsetMin.x, 40f);
			}
			this.SetupKoreanInWord();
		}
		else if (Localization.language == Localization.Languages.SimplifiedChinese)
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -140f);
			if (this.level.Equals("FlyingBlimp"))
			{
				image.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, 40f);
				image.rectTransform.offsetMin = new Vector2(image.rectTransform.offsetMin.x, 40f);
				this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -100f);
			}
		}
		else if (Localization.language == Localization.Languages.Japanese)
		{
			if (this.level.Equals("Flower") || this.level.Equals("FlyingBird") || this.level.Equals("Mouse") || this.level.Equals("SallyStagePlay"))
			{
				image.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, -90f);
			}
			else if (this.level.Equals("Train"))
			{
				image.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, 70f);
			}
			else if (this.level.Equals("Bee"))
			{
				image.transform.localScale = Vector3.one;
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, -60f);
			}
			else if (this.level.Equals("DicePalaceMain"))
			{
				image.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, -70f);
			}
			else
			{
				image.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
				image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x, 55f);
			}
			this.difficultyImage.rectTransform.anchoredPosition = new Vector2(0f, -70f);
			this.SetupJapaneseInWord();
		}
	}

	// Token: 0x060032F9 RID: 13049 RVA: 0x000F1560 File Offset: 0x000EF760
	public void SetupAsianDifficulty()
	{
		this.difficultyText.textComponent.fontSize = 29;
		this.easy.gameObject.GetComponent<TMP_Text>().fontSize = 37f;
		this.normal.gameObject.GetComponent<TMP_Text>().fontSize = 37f;
		this.hard.gameObject.GetComponent<TMP_Text>().fontSize = 37f;
	}

	// Token: 0x060032FA RID: 13050 RVA: 0x000F15D0 File Offset: 0x000EF7D0
	public void SetupJapaneseInWord()
	{
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
		{
			this.inAnimated.enabled = false;
			this.inText.enabled = false;
			return;
		}
		this.inAnimated.preserveAspect = true;
		if (this.level.Equals("Flower") || this.level.Equals("FlyingBird") || this.level.Equals("Mouse") || this.level.Equals("SallyStagePlay") || this.level.Equals("Bee") || this.level.Equals("DicePalaceMain"))
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, this.initialinImagePosY.y - 15.9f);
			this.inAnimated.rectTransform.offsetMin = new Vector2(this.inAnimated.rectTransform.offsetMin.x, this.initialinImagePosX.y - 15.9f);
		}
		else
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, this.initialinImagePosY.y + 6.5f);
			this.inAnimated.rectTransform.offsetMin = new Vector2(this.inAnimated.rectTransform.offsetMin.x, this.initialinImagePosX.y + 6.5f);
		}
	}

	// Token: 0x060032FB RID: 13051 RVA: 0x000F1790 File Offset: 0x000EF990
	public void SetupKoreanInWord()
	{
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
		{
			this.inAnimated.enabled = false;
			this.inText.enabled = false;
			return;
		}
		this.inText.fontStyle = 0;
		if (this.level.Equals("Bird") || this.level.Equals("Dragon") || this.level.Equals("Devil"))
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -160f);
		}
		else if (this.level.Equals("Flower"))
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -140f);
		}
		else if (this.level.Equals("Bee"))
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -130f);
		}
		else if (this.level.Equals("KingDiceTop"))
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -155f);
		}
		else if (this.level.Equals("Frogs") || this.level.Equals("FlyingBlimp") || this.level.Equals("Baroness") || this.level.Equals("FlyingGenie") || this.level.Equals("Clown") || this.level.Equals("SallyStagePlay") || this.level.Equals("FlyingMermaid"))
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -110f);
		}
		else
		{
			this.inAnimated.rectTransform.offsetMax = new Vector2(this.inAnimated.rectTransform.offsetMax.x, -150f);
		}
	}

	// Token: 0x060032FC RID: 13052 RVA: 0x000F1A2C File Offset: 0x000EFC2C
	public void InWordSetup()
	{
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
		{
			this.inAnimated.enabled = false;
			this.inText.enabled = false;
			return;
		}
		if (Localization.language == Localization.Languages.English)
		{
			this.inAnimated.sprite = this.inSprites[Random.Range(0, this.inSprites.Length)];
		}
		this.inAnimated.enabled = (Localization.language != Localization.Languages.Korean && Localization.language != Localization.Languages.SimplifiedChinese && PlayerData.Data.CurrentMap != Scenes.scene_map_world_DLC);
		this.inAnimated.transform.localScale = Vector3.one;
		if (Localization.language == Localization.Languages.French)
		{
			this.inAnimated.transform.localScale = Vector3.one * 1.5f;
		}
		else if (Localization.language == Localization.Languages.PortugueseBrazil || Localization.language == Localization.Languages.SpanishSpain || Localization.language == Localization.Languages.SpanishAmerica)
		{
			this.inAnimated.transform.localScale = Vector3.one * 1.2f;
		}
		else if (Localization.language == Localization.Languages.Russian)
		{
			this.inAnimated.transform.localScale = Vector3.one * 2f;
		}
	}

	// Token: 0x040029CB RID: 10699
	public const int KoreanUpscaleSize = 100;

	// Token: 0x040029CC RID: 10700
	public const float AsianImageScale = 0.9f;

	// Token: 0x040029CD RID: 10701
	public const float KoreanBossTitleScale = 1.2f;

	// Token: 0x040029CE RID: 10702
	public const int KoreanDifficultyFontSize = 29;

	// Token: 0x040029CF RID: 10703
	public const int KoreanDifficultyOptionsFontSize = 37;

	// Token: 0x040029D2 RID: 10706
	[SerializeField]
	public Image inAnimated;

	// Token: 0x040029D3 RID: 10707
	[SerializeField]
	public Image bossTitleImage;

	// Token: 0x040029D4 RID: 10708
	[SerializeField]
	public Image bossNameImage;

	// Token: 0x040029D5 RID: 10709
	[SerializeField]
	public Image difficultyImage;

	// Token: 0x040029D6 RID: 10710
	[SerializeField]
	public Sprite[] inSprites;

	// Token: 0x040029D7 RID: 10711
	[SerializeField]
	public Image[] separatorsAnimated;

	// Token: 0x040029D8 RID: 10712
	[SerializeField]
	public Sprite[] separatorsSprites;

	// Token: 0x040029D9 RID: 10713
	[SerializeField]
	public RectTransform cursor;

	// Token: 0x040029DA RID: 10714
	[Header("Options")]
	[SerializeField]
	public RectTransform easy;

	// Token: 0x040029DB RID: 10715
	[SerializeField]
	public RectTransform normal;

	// Token: 0x040029DC RID: 10716
	[SerializeField]
	public RectTransform normalSeparator;

	// Token: 0x040029DD RID: 10717
	[SerializeField]
	public RectTransform hard;

	// Token: 0x040029DE RID: 10718
	[SerializeField]
	public RectTransform hardSeparator;

	// Token: 0x040029DF RID: 10719
	[SerializeField]
	public RectTransform box;

	// Token: 0x040029E0 RID: 10720
	[SerializeField]
	public Color selectedColor;

	// Token: 0x040029E1 RID: 10721
	[SerializeField]
	public Color unselectedColor;

	// Token: 0x040029E2 RID: 10722
	[Header("Stage")]
	[SerializeField]
	public LocalizationHelper bossImage;

	// Token: 0x040029E3 RID: 10723
	[SerializeField]
	public LocalizationHelper bossName;

	// Token: 0x040029E4 RID: 10724
	[SerializeField]
	public LocalizationHelper difficultyText;

	// Token: 0x040029E5 RID: 10725
	[SerializeField]
	public Material bossCardWhiteMaterial;

	// Token: 0x040029E6 RID: 10726
	[SerializeField]
	public Image difficultySelectionText;

	// Token: 0x040029E7 RID: 10727
	[SerializeField]
	public Text inText;

	// Token: 0x040029E8 RID: 10728
	[Header("Glow")]
	[SerializeField]
	public GlowText glowScript;

	// Token: 0x040029E9 RID: 10729
	[SerializeField]
	public LocalizationHelper bossGlow;

	// Token: 0x040029EA RID: 10730
	[SerializeField]
	public Image asianGlow;

	// Token: 0x040029EB RID: 10731
	public TMP_Text[] difficulyTexts;

	// Token: 0x040029EC RID: 10732
	public Level.Mode[] options;

	// Token: 0x040029ED RID: 10733
	public int index = 1;

	// Token: 0x040029EE RID: 10734
	public float cursorY;

	// Token: 0x040029EF RID: 10735
	public int initialMaxFontSize;

	// Token: 0x040029F0 RID: 10736
	public Vector2 initialinImagePosX;

	// Token: 0x040029F1 RID: 10737
	public Vector2 initialinImagePosY;

	// Token: 0x040029F2 RID: 10738
	public Vector2 initialinDifficultyPos;

	// Token: 0x040029F3 RID: 10739
	public Vector2 initialDifficultyPos;

	// Token: 0x040029F4 RID: 10740
	public Vector2 initialBossNamePos;
}
