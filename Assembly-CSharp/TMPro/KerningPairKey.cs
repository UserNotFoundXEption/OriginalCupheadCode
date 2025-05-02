using System;

namespace TMPro
{
	// Token: 0x02000692 RID: 1682
	public struct KerningPairKey
	{
		// Token: 0x0600474B RID: 18251 RVA: 0x00038B0B File Offset: 0x00036D0B
		public KerningPairKey(int ascii_left, int ascii_right)
		{
			this.ascii_Left = ascii_left;
			this.ascii_Right = ascii_right;
			this.key = (ascii_right << 16) + ascii_left;
		}

		// Token: 0x04003743 RID: 14147
		public int ascii_Left;

		// Token: 0x04003744 RID: 14148
		public int ascii_Right;

		// Token: 0x04003745 RID: 14149
		public int key;
	}
}
