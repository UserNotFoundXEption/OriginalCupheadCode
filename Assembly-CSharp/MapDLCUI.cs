using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004CF RID: 1231
public class MapDLCUI : AbstractMonoBehaviour
{
	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x060032FE RID: 13054 RVA: 0x0002A4BF File Offset: 0x000286BF
	// (set) Token: 0x060032FF RID: 13055 RVA: 0x000F1B78 File Offset: 0x000EFD78
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

	// Token: 0x06003300 RID: 13056 RVA: 0x0002A4C7 File Offset: 0x000286C7
	public void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x06003301 RID: 13057 RVA: 0x000F1BF0 File Offset: 0x000EFDF0
	public override void Awake()
	{
		base.Awake();
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
		this.COLOR_SELECTED = this.menuItems[0].color;
		this.COLOR_INACTIVE = this.menuItems[this.menuItems.Length - 1].color;
	}

	// Token: 0x06003302 RID: 13058 RVA: 0x0002A4D5 File Offset: 0x000286D5
	public void OnDestroy()
	{
		PauseManager.Unpause();
	}

	// Token: 0x06003303 RID: 13059 RVA: 0x000F1C50 File Offset: 0x000EFE50
	public void Update()
	{
		if (!this.inputEnabled)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.Accept))
		{
			this.MenuSelectSound();
			int selection = this.selection;
			if (selection != 0)
			{
				if (selection == 1)
				{
					this.Close();
				}
			}
			else
			{
				this.ExitToTitle();
			}
			return;
		}
		if (this._selectionTimer >= 0.15f)
		{
			if (this.GetButton(CupheadButton.MenuUp))
			{
				this.MenuMoveSound();
				this.selection--;
			}
			if (this.GetButton(CupheadButton.MenuDown))
			{
				this.MenuMoveSound();
				this.selection++;
			}
		}
		else
		{
			this._selectionTimer += Time.deltaTime;
		}
	}

	// Token: 0x06003304 RID: 13060 RVA: 0x000F1D18 File Offset: 0x000EFF18
	public void UpdateVerticalSelection()
	{
		this._selectionTimer = 0f;
		for (int i = 0; i < this.menuItems.Length; i++)
		{
			Text text = this.menuItems[i];
			if (i == this.selection)
			{
				text.color = this.COLOR_SELECTED;
			}
			else
			{
				text.color = this.COLOR_INACTIVE;
			}
		}
	}

	// Token: 0x06003305 RID: 13061 RVA: 0x000F1D7C File Offset: 0x000EFF7C
	public void UpdateSelection()
	{
		this._selectionTimer = 0f;
		for (int i = 0; i < this.menuItems.Length; i++)
		{
			Text text = this.menuItems[i];
			if (i == this.selection)
			{
				text.color = this.COLOR_SELECTED;
			}
			else
			{
				text.color = this.COLOR_INACTIVE;
			}
		}
	}

	// Token: 0x06003306 RID: 13062 RVA: 0x0002A4DC File Offset: 0x000286DC
	public void ExitToTitle()
	{
		PlayerManager.ResetPlayers();
		Dialoguer.EndDialogue();
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x06003307 RID: 13063 RVA: 0x0002A4F2 File Offset: 0x000286F2
	public void Close()
	{
		this.HideMenu();
	}

	// Token: 0x06003308 RID: 13064 RVA: 0x0002A4FA File Offset: 0x000286FA
	public void ShowMenu()
	{
		this.visible = true;
		this.selection = 0;
		this.UpdateVerticalSelection();
		base.StartCoroutine(this.show_cr());
	}

	// Token: 0x06003309 RID: 13065 RVA: 0x000F1DE0 File Offset: 0x000EFFE0
	public IEnumerator show_cr()
	{
		float t = 0f;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.canvasGroup.alpha = Mathf.Lerp(0f, 1f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = 1f;
		yield return null;
		this.Interactable();
		yield break;
	}

	// Token: 0x0600330A RID: 13066 RVA: 0x0002A51D File Offset: 0x0002871D
	public void HideMenu()
	{
		base.StartCoroutine(this.hide_cr());
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		this.inputEnabled = false;
	}

	// Token: 0x0600330B RID: 13067 RVA: 0x000F1DFC File Offset: 0x000EFFFC
	public IEnumerator hide_cr()
	{
		float t = 0f;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.canvasGroup.alpha = Mathf.Lerp(1f, 0f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = 0f;
		yield return null;
		this.selection = 0;
		this.visible = false;
		yield break;
	}

	// Token: 0x0600330C RID: 13068 RVA: 0x0002A54B File Offset: 0x0002874B
	public void Interactable()
	{
		this.selection = 0;
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		this.inputEnabled = true;
	}

	// Token: 0x0600330D RID: 13069 RVA: 0x0002A573 File Offset: 0x00028773
	public bool GetButtonDown(CupheadButton button)
	{
		if (this.input.GetButtonDown(button))
		{
			AudioManager.Play("level_menu_select");
			return true;
		}
		return false;
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x0002A593 File Offset: 0x00028793
	public bool GetButton(CupheadButton button)
	{
		return this.input.GetButton(button);
	}

	// Token: 0x0600330F RID: 13071 RVA: 0x0002A5A9 File Offset: 0x000287A9
	public void MenuSelectSound()
	{
		AudioManager.Play("level_menu_select");
	}

	// Token: 0x06003310 RID: 13072 RVA: 0x0002A5B5 File Offset: 0x000287B5
	public void MenuMoveSound()
	{
		AudioManager.Play("level_menu_move");
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06003311 RID: 13073 RVA: 0x0002A5C1 File Offset: 0x000287C1
	// (set) Token: 0x06003312 RID: 13074 RVA: 0x0002A5C9 File Offset: 0x000287C9
	public bool visible { get; set; }

	// Token: 0x040029F5 RID: 10741
	[SerializeField]
	public Text[] menuItems;

	// Token: 0x040029F6 RID: 10742
	public float _selectionTimer;

	// Token: 0x040029F7 RID: 10743
	public const float _SELECTION_TIME = 0.15f;

	// Token: 0x040029F8 RID: 10744
	public int _selection;

	// Token: 0x040029F9 RID: 10745
	public Color COLOR_SELECTED;

	// Token: 0x040029FA RID: 10746
	public Color COLOR_INACTIVE;

	// Token: 0x040029FB RID: 10747
	public bool inputEnabled;

	// Token: 0x040029FC RID: 10748
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x040029FD RID: 10749
	public CanvasGroup canvasGroup;
}
