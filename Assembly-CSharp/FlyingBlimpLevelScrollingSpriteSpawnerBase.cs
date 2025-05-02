using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000246 RID: 582
public class FlyingBlimpLevelScrollingSpriteSpawnerBase : AbstractPausableComponent
{
	// Token: 0x06001AB7 RID: 6839 RVA: 0x00016B3C File Offset: 0x00014D3C
	public override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x000A9844 File Offset: 0x000A7A44
	public IEnumerator loop_cr()
	{
		float leadTime = 2560f / this.speed;
		float totalWeight = 0f;
		foreach (FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo scrollingSpriteInfo in this.spritePrefabs)
		{
			totalWeight += scrollingSpriteInfo.weight;
		}
		float spacing = Random.Range(0f, this.minSpacing) + MathUtils.ExpRandom(this.averageSpacing - this.minSpacing);
		FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo lastSpawned = null;
		for (;;)
		{
			float waitTime = spacing / this.speed;
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
			FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo toSpawn = lastSpawned;
			foreach (FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo scrollingSpriteInfo2 in this.spritePrefabs)
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
			obj.transform.position = new Vector2(x, this.spawnY);
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

	// Token: 0x06001AB9 RID: 6841 RVA: 0x00016B51 File Offset: 0x00014D51
	public virtual void OnSpawn(GameObject obj)
	{
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x00016B53 File Offset: 0x00014D53
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.spritePrefabs = null;
	}

	// Token: 0x04001585 RID: 5509
	public const float X_IN = 1280f;

	// Token: 0x04001586 RID: 5510
	public const float X_OUT = -1280f;

	// Token: 0x04001587 RID: 5511
	[SerializeField]
	public float spawnY;

	// Token: 0x04001588 RID: 5512
	[SerializeField]
	[Range(0f, 2000f)]
	public float speed = 100f;

	// Token: 0x04001589 RID: 5513
	[SerializeField]
	public float minSpacing = 50f;

	// Token: 0x0400158A RID: 5514
	[SerializeField]
	public float averageSpacing = 100f;

	// Token: 0x0400158B RID: 5515
	[SerializeField]
	public int sortingOrder;

	// Token: 0x0400158C RID: 5516
	[SerializeField]
	public FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo[] spritePrefabs;

	// Token: 0x02000C96 RID: 3222
	[Serializable]
	public class ScrollingSpriteInfo
	{
		// Token: 0x04005ADB RID: 23259
		public SpriteRenderer sprite;

		// Token: 0x04005ADC RID: 23260
		public float weight = 1f;
	}
}
