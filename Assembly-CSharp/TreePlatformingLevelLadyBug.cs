using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003F7 RID: 1015
public class TreePlatformingLevelLadyBug : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x06002C82 RID: 11394 RVA: 0x000253DC File Offset: 0x000235DC
	public override void Awake()
	{
		base.Awake();
		this.manuallySetJumpX = true;
	}

	// Token: 0x06002C83 RID: 11395 RVA: 0x000253EB File Offset: 0x000235EB
	public override void Start()
	{
		this.Setup();
		base.Start();
	}

	// Token: 0x06002C84 RID: 11396 RVA: 0x000D9FA0 File Offset: 0x000D81A0
	public TreePlatformingLevelLadyBug Spawn(Vector3 pos, PlatformingLevelGroundMovementEnemy.Direction dir, bool destroy, TreePlatformingLevelLadyBug.Type type)
	{
		TreePlatformingLevelLadyBug treePlatformingLevelLadyBug = base.Spawn(pos, dir, destroy) as TreePlatformingLevelLadyBug;
		treePlatformingLevelLadyBug.type = type;
		return treePlatformingLevelLadyBug;
	}

	// Token: 0x06002C85 RID: 11397 RVA: 0x000D9FC8 File Offset: 0x000D81C8
	public void Setup()
	{
		switch (this.type)
		{
		case TreePlatformingLevelLadyBug.Type.GroundFast:
			base.GoToGround(true, "Fast_Ground");
			AudioManager.PlayLoop("level_platform_ladybug_ground_fast_loop");
			this.emitAudioFromObject.Add("level_platform_ladybug_ground_fast_loop");
			this.SetMoveSpeed(base.Properties.fastMovement);
			this.noTurn = true;
			base.StartCoroutine(this.no_y_cr());
			break;
		case TreePlatformingLevelLadyBug.Type.GroundSlow:
			base.GoToGround(true, "Slow_Ground");
			AudioManager.PlayLoop("level_platform_ladybug_ground_slow_loop");
			this.emitAudioFromObject.Add("level_platform_ladybug_ground_slow_loop");
			this.SetMoveSpeed(base.Properties.slowMovement);
			this.noTurn = true;
			base.StartCoroutine(this.no_y_cr());
			break;
		case TreePlatformingLevelLadyBug.Type.BounceFast:
			base.animator.Play("Fast_Bounce");
			AudioManager.PlayLoop("level_platform_ladybug_bounce_fast_loop");
			this.emitAudioFromObject.Add("level_platform_ladybug_bounce_fast_loop");
			this.SetMoveSpeed(base.Properties.fastMovement);
			base.StartCoroutine(this.y_cr());
			this.noTurn = true;
			break;
		case TreePlatformingLevelLadyBug.Type.BounceSlow:
			base.animator.Play("Slow_Bounce");
			AudioManager.PlayLoop("level_platform_ladybug_bounce_slow_loop");
			this.emitAudioFromObject.Add("level_platform_ladybug_bounce_slow_loop");
			this.SetMoveSpeed(base.Properties.slowMovement);
			base.StartCoroutine(this.y_cr());
			this.noTurn = true;
			break;
		case TreePlatformingLevelLadyBug.Type.BouncePink:
			this._canParry = true;
			base.animator.Play("Pink_Slow_Ground");
			AudioManager.PlayLoop("level_platform_ladybug_ground_slow_loop");
			this.emitAudioFromObject.Add("level_platform_ladybug_ground_slow_loop");
			this.SetMoveSpeed(base.Properties.slowMovement);
			base.StartCoroutine(this.y_cr());
			this.noTurn = true;
			break;
		}
	}

	// Token: 0x06002C86 RID: 11398 RVA: 0x000DA1A4 File Offset: 0x000D83A4
	public IEnumerator y_cr()
	{
		this.floating = false;
		yield return null;
		for (;;)
		{
			while (!base.Grounded)
			{
				yield return null;
			}
			base.Jump();
			AudioManager.Play("level_platform_ladybug_bounce");
			this.emitAudioFromObject.Add("level_platform_ladybug_bounce");
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C87 RID: 11399 RVA: 0x000DA1C0 File Offset: 0x000D83C0
	public IEnumerator no_y_cr()
	{
		for (;;)
		{
			if (!base.Grounded)
			{
				this.fallInPit = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C88 RID: 11400 RVA: 0x000DA1DC File Offset: 0x000D83DC
	public override void Die()
	{
		base.Die();
		AudioManager.Play("level_platform_ladybug_death");
		this.emitAudioFromObject.Add("level_platform_ladybug_death");
		switch (this.type)
		{
		case TreePlatformingLevelLadyBug.Type.GroundFast:
			AudioManager.Stop("level_platform_ladybug_ground_fast_loop");
			break;
		case TreePlatformingLevelLadyBug.Type.GroundSlow:
			AudioManager.Stop("level_platform_ladybug_ground_slow_loop");
			break;
		case TreePlatformingLevelLadyBug.Type.BounceFast:
			AudioManager.Stop("level_platform_ladybug_bounce_fast_loop");
			break;
		case TreePlatformingLevelLadyBug.Type.BounceSlow:
			AudioManager.Stop("level_platform_ladybug_bounce_slow_loop");
			break;
		case TreePlatformingLevelLadyBug.Type.BouncePink:
			AudioManager.Stop("level_platform_ladybug_ground_slow_loop");
			break;
		}
	}

	// Token: 0x040024B3 RID: 9395
	public TreePlatformingLevelLadyBug.Type type;

	// Token: 0x02001031 RID: 4145
	public enum Type
	{
		// Token: 0x04007388 RID: 29576
		GroundFast,
		// Token: 0x04007389 RID: 29577
		GroundSlow,
		// Token: 0x0400738A RID: 29578
		BounceFast,
		// Token: 0x0400738B RID: 29579
		BounceSlow,
		// Token: 0x0400738C RID: 29580
		BouncePink
	}
}
