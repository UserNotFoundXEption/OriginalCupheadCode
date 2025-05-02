using System;
using UnityEngine;

namespace RektTransform
{
	// Token: 0x0200005F RID: 95
	public struct MinMax
	{
		// Token: 0x060004EF RID: 1263 RVA: 0x0006B760 File Offset: 0x00069960
		public MinMax(Vector2 min, Vector2 max)
		{
			this.min = new Vector2(Mathf.Clamp01(min.x), Mathf.Clamp01(min.y));
			this.max = new Vector2(Mathf.Clamp01(max.x), Mathf.Clamp01(max.y));
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x000057A4 File Offset: 0x000039A4
		public MinMax(float minx, float miny, float maxx, float maxy)
		{
			this.min = new Vector2(Mathf.Clamp01(minx), Mathf.Clamp01(miny));
			this.max = new Vector2(Mathf.Clamp01(maxx), Mathf.Clamp01(maxy));
		}

		// Token: 0x04000493 RID: 1171
		public Vector2 min;

		// Token: 0x04000494 RID: 1172
		public Vector2 max;
	}
}
