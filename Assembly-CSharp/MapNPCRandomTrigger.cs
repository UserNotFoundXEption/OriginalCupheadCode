using System;
using UnityEngine;

// Token: 0x0200049E RID: 1182
public class MapNPCRandomTrigger : MonoBehaviour
{
	// Token: 0x06003161 RID: 12641 RVA: 0x00029178 File Offset: 0x00027378
	public void Start()
	{
		this.loopToWait = Random.Range(this.triggerMinFrequency, this.triggerMaxFrequency + 1);
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x00029193 File Offset: 0x00027393
	public void Looped()
	{
		this.loopToWait--;
		if (this.loopToWait <= 0)
		{
			this.loopToWait = Random.Range(this.triggerMinFrequency, this.triggerMaxFrequency + 1);
			this.Trigger();
		}
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x000291CE File Offset: 0x000273CE
	public void Trigger()
	{
		this.animator.SetTrigger(this.trigger);
	}

	// Token: 0x040028AF RID: 10415
	[SerializeField]
	public Animator animator;

	// Token: 0x040028B0 RID: 10416
	[SerializeField]
	public int triggerMinFrequency = 3;

	// Token: 0x040028B1 RID: 10417
	[SerializeField]
	public int triggerMaxFrequency = 5;

	// Token: 0x040028B2 RID: 10418
	public string trigger = "blink";

	// Token: 0x040028B3 RID: 10419
	public int loopToWait;
}
