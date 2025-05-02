using System;
using UnityEngine;

// Token: 0x0200041D RID: 1053
public class FunhousePlatformingLevelLips : BasicProjectile
{
	// Token: 0x06002DB1 RID: 11697 RVA: 0x00026198 File Offset: 0x00024398
	public override void Awake()
	{
		base.Awake();
		PlatformingLevelExit.OnWinStartEvent += this.OnWin;
	}

	// Token: 0x06002DB2 RID: 11698 RVA: 0x000261B1 File Offset: 0x000243B1
	public void Kiss()
	{
		AudioManager.Play("funhouse_honkbullet_kiss");
		this.emitAudioFromObject.Add("funhouse_honkbullet_kiss");
	}

	// Token: 0x06002DB3 RID: 11699 RVA: 0x000261CD File Offset: 0x000243CD
	public override void OnDestroy()
	{
		PlatformingLevelExit.OnWinStartEvent -= this.OnWin;
		base.OnDestroy();
	}

	// Token: 0x06002DB4 RID: 11700 RVA: 0x000261E6 File Offset: 0x000243E6
	public void OnWin()
	{
		Object.Destroy(base.gameObject);
	}
}
