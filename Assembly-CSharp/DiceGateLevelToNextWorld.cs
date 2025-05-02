using System;
using System.Collections;

// Token: 0x020001C8 RID: 456
public class DiceGateLevelToNextWorld : AbstractLevelInteractiveEntity
{
	// Token: 0x06001570 RID: 5488 RVA: 0x00012395 File Offset: 0x00010595
	public override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.Activate();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x000123B6 File Offset: 0x000105B6
	public override void OnDestroy()
	{
		base.OnDestroy();
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1f);
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x0009C1BC File Offset: 0x0009A3BC
	public IEnumerator go_cr()
	{
		this.activated = true;
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 0f);
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null))
			{
				levelPlayerController.DisableInput();
				levelPlayerController.PauseAll();
			}
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
		{
			if (PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted)
			{
				SceneLoader.LoadScene(Scenes.scene_map_world_2, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
			else
			{
				Cutscene.Load(Scenes.scene_map_world_2, Scenes.scene_cutscene_world2, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass);
			}
		}
		else if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_2)
		{
			if (PlayerData.Data.GetMapData(Scenes.scene_map_world_3).sessionStarted)
			{
				SceneLoader.LoadScene(Scenes.scene_map_world_3, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
			else
			{
				Cutscene.Load(Scenes.scene_map_world_3, Scenes.scene_cutscene_world3, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass);
			}
		}
		yield break;
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x000123C9 File Offset: 0x000105C9
	public override void Show(PlayerId playerId)
	{
		base.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Right, false);
	}

	// Token: 0x0400117E RID: 4478
	public bool activated;
}
