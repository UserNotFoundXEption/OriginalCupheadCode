using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004F4 RID: 1268
public class PlatformHandlingTitleScreenOverride
{
	// Token: 0x0600344C RID: 13388 RVA: 0x0002AF2D File Offset: 0x0002912D
	public PlatformHandlingTitleScreenOverride(StartScreen.InitialLoadData startScreenLoadData)
	{
		this.startScreenLoadData = startScreenLoadData;
	}

	// Token: 0x0600344D RID: 13389 RVA: 0x000F63EC File Offset: 0x000F45EC
	public IEnumerator GetTitleScreenOverrideStatus_cr(MonoBehaviour parent)
	{
		yield return null;
		yield return null;
		this.startScreenLoadData.forceOriginalTitleScreen = SettingsData.Data.forceOriginalTitleScreen;
		yield break;
	}

	// Token: 0x04002B03 RID: 11011
	public static readonly string XboxOneForceOriginalTitleScreenKey = "XboxOne_ForceOriginalTitleScreen";

	// Token: 0x04002B04 RID: 11012
	public static readonly string UWPForceOriginalTitleScreenKey = "UWP_ForceOriginalTitleScreen";

	// Token: 0x04002B05 RID: 11013
	public StartScreen.InitialLoadData startScreenLoadData;
}
