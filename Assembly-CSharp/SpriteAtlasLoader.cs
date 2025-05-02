using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

// Token: 0x02000090 RID: 144
public class SpriteAtlasLoader : AssetLoader<SpriteAtlas>
{
	// Token: 0x060006B4 RID: 1716 RVA: 0x00006CB0 File Offset: 0x00004EB0
	public void OnEnable()
	{
		SpriteAtlasManager.atlasRequested += new SpriteAtlasManager.RequestAtlasCallback(this.atlasRequestedHandler);
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x00006CC3 File Offset: 0x00004EC3
	public void OnDisable()
	{
		SpriteAtlasManager.atlasRequested -= new SpriteAtlasManager.RequestAtlasCallback(this.atlasRequestedHandler);
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x00070474 File Offset: 0x0006E674
	public override Coroutine loadAsset(string assetName, Action<SpriteAtlas> completionHandler)
	{
		Action<SpriteAtlas> completionHandler2 = delegate(SpriteAtlas atlas)
		{
			this.resolveDeferredRequests(assetName, atlas);
			completionHandler(atlas);
		};
		return AssetBundleLoader.LoadSpriteAtlas(assetName, completionHandler2);
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x00006CD6 File Offset: 0x00004ED6
	public override SpriteAtlas loadAssetSynchronous(string assetName)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x00006CDD File Offset: 0x00004EDD
	public override void destroyAsset(SpriteAtlas asset)
	{
		Object.Destroy(asset);
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x000704B8 File Offset: 0x0006E6B8
	public void addDeferredRequest(string assetName, Action<SpriteAtlas> action)
	{
		List<Action<SpriteAtlas>> list;
		if (!this.deferredAtlastRequests.TryGetValue(assetName, out list))
		{
			list = new List<Action<SpriteAtlas>>();
			this.deferredAtlastRequests.Add(assetName, list);
		}
		list.Add(action);
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x000704F4 File Offset: 0x0006E6F4
	public void resolveDeferredRequests(string assetName, SpriteAtlas atlas)
	{
		if (atlas == null)
		{
			return;
		}
		List<Action<SpriteAtlas>> list;
		if (this.deferredAtlastRequests.TryGetValue(assetName, out list))
		{
			foreach (Action<SpriteAtlas> action in list)
			{
				action(atlas);
			}
			this.deferredAtlastRequests.Remove(assetName);
		}
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00070578 File Offset: 0x0006E778
	public void atlasRequestedHandler(string atlasTag, Action<SpriteAtlas> action)
	{
		Action<SpriteAtlas> completionAction = delegate(SpriteAtlas atlas)
		{
			if (atlas == null)
			{
				this.addDeferredRequest(atlasTag, action);
			}
			else
			{
				action(atlas);
			}
		};
		base.loadAssetFromAssetBundle(atlasTag, AssetLoaderOption.None(), completionAction);
	}

	// Token: 0x040004FE RID: 1278
	public Dictionary<string, List<Action<SpriteAtlas>>> deferredAtlastRequests = new Dictionary<string, List<Action<SpriteAtlas>>>();
}
