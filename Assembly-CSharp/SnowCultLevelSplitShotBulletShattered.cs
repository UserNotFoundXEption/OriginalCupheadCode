using System;
using UnityEngine;

// Token: 0x0200039C RID: 924
public class SnowCultLevelSplitShotBulletShattered : BasicProjectile
{
	// Token: 0x060028BF RID: 10431 RVA: 0x000CFAA4 File Offset: 0x000CDCA4
	public override BasicProjectile Create(Vector2 position, float rotation, float speed)
	{
		SnowCultLevelSplitShotBulletShattered snowCultLevelSplitShotBulletShattered = base.Create(position, rotation, speed) as SnowCultLevelSplitShotBulletShattered;
		snowCultLevelSplitShotBulletShattered.animator.Play((!Rand.Bool()) ? "MoonB" : "MoonA");
		snowCultLevelSplitShotBulletShattered.fxTimer = 0f;
		return snowCultLevelSplitShotBulletShattered;
	}

	// Token: 0x060028C0 RID: 10432 RVA: 0x000CFAF0 File Offset: 0x000CDCF0
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		this.fxTimer += CupheadTime.FixedDelta;
		if (this.fxTimer > this.fxDelay)
		{
			this.fxTimer -= this.fxDelay;
			this.trailFX.Create(base.transform.position);
		}
	}

	// Token: 0x04002206 RID: 8710
	[SerializeField]
	public Effect trailFX;

	// Token: 0x04002207 RID: 8711
	[SerializeField]
	public float fxDelay = 0.3f;

	// Token: 0x04002208 RID: 8712
	public float fxTimer;
}
