using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002BA RID: 698
public class MausoleumDialogueInteraction : DialogueInteractionPoint
{
	// Token: 0x06001F13 RID: 7955 RVA: 0x0001A303 File Offset: 0x00018503
	public void BeginDialogue()
	{
		this.Activate();
		this.chaliceAnimator.SetBool("Talking", true);
		this.speechBubble.waitForRealease = false;
	}

	// Token: 0x06001F14 RID: 7956 RVA: 0x0001A328 File Offset: 0x00018528
	public override void Start()
	{
		base.Start();
		Dialoguer.events.onTextPhase += this.OnDialogueTextSound;
	}

	// Token: 0x06001F15 RID: 7957 RVA: 0x0001A346 File Offset: 0x00018546
	public override void OnDestroy()
	{
		base.OnDestroy();
		Dialoguer.events.onTextPhase -= this.OnDialogueTextSound;
	}

	// Token: 0x06001F16 RID: 7958 RVA: 0x0001A364 File Offset: 0x00018564
	public void OnDialogueTextSound(DialoguerTextData data)
	{
		if (!string.IsNullOrEmpty("mausoleum_queen_ghost_speech"))
		{
			AudioManager.Stop("mausoleum_queen_ghost_speech");
		}
		AudioManager.Play("mausoleum_queen_ghost_speech");
	}

	// Token: 0x06001F17 RID: 7959 RVA: 0x000B5508 File Offset: 0x000B3708
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
		this.chaliceAnimator.SetBool("Talking", false);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		SceneLoader.LoadLastMap();
		yield break;
	}

	// Token: 0x04001966 RID: 6502
	public Animator chaliceAnimator;
}
