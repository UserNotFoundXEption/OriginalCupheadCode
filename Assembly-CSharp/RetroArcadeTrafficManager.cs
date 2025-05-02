using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200031E RID: 798
public class RetroArcadeTrafficManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x060022F6 RID: 8950 RVA: 0x0001DA80 File Offset: 0x0001BC80
	public void StartTraffic()
	{
		this.SpawnTrafficLights();
		this.StartUFO();
		base.StartCoroutine(this.move_ufo_cr());
	}

	// Token: 0x060022F7 RID: 8951 RVA: 0x000BF37C File Offset: 0x000BD57C
	public void StartUFO()
	{
		this.trafficUFO.gameObject.SetActive(true);
		AbstractPlayerController next = PlayerManager.GetNext();
		if (next.transform.position.x > 0f)
		{
			this.trafficUFO.transform.position = this.trafficGrid[0, 3].transform.position;
		}
		else
		{
			this.trafficUFO.transform.position = this.trafficGrid[3, 3].transform.position;
		}
	}

	// Token: 0x060022F8 RID: 8952 RVA: 0x000BF410 File Offset: 0x000BD610
	public void SpawnTrafficLights()
	{
		this.trafficGrid = new GameObject[4, 4];
		float num = (float)(Level.Current.Width / 4);
		float num2 = (float)(Level.Current.Height / 4);
		Vector3 vector;
		vector..ctor((float)Level.Current.Left + num / 2f, (float)Level.Current.Ground + num2 / 2f);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				Vector3 vector2;
				vector2..ctor((float)i * num, (float)j * num2);
				GameObject gameObject = Object.Instantiate<GameObject>(this.trafficLightPrefab);
				this.trafficGrid[i, j] = gameObject;
				this.trafficGrid[i, j].transform.position = vector + vector2;
				this.trafficGrid[i, j].transform.parent = base.transform;
			}
		}
	}

	// Token: 0x060022F9 RID: 8953 RVA: 0x000BF508 File Offset: 0x000BD708
	public IEnumerator move_ufo_cr()
	{
		LevelProperties.RetroArcade.Traffic p = base.properties.CurrentState.traffic;
		int lightMainIndex = Random.Range(0, p.lightOrderString.Length);
		string[] lightString = p.lightOrderString[lightMainIndex].Split(new char[]
		{
			','
		});
		int lightX = 0;
		int lightY = 0;
		this.positionsToTravel = new List<Vector3>();
		while (!this.trafficUFO.IsDead)
		{
			lightString = p.lightOrderString[lightMainIndex].Split(new char[]
			{
				','
			});
			for (int i = 0; i < lightString.Length; i++)
			{
				yield return CupheadTime.WaitForSeconds(this, p.lightDelay);
				string getLightCoordX = lightString[i].Substring(1);
				string getLightCoordY = lightString[i].Substring(0, 1);
				Parser.IntTryParse(getLightCoordX, out lightX);
				if (getLightCoordY != null)
				{
					if (!(getLightCoordY == "A"))
					{
						if (!(getLightCoordY == "B"))
						{
							if (!(getLightCoordY == "C"))
							{
								if (getLightCoordY == "D")
								{
									lightY = 3;
								}
							}
							else
							{
								lightY = 2;
							}
						}
						else
						{
							lightY = 1;
						}
					}
					else
					{
						lightY = 0;
					}
				}
				this.trafficGrid[lightX, lightY].GetComponent<Animator>().SetBool("IsGreen", true);
				this.positionsToTravel.Add(this.trafficGrid[lightX, lightY].transform.position);
			}
			this.trafficUFO.StartMoving(this.positionsToTravel, p.moveSpeed, p.moveDelay);
			while (this.trafficUFO.IsMoving && !this.trafficUFO.IsDead)
			{
				yield return null;
			}
			this.ResetLights();
			this.positionsToTravel.Clear();
			lightMainIndex = (lightMainIndex + 1) % p.lightOrderString.Length;
			yield return null;
		}
		this.EndPhase();
		yield break;
	}

	// Token: 0x060022FA RID: 8954 RVA: 0x0001DA9B File Offset: 0x0001BC9B
	public void EndPhase()
	{
		this.StopAllCoroutines();
		this.DestroyLights();
		Object.Destroy(this.trafficUFO.gameObject);
		base.properties.DealDamageToNextNamedState();
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x000BF524 File Offset: 0x000BD724
	public void ResetLights()
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				this.trafficGrid[i, j].GetComponent<Animator>().SetBool("IsGreen", false);
			}
		}
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x000BF574 File Offset: 0x000BD774
	public void DestroyLights()
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				Object.Destroy(this.trafficGrid[i, j]);
			}
		}
	}

	// Token: 0x04001D00 RID: 7424
	[SerializeField]
	public RetroArcadeTrafficUFO trafficUFO;

	// Token: 0x04001D01 RID: 7425
	[SerializeField]
	public GameObject trafficLightPrefab;

	// Token: 0x04001D02 RID: 7426
	public const int GRIDX = 4;

	// Token: 0x04001D03 RID: 7427
	public const int GRIDY = 4;

	// Token: 0x04001D04 RID: 7428
	public GameObject[,] trafficGrid;

	// Token: 0x04001D05 RID: 7429
	public List<Vector3> positionsToTravel;
}
