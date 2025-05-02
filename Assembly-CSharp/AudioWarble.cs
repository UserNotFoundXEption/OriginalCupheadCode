using System;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class AudioWarble : AbstractPausableComponent
{
	// Token: 0x0600074E RID: 1870 RVA: 0x00072778 File Offset: 0x00070978
	public void HandleWarble()
	{
		float[] array = new float[this.warbles.Length];
		float[] array2 = new float[this.warbles.Length];
		float[] array3 = new float[this.warbles.Length];
		float[] array4 = new float[this.warbles.Length];
		for (int i = 0; i < this.warbles.Length; i++)
		{
			array[i] = this.warbles[i].minVal;
			array2[i] = this.warbles[i].maxVal;
			array3[i] = this.warbles[i].warbleTime;
			array4[i] = this.warbles[i].playTime;
		}
		AudioManager.WarbleBGMPitch(this.warbles.Length, array, array2, array3, array4);
	}

	// Token: 0x0400059B RID: 1435
	[SerializeField]
	public AudioWarble.WarbleAttributes[] warbles;

	// Token: 0x020008EC RID: 2284
	[Serializable]
	public class WarbleAttributes
	{
		// Token: 0x040043DB RID: 17371
		public float minVal;

		// Token: 0x040043DC RID: 17372
		public float maxVal;

		// Token: 0x040043DD RID: 17373
		public float warbleTime;

		// Token: 0x040043DE RID: 17374
		public float playTime;
	}
}
