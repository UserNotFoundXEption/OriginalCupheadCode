using System;
using System.Collections;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200063D RID: 1597
public class CustomButtonPlayerSelect : CustomButton
{
	// Token: 0x06004284 RID: 17028 RVA: 0x00035568 File Offset: 0x00033768
	public override void Start()
	{
		base.Start();
		if (!PlayerManager.Multiplayer)
		{
			base.interactable = false;
		}
		base.StartCoroutine(this.update_cr());
	}

	// Token: 0x06004285 RID: 17029 RVA: 0x00136EA4 File Offset: 0x001350A4
	public IEnumerator update_cr()
	{
		while (!this.mapper)
		{
			yield return null;
		}
		for (int i = 0; i < this.selectionTabs.Length; i++)
		{
			this.selectionTabs[i].rectTransform.anchoredPosition = new Vector3((this.associatedText.preferredWidth / 2f + 15f) * (float)(i * 2 - 1), this.selectionTabs[i].rectTransform.anchoredPosition.y, 0f);
		}
		for (;;)
		{
			if (this.myInfo.intData == this.mapper.currentPlayerId)
			{
				this.associatedText.color = base.colors.highlightedColor;
			}
			else
			{
				this.associatedText.color = ((base.currentSelectionState != 1) ? base.colors.normalColor : base.colors.highlightedColor);
			}
			for (int j = 0; j < this.selectionTabs.Length; j++)
			{
				this.selectionTabs[j].enabled = (this.myInfo.intData == this.mapper.currentPlayerId);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004286 RID: 17030 RVA: 0x0003558E File Offset: 0x0003378E
	public override void OnSelect(BaseEventData eventData)
	{
		if (this.IsInteractable())
		{
			base.OnSelect(eventData);
		}
		else
		{
			base.StartCoroutine(this.move_selection_cr());
		}
	}

	// Token: 0x06004287 RID: 17031 RVA: 0x00136EC0 File Offset: 0x001350C0
	public IEnumerator move_selection_cr()
	{
		yield return new WaitForEndOfFrame();
		EventSystem.current.SetSelectedGameObject(this.FindSelectableOnUp().gameObject);
		yield break;
	}

	// Token: 0x06004288 RID: 17032 RVA: 0x00136EDC File Offset: 0x001350DC
	public override void DoStateTransition(Selectable.SelectionState state, bool instant)
	{
		if (this.mapper && this.myInfo.intData == this.mapper.currentPlayerId)
		{
			return;
		}
		switch (state)
		{
		case 0:
			this.associatedText.color = base.colors.normalColor;
			break;
		case 1:
			this.associatedText.color = base.colors.highlightedColor;
			break;
		case 2:
			this.associatedText.color = base.colors.pressedColor;
			break;
		case 3:
			this.associatedText.color = base.colors.disabledColor;
			break;
		}
	}

	// Token: 0x0400344D RID: 13389
	public ControlMapper mapper;

	// Token: 0x0400344E RID: 13390
	[SerializeField]
	public ButtonInfo myInfo;

	// Token: 0x0400344F RID: 13391
	[SerializeField]
	public Image[] selectionTabs;
}
