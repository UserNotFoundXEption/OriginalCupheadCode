using System;
using UnityEngine;

// Token: 0x02000142 RID: 322
public class AirshipStorkLevelProjectile : BasicProjectile
{
	// Token: 0x06000F3A RID: 3898 RVA: 0x0008D188 File Offset: 0x0008B388
	public virtual BasicProjectile Create(Vector2 position, float rotation, float speed, float rotationSpeed, int direction)
	{
		AirshipStorkLevelProjectile airshipStorkLevelProjectile = base.Create(position, rotation, speed) as AirshipStorkLevelProjectile;
		airshipStorkLevelProjectile.rotationSpeed = rotationSpeed;
		airshipStorkLevelProjectile.rotationBase = new GameObject("SpiralProjectileBase").transform;
		airshipStorkLevelProjectile.rotationBase.position = position;
		airshipStorkLevelProjectile.transform.parent = airshipStorkLevelProjectile.rotationBase;
		airshipStorkLevelProjectile.direction = direction;
		return airshipStorkLevelProjectile;
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x0008D1EC File Offset: 0x0008B3EC
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
		if (base.transform.position.y < -360f || base.transform.position.y > 360f)
		{
			this.Die();
		}
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x0000CE53 File Offset: 0x0000B053
	public override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x04000C6F RID: 3183
	public int direction;

	// Token: 0x04000C70 RID: 3184
	public float rotationSpeed;

	// Token: 0x04000C71 RID: 3185
	public Transform rotationBase;
}
