using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005A7 RID: 1447
public class OneTimeScrollingSprite : AbstractPausableComponent
{
	// Token: 0x06003D03 RID: 15619 RVA: 0x000313CA File Offset: 0x0002F5CA
	public void Start()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06003D04 RID: 15620 RVA: 0x00117974 File Offset: 0x00115B74
	public IEnumerator loop_cr()
	{
		SpriteRenderer spriteRenderer = base.GetComponent<SpriteRenderer>();
		while (base.transform.position.x + spriteRenderer.bounds.size.x / 2f > -1280f)
		{
			if (this.OutCondition != null && this.OutCondition())
			{
				yield break;
			}
			Vector2 position = base.transform.localPosition;
			position.x -= this.speed * CupheadTime.Delta;
			base.transform.localPosition = position;
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04003083 RID: 12419
	public const float X_OUT = -1280f;

	// Token: 0x04003084 RID: 12420
	public float speed;

	// Token: 0x04003085 RID: 12421
	public Func<bool> OutCondition;
}
