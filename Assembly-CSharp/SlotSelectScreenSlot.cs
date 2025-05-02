using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004DB RID: 1243
public class SlotSelectScreenSlot : AbstractMonoBehaviour
{
	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x0600337B RID: 13179 RVA: 0x0002A9B9 File Offset: 0x00028BB9
	// (set) Token: 0x0600337C RID: 13180 RVA: 0x0002A9C1 File Offset: 0x00028BC1
	public bool IsEmpty { get; set; }

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x0600337D RID: 13181 RVA: 0x0002A9CA File Offset: 0x00028BCA
	// (set) Token: 0x0600337E RID: 13182 RVA: 0x0002A9D2 File Offset: 0x00028BD2
	public bool isPlayer1Mugman { get; set; }

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x0600337F RID: 13183 RVA: 0x0002A9DB File Offset: 0x00028BDB
	// (set) Token: 0x06003380 RID: 13184 RVA: 0x0002A9E3 File Offset: 0x00028BE3
	public Image noise
	{
		get
		{
			return this.noiseImage;
		}
		set
		{
			this.noiseImage = value;
		}
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x000F4F64 File Offset: 0x000F3164
	public void Init(int slotNumber)
	{
		PlayerData dataForSlot = PlayerData.GetDataForSlot(slotNumber);
		this.cuphead.SetActive(false);
		this.mugman.SetActive(false);
		if (!dataForSlot.GetMapData(Scenes.scene_map_world_1).sessionStarted && !dataForSlot.IsTutorialCompleted && dataForSlot.CountLevelsCompleted(Level.world1BossLevels) == 0)
		{
			this.emptyChild.gameObject.SetActive(true);
			this.mainChild.gameObject.SetActive(false);
			this.mainDLCChild.gameObject.SetActive(false);
			this.IsEmpty = true;
			return;
		}
		this.IsEmpty = false;
		this.emptyChild.gameObject.SetActive(false);
		this.mainChild.gameObject.SetActive(true);
		Localization.Translation translation;
		if (slotNumber == 0)
		{
			if (dataForSlot.isPlayer1Mugman)
			{
				translation = Localization.Translate("TitleScreenMugmanSlot1");
			}
			else
			{
				translation = Localization.Translate("TitleScreenSlot1");
			}
		}
		else if (slotNumber == 1)
		{
			if (dataForSlot.isPlayer1Mugman)
			{
				translation = Localization.Translate("TitleScreenMugmanSlot2");
			}
			else
			{
				translation = Localization.Translate("TitleScreenSlot2");
			}
		}
		else if (dataForSlot.isPlayer1Mugman)
		{
			translation = Localization.Translate("TitleScreenMugmanSlot3");
		}
		else
		{
			translation = Localization.Translate("TitleScreenSlot3");
		}
		this.slotTitle.text = translation.text;
		this.slotSeparator.font = translation.fonts.fontAsset;
		this.slotTitle.font = translation.fonts.fontAsset;
		this.isPlayer1Mugman = dataForSlot.isPlayer1Mugman;
		this.isExpert = dataForSlot.IsHardModeAvailable;
		this.isExpertDLC = dataForSlot.IsHardModeAvailableDLC;
		int num = Mathf.RoundToInt(dataForSlot.GetCompletionPercentage());
		this.isComplete = (num == 200);
		int num2 = Mathf.RoundToInt(dataForSlot.GetCompletionPercentageDLC());
		this.isCompleteDLC = (num2 == 100);
		this.slotPercentage.text = num + num2 + "%";
		if (DLCManager.DLCEnabled())
		{
			this.slotPercentageSelectedBase.text = num + "%";
			this.slotPercentageSelectedDLC.text = num2 + "%";
		}
		Scenes currentMap = dataForSlot.CurrentMap;
		switch (currentMap)
		{
		case Scenes.scene_map_world_2:
			translation = Localization.Translate("TitleScreenWorld2");
			break;
		case Scenes.scene_map_world_3:
			translation = Localization.Translate("TitleScreenWorld3");
			break;
		case Scenes.scene_map_world_4:
			translation = Localization.Translate("TitleScreenWorld4");
			break;
		default:
			if (currentMap != Scenes.scene_map_world_DLC)
			{
				translation = Localization.Translate("TitleScreenWorld1");
			}
			else if (DLCManager.DLCEnabled())
			{
				translation = Localization.Translate("TitleScreenWorldDLC");
			}
			else
			{
				translation = Localization.Translate("TitleScreenWorld1");
			}
			break;
		}
		this.worldMapText.text = translation.text;
		this.worldMapText.font = translation.fonts.fontAsset;
		this.worldMapTextDLC.text = translation.text;
		this.worldMapTextDLC.font = translation.fonts.fontAsset;
	}

	// Token: 0x06003382 RID: 13186 RVA: 0x000F5284 File Offset: 0x000F3484
	public void SetSelected(bool selected)
	{
		if (DLCManager.DLCEnabled() && !this.IsEmpty)
		{
			this.mainChild.gameObject.SetActive(!selected);
			this.mainDLCChild.gameObject.SetActive(selected);
		}
		this.slotTitle.color = ((!selected) ? this.unselectedTextColor : this.selectedTextColor);
		this.slotSeparator.color = ((!selected) ? this.unselectedTextColor : this.selectedTextColor);
		this.slotPercentage.color = ((!selected) ? this.unselectedTextColor : this.selectedTextColor);
		this.worldMapText.color = ((!selected) ? this.unselectedTextColor : this.selectedTextColor);
		this.emptyText.color = ((!selected) ? this.unselectedTextColor : this.selectedTextColor);
		this.boxImage.sprite = ((!selected) ? this.unselectedBoxSprite : ((!this.isPlayer1Mugman) ? this.selectedBoxSprite : this.selectedBoxSpriteMugman));
		if (!this.IsEmpty && this.isComplete)
		{
			this.starImage.sprite = ((!selected) ? this.unselectedBoxSpriteComplete : this.selectedBoxSpriteComplete);
			this.starImage.gameObject.SetActive(true);
		}
		else if (!this.IsEmpty && this.isExpert)
		{
			this.starImage.sprite = ((!selected) ? this.unselectedBoxSpriteExpert : this.selectedBoxSpriteExpert);
			this.starImage.gameObject.SetActive(true);
		}
		else
		{
			this.starImage.gameObject.SetActive(false);
		}
		if (!this.IsEmpty && this.isCompleteDLC)
		{
			this.starImageDLC.sprite = ((!selected) ? this.unselectedBoxSpriteCompleteDLC : this.selectedBoxSpriteCompleteDLC);
			this.starImageDLC.gameObject.SetActive(true);
		}
		else if (!this.IsEmpty && this.isExpertDLC)
		{
			this.starImageDLC.sprite = ((!selected) ? this.unselectedBoxSpriteExpertDLC : this.selectedBoxSpriteExpertDLC);
			this.starImageDLC.gameObject.SetActive(true);
		}
		else
		{
			this.starImageDLC.gameObject.SetActive(false);
		}
		if (this.starImage.gameObject.activeInHierarchy && !this.starImageDLC.gameObject.activeInHierarchy)
		{
			this.starImage.transform.position = this.starImageDLC.transform.position;
		}
		this.noiseImage.sprite = ((!selected) ? this.unselectedNoise : ((!this.isPlayer1Mugman) ? this.selectedNoise : this.selectedNoiseMugman));
	}

	// Token: 0x06003383 RID: 13187 RVA: 0x0002A9EC File Offset: 0x00028BEC
	public string GetSlotTitle()
	{
		return this.slotTitle.text;
	}

	// Token: 0x06003384 RID: 13188 RVA: 0x0002A9F9 File Offset: 0x00028BF9
	public TMP_FontAsset GetSlotTitleFont()
	{
		return this.slotTitle.font;
	}

	// Token: 0x06003385 RID: 13189 RVA: 0x0002AA06 File Offset: 0x00028C06
	public string GetSlotSeparator()
	{
		return this.slotSeparator.text;
	}

	// Token: 0x06003386 RID: 13190 RVA: 0x0002AA13 File Offset: 0x00028C13
	public TMP_FontAsset GetSlotSeparatorFont()
	{
		return this.slotSeparator.font;
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x0002AA20 File Offset: 0x00028C20
	public string GetSlotPercentage()
	{
		return this.slotPercentage.text;
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x0002AA2D File Offset: 0x00028C2D
	public TMP_FontAsset GetSlotPercentageFont()
	{
		return this.slotPercentage.font;
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x000F558C File Offset: 0x000F378C
	public void EnterSelectMenu()
	{
		this.selectingMugman = this.isPlayer1Mugman;
		if (this.selectingMugman)
		{
			this.mugman.SetActive(true);
			this.mugmanSelect.Play("Zoom_In");
		}
		else
		{
			this.cuphead.SetActive(true);
			this.cupheadSelect.Play("Zoom_In");
		}
		this.mainDLCChild.gameObject.SetActive(false);
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x000F5600 File Offset: 0x000F3800
	public void SwapSprite()
	{
		this.noiseImage.enabled = false;
		this.selectingMugman = !this.selectingMugman;
		this.cuphead.SetActive(!this.selectingMugman);
		this.mugman.SetActive(this.selectingMugman);
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x0002AA3A File Offset: 0x00028C3A
	public void StopSelectingPlayer()
	{
		base.StartCoroutine(this.player_zoomout_cr());
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x000F5650 File Offset: 0x000F3850
	public IEnumerator player_zoomout_cr()
	{
		if (this.selectingMugman)
		{
			this.mugmanSelect.Play("Zoom_Out");
			yield return this.mugmanSelect.WaitForAnimationToEnd(this, "Zoom_Out", false, true);
			this.mugman.SetActive(false);
		}
		else
		{
			this.cupheadSelect.Play("Zoom_Out");
			yield return this.cupheadSelect.WaitForAnimationToEnd(this, "Zoom_Out", false, true);
			this.cuphead.SetActive(false);
		}
		yield return null;
		this.selectingMugman = this.isPlayer1Mugman;
		this.noiseImage.enabled = true;
		yield break;
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x000F566C File Offset: 0x000F386C
	public void PlayAnimation(int slotNumber)
	{
		this.isPlayer1Mugman = this.selectingMugman;
		PlayerData dataForSlot = PlayerData.GetDataForSlot(slotNumber);
		Animator animator = (!this.isPlayer1Mugman) ? this.cupheadAnimator : this.mugmanAnimator;
		if (dataForSlot.IsHardModeAvailable)
		{
			if (dataForSlot.NumCoinsCollected >= 40 && dataForSlot.NumSupers(PlayerId.PlayerOne) >= 3)
			{
				animator.Play("100Percent");
			}
			else
			{
				animator.Play("DefeatedDevil");
			}
		}
		else
		{
			animator.Play("Default");
		}
	}

	// Token: 0x04002A9F RID: 10911
	[SerializeField]
	public RectTransform emptyChild;

	// Token: 0x04002AA0 RID: 10912
	[SerializeField]
	public RectTransform mainChild;

	// Token: 0x04002AA1 RID: 10913
	[SerializeField]
	public RectTransform mainDLCChild;

	// Token: 0x04002AA2 RID: 10914
	[SerializeField]
	public TMP_Text worldMapText;

	// Token: 0x04002AA3 RID: 10915
	[SerializeField]
	public TMP_Text worldMapTextDLC;

	// Token: 0x04002AA4 RID: 10916
	[SerializeField]
	public Image boxImage;

	// Token: 0x04002AA5 RID: 10917
	[SerializeField]
	public Image starImage;

	// Token: 0x04002AA6 RID: 10918
	[SerializeField]
	public Image starImageDLC;

	// Token: 0x04002AA7 RID: 10919
	[SerializeField]
	public Image starImageSelectedBase;

	// Token: 0x04002AA8 RID: 10920
	[SerializeField]
	public Image starImageSelectedDLC;

	// Token: 0x04002AA9 RID: 10921
	[SerializeField]
	public Image noiseImage;

	// Token: 0x04002AAA RID: 10922
	[SerializeField]
	public Sprite unselectedBoxSprite;

	// Token: 0x04002AAB RID: 10923
	[SerializeField]
	public Sprite unselectedBoxSpriteExpert;

	// Token: 0x04002AAC RID: 10924
	[SerializeField]
	public Sprite unselectedBoxSpriteComplete;

	// Token: 0x04002AAD RID: 10925
	[SerializeField]
	public Sprite unselectedBoxSpriteExpertDLC;

	// Token: 0x04002AAE RID: 10926
	[SerializeField]
	public Sprite unselectedBoxSpriteCompleteDLC;

	// Token: 0x04002AAF RID: 10927
	[SerializeField]
	public Sprite unselectedNoise;

	// Token: 0x04002AB0 RID: 10928
	[SerializeField]
	public Sprite selectedBoxSpriteMugman;

	// Token: 0x04002AB1 RID: 10929
	[SerializeField]
	public Sprite selectedBoxSprite;

	// Token: 0x04002AB2 RID: 10930
	[SerializeField]
	public Sprite selectedBoxSpriteExpert;

	// Token: 0x04002AB3 RID: 10931
	[SerializeField]
	public Sprite selectedBoxSpriteComplete;

	// Token: 0x04002AB4 RID: 10932
	[SerializeField]
	public Sprite selectedBoxSpriteExpertDLC;

	// Token: 0x04002AB5 RID: 10933
	[SerializeField]
	public Sprite selectedBoxSpriteCompleteDLC;

	// Token: 0x04002AB6 RID: 10934
	[SerializeField]
	public Sprite selectedNoiseMugman;

	// Token: 0x04002AB7 RID: 10935
	[SerializeField]
	public Sprite selectedNoise;

	// Token: 0x04002AB8 RID: 10936
	[SerializeField]
	public GameObject cuphead;

	// Token: 0x04002AB9 RID: 10937
	[SerializeField]
	public Animator cupheadSelect;

	// Token: 0x04002ABA RID: 10938
	[SerializeField]
	public Animator cupheadAnimator;

	// Token: 0x04002ABB RID: 10939
	[SerializeField]
	public GameObject mugman;

	// Token: 0x04002ABC RID: 10940
	[SerializeField]
	public Animator mugmanSelect;

	// Token: 0x04002ABD RID: 10941
	[SerializeField]
	public Animator mugmanAnimator;

	// Token: 0x04002ABE RID: 10942
	[SerializeField]
	public TMP_Text slotTitle;

	// Token: 0x04002ABF RID: 10943
	[SerializeField]
	public TMP_Text slotSeparator;

	// Token: 0x04002AC0 RID: 10944
	[SerializeField]
	public TMP_Text slotPercentage;

	// Token: 0x04002AC1 RID: 10945
	[SerializeField]
	public TMP_Text slotPercentageSelectedBase;

	// Token: 0x04002AC2 RID: 10946
	[SerializeField]
	public TMP_Text slotPercentageSelectedDLC;

	// Token: 0x04002AC3 RID: 10947
	[SerializeField]
	public Text emptyText;

	// Token: 0x04002AC4 RID: 10948
	[SerializeField]
	public Color selectedTextColor;

	// Token: 0x04002AC5 RID: 10949
	[SerializeField]
	public Color unselectedTextColor;

	// Token: 0x04002AC8 RID: 10952
	public bool selectingMugman;

	// Token: 0x04002AC9 RID: 10953
	public bool isExpert;

	// Token: 0x04002ACA RID: 10954
	public bool isExpertDLC;

	// Token: 0x04002ACB RID: 10955
	public bool isComplete;

	// Token: 0x04002ACC RID: 10956
	public bool isCompleteDLC;
}
