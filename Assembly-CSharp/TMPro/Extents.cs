using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x020006A0 RID: 1696
	public struct Extents
	{
		// Token: 0x06004764 RID: 18276 RVA: 0x00038BF4 File Offset: 0x00036DF4
		public Extents(Vector2 min, Vector2 max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x0015A464 File Offset: 0x00158664
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Min (",
				this.min.x.ToString("f2"),
				", ",
				this.min.y.ToString("f2"),
				")   Max (",
				this.max.x.ToString("f2"),
				", ",
				this.max.y.ToString("f2"),
				")"
			});
		}

		// Token: 0x040037B0 RID: 14256
		public Vector2 min;

		// Token: 0x040037B1 RID: 14257
		public Vector2 max;
	}
}
