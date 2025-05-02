using System;
using UnityEngine;

// Token: 0x02000412 RID: 1042
public class StartHotdog : MonoBehaviour
{
	// Token: 0x06002D62 RID: 11618 RVA: 0x00025E04 File Offset: 0x00024004
	public void OnTriggerEnter2D(Collider2D c)
	{
		if (c.tag == "Player")
		{
			this.hotdog.ProjectilesCanHit = true;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0400259B RID: 9627
	public const string PlayerTag = "Player";

	// Token: 0x0400259C RID: 9628
	[SerializeField]
	public CircusPlatformingLevelHotdog hotdog;
}
