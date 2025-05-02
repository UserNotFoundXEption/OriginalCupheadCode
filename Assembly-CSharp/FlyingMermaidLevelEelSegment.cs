using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000281 RID: 641
public class FlyingMermaidLevelEelSegment : AbstractPausableComponent
{
	// Token: 0x06001D1C RID: 7452 RVA: 0x000B0110 File Offset: 0x000AE310
	public FlyingMermaidLevelEelSegment Create(Vector2 position, string sortingLayer, int sortingOrder)
	{
		FlyingMermaidLevelEelSegment flyingMermaidLevelEelSegment = Object.Instantiate<FlyingMermaidLevelEelSegment>(this);
		flyingMermaidLevelEelSegment.transform.position = position;
		flyingMermaidLevelEelSegment.velocity = this.launchSpeed * MathUtils.AngleToDirection(this.angleRange.RandomFloat());
		flyingMermaidLevelEelSegment.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
		if (Random.Range(0f, 1f) > 0.5f)
		{
			flyingMermaidLevelEelSegment.transform.SetScale(new float?(-1f), null, null);
		}
		SpriteRenderer component = flyingMermaidLevelEelSegment.GetComponent<SpriteRenderer>();
		component.sortingLayerName = sortingLayer;
		component.sortingOrder = sortingOrder;
		flyingMermaidLevelEelSegment.animator.Play("Idle", 0, Random.Range(0f, 1f));
		flyingMermaidLevelEelSegment.StartCoroutine(flyingMermaidLevelEelSegment.move_cr());
		return flyingMermaidLevelEelSegment;
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x000B0200 File Offset: 0x000AE400
	public IEnumerator move_cr()
	{
		while (base.transform.position.y > this.despawnY || this.velocity.y > 0f)
		{
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040017C3 RID: 6083
	[SerializeField]
	public float gravity;

	// Token: 0x040017C4 RID: 6084
	[SerializeField]
	public MinMax angleRange;

	// Token: 0x040017C5 RID: 6085
	[SerializeField]
	public float launchSpeed;

	// Token: 0x040017C6 RID: 6086
	[SerializeField]
	public float despawnY;

	// Token: 0x040017C7 RID: 6087
	public Vector2 velocity;
}
