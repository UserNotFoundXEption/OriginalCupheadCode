using System;
using System.Collections.Generic;

namespace DialoguerCore
{
	// Token: 0x020005EA RID: 1514
	[Serializable]
	public class DialoguerGlobalVariables
	{
		// Token: 0x06003E8B RID: 16011 RVA: 0x00032478 File Offset: 0x00030678
		public DialoguerGlobalVariables()
		{
			this.booleans = new List<bool>();
			this.floats = new List<float>();
			this.strings = new List<string>();
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x000324A1 File Offset: 0x000306A1
		public DialoguerGlobalVariables(List<bool> booleans, List<float> floats, List<string> strings)
		{
			this.booleans = booleans;
			this.floats = floats;
			this.strings = strings;
		}

		// Token: 0x0400325A RID: 12890
		public List<bool> booleans;

		// Token: 0x0400325B RID: 12891
		public List<float> floats;

		// Token: 0x0400325C RID: 12892
		public List<string> strings;
	}
}
