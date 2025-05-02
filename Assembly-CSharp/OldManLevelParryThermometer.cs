using System;

// Token: 0x020002E0 RID: 736
public class OldManLevelParryThermometer : ParrySwitch
{
	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x060020AF RID: 8367 RVA: 0x0001BD25 File Offset: 0x00019F25
	// (set) Token: 0x060020B0 RID: 8368 RVA: 0x0001BD2D File Offset: 0x00019F2D
	public bool isActivated { get; set; }

	// Token: 0x060020B1 RID: 8369 RVA: 0x0001BD36 File Offset: 0x00019F36
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		this.isActivated = true;
		base.OnParryPrePause(player);
	}

	// Token: 0x060020B2 RID: 8370 RVA: 0x0001BD46 File Offset: 0x00019F46
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		this.isActivated = false;
		base.gameObject.SetActive(false);
	}
}
