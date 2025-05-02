using System;
using UnityEngine;

// Token: 0x0200042F RID: 1071
public class HarbourPlatformingLevelKrillSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06002E32 RID: 11826 RVA: 0x000DEE14 File Offset: 0x000DD014
	public override void Start()
	{
		base.Start();
		this.posIndex = Random.Range(0, this.posString.Split(new char[]
		{
			','
		}).Length);
		this.typeIndex = Random.Range(0, this.typeString.Split(new char[]
		{
			','
		}).Length);
		this.delayIndex = Random.Range(0, this.delayString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x06002E33 RID: 11827 RVA: 0x000DEE94 File Offset: 0x000DD094
	public override void Spawn()
	{
		base.Spawn();
		this.spawnDelay.min = Parser.FloatParse(this.delayString.Split(new char[]
		{
			','
		})[this.delayIndex]);
		this.spawnDelay.max = Parser.FloatParse(this.delayString.Split(new char[]
		{
			','
		})[this.delayIndex]);
		Vector2 vector = CupheadLevelCamera.Current.transform.position;
		vector.x = CupheadLevelCamera.Current.transform.position.x + (float)Parser.IntParse(this.posString.Split(new char[]
		{
			','
		})[this.posIndex]);
		vector.y = CupheadLevelCamera.Current.Bounds.yMin - 50f;
		this.parryable = (this.typeString.Split(new char[]
		{
			','
		})[this.typeIndex][0] == 'A');
		HarbourPlatformingLevelKrill harbourPlatformingLevelKrill = this.krillPrefab.Spawn(null, vector);
		harbourPlatformingLevelKrill.isParryable = this.parryable;
		harbourPlatformingLevelKrill.SetType(this.typeString.Split(new char[]
		{
			','
		})[this.typeIndex]);
		this.posIndex = (this.posIndex + 1) % this.posString.Split(new char[]
		{
			','
		}).Length;
		this.typeIndex = (this.typeIndex + 1) % this.typeString.Split(new char[]
		{
			','
		}).Length;
		this.delayIndex = (this.delayIndex + 1) % this.delayString.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x04002645 RID: 9797
	[SerializeField]
	public HarbourPlatformingLevelKrill krillPrefab;

	// Token: 0x04002646 RID: 9798
	[SerializeField]
	public string posString = "305,640,356";

	// Token: 0x04002647 RID: 9799
	[SerializeField]
	public string typeString;

	// Token: 0x04002648 RID: 9800
	[SerializeField]
	public string delayString;

	// Token: 0x04002649 RID: 9801
	public int posIndex;

	// Token: 0x0400264A RID: 9802
	public int typeIndex;

	// Token: 0x0400264B RID: 9803
	public int delayIndex;

	// Token: 0x0400264C RID: 9804
	public bool parryable;
}
