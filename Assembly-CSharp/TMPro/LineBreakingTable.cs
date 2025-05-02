using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x02000695 RID: 1685
	[Serializable]
	public class LineBreakingTable
	{
		// Token: 0x06004755 RID: 18261 RVA: 0x00038B75 File Offset: 0x00036D75
		public LineBreakingTable()
		{
			this.leadingCharacters = new Dictionary<int, char>();
			this.followingCharacters = new Dictionary<int, char>();
		}

		// Token: 0x0400374C RID: 14156
		public Dictionary<int, char> leadingCharacters;

		// Token: 0x0400374D RID: 14157
		public Dictionary<int, char> followingCharacters;
	}
}
