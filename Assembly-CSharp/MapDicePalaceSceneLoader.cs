using System;

// Token: 0x0200047B RID: 1147
public class MapDicePalaceSceneLoader : MapSceneLoader
{
	// Token: 0x06003095 RID: 12437 RVA: 0x00028523 File Offset: 0x00026723
	public override void LoadScene()
	{
		if (!PlayerData.Data.GetLevelData(Levels.DicePalaceMain).played)
		{
			SceneLoader.LoadScene(this.scene, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
		else
		{
			SceneLoader.LoadScene(Scenes.scene_level_dice_palace_main, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
	}
}
