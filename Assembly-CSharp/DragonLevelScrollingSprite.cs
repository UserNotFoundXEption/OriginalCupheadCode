using System;
using UnityEngine;

// Token: 0x02000218 RID: 536
public class DragonLevelScrollingSprite : ScrollingSprite
{
	// Token: 0x06001875 RID: 6261 RVA: 0x00014E12 File Offset: 0x00013012
	public override void Awake()
	{
		base.Awake();
		this.playbackSpeed = 0f;
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x00014E25 File Offset: 0x00013025
	public override void Update()
	{
		this.playbackSpeed = Mathf.Lerp(0.1f, 1f, DragonLevel.SPEED);
		base.Update();
	}

	// Token: 0x040013D9 RID: 5081
	public const float MIN_SPEED = 0.1f;
}
