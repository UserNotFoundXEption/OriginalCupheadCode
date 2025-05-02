using System;
using UnityEngine;

// Token: 0x020001BC RID: 444
public class DevilLevelFireball : AbstractProjectile
{
	// Token: 0x0600150D RID: 5389 RVA: 0x0009B100 File Offset: 0x00099300
	public DevilLevelFireball Create(float xPos, float speed, float gravity, float xScale)
	{
		DevilLevelFireball devilLevelFireball = this.InstantiatePrefab<DevilLevelFireball>();
		devilLevelFireball.transform.position = new Vector2(xPos, 500f);
		devilLevelFireball.yVelocity = -speed;
		devilLevelFireball.gravity = gravity;
		devilLevelFireball.transform.SetScale(new float?(xScale), null, null);
		return devilLevelFireball;
	}

	// Token: 0x0600150E RID: 5390 RVA: 0x00011E85 File Offset: 0x00010085
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600150F RID: 5391 RVA: 0x0009B164 File Offset: 0x00099364
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		this.yVelocity -= this.gravity * CupheadTime.FixedDelta;
		base.transform.AddPosition(0f, this.yVelocity * CupheadTime.FixedDelta, 0f);
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x0009B1C0 File Offset: 0x000993C0
	public override void Die()
	{
		base.Die();
		this.poofEffect.Create(base.transform.position);
		foreach (SpriteDeathParts spriteDeathParts in this.parts)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001511 RID: 5393 RVA: 0x00011EA3 File Offset: 0x000100A3
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.poofEffect = null;
		this.parts = null;
	}

	// Token: 0x0400113C RID: 4412
	[SerializeField]
	public Effect poofEffect;

	// Token: 0x0400113D RID: 4413
	[SerializeField]
	public SpriteDeathParts[] parts;

	// Token: 0x0400113E RID: 4414
	public const float SPAWN_Y = 500f;

	// Token: 0x0400113F RID: 4415
	public float yVelocity;

	// Token: 0x04001140 RID: 4416
	public float gravity;
}
