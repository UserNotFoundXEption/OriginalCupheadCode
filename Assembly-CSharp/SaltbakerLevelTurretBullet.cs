using System;
using UnityEngine;

// Token: 0x02000383 RID: 899
public class SaltbakerLevelTurretBullet : BasicProjectile
{
	// Token: 0x060027B7 RID: 10167 RVA: 0x000CC8C4 File Offset: 0x000CAAC4
	public SaltbakerLevelTurretBullet Create(Vector3 pos, float rotation, float speed, SaltbakerLevelSaltbaker parent)
	{
		SaltbakerLevelTurretBullet saltbakerLevelTurretBullet = base.Create(pos, rotation, speed) as SaltbakerLevelTurretBullet;
		saltbakerLevelTurretBullet.parent = parent;
		saltbakerLevelTurretBullet.animator.Play((!Rand.Bool()) ? "B" : "A");
		saltbakerLevelTurretBullet.animator.Update(0f);
		return saltbakerLevelTurretBullet;
	}

	// Token: 0x060027B8 RID: 10168 RVA: 0x0002152D File Offset: 0x0001F72D
	public override void Start()
	{
		base.Start();
		this.parent.OnDeathEvent += this.Die;
	}

	// Token: 0x060027B9 RID: 10169 RVA: 0x0002154D File Offset: 0x0001F74D
	public override void OnDestroy()
	{
		this.parent.OnDeathEvent -= this.Die;
		base.OnDestroy();
	}

	// Token: 0x040020EB RID: 8427
	public SaltbakerLevelSaltbaker parent;
}
