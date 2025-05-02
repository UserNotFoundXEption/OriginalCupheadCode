using System;
using UnityEngine;

// Token: 0x020005A8 RID: 1448
public class PlaneLevelEffect : Effect
{
	// Token: 0x06003D06 RID: 15622 RVA: 0x000313EC File Offset: 0x0002F5EC
	public void Update()
	{
		base.transform.AddPosition(-300f * CupheadTime.Delta * this.speed, 0f, 0f);
	}

	// Token: 0x04003086 RID: 12422
	public const float SPEED = 300f;

	// Token: 0x04003087 RID: 12423
	[Range(0f, 2f)]
	public float speed = 1f;
}
