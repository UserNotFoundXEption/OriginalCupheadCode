using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000297 RID: 663
public class FrogsLevelWaiter : AbstractMonoBehaviour
{
	// Token: 0x06001DF4 RID: 7668 RVA: 0x000194DE File Offset: 0x000176DE
	public void Start()
	{
		base.StartCoroutine(this.waiter_cr());
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x000B2264 File Offset: 0x000B0464
	public IEnumerator waiter_cr()
	{
		float x = base.transform.localPosition.x;
		for (;;)
		{
			base.transform.SetScale(new float?(1f), null, null);
			yield return base.StartCoroutine(this.move_cr(x, -x));
			yield return CupheadTime.WaitForSeconds(this, 2f);
			base.transform.SetScale(new float?(-1f), null, null);
			yield return base.StartCoroutine(this.move_cr(-x, x));
			yield return CupheadTime.WaitForSeconds(this, 2f);
		}
		yield break;
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x000B2280 File Offset: 0x000B0480
	public IEnumerator move_cr(float start, float end)
	{
		float t = 0f;
		base.transform.SetLocalPosition(new float?(start), null, null);
		while (t < 8f)
		{
			float val = t / 8f;
			base.transform.SetLocalPosition(new float?(Mathf.Lerp(start, end, val)), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetLocalPosition(new float?(end), null, null);
		yield break;
	}

	// Token: 0x04001896 RID: 6294
	public const float TIME = 8f;
}
