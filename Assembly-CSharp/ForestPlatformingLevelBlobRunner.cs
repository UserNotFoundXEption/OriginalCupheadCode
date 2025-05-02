using System;
using System.Collections;

// Token: 0x020003E5 RID: 997
public class ForestPlatformingLevelBlobRunner : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x06002BFF RID: 11263 RVA: 0x00024D8F File Offset: 0x00022F8F
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.idle_audio_delayer_cr("level_blobrunner", 2f, 4f));
		this.emitAudioFromObject.Add("level_blobrunner");
	}

	// Token: 0x06002C00 RID: 11264 RVA: 0x00024DC3 File Offset: 0x00022FC3
	public override void FixedUpdate()
	{
		if (!this.melted)
		{
			base.FixedUpdate();
		}
	}

	// Token: 0x06002C01 RID: 11265 RVA: 0x00024DD6 File Offset: 0x00022FD6
	public override void Die()
	{
		this.IdleSounds = false;
		this.melted = true;
		this.collider.enabled = false;
		base.StartCoroutine(this.melt_cr());
	}

	// Token: 0x06002C02 RID: 11266 RVA: 0x000D8F7C File Offset: 0x000D717C
	public IEnumerator melt_cr()
	{
		AudioManager.Stop("level_blobrunner");
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING))
		{
			AudioManager.Play("level_frogs_tall_firefly_death");
		}
		base.animator.Play("Melt");
		yield return base.animator.WaitForAnimationToEnd(this, "Melt", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.Properties.BlobRunnerMeltDelay.RandomFloat());
		base.animator.SetTrigger("Continue");
		AudioManager.Play("level_blobrunner_reform");
		this.emitAudioFromObject.Add("level_blobrunner_reform");
		yield return CupheadTime.WaitForSeconds(this, base.Properties.BlobRunnerUnmeltLoopTime);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Unmelt", false, true);
		this.melted = false;
		this.collider.enabled = true;
		base.Health = base.Properties.Health;
		this.turning = false;
		this.timeSinceTurn = 10000f;
		this.IdleSounds = true;
		yield break;
	}

	// Token: 0x0400246E RID: 9326
	public bool melted;
}
