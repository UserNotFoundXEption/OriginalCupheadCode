using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000590 RID: 1424
public class BouncingProjectile : AbstractProjectile
{
	// Token: 0x06003C19 RID: 15385 RVA: 0x00030AA5 File Offset: 0x0002ECA5
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06003C1A RID: 15386 RVA: 0x00114BB0 File Offset: 0x00112DB0
	public IEnumerator move_cr()
	{
		for (;;)
		{
			if (this.isMoving)
			{
				base.transform.position += this.velocity * this.speed * CupheadTime.FixedDelta;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06003C1B RID: 15387 RVA: 0x00114BCC File Offset: 0x00112DCC
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		Vector3 newVelocity = this.velocity;
		newVelocity.y = Mathf.Min(newVelocity.y, -newVelocity.y);
		this.ChangeDir(newVelocity);
	}

	// Token: 0x06003C1C RID: 15388 RVA: 0x00114C04 File Offset: 0x00112E04
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		Vector3 newVelocity = this.velocity;
		newVelocity.y = Mathf.Max(newVelocity.y, -newVelocity.y);
		this.ChangeDir(newVelocity);
	}

	// Token: 0x06003C1D RID: 15389 RVA: 0x00114C3C File Offset: 0x00112E3C
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
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

	// Token: 0x06003C1E RID: 15390 RVA: 0x00114CB8 File Offset: 0x00112EB8
	public virtual void ChangeDir(Vector3 newVelocity)
	{
		this.velocity = newVelocity;
		this.currentAngle = Mathf.Atan2(this.velocity.y, this.velocity.x) * 57.29578f;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.currentAngle));
	}

	// Token: 0x04002FB0 RID: 12208
	public bool isMoving;

	// Token: 0x04002FB1 RID: 12209
	public float speed;

	// Token: 0x04002FB2 RID: 12210
	public float currentAngle;

	// Token: 0x04002FB3 RID: 12211
	public Vector3 velocity;
}
