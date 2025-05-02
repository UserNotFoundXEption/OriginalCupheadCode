using System;
using UnityEngine;

// Token: 0x020004C4 RID: 1220
public class MapEquipUICardBackSelectSelectionCursor : MapEquipUICursor
{
	// Token: 0x060032AE RID: 12974 RVA: 0x0002A0F3 File Offset: 0x000282F3
	public override void SetPosition(Vector3 position)
	{
		base.SetPosition(position);
		this.Show();
	}

	// Token: 0x060032AF RID: 12975 RVA: 0x0002A102 File Offset: 0x00028302
	public override void Show()
	{
		base.Show();
		base.animator.Play("Idle");
	}

	// Token: 0x060032B0 RID: 12976 RVA: 0x0002A11A File Offset: 0x0002831A
	public void Select()
	{
		base.animator.Play("Select");
	}

	// Token: 0x0400297A RID: 10618
	public int selectedIndex = -1;
}
