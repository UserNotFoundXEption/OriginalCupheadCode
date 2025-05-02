using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200044E RID: 1102
public class MountainPlatformingLevelSatyrSpawner : AbstractPausableComponent
{
	// Token: 0x06002F31 RID: 12081 RVA: 0x000E0F3C File Offset: 0x000DF13C
	public void Start()
	{
		base.StartCoroutine(this.spawn_cr());
		this.directionIndex = Random.Range(0, this.directionString.Split(new char[]
		{
			','
		}).Length);
		this.spawnIndex = Random.Range(0, this.spawnString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x06002F32 RID: 12082 RVA: 0x000E0FA0 File Offset: 0x000DF1A0
	public IEnumerator spawn_cr()
	{
		PlatformingLevelGroundMovementEnemy.Direction direction = PlatformingLevelGroundMovementEnemy.Direction.Right;
		bool isForeground = false;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.spawnDelayRange.RandomFloat());
			Vector2 spawnPos = base.transform.position;
			spawnPos.y = base.transform.position.y;
			spawnPos.x += Random.Range(-this.xRange, this.xRange);
			AbstractPlayerController player = PlayerManager.GetNext();
			if (CupheadLevelCamera.Current.ContainsPoint(spawnPos, new Vector2(0f, 1000f)))
			{
				if (this.spawnString.Split(new char[]
				{
					','
				})[this.spawnIndex][0] == 'F')
				{
					isForeground = true;
				}
				else if (this.spawnString.Split(new char[]
				{
					','
				})[this.spawnIndex][0] == 'B')
				{
					isForeground = false;
				}
				if (this.directionString.Split(new char[]
				{
					','
				})[this.directionIndex][0] == 'L')
				{
					direction = PlatformingLevelGroundMovementEnemy.Direction.Left;
				}
				else if (this.directionString.Split(new char[]
				{
					','
				})[this.directionIndex][0] == 'R')
				{
					direction = PlatformingLevelGroundMovementEnemy.Direction.Right;
				}
				else if (this.directionString.Split(new char[]
				{
					','
				})[this.directionIndex][0] == 'P')
				{
					if (player.transform.position.x < spawnPos.x)
					{
						direction = PlatformingLevelGroundMovementEnemy.Direction.Left;
					}
					else
					{
						direction = PlatformingLevelGroundMovementEnemy.Direction.Right;
					}
				}
				MountainPlatformingLevelSatyr mountainPlatformingLevelSatyr = this.satyrPrefab.Spawn(spawnPos, direction, true) as MountainPlatformingLevelSatyr;
				mountainPlatformingLevelSatyr.Init(direction, isForeground);
				this.directionIndex = (this.directionIndex + 1) % this.directionString.Split(new char[]
				{
					','
				}).Length;
				this.spawnIndex = (this.spawnIndex + 1) % this.spawnString.Split(new char[]
				{
					','
				}).Length;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002F33 RID: 12083 RVA: 0x000E0FBC File Offset: 0x000DF1BC
	public override void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f, 1f);
		Gizmos.DrawLine(base.baseTransform.position - new Vector3(this.xRange, 0f, 0f), base.baseTransform.position + new Vector3(this.xRange, 0f, 0f));
	}

	// Token: 0x04002721 RID: 10017
	[SerializeField]
	public string directionString;

	// Token: 0x04002722 RID: 10018
	[SerializeField]
	public string spawnString;

	// Token: 0x04002723 RID: 10019
	[SerializeField]
	public float xRange;

	// Token: 0x04002724 RID: 10020
	[SerializeField]
	public MountainPlatformingLevelSatyr satyrPrefab;

	// Token: 0x04002725 RID: 10021
	[SerializeField]
	public MinMax spawnDelayRange;

	// Token: 0x04002726 RID: 10022
	public int directionIndex;

	// Token: 0x04002727 RID: 10023
	public int spawnIndex;
}
