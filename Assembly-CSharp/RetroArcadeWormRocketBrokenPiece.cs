using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000328 RID: 808
public class RetroArcadeWormRocketBrokenPiece : BasicProjectile
{
	// Token: 0x0600232D RID: 9005 RVA: 0x0001DC61 File Offset: 0x0001BE61
	public override void Awake()
	{
		base.Awake();
		this.Damage = PlayerManager.DamageMultiplier;
		base.StartCoroutine(this.turnOnCollider_cr());
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x000BFD24 File Offset: 0x000BDF24
	public IEnumerator turnOnCollider_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		base.GetComponent<Collider2D>().enabled = true;
		yield break;
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x0001DC81 File Offset: 0x0001BE81
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}
}
