using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class AssetLocationDatabase : ScriptableObject
{
	// Token: 0x0600069B RID: 1691 RVA: 0x00006B8D File Offset: 0x00004D8D
	public void SetDLCAssets(string[] dlcAssets)
	{
		this.dlcAssetNames = dlcAssets;
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x0600069C RID: 1692 RVA: 0x00006B96 File Offset: 0x00004D96
	public HashSet<string> dlcAssets
	{
		get
		{
			if (this._dlcAssets == null)
			{
				this._dlcAssets = new HashSet<string>(this.dlcAssetNames);
			}
			return this._dlcAssets;
		}
	}

	// Token: 0x040004EF RID: 1263
	[SerializeField]
	public string[] dlcAssetNames;

	// Token: 0x040004F0 RID: 1264
	public HashSet<string> _dlcAssets;

	// Token: 0x020008CD RID: 2253
	public enum AssetType
	{
		// Token: 0x04004356 RID: 17238
		Base,
		// Token: 0x04004357 RID: 17239
		DLC
	}
}
