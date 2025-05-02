using System;

// Token: 0x0200057F RID: 1407
public class ChaliceRecolorEvent : GameEvent
{
	// Token: 0x06003B0B RID: 15115 RVA: 0x0002FFC5 File Offset: 0x0002E1C5
	public ChaliceRecolorEvent(bool enabled)
	{
		this.enabled = enabled;
	}

	// Token: 0x04002F45 RID: 12101
	public bool enabled;
}
