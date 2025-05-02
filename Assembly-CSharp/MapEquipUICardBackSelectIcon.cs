using System;
using UnityEngine;

// Token: 0x020004C3 RID: 1219
public class MapEquipUICardBackSelectIcon : AbstractMapCardIcon
{
	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x060032AA RID: 12970 RVA: 0x0002A0D3 File Offset: 0x000282D3
	// (set) Token: 0x060032AB RID: 12971 RVA: 0x0002A0DB File Offset: 0x000282DB
	public int Index { get; set; }

	// Token: 0x060032AC RID: 12972 RVA: 0x000EE9D0 File Offset: 0x000ECBD0
	public int GetIndexOfNeighbor(Trilean2 direction)
	{
		MapEquipUICardBackSelectIcon mapEquipUICardBackSelectIcon = null;
		if (direction.x < 0)
		{
			mapEquipUICardBackSelectIcon = this.left;
		}
		if (direction.x > 0)
		{
			mapEquipUICardBackSelectIcon = this.right;
		}
		if (direction.y > 0)
		{
			mapEquipUICardBackSelectIcon = this.up;
		}
		if (direction.y < 0)
		{
			mapEquipUICardBackSelectIcon = this.down;
		}
		if (mapEquipUICardBackSelectIcon == null)
		{
			return this.Index;
		}
		return mapEquipUICardBackSelectIcon.Index;
	}

	// Token: 0x04002975 RID: 10613
	[Header("Directions")]
	[SerializeField]
	public MapEquipUICardBackSelectIcon up;

	// Token: 0x04002976 RID: 10614
	[SerializeField]
	public MapEquipUICardBackSelectIcon down;

	// Token: 0x04002977 RID: 10615
	[SerializeField]
	public MapEquipUICardBackSelectIcon left;

	// Token: 0x04002978 RID: 10616
	[SerializeField]
	public MapEquipUICardBackSelectIcon right;
}
