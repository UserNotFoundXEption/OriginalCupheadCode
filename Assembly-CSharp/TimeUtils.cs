using System;

// Token: 0x02000082 RID: 130
public static class TimeUtils
{
	// Token: 0x06000645 RID: 1605 RVA: 0x0006F0B0 File Offset: 0x0006D2B0
	public static int GetCurrentSecond()
	{
		DateTime d = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
		return (int)(DateTime.UtcNow - d).TotalSeconds;
	}
}
