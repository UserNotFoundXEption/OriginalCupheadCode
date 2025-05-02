using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003F4 RID: 1012
public class TreePlatformingLevelDragonflyProjectile : BasicProjectile
{
	// Token: 0x06002C73 RID: 11379 RVA: 0x000252FE File Offset: 0x000234FE
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.bullet_trail_cr());
	}

	// Token: 0x06002C74 RID: 11380 RVA: 0x000D9E54 File Offset: 0x000D8054
	public IEnumerator bullet_trail_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.16f, 0.2f));
			Effect e = this.bulletFX.Create(base.transform.position);
			SpriteRenderer r = e.GetComponent<SpriteRenderer>();
			r.sortingOrder = -1;
			r.sortingLayerName = "Projectiles";
			yield return null;
		}
		yield break;
	}

	// Token: 0x040024AD RID: 9389
	public const string ProjectilesLayerName = "Projectiles";

	// Token: 0x040024AE RID: 9390
	[SerializeField]
	public Effect bulletFX;
}
