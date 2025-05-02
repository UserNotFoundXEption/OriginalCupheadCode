using System;

// Token: 0x0200032D RID: 813
public class RobotLevelRobotAnimator : AbstractPausableComponent
{
	// Token: 0x06002374 RID: 9076 RVA: 0x0001E00A File Offset: 0x0001C20A
	public void ContinueMainAnimation()
	{
		base.animator.SetTrigger("StartMainAnim");
	}

	// Token: 0x06002375 RID: 9077 RVA: 0x0001E01C File Offset: 0x0001C21C
	public void SyncAnimationLayers()
	{
		base.animator.SetTrigger("SyncLayers");
	}

	// Token: 0x06002376 RID: 9078 RVA: 0x0001E02E File Offset: 0x0001C22E
	public void MainAnimationStateOff()
	{
		base.animator.SetBool("MainAnimationActive", false);
	}

	// Token: 0x06002377 RID: 9079 RVA: 0x0001E041 File Offset: 0x0001C241
	public void MainAnimationStateOn()
	{
		base.animator.SetBool("MainAnimationActive", true);
	}
}
