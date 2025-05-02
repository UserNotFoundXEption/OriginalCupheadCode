using System;

namespace TMPro
{
	// Token: 0x02000693 RID: 1683
	[Serializable]
	public class KerningPair
	{
		// Token: 0x0600474C RID: 18252 RVA: 0x00038B27 File Offset: 0x00036D27
		public KerningPair(int left, int right, float offset)
		{
			this.AscII_Left = left;
			this.AscII_Right = right;
			this.XadvanceOffset = offset;
		}

		// Token: 0x04003746 RID: 14150
		public int AscII_Left;

		// Token: 0x04003747 RID: 14151
		public int AscII_Right;

		// Token: 0x04003748 RID: 14152
		public float XadvanceOffset;
	}
}
