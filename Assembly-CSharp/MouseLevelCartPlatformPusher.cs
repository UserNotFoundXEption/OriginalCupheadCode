using System;
using UnityEngine;

// Token: 0x020002C6 RID: 710
public class MouseLevelCartPlatformPusher : AbstractCollidableObject
{
	// Token: 0x06001F9F RID: 8095 RVA: 0x000B64B0 File Offset: 0x000B46B0
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		AbstractPlayerController component = hit.GetComponent<AbstractPlayerController>();
		Collider2D component2 = base.GetComponent<Collider2D>();
		Collider2D component3 = component.GetComponent<Collider2D>();
		if (component.bottom < component2.bounds.max.y)
		{
			if (component.center.x < component2.bounds.center.x)
			{
				float num = component3.bounds.max.x - component2.bounds.min.x;
				if (num > 0f)
				{
					component.transform.AddPosition(-num, 0f, 0f);
				}
			}
			else
			{
				float num2 = component2.bounds.max.x - component3.bounds.min.x;
				if (num2 > 0f)
				{
					component.transform.AddPosition(num2, 0f, 0f);
				}
			}
		}
	}
}
