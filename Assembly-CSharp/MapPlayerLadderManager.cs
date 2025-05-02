using System;
using UnityEngine;

// Token: 0x020004B5 RID: 1205
public class MapPlayerLadderManager : AbstractMapPlayerComponent
{
	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x06003215 RID: 12821 RVA: 0x000298DA File Offset: 0x00027ADA
	// (set) Token: 0x06003216 RID: 12822 RVA: 0x000298E2 File Offset: 0x00027AE2
	public MapPlayerLadderObject Current { get; set; }

	// Token: 0x06003217 RID: 12823 RVA: 0x000298EB File Offset: 0x00027AEB
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06003218 RID: 12824 RVA: 0x000298F3 File Offset: 0x00027AF3
	public override void OnLadderEnter(Vector2 point, MapPlayerLadderObject ladder, MapLadder.Location location)
	{
		base.OnLadderEnter(point, ladder, location);
		base.GetComponent<Collider2D>().enabled = false;
		this.Current = ladder;
	}

	// Token: 0x06003219 RID: 12825 RVA: 0x00029911 File Offset: 0x00027B11
	public override void OnLadderExitComplete()
	{
		base.OnLadderExitComplete();
		base.GetComponent<Collider2D>().enabled = true;
		this.Current = null;
	}
}
