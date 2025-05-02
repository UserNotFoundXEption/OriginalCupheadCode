using System;
using UnityEngine;

// Token: 0x020005A9 RID: 1449
public class RandomAnimation : AbstractPausableComponent
{
	// Token: 0x06003D08 RID: 15624 RVA: 0x00117990 File Offset: 0x00115B90
	public override void Awake()
	{
		base.Awake();
		base.animator.SetInteger("Animation", Random.Range(0, base.animator.GetInteger("Count")));
		base.animator.speed += Random.Range(-this.randomSpeed, this.randomSpeed);
	}

	// Token: 0x04003088 RID: 12424
	[SerializeField]
	[Range(0f, 1f)]
	public float randomSpeed = 0.1f;
}
