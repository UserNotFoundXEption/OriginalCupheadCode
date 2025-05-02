using System;
using UnityEngine;

// Token: 0x0200027E RID: 638
public class FlyingMermaidLevelDebrisSpawner : ScrollingSpriteSpawner
{
	// Token: 0x06001D0C RID: 7436 RVA: 0x000AFD74 File Offset: 0x000ADF74
	public override void OnSpawn(GameObject obj)
	{
		base.OnSpawn(obj);
		FlyingMermaidLevelFloater component = obj.GetComponent<FlyingMermaidLevelFloater>();
		if (component != null)
		{
			component.trackingWater = this.trackingWater;
		}
	}

	// Token: 0x040017B0 RID: 6064
	[SerializeField]
	public GameObject trackingWater;
}
