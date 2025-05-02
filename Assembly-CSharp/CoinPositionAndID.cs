using System;

// Token: 0x02000113 RID: 275
[Serializable]
public struct CoinPositionAndID
{
	// Token: 0x06000D4D RID: 3405 RVA: 0x0000B6A7 File Offset: 0x000098A7
	public CoinPositionAndID(string id, float pos)
	{
		this.CoinID = id;
		this.xPos = pos;
	}

	// Token: 0x04000A6B RID: 2667
	public string CoinID;

	// Token: 0x04000A6C RID: 2668
	public float xPos;
}
