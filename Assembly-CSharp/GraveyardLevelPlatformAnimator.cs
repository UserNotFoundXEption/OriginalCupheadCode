using System;
using UnityEngine;

// Token: 0x020002AD RID: 685
public class GraveyardLevelPlatformAnimator : AbstractMonoBehaviour
{
	// Token: 0x06001EB9 RID: 7865 RVA: 0x000B35C0 File Offset: 0x000B17C0
	public void Start()
	{
		for (int i = 0; i < this.trailSparks.Length; i++)
		{
			this.trailSparks[i].transform.parent = null;
		}
		for (int j = 0; j < this.xPositionBuffer.Length; j++)
		{
			this.xPositionBuffer[j] = base.transform.position.x;
		}
	}

	// Token: 0x06001EBA RID: 7866 RVA: 0x000B3630 File Offset: 0x000B1830
	public void LateUpdate()
	{
		this.strikeSpark.transform.position = new Vector3(this.strikeSpark.transform.position.x, (float)Level.Current.Ground);
		for (int i = this.xPositionBuffer.Length - 1; i > 0; i--)
		{
			this.xPositionBuffer[i] = this.xPositionBuffer[i - 1];
		}
		this.xPositionBuffer[0] = base.transform.position.x;
		for (int j = 0; j < 3; j++)
		{
			this.trailSparks[j].transform.position = new Vector3(this.xPositionBuffer[j * 2 + 1], (float)Level.Current.Ground);
			this.trailSparks[j].transform.localScale = new Vector3(Mathf.Sign(this.xPositionBuffer[1] - this.xPositionBuffer[0]), 1f);
		}
	}

	// Token: 0x04001906 RID: 6406
	[SerializeField]
	public SpriteRenderer strikeSpark;

	// Token: 0x04001907 RID: 6407
	[SerializeField]
	public SpriteRenderer[] trailSparks;

	// Token: 0x04001908 RID: 6408
	public float[] xPositionBuffer = new float[8];
}
