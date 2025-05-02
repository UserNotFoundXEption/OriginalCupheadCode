using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002FE RID: 766
public class RetroArcadeAlienManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x170002EB RID: 747
	// (get) Token: 0x0600220B RID: 8715 RVA: 0x0001D248 File Offset: 0x0001B448
	// (set) Token: 0x0600220C RID: 8716 RVA: 0x0001D250 File Offset: 0x0001B450
	public RetroArcadeAlien.Direction direction { get; set; }

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x0600220D RID: 8717 RVA: 0x0001D259 File Offset: 0x0001B459
	// (set) Token: 0x0600220E RID: 8718 RVA: 0x0001D261 File Offset: 0x0001B461
	public float moveSpeed { get; set; }

	// Token: 0x0600220F RID: 8719 RVA: 0x000BBE70 File Offset: 0x000BA070
	public void StartAliens()
	{
		this.p = base.properties.CurrentState.aliens;
		this.aliens = new RetroArcadeAlien[this.p.numColumns, this.alienPrefabs.Length];
		this.direction = ((!Rand.Bool()) ? RetroArcadeAlien.Direction.Right : RetroArcadeAlien.Direction.Left);
		for (int i = 0; i < this.aliens.GetLength(0); i++)
		{
			for (int j = 0; j < this.aliens.GetLength(1); j++)
			{
				Vector2 position;
				position..ctor(50f * ((float)i - (float)(this.aliens.GetLength(0) - 1) / 2f), 230f - (float)j * 40f + 170f);
				this.aliens[i, j] = this.alienPrefabs[j].Create(position, i, this, this.p);
				this.aliens[i, j].MoveY(-170f);
			}
		}
		this.numDied = 0;
		this.moveSpeed = 640f / this.p.moveTime;
		this.shotRate = this.p.shotRate.Clone();
		this.currentTopRowY = 230f;
		base.StartCoroutine(this.turn_cr());
		base.StartCoroutine(this.shoot_cr());
		base.StartCoroutine(this.randomShot_cr());
		base.StartCoroutine(this.bonus_cr());
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x000BBFE8 File Offset: 0x000BA1E8
	public IEnumerator turn_cr()
	{
		for (;;)
		{
			if ((this.direction == RetroArcadeAlien.Direction.Right && this.getRightmost().transform.position.x > 320f) || (this.direction == RetroArcadeAlien.Direction.Left && this.getLeftmost().transform.position.x < -320f))
			{
				this.direction = ((this.direction != RetroArcadeAlien.Direction.Left) ? RetroArcadeAlien.Direction.Left : RetroArcadeAlien.Direction.Right);
				float num = -40f;
				if (this.currentTopRowY - (float)(this.aliens.GetLength(1) - 1) * 40f + num < -40f)
				{
					num = 230f - this.currentTopRowY;
				}
				for (int i = 0; i < this.aliens.GetLength(0); i++)
				{
					if (this.isColumnAlive(i))
					{
						for (int j = 0; j < this.aliens.GetLength(1); j++)
						{
							this.aliens[i, j].MoveY(num);
						}
					}
				}
				this.currentTopRowY += num;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x000BC004 File Offset: 0x000BA204
	public IEnumerator shoot_cr()
	{
		string[] columnPattern = this.p.shotColumnPattern.RandomChoice<string>().Split(new char[]
		{
			','
		});
		int columnPatternIndex = Random.Range(0, columnPattern.Length);
		yield return CupheadTime.WaitForSeconds(this, this.shotRate.RandomFloat());
		for (;;)
		{
			columnPatternIndex = (columnPatternIndex + 1) % columnPattern.Length;
			int column = 0;
			Parser.IntTryParse(columnPattern[columnPatternIndex], out column);
			column--;
			if (this.isColumnAlive(column))
			{
				this.getBottommostInColumn(column).Shoot();
				yield return CupheadTime.WaitForSeconds(this, this.shotRate.RandomFloat());
			}
		}
		yield break;
	}

	// Token: 0x06002212 RID: 8722 RVA: 0x000BC020 File Offset: 0x000BA220
	public IEnumerator randomShot_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, MathUtils.ExpRandom(this.p.randomShotAverageTime));
		for (;;)
		{
			int column = Random.Range(0, this.aliens.GetLength(0));
			while (!this.isColumnAlive(column))
			{
				column = Random.Range(0, this.aliens.GetLength(0));
			}
			this.getBottommostInColumn(column).Shoot();
			yield return CupheadTime.WaitForSeconds(this, MathUtils.ExpRandom(this.p.randomShotAverageTime));
		}
		yield break;
	}

	// Token: 0x06002213 RID: 8723 RVA: 0x000BC03C File Offset: 0x000BA23C
	public IEnumerator bonus_cr()
	{
		for (int i = 0; i < this.p.bonusAppearCount; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, this.p.bonusAppearTime.RandomFloat());
			this.bonusAlien.Create((!Rand.Bool()) ? RetroArcadeBonusAlien.Direction.Right : RetroArcadeBonusAlien.Direction.Left, this.p);
		}
		yield break;
	}

	// Token: 0x06002214 RID: 8724 RVA: 0x000BC058 File Offset: 0x000BA258
	public RetroArcadeAlien getLeftmost()
	{
		for (int i = 0; i < this.aliens.GetLength(0); i++)
		{
			for (int j = 0; j < this.aliens.GetLength(1); j++)
			{
				if (!this.aliens[i, j].IsDead)
				{
					return this.aliens[i, j];
				}
			}
		}
		return null;
	}

	// Token: 0x06002215 RID: 8725 RVA: 0x000BC0C8 File Offset: 0x000BA2C8
	public RetroArcadeAlien getRightmost()
	{
		for (int i = this.aliens.GetLength(0) - 1; i >= 0; i--)
		{
			for (int j = 0; j < this.aliens.GetLength(1); j++)
			{
				if (!this.aliens[i, j].IsDead)
				{
					return this.aliens[i, j];
				}
			}
		}
		return null;
	}

	// Token: 0x06002216 RID: 8726 RVA: 0x000BC138 File Offset: 0x000BA338
	public RetroArcadeAlien getTopmost()
	{
		for (int i = 0; i < this.aliens.GetLength(1); i++)
		{
			for (int j = 0; j < this.aliens.GetLength(0); j++)
			{
				if (!this.aliens[j, i].IsDead)
				{
					return this.aliens[j, i];
				}
			}
		}
		return null;
	}

	// Token: 0x06002217 RID: 8727 RVA: 0x000BC1A8 File Offset: 0x000BA3A8
	public RetroArcadeAlien getBottommost()
	{
		for (int i = this.aliens.GetLength(1) - 1; i >= 0; i--)
		{
			for (int j = 0; j < this.aliens.GetLength(0); j++)
			{
				if (!this.aliens[j, i].IsDead)
				{
					return this.aliens[j, i];
				}
			}
		}
		return null;
	}

	// Token: 0x06002218 RID: 8728 RVA: 0x000BC218 File Offset: 0x000BA418
	public bool isColumnAlive(int x)
	{
		for (int i = 0; i < this.aliens.GetLength(1); i++)
		{
			if (!this.aliens[x, i].IsDead)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002219 RID: 8729 RVA: 0x000BC25C File Offset: 0x000BA45C
	public RetroArcadeAlien getBottommostInColumn(int x)
	{
		for (int i = this.aliens.GetLength(1) - 1; i >= 0; i--)
		{
			if (!this.aliens[x, i].IsDead)
			{
				return this.aliens[x, i];
			}
		}
		return null;
	}

	// Token: 0x0600221A RID: 8730 RVA: 0x000BC2B0 File Offset: 0x000BA4B0
	public void OnAlienDie(RetroArcadeAlien alien)
	{
		this.numDied++;
		this.moveSpeed = 640f / (this.p.moveTime - (float)this.numDied * this.p.moveTimeDecrease);
		this.shotRate.max -= this.p.shotRateDecrease;
		this.shotRate.min -= this.p.shotRateDecrease;
		if (!this.isColumnAlive(alien.ColumnIndex))
		{
			for (int i = 0; i < this.aliens.GetLength(1); i++)
			{
				this.aliens[alien.ColumnIndex, i].MoveY(170f + (230f - this.aliens[alien.ColumnIndex, 0].transform.position.y));
			}
		}
		if (this.numDied >= this.aliens.Length)
		{
			this.StopAllCoroutines();
			base.properties.DealDamageToNextNamedState();
			base.StartCoroutine(this.waveOver_cr());
		}
	}

	// Token: 0x0600221B RID: 8731 RVA: 0x000BC3DC File Offset: 0x000BA5DC
	public IEnumerator waveOver_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		RetroArcadeAlien[,] array = this.aliens;
		int length = array.GetLength(0);
		int length2 = array.GetLength(1);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				RetroArcadeAlien retroArcadeAlien = array[i, j];
				Object.Destroy(retroArcadeAlien.gameObject);
			}
		}
		yield break;
	}

	// Token: 0x04001C0B RID: 7179
	public const float TOP_ROW_Y = 230f;

	// Token: 0x04001C0C RID: 7180
	public const float COLUMN_SPACING = 50f;

	// Token: 0x04001C0D RID: 7181
	public const float ROW_SPACING = 40f;

	// Token: 0x04001C0E RID: 7182
	public const float TURNAROUND_X = 320f;

	// Token: 0x04001C0F RID: 7183
	public const float MIN_Y = -40f;

	// Token: 0x04001C10 RID: 7184
	public const float OFFSCREEN_MOVE_Y = 170f;

	// Token: 0x04001C11 RID: 7185
	[SerializeField]
	public RetroArcadeAlien[] alienPrefabs;

	// Token: 0x04001C12 RID: 7186
	public RetroArcadeAlien[,] aliens;

	// Token: 0x04001C13 RID: 7187
	[SerializeField]
	public RetroArcadeBonusAlien bonusAlien;

	// Token: 0x04001C16 RID: 7190
	public MinMax shotRate;

	// Token: 0x04001C17 RID: 7191
	public int numDied;

	// Token: 0x04001C18 RID: 7192
	public float currentTopRowY;

	// Token: 0x04001C19 RID: 7193
	public LevelProperties.RetroArcade.Aliens p;
}
