using System;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class AirplaneLevelLaser : ParrySwitch
{
	// Token: 0x06000E17 RID: 3607 RVA: 0x0000C047 File Offset: 0x0000A247
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		player.stats.ParryOneQuarter();
	}

	// Token: 0x06000E18 RID: 3608 RVA: 0x0000C05B File Offset: 0x0000A25B
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		base.StartParryCooldown();
		this.anim.Play("End");
	}

	// Token: 0x04000B2D RID: 2861
	[SerializeField]
	public Animator anim;
}
