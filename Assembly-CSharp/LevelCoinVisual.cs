using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class LevelCoinVisual : AbstractPausableComponent
{
	// Token: 0x06000D57 RID: 3415 RVA: 0x0000B6EC File Offset: 0x000098EC
	public void OnDeathAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}
}
