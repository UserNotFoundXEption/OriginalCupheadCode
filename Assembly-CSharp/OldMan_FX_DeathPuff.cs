using System;

// Token: 0x020002D4 RID: 724
public class OldMan_FX_DeathPuff : AbstractPausableComponent
{
	// Token: 0x06002026 RID: 8230 RVA: 0x0001B3FA File Offset: 0x000195FA
	public void AnimationEvent_SFX_OMM_GnomeDeathPuff()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_popper_deathpoof");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_popper_deathpoof");
	}
}
