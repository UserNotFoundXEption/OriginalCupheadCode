using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003ED RID: 1005
public class ForestPlatformingLevelMushroomProjectile : BasicProjectile
{
	// Token: 0x1700034F RID: 847
	// (get) Token: 0x06002C30 RID: 11312 RVA: 0x0002509B File Offset: 0x0002329B
	public override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06002C31 RID: 11313 RVA: 0x000D9404 File Offset: 0x000D7604
	public override void Start()
	{
		base.Start();
		ForestPlatformingLevelMushroomProjectile.numUntilPink--;
		this.DestroyDistance = -1f;
		if (ForestPlatformingLevelMushroomProjectile.numUntilPink == 0)
		{
			ForestPlatformingLevelMushroomProjectile.numUntilPink = EnemyDatabase.GetProperties(EnemyID.mushroom).MushroomPinkNumber.RandomInt();
			this.SetInt(AbstractProjectile.Variant, 1);
			this.SetParryable(true);
		}
		else
		{
			this.SetInt(AbstractProjectile.Variant, 0);
			this.SetParryable(false);
		}
		base.StartCoroutine(this.trail_cr());
	}

	// Token: 0x06002C32 RID: 11314 RVA: 0x000D948C File Offset: 0x000D768C
	public IEnumerator trail_cr()
	{
		while (!base.dead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.trailPeriod.RandomFloat());
			this.trailPrefab.Create(this.trailRoot.position + this.trailMaxOffset * MathUtils.RandomPointInUnitCircle());
		}
		yield break;
	}

	// Token: 0x06002C33 RID: 11315 RVA: 0x000250A2 File Offset: 0x000232A2
	public override void OnParryDie()
	{
		base.OnParryDie();
		Object.Destroy(this);
	}

	// Token: 0x04002480 RID: 9344
	public static int numUntilPink;

	// Token: 0x04002481 RID: 9345
	[SerializeField]
	public Effect trailPrefab;

	// Token: 0x04002482 RID: 9346
	[SerializeField]
	public MinMax trailPeriod;

	// Token: 0x04002483 RID: 9347
	[SerializeField]
	public float trailMaxOffset;

	// Token: 0x04002484 RID: 9348
	[SerializeField]
	public Transform trailRoot;
}
