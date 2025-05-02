using System;
using UnityEngine;

// Token: 0x02000517 RID: 1303
public class LevelPlayerShadow : AbstractLevelPlayerComponent
{
	// Token: 0x0600370D RID: 14093 RVA: 0x00101A58 File Offset: 0x000FFC58
	public void Start()
	{
		this.shadow = new GameObject(base.gameObject.name + "_Shadow").transform;
		this.spriteRenderer = this.shadow.gameObject.AddComponent<SpriteRenderer>();
		this.shadow.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
		this.spriteRenderer.sprite = this.shadowSprites[0];
		if (Level.Current != null)
		{
			this.spriteRenderer.sortingOrder = Level.Current.playerShadowSortingOrder;
		}
		if (SceneLoader.CurrentLevel == Levels.ChaliceTutorial)
		{
			this.spriteRenderer.gameObject.layer = 31;
		}
	}

	// Token: 0x0600370E RID: 14094 RVA: 0x00101B2C File Offset: 0x000FFD2C
	public void Update()
	{
		if ((base.player.motor.Grounded && !base.player.motor.Dashing) || base.player.IsDead || ((base.player.stats.Loadout.charm == Charm.charm_smoke_dash || base.player.stats.CurseSmokeDash) && !Level.IsChessBoss && base.player.motor.Dashing))
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		this.spriteRenderer.enabled = true;
		Vector3 position = this.shadow.position;
		position.x = base.transform.position.x;
		BoxCollider2D collider = base.player.collider;
		RaycastHit2D raycastHit2D = Physics2D.BoxCast(base.player.transform.position, new Vector2(base.player.collider.size.x, 1f), 0f, (!base.player.motor.GravityReversed) ? Vector2.down : Vector2.up, (float)this.maxDistance, (!base.player.motor.GravityReversed) ? this.groundMask : this.ceilingMask);
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

	// Token: 0x0600370F RID: 14095 RVA: 0x00101D28 File Offset: 0x000FFF28
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

	// Token: 0x06003710 RID: 14096 RVA: 0x0002D0DF File Offset: 0x0002B2DF
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.shadow != null)
		{
			Object.Destroy(this.shadow.gameObject);
		}
	}

	// Token: 0x06003711 RID: 14097 RVA: 0x0002D108 File Offset: 0x0002B308
	public Vector3 ShadowPosition()
	{
		return this.shadow.position;
	}

	// Token: 0x04002C62 RID: 11362
	[Range(1f, 1000f)]
	[SerializeField]
	public int maxDistance = 250;

	// Token: 0x04002C63 RID: 11363
	[SerializeField]
	public Sprite[] shadowSprites;

	// Token: 0x04002C64 RID: 11364
	public Transform shadow;

	// Token: 0x04002C65 RID: 11365
	public SpriteRenderer spriteRenderer;

	// Token: 0x04002C66 RID: 11366
	public readonly int groundMask = 1048576;

	// Token: 0x04002C67 RID: 11367
	public readonly int ceilingMask = 524288;
}
