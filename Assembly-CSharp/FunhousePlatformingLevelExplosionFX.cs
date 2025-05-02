using System;
using UnityEngine;

// Token: 0x02000418 RID: 1048
public class FunhousePlatformingLevelExplosionFX : Effect
{
	// Token: 0x06002D89 RID: 11657 RVA: 0x00026003 File Offset: 0x00024203
	public void SpawnSmoke()
	{
		this.smoke.Create(base.transform.position);
	}

	// Token: 0x06002D8A RID: 11658 RVA: 0x0002601C File Offset: 0x0002421C
	public void FirecrackerLines()
	{
		this.firecracker.Create(base.transform.position);
	}

	// Token: 0x06002D8B RID: 11659 RVA: 0x00026035 File Offset: 0x00024235
	public void MiniExplosion()
	{
		base.GetComponent<EffectRadius>().CreateInRadius();
	}

	// Token: 0x040025C2 RID: 9666
	[SerializeField]
	public Effect smoke;

	// Token: 0x040025C3 RID: 9667
	[SerializeField]
	public Effect firecracker;
}
