using System;
using UnityEngine;

// Token: 0x02000220 RID: 544
public class FlowerLevelFlowerAnimator : AbstractPausableComponent
{
	// Token: 0x060018FE RID: 6398 RVA: 0x0001557B File Offset: 0x0001377B
	public void OnIdleEnd()
	{
		if (this.loops >= this.max)
		{
			this.OnBlink();
			return;
		}
		this.loops++;
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x000155A3 File Offset: 0x000137A3
	public void OnBlink()
	{
		base.animator.SetTrigger("OnBlink");
		this.max = Random.Range(2, 5);
		this.loops = 0;
	}

	// Token: 0x04001425 RID: 5157
	public const int MIN_IDLE_LOOPS = 2;

	// Token: 0x04001426 RID: 5158
	public const int MAX_IDLE_LOOPS = 4;

	// Token: 0x04001427 RID: 5159
	public int loops;

	// Token: 0x04001428 RID: 5160
	public int max = 2;
}
