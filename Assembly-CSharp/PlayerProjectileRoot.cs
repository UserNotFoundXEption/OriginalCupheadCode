using System;
using UnityEngine;

// Token: 0x02000523 RID: 1315
public class PlayerProjectileRoot : AbstractMonoBehaviour
{
	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06003781 RID: 14209 RVA: 0x0002D57E File Offset: 0x0002B77E
	public Vector2 Position
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06003782 RID: 14210 RVA: 0x001037C0 File Offset: 0x001019C0
	public float Rotation
	{
		get
		{
			return base.transform.eulerAngles.z;
		}
	}

	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x06003783 RID: 14211 RVA: 0x001037E0 File Offset: 0x001019E0
	public Vector3 Scale
	{
		get
		{
			float num = 1f;
			return new Vector3(1f, num, 1f);
		}
	}
}
