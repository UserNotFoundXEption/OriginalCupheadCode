using System;
using UnityEngine;

// Token: 0x02000163 RID: 355
public class BeeLevelBackgroundGroup : AbstractMonoBehaviour
{
	// Token: 0x0600110D RID: 4365 RVA: 0x0000E649 File Offset: 0x0000C849
	public void Start()
	{
		this.level = (Level.Current as BeeLevel);
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x00091A70 File Offset: 0x0008FC70
	public void Update()
	{
		if (base.transform.localPosition.y < -800f)
		{
			this.SetY(base.transform.localPosition.y + (float)this.count * 455f);
			this.Randomize();
		}
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x00091AC8 File Offset: 0x0008FCC8
	public void FixedUpdate()
	{
		this.SetY(base.transform.localPosition.y + this.level.Speed * CupheadTime.Delta);
	}

	// Token: 0x06001110 RID: 4368 RVA: 0x00091B08 File Offset: 0x0008FD08
	public void Init(BeeLevelPlatforms platforms, int groupCount)
	{
		this.level = (Level.Current as BeeLevel);
		this.count = groupCount;
		this.platforms = Object.Instantiate<BeeLevelPlatforms>(platforms);
		this.platforms.transform.SetParent(base.transform);
		this.platforms.Init();
		this.Randomize();
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x0000E65B File Offset: 0x0000C85B
	public void Randomize()
	{
		this.DisableAll();
		this.platforms.Randomize(this.level.MissingPlatformCount);
		this.variations[Random.Range(0, this.variations.Length)].SetActive(true);
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x00091B60 File Offset: 0x0008FD60
	public void DisableAll()
	{
		foreach (GameObject gameObject in this.variations)
		{
			gameObject.SetActive(false);
		}
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x0000E694 File Offset: 0x0000C894
	public void SetY(float y)
	{
		base.transform.SetPosition(new float?(0f), new float?(y), new float?(0f));
	}

	// Token: 0x04000DD8 RID: 3544
	public const float MIN_Y = -800f;

	// Token: 0x04000DD9 RID: 3545
	[SerializeField]
	public GameObject[] variations;

	// Token: 0x04000DDA RID: 3546
	public BeeLevel level;

	// Token: 0x04000DDB RID: 3547
	public BeeLevelPlatforms platforms;

	// Token: 0x04000DDC RID: 3548
	public int count;

	// Token: 0x04000DDD RID: 3549
	public int lastCount;
}
