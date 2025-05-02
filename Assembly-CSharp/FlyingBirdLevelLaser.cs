using System;
using UnityEngine;

// Token: 0x02000233 RID: 563
public class FlyingBirdLevelLaser : AbstractMonoBehaviour
{
	// Token: 0x060019D7 RID: 6615 RVA: 0x000A7218 File Offset: 0x000A5418
	public FlyingBirdLevelLaser Create(Vector2 pos, float speed)
	{
		FlyingBirdLevelLaser flyingBirdLevelLaser = this.InstantiatePrefab<FlyingBirdLevelLaser>();
		flyingBirdLevelLaser.transform.position = pos + new Vector2(flyingBirdLevelLaser.size.x, 0f);
		flyingBirdLevelLaser.speed = speed;
		return flyingBirdLevelLaser;
	}

	// Token: 0x060019D8 RID: 6616 RVA: 0x000A7260 File Offset: 0x000A5460
	public override void Awake()
	{
		base.Awake();
		SpriteRenderer component = base.transform.GetComponent<SpriteRenderer>();
		this.size = component.sprite.bounds.size;
	}

	// Token: 0x060019D9 RID: 6617 RVA: 0x000A72A0 File Offset: 0x000A54A0
	public void Update()
	{
		base.transform.AddPosition(-this.speed * CupheadTime.Delta, 0f, 0f);
		if (base.transform.position.x < -740f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040014BE RID: 5310
	public Vector2 size;

	// Token: 0x040014BF RID: 5311
	public float speed;
}
