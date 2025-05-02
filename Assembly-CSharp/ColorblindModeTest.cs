using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000100 RID: 256
[RequireComponent(typeof(SpriteRenderer))]
public class ColorblindModeTest : AbstractMonoBehaviour
{
	// Token: 0x06000BE0 RID: 3040 RVA: 0x0000A809 File Offset: 0x00008A09
	public void Start()
	{
		this.mat = base.GetComponent<SpriteRenderer>().material;
		base.StartCoroutine(this.flash_cr());
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x00081B20 File Offset: 0x0007FD20
	public IEnumerator flash_cr()
	{
		bool goingUp = true;
		float valMin = 0f;
		float valMax = 2f;
		float start = valMin;
		float end = valMax;
		float t = 0f;
		float time = 0.2f;
		for (;;)
		{
			while (t < time)
			{
				t += CupheadTime.Delta;
				this.mat.SetFloat("_Intensity", Mathf.Lerp(start, end, t / time));
				yield return null;
			}
			this.mat.SetFloat("_Intensity", end);
			goingUp = !goingUp;
			start = ((!goingUp) ? valMax : valMin);
			end = ((!goingUp) ? valMin : valMax);
			t = 0f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400097F RID: 2431
	public Material mat;
}
