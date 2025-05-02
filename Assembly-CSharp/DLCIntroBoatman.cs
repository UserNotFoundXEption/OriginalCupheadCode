using System;

// Token: 0x020000BE RID: 190
public class DLCIntroBoatman : AbstractPausableComponent
{
	// Token: 0x060008CF RID: 2255 RVA: 0x00008676 File Offset: 0x00006876
	public void AniEvent_Paddle_SFX()
	{
		AudioManager.Play("sfx_DLC_Intro_PaddleWater");
		this.emitAudioFromObject.Add("sfx_DLC_Intro_PaddleWater");
	}
}
