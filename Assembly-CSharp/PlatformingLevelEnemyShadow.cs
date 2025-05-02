using System;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class PlatformingLevelEnemyShadow : AbstractCollidableObject
{
	// Token: 0x06000A2E RID: 2606 RVA: 0x0007AFFC File Offset: 0x000791FC
	public void Start()
	{
		this.shadow = new GameObject(base.gameObject.name + "_Shadow").transform;
		this.spriteRenderer = this.shadow.gameObject.AddComponent<SpriteRenderer>();
		this.shadow.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
		this.spriteRenderer.sprite = this.shadowSprites[0];
		this.enemy = base.GetComponent<PlatformingLevelGroundMovementEnemy>();
		this.boxCollider = this.enemy.GetComponent<BoxCollider2D>();
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x0007B0A8 File Offset: 0x000792A8
	public void Update()
	{
		if (this.enemy.Grounded || this.enemy.Dead)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		this.spriteRenderer.enabled = true;
		Vector3 position = this.shadow.position;
		position.x = base.transform.position.x;
		RaycastHit2D raycastHit2D = Physics2D.BoxCast(base.transform.position, new Vector2(this.boxCollider.size.x, 1f), 0f, Vector2.down, (float)this.maxDistance, this.groundMask);
		if (raycastHit2D.collider == null)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		LevelPlatform component = raycastHit2D.collider.gameObject.GetComponent<LevelPlatform>();
		if (component != null && !component.AllowShadows)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		position.y = raycastHit2D.point.y;
		this.shadow.position = position;
		this.SetSprite();
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x0007B1E0 File Offset: 0x000793E0
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

	// Token: 0x06000A31 RID: 2609 RVA: 0x0000949A File Offset: 0x0000769A
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.shadow != null)
		{
			Object.Destroy(this.shadow.gameObject);
		}
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x000094C3 File Offset: 0x000076C3
	public Vector3 ShadowPosition()
	{
		return this.shadow.position;
	}

	// Token: 0x04000826 RID: 2086
	[Range(1f, 1000f)]
	[SerializeField]
	public int maxDistance = 250;

	// Token: 0x04000827 RID: 2087
	[SerializeField]
	public Sprite[] shadowSprites;

	// Token: 0x04000828 RID: 2088
	public Transform shadow;

	// Token: 0x04000829 RID: 2089
	public SpriteRenderer spriteRenderer;

	// Token: 0x0400082A RID: 2090
	public PlatformingLevelGroundMovementEnemy enemy;

	// Token: 0x0400082B RID: 2091
	public BoxCollider2D boxCollider;

	// Token: 0x0400082C RID: 2092
	public readonly int groundMask = 1048576;
}
