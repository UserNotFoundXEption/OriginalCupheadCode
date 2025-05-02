using System;
using System.Runtime.InteropServices;

// Token: 0x020004F5 RID: 1269
public static class PlayStation4Utilities
{
	// Token: 0x0600344F RID: 13391
	[DllImport("GetParam")]
	public static extern int get_system_service_param(int param, out int value);

	// Token: 0x06003450 RID: 13392 RVA: 0x000F6408 File Offset: 0x000F4608
	public static int GetSystemServiceParam(int param)
	{
		int result;
		int num = PlayStation4Utilities.get_system_service_param(param, out result);
		if (num != PlayStation4Utilities.SCE_OK)
		{
			throw new Exception("Error getting param. Result code: " + num);
		}
		return result;
	}

	// Token: 0x04002B06 RID: 11014
	public static readonly int SCE_OK;

	// Token: 0x04002B07 RID: 11015
	public static readonly int SCE_SYSTEM_SERVICE_PARAM_ID_ENTER_BUTTON_ASSIGN = 1000;

	// Token: 0x04002B08 RID: 11016
	public static readonly int SCE_SYSTEM_PARAM_ENTER_BUTTON_ASSIGN_CIRCLE;

	// Token: 0x04002B09 RID: 11017
	public static readonly int SCE_SYSTEM_PARAM_ENTER_BUTTON_ASSIGN_CROSS = 1;
}
