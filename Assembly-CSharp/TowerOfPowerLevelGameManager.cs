using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020003A4 RID: 932
public class TowerOfPowerLevelGameManager : LevelProperties.TowerOfPower.Entity
{
	// Token: 0x06002944 RID: 10564 RVA: 0x000D0E74 File Offset: 0x000CF074
	public override void LevelInit(LevelProperties.TowerOfPower properties)
	{
		base.LevelInit(properties);
		this.anyInput = new CupheadInput.AnyPlayerInput(false);
		TowerOfPowerLevelGameInfo.CURRENT_TURN = TowerOfPowerLevelGameInfo.TURN_COUNTER;
		if (TowerOfPowerLevelGameInfo.CURRENT_TURN == 0)
		{
			TowerOfPowerLevelGameInfo.baseDifficulty = Level.Current.mode;
			TowerOfPowerLevelGameInfo.SetDefaultToken(properties.CurrentState.slotMachine.DefaultStartingToken);
			TowerOfPowerLevelGameInfo.MIN_RANK_NEED_TO_GET_TOKEN = properties.CurrentState.slotMachine.MinRankToGainToken;
			this.InitDifficultyBossByIndex();
			this.InitPools();
			this.SetTowerBosses();
			this.InitSlotMachine();
			TowerOfPowerLevelGameInfo.InitEquipment(PlayerId.PlayerOne);
			if (PlayerManager.Multiplayer)
			{
				TowerOfPowerLevelGameInfo.InitEquipment(PlayerId.PlayerTwo);
			}
			if (this.debugSkipToLastFight)
			{
				TowerOfPowerLevelGameInfo.TURN_COUNTER = TowerOfPowerLevelGameInfo.allStageSpaces.Count - 1;
				TowerOfPowerLevelGameInfo.CURRENT_TURN = TowerOfPowerLevelGameInfo.TURN_COUNTER;
			}
		}
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06002945 RID: 10565 RVA: 0x000D0F48 File Offset: 0x000CF148
	public void ChangePlayersWeapon(PlayerId playerId)
	{
		if (playerId == PlayerId.PlayerTwo && !PlayerManager.Multiplayer)
		{
			return;
		}
		bool flag = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BaseCharm == Charm.charm_chalice;
		this.bonusHP[(int)playerId] = 0;
		this.bonusToken[(int)playerId] = 0;
		this.SlotMachineWeapon2Attempt[(int)playerId] = 0;
		TowerOfPowerLevelGameManager.Weapon_Slot weaponSlotEnumByName = TowerOfPowerLevelGameManager.GetWeaponSlotEnumByName(TowerOfPowerLevelGameInfo.SlotOne.RandomChoice<string>());
		int count = TowerOfPowerLevelGameInfo.SlotTwo.Count;
		int num = Random.Range(0, count);
		TowerOfPowerLevelGameManager.Weapon_Slot weaponSlotEnumByName2 = TowerOfPowerLevelGameManager.GetWeaponSlotEnumByName(TowerOfPowerLevelGameInfo.SlotTwo[num]);
		while (weaponSlotEnumByName2 == weaponSlotEnumByName)
		{
			num++;
			this.SlotMachineWeapon2Attempt[(int)playerId]++;
			if (num >= count)
			{
				num = 0;
			}
			weaponSlotEnumByName2 = TowerOfPowerLevelGameManager.GetWeaponSlotEnumByName(TowerOfPowerLevelGameInfo.SlotTwo[num]);
			if (this.SlotMachineWeapon2Attempt[(int)playerId] == count)
			{
				Debug.LogError("The slotTwo list needs at least two kinds of weapon. Modify the Tower of Power in the Level Editor--Slot Two weapon in the SlotMachine section.", null);
				break;
			}
		}
		TowerOfPowerLevelGameManager.Charm_Slot charmSlotEnumByName;
		TowerOfPowerLevelGameManager.Super_Slot superSlotEnumByName;
		if (flag)
		{
			charmSlotEnumByName = TowerOfPowerLevelGameManager.GetCharmSlotEnumByName(TowerOfPowerLevelGameInfo.SlotThreeChalice.RandomChoice<string>());
			superSlotEnumByName = TowerOfPowerLevelGameManager.GetSuperSlotEnumByName(TowerOfPowerLevelGameInfo.SlotFourChalice.RandomChoice<string>());
		}
		else
		{
			charmSlotEnumByName = TowerOfPowerLevelGameManager.GetCharmSlotEnumByName(TowerOfPowerLevelGameInfo.SlotThree.RandomChoice<string>());
			superSlotEnumByName = TowerOfPowerLevelGameManager.GetSuperSlotEnumByName(TowerOfPowerLevelGameInfo.SlotFour.RandomChoice<string>());
		}
		if (charmSlotEnumByName == TowerOfPowerLevelGameManager.Charm_Slot.charm_extra_token)
		{
			this.bonusToken[(int)playerId] = 1;
		}
		if (charmSlotEnumByName == TowerOfPowerLevelGameManager.Charm_Slot.charm_health_up_1)
		{
			this.bonusHP[(int)playerId] = 1;
		}
		else if (charmSlotEnumByName == TowerOfPowerLevelGameManager.Charm_Slot.charm_health_up_2)
		{
			this.bonusHP[(int)playerId] = 2;
		}
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId);
		playerLoadout.primaryWeapon = TowerOfPowerLevelGameManager.GetWeaponEnumByName(weaponSlotEnumByName.ToString());
		playerLoadout.secondaryWeapon = TowerOfPowerLevelGameManager.GetWeaponEnumByName(weaponSlotEnumByName2.ToString());
		playerLoadout.charm = TowerOfPowerLevelGameManager.GetCharmEnumByName(charmSlotEnumByName.ToString());
		playerLoadout.super = TowerOfPowerLevelGameManager.GetSuperEnumByName(superSlotEnumByName.ToString());
	}

