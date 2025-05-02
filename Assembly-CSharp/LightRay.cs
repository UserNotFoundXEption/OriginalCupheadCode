using System;
using UnityEngine;

// Token: 0x020005A6 RID: 1446
public class LightRay : AbstractMonoBehaviour
{
	// Token: 0x06003D00 RID: 15616 RVA: 0x00117844 File Offset: 0x00115A44
	public void Start()
	{
		float num = (!this.randomOffset) ? this.customOffset : Random.Range(0f, 1f);
		this.t = 4f * num / this.speed;
	}

	// Token: 0x06003D01 RID: 15617 RVA: 0x0011788C File Offset: 0x00115A8C
	public void Update()
	{
		if (!this.widthCached)
		{
			Texture2D texture = base.GetComponent<SpriteRenderer>().sprite.texture;
			if (texture == null)
			{
				return;
			}
			this.textureWidth = 2.32324839f / ((float)texture.width / (float)texture.height);
			this.widthCached = true;
		}
		this.accumulator += CupheadTime.Delta;
		while (this.accumulator > 0.0416666679f)
		{
			this.accumulator -= 0.0416666679f;
			this.t += 0.0416666679f;
		}
		Material material = base.GetComponent<SpriteRenderer>().material;
		material.SetFloat("t", this.t);
		material.SetFloat("textureWidth", this.textureWidth);
		material.SetFloat("textureSpeed", this.speed);
	}

	// Token: 0x0400307C RID: 12412
	public float t;

	// Token: 0x0400307D RID: 12413
	public float accumulator;

	// Token: 0x0400307E RID: 12414
	public float textureWidth;

	// Token: 0x0400307F RID: 12415
	[SerializeField]
	public float speed = 0.03f;

	// Token: 0x04003080 RID: 12416
	[SerializeField]
	public bool randomOffset = true;

	// Token: 0x04003081 RID: 12417
	[SerializeField]
	[Range(0f, 1f)]
	public float customOffset = 0.5f;

	// Token: 0x04003082 RID: 12418
	public bool widthCached;
}
