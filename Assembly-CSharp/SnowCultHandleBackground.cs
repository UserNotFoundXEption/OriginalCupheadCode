using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000389 RID: 905
public class SnowCultHandleBackground : AbstractPausableComponent
{
	// Token: 0x06002813 RID: 10259 RVA: 0x000CD430 File Offset: 0x000CB630
	public void Update()
	{
		this.fadeTimer += CupheadTime.Delta;
		for (int i = 0; i < this.fadeRenderers.Length; i++)
		{
			this.fadeRenderers[i].color = new Color(1f, 1f, 1f, Mathf.Lerp(this.fadeMin[i], this.fadeMax[i], Mathf.Abs((this.fadeTimer + this.fadeOffset[i] * this.fadePeriod[i]) % this.fadePeriod[i] - this.fadePeriod[i] / 2f)) / (this.fadePeriod[i] / 2f));
		}
		this.glimmerTimer -= CupheadTime.Delta;
		if (this.glimmerTimer <= 0f)
		{
			this.glimmer.Play("Glimmer", 0, 0f);
			this.glimmerTimer += Random.Range(3.5f, 6.5f);
		}
		this.sparkleTimer -= CupheadTime.Delta;
		if (this.sparkleTimer <= 0f)
		{
			if (this.sparkleList.Count == 0)
			{
				for (int j = 0; j < this.sparkles.Length; j++)
				{
					this.sparkleList.Add(j);
				}
			}
			int index = Random.Range(0, this.sparkleList.Count);
			this.sparkles[this.sparkleList[index]].Play("Sparkle", 0, 0f);
			this.sparkleList.RemoveAt(index);
			this.sparkleTimer = Random.Range(0.25f, 0.75f);
		}
	}

	// Token: 0x06002814 RID: 10260 RVA: 0x000CD5F4 File Offset: 0x000CB7F4
	public void CandleGust()
	{
		foreach (Animator animator in this.candles)
		{
			animator.SetTrigger("OnGust");
		}
	}

	// Token: 0x0400212D RID: 8493
	public float fadeTimer;

	// Token: 0x0400212E RID: 8494
	[SerializeField]
	public SpriteRenderer[] fadeRenderers;

	// Token: 0x0400212F RID: 8495
	[SerializeField]
	public float[] fadePeriod;

	// Token: 0x04002130 RID: 8496
	[SerializeField]
	public float[] fadeOffset;

	// Token: 0x04002131 RID: 8497
	[SerializeField]
	public float[] fadeMin;

	// Token: 0x04002132 RID: 8498
	[SerializeField]
	public float[] fadeMax;

	// Token: 0x04002133 RID: 8499
	[SerializeField]
	public Animator[] candles;

	// Token: 0x04002134 RID: 8500
	[SerializeField]
	public Animator glimmer;

	// Token: 0x04002135 RID: 8501
	public float glimmerTimer = 2f;

	// Token: 0x04002136 RID: 8502
	[SerializeField]
	public Animator[] sparkles;

	// Token: 0x04002137 RID: 8503
	public float sparkleTimer = 1f;

	// Token: 0x04002138 RID: 8504
	public List<int> sparkleList = new List<int>();
}
