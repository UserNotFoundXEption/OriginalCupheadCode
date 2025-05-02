using System;
using UnityEngine;

// Token: 0x020002CE RID: 718
public class MouseLevelGhostMouseBall : AbstractProjectile
{
	// Token: 0x06001FF7 RID: 8183 RVA: 0x000B6E7C File Offset: 0x000B507C
	public MouseLevelGhostMouseBall Create(Vector2 pos, float speed, float childSpeed)
	{
		MouseLevelGhostMouseBall mouseLevelGhostMouseBall = this.InstantiatePrefab<MouseLevelGhostMouseBall>();
		Vector2 vector;
		vector..ctor(PlayerManager.GetNext().transform.position.x, (float)Level.Current.Ground);
		Vector2 normalized = (vector - pos).normalized;
		mouseLevelGhostMouseBall.transform.position = pos;
		mouseLevelGhostMouseBall.velocity = speed * normalized;
		mouseLevelGhostMouseBall.childSpeed = childSpeed;
		mouseLevelGhostMouseBall.state = MouseLevelGhostMouseBall.State.Moving;
		mouseLevelGhostMouseBall.transform.Rotate(0f, 0f, MathUtils.DirectionToAngle(normalized) - 90f);
		return mouseLevelGhostMouseBall;
	}

	// Token: 0x06001FF8 RID: 8184 RVA: 0x000B6F18 File Offset: 0x000B5118
	public override void Update()
	{
		base.Update();
		if (this.state == MouseLevelGhostMouseBall.State.Moving)
		{
			if (base.transform.position.y < (float)Level.Current.Ground)
			{
				this.Explode();
				return;
			}
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
		}
	}

	// Token: 0x06001FF9 RID: 8185 RVA: 0x0001B0DF File Offset: 0x000192DF
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001FFA RID: 8186 RVA: 0x000B6FA0 File Offset: 0x000B51A0
	public void Explode()
	{
		this.state = MouseLevelGhostMouseBall.State.Dead;
		this.childProjectile.Create(base.transform.position, 0f, Vector2.one, this.childSpeed);
		this.childProjectile.Create(base.transform.position, 0f, new Vector2(1f, -1f), -this.childSpeed);
		this.Die();
	}

	// Token: 0x06001FFB RID: 8187 RVA: 0x0001B108 File Offset: 0x00019308
	public override void Die()
	{
		base.Die();
		base.transform.SetLocalEulerAngles(new float?(0f), new float?(0f), new float?(0f));
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x0001B139 File Offset: 0x00019339
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.childProjectile = null;
	}

	// Token: 0x040019FF RID: 6655
	[SerializeField]
	public BasicProjectile childProjectile;

	// Token: 0x04001A00 RID: 6656
	public MouseLevelGhostMouseBall.State state;

	// Token: 0x04001A01 RID: 6657
	public Vector2 velocity;

	// Token: 0x04001A02 RID: 6658
	public float childSpeed;

	// Token: 0x02000DCB RID: 3531
	public enum State
	{
		// Token: 0x040063B6 RID: 25526
		Init,
		// Token: 0x040063B7 RID: 25527
		Moving,
		// Token: 0x040063B8 RID: 25528
		Dead
	}
}
