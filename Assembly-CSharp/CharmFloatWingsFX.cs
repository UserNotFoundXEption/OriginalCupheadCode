using System;
using UnityEngine;

// Token: 0x0200051C RID: 1308
public class CharmFloatWingsFX : Effect
{
	// Token: 0x0600376A RID: 14186 RVA: 0x00103300 File Offset: 0x00101500
	public override Effect Create(Vector3 position, Vector3 scale)
	{
		CharmFloatWingsFX charmFloatWingsFX = base.Create(position, scale) as CharmFloatWingsFX;
		charmFloatWingsFX.anim.Play("Feather", 0, Random.Range(0f, 0.5f));
		charmFloatWingsFX.vel = MathUtils.AngleToDirection((float)Random.Range(0, 360)) * (Random.Range(this.startSpeedMin, this.startSpeedMax) + ((Random.Range(0f, 6f) >= 1f) ? 0f : this.startSpeedMax));
		return charmFloatWingsFX;
	}

	// Token: 0x0600376B RID: 14187 RVA: 0x00103398 File Offset: 0x00101598
	public void FixedUpdate()
	{
		base.transform.position += this.vel;
		this.vel *= this.slowFactor;
		this.vel.y = this.vel.y + this.riseFactor;
	}

	// Token: 0x04002C8A RID: 11402
	[SerializeField]
	public Animator anim;

	// Token: 0x04002C8B RID: 11403
	[SerializeField]
	public Vector3 vel;

	// Token: 0x04002C8C RID: 11404
	[SerializeField]
	public float startSpeedMin = 10f;

	// Token: 0x04002C8D RID: 11405
	[SerializeField]
	public float startSpeedMax = 20f;

	// Token: 0x04002C8E RID: 11406
	[SerializeField]
	public float slowFactor = 0.95f;

	// Token: 0x04002C8F RID: 11407
	[SerializeField]
	public float riseFactor = 0.02f;
}
