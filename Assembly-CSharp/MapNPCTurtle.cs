using System;
using UnityEngine;

// Token: 0x0200049F RID: 1183
public class MapNPCTurtle : MapDialogueInteraction
{
	// Token: 0x06003165 RID: 12645 RVA: 0x000E96EC File Offset: 0x000E78EC
	public override void Start()
	{
		base.Start();
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
		if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) < 2f)
		{
			if (PlayerData.Data.CheckLevelsHaveMinGrade(Level.platformingLevels, LevelScoringData.Grade.P))
			{
				Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 2f);
				PlayerData.SaveCurrentFile();
			}
			else if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) < 1f && PlayerData.Data.CountLevelsHaveMinGrade(Level.platformingLevels, LevelScoringData.Grade.P) > 1)
			{
				Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
				PlayerData.SaveCurrentFile();
			}
		}
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x000E97C8 File Offset: 0x000E79C8
	public override void OnDestroy()
	{
		base.OnDestroy();
		Dialoguer.events.onEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x06003167 RID: 12647 RVA: 0x000E9820 File Offset: 0x000E7A20
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "Pacifist")
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Turtle);
			PlayerData.Data.unlockedBlackAndWhite = true;
			PlayerData.SaveCurrentFile();
			MapUI.Current.Refresh();
		}
	}

	// Token: 0x06003168 RID: 12648 RVA: 0x000E9870 File Offset: 0x000E7A70
	public override void Activate(MapPlayerController player)
	{
		if (this.dialogues[(int)player.id].transform.localScale.x == 1f)
		{
			base.Activate(player);
			if (this.colliderB.OverlapPoint(player.transform.position))
			{
				base.animator.SetTrigger("turn_b");
			}
			else
			{
				base.animator.SetTrigger("turn_a");
			}
		}
	}

	// Token: 0x06003169 RID: 12649 RVA: 0x000291F1 File Offset: 0x000273F1
	public new void OnDialogueEndedHandler()
	{
		base.animator.SetTrigger("return");
	}

	// Token: 0x040028B4 RID: 10420
	[SerializeField]
	public BoxCollider2D colliderB;

	// Token: 0x040028B5 RID: 10421
	[SerializeField]
	public int dialoguerVariableID = 19;

	// Token: 0x040028B6 RID: 10422
	[HideInInspector]
	public bool SkipDialogueEvent;
}
