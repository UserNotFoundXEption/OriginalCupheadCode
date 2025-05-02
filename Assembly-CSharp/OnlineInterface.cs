using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004EC RID: 1260
public interface OnlineInterface
{
	// Token: 0x14000070 RID: 112
	// (add) Token: 0x060033E8 RID: 13288
	// (remove) Token: 0x060033E9 RID: 13289
	event SignInEventHandler OnUserSignedIn;

	// Token: 0x14000071 RID: 113
	// (add) Token: 0x060033EA RID: 13290
	// (remove) Token: 0x060033EB RID: 13291
	event SignOutEventHandler OnUserSignedOut;

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x060033EC RID: 13292
	OnlineUser MainUser { get; }

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x060033ED RID: 13293
	OnlineUser SecondaryUser { get; }

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x060033EE RID: 13294
	bool CloudStorageInitialized { get; }

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x060033EF RID: 13295
	bool SupportsMultipleUsers { get; }

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x060033F0 RID: 13296
	bool SupportsUserSignIn { get; }

	// Token: 0x060033F1 RID: 13297
	void Init();

	// Token: 0x060033F2 RID: 13298
	void Reset();

	// Token: 0x060033F3 RID: 13299
	void SignInUser(bool silent, PlayerId player, ulong controllerId);

	// Token: 0x060033F4 RID: 13300
	void SwitchUser(PlayerId player, ulong controllerId);

	// Token: 0x060033F5 RID: 13301
	OnlineUser GetUserForController(ulong id);

	// Token: 0x060033F6 RID: 13302
	List<ulong> GetControllersForUser(PlayerId player);

	// Token: 0x060033F7 RID: 13303
	bool IsUserSignedIn(PlayerId player);

	// Token: 0x060033F8 RID: 13304
	OnlineUser GetUser(PlayerId player);

	// Token: 0x060033F9 RID: 13305
	void SetUser(PlayerId player, OnlineUser user);

	// Token: 0x060033FA RID: 13306
	Texture2D GetProfilePic(PlayerId player);

	// Token: 0x060033FB RID: 13307
	void GetAchievement(PlayerId player, string id, AchievementEventHandler achievementRetrievedHandler);

	// Token: 0x060033FC RID: 13308
	void UnlockAchievement(PlayerId player, string id);

	// Token: 0x060033FD RID: 13309
	void SyncAchievementsAndStats();

	// Token: 0x060033FE RID: 13310
	void SetStat(PlayerId player, string id, int value);

	// Token: 0x060033FF RID: 13311
	void SetStat(PlayerId player, string id, float value);

	// Token: 0x06003400 RID: 13312
	void SetStat(PlayerId player, string id, string value);

	// Token: 0x06003401 RID: 13313
	void IncrementStat(PlayerId player, string id, int value);

	// Token: 0x06003402 RID: 13314
	void SetRichPresence(PlayerId player, string id, bool active);

	// Token: 0x06003403 RID: 13315
	void SetRichPresenceActive(PlayerId player, bool active);

	// Token: 0x06003404 RID: 13316
	void InitializeCloudStorage(PlayerId player, InitializeCloudStoreHandler handler);

	// Token: 0x06003405 RID: 13317
	void UninitializeCloudStorage();

	// Token: 0x06003406 RID: 13318
	void SaveCloudData(IDictionary<string, string> data, SaveCloudDataHandler handler);

	// Token: 0x06003407 RID: 13319
	void LoadCloudData(string[] keys, LoadCloudDataHandler handler);

	// Token: 0x06003408 RID: 13320
	void UpdateControllerMapping();

	// Token: 0x06003409 RID: 13321
	bool ControllerMappingChanged();
}
