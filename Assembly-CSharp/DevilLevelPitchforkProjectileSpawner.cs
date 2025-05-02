using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B2 RID: 434
public class DevilLevelPitchforkProjectileSpawner
{
	// Token: 0x060014A5 RID: 5285 RVA: 0x000117EC File Offset: 0x0000F9EC
	public DevilLevelPitchforkProjectileSpawner(int numProjectiles, string angleOffsets)
	{
		this.numProjectiles = numProjectiles;
		this.angleOffsets = angleOffsets.Split(new char[]
		{
			','
		});
		this.angleOffsetIndex = Random.Range(0, angleOffsets.Length);
	}

	// Token: 0x060014A6 RID: 5286 RVA: 0x0009A51C File Offset: 0x0009871C
	public List<float> getSpawnAngles()
	{
		List<float> list = new List<float>();
		this.angleOffsetIndex = (this.angleOffsetIndex + 1) % this.angleOffsets.Length;
		float num = 0f;
		Parser.FloatTryParse(this.angleOffsets[this.angleOffsetIndex], out num);
		for (int i = 0; i < this.numProjectiles; i++)
		{
			list.Add((float)i * 360f / (float)this.numProjectiles + num + 90f);
		}
		return list;
	}

	// Token: 0x040010E5 RID: 4325
	public int numProjectiles;

	// Token: 0x040010E6 RID: 4326
	public string[] angleOffsets;

	// Token: 0x040010E7 RID: 4327
	public int angleOffsetIndex;
}
