using System;
using UnityEngine;

// Token: 0x020003F5 RID: 1013
public class TreePlatformingLevelDragonflyProjectileHandler : PlatformingLevelEnemySpawner
{
	// Token: 0x06002C76 RID: 11382 RVA: 0x000D9E70 File Offset: 0x000D8070
	public override void Start()
	{
		base.Start();
		this.dragonflyShots = new TreePlatformingLevelDragonflyShot[base.GetComponentsInChildren<TreePlatformingLevelDragonflyShot>().Length];
		this.dragonflyShots = base.GetComponentsInChildren<TreePlatformingLevelDragonflyShot>();
		this.spawnIndex = Random.Range(0, this.delaySpawnString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x06002C77 RID: 11383 RVA: 0x000D9EC8 File Offset: 0x000D80C8
	public override void Spawn()
	{
		this.spawnDelay.min = Parser.FloatParse(this.delaySpawnString.Split(new char[]
		{
			','
		})[this.spawnIndex]);
		this.spawnDelay.max = Parser.FloatParse(this.delaySpawnString.Split(new char[]
		{
			','
		})[this.spawnIndex]);
		this.Activate();
		base.Spawn();
		this.spawnIndex = (this.spawnIndex + 1) % this.delaySpawnString.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x06002C78 RID: 11384 RVA: 0x000D9F64 File Offset: 0x000D8164
	public void Activate()
	{
		int num = Random.Range(0, this.dragonflyShots.Length);
		if (!this.dragonflyShots[num].isActivated)
		{
			this.dragonflyShots[num].Activate();
		}
	}

	// Token: 0x040024AF RID: 9391
	[SerializeField]
	public string delaySpawnString;

	// Token: 0x040024B0 RID: 9392
	public TreePlatformingLevelDragonflyShot[] dragonflyShots;

	// Token: 0x040024B1 RID: 9393
	public int spawnIndex;
}
