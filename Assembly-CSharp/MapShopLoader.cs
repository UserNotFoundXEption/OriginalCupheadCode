using System;
using UnityEngine;

// Token: 0x020004A4 RID: 1188
public class MapShopLoader : AbstractMapInteractiveEntity
{
	// Token: 0x0600317A RID: 12666 RVA: 0x000E9DD4 File Offset: 0x000E7FD4
	public override void Activate(MapPlayerController player)
	{
		if (AbstractMapInteractiveEntity.HasPopupOpened)
		{
			return;
		}
		AbstractMapInteractiveEntity.HasPopupOpened = true;
		base.Activate(player);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		AudioManager.Play("world_map_level_difficulty_appear");
		Map.Current.OnLoadShop();
		PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.playerOne;
		PlayerData.Data.CurrentMapData.playerTwoPosition = base.transform.position + this.returnPositions.playerTwo;
		if (!PlayerManager.Multiplayer)
		{
			PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.singlePlayer;
		}
		MapBasicStartUI.Current.level = "Shop";
		MapBasicStartUI.Current.In(player);
		MapBasicStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent += this.OnBack;
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x0002926B File Offset: 0x0002746B
	public void OnLoadLevel()
	{
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		AudioNoiseHandler.Instance.BoingSound();
		SceneLoader.LoadScene((!this.isDLCShop) ? Scenes.scene_shop : Scenes.scene_shop_DLC, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.None, null);
	}

	// Token: 0x0600317C RID: 12668 RVA: 0x000E9F04 File Offset: 0x000E8104
	public void OnBack()
	{
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		this.ReCheck();
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
	}

	// Token: 0x040028C0 RID: 10432
	[SerializeField]
	public bool isDLCShop;
}
