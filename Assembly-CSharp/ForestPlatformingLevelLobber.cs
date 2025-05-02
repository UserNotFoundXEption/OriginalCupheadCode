using System;
using UnityEngine;

// Token: 0x020003E9 RID: 1001
public class ForestPlatformingLevelLobber : PlatformingLevelShootingEnemy
{
	// Token: 0x06002C17 RID: 11287 RVA: 0x00024EC8 File Offset: 0x000230C8
	public override void Awake()
	{
		base.Awake();
		base.animator.Play("Idle", 0, Random.Range(0f, 1f));
	}

	// Token: 0x06002C18 RID: 11288 RVA: 0x00024EF0 File Offset: 0x000230F0
	public override void Shoot()
	{
		base.Shoot();
	}

	// Token: 0x06002C19 RID: 11289 RVA: 0x00024EF8 File Offset: 0x000230F8
	public void PlayLobberSound()
	{
		AudioManager.Play("level_forestlobber_shoot");
		this.emitAudioFromObject.Add("level_forestlobber_shoot");
	}

	// Token: 0x06002C1A RID: 11290 RVA: 0x00024F14 File Offset: 0x00023114
	public override void Die()
	{
		AudioManager.Play("level_mermaid_turtle_shell_pop");
		this.emitAudioFromObject.Add("level_mermaid_turtle_shell_pop");
		base.FrameDelayedCallback(new Action(this.Kill), 1);
	}

	// Token: 0x06002C1B RID: 11291 RVA: 0x00024F44 File Offset: 0x00023144
	public void Kill()
	{
		base.Die();
	}

	// Token: 0x06002C1C RID: 11292 RVA: 0x00024F4C File Offset: 0x0002314C
	public override void OnDestroy()
	{
		base.OnDestroy();
		this._shootEffect = null;
	}
}
