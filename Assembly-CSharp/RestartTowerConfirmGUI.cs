using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F3 RID: 243
public class RestartTowerConfirmGUI : AbstractMonoBehaviour
{
	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0000A3B5 File Offset: 0x000085B5
	// (set) Token: 0x06000B75 RID: 2933 RVA: 0x0000A3BC File Offset: 0x000085BC
	public static Color COLOR_SELECTED { get; set; }

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0000A3C4 File Offset: 0x000085C4
	// (set) Token: 0x06000B77 RID: 2935 RVA: 0x0000A3CB File Offset: 0x000085CB
	public static Color COLOR_INACTIVE { get; set; }

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06000B78 RID: 2936 RVA: 0x0000A3D3 File Offset: 0x000085D3
	// (set) Token: 0x06000B79 RID: 2937 RVA: 0x0000A3DB File Offset: 0x000085DB
	public bool restartTowerConfirmMenuOpen { get; set; }

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06000B7A RID: 2938 RVA: 0x0000A3E4 File Offset: 0x000085E4
	// (set) Token: 0x06000B7B RID: 2939 RVA: 0x0000A3EC File Offset: 0x000085EC
	public bool inputEnabled { get; set; }

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0000A3F5 File Offset: 0x000085F5
	// (set) Token: 0x06000B7D RID: 2941 RVA: 0x0007FA74 File Offset: 0x0007DC74
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

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06000B7E RID: 2942 RVA: 0x0000A3FD File Offset: 0x000085FD
	// (set) Token: 0x06000B7F RID: 2943 RVA: 0x0000A405 File Offset: 0x00008605
	public bool justClosed { get; set; }

	// Token: 0x06000B80 RID: 2944 RVA: 0x0007FAFC File Offset: 0x0007DCFC
	public override void Awake()
	{
		base.Awake();
		this.restartTowerConfirmMenuOpen = false;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
		this.currentItems = new List<RestartTowerConfirmGUI.Button>(this.mainObjectButtons);
		RestartTowerConfirmGUI.COLOR_SELECTED = this.currentItems[0].text.color;
		RestartTowerConfirmGUI.COLOR_INACTIVE = this.currentItems[this.currentItems.Count - 1].text.color;
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x0000A40E File Offset: 0x0000860E
	public void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x0007FB88 File Offset: 0x0007DD88
	public void Update()
	{
		this.justClosed = false;
		if (!this.inputEnabled)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.Pause) || this.GetButtonDown(CupheadButton.Cancel))
		{
			this.MenuSelectSound();
			this.HideMenu();
			return;
		}
		if (this.GetButtonDown(CupheadButton.Accept))
		{
			this.MenuSelectSound();
			int verticalSelection = this.verticalSelection;
			if (verticalSelection != 0)
			{
				if (verticalSelection == 1)
				{
					this.ToPauseMenu();
				}
			}
			else
			{
				this.RestartTower();
			}
			return;
		}
		if (this._selectionTimer >= 0.15f)
		{
			if (this.GetButton(CupheadButton.MenuUp))
			{
				this.MenuSelectSound();
				this.verticalSelection--;
			}
			if (this.GetButton(CupheadButton.MenuDown))
			{
				this.MenuSelectSound();
				this.verticalSelection++;
			}
		}
		else
		{
			this._selectionTimer += Time.deltaTime;
		}
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x0007FC7C File Offset: 0x0007DE7C
	public void UpdateVerticalSelection()
	{
		this._selectionTimer = 0f;
		for (int i = 0; i < this.currentItems.Count; i++)
		{
			RestartTowerConfirmGUI.Button button = this.currentItems[i];
			if (i == this.verticalSelection)
			{
				button.text.color = RestartTowerConfirmGUI.COLOR_SELECTED;
			}
			else
			{
				button.text.color = RestartTowerConfirmGUI.COLOR_INACTIVE;
			}
		}
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x0000A41C File Offset: 0x0000861C
	public void ShowMenu()
	{
		this.restartTowerConfirmMenuOpen = true;
		this.verticalSelection = 0;
		this.canvasGroup.alpha = 1f;
		base.FrameDelayedCallback(new Action(this.Interactable), 1);
		this.UpdateVerticalSelection();
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x0007FCF0 File Offset: 0x0007DEF0
	public void HideMenu()
	{
		this.verticalSelection = 0;
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		this.inputEnabled = false;
		this.restartTowerConfirmMenuOpen = false;
		this.justClosed = true;
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x0000A456 File Offset: 0x00008656
	public void Interactable()
	{
		this.verticalSelection = 0;
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		this.inputEnabled = true;
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0000A47E File Offset: 0x0000867E
	public void MenuSelectSound()
	{
		AudioManager.Play("level_menu_select");
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x0000A48A File Offset: 0x0000868A
	public void ToPauseMenu()
	{
		this.restartTowerConfirmMenuOpen = false;
		this.HideMenu();
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x0000A499 File Offset: 0x00008699
	public void RestartTower()
	{
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
		PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerTwo, false);
		SceneLoader.ResetTheTowerOfPower();
		Dialoguer.EndDialogue();
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x0000A4B3 File Offset: 0x000086B3
	public bool GetButtonDown(CupheadButton button)
	{
		if (this.input.GetButtonDown(button))
		{
			AudioManager.Play("level_menu_select");
			return true;
		}
		return false;
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x0000A4D3 File Offset: 0x000086D3
	public bool GetButton(CupheadButton button)
	{
		return this.input.GetButton(button);
	}

	// Token: 0x0400091E RID: 2334
	[SerializeField]
	public GameObject mainObject;

	// Token: 0x0400091F RID: 2335
	[SerializeField]
	public RestartTowerConfirmGUI.Button[] mainObjectButtons;

	// Token: 0x04000920 RID: 2336
	public List<RestartTowerConfirmGUI.Button> currentItems;

	// Token: 0x04000921 RID: 2337
	public CanvasGroup canvasGroup;

	// Token: 0x04000922 RID: 2338
	public AbstractPauseGUI pauseMenu;

	// Token: 0x04000923 RID: 2339
	public float _selectionTimer;

	// Token: 0x04000924 RID: 2340
	public const float _SELECTION_TIME = 0.15f;

	// Token: 0x04000925 RID: 2341
	public int _verticalSelection;

	// Token: 0x04000926 RID: 2342
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04000927 RID: 2343
	public int lastIndex;

	// Token: 0x0200096D RID: 2413
	[Serializable]
	public class Button
	{
		// Token: 0x040046AB RID: 18091
		public Text text;
	}
}
