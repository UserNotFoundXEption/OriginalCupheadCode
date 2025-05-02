using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class ChessRookLevelHazard : AbstractProjectile
{
	// Token: 0x06001334 RID: 4916 RVA: 0x000102AE File Offset: 0x0000E4AE
	public ChessRookLevelHazard Create(Vector3 position, float speed)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		this.speed = speed;
		this.Move();
		return this;
	}

	// Token: 0x06001335 RID: 4917 RVA: 0x000102D6 File Offset: 0x0000E4D6
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x000102F4 File Offset: 0x0000E4F4
	public void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x000972E0 File Offset: 0x000954E0
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.x > -740f)
		{
			base.transform.position += Vector3.left * this.speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		this.Recycle<ChessRookLevelHazard>();
		yield break;
	}

	// Token: 0x04000F8A RID: 3978
	public float speed;
}
