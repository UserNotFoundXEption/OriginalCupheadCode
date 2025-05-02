using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003BD RID: 957
public class TrainLevelTrackBumper : AbstractPausableComponent
{
	// Token: 0x06002A52 RID: 10834 RVA: 0x00023A47 File Offset: 0x00021C47
	public override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06002A53 RID: 10835 RVA: 0x000D3BCC File Offset: 0x000D1DCC
	public IEnumerator main_cr()
	{
		float startingY = base.transform.position.y;
		float t = 0f;
		for (;;)
		{
			t += CupheadTime.Delta;
			float d = (base.transform.position.x + 6000f * t) % 4500f;
			float y = startingY;
			if (d < 600f)
			{
				float num = d / 600f;
				y += Mathf.Sin(num * 3.14159274f) * 3f;
			}
			base.transform.SetPosition(null, new float?(y), null);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002349 RID: 9033
	public const float bumpSpeed = 6000f;

	// Token: 0x0400234A RID: 9034
	public const float bumpHeight = 3f;

	// Token: 0x0400234B RID: 9035
	public const float bumpDuration = 0.1f;

	// Token: 0x0400234C RID: 9036
	public const float bumpPeriod = 0.75f;
}
