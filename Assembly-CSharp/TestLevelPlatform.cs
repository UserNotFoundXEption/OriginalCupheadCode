using System;
using System.Collections;

// Token: 0x0200011D RID: 285
public class TestLevelPlatform : LevelPlatform
{
	// Token: 0x06000D8A RID: 3466 RVA: 0x0000B905 File Offset: 0x00009B05
	public void Start()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06000D8B RID: 3467 RVA: 0x00087744 File Offset: 0x00085944
	public IEnumerator loop_cr()
	{
		for (;;)
		{
			yield return base.TweenLocalPositionX(-700f, 700f, 4f, EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenLocalPositionX(700f, -700f, 4f, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x04000A8F RID: 2703
	public const float X = 700f;

	// Token: 0x04000A90 RID: 2704
	public const float TIME = 4f;

	// Token: 0x04000A91 RID: 2705
	public const EaseUtils.EaseType EASE = EaseUtils.EaseType.easeInOutSine;
}
