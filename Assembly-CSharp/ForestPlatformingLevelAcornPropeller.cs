using System;
using UnityEngine;

// Token: 0x020003E3 RID: 995
public class ForestPlatformingLevelAcornPropeller : AbstractPausableComponent
{
	// Token: 0x06002BF7 RID: 11255 RVA: 0x00024D52 File Offset: 0x00022F52
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06002BF8 RID: 11256 RVA: 0x000D8DA8 File Offset: 0x000D6FA8
	public ForestPlatformingLevelAcornPropeller Create(Vector2 position, float speed)
	{
		ForestPlatformingLevelAcornPropeller forestPlatformingLevelAcornPropeller = this.InstantiatePrefab<ForestPlatformingLevelAcornPropeller>();
		forestPlatformingLevelAcornPropeller.transform.position = position;
		forestPlatformingLevelAcornPropeller.speed = speed;
		return forestPlatformingLevelAcornPropeller;
	}

	// Token: 0x06002BF9 RID: 11257 RVA: 0x000D8DD8 File Offset: 0x000D6FD8
	public void Update()
	{
		base.transform.AddPosition(0f, this.speed * CupheadTime.Delta, 0f);
		this.t += CupheadTime.Delta;
		if (this.t > 5f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04002465 RID: 9317
	public float speed;

	// Token: 0x04002466 RID: 9318
	public float t;

	// Token: 0x04002467 RID: 9319
	public const float LIFETIME = 5f;
}
