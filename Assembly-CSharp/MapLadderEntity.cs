using System;
using UnityEngine;

// Token: 0x0200047F RID: 1151
public class MapLadderEntity : AbstractMapInteractiveEntity
{
	// Token: 0x060030BD RID: 12477 RVA: 0x00028730 File Offset: 0x00026930
	public void Init(MapPlayerLadderObject playerLadder, Vector2 exit, MapLadder.Location location)
	{
		this.playerLadder = playerLadder;
		this.exit = base.transform.position + exit;
		this.location = location;
	}

	// Token: 0x060030BE RID: 12478 RVA: 0x000E7538 File Offset: 0x000E5738
	public override MapUIInteractionDialogue Show(PlayerInput player)
	{
		switch (base.playerChecking.state)
		{
		case MapPlayerController.State.Walking:
		case MapPlayerController.State.LadderExit:
			this.dialogueProperties = MapLadder.DIALOGUE_ENTER;
			break;
		case MapPlayerController.State.LadderEnter:
		case MapPlayerController.State.Ladder:
			this.dialogueProperties = MapLadder.DIALOGUE_EXIT;
			break;
		}
		return base.Show(player);
	}

	// Token: 0x060030BF RID: 12479 RVA: 0x000E7594 File Offset: 0x000E5794
	public override void Activate()
	{
		base.Activate();
		MapPlayerController.State state = base.playerActivating.state;
		if (state != MapPlayerController.State.Walking)
		{
			if (state == MapPlayerController.State.Ladder)
			{
				base.playerActivating.LadderExit(base.transform.position, this.exit, this.location);
			}
		}
		else
		{
			base.playerActivating.LadderEnter(base.transform.position, this.playerLadder, this.location);
		}
	}

	// Token: 0x04002842 RID: 10306
	public MapPlayerLadderObject playerLadder;

	// Token: 0x04002843 RID: 10307
	public Vector2 exit;

	// Token: 0x04002844 RID: 10308
	public MapLadder.Location location;
}
