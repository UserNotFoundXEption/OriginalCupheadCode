using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class RetroArcadeWormPlatform : LevelPlatform
{
	// Token: 0x06002326 RID: 8998 RVA: 0x0001DC14 File Offset: 0x0001BE14
	public void Rise()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002327 RID: 8999 RVA: 0x000BFC80 File Offset: 0x000BDE80
	public IEnumerator move_cr()
	{
		float moveTime = 1f;
		float t = 0f;
		while (t < moveTime)
		{
			t += CupheadTime.FixedDelta;
			base.transform.AddPosition(0f, 50f * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x04001D36 RID: 7478
	public const float MOVE_Y = 50f;

	// Token: 0x04001D37 RID: 7479
	public const float MOVE_Y_SPEED = 50f;
}
