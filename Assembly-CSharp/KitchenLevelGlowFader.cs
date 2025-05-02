using System;
using UnityEngine;

// Token: 0x020002B8 RID: 696
public class KitchenLevelGlowFader : MonoBehaviour
{
	// Token: 0x06001F09 RID: 7945 RVA: 0x000B5370 File Offset: 0x000B3570
	public void Update()
	{
		this.t += CupheadTime.Delta;
		this.rend.color = new Color(1f, 1f, 1f, Mathf.Lerp(this.alphaMin, 1f, (Mathf.Sin(this.t * this.speedModifier) + 1f) / 2f));
	}

	// Token: 0x0400195E RID: 6494
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x0400195F RID: 6495
	[SerializeField]
	[Range(0f, 6.28318548f)]
	public float startOffset;

	// Token: 0x04001960 RID: 6496
	public float t;

	// Token: 0x04001961 RID: 6497
	[SerializeField]
	public float speedModifier = 1f;

	// Token: 0x04001962 RID: 6498
	[SerializeField]
	public float alphaMin = 0.8f;
}
