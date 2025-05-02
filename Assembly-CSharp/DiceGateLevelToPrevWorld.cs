using System;
using System.Collections;

// Token: 0x020001C9 RID: 457
public class DiceGateLevelToPrevWorld : AbstractLevelInteractiveEntity
{
	// Token: 0x06001575 RID: 5493 RVA: 0x00012403 File Offset: 0x00010603
	public override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.Activate();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06001576 RID: 5494 RVA: 0x00012424 File Offset: 0x00010624
	public override void OnDestroy()
	{
		base.OnDestroy();
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1f);
	}

	// Token: 0x06001577 RID: 5495 RVA: 0x0009C1D8 File Offset: 0x0009A3D8
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
		PlayerData.Data.CurrentMapData.hasVisitedDieHouse = true;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		SceneLoader.LoadLastMap();
		yield break;
	}

	// Token: 0x06001578 RID: 5496 RVA: 0x00012437 File Offset: 0x00010637
	public override void Show(PlayerId playerId)
	{
		base.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Left, false);
	}

	// Token: 0x0400117F RID: 4479
	public bool activated;
}
