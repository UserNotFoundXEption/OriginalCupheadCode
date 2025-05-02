using System;

// Token: 0x0200017B RID: 379
public class ChessCastleLevelKingInteractionPoint : DialogueInteractionPoint
{
	// Token: 0x06001207 RID: 4615 RVA: 0x0000F3BC File Offset: 0x0000D5BC
	public void BeginDialogue()
	{
		this.Activate();
		this.speechBubble.waitForRealease = false;
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x0000F3D0 File Offset: 0x0000D5D0
	public override void StartAnimation()
	{
		((ChessCastleLevel)Level.Current).StartTalkAnimation();
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x0000F3E1 File Offset: 0x0000D5E1
	public override void EndAnimation()
	{
		((ChessCastleLevel)Level.Current).EndTalkAnimation();
	}
}
