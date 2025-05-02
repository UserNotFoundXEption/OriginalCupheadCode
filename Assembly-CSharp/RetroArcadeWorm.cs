using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000325 RID: 805
public class RetroArcadeWorm : RetroArcadeEnemy
{
	// Token: 0x0600231F RID: 8991 RVA: 0x0001DC03 File Offset: 0x0001BE03
	public void LevelInit(LevelProperties.RetroArcade properties)
	{
		this.properties = properties;
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x000BFAEC File Offset: 0x000BDCEC
	public void StartWorm()
	{
		base.gameObject.SetActive(true);
		this.p = this.properties.CurrentState.worm;
		this.hp = this.p.hp;
		base.PointsWorth = this.p.pointsGained;
		this.platform.Rise();
		base.transform.SetPosition(new float?(0f), new float?(-650f), null);
		this.direction = ((!Rand.Bool()) ? RetroArcadeWorm.Direction.Right : RetroArcadeWorm.Direction.Left);
		this.tongue.transform.parent = null;
		this.tongue.Init(this.p);
		this.tongue.Extend();
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.rocket_cr());
	}

	// Token: 0x06002321 RID: 8993 RVA: 0x000BFBD0 File Offset: 0x000BDDD0
	public IEnumerator move_cr()
	{
		base.MoveY(430f, 100f);
		while (this.movingY)
		{
			yield return new WaitForFixedUpdate();
		}
		for (;;)
		{
			float normalizedHpRemaining = this.hp / this.p.hp;
			float speed = this.p.moveSpeed.min * Mathf.Pow(this.p.moveSpeed.max / this.p.moveSpeed.min, 1f - normalizedHpRemaining);
			base.transform.AddPosition((float)((this.direction != RetroArcadeWorm.Direction.Left) ? 1 : -1) * speed * CupheadTime.FixedDelta, 0f, 0f);
			if ((this.direction == RetroArcadeWorm.Direction.Left && base.transform.position.x < -160f) || (this.direction == RetroArcadeWorm.Direction.Right && base.transform.position.x > 160f))
			{
				this.direction = ((this.direction != RetroArcadeWorm.Direction.Left) ? RetroArcadeWorm.Direction.Left : RetroArcadeWorm.Direction.Right);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002322 RID: 8994 RVA: 0x000BFBEC File Offset: 0x000BDDEC
	public override void Dead()
	{
		this.StopAllCoroutines();
		foreach (Collider2D collider2D in base.GetComponentsInChildren<Collider2D>())
		{
			collider2D.enabled = false;
		}
		this.properties.DealDamageToNextNamedState();
		this.tongue.Retract();
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x06002323 RID: 8995 RVA: 0x000BFC48 File Offset: 0x000BDE48
	public IEnumerator moveOffscreen_cr()
	{
		base.MoveY(-430f, 100f);
		while (this.movingY)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002324 RID: 8996 RVA: 0x000BFC64 File Offset: 0x000BDE64
	public IEnumerator rocket_cr()
	{
		RetroArcadeWormRocket.Direction rocketDirection = (!Rand.Bool()) ? RetroArcadeWormRocket.Direction.Right : RetroArcadeWormRocket.Direction.Left;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.p.rocketSpawnDelay);
			this.rocketPrefab.Create(rocketDirection, this.p);
		}
		yield break;
	}

	// Token: 0x04001D2C RID: 7468
	public const float OFFSCREEN_Y = -650f;

	// Token: 0x04001D2D RID: 7469
	public const float ONSCREEN_Y = -220f;

	// Token: 0x04001D2E RID: 7470
	public const float MOVE_Y_SPEED = 100f;

	// Token: 0x04001D2F RID: 7471
	public const float TURNAROUND_X = 160f;

	// Token: 0x04001D30 RID: 7472
	public LevelProperties.RetroArcade properties;

	// Token: 0x04001D31 RID: 7473
	public LevelProperties.RetroArcade.Worm p;

	// Token: 0x04001D32 RID: 7474
	[SerializeField]
	public RetroArcadeWormPlatform platform;

	// Token: 0x04001D33 RID: 7475
	[SerializeField]
	public RetroArcadeWormTongue tongue;

	// Token: 0x04001D34 RID: 7476
	[SerializeField]
	public RetroArcadeWormRocket rocketPrefab;

	// Token: 0x04001D35 RID: 7477
	public RetroArcadeWorm.Direction direction;

	// Token: 0x02000E59 RID: 3673
	public enum Direction
	{
		// Token: 0x040067AD RID: 26541
		Left,
		// Token: 0x040067AE RID: 26542
		Right
	}
}
