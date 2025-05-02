using System;
using UnityEngine;

// Token: 0x020002B5 RID: 693
public class HouseLevelMusicNotes : AbstractPausableComponent
{
	// Token: 0x06001EFD RID: 7933 RVA: 0x0001A1BA File Offset: 0x000183BA
	public void ChangeAnimation()
	{
		base.animator.SetInteger("Type", Random.Range(0, 4));
	}
}
