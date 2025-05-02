using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000162 RID: 354
public class BeeLevelBackground : LevelProperties.Bee.Entity
{
	// Token: 0x06001108 RID: 4360 RVA: 0x0000E603 File Offset: 0x0000C803
	public void Start()
	{
		this.level = (Level.Current as BeeLevel);
		base.StartCoroutine(this.middle_cr());
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x0000E622 File Offset: 0x0000C822
	public void Update()
	{
		this.back.speed = -this.level.Speed * 0.35f;
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x0009198C File Offset: 0x0008FB8C
	public override void LevelInit(LevelProperties.Bee properties)
	{
		base.LevelInit(properties);
		int[] array = new int[this.groups.Length];
		List<int> list = new List<int>();
		for (int i = 0; i < this.groups.Length; i++)
		{
			list.Add(i);
		}
		for (int j = 0; j < this.groups.Length; j++)
		{
			int index = Random.Range(0, list.Count);
			array[j] = list[index];
			list.RemoveAt(index);
			this.groups[array[j]].Init(this.platformGroup, this.groups.Length);
			this.groups[array[j]].SetY(-455f * (float)j);
		}
		this.platformGroup.gameObject.SetActive(false);
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x00091A54 File Offset: 0x0008FC54
	public IEnumerator middle_cr()
	{
		SpriteRenderer[] sprites = new SpriteRenderer[this.middleGroups.Length];
		for (int j = 0; j < this.middleGroups.Length; j++)
		{
			sprites[j] = this.middleGroups[j].GetComponentInChildren<SpriteRenderer>();
			this.middleGroups[j].gameObject.SetActive(false);
		}
		int scale = (Random.value <= 0.5f) ? -1 : 1;
		for (;;)
		{
			int i = Random.Range(0, this.middleGroups.Length);
			float height = (float)((int)sprites[i].sprite.bounds.size.y);
			float y = (720f + height) / 2f;
			this.middleGroups[i].gameObject.SetActive(true);
			this.middleGroups[i].SetPosition(new float?(0f), new float?(y), new float?(0f));
			this.middleGroups[i].SetScale(new float?((float)scale), new float?(1f), new float?(1f));
			while (this.middleGroups[i].position.y >= -y)
			{
				this.middleGroups[i].AddPosition(0f, this.level.Speed * 0.75f * CupheadTime.Delta, 0f);
				yield return null;
			}
			this.middleGroups[i].gameObject.SetActive(false);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000DD2 RID: 3538
	public const float GROUP_OFFSET = 455f;

	// Token: 0x04000DD3 RID: 3539
	[SerializeField]
	public BeeLevelPlatforms platformGroup;

	// Token: 0x04000DD4 RID: 3540
	[SerializeField]
	public BeeLevelBackgroundGroup[] groups;

	// Token: 0x04000DD5 RID: 3541
	[SerializeField]
	public Transform[] middleGroups;

	// Token: 0x04000DD6 RID: 3542
	[SerializeField]
	public ScrollingSprite back;

	// Token: 0x04000DD7 RID: 3543
	public BeeLevel level;
}
