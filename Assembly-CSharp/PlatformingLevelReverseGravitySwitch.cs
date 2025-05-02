using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000463 RID: 1123
public class PlatformingLevelReverseGravitySwitch : ParrySwitch
{
	// Token: 0x06002FD7 RID: 12247 RVA: 0x000E2C58 File Offset: 0x000E0E58
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		LevelPlayerController levelPlayerController = player as LevelPlayerController;
		levelPlayerController.motor.SetGravityReversed(!levelPlayerController.motor.GravityReversed);
		base.StartCoroutine(this.start_spin_cr(levelPlayerController));
		base.StartParryCooldown();
	}

	// Token: 0x06002FD8 RID: 12248 RVA: 0x000E2CA0 File Offset: 0x000E0EA0
	public IEnumerator start_spin_cr(LevelPlayerController player)
	{
		base.animator.SetBool("IsSpin", true);
		base.animator.SetBool("IsUp", player.motor.GravityReversed);
		float t = 0f;
		while (t < this.spinTimer)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetBool("IsSpin", false);
		yield return null;
		yield break;
	}

	// Token: 0x0400279D RID: 10141
	[SerializeField]
	public float spinTimer;
}
