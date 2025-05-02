using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200043F RID: 1087
public class MountainPlatformingLevelDragonProjectile : BasicProjectile
{
	// Token: 0x17000362 RID: 866
	// (get) Token: 0x06002EB8 RID: 11960 RVA: 0x00026F71 File Offset: 0x00025171
	public override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06002EB9 RID: 11961 RVA: 0x000E0164 File Offset: 0x000DE364
	public override void Start()
	{
		base.Start();
		MountainPlatformingLevelDragonProjectile.numUntilPink--;
		this.DestroyDistance = -1f;
		if (MountainPlatformingLevelDragonProjectile.numUntilPink <= 0)
		{
			MountainPlatformingLevelDragonProjectile.numUntilPink = EnemyDatabase.GetProperties(EnemyID.dragon).MushroomPinkNumber.RandomInt();
			this.SetParryable(true);
		}
		else
		{
			this.SetParryable(false);
		}
		base.StartCoroutine(this.trail_cr());
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x000E01D4 File Offset: 0x000DE3D4
	public IEnumerator trail_cr()
	{
		while (!base.dead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.trailPeriod.RandomFloat());
			Effect effect = this.trailPrefab.Create(this.trailRoot.position + this.trailMaxOffset * MathUtils.RandomPointInUnitCircle());
			effect.animator.Play("PuffA");
		}
		yield break;
	}

	// Token: 0x06002EBB RID: 11963 RVA: 0x00026F78 File Offset: 0x00025178
	public override void OnParryDie()
	{
		base.OnParryDie();
		Object.Destroy(this);
	}

	// Token: 0x040026BE RID: 9918
	public static int numUntilPink;

	// Token: 0x040026BF RID: 9919
	[SerializeField]
	public Effect trailPrefab;

	// Token: 0x040026C0 RID: 9920
	[SerializeField]
	public MinMax trailPeriod;

	// Token: 0x040026C1 RID: 9921
	[SerializeField]
	public float trailMaxOffset;

	// Token: 0x040026C2 RID: 9922
	[SerializeField]
	public Transform trailRoot;
}
