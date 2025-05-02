using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003D7 RID: 983
public class PlatformingLevelFloatingSpawner : AbstractPausableComponent
{
	// Token: 0x06002B69 RID: 11113 RVA: 0x00024777 File Offset: 0x00022977
	public void Start()
	{
		base.Awake();
		base.StartCoroutine(this.spawn_cr());
	}

	// Token: 0x06002B6A RID: 11114 RVA: 0x000D5E6C File Offset: 0x000D406C
	public IEnumerator spawn_cr()
	{
		bool hashadSuccessfulSpawn = false;
		for (;;)
		{
			if (hashadSuccessfulSpawn)
			{
				yield return CupheadTime.WaitForSeconds(this, this.spawnTime.RandomFloat());
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, this.initialSpawnTime.RandomFloat());
			}
			Vector2 spawnPos = base.transform.position;
			spawnPos.x += Random.Range(-this.xRange, this.xRange);
			if (CupheadLevelCamera.Current.ContainsPoint(spawnPos, new Vector2(0f, 1000f)))
			{
				PlatformingLevelGroundMovementEnemy platformingLevelGroundMovementEnemy = this.enemyPrefab.Spawn(spawnPos, (!MathUtils.RandomBool()) ? PlatformingLevelGroundMovementEnemy.Direction.Right : PlatformingLevelGroundMovementEnemy.Direction.Left, true);
				platformingLevelGroundMovementEnemy.Float(true);
				hashadSuccessfulSpawn = true;
			}
		}
		yield break;
	}

	// Token: 0x06002B6B RID: 11115 RVA: 0x0002478C File Offset: 0x0002298C
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.enemyPrefab = null;
	}

	// Token: 0x06002B6C RID: 11116 RVA: 0x0002479B File Offset: 0x0002299B
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002B6D RID: 11117 RVA: 0x000247AE File Offset: 0x000229AE
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002B6E RID: 11118 RVA: 0x000D5E88 File Offset: 0x000D4088
	public void DrawGizmos(float a)
	{
		Gizmos.color = new Color(1f, 1f, 0f, a);
		Gizmos.DrawLine(base.baseTransform.position - new Vector3(this.xRange, 0f, 0f), base.baseTransform.position + new Vector3(this.xRange, 0f, 0f));
	}

	// Token: 0x040023FA RID: 9210
	[SerializeField]
	public PlatformingLevelGroundMovementEnemy enemyPrefab;

	// Token: 0x040023FB RID: 9211
	[SerializeField]
	public float xRange;

	// Token: 0x040023FC RID: 9212
	[SerializeField]
	public MinMax initialSpawnTime;

	// Token: 0x040023FD RID: 9213
	[SerializeField]
	public MinMax spawnTime;
}
