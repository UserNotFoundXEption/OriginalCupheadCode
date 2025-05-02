using System;
using UnityEngine;

// Token: 0x02000347 RID: 839
public class RumRunnersLevelLaserMask : MonoBehaviour
{
	// Token: 0x060024C4 RID: 9412 RVA: 0x000C4B54 File Offset: 0x000C2D54
	public void Setup(int layerID, int lowestLayerOrder)
	{
		foreach (SpriteRenderer spriteRenderer in this.maskRenderers)
		{
			spriteRenderer.sortingLayerID = layerID;
			spriteRenderer.sortingOrder = lowestLayerOrder - 1;
		}
		foreach (SpriteRenderer spriteRenderer2 in this.clearRenderers)
		{
			spriteRenderer2.sortingLayerID = layerID;
			spriteRenderer2.sortingOrder = lowestLayerOrder + 4;
		}
	}

	// Token: 0x04001E6B RID: 7787
	[SerializeField]
	public SpriteRenderer[] maskRenderers;

	// Token: 0x04001E6C RID: 7788
	[SerializeField]
	public SpriteRenderer[] clearRenderers;
}
