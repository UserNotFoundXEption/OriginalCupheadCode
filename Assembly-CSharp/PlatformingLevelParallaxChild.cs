using System;
using UnityEngine;

// Token: 0x02000455 RID: 1109
public class PlatformingLevelParallaxChild : AbstractMonoBehaviour
{
	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06002F6F RID: 12143 RVA: 0x0002780C File Offset: 0x00025A0C
	public int SortingOrderOffset
	{
		get
		{
			return this._sortingOrderOffset;
		}
	}

	// Token: 0x0400275A RID: 10074
	[SerializeField]
	public int _sortingOrderOffset;
}
