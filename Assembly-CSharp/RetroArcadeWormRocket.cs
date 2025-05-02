using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000327 RID: 807
public class RetroArcadeWormRocket : RetroArcadeEnemy
{
	// Token: 0x06002329 RID: 9001 RVA: 0x000BFC9C File Offset: 0x000BDE9C
	public RetroArcadeWormRocket Create(RetroArcadeWormRocket.Direction direction, LevelProperties.RetroArcade.Worm properties)
	{
		RetroArcadeWormRocket retroArcadeWormRocket = this.InstantiatePrefab<RetroArcadeWormRocket>();
		retroArcadeWormRocket.transform.position = new Vector2((direction != RetroArcadeWormRocket.Direction.Left) ? -330f : 330f, 330f);
		retroArcadeWormRocket.properties = properties;
		retroArcadeWormRocket.direction = direction;
		retroArcadeWormRocket.hp = 1f;
		retroArcadeWormRocket.StartCoroutine(retroArcadeWormRocket.main_cr());
		return retroArcadeWormRocket;
	}

	// Token: 0x0600232A RID: 9002 RVA: 0x0001DC2B File Offset: 0x0001BE2B
	public override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.brokenPiecePrefab.Create(this.brokenPieceRoot.position, -90f, this.properties.rocketBrokenPieceSpeed);
	}

	// Token: 0x0600232B RID: 9003 RVA: 0x000BFD08 File Offset: 0x000BDF08
	public IEnumerator main_cr()
	{
		base.MoveY(-60f, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		while ((this.direction == RetroArcadeWormRocket.Direction.Left && base.transform.position.x > -330f) || (this.direction == RetroArcadeWormRocket.Direction.Right && base.transform.position.x < 330f))
		{
			base.transform.AddPosition((float)((this.direction != RetroArcadeWormRocket.Direction.Right) ? -1 : 1) * this.properties.rocketSpeed * CupheadTime.FixedDelta, 0f, 0f);
			yield return new WaitForFixedUpdate();
		}
		base.MoveY(60f, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04001D38 RID: 7480
	public const float SPAWN_X = 330f;

	// Token: 0x04001D39 RID: 7481
	public const float OFFSCREEN_Y = 330f;

	// Token: 0x04001D3A RID: 7482
	public const float BASE_Y = 270f;

	// Token: 0x04001D3B RID: 7483
	public const float MOVE_Y_SPEED = 500f;

	// Token: 0x04001D3C RID: 7484
	public RetroArcadeWormRocket.Direction direction;

	// Token: 0x04001D3D RID: 7485
	public LevelProperties.RetroArcade.Worm properties;

	// Token: 0x04001D3E RID: 7486
	[SerializeField]
	public BasicProjectile brokenPiecePrefab;

	// Token: 0x04001D3F RID: 7487
	[SerializeField]
	public Transform brokenPieceRoot;

	// Token: 0x02000E5E RID: 3678
	public enum Direction
	{
		// Token: 0x040067C5 RID: 26565
		Left,
		// Token: 0x040067C6 RID: 26566
		Right
	}
}
