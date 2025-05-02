using System;
using UnityEngine;

// Token: 0x020003DD RID: 989
public class PlatformingLevelPathMovementEnemySpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06002BBF RID: 11199 RVA: 0x00024B02 File Offset: 0x00022D02
	public override void Spawn()
	{
		this.enemyPrefab.Spawn(base.transform.position, this.path, this.startPosition, this.destroyEnemyAfterLeavingScreen);
	}

	// Token: 0x06002BC0 RID: 11200 RVA: 0x00024B2D File Offset: 0x00022D2D
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002BC1 RID: 11201 RVA: 0x00024B40 File Offset: 0x00022D40
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002BC2 RID: 11202 RVA: 0x000D78D8 File Offset: 0x000D5AD8
	public new void DrawGizmos(float a)
	{
		this.path.DrawGizmos(a, base.baseTransform.position);
		Gizmos.color = new Color(1f, 0f, 0f, a);
		Gizmos.DrawSphere(this.path.Lerp(this.startPosition) + base.baseTransform.position, 10f);
		Gizmos.DrawWireSphere(this.path.Lerp(this.startPosition) + base.baseTransform.position, 11f);
	}

	// Token: 0x0400243C RID: 9276
	public PlatformingLevelPathMovementEnemy enemyPrefab;

	// Token: 0x0400243D RID: 9277
	[Header("Path")]
	public float startPosition = 0.5f;

	// Token: 0x0400243E RID: 9278
	public PlatformingLevelPathMovementEnemy.Direction direction = PlatformingLevelPathMovementEnemy.Direction.Forward;

	// Token: 0x0400243F RID: 9279
	public VectorPath path;
}
