using System;
using UnityEngine;

// Token: 0x020000AA RID: 170
public class RookParallaxLayer : ParallaxLayer
{
	// Token: 0x06000808 RID: 2056 RVA: 0x0007561C File Offset: 0x0007381C
	public override void UpdateComparative()
	{
		Vector3 position = base.transform.position;
		position.x = base._offset.x + this._camera.transform.position.x * this.percentage;
		position.y = base._offset.y + this._camera.transform.position.y * this.percentage * this.yModifier;
		base.transform.position = position;
	}

	// Token: 0x04000636 RID: 1590
	[SerializeField]
	public float yModifier = 0.5f;
}
