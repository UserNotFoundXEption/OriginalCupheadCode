using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003C1 RID: 961
public class TutorialLevelDoor : AbstractLevelInteractiveEntity
{
	// Token: 0x06002A67 RID: 10855 RVA: 0x00023B44 File Offset: 0x00021D44
	public override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.Activate();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06002A68 RID: 10856 RVA: 0x00023B65 File Offset: 0x00021D65
	public override void OnDestroy()
	{
		base.OnDestroy();
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1f);
	}

	// Token: 0x06002A69 RID: 10857 RVA: 0x000D3D24 File Offset: 0x000D1F24
	public IEnumerator go_cr()
	{
		this.activated = true;
		LevelCoin.OnLevelComplete();
		if (this.isChaliceTutorial)
		{
			PlayerData.Data.IsChaliceTutorialCompleted = true;
		}
		else
		{
			PlayerData.Data.IsTutorialCompleted = true;
		}
		PlayerData.SaveCurrentFile();
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null))
			{
				levelPlayerController.DisableInput();
				levelPlayerController.PauseAll();
			}
		}
		TutorialLevel level = Level.Current as TutorialLevel;
		if (level)
		{
			level.GoBackToHouse();
		}
		else
		{
			ChaliceTutorialLevel chaliceTutorialLevel = Level.Current as ChaliceTutorialLevel;
			if (chaliceTutorialLevel)
			{
				chaliceTutorialLevel.Exit();
			}
		}
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		if (this.isChaliceTutorial)
		{
			SceneLoader.LoadScene(Scenes.scene_map_world_DLC, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
		else
		{
			SceneLoader.LoadScene(Scenes.scene_level_house_elder_kettle, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
		yield break;
	}

	// Token: 0x0400235F RID: 9055
	public bool activated;

	// Token: 0x04002360 RID: 9056
	[SerializeField]
	public bool isChaliceTutorial;
}
