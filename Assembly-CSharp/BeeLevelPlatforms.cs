using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class BeeLevelPlatforms : AbstractMonoBehaviour
{
	// Token: 0x06001129 RID: 4393 RVA: 0x00091EB0 File Offset: 0x000900B0
	public void Init()
	{
		List<Transform> list = new List<Transform>(this.rows);
		for (int i = 0; i < this.rows.Length; i++)
		{
			Transform transform = Object.Instantiate<Transform>(this.rows[i]);
			list.Add(transform);
			transform.transform.SetParent(base.transform);
			transform.transform.AddLocalPosition(0f, -230f, 0f);
		}
		this.rows = list.ToArray();
	}

	// Token: 0x0600112A RID: 4394 RVA: 0x00091F30 File Offset: 0x00090130
	public void Randomize(int missingCount)
	{
		foreach (Transform transform in this.rows)
		{
			List<Transform> list = new List<Transform>(transform.GetChildTransforms());
			foreach (Transform transform2 in list)
			{
				transform2.gameObject.SetActive(true);
			}
			for (int j = 0; j < missingCount; j++)
			{
				if (list.Count <= 1)
				{
					break;
				}
				int num = Random.Range(0, list.Count);
				if (num == 0 && BeeLevelPlatforms.lastPlatform == 0)
				{
					break;
				}
				if (num == 3 && BeeLevelPlatforms.lastPlatform == 2)
				{
					break;
				}
				list[num].gameObject.SetActive(false);
				BeeLevelPlatforms.lastPlatform = num;
				list.RemoveAt(num);
			}
		}
	}

	// Token: 0x04000DE9 RID: 3561
	public const float OFFSET = -230f;

	// Token: 0x04000DEA RID: 3562
	[SerializeField]
	public Transform[] rows;

	// Token: 0x04000DEB RID: 3563
	public static int lastPlatform;
}
