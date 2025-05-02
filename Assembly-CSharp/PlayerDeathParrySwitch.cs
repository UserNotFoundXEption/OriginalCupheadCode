using System;

// Token: 0x02000506 RID: 1286
public class PlayerDeathParrySwitch : ParrySwitch
{
	// Token: 0x060035C9 RID: 13769 RVA: 0x0002C15C File Offset: 0x0002A35C
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		player.stats.OnParry(1f, true);
	}
}
