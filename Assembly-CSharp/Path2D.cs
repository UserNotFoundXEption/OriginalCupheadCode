using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class Path2D : MonoBehaviour
{
	// Token: 0x060004C9 RID: 1225 RVA: 0x0000565B File Offset: 0x0000385B
	public virtual void OnDrawGizmos()
	{
		this.DrawGizmos(0.1f);
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00005668 File Offset: 0x00003868
	public virtual void OnDrawGizmosSelected()
	{
		this.DrawGizmos(1f);
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0006AED8 File Offset: 0x000690D8
	public void DrawGizmos(float a)
	{
		for (int i = 0; i < this.nodes.Count; i++)
		{
			if (i > 0)
			{
				Gizmos.DrawLine(this.nodes[i], this.nodes[i - 1]);
			}
		}
	}

	// Token: 0x04000476 RID: 1142
	public Path2D.Space space;

	// Token: 0x04000477 RID: 1143
	public List<Vector2> nodes = new List<Vector2>(2);

	// Token: 0x020008A7 RID: 2215
	public enum Space
	{
		// Token: 0x04004273 RID: 17011
		Global,
		// Token: 0x04004274 RID: 17012
		Local
	}
}
