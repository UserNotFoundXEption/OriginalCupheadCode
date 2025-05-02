using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000284 RID: 644
public class FlyingMermaidLevelFishSpreadProjectile : BasicProjectile
{
	// Token: 0x06001D2E RID: 7470 RVA: 0x00018BAB File Offset: 0x00016DAB
	public override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.smoke_cr());
		base.StartCoroutine(this.layer_change_cr());
	}

	// Token: 0x06001D2F RID: 7471 RVA: 0x000B02E4 File Offset: 0x000AE4E4
	public IEnumerator smoke_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, Random.Range(0.25f, 0.5f));
		SpriteRenderer sprite = base.GetComponentInChildren<SpriteRenderer>();
		while (!base.dead)
		{
			Effect smoke = this.smokeEffectPrefab.Create(this.smokeEffectRoot.position + MathUtils.RandomPointInUnitCircle() * 15f);
			SpriteRenderer smokeSprite = smoke.GetComponentInChildren<SpriteRenderer>();
			smokeSprite.sortingLayerID = sprite.sortingLayerID;
			smokeSprite.sortingOrder = sprite.sortingOrder - 1;
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.1f, 0.2f));
		}
		yield break;
	}

	// Token: 0x06001D30 RID: 7472 RVA: 0x000B0300 File Offset: 0x000AE500
	public IEnumerator layer_change_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		SpriteRenderer sprite = base.GetComponentInChildren<SpriteRenderer>();
		sprite.sortingLayerName = "Foreground";
		sprite.sortingOrder = 30;
		yield break;
	}

	// Token: 0x040017CB RID: 6091
	[SerializeField]
	public Effect smokeEffectPrefab;

	// Token: 0x040017CC RID: 6092
	[SerializeField]
	public Transform smokeEffectRoot;
}
