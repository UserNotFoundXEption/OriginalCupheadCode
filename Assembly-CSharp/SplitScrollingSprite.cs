using System;
using UnityEngine;

// Token: 0x020005AE RID: 1454
public class SplitScrollingSprite : ScrollingSprite
{
	// Token: 0x06003D20 RID: 15648 RVA: 0x001182CC File Offset: 0x001164CC
	public override void Start()
	{
		base.Start();
		foreach (SpriteRenderer spriteRenderer in base.copyRenderers)
		{
			if (!this.ignoreSelfWhenHandlingSplitSprites || !(spriteRenderer.gameObject == base.gameObject))
			{
				foreach (Sprite sprite in this.splitSprites)
				{
					GameObject gameObject = new GameObject(sprite.name);
					SpriteRenderer spriteRenderer2 = gameObject.AddComponent<SpriteRenderer>();
					spriteRenderer2.sprite = sprite;
					spriteRenderer2.sortingLayerID = spriteRenderer.sortingLayerID;
					spriteRenderer2.sortingOrder = spriteRenderer.sortingOrder;
					gameObject.transform.SetParent(spriteRenderer.transform, false);
					gameObject.transform.localPosition = this.splitOffset;
				}
			}
		}
	}

	// Token: 0x040030AB RID: 12459
	[SerializeField]
	public bool ignoreSelfWhenHandlingSplitSprites;

	// Token: 0x040030AC RID: 12460
	[SerializeField]
	public Vector2 splitOffset;

	// Token: 0x040030AD RID: 12461
	[SerializeField]
	public Sprite[] splitSprites;
}
