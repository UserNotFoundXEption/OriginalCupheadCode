using System;
using UnityEngine;

// Token: 0x02000344 RID: 836
public class RumRunnersLevelGrubIntro : AbstractPausableComponent
{
	// Token: 0x060024AC RID: 9388 RVA: 0x000C4870 File Offset: 0x000C2A70
	public void animationEvent_StartExit()
	{
		Animator component = base.GetComponent<Animator>();
		component.SetLayerWeight(1, 0f);
	}

	// Token: 0x060024AD RID: 9389 RVA: 0x0001EFE7 File Offset: 0x0001D1E7
	public void AnimationEvent_MoveToForeground()
	{
		this.rend.sortingLayerName = "Foreground";
		this.rend.sortingOrder = 200;
	}

	// Token: 0x060024AE RID: 9390 RVA: 0x0001F009 File Offset: 0x0001D209
	public void AnimationEvent_SFX_RUMRUN_FakeAnnouncer_BeginAhhh()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_rumrun_vx_fakeannouncer_begin", 0f, 0.1f);
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_begin_ahhh");
	}

	// Token: 0x060024AF RID: 9391 RVA: 0x0001F029 File Offset: 0x0001D229
	public void AnimationEvent_SFX_RUMRUN_Intro_GrubFliesAway()
	{
		AudioManager.Play("sfx_dlc_rumrun_intro_grubfliesaway");
	}

	// Token: 0x04001E60 RID: 7776
	[SerializeField]
	public SpriteRenderer rend;
}
