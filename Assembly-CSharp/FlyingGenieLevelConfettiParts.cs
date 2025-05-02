using System;
using UnityEngine;

// Token: 0x02000266 RID: 614
public class FlyingGenieLevelConfettiParts : SpriteDeathParts
{
	// Token: 0x06001C08 RID: 7176 RVA: 0x000AD464 File Offset: 0x000AB664
	public FlyingGenieLevelConfettiParts CreatePart(Vector3 position, Color purpleColor)
	{
		FlyingGenieLevelConfettiParts flyingGenieLevelConfettiParts = base.CreatePart(position) as FlyingGenieLevelConfettiParts;
		flyingGenieLevelConfettiParts.purpleVersion.color = purpleColor;
		return flyingGenieLevelConfettiParts;
	}

	// Token: 0x040016BF RID: 5823
	[SerializeField]
	public SpriteRenderer purpleVersion;
}
