using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000300 RID: 768
public class RetroArcadeBouncyBall : RetroArcadeEnemy
{
	// Token: 0x06002220 RID: 8736 RVA: 0x000BC4F4 File Offset: 0x000BA6F4
	public RetroArcadeBouncyBall Create(Vector3 pos, RetroArcadeBouncyManager manager, LevelProperties.RetroArcade.Bouncy properties, float startAngle)
	{
		RetroArcadeBouncyBall retroArcadeBouncyBall = this.InstantiatePrefab<RetroArcadeBouncyBall>();
		retroArcadeBouncyBall.transform.position = pos;
		retroArcadeBouncyBall.startAngle = startAngle;
		retroArcadeBouncyBall.properties = properties;
		retroArcadeBouncyBall.GetComponent<Collider2D>().enabled = false;
		return retroArcadeBouncyBall;
	}

	// Token: 0x06002221 RID: 8737 RVA: 0x0001D27A File Offset: 0x0001B47A
	public void StartMoving(Vector3 middlePos)
	{
		this.hp = 1f;
		base.transform.parent = null;
		base.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002222 RID: 8738 RVA: 0x000BC530 File Offset: 0x000BA730
	public IEnumerator move_cr()
	{
		this.velocity = MathUtils.AngleToDirection(this.startAngle);
		for (;;)
		{
			base.transform.position += this.velocity * this.properties.groupMoveSpeed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002223 RID: 8739 RVA: 0x000BC54C File Offset: 0x000BA74C
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		Vector3 newVelocity = this.velocity;
		newVelocity.y = Mathf.Min(newVelocity.y, -newVelocity.y);
		this.ChangeDir(newVelocity);
	}

	// Token: 0x06002224 RID: 8740 RVA: 0x000BC58C File Offset: 0x000BA78C
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		Vector3 newVelocity = this.velocity;
		newVelocity.y = Mathf.Max(newVelocity.y, -newVelocity.y);
		this.ChangeDir(newVelocity);
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x000BC5CC File Offset: 0x000BA7CC
	public void ChangeDir(Vector3 newVelocity)
	{
		this.velocity = newVelocity;
		this.currentAngle = Mathf.Atan2(this.velocity.y, this.velocity.x) * 57.29578f;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.currentAngle));
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x000BC634 File Offset: 0x000BA834
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
		Vector3 newVelocity = this.velocity;
		if (base.transform.position.x > 0f)
		{
			newVelocity.x = Mathf.Min(newVelocity.x, -newVelocity.x);
			this.ChangeDir(newVelocity);
		}
		else
		{
			newVelocity.x = Mathf.Max(newVelocity.x, -newVelocity.x);
			this.ChangeDir(newVelocity);
		}
	}

	// Token: 0x06002227 RID: 8743 RVA: 0x0001D2AC File Offset: 0x0001B4AC
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (!base.IsDead)
		{
			base.OnCollisionPlayer(hit, phase);
		}
	}

	// Token: 0x06002228 RID: 8744 RVA: 0x0001D2C1 File Offset: 0x0001B4C1
	public override void Dead()
	{
		base.Dead();
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<DamageReceiver>().enabled = false;
		Object.Destroy(base.GetComponent<Rigidbody2D>());
	}

	// Token: 0x04001C1E RID: 7198
	public LevelProperties.RetroArcade.Bouncy properties;

	// Token: 0x04001C1F RID: 7199
	public RetroArcadeBouncyManager manager;

	// Token: 0x04001C20 RID: 7200
	public Vector3 velocity;

	// Token: 0x04001C21 RID: 7201
	public float currentAngle;

	// Token: 0x04001C22 RID: 7202
	public float startAngle;
}
