using System;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class AirplaneLevelSecretTerrierBulletShrapnel : BasicProjectile
{
	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0000C541 File Offset: 0x0000A741
	public override Vector3 Direction
	{
		get
		{
			return -base.transform.up;
		}
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x0008B4E8 File Offset: 0x000896E8
	public override AbstractProjectile Create()
	{
		AirplaneLevelSecretTerrierBulletShrapnel airplaneLevelSecretTerrierBulletShrapnel = (AirplaneLevelSecretTerrierBulletShrapnel)base.Create();
		airplaneLevelSecretTerrierBulletShrapnel.anim.Play("Move", 0, (float)Random.Range(0, 1));
		return airplaneLevelSecretTerrierBulletShrapnel;
	}

	// Token: 0x04000BCC RID: 3020
	[SerializeField]
	public Animator anim;
}
