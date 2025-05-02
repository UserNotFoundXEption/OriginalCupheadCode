using System;

// Token: 0x020004A7 RID: 1191
public class MapTutorialLoader : AbstractMapInteractiveEntity
{
	// Token: 0x06003184 RID: 12676 RVA: 0x000E9FEC File Offset: 0x000E81EC
	public override void Activate(MapPlayerController player)
	{
		base.Activate(player);
		AudioManager.Play("world_map_level_difficulty_appear");
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		Map.Current.OnLoadShop();
		PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.playerOne;
		PlayerData.Data.CurrentMapData.playerTwoPosition = base.transform.position + this.returnPositions.playerTwo;
		if (!PlayerManager.Multiplayer)
		{
			PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.singlePlayer;
		}
		MapBasicStartUI.Current.level = "ElderKettleLevel";
		MapBasicStartUI.Current.In(player);
		MapBasicStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent += this.OnBack;
	}

	// Token: 0x06003185 RID: 12677 RVA: 0x000292C6 File Offset: 0x000274C6
	public void OnLoadLevel()
	{
		AudioNoiseHandler.Instance.BoingSound();
		SceneLoader.LoadScene(Scenes.scene_level_house_elder_kettle, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x06003186 RID: 12678 RVA: 0x000292DD File Offset: 0x000274DD
	public void OnBack()
	{
		this.ReCheck();
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
	}
}
