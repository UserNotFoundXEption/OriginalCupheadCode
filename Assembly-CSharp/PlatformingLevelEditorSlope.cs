using System;
using UnityEngine;

// Token: 0x0200045A RID: 1114
[RequireComponent(typeof(PolygonCollider2D))]
public class PlatformingLevelEditorSlope : AbstractMonoBehaviour
{
	// Token: 0x17000376 RID: 886
	// (get) Token: 0x06002F9A RID: 12186 RVA: 0x00027ACC File Offset: 0x00025CCC
	public PolygonCollider2D polygonCollider
	{
		get
		{
			if (this._polygonCollider == null)
			{
				this._polygonCollider = base.GetComponent<PolygonCollider2D>();
			}
			return this._polygonCollider;
		}
	}

	// Token: 0x06002F9B RID: 12187 RVA: 0x00027AF1 File Offset: 0x00025CF1
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.5f);
	}

	// Token: 0x06002F9C RID: 12188 RVA: 0x00027B04 File Offset: 0x00025D04
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
	}

	// Token: 0x06002F9D RID: 12189 RVA: 0x000E2428 File Offset: 0x000E0628
	public void DrawGizmos(float alpha)
	{
		Gizmos.color = Color.cyan;
		for (int i = 0; i < this.polygonCollider.points.Length; i++)
		{
			Vector3 vector = this.polygonCollider.points[i];
			Vector3 vector2 = (i != this.polygonCollider.points.Length - 1) ? this.polygonCollider.points[i + 1] : this.polygonCollider.points[0];
			Gizmos.DrawLine(vector, vector2);
		}
	}

	// Token: 0x04002778 RID: 10104
	public PolygonCollider2D _polygonCollider;
}
