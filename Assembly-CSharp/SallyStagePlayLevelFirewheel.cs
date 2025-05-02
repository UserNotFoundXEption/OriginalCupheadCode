using System;

// Token: 0x0200035C RID: 860
public class SallyStagePlayLevelFirewheel : AbstractPausableComponent
{
	// Token: 0x06002600 RID: 9728 RVA: 0x0001FE4C File Offset: 0x0001E04C
	public void PlaySound()
	{
		AudioManager.Play("sally_cherub_fireprop_move");
		this.emitAudioFromObject.Add("sally_cherub_fireprop_move");
	}
}
