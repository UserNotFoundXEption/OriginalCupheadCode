using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
public class MapWaterWave : AbstractPausableComponent
{
	// Token: 0x06003188 RID: 12680 RVA: 0x00029321 File Offset: 0x00027521
	public void Start()
	{
		base.StartCoroutine(this.wave_cr());
	}

	// Token: 0x06003189 RID: 12681 RVA: 0x000EA108 File Offset: 0x000E8308
	public IEnumerator wave_cr()
	{
		for (;;)
		{
			base.animator.Play("Wave", 0, this.offsetRange.RandomFloat());
			yield return base.animator.WaitForAnimationToEnd(this, "Wave", false, true);
			yield return CupheadTime.WaitForSeconds(this, this.delayRange.RandomFloat());
		}
		yield break;
	}

	// Token: 0x040028C2 RID: 10434
	[SerializeField]
	public MinMax offsetRange;

	// Token: 0x040028C3 RID: 10435
	[SerializeField]
	public MinMax delayRange;
}
