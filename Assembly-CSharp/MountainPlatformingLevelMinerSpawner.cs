using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000447 RID: 1095
public class MountainPlatformingLevelMinerSpawner : AbstractPausableComponent
{
	// Token: 0x06002EF7 RID: 12023 RVA: 0x000271E1 File Offset: 0x000253E1
	public void Start()
	{
		base.StartCoroutine(this.spawn_cr());
	}

	// Token: 0x06002EF8 RID: 12024 RVA: 0x000E0A5C File Offset: 0x000DEC5C
	public IEnumerator spawn_cr()
	{
		for (;;)
		{
			Vector2 spawnPos = base.transform.position;
			spawnPos.x += Random.Range(-this.xRange, this.xRange);
			if (this.isRespawning)
			{
				yield return CupheadTime.WaitForSeconds(this, this.deathDelayTime.RandomFloat());
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, this.spawnTime.RandomFloat());
			}
			this.enemySpawned = this.enemyPrefab.Spawn(spawnPos, (!MathUtils.RandomBool()) ? PlatformingLevelGroundMovementEnemy.Direction.Right : PlatformingLevelGroundMovementEnemy.Direction.Left, false);
			this.enemySpawned.Float(false);
			while (this.enemySpawned != null)
			{
				yield return null;
			}
			this.isRespawning = true;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002EF9 RID: 12025 RVA: 0x000271F0 File Offset: 0x000253F0
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002EFA RID: 12026 RVA: 0x00027203 File Offset: 0x00025403
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002EFB RID: 12027 RVA: 0x000E0A78 File Offset: 0x000DEC78
	public void DrawGizmos(float a)
	{
		Gizmos.color = new Color(1f, 1f, 0f, a);
		Gizmos.DrawLine(base.baseTransform.position - new Vector3(this.xRange, 0f, 0f), base.baseTransform.position + new Vector3(this.xRange, 0f, 0f));
	}

	// Token: 0x040026F9 RID: 9977
	[SerializeField]
	public PlatformingLevelGroundMovementEnemy enemyPrefab;

	// Token: 0x040026FA RID: 9978
	public PlatformingLevelGroundMovementEnemy enemySpawned;

	// Token: 0x040026FB RID: 9979
	[SerializeField]
	public float xRange;

	// Token: 0x040026FC RID: 9980
	[SerializeField]
	public MinMax deathDelayTime;

	// Token: 0x040026FD RID: 9981
	[SerializeField]
	public MinMax spawnTime;

	// Token: 0x040026FE RID: 9982
	public bool isRespawning;
}
