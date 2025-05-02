using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

// Token: 0x02000088 RID: 136
public class AssetBundleLoader : MonoBehaviour
{
	// Token: 0x06000666 RID: 1638 RVA: 0x000069AF File Offset: 0x00004BAF
	public void Awake()
	{
		if (AssetBundleLoader.Instance != null)
		{
			throw new Exception("Should only be one instance");
		}
		AssetBundleLoader.Instance = this;
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0006F814 File Offset: 0x0006DA14
	public static void UnloadAssetBundles()
	{
		foreach (KeyValuePair<string, AssetBundleLoader.AssetBundleContainer> keyValuePair in AssetBundleLoader.Instance.loadedBundles)
		{
			keyValuePair.Value.assetBundle.Unload(false);
		}
		AssetBundleLoader.Instance.loadedBundles.Clear();
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x0006F890 File Offset: 0x0006DA90
	public static Coroutine LoadSpriteAtlas(string atlasName, Action<SpriteAtlas> completionHandler)
	{
		AssetBundleLoader.AssetBundleLocation location = AssetBundleLoader.AssetBundleLocation.StreamingAssets;
		if (AssetBundleLoader.Instance.atlasLocationDatabase.dlcAssets.Contains(atlasName))
		{
			location = AssetBundleLoader.AssetBundleLocation.DLC;
		}
		string spriteAtlasBundleName = AssetBundleLoader.GetSpriteAtlasBundleName(atlasName);
		return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAsset<SpriteAtlas>(spriteAtlasBundleName, location, atlasName, completionHandler));
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x0006F8DC File Offset: 0x0006DADC
	public static Coroutine LoadMusic(string audioClipName, Action<AudioClip> completionHandler)
	{
		AssetBundleLoader.AssetBundleLocation location = AssetBundleLoader.AssetBundleLocation.StreamingAssets;
		if (AssetBundleLoader.Instance.musicLocationDatabase.dlcAssets.Contains(audioClipName))
		{
			location = AssetBundleLoader.AssetBundleLocation.DLC;
		}
		string musicBundleName = AssetBundleLoader.GetMusicBundleName(audioClipName);
		return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAsset<AudioClip>(musicBundleName, location, audioClipName, completionHandler));
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x0006F928 File Offset: 0x0006DB28
	public static Coroutine LoadFont(string bundleName, string assetName, Action<Font> completionHandler)
	{
		AssetBundleLoader.AssetBundleLocation location = AssetBundleLoader.AssetBundleLocation.StreamingAssets;
		bundleName = AssetBundleLoader.AssetBundlePrefixFont + bundleName.ToLowerInvariant();
		return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAsset<Font>(bundleName, location, assetName, completionHandler));
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0006F964 File Offset: 0x0006DB64
	public static Coroutine LoadTMPFont(string bundleName, Action<Object[]> completionHandler)
	{
		AssetBundleLoader.AssetBundleLocation location = AssetBundleLoader.AssetBundleLocation.StreamingAssets;
		bundleName = AssetBundleLoader.AssetBundlePrefixTMPFont + bundleName.ToLowerInvariant();
		return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAllAssets<Object>(bundleName, location, completionHandler));
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x0006F99C File Offset: 0x0006DB9C
	public static Coroutine LoadTextures(string bundleName, Action<Texture2D[]> completionHandler)
	{
		AssetBundleLoader.AssetBundleLocation location = AssetBundleLoader.AssetBundleLocation.StreamingAssets;
		bundleName = AssetBundleLoader.AssetBundlePrefixTexture + bundleName.ToLowerInvariant();
		return AssetBundleLoader.Instance.StartCoroutine(AssetBundleLoader.Instance.loadAllAssets<Texture2D>(bundleName, location, completionHandler));
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x0006F9D4 File Offset: 0x0006DBD4
	public static Texture2D[] LoadTexturesSynchronous(string bundleName)
	{
		AssetBundleLoader.AssetBundleLocation location = AssetBundleLoader.AssetBundleLocation.StreamingAssets;
		bundleName = AssetBundleLoader.AssetBundlePrefixTexture + bundleName.ToLowerInvariant();
		return AssetBundleLoader.Instance.loadAllAssetsSynchronous<Texture2D>(bundleName, location);
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0006FA04 File Offset: 0x0006DC04
	public IEnumerator loadAssetBundle(string assetBundleName, AssetBundleLoader.AssetBundleLocation location)
	{
		AssetBundleLoader.loadCounter++;
		string path = AssetBundleLoader.getBasePath(location);
		path = Path.Combine(path, "AssetBundles");
		path = Path.Combine(path, assetBundleName);
		AssetBundle assetBundle;
		if (location == AssetBundleLoader.AssetBundleLocation.DLC && DLCManager.UsesAlternateBundleLoadingMechanism())
		{
			DLCManager.AssetBundleLoadWaitInstruction waitInstruction = DLCManager.LoadAssetBundle(path);
			yield return waitInstruction;
			assetBundle = waitInstruction.assetBundle;
		}
		else
		{
			AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
			yield return request;
			assetBundle = request.assetBundle;
		}
		this.loadedBundles.Add(assetBundleName, new AssetBundleLoader.AssetBundleContainer(assetBundle, location));
		AssetBundleLoader.loadCounter--;
		yield break;
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x0006FA30 File Offset: 0x0006DC30
	public AssetBundleLoader.AssetBundleContainer loadAssetBundleSynchronous(string assetBundleName, AssetBundleLoader.AssetBundleLocation location)
	{
		string text = AssetBundleLoader.getBasePath(location);
		text = Path.Combine(text, "AssetBundles");
		text = Path.Combine(text, assetBundleName);
		AssetBundle assetBundle = AssetBundle.LoadFromFile(text);
		AssetBundleLoader.AssetBundleContainer assetBundleContainer = new AssetBundleLoader.AssetBundleContainer(assetBundle, location);
		this.loadedBundles.Add(assetBundleName, assetBundleContainer);
		return assetBundleContainer;
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x0006FA78 File Offset: 0x0006DC78
	public IEnumerator loadAsset<T>(string assetBundleName, AssetBundleLoader.AssetBundleLocation location, string assetName, Action<T> completionHandler) where T : Object
	{
		AssetBundleLoader.loadCounter++;
		AssetBundleLoader.AssetBundleContainer assetBundleContainer;
		if (!this.loadedBundles.TryGetValue(assetBundleName, out assetBundleContainer))
		{
			yield return base.StartCoroutine(this.loadAssetBundle(assetBundleName, location));
			assetBundleContainer = this.loadedBundles[assetBundleName];
		}
		AssetBundleRequest assetRequest = assetBundleContainer.assetBundle.LoadAssetAsync<T>(assetName);
		yield return assetRequest;
		completionHandler(assetRequest.asset as T);
		if (assetBundleContainer.location == AssetBundleLoader.AssetBundleLocation.DLC && DLCManager.UnloadBundlesImmediately() && typeof(T) == typeof(SpriteAtlas))
		{
			this.loadedBundles.Remove(assetBundleContainer.assetBundle.name);
			assetBundleContainer.assetBundle.Unload(false);
		}
		AssetBundleLoader.loadCounter--;
		yield break;
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x0006FAB0 File Offset: 0x0006DCB0
	public IEnumerator loadAllAssets<T>(string assetBundleName, AssetBundleLoader.AssetBundleLocation location, Action<T[]> completionHandler) where T : Object
	{
		AssetBundleLoader.loadCounter++;
		AssetBundleLoader.AssetBundleContainer assetBundleContainer;
		if (!this.loadedBundles.TryGetValue(assetBundleName, out assetBundleContainer))
		{
			yield return base.StartCoroutine(this.loadAssetBundle(assetBundleName, location));
			assetBundleContainer = this.loadedBundles[assetBundleName];
		}
		AssetBundleRequest assetRequest = assetBundleContainer.assetBundle.LoadAllAssetsAsync<T>();
		yield return assetRequest;
		Object[] allAssets = assetRequest.allAssets;
		T[] castAssets = new T[allAssets.Length];
		for (int i = 0; i < allAssets.Length; i++)
		{
			castAssets[i] = (T)((object)allAssets[i]);
		}
		completionHandler(castAssets);
		AssetBundleLoader.loadCounter--;
		yield break;
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x0006FAE0 File Offset: 0x0006DCE0
	public T[] loadAllAssetsSynchronous<T>(string assetBundleName, AssetBundleLoader.AssetBundleLocation location) where T : Object
	{
		AssetBundleLoader.AssetBundleContainer assetBundleContainer;
		if (!this.loadedBundles.TryGetValue(assetBundleName, out assetBundleContainer))
		{
			assetBundleContainer = this.loadAssetBundleSynchronous(assetBundleName, location);
		}
		return assetBundleContainer.assetBundle.LoadAllAssets<T>();
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x000069D2 File Offset: 0x00004BD2
	public static string getBasePath(AssetBundleLoader.AssetBundleLocation location)
	{
		if (location == AssetBundleLoader.AssetBundleLocation.DLC)
		{
			return DLCManager.AssetBundlePath();
		}
		return Application.streamingAssetsPath;
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x000069E6 File Offset: 0x00004BE6
	public static string GetSpriteAtlasBundleName(string atlasName)
	{
		return AssetBundleLoader.AssetBundlePrefixSpriteAtlas + atlasName.ToLowerInvariant();
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x000069F8 File Offset: 0x00004BF8
	public static string GetMusicBundleName(string audioClipName)
	{
		return AssetBundleLoader.AssetBundlePrefixMusic + audioClipName.ToLowerInvariant();
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x00006A0A File Offset: 0x00004C0A
	public static List<string> DEBUG_LoadedAssetBundles()
	{
		return new List<string>(AssetBundleLoader.Instance.loadedBundles.Keys);
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000677 RID: 1655 RVA: 0x00006A20 File Offset: 0x00004C20
	// (set) Token: 0x06000678 RID: 1656 RVA: 0x00006A27 File Offset: 0x00004C27
	public static int loadCounter { get; set; }

	// Token: 0x040004DD RID: 1245
	public static readonly string AssetBundlePrefixSpriteAtlas = "atlas_";

	// Token: 0x040004DE RID: 1246
	public static readonly string AssetBundlePrefixMusic = "music_";

	// Token: 0x040004DF RID: 1247
	public static readonly string AssetBundlePrefixFont = "font_";

	// Token: 0x040004E0 RID: 1248
	public static readonly string AssetBundlePrefixTMPFont = "tmpfont_";

	// Token: 0x040004E1 RID: 1249
	public static readonly string AssetBundlePrefixTexture = "tex_";

	// Token: 0x040004E2 RID: 1250
	public static AssetBundleLoader Instance;

	// Token: 0x040004E3 RID: 1251
	[SerializeField]
	public AssetLocationDatabase atlasLocationDatabase;

	// Token: 0x040004E4 RID: 1252
	[SerializeField]
	public AssetLocationDatabase musicLocationDatabase;

	// Token: 0x040004E5 RID: 1253
	public Dictionary<string, AssetBundleLoader.AssetBundleContainer> loadedBundles = new Dictionary<string, AssetBundleLoader.AssetBundleContainer>();

	// Token: 0x020008C2 RID: 2242
	public enum AssetBundleLocation
	{
		// Token: 0x0400431A RID: 17178
		StreamingAssets,
		// Token: 0x0400431B RID: 17179
		DLC
	}

	// Token: 0x020008C3 RID: 2243
	public class AssetBundleContainer
	{
		// Token: 0x0600525C RID: 21084 RVA: 0x0003F019 File Offset: 0x0003D219
		public AssetBundleContainer(AssetBundle assetBundle, AssetBundleLoader.AssetBundleLocation location)
		{
			this.assetBundle = assetBundle;
			this.location = location;
		}

		// Token: 0x0400431C RID: 17180
		public AssetBundle assetBundle;

		// Token: 0x0400431D RID: 17181
		public AssetBundleLoader.AssetBundleLocation location;
	}
}
