using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200026D RID: 621
public class FlyingGenieLevelMeditateFX : Effect
{
	// Token: 0x06001C89 RID: 7305 RVA: 0x0001834B File Offset: 0x0001654B
	public void Start()
	{
		base.StartCoroutine(this.effect_cr());
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x000AE5C4 File Offset: 0x000AC7C4
	public IEnumerator effect_cr()
	{
		SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
		sprite.color = new Color(1f, 1f, 1f, 0f);
		float t = 0f;
		float time = 1f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			sprite.color = new Color(1f, 1f, 1f, t / time);
			yield return null;
		}
		sprite.color = new Color(1f, 1f, 1f, 1f);
		t = 0f;
		for (;;)
		{
			this.frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (this.frameTime > 0.0833333358f)
			{
				base.transform.SetEulerAngles(null, null, new float?(360f * t));
				this.frameTime -= 0.0833333358f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C8B RID: 7307 RVA: 0x0001835A File Offset: 0x0001655A
	public void EndEffect()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.end_effect_cr());
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x000AE5E0 File Offset: 0x000AC7E0
	public IEnumerator end_effect_cr()
	{
		float t = 0f;
		float time = 1f;
		base.transform.SetEulerAngles(null, null, new float?(0f));
		while (t < time)
		{
			t += CupheadTime.Delta;
			base.transform.SetScale(new float?(1f - t / time), new float?(1f - t / time), null);
			yield return null;
		}
		this.OnEffectComplete();
		yield break;
	}

	// Token: 0x04001732 RID: 5938
	public const float FRAME_TIME = 0.0833333358f;

	// Token: 0x04001733 RID: 5939
	public float frameTime;
}
