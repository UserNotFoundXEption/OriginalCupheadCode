using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000319 RID: 793
public class RetroArcadeTentacle : AbstractProjectile
{
	// Token: 0x060022E2 RID: 8930 RVA: 0x000BF0D8 File Offset: 0x000BD2D8
	public virtual AbstractProjectile Init(Vector3 pos, float targetPosY, bool onLeft, float verticalSpeed, float horizontalSpeed)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.target.transform.SetLocalPosition(new float?(this.targetRoot.localPosition.x + ((!onLeft) ? -15f : 15f)), new float?(this.targetRoot.localPosition.y + targetPosY), null);
		this.verticalSpeed = verticalSpeed;
		this.horizontalSpeed = horizontalSpeed;
		this.onLeft = onLeft;
		base.transform.position = pos;
		this.startPos = pos;
		base.StartCoroutine(this.move_cr());
		return this;
	}

	// Token: 0x060022E3 RID: 8931 RVA: 0x0001D9B7 File Offset: 0x0001BBB7
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x0001D9D5 File Offset: 0x0001BBD5
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060022E5 RID: 8933 RVA: 0x000BF18C File Offset: 0x000BD38C
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 direction = (!this.onLeft) ? Vector3.left : Vector3.right;
		this.canMove = true;
		while (base.transform.position.y < 0f)
		{
			base.transform.position += Vector3.up * this.verticalSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		while (this.canMove && !this.target.IsDead)
		{
			base.transform.position += direction * this.horizontalSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		while (base.transform.position.y > this.startPos.y)
		{
			base.transform.position += Vector3.down * this.verticalSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		this.Recycle<RetroArcadeTentacle>();
		yield break;
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x0001D9F3 File Offset: 0x0001BBF3
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<RetroArcadeTentacle>())
		{
			this.canMove = false;
		}
	}

	// Token: 0x04001CE0 RID: 7392
	[SerializeField]
	public RetroArcadeTentacleTarget target;

	// Token: 0x04001CE1 RID: 7393
	[SerializeField]
	public Transform targetRoot;

	// Token: 0x04001CE2 RID: 7394
	public const float OFFSET = 15f;

	// Token: 0x04001CE3 RID: 7395
	public float verticalSpeed;

	// Token: 0x04001CE4 RID: 7396
	public float horizontalSpeed;

	// Token: 0x04001CE5 RID: 7397
	public bool onLeft;

	// Token: 0x04001CE6 RID: 7398
	public bool canMove;

	// Token: 0x04001CE7 RID: 7399
	public Vector3 startPos;
}
