using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200020A RID: 522
public class DragonLevelBackgroundChange : DragonLevelScrollingSprite
{
	// Token: 0x060017F6 RID: 6134 RVA: 0x00014753 File Offset: 0x00012953
	public override void Start()
	{
		base.Start();
		base.FrameDelayedCallback(new Action(this.DisableSprites), 1);
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x000A2B6C File Offset: 0x000A0D6C
	public void DisableSprites()
	{
		this.fadeTime = 6f;
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

	// Token: 0x060017F8 RID: 6136 RVA: 0x0001476F File Offset: 0x0001296F
	public void StartChange()
	{
		base.StartCoroutine(this.change_cr());
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x000A2C48 File Offset: 0x000A0E48
	public IEnumerator change_cr()
	{
		float t = 0f;
		while (t < this.fadeTime)
		{
			for (int i = 0; i < this.replacementClones.Length; i++)
			{
				if (this.replacementClones[i].transform != null)
				{
					this.replacementClones[i].enabled = true;
					this.replacementClones[i].color = new Color(1f, 1f, 1f, t / this.fadeTime);
				}
			}
			for (int j = 0; j < this.currentClones.Length; j++)
			{
				this.currentClones[j].color = new Color(1f, 1f, 1f, 1f - t / this.fadeTime);
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		for (int k = 0; k < this.replacementClones.Length; k++)
		{
			this.replacementClones[k].color = new Color(1f, 1f, 1f, 1f);
		}
		for (int l = 0; l < this.currentClones.Length; l++)
		{
			this.currentClones[l].enabled = false;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x0001477E File Offset: 0x0001297E
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.replacementClones = null;
		this.currentClones = null;
	}

	// Token: 0x04001369 RID: 4969
	[SerializeField]
	public Transform replacementSprite;

	// Token: 0x0400136A RID: 4970
	public SpriteRenderer[] replacementClones;

	// Token: 0x0400136B RID: 4971
	public SpriteRenderer current;

	// Token: 0x0400136C RID: 4972
	public SpriteRenderer[] currentClones;

	// Token: 0x0400136D RID: 4973
	public bool changeStart;

	// Token: 0x0400136E RID: 4974
	public float fadeTime;
}
