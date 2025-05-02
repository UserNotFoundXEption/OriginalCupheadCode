using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000594 RID: 1428
public class HomingProjectile : AbstractProjectile
{
	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x06003C4C RID: 15436 RVA: 0x00030C5C File Offset: 0x0002EE5C
	// (set) Token: 0x06003C4D RID: 15437 RVA: 0x00030C64 File Offset: 0x0002EE64
	public bool HomingEnabled { get; set; }

	// Token: 0x06003C4E RID: 15438 RVA: 0x001153A8 File Offset: 0x001135A8
	public HomingProjectile Create(Vector2 pos, float launchRotation, float launchSpeed, float homingMoveSpeed, float rotationSpeed, float timeBeforeDeath, float homingEaseTime, AbstractPlayerController player)
	{
		return this.Create(pos, launchRotation, launchSpeed, homingMoveSpeed, rotationSpeed, timeBeforeDeath, 0f, homingEaseTime, player);
	}

	// Token: 0x06003C4F RID: 15439 RVA: 0x001153D0 File Offset: 0x001135D0
	public HomingProjectile Create(Vector2 pos, float launchRotation, float launchSpeed, float homingMoveSpeed, float rotationSpeed, float timeBeforeDeath, float timeBeforeHoming, float homingEaseTime, AbstractPlayerController player)
	{
		HomingProjectile homingProjectile = base.Create() as HomingProjectile;
		homingProjectile.homingDirection = MathUtils.AngleToDirection(launchRotation);
		homingProjectile.launchVelocity = MathUtils.AngleToDirection(launchRotation) * launchSpeed;
		homingProjectile.transform.position = pos;
		homingProjectile.player = player;
		homingProjectile.rotationSpeed = rotationSpeed;
		homingProjectile.homingMoveSpeed = homingMoveSpeed;
		homingProjectile.timeBeforeDeath = timeBeforeDeath;
		homingProjectile.timeBeforeHoming = timeBeforeHoming;
		homingProjectile.easeTime = homingEaseTime;
		homingProjectile.HomingEnabled = true;
		return homingProjectile;
	}

	// Token: 0x06003C50 RID: 15440 RVA: 0x00030C6D File Offset: 0x0002EE6D
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06003C51 RID: 15441 RVA: 0x00030C82 File Offset: 0x0002EE82
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06003C52 RID: 15442 RVA: 0x00115450 File Offset: 0x00113650
	public IEnumerator move_cr()
	{
		float t = 0f;
		while (t < this.timeBeforeDeath + this.easeTime + this.timeBeforeHoming)
		{
			while (!this.HomingEnabled)
			{
				yield return null;
			}
			t += CupheadTime.FixedDelta;
			if (this.player != null && !this.player.IsDead)
			{
				Vector3 center = this.player.center;
				if (this.trackGround)
				{
					center.y = (float)Level.Current.Ground;
				}
				Vector2 direction = (center - base.transform.position).normalized;
				Quaternion quaternion = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(direction));
				Quaternion quaternion2 = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(this.homingDirection));
				this.homingDirection = MathUtils.AngleToDirection(Quaternion.Slerp(quaternion2, quaternion, Mathf.Min(1f, CupheadTime.FixedDelta * this.rotationSpeed)).eulerAngles.z);
			}
			Vector2 homingVelocity = this.homingDirection * this.homingMoveSpeed;
			this.velocity = homingVelocity;
			if (t < this.timeBeforeHoming)
			{
				this.velocity = this.launchVelocity;
			}
			else if (t < this.timeBeforeHoming + this.easeTime)
			{
				float num = EaseUtils.EaseOutSine(0f, 1f, (t - this.timeBeforeHoming) / this.easeTime);
				this.velocity = Vector2.Lerp(this.launchVelocity, homingVelocity, num);
			}
			if (this.faceMoveDirection)
			{
				base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(this.velocity) + this.spriteRotation));
			}
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		this.Die();
		yield break;
	}

	// Token: 0x04002FCF RID: 12239
	public AbstractPlayerController player;

	// Token: 0x04002FD0 RID: 12240
	public Vector2 launchVelocity;

	// Token: 0x04002FD1 RID: 12241
	public float homingMoveSpeed;

	// Token: 0x04002FD2 RID: 12242
	public float rotationSpeed;

	// Token: 0x04002FD3 RID: 12243
	public float timeBeforeDeath;

	// Token: 0x04002FD4 RID: 12244
	public float timeBeforeHoming;

	// Token: 0x04002FD5 RID: 12245
	public float easeTime;

	// Token: 0x04002FD6 RID: 12246
	public Vector2 homingDirection;

	// Token: 0x04002FD8 RID: 12248
	[SerializeField]
	public bool trackGround;

	// Token: 0x04002FD9 RID: 12249
	[SerializeField]
	public bool faceMoveDirection;

	// Token: 0x04002FDA RID: 12250
	[SerializeField]
	public float spriteRotation;

	// Token: 0x04002FDB RID: 12251
	public Vector2 velocity;
}
