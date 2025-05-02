using System;
using UnityEngine;

// Token: 0x020004A0 RID: 1184
public class MapSceneLoader : AbstractMapInteractiveEntity
{
	// Token: 0x0600316B RID: 12651 RVA: 0x000E98F4 File Offset: 0x000E7AF4
	public override void Activate(MapPlayerController player)
	{
		base.Activate(player);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		AudioManager.Play("world_map_level_difficulty_appear");
		base.SetPlayerReturnPos();
		if (this.askDifficulty)
		{
			if (this.scene == Scenes.scene_cutscene_kingdice)
			{
				MapDifficultySelectStartUI.Current.level = "DicePalaceMain";
			}
			MapDifficultySelectStartUI.Current.In(player);
			MapDifficultySelectStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
			MapDifficultySelectStartUI.Current.OnBackEvent += this.OnBack;
		}
		else
		{
			if (this.scene == Scenes.scene_map_world_1)
			{
				PlayerData.Data.GetMapData(Scenes.scene_map_world_1).enteringFrom = PlayerData.MapData.EntryMethod.DiceHouseRight;
				MapBasicStartUI.Current.level = "MapWorld_1";
			}
			else if (this.scene == Scenes.scene_map_world_2)
			{
				PlayerData.Data.GetMapData(Scenes.scene_map_world_2).enteringFrom = PlayerData.MapData.EntryMethod.DiceHouseRight;
				MapBasicStartUI.Current.level = "MapWorld_2";
			}
			else if (this.scene == Scenes.scene_map_world_3)
			{
				if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_4)
				{
					MapBasicStartUI.Current.level = "KingDiceToWorld3WorldMap";
				}
				else
				{
					MapBasicStartUI.Current.level = "MapWorld_3";
				}
			}
			else if (this.scene == Scenes.scene_map_world_4)
			{
				MapBasicStartUI.Current.level = "Inkwell";
			}
			else if (this.scene == Scenes.scene_cutscene_kingdice)
			{
				MapBasicStartUI.Current.level = "KingDice";
			}
			MapBasicStartUI.Current.In(player);
			MapBasicStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
			MapBasicStartUI.Current.OnBackEvent += this.OnBack;
		}
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x000E9A9C File Offset: 0x000E7C9C
	public void OnLoadLevel()
	{
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Paused.ToString(), 0.5f);
		AudioNoiseHandler.Instance.BoingSound();
		this.LoadScene();
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x000E9AD4 File Offset: 0x000E7CD4
	public virtual void LoadScene()
	{
		MapDifficultySelectStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapDifficultySelectStartUI.Current.OnBackEvent -= this.OnBack;
		MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
		SceneLoader.LoadScene(this.scene, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x0600316E RID: 12654 RVA: 0x000E9B48 File Offset: 0x000E7D48
	public void OnBack()
	{
		this.ReCheck();
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		MapDifficultySelectStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapDifficultySelectStartUI.Current.OnBackEvent -= this.OnBack;
		MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
		MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x0002920B File Offset: 0x0002740B
	public override void Reset()
	{
		base.Reset();
		this.dialogueProperties = new AbstractUIInteractionDialogue.Properties("ENTER <sprite=0>");
	}

	// Token: 0x040028B7 RID: 10423
	[SerializeField]
	public Scenes scene;

	// Token: 0x040028B8 RID: 10424
	[SerializeField]
	public bool askDifficulty;
}
