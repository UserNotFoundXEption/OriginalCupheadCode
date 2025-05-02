using System;

// Token: 0x020004FF RID: 1279
public class ArcadePlayerParryEffect : AbstractParryEffect
{
	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x0600354C RID: 13644 RVA: 0x0002BBE3 File Offset: 0x00029DE3
	public override bool IsHit
	{
		get
		{
			return (this.player as ArcadePlayerController).motor.IsHit;
		}
	}

	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x0600354D RID: 13645 RVA: 0x0002BBFA File Offset: 0x00029DFA
	public ArcadePlayerController levelPlayer
	{
		get
		{
			return this.player as ArcadePlayerController;
		}
	}

	// Token: 0x0600354E RID: 13646 RVA: 0x000F9AD8 File Offset: 0x000F7CD8
	public override void SetPlayer(AbstractPlayerController player)
	{
		base.SetPlayer(player);
		this.levelPlayer.motor.OnHitEvent += this.OnHitCancel;
		this.levelPlayer.motor.OnGroundedEvent += this.OnGroundedCancel;
		this.levelPlayer.motor.OnDashStartEvent += this.OnDashCancel;
		this.levelPlayer.weaponManager.OnExStart += this.OnWeaponCancel;
		this.levelPlayer.weaponManager.OnSuperStart += this.OnWeaponCancel;
	}

	// Token: 0x0600354F RID: 13647 RVA: 0x000F9B7C File Offset: 0x000F7D7C
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.levelPlayer.motor.OnHitEvent -= this.OnHitCancel;
		this.levelPlayer.motor.OnGroundedEvent -= this.OnGroundedCancel;
		this.levelPlayer.motor.OnDashStartEvent -= this.OnDashCancel;
		this.levelPlayer.weaponManager.OnExStart -= this.OnWeaponCancel;
		this.levelPlayer.weaponManager.OnSuperStart -= this.OnWeaponCancel;
	}

	// Token: 0x06003550 RID: 13648 RVA: 0x0002BC07 File Offset: 0x00029E07
	public override void OnHitCancel()
	{
		base.OnHitCancel();
		this.levelPlayer.motor.OnParryHit();
	}

	// Token: 0x06003551 RID: 13649 RVA: 0x0002BC1F File Offset: 0x00029E1F
	public void OnDashCancel()
	{
		if (this.didHitSomething || this == null)
		{
			return;
		}
		this.Cancel();
	}

	// Token: 0x06003552 RID: 13650 RVA: 0x0002BC3F File Offset: 0x00029E3F
	public void OnGroundedCancel()
	{
		if (this.didHitSomething || this == null)
		{
			return;
		}
		this.Cancel();
	}

	// Token: 0x06003553 RID: 13651 RVA: 0x0002BC5F File Offset: 0x00029E5F
	public void OnWeaponCancel()
	{
		if (this.didHitSomething || this == null)
		{
			return;
		}
		this.Cancel();
	}

	// Token: 0x06003554 RID: 13652 RVA: 0x0002BC7F File Offset: 0x00029E7F
	public override void Cancel()
	{
		base.Cancel();
		this.levelPlayer.animationController.ResumeNormanAnim();
	}

	// Token: 0x06003555 RID: 13653 RVA: 0x0002BC97 File Offset: 0x00029E97
	public override void CancelSwitch()
	{
		base.CancelSwitch();
		this.levelPlayer.motor.OnParryCanceled();
	}

	// Token: 0x06003556 RID: 13654 RVA: 0x0002BCAF File Offset: 0x00029EAF
	public override void OnPaused()
	{
		base.OnPaused();
		this.levelPlayer.animationController.OnParryPause();
		this.levelPlayer.weaponManager.ParrySuccess();
	}

	// Token: 0x06003557 RID: 13655 RVA: 0x0002BCD7 File Offset: 0x00029ED7
	public override void OnUnpaused()
	{
		base.OnUnpaused();
		this.levelPlayer.animationController.ResumeNormanAnim();
		this.levelPlayer.motor.OnParryComplete();
	}

	// Token: 0x06003558 RID: 13656 RVA: 0x0002BCFF File Offset: 0x00029EFF
	public override void OnSuccess()
	{
		base.OnSuccess();
		this.levelPlayer.weaponManager.ParrySuccess();
		this.levelPlayer.animationController.OnParrySuccess();
	}

	// Token: 0x06003559 RID: 13657 RVA: 0x0002BD27 File Offset: 0x00029F27
	public override void OnEnd()
	{
		base.OnEnd();
		this.levelPlayer.animationController.OnParryAnimEnd();
	}
}
