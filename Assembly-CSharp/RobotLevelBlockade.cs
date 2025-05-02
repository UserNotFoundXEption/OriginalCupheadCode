using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000332 RID: 818
public class RobotLevelBlockade : AbstractCollidableObject
{
	// Token: 0x060023C2 RID: 9154 RVA: 0x000C1BB0 File Offset: 0x000BFDB0
	public RobotLevelBlockade Create(Vector3 origin, int dir)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject);
		gameObject.transform.position = origin + Vector3.up * (float)dir * 300f;
		this.rootSegment = gameObject.GetComponent<RobotLevelBlockade>();
		return this.rootSegment;
	}

	// Token: 0x060023C3 RID: 9155 RVA: 0x0001E297 File Offset: 0x0001C497
	public void InitBlockade(int dir, int xSpeed, int ySpeed)
	{
		this.xSpeed = xSpeed;
		this.ySpeed = ySpeed * -dir;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060023C4 RID: 9156 RVA: 0x0001E2B7 File Offset: 0x0001C4B7
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		base.Awake();
	}

	// Token: 0x060023C5 RID: 9157 RVA: 0x0001E2CA File Offset: 0x0001C4CA
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060023C6 RID: 9158 RVA: 0x0001E2E2 File Offset: 0x0001C4E2
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060023C7 RID: 9159 RVA: 0x000C1C04 File Offset: 0x000BFE04
	public IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.position += (Vector3.left * (float)this.xSpeed + Vector3.up * (float)this.ySpeed) * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001D9E RID: 7582
	public const float heightOffset = 300f;

	// Token: 0x04001D9F RID: 7583
	public DamageDealer damageDealer;

	// Token: 0x04001DA0 RID: 7584
	public RobotLevelBlockade rootSegment;

	// Token: 0x04001DA1 RID: 7585
	public int xSpeed;

	// Token: 0x04001DA2 RID: 7586
	public int ySpeed;
}
