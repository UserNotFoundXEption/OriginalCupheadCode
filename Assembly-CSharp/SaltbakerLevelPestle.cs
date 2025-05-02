using System;
using UnityEngine;

// Token: 0x0200037A RID: 890
public class SaltbakerLevelPestle : AbstractProjectile
{
	// Token: 0x0600273B RID: 10043 RVA: 0x00020F9F File Offset: 0x0001F19F
	public SaltbakerLevelPestle Init(Vector3 spawnPos, float velocityX, float velocityY, float gravity)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = spawnPos;
		this.speed = new Vector3(velocityX, velocityY);
		this.gravity = gravity;
		return this;
	}

	// Token: 0x0600273C RID: 10044 RVA: 0x00020FCF File Offset: 0x0001F1CF
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600273D RID: 10045 RVA: 0x00020FED File Offset: 0x0001F1ED
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		this.Move();
	}

	// Token: 0x0600273E RID: 10046 RVA: 0x000CB1A8 File Offset: 0x000C93A8
	public void Move()
	{
		this.speed += new Vector3(0f, this.gravity * CupheadTime.FixedDelta);
		base.transform.Translate(this.speed * CupheadTime.FixedDelta);
	}

	// Token: 0x04002077 RID: 8311
	public Vector3 speed;

	// Token: 0x04002078 RID: 8312
	public float gravity;
}
