using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200023D RID: 573
public class FlyingBlimpLevelDarken : AbstractPausableComponent
{
	// Token: 0x06001A64 RID: 6756 RVA: 0x0001677C File Offset: 0x0001497C
	public void Update()
	{
		this.children = base.transform.GetComponentsInChildren<SpriteRenderer>();
		if (this.blimpLady.fading && !this.startedFade)
		{
			this.startedFade = true;
			this.StartFade();
		}
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x000167B7 File Offset: 0x000149B7
	public void StartFade()
	{
		base.StartCoroutine(this.fade_cr());
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x000A8624 File Offset: 0x000A6824
	public IEnumerator fade_cr()
	{
		float t = 0f;
		float fadeTime = 0.005f;
		float fadeVal = 1f;
		while (t < fadeTime && fadeVal > this.fadeMax)
		{
			for (int i = 0; i < this.children.Length; i++)
			{
				if (this.children[i].transform != null)
				{
					this.children[i].color = new Color(fadeVal - t / fadeTime, fadeVal - t / fadeTime, fadeVal - t / fadeTime, 1f);
				}
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		base.StartCoroutine(this.dark_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x000A8640 File Offset: 0x000A6840
	public IEnumerator dark_cr()
	{
		for (;;)
		{
			for (int i = 0; i < this.children.Length; i++)
			{
				if (this.children[i].transform != null)
				{
					this.children[i].color = new Color(this.fadeMax, this.fadeMax, this.fadeMax, 1f);
				}
			}
			if (!this.blimpLady.fading)
			{
				break;
			}
			yield return null;
		}
		base.StartCoroutine(this.light_cr());
		yield break;
		yield break;
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x000A865C File Offset: 0x000A685C
	public IEnumerator light_cr()
	{
		float fadeMid = 0.87f;
		while (this.startedFade)
		{
			for (int i = 0; i < this.children.Length; i++)
			{
				if (this.children[i] != null && this.children[i].transform != null)
				{
					this.children[i].color = new Color(fadeMid, fadeMid, fadeMid, 1f);
				}
			}
			if (this.blimpLady.state == FlyingBlimpLevelBlimpLady.State.Idle)
			{
				for (int j = 0; j < this.children.Length; j++)
				{
					if (this.children[j].transform != null)
					{
						this.children[j].color = new Color(1f, 1f, 1f, 1f);
					}
				}
				this.startedFade = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001527 RID: 5415
	[SerializeField]
	public FlyingBlimpLevelBlimpLady blimpLady;

	// Token: 0x04001528 RID: 5416
	public SpriteRenderer[] children;

	// Token: 0x04001529 RID: 5417
	public float fadeMax = 0.75f;

	// Token: 0x0400152A RID: 5418
	public bool startedFade;
}
