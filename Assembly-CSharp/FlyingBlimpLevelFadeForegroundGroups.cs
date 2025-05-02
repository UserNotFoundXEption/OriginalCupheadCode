using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000243 RID: 579
public class FlyingBlimpLevelFadeForegroundGroups : FlyingBlimpLevelScrollingSpriteSpawnerBase
{
	// Token: 0x06001A95 RID: 6805 RVA: 0x000A90E0 File Offset: 0x000A72E0
	public override void Awake()
	{
		base.Awake();
		this.fadeTime = 10f;
		this.daySprites = new List<Transform>();
		this.nightSprites = new List<Transform>();
		if (this.spawnedChild != null)
		{
			this.spawnedChild.transform.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
		}
		for (int i = 0; i < this.spritePrefabs.Length; i++)
		{
			foreach (Transform transform in this.spritePrefabs[i].sprite.transform.GetChildTransforms())
			{
				this.daySprites.Add(transform.transform);
				this.nightSprites.Add(transform.transform.GetChild(0));
			}
		}
		for (int k = 0; k < this.nightSprites.Count; k++)
		{
			if (this.nightSprites[k].transform != null)
			{
				this.nightSprites[k].transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}

	// Token: 0x06001A96 RID: 6806 RVA: 0x00016A00 File Offset: 0x00014C00
	public override void OnSpawn(GameObject obj)
	{
		base.OnSpawn(obj);
		this.spawnedChild = obj.transform.GetChild(0);
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x00016A1B File Offset: 0x00014C1B
	public void Update()
	{
		if (this.moonLady.state == FlyingBlimpLevelMoonLady.State.Morph && !this.startedChange)
		{
			this.startedChange = true;
			this.StartChange();
		}
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x00016A46 File Offset: 0x00014C46
	public void StartChange()
	{
		base.StartCoroutine(this.change_cr());
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x000A9210 File Offset: 0x000A7410
	public IEnumerator change_cr()
	{
		float t = 0f;
		float startSpeed = this.speed;
		float endSpeed = this.speed + this.speed * 0.3f;
		while (t < this.fadeTime)
		{
			for (int i = 0; i < this.nightSprites.Count; i++)
			{
				if (this.nightSprites[i].transform != null)
				{
					this.nightSprites[i].transform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
					this.nightSprites[i].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / this.fadeTime);
				}
			}
			this.speed = Mathf.Lerp(startSpeed, endSpeed, t / this.fadeTime);
			t += CupheadTime.Delta;
			yield return null;
		}
		for (int j = 0; j < this.nightSprites.Count; j++)
		{
			if (this.nightSprites[j].transform != null)
			{
				this.nightSprites[j].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			}
		}
		yield break;
	}

	// Token: 0x04001558 RID: 5464
	[SerializeField]
	public FlyingBlimpLevelMoonLady moonLady;

	// Token: 0x04001559 RID: 5465
	public List<Transform> daySprites;

	// Token: 0x0400155A RID: 5466
	public List<Transform> nightSprites;

	// Token: 0x0400155B RID: 5467
	public Transform spawnedChild;

	// Token: 0x0400155C RID: 5468
	public float fadeTime;

	// Token: 0x0400155D RID: 5469
	public int index;

	// Token: 0x0400155E RID: 5470
	public int allDayChildren;

	// Token: 0x0400155F RID: 5471
	public bool startedChange;
}
