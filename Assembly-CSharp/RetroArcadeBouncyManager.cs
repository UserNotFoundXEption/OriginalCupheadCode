using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000302 RID: 770
public class RetroArcadeBouncyManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x06002239 RID: 8761 RVA: 0x0001D33E File Offset: 0x0001B53E
	public void StartBouncy()
	{
		base.StartCoroutine(this.spawn_balls_cr());
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x000BCAC0 File Offset: 0x000BACC0
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.black;
		foreach (Transform transform in this.spawnPoints)
		{
			Gizmos.DrawWireSphere(transform.position, 50f);
		}
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x000BCB0C File Offset: 0x000BAD0C
	public IEnumerator spawn_balls_cr()
	{
		LevelProperties.RetroArcade.Bouncy p = base.properties.CurrentState.bouncy;
		int counter = 0;
		List<RetroArcadeBouncyBallHolder> holders = new List<RetroArcadeBouncyBallHolder>();
		int typeMainIndex = Random.Range(0, p.typeString.Length);
		string[] typeString = p.typeString[typeMainIndex].Split(new char[]
		{
			','
		});
		int typeIndex = Random.Range(0, typeString.Length);
		string[] ballTypes = new string[3];
		while (counter < p.waveCount)
		{
			typeString = p.typeString[typeMainIndex].Split(new char[]
			{
				','
			});
			int posIndex = Random.Range(0, this.spawnPoints.Length);
			for (int i = 0; i < 3; i++)
			{
				ballTypes[i] = typeString[typeIndex];
				if (typeIndex < typeString.Length - 1)
				{
					typeIndex++;
				}
				else
				{
					typeMainIndex = (typeMainIndex + 1) % p.typeString.Length;
					typeIndex = 0;
				}
			}
			RetroArcadeBouncyBallHolder holder = this.ballHolder.Create(this, p, this.spawnPoints[posIndex].position, ballTypes);
			holders.Add(holder);
			counter++;
			yield return CupheadTime.WaitForSeconds(this, p.spawnRange.RandomFloat());
		}
		bool allDead = true;
		for (;;)
		{
			allDead = true;
			for (int j = 0; j < holders.Count; j++)
			{
				if (!holders[j].IsDead)
				{
					allDead = false;
				}
			}
			if (allDead)
			{
				break;
			}
			yield return null;
		}
		base.properties.DealDamageToNextNamedState();
		foreach (RetroArcadeBouncyBallHolder retroArcadeBouncyBallHolder in holders)
		{
			retroArcadeBouncyBallHolder.DestroyBallsHeld();
			Object.Destroy(retroArcadeBouncyBallHolder.gameObject);
		}
		yield return null;
		yield break;
	}

	// Token: 0x04001C2D RID: 7213
	[SerializeField]
	public RetroArcadeBouncyBallHolder ballHolder;

	// Token: 0x04001C2E RID: 7214
	[SerializeField]
	public Transform[] spawnPoints;

	// Token: 0x04001C2F RID: 7215
	public const int BALLCOUNT = 3;
}
