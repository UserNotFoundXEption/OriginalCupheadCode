using System;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class MountainPlatformingLevelScalePart : AbstractCollidableObject
{
	// Token: 0x06002F3B RID: 12091 RVA: 0x00027594 File Offset: 0x00025794
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<AbstractPlayerController>() != null)
		{
			if (phase == CollisionPhase.Exit)
			{
				this.steppedOn = false;
			}
			else
			{
				this.steppedOn = true;
			}
		}
	}

	// Token: 0x0400272F RID: 10031
	public bool steppedOn;
}
