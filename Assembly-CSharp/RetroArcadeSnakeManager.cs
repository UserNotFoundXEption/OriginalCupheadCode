using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000318 RID: 792
public class RetroArcadeSnakeManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x060022DD RID: 8925 RVA: 0x0001D991 File Offset: 0x0001BB91
	public void StartSnake()
	{
		base.StartCoroutine(this.spawn_snake_cr());
	}

	// Token: 0x060022DE RID: 8926 RVA: 0x000BF0A0 File Offset: 0x000BD2A0
	public IEnumerator spawn_snake_cr()
	{
		AbstractPlayerController player = PlayerManager.GetNext();
		this.snakeFull = new RetroArcadeSnakeBodyPart[8];
		RetroArcadeSnakeBodyPart.Direction direction = RetroArcadeSnakeBodyPart.Direction.Down;
		Vector3 startPos = new Vector3(-player.transform.position.x, 200f);
		this.snakeFull[0] = this.bodyPrefab.Create(new Vector2(startPos.x, startPos.y), true, direction, this, this.snakeFull[0], base.properties.CurrentState.snake.moveSpeed);
		for (int i = 1; i < 8; i++)
		{
			this.snakeFull[i] = this.bodyPrefab.Create(new Vector2(startPos.x, startPos.y + (float)i * 60f), i == 0, direction, this, (i != 0) ? this.snakeFull[i - 1] : this.snakeFull[i], base.properties.CurrentState.snake.moveSpeed);
		}
		this.snakeFull[0].GetPartBehind(this.snakeFull[1]);
		yield return null;
		yield break;
	}

	// Token: 0x060022DF RID: 8927 RVA: 0x0001D9A0 File Offset: 0x0001BBA0
	public void EndPhase()
	{
		base.StartCoroutine(this.end_phase_cr());
	}

	// Token: 0x060022E0 RID: 8928 RVA: 0x000BF0BC File Offset: 0x000BD2BC
	public IEnumerator end_phase_cr()
	{
		for (int j = 0; j < this.snakeFull.Length; j++)
		{
			this.snakeFull[j].Die();
		}
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		for (int i = this.snakeFull.Length - 1; i >= 0; i--)
		{
			Object.Destroy(this.snakeFull[i].gameObject);
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		base.properties.DealDamageToNextNamedState();
		yield return null;
		yield break;
	}

	// Token: 0x04001CDB RID: 7387
	[SerializeField]
	public RetroArcadeSnakeBodyPart bodyPrefab;

	// Token: 0x04001CDC RID: 7388
	public RetroArcadeSnakeBodyPart[] snakeFull;

	// Token: 0x04001CDD RID: 7389
	public const int BODYPARTS = 8;

	// Token: 0x04001CDE RID: 7390
	public const float SPACING = 60f;

	// Token: 0x04001CDF RID: 7391
	public const float OFFSCREEN_Y = 300f;
}
