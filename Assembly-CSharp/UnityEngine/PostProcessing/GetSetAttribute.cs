using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000603 RID: 1539
	public sealed class GetSetAttribute : PropertyAttribute
	{
		// Token: 0x06003FF9 RID: 16377 RVA: 0x00033618 File Offset: 0x00031818
		public GetSetAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x04003321 RID: 13089
		public readonly string name;

		// Token: 0x04003322 RID: 13090
		public bool dirty;
	}
}
