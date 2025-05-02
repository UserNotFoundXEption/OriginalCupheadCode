using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Steamworks;
using UnityEngine;

// Token: 0x020004ED RID: 1261
public class OnlineInterfaceSteam : OnlineInterface
{
	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x0600340B RID: 13323 RVA: 0x000F5B84 File Offset: 0x000F3D84
	public string SavePath
	{
		get
		{
			if (Application.platform == 7 || Application.platform == 2)
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Cuphead\\");
			}
			if (Application.platform == null || Application.platform == 1)
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Application Support/unity.Studio MDHR.Cuphead/Cuphead/");
			}
			return string.Empty;
		}
	}

	// Token: 0x14000072 RID: 114
	// (add) Token: 0x0600340C RID: 13324 RVA: 0x000F5BE4 File Offset: 0x000F3DE4
	// (remove) Token: 0x0600340D RID: 13325 RVA: 0x000F5C1C File Offset: 0x000F3E1C
	public event SignInEventHandler OnUserSignedIn;

	// Token: 0x14000073 RID: 115
	// (add) Token: 0x0600340E RID: 13326 RVA: 0x000F5C54 File Offset: 0x000F3E54
	// (remove) Token: 0x0600340F RID: 13327 RVA: 0x000F5C8C File Offset: 0x000F3E8C
	public event SignOutEventHandler OnUserSignedOut;

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06003410 RID: 13328 RVA: 0x0002AD7B File Offset: 0x00028F7B
	public OnlineUser MainUser
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06003411 RID: 13329 RVA: 0x0002AD7E File Offset: 0x00028F7E
	public OnlineUser SecondaryUser
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x06003412 RID: 13330 RVA: 0x0002AD81 File Offset: 0x00028F81
	public bool CloudStorageInitialized
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x06003413 RID: 13331 RVA: 0x0002AD84 File Offset: 0x00028F84
	public bool SupportsMultipleUsers
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06003414 RID: 13332 RVA: 0x0002AD87 File Offset: 0x00028F87
	public bool SupportsUserSignIn
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003415 RID: 13333 RVA: 0x000F5CC4 File Offset: 0x000F3EC4
	public void Init()
	{
		this.steamManager = new GameObject("SteamManager").AddComponent<SteamManager>();
		this.steamManager.transform.SetParent(Cuphead.Current.transform);
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.RequestCurrentStats();
	}

	// Token: 0x06003416 RID: 13334 RVA: 0x0002AD8A File Offset: 0x00028F8A
	public void Reset()
	{
	}

	// Token: 0x06003417 RID: 13335 RVA: 0x0002AD8C File Offset: 0x00028F8C
	public void SignInUser(bool silent, PlayerId player, ulong controllerId)
	{
		this.OnUserSignedIn(null);
	}

	// Token: 0x06003418 RID: 13336 RVA: 0x0002AD9A File Offset: 0x00028F9A
	public void SwitchUser(PlayerId player, ulong controllerId)
	{
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x0002AD9C File Offset: 0x00028F9C
	public OnlineUser GetUserForController(ulong id)
	{
		return null;
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x0002AD9F File Offset: 0x00028F9F
	public List<ulong> GetControllersForUser(PlayerId player)
	{
		return null;
	}

	// Token: 0x0600341B RID: 13339 RVA: 0x0002ADA2 File Offset: 0x00028FA2
	public bool IsUserSignedIn(PlayerId player)
	{
		return false;
	}

	// Token: 0x0600341C RID: 13340 RVA: 0x0002ADA5 File Offset: 0x00028FA5
	public OnlineUser GetUser(PlayerId player)
	{
		return null;
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x0002ADA8 File Offset: 0x00028FA8
	public void SetUser(PlayerId player, OnlineUser user)
	{
	}

	// Token: 0x0600341E RID: 13342 RVA: 0x0002ADAA File Offset: 0x00028FAA
	public Texture2D GetProfilePic(PlayerId player)
	{
		return null;
	}

	// Token: 0x0600341F RID: 13343 RVA: 0x0002ADAD File Offset: 0x00028FAD
	public void GetAchievement(PlayerId player, string id, AchievementEventHandler achievementRetrievedHandler)
	{
	}

	// Token: 0x06003420 RID: 13344 RVA: 0x000F5D14 File Offset: 0x000F3F14
	public void UnlockAchievement(PlayerId player, string id)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		bool flag;
		SteamUserStats.GetAchievement(id, ref flag);
		if (!flag)
		{
			SteamUserStats.SetAchievement(id);
			SteamUserStats.StoreStats();
		}
	}

	// Token: 0x06003421 RID: 13345 RVA: 0x0002ADAF File Offset: 0x00028FAF
	public void SyncAchievementsAndStats()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.StoreStats();
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x0002ADC2 File Offset: 0x00028FC2
	public void SetStat(PlayerId player, string id, int value)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.SetStat(id, value);
	}

	// Token: 0x06003423 RID: 13347 RVA: 0x0002ADD7 File Offset: 0x00028FD7
	public void SetStat(PlayerId player, string id, float value)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.SetStat(id, value);
	}

	// Token: 0x06003424 RID: 13348 RVA: 0x0002ADEC File Offset: 0x00028FEC
	public void SetStat(PlayerId player, string id, string value)
	{
	}

	// Token: 0x06003425 RID: 13349 RVA: 0x000F5D48 File Offset: 0x000F3F48
	public void IncrementStat(PlayerId player, string id, int value)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		int num;
		SteamUserStats.GetStat(id, ref num);
		int num2 = num + value;
		SteamUserStats.SetStat(id, num2);
		if (id == "Parries" && (num2 == 20 || num2 == 100))
		{
			SteamUserStats.StoreStats();
		}
	}

	// Token: 0x06003426 RID: 13350 RVA: 0x0002ADEE File Offset: 0x00028FEE
	public void SetRichPresence(PlayerId player, string id, bool active)
	{
	}

	// Token: 0x06003427 RID: 13351 RVA: 0x0002ADF0 File Offset: 0x00028FF0
	public void SetRichPresenceActive(PlayerId player, bool active)
	{
	}

	// Token: 0x06003428 RID: 13352 RVA: 0x0002ADF2 File Offset: 0x00028FF2
	public void InitializeCloudStorage(PlayerId player, InitializeCloudStoreHandler handler)
	{
		handler(true);
	}

	// Token: 0x06003429 RID: 13353 RVA: 0x0002ADFB File Offset: 0x00028FFB
	public void UninitializeCloudStorage()
	{
	}

	// Token: 0x0600342A RID: 13354 RVA: 0x000F5D9C File Offset: 0x000F3F9C
	public void SaveCloudData(IDictionary<string, string> data, SaveCloudDataHandler handler)
	{
		string savePath = this.SavePath;
		if (!Directory.Exists(savePath))
		{
			Directory.CreateDirectory(savePath);
		}
		foreach (string text in data.Keys)
		{
			try
			{
				TextWriter textWriter = new StreamWriter(Path.Combine(savePath, text + ".sav"));
				textWriter.Write(data[text]);
				textWriter.Close();
			}
			catch
			{
				Cuphead.Current.StartCoroutine(this.saveFailed_cr(handler));
				return;
			}
		}
		handler(true);
	}

	// Token: 0x0600342B RID: 13355 RVA: 0x000F5E64 File Offset: 0x000F4064
	public IEnumerator saveFailed_cr(SaveCloudDataHandler handler)
	{
		yield return new WaitForSeconds(0.25f);
		handler(false);
		yield break;
	}

	// Token: 0x0600342C RID: 13356 RVA: 0x000F5E80 File Offset: 0x000F4080
	public void LoadCloudData(string[] keys, LoadCloudDataHandler handler)
	{
		string[] array = new string[keys.Length];
		string savePath = this.SavePath;
		for (int i = 0; i < array.Length; i++)
		{
			string path = Path.Combine(savePath, keys[i] + ".sav");
			if (File.Exists(path))
			{
				try
				{
					TextReader textReader = new StreamReader(Path.Combine(savePath, keys[i] + ".sav"));
					array[i] = textReader.ReadToEnd();
					textReader.Close();
				}
				catch
				{
					handler(array, CloudLoadResult.Failed);
				}
			}
			else
			{
				handler(array, CloudLoadResult.NoData);
			}
		}
		handler(array, CloudLoadResult.Success);
	}

	// Token: 0x0600342D RID: 13357 RVA: 0x0002ADFD File Offset: 0x00028FFD
	public void UpdateControllerMapping()
	{
	}

	// Token: 0x0600342E RID: 13358 RVA: 0x0002ADFF File Offset: 0x00028FFF
	public bool ControllerMappingChanged()
	{
		return false;
	}

	// Token: 0x04002AF4 RID: 10996
	public SteamManager steamManager;
}
