using System;

namespace TMPro
{
	// Token: 0x02000684 RID: 1668
	public struct CaretInfo
	{
		// Token: 0x060046F7 RID: 18167 RVA: 0x00038843 File Offset: 0x00036A43
		public CaretInfo(int index, CaretPosition position)
		{
			this.index = index;
			this.position = position;
		}

		// Token: 0x040036F9 RID: 14073
		public int index;

		// Token: 0x040036FA RID: 14074
		public CaretPosition position;
	}
}
