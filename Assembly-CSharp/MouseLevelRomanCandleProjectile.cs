using System;
using UnityEngine;

// Token: 0x020002CF RID: 719
public class MouseLevelRomanCandleProjectile : HomingProjectile
{
	// Token: 0x06001FFE RID: 8190 RVA: 0x0001B150 File Offset: 0x00019350
	public override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}
}
