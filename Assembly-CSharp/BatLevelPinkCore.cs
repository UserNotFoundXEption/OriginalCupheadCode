using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000160 RID: 352
public class BatLevelPinkCore : ParrySwitch
{
	// Token: 0x060010DF RID: 4319 RVA: 0x0000E382 File Offset: 0x0000C582
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.lastSideHit = BatLevelPinkCore.SideHit.None;
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x0000E39C File Offset: 0x0000C59C
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x00091328 File Offset: 0x0008F528
	public void Init(LevelProperties.Bat.BatBouncer properties, Vector2 pos, float angle)
	{
		this.properties = properties;
		base.transform.position = pos;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(angle));
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x00091380 File Offset: 0x0008F580
	public IEnumerator move_cr()
	{
		this.velocity = -base.transform.right;
		for (;;)
		{
			base.transform.position += this.velocity * this.properties.mainBounceSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x0000E3B4 File Offset: 0x0000C5B4
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x0009139C File Offset: 0x0008F59C
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter && this.lastSideHit != BatLevelPinkCore.SideHit.Top)
		{
			Vector3 position = base.transform.position;
			Vector3 vector;
			vector..ctor(base.transform.position.x, (float)Level.Current.Ground, 0f);
			this.collisionPoint = vector - position;
			this.lastSideHit = BatLevelPinkCore.SideHit.Top;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x00091420 File Offset: 0x0008F620
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			if (base.transform.position.x > 0f)
			{
				if (this.lastSideHit != BatLevelPinkCore.SideHit.Right)
				{
					Vector3 vector;
					vector..ctor((float)Level.Current.Left, base.transform.position.y, 0f);
					Vector3 position = base.transform.position;
					this.collisionPoint = vector - position;
					this.lastSideHit = BatLevelPinkCore.SideHit.Right;
					base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
				}
			}
			else if (this.lastSideHit != BatLevelPinkCore.SideHit.Left)
			{
				Vector3 position2 = base.transform.position;
				Vector3 vector2;
				vector2..ctor((float)Level.Current.Right, base.transform.position.y, 0f);
				this.collisionPoint = vector2 - position2;
				this.lastSideHit = BatLevelPinkCore.SideHit.Left;
				base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
			}
		}
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x00091534 File Offset: 0x0008F734
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter && this.lastSideHit != BatLevelPinkCore.SideHit.Bottom)
		{
			Vector3 position = base.transform.position;
			Vector3 vector;
			vector..ctor(base.transform.position.x, (float)Level.Current.Ceiling, 0f);
			this.collisionPoint = vector - position;
			this.lastSideHit = BatLevelPinkCore.SideHit.Bottom;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x000915B8 File Offset: 0x0008F7B8
	public IEnumerator change_direction_cr(Vector3 collisionPoint)
	{
		this.velocity = 1f * (-2f * Vector3.Dot(this.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + this.velocity);
		yield return null;
		yield break;
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x0000E3CB File Offset: 0x0000C5CB
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000DB9 RID: 3513
	public LevelProperties.Bat.BatBouncer properties;

	// Token: 0x04000DBA RID: 3514
	public DamageDealer damageDealer;

	// Token: 0x04000DBB RID: 3515
	public Vector3 velocity;

	// Token: 0x04000DBC RID: 3516
	public Vector3 collisionPoint;

	// Token: 0x04000DBD RID: 3517
	public BatLevelPinkCore.SideHit lastSideHit;

	// Token: 0x02000A5F RID: 2655
	public enum SideHit
	{
		// Token: 0x04004C53 RID: 19539
		Top,
		// Token: 0x04004C54 RID: 19540
		Bottom,
		// Token: 0x04004C55 RID: 19541
		Left,
		// Token: 0x04004C56 RID: 19542
		Right,
		// Token: 0x04004C57 RID: 19543
		None
	}
}
