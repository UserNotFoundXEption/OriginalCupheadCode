using System;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class MusicLoader : AssetLoader<AudioClip>
{
	// Token: 0x060006AC RID: 1708 RVA: 0x00006C35 File Offset: 0x00004E35
	public override Coroutine loadAsset(string assetName, Action<AudioClip> completionHandler)
	{
		return AssetBundleLoader.LoadMusic(assetName, completionHandler);
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00006C3E File Offset: 0x00004E3E
	public override AudioClip loadAssetSynchronous(string assetName)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x00006C45 File Offset: 0x00004E45
	public override void destroyAsset(AudioClip asset)
	{
		Object.Destroy(asset);
	}
}
