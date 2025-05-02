using System;
using UnityEngine;

// Token: 0x02000295 RID: 661
public class FlyingMermaidLevelTurtleSpiralProjectile : BasicProjectile
{
	// Token: 0x06001DEA RID: 7658 RVA: 0x000B20CC File Offset: 0x000B02CC
	public virtual FlyingMermaidLevelTurtleSpiralProjectile Create(Vector2 position, float rotation, float speed, float rotationSpeed)
	{
		FlyingMermaidLevelTurtleSpiralProjectile flyingMermaidLevelTurtleSpiralProjectile = base.Create(position, rotation, speed) as FlyingMermaidLevelTurtleSpiralProjectile;
		flyingMermaidLevelTurtleSpiralProjectile.rotationSpeed = rotationSpeed;
		flyingMermaidLevelTurtleSpiralProjectile.rotationBase = new GameObject("SpiralProjectileBase");
		flyingMermaidLevelTurtleSpiralProjectile.rotationBase.transform.position = position;
		flyingMermaidLevelTurtleSpiralProjectile.transform.parent = flyingMermaidLevelTurtleSpiralProjectile.rotationBase.transform;
		flyingMermaidLevelTurtleSpiralProjectile.animator.Play("A", 0, Random.Range(0f, 1f));
		flyingMermaidLevelTurtleSpiralProjectile.animator.Play("A", 1, Random.Range(0f, 1f));
		return flyingMermaidLevelTurtleSpiralProjectile;
	}

	// Token: 0x06001DEB RID: 7659 RVA: 0x000B2170 File Offset: 0x000B0370
	public override void Move()
	{
		if (this.Speed == 0f)
		{
		}
		base.transform.localPosition += this.rotationBase.transform.InverseTransformDirection(base.transform.right) * this.Speed * CupheadTime.FixedDelta;
		this.rotationBase.transform.AddEulerAngles(0f, 0f, this.rotationSpeed * 360f * CupheadTime.FixedDelta);
	}

	// Token: 0x06001DEC RID: 7660 RVA: 0x0001947B File Offset: 0x0001767B
	public override void Die()
	{
		base.Die();
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x00019483 File Offset: 0x00017683
	public override void OnDestroy()
	{
		base.OnDestroy();
		Object.Destroy(this.rotationBase.gameObject);
	}

	// Token: 0x0400188C RID: 6284
	public float rotationSpeed;

	// Token: 0x0400188D RID: 6285
	public GameObject rotationBase;
}
