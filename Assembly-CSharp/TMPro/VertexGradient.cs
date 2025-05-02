using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x0200069A RID: 1690
	[Serializable]
	public struct VertexGradient
	{
		// Token: 0x0600475E RID: 18270 RVA: 0x00038B93 File Offset: 0x00036D93
		public VertexGradient(Color color)
		{
			this.topLeft = color;
			this.topRight = color;
			this.bottomLeft = color;
			this.bottomRight = color;
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x00038BB1 File Offset: 0x00036DB1
		public VertexGradient(Color color0, Color color1, Color color2, Color color3)
		{
			this.topLeft = color0;
			this.topRight = color1;
			this.bottomLeft = color2;
			this.bottomRight = color3;
		}

		// Token: 0x04003786 RID: 14214
		public Color topLeft;

		// Token: 0x04003787 RID: 14215
		public Color topRight;

		// Token: 0x04003788 RID: 14216
		public Color bottomLeft;

		// Token: 0x04003789 RID: 14217
		public Color bottomRight;
	}
}
