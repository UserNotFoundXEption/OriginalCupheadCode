using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000316 RID: 790
public class RetroArcadeSheriffManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x060022C4 RID: 8900 RVA: 0x0001D8C5 File Offset: 0x0001BAC5
	public void StartSheriff()
	{
		this.SetupSpawnPositions();
		base.StartCoroutine(this.sheriff_cr());
	}

	// Token: 0x060022C5 RID: 8901 RVA: 0x000BE9CC File Offset: 0x000BCBCC
	public void SetupSpawnPositions()
	{
		int num = 6;
		int num2 = 4;
		float num3 = this.bottom.position.y - (float)Level.Current.Ground;
		float num4 = ((float)Level.Current.Right - 20f) / (float)(num - 1) * 2f;
		float num5 = ((float)Level.Current.Ceiling - Mathf.Abs(num3) - 20f) / (float)(num2 - 1) * 2f;
		int num6 = 0;
		this.spawnPositions = new List<Vector3>();
		for (int i = 0; i < num * 2; i++)
		{
			float num7 = (float)Level.Current.Left + 20f + num4 * (float)num6;
			float num8 = (i >= num) ? ((float)Level.Current.Ground + 20f) : ((float)Level.Current.Ceiling - 20f);
			this.spawnPositions.Add(new Vector3(num7, num8));
			num6 = ((i != num - 1) ? (num6 + 1) : 0);
		}
		num6 = 1;
		for (int j = 1; j < num2 * 2 - 1; j++)
		{
			float num9 = (j >= num2) ? ((float)Level.Current.Left + 20f) : ((float)Level.Current.Right - 20f);
			float num10 = (float)Level.Current.Ground + 20f + num5 * (float)num6;
			this.spawnPositions.Add(new Vector3(num9, num10));
			if (j == num2 - 2)
			{
				num6 = 1;
				j = num2 - 1 + 1;
			}
			else
			{
				num6++;
			}
		}
	}

	// Token: 0x060022C6 RID: 8902 RVA: 0x000BEB7C File Offset: 0x000BCD7C
	public IEnumerator sheriff_cr()
	{
		LevelProperties.RetroArcade.Sheriff p = base.properties.CurrentState.sheriff;
		int delayMainIndex = Random.Range(0, p.delayString.Length);
		string[] delayString = p.delayString[delayMainIndex].Split(new char[]
		{
			','
		});
		int delayIndex = Random.Range(0, delayString.Length);
		int colorMainIndex = Random.Range(0, p.colorString.Length);
		string[] colorString = p.colorString[colorMainIndex].Split(new char[]
		{
			','
		});
		int colorIndex = Random.Range(0, colorString.Length);
		RetroArcadeSheriff sheriffChosen = null;
		bool direction = false;
		this.sheriffs = new List<RetroArcadeSheriff>();
		for (int i = 0; i < this.spawnPositions.Count; i++)
		{
			colorString = p.colorString[colorMainIndex].Split(new char[]
			{
				','
			});
			if (colorString[colorIndex][0] == 'G')
			{
				sheriffChosen = this.sheriffGreenPrefab;
			}
			else if (colorString[colorIndex][0] == 'Y')
			{
				sheriffChosen = this.sheriffYellowPrefab;
			}
			else if (colorString[colorIndex][0] == 'O')
			{
				sheriffChosen = this.sheriffOrangePrefab;
			}
			RetroArcadeSheriff item = sheriffChosen.Create(this.spawnPositions[i], p.moveSpeed, direction, 20f, p);
			this.sheriffs.Add(item);
			if (colorIndex < colorString.Length - 1)
			{
				colorIndex++;
			}
			else
			{
				colorMainIndex = (colorMainIndex + 1) % p.colorString.Length;
				colorIndex = 0;
			}
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		foreach (RetroArcadeSheriff retroArcadeSheriff in this.sheriffs)
		{
			retroArcadeSheriff.StartMoving();
		}
		base.StartCoroutine(this.check_if_dead_cr());
		float delay = 0f;
		Parser.FloatTryParse(delayString[delayIndex], out delay);
		yield return CupheadTime.WaitForSeconds(this, delay - this.delaySubstract);
		for (;;)
		{
			int chosen = Random.Range(0, this.sheriffs.Count);
			int countDeadOnes = 0;
			for (int j = 0; j < this.sheriffs.Count; j++)
			{
				if (this.sheriffs[j].IsDead)
				{
					countDeadOnes++;
				}
			}
			if (countDeadOnes >= this.sheriffs.Count)
			{
				break;
			}
			while (this.sheriffs[chosen].IsDead)
			{
				chosen = Random.Range(0, this.sheriffs.Count);
				yield return null;
			}
			AbstractPlayerController player = PlayerManager.GetNext();
			this.sheriffs[chosen].Shoot(player);
			if (delayIndex < delayString.Length - 1)
			{
				delayIndex++;
			}
			else
			{
				delayMainIndex = (delayMainIndex + 1) % p.delayString.Length;
				delayIndex = 0;
			}
			yield return null;
			Parser.FloatTryParse(delayString[delayIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay - this.delaySubstract);
		}
		this.EndPhase();
		yield return null;
		yield break;
	}

	// Token: 0x060022C7 RID: 8903 RVA: 0x000BEB98 File Offset: 0x000BCD98
	public IEnumerator check_if_dead_cr()
	{
		bool[] killedOff = new bool[this.sheriffs.Count];
		for (;;)
		{
			for (int i = 0; i < this.sheriffs.Count; i++)
			{
				if (this.sheriffs[i].IsDead && !killedOff[i])
				{
					killedOff[i] = true;
					this.HandleDeathChange();
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x000BEBB4 File Offset: 0x000BCDB4
	public void HandleDeathChange()
	{
		for (int i = 0; i < this.sheriffs.Count; i++)
		{
			this.sheriffs[i].speed += base.properties.CurrentState.sheriff.moveSpeedAddition;
		}
		this.delaySubstract += base.properties.CurrentState.sheriff.delayMinus;
	}

	// Token: 0x060022C9 RID: 8905 RVA: 0x000BEC2C File Offset: 0x000BCE2C
	public void EndPhase()
	{
		this.StopAllCoroutines();
		foreach (RetroArcadeSheriff retroArcadeSheriff in this.sheriffs)
		{
			Object.Destroy(retroArcadeSheriff.gameObject);
		}
		base.properties.DealDamageToNextNamedState();
	}

	// Token: 0x04001CC3 RID: 7363
	[SerializeField]
	public Transform bottom;

	// Token: 0x04001CC4 RID: 7364
	[SerializeField]
	public RetroArcadeSheriff sheriffGreenPrefab;

	// Token: 0x04001CC5 RID: 7365
	[SerializeField]
	public RetroArcadeSheriff sheriffYellowPrefab;

	// Token: 0x04001CC6 RID: 7366
	[SerializeField]
	public RetroArcadeSheriff sheriffOrangePrefab;

	// Token: 0x04001CC7 RID: 7367
	public List<RetroArcadeSheriff> sheriffs;

	// Token: 0x04001CC8 RID: 7368
	public List<Vector3> spawnPositions;

	// Token: 0x04001CC9 RID: 7369
	public const float offset = 20f;

	// Token: 0x04001CCA RID: 7370
	public float delaySubstract;
}
