using System;

// Token: 0x020004E3 RID: 1251
public interface OnlineAchievement
{
	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x060033C8 RID: 13256
	string Id { get; }

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x060033C9 RID: 13257
	string Name { get; }

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x060033CA RID: 13258
	string Description { get; }

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x060033CB RID: 13259
	bool IsUnlocked { get; }
}
