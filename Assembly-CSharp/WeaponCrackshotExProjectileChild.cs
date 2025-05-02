using System;
using UnityEngine;

// Token: 0x02000541 RID: 1345
public class WeaponCrackshotExProjectileChild : BasicProjectile
{
	// Token: 0x0600389C RID: 14492 RVA: 0x0010853C File Offset: 0x0010673C
	public override void Start()
	{
		base.Start();
		base.animator.SetBool("IsB", Rand.Bool());
		base.animator.Play((!Rand.Bool()) ? "CometStartA" : "CometStartB");
		this.damageDealer.isDLCWeapon = true;
	}

	// Token: 0x0600389D RID: 14493 RVA: 0x00108594 File Offset: 0x00106794
	public override void Die()
	{
		base.Die();
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsTag("Comet"))
		{
			base.animator.Play((!Rand.Bool()) ? "ImpactCometB" : "ImpactCometA");
		}
		else
		{
			base.animator.Play((!Rand.Bool()) ? "ImpactSmallB" : "ImpactSmallA");
		}
	}

	// Token: 0x0600389E RID: 14494 RVA: 0x0002E283 File Offset: 0x0002C483
	public void OnEffectComplete()
	{
		Object.Destroy(base.gameObject);
	}
}
