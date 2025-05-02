using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class ClownLevelBackgroundLights : AbstractPausableComponent
{
	// Token: 0x06001372 RID: 4978 RVA: 0x000104DB File Offset: 0x0000E6DB
	public void Start()
	{
		this.lightVersion = base.GetComponent<SpriteRenderer>();
		base.StartCoroutine(this.lights_cr());
		if (this.occasionalFlicker)
		{
			base.StartCoroutine(this.flicker_cr());
		}
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x00097EF4 File Offset: 0x000960F4
	public IEnumerator lights_cr()
	{
		for (;;)
		{
			this.fadeTime = Random.Range(this.fadeDurationMin, this.fadeDurationMax);
			this.getSecond = Random.Range(this.waitMinSecond, this.waitMaxSecond);
			if (this.fadeIn)
			{
				float t = 0f;
				while (t < this.fadeTime)
				{
					this.lightVersion.color = new Color(1f, 1f, 1f, t / this.fadeTime);
					t += CupheadTime.Delta;
					yield return null;
				}
				this.lightVersion.color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				float t2 = 0f;
				while (t2 < this.fadeTime)
				{
					this.lightVersion.color = new Color(1f, 1f, 1f, 1f - t2 / this.fadeTime);
					t2 += CupheadTime.Delta;
					yield return null;
				}
				this.lightVersion.color = new Color(1f, 1f, 1f, 0f);
				yield return CupheadTime.WaitForSeconds(this, this.getSecond);
			}
			this.fadeIn = !this.fadeIn;
		}
		yield break;
	}

	// Token: 0x06001374 RID: 4980 RVA: 0x00097F10 File Offset: 0x00096110
	public IEnumerator flicker_cr()
	{
		for (;;)
		{
			float waitTime = Random.Range(this.fadeWaitMinSecond, this.fadeWaitMaxSecond);
			float flickerTime = Random.Range(this.flickerMinTime, this.flickerMaxTime);
			float t = 0f;
			while (t < flickerTime)
			{
				this.fadeTime = 0.08f;
				t += CupheadTime.Delta;
				yield return null;
			}
			this.fadeTime = 0f;
			yield return CupheadTime.WaitForSeconds(this, waitTime);
		}
		yield break;
	}

	// Token: 0x04000FC3 RID: 4035
	public float waitMinSecond;

	// Token: 0x04000FC4 RID: 4036
	public float waitMaxSecond = 1f;

	// Token: 0x04000FC5 RID: 4037
	public float fadeDurationMin = 0.5f;

	// Token: 0x04000FC6 RID: 4038
	public float fadeDurationMax = 2f;

	// Token: 0x04000FC7 RID: 4039
	[SerializeField]
	public bool occasionalFlicker;

	// Token: 0x04000FC8 RID: 4040
	public float fadeWaitMinSecond = 5f;

	// Token: 0x04000FC9 RID: 4041
	public float fadeWaitMaxSecond = 10f;

	// Token: 0x04000FCA RID: 4042
	public float flickerMinTime = 1f;

	// Token: 0x04000FCB RID: 4043
	public float flickerMaxTime = 5f;

	// Token: 0x04000FCC RID: 4044
	public SpriteRenderer lightVersion;

	// Token: 0x04000FCD RID: 4045
	public float getSecond;

	// Token: 0x04000FCE RID: 4046
	public float fadeTime;

	// Token: 0x04000FCF RID: 4047
	public bool fadeIn;
}
