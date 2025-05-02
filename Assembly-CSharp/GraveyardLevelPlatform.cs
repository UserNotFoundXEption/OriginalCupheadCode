using System;
using UnityEngine;

// Token: 0x020002AC RID: 684
public class GraveyardLevelPlatform : AbstractMonoBehaviour
{
	// Token: 0x06001EB6 RID: 7862 RVA: 0x00019F49 File Offset: 0x00018149
	public void Start()
	{
		this.center = base.transform.position;
		this.t = -0.8f;
	}

	// Token: 0x06001EB7 RID: 7863 RVA: 0x000B3550 File Offset: 0x000B1750
	public void Update()
	{
		this.t += CupheadTime.Delta * this.speed;
		base.transform.position = this.center + MathUtils.AngleToDirection(-90f + Mathf.Sin(this.t) * this.maxAngle) * this.radius;
	}

	// Token: 0x04001901 RID: 6401
	public float t;

	// Token: 0x04001902 RID: 6402
	public Vector3 center;

	// Token: 0x04001903 RID: 6403
	[SerializeField]
	public float radius = 700f;

	// Token: 0x04001904 RID: 6404
	[SerializeField]
	public float speed = 315f;

	// Token: 0x04001905 RID: 6405
	[SerializeField]
	public float maxAngle = 30f;
}
