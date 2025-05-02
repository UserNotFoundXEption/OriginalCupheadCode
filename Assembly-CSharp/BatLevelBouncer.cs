using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000159 RID: 345
public class BatLevelBouncer : AbstractCollidableObject
{
	// Token: 0x060010AA RID: 4266 RVA: 0x0000E090 File Offset: 0x0000C290
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.lastSideHit = BatLevelBouncer.SideHit.None;
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x0000E0AA File Offset: 0x0000C2AA
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x00090B54 File Offset: 0x0008ED54
	public void Init(LevelProperties.Bat.BatBouncer properties, Vector2 pos, float angle)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.angle = angle;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(angle));
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x00090BB4 File Offset: 0x0008EDB4
	public IEnumerator move_cr()
	{
		this.velocity = -base.transform.right;
		for (;;)
		{
			base.transform.position += this.velocity * this.properties.mainBounceSpeed * CupheadTime.FixedDelta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x0000E0C2 File Offset: 0x0000C2C2
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x00090BD0 File Offset: 0x0008EDD0
	public override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter && this.lastSideHit != BatLevelBouncer.SideHit.Top)
		{
			Vector3 position = base.transform.position;
			Vector3 vector;
			vector..ctor(base.transform.position.x, (float)Level.Current.Ground, 0f);
			this.collisionPoint = vector - position;
			this.lastSideHit = BatLevelBouncer.SideHit.Top;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x00090C54 File Offset: 0x0008EE54
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			if (base.transform.position.x > 0f)
			{
				if (this.lastSideHit != BatLevelBouncer.SideHit.Right)
				{
					Vector3 vector;
					vector..ctor((float)Level.Current.Left, base.transform.position.y, 0f);
					Vector3 position = base.transform.position;
					this.collisionPoint = vector - position;
					this.lastSideHit = BatLevelBouncer.SideHit.Right;
					base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
				}
			}
			else if (this.lastSideHit != BatLevelBouncer.SideHit.Left)
			{
				Vector3 position2 = base.transform.position;
				Vector3 vector2;
				vector2..ctor((float)Level.Current.Right, base.transform.position.y, 0f);
				this.collisionPoint = vector2 - position2;
				this.lastSideHit = BatLevelBouncer.SideHit.Left;
				base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
			}
		}
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x00090D68 File Offset: 0x0008EF68
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter && this.lastSideHit != BatLevelBouncer.SideHit.Bottom)
		{
			Vector3 position = base.transform.position;
			Vector3 vector;
			vector..ctor(base.transform.position.x, (float)Level.Current.Ceiling, 0f);
			this.collisionPoint = vector - position;
			this.lastSideHit = BatLevelBouncer.SideHit.Bottom;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x00090DEC File Offset: 0x0008EFEC
	public IEnumerator change_direction_cr(Vector3 collisionPoint)
	{
		this.velocity = 1f * (-2f * Vector3.Dot(this.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + this.velocity);
		this.counter++;
		if ((float)this.counter >= this.properties.breakCounter && !this.isPink)
		{
			this.SpawnPink();
			this.Die();
		}
		yield return null;
		yield break;
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x00090E10 File Offset: 0x0008F010
	public void SpawnPink()
	{
		BatLevelPinkCore batLevelPinkCore = Object.Instantiate<BatLevelPinkCore>(this.pinkPrefab);
		batLevelPinkCore.Init(this.properties, base.transform.position, this.angle);
	}

	// Token: 0x060010B4 RID: 4276 RVA: 0x0000E0D9 File Offset: 0x0000C2D9
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000D90 RID: 3472
	[SerializeField]
	public BatLevelPinkCore pinkPrefab;

	// Token: 0x04000D91 RID: 3473
	public LevelProperties.Bat.BatBouncer properties;

	// Token: 0x04000D92 RID: 3474
	public DamageDealer damageDealer;

	// Token: 0x04000D93 RID: 3475
	public Vector3 velocity;

	// Token: 0x04000D94 RID: 3476
	public Vector3 collisionPoint;

	// Token: 0x04000D95 RID: 3477
	public BatLevelBouncer.SideHit lastSideHit;

	// Token: 0x04000D96 RID: 3478
	public bool isPink;

	// Token: 0x04000D97 RID: 3479
	public float angle;

	// Token: 0x04000D98 RID: 3480
	public int counter;

	// Token: 0x02000A56 RID: 2646
	public enum SideHit
	{
		// Token: 0x04004C23 RID: 19491
		Top,
		// Token: 0x04004C24 RID: 19492
		Bottom,
		// Token: 0x04004C25 RID: 19493
		Left,
		// Token: 0x04004C26 RID: 19494
		Right,
		// Token: 0x04004C27 RID: 19495
		None
	}
}
