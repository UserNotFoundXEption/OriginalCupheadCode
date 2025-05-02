using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200050E RID: 1294
public class LevelPlayerChaliceDashEffect : Effect
{
	// Token: 0x06003643 RID: 13891 RVA: 0x0002C788 File Offset: 0x0002A988
	public void Start()
	{
		base.StartCoroutine(this.MoveUntilTail());
	}

	// Token: 0x06003644 RID: 13892 RVA: 0x000FE05C File Offset: 0x000FC25C
	public IEnumerator MoveUntilTail()
	{
		yield return new WaitForEndOfFrame();
		float xFacing = base.transform.parent.transform.localScale.x;
		int target = Animator.StringToHash(base.animator.GetLayerName(0) + ".Start");
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == target && base.transform.parent.transform.localScale.x == xFacing)
		{
			yield return null;
		}
		if (base.transform.parent.transform.localScale.x != xFacing)
		{
			base.transform.localPosition = new Vector3(-base.transform.localPosition.x, base.transform.localPosition.y);
		}
		base.transform.parent = null;
		base.transform.localScale = new Vector3(xFacing, 1f);
		yield break;
	}

	// Token: 0x04002C0E RID: 11278
	public float t;
}
