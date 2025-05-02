using System;
using UnityEngine;

// Token: 0x0200038E RID: 910
public class SnowCultLevelBucket : MonoBehaviour
{
	// Token: 0x0600282D RID: 10285 RVA: 0x000CDD98 File Offset: 0x000CBF98
	public void FixedUpdate()
	{
		base.transform.position += this.fallSpeed * Vector3.down * CupheadTime.FixedDelta;
		this.fallSpeed += CupheadTime.FixedDelta * this.accel;
	}

	// Token: 0x04002161 RID: 8545
	[SerializeField]
	public float fallSpeed = 10f;

	// Token: 0x04002162 RID: 8546
	[SerializeField]
	public float accel = 1f;
}
