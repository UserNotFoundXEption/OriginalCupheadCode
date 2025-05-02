using System;
using UnityEngine;

// Token: 0x02000312 RID: 786
public class RetroArcadeOrbiterRobot : RetroArcadeEnemy
{
	// Token: 0x060022AD RID: 8877 RVA: 0x000BE434 File Offset: 0x000BC634
	public RetroArcadeOrbiterRobot Create(RetroArcadeBigRobot parent, LevelProperties.RetroArcade.Robots properties, float angle)
	{
		RetroArcadeOrbiterRobot retroArcadeOrbiterRobot = this.InstantiatePrefab<RetroArcadeOrbiterRobot>();
		retroArcadeOrbiterRobot.transform.position = parent.transform.position + properties.smallRobotRotationDistance * MathUtils.AngleToDirection(angle);
		retroArcadeOrbiterRobot.properties = properties;
		retroArcadeOrbiterRobot.parent = parent;
		retroArcadeOrbiterRobot.angle = angle;
		retroArcadeOrbiterRobot.hp = properties.smallRobotHp;
		return retroArcadeOrbiterRobot;
	}

	// Token: 0x060022AE RID: 8878 RVA: 0x0001D7E2 File Offset: 0x0001B9E2
	public override void Start()
	{
		base.PointsWorth = this.properties.pointsGained;
		base.PointsBonus = this.properties.pointsBonus;
	}

	// Token: 0x060022AF RID: 8879 RVA: 0x000BE4A0 File Offset: 0x000BC6A0
	public override void FixedUpdate()
	{
		this.angle += CupheadTime.FixedDelta * this.properties.smallRobotRotationSpeed;
		base.transform.position = this.parent.transform.position + MathUtils.AngleToDirection(this.angle) * this.properties.smallRobotRotationDistance;
	}

	// Token: 0x060022B0 RID: 8880 RVA: 0x0001D806 File Offset: 0x0001BA06
	public void Shoot()
	{
		this.projectilePrefab.Create(this.projectileRoot.position, -90f, this.properties.smallRobotShootSpeed);
	}

	// Token: 0x04001CA8 RID: 7336
	[SerializeField]
	public BasicProjectile projectilePrefab;

	// Token: 0x04001CA9 RID: 7337
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04001CAA RID: 7338
	public LevelProperties.RetroArcade.Robots properties;

	// Token: 0x04001CAB RID: 7339
	public float angle;

	// Token: 0x04001CAC RID: 7340
	public RetroArcadeBigRobot parent;
}
