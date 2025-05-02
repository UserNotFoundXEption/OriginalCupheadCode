using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200059F RID: 1439
public class ShopScenePlayer : AbstractMonoBehaviour
{
	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06003CCC RID: 15564 RVA: 0x000311B0 File Offset: 0x0002F3B0
	public ShopSceneItem CurrentItem
	{
		get
		{
			this.index = Mathf.Clamp(this.index, 0, this.items.Count - 1);
			return this.items[this.index];
		}
	}

	// Token: 0x140000C6 RID: 198
	// (add) Token: 0x06003CCD RID: 15565 RVA: 0x001167B0 File Offset: 0x001149B0
	// (remove) Token: 0x06003CCE RID: 15566 RVA: 0x001167E8 File Offset: 0x001149E8
	public event Action OnPurchaseEvent;

	// Token: 0x140000C7 RID: 199
	// (add) Token: 0x06003CCF RID: 15567 RVA: 0x00116820 File Offset: 0x00114A20
	// (remove) Token: 0x06003CD0 RID: 15568 RVA: 0x00116858 File Offset: 0x00114A58
	public event Action OnExitEvent;

	// Token: 0x06003CD1 RID: 15569 RVA: 0x00116890 File Offset: 0x00114A90
	public override void Awake()
	{
		base.Awake();
		this.doorPositionOpen = this.door.transform.localPosition.x;
		this.door.transform.SetLocalPosition(new float?(0f), null, null);
		this.doorPositionClosed = this.door.transform.localPosition.x;
		this.currencyCanvasOriginalScale = this.currencyCanvas.localScale.x;
		if (!PlayerManager.Multiplayer && this.player == PlayerId.PlayerTwo)
		{
			this.items[0].gameObject.SetActive(false);
			return;
		}
		this.weaponIndex = 0;
		this.charmIndex = 0;
		for (int i = 0; i < this.items.Count; i++)
		{
			ShopSceneItem shopSceneItem = null;
			ItemType itemType = this.items[i].itemType;
			if (itemType != ItemType.Weapon)
			{
				if (itemType == ItemType.Charm)
				{
					while (this.charmIndex < this.charmItemPrefabs.Length && (PlayerData.Data.IsUnlocked(this.player, this.charmItemPrefabs[this.charmIndex].charm) || !this.charmItemPrefabs[this.charmIndex].IsAvailable))
					{
						this.charmIndex++;
					}
					if (this.charmIndex < this.charmItemPrefabs.Length)
					{
						shopSceneItem = this.charmItemPrefabs[this.charmIndex];
						this.charmIndex++;
					}
				}
			}
			else
			{
				while (this.weaponIndex < this.weaponItemPrefabs.Length && (PlayerData.Data.IsUnlocked(this.player, this.weaponItemPrefabs[this.weaponIndex].weapon) || !this.weaponItemPrefabs[this.weaponIndex].IsAvailable))
				{
					this.weaponIndex++;
				}
				if (this.weaponIndex < this.weaponItemPrefabs.Length)
				{
					shopSceneItem = this.weaponItemPrefabs[this.weaponIndex];
					this.weaponIndex++;
				}
			}
			if (shopSceneItem == null)
			{
				this.items[i].gameObject.SetActive(false);
				this.items.RemoveAt(i);
				i--;
			}
			else
			{
				ShopSceneItem shopSceneItem2 = this.items[i];
				shopSceneItem2.gameObject.SetActive(false);
				this.items[i] = Object.Instantiate<ShopSceneItem>(shopSceneItem);
				this.items[i].transform.position = shopSceneItem2.transform.position;
				this.items[i].spriteShadowObject.transform.SetParent(null);
			}
		}
		foreach (ShopSceneItem shopSceneItem3 in this.items)
		{
			shopSceneItem3.Init(this.player);
		}
	}

	// Token: 0x06003CD2 RID: 15570 RVA: 0x00116BE4 File Offset: 0x00114DE4
	public void Start()
	{
		if (this.player != PlayerId.PlayerOne && !PlayerManager.Multiplayer)
		{
			base.enabled = false;
			base.gameObject.SetActive(false);
			return;
		}
		this.singleDigitCoinPosition = this.coinImage.transform.position;
		if (PlayerData.Data.GetCurrency(this.player) >= 10)
		{
			this.isMoneyDoubleDigit = true;
			this.coinImage.transform.position = this.doubleDigitCoinPosition.position;
		}
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeft;
		this.displayNameText.font = Localization.Instance.fonts[(int)Localization.language][41].fontAsset;
		this.subText.font = Localization.Instance.fonts[(int)Localization.language][41].fontAsset;
		this.descriptionText.font = Localization.Instance.fonts[(int)Localization.language][11].fontAsset;
	}

