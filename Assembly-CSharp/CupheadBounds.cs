using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
[Serializable]
public class CupheadBounds
{
	// Token: 0x06000575 RID: 1397 RVA: 0x00005CAB File Offset: 0x00003EAB
	public CupheadBounds()
	{
		this.left = 0f;
		this.right = 0f;
		this.top = 0f;
		this.bottom = 0f;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x00005CDF File Offset: 0x00003EDF
	public CupheadBounds(float left, float right, float top, float bottom)
	{
		this.left = left;
		this.right = right;
		this.top = top;
		this.bottom = bottom;
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0006CB3C File Offset: 0x0006AD3C
	public CupheadBounds(Rect r)
	{
		this.left = r.center.x - r.x;
		this.top = r.center.y - r.y;
		this.right = r.xMax - r.center.x;
		this.bottom = r.yMax - r.center.y;
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00005D04 File Offset: 0x00003F04
	public static implicit operator CupheadBounds(Rect r)
	{
		return new CupheadBounds(r);
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x00005D0C File Offset: 0x00003F0C
	public CupheadBounds Copy()
	{
		return base.MemberwiseClone() as CupheadBounds;
	}

	// Token: 0x0400049F RID: 1183
	public float left;

	// Token: 0x040004A0 RID: 1184
	public float right;

	// Token: 0x040004A1 RID: 1185
	public float top;

	// Token: 0x040004A2 RID: 1186
	public float bottom;
}
