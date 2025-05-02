using System;
using UnityEngine;

// Token: 0x02000482 RID: 1154
public class MapLevelLoader : AbstractMapInteractiveEntity
{
	// Token: 0x060030CC RID: 12492 RVA: 0x0002887B File Offset: 0x00026A7B
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x060030CD RID: 12493 RVA: 0x000E76B4 File Offset: 0x000E58B4
	public override void Activate(MapPlayerController player)
	{
		if (AbstractMapInteractiveEntity.HasPopupOpened)
		{
			return;
		}
		if (PlatformHelper.ManuallyRefreshDLCAvailability)
		{
			DLCManager.CheckInstallationStatusChanged();
			if (DLCManager.showAvailabilityPrompt)
			{
				DLCManager.ResetAvailabilityPrompt();
				MapEventNotification.Current.ShowEvent(MapEventNotification.Type.DLCAvailable);
				return;
			}
		}
		AbstractMapInteractiveEntity.HasPopupOpened = true;
		base.Activate(player);
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		AudioManager.Play("world_map_level_difficulty_appear");
		Map.Current.OnLoadLevel();
		PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.playerOne;
		PlayerData.Data.CurrentMapData.playerTwoPosition = base.transform.position + this.returnPositions.playerTwo;
		if (!PlayerManager.Multiplayer)
		{
			PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.singlePlayer;
		}
		if (this.askDifficulty)
		{
			MapDifficultySelectStartUI.Current.level = this.level.ToString();
			MapDifficultySelectStartUI.Current.In(player);
			MapDifficultySelectStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
			MapDifficultySelectStartUI.Current.OnBackEvent += this.OnBack;
		}
		else
		{
			string a = this.level.ToString();
			if (a != "Mausoleum" && a != "Devil" && a != "ShmupTutorial" && a != "ChaliceTutorial" && a != "ChessCastle")
			{
				MapConfirmStartUI.Current.level = a;
				MapConfirmStartUI.Current.In(player);
				MapConfirmStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
				MapConfirmStartUI.Current.OnBackEvent += this.OnBack;
				return;
			}
			if (a == "Mausoleum")
			{
				if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
				{
					a = "Mausoleum_1";
				}
				else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_2)
				{
					a = "Mausoleum_2";
				}
				else
				{
					a = "Mausoleum_3";
				}
			}
			else if (a == "ChessCastle")
			{
				a = "KingOfGamesWorldMap";
			}
			MapBasicStartUI.Current.level = a;
			MapBasicStartUI.Current.In(player);
			MapBasicStartUI.Current.OnLoadLevelEvent += this.OnLoadLevel;
			MapBasicStartUI.Current.OnBackEvent += this.OnBack;
		}
	}

	// Token: 0x060030CE RID: 12494 RVA: 0x000E7978 File Offset: 0x000E5B78
	public void OnLoadLevel()
	{
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Paused.ToString(), 0.5f);
		AudioNoiseHandler.Instance.BoingSound();
		if (this.level == Levels.Devil)
		{
			SceneLoader.LoadScene(Scenes.scene_cutscene_devil, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
		else
		{
			SceneLoader.LoadLevel(this.level, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
	}

	// Token: 0x060030CF RID: 12495 RVA: 0x000E79DC File Offset: 0x000E5BDC
	public void OnBack()
	{
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		this.ReCheck();
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		if (this.askDifficulty)
		{
			MapDifficultySelectStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
			MapDifficultySelectStartUI.Current.OnBackEvent -= this.OnBack;
		}
		else
		{
			MapConfirmStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
			MapConfirmStartUI.Current.OnBackEvent -= this.OnBack;
			MapBasicStartUI.Current.OnLoadLevelEvent -= this.OnLoadLevel;
			MapBasicStartUI.Current.OnBackEvent -= this.OnBack;
		}
		if (this.OnBackCallback != null)
		{
			this.OnBackCallback();
			this.OnBackCallback = (Action)Delegate.Remove(this.OnBackCallback, this.OnBackCallback);
		}
	}

	// Token: 0x060030D0 RID: 12496 RVA: 0x00028883 File Offset: 0x00026A83
	public override void Reset()
	{
		base.Reset();
		this.dialogueProperties = new AbstractUIInteractionDialogue.Properties("ENTER <sprite=0>");
	}

	// Token: 0x0400284E RID: 10318
	[SerializeField]
	public Levels level;

	// Token: 0x0400284F RID: 10319
	[SerializeField]
	public bool askDifficulty;

	// Token: 0x04002850 RID: 10320
	public Action OnBackCallback;
}
