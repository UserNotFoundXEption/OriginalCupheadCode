using System;
using UnityEngine;

// Token: 0x020004B6 RID: 1206
public class MapPlayerLadderObject
{
	// Token: 0x0600321A RID: 12826 RVA: 0x0002992C File Offset: 0x00027B2C
	public MapPlayerLadderObject(Vector2 top, Vector2 bottom)
	{
		this.top = top;
		this.bottom = bottom;
	}

	// Token: 0x0400291D RID: 10525
	public readonly Vector2 top;

	// Token: 0x0400291E RID: 10526
	public readonly Vector2 bottom;
}
