using System;
using UnityEngine;

// Token: 0x020003DA RID: 986
public class PlatformingLevelGroundMovementEnemySpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x06002B9C RID: 11164 RVA: 0x000249A8 File Offset: 0x00022BA8
	public override void Awake()
	{
		base.Awake();
		if (!this.chooseSideRandomly)
		{
			this.patternIndex = Random.Range(0, this.patternString.Length);
		}
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x000D7168 File Offset: 0x000D5368
	public override void Spawn()
	{
		PlatformingLevelGroundMovementEnemy.Direction direction;
		if (this.chooseSideRandomly)
		{
			direction = ((!MathUtils.RandomBool()) ? PlatformingLevelGroundMovementEnemy.Direction.Right : PlatformingLevelGroundMovementEnemy.Direction.Left);
		}
		else
		{
			this.patternIndex = (this.patternIndex + 1) % this.patternString.Length;
			direction = ((this.patternString[this.patternIndex] != 'L') ? PlatformingLevelGroundMovementEnemy.Direction.Right : PlatformingLevelGroundMovementEnemy.Direction.Left);
		}
		Vector2 vector;
		vector..ctor((direction != PlatformingLevelGroundMovementEnemy.Direction.Left) ? (CupheadLevelCamera.Current.Bounds.xMin - 50f) : (CupheadLevelCamera.Current.Bounds.xMax + 50f), CupheadLevelCamera.Current.Bounds.yMax);
		PlatformingLevelGroundMovementEnemy platformingLevelGroundMovementEnemy = this.enemyPrefab.Spawn(vector, direction, this.destroyEnemyAfterLeavingScreen);
		platformingLevelGroundMovementEnemy.GoToGround(true, "Run");
	}

	// Token: 0x04002427 RID: 9255
	public PlatformingLevelGroundMovementEnemy enemyPrefab;

	// Token: 0x04002428 RID: 9256
	public bool chooseSideRandomly = true;

	// Token: 0x04002429 RID: 9257
	public string patternString = "LR";

	// Token: 0x0400242A RID: 9258
	public int patternIndex;
}
