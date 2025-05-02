using System;
using UnityEngine;

// Token: 0x0200042B RID: 1067
public class HarbourPlatformingLevelFishSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06002E1E RID: 11806 RVA: 0x000DE8F4 File Offset: 0x000DCAF4
	public override void Start()
	{
		base.Start();
		this.delayIndex = Random.Range(0, this.spawnDelayString.Split(new char[]
		{
			','
		}).Length);
		this.posIndex = Random.Range(0, this.spawnPositionString.Split(new char[]
		{
			','
		}).Length);
		this.sideIndex = Random.Range(0, this.spawnSideString.Split(new char[]
		{
			','
		}).Length);
		this.typeIndex = Random.Range(0, this.typeString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x06002E1F RID: 11807 RVA: 0x000DE998 File Offset: 0x000DCB98
	public override void Spawn()
	{
		base.Spawn();
		this.spawnDelay.min = Parser.FloatParse(this.spawnDelayString.Split(new char[]
		{
			','
		})[this.delayIndex]);
		this.spawnDelay.max = Parser.FloatParse(this.spawnDelayString.Split(new char[]
		{
			','
		})[this.delayIndex]);
		if (this.spawnSideString.Split(new char[]
		{
			','
		})[this.sideIndex][0] == 'L')
		{
			this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMin - 50f;
			this.rotation = 0f;
		}
		else if (this.spawnSideString.Split(new char[]
		{
			','
		})[this.sideIndex][0] == 'R')
		{
			this.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50f;
			this.rotation = 180f;
		}
		this.spawnPosition.y = CupheadLevelCamera.Current.transform.position.y + Parser.FloatParse(this.spawnPositionString.Split(new char[]
		{
			','
		})[this.posIndex]);
		HarbourPlatformingLevelFish harbourPlatformingLevelFish = Object.Instantiate<HarbourPlatformingLevelFish>(this.fishPrefab);
		harbourPlatformingLevelFish.Init(this.spawnPosition, this.rotation, this.typeString.Split(new char[]
		{
			','
		})[this.typeIndex]);
		this.sideIndex = (this.sideIndex + 1) % this.spawnSideString.Split(new char[]
		{
			','
		}).Length;
		this.posIndex = (this.posIndex + 1) % this.spawnPositionString.Split(new char[]
		{
			','
		}).Length;
		this.delayIndex = (this.delayIndex + 1) % this.spawnDelayString.Split(new char[]
		{
			','
		}).Length;
		this.typeIndex = (this.typeIndex + 1) % this.typeString.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x0400262D RID: 9773
	[SerializeField]
	public HarbourPlatformingLevelFish fishPrefab;

	// Token: 0x0400262E RID: 9774
	[SerializeField]
	public string spawnDelayString;

	// Token: 0x0400262F RID: 9775
	[SerializeField]
	public string spawnPositionString;

	// Token: 0x04002630 RID: 9776
	[SerializeField]
	public string spawnSideString;

	// Token: 0x04002631 RID: 9777
	[SerializeField]
	public string typeString;

	// Token: 0x04002632 RID: 9778
	[SerializeField]
	public float movementSpeed;

	// Token: 0x04002633 RID: 9779
	[SerializeField]
	public float sineSpeed;

	// Token: 0x04002634 RID: 9780
	[SerializeField]
	public float sineSize;

	// Token: 0x04002635 RID: 9781
	public float rotation;

	// Token: 0x04002636 RID: 9782
	public int delayIndex;

	// Token: 0x04002637 RID: 9783
	public int posIndex;

	// Token: 0x04002638 RID: 9784
	public int sideIndex;

	// Token: 0x04002639 RID: 9785
	public int typeIndex;

	// Token: 0x0400263A RID: 9786
	public Vector3 spawnPosition;
}
