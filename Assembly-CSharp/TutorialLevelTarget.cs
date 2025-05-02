using System;
using UnityEngine;

// Token: 0x020003C3 RID: 963
public class TutorialLevelTarget : AbstractCollidableObject
{
	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06002A6F RID: 10863 RVA: 0x000D3ECC File Offset: 0x000D20CC
	// (remove) Token: 0x06002A70 RID: 10864 RVA: 0x000D3F04 File Offset: 0x000D2104
	public event Action OnShotEvent;

	// Token: 0x06002A71 RID: 10865 RVA: 0x00023B88 File Offset: 0x00021D88
	public override void OnCollisionPlayerProjectile(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayerProjectile(hit, phase);
		base.GetComponent<Collider2D>().enabled = false;
		if (this.OnShotEvent != null)
		{
			this.OnShotEvent();
		}
	}
}
