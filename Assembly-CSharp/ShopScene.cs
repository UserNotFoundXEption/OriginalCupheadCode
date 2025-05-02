using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200059A RID: 1434
public class ShopScene : AbstractMonoBehaviour
{
	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06003C9E RID: 15518 RVA: 0x00030F9A File Offset: 0x0002F19A
	// (set) Token: 0x06003C9F RID: 15519 RVA: 0x00030FA1 File Offset: 0x0002F1A1
	public static ShopScene Current { get; set; }

	// Token: 0x06003CA0 RID: 15520 RVA: 0x00115E5C File Offset: 0x0011405C
	public override void Awake()
	{
		base.Awake();
		Cuphead.Init(false);
		ShopScene.Current = this;
		this.playerOne.OnPurchaseEvent += this.OnPurchase;
		this.playerTwo.OnPurchaseEvent += this.OnPurchase;
		this.playerOne.OnExitEvent += this.OnExit;
		this.playerTwo.OnExitEvent += this.OnExit;
		SceneLoader.OnFadeOutEndEvent += this.OnLoaded;
	}

	// Token: 0x06003CA1 RID: 15521 RVA: 0x00030FA9 File Offset: 0x0002F1A9
	public void OnDestroy()
	{
		if (ShopScene.Current == this)
		{
			ShopScene.Current = null;
		}
		SceneLoader.OnFadeOutEndEvent -= this.OnLoaded;
	}

	// Token: 0x06003CA2 RID: 15522 RVA: 0x00030FD2 File Offset: 0x0002F1D2
	public void OnLoaded()
	{
		this.pig.OnStart();
		this.playerOne.OnStart();
		this.playerTwo.OnStart();
		InterruptingPrompt.SetCanInterrupt(true);
	}

	// Token: 0x06003CA3 RID: 15523 RVA: 0x00030FFB File Offset: 0x0002F1FB
	public void OnPurchase()
	{
		this.pig.OnPurchase();
	}

	// Token: 0x06003CA4 RID: 15524 RVA: 0x00115EE8 File Offset: 0x001140E8
	public void OnExit()
	{
		if ((!this.playerOne.gameObject.activeInHierarchy || this.playerOne.state == ShopScenePlayer.State.Exiting || this.playerOne.state == ShopScenePlayer.State.Exited) && (!this.playerTwo.gameObject.activeInHierarchy || this.playerTwo.state == ShopScenePlayer.State.Exiting || this.playerTwo.state == ShopScenePlayer.State.Exited))
		{
			base.StartCoroutine(this.exit_cr());
			this.playerOne.OnExit();
			this.playerTwo.OnExit();
		}
	}

	// Token: 0x06003CA5 RID: 15525 RVA: 0x00115F88 File Offset: 0x00114188
	public IEnumerator exit_cr()
	{
		if (this.HasBoughtEverythingForAchievement(PlayerId.PlayerOne))
		{
			OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerOne, "BoughtAllItems");
		}
		if (this.HasBoughtEverythingForAchievement(PlayerId.PlayerTwo))
		{
			OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerTwo, "BoughtAllItems");
		}
		this.pig.OnExit();
		yield return this.pig.animator.WaitForAnimationToEnd(this, "Bye", false, true);
		SceneLoader.LoadLastMap();
		yield break;
	}

	// Token: 0x06003CA6 RID: 15526 RVA: 0x00031008 File Offset: 0x0002F208
	public ShopSceneItem[] GetCharmItems(PlayerId player)
	{
		if (player == PlayerId.PlayerTwo)
		{
			return this.playerTwo.GetCharmItemPrefabs();
		}
		return this.playerOne.GetCharmItemPrefabs();
	}

	// Token: 0x06003CA7 RID: 15527 RVA: 0x00031028 File Offset: 0x0002F228
	public ShopSceneItem[] GetWeaponItems(PlayerId player)
	{
		if (player == PlayerId.PlayerTwo)
		{
			return this.playerTwo.GetWeaponItemPrefabs();
		}
		return this.playerOne.GetWeaponItemPrefabs();
	}

	// Token: 0x06003CA8 RID: 15528 RVA: 0x00115FA4 File Offset: 0x001141A4
	public bool HasBoughtEverythingForAchievement(PlayerId player)
	{
		ShopSceneItem[] array = this.GetCharmItems(player);
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].isPurchasedForBuyAllItemsAchievement(player))
			{
				return false;
			}
		}
		array = this.GetWeaponItems(player);
		for (int j = 0; j < array.Length; j++)
		{
			if (!array[j].isPurchasedForBuyAllItemsAchievement(player))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04003013 RID: 12307
	[SerializeField]
	public ShopScenePlayer playerOne;

	// Token: 0x04003014 RID: 12308
	[SerializeField]
	public ShopScenePlayer playerTwo;

	// Token: 0x04003015 RID: 12309
	[Space(10f)]
	[SerializeField]
	public ShopScenePig pig;

	// Token: 0x04003016 RID: 12310
	public bool isDLCShop;
}
