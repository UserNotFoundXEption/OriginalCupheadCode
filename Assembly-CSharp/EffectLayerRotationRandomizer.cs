using System;
using UnityEngine;

// Token: 0x020005A3 RID: 1443
public class EffectLayerRotationRandomizer : MonoBehaviour
{
	// Token: 0x06003CF5 RID: 15605 RVA: 0x00117610 File Offset: 0x00115810
	public void Awake()
	{
		if (this.randomizeRotation)
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		}
		base.transform.localScale = new Vector3((!this.randomizeXFlip) ? base.transform.localScale.x : ((float)MathUtils.PlusOrMinus()), (!this.randomizeYFlip) ? base.transform.localScale.y : ((float)MathUtils.PlusOrMinus()));
		base.enabled = false;
	}

	// Token: 0x04003072 RID: 12402
	[SerializeField]
	public bool randomizeRotation = true;

	// Token: 0x04003073 RID: 12403
	[SerializeField]
	public bool randomizeXFlip = true;

	// Token: 0x04003074 RID: 12404
	[SerializeField]
	public bool randomizeYFlip = true;
}
