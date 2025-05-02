using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000241 RID: 577
public class FlyingBlimpLevelFadeBackground : ScrollingSprite
{
	// Token: 0x06001A89 RID: 6793 RVA: 0x0001693F File Offset: 0x00014B3F
	public override void Start()
	{
		base.Start();
		base.FrameDelayedCallback(new Action(this.DisableSprites), 1);
	}

	// Token: 0x06001A8A RID: 6794 RVA: 0x000A8F18 File Offset: 0x000A7118
	public void DisableSprites()
	{
		this.fadeTime = 10f;
		this.current = base.GetComponent<SpriteRenderer>();
		this.replacementClones = this.replacementSprite.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		this.currentClones = this.current.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		this.replacementSprite.transform.position = new Vector2(base.transform.position.x, this.replacementSprite.transform.position.y);
		this.replacementSprite.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		for (int i = 0; i < this.replacementClones.Length; i++)
		{
			this.replacementClones[i].enabled = false;
		}
	}

	// Token: 0x06001A8B RID: 6795 RVA: 0x0001695B File Offset: 0x00014B5B
	public override void Update()
	{
		base.Update();
		if (this.moonLady.state == FlyingBlimpLevelMoonLady.State.Morph && !this.startedChange)
		{
			this.startedChange = true;
			this.StartChange();
		}
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x0001698C File Offset: 0x00014B8C
	public void StartChange()
	{
		base.StartCoroutine(this.change_cr());
	}

	// Token: 0x06001A8D RID: 6797 RVA: 0x000A8FF4 File Offset: 0x000A71F4
	public IEnumerator change_cr()
	{
		float t = 0f;
		float alphaValue = 1f;
		float startSpeed = this.speed;
		float endSpeed = this.speed + this.speed * 0.3f;
		while (t < this.fadeTime)
		{
			for (int j = 0; j < this.replacementClones.Length; j++)
			{
				if (this.replacementClones[j].transform != null)
				{
					this.replacementClones[j].enabled = true;
					this.replacementClones[j].color = new Color(1f, 1f, 1f, t / this.fadeTime);
				}
			}
			if (this.fadeOriginal)
			{
				for (int i = 0; i < this.currentClones.Length; i++)
				{
					if (this.currentClones[i].transform != null)
					{
						this.currentClones[i].color = new Color(1f, 1f, 1f, alphaValue - t / this.fadeTime);
						if (alphaValue <= 0f)
						{
							this.currentClones[i].color = new Color(1f, 1f, 1f, 0f);
							yield return null;
						}
					}
				}
			}
			this.speed = Mathf.Lerp(startSpeed, endSpeed, t / this.fadeTime);
			t += CupheadTime.Delta;
			yield return null;
		}
		for (int k = 0; k < this.replacementClones.Length; k++)
		{
			this.replacementClones[k].color = new Color(1f, 1f, 1f, 1f);
		}
		if (this.fadeOriginal)
		{
			for (int l = 0; l < this.currentClones.Length; l++)
			{
				this.currentClones[l].enabled = false;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x0400154A RID: 5450
	public bool fadeOriginal;

	// Token: 0x0400154B RID: 5451
	[SerializeField]
	public FlyingBlimpLevelMoonLady moonLady;

	// Token: 0x0400154C RID: 5452
	[SerializeField]
	public Transform replacementSprite;

	// Token: 0x0400154D RID: 5453
	public SpriteRenderer[] replacementClones;

	// Token: 0x0400154E RID: 5454
	public SpriteRenderer current;

	// Token: 0x0400154F RID: 5455
	public SpriteRenderer[] currentClones;

	// Token: 0x04001550 RID: 5456
	public float fadeTime;

	// Token: 0x04001551 RID: 5457
	public bool startedChange;
}
