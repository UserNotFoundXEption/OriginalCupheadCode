using System;
using UnityEngine;

// Token: 0x02000497 RID: 1175
public class MapNPCJuggler : MonoBehaviour
{
	// Token: 0x06003133 RID: 12595 RVA: 0x000E8F0C File Offset: 0x000E710C
	public void Start()
	{
		this.AddDialoguerEvents();
		if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) == 1f)
		{
			this.animator.SetTrigger("three");
			return;
		}
		int numParriesInRow = PlayerData.Data.GetNumParriesInRow(PlayerId.Any);
		if (numParriesInRow <= 1)
		{
			this.animator.SetTrigger("one");
		}
		else if (numParriesInRow == 2)
		{
			this.animator.SetTrigger("two");
		}
		else if (numParriesInRow == 3)
		{
			this.animator.SetTrigger("three");
		}
		else if (numParriesInRow > 3)
		{
			this.animator.SetTrigger("three");
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			PlayerData.SaveCurrentFile();
		}
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x00028F38 File Offset: 0x00027138
	public void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x06003135 RID: 12597 RVA: 0x00028F40 File Offset: 0x00027140
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x06003136 RID: 12598 RVA: 0x00028F58 File Offset: 0x00027158
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x06003137 RID: 12599 RVA: 0x000E8FD8 File Offset: 0x000E71D8
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "JugglerCoin" && !PlayerData.Data.coinManager.GetCoinCollected(this.coinID))
		{
			PlayerData.Data.coinManager.SetCoinValue(this.coinID, true, PlayerId.Any);
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1);
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1);
			PlayerData.SaveCurrentFile();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Coin);
		}
	}

	// Token: 0x04002897 RID: 10391
	[SerializeField]
	public Animator animator;

	// Token: 0x04002898 RID: 10392
	[SerializeField]
	public int dialoguerVariableID;

	// Token: 0x04002899 RID: 10393
	[SerializeField]
	public string coinID = Guid.NewGuid().ToString();

	// Token: 0x0400289A RID: 10394
	[HideInInspector]
	public bool SkipDialogueEvent;
}
