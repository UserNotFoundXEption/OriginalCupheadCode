using System;

// Token: 0x020004E1 RID: 1249
public interface OnlineUser
{
	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x060033C6 RID: 13254
	string Name { get; }

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x060033C7 RID: 13255
	bool IsSignedIn { get; }
}
