using System;
using System.Collections;

// Token: 0x020002B6 RID: 694
public class HouseLevelTutorial : AbstractLevelInteractiveEntity
{
	// Token: 0x06001EFF RID: 7935 RVA: 0x0001A1DB File Offset: 0x000183DB
	public override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.Activate();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x0001A1FC File Offset: 0x000183FC
	public override void OnDestroy()
	{
		base.OnDestroy();
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1f);
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x000B5250 File Offset: 0x000B3450
	public IEnumerator go_cr()
	{
		this.activated = true;
		HouseLevel level = Level.Current as HouseLevel;
		if (level)
		{
			level.StartTutorial();
		}
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		SceneLoader.LoadScene(Scenes.scene_level_tutorial, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		yield break;
	}

	// Token: 0x06001F02 RID: 7938 RVA: 0x0001A20F File Offset: 0x0001840F
	public override void Show(PlayerId playerId)
	{
		base.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Bottom, false);
	}

	// Token: 0x04001959 RID: 6489
	public bool activated;
}
