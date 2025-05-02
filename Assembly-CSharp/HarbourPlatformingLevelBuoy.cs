using System;
using UnityEngine;

// Token: 0x02000427 RID: 1063
public class HarbourPlatformingLevelBuoy : AbstractPausableComponent
{
	// Token: 0x06002DFC RID: 11772 RVA: 0x00026532 File Offset: 0x00024732
	public void Start()
	{
		this.parrySwitch.OnActivate += this.ParrySoundSFX;
		this.parrySwitch.OnActivate += this.parrySwitch.StartParryCooldown;
	}

	// Token: 0x06002DFD RID: 11773 RVA: 0x000DE214 File Offset: 0x000DC414
	public void PlayIdle()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)) && !AudioManager.CheckIfPlaying("harbour_buoy_idle"))
		{
			AudioManager.Play("harbour_buoy_idle");
			this.emitAudioFromObject.Add("harbour_buoy_idle");
		}
	}

	// Token: 0x06002DFE RID: 11774 RVA: 0x00026567 File Offset: 0x00024767
	public void ParrySoundSFX()
	{
		AudioManager.Play("harbour_buoy_parry");
		this.emitAudioFromObject.Add("harbour_buoy_parry");
	}

	// Token: 0x04002613 RID: 9747
	[SerializeField]
	public ParrySwitch parrySwitch;

	// Token: 0x04002614 RID: 9748
	public const float ON_SCREEN_SOUND_PADDING = 100f;
}
