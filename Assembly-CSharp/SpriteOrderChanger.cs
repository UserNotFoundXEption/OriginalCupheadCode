using System;
using UnityEngine;

// Token: 0x020005B2 RID: 1458
public class SpriteOrderChanger : AbstractMonoBehaviour
{
	// Token: 0x06003D32 RID: 15666 RVA: 0x000315E4 File Offset: 0x0002F7E4
	public override void Awake()
	{
		base.Awake();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06003D33 RID: 15667 RVA: 0x000315F8 File Offset: 0x0002F7F8
	public void Update()
	{
		if (this.t >= this.frameDelay)
		{
			this.t = 0;
			this.spriteRenderer.sortingOrder += this.change;
		}
		this.t++;
	}

	// Token: 0x040030C7 RID: 12487
	public int change = 1;

	// Token: 0x040030C8 RID: 12488
	public int frameDelay = 2;

	// Token: 0x040030C9 RID: 12489
	public SpriteRenderer spriteRenderer;

	// Token: 0x040030CA RID: 12490
	public int t;
}
