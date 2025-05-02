using System;
using UnityEngine;

// Token: 0x0200024D RID: 589
public class FlyingCowboyFloatingSausages : Effect
{
	// Token: 0x06001AE7 RID: 6887 RVA: 0x00016DA5 File Offset: 0x00014FA5
	public void SetAnimation(string name)
	{
		base.animator.Play(name);
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x000A9D64 File Offset: 0x000A7F64
	public void FixedUpdate()
	{
		base.transform.position += Vector3.up * 200f * CupheadTime.FixedDelta;
		if (base.transform.position.y > 460f)
		{
			this.OnEffectComplete();
		}
	}

	// Token: 0x040015B3 RID: 5555
	public const float OFFSET = 100f;

	// Token: 0x040015B4 RID: 5556
	public const float SPEED = 200f;
}
