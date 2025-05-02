using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003A3 RID: 931
public class TowerOfPowerLevelGameInfo : AbstractMonoBehaviour
{
	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06002933 RID: 10547 RVA: 0x000D09B0 File Offset: 0x000CEBB0
	public static TowerOfPowerLevelGameInfo GameInfo
	{
		get
		{
			if (TowerOfPowerLevelGameInfo.gameInfo == null)
			{
				TowerOfPowerLevelGameInfo.gameInfo = new GameObject
				{
					name = "GameInfo"
				}.AddComponent<TowerOfPowerLevelGameInfo>();
			}
			return TowerOfPowerLevelGameInfo.gameInfo;
		}
	}

	// Token: 0x06002934 RID: 10548 RVA: 0x00022B0E File Offset: 0x00020D0E
	public override void Awake()
	{
		base.Awake();
		TowerOfPowerLevelGameInfo.gameInfo = this;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[0] = null;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[1] = null;
		Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06002935 RID: 10549 RVA: 0x00022B32 File Offset: 0x00020D32
	public void CleanUp()
	{
		TowerOfPowerLevelGameInfo.TURN_COUNTER = 0;
		TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerOne);
		if (PlayerManager.Multiplayer)
		{
			TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerTwo);
		}
		TowerOfPowerLevelGameInfo.PLAYER_STATS[0] = null;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[1] = null;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002936 RID: 10550 RVA: 0x00022B6B File Offset: 0x00020D6B
	public static void ResetTowerOfPower()
	{
		TowerOfPowerLevelGameInfo.TURN_COUNTER = 0;
		TowerOfPowerLevelGameInfo.CURRENT_TURN = 0;
		TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerOne);
		if (PlayerManager.Multiplayer)
		{
			TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerTwo);
		}
		TowerOfPowerLevelGameInfo.PLAYER_STATS[0] = null;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[1] = null;
	}

	// Token: 0x06002937 RID: 10551 RVA: 0x000D09F0 File Offset: 0x000CEBF0
	public static void InitAddedPlayer(PlayerId playerId, int startingToken)
	{
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId] = new PlayersStatsBossesHub();
		if (TowerOfPowerLevelGameInfo.CURRENT_TURN == 0)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].HP = 3;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BonusHP = 3;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].SuperCharge = 0f;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].tokenCount = startingToken;
		}
		else
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].HP = 1;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BonusHP = 0;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].SuperCharge = 0f;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].tokenCount = 0;
		}
	}

	// Token: 0x06002938 RID: 10552 RVA: 0x000D0A88 File Offset: 0x000CEC88
	public static void ResetWeapons(PlayerId playerId)
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId] == null)
		{
			return;
		}
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId);
		playerLoadout.primaryWeapon = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].basePrimaryWeapon;
		playerLoadout.secondaryWeapon = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].baseSecondaryWeapon;
		playerLoadout.super = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BaseSuper;
		playerLoadout.charm = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BaseCharm;
		PlayerData.SaveCurrentFile();
	}

	// Token: 0x06002939 RID: 10553 RVA: 0x000D0B04 File Offset: 0x000CED04
	public static void InitEquipment(PlayerId playerId)
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId] == null)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId] = new PlayersStatsBossesHub();
		}
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId);
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].basePrimaryWeapon = playerLoadout.primaryWeapon;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].baseSecondaryWeapon = playerLoadout.secondaryWeapon;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BaseSuper = playerLoadout.super;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BaseCharm = playerLoadout.charm;
	}

	// Token: 0x0600293A RID: 10554 RVA: 0x000D0B84 File Offset: 0x000CED84
	public static void SetPlayersStats(PlayerId playerId)
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId] == null)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId] = new PlayersStatsBossesHub();
		}
		PlayerStatsManager stats = PlayerManager.GetPlayer(playerId).stats;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].HP = stats.Health;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].SuperCharge = stats.SuperMeter;
	}

	// Token: 0x0600293B RID: 10555 RVA: 0x000D0BDC File Offset: 0x000CEDDC
	public static void AddToken()
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[0] == null)
		{
			return;
		}
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[0].HP > 0)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[0].tokenCount++;
		}
		if (PlayerManager.Multiplayer && TowerOfPowerLevelGameInfo.PLAYER_STATS[1].HP > 0)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[1].tokenCount++;
		}
	}

	// Token: 0x0600293C RID: 10556 RVA: 0x000D0C4C File Offset: 0x000CEE4C
	public static void ReduceToken()
	{
		TowerOfPowerLevelGameInfo.PLAYER_STATS[0].tokenCount--;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[0].tokenCount = Mathf.Max(0, TowerOfPowerLevelGameInfo.PLAYER_STATS[0].tokenCount);
		if (PlayerManager.Multiplayer)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[1].tokenCount--;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[1].tokenCount = Mathf.Max(0, TowerOfPowerLevelGameInfo.PLAYER_STATS[1].tokenCount);
		}
	}

	// Token: 0x0600293D RID: 10557 RVA: 0x00022B9F File Offset: 0x00020D9F
	public static void ReduceToken(int playerNum)
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[playerNum] == null || TowerOfPowerLevelGameInfo.PLAYER_STATS[playerNum].tokenCount == 0)
		{
			return;
		}
		TowerOfPowerLevelGameInfo.PLAYER_STATS[playerNum].tokenCount--;
	}

	// Token: 0x0600293E RID: 10558 RVA: 0x000D0CC8 File Offset: 0x000CEEC8
	public static void SetDefaultToken(int defaultTokenCount)
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[0] == null)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[0] = new PlayersStatsBossesHub();
		}
		TowerOfPowerLevelGameInfo.PLAYER_STATS[0].tokenCount = defaultTokenCount;
		if (PlayerManager.Multiplayer)
		{
			if (TowerOfPowerLevelGameInfo.PLAYER_STATS[1] == null)
			{
				TowerOfPowerLevelGameInfo.PLAYER_STATS[1] = new PlayersStatsBossesHub();
			}
			TowerOfPowerLevelGameInfo.PLAYER_STATS[1].tokenCount = defaultTokenCount;
		}
	}

	// Token: 0x0600293F RID: 10559 RVA: 0x000D0D2C File Offset: 0x000CEF2C
	public static bool IsTokenLeft()
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[0] == null && TowerOfPowerLevelGameInfo.PLAYER_STATS[1] == null)
		{
			return false;
		}
		int tokenCount = TowerOfPowerLevelGameInfo.PLAYER_STATS[0].tokenCount;
		int num = (!PlayerManager.Multiplayer) ? 0 : TowerOfPowerLevelGameInfo.PLAYER_STATS[1].tokenCount;
		return tokenCount > 0 || num > 0;
	}

	// Token: 0x06002940 RID: 10560 RVA: 0x00022BD3 File Offset: 0x00020DD3
	public static bool IsTokenLeft(int playerNum)
	{
		return TowerOfPowerLevelGameInfo.PLAYER_STATS[playerNum] != null && TowerOfPowerLevelGameInfo.PLAYER_STATS[playerNum].tokenCount != 0;
	}

	// Token: 0x06002941 RID: 10561 RVA: 0x00022BF5 File Offset: 0x00020DF5
	public void OnDestroy()
	{
		TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerOne);
		if (PlayerManager.Multiplayer)
		{
			TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerTwo);
		}
	}

	// Token: 0x04002286 RID: 8838
	public static TowerOfPowerLevelGameInfo gameInfo;

	// Token: 0x04002287 RID: 8839
	public static List<Levels> allStageSpaces = new List<Levels>();

	// Token: 0x04002288 RID: 8840
	public static Level.Mode[] difficultyByBossIndex;

	// Token: 0x04002289 RID: 8841
	public static List<string> SlotOne = new List<string>();

	// Token: 0x0400228A RID: 8842
	public static List<string> SlotTwo = new List<string>();

	// Token: 0x0400228B RID: 8843
	public static List<string> SlotThree = new List<string>();

	// Token: 0x0400228C RID: 8844
	public static List<string> SlotThreeChalice = new List<string>();

	// Token: 0x0400228D RID: 8845
	public static List<string> SlotFour = new List<string>();

	// Token: 0x0400228E RID: 8846
	public static List<string> SlotFourChalice = new List<string>();

	// Token: 0x0400228F RID: 8847
	public static Level.Mode baseDifficulty;

	// Token: 0x04002290 RID: 8848
	public static int CURRENT_TURN;

	// Token: 0x04002291 RID: 8849
	public static int TURN_COUNTER;

	// Token: 0x04002292 RID: 8850
	public static int MIN_RANK_NEED_TO_GET_TOKEN;

	// Token: 0x04002293 RID: 8851
	public static PlayersStatsBossesHub[] PLAYER_STATS = new PlayersStatsBossesHub[2];
}
