using System;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class ClownLevelCoasterKnob : ParrySwitch
{
	// Token: 0x060013E8 RID: 5096 RVA: 0x00010B8F File Offset: 0x0000ED8F
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		player.stats.ParryOneQuarter();
		this.sprite.GetComponent<SpriteRenderer>().enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x04001031 RID: 4145
	[SerializeField]
	public SpriteRenderer sprite;
}
