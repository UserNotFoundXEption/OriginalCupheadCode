using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000306 RID: 774
public class RetroArcadeChaser : RetroArcadeEnemy
{
	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06002252 RID: 8786 RVA: 0x0001D3E5 File Offset: 0x0001B5E5
	// (set) Token: 0x06002253 RID: 8787 RVA: 0x0001D3ED File Offset: 0x0001B5ED
	public bool IsDone { get; set; }

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06002254 RID: 8788 RVA: 0x0001D3F6 File Offset: 0x0001B5F6
	// (set) Token: 0x06002255 RID: 8789 RVA: 0x0001D3FE File Offset: 0x0001B5FE
	public bool HomingEnabled { get; set; }

	// Token: 0x06002256 RID: 8790 RVA: 0x000BD350 File Offset: 0x000BB550
	public virtual RetroArcadeChaser Init(Vector3 pos, float launchRotation, float launchSpeed, float homingMoveSpeed, float rotationSpeed, float timeBeforeDeath, float hp, AbstractPlayerController player, LevelProperties.RetroArcade.Chasers properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.homingDirection = MathUtils.AngleToDirection(launchRotation);
		this.launchVelocity = MathUtils.AngleToDirection(launchRotation) * launchSpeed;
		base.transform.position = pos;
		this.player = player;
		this.rotationSpeed = rotationSpeed;
		this.homingMoveSpeed = homingMoveSpeed;
		this.timeBeforeDeath = timeBeforeDeath;
		this.HomingEnabled = true;
		this.hp = hp;
		this.StartChaser();
		return this;
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x0001D407 File Offset: 0x0001B607
	public void StartChaser()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002258 RID: 8792 RVA: 0x000BD3D8 File Offset: 0x000BB5D8
	public IEnumerator move_cr()
	{
		float t = 0f;
		while (t < this.timeBeforeDeath + this.timeBeforeHoming)
		{
			while (!this.HomingEnabled)
			{
				yield return null;
			}
			t += CupheadTime.FixedDelta;
			if (this.player != null && !this.player.IsDead)
			{
				Vector3 center = this.player.center;
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
			else if (t < this.timeBeforeHoming)
			{
				float num = EaseUtils.EaseOutSine(0f, 1f, t - this.timeBeforeHoming);
				this.velocity = Vector2.Lerp(this.launchVelocity, homingVelocity, num);
			}
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		float offset = 100f;
		while (base.transform.position.x > -640f - offset && base.transform.position.x < 640f + offset && base.transform.position.x > -360f - offset && base.transform.position.x < 360f + offset)
		{
			base.transform.position += this.velocity.normalized * this.homingMoveSpeed * CupheadTime.FixedDelta;
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(this.velocity) + 180f));
			yield return new WaitForFixedUpdate();
		}
		this.IsDone = true;
		this.Recycle<RetroArcadeChaser>();
		yield break;
	}

	// Token: 0x04001C50 RID: 7248
	public AbstractPlayerController player;

	// Token: 0x04001C51 RID: 7249
	public Vector2 launchVelocity;

	// Token: 0x04001C52 RID: 7250
	public float homingMoveSpeed;

	// Token: 0x04001C53 RID: 7251
	public float rotationSpeed;

	// Token: 0x04001C54 RID: 7252
	public float timeBeforeDeath;

	// Token: 0x04001C55 RID: 7253
	public float timeBeforeHoming;

	// Token: 0x04001C56 RID: 7254
	public Vector2 homingDirection;

	// Token: 0x04001C58 RID: 7256
	public Vector2 velocity;
}
