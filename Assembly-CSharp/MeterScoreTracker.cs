using System;
using System.Collections.Generic;

// Token: 0x02000522 RID: 1314
public class MeterScoreTracker
{
	// Token: 0x0600377C RID: 14204 RVA: 0x0002D513 File Offset: 0x0002B713
	public MeterScoreTracker(MeterScoreTracker.Type type)
	{
		this.type = type;
	}

	// Token: 0x0600377D RID: 14205 RVA: 0x0002D522 File Offset: 0x0002B722
	public void Add(DamageDealer damageDealer)
	{
		damageDealer.OnDealDamage += this.OnDealDamage;
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x0002D536 File Offset: 0x0002B736
	public void Add(AbstractProjectile projectile)
	{
		projectile.AddToMeterScoreTracker(this);
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x0002D53F File Offset: 0x0002B73F
	public void OnDealDamage(float damage, DamageReceiver damageReceiver, DamageDealer damageDealer)
	{
		if (!this.alreadyAddedScore)
		{
			Level.ScoringData.superMeterUsed += ((this.type != MeterScoreTracker.Type.Super) ? 1 : 5);
			this.alreadyAddedScore = true;
		}
	}

	// Token: 0x04002C9F RID: 11423
	public MeterScoreTracker.Type type;

	// Token: 0x04002CA0 RID: 11424
	public bool alreadyAddedScore;

	// Token: 0x04002CA1 RID: 11425
	public List<AbstractProjectile> projectilesToAdd;

	// Token: 0x020011A9 RID: 4521
	public enum Type
	{
		// Token: 0x04007BA3 RID: 31651
		Super,
		// Token: 0x04007BA4 RID: 31652
		Ex
	}
}
