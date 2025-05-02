using System;
using System.Collections.Generic;

namespace DialoguerCore
{
	// Token: 0x020005ED RID: 1517
	[Serializable]
	public class DialoguerVariables
	{
		// Token: 0x06003E93 RID: 16019 RVA: 0x0003250C File Offset: 0x0003070C
		public DialoguerVariables(List<bool> booleans, List<float> floats, List<string> strings)
		{
			this.booleans = booleans;
			this.floats = floats;
			this.strings = strings;
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x0011CC78 File Offset: 0x0011AE78
		public DialoguerVariables Clone()
		{
			List<bool> list = new List<bool>();
			for (int i = 0; i < this.booleans.Count; i++)
			{
				list.Add(this.booleans[i]);
			}
			List<float> list2 = new List<float>();
			for (int j = 0; j < this.floats.Count; j++)
			{
				list2.Add(this.floats[j]);
			}
			List<string> list3 = new List<string>();
			for (int k = 0; k < this.strings.Count; k++)
			{
				list3.Add(this.strings[k]);
			}
			return new DialoguerVariables(list, list2, list3);
		}

		// Token: 0x0400326C RID: 12908
		public readonly List<bool> booleans;

		// Token: 0x0400326D RID: 12909
		public readonly List<float> floats;

		// Token: 0x0400326E RID: 12910
		public readonly List<string> strings;
	}
}
