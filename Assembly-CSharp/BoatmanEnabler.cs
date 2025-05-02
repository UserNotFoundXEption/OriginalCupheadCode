using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000473 RID: 1139
public class BoatmanEnabler : MapLevelDependentObstacle
{
	// Token: 0x0600306D RID: 12397 RVA: 0x00028356 File Offset: 0x00026556
	public override void Start()
	{
		if (DLCManager.DLCEnabled())
		{
			base.StartCoroutine(this.check_cr());
		}
	}

	// Token: 0x0600306E RID: 12398 RVA: 0x000E6070 File Offset: 0x000E4270
	public IEnumerator check_cr()
	{
		if (this.forceBoatmanUnlocking || PlayerData.Data.CurrentMap == Scenes.scene_map_world_DLC)
		{
			this.OnConditionAlreadyMet();
		}
		else if (!PlayerData.Data.hasUnlockedBoatman)
		{
			if (PlayerData.Data.hasUnlockedFirstSuper)
			{
				PlayerData.Data.shouldShowBoatmanTooltip = true;
				while (!MapEventNotification.Current.showing)
				{
					yield return null;
				}
				while (MapEventNotification.Current.showing)
				{
					yield return null;
				}
				base.StartCoroutine(this.showAppear_cr());
			}
			else if (PlayerData.Data.GetLevelData(Levels.Mausoleum).completed)
			{
				while (AbstractEquipUI.Current.CurrentState == AbstractEquipUI.ActiveState.Inactive)
				{
					yield return null;
				}
				while (AbstractEquipUI.Current.CurrentState == AbstractEquipUI.ActiveState.Active)
				{
					yield return null;
				}
				base.StartCoroutine(this.showAppear_cr());
			}
		}
		else
		{
			this.OnConditionAlreadyMet();
		}
		yield break;
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x000E608C File Offset: 0x000E428C
	public IEnumerator showAppear_cr()
	{
		Map.Current.CurrentState = Map.State.Event;
		MapEventNotification.Current.showing = true;
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		CupheadMapCamera cupheadMapCamera = Object.FindObjectOfType<CupheadMapCamera>();
		Vector3 cameraStartPos = cupheadMapCamera.transform.position;
		if (this.panCamera)
		{
			yield return cupheadMapCamera.MoveToPosition(base.CameraPosition, 0.5f, 0.9f);
		}
		base.MapMeetCondition();
		while (base.CurrentState != AbstractMapLevelDependentEntity.State.Complete)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		if (this.panCamera)
		{
			cupheadMapCamera.MoveToPosition(cameraStartPos, 0.75f, 1f);
		}
		Map.Current.CurrentState = Map.State.Ready;
		MapEventNotification.Current.showing = false;
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
		yield break;
	}

	// Token: 0x0400280C RID: 10252
	public bool forceBoatmanUnlocking;
}
