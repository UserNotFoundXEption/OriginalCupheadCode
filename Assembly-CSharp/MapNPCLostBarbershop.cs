using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000498 RID: 1176
public class MapNPCLostBarbershop : AbstractMapInteractiveEntity
{
	// Token: 0x06003139 RID: 12601 RVA: 0x00028F80 File Offset: 0x00027180
	public void Start()
	{
		this.AddDialoguerEvents();
	}

	// Token: 0x0600313A RID: 12602 RVA: 0x00028F88 File Offset: 0x00027188
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.RemoveDialoguerEvents();
	}

	// Token: 0x0600313B RID: 12603 RVA: 0x000E9060 File Offset: 0x000E7260
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueEndedHandler;
	}

	// Token: 0x0600313C RID: 12604 RVA: 0x000E90B0 File Offset: 0x000E72B0
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.OnDialogueEndedHandler;
	}

	// Token: 0x0600313D RID: 12605 RVA: 0x00028F96 File Offset: 0x00027196
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "LostBarberFound")
		{
			base.GetComponent<MapDialogueInteraction>().disabledActivations = true;
			this.reunited = true;
		}
	}

	// Token: 0x0600313E RID: 12606 RVA: 0x00028FC7 File Offset: 0x000271C7
	public override void Activate()
	{
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x00028FC9 File Offset: 0x000271C9
	public override MapUIInteractionDialogue Show(PlayerInput player)
	{
		this.FoundBarbershopSFX();
		base.animator.SetTrigger(this.triggerShow);
		return null;
	}

	// Token: 0x06003140 RID: 12608 RVA: 0x00028FE3 File Offset: 0x000271E3
	public void OnDialogueEndedHandler()
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (this.reunited)
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			PlayerData.SaveCurrentFile();
			base.StartCoroutine(this.found_cr());
		}
	}

	// Token: 0x06003141 RID: 12609 RVA: 0x000E9100 File Offset: 0x000E7300
	public IEnumerator found_cr()
	{
		base.animator.SetTrigger(this.triggerHide);
		yield return base.animator.WaitForAnimationToEnd(this, "anim_map_barbershop_outtro_d", false, true);
		this.playerCanWalkBehind = true;
		base.SetLayer(base.GetComponent<SpriteRenderer>());
		for (int i = 0; i < this.mapNPCBarbershops.Length; i++)
		{
			this.mapNPCBarbershops[i].NowFour();
			if (!(this.mapNPCBarbershops[i].mapDialogueInteraction == null))
			{
				for (int j = 0; j < this.mapNPCBarbershops[i].mapDialogueInteraction.dialogues.Length; j++)
				{
					if (!(this.mapNPCBarbershops[i].mapDialogueInteraction.dialogues[j] == null))
					{
						this.mapNPCBarbershops[i].mapDialogueInteraction.Hide(this.mapNPCBarbershops[i].mapDialogueInteraction.dialogues[j]);
					}
				}
			}
		}
		this.Hide(null);
		while (CupheadMapCamera.Current != null && CupheadMapCamera.Current.IsCameraFarFromPlayer())
		{
			yield return null;
		}
		for (int k = 0; k < this.mapNPCBarbershops.Length; k++)
		{
			this.mapNPCBarbershops[k].CleanUp();
		}
		yield break;
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x0002901E File Offset: 0x0002721E
	public void FoundBarbershopSFX()
	{
		if (!this.FirstTimeFoundSFX)
		{
			AudioManager.Play("find_barbershop_member");
			this.FirstTimeFoundSFX = true;
		}
	}

	// Token: 0x0400289B RID: 10395
	[SerializeField]
	public string triggerShow;

	// Token: 0x0400289C RID: 10396
	[SerializeField]
	public string triggerHide;

	// Token: 0x0400289D RID: 10397
	[SerializeField]
	public MapNPCBarbershop[] mapNPCBarbershops;

	// Token: 0x0400289E RID: 10398
	[SerializeField]
	public int dialoguerVariableID = 10;

	// Token: 0x0400289F RID: 10399
	public bool reunited;

	// Token: 0x040028A0 RID: 10400
	public bool FirstTimeFoundSFX;

	// Token: 0x040028A1 RID: 10401
	public SpriteRenderer spriteRenderer;

	// Token: 0x040028A2 RID: 10402
	[HideInInspector]
	public bool SkipDialogueEvent;
}
