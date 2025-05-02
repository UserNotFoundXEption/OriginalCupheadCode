using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000232 RID: 562
public class FlyingBirdLevelHeartProjectile : BasicProjectile
{
	// Token: 0x060019D4 RID: 6612 RVA: 0x00016046 File Offset: 0x00014246
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.spawn_fx_cr());
	}

	// Token: 0x060019D5 RID: 6613 RVA: 0x000A71FC File Offset: 0x000A53FC
	public IEnumerator spawn_fx_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.17f);
		for (;;)
		{
			this.FX.Create(base.transform.position).transform.SetEulerAngles(null, null, new float?(base.transform.eulerAngles.z));
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
		}
		yield break;
	}

	// Token: 0x040014BD RID: 5309
	[SerializeField]
	public Effect FX;
}
