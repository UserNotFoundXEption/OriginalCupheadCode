using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class PolygonColliderUpdater : AbstractMonoBehaviour
{
	// Token: 0x06000822 RID: 2082 RVA: 0x000759C0 File Offset: 0x00073BC0
	public override void Awake()
	{
		base.Awake();
		this.colliders = new Dictionary<string, PolygonCollider2D>();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.sprite = null;
		if (base.GetComponent<Collider2D>() != null)
		{
			Object.Destroy(base.GetComponent<Collider2D>());
		}
		base.StartCoroutine(this.collider_cr());
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x00075A1C File Offset: 0x00073C1C
	public IEnumerator collider_cr()
	{
		for (;;)
		{
			if (this.spriteRenderer.sprite != this.sprite)
			{
				this.sprite = this.spriteRenderer.sprite;
				foreach (PolygonCollider2D polygonCollider2D in base.GetComponents<PolygonCollider2D>())
				{
					polygonCollider2D.enabled = false;
				}
				if (this.colliders.ContainsKey(this.sprite.name))
				{
					this.colliders[this.sprite.name].enabled = true;
				}
				else
				{
					this.colliders[this.sprite.name] = base.gameObject.AddComponent<PolygonCollider2D>();
					this.colliders[this.sprite.name].isTrigger = true;
				}
			}
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x04000638 RID: 1592
	public SpriteRenderer spriteRenderer;

	// Token: 0x04000639 RID: 1593
	public Sprite sprite;

	// Token: 0x0400063A RID: 1594
	public Dictionary<string, PolygonCollider2D> colliders;
}
