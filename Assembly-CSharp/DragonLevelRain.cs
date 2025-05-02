using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000217 RID: 535
public class DragonLevelRain : AbstractPausableComponent
{
	// Token: 0x06001872 RID: 6258 RVA: 0x00014DEF File Offset: 0x00012FEF
	public void StartRain()
	{
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.fade_cr());
	}

	// Token: 0x06001873 RID: 6259 RVA: 0x000A3DE8 File Offset: 0x000A1FE8
	public IEnumerator fade_cr()
	{
		float alpha = 0f;
		while (alpha < 1f)
		{
			for (int i = 0; i < this.rainRenderers.Length; i++)
			{
				Color color = this.rainRenderers[i].color;
				color.a = alpha;
				this.rainRenderers[i].color = color;
			}
			alpha += CupheadTime.Delta / this.fadeTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x040013D7 RID: 5079
	[SerializeField]
	public float fadeTime;

	// Token: 0x040013D8 RID: 5080
	[SerializeField]
	public SpriteRenderer[] rainRenderers;
}
