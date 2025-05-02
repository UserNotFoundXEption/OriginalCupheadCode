using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000248 RID: 584
public class FlyingBlimpLevelSparkle : ScrollingSprite
{
	// Token: 0x06001AC3 RID: 6851 RVA: 0x00016BD6 File Offset: 0x00014DD6
	public override void Start()
	{
		base.Start();
		base.FrameDelayedCallback(new Action(this.DisableStars), 1);
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x000A98BC File Offset: 0x000A7ABC
	public void DisableStars()
	{
		this.twinkleSprite = base.GetComponent<SpriteRenderer>();
		this.starClones = this.starSprite.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		this.twinkleClones = base.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		this.starSprite.enabled = false;
		this.twinkleSprite.enabled = false;
		this.change = true;
		this.fadeTime = 0.8f;
		for (int i = 0; i < this.starClones.Length; i++)
		{
			this.starClones[i].enabled = false;
		}
		for (int j = 0; j < this.twinkleClones.Length; j++)
		{
			this.twinkleClones[j].enabled = false;
		}
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x00016BF2 File Offset: 0x00014DF2
	public override void Update()
	{
		base.Update();
		if (this.moonLady.state == FlyingBlimpLevelMoonLady.State.Morph && this.change)
		{
			base.StartCoroutine(this.fadein_cr());
			this.change = false;
		}
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x000A9980 File Offset: 0x000A7B80
	public IEnumerator fadein_cr()
	{
		float t = 0f;
		this.starSprite.enabled = true;
		while (t < this.fadeTime)
		{
			this.starSprite.color = new Color(1f, 1f, 1f, t / this.fadeTime);
			for (int i = 0; i < this.starClones.Length; i++)
			{
				this.starClones[i].enabled = true;
				this.starClones[i].color = new Color(1f, 1f, 1f, t / this.fadeTime);
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		this.starSprite.color = new Color(1f, 1f, 1f, 1f);
		for (int j = 0; j < this.starClones.Length; j++)
		{
			this.starClones[j].color = new Color(1f, 1f, 1f, 1f);
		}
		for (int k = 0; k < this.twinkleClones.Length; k++)
		{
			this.twinkleClones[k].color = new Color(1f, 1f, 1f, 1f);
		}
		base.StartCoroutine(this.twinkle_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x000A999C File Offset: 0x000A7B9C
	public IEnumerator twinkle_cr()
	{
		this.twinkleSprite.enabled = true;
		for (int i = 0; i < this.twinkleClones.Length; i++)
		{
			this.twinkleClones[i].enabled = true;
		}
		for (;;)
		{
			this.getSecond = Random.Range(this.minSecond, this.maxSecond);
			if (this.fadeIn)
			{
				float t = 0f;
				while (t < this.fadeTime)
				{
					this.twinkleSprite.color = new Color(1f, 1f, 1f, t / this.fadeTime);
					for (int j = 0; j < this.twinkleClones.Length; j++)
					{
						this.twinkleClones[j].color = new Color(1f, 1f, 1f, t / this.fadeTime);
					}
					t += CupheadTime.Delta;
					yield return null;
				}
				this.twinkleSprite.color = new Color(1f, 1f, 1f, 1f);
				for (int k = 0; k < this.twinkleClones.Length; k++)
				{
					this.twinkleClones[k].color = new Color(1f, 1f, 1f, 1f);
				}
			}
			else
			{
				float t2 = 0f;
				while (t2 < this.fadeTime)
				{
					this.twinkleSprite.color = new Color(1f, 1f, 1f, 1f - t2 / this.fadeTime);
					for (int l = 0; l < this.twinkleClones.Length; l++)
					{
						this.twinkleClones[l].color = new Color(1f, 1f, 1f, 1f - t2 / this.fadeTime);
					}
					t2 += CupheadTime.Delta;
					yield return null;
				}
				this.twinkleSprite.color = new Color(1f, 1f, 1f, 0f);
				for (int m = 0; m < this.twinkleClones.Length; m++)
				{
					this.twinkleClones[m].color = new Color(1f, 1f, 1f, 0f);
				}
				yield return CupheadTime.WaitForSeconds(this, this.getSecond);
			}
			this.fadeIn = !this.fadeIn;
		}
		yield break;
	}

	// Token: 0x0400158F RID: 5519
	[SerializeField]
	public float minSecond;

	// Token: 0x04001590 RID: 5520
	[SerializeField]
	public float maxSecond;

	// Token: 0x04001591 RID: 5521
	public float getSecond;

	// Token: 0x04001592 RID: 5522
	public float fadeTime;

	// Token: 0x04001593 RID: 5523
	public float setSpeed;

	// Token: 0x04001594 RID: 5524
	public bool fadeIn;

	// Token: 0x04001595 RID: 5525
	public bool change;

	// Token: 0x04001596 RID: 5526
	[SerializeField]
	public FlyingBlimpLevelMoonLady moonLady;

	// Token: 0x04001597 RID: 5527
	public SpriteRenderer twinkleSprite;

	// Token: 0x04001598 RID: 5528
	[SerializeField]
	public SpriteRenderer starSprite;

	// Token: 0x04001599 RID: 5529
	public SpriteRenderer[] twinkleClones;

	// Token: 0x0400159A RID: 5530
	public SpriteRenderer[] starClones;
}