	// Token: 0x06003CD3 RID: 15571 RVA: 0x00116CFC File Offset: 0x00114EFC
	public void Update()
	{
		if (InterruptingPrompt.IsInterrupting())
		{
			return;
		}
		if (PlayerData.Data.GetCurrency(this.player) >= 10 && !this.isMoneyDoubleDigit)
		{
			this.isMoneyDoubleDigit = true;
			this.coinImage.transform.position = this.doubleDigitCoinPosition.position;
		}
		else if (PlayerData.Data.GetCurrency(this.player) < 10 && this.isMoneyDoubleDigit)
		{
			this.isMoneyDoubleDigit = false;
			this.coinImage.transform.position = this.singleDigitCoinPosition;
		}
		int currency = PlayerData.Data.GetCurrency(this.player);
		Sprite sprite;
		if (currency < 0)
		{
			sprite = this.coinSprites[0];
		}
		else if (currency > this.coinSprites.Count - 1)
		{
			sprite = this.coinSprites[this.coinSprites.Count - 1];
		}
		else
		{
			sprite = this.coinSprites[currency];
		}
		this.currencyNbImage.sprite = sprite;
		switch (this.state)
		{
		case ShopScenePlayer.State.Selecting:
			if (this.items.Count > 0 && this.CurrentItem.state != ShopSceneItem.State.Ready)
			{
				return;
			}
			if (this.items.Count > 1 && this.input.GetButtonDown(18))
			{
				AudioManager.Play("shop_selection_change");
				this.index--;
				this.UpdateSelection();
				return;
			}
			if (this.items.Count > 1 && this.input.GetButtonDown(20))
			{
				AudioManager.Play("shop_selection_change");
				this.index++;
				this.UpdateSelection();
				return;
			}
			if (this.items.Count > 0 && this.input.GetButtonDown(13))
			{
				if (this.CurrentItem.Purchase())
				{
					this.Purchase();
				}
				else
				{
					this.CantPurchase();
				}
			}
			if (this.input.GetButtonDown(14) || this.playerLeft)
			{
				this.Exit();
			}
			break;
		case ShopScenePlayer.State.Purchasing:
			return;
		case ShopScenePlayer.State.Exited:
			if (this.input.GetButtonDown(13) && !this.exitingShop)
			{
				this.StopAllCoroutines();
				this.state = ShopScenePlayer.State.Init;
				this.OnStart();
			}
			break;
		}
	}

	// Token: 0x06003CD4 RID: 15572 RVA: 0x000311E2 File Offset: 0x0002F3E2
	public ShopSceneItem[] GetWeaponItemPrefabs()
	{
		return this.weaponItemPrefabs;
	}

	// Token: 0x06003CD5 RID: 15573 RVA: 0x000311EA File Offset: 0x0002F3EA
	public ShopSceneItem[] GetCharmItemPrefabs()
	{
		return this.charmItemPrefabs;
	}

