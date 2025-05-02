using System;
using UnityEngine;

// Token: 0x020001DC RID: 476
public class DicePalaceDominoLevelBaseTile : AbstractCollidableObject
{
	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06001624 RID: 5668 RVA: 0x00012C76 File Offset: 0x00010E76
	// (set) Token: 0x06001625 RID: 5669 RVA: 0x00012C7E File Offset: 0x00010E7E
	public int currentColourIndex { get; set; }

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06001626 RID: 5670 RVA: 0x00012C87 File Offset: 0x00010E87
	// (set) Token: 0x06001627 RID: 5671 RVA: 0x00012C8F File Offset: 0x00010E8F
	public bool isActivated { get; set; }

	// Token: 0x06001628 RID: 5672 RVA: 0x00012C98 File Offset: 0x00010E98
	public virtual void InitTile()
	{
		this.isActivated = true;
	}

	// Token: 0x06001629 RID: 5673 RVA: 0x00012CA1 File Offset: 0x00010EA1
	public virtual void InitTile(DicePalaceDominoLevelFloor parent, LevelProperties.DicePalaceDomino properties)
	{
		this.properties = properties;
		this.isActivated = true;
	}

	// Token: 0x0600162A RID: 5674 RVA: 0x00012CB1 File Offset: 0x00010EB1
	public virtual void DeactivateTile()
	{
		this.isActivated = false;
	}

	// Token: 0x0600162B RID: 5675 RVA: 0x00012CBA File Offset: 0x00010EBA
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400120F RID: 4623
	[SerializeField]
	public Sprite[] colours;

	// Token: 0x04001210 RID: 4624
	public LevelProperties.DicePalaceDomino properties;
}
