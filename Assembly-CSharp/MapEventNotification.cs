using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004D1 RID: 1233
public class MapEventNotification : AbstractMonoBehaviour
{
	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06003314 RID: 13076 RVA: 0x0002A5D2 File Offset: 0x000287D2
	// (set) Token: 0x06003315 RID: 13077 RVA: 0x0002A5D9 File Offset: 0x000287D9
	public static MapEventNotification Current { get; set; }

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06003316 RID: 13078 RVA: 0x0002A5E1 File Offset: 0x000287E1
	// (set) Token: 0x06003317 RID: 13079 RVA: 0x0002A5E9 File Offset: 0x000287E9
	public bool showing { get; set; }

	// Token: 0x06003318 RID: 13080 RVA: 0x000F1EA0 File Offset: 0x000F00A0
	public override void Awake()
	{
		base.Awake();
		MapEventNotification.Current = this;
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
		for (int i = 0; i < this.sparkleAnimatorsContract.Length; i++)
		{
			this.sparkleAnimatorsContract[i] = Object.Instantiate<GameObject>(this.sparklePrefab, this.sparkleTransformContract).GetComponent<Animator>();
		}
		for (int j = 0; j < this.sparkleAnimatorsCoin1.Length; j++)
		{
			this.sparkleAnimatorsCoin1[j] = Object.Instantiate<GameObject>(this.sparklePrefab, this.sparkleTransformCoin1).GetComponent<Animator>();
		}
		for (int k = 0; k < this.sparkleAnimatorsCoin2.Length; k++)
		{
			this.sparkleAnimatorsCoin2[k] = Object.Instantiate<GameObject>(this.sparklePrefab, this.sparkleTransformCoin2).GetComponent<Animator>();
		}
		for (int l = 0; l < this.sparkleAnimatorsCoin3.Length; l++)
		{
			this.sparkleAnimatorsCoin3[l] = Object.Instantiate<GameObject>(this.sparklePrefab, this.sparkleTransformCoin3).GetComponent<Animator>();
		}
		this.dlcUI = Object.Instantiate<GameObject>(this.dlcUIPrefab, this.dlcUIRoot).GetComponent<MapDLCUI>();
		this.dlcUI.Init(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x0002A5F2 File Offset: 0x000287F2
	public void OnDestroy()
	{
		if (MapEventNotification.Current == this)
		{
			MapEventNotification.Current = null;
		}
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x000F1FF4 File Offset: 0x000F01F4
	public void Update()
	{
		if (this.superShowing)
		{
			if (this.input.GetAnyButtonDown())
			{
				base.StartCoroutine(this.tweenOut_cr(1.5f));
				base.animator.SetTrigger("hide_super");
				this.superShowing = false;
			}
			this.timeBeforeNextSparkleCoin1 -= CupheadTime.Delta;
			for (int i = 0; i < this.sparkleAnimatorsCoin1.Length; i++)
			{
				if (this.timeBeforeNextSparkleCoin1 <= 0f)
				{
					if (this.sparkleAnimatorsCoin1[i].GetCurrentAnimatorStateInfo(0).IsName("Empty"))
					{
						this.timeBeforeNextSparkleCoin1 = this.timeBetweenSparkle;
						this.sparkleAnimatorsCoin1[i].transform.position = new Vector3(this.sparkleTransformCoin1.position.x + Random.Range(this.sparkleTransformCoin1.sizeDelta.x * -0.5f, this.sparkleTransformCoin1.sizeDelta.x * 0.5f), this.sparkleTransformCoin1.position.y + Random.Range(this.sparkleTransformCoin1.sizeDelta.y * -0.5f, this.sparkleTransformCoin1.sizeDelta.y * 0.5f), 101f);
						this.sparkleAnimatorsCoin1[i].SetTrigger(Random.Range(0, 4).ToStringInvariant());
					}
				}
			}
		}
		if (this.tooltipShowing && this.input.GetAnyButtonDown())
		{
			base.StartCoroutine(this.tweenOut_cr(1.5f));
			base.animator.SetTrigger("hide_tooltip");
			this.tooltipShowing = false;
		}
		if (this.tooltipEquipShowing && this.input.GetButtonDown(CupheadButton.EquipMenu))
		{
			base.StartCoroutine(this.tweenOut_cr(0.5f));
			base.animator.SetTrigger("hide_tooltip");
			this.tooltipShowing = false;
		}
		if (this.coinShowing)
		{
			if (this.input.GetAnyButtonDown())
			{
				base.StartCoroutine(this.tweenOut_cr(1.5f));
				base.animator.SetTrigger("hide_coin");
				this.coinShowing = false;
			}
			this.timeBeforeNextSparkleCoin1 -= CupheadTime.Delta;
			this.timeBeforeNextSparkleCoin2 -= CupheadTime.Delta;
			this.timeBeforeNextSparkleCoin3 -= CupheadTime.Delta;
			for (int j = 0; j < this.sparkleAnimatorsCoin1.Length; j++)
			{
				if (this.timeBeforeNextSparkleCoin1 <= 0f)
				{
					if (this.sparkleAnimatorsCoin1[j].GetCurrentAnimatorStateInfo(0).IsName("Empty"))
					{
						this.timeBeforeNextSparkleCoin1 = this.timeBetweenSparkle;
						this.sparkleAnimatorsCoin1[j].transform.position = new Vector3(this.sparkleTransformCoin1.position.x + Random.Range(this.sparkleTransformCoin1.sizeDelta.x * -0.5f, this.sparkleTransformCoin1.sizeDelta.x * 0.5f), this.sparkleTransformCoin1.position.y + Random.Range(this.sparkleTransformCoin1.sizeDelta.y * -0.5f, this.sparkleTransformCoin1.sizeDelta.y * 0.5f), 101f);
						this.sparkleAnimatorsCoin1[j].SetTrigger(Random.Range(0, 4).ToStringInvariant());
					}
				}
			}
			for (int k = 0; k < this.sparkleAnimatorsCoin2.Length; k++)
			{
				if (this.timeBeforeNextSparkleCoin2 <= 0f)
				{
					if (this.sparkleAnimatorsCoin2[k].GetCurrentAnimatorStateInfo(0).IsName("Empty"))
					{
						this.timeBeforeNextSparkleCoin2 = this.timeBetweenSparkle;
						this.sparkleAnimatorsCoin2[k].transform.position = new Vector3(this.sparkleTransformCoin2.position.x + Random.Range(this.sparkleTransformCoin2.sizeDelta.x * -0.5f, this.sparkleTransformCoin2.sizeDelta.x * 0.5f), this.sparkleTransformCoin2.position.y + Random.Range(this.sparkleTransformCoin2.sizeDelta.y * -0.5f, this.sparkleTransformCoin2.sizeDelta.y * 0.5f), 101f);
						this.sparkleAnimatorsCoin2[k].SetTrigger(Random.Range(0, 4).ToStringInvariant());
					}
				}
			}
			for (int l = 0; l < this.sparkleAnimatorsCoin3.Length; l++)
			{
				if (this.timeBeforeNextSparkleCoin3 <= 0f)
				{
					if (this.sparkleAnimatorsCoin3[l].GetCurrentAnimatorStateInfo(0).IsName("Empty"))
					{
						this.timeBeforeNextSparkleCoin3 = this.timeBetweenSparkle;
						this.sparkleAnimatorsCoin3[l].transform.position = new Vector3(this.sparkleTransformCoin3.position.x + Random.Range(this.sparkleTransformCoin3.sizeDelta.x * -0.5f, this.sparkleTransformCoin3.sizeDelta.x * 0.5f), this.sparkleTransformCoin3.position.y + Random.Range(this.sparkleTransformCoin3.sizeDelta.y * -0.5f, this.sparkleTransformCoin3.sizeDelta.y * 0.5f), 101f);
						this.sparkleAnimatorsCoin3[l].SetTrigger(Random.Range(0, 4).ToStringInvariant());
					}
				}
			}
		}
		if (this.sparkling)
		{
			if (this.input.GetAnyButtonDown())
			{
				base.StartCoroutine(this.tweenOut_cr(1.5f));
				base.animator.SetTrigger("hide");
				this.sparkling = false;
			}
			this.timeBeforeNextSparkleContract -= CupheadTime.Delta;
			for (int m = 0; m < this.sparkleAnimatorsContract.Length; m++)
			{
				if (this.timeBeforeNextSparkleContract <= 0f)
				{
					if (this.sparkleAnimatorsContract[m].GetCurrentAnimatorStateInfo(0).IsName("Empty"))
					{
						this.timeBeforeNextSparkleContract = this.timeBetweenSparkle;
						this.sparkleAnimatorsContract[m].transform.position = new Vector3(this.sparkleTransformContract.position.x + Random.Range(this.sparkleTransformContract.sizeDelta.x * -0.5f, this.sparkleTransformContract.sizeDelta.x * 0.5f), this.sparkleTransformContract.position.y + Random.Range(this.sparkleTransformContract.sizeDelta.y * -0.5f, this.sparkleTransformContract.sizeDelta.y * 0.5f), 101f);
						this.sparkleAnimatorsContract[m].SetTrigger(Random.Range(0, 4).ToStringInvariant());
					}
				}
			}
		}
		if (this.dlcAvailableShowing && !this.dlcUI.visible)
		{
			base.StartCoroutine(this.tweenOut_cr(0.25f));
			this.dlcAvailableShowing = false;
		}
		if (this.ingredientShowing && this.input.GetAnyButtonDown())
		{
			base.StartCoroutine(this.tweenOut_cr(1.5f));
			base.animator.SetTrigger("hide_ingred");
			this.ingredientShowing = false;
		}
		if (this.djimmiShowing && this.input.GetAnyButtonDown())
		{
			base.animator.SetTrigger("hide_djimmi");
			this.djimmiShowing = false;
		}
	}

	// Token: 0x0600331B RID: 13083 RVA: 0x0002A60A File Offset: 0x0002880A
	public void SparkleStart()
	{
		this.sparkling = true;
		base.StartCoroutine(this.showGlyphs_cr());
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x000F286C File Offset: 0x000F0A6C
	public IEnumerator showGlyphs_cr()
	{
		yield return new WaitForSeconds(0.5f);
		float t = 0f;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.glyphCanvasGroup.alpha = Mathf.Lerp(0f, 1f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.glyphCanvasGroup.alpha = 1f;
		while (!this.input.GetButtonDown(CupheadButton.Accept))
		{
			yield return null;
		}
		base.animator.SetTrigger("hide");
		yield return null;
		yield return base.animator.WaitForAnimationToEnd(this, "anim_map_ui_contract_end", 0, false, true);
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0600331D RID: 13085 RVA: 0x000F2888 File Offset: 0x000F0A88
	public void DebugShowContract(Levels level)
	{
		base.gameObject.SetActive(true);
		this.super1.SetActive(false);
		this.super2.SetActive(false);
		this.super3.SetActive(false);
		this.coin2.SetActive(false);
		this.coin3.SetActive(false);
		this.coinVariable.SetActive(false);
		this.coinVariableText.enabled = false;
		this.curseCharm.SetActive(false);
		this.airplaneIngred.SetActive(false);
		this.rumIngred.SetActive(false);
		this.oldManIngred.SetActive(false);
		this.snowCultIngred.SetActive(false);
		this.cowboyIngred.SetActive(false);
		InterruptingPrompt.SetCanInterrupt(true);
		this.tooltipEquipGlyph.SetActive(false);
		AudioManager.Play("world_map_soul_contract_open");
		AudioManager.PlayLoop("world_map_soul_contract_stamp_shimmer_loop");
		base.animator.SetTrigger("show");
		TranslationElement translationElement = Localization.Find(level.ToString());
		this.localizationHelper.ApplyTranslation(translationElement, null);
		this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
		string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
		this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockContract"), new LocalizationHelper.LocalizationSubtext[]
		{
			new LocalizationHelper.LocalizationSubtext("CONTRACT", translationElement.translation.text.Replace("\\n", newValue), false)
		});
		this.showing = true;
		this.canvasGroup.alpha = 1f;
	}

	// Token: 0x0600331E RID: 13086 RVA: 0x0002A620 File Offset: 0x00028820
	public void DebugShowEvent(Levels level)
	{
		this.DebugShowContract(level);
	}

	// Token: 0x0600331F RID: 13087 RVA: 0x000F2A48 File Offset: 0x000F0C48
	public IEnumerator HideContract()
	{
		this.showing = true;
		base.animator.SetTrigger("hide");
		yield return null;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x000F2A64 File Offset: 0x000F0C64
	public void ShowEvent(MapEventNotification.Type eventType)
	{
		this.EventQueue.Enqueue(delegate
		{
			this.InternalShowEvent(eventType);
		});
	}

	// Token: 0x06003321 RID: 13089 RVA: 0x000F2A9C File Offset: 0x000F0C9C
	public void ShowVariableCoinEvent(int coinCount)
	{
		if (coinCount > 1)
		{
			this.coinVariableCount = coinCount;
			this.EventQueue.Enqueue(delegate
			{
				this.InternalShowEvent(MapEventNotification.Type.CoinVariable);
			});
		}
		else
		{
			this.EventQueue.Enqueue(delegate
			{
				this.InternalShowEvent(MapEventNotification.Type.Coin);
			});
		}
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x000F2AEC File Offset: 0x000F0CEC
	public void InternalShowEvent(MapEventNotification.Type eventType)
	{
		base.gameObject.SetActive(true);
		this.super1.SetActive(false);
		this.super2.SetActive(false);
		this.super3.SetActive(false);
		this.coin2.SetActive(false);
		this.coin3.SetActive(false);
		this.coinVariable.SetActive(false);
		this.coinVariableText.enabled = false;
		this.curseCharm.SetActive(false);
		this.airplaneIngred.SetActive(false);
		this.rumIngred.SetActive(false);
		this.oldManIngred.SetActive(false);
		this.snowCultIngred.SetActive(false);
		this.cowboyIngred.SetActive(false);
		InterruptingPrompt.SetCanInterrupt(true);
		switch (eventType)
		{
		case MapEventNotification.Type.SoulContract:
		{
			this.confirmGlyph.SetActive(true);
			this.tooltipEquipGlyph.SetActive(false);
			AudioManager.Play("world_map_soul_contract_open");
			AudioManager.PlayLoop("world_map_soul_contract_stamp_shimmer_loop");
			base.animator.SetTrigger("show");
			TranslationElement translationElement = Localization.Find(Level.PreviousLevel.ToString());
			this.localizationHelper.ApplyTranslation(translationElement, null);
			this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
			string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockContract"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("CONTRACT", translationElement.translation.text.Replace("\\n", newValue), false)
			});
			break;
		}
		case MapEventNotification.Type.Super:
		{
			this.confirmGlyph.SetActive(true);
			base.animator.SetTrigger("show_super");
			AudioManager.Stop("world_level_bridge_building_poof");
			AudioManager.Play("world_map_super_open");
			AudioManager.PlayLoop("world_map_super_loop");
			base.StartCoroutine(this.SuperInRoutine());
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockSuper"), null);
			Scenes currentMap = PlayerData.Data.CurrentMap;
			if (currentMap != Scenes.scene_map_world_1)
			{
				if (currentMap != Scenes.scene_map_world_2)
				{
					if (currentMap == Scenes.scene_map_world_3)
					{
						this.super3.SetActive(true);
					}
				}
				else
				{
					this.super2.SetActive(true);
				}
			}
			else
			{
				this.super1.SetActive(true);
			}
			break;
		}
		case MapEventNotification.Type.Coin:
			this.confirmGlyph.SetActive(true);
			AudioManager.Play("world_map_coin_open");
			base.StartCoroutine(this.CoinInRoutine());
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("GotACoin"), null);
			base.animator.SetTrigger("show_coin");
			break;
		case MapEventNotification.Type.ThreeCoins:
			this.confirmGlyph.SetActive(true);
			this.coin2.SetActive(true);
			this.coin3.SetActive(true);
			AudioManager.Play("world_map_coin_open");
			base.StartCoroutine(this.CoinInRoutine());
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("GotThreeCoins"), null);
			base.animator.SetTrigger("show_coin");
			break;
		case MapEventNotification.Type.Tooltip:
			this.confirmGlyph.SetActive(true);
			this.tooltipEquipGlyph.SetActive(false);
			base.StartCoroutine(this.TooltipInRoutine());
			base.animator.SetTrigger("show_tooltip");
			AudioManager.Play("menu_cardup");
			break;
		case MapEventNotification.Type.TooltipEquip:
			this.confirmGlyph.SetActive(false);
			this.tooltipEquipGlyph.SetActive(true);
			base.StartCoroutine(this.TooltipEquipInRoutine());
			base.animator.SetTrigger("show_tooltip");
			AudioManager.Play("menu_cardup");
			break;
		case MapEventNotification.Type.DLCAvailable:
			base.GetComponent<Animator>().enabled = false;
			base.transform.Find("Darker").gameObject.SetActive(false);
			base.transform.Find("Background").gameObject.SetActive(false);
			base.transform.Find("Text").gameObject.SetActive(false);
			base.transform.Find("LetterboxTop").gameObject.SetActive(false);
			base.transform.Find("LetterboxBottom").gameObject.SetActive(false);
			this.confirmGlyph.SetActive(true);
			this.notificationLocalizationHelper.textComponent.text = string.Empty;
			this.dlcUI.ShowMenu();
			base.StartCoroutine(this.DLCAvailableRoutine());
			break;
		case MapEventNotification.Type.AirplaneIngredient:
		{
			this.confirmGlyph.SetActive(true);
			this.airplaneIngred.SetActive(true);
			base.StartCoroutine(this.IngredientRoutine());
			base.animator.SetTrigger("show_ingred_airplane");
			AudioManager.Play("sfx_dlc_worldmap_ingredient");
			TranslationElement translationElement = Localization.Find("AirplaneIngredient");
			this.localizationHelper.ApplyTranslation(translationElement, null);
			this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
			string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\\n", newValue), false)
			});
			break;
		}
		case MapEventNotification.Type.RumIngredient:
		{
			this.confirmGlyph.SetActive(true);
			this.rumIngred.SetActive(true);
			base.StartCoroutine(this.IngredientRoutine());
			base.animator.SetTrigger("show_ingred_rum");
			TranslationElement translationElement = Localization.Find("RumIngredient");
			AudioManager.Play("sfx_dlc_worldmap_ingredient");
			this.localizationHelper.ApplyTranslation(translationElement, null);
			this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
			string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\\n", newValue), false)
			});
			break;
		}
		case MapEventNotification.Type.OldManIngredient:
		{
			this.confirmGlyph.SetActive(true);
			this.oldManIngred.SetActive(true);
			base.StartCoroutine(this.IngredientRoutine());
			base.animator.SetTrigger("show_ingred_oldman");
			AudioManager.Play("sfx_dlc_worldmap_ingredient");
			TranslationElement translationElement = Localization.Find("OldManIngredient");
			this.localizationHelper.ApplyTranslation(translationElement, null);
			this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
			string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\\n", newValue), false)
			});
			break;
		}
		case MapEventNotification.Type.SnowIngredient:
		{
			this.confirmGlyph.SetActive(true);
			this.snowCultIngred.SetActive(true);
			base.StartCoroutine(this.IngredientRoutine());
			base.animator.SetTrigger("show_ingred_snowcult");
			AudioManager.Play("sfx_dlc_worldmap_ingredient");
			TranslationElement translationElement = Localization.Find("SnowCultIngredient");
			this.localizationHelper.ApplyTranslation(translationElement, null);
			this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
			string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\\n", newValue), false)
			});
			break;
		}
		case MapEventNotification.Type.CowboyIngredient:
		{
			this.confirmGlyph.SetActive(true);
			this.cowboyIngred.SetActive(true);
			base.StartCoroutine(this.IngredientRoutine());
			base.animator.SetTrigger("show_ingred_cowboy");
			AudioManager.Play("sfx_dlc_worldmap_ingredient");
			TranslationElement translationElement = Localization.Find("CowboyIngredient");
			this.localizationHelper.ApplyTranslation(translationElement, null);
			this.localizationHelper.textMeshProComponent.text = this.localizationHelper.textMeshProComponent.text.ToUpper().Replace("\\N", "\\n");
			string newValue = (Localization.language != Localization.Languages.Japanese) ? " " : string.Empty;
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("UnlockIngredient"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("INGREDIENT", translationElement.translation.text.Replace("\\n", newValue), false)
			});
			break;
		}
		case MapEventNotification.Type.CoinVariable:
			this.confirmGlyph.SetActive(true);
			this.coinVariable.SetActive(true);
			this.coinVariableText.text = "x" + this.coinVariableCount.ToString();
			this.coinVariableText.enabled = true;
			AudioManager.Play("world_map_coin_open");
			base.StartCoroutine(this.CoinInRoutine());
			this.notificationLocalizationHelper.ApplyTranslation(Localization.Find("GotACoin"), null);
			base.animator.SetTrigger("show_coinvariable");
			break;
		case MapEventNotification.Type.Djimmi:
		{
			AudioManager.Play("sfx_worldmap_djimmi_open");
			TranslationElement translationElement = Localization.Find("GameDjimmi_Tooltip_Wish" + (3 - PlayerData.Data.djimmiWishes).ToString());
			this.notificationLocalizationHelper.ApplyTranslation(translationElement, null);
			base.animator.SetTrigger("show_djimmi");
			base.StartCoroutine(this.DjimmiRoutine());
			break;
		}
		case MapEventNotification.Type.DjimmiFreed:
		{
			AudioManager.Play("sfx_worldmap_djimmi_open");
			TranslationElement translationElement = Localization.Find("GameDjimmi_Tooltip_Freed");
			this.notificationLocalizationHelper.ApplyTranslation(translationElement, null);
			base.animator.SetTrigger("show_djimmi");
			base.StartCoroutine(this.DjimmiRoutine());
			break;
		}
		}
		this.showing = true;
		base.StartCoroutine(this.tweenIn_cr());
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x000F35E0 File Offset: 0x000F17E0
	public void ShowTooltipEvent(TooltipEvent tooltipEvent)
	{
		InterruptingPrompt.SetCanInterrupt(true);
		switch (tooltipEvent)
		{
		case TooltipEvent.Turtle:
			this.tooltipPortrait.sprite = this.TurtleSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Pacifist_Tooltip_NewAudioVisMode"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.Canteen:
			this.tooltipPortrait.sprite = this.CanteenSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Canteen_Tooltip_ShmupWeapons"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.ShopKeep:
			this.tooltipPortrait.sprite = this.ShopkeepSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Shopkeeper_Tooltip_NewPurchase"), null);
			this.ShowEvent(MapEventNotification.Type.TooltipEquip);
			break;
		case TooltipEvent.Professional:
			this.tooltipPortrait.sprite = this.ForkSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Professional_Tooltip_SuperEquip"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.KingDice:
			this.tooltipPortrait.sprite = this.KingDiceSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("KingDice_Tooltip_RegularSoulContracts"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.Mausoleum:
			this.tooltipPortrait.sprite = this.MausoleumSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Chalice_Tooltip_NewSuperEquip"), null);
			this.ShowEvent(MapEventNotification.Type.TooltipEquip);
			break;
		case TooltipEvent.Boatman:
			this.tooltipPortrait.sprite = this.BoatmanSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Boatman_Tooltip_UpgradedSave"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.Chalice:
			this.tooltipPortrait.sprite = this.SaltbakerSpriteB;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Chalice_Tooltip_CharmEquip"), null);
			this.ShowEvent(MapEventNotification.Type.TooltipEquip);
			break;
		case TooltipEvent.ChaliceTutorialEquipCharm:
			this.tooltipPortrait.sprite = this.SaltbakerSpriteA;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find((!PlayerManager.Multiplayer) ? "Saltbaker_Tooltip_ChaliceTutorialSingle" : "Saltbaker_Tooltip_ChaliceTutorialMulti"), null);
			this.ShowEvent(MapEventNotification.Type.TooltipEquip);
			break;
		case TooltipEvent.SimpleIngredient:
			this.tooltipPortrait.sprite = this.SaltbakerSpriteA;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Saltbaker_Tooltip_SimpleIngredient"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.BackToKitchen:
			this.tooltipPortrait.sprite = this.ChaliceSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Chalice_Tooltip_GotAllIngredients"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		case TooltipEvent.ChaliceFan:
			this.tooltipPortrait.sprite = this.ChaliceFanSprite;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("ChaliceFan_Tooltip_NewFilter"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		default:
			this.tooltipPortrait.sprite = null;
			this.tooltipLocalizationHelper.ApplyTranslation(Localization.Find("Shopkeeper_Tooltip_NewPurchase"), null);
			this.ShowEvent(MapEventNotification.Type.Tooltip);
			break;
		}
	}

	// Token: 0x06003324 RID: 13092 RVA: 0x000F38D4 File Offset: 0x000F1AD4
	public IEnumerator CoinInRoutine()
	{
		yield return new WaitForSeconds(1f);
		this.coinShowing = true;
		yield break;
	}

	// Token: 0x06003325 RID: 13093 RVA: 0x000F38F0 File Offset: 0x000F1AF0
	public IEnumerator TooltipInRoutine()
	{
		yield return new WaitForSeconds(1f);
		this.tooltipShowing = true;
		yield break;
	}

	// Token: 0x06003326 RID: 13094 RVA: 0x000F390C File Offset: 0x000F1B0C
	public IEnumerator TooltipEquipInRoutine()
	{
		yield return new WaitForSeconds(1f);
		this.tooltipEquipShowing = true;
		yield break;
	}

	// Token: 0x06003327 RID: 13095 RVA: 0x000F3928 File Offset: 0x000F1B28
	public IEnumerator SuperInRoutine()
	{
		yield return new WaitForSeconds(1f);
		this.superShowing = true;
		yield break;
	}

	// Token: 0x06003328 RID: 13096 RVA: 0x000F3944 File Offset: 0x000F1B44
	public IEnumerator DLCAvailableRoutine()
	{
		yield return new WaitForSeconds(1f);
		this.dlcAvailableShowing = true;
		yield break;
	}

	// Token: 0x06003329 RID: 13097 RVA: 0x000F3960 File Offset: 0x000F1B60
	public IEnumerator IngredientRoutine()
	{
		this.ingredientStarburst.SetActive(true);
		yield return new WaitForSeconds(1f);
		this.ingredientShowing = true;
		yield break;
	}

	// Token: 0x0600332A RID: 13098 RVA: 0x0002A629 File Offset: 0x00028829
	public void AniEvent_DjimmiAppear()
	{
		AudioManager.Play("sfx_worldmap_djimmi_entrance");
	}

	// Token: 0x0600332B RID: 13099 RVA: 0x0002A635 File Offset: 0x00028835
	public void AniEvent_DjimmiLaugh()
	{
		AudioManager.Play("sfx_worldmap_djimmi_laugh");
	}

	// Token: 0x0600332C RID: 13100 RVA: 0x0002A641 File Offset: 0x00028841
	public void AniEvent_DjimmiMagicLoop()
	{
		AudioManager.PlayLoop("sfx_worldmap_djimmi_magic");
		AudioManager.FadeSFXVolumeLinear("sfx_worldmap_djimmi_magic", 0.5f, 0.5f);
	}

	// Token: 0x0600332D RID: 13101 RVA: 0x000F397C File Offset: 0x000F1B7C
	public IEnumerator DjimmiRoutine()
	{
		yield return new WaitForSeconds(1f);
		this.djimmiShowing = true;
		yield return base.animator.WaitForAnimationToStart(this, "anim_map_djimmi_out", false);
		AudioManager.Play("sfx_worldmap_djimmi_disappear");
		AudioManager.Stop("sfx_worldmap_djimmi_magic");
		base.StartCoroutine(this.tweenOut_cr(1.5f));
		yield break;
	}

	// Token: 0x0600332E RID: 13102 RVA: 0x000F3998 File Offset: 0x000F1B98
	public IEnumerator tweenIn_cr()
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
		yield break;
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x000F39B4 File Offset: 0x000F1BB4
	public IEnumerator tweenOut_cr(float time = 1.5f)
	{
		AudioManager.FadeSFXVolume("world_map_soul_contract_stamp_shimmer_loop", 0f, 5f);
		AudioManager.FadeSFXVolume("world_map_super_loop", 0f, 5f);
		yield return new WaitForSeconds(time);
		float t = 0f;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.canvasGroup.alpha = Mathf.Lerp(1f, 0f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = 0f;
		while (InterruptingPrompt.IsInterrupting())
		{
			yield return null;
		}
		this.showing = false;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x000F39D8 File Offset: 0x000F1BD8
	public IEnumerator text_cr()
	{
		yield return base.StartCoroutine(this.textScale_cr(0.9f, 1.1f, 0.5f));
		yield return base.StartCoroutine(this.textScale_cr(1.1f, 0.9f, 0.5f));
		while (!this.input.GetButtonDown(CupheadButton.Accept))
		{
			yield return null;
		}
		this.showing = false;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06003331 RID: 13105 RVA: 0x000F39F4 File Offset: 0x000F1BF4
	public IEnumerator textScale_cr(float start, float end, float time)
	{
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.text.transform.localScale = Vector3.one * EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.text.transform.localScale = Vector3.one * end;
		yield return null;
		yield break;
	}

	// Token: 0x04002A0D RID: 10765
	[SerializeField]
	public Image background;

	// Token: 0x04002A0E RID: 10766
	[SerializeField]
	public TextMeshProUGUI text;

	// Token: 0x04002A0F RID: 10767
	[SerializeField]
	public LocalizationHelper localizationHelper;

	// Token: 0x04002A10 RID: 10768
	[SerializeField]
	public LocalizationHelper notificationLocalizationHelper;

	// Token: 0x04002A11 RID: 10769
	[SerializeField]
	public RectTransform sparkleTransformContract;

	// Token: 0x04002A12 RID: 10770
	[SerializeField]
	public RectTransform sparkleTransformCoin1;

	// Token: 0x04002A13 RID: 10771
	[SerializeField]
	public RectTransform sparkleTransformCoin2;

	// Token: 0x04002A14 RID: 10772
	[SerializeField]
	public RectTransform sparkleTransformCoin3;

	// Token: 0x04002A15 RID: 10773
	[SerializeField]
	public GameObject sparklePrefab;

	// Token: 0x04002A16 RID: 10774
	[SerializeField]
	public CanvasGroup glyphCanvasGroup;

	// Token: 0x04002A17 RID: 10775
	[SerializeField]
	public GameObject coin2;

	// Token: 0x04002A18 RID: 10776
	[SerializeField]
	public GameObject coin3;

	// Token: 0x04002A19 RID: 10777
	[SerializeField]
	public GameObject coinVariable;

	// Token: 0x04002A1A RID: 10778
	[SerializeField]
	public Text coinVariableText;

	// Token: 0x04002A1B RID: 10779
	[SerializeField]
	public GameObject super1;

	// Token: 0x04002A1C RID: 10780
	[SerializeField]
	public GameObject super2;

	// Token: 0x04002A1D RID: 10781
	[SerializeField]
	public GameObject super3;

	// Token: 0x04002A1E RID: 10782
	[SerializeField]
	public GameObject curseCharm;

	// Token: 0x04002A1F RID: 10783
	[SerializeField]
	public GameObject ingredientStarburst;

	// Token: 0x04002A20 RID: 10784
	[SerializeField]
	public GameObject airplaneIngred;

	// Token: 0x04002A21 RID: 10785
	[SerializeField]
	public GameObject rumIngred;

	// Token: 0x04002A22 RID: 10786
	[SerializeField]
	public GameObject oldManIngred;

	// Token: 0x04002A23 RID: 10787
	[SerializeField]
	public GameObject snowCultIngred;

	// Token: 0x04002A24 RID: 10788
	[SerializeField]
	public GameObject cowboyIngred;

	// Token: 0x04002A25 RID: 10789
	[SerializeField]
	public GameObject confirmGlyph;

	// Token: 0x04002A26 RID: 10790
	[SerializeField]
	public GameObject dlcUIPrefab;

	// Token: 0x04002A27 RID: 10791
	[SerializeField]
	public Transform dlcUIRoot;

	// Token: 0x04002A28 RID: 10792
	[Header("Tooltips")]
	[SerializeField]
	public CanvasGroup tooltipCanvasGroup;

	// Token: 0x04002A29 RID: 10793
	[SerializeField]
	public Image tooltipPortrait;

	// Token: 0x04002A2A RID: 10794
	[SerializeField]
	public LocalizationHelper tooltipLocalizationHelper;

	// Token: 0x04002A2B RID: 10795
	[SerializeField]
	public GameObject tooltipEquipGlyph;

	// Token: 0x04002A2C RID: 10796
	[SerializeField]
	public Sprite TurtleSprite;

	// Token: 0x04002A2D RID: 10797
	[SerializeField]
	public Sprite CanteenSprite;

	// Token: 0x04002A2E RID: 10798
	[SerializeField]
	public Sprite ShopkeepSprite;

	// Token: 0x04002A2F RID: 10799
	[SerializeField]
	public Sprite ForkSprite;

	// Token: 0x04002A30 RID: 10800
	[SerializeField]
	public Sprite KingDiceSprite;

	// Token: 0x04002A31 RID: 10801
	[SerializeField]
	public Sprite MausoleumSprite;

	// Token: 0x04002A32 RID: 10802
	[SerializeField]
	public Sprite SaltbakerSpriteA;

	// Token: 0x04002A33 RID: 10803
	[SerializeField]
	public Sprite SaltbakerSpriteB;

	// Token: 0x04002A34 RID: 10804
	[SerializeField]
	public Sprite ChaliceSprite;

	// Token: 0x04002A35 RID: 10805
	[SerializeField]
	public Sprite ChaliceFanSprite;

	// Token: 0x04002A36 RID: 10806
	[SerializeField]
	public Sprite BoatmanSprite;

	// Token: 0x04002A37 RID: 10807
	public CanvasGroup canvasGroup;

	// Token: 0x04002A39 RID: 10809
	public bool sparkling;

	// Token: 0x04002A3A RID: 10810
	public bool coinShowing;

	// Token: 0x04002A3B RID: 10811
	public bool tooltipShowing;

	// Token: 0x04002A3C RID: 10812
	public bool tooltipEquipShowing;

	// Token: 0x04002A3D RID: 10813
	public bool superShowing;

	// Token: 0x04002A3E RID: 10814
	public bool dlcAvailableShowing;

	// Token: 0x04002A3F RID: 10815
	public bool ingredientShowing;

	// Token: 0x04002A40 RID: 10816
	public bool djimmiShowing;

	// Token: 0x04002A41 RID: 10817
	public int coinVariableCount;

	// Token: 0x04002A42 RID: 10818
	public Animator[] sparkleAnimatorsContract = new Animator[3];

	// Token: 0x04002A43 RID: 10819
	public Animator[] sparkleAnimatorsCoin1 = new Animator[3];

	// Token: 0x04002A44 RID: 10820
	public Animator[] sparkleAnimatorsCoin2 = new Animator[3];

	// Token: 0x04002A45 RID: 10821
	public Animator[] sparkleAnimatorsCoin3 = new Animator[3];

	// Token: 0x04002A46 RID: 10822
	public float timeBeforeNextSparkleContract = 0.2f;

	// Token: 0x04002A47 RID: 10823
	public float timeBeforeNextSparkleCoin1 = 0.2f;

	// Token: 0x04002A48 RID: 10824
	public float timeBeforeNextSparkleCoin2 = 0.2f;

	// Token: 0x04002A49 RID: 10825
	public float timeBeforeNextSparkleCoin3 = 0.2f;

	// Token: 0x04002A4A RID: 10826
	[SerializeField]
	public float timeBetweenSparkle = 0.3f;

	// Token: 0x04002A4B RID: 10827
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04002A4C RID: 10828
	public Queue<Action> EventQueue = new Queue<Action>();

	// Token: 0x04002A4D RID: 10829
	public MapDLCUI dlcUI;

	// Token: 0x02001126 RID: 4390
	public enum Type
	{
		// Token: 0x04007903 RID: 30979
		SoulContract,
		// Token: 0x04007904 RID: 30980
		Super,
		// Token: 0x04007905 RID: 30981
		Coin,
		// Token: 0x04007906 RID: 30982
		ThreeCoins,
		// Token: 0x04007907 RID: 30983
		Blueprint,
		// Token: 0x04007908 RID: 30984
		Tooltip,
		// Token: 0x04007909 RID: 30985
		TooltipEquip,
		// Token: 0x0400790A RID: 30986
		DLCAvailable,
		// Token: 0x0400790B RID: 30987
		AirplaneIngredient,
		// Token: 0x0400790C RID: 30988
		RumIngredient,
		// Token: 0x0400790D RID: 30989
		OldManIngredient,
		// Token: 0x0400790E RID: 30990
		SnowIngredient,
		// Token: 0x0400790F RID: 30991
		CowboyIngredient,
		// Token: 0x04007910 RID: 30992
		CoinVariable,
		// Token: 0x04007911 RID: 30993
		Djimmi,
		// Token: 0x04007912 RID: 30994
		DjimmiFreed
	}
}
