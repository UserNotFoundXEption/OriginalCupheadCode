using System;
using UnityEngine;

// Token: 0x02000400 RID: 1024
public class CircusPlatformingLevelBalloonSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06002CDD RID: 11485 RVA: 0x000DB2DC File Offset: 0x000D94DC
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
		this.pinkSplits = this.pinkString.Split(new char[]
		{
			','
		});
		this.pinkIndex = Random.Range(0, this.pinkSplits.Length);
	}

	// Token: 0x06002CDE RID: 11486 RVA: 0x000DB38C File Offset: 0x000D958C
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
		CircusPlatformingLevelBalloon circusPlatformingLevelBalloon = Object.Instantiate<CircusPlatformingLevelBalloon>(this.balloonPrefab);
		circusPlatformingLevelBalloon.Init(this.spawnPosition, this.rotation, this.spreadCount, this.pinkSplits[this.pinkIndex]);
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
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkSplits.Length;
	}

	// Token: 0x04002510 RID: 9488
	[SerializeField]
	public CircusPlatformingLevelBalloon balloonPrefab;

	// Token: 0x04002511 RID: 9489
	[SerializeField]
	public string spawnDelayString;

	// Token: 0x04002512 RID: 9490
	[SerializeField]
	public string spawnPositionString;

	// Token: 0x04002513 RID: 9491
	[SerializeField]
	public string spawnSideString;

	// Token: 0x04002514 RID: 9492
	[SerializeField]
	public string spreadCount;

	// Token: 0x04002515 RID: 9493
	[SerializeField]
	public string pinkString;

	// Token: 0x04002516 RID: 9494
	public float rotation;

	// Token: 0x04002517 RID: 9495
	public int delayIndex;

	// Token: 0x04002518 RID: 9496
	public int posIndex;

	// Token: 0x04002519 RID: 9497
	public int sideIndex;

	// Token: 0x0400251A RID: 9498
	public Vector3 spawnPosition;

	// Token: 0x0400251B RID: 9499
	public string[] pinkSplits;

	// Token: 0x0400251C RID: 9500
	public int pinkIndex;
}
