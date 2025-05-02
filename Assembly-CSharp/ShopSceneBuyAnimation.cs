using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200059B RID: 1435
public class ShopSceneBuyAnimation : MonoBehaviour
{
	// Token: 0x06003CAA RID: 15530 RVA: 0x0011600C File Offset: 0x0011420C
	public void Start()
	{
		this.rightIndex = new List<int>();
		this.indexes = new List<int>
		{
			0,
			1,
			2,
			3,
			4,
			5
		};
		int index = Random.Range(0, 6);
		this.rightIndex.Add(this.indexes[index]);
		this.indexes.RemoveAt(index);
		int index2 = Random.Range(0, 5);
		this.rightIndex.Add(this.indexes[index2]);
		this.indexes.RemoveAt(index2);
		int index3 = Random.Range(0, 4);
		this.rightIndex.Add(this.indexes[index3]);
		this.indexes.RemoveAt(index3);
		for (int i = 0; i < this.rightIndex.Count; i++)
		{
			this.coins[this.rightIndex[i]].gameObject.SetActive(true);
		}
	}

	// Token: 0x06003CAB RID: 15531 RVA: 0x0011611C File Offset: 0x0011431C
	public void OnDestroy()
	{
		for (int i = 0; i < this.coins.Length; i++)
		{
			this.coins[i] = null;
		}
	}

	// Token: 0x04003017 RID: 12311
	public GameObject[] coins;

	// Token: 0x04003018 RID: 12312
	public List<int> indexes;

	// Token: 0x04003019 RID: 12313
	public List<int> rightIndex;
}
