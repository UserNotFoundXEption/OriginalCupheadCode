using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000384 RID: 900
public class SlimeLevelLightRay : AbstractPausableComponent
{
	// Token: 0x060027BB RID: 10171 RVA: 0x00021575 File Offset: 0x0001F775
	public override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x060027BC RID: 10172 RVA: 0x000CC924 File Offset: 0x000CAB24
	public IEnumerator main_cr()
	{
		bool fadingOut = this.startVisible;
		SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
		if (!this.startVisible)
		{
			sprite.color = new Color(1f, 1f, 1f, 0f);
		}
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.holdTime);
			if (fadingOut)
			{
				float t = 0f;
				while (t < this.fadeTime)
				{
					sprite.color = new Color(1f, 1f, 1f, 1f - t / this.fadeTime);
					t += CupheadTime.Delta;
					yield return null;
				}
				sprite.color = new Color(1f, 1f, 1f, 0f);
			}
			else
			{
				float t2 = 0f;
				while (t2 < this.fadeTime)
				{
					sprite.color = new Color(1f, 1f, 1f, t2 / this.fadeTime);
					t2 += CupheadTime.Delta;
					yield return null;
				}
				sprite.color = new Color(1f, 1f, 1f, 1f);
			}
			fadingOut = !fadingOut;
		}
		yield break;
	}

	// Token: 0x040020EC RID: 8428
	[SerializeField]
	public float holdTime;

	// Token: 0x040020ED RID: 8429
	[SerializeField]
	public float fadeTime;

	// Token: 0x040020EE RID: 8430
	[SerializeField]
	public bool startVisible;
}
