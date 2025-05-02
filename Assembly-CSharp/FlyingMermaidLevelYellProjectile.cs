using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000296 RID: 662
public class FlyingMermaidLevelYellProjectile : AbstractProjectile
{
	// Token: 0x06001DEF RID: 7663 RVA: 0x000B2200 File Offset: 0x000B0400
	public FlyingMermaidLevelYellProjectile Create(Vector2 pos, float trackSpeed, float angle, AbstractPlayerController target)
	{
		FlyingMermaidLevelYellProjectile flyingMermaidLevelYellProjectile = base.Create() as FlyingMermaidLevelYellProjectile;
		flyingMermaidLevelYellProjectile.trackSpeed = trackSpeed;
		flyingMermaidLevelYellProjectile.target = target;
		flyingMermaidLevelYellProjectile.direction = MathUtils.AngleToDirection(angle);
		flyingMermaidLevelYellProjectile.transform.position = pos;
		return flyingMermaidLevelYellProjectile;
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x000194A3 File Offset: 0x000176A3
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x000194B8 File Offset: 0x000176B8
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x000B2248 File Offset: 0x000B0448
	public IEnumerator move_cr()
	{
		float speed = this.launchSpeed;
		float t = 0f;
		for (;;)
		{
			t += CupheadTime.FixedDelta;
			FlyingMermaidLevelYellProjectile.State state = this.state;
			if (state != FlyingMermaidLevelYellProjectile.State.Slowing)
			{
				if (state != FlyingMermaidLevelYellProjectile.State.Stopped)
				{
					if (state == FlyingMermaidLevelYellProjectile.State.Tracking)
					{
						if (t < this.attackEaseTime)
						{
							speed = EaseUtils.EaseInSine(0f, this.trackSpeed, t / this.attackEaseTime);
						}
						else
						{
							speed = this.trackSpeed;
						}
					}
				}
				else if (t >= this.waitTime)
				{
					this.state = FlyingMermaidLevelYellProjectile.State.Tracking;
					t = 0f;
					if (this.target == null || this.target.IsDead)
					{
						this.target = PlayerManager.GetNext();
					}
					if (this.target != null)
					{
						this.direction = (this.target.center - base.transform.position).normalized;
						base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(this.direction) + 180f));
						base.animator.SetTrigger("Continue");
					}
				}
			}
			else if (t < this.stopTime)
			{
				speed = EaseUtils.EaseOutSine(this.launchSpeed, 0f, t / this.stopTime);
			}
			else
			{
				speed = 0f;
				this.state = FlyingMermaidLevelYellProjectile.State.Stopped;
				t = 0f;
			}
			Vector2 pos = base.transform.localPosition;
			pos += speed * CupheadTime.FixedDelta * this.direction;
			base.transform.localPosition = pos;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x0400188E RID: 6286
	public float launchSpeed;

	// Token: 0x0400188F RID: 6287
	public float stopTime;

	// Token: 0x04001890 RID: 6288
	public float waitTime;

	// Token: 0x04001891 RID: 6289
	public float attackEaseTime;

	// Token: 0x04001892 RID: 6290
	public FlyingMermaidLevelYellProjectile.State state;

	// Token: 0x04001893 RID: 6291
	public float trackSpeed;

	// Token: 0x04001894 RID: 6292
	public AbstractPlayerController target;

	// Token: 0x04001895 RID: 6293
	public Vector2 direction;

	// Token: 0x02000D5C RID: 3420
	public enum State
	{
		// Token: 0x040060D0 RID: 24784
		Slowing,
		// Token: 0x040060D1 RID: 24785
		Stopped,
		// Token: 0x040060D2 RID: 24786
		Tracking
	}
}
