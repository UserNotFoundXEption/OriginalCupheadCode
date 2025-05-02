using System;

// Token: 0x020002FC RID: 764
public class PlayersStatsBossesHub
{
	// Token: 0x060021FF RID: 8703 RVA: 0x0001D17B File Offset: 0x0001B37B
	public void LoseBonusHP()
	{
		if (this.BonusHP > 0)
		{
			this.BonusHP--;
		}
	}

	// Token: 0x06002200 RID: 8704 RVA: 0x0001D197 File Offset: 0x0001B397
	public void LoseHealerHP()
	{
		if (this.healerHP > 0)
		{
			this.healerHP--;
		}
	}

	// Token: 0x04001BFA RID: 7162
	public int HP;

	// Token: 0x04001BFB RID: 7163
	public int BonusHP;

	// Token: 0x04001BFC RID: 7164
	public float SuperCharge;

	// Token: 0x04001BFD RID: 7165
	public Weapon basePrimaryWeapon;

	// Token: 0x04001BFE RID: 7166
	public Weapon baseSecondaryWeapon;

	// Token: 0x04001BFF RID: 7167
	public Super BaseSuper;

	// Token: 0x04001C00 RID: 7168
	public Charm BaseCharm;

	// Token: 0x04001C01 RID: 7169
	public int tokenCount;

	// Token: 0x04001C02 RID: 7170
	public int healerHP;

	// Token: 0x04001C03 RID: 7171
	public int healerHPReceived;

	// Token: 0x04001C04 RID: 7172
	public int healerHPCounter;
}
