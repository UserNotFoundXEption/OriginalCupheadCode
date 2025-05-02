using System;
using UnityEngine;

// Token: 0x0200034E RID: 846
public class RumRunnersLevelPoliceBullet : BasicProjectile
{
	// Token: 0x1700030F RID: 783
	// (get) Token: 0x0600250A RID: 9482 RVA: 0x0001F412 File Offset: 0x0001D612
	// (set) Token: 0x0600250B RID: 9483 RVA: 0x0001F41A File Offset: 0x0001D61A
	public float spiderDamage { get; set; }

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x0600250C RID: 9484 RVA: 0x0001F423 File Offset: 0x0001D623
	// (set) Token: 0x0600250D RID: 9485 RVA: 0x0001F42B File Offset: 0x0001D62B
	public RumRunnersLevelPoliceman.Direction direction { get; set; }

	// Token: 0x0600250E RID: 9486 RVA: 0x000C56F8 File Offset: 0x000C38F8
	public override void Start()
	{
		base.Start();
		this.spiderDamageDealer = new DamageDealer(this);
		this.spiderDamageDealer.SetDamage(this.spiderDamage);
		this.spiderDamageDealer.OnDealDamage += this.OnDealDamage;
		this.spiderDamageDealer.SetStoneTime(base.StoneTime);
		this.spiderDamageDealer.PlayerId = this.PlayerId;
	}

	// Token: 0x0600250F RID: 9487 RVA: 0x000C5764 File Offset: 0x000C3964
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			DamageReceiver damageReceiver = hit.GetComponent<DamageReceiver>();
			if (damageReceiver == null)
			{
				DamageReceiverChild component = hit.GetComponent<DamageReceiverChild>();
				if (component != null)
				{
					damageReceiver = component.Receiver;
				}
			}
			if (damageReceiver != null && damageReceiver.GetComponent<RumRunnersLevelSpider>() != null)
			{
				this.spiderDamageDealer.DealDamage(hit);
				base.OnCollisionEnemy(hit, phase);
				this.Die();
			}
		}
	}

	// Token: 0x04001EA5 RID: 7845
	public DamageDealer spiderDamageDealer;
}
