using System;
using UnityEngine;

// Token: 0x0200051D RID: 1309
public class CharmFloatWingsFXAlt : Effect
{
	// Token: 0x0600376D RID: 14189 RVA: 0x001033F0 File Offset: 0x001015F0
	public override Effect Create(Vector3 position, Vector3 scale)
	{
		CharmFloatWingsFXAlt charmFloatWingsFXAlt = base.Create(position, scale) as CharmFloatWingsFXAlt;
		charmFloatWingsFXAlt.anim.speed = 1f;
		charmFloatWingsFXAlt.anim.Play("Feather", 0, Random.Range(0f, 0.5f));
		charmFloatWingsFXAlt.vel = MathUtils.AngleToDirection((float)(Random.Range(-45, -145) + ((!Rand.Bool()) ? -50 : 50))) * this.startSpeed;
		charmFloatWingsFXAlt.vel.y = 0f;
		charmFloatWingsFXAlt.startVel = charmFloatWingsFXAlt.vel.x;
		charmFloatWingsFXAlt.transform.rotation = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(this.vel) + -90f * Mathf.Sign(this.startVel));
		return charmFloatWingsFXAlt;
	}

	// Token: 0x0600376E RID: 14190 RVA: 0x001034D8 File Offset: 0x001016D8
	public void FixedUpdate()
	{
		if (CupheadTime.FixedDelta > 0f)
		{
			base.transform.position += this.vel;
			this.vel -= this.slowFactor * this.startVel * Vector3.right;
			this.vel.y = this.vel.y + this.riseFactor;
			if (Mathf.Sign(this.vel.x) != Mathf.Sign(this.startVel))
			{
				this.vel.x = this.vel.x * 0.95f;
			}
			base.transform.rotation = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(this.vel) + -90f * Mathf.Sign(this.startVel));
		}
	}

	// Token: 0x04002C90 RID: 11408
	[SerializeField]
	public Animator anim;

	// Token: 0x04002C91 RID: 11409
	[SerializeField]
	public Vector3 vel;

	// Token: 0x04002C92 RID: 11410
	[SerializeField]
	public float startVel;

	// Token: 0x04002C93 RID: 11411
	[SerializeField]
	public float startSpeed = 30f;

	// Token: 0x04002C94 RID: 11412
	[SerializeField]
	public float slowFactor = 0.95f;

	// Token: 0x04002C95 RID: 11413
	[SerializeField]
	public float riseFactor = 0.02f;
}
