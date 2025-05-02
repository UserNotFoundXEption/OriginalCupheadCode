using System;

// Token: 0x02000483 RID: 1155
public class MapLevelLoaderChaliceTutorial : MapLevelLoader
{
	// Token: 0x060030D2 RID: 12498 RVA: 0x000288A3 File Offset: 0x00026AA3
	public override void Activate(MapPlayerController player)
	{
		if (PlayerData.Data.Loadouts.GetPlayerLoadout(player.id).charm == Charm.charm_chalice)
		{
			base.Activate(player);
		}
		else
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.ChaliceTutorialEquipCharm);
		}
	}
}
