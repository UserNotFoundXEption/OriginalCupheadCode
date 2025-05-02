using System;
using UnityEngine;

// Token: 0x02000322 RID: 802
public class RetroArcadeUFOAlien : RetroArcadeEnemy
{
	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06002310 RID: 8976 RVA: 0x0001DBAF File Offset: 0x0001BDAF
	public float NormalizedHpRemaining
	{
		get
		{
			return this.hp / this.properties.hp;
		}
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x000BF734 File Offset: 0x000BD934
	public RetroArcadeUFOAlien Create(RetroArcadeUFO parent, LevelProperties.RetroArcade.UFO properties)
	{
		RetroArcadeUFOAlien retroArcadeUFOAlien = this.InstantiatePrefab<RetroArcadeUFOAlien>();
		retroArcadeUFOAlien.properties = properties;
		retroArcadeUFOAlien.parent = parent;
		retroArcadeUFOAlien.hp = properties.hp;
		retroArcadeUFOAlien.transform.parent = parent.transform;
		retroArcadeUFOAlien.transform.position = parent.transform.position;
		this.cyclePositionIndex = Random.Range(0, retroArcadeUFOAlien.properties.cyclePositionX.Length);
		return retroArcadeUFOAlien;
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x000BF7A4 File Offset: 0x000BD9A4
	public override void Start()
	{
		base.transform.position = base.transform.position + new Vector3(this.properties.initialPositionX, (float)this.properties.alienYPosition, 0f) * this.parent.transform.localScale.y;
		base.PointsWorth = this.properties.pointsGained;
		base.PointsWorth = this.properties.pointsBonus;
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x0001DBC3 File Offset: 0x0001BDC3
	public override void Dead()
	{
		base.Dead();
		this.parent.OnAlienDie();
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x000BF82C File Offset: 0x000BDA2C
	public override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.OnDamageTaken(info);
		base.transform.position = new Vector3(this.properties.cyclePositionX[this.cyclePositionIndex], base.transform.position.y, 0f);
		this.cyclePositionIndex = (this.cyclePositionIndex + 1) % this.properties.cyclePositionX.Length;
	}

	// Token: 0x04001D1A RID: 7450
	public LevelProperties.RetroArcade.UFO properties;

	// Token: 0x04001D1B RID: 7451
	public RetroArcadeUFO parent;

	// Token: 0x04001D1C RID: 7452
	public int cyclePositionIndex;
}
