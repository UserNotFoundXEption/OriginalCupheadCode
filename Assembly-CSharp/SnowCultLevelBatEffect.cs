using System;
using UnityEngine;

// Token: 0x0200038D RID: 909
public class SnowCultLevelBatEffect : Effect
{
	// Token: 0x0600282B RID: 10283 RVA: 0x00021B2C File Offset: 0x0001FD2C
	public void SetColor(string s)
	{
		this.colorString = s;
		base.animator.Play(this.baseAnimName + s);
		if (this.secondaryRenderer)
		{
			this.secondaryRenderer.flipX = Rand.Bool();
		}
	}

	// Token: 0x0400215E RID: 8542
	[SerializeField]
	public SpriteRenderer secondaryRenderer;

	// Token: 0x0400215F RID: 8543
	[SerializeField]
	public string baseAnimName;

	// Token: 0x04002160 RID: 8544
	public string colorString;
}
