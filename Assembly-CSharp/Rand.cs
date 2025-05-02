using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
public static class Rand
{
	// Token: 0x060005B1 RID: 1457 RVA: 0x000060B3 File Offset: 0x000042B3
	public static bool Bool()
	{
		return Random.Range(0, 2) == 1;
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x000060C9 File Offset: 0x000042C9
	public static int PosOrNeg()
	{
		return (!Rand.Bool()) ? -1 : 1;
	}
}
