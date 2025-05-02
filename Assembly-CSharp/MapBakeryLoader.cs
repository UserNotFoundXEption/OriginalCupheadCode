using System;

// Token: 0x02000476 RID: 1142
public class MapBakeryLoader : AbstractMapInteractiveEntity
{
	// Token: 0x06003078 RID: 12408 RVA: 0x000283B7 File Offset: 0x000265B7
	public void Start()
	{
		if (PlayerData.Data.shouldShowChaliceTooltip)
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Chalice);
			PlayerData.Data.shouldShowChaliceTooltip = false;
		}
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x000E6254 File Offset: 0x000E4454
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
		PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.playerOne;
		PlayerData.Data.CurrentMapData.playerTwoPosition = base.transform.position + this.returnPositions.playerTwo;
		if (!PlayerManager.Multiplayer)
		{
			PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.singlePlayer;
		}
		if (PlayerData.Data.GetLevelData(Levels.Saltbaker).played && !this.HoldingLAndR())
		{
			MapDifficultySelectStartUI.Current.level = "Saltbaker";
			MapDifficultySelectStartUI.Current.In(player);
			MapDifficultySelectStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
			MapDifficultySelectStartUI.Current.OnBackEvent += this.OnBack;
			this.loadKitchen = false;
		}
		else
		{
			MapBasicStartUI.Current.level = "BakeryWorldMap";
			MapBasicStartUI.Current.In(player);
			MapBasicStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
			MapBasicStartUI.Current.OnBackEvent += this.OnBack;
			this.loadKitchen = true;
		}
	}

	// Token: 0x0600307A RID: 12410 RVA: 0x000E63F4 File Offset: 0x000E45F4
	public bool HoldingLAndR()
	{
		return (Map.Current.players[0] && Map.Current.players[0].input.actions.GetButton(11) && Map.Current.players[0].input.actions.GetButton(12)) || (Map.Current.players[1] && Map.Current.players[1].input.actions.GetButton(11) && Map.Current.players[1].input.actions.GetButton(12));
	}

	// Token: 0x0600307B RID: 12411 RVA: 0x000283DE File Offset: 0x000265DE
	public void OnLoadLevel()
	{
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		AudioNoiseHandler.Instance.BoingSound();
		if (this.loadKitchen)
		{
			SceneLoader.LoadScene(Scenes.scene_level_kitchen, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.None, null);
		}
		else
		{
			SceneLoader.LoadScene(Scenes.scene_level_saltbaker, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
	}

	// Token: 0x0600307C RID: 12412 RVA: 0x000E64BC File Offset: 0x000E46BC
	public void OnBack()
	{
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		this.ReCheck();
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		MapDifficultySelectStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapDifficultySelectStartUI.Current.OnBackEvent -= this.OnBack;
		MapConfirmStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapConfirmStartUI.Current.OnBackEvent -= this.OnBack;
		MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
	}

	// Token: 0x04002818 RID: 10264
	public MapPlayerController player1;

	// Token: 0x04002819 RID: 10265
	public MapPlayerController player2;

	// Token: 0x0400281A RID: 10266
	public bool p1InTrigger;

	// Token: 0x0400281B RID: 10267
	public bool p2InTrigger;

	// Token: 0x0400281C RID: 10268
	public bool loadKitchen;
}
