using System;
using UnityEngine;

// Token: 0x0200025A RID: 602
public class FlyingCowboyLevelOverlayToggle : MonoBehaviour
{
	// Token: 0x06001BC0 RID: 7104 RVA: 0x000AC974 File Offset: 0x000AAB74
	public void Start()
	{
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in this.overlayRenderers)
		{
			if (Random.value < this.overlayProbability)
			{
				spriteRenderer.enabled = true;
				spriteRenderer.sortingOrder = component.sortingOrder + 1;
			}
			else
			{
				spriteRenderer.enabled = false;
			}
		}
	}

	// Token: 0x0400168F RID: 5775
	[SerializeField]
	[Range(0f, 1f)]
	public float overlayProbability;

	// Token: 0x04001690 RID: 5776
	[SerializeField]
	public SpriteRenderer[] overlayRenderers;
}
