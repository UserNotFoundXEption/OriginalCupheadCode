using System;
using UnityEngine;

// Token: 0x020002A9 RID: 681
public class FrogsLevelTallFireflyRoot : AbstractMonoBehaviour
{
	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06001EAB RID: 7851 RVA: 0x00019EF2 File Offset: 0x000180F2
	public float radius
	{
		get
		{
			return this._radius;
		}
	}

	// Token: 0x06001EAC RID: 7852 RVA: 0x000B33D0 File Offset: 0x000B15D0
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
		Gizmos.DrawSphere(base.baseTransform.position, this.radius);
		Gizmos.color = new Color(1f, 0f, 0f, 1f);
		Gizmos.DrawWireSphere(base.baseTransform.position, this.radius);
	}

	// Token: 0x040018FB RID: 6395
	[SerializeField]
	public float _radius = 100f;
}
