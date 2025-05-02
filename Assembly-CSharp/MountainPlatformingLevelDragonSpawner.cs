using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000440 RID: 1088
public class MountainPlatformingLevelDragonSpawner : AbstractPausableComponent
{
	// Token: 0x06002EBD RID: 11965 RVA: 0x00026F8E File Offset: 0x0002518E
	public void Start()
	{
		base.StartCoroutine(this.spawn_cr());
		this.spawnIndex = Random.Range(0, this.spawnString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x06002EBE RID: 11966 RVA: 0x000E01F0 File Offset: 0x000DE3F0
	public IEnumerator spawn_cr()
	{
		for (;;)
		{
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(0f, 1000f)))
			{
				if ((this.isElevator && MountainPlatformingLevelElevatorHandler.elevatorIsMoving) || !this.isElevator)
				{
					MountainPlatformingLevelDragon dragonPrefab = null;
					int scale = 1;
					int spawnPoint = 1;
					Parser.IntTryParse(this.spawnString.Split(new char[]
					{
						','
					})[this.spawnIndex], out spawnPoint);
					Vector3 startPos = new Vector3(this.spawnPoints[spawnPoint - 1].position.x, this.spawnPoints[spawnPoint - 1].position.y + 500f);
					if (spawnPoint != 1)
					{
						if (spawnPoint != 2)
						{
							if (spawnPoint == 3)
							{
								dragonPrefab = this.dragonSidePrefab;
								scale = -1;
							}
						}
						else
						{
							dragonPrefab = this.dragonMiddlePrefab;
						}
					}
					else
					{
						dragonPrefab = this.dragonSidePrefab;
					}
					MountainPlatformingLevelDragon dragon = Object.Instantiate<MountainPlatformingLevelDragon>(dragonPrefab);
					dragon.Init(startPos, this.spawnPoints[spawnPoint - 1].position);
					dragon.transform.SetScale(new float?((float)scale), null, null);
					this.spawnIndex = (this.spawnIndex + 1) % this.spawnString.Split(new char[]
					{
						','
					}).Length;
					yield return CupheadTime.WaitForSeconds(this, this.spawnDelay);
				}
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002EBF RID: 11967 RVA: 0x00026FC1 File Offset: 0x000251C1
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002EC0 RID: 11968 RVA: 0x00026FD4 File Offset: 0x000251D4
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002EC1 RID: 11969 RVA: 0x000E020C File Offset: 0x000DE40C
	public void DrawGizmos(float a)
	{
		Gizmos.color = new Color(1f, 0f, 1f, a);
		foreach (Transform transform in this.spawnPoints)
		{
			Gizmos.DrawWireSphere(transform.position, 30f);
		}
	}

	// Token: 0x040026C3 RID: 9923
	[SerializeField]
	public bool isElevator;

	// Token: 0x040026C4 RID: 9924
	[SerializeField]
	public Transform[] spawnPoints;

	// Token: 0x040026C5 RID: 9925
	[SerializeField]
	public MountainPlatformingLevelDragon dragonMiddlePrefab;

	// Token: 0x040026C6 RID: 9926
	[SerializeField]
	public MountainPlatformingLevelDragon dragonSidePrefab;

	// Token: 0x040026C7 RID: 9927
	[SerializeField]
	public string spawnString;

	// Token: 0x040026C8 RID: 9928
	[SerializeField]
	public float spawnDelay;

	// Token: 0x040026C9 RID: 9929
	public int spawnIndex;
}
