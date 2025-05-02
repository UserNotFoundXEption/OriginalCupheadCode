using System;
using UnityEngine;

// Token: 0x02000458 RID: 1112
[RequireComponent(typeof(CircleCollider2D))]
public class PlatformingLevelEditorCircle : AbstractMonoBehaviour
{
	// Token: 0x17000375 RID: 885
	// (get) Token: 0x06002F90 RID: 12176 RVA: 0x00027A2C File Offset: 0x00025C2C
	public CircleCollider2D circleCollider
	{
		get
		{
			if (this._circleCollider == null)
			{
				this._circleCollider = base.GetComponent<CircleCollider2D>();
			}
			return this._circleCollider;
		}
	}

	// Token: 0x06002F91 RID: 12177 RVA: 0x00027A51 File Offset: 0x00025C51
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.5f);
	}

	// Token: 0x06002F92 RID: 12178 RVA: 0x00027A64 File Offset: 0x00025C64
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
	}

	// Token: 0x06002F93 RID: 12179 RVA: 0x000E1F04 File Offset: 0x000E0104
	public void DrawGizmos(float alpha)
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(base.transform.position + this.circleCollider.offset, this.circleCollider.radius);
	}

	// Token: 0x0400276D RID: 10093
	public CircleCollider2D _circleCollider;
}
