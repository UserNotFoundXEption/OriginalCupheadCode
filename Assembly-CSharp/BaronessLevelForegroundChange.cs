using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class BaronessLevelForegroundChange : AbstractPausableComponent
{
	// Token: 0x06001074 RID: 4212 RVA: 0x0000DE1C File Offset: 0x0000C01C
	public override void Awake()
	{
		base.Awake();
		this.bossNotDead = true;
		this.currentClones = new List<OneTimeScrollingSprite>();
		base.StartCoroutine(this.start_phase2_cr());
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x00090528 File Offset: 0x0008E728
	public IEnumerator start_phase2_cr()
	{
		for (int i = 0; i < this.sprites.Length; i++)
		{
			this.sprites[i].speed = 0f;
		}
		foreach (OneTimeScrollingSprite oneTimeScrollingSprite in this.currentClones)
		{
			if (oneTimeScrollingSprite != null)
			{
				oneTimeScrollingSprite.GetComponent<OneTimeScrollingSprite>().speed = 0f;
			}
		}
		while (this.baroness.state != BaronessLevelCastle.State.Chase)
		{
			yield return null;
		}
		this.StartLoop();
		for (;;)
		{
			if (!this.baroness.pauseScrolling)
			{
				for (int j = 0; j < this.sprites.Length; j++)
				{
					this.sprites[j].speed = this.speed;
				}
				foreach (OneTimeScrollingSprite oneTimeScrollingSprite2 in this.currentClones)
				{
					if (oneTimeScrollingSprite2 != null)
					{
						oneTimeScrollingSprite2.GetComponent<OneTimeScrollingSprite>().speed = this.speed;
					}
				}
			}
			else
			{
				for (int k = 0; k < this.sprites.Length; k++)
				{
					this.sprites[k].speed = 0f;
				}
				foreach (OneTimeScrollingSprite oneTimeScrollingSprite3 in this.currentClones)
				{
					if (oneTimeScrollingSprite3 != null)
					{
						oneTimeScrollingSprite3.GetComponent<OneTimeScrollingSprite>().speed = 0f;
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x0000DE43 File Offset: 0x0000C043
	public void StartLoop()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x00090544 File Offset: 0x0008E744
	public IEnumerator loop_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		float leadTime = 0f / this.speed;
		float totalWeight = 0f;
		foreach (BaronessLevelForegroundChange.ScrollingSpriteInfo scrollingSpriteInfo in this.spritePrefabs)
		{
			totalWeight += scrollingSpriteInfo.weight;
		}
		float spacing = Random.Range(0f, this.minSpacing) + MathUtils.ExpRandom(this.averageSpacing - this.minSpacing);
		BaronessLevelForegroundChange.ScrollingSpriteInfo lastSpawned = null;
		for (;;)
		{
			if (this.bossNotDead && !this.baroness.pauseScrolling)
			{
				float waitTime = spacing / this.speed;
				if (leadTime > waitTime)
				{
					leadTime -= waitTime;
					yield return null;
				}
				else
				{
					if (leadTime > 0f)
					{
						waitTime -= leadTime;
						leadTime = 0f;
						yield return null;
					}
					yield return CupheadTime.WaitForSeconds(this, waitTime);
				}
				float maxP = totalWeight;
				if (lastSpawned != null)
				{
					maxP -= lastSpawned.weight;
					yield return null;
				}
				float p = Random.Range(0f, maxP);
				float cumulativeWeight = 0f;
				BaronessLevelForegroundChange.ScrollingSpriteInfo toSpawn = lastSpawned;
				foreach (BaronessLevelForegroundChange.ScrollingSpriteInfo scrollingSpriteInfo2 in this.spritePrefabs)
				{
					toSpawn = scrollingSpriteInfo2;
					if (scrollingSpriteInfo2 != lastSpawned)
					{
						cumulativeWeight += scrollingSpriteInfo2.weight;
						if (cumulativeWeight >= p)
						{
							break;
						}
					}
				}
				SpriteRenderer sprite = Object.Instantiate<SpriteRenderer>(toSpawn.sprite);
				GameObject obj = sprite.gameObject;
				float x = -1280f - leadTime * this.speed + sprite.bounds.size.x / 2f;
				obj.transform.position = new Vector2(x, this.spawnY);
				obj.transform.SetParent(base.transform, false);
				sprite.sortingOrder = this.sortingOrder;
				OneTimeScrollingSprite scrollingSprite = obj.AddComponent<OneTimeScrollingSprite>();
				scrollingSprite.speed = this.speed;
				spacing = this.minSpacing + MathUtils.ExpRandom(this.averageSpacing - this.minSpacing) - sprite.bounds.size.x;
				this.OnSpawn(obj);
				lastSpawned = toSpawn;
				this.currentClones.Add(scrollingSprite);
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x0000DE52 File Offset: 0x0000C052
	public virtual void OnSpawn(GameObject obj)
	{
	}

	// Token: 0x04000D68 RID: 3432
	public const float X_IN = -1280f;

	// Token: 0x04000D69 RID: 3433
	public const float X_OUT = 1280f;

	// Token: 0x04000D6A RID: 3434
	[SerializeField]
	public float spawnY;

	// Token: 0x04000D6B RID: 3435
	[SerializeField]
	[Range(0f, -2000f)]
	public float speed;

	// Token: 0x04000D6C RID: 3436
	[SerializeField]
	[Range(0f, -2000f)]
	public float minSpacing;

	// Token: 0x04000D6D RID: 3437
	[SerializeField]
	[Range(0f, -2000f)]
	public float averageSpacing;

	// Token: 0x04000D6E RID: 3438
	[SerializeField]
	public int sortingOrder;

	// Token: 0x04000D6F RID: 3439
	[SerializeField]
	public BaronessLevelForegroundChange.ScrollingSpriteInfo[] spritePrefabs;

	// Token: 0x04000D70 RID: 3440
	[SerializeField]
	public BaronessLevelCastle baroness;

	// Token: 0x04000D71 RID: 3441
	[SerializeField]
	public OneTimeScrollingSprite[] sprites;

	// Token: 0x04000D72 RID: 3442
	public List<OneTimeScrollingSprite> currentClones;

	// Token: 0x04000D73 RID: 3443
	public bool bossNotDead;

	// Token: 0x02000A43 RID: 2627
	[Serializable]
	public class ScrollingSpriteInfo
	{
		// Token: 0x04004B97 RID: 19351
		public SpriteRenderer sprite;

		// Token: 0x04004B98 RID: 19352
		public float weight = 1f;
	}
}
