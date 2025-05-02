using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000604 RID: 1540
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x06003FFA RID: 16378 RVA: 0x00033627 File Offset: 0x00031827
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x04003323 RID: 13091
		public readonly float min;
	}
}
