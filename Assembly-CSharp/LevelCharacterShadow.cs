using System;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class LevelCharacterShadow : AbstractPausableComponent
{
	// Token: 0x06000D49 RID: 3401 RVA: 0x00086C68 File Offset: 0x00084E68
	public void Start()
	{
		this.shadow = new GameObject(base.gameObject.name + "_Shadow").transform;
		this.spriteRenderer = this.shadow.gameObject.AddComponent<SpriteRenderer>();
		this.shadow.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
		this.spriteRenderer.sprite = this.shadowSprites[0];
		if (this.isBGLayer)
		{
			this.spriteRenderer.sortingLayerName = SpriteLayer.Background.ToString();
			this.spriteRenderer.sortingOrder = 100;
		}
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x00086D28 File Offset: 0x00084F28
	public void Update()
	{
		Vector3 position = this.shadow.position;
		position.x = base.transform.position.x;
		Collider2D component = this.root.GetComponent<Collider2D>();
		RaycastHit2D raycastHit2D = Physics2D.BoxCast(this.root.transform.position, new Vector2(component.bounds.size.x, 1f), 0f, Vector2.down, (float)this.maxDistance, this.groundMask);
		if (raycastHit2D.collider == null)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		LevelPlatform component2 = raycastHit2D.collider.gameObject.GetComponent<LevelPlatform>();
		if (component2 != null && !component2.AllowShadows)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		position.y = raycastHit2D.point.y;
		this.shadow.position = position;
		this.SetSprite();
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x00086E3C File Offset: 0x0008503C
	public void SetSprite()
	{
		int num = (int)(Mathf.Abs(base.transform.position.y - this.shadow.position.y) / (float)this.maxDistance * (float)this.shadowSprites.Length);
		if (num < 0 || num >= this.shadowSprites.Length)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		this.spriteRenderer.enabled = true;
		this.spriteRenderer.sprite = this.shadowSprites[num];
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x0000B67E File Offset: 0x0000987E
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.shadow != null)
		{
			Object.Destroy(this.shadow.gameObject);
		}
	}

	// Token: 0x04000A64 RID: 2660
	[Range(1f, 1000f)]
	[SerializeField]
	public int maxDistance = 250;

	// Token: 0x04000A65 RID: 2661
	[SerializeField]
	public Transform root;

	// Token: 0x04000A66 RID: 2662
	[SerializeField]
	public Sprite[] shadowSprites;

	// Token: 0x04000A67 RID: 2663
	[SerializeField]
	public bool isBGLayer;

	// Token: 0x04000A68 RID: 2664
	public Transform shadow;

	// Token: 0x04000A69 RID: 2665
	public SpriteRenderer spriteRenderer;

	// Token: 0x04000A6A RID: 2666
	public readonly int groundMask = 1048576;
}
