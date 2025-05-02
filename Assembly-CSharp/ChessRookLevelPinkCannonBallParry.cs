using System;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class ChessRookLevelPinkCannonBallParry : AbstractProjectile
{
	// Token: 0x1700025A RID: 602
	// (get) Token: 0x0600134A RID: 4938 RVA: 0x00010381 File Offset: 0x0000E581
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x00010388 File Offset: 0x0000E588
	public override void RandomizeVariant()
	{
	}

	// Token: 0x0600134C RID: 4940 RVA: 0x0001038A File Offset: 0x0000E58A
	public override void SetTrigger(string trigger)
	{
	}

	// Token: 0x0600134D RID: 4941 RVA: 0x0001038C File Offset: 0x0000E58C
	public override void OnParry(AbstractPlayerController player)
	{
		this.main.GotParried(player);
	}

	// Token: 0x04000FA0 RID: 4000
	[SerializeField]
	public ChessRookLevelPinkCannonBall main;
}
