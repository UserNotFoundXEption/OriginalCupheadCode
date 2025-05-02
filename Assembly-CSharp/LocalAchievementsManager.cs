using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000083 RID: 131
public static class LocalAchievementsManager
{
	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06000646 RID: 1606 RVA: 0x0006F0E4 File Offset: 0x0006D2E4
	// (remove) Token: 0x06000647 RID: 1607 RVA: 0x0006F118 File Offset: 0x0006D318
	public static event Action<LocalAchievementsManager.Achievement> AchievementUnlockedEvent;

	// Token: 0x06000648 RID: 1608 RVA: 0x0000688D File Offset: 0x00004A8D
	public static void Initialize()
	{
		if (LocalAchievementsManager.initialized)
		{
			return;
		}
		LocalAchievementsManager.initialized = true;
		LocalAchievementsManager.loadFromCloud();
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0006F14C File Offset: 0x0006D34C
	public static void UnlockAchievement(PlayerId playerId, string achievementName)
	{
		LocalAchievementsManager.Achievement achievement = (LocalAchievementsManager.Achievement)Enum.Parse(typeof(LocalAchievementsManager.Achievement), achievementName);
		if (LocalAchievementsManager.IsAchievementUnlocked(achievement))
		{
			return;
		}
		LocalAchievementsManager.achievementData.unlockedAchievements.Add(achievement);
		LocalAchievementsManager.saveToCloud();
		if (LocalAchievementsManager.AchievementUnlockedEvent != null)
		{
			LocalAchievementsManager.AchievementUnlockedEvent(achievement);
		}
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x0006F1A8 File Offset: 0x0006D3A8
	public static void IncrementStat(PlayerId player, string id, int value)
	{
		if (id == "Parries")
		{
			if (LocalAchievementsManager.achievementData.parryCount >= 100)
			{
				return;
			}
			LocalAchievementsManager.achievementData.parryCount += value;
			bool flag = true;
			if (LocalAchievementsManager.achievementData.parryCount >= 20)
			{
				LocalAchievementsManager.UnlockAchievement(PlayerId.Any, "ParryApprentice");
				flag = false;
			}
			if (LocalAchievementsManager.achievementData.parryCount >= 100)
			{
				LocalAchievementsManager.UnlockAchievement(PlayerId.Any, "ParryMaster");
				flag = false;
			}
			if (flag)
			{
				LocalAchievementsManager.saveToCloud();
			}
		}
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x000068A5 File Offset: 0x00004AA5
	public static IList<LocalAchievementsManager.Achievement> GetUnlockedAchievements()
	{
		return LocalAchievementsManager.achievementData.unlockedAchievements;
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x000068B1 File Offset: 0x00004AB1
	public static bool IsAchievementUnlocked(LocalAchievementsManager.Achievement achievement)
	{
		return LocalAchievementsManager.achievementData.unlockedAchievements.Contains(achievement);
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0006F23C File Offset: 0x0006D43C
	public static bool IsHiddenAchievement(LocalAchievementsManager.Achievement achievement)
	{
		return achievement == LocalAchievementsManager.Achievement.FoundSecretPassage || achievement == LocalAchievementsManager.Achievement.SmallPlaneOnlyWin || achievement == LocalAchievementsManager.Achievement.FoundAllMoney || achievement == LocalAchievementsManager.Achievement.PacifistRun || achievement == LocalAchievementsManager.Achievement.NoHitsTakenDicePalace || achievement == LocalAchievementsManager.Achievement.BadEnding || achievement == LocalAchievementsManager.Achievement.CompleteDevil || achievement == LocalAchievementsManager.Achievement.DefeatDevilPhase2 || achievement == LocalAchievementsManager.Achievement.Paladin;
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x0006F294 File Offset: 0x0006D494
	public static void saveToCloud()
	{
		if (OnlineManager.Instance.Interface.CloudStorageInitialized)
		{
			string value = JsonUtility.ToJson(LocalAchievementsManager.achievementData);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary[LocalAchievementsManager.CloudKey] = value;
			OnlineInterface @interface = OnlineManager.Instance.Interface;
			IDictionary<string, string> data = dictionary;
			if (LocalAchievementsManager.<>f__mg$cache0 == null)
			{
				LocalAchievementsManager.<>f__mg$cache0 = new SaveCloudDataHandler(LocalAchievementsManager.onSavedCloudData);
			}
			@interface.SaveCloudData(data, LocalAchievementsManager.<>f__mg$cache0);
		}
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x000068C3 File Offset: 0x00004AC3
	public static void onSavedCloudData(bool success)
	{
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x0006F300 File Offset: 0x0006D500
	public static void loadFromCloud()
	{
		if (OnlineManager.Instance.Interface.CloudStorageInitialized)
		{
			OnlineInterface @interface = OnlineManager.Instance.Interface;
			string[] keys = new string[]
			{
				LocalAchievementsManager.CloudKey
			};
			if (LocalAchievementsManager.<>f__mg$cache1 == null)
			{
				LocalAchievementsManager.<>f__mg$cache1 = new LoadCloudDataHandler(LocalAchievementsManager.onLoadedCloudData);
			}
			@interface.LoadCloudData(keys, LocalAchievementsManager.<>f__mg$cache1);
		}
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x0006F35C File Offset: 0x0006D55C
	public static void onLoadedCloudData(string[] data, CloudLoadResult result)
	{
		if (result == CloudLoadResult.Failed)
		{
			LocalAchievementsManager.loadFromCloud();
			return;
		}
		try
		{
			if (result == CloudLoadResult.NoData)
			{
				LocalAchievementsManager.achievementData = new LocalAchievementsManager.AchievementData();
				LocalAchievementsManager.saveToCloud();
			}
			else
			{
				LocalAchievementsManager.achievementData = JsonUtility.FromJson<LocalAchievementsManager.AchievementData>(data[0]);
			}
		}
		catch (ArgumentException)
		{
			LocalAchievementsManager.achievementData = new LocalAchievementsManager.AchievementData();
		}
	}

	// Token: 0x040004C9 RID: 1225
	public static readonly string CloudKey = "cuphead_ach";

	// Token: 0x040004CA RID: 1226
	public static readonly LocalAchievementsManager.Achievement[] DLCAchievements = new LocalAchievementsManager.Achievement[]
	{
		LocalAchievementsManager.Achievement.CompleteWorldDLC,
		LocalAchievementsManager.Achievement.ARankWorldDLC,
		LocalAchievementsManager.Achievement.DefeatBossAsChalice,
		LocalAchievementsManager.Achievement.DefeatXBossesAsChalice,
		LocalAchievementsManager.Achievement.ChaliceSuperWin,
		LocalAchievementsManager.Achievement.DefeatBossDLCWeapon,
		LocalAchievementsManager.Achievement.DefeatAllKOG,
		LocalAchievementsManager.Achievement.DefeatKOGGauntlet,
		LocalAchievementsManager.Achievement.DefeatSaltbaker,
		LocalAchievementsManager.Achievement.SRankAnyDLC,
		LocalAchievementsManager.Achievement.DefeatBossNoMinions,
		LocalAchievementsManager.Achievement.HP9,
		LocalAchievementsManager.Achievement.DefeatDevilPhase2,
		LocalAchievementsManager.Achievement.Paladin
	};

	// Token: 0x040004CC RID: 1228
	public static bool initialized;

	// Token: 0x040004CD RID: 1229
	public static LocalAchievementsManager.AchievementData achievementData;

	// Token: 0x040004CE RID: 1230
	[CompilerGenerated]
	private static SaveCloudDataHandler <>f__mg$cache0;

	// Token: 0x040004CF RID: 1231
	[CompilerGenerated]
	private static LoadCloudDataHandler <>f__mg$cache1;

	// Token: 0x020008C0 RID: 2240
	public enum Achievement
	{
		// Token: 0x040042ED RID: 17133
		DefeatBoss,
		// Token: 0x040042EE RID: 17134
		ParryApprentice,
		// Token: 0x040042EF RID: 17135
		ParryMaster,
		// Token: 0x040042F0 RID: 17136
		ExWin,
		// Token: 0x040042F1 RID: 17137
		SuperWin,
		// Token: 0x040042F2 RID: 17138
		ParryChain,
		// Token: 0x040042F3 RID: 17139
		NoHitsTaken,
		// Token: 0x040042F4 RID: 17140
		ARankWorld1,
		// Token: 0x040042F5 RID: 17141
		ARankWorld2,
		// Token: 0x040042F6 RID: 17142
		ARankWorld3,
		// Token: 0x040042F7 RID: 17143
		CompleteWorld1,
		// Token: 0x040042F8 RID: 17144
		CompleteWorld2,
		// Token: 0x040042F9 RID: 17145
		CompleteWorld3,
		// Token: 0x040042FA RID: 17146
		UnlockedAllSupers,
		// Token: 0x040042FB RID: 17147
		FoundAllLevelMoney,
		// Token: 0x040042FC RID: 17148
		BoughtAllItems,
		// Token: 0x040042FD RID: 17149
		CompleteDicePalace,
		// Token: 0x040042FE RID: 17150
		ARankWorld4,
		// Token: 0x040042FF RID: 17151
		GoodEnding,
		// Token: 0x04004300 RID: 17152
		SRank,
		// Token: 0x04004301 RID: 17153
		NewGamePlus,
		// Token: 0x04004302 RID: 17154
		FoundSecretPassage,
		// Token: 0x04004303 RID: 17155
		SmallPlaneOnlyWin,
		// Token: 0x04004304 RID: 17156
		FoundAllMoney,
		// Token: 0x04004305 RID: 17157
		PacifistRun,
		// Token: 0x04004306 RID: 17158
		NoHitsTakenDicePalace,
		// Token: 0x04004307 RID: 17159
		BadEnding,
		// Token: 0x04004308 RID: 17160
		CompleteDevil,
		// Token: 0x04004309 RID: 17161
		CompleteWorldDLC,
		// Token: 0x0400430A RID: 17162
		ARankWorldDLC,
		// Token: 0x0400430B RID: 17163
		DefeatBossAsChalice,
		// Token: 0x0400430C RID: 17164
		DefeatXBossesAsChalice,
		// Token: 0x0400430D RID: 17165
		ChaliceSuperWin,
		// Token: 0x0400430E RID: 17166
		DefeatBossDLCWeapon,
		// Token: 0x0400430F RID: 17167
		DefeatAllKOG,
		// Token: 0x04004310 RID: 17168
		DefeatKOGGauntlet,
		// Token: 0x04004311 RID: 17169
		DefeatSaltbaker,
		// Token: 0x04004312 RID: 17170
		SRankAnyDLC,
		// Token: 0x04004313 RID: 17171
		DefeatBossNoMinions,
		// Token: 0x04004314 RID: 17172
		HP9,
		// Token: 0x04004315 RID: 17173
		DefeatDevilPhase2,
		// Token: 0x04004316 RID: 17174
		Paladin
	}

	// Token: 0x020008C1 RID: 2241
	[Serializable]
	public class AchievementData
	{
		// Token: 0x04004317 RID: 17175
		public List<LocalAchievementsManager.Achievement> unlockedAchievements = new List<LocalAchievementsManager.Achievement>();

		// Token: 0x04004318 RID: 17176
		public int parryCount;
	}
}
