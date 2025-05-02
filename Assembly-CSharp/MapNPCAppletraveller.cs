using System;
using UnityEngine;

// Token: 0x0200048A RID: 1162
public class MapNPCAppletraveller : MonoBehaviour
{
	// Token: 0x060030E5 RID: 12517 RVA: 0x00028A58 File Offset: 0x00026C58
	public void Start()
	{
		this.squareRadiusStartWaving = this.radiusStartWaving * this.radiusStartWaving;
		this.squareRadiusStopWaving = this.radiusStopWaving * this.radiusStopWaving;
		this.AddDialoguerEvents();
	}

	// Token: 0x060030E6 RID: 12518 RVA: 0x00028A86 File Offset: 0x00026C86
	public void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x060030E7 RID: 12519 RVA: 0x000E7E60 File Offset: 0x000E6060
	public void Update()
	{
		if ((this.state == MapNPCAppletraveller.AppletravellerState.idle && (base.transform.position - Map.Current.players[0].transform.position).sqrMagnitude < this.squareRadiusStartWaving) || (PlayerManager.Multiplayer && (base.transform.position - Map.Current.players[1].transform.position).sqrMagnitude < this.squareRadiusStartWaving))
		{
			this.state = MapNPCAppletraveller.AppletravellerState.wave;
			this.animator.SetTrigger("wave");
		}
		else if (this.state == MapNPCAppletraveller.AppletravellerState.wave)
		{
			if ((base.transform.position - Map.Current.players[0].transform.position).sqrMagnitude > this.squareRadiusStartWaving && (!PlayerManager.Multiplayer || (base.transform.position - Map.Current.players[1].transform.position).sqrMagnitude > this.squareRadiusStartWaving))
			{
				this.state = MapNPCAppletraveller.AppletravellerState.idle;
				this.animator.SetTrigger("next");
			}
			if ((base.transform.position - Map.Current.players[0].transform.position).sqrMagnitude < this.squareRadiusStopWaving || (PlayerManager.Multiplayer && (base.transform.position - Map.Current.players[1].transform.position).sqrMagnitude < this.squareRadiusStopWaving))
			{
				this.state = MapNPCAppletraveller.AppletravellerState.wait;
				this.animator.SetTrigger("next");
			}
		}
		else if ((base.transform.position - Map.Current.players[0].transform.position).sqrMagnitude > this.squareRadiusStartWaving && (!PlayerManager.Multiplayer || (base.transform.position - Map.Current.players[1].transform.position).sqrMagnitude > this.squareRadiusStartWaving))
		{
			this.state = MapNPCAppletraveller.AppletravellerState.idle;
		}
	}

	// Token: 0x060030E8 RID: 12520 RVA: 0x000E80D0 File Offset: 0x000E62D0
	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.position, this.radiusStartWaving);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, this.radiusStopWaving);
	}

	// Token: 0x060030E9 RID: 12521 RVA: 0x00028A8E File Offset: 0x00026C8E
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x060030EA RID: 12522 RVA: 0x00028AA6 File Offset: 0x00026CA6
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x000E8120 File Offset: 0x000E6320
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "MacCoin" && !PlayerData.Data.coinManager.GetCoinCollected(this.coinID1))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID1, true, PlayerId.Any);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID2, true, PlayerId.Any);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID3, true, PlayerId.Any);
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 3);
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 3);
			PlayerData.SaveCurrentFile();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.ThreeCoins);
		}
	}

	// Token: 0x0400285B RID: 10331
	[SerializeField]
	public Animator animator;

	// Token: 0x0400285C RID: 10332
	[SerializeField]
	public int dialoguerVariableID = 6;

	// Token: 0x0400285D RID: 10333
	[SerializeField]
	public string coinID1 = Guid.NewGuid().ToString();

	// Token: 0x0400285E RID: 10334
	[SerializeField]
	public string coinID2 = Guid.NewGuid().ToString();

	// Token: 0x0400285F RID: 10335
	[SerializeField]
	public string coinID3 = Guid.NewGuid().ToString();

	// Token: 0x04002860 RID: 10336
	[SerializeField]
	public float radiusStartWaving = 50f;

	// Token: 0x04002861 RID: 10337
	[SerializeField]
	public float radiusStopWaving = 10f;

	// Token: 0x04002862 RID: 10338
	public float squareRadiusStartWaving;

	// Token: 0x04002863 RID: 10339
	public float squareRadiusStopWaving;

	// Token: 0x04002864 RID: 10340
	public MapNPCAppletraveller.AppletravellerState state;

	// Token: 0x04002865 RID: 10341
	[HideInInspector]
	public bool SkipDialogueEvent;

	// Token: 0x020010FE RID: 4350
	public enum AppletravellerState
	{
		// Token: 0x04007843 RID: 30787
		idle,
		// Token: 0x04007844 RID: 30788
		wave,
		// Token: 0x04007845 RID: 30789
		wait
	}
}
