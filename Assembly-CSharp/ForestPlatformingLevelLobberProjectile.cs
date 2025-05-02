using System;
using UnityEngine;

// Token: 0x020003EA RID: 1002
public class ForestPlatformingLevelLobberProjectile : BasicProjectile
{
	// Token: 0x1700034E RID: 846
	// (get) Token: 0x06002C1E RID: 11294 RVA: 0x00024F63 File Offset: 0x00023163
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002C1F RID: 11295 RVA: 0x000D928C File Offset: 0x000D748C
	public override void Awake()
	{
		base.Awake();
		base.transform.SetScale(new float?((float)((!MathUtils.RandomBool()) ? -1 : 1)), null, null);
		base.animator.Play("A", 0, Random.Range(0f, 1f));
	}

	// Token: 0x06002C20 RID: 11296 RVA: 0x000D92F4 File Offset: 0x000D74F4
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		LevelPlatform component = hit.GetComponent<LevelPlatform>();
		if (component == null || (!component.canFallThrough && Mathf.Abs(this._accumulativeGravity) > base.transform.right.y * this.Speed))
		{
			this.explode();
		}
	}

	// Token: 0x06002C21 RID: 11297 RVA: 0x000D9358 File Offset: 0x000D7558
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		LevelPlatform component = hit.GetComponent<LevelPlatform>();
		if (component != null && !component.canFallThrough && Mathf.Abs(this._accumulativeGravity) > base.transform.right.y * this.Speed)
		{
			this.explode();
		}
	}

	// Token: 0x06002C22 RID: 11298 RVA: 0x00024F66 File Offset: 0x00023166
	public void explode()
	{
		if (!base.dead)
		{
			this.explosionPrefab.Create(base.transform.position);
			this.Die();
		}
	}

	// Token: 0x06002C23 RID: 11299 RVA: 0x00024F90 File Offset: 0x00023190
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002C24 RID: 11300 RVA: 0x00024FA3 File Offset: 0x000231A3
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.explosionPrefab = null;
	}

	// Token: 0x0400247E RID: 9342
	[SerializeField]
	public ForestPlatformingLevelLobberProjectileExplosion explosionPrefab;
}
