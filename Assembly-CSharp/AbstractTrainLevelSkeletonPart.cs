using System;
using UnityEngine;

// Token: 0x020003A5 RID: 933
public class AbstractTrainLevelSkeletonPart : AbstractCollidableObject
{
	// Token: 0x06002964 RID: 10596 RVA: 0x00022C7D File Offset: 0x00020E7D
	public override void Awake()
	{
		base.Awake();
		this.exploder = base.GetComponent<LevelBossDeathExploder>();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002965 RID: 10597 RVA: 0x000D1F50 File Offset: 0x000D0150
	public void SetPosition(TrainLevelSkeleton.Position position)
	{
		switch (position)
		{
		case TrainLevelSkeleton.Position.Right:
			base.transform.SetPosition(new float?(470f), null, null);
			break;
		default:
			base.transform.SetPosition(new float?(0f), null, null);
			break;
		case TrainLevelSkeleton.Position.Left:
			base.transform.SetPosition(new float?(-470f), null, null);
			break;
		}
	}

	// Token: 0x06002966 RID: 10598 RVA: 0x00022C9C File Offset: 0x00020E9C
	public void In()
	{
		base.animator.Play("In");
	}

	// Token: 0x06002967 RID: 10599 RVA: 0x00022CAE File Offset: 0x00020EAE
	public void Out()
	{
		base.animator.SetTrigger("Out");
	}

	// Token: 0x06002968 RID: 10600 RVA: 0x00022CC0 File Offset: 0x00020EC0
	public void Die()
	{
		this.exploder.StartExplosion();
		base.animator.Play("Death");
	}

	// Token: 0x06002969 RID: 10601 RVA: 0x00022CDD File Offset: 0x00020EDD
	public void EndDeath()
	{
		this.exploder.StopExplosions();
	}

	// Token: 0x0600296A RID: 10602 RVA: 0x00022CEA File Offset: 0x00020EEA
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600296B RID: 10603 RVA: 0x00022D02 File Offset: 0x00020F02
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x040022AF RID: 8879
	public const float X = 470f;

	// Token: 0x040022B0 RID: 8880
	public LevelBossDeathExploder exploder;

	// Token: 0x040022B1 RID: 8881
	public DamageDealer damageDealer;
}
