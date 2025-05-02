using System;

// Token: 0x0200050A RID: 1290
public interface IParryAttack
{
	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x060035DF RID: 13791
	// (set) Token: 0x060035E0 RID: 13792
	bool AttackParryUsed { get; set; }

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x060035E1 RID: 13793
	// (set) Token: 0x060035E2 RID: 13794
	bool HasHitEnemy { get; set; }
}
