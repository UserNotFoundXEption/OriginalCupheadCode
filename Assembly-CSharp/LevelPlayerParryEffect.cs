using System;
using UnityEngine;

// Token: 0x02000516 RID: 1302
public class LevelPlayerParryEffect : AbstractParryEffect
{
	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x060036FE RID: 14078 RVA: 0x0002CF34 File Offset: 0x0002B134
	public override bool IsHit
	{
		get
		{
			return (this.player as LevelPlayerController).motor.IsHit;
		}
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x060036FF RID: 14079 RVA: 0x0002CF4B File Offset: 0x0002B14B
	public LevelPlayerController levelPlayer
	{
		get
		{
			return this.player as LevelPlayerController;
		}
	}

	// Token: 0x06003700 RID: 14080 RVA: 0x001018D0 File Offset: 0x000FFAD0
	public override void SetPlayer(AbstractPlayerController player)
	{
		base.SetPlayer(player);
		if (player.stats.Loadout.charm == Charm.charm_chalice)
		{
			base.GetComponent<CircleCollider2D>().offset = new Vector2(29.5f, 10f);
			base.GetComponent<CircleCollider2D>().radius = 80f;
		}
		this.levelPlayer.motor.OnHitEvent += this.OnHitCancel;
		this.levelPlayer.motor.OnGroundedEvent += this.OnGroundedCancel;
		this.levelPlayer.motor.OnDashStartEvent += this.OnDashCancel;
		this.levelPlayer.weaponManager.OnExStart += this.OnWeaponCancel;
		this.levelPlayer.weaponManager.OnSuperStart += this.OnWeaponCancel;
	}

	// Token: 0x06003701 RID: 14081 RVA: 0x001019B8 File Offset: 0x000FFBB8
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.levelPlayer.motor.OnHitEvent -= this.OnHitCancel;
		this.levelPlayer.motor.OnGroundedEvent -= this.OnGroundedCancel;
		this.levelPlayer.motor.OnDashStartEvent -= this.OnDashCancel;
		this.levelPlayer.weaponManager.OnExStart -= this.OnWeaponCancel;
		this.levelPlayer.weaponManager.OnSuperStart -= this.OnWeaponCancel;
	}

	// Token: 0x06003702 RID: 14082 RVA: 0x0002CF58 File Offset: 0x0002B158
	public override void OnHitCancel()
	{
		base.OnHitCancel();
		this.levelPlayer.motor.OnParryHit();
	}

	// Token: 0x06003703 RID: 14083 RVA: 0x0002CF70 File Offset: 0x0002B170
	public void OnDashCancel()
	{
		if (this.didHitSomething || this == null)
		{
			return;
		}
		this.Cancel();
	}

	// Token: 0x06003704 RID: 14084 RVA: 0x0002CF90 File Offset: 0x0002B190
	public void OnGroundedCancel()
	{
		if (this.player.stats.isChalice)
		{
			return;
		}
		if (this.didHitSomething || this == null)
		{
			return;
		}
		this.Cancel();
	}

	// Token: 0x06003705 RID: 14085 RVA: 0x0002CFC6 File Offset: 0x0002B1C6
	public void OnWeaponCancel()
	{
		if (this.didHitSomething || this == null)
		{
			return;
		}
		this.Cancel();
	}

	// Token: 0x06003706 RID: 14086 RVA: 0x0002CFE6 File Offset: 0x0002B1E6
	public override void Cancel()
	{
		base.Cancel();
		this.levelPlayer.animationController.ResumeNormanAnim();
	}

	// Token: 0x06003707 RID: 14087 RVA: 0x0002CFFE File Offset: 0x0002B1FE
	public override void CancelSwitch()
	{
		base.CancelSwitch();
		this.levelPlayer.motor.OnParryCanceled();
	}

	// Token: 0x06003708 RID: 14088 RVA: 0x0002D016 File Offset: 0x0002B216
	public override void OnPaused()
	{
		base.OnPaused();
		this.levelPlayer.animationController.OnParryPause();
		this.levelPlayer.weaponManager.ParrySuccess();
	}

	// Token: 0x06003709 RID: 14089 RVA: 0x0002D03E File Offset: 0x0002B23E
	public override void OnUnpaused()
	{
		base.OnUnpaused();
		this.levelPlayer.animationController.ResumeNormanAnim();
		this.levelPlayer.motor.OnParryComplete();
	}

	// Token: 0x0600370A RID: 14090 RVA: 0x0002D066 File Offset: 0x0002B266
	public override void OnSuccess()
	{
		base.OnSuccess();
		this.levelPlayer.weaponManager.ParrySuccess();
		this.levelPlayer.animationController.OnParrySuccess();
	}

	// Token: 0x0600370B RID: 14091 RVA: 0x0002D08E File Offset: 0x0002B28E
	public override void OnEnd()
	{
		base.OnEnd();
		this.levelPlayer.motor.ResetChaliceDoubleJump();
		this.levelPlayer.animationController.OnParryAnimEnd();
	}

	// Token: 0x04002C5F RID: 11359
	public const float CHALICE_X_OFFSET = 29.5f;

	// Token: 0x04002C60 RID: 11360
	public const float CHALICE_Y_OFFSET = 10f;

	// Token: 0x04002C61 RID: 11361
	public const float CHALICE_RADIUS = 80f;
}
