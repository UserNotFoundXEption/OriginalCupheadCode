using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001EB RID: 491
public class DicePalaceFlyingHorseLevelLights : AbstractPausableComponent
{
	// Token: 0x060016B3 RID: 5811 RVA: 0x0001353C File Offset: 0x0001173C
	public void Start()
	{
		base.FrameDelayedCallback(new Action(this.GetSprites), 1);
	}

	// Token: 0x060016B4 RID: 5812 RVA: 0x00013552 File Offset: 0x00011752
	public void GetSprites()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060016B5 RID: 5813 RVA: 0x0009F8D0 File Offset: 0x0009DAD0
	public IEnumerator move_cr()
	{
		for (;;)
		{
			if (base.transform.position.x > -640f - this.size)
			{
				base.transform.position += Vector3.left * this.speed * CupheadTime.Delta;
			}
			else
			{
				base.transform.position = new Vector3(640f + this.size, base.transform.position.y, 0f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001271 RID: 4721
	[SerializeField]
	public float size = 500f;

	// Token: 0x04001272 RID: 4722
	[SerializeField]
	public float speed = 30f;
}
