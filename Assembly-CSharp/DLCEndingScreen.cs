using System;

// Token: 0x020000BB RID: 187
public class DLCEndingScreen : DLCCutsceneScreen
{
	// Token: 0x060008AC RID: 2220 RVA: 0x00008472 File Offset: 0x00006672
	public void AniEvent_SwapChars()
	{
		((DLCEndingCutscene)this.cutscene).SwapChars();
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x00008484 File Offset: 0x00006684
	public new void AniEvent_IrisOut()
	{
		((DLCEndingCutscene)this.cutscene).IrisOut();
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x00008496 File Offset: 0x00006696
	public void AniEvent_StartShake()
	{
		((DLCEndingCutscene)this.cutscene).StartShake();
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x000084A8 File Offset: 0x000066A8
	public void AniEvent_StopShake()
	{
		((DLCEndingCutscene)this.cutscene).StopShake();
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x000084BA File Offset: 0x000066BA
	public void AniEvent_AdvanceMusic()
	{
		((DLCEndingCutscene)this.cutscene).AdvanceMusic();
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x000084CC File Offset: 0x000066CC
	public void AniEvent_ActivateChaliceRArm()
	{
		base.animator.Play("ChaliceArmRLoop", 1, 0f);
		base.animator.Update(0f);
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x000084F4 File Offset: 0x000066F4
	public void AniEvent_LowerChaliceRArm()
	{
		base.animator.SetTrigger("Arm");
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x00008506 File Offset: 0x00006706
	public void AniEvent_HideChaliceRArm()
	{
		base.animator.Play("None", 1, 0f);
		base.animator.Update(0f);
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0000852E File Offset: 0x0000672E
	public void AnimEvent_SFX_Ending_BakeryGoesDown()
	{
		AudioManager.Play("sfx_DLC_Cutscene_Ending_BakeryGoesDown");
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0000853A File Offset: 0x0000673A
	public void AnimEvent_SFX_Ending_ChaliceGlassBreak()
	{
		AudioManager.Play("sfx_DLC_Cutscene_Ending_ChaliceGlassBreak");
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x00008546 File Offset: 0x00006746
	public void AnimEvent_SFX_Ending_ChaliceGlassShake()
	{
		AudioManager.Play("sfx_DLC_Cutscene_Ending_ChaliceGlassShake");
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00008552 File Offset: 0x00006752
	public void AnimEvent_SFX_Ending_ChaliceWink()
	{
		AudioManager.Play("sfx_DLC_Cutscene_Ending_ChaliceWink");
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x0000855E File Offset: 0x0000675E
	public void AnimEvent_SFX_Ending_ChaliceHug()
	{
		AudioManager.Play("sfx_dlc_cutscene_ending_chalicehug");
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x0000856A File Offset: 0x0000676A
	public void AnimEvent_SFX_Ending_CollapsingBegins()
	{
		AudioManager.Play("sfx_DLC_Cutscene_Ending_CollapsingBegins");
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x00008576 File Offset: 0x00006776
	public void AnimEvent_SFX_Ending_EscapingBaker()
	{
		AudioManager.Play("sfx_DLC_Cutscene_Ending_EscapingBaker");
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x00008582 File Offset: 0x00006782
	public void AnimEvent_SFX_Ending_EscapingGroup()
	{
		AudioManager.Play("sfx_DLC_Cutscene_Ending_EscapingGroup");
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x0000858E File Offset: 0x0000678E
	public void AnimEvent_SFX_Ending_BakerSit()
	{
		AudioManager.Play("sfx_DLC_Cutscene_Ending_BakerSit");
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x0000859A File Offset: 0x0000679A
	public void AnimEvent_SFX_Ending_RumbleLoopStart()
	{
		AudioManager.PlayLoop("sfx_DLC_Cutscene_Ending_Rumble_Loop");
		AudioManager.FadeSFXVolume("sfx_DLC_Cutscene_Ending_Rumble_Loop", 0.2f, 3f);
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x000085BA File Offset: 0x000067BA
	public void AnimEvent_SFX_Ending_RumbleLoopStop()
	{
		AudioManager.FadeSFXVolume("sfx_DLC_Cutscene_Ending_Rumble_Loop", 0f, 3f);
		AudioManager.Stop("sfx_DLC_Cutscene_Ending_Rumble_Loop");
	}
}
