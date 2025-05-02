using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200041C RID: 1052
public class FunhousePlatformingLevelJackSpawner : AbstractPausableComponent
{
	// Token: 0x06002DAD RID: 11693 RVA: 0x00026181 File Offset: 0x00024381
	public void Start()
	{
		base.StartCoroutine(this.spawn_cr());
	}

	// Token: 0x06002DAE RID: 11694 RVA: 0x000DD5F8 File Offset: 0x000DB7F8
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
			spawnPos.y = base.transform.position.y;
			spawnPos.x += Random.Range(-this.xRange, this.xRange);
			if (CupheadLevelCamera.Current.ContainsPoint(spawnPos, new Vector2(0f, 1000f)))
			{
				this.jackPrefab.Spawn(spawnPos).SelectDirection(this.isBottom);
				hashadSuccessfulSpawn = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002DAF RID: 11695 RVA: 0x000DD614 File Offset: 0x000DB814
	public override void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f, 1f);
		Gizmos.DrawLine(base.baseTransform.position - new Vector3(this.xRange, 0f, 0f), base.baseTransform.position + new Vector3(this.xRange, 0f, 0f));
	}

	// Token: 0x040025DC RID: 9692
	[SerializeField]
	public MinMax initialSpawnTime;

	// Token: 0x040025DD RID: 9693
	[SerializeField]
	public MinMax spawnTime;

	// Token: 0x040025DE RID: 9694
	[SerializeField]
	public float xRange;

	// Token: 0x040025DF RID: 9695
	[SerializeField]
	public FunhousePlatformingLevelJack jackPrefab;

	// Token: 0x040025E0 RID: 9696
	[SerializeField]
	public bool isBottom;
}
