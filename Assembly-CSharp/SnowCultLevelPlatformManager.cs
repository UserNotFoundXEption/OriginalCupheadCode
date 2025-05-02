using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000395 RID: 917
public class SnowCultLevelPlatformManager : AbstractCollidableObject
{
	// Token: 0x0600287E RID: 10366 RVA: 0x00021FDC File Offset: 0x000201DC
	public void Start()
	{
		this.platforms = new List<SnowCultLevelPlatform>();
		this.InstantiatePlatforms();
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x000CE93C File Offset: 0x000CCB3C
	public void InstantiatePlatforms()
	{
		for (int i = 0; i < 20; i++)
		{
			SnowCultLevelPlatform snowCultLevelPlatform = Object.Instantiate<SnowCultLevelPlatform>(this.platformPrefab);
			snowCultLevelPlatform.gameObject.SetActive(false);
			snowCultLevelPlatform.transform.parent = base.transform;
			this.platforms.Add(snowCultLevelPlatform);
		}
	}

	// Token: 0x040021B6 RID: 8630
	public const int NUM_OF_PLATFORMS = 20;

	// Token: 0x040021B7 RID: 8631
	[SerializeField]
	public SnowCultLevelPlatform platformPrefab;

	// Token: 0x040021B8 RID: 8632
	public List<SnowCultLevelPlatform> platforms;
}
