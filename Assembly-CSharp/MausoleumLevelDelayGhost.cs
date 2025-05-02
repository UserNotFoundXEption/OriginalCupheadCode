using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002BD RID: 701
public class MausoleumLevelDelayGhost : MausoleumLevelGhostBase
{
	// Token: 0x06001F23 RID: 7971 RVA: 0x000B5898 File Offset: 0x000B3A98
	public MausoleumLevelDelayGhost Create(Vector2 position, float rotation, float speed, LevelProperties.Mausoleum.DelayGhost properties)
	{
		MausoleumLevelDelayGhost mausoleumLevelDelayGhost = base.Create(position, rotation, speed) as MausoleumLevelDelayGhost;
		mausoleumLevelDelayGhost.properties = properties;
		return mausoleumLevelDelayGhost;
	}

	// Token: 0x06001F24 RID: 7972 RVA: 0x0001A3E6 File Offset: 0x000185E6
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.wait_cr());
	}

	// Token: 0x06001F25 RID: 7973 RVA: 0x000B58C0 File Offset: 0x000B3AC0
	public IEnumerator wait_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.dashDelay);
		this.Speed = this.properties.speed;
		yield return null;
		yield break;
	}

	// Token: 0x04001970 RID: 6512
	public LevelProperties.Mausoleum.DelayGhost properties;
}
