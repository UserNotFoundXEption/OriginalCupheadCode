using System;
using UnityEngine;

// Token: 0x02000091 RID: 145
public class TextureLoader : AssetLoader<Texture2D[]>
{
	// Token: 0x060006BD RID: 1725 RVA: 0x00006CED File Offset: 0x00004EED
	public override Coroutine loadAsset(string assetName, Action<Texture2D[]> completionHandler)
	{
		return AssetBundleLoader.LoadTextures(assetName, completionHandler);
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x00006CF6 File Offset: 0x00004EF6
	public override Texture2D[] loadAssetSynchronous(string assetName)
	{
		return AssetBundleLoader.LoadTexturesSynchronous(assetName);
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x000705C0 File Offset: 0x0006E7C0
	public override void destroyAsset(Texture2D[] asset)
	{
		for (int i = 0; i < asset.Length; i++)
		{
			Object.Destroy(asset[i]);
		}
	}
}
