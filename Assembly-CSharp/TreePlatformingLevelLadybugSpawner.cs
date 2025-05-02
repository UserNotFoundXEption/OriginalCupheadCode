using System;
using UnityEngine;

// Token: 0x020003F8 RID: 1016
public class TreePlatformingLevelLadybugSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06002C8A RID: 11402 RVA: 0x000DA27C File Offset: 0x000D847C
	public override void Start()
	{
		base.Start();
		this.typeIndex = Random.Range(0, this.typeString.Split(new char[]
		{
			','
		}).Length);
		this.sideIndex = Random.Range(0, this.sideString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x06002C8B RID: 11403 RVA: 0x000DA2D8 File Offset: 0x000D84D8
	public override void Spawn()
	{
		PlatformingLevelGroundMovementEnemy.Direction dir = PlatformingLevelGroundMovementEnemy.Direction.Right;
		TreePlatformingLevelLadyBug.Type type = TreePlatformingLevelLadyBug.Type.BounceFast;
		if (this.sideString.Split(new char[]
		{
			','
		})[this.sideIndex][0] == 'R')
		{
			this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMin - 50f;
			dir = PlatformingLevelGroundMovementEnemy.Direction.Right;
		}
		else if (this.sideString.Split(new char[]
		{
			','
		})[this.sideIndex][0] == 'L')
		{
			this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50f;
			dir = PlatformingLevelGroundMovementEnemy.Direction.Left;
		}
		string text = this.typeString.Split(new char[]
		{
			','
		})[this.typeIndex];
		if (text != null)
		{
			if (!(text == "BS"))
			{
				if (!(text == "BF"))
				{
					if (!(text == "GS"))
					{
						if (!(text == "GF"))
						{
							if (text == "P")
							{
								type = TreePlatformingLevelLadyBug.Type.BouncePink;
								this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50f;
								dir = PlatformingLevelGroundMovementEnemy.Direction.Left;
							}
						}
						else
						{
							type = TreePlatformingLevelLadyBug.Type.GroundFast;
						}
					}
					else
					{
						type = TreePlatformingLevelLadyBug.Type.GroundSlow;
					}
				}
				else
				{
					type = TreePlatformingLevelLadyBug.Type.BounceFast;
					this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50f;
					dir = PlatformingLevelGroundMovementEnemy.Direction.Left;
				}
			}
			else
			{
				type = TreePlatformingLevelLadyBug.Type.BounceSlow;
				this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50f;
				dir = PlatformingLevelGroundMovementEnemy.Direction.Left;
			}
		}
		this.spawnPosition.y = CupheadLevelCamera.Current.transform.position.y;
		this.ladybugPrefab.Spawn(this.spawnPosition, dir, true, type);
		this.typeIndex = (this.typeIndex + 1) % this.typeString.Split(new char[]
		{
			','
		}).Length;
		this.sideIndex = (this.sideIndex + 1) % this.sideString.Split(new char[]
		{
			','
		}).Length;
		base.Spawn();
	}

	// Token: 0x040024B4 RID: 9396
	[SerializeField]
	public TreePlatformingLevelLadyBug ladybugPrefab;

	// Token: 0x040024B5 RID: 9397
	[SerializeField]
	public string typeString;

	// Token: 0x040024B6 RID: 9398
	[SerializeField]
	public string sideString;

	// Token: 0x040024B7 RID: 9399
	public int typeIndex;

	// Token: 0x040024B8 RID: 9400
	public int sideIndex;

	// Token: 0x040024B9 RID: 9401
	public Vector3 spawnPosition;
}
