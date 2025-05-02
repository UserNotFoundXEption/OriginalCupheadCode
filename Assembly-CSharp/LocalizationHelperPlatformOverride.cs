using System;
using UnityEngine;

// Token: 0x0200046E RID: 1134
public class LocalizationHelperPlatformOverride : MonoBehaviour
{
	// Token: 0x06003023 RID: 12323 RVA: 0x000E4C38 File Offset: 0x000E2E38
	public bool HasOverrideForCurrentPlatform(out int newID)
	{
		RuntimePlatform platform = Application.platform;
		for (int i = 0; i < this.overrides.Length; i++)
		{
			LocalizationHelperPlatformOverride.OverrideInfo overrideInfo = this.overrides[i];
			if (overrideInfo.platform == platform)
			{
				newID = overrideInfo.id;
				return true;
			}
		}
		newID = -1;
		return false;
	}

	// Token: 0x040027D1 RID: 10193
	public LocalizationHelperPlatformOverride.OverrideInfo[] overrides;

	// Token: 0x020010EB RID: 4331
	[Serializable]
	public class OverrideInfo
	{
		// Token: 0x040077EC RID: 30700
		public RuntimePlatform platform;

		// Token: 0x040077ED RID: 30701
		public int id;
	}
}
