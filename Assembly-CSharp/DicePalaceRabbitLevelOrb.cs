using System;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class DicePalaceRabbitLevelOrb : AbstractProjectile
{
	// Token: 0x060017A7 RID: 6055 RVA: 0x0001423D File Offset: 0x0001243D
	public override void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		base.Update();
	}

	// Token: 0x060017A8 RID: 6056 RVA: 0x0001425B File Offset: 0x0001245B
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060017A9 RID: 6057 RVA: 0x00014279 File Offset: 0x00012479
	public void SetAsGold(bool isGold)
	{
		if (isGold)
		{
			base.animator.SetTrigger("Gold");
		}
		else
		{
			base.animator.SetTrigger("Blue");
		}
	}
}
