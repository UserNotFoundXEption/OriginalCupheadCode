using System;
using UnityEngine;

// Token: 0x02000484 RID: 1156
public class MapLevelLoaderLadder : MapLevelLoader
{
	// Token: 0x060030D4 RID: 12500 RVA: 0x000288E8 File Offset: 0x00026AE8
	public void EnableShadow(bool enabled)
	{
		this.shadowRenderer.enabled = enabled;
	}

	// Token: 0x060030D5 RID: 12501 RVA: 0x000E7AC4 File Offset: 0x000E5CC4
	public void animationEvent_DownStarted()
	{
		int num = Random.Range(0, this.smokeRenderers.Length);
		for (int i = 0; i < this.smokeRenderers.Length; i++)
		{
			this.smokeRenderers[i].enabled = (i == num);
		}
	}

	// Token: 0x04002851 RID: 10321
	[SerializeField]
	public SpriteRenderer shadowRenderer;

	// Token: 0x04002852 RID: 10322
	[SerializeField]
	public SpriteRenderer[] smokeRenderers;
}
