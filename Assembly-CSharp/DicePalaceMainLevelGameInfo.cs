using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001F9 RID: 505
public class DicePalaceMainLevelGameInfo : AbstractMonoBehaviour
{
	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06001749 RID: 5961 RVA: 0x000A12D0 File Offset: 0x0009F4D0
	public static DicePalaceMainLevelGameInfo GameInfo
	{
		get
		{
			if (DicePalaceMainLevelGameInfo.gameInfo == null)
			{
				DicePalaceMainLevelGameInfo.gameInfo = new GameObject
				{
					name = "GameInfo"
				}.AddComponent<DicePalaceMainLevelGameInfo>();
			}
			return DicePalaceMainLevelGameInfo.gameInfo;
		}
	}

	// Token: 0x0600174A RID: 5962 RVA: 0x00013D77 File Offset: 0x00011F77
	public override void Awake()
	{
		base.Awake();
		DicePalaceMainLevelGameInfo.gameInfo = this;
		DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = true;
		DicePalaceMainLevelGameInfo.SAFE_INDEXES = new List<int>();
		DicePalaceMainLevelGameInfo.ChooseHearts();
		Object.DontDestroyOnLoad(this);
	}

	// Token: 0x0600174B RID: 5963 RVA: 0x000A1310 File Offset: 0x0009F510
	public void CleanUp()
	{
		DicePalaceMainLevelGameInfo.SAFE_INDEXES.Clear();
		DicePalaceMainLevelGameInfo.TURN_COUNTER = 0;
		DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = 0;
		DicePalaceMainLevelGameInfo.ChooseHearts();
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = null;
		DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = null;
		DicePalaceMainLevelGameInfo.PLAYED_INTRO_SFX = false;
		DicePalaceMainLevelGameInfo.CHALICE_PLAYER = -1;
		DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = true;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600174C RID: 5964 RVA: 0x00013DA0 File Offset: 0x00011FA0
	public static void CleanUpRetry()
	{
		DicePalaceMainLevelGameInfo.SAFE_INDEXES.Clear();
		DicePalaceMainLevelGameInfo.TURN_COUNTER = 0;
		DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = 0;
		DicePalaceMainLevelGameInfo.ChooseHearts();
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = null;
		DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = null;
		DicePalaceMainLevelGameInfo.PLAYED_INTRO_SFX = false;
		DicePalaceMainLevelGameInfo.CHALICE_PLAYER = -1;
		DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = true;
	}

	// Token: 0x0600174D RID: 5965 RVA: 0x00013DDB File Offset: 0x00011FDB
	public static void ChooseHearts()
	{
		DicePalaceMainLevelGameInfo.HEART_INDEXES[0] = Random.Range(0, 3);
		DicePalaceMainLevelGameInfo.HEART_INDEXES[1] = Random.Range(4, 7);
		DicePalaceMainLevelGameInfo.HEART_INDEXES[2] = Random.Range(8, 11);
	}

	// Token: 0x0600174E RID: 5966 RVA: 0x000A1364 File Offset: 0x0009F564
	public static void SetPlayersStats()
	{
		if (DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS == null)
		{
			DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = new PlayersStatsBossesHub();
		}
		PlayerStatsManager stats = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats;
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.healerHP = stats.HealerHP;
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.healerHPReceived = stats.HealerHPReceived;
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.healerHPCounter = stats.HealerHPCounter;
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.HP = stats.Health;
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.SuperCharge = stats.SuperMeter;
		if (PlayerManager.Multiplayer)
		{
			if (DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS == null)
			{
				DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = new PlayersStatsBossesHub();
			}
			PlayerStatsManager stats2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats;
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.healerHP = stats2.HealerHP;
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.healerHPReceived = stats2.HealerHPReceived;
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.healerHPCounter = stats2.HealerHPCounter;
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.HP = stats2.Health;
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.SuperCharge = stats2.SuperMeter;
		}
	}

	// Token: 0x040012EC RID: 4844
	public static DicePalaceMainLevelGameInfo gameInfo;

	// Token: 0x040012ED RID: 4845
	public static int TURN_COUNTER;

	// Token: 0x040012EE RID: 4846
	public static int PLAYER_SPACES_MOVED;

	// Token: 0x040012EF RID: 4847
	public static List<int> SAFE_INDEXES;

	// Token: 0x040012F0 RID: 4848
	public static int[] HEART_INDEXES = new int[3];

	// Token: 0x040012F1 RID: 4849
	public static PlayersStatsBossesHub PLAYER_ONE_STATS;

	// Token: 0x040012F2 RID: 4850
	public static PlayersStatsBossesHub PLAYER_TWO_STATS;

	// Token: 0x040012F3 RID: 4851
	public static bool PLAYED_INTRO_SFX;

	// Token: 0x040012F4 RID: 4852
	public static bool IS_FIRST_ENTRY = true;

	// Token: 0x040012F5 RID: 4853
	public static int CHALICE_PLAYER = -1;
}
