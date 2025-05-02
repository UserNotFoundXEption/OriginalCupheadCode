using System;

// Token: 0x0200034D RID: 845
public class RumRunnersLevelPh2StartAnimation : AbstractPausableComponent
{
	// Token: 0x1700030D RID: 781
	// (get) Token: 0x060024FF RID: 9471 RVA: 0x0001F378 File Offset: 0x0001D578
	// (set) Token: 0x06002500 RID: 9472 RVA: 0x0001F380 File Offset: 0x0001D580
	public bool showBug { get; set; }

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x06002501 RID: 9473 RVA: 0x0001F389 File Offset: 0x0001D589
	// (set) Token: 0x06002502 RID: 9474 RVA: 0x0001F391 File Offset: 0x0001D591
	public bool dropped { get; set; }

	// Token: 0x06002503 RID: 9475 RVA: 0x0001F39A File Offset: 0x0001D59A
	public void animationEvent_StartWeb()
	{
		base.animator.Play("Loop", 1);
	}

	// Token: 0x06002504 RID: 9476 RVA: 0x0001F3AD File Offset: 0x0001D5AD
	public void animationEvent_EndWeb()
	{
		base.animator.Play("Off", 1);
	}

	// Token: 0x06002505 RID: 9477 RVA: 0x0001F3C0 File Offset: 0x0001D5C0
	public void animationEvent_ShowBug()
	{
		this.showBug = true;
	}

	// Token: 0x06002506 RID: 9478 RVA: 0x0001F3C9 File Offset: 0x0001D5C9
	public void animationEvent_RopeDrop()
	{
		this.dropped = true;
	}

	// Token: 0x06002507 RID: 9479 RVA: 0x0001F3D2 File Offset: 0x0001D5D2
	public void AnimationEvent_SFX_RUMRUN_ExitPhase1_SpiderReturns()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_ExitPhase1_SpiderReturns");
		this.emitAudioFromObject.Add("sfx_DLC_RUMRUN_ExitPhase1_SpiderReturns");
	}

	// Token: 0x06002508 RID: 9480 RVA: 0x0001F3EE File Offset: 0x0001D5EE
	public void AnimationEvent_SFX_RUMRUN_ExitPhase1_GrammoDrop()
	{
		AudioManager.Play("sfx_dlc_rumrun_exitphase1_grammodrop");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_exitphase1_grammodrop");
	}
}
