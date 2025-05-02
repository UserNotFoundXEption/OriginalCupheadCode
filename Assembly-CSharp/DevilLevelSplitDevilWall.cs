using System;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class DevilLevelSplitDevilWall : AbstractProjectile
{
	// Token: 0x06001562 RID: 5474 RVA: 0x0009C088 File Offset: 0x0009A288
	public DevilLevelSplitDevilWall Create(float xPos, float xVelocity, float distance, DevilLevelSplitDevil devil)
	{
		DevilLevelSplitDevilWall devilLevelSplitDevilWall = base.Create(new Vector2(xPos, 30f)) as DevilLevelSplitDevilWall;
		devilLevelSplitDevilWall.xVelocity = xVelocity;
		devilLevelSplitDevilWall.DestroyDistance = distance;
		devilLevelSplitDevilWall.devil = devil;
		devilLevelSplitDevilWall.UpdateColor();
		CupheadLevelCamera.Current.StartShake(4f);
		return devilLevelSplitDevilWall;
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x0009C0D8 File Offset: 0x0009A2D8
	public override void Update()
	{
		base.Update();
		if (base.dead)
		{
			return;
		}
		if (this.devil == null)
		{
			this.Die();
			return;
		}
		base.transform.AddPosition(this.xVelocity * CupheadTime.Delta, 0f, 0f);
		base.transform.SetScale(new float?(Random.Range(0.9f, 1f)), null, null);
		this.UpdateColor();
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x000122AF File Offset: 0x000104AF
	public void UpdateColor()
	{
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x000122B1 File Offset: 0x000104B1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x000122CF File Offset: 0x000104CF
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x000122E2 File Offset: 0x000104E2
	public override void OnDestroy()
	{
		if (Object.FindObjectsOfType<DevilLevelSplitDevilWall>().Length <= 1)
		{
			CupheadLevelCamera.Current.EndShake(0.5f);
		}
		base.OnDestroy();
	}

	// Token: 0x0400117A RID: 4474
	public float xVelocity;

	// Token: 0x0400117B RID: 4475
	public DevilLevelSplitDevil devil;

	// Token: 0x0400117C RID: 4476
	public const float Y_POS = 30f;
}
