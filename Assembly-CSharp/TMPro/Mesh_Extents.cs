using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x020006A1 RID: 1697
	[Serializable]
	public struct Mesh_Extents
	{
		// Token: 0x06004766 RID: 18278 RVA: 0x00038C04 File Offset: 0x00036E04
		public Mesh_Extents(Vector2 min, Vector2 max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x0015A508 File Offset: 0x00158708
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

		// Token: 0x040037B2 RID: 14258
		public Vector2 min;

		// Token: 0x040037B3 RID: 14259
		public Vector2 max;
	}
}
