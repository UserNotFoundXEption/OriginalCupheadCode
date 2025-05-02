using System;
using UnityEngine;

// Token: 0x0200039E RID: 926
public class SnowCultLevelWhaleCollision : AbstractCollidableObject
{
	// Token: 0x060028C9 RID: 10441 RVA: 0x0002248A File Offset: 0x0002068A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.wiz.PlayerHitByWhale(hit, phase);
		}
	}

	// Token: 0x04002213 RID: 8723
	[SerializeField]
	public SnowCultLevelWizard wiz;
}
