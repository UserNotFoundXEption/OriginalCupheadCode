using System;

// Token: 0x0200017C RID: 380
public class ChessCastleLevelStart : AbstractLevelInteractiveEntity
{
	// Token: 0x0600120B RID: 4619 RVA: 0x0000F3FA File Offset: 0x0000D5FA
	public override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		this.activated = true;
		base.Activate();
		((ChessCastleLevel)Level.Current).StartChessLevel();
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x0000F424 File Offset: 0x0000D624
	public override void Show(PlayerId playerId)
	{
		base.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Bottom, false);
	}

	// Token: 0x04000E8E RID: 3726
	public bool activated;
}
