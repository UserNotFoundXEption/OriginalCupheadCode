using System;
using UnityEngine;

// Token: 0x02000311 RID: 785
public class RetroArcadeBonusRobot : RetroArcadeEnemy
{
	// Token: 0x060022A9 RID: 8873 RVA: 0x000BE338 File Offset: 0x000BC538
	public RetroArcadeBonusRobot Create(RetroArcadeBonusRobot.Direction direction, LevelProperties.RetroArcade.Robots properties)
	{
		RetroArcadeBonusRobot retroArcadeBonusRobot = this.InstantiatePrefab<RetroArcadeBonusRobot>();
		retroArcadeBonusRobot.transform.position = new Vector2((direction != RetroArcadeBonusRobot.Direction.Left) ? -400f : 400f, 250f);
		retroArcadeBonusRobot.properties = properties;
		retroArcadeBonusRobot.direction = direction;
		retroArcadeBonusRobot.hp = properties.bonusHp;
		return retroArcadeBonusRobot;
	}

	// Token: 0x060022AA RID: 8874 RVA: 0x0001D7B6 File Offset: 0x0001B9B6
	public override void Start()
	{
		base.PointsWorth = this.properties.pointsGained;
		base.PointsBonus = this.properties.pointsBonus;
	}

	// Token: 0x060022AB RID: 8875 RVA: 0x000BE398 File Offset: 0x000BC598
	public override void FixedUpdate()
	{
		base.transform.AddPosition((float)((this.direction != RetroArcadeBonusRobot.Direction.Right) ? -1 : 1) * this.properties.bonusMoveSpeed * CupheadTime.FixedDelta, 0f, 0f);
		if ((this.direction != RetroArcadeBonusRobot.Direction.Left) ? (base.transform.position.x > 400f) : (base.transform.position.x < -400f))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04001CA4 RID: 7332
	public const float SPAWN_X = 400f;

	// Token: 0x04001CA5 RID: 7333
	public const float SPAWN_Y = 250f;

	// Token: 0x04001CA6 RID: 7334
	public LevelProperties.RetroArcade.Robots properties;

	// Token: 0x04001CA7 RID: 7335
	public RetroArcadeBonusRobot.Direction direction;

	// Token: 0x02000E42 RID: 3650
	public enum Direction
	{
		// Token: 0x0400670A RID: 26378
		Left,
		// Token: 0x0400670B RID: 26379
		Right
	}
}
