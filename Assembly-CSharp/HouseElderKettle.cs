using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002B3 RID: 691
public class HouseElderKettle : DialogueInteractionPoint
{
	// Token: 0x06001EEC RID: 7916 RVA: 0x0001A136 File Offset: 0x00018336
	public void BeginDialogue()
	{
		this.Activate();
		this.speechBubble.waitForRealease = false;
	}

	// Token: 0x06001EED RID: 7917 RVA: 0x000B4DB0 File Offset: 0x000B2FB0
	public override void Start()
	{
		base.Start();
		this.hasTarget = false;
		Dialoguer.events.onTextPhase += this.OnDialogueTextSound;
		Dialoguer.events.onStarted += this.StartTalkingCoroutine;
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x06001EEE RID: 7918 RVA: 0x000B4E0C File Offset: 0x000B300C
	public override void OnDestroy()
	{
		base.OnDestroy();
		Dialoguer.events.onTextPhase -= this.OnDialogueTextSound;
		Dialoguer.events.onStarted -= this.StartTalkingCoroutine;
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x06001EEF RID: 7919 RVA: 0x000B4E64 File Offset: 0x000B3064
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "ElderKettleBottle")
		{
			base.animator.SetTrigger("Bottle");
			base.StartCoroutine(this.bottle_sound_cr());
		}
		if (message == "ElderKettleFirstWeapon")
		{
			base.animator.SetTrigger("Continue");
		}
	}

	// Token: 0x06001EF0 RID: 7920 RVA: 0x0001A14A File Offset: 0x0001834A
	public void StartTalkingCoroutine()
	{
		base.StartCoroutine(this.talking_crs());
	}

	// Token: 0x06001EF1 RID: 7921 RVA: 0x000B4EC0 File Offset: 0x000B30C0
	public void OnDialogueTextSound(DialoguerTextData data)
	{
		if (!string.IsNullOrEmpty(this.lastDialogueSFXName))
		{
			AudioManager.Stop(this.lastDialogueSFXName);
		}
		if (data.metadata == "excitedburst")
		{
			if (this.playFirstGroupExcited)
			{
				AudioManager.Play("ek_excitedburst");
				this.lastDialogueSFXName = "ek_excitedburst";
				this.playFirstGroupExcited = false;
			}
			else
			{
				AudioManager.Play("ek_excitedburst2");
				this.lastDialogueSFXName = "ek_excitedburst2";
				this.playFirstGroupExcited = true;
			}
		}
		else if (data.metadata == "laugh")
		{
			if (this.playFirstGroupLaugh)
			{
				AudioManager.Play("ek_laugh");
				this.lastDialogueSFXName = "ek_laugh";
				this.playFirstGroupLaugh = false;
			}
			else
			{
				AudioManager.Play("ek_laugh2");
				this.lastDialogueSFXName = "ek_laugh2";
				this.playFirstGroupLaugh = true;
			}
		}
		else if (data.metadata == "mckellen")
		{
			if (this.playFirstGroupMckellen)
			{
				AudioManager.Play("ek_mckellen");
				this.lastDialogueSFXName = "ek_mckellen";
				this.playFirstGroupMckellen = false;
			}
			else
			{
				AudioManager.Play("ek_mckellen2");
				this.lastDialogueSFXName = "ek_mckellen2";
				this.playFirstGroupMckellen = true;
			}
		}
		else if (data.metadata == "warstory")
		{
			if (this.playFirstGroupWarstory)
			{
				AudioManager.Play("ek_warstory");
				this.lastDialogueSFXName = "ek_warstory";
				this.playFirstGroupWarstory = false;
			}
			else
			{
				AudioManager.Play("ek_warstory2");
				this.lastDialogueSFXName = "ek_warstory2";
				this.playFirstGroupWarstory = true;
			}
		}
	}

	// Token: 0x06001EF2 RID: 7922 RVA: 0x0001A159 File Offset: 0x00018359
	public void LoopAnimation()
	{
		this.nbLoopsAnimator--;
	}

	// Token: 0x06001EF3 RID: 7923 RVA: 0x000B5070 File Offset: 0x000B3270
	public IEnumerator bottle_sound_cr()
	{
		yield return new WaitForSeconds(0.1f);
		AudioManager.Play("sfx_potion_reveal");
		yield break;
	}

	// Token: 0x06001EF4 RID: 7924 RVA: 0x000B5084 File Offset: 0x000B3284
	public IEnumerator talking_crs()
	{
		base.animator.SetBool("IsTalking", true);
		this.nbLoopsAnimator = Random.Range(2, 7);
		while (this.conversationIsActive)
		{
			if (this.nbLoopsAnimator == 0)
			{
				base.animator.SetTrigger("Continue");
				this.nbLoopsAnimator = Random.Range(2, 7);
			}
			yield return null;
		}
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Talking_Loop_B"))
		{
			base.animator.SetTrigger("Continue");
		}
		base.animator.SetBool("IsTalking", false);
		yield break;
	}

	// Token: 0x06001EF5 RID: 7925 RVA: 0x000B50A0 File Offset: 0x000B32A0
	public override IEnumerator ReactivateInputsCoroutine(LevelPlayerMotor playerOneMotor, LevelPlayerMotor playerTwoMotor, LevelPlayerWeaponManager playerOneWeaponManager, LevelPlayerWeaponManager playerTwoWeaponManager, Animator animator)
	{
		this.speechBubble.preventQuit = true;
		AbstractPlayerController playercontroller = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		if (playercontroller != null && playercontroller.animator != null && playercontroller.animator.GetCurrentAnimatorStateInfo(0).IsName("Power_Up"))
		{
			yield return playercontroller.animator.WaitForAnimationToEnd(this, "Power_Up", false, true);
		}
		this.speechBubble.preventQuit = false;
		yield return base.StartCoroutine(this.<ReactivateInputsCoroutine>__BaseCallProxy0(playerOneMotor, playerTwoMotor, playerOneWeaponManager, playerTwoWeaponManager, animator));
		yield break;
	}

	// Token: 0x0400194D RID: 6477
	public string lastDialogueSFXName;

	// Token: 0x0400194E RID: 6478
	public int nbLoopsAnimator;

	// Token: 0x0400194F RID: 6479
	public bool playFirstGroupMckellen = true;

	// Token: 0x04001950 RID: 6480
	public bool playFirstGroupWarstory = true;

	// Token: 0x04001951 RID: 6481
	public bool playFirstGroupExcited = true;

	// Token: 0x04001952 RID: 6482
	public bool playFirstGroupLaugh = true;
}
