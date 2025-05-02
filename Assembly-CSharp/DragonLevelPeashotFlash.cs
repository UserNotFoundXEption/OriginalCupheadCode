using System;
using UnityEngine;

// Token: 0x02000214 RID: 532
public class DragonLevelPeashotFlash : AbstractPausableComponent
{
	// Token: 0x0600185D RID: 6237 RVA: 0x000A3AA0 File Offset: 0x000A1CA0
	public void Flash()
	{
		base.transform.SetScale(new float?(1f), new float?((float)MathUtils.PlusOrMinus()), new float?(1f));
		base.animator.SetInteger("i", Random.Range(0, 4));
		base.animator.SetTrigger("OnChange");
	}
}
