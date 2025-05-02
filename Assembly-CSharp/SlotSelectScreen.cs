using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004DA RID: 1242
public class SlotSelectScreen : AbstractMonoBehaviour
{
	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06003364 RID: 13156 RVA: 0x0002A8E3 File Offset: 0x00028AE3
	public bool RespondToDeadPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003365 RID: 13157 RVA: 0x000F4240 File Offset: 0x000F2440
	public override void Awake()
	{
		base.Awake();
		Cuphead.Init(false);
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.isConsole = PlatformHelper.IsConsole;
		PlayerData.inGame = false;
		List<Text> list = new List<Text>(this.mainMenuItems);
		List<SlotSelectScreen.MainMenuItem> list2 = new List<SlotSelectScreen.MainMenuItem>((SlotSelectScreen.MainMenuItem[])Enum.GetValues(typeof(SlotSelectScreen.MainMenuItem)));
		if (this.isConsole)
		{
			this.mainMenuItems[4].gameObject.SetActive(false);
			list.RemoveAt(4);
			list2.RemoveAt(4);
		}
		if (!PlatformHelper.ShowDLCMenuItem)
		{
			this.mainMenuItems[3].gameObject.SetActive(false);
			list.RemoveAt(3);
			list2.RemoveAt(3);
		}
		if (!PlatformHelper.ShowAchievements)
		{
			this.mainMenuItems[1].gameObject.SetActive(false);
			list.RemoveAt(1);
			list2.RemoveAt(1);
		}
		this.mainMenuItems = list.ToArray();
		this._availableMainMenuItems = list2.ToArray();
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x000F4338 File Offset: 0x000F2538
	public void Update()
	{
		if (this.dataStatus == SlotSelectScreen.SaveDataStatus.Received)
		{
			this.dataStatus = SlotSelectScreen.SaveDataStatus.Initialized;
			base.StartCoroutine(this.allDataLoaded_cr());
		}
		this.timeSinceStart += Time.deltaTime;
		switch (this.state)
		{
		case SlotSelectScreen.State.MainMenu:
			this.UpdateMainMenu();
			break;
		case SlotSelectScreen.State.AchievementsMenu:
			this.UpdateAchievementsMenu();
			break;
		case SlotSelectScreen.State.OptionsMenu:
			this.UpdateOptionsMenu();
			break;
		case SlotSelectScreen.State.DLC:
			this.UpdateDLCMenu();
			break;
		case SlotSelectScreen.State.SlotSelect:
			this.UpdateSlotSelect();
			break;
		case SlotSelectScreen.State.ConfirmDelete:
			this.UpdateConfirmDelete();
			break;
		case SlotSelectScreen.State.PlayerSelect:
			this.UpdatePlayerSelect();
			break;
		}
	}

	// Token: 0x06003367 RID: 13159 RVA: 0x000F43F4 File Offset: 0x000F25F4
	public void Start()
	{
		if (StartScreenAudio.Instance == null)
		{
			Object.Instantiate(Resources.Load("Audio/TitleScreenAudio"));
			SceneLoader.OnLoaderCompleteEvent += this.PlayMusic;
		}
		CupheadLevelCamera.Current.StartSmoothShake(8f, 3f, 2);
		this.SetState(SlotSelectScreen.State.InitializeStorage);
		PlayerData.Init(new PlayerData.PlayerDataInitHandler(this.OnPlayerDataInitialized));
	}

	// Token: 0x06003368 RID: 13160 RVA: 0x0002A8E6 File Offset: 0x00028AE6
	public void PlayMusic()
	{
		AudioManager.PlayBGMPlaylistManually(true);
	}

	// Token: 0x06003369 RID: 13161 RVA: 0x0002A8EE File Offset: 0x00028AEE
	public void OnDestroy()
	{
		SceneLoader.OnLoaderCompleteEvent -= this.PlayMusic;
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x000F4460 File Offset: 0x000F2660
	public void SetState(SlotSelectScreen.State state)
	{
		this.state = state;
		this.mainMenuChild.gameObject.SetActive(state == SlotSelectScreen.State.MainMenu);
		this.LoadingChild.gameObject.SetActive(state == SlotSelectScreen.State.InitializeStorage);
		this.slotSelectChild.gameObject.SetActive(state == SlotSelectScreen.State.SlotSelect || state == SlotSelectScreen.State.ConfirmDelete || state == SlotSelectScreen.State.PlayerSelect);
		this.confirmDeleteChild.gameObject.SetActive(state == SlotSelectScreen.State.ConfirmDelete);
		this.confirmPrompt.gameObject.SetActive(state == SlotSelectScreen.State.MainMenu || state == SlotSelectScreen.State.OptionsMenu || state == SlotSelectScreen.State.SlotSelect || state == SlotSelectScreen.State.ConfirmDelete || state == SlotSelectScreen.State.PlayerSelect);
		this.confirmGlyph.gameObject.SetActive(this.confirmPrompt.gameObject.activeSelf);
		this.confirmSpacer.gameObject.SetActive(this.confirmPrompt.gameObject.activeSelf);
		this.backPrompt.gameObject.SetActive(state == SlotSelectScreen.State.OptionsMenu || state == SlotSelectScreen.State.SlotSelect || state == SlotSelectScreen.State.ConfirmDelete || state == SlotSelectScreen.State.PlayerSelect || state == SlotSelectScreen.State.AchievementsMenu || state == SlotSelectScreen.State.DLC);
		this.backGlyph.gameObject.SetActive(this.backPrompt.gameObject.activeSelf);
		this.backSpacer.gameObject.SetActive(this.backPrompt.gameObject.activeSelf);
		this.deletePrompt.gameObject.SetActive(state == SlotSelectScreen.State.SlotSelect);
		this.deleteGlyph.gameObject.SetActive(this.deletePrompt.gameObject.activeSelf);
		this.deleteSpacer.gameObject.SetActive(this.deletePrompt.gameObject.activeSelf);
		this.storePrompt.gameObject.SetActive(state == SlotSelectScreen.State.DLC && DLCManager.CanRedirectToStore() && !DLCManager.DLCEnabled());
		this.storeGlyph.gameObject.SetActive(this.storePrompt.gameObject.activeSelf);
		this.storeSpacer.gameObject.SetActive(this.storePrompt.gameObject.activeSelf);
		this.playerProfiles.gameObject.SetActive(state == SlotSelectScreen.State.SlotSelect || state == SlotSelectScreen.State.MainMenu);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, state == SlotSelectScreen.State.SlotSelect || state == SlotSelectScreen.State.MainMenu);
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x000F46B4 File Offset: 0x000F28B4
	public void UpdateMainMenu()
	{
		if (this.timeSinceStart < 0.75f)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.MenuDown))
		{
			AudioManager.Play("level_menu_move");
			this._mainMenuSelection = (this._mainMenuSelection + 1) % this.mainMenuItems.Length;
		}
		if (this.GetButtonDown(CupheadButton.MenuUp))
		{
			AudioManager.Play("level_menu_move");
			this._mainMenuSelection--;
			if (this._mainMenuSelection < 0)
			{
				this._mainMenuSelection = this.mainMenuItems.Length - 1;
			}
		}
		for (int i = 0; i < this.mainMenuItems.Length; i++)
		{
			this.mainMenuItems[i].color = ((this._mainMenuSelection != i) ? this.mainMenuUnselectedColor : this.mainMenuSelectedColor);
		}
		if (this.GetButtonDown(CupheadButton.Accept))
		{
			AudioManager.Play("level_menu_select");
			switch (this._availableMainMenuItems[this._mainMenuSelection])
			{
			case SlotSelectScreen.MainMenuItem.Start:
				this.SetState(SlotSelectScreen.State.SlotSelect);
				for (int j = 0; j < 3; j++)
				{
					this.slots[j].Init(j);
				}
				break;
			case SlotSelectScreen.MainMenuItem.Achievements:
				this.SetState(SlotSelectScreen.State.AchievementsMenu);
				this.achievements.ShowAchievements();
				break;
			case SlotSelectScreen.MainMenuItem.Options:
				this.SetState(SlotSelectScreen.State.OptionsMenu);
				this.options.ShowMainOptionMenu();
				break;
			case SlotSelectScreen.MainMenuItem.DLC:
				this.SetState(SlotSelectScreen.State.DLC);
				this.dlcMenu.ShowDLCMenu();
				break;
			case SlotSelectScreen.MainMenuItem.Exit:
				Application.Quit();
				break;
			}
		}
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x000F4844 File Offset: 0x000F2A44
	public void UpdateOptionsMenu()
	{
		this.prompts.gameObject.SetActive(!Cuphead.Current.controlMapper.isOpen);
		if (!this.options.optionMenuOpen && !this.options.justClosed)
		{
			this.SetState(SlotSelectScreen.State.MainMenu);
		}
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x0002A901 File Offset: 0x00028B01
	public void UpdateAchievementsMenu()
	{
		if (!this.achievements.achievementsMenuOpen && !this.achievements.justClosed)
		{
			this.SetState(SlotSelectScreen.State.MainMenu);
		}
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x0002A92A File Offset: 0x00028B2A
	public void UpdateDLCMenu()
	{
		if (!this.dlcMenu.dlcMenuOpen && !this.dlcMenu.justClosed)
		{
			this.SetState(SlotSelectScreen.State.MainMenu);
		}
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x000F489C File Offset: 0x000F2A9C
	public void UpdatePlayerSelect()
	{
		if (PlayerData.inGame)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.MenuLeft) || this.GetButtonDown(CupheadButton.MenuRight))
		{
			AudioManager.Play("level_menu_move");
			this.slots[this._slotSelection].SwapSprite();
		}
		else if (this.GetButtonDown(CupheadButton.Cancel))
		{
			AudioManager.Play("level_menu_select");
			for (int i = 0; i < this.slots.Length; i++)
			{
				if (i != this._slotSelection)
				{
					base.StartCoroutine(this.activate_noise_cr(i));
				}
			}
			this.slots[this._slotSelection].StopSelectingPlayer();
			this.SetState(SlotSelectScreen.State.SlotSelect);
		}
		else if (this.GetButtonDown(CupheadButton.Accept))
		{
			AudioManager.Play("ui_menu_confirm");
			this.slots[this._slotSelection].PlayAnimation(this._slotSelection);
			base.StartCoroutine(this.game_start_cr());
		}
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x000F4994 File Offset: 0x000F2B94
	public void UpdateSlotSelect()
	{
		if (PlayerData.inGame)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.MenuDown))
		{
			AudioManager.Play("ui_saveslot_move");
			this._slotSelection = (this._slotSelection + 1) % 3;
		}
		if (this.GetButtonDown(CupheadButton.MenuUp))
		{
			AudioManager.Play("ui_saveslot_move");
			this._slotSelection--;
			if (this._slotSelection < 0)
			{
				this._slotSelection = 2;
			}
		}
		for (int i = 0; i < 3; i++)
		{
			this.slots[i].SetSelected(this._slotSelection == i);
		}
		if (this.GetButtonDown(CupheadButton.Accept))
		{
			AudioManager.Play("level_select");
			for (int j = 0; j < this.slots.Length; j++)
			{
				if (j != this._slotSelection)
				{
					this.slots[j].noise.gameObject.SetActive(false);
				}
			}
			this.slots[this._slotSelection].EnterSelectMenu();
			this.SetState(SlotSelectScreen.State.PlayerSelect);
		}
		else if (this.GetButtonDown(CupheadButton.Cancel))
		{
			AudioManager.Play("level_menu_select");
			this.SetState(SlotSelectScreen.State.MainMenu);
		}
		else if (!this.slots[this._slotSelection].IsEmpty && this.GetButtonDown(CupheadButton.EquipMenu))
		{
			AudioManager.Play("level_menu_select");
			this.SetState(SlotSelectScreen.State.ConfirmDelete);
			this._confirmDeleteSelection = 1;
			this.confirmDeleteSlotTitle.text = this.slots[this._slotSelection].GetSlotTitle();
			this.confirmDeleteSlotTitle.font = this.slots[this._slotSelection].GetSlotTitleFont();
			this.confirmDeleteSlotSeparator.text = this.slots[this._slotSelection].GetSlotSeparator();
			this.confirmDeleteSlotSeparator.font = this.slots[this._slotSelection].GetSlotSeparatorFont();
			this.confirmDeleteSlotPercentage.text = this.slots[this._slotSelection].GetSlotPercentage() + "?";
			this.confirmDeleteSlotPercentage.font = this.slots[this._slotSelection].GetSlotPercentageFont();
		}
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x000F4BBC File Offset: 0x000F2DBC
	public IEnumerator game_start_cr()
	{
		PlayerData.inGame = true;
		for (int i = 0; i < 45; i++)
		{
			yield return null;
		}
		this.EnterGame();
		yield break;
	}

	// Token: 0x06003372 RID: 13170 RVA: 0x000F4BD8 File Offset: 0x000F2DD8
	public IEnumerator activate_noise_cr(int index)
	{
		for (int i = 0; i < 10; i++)
		{
			yield return null;
		}
		this.slots[index].noise.gameObject.SetActive(true);
		yield return null;
		yield break;
	}

	// Token: 0x06003373 RID: 13171 RVA: 0x000F4BFC File Offset: 0x000F2DFC
	public void EnterGame()
	{
		DLCManager.RefreshDLC();
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerData.CurrentSaveFileIndex = this._slotSelection;
		PlayerManager.player1IsMugman = this.slots[this._slotSelection].isPlayer1Mugman;
		PlayerData.GetDataForSlot(this._slotSelection).isPlayer1Mugman = PlayerManager.player1IsMugman;
		if (!DLCManager.DLCEnabled())
		{
			PlayerData data = PlayerData.Data;
			for (int i = 0; i < 2; i++)
			{
				PlayerId player = (PlayerId)i;
				PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = data.Loadouts.GetPlayerLoadout(player);
				if (Array.IndexOf<Weapon>(PlayerData.WeaponsDLC, playerLoadout.secondaryWeapon) >= 0)
				{
					playerLoadout.secondaryWeapon = Weapon.None;
				}
				if (Array.IndexOf<Weapon>(PlayerData.WeaponsDLC, playerLoadout.primaryWeapon) >= 0)
				{
					playerLoadout.primaryWeapon = Weapon.level_weapon_peashot;
					if (playerLoadout.secondaryWeapon == Weapon.level_weapon_peashot)
					{
						playerLoadout.secondaryWeapon = Weapon.None;
					}
				}
				if (Array.IndexOf<Charm>(PlayerData.CharmsDLC, playerLoadout.charm) >= 0)
				{
					playerLoadout.charm = Charm.None;
				}
			}
		}
		Level.ResetPreviousLevelInfo();
		if (!this.slots[this._slotSelection].IsEmpty)
		{
			if (!DLCManager.DLCEnabled() && PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
			{
				PlayerData.Data.CurrentMap = Scenes.scene_map_world_1;
				PlayerData.Data.GetMapData(Scenes.scene_map_world_1).sessionStarted = false;
			}
			SceneLoader.LoadScene(PlayerData.Data.CurrentMap, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
		else
		{
			PlayerData.Data.CurrentMap = Scenes.scene_map_world_1;
			PlayerData.Data.GetMapData(Scenes.scene_map_world_1).sessionStarted = false;
			Cutscene.Load(Scenes.scene_level_house_elder_kettle, Scenes.scene_cutscene_intro, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass);
		}
		PlayerData.inGame = true;
		if (StartScreenAudio.Instance != null)
		{
			Object.Destroy(StartScreenAudio.Instance.gameObject);
		}
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x000F4DB4 File Offset: 0x000F2FB4
	public void UpdateConfirmDelete()
	{
		if (this.GetButtonDown(CupheadButton.MenuDown))
		{
			AudioManager.Play("level_menu_move");
			this._confirmDeleteSelection = (this._confirmDeleteSelection + 1) % 2;
		}
		if (this.GetButtonDown(CupheadButton.MenuUp))
		{
			AudioManager.Play("level_menu_move");
			this._confirmDeleteSelection--;
			if (this._confirmDeleteSelection < 0)
			{
				this._confirmDeleteSelection = 1;
			}
		}
		for (int i = 0; i < 2; i++)
		{
			this.confirmDeleteItems[i].color = ((this._confirmDeleteSelection != i) ? this.confirmDeleteUnselectedColor : this.confirmDeleteSelectedColor);
		}
		if (this.GetButtonDown(CupheadButton.Accept))
		{
			SlotSelectScreen.ConfirmDeleteItem confirmDeleteSelection = (SlotSelectScreen.ConfirmDeleteItem)this._confirmDeleteSelection;
			if (confirmDeleteSelection != SlotSelectScreen.ConfirmDeleteItem.Yes)
			{
				if (confirmDeleteSelection == SlotSelectScreen.ConfirmDeleteItem.No)
				{
					AudioManager.Play("level_menu_select");
					this.SetState(SlotSelectScreen.State.SlotSelect);
				}
			}
			else
			{
				AudioManager.Play("level_menu_select");
				PlayerData.ClearSlot(this._slotSelection);
				this.slots[this._slotSelection].Init(this._slotSelection);
				this.SetState(SlotSelectScreen.State.SlotSelect);
			}
		}
		if (this.GetButtonDown(CupheadButton.Cancel))
		{
			AudioManager.Play("level_menu_select");
			this.SetState(SlotSelectScreen.State.SlotSelect);
		}
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x000F4EF0 File Offset: 0x000F30F0
	public void OnPlayerDataInitialized(bool success)
	{
		if (!success)
		{
			PlayerData.Init(new PlayerData.PlayerDataInitHandler(this.OnPlayerDataInitialized));
			return;
		}
		if (PlatformHelper.IsConsole && !PlatformHelper.PreloadSettingsData)
		{
			SettingsData.LoadFromCloud(new SettingsData.SettingsDataLoadFromCloudHandler(this.OnSettingsDataLoaded));
		}
		else
		{
			this.dataStatus = SlotSelectScreen.SaveDataStatus.Received;
		}
	}

	// Token: 0x06003376 RID: 13174 RVA: 0x0002A953 File Offset: 0x00028B53
	public void OnSettingsDataLoaded(bool success)
	{
		if (!success)
		{
			SettingsData.LoadFromCloud(new SettingsData.SettingsDataLoadFromCloudHandler(this.OnSettingsDataLoaded));
			return;
		}
		SettingsData.ApplySettingsOnStartup();
		base.StartCoroutine(this.allDataLoaded_cr());
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x000F4F48 File Offset: 0x000F3148
	public IEnumerator allDataLoaded_cr()
	{
		yield return null;
		this.SetState(SlotSelectScreen.State.MainMenu);
		for (int i = 0; i < 3; i++)
		{
			this.slots[i].Init(i);
		}
		ControllerDisconnectedPrompt.Instance.allowedToShow = true;
		this.options = this.optionsPrefab.InstantiatePrefab<OptionsGUI>();
		this.options.rectTransform.SetParent(this.optionsRoot, false);
		this.options.Init(false);
		if (PlatformHelper.ShowAchievements)
		{
			this.achievements = this.achievementsPrefab.InstantiatePrefab<AchievementsGUI>();
			this.achievements.rectTransform.SetParent(this.achievementsRoot, false);
			this.achievements.Init(false);
		}
		if (PlatformHelper.ShowDLCMenuItem)
		{
			this.dlcMenu = this.dlcMenuPrefab.InstantiatePrefab<DLCGUI>();
			this.dlcMenu.rectTransform.SetParent(this.dlcMenuRoot, false);
			this.dlcMenu.Init(false);
		}
		if (PlatformHelper.IsConsole)
		{
			PlayerManager.LoadControllerMappings(PlayerId.PlayerOne);
		}
		this.SetRichPresence();
		yield break;
	}

	// Token: 0x06003378 RID: 13176 RVA: 0x0002A97F File Offset: 0x00028B7F
	public bool GetButtonDown(CupheadButton button)
	{
		return this.input.GetButtonDown(button);
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x0002A995 File Offset: 0x00028B95
	public void SetRichPresence()
	{
		OnlineManager.Instance.Interface.SetRichPresence(PlayerId.Any, "SlotSelect", true);
	}

	// Token: 0x04002A70 RID: 10864
	public SlotSelectScreen.State state;

	// Token: 0x04002A71 RID: 10865
	[SerializeField]
	public RectTransform LoadingChild;

	// Token: 0x04002A72 RID: 10866
	[SerializeField]
	public RectTransform mainMenuChild;

	// Token: 0x04002A73 RID: 10867
	[SerializeField]
	public RectTransform slotSelectChild;

	// Token: 0x04002A74 RID: 10868
	[SerializeField]
	public RectTransform confirmDeleteChild;

	// Token: 0x04002A75 RID: 10869
	[SerializeField]
	public Text[] mainMenuItems;

	// Token: 0x04002A76 RID: 10870
	[SerializeField]
	public SlotSelectScreenSlot[] slots;

	// Token: 0x04002A77 RID: 10871
	[SerializeField]
	public Text[] confirmDeleteItems;

	// Token: 0x04002A78 RID: 10872
	[SerializeField]
	public RectTransform playerProfiles;

	// Token: 0x04002A79 RID: 10873
	[SerializeField]
	public RectTransform confirmPrompt;

	// Token: 0x04002A7A RID: 10874
	[SerializeField]
	public RectTransform confirmGlyph;

	// Token: 0x04002A7B RID: 10875
	[SerializeField]
	public RectTransform confirmSpacer;

	// Token: 0x04002A7C RID: 10876
	[SerializeField]
	public RectTransform backPrompt;

	// Token: 0x04002A7D RID: 10877
	[SerializeField]
	public RectTransform backGlyph;

	// Token: 0x04002A7E RID: 10878
	[SerializeField]
	public RectTransform backSpacer;

	// Token: 0x04002A7F RID: 10879
	[SerializeField]
	public RectTransform storePrompt;

	// Token: 0x04002A80 RID: 10880
	[SerializeField]
	public RectTransform storeGlyph;

	// Token: 0x04002A81 RID: 10881
	[SerializeField]
	public RectTransform storeSpacer;

	// Token: 0x04002A82 RID: 10882
	[SerializeField]
	public RectTransform deletePrompt;

	// Token: 0x04002A83 RID: 10883
	[SerializeField]
	public RectTransform deleteGlyph;

	// Token: 0x04002A84 RID: 10884
	[SerializeField]
	public RectTransform deleteSpacer;

	// Token: 0x04002A85 RID: 10885
	[SerializeField]
	public RectTransform prompts;

	// Token: 0x04002A86 RID: 10886
	[SerializeField]
	public Color mainMenuSelectedColor;

	// Token: 0x04002A87 RID: 10887
	[SerializeField]
	public Color mainMenuUnselectedColor;

	// Token: 0x04002A88 RID: 10888
	[SerializeField]
	public Color confirmDeleteSelectedColor;

	// Token: 0x04002A89 RID: 10889
	[SerializeField]
	public Color confirmDeleteUnselectedColor;

	// Token: 0x04002A8A RID: 10890
	[SerializeField]
	public OptionsGUI optionsPrefab;

	// Token: 0x04002A8B RID: 10891
	[SerializeField]
	public RectTransform optionsRoot;

	// Token: 0x04002A8C RID: 10892
	[SerializeField]
	public AchievementsGUI achievementsPrefab;

	// Token: 0x04002A8D RID: 10893
	[SerializeField]
	public RectTransform achievementsRoot;

	// Token: 0x04002A8E RID: 10894
	[SerializeField]
	public DLCGUI dlcMenuPrefab;

	// Token: 0x04002A8F RID: 10895
	[SerializeField]
	public RectTransform dlcMenuRoot;

	// Token: 0x04002A90 RID: 10896
	[SerializeField]
	public TMP_Text confirmDeleteSlotTitle;

	// Token: 0x04002A91 RID: 10897
	[SerializeField]
	public TMP_Text confirmDeleteSlotSeparator;

	// Token: 0x04002A92 RID: 10898
	[SerializeField]
	public TMP_Text confirmDeleteSlotPercentage;

	// Token: 0x04002A93 RID: 10899
	public OptionsGUI options;

	// Token: 0x04002A94 RID: 10900
	public AchievementsGUI achievements;

	// Token: 0x04002A95 RID: 10901
	public DLCGUI dlcMenu;

	// Token: 0x04002A96 RID: 10902
	public int _slotSelection;

	// Token: 0x04002A97 RID: 10903
	public int _mainMenuSelection;

	// Token: 0x04002A98 RID: 10904
	public SlotSelectScreen.MainMenuItem[] _availableMainMenuItems;

	// Token: 0x04002A99 RID: 10905
	public int _confirmDeleteSelection;

	// Token: 0x04002A9A RID: 10906
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04002A9B RID: 10907
	public bool isConsole;

	// Token: 0x04002A9C RID: 10908
	public const string PATH = "Audio/TitleScreenAudio";

	// Token: 0x04002A9D RID: 10909
	public float timeSinceStart;

	// Token: 0x04002A9E RID: 10910
	public SlotSelectScreen.SaveDataStatus dataStatus;

	// Token: 0x0200113C RID: 4412
	public enum State
	{
		// Token: 0x0400796D RID: 31085
		InitializeStorage,
		// Token: 0x0400796E RID: 31086
		MainMenu,
		// Token: 0x0400796F RID: 31087
		AchievementsMenu,
		// Token: 0x04007970 RID: 31088
		OptionsMenu,
		// Token: 0x04007971 RID: 31089
		DLC,
		// Token: 0x04007972 RID: 31090
		SlotSelect,
		// Token: 0x04007973 RID: 31091
		ConfirmDelete,
		// Token: 0x04007974 RID: 31092
		PlayerSelect
	}

	// Token: 0x0200113D RID: 4413
	public enum MainMenuItem
	{
		// Token: 0x04007976 RID: 31094
		Start,
		// Token: 0x04007977 RID: 31095
		Achievements,
		// Token: 0x04007978 RID: 31096
		Options,
		// Token: 0x04007979 RID: 31097
		DLC,
		// Token: 0x0400797A RID: 31098
		Exit
	}

	// Token: 0x0200113E RID: 4414
	public enum ConfirmDeleteItem
	{
		// Token: 0x0400797C RID: 31100
		Yes,
		// Token: 0x0400797D RID: 31101
		No
	}

	// Token: 0x0200113F RID: 4415
	public enum SaveDataStatus
	{
		// Token: 0x0400797F RID: 31103
		Uninitialized,
		// Token: 0x04007980 RID: 31104
		Received,
		// Token: 0x04007981 RID: 31105
		Initialized
	}
}
