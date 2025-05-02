using System;
using UnityEngine;

// Token: 0x02000313 RID: 787
public class RetroArcadeRobotBouncingProjectile : AbstractProjectile
{
	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x060022B2 RID: 8882 RVA: 0x0001D83C File Offset: 0x0001BA3C
	public override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x060022B3 RID: 8883 RVA: 0x0001D843 File Offset: 0x0001BA43
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060022B4 RID: 8884 RVA: 0x000BE510 File Offset: 0x000BC710
	public RetroArcadeRobotBouncingProjectile Create(Vector2 pos, float speed, float angle, bool bounce)
	{
		RetroArcadeRobotBouncingProjectile retroArcadeRobotBouncingProjectile = this.InstantiatePrefab<RetroArcadeRobotBouncingProjectile>();
		retroArcadeRobotBouncingProjectile.transform.position = pos;
		retroArcadeRobotBouncingProjectile.velocity = speed * MathUtils.AngleToDirection(angle);
		retroArcadeRobotBouncingProjectile.bounce = bounce;
		return retroArcadeRobotBouncingProjectile;
	}

	// Token: 0x060022B5 RID: 8885 RVA: 0x0001D846 File Offset: 0x0001BA46
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		this.damageDealer.DealDamage(hit);
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060022B6 RID: 8886 RVA: 0x000BE550 File Offset: 0x000BC750
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		float radius = base.GetComponent<CircleCollider2D>().radius;
		if (this.bounce)
		{
			if ((this.velocity.x < 0f && base.transform.position.x < (float)Level.Current.Left + radius) || (this.velocity.x > 0f && base.transform.position.x > (float)Level.Current.Right - radius))
			{
				this.velocity.x = this.velocity.x * -1f;
			}
			if (this.velocity.y < 0f && base.transform.position.y < (float)Level.Current.Ground + radius)
			{
				this.velocity.y = this.velocity.y * -1f;
			}
		}
	}

	// Token: 0x04001CAD RID: 7341
	public bool bounce;

	// Token: 0x04001CAE RID: 7342
	public float attackDelay;

	// Token: 0x04001CAF RID: 7343
	public Vector2 velocity;

	// Token: 0x04001CB0 RID: 7344
	public DevilLevelSittingDevil parent;
}
