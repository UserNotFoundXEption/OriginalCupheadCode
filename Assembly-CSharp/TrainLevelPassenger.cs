using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003B5 RID: 949
public class TrainLevelPassenger : AbstractPausableComponent
{
	// Token: 0x06002A1A RID: 10778 RVA: 0x00023719 File Offset: 0x00021919
	public override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06002A1B RID: 10779 RVA: 0x000D3438 File Offset: 0x000D1638
	public IEnumerator main_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(3f, 8f));
			base.animator.SetTrigger("Continue");
		}
		yield break;
	}
}
