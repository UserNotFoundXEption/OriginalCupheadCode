using System;
using UnityEngine;

// Token: 0x0200048F RID: 1167
public class MapNPCCanteen : MonoBehaviour
{
	// Token: 0x0600310A RID: 12554 RVA: 0x000E86E0 File Offset: 0x000E68E0
	public void Start()
	{
		if (PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Weapon.plane_weapon_bomb) && (PlayerManager.Multiplayer || PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Weapon.plane_weapon_bomb)))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
		}
		this.AddDialoguerEvents();
	}

	// Token: 0x0600310B RID: 12555 RVA: 0x00028CDA File Offset: 0x00026EDA
	public void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x0600310C RID: 12556 RVA: 0x00028CE2 File Offset: 0x00026EE2
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600310D RID: 12557 RVA: 0x00028CFA File Offset: 0x00026EFA
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x0600310E RID: 12558 RVA: 0x000E8738 File Offset: 0x000E6938
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "CanteenWeaponTwo")
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Canteen);
			if (!PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Weapon.plane_weapon_bomb))
			{
				PlayerData.Data.Gift(PlayerId.PlayerOne, Weapon.plane_weapon_bomb);
				if (!PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).HasEquippedSecondarySHMUPWeapon)
				{
					PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).MustNotifySwitchSHMUPWeapon = true;
				}
				PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).HasEquippedSecondarySHMUPWeapon = true;
			}
			if (!PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Weapon.plane_weapon_bomb))
			{
				PlayerData.Data.Gift(PlayerId.PlayerTwo, Weapon.plane_weapon_bomb);
				if (!PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).HasEquippedSecondarySHMUPWeapon)
				{
					PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).MustNotifySwitchSHMUPWeapon = true;
				}
				PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).HasEquippedSecondarySHMUPWeapon = true;
			}
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			PlayerData.SaveCurrentFile();
		}
	}

	// Token: 0x0400287B RID: 10363
	[SerializeField]
	public int dialoguerVariableID = 13;

	// Token: 0x0400287C RID: 10364
	[HideInInspector]
	public bool SkipDialogueEvent;
}
