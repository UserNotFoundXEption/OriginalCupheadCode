using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000429 RID: 1065
public class HarbourPlatformingLevelCrab : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x06002E0E RID: 11790 RVA: 0x000DE584 File Offset: 0x000DC784
	public override void Awake()
	{
		base.Awake();
		base.animator.SetBool("goingLeft", base.direction != PlatformingLevelGroundMovementEnemy.Direction.Right);
		base.GetComponent<DamageReceiver>().enabled = false;
		this.walkingBack = false;
		this.SetTurnTarget(this.target);
	}

	// Token: 0x06002E0F RID: 11791 RVA: 0x00026684 File Offset: 0x00024884
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.play_loop_SFX());
	}

	// Token: 0x06002E10 RID: 11792 RVA: 0x00026699 File Offset: 0x00024899
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (phase == CollisionPhase.Enter && hit.GetComponent<HarbourPlatformingLevelCrab>())
		{
			base.StartCoroutine(this.prepare_turn_cr(hit));
		}
	}

	// Token: 0x06002E11 RID: 11793 RVA: 0x000266C7 File Offset: 0x000248C7
	public override void CalculateDirection()
	{
	}

	// Token: 0x06002E12 RID: 11794 RVA: 0x000DE5DC File Offset: 0x000DC7DC
	public IEnumerator prepare_turn_cr(GameObject hit)
	{
		float dist = Vector3.Distance(hit.transform.position, base.transform.position);
		while (dist > 670f)
		{
			dist = Vector3.Distance(hit.transform.position, base.transform.position);
			yield return null;
		}
		this.Turn();
		yield return null;
		yield break;
	}

	// Token: 0x06002E13 RID: 11795 RVA: 0x000DE600 File Offset: 0x000DC800
	public override Coroutine Turn()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(1000f, 1000f)))
		{
			AudioManager.Play("harbour_crab_turn");
			this.emitAudioFromObject.Add("harbour_crab_turn");
		}
		this.walkingBack = !this.walkingBack;
		base.animator.SetBool("walkingBack", this.walkingBack);
		base.animator.SetBool("goingLeft", base.direction != PlatformingLevelGroundMovementEnemy.Direction.Right);
		this.target = ((base.direction != PlatformingLevelGroundMovementEnemy.Direction.Right) ? "Turn_Right" : "Turn_Left");
		this.SetTurnTarget(this.target);
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING))
		{
			CupheadLevelCamera.Current.Shake(10f, 0.4f, false);
		}
		return base.Turn();
	}

	// Token: 0x06002E14 RID: 11796 RVA: 0x000DE70C File Offset: 0x000DC90C
	public IEnumerator play_loop_SFX()
	{
		bool playerLeft = false;
		for (;;)
		{
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(1000f, 1000f)))
			{
				playerLeft = false;
				if (!AudioManager.CheckIfPlaying("harbour_crab_walk") && !AudioManager.CheckIfPlaying("harbour_crab_turn"))
				{
					AudioManager.PlayLoop("harbour_crab_walk");
					this.emitAudioFromObject.Add("harbour_crab_walk");
				}
			}
			else if (!playerLeft)
			{
				AudioManager.Stop("harbour_crab_walk");
				playerLeft = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002622 RID: 9762
	public const float ON_SCREEN_SOUND_PADDING = 1000f;

	// Token: 0x04002623 RID: 9763
	public string target;

	// Token: 0x04002624 RID: 9764
	public bool walkingBack;
}
