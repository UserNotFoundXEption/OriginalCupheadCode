using System;
using UnityEngine;

// Token: 0x020002B0 RID: 688
public class GraveyardLevelSplitDevilBeamIgniteFX : Effect
{
	// Token: 0x06001EDD RID: 7901 RVA: 0x000B4690 File Offset: 0x000B2890
	public Effect Create(Vector3 position, Animator fireBeamAnimator)
	{
		GraveyardLevelSplitDevilBeamIgniteFX graveyardLevelSplitDevilBeamIgniteFX = base.Create(position) as GraveyardLevelSplitDevilBeamIgniteFX;
		graveyardLevelSplitDevilBeamIgniteFX.fireBeamAnimator = fireBeamAnimator;
		graveyardLevelSplitDevilBeamIgniteFX.UpdateFade(1f);
		return graveyardLevelSplitDevilBeamIgniteFX;
	}

	// Token: 0x06001EDE RID: 7902 RVA: 0x000B46C0 File Offset: 0x000B28C0
	public void Update()
	{
		this.frameTimer += CupheadTime.Delta;
		while (this.frameTimer > 0.0416666679f)
		{
			this.frameTimer -= 0.0416666679f;
			this.UpdateFade(0.25f);
		}
	}

	// Token: 0x06001EDF RID: 7903 RVA: 0x000B4718 File Offset: 0x000B2918
	public void UpdateFade(float amount)
	{
		bool @bool = this.fireBeamAnimator.GetBool("Smoke");
		foreach (SpriteRenderer spriteRenderer in this.groundRends)
		{
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Clamp(spriteRenderer.color.a + ((!@bool) ? (-amount) : amount), 0f, 1f));
		}
		foreach (SpriteRenderer spriteRenderer2 in this.noGroundRends)
		{
			spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, Mathf.Clamp(spriteRenderer2.color.a + ((!@bool) ? amount : (-amount)), 0f, 1f));
		}
	}

	// Token: 0x04001939 RID: 6457
	[SerializeField]
	public Animator fireBeamAnimator;

	// Token: 0x0400193A RID: 6458
	[SerializeField]
	public SpriteRenderer[] groundRends;

	// Token: 0x0400193B RID: 6459
	[SerializeField]
	public SpriteRenderer[] noGroundRends;

	// Token: 0x0400193C RID: 6460
	public float frameTimer;
}