	// Token: 0x06003CD6 RID: 15574 RVA: 0x000311F2 File Offset: 0x0002F3F2
	public void OnStart()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		base.StartCoroutine(this.in_cr());
	}

	// Token: 0x06003CD7 RID: 15575 RVA: 0x00116F98 File Offset: 0x00115198
	public void Purchase()
	{
		AudioManager.Play("shop_purchase");
		this.UpdateSelection();
		if (this.scaleCoinCoroutine != null)
		{
			base.StopCoroutine(this.scaleCoinCoroutine);
		}
		this.scaleCoinCoroutine = base.StartCoroutine(this.scaleCoin_cr());
		this.state = ShopScenePlayer.State.Purchasing;
		if (this.OnPurchaseEvent != null)
		{
			this.OnPurchaseEvent();
		}
	}

	// Token: 0x06003CD8 RID: 15576 RVA: 0x00031212 File Offset: 0x0002F412
	public void CantPurchase()
	{
		AudioManager.Play("shop_cantpurchase");
		if (this.moveItemCantPurchaseCoroutine != null)
		{
			base.StopCoroutine(this.moveItemCantPurchaseCoroutine);
		}
		this.moveItemCantPurchaseCoroutine = base.StartCoroutine(this.cantBuy_cr());
	}

	// Token: 0x06003CD9 RID: 15577 RVA: 0x00116FFC File Offset: 0x001151FC
	public void Exit()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.state = ShopScenePlayer.State.Exiting;
		if (this.moveItemCantPurchaseCoroutine != null)
		{
			base.StopCoroutine(this.moveItemCantPurchaseCoroutine);
		}
		base.StartCoroutine(this.out_cr());
		if (this.OnExitEvent != null)
		{
			this.OnExitEvent();
		}
	}

	// Token: 0x06003CDA RID: 15578 RVA: 0x00031247 File Offset: 0x0002F447
	public void OnExit()
	{
		this.exitingShop = true;
	}

	// Token: 0x06003CDB RID: 15579 RVA: 0x0011705C File Offset: 0x0011525C
	public void UpdateSelection()
	{
		if (this.items.Count == 0)
		{
			Localization.Translation translation = Localization.Translate("out_of_stock_name");
			this.displayNameText.text = translation.text;
			this.displayNameText.font = translation.fonts.fontAsset;
			Localization.Translation translation2 = Localization.Translate("out_of_stock_subtext");
			this.subText.text = translation2.text;
			this.subText.font = translation2.fonts.fontAsset;
			Localization.Translation translation3 = Localization.Translate("out_of_stock_description");
			this.descriptionText.text = translation3.text;
			this.descriptionText.font = translation3.fonts.fontAsset;
			this.priceSpriteRenderer.enabled = false;
			this.chalkCoinSpriteRenderer.enabled = false;
			return;
		}
		foreach (ShopSceneItem shopSceneItem in this.items)
		{
			shopSceneItem.Deselect();
		}
		this.CurrentItem.Select();
		if (this.CurrentItem.Purchased)
		{
			this.displayNameText.text = Localization.Translate("item_purchased_name").text;
			this.subText.text = Localization.Translate("item_purchased_subtext").text;
			this.descriptionText.text = Localization.Translate("item_purchased_description").text;
			return;
		}
		this.displayNameText.text = this.CurrentItem.DisplayName.ToUpper();
		this.priceSpriteRenderer.sprite = this.priceSprites[this.CurrentItem.Value - 1];
		this.subText.text = this.CurrentItem.Subtext;
		this.descriptionText.text = this.CurrentItem.Description;
		if (this.CurrentItem.charm == Charm.charm_curse)
		{
			this.displayNameText.text = Localization.Translate("charm_broken_name").text.ToUpper();
			this.subText.text = Localization.Translate("charm_broken_subtext").text;
			this.descriptionText.text = Localization.Translate("charm_broken_description").text;
		}
	}

	// Token: 0x06003CDC RID: 15580 RVA: 0x001172D0 File Offset: 0x001154D0
	public void OnDoorTweened(float value)
	{
		this.door.SetLocalPosition(new float?(Mathf.Lerp(this.doorPositionClosed, this.doorPositionOpen, value)), null, null);
	}

	// Token: 0x06003CDD RID: 15581 RVA: 0x00117314 File Offset: 0x00115514
	public IEnumerator in_cr()
	{
		if (this.firstStart)
		{
			yield return new WaitForSeconds((this.player != PlayerId.PlayerOne) ? 1.4f : 1.1f);
		}
		this.firstStart = false;
		this.input = PlayerManager.GetPlayerInput(this.player);
		this.UpdateSelection();
		if (this.player == PlayerId.PlayerOne)
		{
			AudioManager.Play("shop_slide_open_cuphead");
		}
		else
		{
			AudioManager.Play("shop_slide_open_mugman");
		}
		yield return base.TweenValue(0f, 1f, 1f, EaseUtils.EaseType.easeOutBounce, new AbstractMonoBehaviour.TweenUpdateHandler(this.OnDoorTweened));
		this.state = ShopScenePlayer.State.Selecting;
		yield break;
	}

	// Token: 0x06003CDE RID: 15582 RVA: 0x00117330 File Offset: 0x00115530
	public IEnumerator out_cr()
	{
		if (this.player == PlayerId.PlayerOne)
		{
			AudioManager.Play("shop_slide_close_cuphead");
		}
		else
		{
			AudioManager.Play("shop_slide_close_mugman");
		}
		foreach (ShopSceneItem shopSceneItem in this.items)
		{
			shopSceneItem.Deselect();
		}
		yield return base.TweenValue(1f, 0f, 1f, EaseUtils.EaseType.easeOutBounce, new AbstractMonoBehaviour.TweenUpdateHandler(this.OnDoorTweened));
		this.state = ShopScenePlayer.State.Exited;
		yield break;
	}

	// Token: 0x06003CDF RID: 15583 RVA: 0x0011734C File Offset: 0x0011554C
	public IEnumerator scaleCoin_cr()
	{
		while (this.currencyCanvas.localScale.x < this.currencyCanvasOriginalScale * this.currencyCanvasMultiplier)
		{
			this.currencyCanvas.localScale = new Vector2(this.currencyCanvas.localScale.x + this.currencyCanvasScaleValue, this.currencyCanvas.localScale.y + this.currencyCanvasScaleValue);
			yield return null;
		}
		while (this.currencyCanvas.localScale.x > this.currencyCanvasOriginalScale)
		{
			this.currencyCanvas.localScale = new Vector2(this.currencyCanvas.localScale.x - this.currencyCanvasScaleValue, this.currencyCanvas.localScale.y - this.currencyCanvasScaleValue);
			yield return null;
		}
		this.scaleCoinCoroutine = null;
		base.StartCoroutine(this.addNewItem_cr());
		yield break;
	}

	// Token: 0x06003CE0 RID: 15584 RVA: 0x00117368 File Offset: 0x00115568
	public IEnumerator cantBuy_cr()
	{
		float startPositionY = this.CurrentItem.endPosition.y;
		while (this.CurrentItem.transform.localPosition.y > startPositionY - this.CurrentItem.cantPurchaseYMovementPosition)
		{
			this.CurrentItem.transform.localPosition = new Vector2(this.CurrentItem.transform.localPosition.x, this.CurrentItem.transform.localPosition.y - this.CurrentItem.cantPurchaseYMovementValue);
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		while (this.CurrentItem.transform.position.y < startPositionY)
		{
			this.CurrentItem.transform.localPosition = new Vector2(this.CurrentItem.transform.localPosition.x, this.CurrentItem.transform.localPosition.y + this.CurrentItem.cantPurchaseYMovementValue * 1.5f);
			yield return null;
		}
		this.moveItemCantPurchaseCoroutine = null;
		yield break;
	}

	// Token: 0x06003CE1 RID: 15585 RVA: 0x00117384 File Offset: 0x00115584
	public IEnumerator addNewItem_cr()
	{
		ItemType type = this.CurrentItem.itemType;
		ShopSceneItem originalItem = this.CurrentItem;
		if (type == ItemType.Charm)
		{
			bool foundItem = false;
			for (int i = this.charmIndex; i < this.charmItemPrefabs.Length; i++)
			{
				if (!PlayerData.Data.IsUnlocked(this.player, this.charmItemPrefabs[i].charm) && this.charmItemPrefabs[i].IsAvailable)
				{
					foundItem = true;
					int itemIndex = this.items.IndexOf(this.CurrentItem);
					this.items[itemIndex] = Object.Instantiate<ShopSceneItem>(this.charmItemPrefabs[i]);
					this.items[itemIndex].player = this.player;
					this.items[itemIndex].startPosition = originalItem.startPosition;
					this.items[itemIndex].endPosition = originalItem.endPosition;
					Vector3 startPosition = this.items[itemIndex].startPosition;
					startPosition.y += 800f;
					this.items[itemIndex].transform.position = this.items[itemIndex].startPosition;
					this.items[itemIndex].spriteShadowObject.transform.SetParent(null);
					this.items[itemIndex].transform.position = startPosition;
					Vector3 originalShadowScale = this.items[itemIndex].spriteShadowObject.transform.localScale;
					this.items[itemIndex].spriteShadowObject.transform.localScale = Vector3.zero;
					this.items[itemIndex].TweenLocalPositionY(this.items[itemIndex].transform.position.y, this.items[itemIndex].startPosition.y, 0.5f, EaseUtils.EaseType.linear);
					float t = 0f;
					float TIME = 0.5f;
					while (t < TIME)
					{
						float val = t / TIME;
						Vector3 newScale = Vector3.Lerp(Vector3.zero, originalShadowScale, EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, val));
						this.items[itemIndex].spriteShadowObject.transform.localScale = newScale;
						t += Time.deltaTime;
						yield return null;
					}
					SpriteRenderer dustPoof = Object.Instantiate<SpriteRenderer>(this.poofPrefab, this.items[itemIndex].transform.position + this.items[itemIndex].poofOffset, Quaternion.identity);
					AudioManager.Play("item_drop");
					dustPoof.sortingOrder = 501;
					Object.Destroy(dustPoof.gameObject, 3f);
					yield return this.items[itemIndex].TweenLocalPositionY(this.items[itemIndex].transform.position.y, this.items[itemIndex].transform.position.y + 30f, 0.1f, EaseUtils.EaseType.linear);
					yield return this.items[itemIndex].TweenLocalPositionY(this.items[itemIndex].transform.position.y, this.items[itemIndex].transform.position.y - 30f, 0.1f, EaseUtils.EaseType.linear);
					yield return CupheadTime.WaitForSeconds(this, 0.2f);
					this.charmIndex = i + 1;
					this.UpdateSelection();
					break;
				}
			}
			if (!foundItem)
			{
				this.CurrentItem.spriteShadowObject.gameObject.SetActive(false);
				this.items.Remove(this.CurrentItem);
				this.UpdateSelection();
			}
		}
		else if (type == ItemType.Weapon)
		{
			bool foundItem2 = false;
			for (int j = this.weaponIndex; j < this.weaponItemPrefabs.Length; j++)
			{
				if (!PlayerData.Data.IsUnlocked(this.player, this.weaponItemPrefabs[j].weapon) && this.weaponItemPrefabs[j].IsAvailable)
				{
					foundItem2 = true;
					int itemIndex2 = this.items.IndexOf(this.CurrentItem);
					this.items[itemIndex2] = Object.Instantiate<ShopSceneItem>(this.weaponItemPrefabs[j]);
					this.items[itemIndex2].player = this.player;
					this.items[itemIndex2].startPosition = originalItem.startPosition;
					this.items[itemIndex2].endPosition = originalItem.endPosition;
					Vector3 startPosition2 = this.items[itemIndex2].startPosition;
					startPosition2.y += 800f;
					this.items[itemIndex2].transform.position = this.items[itemIndex2].startPosition;
					this.items[itemIndex2].spriteShadowObject.transform.SetParent(null);
					this.items[itemIndex2].transform.position = startPosition2;
					Vector3 originalShadowScale2 = this.items[itemIndex2].spriteShadowObject.transform.localScale;
					this.items[itemIndex2].spriteShadowObject.transform.localScale = Vector3.zero;
					this.items[itemIndex2].TweenLocalPositionY(this.items[itemIndex2].transform.position.y, this.items[itemIndex2].startPosition.y, 0.5f, EaseUtils.EaseType.linear);
					float t2 = 0f;
					float TIME2 = 0.5f;
					while (t2 < TIME2)
					{
						float val2 = t2 / TIME2;
						Vector3 newScale2 = Vector3.Lerp(Vector3.zero, originalShadowScale2, EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, val2));
						this.items[itemIndex2].spriteShadowObject.transform.localScale = newScale2;
						t2 += Time.deltaTime;
						yield return null;
					}
					SpriteRenderer dustPoof2 = Object.Instantiate<SpriteRenderer>(this.poofPrefab, this.items[itemIndex2].transform.position + this.items[itemIndex2].poofOffset, Quaternion.identity);
					AudioManager.Play("item_drop");
					dustPoof2.sortingOrder = 401;
					Object.Destroy(dustPoof2.gameObject, 3f);
					yield return this.items[itemIndex2].TweenLocalPositionY(this.items[itemIndex2].transform.position.y, this.items[itemIndex2].transform.position.y + 30f, 0.1f, EaseUtils.EaseType.linear);
					yield return this.items[itemIndex2].TweenLocalPositionY(this.items[itemIndex2].transform.position.y, this.items[itemIndex2].transform.position.y - 30f, 0.1f, EaseUtils.EaseType.linear);
					yield return CupheadTime.WaitForSeconds(this, 0.2f);
					this.weaponIndex = j + 1;
					this.UpdateSelection();
					break;
				}
			}
			if (!foundItem2)
			{
				this.CurrentItem.spriteShadowObject.gameObject.SetActive(false);
				this.items.Remove(this.CurrentItem);
				this.UpdateSelection();
			}
		}
		this.state = ShopScenePlayer.State.Selecting;
		yield break;
	}

	// Token: 0x06003CE2 RID: 15586 RVA: 0x00031250 File Offset: 0x0002F450
	public void OnPlayerLeft(PlayerId playerId)
	{
		if (playerId == this.player)
		{
			this.playerLeft = true;
		}
	}

	// Token: 0x06003CE3 RID: 15587 RVA: 0x00031265 File Offset: 0x0002F465
	public void OnDestroy()
	{
		this.weaponItemPrefabs = null;
		this.charmItemPrefabs = null;
		this.currencyNbImage = null;
		this.coinImage = null;
		this.priceSprites = null;
		this.poofPrefab = null;
		this.items = null;
	}

	// Token: 0x0400303D RID: 12349
	public const float DOOR_TIME = 1f;

	// Token: 0x0400303E RID: 12350
	public const float START_DELAY = 1f;

	// Token: 0x0400303F RID: 12351
	[SerializeField]
	public PlayerId player;

	// Token: 0x04003040 RID: 12352
	[Header("Visuals")]
	[SerializeField]
	public Transform door;

	// Token: 0x04003041 RID: 12353
	[Header("Items")]
	[SerializeField]
	public List<ShopSceneItem> items;

	// Token: 0x04003042 RID: 12354
	[SerializeField]
	public ShopSceneItem[] weaponItemPrefabs;

	// Token: 0x04003043 RID: 12355
	[SerializeField]
	public ShopSceneItem[] charmItemPrefabs;

	// Token: 0x04003044 RID: 12356
	[Header("UI Elements")]
	[SerializeField]
	public TMP_Text currencyText;

	// Token: 0x04003045 RID: 12357
	[Space(10f)]
	[SerializeField]
	public TextMeshProUGUI displayNameText;

	// Token: 0x04003046 RID: 12358
	[SerializeField]
	public TextMeshProUGUI subText;

	// Token: 0x04003047 RID: 12359
	[SerializeField]
	public TextMeshProUGUI descriptionText;

	// Token: 0x04003048 RID: 12360
	[SerializeField]
	public List<Sprite> coinSprites;

	// Token: 0x04003049 RID: 12361
	[SerializeField]
	public Image currencyNbImage;

	// Token: 0x0400304A RID: 12362
	[SerializeField]
	public Image coinImage;

	// Token: 0x0400304B RID: 12363
	[SerializeField]
	public Transform doubleDigitCoinPosition;

	// Token: 0x0400304C RID: 12364
	public Vector3 singleDigitCoinPosition;

	// Token: 0x0400304D RID: 12365
	[SerializeField]
	public Transform currencyCanvas;

	// Token: 0x0400304E RID: 12366
	[SerializeField]
	public float currencyCanvasScaleValue;

	// Token: 0x0400304F RID: 12367
	[SerializeField]
	public float currencyCanvasMultiplier;

	// Token: 0x04003050 RID: 12368
	[SerializeField]
	public SpriteRenderer poofPrefab;

	// Token: 0x04003051 RID: 12369
	[SerializeField]
	public Sprite[] priceSprites;

	// Token: 0x04003052 RID: 12370
	[SerializeField]
	public SpriteRenderer priceSpriteRenderer;

	// Token: 0x04003053 RID: 12371
	[SerializeField]
	public SpriteRenderer chalkCoinSpriteRenderer;

	// Token: 0x04003054 RID: 12372
	public Player input;

	// Token: 0x04003055 RID: 12373
	public float doorPositionClosed;

	// Token: 0x04003056 RID: 12374
	public float doorPositionOpen;

	// Token: 0x04003057 RID: 12375
	public ShopScenePlayer.State state;

	// Token: 0x04003058 RID: 12376
	public int index;

	// Token: 0x04003059 RID: 12377
	public float currencyCanvasOriginalScale;

	// Token: 0x0400305A RID: 12378
	public Coroutine scaleCoinCoroutine;

	// Token: 0x0400305B RID: 12379
	public Coroutine moveItemCantPurchaseCoroutine;

	// Token: 0x0400305E RID: 12382
	public bool exitingShop;

	// Token: 0x0400305F RID: 12383
	public bool firstStart = true;

	// Token: 0x04003060 RID: 12384
	public bool playerLeft;

	// Token: 0x04003061 RID: 12385
	public bool isMoneyDoubleDigit;

	// Token: 0x04003062 RID: 12386
	public int weaponIndex;

	// Token: 0x04003063 RID: 12387
	public int charmIndex;

	// Token: 0x02001231 RID: 4657
	public enum State
	{
		// Token: 0x04007E15 RID: 32277
		Init,
		// Token: 0x04007E16 RID: 32278
		Selecting,
		// Token: 0x04007E17 RID: 32279
		Viewing,
		// Token: 0x04007E18 RID: 32280
		Purchasing,
		// Token: 0x04007E19 RID: 32281
		Exiting,
		// Token: 0x04007E1A RID: 32282
		Exited
	}
}
