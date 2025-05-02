using System;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class DicePalaceCardLevelBlock : LevelPlatform
{
	// Token: 0x060015D6 RID: 5590 RVA: 0x0001284A File Offset: 0x00010A4A
	public override void AddChild(Transform player)
	{
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x0001284C File Offset: 0x00010A4C
	public void DestroyBlock()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040011C7 RID: 4551
	public DicePalaceCardLevelBlock.Suit suit;

	// Token: 0x040011C8 RID: 4552
	public int stopOffsetX;

	// Token: 0x040011C9 RID: 4553
	public DicePalaceCardLevelGridBlock[,] gridBlock;

	// Token: 0x040011CA RID: 4554
	public float YCheck;

	// Token: 0x040011CB RID: 4555
	public DamageDealer damageDealer;

	// Token: 0x02000B73 RID: 2931
	public enum Suit
	{
		// Token: 0x040053BB RID: 21435
		Hearts = 1,
		// Token: 0x040053BC RID: 21436
		Spades,
		// Token: 0x040053BD RID: 21437
		Clubs,
		// Token: 0x040053BE RID: 21438
		Diamonds
	}
}
