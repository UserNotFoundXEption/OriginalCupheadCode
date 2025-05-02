using System;
using UnityEngine;

// Token: 0x02000135 RID: 309
public class AirplaneLevelTerrierSmokeFX : Effect
{
	// Token: 0x06000EB6 RID: 3766 RVA: 0x0000C743 File Offset: 0x0000A943
	public void Step(float t)
	{
		this.myTransform.position += this.vel * t;
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x0000C767 File Offset: 0x0000A967
	public override void OnEffectComplete()
	{
		if (this.dead)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			this.inUse = false;
		}
	}

	// Token: 0x04000C0F RID: 3087
	public SpriteRenderer rend;

	// Token: 0x04000C10 RID: 3088
	public Vector3 vel;

	// Token: 0x04000C11 RID: 3089
	public bool dead;

	// Token: 0x04000C12 RID: 3090
	public new bool inUse = true;

	// Token: 0x04000C13 RID: 3091
	public Transform myTransform;
}
