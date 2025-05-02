using System;
using UnityEngine;

// Token: 0x0200029A RID: 666
public class FrogsLevelDemonTrigger : AbstractCollidableObject
{
	// Token: 0x06001E0C RID: 7692 RVA: 0x00019573 File Offset: 0x00017773
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001E0D RID: 7693 RVA: 0x0001959E File Offset: 0x0001779E
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.isTriggered = true;
	}

	// Token: 0x06001E0E RID: 7694 RVA: 0x000195BF File Offset: 0x000177BF
	public bool getTrigger()
	{
		return this.isTriggered;
	}

	// Token: 0x0400189C RID: 6300
	public DamageReceiver damageReceiver;

	// Token: 0x0400189D RID: 6301
	public bool isTriggered;
}
