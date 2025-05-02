using System;
using UnityEngine;

// Token: 0x02000420 RID: 1056
public class FunhousePlatformingLevelRocketSpawner : PlatformingLevelGroundMovementEnemySpawner
{
	// Token: 0x06002DC6 RID: 11718 RVA: 0x000DD8A0 File Offset: 0x000DBAA0
	public override void Start()
	{
		base.Start();
		this.topBottomIndex = Random.Range(0, this.topBottomString.Split(new char[]
		{
			','
		}).Length);
		this.directionPattern = this.patternString.Split(new char[]
		{
			','
		});
		this.directionIndex = Random.Range(0, this.directionPattern.Length);
	}

	// Token: 0x06002DC7 RID: 11719 RVA: 0x000DD908 File Offset: 0x000DBB08
	public override void Spawn()
	{
		if (this.topBottomString.Split(new char[]
		{
			','
		})[this.topBottomIndex][0] == 'T')
		{
			this.isTop = true;
		}
		else if (this.topBottomString.Split(new char[]
		{
			','
		})[this.topBottomIndex][0] == 'B')
		{
			this.isTop = false;
		}
		bool flag = (!this.chooseSideRandomly) ? (this.directionPattern[this.directionIndex] == "R") : Rand.Bool();
		this.directionIndex = (this.directionIndex + 1) % this.directionPattern.Length;
		float num = (!flag) ? (CupheadLevelCamera.Current.Bounds.xMin - 50f) : (CupheadLevelCamera.Current.Bounds.xMax + 50f);
		float y = CupheadLevelCamera.Current.transform.position.y;
		PlatformingLevelGroundMovementEnemy platformingLevelGroundMovementEnemy = this.enemyPrefab.Spawn<PlatformingLevelGroundMovementEnemy>();
		platformingLevelGroundMovementEnemy.GetComponent<FunhousePlatformingLevelRocket>().Init(new Vector2(num, y), this.isTop, flag);
		platformingLevelGroundMovementEnemy.GoToGround(true, "Idle");
		this.topBottomIndex = (this.topBottomIndex + 1) % this.topBottomString.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x040025EC RID: 9708
	public const string Right = "R";

	// Token: 0x040025ED RID: 9709
	[SerializeField]
	public string topBottomString;

	// Token: 0x040025EE RID: 9710
	public int topBottomIndex;

	// Token: 0x040025EF RID: 9711
	public bool isTop;

	// Token: 0x040025F0 RID: 9712
	public string[] directionPattern;

	// Token: 0x040025F1 RID: 9713
	public int directionIndex;
}
