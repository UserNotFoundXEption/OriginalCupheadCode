using System;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class ChessBishopLevelIntroCandle : MonoBehaviour
{
	// Token: 0x06001246 RID: 4678 RVA: 0x0000F717 File Offset: 0x0000D917
	public void AniEvent_StartMove()
	{
		this.moving = true;
		this.glow.SetActive(false);
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x00094B24 File Offset: 0x00092D24
	public void Update()
	{
		this.shadow.transform.position = new Vector3(this.shadow.transform.position.x, -40f);
	}

	// Token: 0x04000ECC RID: 3788
	public bool moving;

	// Token: 0x04000ECD RID: 3789
	[SerializeField]
	public GameObject glow;

	// Token: 0x04000ECE RID: 3790
	[SerializeField]
	public GameObject shadow;
}
