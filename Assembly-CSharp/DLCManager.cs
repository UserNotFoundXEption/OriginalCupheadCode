using System;
using System.Collections;
using Steamworks;
using UnityEngine;
using UnityEngine.U2D;

// Token: 0x020004E0 RID: 1248
public class DLCManager
{
	// Token: 0x060033AC RID: 13228 RVA: 0x0002AC5C File Offset: 0x00028E5C
	public static void RefreshDLC()
	{
		DLCManager.refreshDLC();
	}

	// Token: 0x060033AD RID: 13229 RVA: 0x0002AC63 File Offset: 0x00028E63
	public static void CheckInstallationStatusChanged()
	{
		DLCManager.checkInstallationStatusChanged();
	}

	// Token: 0x060033AE RID: 13230 RVA: 0x000F5AC4 File Offset: 0x000F3CC4
	public static bool DLCEnabled()
	{
		bool result = false;
		DLCManager.dlcEnabled(ref result);
		return result;
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x000F5ADC File Offset: 0x000F3CDC
	public static string AssetBundlePath()
	{
		string result = null;
		DLCManager.assetBundlePath(ref result);
		return result;
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x000F5AF4 File Offset: 0x000F3CF4
	public static bool UsesAlternateBundleLoadingMechanism()
	{
		bool result = false;
		DLCManager.usesAlternateBundleLoadingMechanism(ref result);
		return result;
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x000F5B0C File Offset: 0x000F3D0C
	public static DLCManager.AssetBundleLoadWaitInstruction LoadAssetBundle(string path)
	{
		DLCManager.AssetBundleLoadWaitInstruction result = null;
		DLCManager.loadAssetBundle(path, ref result);
		return result;
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x000F5B24 File Offset: 0x000F3D24
	public static bool UnloadBundlesImmediately()
	{
		bool result = false;
		DLCManager.unloadBundlesImmediately(ref result);
		return result;
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x000F5B3C File Offset: 0x000F3D3C
	public static bool CanRedirectToStore()
	{
		bool result = false;
		DLCManager.canRedirectToStore(ref result);
		return result;
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x0002AC6A File Offset: 0x00028E6A
	public static void LaunchStore()
	{
		DLCManager.launchStore();
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x0002AC71 File Offset: 0x00028E71
	public static Coroutine[] LoadPersistentAssets()
	{
		if (DLCManager.persistentAssetsLoaded || !DLCManager.DLCEnabled())
		{
			return null;
		}
		DLCManager.persistentAssetsLoaded = true;
		return new Coroutine[]
		{
			AssetLoader<SpriteAtlas>.LoadPersistentAssetsDLC()
		};
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x0002AC9D File Offset: 0x00028E9D
	public static void ResetAvailabilityPrompt()
	{
		DLCManager.availabilityPromptTriggered = true;
		DLCManager.showAvailabilityPrompt = false;
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x060033B7 RID: 13239 RVA: 0x0002ACAB File Offset: 0x00028EAB
	// (set) Token: 0x060033B8 RID: 13240 RVA: 0x0002ACB2 File Offset: 0x00028EB2
	public static bool persistentAssetsLoaded { get; set; }

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x060033B9 RID: 13241 RVA: 0x0002ACBA File Offset: 0x00028EBA
	// (set) Token: 0x060033BA RID: 13242 RVA: 0x0002ACC1 File Offset: 0x00028EC1
	public static bool showAvailabilityPrompt { get; set; }

	// Token: 0x060033BB RID: 13243 RVA: 0x000F5B54 File Offset: 0x000F3D54
	public static bool steamDLCStatus()
	{
		ulong num;
		ulong num2;
		return SteamApps.BIsDlcInstalled(DLCManager.DLCAppID) && !SteamApps.GetDlcDownloadProgress(DLCManager.DLCAppID, ref num, ref num2);
	}

	// Token: 0x060033BC RID: 13244 RVA: 0x0002ACC9 File Offset: 0x00028EC9
	public static void refreshDLC()
	{
		if (!SteamManager.Initialized)
		{
			DLCManager.dlcAvailable = false;
			return;
		}
		if (!DLCManager.dlcAvailable)
		{
			DLCManager.dlcAvailable = DLCManager.steamDLCStatus();
		}
	}

	// Token: 0x060033BD RID: 13245 RVA: 0x0002ACF0 File Offset: 0x00028EF0
	public static void checkInstallationStatusChanged()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (DLCManager.DLCEnabled() || DLCManager.availabilityPromptTriggered)
		{
			return;
		}
		if (DLCManager.steamDLCStatus())
		{
			DLCManager.showAvailabilityPrompt = true;
		}
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x0002AD22 File Offset: 0x00028F22
	public static void dlcEnabled(ref bool enabled)
	{
		enabled = DLCManager.dlcAvailable;
	}

	// Token: 0x060033BF RID: 13247 RVA: 0x0002AD2B File Offset: 0x00028F2B
	public static void assetBundlePath(ref string path)
	{
		path = Application.streamingAssetsPath;
	}

	// Token: 0x060033C0 RID: 13248 RVA: 0x0002AD34 File Offset: 0x00028F34
	public static void usesAlternateBundleLoadingMechanism(ref bool usesAlternate)
	{
		usesAlternate = false;
	}

	// Token: 0x060033C1 RID: 13249 RVA: 0x0002AD39 File Offset: 0x00028F39
	public static void unloadBundlesImmediately(ref bool unloadImmediately)
	{
		unloadImmediately = false;
	}

	// Token: 0x060033C2 RID: 13250 RVA: 0x0002AD3E File Offset: 0x00028F3E
	public static void loadAssetBundle(string path, ref DLCManager.AssetBundleLoadWaitInstruction waitInstruction)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060033C3 RID: 13251 RVA: 0x0002AD45 File Offset: 0x00028F45
	public static void canRedirectToStore(ref bool canRedirect)
	{
		canRedirect = true;
	}

	// Token: 0x060033C4 RID: 13252 RVA: 0x0002AD4A File Offset: 0x00028F4A
	public static void launchStore()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamFriends.ActivateGameOverlayToStore(DLCManager.DLCAppID, 0);
	}

	// Token: 0x04002AEB RID: 10987
	public static bool availabilityPromptTriggered;

	// Token: 0x04002AEE RID: 10990
	public static readonly AppId_t DLCAppID = new AppId_t(1117850u);

	// Token: 0x04002AEF RID: 10991
	public static bool dlcAvailable;

	// Token: 0x02001149 RID: 4425
	public class AssetBundleLoadWaitInstruction : IEnumerator
	{
		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x06007D25 RID: 32037 RVA: 0x000540E7 File Offset: 0x000522E7
		public object Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06007D26 RID: 32038 RVA: 0x000540EA File Offset: 0x000522EA
		public bool MoveNext()
		{
			return !this.complete;
		}

		// Token: 0x06007D27 RID: 32039 RVA: 0x000540F5 File Offset: 0x000522F5
		public void Reset()
		{
		}

		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x06007D28 RID: 32040 RVA: 0x000540F7 File Offset: 0x000522F7
		// (set) Token: 0x06007D29 RID: 32041 RVA: 0x000540FF File Offset: 0x000522FF
		public AssetBundle assetBundle { get; set; }

		// Token: 0x040079A9 RID: 31145
		public bool complete;
	}
}
