using System;
using UnityEngine;

// Token: 0x020002FD RID: 765
public class RetroArcadeAlien : RetroArcadeEnemy
{
	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06002202 RID: 8706 RVA: 0x0001D1BB File Offset: 0x0001B3BB
	// (set) Token: 0x06002203 RID: 8707 RVA: 0x0001D1C3 File Offset: 0x0001B3C3
	public int ColumnIndex { get; set; }

	// Token: 0x06002204 RID: 8708 RVA: 0x000BBDC8 File Offset: 0x000B9FC8
	public RetroArcadeAlien Create(Vector2 position, int columnIndex, RetroArcadeAlienManager manager, LevelProperties.RetroArcade.Aliens properties)
	{
		RetroArcadeAlien retroArcadeAlien = this.InstantiatePrefab<RetroArcadeAlien>();
		retroArcadeAlien.transform.position = position;
		retroArcadeAlien.properties = properties;
		retroArcadeAlien.manager = manager;
		retroArcadeAlien.hp = properties.hp;
		retroArcadeAlien.ColumnIndex = columnIndex;
		return retroArcadeAlien;
	}

	// Token: 0x06002205 RID: 8709 RVA: 0x0001D1CC File Offset: 0x0001B3CC
	public override void Start()
	{
		base.PointsWorth = this.properties.pointsGained;
		base.PointsBonus = this.properties.pointsBonus;
	}

	// Token: 0x06002206 RID: 8710 RVA: 0x000BBE14 File Offset: 0x000BA014
	public override void FixedUpdate()
	{
		if (this.movingY)
		{
			return;
		}
		base.transform.AddPosition((float)((this.manager.direction != RetroArcadeAlien.Direction.Right) ? -1 : 1) * this.manager.moveSpeed * CupheadTime.FixedDelta, 0f, 0f);
	}

	// Token: 0x06002207 RID: 8711 RVA: 0x0001D1F0 File Offset: 0x0001B3F0
	public void MoveY(float moveAmount)
	{
		base.MoveY(moveAmount, 500f);
	}

	// Token: 0x06002208 RID: 8712 RVA: 0x0001D1FE File Offset: 0x0001B3FE
	public override void Dead()
	{
		base.Dead();
		this.manager.OnAlienDie(this);
	}

	// Token: 0x06002209 RID: 8713 RVA: 0x0001D212 File Offset: 0x0001B412
	public void Shoot()
	{
		this.bulletPrefab.Create(this.bulletRoot.position, -90f, this.properties.bulletSpeed);
	}

	// Token: 0x04001C05 RID: 7173
	public const float MOVE_Y_SPEED = 500f;

	// Token: 0x04001C06 RID: 7174
	[SerializeField]
	public BasicProjectile bulletPrefab;

	// Token: 0x04001C07 RID: 7175
	[SerializeField]
	public Transform bulletRoot;

	// Token: 0x04001C08 RID: 7176
	public LevelProperties.RetroArcade.Aliens properties;

	// Token: 0x04001C09 RID: 7177
	public RetroArcadeAlienManager manager;

	// Token: 0x02000E20 RID: 3616
	public enum Direction
	{
		// Token: 0x04006636 RID: 26166
		Left,
		// Token: 0x04006637 RID: 26167
		Right
	}
}
