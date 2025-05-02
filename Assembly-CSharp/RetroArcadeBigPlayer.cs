using System;
using UnityEngine;

// Token: 0x0200030E RID: 782
public class RetroArcadeBigPlayer : AbstractPausableComponent
{
	// Token: 0x06002287 RID: 8839 RVA: 0x000BDDCC File Offset: 0x000BBFCC
	public void Init(ArcadePlayerController player)
	{
		this.player = player;
		if (player == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		player.motor.OnHitEvent += this.OnHit;
		base.animator.SetBool("IsCuphead", this.isCuphead);
	}

	// Token: 0x06002288 RID: 8840 RVA: 0x000BDE28 File Offset: 0x000BC028
	public void FixedUpdate()
	{
		if (this.player == null || !this.trackingInputs)
		{
			return;
		}
		base.animator.SetBool("Dead", this.player.IsDead);
		base.animator.Update(Time.deltaTime);
		base.animator.Update(0f);
		if (base.animator.GetCurrentAnimatorStateInfo(3).IsName("Idle"))
		{
			if (this.player.input.actions.GetButtonDown(3))
			{
				base.animator.SetTrigger("A");
			}
			if (this.player.input.actions.GetButtonDown(2))
			{
				base.animator.SetTrigger("B");
			}
			if (this.player.input.actions.GetButtonDown(7))
			{
				base.animator.SetTrigger("C");
			}
			base.animator.SetInteger("MoveX", this.player.input.GetAxisInt(PlayerInput.Axis.X, false, false));
		}
		else
		{
			base.animator.Play("Idle", 2);
			base.animator.Play("Idle", 1);
			base.animator.SetInteger("MoveX", 0);
		}
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x0001D5AD File Offset: 0x0001B7AD
	public void OnHit()
	{
		base.animator.SetTrigger("Hit");
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x0001D5BF File Offset: 0x0001B7BF
	public void LevelStart()
	{
		this.trackingInputs = true;
	}

	// Token: 0x0600228B RID: 8843 RVA: 0x0001D5C8 File Offset: 0x0001B7C8
	public void OnVictory()
	{
		if (this.player != null && !this.player.IsDead)
		{
			base.animator.SetTrigger("Victory");
		}
	}

	// Token: 0x0600228C RID: 8844 RVA: 0x0001D5FB File Offset: 0x0001B7FB
	public void PlayButtonASound()
	{
		AudioManager.Play("level_button_a");
		this.emitAudioFromObject.Add("level_button_a");
	}

	// Token: 0x0600228D RID: 8845 RVA: 0x0001D617 File Offset: 0x0001B817
	public void PlayButtonBSound()
	{
		AudioManager.Play("level_button_b");
		this.emitAudioFromObject.Add("level_button_b");
	}

	// Token: 0x0600228E RID: 8846 RVA: 0x0001D633 File Offset: 0x0001B833
	public void PlayButtonCSound()
	{
		AudioManager.Play("level_button_c");
		this.emitAudioFromObject.Add("level_button_c");
	}

	// Token: 0x04001C88 RID: 7304
	public const int BOIL_LAYER = 0;

	// Token: 0x04001C89 RID: 7305
	public const int BUTTON_LAYER = 1;

	// Token: 0x04001C8A RID: 7306
	public const int STICK_LAYER = 2;

	// Token: 0x04001C8B RID: 7307
	public const int MAIN_LAYER = 3;

	// Token: 0x04001C8C RID: 7308
	public ArcadePlayerController player;

	// Token: 0x04001C8D RID: 7309
	[SerializeField]
	public bool isCuphead;

	// Token: 0x04001C8E RID: 7310
	public bool trackingInputs;
}
