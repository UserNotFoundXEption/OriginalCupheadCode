using System;
using UnityEngine;

// Token: 0x02000249 RID: 585
public class FlyingBlimpLevelSpawnRadius : AbstractMonoBehaviour
{
	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06001AC9 RID: 6857 RVA: 0x00016C3D File Offset: 0x00014E3D
	public float radius
	{
		get
		{
			return this._radius;
		}
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x000A99B8 File Offset: 0x000A7BB8
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
		Gizmos.DrawSphere(base.baseTransform.position, this.radius);
		Gizmos.color = new Color(1f, 0f, 0f, 1f);
		Gizmos.DrawWireSphere(base.baseTransform.position, this.radius);
	}

	// Token: 0x0400159B RID: 5531
	[SerializeField]
	public float _radius = 100f;
}
