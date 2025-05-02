using System;
using UnityEngine;

// Token: 0x0200059E RID: 1438
public class ShopScenePig : AbstractMonoBehaviour
{
	// Token: 0x06003CC7 RID: 15559 RVA: 0x0011675C File Offset: 0x0011495C
	public void OnIdleLoop()
	{
		this.idleLoops++;
		if (this.idleLoops >= this.idleLoopsMax)
		{
			base.animator.SetTrigger("OnClock");
			this.idleLoopsMax = Random.Range(20, 35);
			this.idleLoops = 0;
		}
	}

	// Token: 0x06003CC8 RID: 15560 RVA: 0x0003114D File Offset: 0x0002F34D
	public void OnStart()
	{
		AudioManager.Play("shop_pig_welcome");
		base.animator.Play("Welcome");
	}

	// Token: 0x06003CC9 RID: 15561 RVA: 0x00031169 File Offset: 0x0002F369
	public void OnPurchase()
	{
		AudioManager.Play("shop_pig_nod");
		base.animator.Play("Nod");
	}

	// Token: 0x06003CCA RID: 15562 RVA: 0x00031185 File Offset: 0x0002F385
	public void OnExit()
	{
		AudioManager.Play("shop_pig_bye");
		base.animator.Play("Bye");
	}

	// Token: 0x04003039 RID: 12345
	public const int CLOCK_LOOPS_MIN = 20;

	// Token: 0x0400303A RID: 12346
	public const int CLOCK_LOOPS_MAX = 35;

	// Token: 0x0400303B RID: 12347
	public int idleLoopsMax = 35;

	// Token: 0x0400303C RID: 12348
	public int idleLoops;
}
