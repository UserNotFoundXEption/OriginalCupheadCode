using System;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public class MapNPCProfessional : MonoBehaviour
{
	// Token: 0x06003150 RID: 12624 RVA: 0x000E93B8 File Offset: 0x000E75B8
	public void Start()
	{
		this.AddDialoguerEvents();
		if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) < 3f)
		{
			int num = PlayerData.Data.CountLevelsHaveMinGrade(Level.world1BossLevels, LevelScoringData.Grade.AMinus);
			num += PlayerData.Data.CountLevelsHaveMinGrade(Level.world2BossLevels, LevelScoringData.Grade.AMinus);
			num += PlayerData.Data.CountLevelsHaveMinGrade(Level.world3BossLevels, LevelScoringData.Grade.AMinus);
			num += PlayerData.Data.CountLevelsHaveMinGrade(Level.world4BossLevels, LevelScoringData.Grade.AMinus);
			num += PlayerData.Data.CountLevelsHaveMinGrade(Level.platformingLevels, LevelScoringData.Grade.AMinus);
			if (num >= 15)
			{
				Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 3f);
				PlayerData.SaveCurrentFile();
			}
			else if (num >= 10)
			{
				if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) < 2f)
				{
					Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 2f);
					PlayerData.SaveCurrentFile();
				}
			}
			else if (num >= 5 && Dialoguer.GetGlobalFloat(this.dialoguerVariableID) < 1f)
			{
				Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
				PlayerData.SaveCurrentFile();
			}
		}
	}

	// Token: 0x06003151 RID: 12625 RVA: 0x000290C6 File Offset: 0x000272C6
	public void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x06003152 RID: 12626 RVA: 0x000290CE File Offset: 0x000272CE
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x06003153 RID: 12627 RVA: 0x000290E6 File Offset: 0x000272E6
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x06003154 RID: 12628 RVA: 0x000E94CC File Offset: 0x000E76CC
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "RetroColorUnlock")
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Professional);
			PlayerData.Data.unlocked2Strip = true;
			PlayerData.SaveCurrentFile();
			MapUI.Current.Refresh();
		}
	}

	// Token: 0x040028AA RID: 10410
	[SerializeField]
	public int dialoguerVariableID = 20;

	// Token: 0x040028AB RID: 10411
	[HideInInspector]
	public bool SkipDialogueEvent;
}
