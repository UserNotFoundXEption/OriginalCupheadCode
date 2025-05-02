using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000263 RID: 611
public class FlyingGenieLevelBackgroundShade : AbstractPausableComponent
{
	// Token: 0x06001BEE RID: 7150 RVA: 0x00017AF6 File Offset: 0x00015CF6
	public void Start()
	{
		base.FrameDelayedCallback(new Action(this.GetSprites), 1);
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x00017B0C File Offset: 0x00015D0C
	public void GetSprites()
	{
		this.darkClones = this.darkSprite.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		base.StartCoroutine(this.fade_sprite_cr());
	}

	// Token: 0x06001BF0 RID: 7152 RVA: 0x000ACF70 File Offset: 0x000AB170
	public IEnumerator fade_sprite_cr()
	{
		for (;;)
		{
			float t = FlyingGenieLevel.mainTimer;
			float period = 12f;
			float shade = (Mathf.Sin(t * 3.14159274f * 2f / period) + 1f) / 2f;
			shade = Mathf.Lerp(this.fullSunOpacity, this.fullShadeOpactity, shade);
			foreach (SpriteRenderer spriteRenderer in this.darkClones)
			{
				spriteRenderer.color = new Color(spriteRenderer.GetComponent<SpriteRenderer>().color.r, spriteRenderer.GetComponent<SpriteRenderer>().color.g, spriteRenderer.GetComponent<SpriteRenderer>().color.b, shade);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040016B0 RID: 5808
	[SerializeField]
	public SpriteRenderer darkSprite;

	// Token: 0x040016B1 RID: 5809
	public SpriteRenderer[] darkClones;

	// Token: 0x040016B2 RID: 5810
	[SerializeField]
	public float fullSunOpacity;

	// Token: 0x040016B3 RID: 5811
	[SerializeField]
	public float fullShadeOpactity = 1f;
}
