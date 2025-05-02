using System;

// Token: 0x02000067 RID: 103
public static class PlatformHelper
{
	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000568 RID: 1384 RVA: 0x00005C74 File Offset: 0x00003E74
	public static bool IsConsole
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000569 RID: 1385 RVA: 0x00005C77 File Offset: 0x00003E77
	public static bool PreloadSettingsData
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x0600056A RID: 1386 RVA: 0x00005C7A File Offset: 0x00003E7A
	public static bool ShowAchievements
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x0600056B RID: 1387 RVA: 0x00005C7D File Offset: 0x00003E7D
	public static bool ShowDLCMenuItem
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x0600056C RID: 1388 RVA: 0x00005C80 File Offset: 0x00003E80
	public static bool GarbageCollectOnPause
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x0600056D RID: 1389 RVA: 0x00005C83 File Offset: 0x00003E83
	public static bool ForceAdditionalHeapMemory
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x0600056E RID: 1390 RVA: 0x00005C86 File Offset: 0x00003E86
	public static bool ManuallyRefreshDLCAvailability
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x0600056F RID: 1391 RVA: 0x00005C89 File Offset: 0x00003E89
	public static bool CanSwitchUserFromPause
	{
		get
		{
			return false;
		}
	}
}
