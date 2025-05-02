using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000446 RID: 1094
public class MountainPlatformingLevelMinerRope : AbstractPausableComponent
{
	// Token: 0x06002EF4 RID: 12020 RVA: 0x000271C8 File Offset: 0x000253C8
	public void PullRope(float ascendTime, Vector2 startPos)
	{
		base.StartCoroutine(this.pull_up_rope_cr(ascendTime, startPos));
	}

	// Token: 0x06002EF5 RID: 12021 RVA: 0x000E0A30 File Offset: 0x000DEC30
	public IEnumerator pull_up_rope_cr(float ascendTime, Vector2 startPos)
	{
		float t = 0f;
		Vector3 end = new Vector3(base.transform.position.x, startPos.y + 400f);
		Vector3 start = base.transform.position;
		while (t < ascendTime)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / ascendTime);
			base.transform.position = Vector2.Lerp(start, end, val);
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}
}
