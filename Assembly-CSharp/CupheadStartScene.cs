using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

// Token: 0x020000AF RID: 175
public class CupheadStartScene : AbstractMonoBehaviour
{
	// Token: 0x06000833 RID: 2099 RVA: 0x00007E19 File Offset: 0x00006019
	public override void Awake()
	{
		Application.targetFrameRate = 60;
		Cuphead.Init(true);
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x00007E28 File Offset: 0x00006028
	public void Start()
	{
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00075C10 File Offset: 0x00073E10
	public IEnumerator start_cr()
	{
		yield return null;
		yield return null;
		AssetLoader<Texture2D[]>.LoadAssetSynchronous("screen_fx", AssetLoaderOption.DontDestroyOnUnload());
		Object.FindObjectOfType<ChromaticAberrationFilmGrain>().Initialize(AssetLoader<Texture2D[]>.GetCachedAsset("screen_fx"));
		if (PlatformHelper.ForceAdditionalHeapMemory)
		{
			HeapAllocator.Allocate(100);
			yield return null;
			yield return null;
		}
		if (PlatformHelper.PreloadSettingsData)
		{
			OnlineManager.Instance.Init();
			SettingsData.LoadFromCloud(new SettingsData.SettingsDataLoadFromCloudHandler(this.OnSettingsDataLoaded));
			while (!this.settingsDataLoaded)
			{
				yield return null;
			}
		}
		StartScreen.InitialLoadData startScreenLoadData = new StartScreen.InitialLoadData();
		PlatformHandlingTitleScreenOverride titleScreenOverride = new PlatformHandlingTitleScreenOverride(startScreenLoadData);
		yield return base.StartCoroutine(titleScreenOverride.GetTitleScreenOverrideStatus_cr(this));
		StartScreen.initialLoadData = startScreenLoadData;
		titleScreenOverride = null;
		Coroutine[] fontCoroutines = FontLoader.Initialize();
		foreach (Coroutine coroutine in fontCoroutines)
		{
			yield return coroutine;
		}
		while (AssetBundleLoader.loadCounter > 0 || !AssetLoader<SpriteAtlas>.persistentAssetsLoaded || !AssetLoader<AudioClip>.persistentAssetsLoaded || !AssetLoader<Texture2D[]>.persistentAssetsLoaded)
		{
			yield return null;
		}
		yield return null;
		Cuphead.Init(false);
		yield return new WaitForSeconds(0.1f);
		DLCManager.RefreshDLC();
		yield return null;
		yield return null;
		Coroutine[] coroutines = DLCManager.LoadPersistentAssets();
		if (coroutines != null)
		{
			foreach (Coroutine coroutine2 in coroutines)
			{
				yield return coroutine2;
			}
			yield return null;
			yield return null;
		}
		string titleSceneName = "scene_title";
		string[] preloadAtlases = AssetLoader<SpriteAtlas>.GetPreloadAssetNames(titleSceneName);
		foreach (string atlas in preloadAtlases)
		{
			yield return AssetLoader<SpriteAtlas>.LoadAsset(atlas, AssetLoaderOption.None());
		}
		string[] preloadMusic = AssetLoader<AudioClip>.GetPreloadAssetNames(titleSceneName);
		foreach (string clip in preloadMusic)
		{
			yield return AssetLoader<AudioClip>.LoadAsset(clip, AssetLoaderOption.None());
		}
		yield return null;
		yield return null;
		SceneManager.LoadSceneAsync(1);
		yield break;
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00007E37 File Offset: 0x00006037
	public void OnSettingsDataLoaded(bool success)
	{
		if (!success)
		{
			SettingsData.LoadFromCloud(new SettingsData.SettingsDataLoadFromCloudHandler(this.OnSettingsDataLoaded));
			return;
		}
		this.settingsDataLoaded = true;
	}

	// Token: 0x04000648 RID: 1608
	public bool settingsDataLoaded;
}
