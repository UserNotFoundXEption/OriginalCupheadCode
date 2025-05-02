using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000449 RID: 1097
public class MountainPlatformingLevelMudmanSpawner : AbstractPausableComponent
{
	// Token: 0x06002F0F RID: 12047 RVA: 0x00027348 File Offset: 0x00025548
	public void SpawnMudmen()
	{
		base.StartCoroutine(this.spawn_cr());
	}

	// Token: 0x06002F10 RID: 12048 RVA: 0x000E0BE8 File Offset: 0x000DEDE8
	public IEnumerator spawn_cr()
	{
		string[] mudmanSize = this.mudmanSizeString.Split(new char[]
		{
			','
		});
		string[] mudmanBig = this.mudmanBigSpawnString.Split(new char[]
		{
			','
		});
		string[] mudmanSmall = this.mudmanSmallSpawnString.Split(new char[]
		{
			','
		});
		int mudmanSizeIndex = Random.Range(0, mudmanSize.Length);
		int mudmanBigIndex = Random.Range(0, mudmanBig.Length);
		int mudmanSmallIndex = Random.Range(0, mudmanSmall.Length);
		PlatformingLevelGroundMovementEnemy.Direction dir = PlatformingLevelGroundMovementEnemy.Direction.Left;
		yield return CupheadTime.WaitForSeconds(this, this.initialDelayRange.RandomFloat());
		while (MountainPlatformingLevelElevatorHandler.elevatorIsMoving)
		{
			if (mudmanSize[mudmanSizeIndex][0] == 'B')
			{
				string[] array = mudmanBig[mudmanBigIndex].Split(new char[]
				{
					'-'
				});
				foreach (string s in array)
				{
					int num = 1;
					Parser.IntTryParse(s, out num);
					dir = ((num >= 3) ? PlatformingLevelGroundMovementEnemy.Direction.Left : PlatformingLevelGroundMovementEnemy.Direction.Right);
					MountainPlatformingLevelMudman mountainPlatformingLevelMudman = Object.Instantiate<MountainPlatformingLevelMudman>(this.bigMudman);
					mountainPlatformingLevelMudman.Init(this.spawnPoints[num - 1].position, dir);
				}
				mudmanBigIndex = (mudmanBigIndex + 1) % mudmanBig.Length;
			}
			else if (mudmanSize[mudmanSizeIndex][0] == 'S')
			{
				string[] array3 = mudmanSmall[mudmanSmallIndex].Split(new char[]
				{
					'-'
				});
				foreach (string s2 in array3)
				{
					int num2 = 1;
					Parser.IntTryParse(s2, out num2);
					dir = ((num2 >= 3) ? PlatformingLevelGroundMovementEnemy.Direction.Left : PlatformingLevelGroundMovementEnemy.Direction.Right);
					MountainPlatformingLevelMudman mountainPlatformingLevelMudman2 = Object.Instantiate<MountainPlatformingLevelMudman>(this.smallMudman);
					mountainPlatformingLevelMudman2.Init(this.spawnPoints[num2 - 1].position, dir);
				}
				mudmanSmallIndex = (mudmanSmallIndex + 1) % mudmanSmall.Length;
			}
			mudmanSizeIndex = (mudmanSizeIndex + 1) % mudmanSize.Length;
			yield return CupheadTime.WaitForSeconds(this, this.spawnDelayRange.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002F11 RID: 12049 RVA: 0x00027357 File Offset: 0x00025557
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002F12 RID: 12050 RVA: 0x0002736A File Offset: 0x0002556A
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002F13 RID: 12051 RVA: 0x000E0C04 File Offset: 0x000DEE04
	public void DrawGizmos(float a)
	{
		Gizmos.color = new Color(1f, 0f, 0f, a);
		foreach (Transform transform in this.spawnPoints)
		{
			Gizmos.DrawWireSphere(transform.position, 30f);
		}
	}

	// Token: 0x04002704 RID: 9988
	[SerializeField]
	public Transform[] spawnPoints;

	// Token: 0x04002705 RID: 9989
	[SerializeField]
	public MountainPlatformingLevelMudman bigMudman;

	// Token: 0x04002706 RID: 9990
	[SerializeField]
	public MountainPlatformingLevelMudman smallMudman;

	// Token: 0x04002707 RID: 9991
	[SerializeField]
	public MinMax spawnDelayRange;

	// Token: 0x04002708 RID: 9992
	[SerializeField]
	public MinMax initialDelayRange;

	// Token: 0x04002709 RID: 9993
	[SerializeField]
	public string mudmanSizeString;

	// Token: 0x0400270A RID: 9994
	[SerializeField]
	public string mudmanBigSpawnString;

	// Token: 0x0400270B RID: 9995
	[SerializeField]
	public string mudmanSmallSpawnString;
}
