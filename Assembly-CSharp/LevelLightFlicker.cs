using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class LevelLightFlicker : AbstractPausableComponent
{
	// Token: 0x06000A34 RID: 2612 RVA: 0x000094F5 File Offset: 0x000076F5
	public void Start()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x0007B270 File Offset: 0x00079470
	public IEnumerator flicker_cr()
	{
		float flickerTime = 0.3f;
		for (;;)
		{
			int counter = 0;
			float waitTime = Random.Range(this.fadeWaitMinSecond, this.fadeWaitMaxSecond);
			float t = 0f;
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			while (counter < this.countUntilPause)
			{
				while (t < flickerTime)
				{
					this.sprite.color = new Color(1f, 1f, 1f, 1f - t / flickerTime);
					t += CupheadTime.Delta;
					yield return null;
				}
				t = 0f;
				this.sprite.color = new Color(1f, 1f, 1f, 0f);
				while (t < flickerTime)
				{
					this.sprite.color = new Color(1f, 1f, 1f, t / flickerTime);
					t += CupheadTime.Delta;
					yield return null;
				}
				this.sprite.color = new Color(1f, 1f, 1f, 1f);
				counter++;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400082D RID: 2093
	[SerializeField]
	public float fadeWaitMinSecond = 8f;

	// Token: 0x0400082E RID: 2094
	[SerializeField]
	public float fadeWaitMaxSecond = 25f;

	// Token: 0x0400082F RID: 2095
	[SerializeField]
	public int countUntilPause = 3;

	// Token: 0x04000830 RID: 2096
	public SpriteRenderer sprite;
}
