using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000605 RID: 1541
	public sealed class TrackballAttribute : PropertyAttribute
	{
		// Token: 0x06003FFB RID: 16379 RVA: 0x00033636 File Offset: 0x00031836
		public TrackballAttribute(string method)
		{
			this.method = method;
		}

		// Token: 0x04003324 RID: 13092
		public readonly string method;
	}
}
