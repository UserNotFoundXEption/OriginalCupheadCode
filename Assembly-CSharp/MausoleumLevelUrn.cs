using System;
using UnityEngine;

// Token: 0x020002C1 RID: 705
public class MausoleumLevelUrn : AbstractCollidableObject
{
	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06001F40 RID: 8000 RVA: 0x0001A4EE File Offset: 0x000186EE
	// (set) Token: 0x06001F41 RID: 8001 RVA: 0x0001A4F5 File Offset: 0x000186F5
	public static Vector3 URN_POS { get; set; }

	// Token: 0x06001F42 RID: 8002 RVA: 0x0001A4FD File Offset: 0x000186FD
	public override void Awake()
	{
		base.Awake();
		MausoleumLevelUrn.URN_POS = base.transform.position;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001F43 RID: 8003 RVA: 0x0001A520 File Offset: 0x00018720
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0400197F RID: 6527
	public DamageDealer damageDealer;
}
