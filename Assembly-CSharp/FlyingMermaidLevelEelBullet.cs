using System;
using UnityEngine;

// Token: 0x02000280 RID: 640
public class FlyingMermaidLevelEelBullet : BasicProjectile
{
	// Token: 0x06001D1A RID: 7450 RVA: 0x000B00D0 File Offset: 0x000AE2D0
	public void RotateSpark()
	{
		this.spark.transform.SetEulerAngles(null, null, new float?((float)Random.Range(0, 360)));
	}

	// Token: 0x040017C2 RID: 6082
	[SerializeField]
	public Transform spark;
}
