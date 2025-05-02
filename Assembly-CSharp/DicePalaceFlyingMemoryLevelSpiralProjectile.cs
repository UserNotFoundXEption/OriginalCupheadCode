using System;
using UnityEngine;

// Token: 0x020001F2 RID: 498
public class DicePalaceFlyingMemoryLevelSpiralProjectile : BasicProjectile
{
	// Token: 0x060016F8 RID: 5880 RVA: 0x000A0988 File Offset: 0x0009EB88
	public virtual BasicProjectile Create(Vector2 position, float rotation, float speed, float rotationSpeed, int direction)
	{
		DicePalaceFlyingMemoryLevelSpiralProjectile dicePalaceFlyingMemoryLevelSpiralProjectile = base.Create(position, rotation, speed) as DicePalaceFlyingMemoryLevelSpiralProjectile;
		dicePalaceFlyingMemoryLevelSpiralProjectile.rotationSpeed = rotationSpeed;
		dicePalaceFlyingMemoryLevelSpiralProjectile.rotationBase = new GameObject("SpiralProjectileBase").transform;
		dicePalaceFlyingMemoryLevelSpiralProjectile.rotationBase.position = position;
		dicePalaceFlyingMemoryLevelSpiralProjectile.transform.parent = dicePalaceFlyingMemoryLevelSpiralProjectile.rotationBase;
		dicePalaceFlyingMemoryLevelSpiralProjectile.direction = direction;
		return dicePalaceFlyingMemoryLevelSpiralProjectile;
	}

	// Token: 0x060016F9 RID: 5881 RVA: 0x000A09EC File Offset: 0x0009EBEC
	public override void Move()
	{
		float num = 360f;
		if (this.direction == 1)
		{
			num = -360f;
		}
		else if (this.direction == 2)
		{
			num = 360f;
		}
		if (this.Speed == 0f)
		{
		}
		base.transform.localPosition += this.rotationBase.InverseTransformDirection(base.transform.right) * this.Speed * CupheadTime.FixedDelta;
		this.rotationBase.AddEulerAngles(0f, 0f, this.rotationSpeed * num * CupheadTime.FixedDelta);
	}

	// Token: 0x060016FA RID: 5882 RVA: 0x00013883 File Offset: 0x00011A83
	public override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x040012B9 RID: 4793
	public int direction;

	// Token: 0x040012BA RID: 4794
	public float rotationSpeed;

	// Token: 0x040012BB RID: 4795
	public Transform rotationBase;
}
