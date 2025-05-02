using System;

// Token: 0x02000179 RID: 377
public class ChessCastleLevelExit : AbstractLevelInteractiveEntity
{
	// Token: 0x06001202 RID: 4610 RVA: 0x0000F344 File Offset: 0x0000D544
	public override void Activate()
	{
		if (this.activated)
		{
			return;
		}
		base.Activate();
		this.activated = true;
		((ChessCastleLevel)Level.Current).Exit();
	}

	// Token: 0x06001203 RID: 4611 RVA: 0x0000F36E File Offset: 0x0000D56E
	public override void Show(PlayerId playerId)
	{
		base.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Bottom, false);
	}

	// Token: 0x04000E8D RID: 3725
	public bool activated;
}
