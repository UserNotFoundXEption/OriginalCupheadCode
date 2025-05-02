using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020000CA RID: 202
[Serializable]
public class SettingsData
{
	// Token: 0x06000972 RID: 2418 RVA: 0x00078CEC File Offset: 0x00076EEC
	public SettingsData()
	{
		this.overscan = 0f;
		this.chromaticAberration = 1f;
		this.screenWidth = Screen.currentResolution.width;
		this.screenHeight = Screen.currentResolution.height;
		this.fullScreen = Screen.fullScreen;
		this.vSyncCount = QualitySettings.vSyncCount;
		this.masterVolume = SettingsData.originalMasterVolume;
		this.sFXVolume = SettingsData.originalsFXVolume;
		this.musicVolume = SettingsData.originalMusicVolume;
		this.hasBootedUpGame = false;
		this.SetCameraEffectDefaults();
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06000973 RID: 2419 RVA: 0x00078D90 File Offset: 0x00076F90
	public static SettingsData Data
	{
		get
		{
			if (SettingsData._data == null)
			{
				if (!SettingsData.originalAudioValuesInitialized)
				{
					SettingsData.originalAudioValuesInitialized = true;
					SettingsData.originalMasterVolume = AudioManager.masterVolume;
					SettingsData.originalsFXVolume = AudioManager.sfxOptionsVolume;
					SettingsData.originalMusicVolume = AudioManager.bgmOptionsVolume;
				}
				if (SettingsData.hasKey())
				{
					try
					{
						SettingsData._data = JsonUtility.FromJson<SettingsData>(PlayerPrefs.GetString("cuphead_settings_data_v1"));
					}
					catch (ArgumentException)
					{
						SettingsData._data = new SettingsData();
						SettingsData.Save();
					}
				}
				else
				{
					SettingsData._data = new SettingsData();
					SettingsData.Save();
				}
				if (SettingsData._data == null)
				{
					return null;
				}
				SettingsData.ApplySettings();
			}
			return SettingsData._data;
		}
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00078E4C File Offset: 0x0007704C
	public static void Save()
	{
		string text = JsonUtility.ToJson(SettingsData._data);
		PlayerPrefs.SetString("cuphead_settings_data_v1", text);
		PlayerPrefs.Save();
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x00078E74 File Offset: 0x00077074
	public static void LoadFromCloud(SettingsData.SettingsDataLoadFromCloudHandler handler)
	{
		SettingsData._loadFromCloudHandler = handler;
		if (OnlineManager.Instance.Interface.CloudStorageInitialized)
		{
			OnlineInterface @interface = OnlineManager.Instance.Interface;
			string[] keys = new string[]
			{
				"cuphead_settings_data_v1"
			};
			if (SettingsData.<>f__mg$cache0 == null)
			{
				SettingsData.<>f__mg$cache0 = new LoadCloudDataHandler(SettingsData.OnLoadedCloudData);
			}
			@interface.LoadCloudData(keys, SettingsData.<>f__mg$cache0);
		}
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x00078ED8 File Offset: 0x000770D8
	public static void SaveToCloud()
	{
		if (OnlineManager.Instance.Interface.CloudStorageInitialized)
		{
			string value = JsonUtility.ToJson(SettingsData._data);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["cuphead_settings_data_v1"] = value;
			OnlineInterface @interface = OnlineManager.Instance.Interface;
			IDictionary<string, string> data = dictionary;
			if (SettingsData.<>f__mg$cache1 == null)
			{
				SettingsData.<>f__mg$cache1 = new SaveCloudDataHandler(SettingsData.OnSavedCloudData);
			}
			@interface.SaveCloudData(data, SettingsData.<>f__mg$cache1);
		}
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x00008D50 File Offset: 0x00006F50
	public static void OnSavedCloudData(bool success)
	{
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x00078F44 File Offset: 0x00077144
	public static void OnLoadedCloudData(string[] data, CloudLoadResult result)
	{
		if (result == CloudLoadResult.Failed)
		{
			SettingsData.LoadFromCloud(SettingsData._loadFromCloudHandler);
			return;
		}
		try
		{
			if (result == CloudLoadResult.NoData)
			{
				if (SettingsData.hasKey())
				{
					try
					{
						SettingsData._data = JsonUtility.FromJson<SettingsData>(PlayerPrefs.GetString("cuphead_settings_data_v1"));
					}
					catch (ArgumentException)
					{
						SettingsData._data = new SettingsData();
					}
				}
				else
				{
					SettingsData._data = new SettingsData();
				}
				SettingsData.SaveToCloud();
			}
			else
			{
				SettingsData._data = JsonUtility.FromJson<SettingsData>(data[0]);
			}
		}
		catch (ArgumentException)
		{
		}
		if (SettingsData._loadFromCloudHandler != null)
		{
			SettingsData._loadFromCloudHandler(true);
			SettingsData._loadFromCloudHandler = null;
		}
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00008D52 File Offset: 0x00006F52
	public static void Reset()
	{
		SettingsData._data = new SettingsData();
		SettingsData.Save();
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00008D63 File Offset: 0x00006F63
	public static void ApplySettings()
	{
		if (SettingsData.OnSettingsAppliedEvent != null)
		{
			SettingsData.OnSettingsAppliedEvent();
		}
		SettingsData.Save();
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00079008 File Offset: 0x00077208
	public static void ApplySettingsOnStartup()
	{
		if (Screen.width < 320 || Screen.height < 240)
		{
			SettingsData.Data.screenWidth = 640;
			SettingsData.Data.screenHeight = 480;
			SettingsData.Data.fullScreen = false;
			Screen.SetResolution(SettingsData.Data.screenWidth, SettingsData.Data.screenHeight, SettingsData.Data.fullScreen);
		}
		QualitySettings.vSyncCount = SettingsData.Data.vSyncCount;
		AudioManager.masterVolume = SettingsData.Data.masterVolume;
		AudioManager.sfxOptionsVolume = SettingsData.Data.sFXVolume;
		AudioManager.bgmOptionsVolume = SettingsData.Data.musicVolume;
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x00008D7E File Offset: 0x00006F7E
	public static bool hasKey()
	{
		return PlayerPrefs.HasKey("cuphead_settings_data_v1");
	}

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x0600097D RID: 2429 RVA: 0x000790BC File Offset: 0x000772BC
	// (remove) Token: 0x0600097E RID: 2430 RVA: 0x000790F0 File Offset: 0x000772F0
	public static event Action OnSettingsAppliedEvent;

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x0600097F RID: 2431 RVA: 0x00008D8A File Offset: 0x00006F8A
	public bool vintageAudioEnabled
	{
		get
		{
			return PlayerData.inGame && PlayerData.Data.vintageAudioEnabled;
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06000980 RID: 2432 RVA: 0x00008DA2 File Offset: 0x00006FA2
	public BlurGamma.Filter filter
	{
		get
		{
			if (!PlayerData.inGame)
			{
				return BlurGamma.Filter.None;
			}
			return PlayerData.Data.filter;
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x06000981 RID: 2433 RVA: 0x00008DBA File Offset: 0x00006FBA
	// (set) Token: 0x06000982 RID: 2434 RVA: 0x00008DC8 File Offset: 0x00006FC8
	public float Brightness
	{
		get
		{
			this.ClampBrightness();
			return this.brightness;
		}
		set
		{
			this.brightness = value;
			this.ClampBrightness();
		}
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x00008DD7 File Offset: 0x00006FD7
	public void SetCameraEffectDefaults()
	{
		this.chromaticAberrationEffect = true;
		this.noiseEffect = true;
		this.subtleBlurEffect = true;
		this.brightness = 0f;
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x00008DF9 File Offset: 0x00006FF9
	public void ClampBrightness()
	{
		if (this.brightness < -1f)
		{
			this.brightness = -1f;
		}
		if (this.brightness > 1f)
		{
			this.brightness = 1f;
		}
	}

	// Token: 0x0400071F RID: 1823
	public const string KEY = "cuphead_settings_data_v1";

	// Token: 0x04000720 RID: 1824
	public static SettingsData.SettingsDataLoadFromCloudHandler _loadFromCloudHandler;

	// Token: 0x04000721 RID: 1825
	public static SettingsData _data;

	// Token: 0x04000723 RID: 1827
	public bool hasBootedUpGame;

	// Token: 0x04000724 RID: 1828
	public float overscan;

	// Token: 0x04000725 RID: 1829
	public float chromaticAberration;

	// Token: 0x04000726 RID: 1830
	public int screenWidth;

	// Token: 0x04000727 RID: 1831
	public int screenHeight;

	// Token: 0x04000728 RID: 1832
	public int vSyncCount;

	// Token: 0x04000729 RID: 1833
	public bool fullScreen;

	// Token: 0x0400072A RID: 1834
	public bool forceOriginalTitleScreen;

	// Token: 0x0400072B RID: 1835
	public float masterVolume;

	// Token: 0x0400072C RID: 1836
	public float sFXVolume;

	// Token: 0x0400072D RID: 1837
	public float musicVolume;

	// Token: 0x0400072E RID: 1838
	public static bool originalAudioValuesInitialized;

	// Token: 0x0400072F RID: 1839
	public static float originalMasterVolume;

	// Token: 0x04000730 RID: 1840
	public static float originalsFXVolume;

	// Token: 0x04000731 RID: 1841
	public static float originalMusicVolume;

	// Token: 0x04000732 RID: 1842
	public bool canVibrate = true;

	// Token: 0x04000733 RID: 1843
	public bool rotateControlsWithCamera;

	// Token: 0x04000734 RID: 1844
	public int language = -1;

	// Token: 0x04000735 RID: 1845
	public bool chromaticAberrationEffect;

	// Token: 0x04000736 RID: 1846
	public bool noiseEffect;

	// Token: 0x04000737 RID: 1847
	public bool subtleBlurEffect;

	// Token: 0x04000738 RID: 1848
	[SerializeField]
	public float brightness;

	// Token: 0x04000739 RID: 1849
	[CompilerGenerated]
	private static LoadCloudDataHandler <>f__mg$cache0;

	// Token: 0x0400073A RID: 1850
	[CompilerGenerated]
	private static SaveCloudDataHandler <>f__mg$cache1;

	// Token: 0x02000932 RID: 2354
	// (Invoke) Token: 0x06005439 RID: 21561
	public delegate void SettingsDataLoadFromCloudHandler(bool success);
}
