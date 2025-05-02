using System;
using UnityEngine;

// Token: 0x020003FB RID: 1019
public class TreePlatformingLevelLogProjectile : BasicProjectile
{
	// Token: 0x06002CA5 RID: 11429 RVA: 0x000DABBC File Offset: 0x000D8DBC
	public AbstractProjectile Create(Vector2 position, float rotation, float speed, bool isLeft, bool parry)
	{
		TreePlatformingLevelLogProjectile treePlatformingLevelLogProjectile = base.Create(position, rotation, speed) as TreePlatformingLevelLogProjectile;
		treePlatformingLevelLogProjectile.animator.SetFloat("Direction", (float)((!isLeft) ? -1 : 1));
		treePlatformingLevelLogProjectile.animator.SetTrigger("Start");
		treePlatformingLevelLogProjectile.SetParryable(parry);
		return treePlatformingLevelLogProjectile;
	}

	// Token: 0x06002CA6 RID: 11430 RVA: 0x0002548C File Offset: 0x0002368C
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.animator.SetBool("Parry", parryable);
	}

	// Token: 0x06002CA7 RID: 11431 RVA: 0x000254A6 File Offset: 0x000236A6
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}
}
