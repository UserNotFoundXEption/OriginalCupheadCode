using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001FF RID: 511
public class DicePalacePachinkoLevelPipes : LevelProperties.DicePalacePachinko.Entity
{
	// Token: 0x06001790 RID: 6032 RVA: 0x000A1DD8 File Offset: 0x0009FFD8
	public override void LevelInit(LevelProperties.DicePalacePachinko properties)
	{
		base.LevelInit(properties);
		this.spawnPointIndex = Random.Range(0, properties.CurrentState.balls.spawnOrderString.Split(new char[]
		{
			','
		}).Length);
		this.spawnDelayIndex = Random.Range(0, properties.CurrentState.balls.ballDelayString.Split(new char[]
		{
			','
		}).Length);
		this.pinkBallSpawnIndex = Random.Range(0, properties.CurrentState.balls.pinkString.Split(new char[]
		{
			','
		}).Length);
		this.currentBallCount = 0;
		Level.Current.OnIntroEvent += this.StartAttack;
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x000140E6 File Offset: 0x000122E6
	public void StartAttack()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06001792 RID: 6034 RVA: 0x000A1E94 File Offset: 0x000A0094
	public IEnumerator attack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.balls.initialAttackDelay);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.properties.CurrentState.balls.ballDelayString.Split(new char[]
			{
				','
			})[this.spawnDelayIndex]));
			AbstractProjectile ball = this.ballPrefab.Create(this.spawnPoints[Parser.IntParse(base.properties.CurrentState.balls.spawnOrderString.Split(new char[]
			{
				','
			})[this.spawnPointIndex]) - 1].position);
			ball.GetComponent<DicePalacePachinkoLevelPipeBall>().InitBall(base.properties);
			if (this.currentBallCount < Parser.IntParse(base.properties.CurrentState.balls.pinkString.Split(new char[]
			{
				','
			})[this.pinkBallSpawnIndex]))
			{
				this.currentBallCount++;
			}
			else
			{
				ball.SetParryable(true);
				ball.GetComponentInChildren<SpriteRenderer>().color = Color.red;
				this.pinkBallSpawnIndex = Random.Range(0, base.properties.CurrentState.balls.pinkString.Split(new char[]
				{
					','
				}).Length);
				this.currentBallCount = 0;
			}
			this.spawnPointIndex++;
			if (this.spawnPointIndex >= base.properties.CurrentState.balls.spawnOrderString.Split(new char[]
			{
				','
			}).Length)
			{
				this.spawnPointIndex = 0;
			}
			this.spawnDelayIndex++;
			if (this.spawnDelayIndex >= base.properties.CurrentState.balls.ballDelayString.Split(new char[]
			{
				','
			}).Length)
			{
				this.spawnDelayIndex = 0;
			}
		}
		yield break;
	}

	// Token: 0x06001793 RID: 6035 RVA: 0x000140F5 File Offset: 0x000122F5
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x04001321 RID: 4897
	[SerializeField]
	public Transform[] spawnPoints;

	// Token: 0x04001322 RID: 4898
	[SerializeField]
	public DicePalacePachinkoLevelPipeBall ballPrefab;

	// Token: 0x04001323 RID: 4899
	public int spawnDelayIndex;

	// Token: 0x04001324 RID: 4900
	public int spawnPointIndex;

	// Token: 0x04001325 RID: 4901
	public int pinkBallSpawnIndex;

	// Token: 0x04001326 RID: 4902
	public int currentBallCount;
}
