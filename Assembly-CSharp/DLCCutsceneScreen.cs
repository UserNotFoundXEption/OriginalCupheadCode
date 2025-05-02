using System;
using UnityEngine;

// Token: 0x020000B9 RID: 185
public class DLCCutsceneScreen : AbstractMonoBehaviour
{
	// Token: 0x06000885 RID: 2181 RVA: 0x000082CB File Offset: 0x000064CB
	public void AniEvent_ShowText()
	{
		this.cutscene.ShowText();
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x000082D8 File Offset: 0x000064D8
	public void AniEvent_ShowArrow()
	{
		this.cutscene.ShowArrow();
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x000082E5 File Offset: 0x000064E5
	public void AniEvent_IrisIn()
	{
		this.cutscene.IrisIn();
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x000082F2 File Offset: 0x000064F2
	public void AniEvent_IrisOut()
	{
		this.cutscene.IrisOut();
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x000082FF File Offset: 0x000064FF
	public void AnimEvent_SFX_IntroStart_SeagullCall_1()
	{
		AudioManager.Play("sfx_dlc_intro_seagullcall_1");
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x0000830B File Offset: 0x0000650B
	public void AnimEvent_SFX_IntroStart_SeagullCall_2()
	{
		AudioManager.Play("sfx_dlc_intro_seagullcall_2");
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00008317 File Offset: 0x00006517
	public void AnimEvent_SFX_IntroStart_SeagullCall_3()
	{
		AudioManager.Play("sfx_dlc_intro_seagullcall_3");
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00008323 File Offset: 0x00006523
	public void SFX_IntroStart_OceanAmbLoopStart()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_intro_oceanamb_loop", 0.5f, 1f);
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x00008339 File Offset: 0x00006539
	public void SFX_IntroStart_OceanAmbLoopStop()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_intro_oceanamb_loop", 0f, 0.1f);
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x0000834F File Offset: 0x0000654F
	public void AnimEvent_SFX_Intro_ChalliceAppear()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_challiceappear");
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x0000835B File Offset: 0x0000655B
	public void AnimEvent_SFX_Intro_Challiceglows()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_challiceglows");
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x00008367 File Offset: 0x00006567
	public void AnimEvent_SFX_Intro_EatCookie()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_eatcookie");
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x00008373 File Offset: 0x00006573
	public void AnimEvent_SFX_Intro_EnterBakery()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_enterbakery");
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0000837F File Offset: 0x0000657F
	public void AnimEvent_SFX_Intro_FollowChallice()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_followchallice");
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0000838B File Offset: 0x0000658B
	public void AnimEvent_SFX_Intro_Recipeaccept()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_recipeaccept");
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x00008397 File Offset: 0x00006597
	public void AnimEvent_SFX_Intro_SaltBakerRecipe()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_saltbakerrecipe");
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x000083A3 File Offset: 0x000065A3
	public void AnimEvent_SFX_Intro_CookieMagic()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_cookiemagic");
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x000083AF File Offset: 0x000065AF
	public void AnimEvent_SFX_Intro_FirstSwapGhost()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_firstswapghost");
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x000083BB File Offset: 0x000065BB
	public void AnimEvent_SFX_Intro_SecondSwapGhost()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_secondswapghost");
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x000083C7 File Offset: 0x000065C7
	public void AnimEvent_SFX_SaltBakerPre_ChaliceReveal()
	{
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_chalicereveal");
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x000083D3 File Offset: 0x000065D3
	public void AnimEvent_SFX_SaltBakerPre_EndLeanIn()
	{
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_endleanin");
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x000083DF File Offset: 0x000065DF
	public void AnimEvent_SFX_SaltBakerPre_HelpClose()
	{
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_helpclose");
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x000083EB File Offset: 0x000065EB
	public void AnimEvent_SFX_SaltBakerPre_KnifeOakTableLol()
	{
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_knifedefinitelyoaktable");
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x000083F7 File Offset: 0x000065F7
	public void AnimEvent_SFX_SaltBakerPre_KnifeSwipe()
	{
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_knifeswipe");
	}

	// Token: 0x04000682 RID: 1666
	[SerializeField]
	public DLCGenericCutscene cutscene;
}
