using System;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class DevilLevelTear : AbstractProjectile
{
	// Token: 0x06001569 RID: 5481 RVA: 0x0009C16C File Offset: 0x0009A36C
	public DevilLevelTear CreateTear(Vector2 position, float speed)
	{
		DevilLevelTear devilLevelTear = this.InstantiatePrefab<DevilLevelTear>();
		devilLevelTear.transform.position = position;
		devilLevelTear.speed = speed;
		devilLevelTear.animator.Play("Drop_" + Random.Range(1, 7).ToStringInvariant());
		return devilLevelTear;
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x0001230E File Offset: 0x0001050E
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600156B RID: 5483 RVA: 0x0001232C File Offset: 0x0001052C
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		base.transform.AddPosition(0f, -this.speed * CupheadTime.FixedDelta, 0f);
	}

	// Token: 0x0400117D RID: 4477
	public float speed;
}
