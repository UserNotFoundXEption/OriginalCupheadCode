using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200048D RID: 1165
public class MapNPCBarbershopSong : MonoBehaviour
{
	// Token: 0x060030F7 RID: 12535 RVA: 0x00028C04 File Offset: 0x00026E04
	public void Start()
	{
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.AddDialoguerEvents();
	}

	// Token: 0x060030F8 RID: 12536 RVA: 0x000E81EC File Offset: 0x000E63EC
	public void Update()
	{
		if (this.songCoroutine != null && this.delay && (this.input.GetAnyButtonDown() || this.songEndedOrPlayerStop))
		{
			base.StopCoroutine(this.songCoroutine);
			this.songCoroutine = null;
			this.delay = false;
			this.songEndedOrPlayerStop = true;
			AudioManager.Stop("mus_barbershop");
			AudioManager.FadeBGMVolume(1f, 0.5f, false);
			AudioManager.FadeSFXVolume("worldmap_hint_djimmithegreat", 1f, 0.5f);
			for (int i = 0; i < this.barbershopAnimators.Length; i++)
			{
				this.barbershopAnimators[i].SetTrigger("endsong");
			}
			this.songEndedOrPlayerStop = false;
			if (Map.Current != null)
			{
				Map.Current.CurrentState = Map.State.Ready;
			}
			for (int j = 0; j < Map.Current.players.Length; j++)
			{
				if (!(Map.Current.players[j] == null))
				{
					Map.Current.players[j].Enable();
				}
			}
		}
	}

	// Token: 0x060030F9 RID: 12537 RVA: 0x00028C18 File Offset: 0x00026E18
	public void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x000E8310 File Offset: 0x000E6510
	public IEnumerator sing_cr()
	{
		AudioManager.FadeBGMVolume(0f, 0.5f, true);
		AudioManager.FadeSFXVolume("worldmap_hint_djimmithegreat", 0.01f, 0.5f);
		AudioManager.Play("mus_barbershop");
		yield return null;
		for (int i = 0; i < Map.Current.players.Length; i++)
		{
			if (!(Map.Current.players[i] == null))
			{
				Map.Current.players[i].Disable();
			}
		}
		if (Map.Current != null)
		{
			Map.Current.CurrentState = Map.State.Event;
		}
		for (int j = 0; j < this.barbershopAnimators.Length; j++)
		{
			this.barbershopAnimators[j].SetTrigger("sing");
		}
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.delay = true;
		yield return this.barbershopAnimators[3].WaitForAnimationToStart(this, "anim_map_barbershop_sing_hold", false);
		this.barbershopAnimators[0].SetTrigger("trans");
		yield return new WaitForSeconds(0.0833333358f);
		this.barbershopAnimators[1].SetTrigger("trans");
		yield return new WaitForSeconds(0.0833333358f);
		this.barbershopAnimators[2].SetTrigger("trans");
		yield return new WaitForSeconds(0.0833333358f);
		this.barbershopAnimators[3].SetTrigger("trans");
		yield return this.barbershopAnimators[3].WaitForAnimationToStart(this, "anim_map_barbershop_sing_idle_boil", true);
		this.barbershopAnimators[0].SetTrigger("blink");
		yield return new WaitForSeconds(0.0833333358f);
		this.barbershopAnimators[1].SetTrigger("blink");
		yield return new WaitForSeconds(0.0833333358f);
		this.barbershopAnimators[2].SetTrigger("blink");
		yield return new WaitForSeconds(0.0833333358f);
		this.barbershopAnimators[3].SetTrigger("blink");
		yield return this.barbershopAnimators[3].WaitForAnimationToStart(this, "anim_map_barbershop_sing_main_loop", true);
		while (!this.songEndedOrPlayerStop)
		{
			yield return null;
			if (!AudioManager.CheckIfPlaying("mus_barbershop"))
			{
				this.songEndedOrPlayerStop = true;
			}
		}
		for (int k = 0; k < this.barbershopAnimators.Length; k++)
		{
			this.barbershopAnimators[k].SetTrigger("endsong");
		}
		yield return null;
		this.songCoroutine = null;
		this.songEndedOrPlayerStop = false;
		if (Map.Current != null)
		{
			Map.Current.CurrentState = Map.State.Ready;
		}
		for (int l = 0; l < Map.Current.players.Length; l++)
		{
			if (!(Map.Current.players[l] == null))
			{
				Map.Current.players[l].Enable();
			}
		}
		AudioManager.FadeBGMVolume(1f, 0.5f, false);
		AudioManager.FadeSFXVolume("worldmap_hint_djimmithegreat", 1f, 0.5f);
		yield break;
	}

	// Token: 0x060030FB RID: 12539 RVA: 0x00028C20 File Offset: 0x00026E20
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x00028C38 File Offset: 0x00026E38
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060030FD RID: 12541 RVA: 0x000E832C File Offset: 0x000E652C
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "QuartetSing")
		{
			if (this.songCoroutine != null)
			{
				base.StopCoroutine(this.songCoroutine);
			}
			this.songCoroutine = base.StartCoroutine(this.sing_cr());
		}
	}

	// Token: 0x0400286D RID: 10349
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x0400286E RID: 10350
	[SerializeField]
	public Animator[] barbershopAnimators = new Animator[4];

	// Token: 0x0400286F RID: 10351
	public Coroutine songCoroutine;

	// Token: 0x04002870 RID: 10352
	public bool songEndedOrPlayerStop;

	// Token: 0x04002871 RID: 10353
	public bool delay;

	// Token: 0x04002872 RID: 10354
	[HideInInspector]
	public bool SkipDialogueEvent;
}
