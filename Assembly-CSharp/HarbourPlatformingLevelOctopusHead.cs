using System;
using UnityEngine;

// Token: 0x02000433 RID: 1075
public class HarbourPlatformingLevelOctopusHead : LevelPlatform
{
	// Token: 0x06002E62 RID: 11874 RVA: 0x00026ADD File Offset: 0x00024CDD
	public override void AddChild(Transform player)
	{
		base.AddChild(player);
		this.octopus.animator.SetBool("playerOn", true);
	}

	// Token: 0x06002E63 RID: 11875 RVA: 0x00026AFC File Offset: 0x00024CFC
	public override void OnPlayerExit(Transform player)
	{
		base.OnPlayerExit(player);
		if (base.transform.childCount <= 1)
		{
			this.octopus.animator.SetBool("playerOn", false);
		}
	}

	// Token: 0x04002678 RID: 9848
	[SerializeField]
	public HarbourPlatformingLevelOctopus octopus;
}
