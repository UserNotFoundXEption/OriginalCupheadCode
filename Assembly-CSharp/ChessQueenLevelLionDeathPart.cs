using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class ChessQueenLevelLionDeathPart : AbstractPausableComponent
{
	// Token: 0x06001306 RID: 4870 RVA: 0x00010006 File Offset: 0x0000E206
	public void Start()
	{
		base.animator.Play("Loop", 0, Random.Range(0f, 1f));
		base.StartCoroutine(this.grow_cr());
	}

	// Token: 0x06001307 RID: 4871 RVA: 0x00096A34 File Offset: 0x00094C34
	public IEnumerator grow_cr()
	{
		float elapsed = 0f;
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.0416666679f, false);
		for (;;)
		{
			yield return wait;
			elapsed += wait.frameTime + wait.accumulator;
			Vector3 scale = base.transform.localScale;
			scale.x = (1f + elapsed * this.growthSpeed) * Mathf.Sign(scale.x);
			scale.y = 1f + elapsed * this.growthSpeed;
			base.transform.localScale = scale;
		}
		yield break;
	}

	// Token: 0x04000F5E RID: 3934
	[SerializeField]
	public float growthSpeed;
}
