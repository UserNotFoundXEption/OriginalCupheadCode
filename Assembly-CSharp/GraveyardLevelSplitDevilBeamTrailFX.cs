using System;
using UnityEngine;

// Token: 0x020002B1 RID: 689
public class GraveyardLevelSplitDevilBeamTrailFX : Effect
{
	// Token: 0x06001EE1 RID: 7905 RVA: 0x000B484C File Offset: 0x000B2A4C
	public Effect Create(Vector3 position, Vector3 scale, GraveyardLevelSplitDevilBeam main, int anim)
	{
		GraveyardLevelSplitDevilBeamTrailFX graveyardLevelSplitDevilBeamTrailFX = base.Create(position) as GraveyardLevelSplitDevilBeamTrailFX;
		graveyardLevelSplitDevilBeamTrailFX.transform.localScale = scale;
		graveyardLevelSplitDevilBeamTrailFX.main = main;
		graveyardLevelSplitDevilBeamTrailFX.animator.Play(anim.ToString());
		graveyardLevelSplitDevilBeamTrailFX.animator.Update(0f);
		graveyardLevelSplitDevilBeamTrailFX.rend.sortingOrder = -5 + anim;
		graveyardLevelSplitDevilBeamTrailFX.UpdateFade(1f);
		return graveyardLevelSplitDevilBeamTrailFX;
	}

	// Token: 0x06001EE2 RID: 7906 RVA: 0x000B48C0 File Offset: 0x000B2AC0
	public void Update()
	{
		this.frameTimer += CupheadTime.Delta;
		while (this.frameTimer > 0.0416666679f)
		{
			this.frameTimer -= 0.0416666679f;
			this.UpdateFade(0.25f);
		}
	}

	// Token: 0x06001EE3 RID: 7907 RVA: 0x000B4918 File Offset: 0x000B2B18
	public void UpdateFade(float amount)
	{
		this.rend.color = new Color(this.rend.color.r, this.rend.color.g, this.rend.color.b, Mathf.Clamp(this.rend.color.a + (this.main.devil.isAngel ? (-amount) : amount), 0f, 1f));
	}

	// Token: 0x0400193D RID: 6461
	[SerializeField]
	public GraveyardLevelSplitDevilBeam main;

	// Token: 0x0400193E RID: 6462
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x0400193F RID: 6463
	public float frameTimer;
}
