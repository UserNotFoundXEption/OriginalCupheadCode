using System;

// Token: 0x02000579 RID: 1401
public class PlayerDebug
{
	// Token: 0x06003AC3 RID: 15043 RVA: 0x0002FC59 File Offset: 0x0002DE59
	public static void Enable()
	{
		PlayerDebug.Enabled = true;
	}

	// Token: 0x06003AC4 RID: 15044 RVA: 0x0002FC61 File Offset: 0x0002DE61
	public static void Disable()
	{
		PlayerDebug.Enabled = false;
	}

	// Token: 0x06003AC5 RID: 15045 RVA: 0x0002FC69 File Offset: 0x0002DE69
	public static void Toggle()
	{
		PlayerDebug.Enabled = !PlayerDebug.Enabled;
	}

	// Token: 0x04002F1B RID: 12059
	public static bool Enabled = true;
}
