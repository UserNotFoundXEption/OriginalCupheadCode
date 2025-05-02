using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class ClownLevelCoasterPiece : AbstractCollidableObject
{
	// Token: 0x060013EA RID: 5098 RVA: 0x00010BC8 File Offset: 0x0000EDC8
	public void Init(Vector3 startPos)
	{
		base.transform.position = startPos;
	}

	// Token: 0x04001032 RID: 4146
	public List<ClownLevelRiders> riders;

	// Token: 0x04001033 RID: 4147
	public Transform newPieceRoot;

	// Token: 0x04001034 RID: 4148
	public Transform tailRoot;

	// Token: 0x04001035 RID: 4149
	public Transform ridersFrontRoot;

	// Token: 0x04001036 RID: 4150
	public Transform ridersBackRoot;
}
