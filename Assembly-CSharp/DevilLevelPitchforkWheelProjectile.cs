using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class DevilLevelPitchforkWheelProjectile : AbstractProjectile
{
	// Token: 0x1700026E RID: 622
	// (get) Token: 0x0600154F RID: 5455 RVA: 0x000121D8 File Offset: 0x000103D8
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06001550 RID: 5456 RVA: 0x000121DB File Offset: 0x000103DB
	public override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x0009BEF4 File Offset: 0x0009A0F4
	public DevilLevelPitchforkWheelProjectile Create(Vector2 pos, float attackDelay, float speed, DevilLevelSittingDevil parent)
	{
		DevilLevelPitchforkWheelProjectile devilLevelPitchforkWheelProjectile = this.InstantiatePrefab<DevilLevelPitchforkWheelProjectile>();
		devilLevelPitchforkWheelProjectile.transform.position = pos;
		devilLevelPitchforkWheelProjectile.attackDelay = attackDelay;
		devilLevelPitchforkWheelProjectile.speed = speed;
		devilLevelPitchforkWheelProjectile.state = DevilLevelPitchforkWheelProjectile.State.Idle;
		devilLevelPitchforkWheelProjectile.StartCoroutine(devilLevelPitchforkWheelProjectile.main_cr());
		devilLevelPitchforkWheelProjectile.parent = parent;
		return devilLevelPitchforkWheelProjectile;
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x000121E2 File Offset: 0x000103E2
	public override void Update()
	{
		base.Update();
		if (this.parent == null)
		{
			this.Die();
		}
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x00012201 File Offset: 0x00010401
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x0009BF44 File Offset: 0x0009A144
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!base.dead && this.state != DevilLevelPitchforkWheelProjectile.State.Idle)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		}
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x0009BFA0 File Offset: 0x0009A1A0
	public IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.attackDelay);
		this.state = DevilLevelPitchforkWheelProjectile.State.Attacking;
		this.velocity = this.speed * (PlayerManager.GetNext().center - base.transform.position).normalized;
		while (this.state == DevilLevelPitchforkWheelProjectile.State.Attacking)
		{
			float colliderRadius = base.GetComponent<CircleCollider2D>().radius;
			if (base.transform.position.x < (float)Level.Current.Left + colliderRadius || base.transform.position.x > (float)Level.Current.Right - colliderRadius || base.transform.position.y < (float)Level.Current.Ground + colliderRadius || base.transform.position.y > (float)Level.Current.Ceiling - colliderRadius)
			{
				this.velocity *= -1f;
				this.state = DevilLevelPitchforkWheelProjectile.State.Returning;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x0001221F File Offset: 0x0001041F
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001173 RID: 4467
	public DevilLevelPitchforkWheelProjectile.State state;

	// Token: 0x04001174 RID: 4468
	public float attackDelay;

	// Token: 0x04001175 RID: 4469
	public Vector2 velocity;

	// Token: 0x04001176 RID: 4470
	public float speed;

	// Token: 0x04001177 RID: 4471
	public DevilLevelSittingDevil parent;

	// Token: 0x02000B5E RID: 2910
	public enum State
	{
		// Token: 0x04005333 RID: 21299
		Idle,
		// Token: 0x04005334 RID: 21300
		Attacking,
		// Token: 0x04005335 RID: 21301
		Returning
	}
}
