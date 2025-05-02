using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000139 RID: 313
public class AirshipClamLevelBarnacle : AbstractProjectile
{
	// Token: 0x06000ECD RID: 3789 RVA: 0x0000C89B File Offset: 0x0000AA9B
	public override void Update()
	{
		this.damageDealer.Update();
		base.Update();
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x0000C8AE File Offset: 0x0000AAAE
	public void InitBarnacle(int dir, LevelProperties.AirshipClam properties)
	{
		this.properties = properties;
		this.direction = dir;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x0008C448 File Offset: 0x0008A648
	public IEnumerator move_cr()
	{
		this.velocity = new Vector3(this.properties.CurrentState.barnacles.initialArcMovementX * (float)this.direction, this.properties.CurrentState.barnacles.initialArcMovementY, 0f);
		for (;;)
		{
			base.transform.position += this.velocity * CupheadTime.Delta;
			this.velocity.y = this.velocity.y + this.properties.CurrentState.barnacles.parryGravity;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x0000C8CB File Offset: 0x0000AACB
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		if (phase == CollisionPhase.Enter)
		{
			this.velocity.x = 0f;
		}
		base.OnCollisionWalls(hit, phase);
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x0008C464 File Offset: 0x0008A664
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		this.velocity.y = 0f;
		this.velocity.x = this.properties.CurrentState.barnacles.rollingSpeed * (float)(-(float)this.direction);
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x0000C8EB File Offset: 0x0000AAEB
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x0000C90D File Offset: 0x0000AB0D
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x04000C1E RID: 3102
	public int direction;

	// Token: 0x04000C1F RID: 3103
	public Vector3 velocity;

	// Token: 0x04000C20 RID: 3104
	public LevelProperties.AirshipClam properties;
}
