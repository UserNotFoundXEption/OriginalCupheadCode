using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000340 RID: 832
public class RumRunnersLevelCopBallDeathEffect : Effect
{
	// Token: 0x0600247B RID: 9339 RVA: 0x000C40C0 File Offset: 0x000C22C0
	public override void Initialize(Vector3 position, Vector3 scale, bool randomR)
	{
		int i = Random.Range(0, base.animator.GetInteger("Count"));
		base.animator.SetInteger("Effect", i);
		Transform transform = base.transform;
		transform.position = position;
		transform.localScale = scale;
		if (randomR)
		{
			transform.eulerAngles = new Vector3(0f, 0f, (float)(Random.Range(0, 8) * 45));
		}
		List<int> list = new List<int>();
		for (i = 0; i < 5; i++)
		{
			list.Add(i);
		}
		list.RemoveAt(Random.Range(0, list.Count));
		list.RemoveAt(Random.Range(0, list.Count));
		for (i = 0; i < 3; i++)
		{
			this.shrapnel[i].Play(list[i].ToString());
			this.shrapnel[i].transform.parent = null;
		}
	}

	// Token: 0x04001E32 RID: 7730
	[SerializeField]
	public Animator[] shrapnel;
}
