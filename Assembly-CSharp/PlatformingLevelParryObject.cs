using System;
using UnityEngine;

// Token: 0x02000461 RID: 1121
public class PlatformingLevelParryObject : ParrySwitch
{
	// Token: 0x06002FCC RID: 12236 RVA: 0x00027CC6 File Offset: 0x00025EC6
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002FCD RID: 12237 RVA: 0x00027CD9 File Offset: 0x00025ED9
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002FCE RID: 12238 RVA: 0x00027CF1 File Offset: 0x00025EF1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.hurtsPlayer && this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x04002796 RID: 10134
	public bool hurtsPlayer;

	// Token: 0x04002797 RID: 10135
	public DamageDealer damageDealer;
}
