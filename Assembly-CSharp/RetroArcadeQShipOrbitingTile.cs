using System;

// Token: 0x0200030C RID: 780
public class RetroArcadeQShipOrbitingTile : AbstractCollidableObject
{
	// Token: 0x0600227E RID: 8830 RVA: 0x000BDAC8 File Offset: 0x000BBCC8
	public RetroArcadeQShipOrbitingTile Create(RetroArcadeQShip parent, float angle, LevelProperties.RetroArcade.QShip properties)
	{
		RetroArcadeQShipOrbitingTile retroArcadeQShipOrbitingTile = this.InstantiatePrefab<RetroArcadeQShipOrbitingTile>();
		retroArcadeQShipOrbitingTile.transform.position = parent.transform.position + properties.tileRotationDistance * MathUtils.AngleToDirection(angle);
		retroArcadeQShipOrbitingTile.properties = properties;
		retroArcadeQShipOrbitingTile.transform.parent = parent.transform;
		retroArcadeQShipOrbitingTile.parent = parent;
		retroArcadeQShipOrbitingTile.angle = angle;
		DamageReceiver component = retroArcadeQShipOrbitingTile.GetComponent<DamageReceiver>();
		component.OnDamageTaken += retroArcadeQShipOrbitingTile.OnDamageTaken;
		return retroArcadeQShipOrbitingTile;
	}

	// Token: 0x0600227F RID: 8831 RVA: 0x000BDB54 File Offset: 0x000BBD54
	public void FixedUpdate()
	{
		this.angle += CupheadTime.FixedDelta * this.parent.TileRotationSpeed;
		base.transform.position = this.parent.transform.position + MathUtils.AngleToDirection(this.angle) * this.properties.tileRotationDistance;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.angle));
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x0001D590 File Offset: 0x0001B790
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.parent.ShootProjectile();
	}

	// Token: 0x04001C7F RID: 7295
	public float angle;

	// Token: 0x04001C80 RID: 7296
	public RetroArcadeQShip parent;

	// Token: 0x04001C81 RID: 7297
	public LevelProperties.RetroArcade.QShip properties;
}
