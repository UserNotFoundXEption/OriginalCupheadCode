using System;
using UnityEngine;

// Token: 0x0200038C RID: 908
public class SnowCultLevelBatDrip : SnowCultLevelBatEffect
{
	// Token: 0x06002829 RID: 10281 RVA: 0x000CDCEC File Offset: 0x000CBEEC
	public void FixedUpdate()
	{
		base.transform.position += this.vel * CupheadTime.FixedDelta;
		this.vel.y = this.vel.y - this.gravity * CupheadTime.FixedDelta;
		if (base.transform.position.y <= (float)Level.Current.Ground + -20f)
		{
			this.vel = Vector3.zero;
			this.gravity = 0f;
			base.animator.Play("Splat" + this.colorString);
		}
	}

	// Token: 0x0400215B RID: 8539
	public const float GROUND_OFFSET = -20f;

	// Token: 0x0400215C RID: 8540
	[SerializeField]
	public float gravity = 10f;

	// Token: 0x0400215D RID: 8541
	public Vector3 vel;
}
