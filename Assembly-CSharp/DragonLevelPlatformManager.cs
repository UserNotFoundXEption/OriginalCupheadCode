using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000215 RID: 533
public class DragonLevelPlatformManager : AbstractPausableComponent
{
	// Token: 0x0600185F RID: 6239 RVA: 0x000A3B00 File Offset: 0x000A1D00
	public void Init(LevelProperties.Dragon.Clouds properties)
	{
		this.properties = properties;
		this.toggleDelay = true;
		this.platforms = new List<DragonLevelCloudPlatform>();
		for (int i = 0; i < this.maxPlatforms; i++)
		{
			DragonLevelCloudPlatform dragonLevelCloudPlatform = Object.Instantiate<DragonLevelCloudPlatform>(this.platformPrefab);
			dragonLevelCloudPlatform.gameObject.SetActive(false);
			dragonLevelCloudPlatform.transform.parent = base.transform;
			this.platforms.Add(dragonLevelCloudPlatform);
		}
		base.StartCoroutine(this.spawn_platforms());
		base.StartCoroutine(this.run_delay_cr());
	}

	// Token: 0x06001860 RID: 6240 RVA: 0x000A3B8C File Offset: 0x000A1D8C
	public void UpdateProperties(LevelProperties.Dragon.Clouds properties)
	{
		this.toggleDelay = true;
		this.properties = properties;
		foreach (DragonLevelCloudPlatform dragonLevelCloudPlatform in this.platforms)
		{
			dragonLevelCloudPlatform.GetProperties(properties, false);
		}
		base.StartCoroutine(this.run_delay_cr());
	}

	// Token: 0x06001861 RID: 6241 RVA: 0x00014D2F File Offset: 0x00012F2F
	public void DestroyObjectPool(DragonLevelCloudPlatform obj)
	{
		this.platforms.Add(obj);
		obj.gameObject.SetActive(false);
	}

	// Token: 0x06001862 RID: 6242 RVA: 0x000A3C04 File Offset: 0x000A1E04
	public IEnumerator run_delay_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.cloudSpeed / 200f);
		this.toggleDelay = false;
		yield break;
	}

	// Token: 0x06001863 RID: 6243 RVA: 0x000A3C20 File Offset: 0x000A1E20
	public IEnumerator spawn_platforms()
	{
		List<string> positions = new List<string>(this.properties.cloudPositions);
		int mainIndex = Random.Range(0, positions.Count);
		string[] positionString = positions[mainIndex].Split(new char[]
		{
			','
		});
		int positionIndex = Random.Range(0, positionString.Length);
		int platformIndex = 0;
		float platformWidth = this.platformPrefab.GetComponent<Renderer>().bounds.size.x / 2f;
		float waitTime = 0f;
		float position = 0f;
		for (;;)
		{
			while (this.toggleDelay)
			{
				yield return null;
			}
			positionString = positions[mainIndex].Split(new char[]
			{
				','
			});
			this.startPosition = ((!this.properties.movingRight) ? (640f + platformWidth) : (-640f - platformWidth));
			if (positionString[positionIndex][0] == 'D')
			{
				Parser.FloatTryParse(positionString[positionIndex].Substring(1), out waitTime);
			}
			else
			{
				string[] array = positionString[positionIndex].Split(new char[]
				{
					'-'
				});
				foreach (string s in array)
				{
					Parser.FloatTryParse(s, out position);
					this.platforms[platformIndex].transform.position = new Vector3(this.startPosition, 360f - position, 0f);
					this.platforms[platformIndex].gameObject.SetActive(true);
					this.platforms[platformIndex].GetProperties(this, this.properties);
					platformIndex = (platformIndex + 1) % this.platforms.Count;
				}
				waitTime = this.properties.cloudDelay;
			}
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			if (positionIndex < positionString.Length - 1)
			{
				positionIndex++;
			}
			else if (positions.Count > 1)
			{
				positions.Remove(positions[mainIndex]);
				positionIndex = 0;
				mainIndex = Random.Range(0, positions.Count);
			}
			else
			{
				positionIndex = 0;
				mainIndex = 0;
				positions = new List<string>(this.properties.cloudPositions);
			}
		}
		yield break;
	}

	// Token: 0x06001864 RID: 6244 RVA: 0x00014D49 File Offset: 0x00012F49
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.platformPrefab = null;
	}

	// Token: 0x040013C7 RID: 5063
	public const float LEFT_X = -1280f;

	// Token: 0x040013C8 RID: 5064
	public const float RIGHT_X = 1280f;

	// Token: 0x040013C9 RID: 5065
	[SerializeField]
	public List<DragonLevelCloudPlatform> platforms;

	// Token: 0x040013CA RID: 5066
	[SerializeField]
	public DragonLevelCloudPlatform platformPrefab;

	// Token: 0x040013CB RID: 5067
	public LevelProperties.Dragon.Clouds properties;

	// Token: 0x040013CC RID: 5068
	public int maxPlatforms = 20;

	// Token: 0x040013CD RID: 5069
	public float startPosition;

	// Token: 0x040013CE RID: 5070
	public bool toggleDelay;
}
