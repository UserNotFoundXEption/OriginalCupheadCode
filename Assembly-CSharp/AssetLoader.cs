using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000089 RID: 137
public abstract class AssetLoader<T> : MonoBehaviour where T : class
{
	// Token: 0x0600067A RID: 1658 RVA: 0x00006A63 File Offset: 0x00004C63
	public AssetLoader()
	{
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00006A81 File Offset: 0x00004C81
	public void Awake()
	{
		if (AssetLoader<T>.Instance != null)
		{
			throw new Exception("More than one instance found");
		}
		AssetLoader<T>.Instance = this;
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00006AA4 File Offset: 0x00004CA4
	public void Start()
	{
		base.StartCoroutine(this.loadPersistentAssets());
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x0006FB14 File Offset: 0x0006DD14
	public IEnumerator loadPersistentAssets()
	{
		if (this.sceneAssetDatabase != null)
		{
			foreach (string assetName in this.sceneAssetDatabase.persistentAssets)
			{
				yield return this.loadAssetFromAssetBundle(assetName, AssetLoaderOption.PersistInCache(), null);
			}
		}
		AssetLoader<T>.persistentAssetsLoaded = true;
		yield break;
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x0006FB30 File Offset: 0x0006DD30
	public static string[] GetPreloadAssetNames(string sceneName)
	{
		string[] result;
		if (!AssetLoader<T>.Instance.sceneAssetDatabase.sceneAssetMappings.TryGetValue(sceneName, out result))
		{
			return new string[0];
		}
		return result;
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00006AB3 File Offset: 0x00004CB3
	public static Coroutine LoadAsset(string assetName, AssetLoaderOption option)
	{
		return AssetLoader<T>.Instance.loadAssetFromAssetBundle(assetName, option, null);
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x00006AC2 File Offset: 0x00004CC2
	public static T LoadAssetSynchronous(string assetName, AssetLoaderOption option)
	{
		return AssetLoader<T>.Instance.loadAssetFromAssetBundleSynchronous(assetName, option);
	}

	// Token: 0x06000681 RID: 1665
	public abstract Coroutine loadAsset(string assetName, Action<T> completionHandler);

	// Token: 0x06000682 RID: 1666
	public abstract T loadAssetSynchronous(string assetName);

	// Token: 0x06000683 RID: 1667 RVA: 0x00006AD0 File Offset: 0x00004CD0
	public static Coroutine LoadPersistentAssetsDLC()
	{
		return AssetLoader<T>.Instance.loadPersistentAssetsDLC();
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00006ADC File Offset: 0x00004CDC
	public Coroutine loadPersistentAssetsDLC()
	{
		return base.StartCoroutine(this.loadPersistentAssetsDLC_cr());
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x0006FB64 File Offset: 0x0006DD64
	public IEnumerator loadPersistentAssetsDLC_cr()
	{
		foreach (string assetName in this.sceneAssetDatabase.persistentAssetsDLC)
		{
			yield return this.loadAssetFromAssetBundle(assetName, AssetLoaderOption.PersistInCache(), null);
		}
		yield break;
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x0006FB80 File Offset: 0x0006DD80
	public static T GetCachedAsset(string assetName)
	{
		T result;
		if (AssetLoader<T>.Instance.tryGetAsset(assetName, out result))
		{
			return result;
		}
		throw new Exception("Asset not cached: " + assetName);
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x0006FBB4 File Offset: 0x0006DDB4
	public static void UnloadAssets(params string[] persistentTagsToUnload)
	{
		List<string> list = new List<string>(AssetLoader<T>.Instance.loadedAssets.Keys);
		for (int i = list.Count - 1; i >= 0; i--)
		{
			AssetLoader<T>.AssetContainer<T> assetContainer = AssetLoader<T>.Instance.loadedAssets[list[i]];
			if ((assetContainer.assetOption.type & AssetLoaderOption.Type.PersistInCache) != AssetLoaderOption.Type.None)
			{
				list.RemoveAt(i);
			}
			else if ((assetContainer.assetOption.type & AssetLoaderOption.Type.PersistInCacheTagged) != AssetLoaderOption.Type.None && Array.IndexOf<string>(persistentTagsToUnload, (string)assetContainer.assetOption.context) < 0)
			{
				list.RemoveAt(i);
			}
			else if ((assetContainer.assetOption.type & AssetLoaderOption.Type.DontDestroyOnUnload) == AssetLoaderOption.Type.None)
			{
				AssetLoader<T>.Instance.destroyAsset(assetContainer.asset);
			}
		}
		foreach (string key in list)
		{
			AssetLoader<T>.Instance.loadedAssets.Remove(key);
		}
	}

	// Token: 0x06000688 RID: 1672
	public abstract void destroyAsset(T asset);

	// Token: 0x06000689 RID: 1673 RVA: 0x00006AEA File Offset: 0x00004CEA
	public void cacheAsset(string assetName, AssetLoader<T>.AssetContainer<T> container)
	{
		this.loadedAssets.Add(assetName, container);
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x0006FCDC File Offset: 0x0006DEDC
	public bool tryGetAsset(string assetName, out T asset)
	{
		asset = (T)((object)null);
		AssetLoader<T>.AssetContainer<T> assetContainer;
		if (this.loadedAssets.TryGetValue(assetName, out assetContainer))
		{
			asset = assetContainer.asset;
			return true;
		}
		return false;
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x0006FD18 File Offset: 0x0006DF18
	public Coroutine loadAssetFromAssetBundle(string assetName, AssetLoaderOption option, Action<T> completionAction)
	{
		T obj;
		if (this.tryGetAsset(assetName, out obj))
		{
			if (completionAction != null)
			{
				completionAction(obj);
			}
			return null;
		}
		if (!DLCManager.DLCEnabled() && this.assetLocationDatabase.dlcAssets.Contains(assetName))
		{
			if (completionAction != null)
			{
				completionAction((T)((object)null));
			}
			return null;
		}
		AssetLoader<T>.LoadOperation loadOperation;
		if (!this.loadOperations.TryGetValue(assetName, out loadOperation))
		{
			loadOperation = new AssetLoader<T>.LoadOperation();
			this.loadOperations.Add(assetName, loadOperation);
			loadOperation.coroutine = this.loadAsset(assetName, delegate(T asset)
			{
				this.cacheAsset(assetName, new AssetLoader<T>.AssetContainer<T>(asset, option));
				AssetLoader<T>.LoadOperation loadOperation2 = this.loadOperations[assetName];
				foreach (Action<T> action in loadOperation2.completionHandlers)
				{
					if (action != null)
					{
						action(asset);
					}
				}
				this.loadOperations.Remove(assetName);
			});
		}
		loadOperation.completionHandlers.Add(completionAction);
		return loadOperation.coroutine;
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x0006FDFC File Offset: 0x0006DFFC
	public T loadAssetFromAssetBundleSynchronous(string assetName, AssetLoaderOption option)
	{
		T result;
		if (this.tryGetAsset(assetName, out result))
		{
			return result;
		}
		T t = this.loadAssetSynchronous(assetName);
		this.cacheAsset(assetName, new AssetLoader<T>.AssetContainer<T>(t, option));
		return t;
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x00006AF9 File Offset: 0x00004CF9
	public static bool IsDLCAsset(string assetName)
	{
		return AssetLoader<T>.Instance.assetLocationDatabase.dlcAssets.Contains(assetName);
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x0006FE30 File Offset: 0x0006E030
	public static List<string> DEBUG_GetLoadedAssets()
	{
		List<string> list = new List<string>(AssetLoader<T>.Instance.loadedAssets.Count);
		foreach (KeyValuePair<string, AssetLoader<T>.AssetContainer<T>> keyValuePair in AssetLoader<T>.Instance.loadedAssets)
		{
			AssetLoader<T>.AssetContainer<T> value = keyValuePair.Value;
			string text = string.Format("{0} ({1})", keyValuePair.Key, value.assetOption.type.ToString());
			if ((value.assetOption.type & AssetLoaderOption.Type.PersistInCacheTagged) != AssetLoaderOption.Type.None)
			{
				text += string.Format(" [Tag={0}]", value.assetOption.context);
			}
			list.Add(text);
		}
		return list;
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x0600068F RID: 1679 RVA: 0x00006B10 File Offset: 0x00004D10
	// (set) Token: 0x06000690 RID: 1680 RVA: 0x00006B1C File Offset: 0x00004D1C
	public static bool persistentAssetsLoaded
	{
		get
		{
			return AssetLoader<T>.Instance._persistentAssetsLoaded;
		}
		set
		{
			AssetLoader<T>.Instance._persistentAssetsLoaded = value;
		}
	}

	// Token: 0x040004E7 RID: 1255
	public static AssetLoader<T> Instance;

	// Token: 0x040004E8 RID: 1256
	[SerializeField]
	public RuntimeSceneAssetDatabase sceneAssetDatabase;

	// Token: 0x040004E9 RID: 1257
	[SerializeField]
	public AssetLocationDatabase assetLocationDatabase;

	// Token: 0x040004EA RID: 1258
	public Dictionary<string, AssetLoader<T>.LoadOperation> loadOperations = new Dictionary<string, AssetLoader<T>.LoadOperation>();

	// Token: 0x040004EB RID: 1259
	public Dictionary<string, AssetLoader<T>.AssetContainer<T>> loadedAssets = new Dictionary<string, AssetLoader<T>.AssetContainer<T>>();

	// Token: 0x040004EC RID: 1260
	public bool _persistentAssetsLoaded;

	// Token: 0x020008C7 RID: 2247
	public class AssetContainer<U>
	{
		// Token: 0x0600526F RID: 21103 RVA: 0x0003F0BC File Offset: 0x0003D2BC
		public AssetContainer(U asset, AssetLoaderOption assetOption)
		{
			this.asset = asset;
			this.assetOption = assetOption;
		}

		// Token: 0x0400433D RID: 17213
		public U asset;

		// Token: 0x0400433E RID: 17214
		public AssetLoaderOption assetOption;
	}

	// Token: 0x020008C8 RID: 2248
	public class LoadOperation
	{
		// Token: 0x0400433F RID: 17215
		public Coroutine coroutine;

		// Token: 0x04004340 RID: 17216
		public List<Action<T>> completionHandlers = new List<Action<T>>();
	}
}
