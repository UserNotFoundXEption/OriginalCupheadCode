using System;
using UnityEngine;

// Token: 0x02000491 RID: 1169
public class MapNPCCircusgirl : MonoBehaviour
{
	// Token: 0x0600311B RID: 12571 RVA: 0x000E8A7C File Offset: 0x000E6C7C
	public void Start()
	{
		if (PlayerData.Data.coinManager.GetCoinCollected(this.coinID))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 2f);
		}
		else
		{
			this.AddDialoguerEvents();
			OnlineManager.Instance.Interface.GetAchievement(PlayerId.PlayerOne, "FoundSecretPassage", delegate(OnlineAchievement achievement)
			{
				if (achievement.IsUnlocked)
				{
					Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
				}
			});
		}
	}

	// Token: 0x0600311C RID: 12572 RVA: 0x00028DF8 File Offset: 0x00026FF8
	public void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x0600311D RID: 12573 RVA: 0x00028E00 File Offset: 0x00027000
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600311E RID: 12574 RVA: 0x00028E18 File Offset: 0x00027018
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600311F RID: 12575 RVA: 0x000E8AE0 File Offset: 0x000E6CE0
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "GingerbreadCoin" && !PlayerData.Data.coinManager.GetCoinCollected(this.coinID))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 2f);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID, true, PlayerId.Any);
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1);
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1);
			PlayerData.SaveCurrentFile();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Coin);
		}
	}

	// Token: 0x04002884 RID: 10372
	[SerializeField]
	public int dialoguerVariableID = 7;

	// Token: 0x04002885 RID: 10373
	[SerializeField]
	public string coinID = Guid.NewGuid().ToString();

	// Token: 0x04002886 RID: 10374
	[HideInInspector]
	public bool SkipDialogueEvent;
}
