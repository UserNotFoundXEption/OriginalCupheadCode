using System;
using UnityEngine;

// Token: 0x020005A0 RID: 1440
public class AnimatableSortingOrder : MonoBehaviour
{
	// Token: 0x06003CE5 RID: 15589 RVA: 0x000312A0 File Offset: 0x0002F4A0
	public void Start()
	{
		this.sr = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06003CE6 RID: 15590 RVA: 0x001173A0 File Offset: 0x001155A0
	public void LateUpdate()
	{
		int sortingOrder = (int)this.sortingLayer;
		this.sr.sortingOrder = sortingOrder;
	}

	// Token: 0x04003064 RID: 12388
	public SpriteRenderer sr;

	// Token: 0x04003065 RID: 12389
	public float sortingLayer;
}
