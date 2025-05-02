using System;
using UnityEngine;

// Token: 0x020005B0 RID: 1456
public class SpriteDeathPartsDLC : SpriteDeathParts
{
	// Token: 0x06003D29 RID: 15657 RVA: 0x001185C0 File Offset: 0x001167C0
	public void Start()
	{
		if (this.progressiveBlur)
		{
			this.rend.material.SetFloat("_BlurAmount", 0f);
			this.rend.material.SetFloat("_BlurLerp", 0f);
		}
		if (this.progressiveDim)
		{
			this.startColor = this.rend.color;
		}
	}

	// Token: 0x06003D2A RID: 15658 RVA: 0x00031575 File Offset: 0x0002F775
	public void SetVelocity(Vector3 vel)
	{
		this.velocity = vel;
	}

	// Token: 0x06003D2B RID: 15659 RVA: 0x00031583 File Offset: 0x0002F783
	public void FixedUpdate()
	{
		if (CupheadTime.FixedDelta > 0f)
		{
			this.Step(CupheadTime.FixedDelta * 1.2f);
		}
	}

	// Token: 0x06003D2C RID: 15660 RVA: 0x00118628 File Offset: 0x00116828
	public override void Step(float deltaTime)
	{
		base.Step(deltaTime);
		if (this.progressiveBlur)
		{
			this.rend.material.SetFloat("_BlurAmount", this.rend.material.GetFloat("_BlurAmount") + deltaTime * this.blurIncreaseSpeed);
			this.rend.material.SetFloat("_BlurLerp", this.rend.material.GetFloat("_BlurLerp") + deltaTime * this.blurIncreaseSpeed);
		}
		if (this.progressiveDim)
		{
			this.dimTimer += deltaTime * this.dimIncreaseSpeed;
			this.rend.color = Color.Lerp(this.startColor, Color.black, this.dimTimer);
		}
	}

	// Token: 0x06003D2D RID: 15661 RVA: 0x000315A5 File Offset: 0x0002F7A5
	public override void Update()
	{
	}

	// Token: 0x040030BB RID: 12475
	public const float UPDATE_TIMING_ADJUST = 1.2f;

	// Token: 0x040030BC RID: 12476
	[SerializeField]
	public bool progressiveBlur;

	// Token: 0x040030BD RID: 12477
	[SerializeField]
	public float blurIncreaseSpeed = 3f;

	// Token: 0x040030BE RID: 12478
	[SerializeField]
	public bool progressiveDim;

	// Token: 0x040030BF RID: 12479
	[SerializeField]
	public float dimIncreaseSpeed = 3f;

	// Token: 0x040030C0 RID: 12480
	public Color startColor;

	// Token: 0x040030C1 RID: 12481
	public float dimTimer;

	// Token: 0x040030C2 RID: 12482
	[SerializeField]
	public SpriteRenderer rend;
}