	// Token: 0x06002946 RID: 10566 RVA: 0x000D112C File Offset: 0x000CF32C
	public IEnumerator startMiniBoss_cr(Levels level)
	{
		TowerOfPowerLevelGameInfo.SetPlayersStats(PlayerId.PlayerOne);
		if (PlayerManager.Multiplayer)
		{
			TowerOfPowerLevelGameInfo.SetPlayersStats(PlayerId.PlayerTwo);
		}
		Level.ScoringData.time += Level.Current.LevelTime;
		SceneLoader.LoadLevel(level, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
		yield return null;
		yield break;
	}

	// Token: 0x06002947 RID: 10567 RVA: 0x000D1148 File Offset: 0x000CF348
	public static TowerOfPowerLevelGameManager.Weapon_Slot GetWeaponSlotEnumByName(string Name)
	{
		return (TowerOfPowerLevelGameManager.Weapon_Slot)Enum.Parse(typeof(TowerOfPowerLevelGameManager.Weapon_Slot), Name);
	}

	// Token: 0x06002948 RID: 10568 RVA: 0x000D116C File Offset: 0x000CF36C
	public static TowerOfPowerLevelGameManager.Charm_Slot GetCharmSlotEnumByName(string Name)
	{
		return (TowerOfPowerLevelGameManager.Charm_Slot)Enum.Parse(typeof(TowerOfPowerLevelGameManager.Charm_Slot), Name);
	}

	// Token: 0x06002949 RID: 10569 RVA: 0x000D1190 File Offset: 0x000CF390
	public static TowerOfPowerLevelGameManager.Super_Slot GetSuperSlotEnumByName(string Name)
	{
		return (TowerOfPowerLevelGameManager.Super_Slot)Enum.Parse(typeof(TowerOfPowerLevelGameManager.Super_Slot), Name);
	}

	// Token: 0x0600294A RID: 10570 RVA: 0x000D11B4 File Offset: 0x000CF3B4
	public static Weapon GetWeaponEnumByName(string Name)
	{
		Weapon result = Weapon.None;
		if (Enum.IsDefined(typeof(Weapon), Name))
		{
			result = (Weapon)Enum.Parse(typeof(Weapon), Name);
		}
		return result;
	}

	// Token: 0x0600294B RID: 10571 RVA: 0x000D11F4 File Offset: 0x000CF3F4
	public static Charm GetCharmEnumByName(string Name)
	{
		Charm result = Charm.None;
		if (Enum.IsDefined(typeof(Charm), Name))
		{
			result = (Charm)Enum.Parse(typeof(Charm), Name);
		}
		return result;
	}

	// Token: 0x0600294C RID: 10572 RVA: 0x000D1234 File Offset: 0x000CF434
	public static Super GetSuperEnumByName(string Name)
	{
		Super result = Super.None;
		if (Enum.IsDefined(typeof(Super), Name))
		{
			result = (Super)Enum.Parse(typeof(Super), Name);
		}
		return result;
	}

	// Token: 0x0600294D RID: 10573 RVA: 0x00022C0D File Offset: 0x00020E0D
	public void InitPools()
	{
		this.InitBossPools();
		this.InitShmupPools();
		this.InitKingDicePools();
	}

	// Token: 0x0600294E RID: 10574 RVA: 0x000D1274 File Offset: 0x000CF474
	public void InitBossPools()
	{
		string[] array = base.properties.CurrentState.bossesPropertises.PoolOneString.Split(new char[]
		{
			','
		});
		string[] array2 = base.properties.CurrentState.bossesPropertises.PoolTwoString.Split(new char[]
		{
			','
		});
		string[] array3 = base.properties.CurrentState.bossesPropertises.PoolThreeString.Split(new char[]
		{
			','
		});
		this.BossPools = new List<Levels>[3];
		this.BossPools[0] = new List<Levels>();
		for (int i = 0; i < array.Length; i++)
		{
			this.BossPools[0].Add(Level.GetEnumByName(array[i]));
		}
		this.BossPools[1] = new List<Levels>();
		for (int j = 0; j < array2.Length; j++)
		{
			this.BossPools[1].Add(Level.GetEnumByName(array2[j]));
		}
		this.BossPools[2] = new List<Levels>();
		for (int k = 0; k < array3.Length; k++)
		{
			this.BossPools[2].Add(Level.GetEnumByName(array3[k]));
		}
	}

	// Token: 0x0600294F RID: 10575 RVA: 0x000D13AC File Offset: 0x000CF5AC
	public void InitShmupPools()
	{
		this.ShmupPlacement.Clear();
		string[] array = base.properties.CurrentState.bossesPropertises.ShmupPoolOneString.Split(new char[]
		{
			','
		});
		string[] array2 = base.properties.CurrentState.bossesPropertises.ShmupPoolTwoString.Split(new char[]
		{
			','
		});
		string[] array3 = base.properties.CurrentState.bossesPropertises.ShmupPoolThreeString.Split(new char[]
		{
			','
		});
		List<string> list = base.properties.CurrentState.bossesPropertises.ShmupPlacementString.Split(new char[]
		{
			','
		}).ToList<string>();
		string shmupCountString = base.properties.CurrentState.bossesPropertises.ShmupCountString;
		string[] array4 = shmupCountString.Split(new char[]
		{
			','
		});
		int num = Parser.IntParse(array4[Random.Range(0, array4.Length)]);
		if (num > 0)
		{
			do
			{
				int placement = Parser.IntParse(list[Random.Range(0, list.Count)]);
				this.ShmupPlacement.Add(placement);
				list.RemoveAll((string x) => x == placement.ToString());
			}
			while (this.ShmupPlacement.Count < num && list.Count != 0);
		}
		this.ShmupPools = new List<Levels>[3];
		this.ShmupPools[0] = new List<Levels>();
		for (int i = 0; i < array.Length; i++)
		{
			this.ShmupPools[0].Add(Level.GetEnumByName(array[i]));
		}
		this.ShmupPools[1] = new List<Levels>();
		for (int j = 0; j < array2.Length; j++)
		{
			this.ShmupPools[1].Add(Level.GetEnumByName(array2[j]));
		}
		this.ShmupPools[2] = new List<Levels>();
		for (int k = 0; k < array3.Length; k++)
		{
			this.ShmupPools[2].Add(Level.GetEnumByName(array3[k]));
		}
	}

	// Token: 0x06002950 RID: 10576 RVA: 0x000D15D0 File Offset: 0x000CF7D0
	public void InitKingDicePools()
	{
		string[] array = base.properties.CurrentState.bossesPropertises.KingDicePoolOneString.Split(new char[]
		{
			','
		});
		string[] array2 = base.properties.CurrentState.bossesPropertises.KingDicePoolTwoString.Split(new char[]
		{
			','
		});
		string[] array3 = base.properties.CurrentState.bossesPropertises.KingDicePoolThreeString.Split(new char[]
		{
			','
		});
		string[] array4 = base.properties.CurrentState.bossesPropertises.KingDicePoolFourString.Split(new char[]
		{
			','
		});
		int kingDiceMiniBossCount = base.properties.CurrentState.bossesPropertises.KingDiceMiniBossCount;
		this.KingDicePools = new List<Levels>[4];
		this.KingDicePools[0] = new List<Levels>();
		for (int i = 0; i < array.Length; i++)
		{
			this.KingDicePools[0].Add(Level.GetEnumByName(array[i]));
		}
		this.KingDicePools[1] = new List<Levels>();
		for (int j = 0; j < array2.Length; j++)
		{
			this.KingDicePools[1].Add(Level.GetEnumByName(array2[j]));
		}
		this.KingDicePools[2] = new List<Levels>();
		for (int k = 0; k < array3.Length; k++)
		{
			this.KingDicePools[2].Add(Level.GetEnumByName(array3[k]));
		}
		this.KingDicePools[3] = new List<Levels>();
		for (int l = 0; l < array4.Length; l++)
		{
			this.KingDicePools[3].Add(Level.GetEnumByName(array4[l]));
		}
	}

	// Token: 0x06002951 RID: 10577 RVA: 0x000D1784 File Offset: 0x000CF984
	public void SetTowerBosses()
	{
		TowerOfPowerLevelGameInfo.allStageSpaces.Clear();
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (i == 2 && j == 2)
				{
					int kingDiceMiniBossCount = base.properties.CurrentState.bossesPropertises.KingDiceMiniBossCount;
					for (int k = 0; k < kingDiceMiniBossCount; k++)
					{
						this.SetKingDiceBosses(k);
					}
					TowerOfPowerLevelGameInfo.allStageSpaces.Add(Levels.DicePalaceMain);
					if (base.properties.CurrentState.bossesPropertises.DevilFinalBoss)
					{
						TowerOfPowerLevelGameInfo.allStageSpaces.Add(Levels.Devil);
					}
				}
				else
				{
					int count = TowerOfPowerLevelGameInfo.allStageSpaces.Count;
					if (this.ShmupPlacement.Contains(count + 1))
					{
						this.AddShmupInTower(this.ShmupPlacement.IndexOf(count + 1));
					}
					else
					{
						this.AddBossInTower(i);
					}
				}
			}
		}
	}

	// Token: 0x06002952 RID: 10578 RVA: 0x000D187C File Offset: 0x000CFA7C
	public void AddBossInTower(int tier)
	{
		this.BossPools[tier].RemoveAll((Levels x) => TowerOfPowerLevelGameInfo.allStageSpaces.Contains(x));
		if (this.BossPools[tier].Count == 0)
		{
			Debug.LogError("Number of Boss in the pool " + tier + " is empty.", null);
			return;
		}
		Levels randLv = this.BossPools[tier].RandomChoice<Levels>();
		if (TowerOfPowerLevelGameInfo.allStageSpaces.Contains(randLv))
		{
			Debug.LogError("RemoveAll(x => allStageSpaces.Contains(x) don't work like experted", null);
		}
		else
		{
			TowerOfPowerLevelGameInfo.allStageSpaces.Add(randLv);
			this.BossPools[tier].RemoveAll((Levels x) => x == randLv);
		}
	}

	// Token: 0x06002953 RID: 10579 RVA: 0x000D194C File Offset: 0x000CFB4C
	public void AddShmupInTower(int tier)
	{
		this.ShmupPools[tier].RemoveAll((Levels x) => TowerOfPowerLevelGameInfo.allStageSpaces.Contains(x));
		if (this.ShmupPools[tier].Count == 0)
		{
			Debug.LogError("Number of Boss in the pool " + tier + " is empty.", null);
			return;
		}
		Levels randLv = this.ShmupPools[tier].RandomChoice<Levels>();
		if (TowerOfPowerLevelGameInfo.allStageSpaces.Contains(randLv))
		{
			Debug.LogError("RemoveAll(x => allStageSpaces.Contains(x) don't work like experted", null);
		}
		else
		{
			TowerOfPowerLevelGameInfo.allStageSpaces.Add(randLv);
			this.ShmupPools[tier].RemoveAll((Levels x) => x == randLv);
		}
	}

	// Token: 0x06002954 RID: 10580 RVA: 0x000D1A1C File Offset: 0x000CFC1C
	public void SetKingDiceBosses(int tier)
	{
		this.KingDicePools[tier].RemoveAll((Levels x) => TowerOfPowerLevelGameInfo.allStageSpaces.Contains(x));
		if (this.KingDicePools[tier].Count == 0)
		{
			Debug.LogError("Number of Boss in the pool " + tier + " is empty.", null);
			return;
		}
		Levels randLv = this.KingDicePools[tier].RandomChoice<Levels>();
		if (TowerOfPowerLevelGameInfo.allStageSpaces.Contains(randLv))
		{
			Debug.LogError("RemoveAll(x => allStageSpaces.Contains(x) don't work like expected", null);
		}
		else
		{
			TowerOfPowerLevelGameInfo.allStageSpaces.Add(randLv);
			this.KingDicePools[tier].RemoveAll((Levels x) => x == randLv);
		}
	}

	// Token: 0x06002955 RID: 10581 RVA: 0x000D1AEC File Offset: 0x000CFCEC
	public void InitDifficultyBossByIndex()
	{
		string[] array = base.properties.CurrentState.bossesPropertises.MiniBossDifficultyByIndex.Split(new char[]
		{
			','
		});
		TowerOfPowerLevelGameInfo.difficultyByBossIndex = new Level.Mode[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			int num = 0;
			Parser.IntTryParse(array[i], out num);
			TowerOfPowerLevelGameInfo.difficultyByBossIndex[i] = (Level.Mode)num;
		}
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x000D1B58 File Offset: 0x000CFD58
	public void InitSlotMachine()
	{
		string[] array = base.properties.CurrentState.slotMachine.SlotOneWeapon.Split(new char[]
		{
			','
		});
		string[] array2 = base.properties.CurrentState.slotMachine.SlotTwoWeapon.Split(new char[]
		{
			','
		});
		string[] array3 = base.properties.CurrentState.slotMachine.SlotThreeCharm.Split(new char[]
		{
			','
		});
		string[] array4 = base.properties.CurrentState.slotMachine.SlotThreeChalice.Split(new char[]
		{
			','
		});
		string[] array5 = base.properties.CurrentState.slotMachine.SlotFourSuper.Split(new char[]
		{
			','
		});
		string[] array6 = base.properties.CurrentState.slotMachine.SlotFourChalice.Split(new char[]
		{
			','
		});
		for (int i = 0; i < array.Length; i++)
		{
			string item = (!(array[i] != "None")) ? array[i] : ("level_weapon_" + array[i]);
			TowerOfPowerLevelGameInfo.SlotOne.Add(item);
		}
		for (int j = 0; j < array2.Length; j++)
		{
			string item = (!(array2[j] != "None")) ? array2[j] : ("level_weapon_" + array2[j]);
			TowerOfPowerLevelGameInfo.SlotTwo.Add(item);
		}
		for (int k = 0; k < array3.Length; k++)
		{
			string item = (!(array3[k] != "None")) ? array3[k] : ("charm_" + array3[k]);
			TowerOfPowerLevelGameInfo.SlotThree.Add(item);
		}
		for (int l = 0; l < array4.Length; l++)
		{
			string item = (!(array4[l] != "None")) ? array4[l] : ("charm_" + array4[l]);
			TowerOfPowerLevelGameInfo.SlotThreeChalice.Add(item);
		}
		for (int m = 0; m < array5.Length; m++)
		{
			string item = (!(array5[m] != "None")) ? array5[m] : ("level_super_" + array5[m]);
			TowerOfPowerLevelGameInfo.SlotFour.Add(item);
		}
		for (int n = 0; n < array6.Length; n++)
		{
			string item = (!(array6[n] != "None")) ? array6[n] : ("level_super_chalice_" + array6[n]);
			TowerOfPowerLevelGameInfo.SlotFourChalice.Add(item);
		}
	}

	// Token: 0x06002957 RID: 10583 RVA: 0x000D1E34 File Offset: 0x000D0034
	public void SetDifficulty()
	{
		int num = TowerOfPowerLevelGameInfo.CURRENT_TURN;
		if (num >= TowerOfPowerLevelGameInfo.difficultyByBossIndex.Length)
		{
			num = TowerOfPowerLevelGameInfo.difficultyByBossIndex.Length - 1;
		}
		Level.SetCurrentMode(TowerOfPowerLevelGameInfo.difficultyByBossIndex[num]);
	}

	// Token: 0x06002958 RID: 10584 RVA: 0x000D1E6C File Offset: 0x000D006C
	public void RevivePlayer(PlayerId playerId)
	{
		PlayerStatsManager stats = PlayerManager.GetPlayer(playerId).stats;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].HP = 3;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BonusHP = 3;
		stats.SetHealth(3);
	}

	// Token: 0x06002959 RID: 10585 RVA: 0x000D1EA8 File Offset: 0x000D00A8
	public IEnumerator main_cr()
	{
		int turn = TowerOfPowerLevelGameInfo.CURRENT_TURN;
		List<Levels> allStage = TowerOfPowerLevelGameInfo.allStageSpaces;
		if (turn > 0)
		{
			this.showingScorecard = true;
			while (SceneLoader.CurrentlyLoading)
			{
				yield return null;
			}
			this.scorecard.gameObject.SetActive(true);
			while (!this.scorecard.done)
			{
				yield return null;
			}
			this.showingScorecard = false;
			this.scorecard.gameObject.SetActive(false);
			if (PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead && TowerOfPowerLevelGameInfo.IsTokenLeft(0))
			{
				TowerOfPowerLevelGameInfo.ReduceToken(0);
				this.RevivePlayer(PlayerId.PlayerOne);
			}
			if (PlayerManager.Multiplayer && PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead && TowerOfPowerLevelGameInfo.IsTokenLeft(1))
			{
				TowerOfPowerLevelGameInfo.ReduceToken(1);
				this.RevivePlayer(PlayerId.PlayerTwo);
			}
		}
		if ((turn != 0 && turn % 3 == 0 && turn < 8) || allStage[turn] == Levels.Devil || this.debugForceSlotMachineEveryTurn || (turn == 1 && this.debugForceSlotMachineAfterOneFight))
		{
			if (!PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead)
			{
				this.ChangePlayersWeapon(PlayerId.PlayerOne);
				this.slotsDone[0] = false;
				base.StartCoroutine(this.play_slot_machine_cr(PlayerId.PlayerOne));
			}
			else
			{
				this.slotsDone[0] = true;
			}
			if (PlayerManager.Multiplayer && !PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead)
			{
				this.slotsDone[1] = false;
				this.ChangePlayersWeapon(PlayerId.PlayerTwo);
				base.StartCoroutine(this.play_slot_machine_cr(PlayerId.PlayerTwo));
			}
			else
			{
				this.slotsDone[1] = true;
			}
		}
		else
		{
			base.StartCoroutine(this.go_to_next_level_cr());
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600295A RID: 10586 RVA: 0x000D1EC4 File Offset: 0x000D00C4
	public IEnumerator spin_slot_machine_cr(PlayerId playerId)
	{
		yield return null;
		yield break;
	}

	// Token: 0x0600295B RID: 10587 RVA: 0x000D1ED8 File Offset: 0x000D00D8
	public IEnumerator stop_slot_machine_cr(PlayerId playerId)
	{
		for (;;)
		{
			if (PauseManager.state == PauseManager.State.Paused)
			{
				this.waitForButtonRelease = 3;
				yield return null;
			}
			if (PlayerManager.GetPlayer(playerId).input.actions.GetButtonUp(13))
			{
				this.waitForButtonRelease = 0;
			}
			if (this.waitForButtonRelease == 0 && PlayerManager.GetPlayer(playerId).input.actions.GetButtonDown(13))
			{
				break;
			}
			if (this.waitForButtonRelease > 0)
			{
				this.waitForButtonRelease--;
			}
			yield return this.slowdown_slots_cr();
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600295C RID: 10588 RVA: 0x000D1EFC File Offset: 0x000D00FC
	public IEnumerator slowdown_slots_cr()
	{
		yield return null;
		yield break;
	}

	// Token: 0x0600295D RID: 10589 RVA: 0x000D1F10 File Offset: 0x000D0110
	public IEnumerator play_slot_machine_cr(PlayerId playerId)
	{
		yield return this.spin_slot_machine_cr(playerId);
		this.slotsConfirm[(int)playerId] = false;
		this.slotsCanSpinAgain[(int)playerId] = false;
		this.slotsAreSpinning[(int)playerId] = true;
		yield return this.stop_slot_machine_cr(playerId);
		this.slotsAreSpinning[(int)playerId] = false;
		this.slotsConfirm[(int)playerId] = true;
		bool playAgain = false;
		while (!PlayerManager.GetPlayerInput(playerId).GetButtonDown(13))
		{
			if (TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].tokenCount > 0)
			{
				this.slotsCanSpinAgain[(int)playerId] = true;
				if (PlayerManager.GetPlayer(playerId).input.actions.GetButtonDown(7))
				{
					playAgain = true;
					TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].tokenCount--;
					this.ChangePlayersWeapon(playerId);
					IL_1B9:
					this.slotsConfirm[(int)playerId] = false;
					this.slotsCanSpinAgain[(int)playerId] = false;
					yield return null;
					if (playAgain)
					{
						yield return this.play_slot_machine_cr(playerId);
					}
					else
					{
						this.slotsDone[(int)playerId] = true;
						bool chaliceCharmEquipped = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BaseCharm == Charm.charm_chalice;
						PlayerData.PlayerLoadouts.PlayerLoadout P1loadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId);
						if (this.bonusHP[(int)playerId] > 0)
						{
							P1loadout.charm = ((!chaliceCharmEquipped) ? Charm.None : Charm.charm_chalice);
						}
						if (this.bonusToken[(int)playerId] > 0)
						{
							P1loadout.charm = ((!chaliceCharmEquipped) ? Charm.None : Charm.charm_chalice);
						}
						PlayerStatsManager playerStats = PlayerManager.GetPlayer(playerId).stats;
						int hp = playerStats.Health + this.bonusHP[(int)playerId];
						if (hp > 8)
						{
							hp = 8;
						}
						TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].HP = hp;
						TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BonusHP = hp - 3;
						playerStats.SetHealth(hp);
						this.bonusHP[(int)playerId] = 0;
						TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].tokenCount += this.bonusToken[(int)playerId];
						this.bonusToken[(int)playerId] = 0;
						if (this.slotsDone[0] && this.slotsDone[1])
						{
							yield return this.go_to_next_level_cr();
						}
					}
					yield break;
				}
			}
			yield return null;
		}
		goto IL_1B9;
	}

	// Token: 0x0600295E RID: 10590 RVA: 0x00022C21 File Offset: 0x00020E21
	public bool SlotsDone()
	{
		if (PlayerManager.Multiplayer)
		{
			return this.slotsDone[0] && this.slotsDone[1];
		}
		return this.slotsDone[0];
	}

	// Token: 0x0600295F RID: 10591 RVA: 0x000D1F34 File Offset: 0x000D0134
	public IEnumerator go_to_next_level_cr()
	{
		while (SceneLoader.CurrentlyLoading)
		{
			yield return null;
		}
		for (;;)
		{
			if (PauseManager.state == PauseManager.State.Paused)
			{
				this.waitForButtonRelease = 3;
				yield return null;
			}
			if (this.anyInput.GetButtonUp(CupheadButton.Accept))
			{
				this.waitForButtonRelease = 0;
			}
			if (this.waitForButtonRelease == 0 && this.anyInput.GetButtonDown(CupheadButton.Accept))
			{
				break;
			}
			if (this.waitForButtonRelease > 0)
			{
				this.waitForButtonRelease--;
			}
			yield return null;
		}
		int currentLevel = TowerOfPowerLevelGameInfo.CURRENT_TURN;
		if (TowerOfPowerLevelGameInfo.CURRENT_TURN < TowerOfPowerLevelGameInfo.allStageSpaces.Count)
		{
			if (TowerOfPowerLevelGameInfo.PLAYER_STATS[0] != null)
			{
				this.SetDifficulty();
				yield return base.StartCoroutine(this.startMiniBoss_cr(TowerOfPowerLevelGameInfo.allStageSpaces[currentLevel]));
			}
		}
		else
		{
			SceneLoader.LoadLastMap();
		}
		yield break;
	}

	// Token: 0x04002294 RID: 8852
	public const string PREWEAPON_NAME = "level_weapon_";

	// Token: 0x04002295 RID: 8853
	public const string PRESUPER_NAME = "level_super_";

	// Token: 0x04002296 RID: 8854
	public const string PRESUPER_CHALICE_NAME = "level_super_chalice_";

	// Token: 0x04002297 RID: 8855
	public const string PRECHARM_NAME = "charm_";

	// Token: 0x04002298 RID: 8856
	public const string PRECHARM_CHALICE_NAME = "charm_chalice_";

	// Token: 0x04002299 RID: 8857
	[SerializeField]
	public float advanceDelay = 10f;

	// Token: 0x0400229A RID: 8858
	public CupheadInput.AnyPlayerInput anyInput;

	// Token: 0x0400229B RID: 8859
	public int[] bonusHP = new int[2];

	// Token: 0x0400229C RID: 8860
	public int[] bonusToken = new int[2];

	// Token: 0x0400229D RID: 8861
	public List<Levels>[] BossPools;

	// Token: 0x0400229E RID: 8862
	public List<Levels>[] ShmupPools;

	// Token: 0x0400229F RID: 8863
	public List<Levels>[] KingDicePools;

	// Token: 0x040022A0 RID: 8864
	public List<int> ShmupPlacement = new List<int>();

	// Token: 0x040022A1 RID: 8865
	public int[] SlotMachineWeapon2Attempt = new int[2];

	// Token: 0x040022A2 RID: 8866
	public bool[] slotsAreSpinning = new bool[2];

	// Token: 0x040022A3 RID: 8867
	public bool[] slotsCanSpinAgain = new bool[2];

	// Token: 0x040022A4 RID: 8868
	public bool[] slotsConfirm = new bool[2];

	// Token: 0x040022A5 RID: 8869
	public bool[] slotsDone = new bool[]
	{
		true,
		true
	};

	// Token: 0x040022A6 RID: 8870
	public int waitForButtonRelease;

	// Token: 0x040022A7 RID: 8871
	[SerializeField]
	public TowerOfPowerScorecard scorecard;

	// Token: 0x040022A8 RID: 8872
	public bool showingScorecard;

	// Token: 0x040022A9 RID: 8873
	[SerializeField]
	public bool debugForceSlotMachineEveryTurn;

	// Token: 0x040022AA RID: 8874
	[SerializeField]
	public bool debugForceSlotMachineAfterOneFight;

	// Token: 0x040022AB RID: 8875
	[SerializeField]
	public bool debugSkipToLastFight;

	// Token: 0x02000F8F RID: 3983
	public enum Charm_Slot
	{
		// Token: 0x0400706E RID: 28782
		charm_health_up_1,
		// Token: 0x0400706F RID: 28783
		charm_health_up_2,
		// Token: 0x04007070 RID: 28784
		charm_super_builder,
		// Token: 0x04007071 RID: 28785
		charm_smoke_dash,
		// Token: 0x04007072 RID: 28786
		charm_parry_plus,
		// Token: 0x04007073 RID: 28787
		charm_pit_saver,
		// Token: 0x04007074 RID: 28788
		charm_parry_attack,
		// Token: 0x04007075 RID: 28789
		charm_chalice,
		// Token: 0x04007076 RID: 28790
		charm_directional_dash,
		// Token: 0x04007077 RID: 28791
		None,
		// Token: 0x04007078 RID: 28792
		charm_extra_token
	}

	// Token: 0x02000F90 RID: 3984
	public enum Weapon_Slot
	{
		// Token: 0x0400707A RID: 28794
		level_weapon_peashot,
		// Token: 0x0400707B RID: 28795
		level_weapon_spreadshot,
		// Token: 0x0400707C RID: 28796
		level_weapon_arc,
		// Token: 0x0400707D RID: 28797
		level_weapon_homing,
		// Token: 0x0400707E RID: 28798
		level_weapon_exploder,
		// Token: 0x0400707F RID: 28799
		level_weapon_boomerang,
		// Token: 0x04007080 RID: 28800
		level_weapon_charge,
		// Token: 0x04007081 RID: 28801
		level_weapon_bouncer,
		// Token: 0x04007082 RID: 28802
		level_weapon_wide_shot,
		// Token: 0x04007083 RID: 28803
		plane_weapon_peashot,
		// Token: 0x04007084 RID: 28804
		plane_weapon_laser,
		// Token: 0x04007085 RID: 28805
		plane_weapon_bomb,
		// Token: 0x04007086 RID: 28806
		plane_chalice_weapon_3way,
		// Token: 0x04007087 RID: 28807
		arcade_weapon_peashot,
		// Token: 0x04007088 RID: 28808
		arcade_weapon_rocket_peashot,
		// Token: 0x04007089 RID: 28809
		None
	}

	// Token: 0x02000F91 RID: 3985
	public enum Super_Slot
	{
		// Token: 0x0400708B RID: 28811
		level_super_beam,
		// Token: 0x0400708C RID: 28812
		level_super_ghost,
		// Token: 0x0400708D RID: 28813
		level_super_invincible,
		// Token: 0x0400708E RID: 28814
		level_super_chalice_shmup,
		// Token: 0x0400708F RID: 28815
		level_super_chalice_vert_beam,
		// Token: 0x04007090 RID: 28816
		level_super_chalice_shield,
		// Token: 0x04007091 RID: 28817
		plane_super_bomb,
		// Token: 0x04007092 RID: 28818
		plane_super_chalice_stream,
		// Token: 0x04007093 RID: 28819
		None
	}
}
