using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200059D RID: 1437
public class ShopSceneItem : AbstractMonoBehaviour
{
	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06003CB2 RID: 15538 RVA: 0x000310B6 File Offset: 0x0002F2B6
	// (set) Token: 0x06003CB3 RID: 15539 RVA: 0x000310BE File Offset: 0x0002F2BE
	public ShopSceneItem.State state { get; set; }

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x06003CB4 RID: 15540 RVA: 0x000310C7 File Offset: 0x0002F2C7
	public bool Purchased
	{
		get
		{
			return this.isPurchased(this.player);
		}
	}

	// Token: 0x06003CB5 RID: 15541 RVA: 0x0011625C File Offset: 0x0011445C
	public bool isPurchased(PlayerId player)
	{
		switch (this.itemType)
		{
		case ItemType.Weapon:
			return PlayerData.Data.IsUnlocked(player, this.weapon);
		case ItemType.Super:
			return PlayerData.Data.IsUnlocked(player, this.super);
		case ItemType.Charm:
			return PlayerData.Data.IsUnlocked(player, this.charm);
		default:
			return false;
		}
	}

	// Token: 0x06003CB6 RID: 15542 RVA: 0x000310D5 File Offset: 0x0002F2D5
	public bool isPurchasedForBuyAllItemsAchievement(PlayerId player)
	{
		return this.isDLCItem || this.isPurchased(player);
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x001162C0 File Offset: 0x001144C0
	public string DisplayName
	{
		get
		{
			switch (this.itemType)
			{
			case ItemType.Weapon:
				return WeaponProperties.GetDisplayName(this.weapon);
			case ItemType.Super:
				return WeaponProperties.GetDisplayName(this.super);
			case ItemType.Charm:
				return WeaponProperties.GetDisplayName(this.charm);
			default:
				return string.Empty;
			}
		}
	}

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x06003CB8 RID: 15544 RVA: 0x00116314 File Offset: 0x00114514
	public string Subtext
	{
		get
		{
			switch (this.itemType)
			{
			case ItemType.Weapon:
				return WeaponProperties.GetSubtext(this.weapon);
			case ItemType.Super:
				return WeaponProperties.GetSubtext(this.super);
			case ItemType.Charm:
				return WeaponProperties.GetSubtext(this.charm);
			default:
				return string.Empty;
			}
		}
	}

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x06003CB9 RID: 15545 RVA: 0x00116368 File Offset: 0x00114568
	public string Description
	{
		get
		{
			switch (this.itemType)
			{
			case ItemType.Weapon:
				return WeaponProperties.GetDescription(this.weapon);
			case ItemType.Super:
				return WeaponProperties.GetDescription(this.super);
			case ItemType.Charm:
				return WeaponProperties.GetDescription(this.charm);
			default:
				return string.Empty;
			}
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06003CBA RID: 15546 RVA: 0x001163BC File Offset: 0x001145BC
	public int Value
	{
		get
		{
			switch (this.itemType)
			{
			case ItemType.Weapon:
				return WeaponProperties.GetValue(this.weapon);
			case ItemType.Super:
				return WeaponProperties.GetValue(this.super);
			case ItemType.Charm:
				return WeaponProperties.GetValue(this.charm);
			default:
				return 0;
			}
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06003CBB RID: 15547 RVA: 0x000310EB File Offset: 0x0002F2EB
	public bool IsAvailable
	{
		get
		{
			return !this.isDLCItem || (this.isDLCItem && DLCManager.DLCEnabled());
		}
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x0011640C File Offset: 0x0011460C
	public void Init(PlayerId player)
	{
		this.startPosition = base.transform.localPosition;
		this.endPosition = this.startPosition;
		this.endPosition.y = this.endPosition.y + 40f;
		this.player = player;
		if (this.Purchased)
		{
			this.SetSprite(ShopSceneItem.SpriteState.Purchased);
		}
		else
		{
			this.SetSprite(ShopSceneItem.SpriteState.Inactive);
		}
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x00116474 File Offset: 0x00114674
	public void SetSprite(ShopSceneItem.SpriteState spriteState)
	{
		this.spriteInactive.enabled = false;
		this.spriteSelected.enabled = false;
		this.spritePurchased.enabled = false;
		switch (spriteState)
		{
		case ShopSceneItem.SpriteState.Inactive:
			this.spriteInactive.enabled = true;
			break;
		case ShopSceneItem.SpriteState.Selected:
			this.spriteSelected.enabled = true;
			break;
		case ShopSceneItem.SpriteState.Purchased:
			this.spritePurchased.enabled = true;
			break;
		}
	}

	// Token: 0x06003CBE RID: 15550 RVA: 0x001164F4 File Offset: 0x001146F4
	public void Select()
	{
		if (this.state != ShopSceneItem.State.Ready)
		{
			return;
		}
		if (!this.Purchased)
		{
			this.SetSprite(ShopSceneItem.SpriteState.Selected);
		}
		this.StopAllCoroutines();
		base.StartCoroutine(this.float_cr(base.transform.localPosition, this.endPosition, this.spriteShadowObject.transform.localScale, this.originalShadowScale * 0.8f));
	}

	// Token: 0x06003CBF RID: 15551 RVA: 0x00116564 File Offset: 0x00114764
	public void Deselect()
	{
		if (this.state != ShopSceneItem.State.Ready)
		{
			return;
		}
		if (!this.Purchased)
		{
			this.SetSprite(ShopSceneItem.SpriteState.Inactive);
		}
		this.StopAllCoroutines();
		base.StartCoroutine(this.float_cr(base.transform.localPosition, this.startPosition, this.spriteShadowObject.transform.localScale, this.originalShadowScale));
	}

	// Token: 0x06003CC0 RID: 15552 RVA: 0x0003110E File Offset: 0x0002F30E
	public void UpdateFloat(float value)
	{
		base.transform.localPosition = Vector3.Lerp(this.startPosition, this.endPosition, value);
	}

	// Token: 0x06003CC1 RID: 15553 RVA: 0x001165CC File Offset: 0x001147CC
	public void UpdatePurchasedColor(float value)
	{
		Color white = Color.white;
		Color black = Color.black;
		this.spritePurchased.color = Color.Lerp(white, black, value);
	}

	// Token: 0x06003CC2 RID: 15554 RVA: 0x001165F8 File Offset: 0x001147F8
	public bool Purchase()
	{
		if (this.state != ShopSceneItem.State.Ready)
		{
			return false;
		}
		if (this.Purchased)
		{
			return false;
		}
		bool flag = false;
		switch (this.itemType)
		{
		case ItemType.Weapon:
			flag = PlayerData.Data.Buy(this.player, this.weapon);
			break;
		case ItemType.Super:
			flag = PlayerData.Data.Buy(this.player, this.super);
			break;
		case ItemType.Charm:
			flag = PlayerData.Data.Buy(this.player, this.charm);
			break;
		}
		if (flag)
		{
			base.StartCoroutine(this.purchase_cr());
			if (ShopScene.Current.HasBoughtEverythingForAchievement(this.player))
			{
				OnlineManager.Instance.Interface.UnlockAchievement(this.player, "BoughtAllItems");
			}
			if (!PlayerData.Data.hasMadeFirstPurchase)
			{
				PlayerData.Data.shouldShowShopkeepTooltip = true;
				PlayerData.Data.hasMadeFirstPurchase = true;
				PlayerData.SaveCurrentFile();
			}
		}
		return flag;
	}

	// Token: 0x06003CC3 RID: 15555 RVA: 0x00116708 File Offset: 0x00114908
	public IEnumerator float_cr(Vector3 start, Vector3 end, Vector3 startShadowScale, Vector3 endShadowScale)
	{
		float t = 0f;
		float time = 0.3f * (Vector3.Distance(start, end) / Vector3.Distance(this.startPosition, this.endPosition));
		while (t < time)
		{
			float val = t / time;
			base.transform.localPosition = Vector3.Lerp(start, end, EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, val));
			this.spriteShadowObject.transform.localScale = Vector3.Lerp(startShadowScale, endShadowScale, EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, val));
			t += base.LocalDeltaTime;
			yield return null;
		}
		base.transform.localPosition = end;
		yield return null;
		yield break;
	}

	// Token: 0x06003CC4 RID: 15556 RVA: 0x00116740 File Offset: 0x00114940
	public IEnumerator purchase_cr()
	{
		this.state = ShopSceneItem.State.Busy;
		this.SetSprite(ShopSceneItem.SpriteState.Purchased);
		SpriteRenderer buyAnim = Object.Instantiate<SpriteRenderer>(this.buyAnimation, base.GetComponentInChildren<SpriteRenderer>().bounds.center, Quaternion.identity);
		buyAnim.sortingOrder = base.GetComponentInChildren<SpriteRenderer>().sortingOrder;
		this.spriteShadowObject.gameObject.SetActive(false);
		yield return base.TweenValue(0f, 1f, 0.0001f, EaseUtils.EaseType.linear, new AbstractMonoBehaviour.TweenUpdateHandler(this.UpdatePurchasedColor));
		this.state = ShopSceneItem.State.Ready;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06003CC5 RID: 15557 RVA: 0x0003112D File Offset: 0x0002F32D
	public void OnDestroy()
	{
		this.buyAnimation = null;
		this.spriteShadow = null;
	}

	// Token: 0x04003024 RID: 12324
	public const float FLOAT_TIME = 0.3f;

	// Token: 0x04003025 RID: 12325
	public ItemType itemType;

	// Token: 0x04003026 RID: 12326
	[Space(5f)]
	public Weapon weapon = Weapon.None;

	// Token: 0x04003027 RID: 12327
	public Super super = Super.None;

	// Token: 0x04003028 RID: 12328
	public Charm charm = Charm.None;

	// Token: 0x04003029 RID: 12329
	[Header("Sprites")]
	public SpriteRenderer spriteInactive;

	// Token: 0x0400302A RID: 12330
	public SpriteRenderer spriteSelected;

	// Token: 0x0400302B RID: 12331
	public SpriteRenderer spritePurchased;

	// Token: 0x0400302C RID: 12332
	public SpriteRenderer spriteShadowObject;

	// Token: 0x0400302D RID: 12333
	public Sprite spriteShadow;

	// Token: 0x0400302E RID: 12334
	public float cantPurchaseYMovementPosition;

	// Token: 0x0400302F RID: 12335
	public float cantPurchaseYMovementValue;

	// Token: 0x04003030 RID: 12336
	public Vector3 poofOffset;

	// Token: 0x04003031 RID: 12337
	[HideInInspector]
	public Vector3 endPosition;

	// Token: 0x04003032 RID: 12338
	public PlayerId player;

	// Token: 0x04003033 RID: 12339
	[HideInInspector]
	public Vector3 startPosition;

	// Token: 0x04003034 RID: 12340
	public Vector3 originalShadowScale;

	// Token: 0x04003035 RID: 12341
	public SpriteRenderer buyAnimation;

	// Token: 0x04003036 RID: 12342
	public Coroutine selectionCoroutine;

	// Token: 0x04003038 RID: 12344
	[SerializeField]
	public bool isDLCItem;

	// Token: 0x0200122D RID: 4653
	public enum State
	{
		// Token: 0x04007DFE RID: 32254
		Ready,
		// Token: 0x04007DFF RID: 32255
		Busy
	}

	// Token: 0x0200122E RID: 4654
	public enum SpriteState
	{
		// Token: 0x04007E01 RID: 32257
		Inactive,
		// Token: 0x04007E02 RID: 32258
		Selected,
		// Token: 0x04007E03 RID: 32259
		Purchased
	}
}
