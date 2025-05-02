using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000104 RID: 260
public class LevelPauseGUI : AbstractPauseGUI
{
	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000C14 RID: 3092 RVA: 0x0000A9FD File Offset: 0x00008BFD
	// (set) Token: 0x06000C15 RID: 3093 RVA: 0x0000AA04 File Offset: 0x00008C04
	public static Color COLOR_SELECTED { get; set; }

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000C16 RID: 3094 RVA: 0x0000AA0C File Offset: 0x00008C0C
	// (set) Token: 0x06000C17 RID: 3095 RVA: 0x0000AA13 File Offset: 0x00008C13
	public static Color COLOR_INACTIVE { get; set; }

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06000C18 RID: 3096 RVA: 0x000824AC File Offset: 0x000806AC
	// (remove) Token: 0x06000C19 RID: 3097 RVA: 0x000824E0 File Offset: 0x000806E0
	public static event Action OnPauseEvent;

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x06000C1A RID: 3098 RVA: 0x00082514 File Offset: 0x00080714
	// (remove) Token: 0x06000C1B RID: 3099 RVA: 0x00082548 File Offset: 0x00080748
	public static event Action OnUnpauseEvent;

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x06000C1C RID: 3100 RVA: 0x0000AA1B File Offset: 0x00008C1B
	// (set) Token: 0x06000C1D RID: 3101 RVA: 0x0008257C File Offset: 0x0008077C
	public int selection
	{
		get
		{
			return this._selection;
		}
		set
		{
			bool flag = value > this._selection;
			int num = (int)Mathf.Repeat((float)value, (float)this.menuItems.Length);
			while (!this.menuItems[num].gameObject.activeSelf)
			{
				num = ((!flag) ? (num - 1) : (num + 1));
				num = (int)Mathf.Repeat((float)num, (float)this.menuItems.Length);
			}
			this._selection = num;
			this.UpdateSelection();
		}
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x06000C1E RID: 3102 RVA: 0x000825F4 File Offset: 0x000807F4
	public override bool CanPause
	{
		get
		{
			return Level.Current.Started && !Level.Current.Ending && PauseManager.state != PauseManager.State.Paused && !SceneLoader.CurrentlyLoading && !this.forceDisablePause;
		}
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x0000AA23 File Offset: 0x00008C23
	public void OnEnable()
	{
		Localization.OnLanguageChangedEvent += this.onLanguageChangedEventHandler;
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x0000AA36 File Offset: 0x00008C36
	public void OnDisable()
	{
		Localization.OnLanguageChangedEvent -= this.onLanguageChangedEventHandler;
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x0000AA49 File Offset: 0x00008C49
	public override void Awake()
	{
		base.Awake();
		LevelPauseGUI.COLOR_SELECTED = this.menuItems[0].color;
		LevelPauseGUI.COLOR_INACTIVE = this.menuItems[this.menuItems.Length - 1].color;
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x0000AA7E File Offset: 0x00008C7E
	public override void Init(bool checkIfDead, OptionsGUI options, AchievementsGUI achievements)
	{
		this.Init(checkIfDead, options, achievements, null);
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x00082640 File Offset: 0x00080840
	public override void Init(bool checkIfDead, OptionsGUI options, AchievementsGUI achievements, RestartTowerConfirmGUI restartTowerConfirm)
	{
		base.Init(checkIfDead, options, achievements);
		this.options = options;
		this.achievements = achievements;
		this.restartTowerConfirm = restartTowerConfirm;
		if (PlatformHelper.IsConsole && this.menuItems.Length > 7)
		{
			this.menuItems[7].gameObject.SetActive(false);
		}
		if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane)
		{
			this.menuItems[2].gameObject.SetActive(true);
			this.updateRotateControlsToggleVisualValue();
		}
		else if (!PlatformHelper.ShowAchievements && this.menuItems.Length > 2)
		{
			this.menuItems[2].gameObject.SetActive(false);
		}
		if (Level.IsTowerOfPower)
		{
			this.ReplaceRestartWRestartTowerOfPower();
		}
		options.Init(checkIfDead);
		if (achievements != null)
		{
			achievements.Init(checkIfDead);
		}
		if (restartTowerConfirm != null)
		{
			restartTowerConfirm.Init(checkIfDead);
		}
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x0000AA8A File Offset: 0x00008C8A
	public void ForceDisablePause(bool value)
	{
		this.forceDisablePause = value;
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x00082744 File Offset: 0x00080944
	public override void OnPause()
	{
		base.OnPause();
		if (CupheadLevelCamera.Current != null)
		{
			CupheadLevelCamera.Current.StartBlur();
		}
		else
		{
			CupheadMapCamera.Current.StartBlur();
		}
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, PlatformHelper.CanSwitchUserFromPause);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, PlatformHelper.CanSwitchUserFromPause);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		this.menuItems[4].gameObject.SetActive(PlayerManager.Multiplayer);
		if (LevelPauseGUI.OnPauseEvent != null)
		{
			LevelPauseGUI.OnPauseEvent();
		}
		this.selection = 0;
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x000827D0 File Offset: 0x000809D0
	public override void OnUnpause()
	{
		base.OnUnpause();
		if (CupheadLevelCamera.Current != null)
		{
			CupheadLevelCamera.Current.EndBlur();
		}
		else
		{
			CupheadMapCamera.Current.EndBlur();
		}
		if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane)
		{
			SettingsData.Save();
			if (PlatformHelper.IsConsole)
			{
				SettingsData.SaveToCloud();
			}
		}
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		if (LevelPauseGUI.OnUnpauseEvent != null)
		{
			LevelPauseGUI.OnUnpauseEvent();
		}
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x0000AA93 File Offset: 0x00008C93
	public void OnDestroy()
	{
		PauseManager.Unpause();
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x00082870 File Offset: 0x00080A70
	public override void Update()
	{
		base.Update();
		if (base.state != AbstractPauseGUI.State.Paused || this.options.optionMenuOpen || this.options.justClosed || (this.achievements != null && (this.achievements.achievementsMenuOpen || this.achievements.justClosed)) || (this.restartTowerConfirm != null && (this.restartTowerConfirm.restartTowerConfirmMenuOpen || this.restartTowerConfirm.justClosed)))
		{
			return;
		}
		if (base.GetButtonDown(CupheadButton.Pause) || base.GetButtonDown(CupheadButton.Cancel))
		{
			this.Unpause();
			return;
		}
		if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane && this.selection == 2 && (base.GetButtonDown(CupheadButton.Accept) || base.GetButtonDown(CupheadButton.MenuLeft) || base.GetButtonDown(CupheadButton.MenuRight)))
		{
			base.MenuSelectSound();
			this.ToggleRotateControls();
			return;
		}
		if (base.GetButtonDown(CupheadButton.Accept))
		{
			base.MenuSelectSound();
			this.Select();
			return;
		}
		if (this._selectionTimer >= 0.15f)
		{
			if (base.GetButton(CupheadButton.MenuUp))
			{
				base.MenuMoveSound();
				this.selection--;
			}
			if (base.GetButton(CupheadButton.MenuDown))
			{
				base.MenuMoveSound();
				this.selection++;
			}
		}
		else
		{
			this._selectionTimer += Time.deltaTime;
		}
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x00082A1C File Offset: 0x00080C1C
	public void Select()
	{
		switch (this.selection)
		{
		case 0:
			this.Unpause();
			break;
		case 1:
			this.Restart();
			break;
		case 2:
			this.Achievements();
			break;
		case 3:
			this.Options();
			break;
		case 4:
			this.Player2Leave();
			break;
		case 5:
			this.Exit();
			break;
		case 6:
			this.ExitToTitle();
			break;
		case 7:
			this.ExitToDesktop();
			break;
		}
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x0000AA9A File Offset: 0x00008C9A
	public override void OnUnpauseSound()
	{
		base.OnUnpauseSound();
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x00082AB4 File Offset: 0x00080CB4
	public void UpdateSelection()
	{
		this._selectionTimer = 0f;
		for (int i = 0; i < this.menuItems.Length; i++)
		{
			Text text = this.menuItems[i];
			if (i == this.selection)
			{
				text.color = LevelPauseGUI.COLOR_SELECTED;
			}
			else
			{
				text.color = LevelPauseGUI.COLOR_INACTIVE;
			}
		}
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x00082B18 File Offset: 0x00080D18
	public void Restart()
	{
		if (Level.IsTowerOfPower)
		{
			this.RestartTowerConfirm();
		}
		else
		{
			this.OnUnpauseSound();
			base.state = AbstractPauseGUI.State.Animating;
			PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
			PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
			SceneLoader.ReloadLevel();
			Dialoguer.EndDialogue();
			if (Level.IsDicePalaceMain || Level.IsDicePalace)
			{
				DicePalaceMainLevelGameInfo.CleanUpRetry();
			}
		}
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x0000AAA2 File Offset: 0x00008CA2
	public void ReplaceRestartWRestartTowerOfPower()
	{
		this.retryLocHelper.currentID = Localization.Find("OptionMenuRestartTower").id;
		this.retryLocHelper.ApplyTranslation();
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x0000AAC9 File Offset: 0x00008CC9
	public void Exit()
	{
		base.state = AbstractPauseGUI.State.Animating;
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
		Dialoguer.EndDialogue();
		if (Level.IsDicePalaceMain || Level.IsDicePalace)
		{
			DicePalaceMainLevelGameInfo.CleanUpRetry();
		}
		SceneLoader.LoadLastMap();
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x0000AB03 File Offset: 0x00008D03
	public void Player2Leave()
	{
		PlayerManager.PlayerLeave(PlayerId.PlayerTwo);
		this.Unpause();
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x0000AB11 File Offset: 0x00008D11
	public void ExitToTitle()
	{
		base.state = AbstractPauseGUI.State.Animating;
		PlayerManager.ResetPlayers();
		Dialoguer.EndDialogue();
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x0000AB2E File Offset: 0x00008D2E
	public void ExitToDesktop()
	{
		Dialoguer.EndDialogue();
		Application.Quit();
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x0000AB3A File Offset: 0x00008D3A
	public void Options()
	{
		base.StartCoroutine(this.in_options_cr());
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x00082B78 File Offset: 0x00080D78
	public IEnumerator in_options_cr()
	{
		this.HideImmediate();
		this.options.ShowMainOptionMenu();
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
		while (this.options.optionMenuOpen)
		{
			yield return null;
		}
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, PlatformHelper.CanSwitchUserFromPause);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, PlatformHelper.CanSwitchUserFromPause);
		this.selection = 0;
		this.ShowImmediate();
		yield return null;
		yield break;
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x0000AB49 File Offset: 0x00008D49
	public void RestartTowerConfirm()
	{
		base.StartCoroutine(this.in_restarttowerconfirm_cr());
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x00082B94 File Offset: 0x00080D94
	public IEnumerator in_restarttowerconfirm_cr()
	{
		this.HideImmediate();
		this.restartTowerConfirm.ShowMenu();
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
		while (this.restartTowerConfirm.restartTowerConfirmMenuOpen)
		{
			yield return null;
		}
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, PlatformHelper.CanSwitchUserFromPause);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, PlatformHelper.CanSwitchUserFromPause);
		this.selection = 0;
		this.ShowImmediate();
		yield return null;
		yield break;
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x0000AB58 File Offset: 0x00008D58
	public void Achievements()
	{
		base.StartCoroutine(this.in_achievements_cr());
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x00082BB0 File Offset: 0x00080DB0
	public IEnumerator in_achievements_cr()
	{
		this.HideImmediate();
		this.achievements.ShowAchievements();
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
		while (this.achievements.achievementsMenuOpen)
		{
			yield return null;
		}
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, PlatformHelper.CanSwitchUserFromPause);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, PlatformHelper.CanSwitchUserFromPause);
		this.selection = 0;
		this.ShowImmediate();
		yield return null;
		yield break;
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0000AB67 File Offset: 0x00008D67
	public void ToggleRotateControls()
	{
		SettingsData.Data.rotateControlsWithCamera = !SettingsData.Data.rotateControlsWithCamera;
		this.updateRotateControlsToggleVisualValue();
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x00082BCC File Offset: 0x00080DCC
	public void updateRotateControlsToggleVisualValue()
	{
		Text text = this.menuItems[2];
		text.GetComponent<LocalizationHelper>().ApplyTranslation(Localization.Find("CameraRotationControl"), null);
		text.text = string.Format(text.text, (!SettingsData.Data.rotateControlsWithCamera) ? "A" : "B");
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x0000AB86 File Offset: 0x00008D86
	public void onLanguageChangedEventHandler()
	{
		if (Level.Current != null && Level.Current.CurrentLevel == Levels.Airplane)
		{
			base.StartCoroutine(this.changeRotationToggleLanguage_cr());
		}
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x00082C28 File Offset: 0x00080E28
	public IEnumerator changeRotationToggleLanguage_cr()
	{
		yield return null;
		yield return null;
		yield return null;
		this.updateRotateControlsToggleVisualValue();
		yield break;
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x0000ABB9 File Offset: 0x00008DB9
	public override void InAnimation(float i)
	{
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x0000ABBB File Offset: 0x00008DBB
	public override void OutAnimation(float i)
	{
	}

	// Token: 0x040009B2 RID: 2482
	[SerializeField]
	public Text[] menuItems;

	// Token: 0x040009B3 RID: 2483
	public OptionsGUI options;

	// Token: 0x040009B4 RID: 2484
	public AchievementsGUI achievements;

	// Token: 0x040009B5 RID: 2485
	public RestartTowerConfirmGUI restartTowerConfirm;

	// Token: 0x040009B6 RID: 2486
	public float _selectionTimer;

	// Token: 0x040009B7 RID: 2487
	public const float _SELECTION_TIME = 0.15f;

	// Token: 0x040009B8 RID: 2488
	[SerializeField]
	public LocalizationHelper retryLocHelper;

	// Token: 0x040009B9 RID: 2489
	public int _selection;

	// Token: 0x040009BA RID: 2490
	public bool forceDisablePause;

	// Token: 0x02000981 RID: 2433
	public enum MenuItems
	{
		// Token: 0x04004712 RID: 18194
		Unpause,
		// Token: 0x04004713 RID: 18195
		Restart,
		// Token: 0x04004714 RID: 18196
		Achievements,
		// Token: 0x04004715 RID: 18197
		Options,
		// Token: 0x04004716 RID: 18198
		Player2Leave,
		// Token: 0x04004717 RID: 18199
		ExitToMap,
		// Token: 0x04004718 RID: 18200
		ExitToTitle,
		// Token: 0x04004719 RID: 18201
		ExitToDesktop
	}
}
