using System;
using UnityEngine;

// Token: 0x02000401 RID: 1025
public class CircusPlatformingLevelBallRunner : PlatformingLevelPathMovementEnemy
{
	// Token: 0x06002CE0 RID: 11488 RVA: 0x000DB5B8 File Offset: 0x000D97B8
	public override void Die()
	{
		AudioManager.Play("circus_generic_death_fun");
		this.emitAudioFromObject.Add("circus_generic_death_fun");
		this.ball.transform.parent = null;
		this.ball.isMoving = true;
		this.ball.direction = new Vector3((float)this._direction, 0f, 0f);
		base.Die();
	}

	// Token: 0x06002CE1 RID: 11489 RVA: 0x000DB624 File Offset: 0x000D9824
	public void IdleSFX()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
		{
			AudioManager.Play("circus_ball_runner_idle");
			this.emitAudioFromObject.Add("circus_ball_runner_idle");
		}
	}

	// Token: 0x0400251D RID: 9501
	public const float ON_SCREEN_SOUND_PADDING = 100f;

	// Token: 0x0400251E RID: 9502
	[SerializeField]
	public CircusPlatformingLevelBallRunnerBall ball;

	// Token: 0x0400251F RID: 9503
	[SerializeField]
	public Transform ballRoot;
}
