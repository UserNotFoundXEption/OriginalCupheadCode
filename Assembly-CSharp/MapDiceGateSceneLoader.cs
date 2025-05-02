using System;
using UnityEngine;

// Token: 0x0200047A RID: 1146
public class MapDiceGateSceneLoader : AbstractMapInteractiveEntity
{
	// Token: 0x0600308F RID: 12431 RVA: 0x000E69C8 File Offset: 0x000E4BC8
	public override void Activate(MapPlayerController player)
	{
		base.Activate(player);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		AudioManager.Play("world_map_level_difficulty_appear");
		Map.Current.OnLoadLevel();
		base.SetPlayerReturnPos();
		if (this.nextWorld == Scenes.scene_map_world_1)
		{
			MapBasicStartUI.Current.level = "MapWorld_1";
		}
		else if (this.nextWorld == Scenes.scene_map_world_2)
		{
			MapBasicStartUI.Current.level = ((!PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted) ? "DieHouse" : "MapWorld_2");
		}
		else if (this.nextWorld == Scenes.scene_map_world_3)
		{
			MapBasicStartUI.Current.level = ((!PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted) ? "DieHouse" : "MapWorld_3");
		}
		else if (this.nextWorld == Scenes.scene_map_world_4)
		{
			MapBasicStartUI.Current.level = "Inkwell";
		}
		MapBasicStartUI.Current.In(player);
		MapBasicStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent += this.OnBack;
	}

	// Token: 0x06003090 RID: 12432 RVA: 0x000E6AF0 File Offset: 0x000E4CF0
	public void OnLoadLevel()
	{
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Paused.ToString(), 0.5f);
		AudioNoiseHandler.Instance.BoingSound();
		this.CheckSceneToLoad();
	}

	// Token: 0x06003091 RID: 12433 RVA: 0x000E6B28 File Offset: 0x000E4D28
	public void CheckSceneToLoad()
	{
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
		{
			if (PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted)
			{
				PlayerData.Data.GetMapData(Scenes.scene_map_world_2).enteringFrom = PlayerData.MapData.EntryMethod.DiceHouseLeft;
				SceneLoader.LoadScene(this.nextWorld, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
			else
			{
				SceneLoader.LoadScene(this.diceGate, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
		}
		else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_2)
		{
			if (PlayerData.Data.GetMapData(Scenes.scene_map_world_3).sessionStarted)
			{
				PlayerData.Data.GetMapData(Scenes.scene_map_world_3).enteringFrom = PlayerData.MapData.EntryMethod.DiceHouseLeft;
				SceneLoader.LoadScene(this.nextWorld, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
			else
			{
				SceneLoader.LoadScene(this.diceGate, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
		}
	}

	// Token: 0x06003092 RID: 12434 RVA: 0x000284C7 File Offset: 0x000266C7
	public void OnBack()
	{
		this.ReCheck();
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
	}

	// Token: 0x06003093 RID: 12435 RVA: 0x00028503 File Offset: 0x00026703
	public override void Reset()
	{
		base.Reset();
		this.dialogueProperties = new AbstractUIInteractionDialogue.Properties("ENTER <sprite=0>");
	}

	// Token: 0x04002829 RID: 10281
	[SerializeField]
	public Scenes nextWorld;

	// Token: 0x0400282A RID: 10282
	public readonly Scenes diceGate = Scenes.scene_level_dice_gate;

	// Token: 0x0400282B RID: 10283
	[SerializeField]
	public bool askDifficulty;
}
