using System;
using UnityEngine;

// Token: 0x020003E4 RID: 996
public class ForestPlatformingLevelAcornSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06002BFB RID: 11259 RVA: 0x000D8E40 File Offset: 0x000D7040
	public override void Awake()
	{
		base.Awake();
		this.leftRightIndex = Random.Range(0, this.leftRightString.Length);
		this.yPattern = this.yString.Split(new char[]
		{
			','
		});
		this.yIndex = Random.Range(0, this.yPattern.Length);
	}

	// Token: 0x06002BFC RID: 11260 RVA: 0x000D8E9C File Offset: 0x000D709C
	public override void Spawn()
	{
		this.leftRightIndex = (this.leftRightIndex + 1) % this.leftRightString.Length;
		ForestPlatformingLevelAcorn.Direction direction = (this.leftRightString[this.leftRightIndex] != 'L') ? ForestPlatformingLevelAcorn.Direction.Right : ForestPlatformingLevelAcorn.Direction.Left;
		this.yIndex = (this.yIndex + 1) % this.yPattern.Length;
		float num = 0f;
		Parser.FloatTryParse(this.yPattern[this.yIndex], out num);
		Vector2 position;
		position..ctor((direction != ForestPlatformingLevelAcorn.Direction.Left) ? (CupheadLevelCamera.Current.Bounds.xMin - 50f) : (CupheadLevelCamera.Current.Bounds.xMax + 50f), CupheadLevelCamera.Current.Bounds.yMax - num);
		this.enemyPrefab.Spawn(position, direction, false);
	}

	// Token: 0x06002BFD RID: 11261 RVA: 0x00024D78 File Offset: 0x00022F78
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.enemyPrefab = null;
	}

	// Token: 0x04002468 RID: 9320
	public ForestPlatformingLevelAcorn enemyPrefab;

	// Token: 0x04002469 RID: 9321
	public string leftRightString = "LR";

	// Token: 0x0400246A RID: 9322
	public string yString = "150,50";

	// Token: 0x0400246B RID: 9323
	public int leftRightIndex;

	// Token: 0x0400246C RID: 9324
	public int yIndex;

	// Token: 0x0400246D RID: 9325
	public string[] yPattern;
}
