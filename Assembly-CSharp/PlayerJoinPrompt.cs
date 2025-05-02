using System;

// Token: 0x020000F2 RID: 242
public class PlayerJoinPrompt : FlashingPrompt
{
	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06000B72 RID: 2930 RVA: 0x0000A3A6 File Offset: 0x000085A6
	public override bool ShouldShow
	{
		get
		{
			return PlayerManager.ShouldShowJoinPrompt;
		}
	}
}
