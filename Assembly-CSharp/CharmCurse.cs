using System;
using System.Collections.Generic;

// Token: 0x0200051A RID: 1306
public static class CharmCurse
{
	// Token: 0x0600375B RID: 14171 RVA: 0x00103094 File Offset: 0x00101294
	public static int CalculateLevel(PlayerId playerId)
	{
		if (!PlayerData.Data.GetLevelData(Levels.Graveyard).completed)
		{
			return -1;
		}
		int num = PlayerData.Data.CalculateCurseCharmAccumulatedValue(playerId, CharmCurse.CountableLevels);
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

	// Token: 0x0600375C RID: 14172 RVA: 0x001030FC File Offset: 0x001012FC
	public static bool IsMaxLevel(PlayerId playerId)
	{
		int[] levelThreshold = WeaponProperties.CharmCurse.levelThreshold;
		return CharmCurse.CalculateLevel(playerId) == levelThreshold.Length - 1;
	}

	// Token: 0x0600375D RID: 14173 RVA: 0x0002D3A3 File Offset: 0x0002B5A3
	public static int GetHealthModifier(int charmLevel)
	{
		if (charmLevel < 0)
		{
			return 0;
		}
		return WeaponProperties.CharmCurse.healthModifierValues[charmLevel];
	}

	// Token: 0x0600375E RID: 14174 RVA: 0x0002D3B5 File Offset: 0x0002B5B5
	public static float GetSuperMeterAmount(int charmLevel)
	{
		if (charmLevel < 0)
		{
			return 0f;
		}
		return WeaponProperties.CharmCurse.superMeterAmount[charmLevel];
	}

	// Token: 0x0600375F RID: 14175 RVA: 0x0002D3CB File Offset: 0x0002B5CB
	public static int GetSmokeDashInterval(int charmLevel)
	{
		if (charmLevel < 0)
		{
			return 0;
		}
		return WeaponProperties.CharmCurse.smokeDashInterval[charmLevel];
	}

	// Token: 0x06003760 RID: 14176 RVA: 0x0002D3DD File Offset: 0x0002B5DD
	public static int GetWhetstoneInterval(int charmLevel)
	{
		if (charmLevel < 0)
		{
			return 0;
		}
		return WeaponProperties.CharmCurse.whetstoneInterval[charmLevel];
	}

	// Token: 0x06003761 RID: 14177 RVA: 0x0010311C File Offset: 0x0010131C
	public static int GetHealerInterval(int charmLevel, int hpReceived)
	{
		if (charmLevel < 0)
		{
			return 0;
		}
		if (CharmCurse.healerCharmIntervals == null)
		{
			string[] healerInterval = WeaponProperties.CharmCurse.healerInterval;
			CharmCurse.healerCharmIntervals = new List<int[]>(healerInterval.Length);
			foreach (string text in healerInterval)
			{
				string[] array2 = text.Split(new char[]
				{
					','
				});
				if (array2.Length != 3)
				{
					throw new Exception("Invalid healer intervals");
				}
				int[] array3 = new int[array2.Length];
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j] = Parser.IntParse(array2[j]);
				}
				CharmCurse.healerCharmIntervals.Add(array3);
			}
		}
		return CharmCurse.healerCharmIntervals[charmLevel][hpReceived];
	}

	// Token: 0x04002C83 RID: 11395
	public static readonly Levels[] CountableLevels = new Levels[]
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

	// Token: 0x04002C84 RID: 11396
	public static List<int[]> healerCharmIntervals;
}
