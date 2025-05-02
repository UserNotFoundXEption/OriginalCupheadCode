using System;
using UnityEngine;

// Token: 0x020003DB RID: 987
public class PlatformingLevelJumpTrigger : AbstractCollidableObject
{
	// Token: 0x06002B9F RID: 11167 RVA: 0x000D7250 File Offset: 0x000D5450
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			PlatformingLevelGroundMovementEnemy component = hit.GetComponent<PlatformingLevelGroundMovementEnemy>();
			if (component != null && component.direction == this.direction)
			{
				component.Jump();
			}
		}
	}

	// Token: 0x06002BA0 RID: 11168 RVA: 0x000249DA File Offset: 0x00022BDA
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x000D7298 File Offset: 0x000D5498
	public void DrawGizmos(float a)
	{
		BoxCollider2D component = base.GetComponent<BoxCollider2D>();
		Gizmos.color = new Color(1f, 1f, 0f, a);
		Gizmos.DrawWireCube(component.bounds.center, component.bounds.size);
	}

	// Token: 0x0400242B RID: 9259
	[SerializeField]
	public PlatformingLevelGroundMovementEnemy.Direction direction;
}
