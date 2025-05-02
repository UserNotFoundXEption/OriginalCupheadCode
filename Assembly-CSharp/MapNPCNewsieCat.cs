using System;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class MapNPCNewsieCat : MonoBehaviour
{
	// Token: 0x06003148 RID: 12616 RVA: 0x000E91FC File Offset: 0x000E73FC
	public void Start()
	{
		Dialoguer.SetGlobalFloat(40, 0f);
		if (!PlayerData.Data.coinManager.GetCoinCollected(this.coinID1))
		{
			Dialoguer.SetGlobalFloat(39, 0f);
		}
		Levels[] levels = new Levels[]
		{
			Levels.Veggies,
			Levels.Slime,
			Levels.FlyingBlimp,
			Levels.Flower,
			Levels.Frogs
		};
		Levels[] array = new Levels[]
		{
			Levels.OldMan,
			Levels.RumRunners,
			Levels.Airplane,
			Levels.SnowCult,
			Levels.FlyingCowboy,
			Levels.Saltbaker
		};
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (PlayerData.Data.CheckLevelCompleted(array[i]))
			{
				num++;
			}
		}
		if (num > 0)
		{
			Dialoguer.SetGlobalFloat(24, 3f);
		}
		else if (PlayerData.Data.CheckLevelCompleted(Levels.Devil))
		{
			Dialoguer.SetGlobalFloat(24, 2f);
		}
		else if (PlayerData.Data.CheckLevelsCompleted(levels))
		{
			Dialoguer.SetGlobalFloat(24, 1f);
		}
		else
		{
			Dialoguer.SetGlobalFloat(24, 0f);
		}
		this.AddDialoguerEvents();
	}

	// Token: 0x06003149 RID: 12617 RVA: 0x00029074 File Offset: 0x00027274
	public void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x0002907C File Offset: 0x0002727C
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600314B RID: 12619 RVA: 0x00029094 File Offset: 0x00027294
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600314C RID: 12620 RVA: 0x000E92FC File Offset: 0x000E74FC
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "NewsieCoin" && !PlayerData.Data.coinManager.GetCoinCollected(this.coinID1))
		{
			Dialoguer.SetGlobalFloat(39, 1f);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID1, true, PlayerId.Any);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID2, true, PlayerId.Any);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID3, true, PlayerId.Any);
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 3);
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 3);
			PlayerData.SaveCurrentFile();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.ThreeCoins);
		}
	}

	// Token: 0x040028A4 RID: 10404
	public const int DIALOGUER_VAR_INDEX = 24;

	// Token: 0x040028A5 RID: 10405
	public const int DIALOGUER_VAR_GOT_COIN = 39;

	// Token: 0x040028A6 RID: 10406
	public const int DIALOGUER_VAR_INTERACT_COUNTER = 40;

	// Token: 0x040028A7 RID: 10407
	[SerializeField]
	public string coinID1 = Guid.NewGuid().ToString();

	// Token: 0x040028A8 RID: 10408
	[SerializeField]
	public string coinID2 = Guid.NewGuid().ToString();

	// Token: 0x040028A9 RID: 10409
	[SerializeField]
	public string coinID3 = Guid.NewGuid().ToString();
}
