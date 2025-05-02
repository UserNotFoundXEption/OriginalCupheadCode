using System;
using UnityEngine;

// Token: 0x02000270 RID: 624
public class FlyingGenieLevelMummyDeathEffect : Effect
{
	// Token: 0x06001CA3 RID: 7331 RVA: 0x000AE998 File Offset: 0x000ACB98
	public FlyingGenieLevelMummyDeathEffect Create(Vector3 pos, Color purpleColor)
	{
		FlyingGenieLevelMummyDeathEffect flyingGenieLevelMummyDeathEffect = base.Create(pos) as FlyingGenieLevelMummyDeathEffect;
		flyingGenieLevelMummyDeathEffect.transform.position = pos;
		flyingGenieLevelMummyDeathEffect.purpleColor = purpleColor;
		return flyingGenieLevelMummyDeathEffect;
	}

	// Token: 0x06001CA4 RID: 7332 RVA: 0x000AE9C8 File Offset: 0x000ACBC8
	public void CreateConfetti()
	{
		foreach (FlyingGenieLevelConfettiParts flyingGenieLevelConfettiParts in this.parts)
		{
			flyingGenieLevelConfettiParts.CreatePart(base.transform.position, this.purpleColor);
		}
	}

	// Token: 0x0400173F RID: 5951
	[SerializeField]
	public FlyingGenieLevelConfettiParts[] parts;

	// Token: 0x04001740 RID: 5952
	public Color purpleColor;
}
