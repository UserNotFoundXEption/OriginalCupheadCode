using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200030D RID: 781
public class RetroArcadeQShipTentacle : RetroArcadeEnemy
{
	// Token: 0x06002282 RID: 8834 RVA: 0x000BDBF0 File Offset: 0x000BBDF0
	public RetroArcadeQShipTentacle Create(RetroArcadeQShipTentacle.Direction direction, LevelProperties.RetroArcade.QShip properties)
	{
		RetroArcadeQShipTentacle retroArcadeQShipTentacle = this.InstantiatePrefab<RetroArcadeQShipTentacle>();
		retroArcadeQShipTentacle.transform.position = new Vector2((direction != RetroArcadeQShipTentacle.Direction.Left) ? -400f : 400f, -140f);
		retroArcadeQShipTentacle.properties = properties;
		retroArcadeQShipTentacle.direction = direction;
		retroArcadeQShipTentacle.hp = 1f;
		retroArcadeQShipTentacle.transform.SetScale(new float?((float)((direction != RetroArcadeQShipTentacle.Direction.Right) ? -1 : 1)), new float?(1f), new float?(1f));
		return retroArcadeQShipTentacle;
	}

	// Token: 0x06002283 RID: 8835 RVA: 0x000BDC80 File Offset: 0x000BBE80
	public override void FixedUpdate()
	{
		base.transform.AddPosition((float)((this.direction != RetroArcadeQShipTentacle.Direction.Right) ? -1 : 1) * this.properties.tentacleSpeed * CupheadTime.FixedDelta, 0f, 0f);
		if ((this.direction != RetroArcadeQShipTentacle.Direction.Left) ? (base.transform.position.x > 400f) : (base.transform.position.x < -400f))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002284 RID: 8836 RVA: 0x000BDD1C File Offset: 0x000BBF1C
	public override void Dead()
	{
		this.StopAllCoroutines();
		foreach (Collider2D collider2D in base.GetComponentsInChildren<Collider2D>())
		{
			collider2D.enabled = false;
		}
		base.IsDead = true;
		foreach (SpriteRenderer spriteRenderer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.color = new Color(0f, 0f, 0f, 0.25f);
		}
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x06002285 RID: 8837 RVA: 0x000BDDB0 File Offset: 0x000BBFB0
	public IEnumerator moveOffscreen_cr()
	{
		base.MoveY(-250f - base.transform.position.y, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04001C82 RID: 7298
	public const float SPAWN_X = 400f;

	// Token: 0x04001C83 RID: 7299
	public const float SPAWN_Y = -140f;

	// Token: 0x04001C84 RID: 7300
	public const float OFFSCREEN_Y = -250f;

	// Token: 0x04001C85 RID: 7301
	public const float MOVE_Y_SPEED = 500f;

	// Token: 0x04001C86 RID: 7302
	public LevelProperties.RetroArcade.QShip properties;

	// Token: 0x04001C87 RID: 7303
	public RetroArcadeQShipTentacle.Direction direction;

	// Token: 0x02000E3B RID: 3643
	public enum Direction
	{
		// Token: 0x040066DF RID: 26335
		Left,
		// Token: 0x040066E0 RID: 26336
		Right
	}
}
