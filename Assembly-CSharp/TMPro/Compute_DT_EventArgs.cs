using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x0200068B RID: 1675
	public class Compute_DT_EventArgs
	{
		// Token: 0x06004739 RID: 18233 RVA: 0x00038A35 File Offset: 0x00036C35
		public Compute_DT_EventArgs(Compute_DistanceTransform_EventTypes type, float progress)
		{
			this.EventType = type;
			this.ProgressPercentage = progress;
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x00038A4B File Offset: 0x00036C4B
		public Compute_DT_EventArgs(Compute_DistanceTransform_EventTypes type, Color[] colors)
		{
			this.EventType = type;
			this.Colors = colors;
		}

		// Token: 0x0400371A RID: 14106
		public Compute_DistanceTransform_EventTypes EventType;

		// Token: 0x0400371B RID: 14107
		public float ProgressPercentage;

		// Token: 0x0400371C RID: 14108
		public Color[] Colors;
	}
}
