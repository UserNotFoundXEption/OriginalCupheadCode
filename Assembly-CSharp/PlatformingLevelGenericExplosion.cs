using System;
using UnityEngine;

// Token: 0x020003D8 RID: 984
public class PlatformingLevelGenericExplosion : Effect
{
	// Token: 0x06002B70 RID: 11120 RVA: 0x000D5F00 File Offset: 0x000D4100
	public override Effect Create(Vector3 position, Vector3 scale)
	{
		float num = Random.Range(0f, 1f);
		if (num < this.lightningOnlyChance + this.starOnlyChance + this.starsPlusLightningChance)
		{
			if (num < this.lightningOnlyChance || num > this.lightningOnlyChance + this.starOnlyChance)
			{
				this.lightningPrefab.Create(position, scale);
			}
			if (num > this.lightningOnlyChance)
			{
				this.starsPrefab.Create(position, scale);
			}
		}
		return base.Create(position, scale);
	}

	// Token: 0x06002B71 RID: 11121 RVA: 0x000247C9 File Offset: 0x000229C9
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.lightningPrefab = null;
		this.starsPrefab = null;
	}

	// Token: 0x040023FE RID: 9214
	[SerializeField]
	public Effect lightningPrefab;

	// Token: 0x040023FF RID: 9215
	[SerializeField]
	public Effect starsPrefab;

	// Token: 0x04002400 RID: 9216
	[SerializeField]
	public float lightningOnlyChance;

	// Token: 0x04002401 RID: 9217
	[SerializeField]
	public float starOnlyChance;

	// Token: 0x04002402 RID: 9218
	[SerializeField]
	public float starsPlusLightningChance;
}
