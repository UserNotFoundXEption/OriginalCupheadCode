using System;
using UnityEngine;

// Token: 0x02000480 RID: 1152
public class MapLayerChanger : AbstractMonoBehaviour
{
	// Token: 0x060030C1 RID: 12481 RVA: 0x000E7620 File Offset: 0x000E5820
	public void OnTriggerEnter2D(Collider2D collider)
	{
		SpriteRenderer[] componentsInChildren = collider.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			spriteRenderer.sortingOrder = this.sortingOrder;
		}
	}

	// Token: 0x060030C2 RID: 12482 RVA: 0x000E765C File Offset: 0x000E585C
	public void OnTriggerStay2D(Collider2D collider)
	{
		SpriteRenderer[] componentsInChildren = collider.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			spriteRenderer.sortingOrder = this.sortingOrder;
		}
	}

	// Token: 0x04002845 RID: 10309
	[SerializeField]
	public int sortingOrder;
}
