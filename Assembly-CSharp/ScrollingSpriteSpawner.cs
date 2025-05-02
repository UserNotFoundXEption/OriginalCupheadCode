using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class ScrollingSpriteSpawner : AbstractPausableComponent
{
	// Token: 0x06000A49 RID: 2633 RVA: 0x000095A9 File Offset: 0x000077A9
	public override void Awake()
	{
		base.Awake();
		if (!this.customStart)
		{
			base.StartCoroutine(this.loop_cr(false));
		}
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x000095CA File Offset: 0x000077CA
	public void StartLoop(bool ensureInitialOffscreenSpawn = false)
	{
		base.StartCoroutine(this.loop_cr(ensureInitialOffscreenSpawn));
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x0007B6BC File Offset: 0x000798BC
	public IEnumerator loop_cr(bool ensureInitialOffscreenSpawn)
	{
		float leadTime = 2560f / this.speed;
		float totalWeight = 0f;
		foreach (ScrollingSpriteSpawner.ScrollingSpriteInfo scrollingSpriteInfo in this.spritePrefabs)
		{
			totalWeight += scrollingSpriteInfo.weight;
		}
		float spacing = Random.Range(0f, this.minSpacing) + MathUtils.ExpRandom(this.averageSpacing - this.minSpacing);
		ScrollingSpriteSpawner.ScrollingSpriteInfo lastSpawned = null;
		for (;;)
		{
			while (this.pauseScrolling)
			{
				yield return null;
			}
			float waitTime = spacing / this.speed;
			if (ensureInitialOffscreenSpawn)
			{
				waitTime = Mathf.Max(waitTime, leadTime * 1.1f);
			}
			if (leadTime > waitTime)
			{
				leadTime -= waitTime;
			}
			else
			{
				if (leadTime > 0f)
				{
					waitTime -= leadTime;
					leadTime = 0f;
				}
				yield return CupheadTime.WaitForSeconds(this, waitTime);
			}
			float maxP = totalWeight;
			if (lastSpawned != null)
			{
				maxP -= lastSpawned.weight;
			}
			float p = Random.Range(0f, maxP);
			float cumulativeWeight = 0f;
			ScrollingSpriteSpawner.ScrollingSpriteInfo toSpawn = lastSpawned;
			foreach (ScrollingSpriteSpawner.ScrollingSpriteInfo scrollingSpriteInfo2 in this.spritePrefabs)
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
			float x = 1280f - leadTime * this.speed + sprite.bounds.size.x / 2f;
			if (this.usePrefabY)
			{
				obj.transform.position = new Vector3(x, toSpawn.sprite.transform.position.y);
			}
			else
			{
				obj.transform.position = new Vector2(x, this.spawnY);
			}
			obj.transform.SetParent(base.transform, false);
			sprite.sortingOrder = this.sortingOrder;
			OneTimeScrollingSprite scrollingSprite = obj.AddComponent<OneTimeScrollingSprite>();
			scrollingSprite.speed = this.speed;
			spacing = this.minSpacing + MathUtils.ExpRandom(this.averageSpacing - this.minSpacing) + sprite.bounds.size.x;
			this.OnSpawn(obj);
			lastSpawned = toSpawn;
		}
		yield break;
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x000095DA File Offset: 0x000077DA
	public void HandlePausing(bool pause)
	{
		this.pauseScrolling = pause;
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x000095E3 File Offset: 0x000077E3
	public virtual void OnSpawn(GameObject obj)
	{
	}

	// Token: 0x0400083D RID: 2109
	public const float X_IN = 1280f;

	// Token: 0x0400083E RID: 2110
	public const float X_OUT = -1280f;

	// Token: 0x0400083F RID: 2111
	[SerializeField]
	public bool customStart;

	// Token: 0x04000840 RID: 2112
	[SerializeField]
	public float spawnY;

	// Token: 0x04000841 RID: 2113
	[SerializeField]
	public bool usePrefabY;

	// Token: 0x04000842 RID: 2114
	[SerializeField]
	[Range(0f, 2000f)]
	public float speed = 100f;

	// Token: 0x04000843 RID: 2115
	[SerializeField]
	public float minSpacing = 50f;

	// Token: 0x04000844 RID: 2116
	[SerializeField]
	public float averageSpacing = 100f;

	// Token: 0x04000845 RID: 2117
	[SerializeField]
	public int sortingOrder;

	// Token: 0x04000846 RID: 2118
	[SerializeField]
	public ScrollingSpriteSpawner.ScrollingSpriteInfo[] spritePrefabs;

	// Token: 0x04000847 RID: 2119
	public bool pauseScrolling;

	// Token: 0x0200094F RID: 2383
	[Serializable]
	public class ScrollingSpriteInfo
	{
		// Token: 0x04004604 RID: 17924
		public SpriteRenderer sprite;

		// Token: 0x04004605 RID: 17925
		public float weight = 1f;
	}
}
