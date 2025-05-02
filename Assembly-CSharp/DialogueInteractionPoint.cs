using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class DialogueInteractionPoint : SpeechInteractionPoint
{
	// Token: 0x060009B0 RID: 2480 RVA: 0x00079124 File Offset: 0x00077324
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = new Color(0.8627451f, 0.8627451f, 0.8627451f);
		Vector3 position = base.transform.position;
		position.x += this.speechBubblePosition.x;
		position.y += this.speechBubblePosition.y;
		Gizmos.DrawWireSphere(position, this.interactionDistance * 0.5f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.playerOneDialoguePosition, 10f);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.playerTwoDialoguePosition, 10f);
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x000791E0 File Offset: 0x000773E0
	public new virtual void Start()
	{
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueEndedHandler;
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeave;
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x0007923C File Offset: 0x0007743C
	public override void OnDestroy()
	{
		base.OnDestroy();
		Dialoguer.events.onEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.OnDialogueEndedHandler;
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeave;
		this.onEndedActionQueue.Clear();
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x000792A8 File Offset: 0x000774A8
	public void OnDialogueEndedHandler()
	{
		if (base.AbleToActivate())
		{
			this.Show(PlayerId.PlayerOne);
		}
		foreach (Action action in this.onEndedActionQueue)
		{
			action();
		}
		this.onEndedActionQueue.Clear();
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x00079320 File Offset: 0x00077520
	public override void Activate()
	{
		if (this.speechBubble.displayState == SpeechBubble.DisplayState.Hidden)
		{
			Vector3 position = base.transform.position;
			position.x += this.speechBubblePosition.x;
			position.y += this.speechBubblePosition.y;
			this.speechBubble.basePosition = position;
			if (this.cutsceneCoroutine != null)
			{
				base.StopCoroutine(this.cutsceneCoroutine);
			}
			this.cutsceneCoroutine = base.StartCoroutine(this.CutScene_cr());
		}
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x000793B8 File Offset: 0x000775B8
	public void OnPlayerJoined(PlayerId player)
	{
		if (player == PlayerId.PlayerTwo)
		{
			AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			player2.OnReviveEvent += this.OnRevive;
		}
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x000793E8 File Offset: 0x000775E8
	public IEnumerator move_cr(AbstractPlayerController player, float xPosition)
	{
		if (player == null)
		{
			yield break;
		}
		yield return null;
		while (!player.gameObject.activeSelf)
		{
			yield return null;
		}
		LevelPlayerMotor playerMotor = null;
		LevelPlayerWeaponManager playerWeaponManager = null;
		if (player)
		{
			playerMotor = player.GetComponent<LevelPlayerMotor>();
			playerWeaponManager = player.GetComponent<LevelPlayerWeaponManager>();
			if (playerWeaponManager)
			{
				playerWeaponManager.DisableInput();
			}
			if (playerMotor)
			{
				while (playerMotor.Dashing)
				{
					yield return null;
				}
				playerMotor.DisableInput();
				yield return playerMotor.StartCoroutine(playerMotor.MoveToX_cr(xPosition, 1));
			}
			this.onEndedActionQueue.Add(delegate
			{
				this.$this.StartCoroutine(this.$this.ReactivateInputsCoroutine(playerMotor, null, playerWeaponManager, null, this.$this.animatorOnEnd));
			});
		}
		yield break;
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x00008FF7 File Offset: 0x000071F7
	public void OnPlayerLeave(PlayerId player)
	{
		if (player == PlayerId.PlayerTwo)
		{
		}
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x00079414 File Offset: 0x00077614
	public void OnRevive(Vector3 pos)
	{
		if (this.conversationIsActive)
		{
			AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			base.StartCoroutine(this.move_cr(player, this.playerTwoDialoguePosition.x));
		}
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x0007944C File Offset: 0x0007764C
	public IEnumerator CutScene_cr()
	{
		if (this.speechBubble.displayState != SpeechBubble.DisplayState.Hidden)
		{
			yield break;
		}
		Coroutine playerOneMove = null;
		Coroutine playerTwoMove = null;
		this.conversationIsActive = true;
		AbstractPlayerController playerOne = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController playerTwo = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		playerOneMove = base.StartCoroutine(this.move_cr(playerOne, this.playerOneDialoguePosition.x));
		playerTwoMove = base.StartCoroutine(this.move_cr(playerTwo, this.playerTwoDialoguePosition.x));
		yield return playerOneMove;
		yield return playerTwoMove;
		if (this.animatorOnStart != null)
		{
			this.StartAnimation();
			while (!this.animatorOnStart.GetCurrentAnimatorStateInfo(0).IsName(this.animationOnStartTextName))
			{
				yield return null;
			}
		}
		Dialoguer.StartDialogue(this.dialogueInteraction);
		this.onEndedActionQueue.Add(delegate
		{
			if (this.$this.animatorOnEnd != null)
			{
				this.$this.EndAnimation();
			}
			playerOne = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			playerTwo = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			LevelPlayerMotor playerTwoMotor = null;
			LevelPlayerWeaponManager playerTwoWeaponManager = null;
			if (playerTwo != null)
			{
				playerTwoMotor = playerTwo.GetComponent<LevelPlayerMotor>();
				playerTwoWeaponManager = playerTwo.GetComponent<LevelPlayerWeaponManager>();
			}
			this.$this.conversationIsActive = false;
			this.$this.StartCoroutine(this.$this.ReactivateInputsCoroutine(playerOne.GetComponent<LevelPlayerMotor>(), playerTwoMotor, playerOne.GetComponent<LevelPlayerWeaponManager>(), playerTwoWeaponManager, this.$this.animatorOnEnd));
		});
		yield break;
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x00079468 File Offset: 0x00077668
	public virtual IEnumerator ReactivateInputsCoroutine(LevelPlayerMotor playerOneMotor, LevelPlayerMotor playerTwoMotor, LevelPlayerWeaponManager playerOneWeaponManager, LevelPlayerWeaponManager playerTwoWeaponManager, Animator animator)
	{
		if (animator != null)
		{
			if (this.animationOnGiveBackInputAtEnd != null && this.animationOnGiveBackInputAtEnd != string.Empty)
			{
				while (!animator.GetCurrentAnimatorStateInfo(0).IsName(this.animationOnGiveBackInput) && (!animator.GetCurrentAnimatorStateInfo(0).IsName(this.animationOnGiveBackInputAtEnd) || (double)animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.99))
				{
					yield return null;
				}
			}
			else
			{
				while (!animator.GetCurrentAnimatorStateInfo(0).IsName(this.animationOnGiveBackInput))
				{
					yield return null;
				}
			}
		}
		playerOneMotor.ClearBufferedInput();
		playerOneMotor.EnableInput();
		playerOneWeaponManager.EnableInput();
		if (playerTwoMotor)
		{
			playerTwoMotor.ClearBufferedInput();
			playerTwoMotor.EnableInput();
		}
		if (playerTwoWeaponManager)
		{
			playerTwoWeaponManager.EnableInput();
		}
		yield break;
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x00009000 File Offset: 0x00007200
	public virtual void StartAnimation()
	{
		this.animatorOnStart.SetTrigger(this.animationTriggerOnStart);
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x00009013 File Offset: 0x00007213
	public virtual void EndAnimation()
	{
		this.animatorOnEnd.SetTrigger(this.animationTriggerOnEnd);
	}

	// Token: 0x04000744 RID: 1860
	[SerializeField]
	public SpeechBubble speechBubble;

	// Token: 0x04000745 RID: 1861
	[SerializeField]
	public DialoguerDialogues dialogueInteraction;

	// Token: 0x04000746 RID: 1862
	[SerializeField]
	public Vector2 speechBubblePosition;

	// Token: 0x04000747 RID: 1863
	public Vector2 playerOneDialoguePosition;

	// Token: 0x04000748 RID: 1864
	public Vector2 playerTwoDialoguePosition;

	// Token: 0x04000749 RID: 1865
	public Animator animatorOnStart;

	// Token: 0x0400074A RID: 1866
	public string animationTriggerOnStart;

	// Token: 0x0400074B RID: 1867
	public string animationOnStartTextName;

	// Token: 0x0400074C RID: 1868
	public Animator animatorOnEnd;

	// Token: 0x0400074D RID: 1869
	public string animationTriggerOnEnd;

	// Token: 0x0400074E RID: 1870
	public string animationOnGiveBackInput;

	// Token: 0x0400074F RID: 1871
	public string animationOnGiveBackInputAtEnd;

	// Token: 0x04000750 RID: 1872
	public Coroutine cutsceneCoroutine;

	// Token: 0x04000751 RID: 1873
	public List<Action> onEndedActionQueue = new List<Action>();

	// Token: 0x04000752 RID: 1874
	[HideInInspector]
	public bool conversationIsActive;
}
