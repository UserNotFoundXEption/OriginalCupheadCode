using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000307 RID: 775
public class RetroArcadeChaserManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x0600225A RID: 8794 RVA: 0x0001D41E File Offset: 0x0001B61E
	public void StartChasers()
	{
		this.SetupSpawnPoints();
		base.StartCoroutine(this.chasers_cr());
	}

	// Token: 0x0600225B RID: 8795 RVA: 0x000BD3F4 File Offset: 0x000BB5F4
	public void SetupSpawnPoints()
	{
		int num = 8;
		float num2 = (float)(Level.Current.Right / (num - 1) * 2);
		float num3 = 5f;
		int num4 = 0;
		this.spawnPositions = new Vector3[num * 2];
		for (int i = 0; i < num * 2; i++)
		{
			float num5 = (float)Level.Current.Left + num2 * (float)num4;
			float num6 = (i >= num) ? ((float)Level.Current.Ground + num3) : ((float)Level.Current.Ceiling - num3);
			this.spawnPositions[i] = new Vector3(num5, num6);
			num4 = ((i != num - 1) ? (num4 + 1) : 0);
		}
	}

	// Token: 0x0600225C RID: 8796 RVA: 0x000BD4B0 File Offset: 0x000BB6B0
	public IEnumerator chasers_cr()
	{
		LevelProperties.RetroArcade.Chasers p = base.properties.CurrentState.chasers;
		int mainColorIndex = Random.Range(0, p.colorString.Length);
		string[] colorString = p.colorString[mainColorIndex].Split(new char[]
		{
			','
		});
		int mainDelayIndex = Random.Range(0, p.delayString.Length);
		string[] delayString = p.delayString[mainDelayIndex].Split(new char[]
		{
			','
		});
		int delayIndex = Random.Range(0, delayString.Length);
		int mainOrderIndex = Random.Range(0, p.orderString.Length);
		string[] orderString = p.orderString[mainOrderIndex].Split(new char[]
		{
			','
		});
		int orderIndex = Random.Range(0, orderString.Length);
		RetroArcadeChaser chaserSelected = null;
		int spawnIndex = 0;
		float delay = 0f;
		float chaserSpeed = 0f;
		float chaserrotation = 0f;
		float chaserHp = 0f;
		float chaserDuration = 0f;
		this.chasers = new List<RetroArcadeChaser>();
		for (int i = 0; i < colorString.Length; i++)
		{
			AbstractPlayerController player = PlayerManager.GetNext();
			orderString = p.orderString[mainOrderIndex].Split(new char[]
			{
				','
			});
			delayString = p.delayString[mainDelayIndex].Split(new char[]
			{
				','
			});
			if (colorString[i][0] == 'G')
			{
				chaserSelected = this.chaserGreenPrefab;
				chaserSpeed = p.greenSpeed;
				chaserrotation = p.greenRotation;
				chaserHp = p.greenHP;
				chaserDuration = p.greenDuration;
			}
			else if (colorString[i][0] == 'Y')
			{
				chaserSelected = this.chaserYellowPrefab;
				chaserSpeed = p.yellowSpeed;
				chaserrotation = p.yellowRotation;
				chaserHp = p.yellowHP;
				chaserDuration = p.yellowDuration;
			}
			else if (colorString[i][0] == 'O')
			{
				chaserSelected = this.chaserOrangePrefab;
				chaserSpeed = p.orangeSpeed;
				chaserrotation = p.orangeRotation;
				chaserHp = p.orangeHP;
				chaserDuration = p.orangeDuration;
			}
			Parser.IntTryParse(orderString[orderIndex], out spawnIndex);
			Parser.FloatTryParse(delayString[delayIndex], out delay);
			RetroArcadeChaser chaser = chaserSelected.Spawn<RetroArcadeChaser>();
			chaser.Init(this.spawnPositions[spawnIndex], 0f, chaserSpeed, chaserSpeed, chaserrotation, chaserDuration, chaserHp, player, p);
			this.chasers.Add(chaser);
			if (orderIndex < p.orderString.Length - 1)
			{
				orderIndex++;
			}
			else
			{
				mainOrderIndex = (mainOrderIndex + 1) % p.orderString.Length;
				orderIndex = 0;
			}
			if (delayIndex < p.delayString.Length - 1)
			{
				delayIndex++;
			}
			else
			{
				mainDelayIndex = (mainDelayIndex + 1) % p.delayString.Length;
				delayIndex = 0;
			}
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		bool isDone = false;
		while (!isDone)
		{
			isDone = true;
			foreach (RetroArcadeChaser retroArcadeChaser in this.chasers)
			{
				isDone = retroArcadeChaser.IsDone;
			}
			yield return null;
		}
		foreach (RetroArcadeChaser retroArcadeChaser2 in this.chasers)
		{
			Object.Destroy(retroArcadeChaser2);
		}
		base.properties.DealDamageToNextNamedState();
		yield return null;
		yield break;
	}

	// Token: 0x04001C59 RID: 7257
	[SerializeField]
	public RetroArcadeChaser chaserGreenPrefab;

	// Token: 0x04001C5A RID: 7258
	[SerializeField]
	public RetroArcadeChaser chaserYellowPrefab;

	// Token: 0x04001C5B RID: 7259
	[SerializeField]
	public RetroArcadeChaser chaserOrangePrefab;

	// Token: 0x04001C5C RID: 7260
	public Vector3[] spawnPositions;

	// Token: 0x04001C5D RID: 7261
	public List<RetroArcadeChaser> chasers;
}
