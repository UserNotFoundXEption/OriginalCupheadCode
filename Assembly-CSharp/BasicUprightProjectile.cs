using System;
using UnityEngine;

// Token: 0x0200058F RID: 1423
public class BasicUprightProjectile : BasicProjectile
{
	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x06003C16 RID: 15382 RVA: 0x00030A95 File Offset: 0x0002EC95
	public override Vector3 Direction
	{
		get
		{
			return this._direction;
		}
	}

	// Token: 0x06003C17 RID: 15383 RVA: 0x00114B70 File Offset: 0x00112D70
	public override AbstractProjectile Create(Vector2 position, float rotation)
	{
		BasicUprightProjectile basicUprightProjectile = base.Create(position, 0f) as BasicUprightProjectile;
		basicUprightProjectile._direction = Quaternion.Euler(0f, 0f, rotation) * Vector3.right;
		return basicUprightProjectile;
	}

	// Token: 0x04002FAF RID: 12207
	public Vector3 _direction;
}
