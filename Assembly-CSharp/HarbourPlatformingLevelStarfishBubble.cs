using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000435 RID: 1077
public class HarbourPlatformingLevelStarfishBubble : Effect
{
	// Token: 0x06002E6E RID: 11886 RVA: 0x00026B78 File Offset: 0x00024D78
	public void Start()
	{
		this.sinWaveStrength = Random.Range(0.4f, 0.9f);
		this.speed = Random.Range(50f, 100f);
		base.StartCoroutine(this.deathrotation_cr());
	}

	// Token: 0x06002E6F RID: 11887 RVA: 0x000DF95C File Offset: 0x000DDB5C
	public IEnumerator deathrotation_cr()
	{
		float totalTime = 0f;
		float maxTime = Random.Range(4f, 7f);
		float t = Random.Range(0f, 6.28318548f);
		while (totalTime < maxTime)
		{
			totalTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			base.transform.SetPosition(new float?(base.transform.position.x + Mathf.Sin(t) * this.sinWaveStrength * CupheadTime.Delta * 60f), null, null);
			base.transform.AddPosition(0f, this.speed * CupheadTime.Delta, 0f);
			yield return null;
		}
		base.animator.Play("Pop");
		yield return null;
		yield break;
	}

	// Token: 0x04002683 RID: 9859
	public const float ROTATE_FRAME_TIME = 0.0833333358f;

	// Token: 0x04002684 RID: 9860
	public Vector3 pos;

	// Token: 0x04002685 RID: 9861
	public float speed;

	// Token: 0x04002686 RID: 9862
	public float sinWaveStrength;
}
