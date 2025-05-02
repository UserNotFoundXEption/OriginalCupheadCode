using System;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class DicePalaceCardLevelGridBlock : AbstractCollidableObject
{
	// Token: 0x060015DC RID: 5596 RVA: 0x0001287A File Offset: 0x00010A7A
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x040011D0 RID: 4560
	public float Xcoordinate;

	// Token: 0x040011D1 RID: 4561
	public float Ycoordinate;

	// Token: 0x040011D2 RID: 4562
	public bool hasBlock;

	// Token: 0x040011D3 RID: 4563
	public float size;

	// Token: 0x040011D4 RID: 4564
	public DicePalaceCardLevelBlock blockHeld;
}
