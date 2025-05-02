using System;

// Token: 0x0200017F RID: 383
public class ChessBishopLevelBishopIntro : AbstractCollidableObject
{
	// Token: 0x06001234 RID: 4660 RVA: 0x0000F640 File Offset: 0x0000D840
	public void AniEvent_BishopIntroSFX()
	{
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x0000F642 File Offset: 0x0000D842
	public void AnimationEvent_SFX_KOG_Bishop_Intro_Vocal()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_intro_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_intro_vocal");
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x0000F65E File Offset: 0x0000D85E
	public void AnimationEvent_SFX_KOG_Bishop_Intro_Sfx()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_intro_sfx");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_intro_sfx");
	}
}
