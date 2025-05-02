using System;
using UnityEngine;

// Token: 0x020001F5 RID: 501
public class DicePalaceMainLevelBoardSpace : MonoBehaviour
{
	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06001725 RID: 5925 RVA: 0x00013AEE File Offset: 0x00011CEE
	// (set) Token: 0x06001726 RID: 5926 RVA: 0x00013B1F File Offset: 0x00011D1F
	public bool HasHeart
	{
		get
		{
			return !(this.heartSpace == null) && !(this.odds == null) && this.heartSpace.activeSelf;
		}
		set
		{
			if (this.heartSpace != null && this.odds != null)
			{
				this.odds.SetActive(!value);
				this.heartSpace.SetActive(value);
			}
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06001727 RID: 5927 RVA: 0x00013B5E File Offset: 0x00011D5E
	public Transform Pivot
	{
		get
		{
			return this.pivot;
		}
	}

	// Token: 0x1700027E RID: 638
	// (set) Token: 0x06001728 RID: 5928 RVA: 0x00013B66 File Offset: 0x00011D66
	public bool Clear
	{
		set
		{
			this.space.SetActive(!value);
			this.clearSpace.SetActive(value);
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06001729 RID: 5929 RVA: 0x00013B83 File Offset: 0x00011D83
	public Vector3 HeartSpacePosition
	{
		get
		{
			if (this.heartSpace != null)
			{
				return this.heartSpace.transform.position;
			}
			return Vector3.zero;
		}
	}

	// Token: 0x040012D5 RID: 4821
	[SerializeField]
	public GameObject space;

	// Token: 0x040012D6 RID: 4822
	[SerializeField]
	public GameObject clearSpace;

	// Token: 0x040012D7 RID: 4823
	[SerializeField]
	public GameObject odds;

	// Token: 0x040012D8 RID: 4824
	[SerializeField]
	public GameObject heartSpace;

	// Token: 0x040012D9 RID: 4825
	[SerializeField]
	public Transform pivot;
}
