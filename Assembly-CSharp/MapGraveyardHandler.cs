using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x0200047E RID: 1150
public class MapGraveyardHandler : MapDialogueInteraction
{
	// Token: 0x17000390 RID: 912
	// (get) Token: 0x060030A9 RID: 12457 RVA: 0x00028613 File Offset: 0x00026813
	// (set) Token: 0x060030AA RID: 12458 RVA: 0x0002861B File Offset: 0x0002681B
	public bool canReenter { get; set; }

	// Token: 0x060030AB RID: 12459 RVA: 0x000E6F94 File Offset: 0x000E5194
	public override void Start()
	{
		this.extantGhosts = new Animator[2];
		if (!PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, Charm.charm_curse) && !PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, Charm.charm_curse))
		{
			foreach (MapGraveyardGrave mapGraveyardGrave in this.grave)
			{
				mapGraveyardGrave.SetInteractable(false);
			}
			base.gameObject.SetActive(false);
			return;
		}
		base.Start();
		this.puzzleOrder = PlayerData.Data.curseCharmPuzzleOrder;
		this.AddDialoguerEvents();
		if (!PlayerData.Data.curseCharmPuzzleComplete)
		{
			this.ResetGraves();
		}
		else
		{
			if (!PlayerData.Data.GetLevelData(Levels.Graveyard).completed)
			{
				this.showBeam();
			}
			this.grave[5].SetInteractable(true);
		}
	}

	// Token: 0x060030AC RID: 12460 RVA: 0x00028624 File Offset: 0x00026824
	public override MapUIInteractionDialogue Show(PlayerInput player)
	{
		if (!PlayerData.Data.GetLevelData(Levels.Graveyard).completed)
		{
			return base.Show(player);
		}
		return null;
	}

	// Token: 0x060030AD RID: 12461 RVA: 0x00028648 File Offset: 0x00026848
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x060030AE RID: 12462 RVA: 0x00028660 File Offset: 0x00026860
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060030AF RID: 12463 RVA: 0x00028678 File Offset: 0x00026878
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (metadata == "LOADGRAVEYARD")
		{
			base.StartCoroutine(this.load_fight_cr());
		}
	}

	// Token: 0x060030B0 RID: 12464 RVA: 0x000E7070 File Offset: 0x000E5270
	public IEnumerator load_fight_cr()
	{
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		base.SetPlayerReturnPos();
		Map.Current.CurrentState = Map.State.Graveyard;
		if (Map.Current.players[0] != null)
		{
			Map.Current.players[0].animator.SetTrigger("Sleep");
		}
		if (Map.Current.players[1] != null)
		{
			Map.Current.players[1].animator.SetTrigger("Sleep");
		}
		yield return new WaitForSeconds(1f);
		SceneLoader.LoadScene(Scenes.scene_level_graveyard, SceneLoader.Transition.Blur, SceneLoader.Transition.Blur, SceneLoader.Icon.HourglassBroken, null);
		yield break;
	}

	// Token: 0x060030B1 RID: 12465 RVA: 0x00028697 File Offset: 0x00026897
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.RemoveDialoguerEvents();
	}

	// Token: 0x060030B2 RID: 12466 RVA: 0x000286A5 File Offset: 0x000268A5
	public void showBeam()
	{
		this.beamAnimator.Play("Aura", 1, 0f);
		this.beamAnimator.Play("Start", 0, 0f);
		this.beamAnimator.Update(0f);
	}

	// Token: 0x060030B3 RID: 12467 RVA: 0x000E708C File Offset: 0x000E528C
	public void ActivatedGrave(int index, int playerNum, Vector3 ghostPos)
	{
		if (!PlayerData.Data.curseCharmPuzzleComplete)
		{
			if (index >= 0 && this.entryCount < 3)
			{
				Animator component = Object.Instantiate<GameObject>(this.ghostPrefab, ghostPos, Quaternion.identity).GetComponent<Animator>();
				this.SFX_GRAVEYARD_Interact(this.entryCount);
				if (index == this.puzzleOrder[this.entryCount])
				{
					this.correctCount++;
				}
				this.entryCount++;
				if (this.entryCount == this.puzzleOrder.Length)
				{
					if (this.correctCount == this.entryCount)
					{
						component.Play("Yes");
						this.SFX_GRAVEYARD_Positive();
						this.showBeam();
						PlayerData.Data.curseCharmPuzzleComplete = true;
						PlayerData.SaveCurrentFile();
					}
					else
					{
						component.Play("No");
						this.SFX_GRAVEYARD_Negative();
						base.StartCoroutine(this.reset_cr());
					}
					this.extantGhosts[0].SetTrigger("EngageEnd");
					this.extantGhosts[1].SetTrigger("EngageEnd");
				}
				else
				{
					component.Play("EngageStart");
					this.extantGhosts[this.entryCount - 1] = component;
				}
			}
		}
		else if ((index == -1 && !PlayerData.Data.GetLevelData(Levels.Graveyard).completed) || this.canReenter)
		{
			this.StartSpeechBubble();
		}
	}

	// Token: 0x060030B4 RID: 12468 RVA: 0x000E71F8 File Offset: 0x000E53F8
	public void UpdateReenterCodeActive()
	{
		switch (this.interactor)
		{
		default:
			if (base.PlayerWithinDistance(0))
			{
				Player actions = Map.Current.players[0].input.actions;
				if (actions.GetButton(11) && actions.GetButton(12))
				{
					this.currentDuration += CupheadTime.Delta;
				}
				else
				{
					this.currentDuration = 0f;
				}
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Mugman:
			if (base.PlayerWithinDistance(1))
			{
				Player actions2 = Map.Current.players[1].input.actions;
				if (actions2.GetButton(11) && actions2.GetButton(12))
				{
					this.currentDuration += CupheadTime.Delta;
				}
				else
				{
					this.currentDuration = 0f;
				}
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Either:
		{
			bool flag = false;
			if (base.PlayerWithinDistance(0))
			{
				Player actions3 = Map.Current.players[0].input.actions;
				if (actions3.GetButton(11) && actions3.GetButton(12))
				{
					this.currentDuration += CupheadTime.Delta;
					flag = true;
				}
			}
			if (base.PlayerWithinDistance(1))
			{
				Player actions4 = Map.Current.players[1].input.actions;
				if (actions4.GetButton(11) && actions4.GetButton(12))
				{
					this.currentDuration += CupheadTime.Delta;
					flag = true;
				}
			}
			if (!flag)
			{
				this.currentDuration = 0f;
			}
			break;
		}
		case AbstractMapInteractiveEntity.Interactor.Both:
			if (Map.Current.players[0] == null || Map.Current.players[1] == null)
			{
				this.canReenter = false;
			}
			if (base.PlayerWithinDistance(0) && base.PlayerWithinDistance(1))
			{
				if (Map.Current.players[0].input.actions.GetButton(13))
				{
					if (Map.Current.players[1].input.actions.GetButton(13))
					{
						this.currentDuration += CupheadTime.Delta;
					}
					else
					{
						this.currentDuration = 0f;
					}
				}
				else
				{
					this.currentDuration = 0f;
				}
			}
			break;
		}
		if (this.currentDuration >= this.pressDurationToReEnable && !this.canReenter && PlayerData.Data.GetLevelData(Levels.Graveyard).completed)
		{
			this.SFX_GRAVEYARD_Positive();
			this.showBeam();
			this.canReenter = true;
		}
	}

	// Token: 0x060030B5 RID: 12469 RVA: 0x000E74D8 File Offset: 0x000E56D8
	public void ResetGraves()
	{
		foreach (MapGraveyardGrave mapGraveyardGrave in this.grave)
		{
			mapGraveyardGrave.SetInteractable(true);
		}
		this.entryCount = 0;
		this.correctCount = 0;
	}

	// Token: 0x060030B6 RID: 12470 RVA: 0x000E751C File Offset: 0x000E571C
	public IEnumerator reset_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.ResetGraves();
		yield break;
	}

	// Token: 0x060030B7 RID: 12471 RVA: 0x000286E3 File Offset: 0x000268E3
	public override void Update()
	{
		this.UpdateReenterCodeActive();
	}

	// Token: 0x060030B8 RID: 12472 RVA: 0x000286EB File Offset: 0x000268EB
	public void SFX_GRAVEYARD_Activate()
	{
		AudioManager.Play("sfx_dlc_worldmap_graveyard_activate");
	}

	// Token: 0x060030B9 RID: 12473 RVA: 0x000286F7 File Offset: 0x000268F7
	public void SFX_GRAVEYARD_Interact(int i)
	{
		AudioManager.Play("sfx_dlc_worldmap_graveyard_interact_" + (i + 1));
	}

	// Token: 0x060030BA RID: 12474 RVA: 0x00028710 File Offset: 0x00026910
	public void SFX_GRAVEYARD_Negative()
	{
		AudioManager.Play("sfx_dlc_worldmap_graveyard_negative");
	}

	// Token: 0x060030BB RID: 12475 RVA: 0x0002871C File Offset: 0x0002691C
	public void SFX_GRAVEYARD_Positive()
	{
		AudioManager.Play("sfx_dlc_worldmap_graveyard_positive");
	}

	// Token: 0x04002837 RID: 10295
	[SerializeField]
	public GameObject graveFire;

	// Token: 0x04002838 RID: 10296
	[SerializeField]
	public MapGraveyardGrave[] grave;

	// Token: 0x04002839 RID: 10297
	[SerializeField]
	public float pressDurationToReEnable = 1f;

	// Token: 0x0400283A RID: 10298
	[SerializeField]
	public GameObject ghostPrefab;

	// Token: 0x0400283B RID: 10299
	[SerializeField]
	public Animator beamAnimator;

	// Token: 0x0400283C RID: 10300
	public int[] puzzleOrder;

	// Token: 0x0400283D RID: 10301
	public int entryCount;

	// Token: 0x0400283E RID: 10302
	public int correctCount;

	// Token: 0x0400283F RID: 10303
	public float currentDuration;

	// Token: 0x04002840 RID: 10304
	public Animator[] extantGhosts;
}
