using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020000C8 RID: 200
[Serializable]
public class PlayerData
{
	// Token: 0x06000917 RID: 2327 RVA: 0x00077250 File Offset: 0x00075450
	public PlayerData()
	{
		if (string.IsNullOrEmpty(PlayerData.emptyDialoguerState))
		{
			Dialoguer.Initialize();
			PlayerData.emptyDialoguerState = Dialoguer.GetGlobalVariablesState();
		}
		this.dialoguerState = PlayerData.emptyDialoguerState;
	}

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x06000918 RID: 2328 RVA: 0x00008948 File Offset: 0x00006B48
	// (set) Token: 0x06000919 RID: 2329 RVA: 0x0000895E File Offset: 0x00006B5E
	public static int CurrentSaveFileIndex
	{
		get
		{
			return Mathf.Clamp(PlayerData._CurrentSaveFileIndex, 0, PlayerData.SAVE_FILE_KEYS.Length - 1);
		}
		set
		{
			PlayerData._CurrentSaveFileIndex = Mathf.Clamp(value, 0, PlayerData.SAVE_FILE_KEYS.Length - 1);
			PlayerData.Data.LoadDialogueVariables();
		}
	}

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x0600091A RID: 2330 RVA: 0x0000897F File Offset: 0x00006B7F
	// (set) Token: 0x0600091B RID: 2331 RVA: 0x00008986 File Offset: 0x00006B86
	public static bool Initialized
	{
		get
		{
			return PlayerData._initialized;
		}
		set
		{
			PlayerData._initialized = value;
		}
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x0600091C RID: 2332 RVA: 0x0000898E File Offset: 0x00006B8E
	public static PlayerData Data
	{
		get
		{
			return PlayerData.GetDataForSlot(PlayerData.CurrentSaveFileIndex);
		}
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x000772EC File Offset: 0x000754EC
	public static PlayerData GetDataForSlot(int slot)
	{
		if (PlayerData._saveFiles == null || PlayerData._saveFiles.Length != PlayerData.SAVE_FILE_KEYS.Length)
		{
			PlayerData._saveFiles = new PlayerData[PlayerData.SAVE_FILE_KEYS.Length];
			for (int i = 0; i < PlayerData.SAVE_FILE_KEYS.Length; i++)
			{
				PlayerData._saveFiles[i] = new PlayerData();
			}
		}
		if (PlayerData._saveFiles[slot].curseCharmPuzzleOrder == null || PlayerData._saveFiles[slot].curseCharmPuzzleOrder.Length == 0)
		{
			PlayerData._saveFiles[slot].CreateCursePuzzleVariables();
		}
		return PlayerData._saveFiles[slot];
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x00077384 File Offset: 0x00075584
	public void CreateCursePuzzleVariables()
	{
		List<int> list = new List<int>
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7
		};
		this.curseCharmPuzzleOrder = new int[3];
		for (int i = 0; i < this.curseCharmPuzzleOrder.Length; i++)
		{
			int index = Random.Range(0, list.Count);
			this.curseCharmPuzzleOrder[i] = list[index];
			list.Remove(list[index]);
		}
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x0000899A File Offset: 0x00006B9A
	public static void ClearSlot(int slot)
	{
		if (PlayerData._saveFiles == null || PlayerData._saveFiles.Length != PlayerData.SAVE_FILE_KEYS.Length)
		{
			return;
		}
		PlayerData.ResetDialoguer();
		PlayerData._saveFiles[slot] = new PlayerData();
		PlayerData.Save(slot);
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x00077424 File Offset: 0x00075624
	public static void Init(PlayerData.PlayerDataInitHandler handler)
	{
		PlayerData._saveFiles = new PlayerData[PlayerData.SAVE_FILE_KEYS.Length];
		for (int i = 0; i < PlayerData.SAVE_FILE_KEYS.Length; i++)
		{
			PlayerData._saveFiles[i] = new PlayerData();
		}
		PlayerData._playerDatatInitHandler = handler;
		OnlineInterface @interface = OnlineManager.Instance.Interface;
		PlayerId player = PlayerId.PlayerOne;
		if (PlayerData.<>f__mg$cache0 == null)
		{
			PlayerData.<>f__mg$cache0 = new InitializeCloudStoreHandler(PlayerData.OnCloudStorageInitialized);
		}
		@interface.InitializeCloudStorage(player, PlayerData.<>f__mg$cache0);
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x000089D1 File Offset: 0x00006BD1
	public void LoadDialogueVariables()
	{
		Dialoguer.Initialize();
		Dialoguer.EndDialogue();
		Dialoguer.SetGlobalVariablesState(this.dialoguerState);
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0007749C File Offset: 0x0007569C
	public static void OnCloudStorageInitialized(bool success)
	{
		if (!success)
		{
			PlayerData._playerDatatInitHandler(false);
			return;
		}
		OnlineInterface @interface = OnlineManager.Instance.Interface;
		string[] save_FILE_KEYS = PlayerData.SAVE_FILE_KEYS;
		if (PlayerData.<>f__mg$cache1 == null)
		{
			PlayerData.<>f__mg$cache1 = new LoadCloudDataHandler(PlayerData.OnLoaded);
		}
		@interface.LoadCloudData(save_FILE_KEYS, PlayerData.<>f__mg$cache1);
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x000774EC File Offset: 0x000756EC
	public static void OnLoaded(string[] data, CloudLoadResult result)
	{
		if (result == CloudLoadResult.Failed)
		{
			Debug.LogError("[PlayerData] LOAD FAILED", null);
			OnlineInterface @interface = OnlineManager.Instance.Interface;
			string[] save_FILE_KEYS = PlayerData.SAVE_FILE_KEYS;
			if (PlayerData.<>f__mg$cache2 == null)
			{
				PlayerData.<>f__mg$cache2 = new LoadCloudDataHandler(PlayerData.OnLoaded);
			}
			@interface.LoadCloudData(save_FILE_KEYS, PlayerData.<>f__mg$cache2);
			return;
		}
		if (result == CloudLoadResult.NoData)
		{
			Debug.LogError("[PlayerData] No data. Saving default data to cloud", null);
			PlayerData.SaveAll();
			return;
		}
		bool flag = false;
		for (int i = 0; i < PlayerData.SAVE_FILE_KEYS.Length; i++)
		{
			if (data[i] != null)
			{
				PlayerData playerData = null;
				try
				{
					playerData = JsonUtility.FromJson<PlayerData>(data[i]);
					if (playerData != null && !playerData.coinManager.hasMigratedCoins)
					{
						playerData = PlayerData.Migrate(playerData);
						flag = true;
					}
				}
				catch (ArgumentException ex)
				{
					Debug.LogError("Unable to parse player data. " + ex.StackTrace, null);
				}
				if (playerData == null)
				{
					Debug.LogError("[PlayerData] Data could not be unserialized for key: " + PlayerData.SAVE_FILE_KEYS[i], null);
				}
				else
				{
					PlayerData._saveFiles[i] = playerData;
				}
			}
		}
		PlayerData.Initialized = true;
		if (flag)
		{
			PlayerData.SaveAll();
		}
		if (PlayerData._playerDatatInitHandler != null)
		{
			PlayerData._playerDatatInitHandler(true);
			PlayerData._playerDatatInitHandler = null;
		}
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x00077628 File Offset: 0x00075828
	public static PlayerData Migrate(PlayerData playerData)
	{
		for (int i = 0; i < playerData.coinManager.LevelsAndCoins.Count; i++)
		{
			PlayerData.PlayerCoinManager.LevelAndCoins levelAndCoins = new PlayerData.PlayerCoinManager.LevelAndCoins();
			levelAndCoins.level = playerData.coinManager.LevelsAndCoins[i].level;
			playerData.coinManager.LevelsAndCoins[i] = levelAndCoins;
		}
		for (int j = 0; j < playerData.coinManager.coins.Count; j++)
		{
			string coinID = playerData.coinManager.coins[j].coinID;
			bool flag = false;
			for (int k = 0; k < PlayerData.platformingCoinIDs.Length; k++)
			{
				List<PlayerData.PlayerCoinManager.LevelAndCoins> levelsAndCoins = playerData.coinManager.LevelsAndCoins;
				int index = -1;
				for (int l = 0; l < levelsAndCoins.Count; l++)
				{
					if (levelsAndCoins[l].level == PlayerData.platformingCoinIDs[k].levelId)
					{
						index = l;
					}
				}
				for (int m = 0; m < PlayerData.platformingCoinIDs[k].coinIds.Length; m++)
				{
					string coinID2 = PlayerData.platformingCoinIDs[k].coinIds[m][0];
					for (int n = 0; n < PlayerData.platformingCoinIDs[k].coinIds[m].Length; n++)
					{
						if (coinID == PlayerData.platformingCoinIDs[k].coinIds[m][n])
						{
							playerData.coinManager.coins[j].coinID = coinID2;
							flag = true;
							switch (m)
							{
							case 0:
								levelsAndCoins[index].Coin1Collected = true;
								break;
							case 1:
								levelsAndCoins[index].Coin2Collected = true;
								break;
							case 2:
								levelsAndCoins[index].Coin3Collected = true;
								break;
							case 3:
								levelsAndCoins[index].Coin4Collected = true;
								break;
							case 4:
								levelsAndCoins[index].Coin5Collected = true;
								break;
							}
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		playerData.coinManager.hasMigratedCoins = true;
		return playerData;
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x000089E8 File Offset: 0x00006BE8
	public static string GetSaveFileKey(int fileIndex)
	{
		return PlayerData.SAVE_FILE_KEYS[fileIndex];
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x0007788C File Offset: 0x00075A8C
	public static void Save(int fileIndex)
	{
		PlayerData._saveFiles[fileIndex].dialoguerState = Dialoguer.GetGlobalVariablesState();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary[PlayerData.SAVE_FILE_KEYS[fileIndex]] = JsonUtility.ToJson(PlayerData._saveFiles[fileIndex]);
		OnlineInterface @interface = OnlineManager.Instance.Interface;
		IDictionary<string, string> data = dictionary;
		if (PlayerData.<>f__mg$cache3 == null)
		{
			PlayerData.<>f__mg$cache3 = new SaveCloudDataHandler(PlayerData.OnSaved);
		}
		@interface.SaveCloudData(data, PlayerData.<>f__mg$cache3);
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x000778F8 File Offset: 0x00075AF8
	public static void SaveAll()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int i = 0; i < PlayerData.SAVE_FILE_KEYS.Length; i++)
		{
			dictionary[PlayerData.SAVE_FILE_KEYS[i]] = JsonUtility.ToJson(PlayerData._saveFiles[i]);
		}
		OnlineInterface @interface = OnlineManager.Instance.Interface;
		IDictionary<string, string> data = dictionary;
		if (PlayerData.<>f__mg$cache4 == null)
		{
			PlayerData.<>f__mg$cache4 = new SaveCloudDataHandler(PlayerData.OnSavedAll);
		}
		@interface.SaveCloudData(data, PlayerData.<>f__mg$cache4);
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x000089F1 File Offset: 0x00006BF1
	public static void OnSaved(bool success)
	{
		if (!success)
		{
			Debug.LogError("[PlayerData] SAVE FAILED. Retrying...", null);
			PlayerData.Save(PlayerData.CurrentSaveFileIndex);
		}
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x00008A13 File Offset: 0x00006C13
	public static void OnSavedAll(bool success)
	{
		if (success)
		{
			PlayerData.Initialized = true;
			if (PlayerData._playerDatatInitHandler != null)
			{
				PlayerData._playerDatatInitHandler(true);
				PlayerData._playerDatatInitHandler = null;
			}
		}
		else
		{
			Debug.LogError("[PlayerData] SAVE FAILED. Retrying...", null);
			PlayerData.SaveAll();
		}
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x00008A51 File Offset: 0x00006C51
	public static void SaveCurrentFile()
	{
		PlayerData.Save(PlayerData.CurrentSaveFileIndex);
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00008A5D File Offset: 0x00006C5D
	public static void ResetDialoguer()
	{
		Dialoguer.SetGlobalVariablesState(PlayerData.emptyDialoguerState);
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x0007796C File Offset: 0x00075B6C
	public static void ResetAll()
	{
		for (int i = 0; i < PlayerData.SAVE_FILE_KEYS.Length; i++)
		{
			PlayerData.ClearSlot(i);
		}
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00008A69 File Offset: 0x00006C69
	public static void Unload()
	{
		PlayerData._saveFiles = null;
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x0600092E RID: 2350 RVA: 0x00008A71 File Offset: 0x00006C71
	public PlayerData.PlayerLoadouts Loadouts
	{
		get
		{
			return this.loadouts;
		}
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x0600092F RID: 2351 RVA: 0x00008A79 File Offset: 0x00006C79
	// (set) Token: 0x06000930 RID: 2352 RVA: 0x00008A81 File Offset: 0x00006C81
	public bool IsHardModeAvailable
	{
		get
		{
			return this._isHardModeAvailable;
		}
		set
		{
			this._isHardModeAvailable = value;
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06000931 RID: 2353 RVA: 0x00008A8A File Offset: 0x00006C8A
	// (set) Token: 0x06000932 RID: 2354 RVA: 0x00008A92 File Offset: 0x00006C92
	public bool IsHardModeAvailableDLC
	{
		get
		{
			return this._isHardModeAvailableDLC;
		}
		set
		{
			this._isHardModeAvailableDLC = value;
		}
	}

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000933 RID: 2355 RVA: 0x00008A9B File Offset: 0x00006C9B
	// (set) Token: 0x06000934 RID: 2356 RVA: 0x00008AA3 File Offset: 0x00006CA3
	public bool IsTutorialCompleted
	{
		get
		{
			return this._isTutorialCompleted;
		}
		set
		{
			this._isTutorialCompleted = value;
		}
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06000935 RID: 2357 RVA: 0x00008AAC File Offset: 0x00006CAC
	// (set) Token: 0x06000936 RID: 2358 RVA: 0x00008AB4 File Offset: 0x00006CB4
	public bool IsFlyingTutorialCompleted
	{
		get
		{
			return this._isFlyingTutorialCompleted;
		}
		set
		{
			this._isFlyingTutorialCompleted = value;
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06000937 RID: 2359 RVA: 0x00008ABD File Offset: 0x00006CBD
	// (set) Token: 0x06000938 RID: 2360 RVA: 0x00008AC5 File Offset: 0x00006CC5
	public bool IsChaliceTutorialCompleted
	{
		get
		{
			return this._isChaliceTutorialCompleted;
		}
		set
		{
			this._isChaliceTutorialCompleted = value;
		}
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x00077998 File Offset: 0x00075B98
	public bool IsUnlocked(PlayerId player, Weapon value)
	{
		if (player == PlayerId.PlayerOne)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value);
		}
		if (player == PlayerId.PlayerTwo)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value);
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return false;
		}
		return this.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value) || this.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value);
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x00077A24 File Offset: 0x00075C24
	public bool IsUnlocked(PlayerId player, Super value)
	{
		if (player == PlayerId.PlayerOne)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value);
		}
		if (player == PlayerId.PlayerTwo)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value);
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return false;
		}
		return this.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value) || this.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value);
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00077AB0 File Offset: 0x00075CB0
	public bool IsUnlocked(PlayerId player, Charm value)
	{
		if (player == PlayerId.PlayerOne)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value);
		}
		if (player == PlayerId.PlayerTwo)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value);
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return false;
		}
		return this.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value) || this.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value);
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x00077B3C File Offset: 0x00075D3C
	public bool HasNewPurchase(PlayerId player)
	{
		if (player == PlayerId.PlayerOne)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerOne).newPurchase;
		}
		if (player == PlayerId.PlayerTwo)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerTwo).newPurchase;
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return false;
		}
		return this.inventories.GetPlayer(PlayerId.PlayerOne).newPurchase || this.inventories.GetPlayer(PlayerId.PlayerTwo).newPurchase;
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x00077BC4 File Offset: 0x00075DC4
	public void ResetHasNewPurchase(PlayerId player)
	{
		if (player == PlayerId.PlayerOne)
		{
			this.inventories.GetPlayer(PlayerId.PlayerOne).newPurchase = false;
			return;
		}
		if (player == PlayerId.PlayerTwo)
		{
			this.inventories.GetPlayer(PlayerId.PlayerTwo).newPurchase = false;
			return;
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return;
		}
		this.inventories.GetPlayer(PlayerId.PlayerOne).newPurchase = false;
		this.inventories.GetPlayer(PlayerId.PlayerTwo).newPurchase = false;
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x00008ACE File Offset: 0x00006CCE
	public bool Buy(PlayerId player, Weapon value)
	{
		return this.inventories.GetPlayer(player).Buy(value);
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x00008AE2 File Offset: 0x00006CE2
	public bool Buy(PlayerId player, Super value)
	{
		return this.inventories.GetPlayer(player).Buy(value);
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x00008AF6 File Offset: 0x00006CF6
	public bool Buy(PlayerId player, Charm value)
	{
		return this.inventories.GetPlayer(player).Buy(value);
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x00008B0A File Offset: 0x00006D0A
	public void Gift(PlayerId player, Weapon value)
	{
		this.inventories.GetPlayer(player)._weapons.Add(value);
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x00008B23 File Offset: 0x00006D23
	public void Gift(PlayerId player, Super value)
	{
		this.inventories.GetPlayer(player)._supers.Add(value);
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x00008B3C File Offset: 0x00006D3C
	public void Gift(PlayerId player, Charm value)
	{
		this.inventories.GetPlayer(player)._charms.Add(value);
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x00008B55 File Offset: 0x00006D55
	public int NumWeapons(PlayerId player)
	{
		return this.inventories.GetPlayer(player)._weapons.Count;
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00008B6D File Offset: 0x00006D6D
	public int NumCharms(PlayerId player)
	{
		return this.inventories.GetPlayer(player)._charms.Count;
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x00008B85 File Offset: 0x00006D85
	public int NumSupers(PlayerId player)
	{
		return this.inventories.GetPlayer(player)._supers.Count;
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00008B9D File Offset: 0x00006D9D
	public int GetCurrency(PlayerId player)
	{
		return this.inventories.GetPlayer(player).money;
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x00008BB0 File Offset: 0x00006DB0
	public void AddCurrency(PlayerId player, int value)
	{
		this.inventories.GetPlayer(player).money += value;
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x00008BCB File Offset: 0x00006DCB
	public void ResetLevelCoinManager()
	{
		this.levelCoinManager = new PlayerData.PlayerCoinManager();
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x00008BD8 File Offset: 0x00006DD8
	public bool GetCoinCollected(LevelCoin coin)
	{
		return this.coinManager.GetCoinCollected(coin);
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x00008BE6 File Offset: 0x00006DE6
	public void SetLevelCoinCollected(LevelCoin coin, bool collected, PlayerId player)
	{
		this.levelCoinManager.SetCoinValue(coin, collected, player);
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x0600094C RID: 2380 RVA: 0x00008BF6 File Offset: 0x00006DF6
	public int NumCoinsCollected
	{
		get
		{
			return this.coinManager.NumCoinsCollected();
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x0600094D RID: 2381 RVA: 0x00008C03 File Offset: 0x00006E03
	public int NumCoinsCollectedMainGame
	{
		get
		{
			return this.coinManager.NumCoinsCollected(false);
		}
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x00077C44 File Offset: 0x00075E44
	public int GetNumCoinsCollectedInLevel(Levels level)
	{
		List<PlayerData.PlayerCoinManager.LevelAndCoins> levelsAndCoins = this.coinManager.LevelsAndCoins;
		for (int i = 0; i < levelsAndCoins.Count; i++)
		{
			if (levelsAndCoins[i].level == level)
			{
				int num = 0;
				if (levelsAndCoins[i].Coin1Collected)
				{
					num++;
				}
				if (levelsAndCoins[i].Coin2Collected)
				{
					num++;
				}
				if (levelsAndCoins[i].Coin3Collected)
				{
					num++;
				}
				if (levelsAndCoins[i].Coin4Collected)
				{
					num++;
				}
				if (levelsAndCoins[i].Coin5Collected)
				{
					num++;
				}
				return num;
			}
		}
		return 0;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x00077CF4 File Offset: 0x00075EF4
	public void ApplyLevelCoins()
	{
		foreach (PlayerData.PlayerCoinProperties playerCoinProperties in this.levelCoinManager.coins)
		{
			this.coinManager.SetCoinValue(playerCoinProperties.coinID, playerCoinProperties.collected, playerCoinProperties.player);
			if (playerCoinProperties.collected)
			{
				PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1);
				PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1);
			}
		}
		this.levelCoinManager = new PlayerData.PlayerCoinManager();
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06000950 RID: 2384 RVA: 0x00008C11 File Offset: 0x00006E11
	public PlayerData.MapData CurrentMapData
	{
		get
		{
			return this.mapDataManager.GetCurrentMapData();
		}
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x00008C1E File Offset: 0x00006E1E
	public PlayerData.MapData GetMapData(Scenes map)
	{
		return this.mapDataManager.GetMapData(map);
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06000952 RID: 2386 RVA: 0x00008C2C File Offset: 0x00006E2C
	// (set) Token: 0x06000953 RID: 2387 RVA: 0x00008C39 File Offset: 0x00006E39
	public Scenes CurrentMap
	{
		get
		{
			return this.mapDataManager.currentMap;
		}
		set
		{
			this.mapDataManager.currentMap = value;
		}
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x00008C47 File Offset: 0x00006E47
	public PlayerData.PlayerLevelDataObject GetLevelData(Levels levelID)
	{
		return this.levelDataManager.GetLevelData(levelID);
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x00077D9C File Offset: 0x00075F9C
	public int CountLevelsCompleted(Levels[] levels)
	{
		int num = 0;
		foreach (Levels levelID in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (levelData.completed)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x00077DE0 File Offset: 0x00075FE0
	public bool CheckLevelsCompleted(Levels[] levels)
	{
		foreach (Levels levelID in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (!levelData.completed)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x00077E20 File Offset: 0x00076020
	public bool CheckLevelCompleted(Levels level)
	{
		PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(level);
		return levelData.completed;
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x00077E44 File Offset: 0x00076044
	public int CountLevelsHaveMinGrade(Levels[] levels, LevelScoringData.Grade minGrade)
	{
		int num = 0;
		foreach (Levels levelID in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (levelData.completed && levelData.grade >= minGrade)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x00077E94 File Offset: 0x00076094
	public bool CheckLevelsHaveMinGrade(Levels[] levels, LevelScoringData.Grade minGrade)
	{
		foreach (Levels levelID in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (!levelData.completed || levelData.grade < minGrade)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x00077EE0 File Offset: 0x000760E0
	public int CountLevelsHaveMinDifficulty(Levels[] levels, Level.Mode minDifficulty)
	{
		int num = 0;
		foreach (Levels levelID in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (levelData.completed && levelData.difficultyBeaten >= minDifficulty)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x00077F30 File Offset: 0x00076130
	public bool CheckLevelsHaveMinDifficulty(Levels[] levels, Level.Mode minDifficulty)
	{
		foreach (Levels levelID in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (!levelData.completed || levelData.difficultyBeaten < minDifficulty)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x00077F7C File Offset: 0x0007617C
	public int CountLevelsChaliceCompleted(Levels[] levels, PlayerId playerId)
	{
		int num = 0;
		foreach (Levels levelID in levels)
		{
			if ((playerId == PlayerId.PlayerOne && this.GetLevelData(levelID).completedAsChaliceP1) || (playerId == PlayerId.PlayerTwo && this.GetLevelData(levelID).completedAsChaliceP2))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x00077FDC File Offset: 0x000761DC
	public static float CurseCharmValue(Levels level)
	{
		List<Levels> list = new List<Levels>(Level.world1BossLevels);
		if (Array.IndexOf<Levels>(Level.world1BossLevels, level) >= 0)
		{
			return 2f;
		}
		if (Array.IndexOf<Levels>(Level.world2BossLevels, level) >= 0)
		{
			return 2.5f;
		}
		if (Array.IndexOf<Levels>(Level.world3BossLevels, level) >= 0)
		{
			return 3f;
		}
		if (Array.IndexOf<Levels>(Level.world4MiniBossLevels, level) >= 0)
		{
			return 1f;
		}
		if (level == Levels.DicePalaceMain)
		{
			return 1f;
		}
		if (level == Levels.Devil)
		{
			return 4f;
		}
		if (Array.IndexOf<Levels>(Level.worldDLCBossLevels, level) >= 0)
		{
			return 3f;
		}
		if (level == Levels.Saltbaker)
		{
			return 4f;
		}
		return 0f;
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x000780A0 File Offset: 0x000762A0
	public int completionPercentageOnly_CalculateCurseCharmLevel(PlayerId playerId)
	{
		if (!this.GetLevelData(Levels.Graveyard).completed)
		{
			return -1;
		}
		Levels[] levels = new Levels[]
		{
			Levels.Veggies,
			Levels.Slime,
			Levels.FlyingBlimp,
			Levels.Flower,
			Levels.Frogs,
			Levels.Baroness,
			Levels.Clown,
			Levels.FlyingGenie,
			Levels.Dragon,
			Levels.FlyingBird,
			Levels.Bee,
			Levels.Pirate,
			Levels.SallyStagePlay,
			Levels.Mouse,
			Levels.Robot,
			Levels.FlyingMermaid,
			Levels.Train,
			Levels.DicePalaceBooze,
			Levels.DicePalaceChips,
			Levels.DicePalaceCigar,
			Levels.DicePalaceDomino,
			Levels.DicePalaceEightBall,
			Levels.DicePalaceFlyingHorse,
			Levels.DicePalaceFlyingMemory,
			Levels.DicePalaceRabbit,
			Levels.DicePalaceRoulette,
			Levels.DicePalaceMain,
			Levels.Devil,
			Levels.Airplane,
			Levels.RumRunners,
			Levels.OldMan,
			Levels.SnowCult,
			Levels.FlyingCowboy,
			Levels.Saltbaker
		};
		int num = this.CalculateCurseCharmAccumulatedValue(playerId, levels);
		int[] levelThreshold = WeaponProperties.CharmCurse.levelThreshold;
		for (int i = 0; i < levelThreshold.Length; i++)
		{
			if (num < levelThreshold[i])
			{
				return i - 1;
			}
		}
		return levelThreshold.Length - 1;
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0007810C File Offset: 0x0007630C
	public bool completionPercentageOnly_CurseCharmIsMaxLevel(PlayerId playerId)
	{
		int[] levelThreshold = WeaponProperties.CharmCurse.levelThreshold;
		return this.completionPercentageOnly_CalculateCurseCharmLevel(playerId) == levelThreshold.Length - 1;
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x00078130 File Offset: 0x00076330
	public int CalculateCurseCharmAccumulatedValue(PlayerId playerId, Levels[] levels)
	{
		float num = 0f;
		foreach (Levels levels2 in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levels2);
			if (playerId == PlayerId.PlayerOne && levelData.curseCharmP1)
			{
				num += PlayerData.CurseCharmValue(levels2);
			}
			else if (playerId == PlayerId.PlayerTwo && levelData.curseCharmP2)
			{
				num += PlayerData.CurseCharmValue(levels2);
			}
		}
		return (int)num;
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x000781A4 File Offset: 0x000763A4
	public float GetCompletionPercentage()
	{
		List<Levels> list = new List<Levels>();
		list.AddRange(Level.world1BossLevels);
		list.AddRange(Level.world2BossLevels);
		list.AddRange(Level.world3BossLevels);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		foreach (Levels levelID in list)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (levelData.completed)
			{
				num++;
				Level.Mode difficultyBeaten = levelData.difficultyBeaten;
				if (difficultyBeaten != Level.Mode.Normal)
				{
					if (difficultyBeaten == Level.Mode.Hard)
					{
						num2++;
						num6++;
					}
				}
				else
				{
					num2++;
				}
			}
		}
		foreach (Levels levelID2 in Level.platformingLevels)
		{
			PlayerData.PlayerLevelDataObject levelData2 = this.GetLevelData(levelID2);
			if (levelData2.completed)
			{
				num3++;
			}
		}
		int num9 = this.coinManager.NumCoinsCollected(false);
		int num10 = this.NumSupers(PlayerId.PlayerOne);
		PlayerData.PlayerLevelDataObject levelData3 = this.GetLevelData(Levels.DicePalaceMain);
		if (levelData3.completed)
		{
			num4++;
			if (levelData3.difficultyBeaten == Level.Mode.Hard)
			{
				num7++;
			}
		}
		PlayerData.PlayerLevelDataObject levelData4 = this.GetLevelData(Levels.Devil);
		if (levelData4.completed)
		{
			num5++;
			if (levelData4.difficultyBeaten == Level.Mode.Hard)
			{
				num8++;
			}
		}
		return (float)num * 1.5f + (float)num3 * 1.5f + (float)num9 * 0.5f + (float)num10 * 1.5f + (float)(num2 * 2) + (float)(num4 * 3) + (float)(num5 * 4) + (float)(num6 * 5) + (float)(num7 * 7) + (float)(num8 * 8);
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x0007838C File Offset: 0x0007658C
	public float GetCompletionPercentageDLC()
	{
		if (!DLCManager.DLCEnabled())
		{
			return 0f;
		}
		List<Levels> list = new List<Levels>();
		list.AddRange(Level.worldDLCBossLevels);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		foreach (Levels levelID in list)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (levelData.completed)
			{
				num++;
				Level.Mode difficultyBeaten = levelData.difficultyBeaten;
				if (difficultyBeaten != Level.Mode.Normal)
				{
					if (difficultyBeaten == Level.Mode.Hard)
					{
						num2++;
						num3++;
					}
				}
				else
				{
					num2++;
				}
			}
		}
		int num9 = this.coinManager.NumCoinsCollected(true);
		PlayerData.PlayerLevelDataObject levelData2 = this.GetLevelData(Levels.Saltbaker);
		if (levelData2.completed)
		{
			num4++;
			if (levelData2.difficultyBeaten == Level.Mode.Hard)
			{
				num5++;
			}
		}
		if (this.curseCharmPuzzleComplete)
		{
			num6++;
		}
		if (this.GetLevelData(Levels.Graveyard).completed)
		{
			num7++;
		}
		if (this.completionPercentageOnly_CurseCharmIsMaxLevel(PlayerId.PlayerOne) || this.completionPercentageOnly_CurseCharmIsMaxLevel(PlayerId.PlayerTwo))
		{
			num8++;
		}
		return (float)num * 3.5f + (float)num2 * 5f + (float)num3 * 4.5f + (float)num4 * 6f + (float)num5 * 6f + (float)num9 * 1f + (float)num6 * 1f + (float)num7 * 3f + (float)num8 * 3f;
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00078548 File Offset: 0x00076748
	public int DeathCount(PlayerId player)
	{
		if (player == PlayerId.PlayerOne)
		{
			return this.statictics.GetPlayer(PlayerId.PlayerOne).DeathCount();
		}
		if (player == PlayerId.PlayerTwo)
		{
			return this.statictics.GetPlayer(PlayerId.PlayerTwo).DeathCount();
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return 0;
		}
		return this.statictics.GetPlayer(PlayerId.PlayerOne).DeathCount() + this.statictics.GetPlayer(PlayerId.PlayerTwo).DeathCount();
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x000785C8 File Offset: 0x000767C8
	public void Die(PlayerId player)
	{
		if (player != PlayerId.PlayerOne)
		{
			if (player != PlayerId.PlayerTwo)
			{
				if (player != PlayerId.Any && player != PlayerId.None)
				{
				}
			}
			else
			{
				this.statictics.GetPlayer(PlayerId.PlayerTwo).Die();
			}
		}
		else
		{
			this.statictics.GetPlayer(PlayerId.PlayerOne).Die();
		}
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00078630 File Offset: 0x00076830
	public int GetNumParriesInRow(PlayerId player)
	{
		if (player == PlayerId.PlayerOne)
		{
			return this.statictics.GetPlayer(PlayerId.PlayerOne).numParriesInRow;
		}
		if (player == PlayerId.PlayerTwo)
		{
			return this.statictics.GetPlayer(PlayerId.PlayerTwo).numParriesInRow;
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return 0;
		}
		return Mathf.Max(this.statictics.GetPlayer(PlayerId.PlayerOne).numParriesInRow, this.statictics.GetPlayer(PlayerId.PlayerTwo).numParriesInRow);
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x000786B4 File Offset: 0x000768B4
	public void SetNumParriesInRow(PlayerId player, int numParriesInRow)
	{
		if (player != PlayerId.PlayerOne)
		{
			if (player != PlayerId.PlayerTwo)
			{
				if (player != PlayerId.Any && player != PlayerId.None)
				{
				}
			}
			else
			{
				this.statictics.GetPlayer(PlayerId.PlayerTwo).numParriesInRow = numParriesInRow;
			}
		}
		else
		{
			this.statictics.GetPlayer(PlayerId.PlayerOne).numParriesInRow = numParriesInRow;
		}
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00008C55 File Offset: 0x00006E55
	public void IncrementKingOfGamesCounter()
	{
		if (this.CountLevelsCompleted(Level.kingOfGamesLevels) == Level.kingOfGamesLevels.Length)
		{
			return;
		}
		this.chessBossAttemptCounter++;
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x00008C7D File Offset: 0x00006E7D
	public void ResetKingOfGamesCounter()
	{
		this.chessBossAttemptCounter = 0;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0007871C File Offset: 0x0007691C
	public bool TryActivateDjimmi()
	{
		if (this.DjimmiFreedCurrentRegion())
		{
			if (this.DjimmiActivatedCurrentRegion())
			{
				if (this.CurrentMap == Scenes.scene_map_world_DLC)
				{
					this.djimmiActivatedInfiniteWishDLC = false;
				}
				else
				{
					this.djimmiActivatedInfiniteWishBaseGame = false;
				}
				AudioManager.Play("sfx_worldmap_djimmi_deactivate");
			}
			else
			{
				if (this.CurrentMap == Scenes.scene_map_world_DLC)
				{
					this.djimmiActivatedInfiniteWishDLC = true;
				}
				else
				{
					this.djimmiActivatedInfiniteWishBaseGame = true;
				}
				MapEventNotification.Current.ShowEvent(MapEventNotification.Type.DjimmiFreed);
			}
			PlayerData.SaveCurrentFile();
			return true;
		}
		if (this.djimmiActivatedCountedWish)
		{
			this.djimmiActivatedCountedWish = false;
			this.djimmiWishes++;
			PlayerData.SaveCurrentFile();
			AudioManager.Play("sfx_worldmap_djimmi_deactivate");
			return true;
		}
		if (!this.djimmiActivatedCountedWish && this.djimmiWishes > 0)
		{
			this.djimmiActivatedCountedWish = true;
			this.djimmiWishes--;
			PlayerData.SaveCurrentFile();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Djimmi);
			return true;
		}
		return false;
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00008C86 File Offset: 0x00006E86
	public bool DjimmiActivatedCurrentRegion()
	{
		return (this.CurrentMap != Scenes.scene_map_world_DLC) ? this.DjimmiActivatedBaseGame() : this.DjimmiActivatedDLC();
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x00008CA6 File Offset: 0x00006EA6
	public bool DjimmiActivatedBaseGame()
	{
		return this.djimmiActivatedCountedWish || this.djimmiActivatedInfiniteWishBaseGame;
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x00008CBC File Offset: 0x00006EBC
	public bool DjimmiActivatedDLC()
	{
		return this.djimmiActivatedCountedWish || this.djimmiActivatedInfiniteWishDLC;
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x00008CD2 File Offset: 0x00006ED2
	public bool DjimmiFreedCurrentRegion()
	{
		return (this.CurrentMap != Scenes.scene_map_world_DLC) ? this.DjimmiFreedBaseGame() : this.DjimmiFreedDLC();
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x00078814 File Offset: 0x00076A14
	public bool DjimmiFreedBaseGame()
	{
		return this.CheckLevelsCompleted(Level.world1BossLevels) && this.CheckLevelsCompleted(Level.world2BossLevels) && this.CheckLevelsCompleted(Level.world3BossLevels) && this.CheckLevelsCompleted(Level.world4BossLevels) && this.CheckLevelsCompleted(Level.platformingLevels);
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00008CF2 File Offset: 0x00006EF2
	public bool DjimmiFreedDLC()
	{
		return this.CheckLevelsCompleted(Level.worldDLCBossLevelsWithSaltbaker) && this.CheckLevelsCompleted(Level.kingOfGamesLevels);
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x00008D12 File Offset: 0x00006F12
	public void DeactivateDjimmi()
	{
		if (this.DjimmiFreedCurrentRegion())
		{
			if (this.CurrentMap == Scenes.scene_map_world_DLC)
			{
				this.djimmiActivatedInfiniteWishDLC = false;
			}
			else
			{
				this.djimmiActivatedInfiniteWishBaseGame = false;
			}
		}
		else
		{
			this.djimmiActivatedCountedWish = false;
		}
		PlayerData.SaveCurrentFile();
	}

	// Token: 0x040006DD RID: 1757
	public static readonly PlayerData.LevelCoinIds[] platformingCoinIDs = new PlayerData.LevelCoinIds[]
	{
		new PlayerData.LevelCoinIds(Levels.Platforming_Level_1_1, new string[][]
		{
			new string[]
			{
				"scene_level_platforming_1_1F::Level_Coin :: 5fd52d1b-a7f2-43a6-80e2-cb170cbc7d4d"
			},
			new string[]
			{
				"scene_level_platforming_1_1F::Level_Coin :: 63c021bf-52f0-41de-bedf-c77117d244cc"
			},
			new string[]
			{
				"scene_level_platforming_1_1F::Level_Coin :: 245037a6-1fa2-4167-a631-0723abff8138"
			},
			new string[]
			{
				"scene_level_platforming_1_1F::Level_Coin :: eaefb009-c117-4b9a-96c1-7abc5558d213"
			},
			new string[]
			{
				"scene_level_platforming_1_1F::Level_Coin :: 5526f7bc-a902-4c13-9e7a-1632a5abe378"
			}
		}),
		new PlayerData.LevelCoinIds(Levels.Platforming_Level_1_2, new string[][]
		{
			new string[]
			{
				"scene_level_platforming_1_2F::Level_Coin :: 323989de-349e-4740-a764-dbc12217a27c"
			},
			new string[]
			{
				"scene_level_platforming_1_2F::Level_Coin :: 55a46261-b14c-4065-9ada-18524eaed9f3"
			},
			new string[]
			{
				"scene_level_platforming_1_2F::Level_Coin :: da0983f6-62d4-4ace-81f2-cad7181d5fe9"
			},
			new string[]
			{
				"scene_level_platforming_1_2F::Level_Coin :: 7088ec51-4792-49c0-ab2c-c45ec9deb9f0"
			},
			new string[]
			{
				"scene_level_platforming_1_2F::Level_Coin :: e02954c1-ff76-4ba4-849f-90aae53a7787"
			}
		}),
		new PlayerData.LevelCoinIds(Levels.Platforming_Level_2_1, new string[][]
		{
			new string[]
			{
				"scene_level_platforming_2_1F::Level_Coin :: 24ef654a-a65b-4a1c-b5e5-c3c64e250646"
			},
			new string[]
			{
				"scene_level_platforming_2_1F::Level_Coin :: b8d96f03-d264-4a61-9ab9-07de34f660aa"
			},
			new string[]
			{
				"scene_level_platforming_2_1F::Level_Coin :: 383d9b3b-c280-4825-a6b3-1a21fe42d0ac"
			},
			new string[]
			{
				"scene_level_platforming_2_1F::Level_Coin :: f1b99bcd-0fa8-4aac-9a54-f310e173ddf9"
			},
			new string[]
			{
				"scene_level_platforming_2_1F::Level_Coin :: c763ef21-2ee7-491c-a143-b906856fed6c"
			}
		}),
		new PlayerData.LevelCoinIds(Levels.Platforming_Level_2_2, new string[][]
		{
			new string[]
			{
				"scene_level_platforming_2_2F::Level_Coin :: 9025a0e9-fff1-4f14-93d1-1930eef27405",
				"scene_level_platforming_2_2F::Level_Coin :: abbfb110-69d1-4948-9c70-223c6425c6f5",
				"scene_level_platforming_2_2F::Level_Coin :: 159497a2-3ded-4c0e-8852-4f6c41046df7",
				"scene_level_platforming_2_2F::Level_Coin :: 22bd722b-bf79-438b-92b0-56c638ae7114",
				"scene_level_platforming_2_2F::Level_Coin :: 84c8547b-b9b8-4fe9-9b0f-75980a3f5454",
				"scene_level_platforming_2_2F::Level_Coin :: d8d1b996-c4ef-4586-9c69-a3f18ebaeece",
				"scene_level_platforming_2_2F::Level_Coin :: 79695e06-f5c3-4826-96e8-5318399cdaf0",
				"scene_level_platforming_2_2F::Level_Coin :: 3aa60c71-a8c9-4b44-b53e-f954c9c70b29"
			},
			new string[]
			{
				"scene_level_platforming_2_2F::Level_Coin :: 284ea6f9-5db4-4f80-b0e5-1d9513a8acb7"
			},
			new string[]
			{
				"scene_level_platforming_2_2F::Level_Coin :: 43a8fc82-b8b8-4a92-b56f-c3e718b46b2c"
			},
			new string[]
			{
				"scene_level_platforming_2_2F::Level_Coin :: bf86d025-4524-4ce8-ba07-540ef3f61ed8"
			},
			new string[]
			{
				"scene_level_platforming_2_2F::Level_Coin :: a7c0e2b9-9560-4ed7-a3a4-428365222cb9"
			}
		}),
		new PlayerData.LevelCoinIds(Levels.Platforming_Level_3_1, new string[][]
		{
			new string[]
			{
				"scene_level_platforming_3_1F::Level_Coin :: 26ba2e1d-4b0a-4964-ba4d-f58655ef47db",
				"scene_level_platforming_3_1F::Level_Coin :: 8d1cd543-fa2f-41d6-9e50-d8ea356c9d26",
				"scene_level_platforming_3_1F::Level_Coin :: 90912b91-c396-429a-b061-0af90b666a0f",
				"scene_level_platforming_3_1F::Level_Coin :: 7a4de11e-fed9-479a-8ace-57bb7a00baa7",
				"scene_level_platforming_3_1F::Level_Coin :: eabb3294-336c-4615-8975-210343a039b5",
				"scene_level_platforming_3_1F::Level_Coin :: 6c032ae2-7bb9-4236-abc4-c27177201615",
				"scene_level_platforming_3_1F::Level_Coin :: 6fcd5ca7-9953-4343-a7f4-55d3fbc7d287",
				"scene_level_platforming_3_1F::Level_Coin :: e280e5f3-9fa1-4587-9139-84c127413e7a"
			},
			new string[]
			{
				"scene_level_platforming_3_1F::Level_Coin :: 0f13fbe6-1041-445f-97ed-1bbe2cb0339e",
				"scene_level_platforming_3_1F::Level_Coin :: 9aa051bf-5ec9-47b2-93f5-09f1495e78f2",
				"scene_level_platforming_3_1F::Level_Coin :: 4f4c2a23-244a-484b-84c9-ca5c6fc4e6bb",
				"scene_level_platforming_3_1F::Level_Coin :: 5c1e1ce4-055a-4ed6-8f5a-c667dbcac5af",
				"scene_level_platforming_3_1F::Level_Coin :: c1b74075-ae62-45ab-8d60-08286a35936f",
				"scene_level_platforming_3_1F::Level_Coin :: 5e1c290f-e2a4-4410-a52c-ba41ce7e56c5",
				"scene_level_platforming_3_1F::Level_Coin :: b18f581d-67b3-4020-b031-3a5bb62a9fa1",
				"scene_level_platforming_3_1F::Level_Coin :: 7b9e2b26-9132-4558-922b-ea400d4fdb0f"
			},
			new string[]
			{
				"scene_level_platforming_3_1F::Level_Coin :: 0086a9b3-87b8-4406-b97b-b94a1fd60bb0",
				"scene_level_platforming_3_1F::Level_Coin :: 273f231f-11d1-42db-888d-7d78696b934b",
				"scene_level_platforming_3_1F::Level_Coin :: cab629f0-54fa-43d3-8573-5d82db28e5c9",
				"scene_level_platforming_3_1F::Level_Coin :: b9ffa14a-984d-426b-8a96-7e71c58d8542",
				"scene_level_platforming_3_1F::Level_Coin :: b0f7e7a4-16a8-4a58-9abc-51f2aac1aac3",
				"scene_level_platforming_3_1F::Level_Coin :: 2b7cac59-e975-47f2-bf74-e49cb612266a",
				"scene_level_platforming_3_1F::Level_Coin :: e74bfad7-8657-4d6b-b853-9fef027e8600",
				"scene_level_platforming_3_1F::Level_Coin :: 6153c3cb-493f-465e-b6b2-dcddd5c0c50e"
			},
			new string[]
			{
				"scene_level_platforming_3_1F::Level_Coin :: 0a6fbbe4-5c13-4b17-9b58-91e7bbdacde4",
				"scene_level_platforming_3_1F::Level_Coin :: f72bdba8-cc0b-4d17-a83f-9892c3507b1c",
				"scene_level_platforming_3_1F::Level_Coin :: 5eaabcfd-0101-4ff5-92f4-a65d885be960",
				"scene_level_platforming_3_1F::Level_Coin :: 036a2830-7c80-443b-b9f6-1576dbf5cb33",
				"scene_level_platforming_3_1F::Level_Coin :: 1c782442-7e15-4a48-a66b-19c5a862e61e",
				"scene_level_platforming_3_1F::Level_Coin :: 01b6dc66-dd9a-4a6f-ac4d-e93a173395ef",
				"scene_level_platforming_3_1F::Level_Coin :: 4ca8faee-fb21-4f5a-b521-4deb89d853c3",
				"scene_level_platforming_3_1F::Level_Coin :: 5bfd4fdf-546c-4751-b2a7-eb99c7cdd2f4"
			},
			new string[]
			{
				"scene_level_platforming_3_1F::Level_Coin :: beb664ad-5577-4055-9164-b1b2f77430f3",
				"scene_level_platforming_3_1F::Level_Coin :: 2ffc0eef-d922-4825-bfb4-7377c16e197d",
				"scene_level_platforming_3_1F::Level_Coin :: 0c636a66-f96c-4046-9ccd-12897ab77649",
				"scene_level_platforming_3_1F::Level_Coin :: 267b5e81-84e6-4297-848c-bea5549b1690",
				"scene_level_platforming_3_1F::Level_Coin :: 76e64c16-b4d3-472f-85ae-d1dbd5c055e3",
				"scene_level_platforming_3_1F::Level_Coin :: 05b62218-8f30-4d74-bab4-7d27f4e0ab90",
				"scene_level_platforming_3_1F::Level_Coin :: 6222ae58-b0c8-44e8-81f6-417f00cc1be1",
				"scene_level_platforming_3_1F::Level_Coin :: 54c21221-4a03-4437-a2bd-a5972c3e2bfc"
			}
		}),
		new PlayerData.LevelCoinIds(Levels.Platforming_Level_3_2, new string[][]
		{
			new string[]
			{
				"scene_level_platforming_3_2F::Level_Coin :: 5da68904-6505-4841-9684-71d2931c1bd6"
			},
			new string[]
			{
				"scene_level_platforming_3_2F::Level_Coin :: 999c9b0d-d554-471d-ad96-ee6d57ccfd19"
			},
			new string[]
			{
				"scene_level_platforming_3_2F::Level_Coin :: cf0a7cae-d8d9-4be0-9502-8b8544606e04"
			},
			new string[]
			{
				"scene_level_platforming_3_2F::Level_Coin :: e671db16-cf6e-421c-937c-2b6f5c7ad0e7"
			},
			new string[]
			{
				"scene_level_platforming_3_2F::Level_Coin :: 084a7b75-e752-452f-8710-687db1e165fe"
			}
		})
	};

	// Token: 0x040006DE RID: 1758
	public const string KEY = "cuphead_player_data_v1_slot_";

	// Token: 0x040006DF RID: 1759
	public static readonly string[] SAVE_FILE_KEYS = new string[]
	{
		"cuphead_player_data_v1_slot_0",
		"cuphead_player_data_v1_slot_1",
		"cuphead_player_data_v1_slot_2"
	};

	// Token: 0x040006E0 RID: 1760
	public static readonly Weapon[] WeaponsDLC = new Weapon[]
	{
		Weapon.level_weapon_wide_shot,
		Weapon.level_weapon_upshot,
		Weapon.level_weapon_crackshot
	};

	// Token: 0x040006E1 RID: 1761
	public static readonly Charm[] CharmsDLC = new Charm[]
	{
		Charm.charm_chalice,
		Charm.charm_healer,
		Charm.charm_curse
	};

	// Token: 0x040006E2 RID: 1762
	public static string emptyDialoguerState = string.Empty;

	// Token: 0x040006E3 RID: 1763
	public static int _CurrentSaveFileIndex = 0;

	// Token: 0x040006E4 RID: 1764
	public static bool _initialized = false;

	// Token: 0x040006E5 RID: 1765
	public static bool inGame = false;

	// Token: 0x040006E6 RID: 1766
	public static PlayerData[] _saveFiles;

	// Token: 0x040006E7 RID: 1767
	public static PlayerData.PlayerDataInitHandler _playerDatatInitHandler;

	// Token: 0x040006E8 RID: 1768
	public bool isPlayer1Mugman;

	// Token: 0x040006E9 RID: 1769
	public bool hasMadeFirstPurchase;

	// Token: 0x040006EA RID: 1770
	public bool hasBeatenAnyBossOnEasy;

	// Token: 0x040006EB RID: 1771
	public bool hasBeatenAnyDLCBossOnEasy;

	// Token: 0x040006EC RID: 1772
	public bool hasUnlockedFirstSuper;

	// Token: 0x040006ED RID: 1773
	public bool shouldShowShopkeepTooltip;

	// Token: 0x040006EE RID: 1774
	public bool shouldShowTurtleTooltip;

	// Token: 0x040006EF RID: 1775
	public bool shouldShowCanteenTooltip;

	// Token: 0x040006F0 RID: 1776
	public bool shouldShowForkTooltip;

	// Token: 0x040006F1 RID: 1777
	public bool shouldShowKineDiceTooltip;

	// Token: 0x040006F2 RID: 1778
	public bool shouldShowMausoleumTooltip;

	// Token: 0x040006F3 RID: 1779
	public bool hasUnlockedBoatman;

	// Token: 0x040006F4 RID: 1780
	public bool shouldShowBoatmanTooltip;

	// Token: 0x040006F5 RID: 1781
	public bool shouldShowChaliceTooltip;

	// Token: 0x040006F6 RID: 1782
	public bool hasTalkedToChaliceFan;

	// Token: 0x040006F7 RID: 1783
	public int[] curseCharmPuzzleOrder;

	// Token: 0x040006F8 RID: 1784
	public bool curseCharmPuzzleComplete;

	// Token: 0x040006F9 RID: 1785
	public MapCastleZones.Zone currentChessBossZone;

	// Token: 0x040006FA RID: 1786
	public List<MapCastleZones.Zone> usedChessBossZones = new List<MapCastleZones.Zone>();

	// Token: 0x040006FB RID: 1787
	public int chessBossAttemptCounter;

	// Token: 0x040006FC RID: 1788
	public bool djimmiActivatedCountedWish;

	// Token: 0x040006FD RID: 1789
	public bool djimmiActivatedInfiniteWishBaseGame;

	// Token: 0x040006FE RID: 1790
	public bool djimmiActivatedInfiniteWishDLC;

	// Token: 0x040006FF RID: 1791
	public int djimmiWishes = 3;

	// Token: 0x04000700 RID: 1792
	public bool djimmiFreed;

	// Token: 0x04000701 RID: 1793
	public bool djimmiFreedDLC;

	// Token: 0x04000702 RID: 1794
	public int dummy;

	// Token: 0x04000703 RID: 1795
	[SerializeField]
	public PlayerData.PlayerLoadouts loadouts = new PlayerData.PlayerLoadouts();

	// Token: 0x04000704 RID: 1796
	[SerializeField]
	public bool _isHardModeAvailable;

	// Token: 0x04000705 RID: 1797
	[SerializeField]
	public bool _isHardModeAvailableDLC;

	// Token: 0x04000706 RID: 1798
	[SerializeField]
	public bool _isTutorialCompleted;

	// Token: 0x04000707 RID: 1799
	[SerializeField]
	public bool _isFlyingTutorialCompleted;

	// Token: 0x04000708 RID: 1800
	[SerializeField]
	public bool _isChaliceTutorialCompleted;

	// Token: 0x04000709 RID: 1801
	[SerializeField]
	public PlayerData.PlayerInventories inventories = new PlayerData.PlayerInventories();

	// Token: 0x0400070A RID: 1802
	public string dialoguerState;

	// Token: 0x0400070B RID: 1803
	[SerializeField]
	public PlayerData.PlayerCoinManager coinManager = new PlayerData.PlayerCoinManager();

	// Token: 0x0400070C RID: 1804
	public PlayerData.PlayerCoinManager levelCoinManager = new PlayerData.PlayerCoinManager();

	// Token: 0x0400070D RID: 1805
	public bool unlockedBlackAndWhite;

	// Token: 0x0400070E RID: 1806
	public bool unlocked2Strip;

	// Token: 0x0400070F RID: 1807
	public bool unlockedChaliceRecolor;

	// Token: 0x04000710 RID: 1808
	public bool vintageAudioEnabled;

	// Token: 0x04000711 RID: 1809
	public bool pianoAudioEnabled;

	// Token: 0x04000712 RID: 1810
	public BlurGamma.Filter filter;

	// Token: 0x04000713 RID: 1811
	[SerializeField]
	public PlayerData.MapDataManager mapDataManager = new PlayerData.MapDataManager();

	// Token: 0x04000714 RID: 1812
	[SerializeField]
	public PlayerData.PlayerLevelDataManager levelDataManager = new PlayerData.PlayerLevelDataManager();

	// Token: 0x04000715 RID: 1813
	[SerializeField]
	public PlayerData.PlayerStats statictics = new PlayerData.PlayerStats();

	// Token: 0x04000716 RID: 1814
	[CompilerGenerated]
	private static InitializeCloudStoreHandler <>f__mg$cache0;

	// Token: 0x04000717 RID: 1815
	[CompilerGenerated]
	private static LoadCloudDataHandler <>f__mg$cache1;

	// Token: 0x04000718 RID: 1816
	[CompilerGenerated]
	private static LoadCloudDataHandler <>f__mg$cache2;

	// Token: 0x04000719 RID: 1817
	[CompilerGenerated]
	private static SaveCloudDataHandler <>f__mg$cache3;

	// Token: 0x0400071A RID: 1818
	[CompilerGenerated]
	private static SaveCloudDataHandler <>f__mg$cache4;

	// Token: 0x02000925 RID: 2341
	public struct LevelCoinIds
	{
		// Token: 0x06005407 RID: 21511 RVA: 0x0003FB6B File Offset: 0x0003DD6B
		public LevelCoinIds(Levels level, string[][] coins)
		{
			this.levelId = level;
			this.coinIds = coins;
		}

		// Token: 0x04004539 RID: 17721
		public Levels levelId;

		// Token: 0x0400453A RID: 17722
		public string[][] coinIds;
	}

	// Token: 0x02000926 RID: 2342
	// (Invoke) Token: 0x06005409 RID: 21513
	public delegate void PlayerDataInitHandler(bool success);

	// Token: 0x02000927 RID: 2343
	[Serializable]
	public class PlayerLoadouts
	{
		// Token: 0x0600540C RID: 21516 RVA: 0x0003FB7B File Offset: 0x0003DD7B
		public PlayerLoadouts()
		{
			this.playerOne = new PlayerData.PlayerLoadouts.PlayerLoadout();
			this.playerTwo = new PlayerData.PlayerLoadouts.PlayerLoadout();
		}

		// Token: 0x0600540D RID: 21517 RVA: 0x0003FB99 File Offset: 0x0003DD99
		public PlayerLoadouts(PlayerData.PlayerLoadouts.PlayerLoadout playerOne, PlayerData.PlayerLoadouts.PlayerLoadout playerTwo)
		{
			this.playerOne = playerOne;
			this.playerTwo = playerTwo;
		}

		// Token: 0x0600540E RID: 21518 RVA: 0x0003FBAF File Offset: 0x0003DDAF
		public PlayerData.PlayerLoadouts.PlayerLoadout GetPlayerLoadout(PlayerId player)
		{
			if (player == PlayerId.PlayerOne)
			{
				return this.playerOne;
			}
			if (player != PlayerId.PlayerTwo)
			{
				return null;
			}
			return this.playerTwo;
		}

		// Token: 0x0400453B RID: 17723
		public PlayerData.PlayerLoadouts.PlayerLoadout playerOne;

		// Token: 0x0400453C RID: 17724
		public PlayerData.PlayerLoadouts.PlayerLoadout playerTwo;

		// Token: 0x020015CF RID: 5583
		[Serializable]
		public class PlayerLoadout
		{
			// Token: 0x06008745 RID: 34629 RVA: 0x0005B923 File Offset: 0x00059B23
			public PlayerLoadout()
			{
				this.primaryWeapon = Weapon.level_weapon_peashot;
				this.secondaryWeapon = Weapon.None;
				this.super = Super.None;
				this.charm = Charm.None;
			}

			// Token: 0x17001A34 RID: 6708
			// (get) Token: 0x06008746 RID: 34630 RVA: 0x0005B957 File Offset: 0x00059B57
			// (set) Token: 0x06008747 RID: 34631 RVA: 0x0005B95F File Offset: 0x00059B5F
			public bool HasEquippedSecondaryRegularWeapon { get; set; }

			// Token: 0x17001A35 RID: 6709
			// (get) Token: 0x06008748 RID: 34632 RVA: 0x0005B968 File Offset: 0x00059B68
			// (set) Token: 0x06008749 RID: 34633 RVA: 0x0005B970 File Offset: 0x00059B70
			public bool HasEquippedSecondarySHMUPWeapon { get; set; }

			// Token: 0x17001A36 RID: 6710
			// (get) Token: 0x0600874A RID: 34634 RVA: 0x0005B979 File Offset: 0x00059B79
			// (set) Token: 0x0600874B RID: 34635 RVA: 0x0005B981 File Offset: 0x00059B81
			public bool MustNotifySwitchRegularWeapon { get; set; }

			// Token: 0x17001A37 RID: 6711
			// (get) Token: 0x0600874C RID: 34636 RVA: 0x0005B98A File Offset: 0x00059B8A
			// (set) Token: 0x0600874D RID: 34637 RVA: 0x0005B992 File Offset: 0x00059B92
			public bool MustNotifySwitchSHMUPWeapon { get; set; }

			// Token: 0x0400919A RID: 37274
			public Weapon primaryWeapon;

			// Token: 0x0400919B RID: 37275
			public Weapon secondaryWeapon;

			// Token: 0x0400919C RID: 37276
			public Super super;

			// Token: 0x0400919D RID: 37277
			public Charm charm;
		}
	}

	// Token: 0x02000928 RID: 2344
	[Serializable]
	public class PlayerInventories
	{
		// Token: 0x06005410 RID: 21520 RVA: 0x0003FBF0 File Offset: 0x0003DDF0
		public PlayerData.PlayerInventory GetPlayer(PlayerId player)
		{
			if (player == PlayerId.PlayerOne)
			{
				return this.playerOne;
			}
			if (player != PlayerId.PlayerTwo)
			{
				return null;
			}
			return this.playerTwo;
		}

		// Token: 0x0400453D RID: 17725
		public int dummy;

		// Token: 0x0400453E RID: 17726
		public PlayerData.PlayerInventory playerOne = new PlayerData.PlayerInventory();

		// Token: 0x0400453F RID: 17727
		public PlayerData.PlayerInventory playerTwo = new PlayerData.PlayerInventory();
	}

	// Token: 0x02000929 RID: 2345
	[Serializable]
	public class PlayerInventory
	{
		// Token: 0x06005411 RID: 21521 RVA: 0x001C10AC File Offset: 0x001BF2AC
		public PlayerInventory()
		{
			this.money = 0;
			this._weapons = new List<Weapon>();
			this._supers = new List<Super>();
			this._charms = new List<Charm>();
			this._weapons.Add(Weapon.level_weapon_peashot);
			this._weapons.Add(Weapon.plane_weapon_peashot);
		}

		// Token: 0x06005412 RID: 21522 RVA: 0x0003FC13 File Offset: 0x0003DE13
		public bool IsUnlocked(Weapon weapon)
		{
			return this._weapons.Contains(weapon);
		}

		// Token: 0x06005413 RID: 21523 RVA: 0x0003FC21 File Offset: 0x0003DE21
		public bool IsUnlocked(Super super)
		{
			return this._supers.Contains(super);
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x0003FC2F File Offset: 0x0003DE2F
		public bool IsUnlocked(Charm charm)
		{
			return this._charms.Contains(charm);
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x001C1108 File Offset: 0x001BF308
		public bool Buy(Weapon value)
		{
			if (this.IsUnlocked(value))
			{
				return false;
			}
			if (this.money < WeaponProperties.GetValue(value))
			{
				return false;
			}
			this.money -= WeaponProperties.GetValue(value);
			this._weapons.Add(value);
			this.newPurchase = true;
			return true;
		}

		// Token: 0x06005416 RID: 21526 RVA: 0x001C1160 File Offset: 0x001BF360
		public bool Buy(Super value)
		{
			if (this.IsUnlocked(value))
			{
				return false;
			}
			if (this.money < WeaponProperties.GetValue(value))
			{
				return false;
			}
			this.money -= WeaponProperties.GetValue(value);
			this._supers.Add(value);
			this.newPurchase = true;
			return true;
		}

		// Token: 0x06005417 RID: 21527 RVA: 0x001C11B8 File Offset: 0x001BF3B8
		public bool Buy(Charm value)
		{
			if (this.IsUnlocked(value))
			{
				return false;
			}
			if (this.money < WeaponProperties.GetValue(value))
			{
				return false;
			}
			this.money -= WeaponProperties.GetValue(value);
			this._charms.Add(value);
			this.newPurchase = true;
			return true;
		}

		// Token: 0x04004540 RID: 17728
		public const int STARTING_MONEY = 0;

		// Token: 0x04004541 RID: 17729
		public int money;

		// Token: 0x04004542 RID: 17730
		public bool newPurchase;

		// Token: 0x04004543 RID: 17731
		public List<Weapon> _weapons;

		// Token: 0x04004544 RID: 17732
		public List<Super> _supers;

		// Token: 0x04004545 RID: 17733
		public List<Charm> _charms;
	}

	// Token: 0x0200092A RID: 2346
	[Serializable]
	public class PlayerCoinManager
	{
		// Token: 0x06005418 RID: 21528 RVA: 0x001C1210 File Offset: 0x001BF410
		public PlayerCoinManager()
		{
			this.LevelsAndCoins = new List<PlayerData.PlayerCoinManager.LevelAndCoins>();
			IEnumerator enumerator = Enum.GetValues(typeof(Levels)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Levels level = (Levels)obj;
					PlayerData.PlayerCoinManager.LevelAndCoins levelAndCoins = new PlayerData.PlayerCoinManager.LevelAndCoins();
					levelAndCoins.level = level;
					this.LevelsAndCoins.Add(levelAndCoins);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x0003FC3D File Offset: 0x0003DE3D
		public bool GetCoinCollected(LevelCoin coin)
		{
			return this.GetCoinCollected(coin.GlobalID);
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x0003FC4B File Offset: 0x0003DE4B
		public bool GetCoinCollected(string coinID)
		{
			return this.ContainsCoin(coinID) && this.GetCoin(coinID).collected;
		}

		// Token: 0x0600541B RID: 21531 RVA: 0x001C12B8 File Offset: 0x001BF4B8
		public int NumCoinsCollected()
		{
			int num = 0;
			foreach (PlayerData.PlayerCoinProperties playerCoinProperties in this.coins)
			{
				if (playerCoinProperties.collected)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x001C1320 File Offset: 0x001BF520
		public int NumCoinsCollected(bool DLC)
		{
			int num = 0;
			foreach (PlayerData.PlayerCoinProperties playerCoinProperties in this.coins)
			{
				if (playerCoinProperties.collected && this.IsDLCCoin(playerCoinProperties.coinID) == DLC)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600541D RID: 21533 RVA: 0x0003FC67 File Offset: 0x0003DE67
		public void SetCoinValue(LevelCoin coin, bool collected, PlayerId player)
		{
			this.SetCoinValue(coin.GlobalID, collected, player);
		}

		// Token: 0x0600541E RID: 21534 RVA: 0x001C139C File Offset: 0x001BF59C
		public void SetCoinValue(string coinID, bool collected, PlayerId player)
		{
			if (this.ContainsCoin(coinID))
			{
				PlayerData.PlayerCoinProperties coin = this.GetCoin(coinID);
				coin.collected = collected;
				coin.player = player;
			}
			else
			{
				this.AddCoin(new PlayerData.PlayerCoinProperties(coinID)
				{
					collected = collected
				});
			}
		}

		// Token: 0x0600541F RID: 21535 RVA: 0x0003FC77 File Offset: 0x0003DE77
		public PlayerData.PlayerCoinProperties GetCoin(LevelCoin coin)
		{
			return this.GetCoin(coin.GlobalID);
		}

		// Token: 0x06005420 RID: 21536 RVA: 0x001C13E8 File Offset: 0x001BF5E8
		public PlayerData.PlayerCoinProperties GetCoin(string coinID)
		{
			for (int i = 0; i < this.coins.Count; i++)
			{
				if (this.coins[i].coinID == coinID)
				{
					return this.coins[i];
				}
			}
			return null;
		}

		// Token: 0x06005421 RID: 21537 RVA: 0x0003FC85 File Offset: 0x0003DE85
		public void AddCoin(LevelCoin coin)
		{
			this.AddCoin(coin.GlobalID);
		}

		// Token: 0x06005422 RID: 21538 RVA: 0x0003FC93 File Offset: 0x0003DE93
		public void AddCoin(string coinID)
		{
			if (!this.ContainsCoin(coinID))
			{
				this.coins.Add(new PlayerData.PlayerCoinProperties(coinID));
			}
			this.RegisterCoin(coinID);
		}

		// Token: 0x06005423 RID: 21539 RVA: 0x0003FCB9 File Offset: 0x0003DEB9
		public void AddCoin(PlayerData.PlayerCoinProperties coin)
		{
			if (!this.ContainsCoin(coin.coinID))
			{
				this.coins.Add(coin);
			}
			this.RegisterCoin(coin.coinID);
		}

		// Token: 0x06005424 RID: 21540 RVA: 0x001C143C File Offset: 0x001BF63C
		public void RegisterCoin(string coinID)
		{
			PlatformingLevel platformingLevel = Level.Current as PlatformingLevel;
			if (platformingLevel)
			{
				List<PlayerData.PlayerCoinManager.LevelAndCoins> levelsAndCoins = this.LevelsAndCoins;
				int num = -1;
				for (int i = 0; i < levelsAndCoins.Count; i++)
				{
					if (levelsAndCoins[i].level == platformingLevel.CurrentLevel)
					{
						num = i;
					}
				}
				if (num >= 0)
				{
					for (int j = 0; j < platformingLevel.LevelCoinsIDs.Count; j++)
					{
						if (platformingLevel.LevelCoinsIDs[j].CoinID == coinID)
						{
							switch (j)
							{
							case 0:
								levelsAndCoins[num].Coin1Collected = true;
								break;
							case 1:
								levelsAndCoins[num].Coin2Collected = true;
								break;
							case 2:
								levelsAndCoins[num].Coin3Collected = true;
								break;
							case 3:
								levelsAndCoins[num].Coin4Collected = true;
								break;
							case 4:
								levelsAndCoins[num].Coin5Collected = true;
								break;
							}
							break;
						}
					}
				}
			}
			else if (Map.Current != null)
			{
				List<PlayerData.PlayerCoinManager.LevelAndCoins> levelsAndCoins2 = PlayerData.Data.coinManager.LevelsAndCoins;
				int num2 = -1;
				for (int k = 0; k < levelsAndCoins2.Count; k++)
				{
					if (levelsAndCoins2[k].level == Map.Current.level)
					{
						num2 = k;
					}
				}
				if (num2 >= 0)
				{
					for (int l = 0; l < Map.Current.LevelCoinsIDs.Count; l++)
					{
						if (Map.Current.LevelCoinsIDs[l].CoinID == coinID)
						{
							switch (l)
							{
							case 0:
								levelsAndCoins2[num2].Coin1Collected = true;
								break;
							case 1:
								levelsAndCoins2[num2].Coin2Collected = true;
								break;
							case 2:
								levelsAndCoins2[num2].Coin3Collected = true;
								break;
							case 3:
								levelsAndCoins2[num2].Coin4Collected = true;
								break;
							case 4:
								levelsAndCoins2[num2].Coin5Collected = true;
								break;
							}
							break;
						}
					}
				}
			}
			bool flag = true;
			foreach (Levels level in Level.platformingLevels)
			{
				if (PlayerData.Data.GetNumCoinsCollectedInLevel(level) < 5)
				{
					flag = false;
				}
			}
			if (flag)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "FoundAllLevelMoney");
			}
			if (PlayerData.Data.NumCoinsCollectedMainGame >= 40)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "FoundAllMoney");
			}
		}

		// Token: 0x06005425 RID: 21541 RVA: 0x0003FCE4 File Offset: 0x0003DEE4
		public bool ContainsCoin(LevelCoin coin)
		{
			return this.ContainsCoin(coin.GlobalID);
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x001C1728 File Offset: 0x001BF928
		public bool ContainsCoin(string coinID)
		{
			for (int i = 0; i < this.coins.Count; i++)
			{
				if (this.coins[i].coinID == coinID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x0003FCF2 File Offset: 0x0003DEF2
		public bool IsDLCCoin(LevelCoin coin)
		{
			return this.IsDLCCoin(coin.GlobalID);
		}

		// Token: 0x06005428 RID: 21544 RVA: 0x001C1770 File Offset: 0x001BF970
		public bool IsDLCCoin(string coinID)
		{
			return coinID == "619e92f1-e0fd-4f6e-9c2d-5ce5dbaf393f" || coinID == "scene_level_chalice_tutorial::Level_Coin :: 578c0218-df9e-4cdd-932a-a1277b5b7129" || coinID == "a37b3d37-a32e-4b88-a583-34489496494d" || coinID == "25f15554-d229-4330-96cc-ac8a13c18ea0" || coinID == "eacf4228-e200-4839-9d79-3439cfcc5824" || coinID == "47f7edb1-b5c5-4afb-9acb-a46f5e6df557" || coinID == "3826615a-498b-4158-af7b-0d01acbc18c8" || coinID == "d52b1cc6-414c-4a7c-9f8a-250316566d58" || coinID == "fc2c48cd-5dec-472a-ae18-dccfc94232c6" || coinID == "16732bc8-7230-467a-a9ac-ff9c62ab7657" || coinID == "e0c6e8bc-0c56-4e52-a9a1-c53887f5ca4c" || coinID == "19090606-09e8-4e56-92ac-e08200926b94" || coinID == "39bfe6d8-0dbc-4886-9998-52c67b57969e" || coinID == "7f3422f5-6650-497f-9c35-9735b64100d6" || coinID == "9970ad6a-560a-4ae3-9d15-a6b636b67024" || coinID == "3367b9b0-da35-4c81-a895-2720862b5b1b";
		}

		// Token: 0x04004546 RID: 17734
		public int dummy;

		// Token: 0x04004547 RID: 17735
		public List<PlayerData.PlayerCoinProperties> coins = new List<PlayerData.PlayerCoinProperties>();

		// Token: 0x04004548 RID: 17736
		public bool hasMigratedCoins;

		// Token: 0x04004549 RID: 17737
		public List<PlayerData.PlayerCoinManager.LevelAndCoins> LevelsAndCoins = new List<PlayerData.PlayerCoinManager.LevelAndCoins>();

		// Token: 0x020015D0 RID: 5584
		[Serializable]
		public class LevelAndCoins
		{
			// Token: 0x040091A2 RID: 37282
			public Levels level;

			// Token: 0x040091A3 RID: 37283
			public bool Coin1Collected;

			// Token: 0x040091A4 RID: 37284
			public bool Coin2Collected;

			// Token: 0x040091A5 RID: 37285
			public bool Coin3Collected;

			// Token: 0x040091A6 RID: 37286
			public bool Coin4Collected;

			// Token: 0x040091A7 RID: 37287
			public bool Coin5Collected;
		}
	}

	// Token: 0x0200092B RID: 2347
	[Serializable]
	public class PlayerCoinProperties
	{
		// Token: 0x06005429 RID: 21545 RVA: 0x0003FD00 File Offset: 0x0003DF00
		public PlayerCoinProperties()
		{
		}

		// Token: 0x0600542A RID: 21546 RVA: 0x0003FD1E File Offset: 0x0003DF1E
		public PlayerCoinProperties(LevelCoin coin)
		{
			this.coinID = coin.GlobalID;
		}

		// Token: 0x0600542B RID: 21547 RVA: 0x0003FD48 File Offset: 0x0003DF48
		public PlayerCoinProperties(string coinID)
		{
			this.coinID = coinID;
		}

		// Token: 0x0400454A RID: 17738
		public string coinID = string.Empty;

		// Token: 0x0400454B RID: 17739
		public bool collected;

		// Token: 0x0400454C RID: 17740
		public PlayerId player = PlayerId.None;
	}

	// Token: 0x0200092C RID: 2348
	[Serializable]
	public class MapData
	{
		// Token: 0x0400454D RID: 17741
		public Scenes mapId;

		// Token: 0x0400454E RID: 17742
		public bool sessionStarted;

		// Token: 0x0400454F RID: 17743
		public bool hasVisitedDieHouse;

		// Token: 0x04004550 RID: 17744
		public bool hasKingDiceDisappeared;

		// Token: 0x04004551 RID: 17745
		public Vector3 playerOnePosition = Vector3.zero;

		// Token: 0x04004552 RID: 17746
		public Vector3 playerTwoPosition = Vector3.zero;

		// Token: 0x04004553 RID: 17747
		[NonSerialized]
		public PlayerData.MapData.EntryMethod enteringFrom;

		// Token: 0x020015D1 RID: 5585
		public enum EntryMethod
		{
			// Token: 0x040091A9 RID: 37289
			None,
			// Token: 0x040091AA RID: 37290
			DiceHouseLeft,
			// Token: 0x040091AB RID: 37291
			DiceHouseRight,
			// Token: 0x040091AC RID: 37292
			Boatman
		}
	}

	// Token: 0x0200092D RID: 2349
	[Serializable]
	public class MapDataManager
	{
		// Token: 0x0600542D RID: 21549 RVA: 0x0003FD8B File Offset: 0x0003DF8B
		public MapDataManager()
		{
			this.mapData = new List<PlayerData.MapData>();
		}

		// Token: 0x0600542E RID: 21550 RVA: 0x0003FDA5 File Offset: 0x0003DFA5
		public PlayerData.MapData GetCurrentMapData()
		{
			return this.GetMapData(this.currentMap);
		}

		// Token: 0x0600542F RID: 21551 RVA: 0x001C187C File Offset: 0x001BFA7C
		public PlayerData.MapData GetMapData(Scenes map)
		{
			for (int i = 0; i < this.mapData.Count; i++)
			{
				if (this.mapData[i].mapId == map)
				{
					return this.mapData[i];
				}
			}
			PlayerData.MapData mapData = new PlayerData.MapData();
			mapData.mapId = map;
			this.mapData.Add(mapData);
			return mapData;
		}

		// Token: 0x04004554 RID: 17748
		public Scenes currentMap = Scenes.scene_map_world_1;

		// Token: 0x04004555 RID: 17749
		public List<PlayerData.MapData> mapData;
	}

	// Token: 0x0200092E RID: 2350
	[Serializable]
	public class PlayerLevelDataManager
	{
		// Token: 0x06005430 RID: 21552 RVA: 0x001C18E4 File Offset: 0x001BFAE4
		public PlayerLevelDataManager()
		{
			this.levelObjects = new List<PlayerData.PlayerLevelDataObject>();
			foreach (Levels levels in EnumUtils.GetValues<Levels>())
			{
				PlayerData.PlayerLevelDataObject playerLevelDataObject = new PlayerData.PlayerLevelDataObject(levels);
				playerLevelDataObject.levelID = levels;
				this.levelObjects.Add(playerLevelDataObject);
			}
		}

		// Token: 0x06005431 RID: 21553 RVA: 0x001C193C File Offset: 0x001BFB3C
		public PlayerData.PlayerLevelDataObject GetLevelData(Levels levelID)
		{
			for (int i = 0; i < this.levelObjects.Count; i++)
			{
				if (this.levelObjects[i].levelID == levelID)
				{
					return this.levelObjects[i];
				}
			}
			PlayerData.PlayerLevelDataObject playerLevelDataObject = new PlayerData.PlayerLevelDataObject(levelID);
			this.levelObjects.Add(playerLevelDataObject);
			return playerLevelDataObject;
		}

		// Token: 0x04004556 RID: 17750
		public int dummy;

		// Token: 0x04004557 RID: 17751
		public List<PlayerData.PlayerLevelDataObject> levelObjects;
	}

	// Token: 0x0200092F RID: 2351
	[Serializable]
	public class PlayerLevelDataObject
	{
		// Token: 0x06005432 RID: 21554 RVA: 0x0003FDB3 File Offset: 0x0003DFB3
		public PlayerLevelDataObject(Levels id)
		{
			this.levelID = id;
		}

		// Token: 0x04004558 RID: 17752
		public Levels levelID;

		// Token: 0x04004559 RID: 17753
		public bool completed;

		// Token: 0x0400455A RID: 17754
		public bool completedAsChaliceP1;

		// Token: 0x0400455B RID: 17755
		public bool completedAsChaliceP2;

		// Token: 0x0400455C RID: 17756
		public bool played;

		// Token: 0x0400455D RID: 17757
		public LevelScoringData.Grade grade;

		// Token: 0x0400455E RID: 17758
		public Level.Mode difficultyBeaten;

		// Token: 0x0400455F RID: 17759
		public float bestTime = float.MaxValue;

		// Token: 0x04004560 RID: 17760
		public bool curseCharmP1;

		// Token: 0x04004561 RID: 17761
		public bool curseCharmP2;

		// Token: 0x04004562 RID: 17762
		public int bgmPlayListCurrent;
	}

	// Token: 0x02000930 RID: 2352
	[Serializable]
	public class PlayerStats
	{
		// Token: 0x06005434 RID: 21556 RVA: 0x0003FDEB File Offset: 0x0003DFEB
		public PlayerData.PlayerStat GetPlayer(PlayerId player)
		{
			if (player == PlayerId.PlayerOne)
			{
				return this.playerOne;
			}
			if (player != PlayerId.PlayerTwo)
			{
				return null;
			}
			return this.playerTwo;
		}

		// Token: 0x04004563 RID: 17763
		public int dummy;

		// Token: 0x04004564 RID: 17764
		public PlayerData.PlayerStat playerOne = new PlayerData.PlayerStat();

		// Token: 0x04004565 RID: 17765
		public PlayerData.PlayerStat playerTwo = new PlayerData.PlayerStat();
	}

	// Token: 0x02000931 RID: 2353
	[Serializable]
	public class PlayerStat
	{
		// Token: 0x06005435 RID: 21557 RVA: 0x0003FE0E File Offset: 0x0003E00E
		public PlayerStat()
		{
			this.numDeaths = 0;
			this.numParriesInRow = 0;
		}

		// Token: 0x06005436 RID: 21558 RVA: 0x0003FE24 File Offset: 0x0003E024
		public int DeathCount()
		{
			return this.numDeaths;
		}

		// Token: 0x06005437 RID: 21559 RVA: 0x0003FE2C File Offset: 0x0003E02C
		public void Die()
		{
			this.numDeaths++;
		}

		// Token: 0x04004566 RID: 17766
		public int numDeaths;

		// Token: 0x04004567 RID: 17767
		public int numParriesInRow;
	}
}
