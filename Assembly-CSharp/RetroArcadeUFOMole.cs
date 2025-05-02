using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000323 RID: 803
public class RetroArcadeUFOMole : RetroArcadeEnemy
{
	// Token: 0x06002316 RID: 8982 RVA: 0x000BF898 File Offset: 0x000BDA98
	public RetroArcadeUFOMole Create(LevelProperties.RetroArcade.UFO properties)
	{
		RetroArcadeUFOMole retroArcadeUFOMole = this.InstantiatePrefab<RetroArcadeUFOMole>();
		retroArcadeUFOMole.properties = properties;
		retroArcadeUFOMole.direction = ((!Rand.Bool()) ? RetroArcadeUFOMole.Direction.Right : RetroArcadeUFOMole.Direction.Left);
		retroArcadeUFOMole.hp = properties.hp;
		retroArcadeUFOMole.StartCoroutine(retroArcadeUFOMole.main_cr());
		return retroArcadeUFOMole;
	}

	// Token: 0x06002317 RID: 8983 RVA: 0x000BF8E4 File Offset: 0x000BDAE4
	public IEnumerator main_cr()
	{
		base.transform.SetPosition(new float?(Random.Range(-200f, 200f)), new float?(-167f), null);
		this.direction = ((!Rand.Bool()) ? RetroArcadeUFOMole.Direction.Right : RetroArcadeUFOMole.Direction.Left);
		SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
		Collider2D col = base.GetComponent<Collider2D>();
		sprite.sortingOrder = 90;
		col.enabled = false;
		base.MoveY(-66f - base.transform.position.y, this.properties.moleAttackSpeed);
		while (this.movingY)
		{
			yield return new WaitForFixedUpdate();
		}
		bool[] leftOfPlayer = new bool[2];
		bool firstCheck = true;
		for (;;)
		{
			bool shouldAttack = false;
			while (!shouldAttack)
			{
				base.transform.AddPosition((float)((this.direction != RetroArcadeUFOMole.Direction.Left) ? 1 : -1) * this.properties.moleSpeed * CupheadTime.FixedDelta, 0f, 0f);
				if ((this.direction == RetroArcadeUFOMole.Direction.Left && base.transform.position.x < -200f) || (this.direction == RetroArcadeUFOMole.Direction.Right && base.transform.position.x > 200f))
				{
					this.direction = ((this.direction != RetroArcadeUFOMole.Direction.Left) ? RetroArcadeUFOMole.Direction.Left : RetroArcadeUFOMole.Direction.Right);
				}
				for (int i = 0; i < 2; i++)
				{
					ArcadePlayerController arcadePlayerController = ((i != 0) ? PlayerManager.GetPlayer(PlayerId.PlayerTwo) : PlayerManager.GetPlayer(PlayerId.PlayerOne)) as ArcadePlayerController;
					if (!(arcadePlayerController == null))
					{
						bool flag = leftOfPlayer[i];
						leftOfPlayer[i] = (arcadePlayerController.center.x < base.transform.position.x);
						if (leftOfPlayer[i] != flag && Mathf.Abs(base.transform.position.x) < 150f && arcadePlayerController.motor.Grounded && !firstCheck)
						{
							shouldAttack = true;
						}
					}
				}
				firstCheck = false;
				yield return new WaitForFixedUpdate();
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.moleWarningDelay);
			base.MoveY(-167f - base.transform.position.y, this.properties.moleAttackSpeed);
			while (this.movingY)
			{
				yield return new WaitForFixedUpdate();
			}
			sprite.sortingOrder = 200;
			col.enabled = true;
			base.MoveY(-114f - base.transform.position.y, this.properties.moleAttackSpeed);
			while (this.movingY)
			{
				yield return new WaitForFixedUpdate();
			}
			base.MoveY(-167f - base.transform.position.y, this.properties.moleAttackSpeed);
			while (this.movingY)
			{
				yield return new WaitForFixedUpdate();
			}
			sprite.sortingOrder = 90;
			col.enabled = false;
			base.MoveY(-66f - base.transform.position.y, this.properties.moleAttackSpeed);
			while (this.movingY)
			{
				yield return new WaitForFixedUpdate();
			}
			firstCheck = true;
		}
		yield break;
	}

	// Token: 0x06002318 RID: 8984 RVA: 0x0001DBDE File Offset: 0x0001BDDE
	public void OnWaveEnd()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x000BF900 File Offset: 0x000BDB00
	public IEnumerator moveOffscreen_cr()
	{
		base.MoveY(-167f - base.transform.position.y, this.properties.moleAttackSpeed);
		while (this.movingY)
		{
			yield return new WaitForFixedUpdate();
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04001D1D RID: 7453
	public const float BACKGROUND_Y = -66f;

	// Token: 0x04001D1E RID: 7454
	public const float UNDERGROUND_Y = -167f;

	// Token: 0x04001D1F RID: 7455
	public const float POPUP_Y = -114f;

	// Token: 0x04001D20 RID: 7456
	public const float TURNAROUND_X = 200f;

	// Token: 0x04001D21 RID: 7457
	public const float MAX_ATTACK_X = 150f;

	// Token: 0x04001D22 RID: 7458
	public const int BACKGROUND_SORT_ORDER = 90;

	// Token: 0x04001D23 RID: 7459
	public const int ATTACK_SORT_ORDER = 200;

	// Token: 0x04001D24 RID: 7460
	public RetroArcadeUFOMole.Direction direction;

	// Token: 0x04001D25 RID: 7461
	public LevelProperties.RetroArcade.UFO properties;

	// Token: 0x02000E56 RID: 3670
	public enum Direction
	{
		// Token: 0x0400679D RID: 26525
		Left,
		// Token: 0x0400679E RID: 26526
		Right
	}
}
