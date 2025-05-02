using System;
using System.Collections.Generic;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F1 RID: 241
public class OptionsGUI : AbstractMonoBehaviour
{
	// Token: 0x170001CB RID: 459
	// (get) Token: 0x06000B3E RID: 2878 RVA: 0x0000A1CE File Offset: 0x000083CE
	// (set) Token: 0x06000B3F RID: 2879 RVA: 0x0000A1D6 File Offset: 0x000083D6
	public OptionsGUI.State state { get; set; }

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06000B40 RID: 2880 RVA: 0x0000A1DF File Offset: 0x000083DF
	// (set) Token: 0x06000B41 RID: 2881 RVA: 0x0000A1E6 File Offset: 0x000083E6
	public static Color COLOR_SELECTED { get; set; }

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0000A1EE File Offset: 0x000083EE
	// (set) Token: 0x06000B43 RID: 2883 RVA: 0x0000A1F5 File Offset: 0x000083F5
	public static Color COLOR_INACTIVE { get; set; }

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06000B44 RID: 2884 RVA: 0x0000A1FD File Offset: 0x000083FD
	// (set) Token: 0x06000B45 RID: 2885 RVA: 0x0000A205 File Offset: 0x00008405
	public bool optionMenuOpen { get; set; }

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06000B46 RID: 2886 RVA: 0x0000A20E File Offset: 0x0000840E
	// (set) Token: 0x06000B47 RID: 2887 RVA: 0x0000A216 File Offset: 0x00008416
	public bool inputEnabled { get; set; }

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06000B48 RID: 2888 RVA: 0x0000A21F File Offset: 0x0000841F
	// (set) Token: 0x06000B49 RID: 2889 RVA: 0x0007E364 File Offset: 0x0007C564
	public int verticalSelection
	{
		get
		{
			return this._verticalSelection;
		}
		set
		{
			bool flag = value > this._verticalSelection;
			int num = (int)Mathf.Repeat((float)value, (float)this.currentItems.Count);
			while (!this.currentItems[num].text.gameObject.activeSelf)
			{
				num = ((!flag) ? (num - 1) : (num + 1));
				num = (int)Mathf.Repeat((float)num, (float)this.currentItems.Count);
			}
			this._verticalSelection = num;
			this.UpdateVerticalSelection();
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06000B4A RID: 2890 RVA: 0x0000A227 File Offset: 0x00008427
	// (set) Token: 0x06000B4B RID: 2891 RVA: 0x0000A22F File Offset: 0x0000842F
	public bool justClosed { get; set; }

	// Token: 0x06000B4C RID: 2892 RVA: 0x0007E3EC File Offset: 0x0007C5EC
	public override void Awake()
	{
		base.Awake();
		this.isConsole = PlatformHelper.IsConsole;
		this.showAlignOption = true;
		this.showTitleScreenOption = DLCManager.DLCEnabled();
		this.optionMenuOpen = false;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
		this.currentItems = new List<OptionsGUI.Button>(this.mainObjectButtons);
		this.resolutions = new List<Resolution>();
		foreach (Resolution resolution in Screen.resolutions)
		{
			Resolution item = default(Resolution);
			item.width = resolution.width;
			item.height = resolution.height;
			item.refreshRate = 60;
			if (!this.resolutions.Contains(item))
			{
				this.resolutions.Add(item);
			}
		}
		this.SetupButtons();
		OptionsGUI.COLOR_SELECTED = this.currentItems[0].text.color;
		OptionsGUI.COLOR_INACTIVE = this.currentItems[this.currentItems.Count - 1].text.color;
		this.initialAudioCenter = this.audioObject.transform.localPosition.x;
		this.initialVisualCenter = this.visualObject.transform.localPosition.x;
		this.initialAudioBackCenter = this.audioObjectButtons[4].text.transform.localPosition.x;
		this.initialVisualBackCenter = this.visualObjectButtons[7].text.transform.localPosition.x;
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x0000A238 File Offset: 0x00008438
	public void Start()
	{
		Localization.OnLanguageChangedEvent += this.UpdateLanguages;
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x0000A24B File Offset: 0x0000844B
	public void OnDestroy()
	{
		Localization.OnLanguageChangedEvent -= this.UpdateLanguages;
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x0007E5A0 File Offset: 0x0007C7A0
	public void UpdateLanguages()
	{
		for (int i = 0; i < this.audioObjectButtons.Length; i++)
		{
			if (this.audioObjectButtons[i].localizationHelper != null)
			{
				this.audioObjectButtons[i].localizationHelper.ApplyTranslation();
			}
		}
		for (int j = 0; j < this.visualObjectButtons.Length; j++)
		{
			if (this.visualObjectButtons[j].localizationHelper != null)
			{
				this.visualObjectButtons[j].localizationHelper.ApplyTranslation();
			}
		}
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x0007E634 File Offset: 0x0007C834
	public void SetupButtons()
	{
		string[] array = new string[this.resolutions.Count];
		int index = 0;
		for (int i = 0; i < this.resolutions.Count; i++)
		{
			array[i] = this.resolutions[i].width + "x" + this.resolutions[i].height;
			if (Screen.width == this.resolutions[i].width && Screen.height == this.resolutions[i].height)
			{
				index = i;
			}
		}
		if (this.isConsole)
		{
			foreach (GameObject gameObject in this.PcOnlyObjects)
			{
				gameObject.SetActive(false);
			}
		}
		if (!DLCManager.DLCEnabled())
		{
			foreach (GameObject gameObject2 in this.dlcHideObjects)
			{
				gameObject2.SetActive(false);
			}
		}
		bool active = PlayerData.inGame && (PlayerData.Data.unlockedBlackAndWhite || PlayerData.Data.unlocked2Strip || PlayerData.Data.unlockedChaliceRecolor);
		foreach (GameObject gameObject3 in this.FilterUnlockedOnlyObjects)
		{
			gameObject3.SetActive(active);
		}
		if (!this.isConsole)
		{
			this.visualObjectButtons[0].options = array;
			this.visualObjectButtons[1].options = new string[]
			{
				"OptionMenuDisplayWindowed",
				"OptionMenuDisplayFullscreen"
			};
			this.visualObjectButtons[2].options = new string[]
			{
				"OptionMenuOn",
				"OptionMenuOff"
			};
		}
		if (this.showAlignOption)
		{
			this.visualObjectButtons[3].options = this.slider;
			this.visualObjectButtons[3].wrap = false;
		}
		this.visualObjectButtons[4].options = this.slider;
		this.visualObjectButtons[4].wrap = false;
		this.visualObjectButtons[5].options = this.slider;
		this.visualObjectButtons[5].wrap = false;
		if (this.showTitleScreenOption)
		{
			this.visualObjectButtons[6].options = new string[]
			{
				"TitleScreenOptionsMenuOriginal",
				"TitleScreenOptionsMenuDLC"
			};
			this.visualObjectButtons[6].wrap = true;
		}
		List<string> list = new List<string>();
		this.unlockedFilters = new List<BlurGamma.Filter>();
		this.unlockedFilters.Add(BlurGamma.Filter.None);
		list.Add("OptionMenuFilterNone");
		if (PlayerData.Data.unlocked2Strip)
		{
			list.Add("OptionMenuFilter2Strip");
			this.unlockedFilters.Add(BlurGamma.Filter.TwoStrip);
		}
		if (PlayerData.Data.unlockedBlackAndWhite)
		{
			list.Add("OptionMenuFilterBlackWhite");
			this.unlockedFilters.Add(BlurGamma.Filter.BW);
		}
		if (PlayerData.Data.unlockedChaliceRecolor)
		{
			list.Add("ChaliceCostumeOptionsMenu");
			this.unlockedFilters.Add(BlurGamma.Filter.Chalice);
		}
		this.visualObjectButtons[7].options = list.ToArray();
		if (!this.isConsole)
		{
			this.visualObjectButtons[0].updateSelection(index);
			this.visualObjectButtons[1].updateSelection((!Screen.fullScreen) ? 0 : 1);
			this.visualObjectButtons[2].updateSelection((QualitySettings.vSyncCount <= 0) ? 1 : 0);
		}
		if (this.showAlignOption)
		{
			this.visualObjectButtons[3].updateSelection(this.floatToSliderIndex(SettingsData.Data.overscan, 0f, 1f));
		}
		this.visualObjectButtons[4].updateSelection(this.floatToSliderIndex(SettingsData.Data.Brightness, -1f, 1f));
		this.visualObjectButtons[5].updateSelection(this.floatToSliderIndex(SettingsData.Data.chromaticAberration, 0.5f, 1.5f));
		if (this.showTitleScreenOption)
		{
			this.visualObjectButtons[6].updateSelection((!SettingsData.Data.forceOriginalTitleScreen) ? 1 : 0);
		}
		this.visualObjectButtons[7].updateSelection(Mathf.Min((int)SettingsData.Data.filter, list.Count - 1));
		this.audioObjectButtons[0].options = this.slider;
		this.audioObjectButtons[0].wrap = false;
		this.audioObjectButtons[1].options = this.slider;
		this.audioObjectButtons[1].wrap = false;
		this.audioObjectButtons[2].options = this.slider;
		this.audioObjectButtons[2].wrap = false;
		this.audioObjectButtons[3].options = new string[]
		{
			"OptionMenuOff",
			"OptionMenuOn"
		};
		this.audioObjectButtons[0].updateSelection(this.floatToSliderIndex(SettingsData.Data.masterVolume, -48f, 0f));
		this.audioObjectButtons[1].updateSelection(this.floatToSliderIndex(SettingsData.Data.sFXVolume, -48f, 0f));
		this.audioObjectButtons[2].updateSelection(this.floatToSliderIndex(SettingsData.Data.musicVolume, -48f, 0f));
		this.audioObjectButtons[3].updateSelection((!SettingsData.Data.vintageAudioEnabled) ? 0 : 1);
		int index2 = 0;
		for (int m = 0; m < this.languageTranslations.Length; m++)
		{
			if (this.languageTranslations[m].language == Localization.language)
			{
				index2 = m;
				break;
			}
		}
		string[] array3 = new string[this.languageTranslations.Length];
		for (int n = 0; n < this.languageTranslations.Length; n++)
		{
			array3[n] = "Language" + this.languageTranslations[n].translation;
		}
		this.languageObjectButtons[0].options = array3;
		this.languageObjectButtons[0].updateSelection(index2);
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x0007EC84 File Offset: 0x0007CE84
	public void ChangeStateCustomLayoutScripts()
	{
		string text = this.visualObjectButtons[1].text.text;
		string value = Localization.Find(this.visualObjectButtons[1].options[0]).translation.SanitizedText();
		bool enabled = text.Equals(value);
		for (int i = 0; i < this.customPositionning.Length; i++)
		{
			this.customPositionning[i].enabled = enabled;
		}
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x0000A25E File Offset: 0x0000845E
	public void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x0007ECFC File Offset: 0x0007CEFC
	public void Update()
	{
		this.justClosed = false;
		if (!this.inputEnabled)
		{
			return;
		}
		if (this.state == OptionsGUI.State.Controls)
		{
			if (Cuphead.Current.controlMapper.isOpen)
			{
				return;
			}
			this.state = OptionsGUI.State.MainOptions;
			this.canvasGroup.alpha = 1f;
			this.ToggleSubMenu(OptionsGUI.State.MainOptions);
			PlayerManager.ControlsChanged();
			return;
		}
		else
		{
			if (this.GetButtonDown(CupheadButton.Pause) || this.GetButtonDown(CupheadButton.Cancel))
			{
				if (this.state == OptionsGUI.State.MainOptions)
				{
					this.MenuSelectSound();
					this.HideMainOptionMenu();
				}
				else
				{
					this.MenuSelectSound();
					this.ToMainOptions();
				}
				return;
			}
			if (this.GetButtonDown(CupheadButton.Accept))
			{
				switch (this.state)
				{
				case OptionsGUI.State.MainOptions:
					this.OptionSelect();
					break;
				case OptionsGUI.State.Visual:
					this.VisualSelect();
					break;
				case OptionsGUI.State.Audio:
					this.AudioSelect();
					break;
				case OptionsGUI.State.Language:
					this.LanguageSelect();
					break;
				}
				return;
			}
			if (this._selectionTimer >= 0.15f)
			{
				if (this.GetButton(CupheadButton.MenuUp))
				{
					this.MenuMoveSound();
					this.verticalSelection--;
				}
				if (this.GetButton(CupheadButton.MenuDown))
				{
					this.MenuMoveSound();
					this.verticalSelection++;
				}
				if (this.GetButton(CupheadButton.MenuRight) && this.currentItems[this.verticalSelection].options.Length > 0)
				{
					this.currentItems[this.verticalSelection].incrementSelection();
					this.UpdateHorizontalSelection();
				}
				if (this.GetButton(CupheadButton.MenuLeft) && this.currentItems[this.verticalSelection].options.Length > 0)
				{
					this.currentItems[this.verticalSelection].decrementSelection();
					this.UpdateHorizontalSelection();
				}
			}
			else
			{
				this._selectionTimer += Time.deltaTime;
			}
			return;
		}
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x0007EEFC File Offset: 0x0007D0FC
	public void UpdateVerticalSelection()
	{
		this._selectionTimer = 0f;
		if (this.state == OptionsGUI.State.Controls)
		{
			return;
		}
		if (this.state == OptionsGUI.State.Visual && this.isConsole && this.showAlignOption && this._verticalSelection < 3)
		{
			this._verticalSelection = 3;
		}
		if (this.state == OptionsGUI.State.Visual && this.isConsole && !this.showAlignOption && this._verticalSelection < 4)
		{
			this._verticalSelection = 4;
		}
		for (int i = 0; i < this.currentItems.Count; i++)
		{
			OptionsGUI.Button button = this.currentItems[i];
			if (i == this.verticalSelection)
			{
				button.text.color = OptionsGUI.COLOR_SELECTED;
			}
			else
			{
				button.text.color = OptionsGUI.COLOR_INACTIVE;
			}
		}
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x0007EFE8 File Offset: 0x0007D1E8
	public void UpdateHorizontalSelection()
	{
		this._selectionTimer = 0f;
		for (int i = 0; i < this.currentItems.Count; i++)
		{
			OptionsGUI.Button button = this.currentItems[i];
			if (i == this.verticalSelection && this.currentItems[i].options.Length > 0)
			{
				OptionsGUI.State state = this.state;
				if (state != OptionsGUI.State.Audio)
				{
					if (state != OptionsGUI.State.Visual)
					{
						if (state == OptionsGUI.State.Language)
						{
							this.LanguageHorizontalSelect(this.currentItems[i]);
						}
					}
					else
					{
						this.VisualHorizontalSelect(this.currentItems[i]);
					}
				}
				else
				{
					this.AudioHorizontalSelect(this.currentItems[i]);
				}
			}
		}
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x0007F0B4 File Offset: 0x0007D2B4
	public void ShowMainOptionMenu()
	{
		this.state = OptionsGUI.State.MainOptions;
		this.ToggleSubMenu(this.state);
		this.optionMenuOpen = true;
		this.verticalSelection = 0;
		this.canvasGroup.alpha = 1f;
		base.FrameDelayedCallback(new Action(this.Interactable), 1);
		this.UpdateVerticalSelection();
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x0007F10C File Offset: 0x0007D30C
	public void HideMainOptionMenu()
	{
		SettingsData.Save();
		if (PlatformHelper.IsConsole)
		{
			SettingsData.SaveToCloud();
		}
		if (this.savePlayerData)
		{
			PlayerData.SaveCurrentFile();
		}
		this.savePlayerData = false;
		this.verticalSelection = 0;
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		this.inputEnabled = false;
		this.optionMenuOpen = false;
		this.justClosed = true;
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x0000A26C File Offset: 0x0000846C
	public void Interactable()
	{
		this.verticalSelection = 0;
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		this.inputEnabled = true;
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x0007F188 File Offset: 0x0007D388
	public void OptionSelect()
	{
		this.MenuSelectSound();
		switch (this.verticalSelection)
		{
		case 0:
			this.ToAudio();
			break;
		case 1:
			this.ToVisual();
			break;
		case 2:
			this.ToControls();
			break;
		case 3:
			this.ToLanguage();
			break;
		case 4:
			this.ToPauseMenu();
			break;
		}
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x0000A294 File Offset: 0x00008494
	public void MenuSelectSound()
	{
		AudioManager.Play("level_menu_select");
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x0000A2A0 File Offset: 0x000084A0
	public void MenuMoveSound()
	{
		AudioManager.Play("level_menu_move");
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x0000A2AC File Offset: 0x000084AC
	public void ToVisual()
	{
		this.state = OptionsGUI.State.Visual;
		this.CenterVisual();
		if (!this.isConsole)
		{
			this.ChangeStateCustomLayoutScripts();
		}
		this.ToggleSubMenu(this.state);
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x0000A2D8 File Offset: 0x000084D8
	public void ToAudio()
	{
		this.state = OptionsGUI.State.Audio;
		this.CenterAudio();
		this.ToggleSubMenu(this.state);
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x0000A2F3 File Offset: 0x000084F3
	public void ToLanguage()
	{
		this.state = OptionsGUI.State.Language;
		this.ToggleSubMenu(this.state);
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x0000A308 File Offset: 0x00008508
	public void ToControls()
	{
		this.state = OptionsGUI.State.Controls;
		this.ToggleSubMenu(this.state);
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x0000A31D File Offset: 0x0000851D
	public void ToPauseMenu()
	{
		this.optionMenuOpen = false;
		this.HideMainOptionMenu();
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x0007F1F8 File Offset: 0x0007D3F8
	public void ToggleSubMenu(OptionsGUI.State state)
	{
		this.currentItems.Clear();
		switch (state)
		{
		case OptionsGUI.State.MainOptions:
			this.mainObject.SetActive(true);
			this.visualObject.SetActive(false);
			this.audioObject.SetActive(false);
			this.languageObject.SetActive(false);
			this.bigCard.SetActive(false);
			this.bigNoise.SetActive(false);
			this.currentItems.AddRange(this.mainObjectButtons);
			break;
		case OptionsGUI.State.Visual:
			this.mainObject.SetActive(false);
			this.visualObject.SetActive(true);
			this.audioObject.SetActive(false);
			this.bigCard.SetActive(true);
			this.bigNoise.SetActive(true);
			this.currentItems.AddRange(this.visualObjectButtons);
			break;
		case OptionsGUI.State.Audio:
			this.mainObject.SetActive(false);
			this.visualObject.SetActive(false);
			this.audioObject.SetActive(true);
			this.languageObject.SetActive(false);
			this.bigCard.SetActive(true);
			this.bigNoise.SetActive(true);
			this.currentItems.AddRange(this.audioObjectButtons);
			break;
		case OptionsGUI.State.Controls:
			this.mainObject.SetActive(false);
			this.visualObject.SetActive(false);
			this.audioObject.SetActive(false);
			this.languageObject.SetActive(false);
			this.ShowControlMapper();
			break;
		case OptionsGUI.State.Language:
			this.languageObjectButtons[0].updateSelection((int)Localization.language);
			this.mainObject.SetActive(false);
			this.audioObject.SetActive(false);
			this.languageObject.SetActive(true);
			this.bigCard.SetActive(false);
			this.bigNoise.SetActive(false);
			this.currentItems.AddRange(this.languageObjectButtons);
			break;
		}
		if (state != OptionsGUI.State.Controls)
		{
			this.verticalSelection = 0;
			this.UpdateVerticalSelection();
		}
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x0007F3F0 File Offset: 0x0007D5F0
	public void ShowControlMapper()
	{
		ControlMapper controlMapper = Cuphead.Current.controlMapper;
		Canvas componentInChildren = controlMapper.GetComponentInChildren<Canvas>(true);
		CupheadUICamera cupheadUICamera = Object.FindObjectOfType<CupheadUICamera>();
		if (cupheadUICamera != null && componentInChildren != null)
		{
			componentInChildren.worldCamera = cupheadUICamera.GetComponent<Camera>();
		}
		controlMapper.showPlayers = true;
		if (PlatformHelper.IsConsole)
		{
			controlMapper.showKeyboard = false;
			controlMapper.showControllerGroupButtons = false;
		}
		controlMapper.showControllerGroupButtons = !PlatformHelper.IsConsole;
		controlMapper.Reset();
		controlMapper.Open();
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x0000A32C File Offset: 0x0000852C
	public void ToMainOptions()
	{
		this.state = OptionsGUI.State.MainOptions;
		this.ToggleSubMenu(this.state);
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x0007F49C File Offset: 0x0007D69C
	public void VisualHorizontalSelect(OptionsGUI.Button button)
	{
		switch (this.verticalSelection)
		{
		case 0:
			this.MenuSelectSound();
			if (button.selection < this.resolutions.Count)
			{
				SettingsData.Data.screenWidth = this.resolutions[button.selection].width;
				SettingsData.Data.screenHeight = this.resolutions[button.selection].height;
				Screen.SetResolution(SettingsData.Data.screenWidth, SettingsData.Data.screenHeight, Screen.fullScreen, 60);
			}
			break;
		case 1:
			this.MenuSelectSound();
			SettingsData.Data.fullScreen = (button.selection == 1);
			if (!this.isConsole)
			{
				this.ChangeStateCustomLayoutScripts();
			}
			Screen.fullScreen = SettingsData.Data.fullScreen;
			break;
		case 2:
			this.MenuSelectSound();
			SettingsData.Data.vSyncCount = ((button.selection != 0) ? 0 : 1);
			QualitySettings.vSyncCount = SettingsData.Data.vSyncCount;
			break;
		case 3:
			SettingsData.Data.overscan = this.sliderIndexToFloat(button.selection, 0f, 1f);
			break;
		case 4:
			SettingsData.Data.Brightness = this.sliderIndexToFloat(button.selection, -1f, 1f);
			break;
		case 5:
			SettingsData.Data.chromaticAberration = this.sliderIndexToFloat(button.selection, 0.5f, 1.5f);
			break;
		case 6:
			SettingsData.Data.forceOriginalTitleScreen = !SettingsData.Data.forceOriginalTitleScreen;
			break;
		case 7:
			this.MenuSelectSound();
			PlayerData.Data.filter = this.unlockedFilters[button.selection];
			EventManager.Instance.Raise(new ChaliceRecolorEvent(this.unlockedFilters[button.selection] == BlurGamma.Filter.Chalice));
			this.savePlayerData = true;
			break;
		}
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x0007F6B4 File Offset: 0x0007D8B4
	public void VisualSelect()
	{
		AudioManager.Play("level_menu_select");
		int verticalSelection = this.verticalSelection;
		if (verticalSelection == 8)
		{
			this.ToMainOptions();
		}
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0007F6EC File Offset: 0x0007D8EC
	public void LanguageSelect()
	{
		AudioManager.Play("level_menu_select");
		int verticalSelection = this.verticalSelection;
		if (verticalSelection == 1)
		{
			this.ToMainOptions();
		}
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x0007F724 File Offset: 0x0007D924
	public void LanguageHorizontalSelect(OptionsGUI.Button button)
	{
		int verticalSelection = this.verticalSelection;
		if (verticalSelection == 0)
		{
			Localization.language = this.languageTranslations[button.selection].language;
			button.updateSelection(button.selection);
			for (int i = 0; i < this.elementsToTranslate.Length; i++)
			{
				this.elementsToTranslate[i].ApplyTranslation();
			}
		}
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x0007F798 File Offset: 0x0007D998
	public void AudioHorizontalSelect(OptionsGUI.Button button)
	{
		switch (this.verticalSelection)
		{
		case 0:
			AudioManager.masterVolume = ((button.selection > 0) ? this.sliderIndexToFloat(button.selection, -48f, 0f) : -80f);
			SettingsData.Data.masterVolume = AudioManager.masterVolume;
			break;
		case 1:
			AudioManager.sfxOptionsVolume = ((button.selection > 0) ? this.sliderIndexToFloat(button.selection, -48f, 0f) : -80f);
			SettingsData.Data.sFXVolume = AudioManager.sfxOptionsVolume;
			break;
		case 2:
			AudioManager.bgmOptionsVolume = ((button.selection > 0) ? this.sliderIndexToFloat(button.selection, -48f, 0f) : -80f);
			SettingsData.Data.musicVolume = AudioManager.bgmOptionsVolume;
			break;
		case 3:
			this.MenuSelectSound();
			if (button.selection == 0)
			{
				PlayerData.Data.vintageAudioEnabled = false;
			}
			else if (button.options[button.selection] == button.options[1])
			{
				PlayerData.Data.vintageAudioEnabled = true;
			}
			this.savePlayerData = true;
			break;
		}
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x0000A341 File Offset: 0x00008541
	public void MasterVolume(string option)
	{
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0007F8F0 File Offset: 0x0007DAF0
	public void AudioSelect()
	{
		AudioManager.Play("level_menu_select");
		int verticalSelection = this.verticalSelection;
		if (verticalSelection == 4)
		{
			this.ToMainOptions();
		}
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x0000A343 File Offset: 0x00008543
	public bool GetButtonDown(CupheadButton button)
	{
		return this.input.GetButtonDown(button);
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x0000A359 File Offset: 0x00008559
	public bool GetButton(CupheadButton button)
	{
		return this.input.GetButton(button);
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x0000A36F File Offset: 0x0000856F
	public float sliderIndexToFloat(int index, float min, float max)
	{
		if (index != this.lastIndex)
		{
			this.MenuSelectSound();
		}
		this.lastIndex = index;
		return (float)index / (float)(this.slider.Length - 1) * (max - min) + min;
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x0007F928 File Offset: 0x0007DB28
	public int floatToSliderIndex(float value, float min, float max)
	{
		int num = Mathf.RoundToInt((value - min) / (max - min) * (float)(this.slider.Length - 1));
		if (num > this.slider.Length - 1)
		{
			num = this.slider.Length - 1;
		}
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x0007F974 File Offset: 0x0007DB74
	public void CenterAudio()
	{
		float num = this.audioCenterPositions[(int)Localization.language];
		this.audioObject.transform.SetLocalPosition(new float?(this.initialAudioCenter + num), null, null);
		this.audioObjectButtons[4].text.transform.SetLocalPosition(new float?(this.initialAudioBackCenter - num), null, null);
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0007F9F4 File Offset: 0x0007DBF4
	public void CenterVisual()
	{
		float num = this.visualCenterPositions[(int)Localization.language];
		this.visualObject.transform.SetLocalPosition(new float?(this.initialVisualCenter + num), null, null);
		this.visualObjectButtons[7].text.transform.SetLocalPosition(new float?(this.initialVisualBackCenter - num), null, null);
	}

	// Token: 0x040008E9 RID: 2281
	public const float BRIGHTNESS_MAX = 1f;

	// Token: 0x040008EA RID: 2282
	public const float VOLUME_MIN = -48f;

	// Token: 0x040008EB RID: 2283
	public const float CHROMATIC_ABERRATION_MIN = 0.5f;

	// Token: 0x040008EC RID: 2284
	public const float CHROMATIC_ABERRATION_MAX = 1.5f;

	// Token: 0x040008ED RID: 2285
	public const float VOLUME_NONE = -80f;

	// Token: 0x040008F3 RID: 2291
	[SerializeField]
	public GameObject mainObject;

	// Token: 0x040008F4 RID: 2292
	[SerializeField]
	public GameObject visualObject;

	// Token: 0x040008F5 RID: 2293
	[SerializeField]
	public GameObject audioObject;

	// Token: 0x040008F6 RID: 2294
	[SerializeField]
	public GameObject languageObject;

	// Token: 0x040008F7 RID: 2295
	[SerializeField]
	public OptionsGUI.Button[] mainObjectButtons;

	// Token: 0x040008F8 RID: 2296
	[SerializeField]
	public GameObject[] PcOnlyObjects;

	// Token: 0x040008F9 RID: 2297
	[SerializeField]
	public GameObject[] playStation4HideObjects;

	// Token: 0x040008FA RID: 2298
	[SerializeField]
	public GameObject[] dlcHideObjects;

	// Token: 0x040008FB RID: 2299
	[SerializeField]
	public GameObject[] FilterUnlockedOnlyObjects;

	// Token: 0x040008FC RID: 2300
	[SerializeField]
	public GameObject bigCard;

	// Token: 0x040008FD RID: 2301
	[SerializeField]
	public GameObject bigNoise;

	// Token: 0x040008FE RID: 2302
	[SerializeField]
	public OptionsGUI.Button[] visualObjectButtons;

	// Token: 0x040008FF RID: 2303
	[SerializeField]
	public OptionsGUI.Button[] audioObjectButtons;

	// Token: 0x04000900 RID: 2304
	[SerializeField]
	public OptionsGUI.Button[] languageObjectButtons;

	// Token: 0x04000901 RID: 2305
	[SerializeField]
	public OptionsGUI.LanguageTranslation[] languageTranslations;

	// Token: 0x04000902 RID: 2306
	[SerializeField]
	public LocalizationHelper[] elementsToTranslate;

	// Token: 0x04000903 RID: 2307
	[SerializeField]
	public float[] audioCenterPositions;

	// Token: 0x04000904 RID: 2308
	[SerializeField]
	public float[] visualCenterPositions;

	// Token: 0x04000905 RID: 2309
	[SerializeField]
	public CustomLanguageLayout[] customPositionning;

	// Token: 0x04000906 RID: 2310
	public List<OptionsGUI.Button> currentItems;

	// Token: 0x04000907 RID: 2311
	public List<BlurGamma.Filter> unlockedFilters;

	// Token: 0x04000908 RID: 2312
	public bool isConsole;

	// Token: 0x04000909 RID: 2313
	public bool showAlignOption;

	// Token: 0x0400090A RID: 2314
	public bool showTitleScreenOption;

	// Token: 0x0400090B RID: 2315
	public string[] slider = new string[]
	{
		"|----------",
		"-|---------",
		"--|--------",
		"---|-------",
		"----|------",
		"-----|-----",
		"------|----",
		"-------|---",
		"--------|--",
		"---------|-",
		"----------|"
	};

	// Token: 0x0400090C RID: 2316
	public CanvasGroup canvasGroup;

	// Token: 0x0400090D RID: 2317
	public AbstractPauseGUI pauseMenu;

	// Token: 0x0400090E RID: 2318
	public float _selectionTimer;

	// Token: 0x0400090F RID: 2319
	public const float _SELECTION_TIME = 0.15f;

	// Token: 0x04000910 RID: 2320
	public int _verticalSelection;

	// Token: 0x04000911 RID: 2321
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04000912 RID: 2322
	public int lastIndex;

	// Token: 0x04000913 RID: 2323
	public List<Resolution> resolutions;

	// Token: 0x04000915 RID: 2325
	public float initialAudioCenter;

	// Token: 0x04000916 RID: 2326
	public float initialVisualCenter;

	// Token: 0x04000917 RID: 2327
	public float initialAudioBackCenter;

	// Token: 0x04000918 RID: 2328
	public float initialVisualBackCenter;

	// Token: 0x04000919 RID: 2329
	public bool savePlayerData;

	// Token: 0x02000967 RID: 2407
	[Serializable]
	public struct LanguageTranslation
	{
		// Token: 0x0400468E RID: 18062
		[SerializeField]
		public Localization.Languages language;

		// Token: 0x0400468F RID: 18063
		[SerializeField]
		public string translation;
	}

	// Token: 0x02000968 RID: 2408
	public enum State
	{
		// Token: 0x04004691 RID: 18065
		MainOptions,
		// Token: 0x04004692 RID: 18066
		Visual,
		// Token: 0x04004693 RID: 18067
		Audio,
		// Token: 0x04004694 RID: 18068
		Controls,
		// Token: 0x04004695 RID: 18069
		Language
	}

	// Token: 0x02000969 RID: 2409
	public enum VisualOptions
	{
		// Token: 0x04004697 RID: 18071
		Resolution,
		// Token: 0x04004698 RID: 18072
		Display,
		// Token: 0x04004699 RID: 18073
		VSync,
		// Token: 0x0400469A RID: 18074
		Align,
		// Token: 0x0400469B RID: 18075
		Brightness,
		// Token: 0x0400469C RID: 18076
		ChromaticAberration,
		// Token: 0x0400469D RID: 18077
		TitleScreen,
		// Token: 0x0400469E RID: 18078
		Filter
	}

	// Token: 0x0200096A RID: 2410
	public enum AudioOptions
	{
		// Token: 0x040046A0 RID: 18080
		MasterVol,
		// Token: 0x040046A1 RID: 18081
		SFXVol,
		// Token: 0x040046A2 RID: 18082
		MusicVol,
		// Token: 0x040046A3 RID: 18083
		Vintage
	}

	// Token: 0x0200096B RID: 2411
	public enum LanguageOptions
	{
		// Token: 0x040046A5 RID: 18085
		Language
	}

	// Token: 0x0200096C RID: 2412
	[Serializable]
	public class Button
	{
		// Token: 0x060054F4 RID: 21748 RVA: 0x001C518C File Offset: 0x001C338C
		public void updateSelection(int index)
		{
			this.selection = index;
			if (this.localizationHelper == null)
			{
				this.text.text = this.options[index];
			}
			else
			{
				this.localizationHelper.ApplyTranslation(Localization.Find(this.options[index]), null);
			}
		}

		// Token: 0x060054F5 RID: 21749 RVA: 0x00040443 File Offset: 0x0003E643
		public void incrementSelection()
		{
			if (this.wrap || this.selection < this.options.Length - 1)
			{
				this.updateSelection((this.selection + 1) % this.options.Length);
			}
		}

		// Token: 0x060054F6 RID: 21750 RVA: 0x001C51E4 File Offset: 0x001C33E4
		public void decrementSelection()
		{
			if (this.wrap || this.selection > 0)
			{
				this.updateSelection((this.selection != 0) ? (this.selection - 1) : (this.options.Length - 1));
			}
		}

		// Token: 0x040046A6 RID: 18086
		public Text text;

		// Token: 0x040046A7 RID: 18087
		public LocalizationHelper localizationHelper;

		// Token: 0x040046A8 RID: 18088
		public string[] options;

		// Token: 0x040046A9 RID: 18089
		public int selection;

		// Token: 0x040046AA RID: 18090
		public bool wrap = true;
	}
}
