using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200031A RID: 794
public class RetroArcadeTentacleManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x060022E8 RID: 8936 RVA: 0x0001DA1C File Offset: 0x0001BC1C
	public void StartTentacle()
	{
		this.SetupYSpawnpoints();
		base.StartCoroutine(this.spawn_tentacles_cr());
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x000BF1A8 File Offset: 0x000BD3A8
	public void SetupYSpawnpoints()
	{
		this.YSpawnPoints = new float[8];
		float num = (float)Level.Current.Ground - this.bottom.position.y;
		this.offset = ((float)Level.Current.Height - num) / 8f;
		for (int i = 0; i < 8; i++)
		{
			float num2 = this.offset * (float)i;
			this.YSpawnPoints[i] = num2;
		}
	}

	// Token: 0x060022EA RID: 8938 RVA: 0x000BF220 File Offset: 0x000BD420
	public IEnumerator spawn_tentacles_cr()
	{
		this.octopusHead.SetActive(true);
		LevelProperties.RetroArcade.Tentacle p = base.properties.CurrentState.tentacle;
		int mainTargetIndex = Random.Range(0, p.targetString.Length);
		string[] targetString = p.targetString[mainTargetIndex].Split(new char[]
		{
			','
		});
		int targetIndex = Random.Range(0, targetString.Length);
		int counter = 0;
		int positionIndex = 0;
		bool spawningLeft = Rand.Bool();
		RetroArcadeTentacle tentacle = null;
		RetroArcadeTentacle lastLeftTentacle = null;
		RetroArcadeTentacle lastRightTentacle = null;
		this.tentacles = new RetroArcadeTentacle[p.tentacleCount];
		while (counter < p.tentacleCount)
		{
			targetString = p.targetString[mainTargetIndex].Split(new char[]
			{
				','
			});
			Parser.IntTryParse(targetString[targetIndex], out positionIndex);
			Vector3 spawnPoint = new Vector3((!spawningLeft) ? 320f : -320f, -500f);
			if (spawningLeft)
			{
				while (lastLeftTentacle != null)
				{
					if (lastLeftTentacle.transform.position.x >= -240f)
					{
						break;
					}
					yield return null;
				}
			}
			else
			{
				while (lastRightTentacle != null)
				{
					if (lastRightTentacle.transform.position.x <= 240f)
					{
						break;
					}
					yield return null;
				}
			}
			tentacle = this.tentaclePrefab.Spawn<RetroArcadeTentacle>();
			tentacle.Init(spawnPoint, this.YSpawnPoints[positionIndex], spawningLeft, p.risingSpeed, p.moveSpeed);
			this.tentacles[counter] = tentacle;
			if (spawningLeft)
			{
				lastLeftTentacle = tentacle;
			}
			else
			{
				lastRightTentacle = tentacle;
			}
			if (targetIndex < targetString.Length - 1)
			{
				targetIndex++;
			}
			else
			{
				mainTargetIndex = (mainTargetIndex + 1) % p.targetString.Length;
				targetIndex = 0;
			}
			spawningLeft = !spawningLeft;
			counter++;
			yield return null;
		}
		int countDeadOnes = 0;
		for (;;)
		{
			countDeadOnes = 0;
			for (int i = 0; i < this.tentacles.Length; i++)
			{
				if (this.tentacles[i] == null)
				{
					countDeadOnes++;
				}
			}
			if (countDeadOnes >= this.tentacles.Length)
			{
				break;
			}
			yield return null;
		}
		this.octopusHead.SetActive(false);
		base.properties.DealDamageToNextNamedState();
		yield return null;
		yield break;
	}

	// Token: 0x04001CE8 RID: 7400
	public const float LEFT_SIDE_SPAWN = -320f;

	// Token: 0x04001CE9 RID: 7401
	public const float RIGHT_SIDE_SPAWN = 320f;

	// Token: 0x04001CEA RID: 7402
	public const int SPACES_COUNT = 8;

	// Token: 0x04001CEB RID: 7403
	public const float SPAWN_OFFSET = 240f;

	// Token: 0x04001CEC RID: 7404
	[SerializeField]
	public GameObject octopusHead;

	// Token: 0x04001CED RID: 7405
	[SerializeField]
	public RetroArcadeTentacle tentaclePrefab;

	// Token: 0x04001CEE RID: 7406
	[SerializeField]
	public Transform bottom;

	// Token: 0x04001CEF RID: 7407
	public RetroArcadeTentacle[] tentacles;

	// Token: 0x04001CF0 RID: 7408
	public float[] YSpawnPoints;

	// Token: 0x04001CF1 RID: 7409
	public float offset;
}
