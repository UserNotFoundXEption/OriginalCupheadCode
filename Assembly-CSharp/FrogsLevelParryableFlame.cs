using System;

// Token: 0x020002A1 RID: 673
public class FrogsLevelParryableFlame : ParrySwitch
{
	// Token: 0x06001E53 RID: 7763 RVA: 0x00019904 File Offset: 0x00017B04
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		player.stats.ParryOneQuarter();
	}
}
