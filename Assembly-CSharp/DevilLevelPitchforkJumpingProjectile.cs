using System;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class DevilLevelPitchforkJumpingProjectile : AbstractProjectile
{
	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06001522 RID: 5410 RVA: 0x00011F80 File Offset: 0x00010180
	public override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06001523 RID: 5411 RVA: 0x0009B694 File Offset: 0x00099894
	public DevilLevelPitchforkJumpingProjectile Create(Vector2 pos, MinMax launchAngle, MinMax launchSpeed, float gravity, int numJumps, DevilLevelSittingDevil parent)
	{
		DevilLevelPitchforkJumpingProjectile devilLevelPitchforkJumpingProjectile = this.InstantiatePrefab<DevilLevelPitchforkJumpingProjectile>();
		devilLevelPitchforkJumpingProjectile.transform.position = pos;
		devilLevelPitchforkJumpingProjectile.launchSpeed = launchSpeed;
		devilLevelPitchforkJumpingProjectile.launchAngle = launchAngle;
		devilLevelPitchforkJumpingProjectile.gravity = gravity;
		devilLevelPitchforkJumpingProjectile.parent = parent;
		devilLevelPitchforkJumpingProjectile.jumpsRemaining = numJumps;
		return devilLevelPitchforkJumpingProjectile;
	}

	// Token: 0x06001524 RID: 5412 RVA: 0x00011F87 File Offset: 0x00010187
	public override void Update()
	{
		base.Update();
		if (this.parent == null)
		{
			this.Die();
		}
	}

	// Token: 0x06001525 RID: 5413 RVA: 0x00011FA6 File Offset: 0x000101A6
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001526 RID: 5414 RVA: 0x0009B6E0 File Offset: 0x000998E0
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!base.dead && this.state == DevilLevelPitchforkJumpingProjectile.State.Jumping)
		{
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			float radius = base.GetComponent<CircleCollider2D>().radius;
			if (base.transform.position.y < (float)Level.Current.Ground + radius)
			{
				base.transform.SetPosition(null, new float?((float)Level.Current.Ground + radius), null);
				if (this.jumpsRemaining > 0)
				{
					this.state = DevilLevelPitchforkJumpingProjectile.State.OnGround;
				}
				else
				{
					this.Die();
				}
			}
		}
	}

	// Token: 0x06001527 RID: 5415 RVA: 0x0009B7D8 File Offset: 0x000999D8
	public void Jump()
	{
		float num = float.MaxValue;
		Vector2 vector = Vector2.zero;
		Vector3 center = PlayerManager.GetNext().center;
		Vector2 vector2 = center - base.transform.position;
		vector2.x = Mathf.Abs(vector2.x);
		float radius = base.GetComponent<CircleCollider2D>().radius;
		AudioManager.Play("devil_projectile_move");
		this.emitAudioFromObject.Add("devil_projectile_move");
		float num2;
		if (center.x < base.transform.position.x)
		{
			num2 = base.transform.position.x - ((float)Level.Current.Left + radius);
		}
		else
		{
			num2 = (float)Level.Current.Right - radius - base.transform.position.x;
		}
		for (float num3 = 0f; num3 < 1f; num3 += 0.01f)
		{
			float floatAt = this.launchAngle.GetFloatAt(num3);
			float floatAt2 = this.launchSpeed.GetFloatAt(num3);
			Vector2 vector3 = MathUtils.AngleToDirection(floatAt) * floatAt2;
			float num4 = vector2.x / vector3.x;
			float num5 = vector3.y * num4 - 0.5f * this.gravity * num4 * num4;
			float num6 = Mathf.Abs(vector2.y - num5);
			float num7 = vector3.y - this.gravity * num4;
			if (num7 <= 0f)
			{
				float num8 = num2 / vector3.x;
				float num9 = vector3.y * num8 - 0.5f * this.gravity * num8 * num8;
				if (num9 <= (float)Level.Current.Ground + radius)
				{
					if (num6 < num)
					{
						num = num6;
						vector = vector3;
					}
				}
			}
		}
		if (center.x < base.transform.position.x)
		{
			vector.x *= -1f;
		}
		this.velocity = vector;
		this.state = DevilLevelPitchforkJumpingProjectile.State.Jumping;
		this.jumpsRemaining--;
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x00011FC4 File Offset: 0x000101C4
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001151 RID: 4433
	public DevilLevelPitchforkJumpingProjectile.State state;

	// Token: 0x04001152 RID: 4434
	public Vector2 velocity;

	// Token: 0x04001153 RID: 4435
	public MinMax launchSpeed;

	// Token: 0x04001154 RID: 4436
	public MinMax launchAngle;

	// Token: 0x04001155 RID: 4437
	public float gravity;

	// Token: 0x04001156 RID: 4438
	public int jumpsRemaining;

	// Token: 0x04001157 RID: 4439
	public DevilLevelSittingDevil parent;

	// Token: 0x02000B58 RID: 2904
	public enum State
	{
		// Token: 0x0400531A RID: 21274
		Idle,
		// Token: 0x0400531B RID: 21275
		Jumping,
		// Token: 0x0400531C RID: 21276
		OnGround
	}
}
