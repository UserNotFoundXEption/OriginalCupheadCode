using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008F RID: 143
public class RuntimeSceneAssetDatabase : ScriptableObject
{
	// Token: 0x17000144 RID: 324
	// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00006C55 File Offset: 0x00004E55
	public HashSet<string> persistentAssets
	{
		get
		{
			if (this._persistentAssets == null)
			{
				this._persistentAssets = new HashSet<string>(this.INTERNAL_persistentAssetNames);
			}
			return this._persistentAssets;
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x060006B1 RID: 1713 RVA: 0x00006C79 File Offset: 0x00004E79
	public HashSet<string> persistentAssetsDLC
	{
		get
		{
			if (this._persistentAssetsDLC == null)
			{
				this._persistentAssetsDLC = new HashSet<string>(this.INTERNAL_persistentAssetNamesDLC);
			}
			return this._persistentAssetsDLC;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00070414 File Offset: 0x0006E614
	public Dictionary<string, string[]> sceneAssetMappings
	{
		get
		{
			if (this._sceneAssetMappings == null)
			{
				this._sceneAssetMappings = new Dictionary<string, string[]>();
				foreach (RuntimeSceneAssetDatabase.SceneAssetMapping sceneAssetMapping in this.INTERNAL_sceneAssetMappings)
				{
					this._sceneAssetMappings.Add(sceneAssetMapping.sceneName, sceneAssetMapping.assetNames);
				}
			}
			return this._sceneAssetMappings;
		}
	}

	// Token: 0x040004F8 RID: 1272
	public string[] INTERNAL_persistentAssetNames;

	// Token: 0x040004F9 RID: 1273
	public string[] INTERNAL_persistentAssetNamesDLC;

	// Token: 0x040004FA RID: 1274
	public RuntimeSceneAssetDatabase.SceneAssetMapping[] INTERNAL_sceneAssetMappings;

	// Token: 0x040004FB RID: 1275
	public HashSet<string> _persistentAssets;

	// Token: 0x040004FC RID: 1276
	public HashSet<string> _persistentAssetsDLC;

	// Token: 0x040004FD RID: 1277
	public Dictionary<string, string[]> _sceneAssetMappings;

	// Token: 0x020008D2 RID: 2258
	[Serializable]
	public class SceneAssetMapping
	{
		// Token: 0x04004390 RID: 17296
		public string sceneName;

		// Token: 0x04004391 RID: 17297
		public string[] assetNames;
	}
}
