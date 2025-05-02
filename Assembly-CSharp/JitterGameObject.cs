using System;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class JitterGameObject : MonoBehaviour
{
	// Token: 0x06000B38 RID: 2872 RVA: 0x0000A194 File Offset: 0x00008394
	public void Start()
	{
		this.jitterDelay = 0.0833333358f;
		this.tr = base.transform;
		this.startingPosition = this.tr.position;
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x0007E194 File Offset: 0x0007C394
	public void Update()
	{
		this.currentJitterDelay -= CupheadTime.Delta;
		if (this.currentJitterDelay <= 0f)
		{
			this.currentJitterDelay = this.jitterDelay;
			float num = Random.Range(0f, 6.28318548f);
			this.tr.position = this.startingPosition + new Vector3(Mathf.Cos(num), Mathf.Sin(num), 0f) * this.jitterAmplitude;
		}
	}

	// Token: 0x040008E0 RID: 2272
	public float jitterAmplitude = 0.1f;

	// Token: 0x040008E1 RID: 2273
	public float jitterDelay = 0.1f;

	// Token: 0x040008E2 RID: 2274
	public float currentJitterDelay;

	// Token: 0x040008E3 RID: 2275
	public Transform tr;

	// Token: 0x040008E4 RID: 2276
	public Vector3 startingPosition;
}
