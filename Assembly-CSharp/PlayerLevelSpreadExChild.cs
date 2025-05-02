using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000553 RID: 1363
public class PlayerLevelSpreadExChild : BasicProjectile
{
	// Token: 0x06003915 RID: 14613 RVA: 0x0002E7E2 File Offset: 0x0002C9E2
	public override void Start()
	{
		base.Start();
		this.damageDealer.SetDamageSource(DamageDealer.DamageSource.Ex);
		base.StartCoroutine(this.trail_cr());
	}

	// Token: 0x06003916 RID: 14614 RVA: 0x0010A944 File Offset: 0x00108B44
	public IEnumerator trail_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.15f);
			Transform t = this.trailEffectPrefab.Create(base.transform.position).transform;
			t.SetParent(base.transform);
			t.ResetLocalTransforms();
			t.AddPositionForward2D(100f);
			t.SetParent(null);
		}
		yield break;
	}

	// Token: 0x06003917 RID: 14615 RVA: 0x0002E803 File Offset: 0x0002CA03
	public void _OnDieAnimComplete()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04002DEC RID: 11756
	public const float TRAIL_TIME = 0.15f;

	// Token: 0x04002DED RID: 11757
	[SerializeField]
	public Effect trailEffectPrefab;
}
