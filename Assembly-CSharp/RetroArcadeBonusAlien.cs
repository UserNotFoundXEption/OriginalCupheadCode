using System;
using UnityEngine;

// Token: 0x020002FF RID: 767
public class RetroArcadeBonusAlien : RetroArcadeEnemy
{
	// Token: 0x0600221D RID: 8733 RVA: 0x000BC3F8 File Offset: 0x000BA5F8
	public RetroArcadeBonusAlien Create(RetroArcadeBonusAlien.Direction direction, LevelProperties.RetroArcade.Aliens properties)
	{
		RetroArcadeBonusAlien retroArcadeBonusAlien = this.InstantiatePrefab<RetroArcadeBonusAlien>();
		retroArcadeBonusAlien.transform.position = new Vector2((direction != RetroArcadeBonusAlien.Direction.Left) ? -400f : 400f, 270f);
		retroArcadeBonusAlien.properties = properties;
		retroArcadeBonusAlien.direction = direction;
		retroArcadeBonusAlien.hp = 1f;
		return retroArcadeBonusAlien;
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x000BC458 File Offset: 0x000BA658
	public override void FixedUpdate()
	{
		base.transform.AddPosition((float)((this.direction != RetroArcadeBonusAlien.Direction.Right) ? -1 : 1) * this.properties.bonusMoveSpeed * CupheadTime.FixedDelta, 0f, 0f);
		if ((this.direction != RetroArcadeBonusAlien.Direction.Left) ? (base.transform.position.x > 400f) : (base.transform.position.x < -400f))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04001C1A RID: 7194
	public const float SPAWN_X = 400f;

	// Token: 0x04001C1B RID: 7195
	public const float SPAWN_Y = 270f;

	// Token: 0x04001C1C RID: 7196
	public LevelProperties.RetroArcade.Aliens properties;

	// Token: 0x04001C1D RID: 7197
	public RetroArcadeBonusAlien.Direction direction;

	// Token: 0x02000E26 RID: 3622
	public enum Direction
	{
		// Token: 0x04006657 RID: 26199
		Left,
		// Token: 0x04006658 RID: 26200
		Right
	}
}
