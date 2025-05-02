using System;
using UnityEngine;

// Token: 0x02000402 RID: 1026
public class CircusPlatformingLevelBallRunnerBall : AbstractCollidableObject
{
	// Token: 0x06002CE3 RID: 11491 RVA: 0x000257AA File Offset: 0x000239AA
	public void Start()
	{
		base.animator.SetBool("isBlue", Rand.Bool());
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002CE4 RID: 11492 RVA: 0x000DB67C File Offset: 0x000D987C
	public override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
		if (hit.GetComponentInParent<PlatformingLevelEditorPlatform>() && phase == CollisionPhase.Enter && this.isMoving)
		{
			this.direction *= -1f;
		}
	}

	// Token: 0x06002CE5 RID: 11493 RVA: 0x000257CC File Offset: 0x000239CC
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002CE6 RID: 11494 RVA: 0x000257E4 File Offset: 0x000239E4
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002CE7 RID: 11495 RVA: 0x00025802 File Offset: 0x00023A02
	public void FixedUpdate()
	{
		if (this.isMoving)
		{
			this.Move();
		}
	}

	// Token: 0x06002CE8 RID: 11496 RVA: 0x00025815 File Offset: 0x00023A15
	public virtual void Move()
	{
		base.transform.position += this.direction * this.Speed * CupheadTime.FixedDelta;
	}

	// Token: 0x04002520 RID: 9504
	public bool isMoving;

	// Token: 0x04002521 RID: 9505
	[SerializeField]
	public float Speed = 500f;

	// Token: 0x04002522 RID: 9506
	[SerializeField]
	public CircusPlatformingLevelBallRunner runner;

	// Token: 0x04002523 RID: 9507
	public DamageDealer damageDealer;

	// Token: 0x04002524 RID: 9508
	public Vector3 direction = Vector3.right;
}
