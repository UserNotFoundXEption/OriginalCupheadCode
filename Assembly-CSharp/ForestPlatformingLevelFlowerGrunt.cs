using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003E8 RID: 1000
public class ForestPlatformingLevelFlowerGrunt : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x06002C10 RID: 11280 RVA: 0x000D9204 File Offset: 0x000D7404
	public override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.idle_audio_delayer_cr("level_flowergrunt", 2f, 4f));
		this.emitAudioFromObject.Add("level_flowergrunt");
		this.emitAudioFromObject.Add("level_flowergrunt_float");
	}

	// Token: 0x06002C11 RID: 11281 RVA: 0x00024E5E File Offset: 0x0002305E
	public override void OnStart()
	{
		base.OnStart();
		if (this.floating)
		{
			AudioManager.Play("level_flowergrunt_float");
			base.StartCoroutine(this.handle_float_cr());
		}
	}

	// Token: 0x06002C12 RID: 11282 RVA: 0x000D9254 File Offset: 0x000D7454
	public IEnumerator handle_float_cr()
	{
		while (this.floating)
		{
			yield return null;
		}
		AudioManager.Play("level_flowergrunt_land");
		this.emitAudioFromObject.Add("level_flowergrunt_land");
		yield break;
	}

	// Token: 0x06002C13 RID: 11283 RVA: 0x00024E88 File Offset: 0x00023088
	public override void Die()
	{
		AudioManager.Play("level_flowergrunt_death");
		this.emitAudioFromObject.Add("level_flowergrunt_death");
		base.FrameDelayedCallback(new Action(this.Kill), 1);
	}

	// Token: 0x06002C14 RID: 11284 RVA: 0x00024EB8 File Offset: 0x000230B8
	public void Kill()
	{
		base.Die();
	}

	// Token: 0x06002C15 RID: 11285 RVA: 0x000D9270 File Offset: 0x000D7470
	public float adjustSpeed(float speed)
	{
		return Random.Range(speed * 0.12f, speed);
	}
}
