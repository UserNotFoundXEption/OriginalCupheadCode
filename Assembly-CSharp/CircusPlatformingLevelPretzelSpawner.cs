using System;
using UnityEngine;

// Token: 0x02000410 RID: 1040
public class CircusPlatformingLevelPretzelSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06002D4E RID: 11598 RVA: 0x000DC61C File Offset: 0x000DA81C
	public override void Start()
	{
		base.Start();
		this.sideIndex = Random.Range(0, this.sideString.Split(new char[]
		{
			','
		}).Length);
		if (this.path == null || this.path.Length == 0)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002D4F RID: 11599 RVA: 0x000DC678 File Offset: 0x000DA878
	public override void Spawn()
	{
		base.Spawn();
		bool goingLeft = false;
		int startPosition = -1;
		if (this.sideString.Split(new char[]
		{
			','
		})[this.sideIndex][0] == 'L')
		{
			goingLeft = true;
			startPosition = this.path.Length - 1;
			for (int i = 0; i < this.path.Length; i++)
			{
				if (this.path[i].position.x > CupheadLevelCamera.Current.Bounds.xMax + 100f)
				{
					startPosition = i;
					break;
				}
			}
		}
		else if (this.sideString.Split(new char[]
		{
			','
		})[this.sideIndex][0] == 'R')
		{
			startPosition = 0;
			for (int j = this.path.Length - 1; j >= 0; j--)
			{
				if (this.path[j].position.x < CupheadLevelCamera.Current.Bounds.xMin - 100f)
				{
					startPosition = j;
					break;
				}
			}
			goingLeft = false;
		}
		this.spawnPosition.y = CupheadLevelCamera.Current.transform.position.y;
		CircusPlatformingLevelPretzel circusPlatformingLevelPretzel = this.pretzelPrefab.Spawn<CircusPlatformingLevelPretzel>();
		circusPlatformingLevelPretzel.SetPath(this.path);
		circusPlatformingLevelPretzel.goingLeft = goingLeft;
		circusPlatformingLevelPretzel.SetStartPosition(startPosition);
		this.sideIndex = (this.sideIndex + 1) % this.sideString.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x0400258A RID: 9610
	[SerializeField]
	public CircusPlatformingLevelPretzel pretzelPrefab;

	// Token: 0x0400258B RID: 9611
	[SerializeField]
	public string sideString;

	// Token: 0x0400258C RID: 9612
	[SerializeField]
	public Transform[] path;

	// Token: 0x0400258D RID: 9613
	public int sideIndex;

	// Token: 0x0400258E RID: 9614
	public Vector3 spawnPosition;
}
