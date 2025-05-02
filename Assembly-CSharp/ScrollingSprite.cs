using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005AD RID: 1453
public class ScrollingSprite : AbstractPausableComponent
{
	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x06003D17 RID: 15639 RVA: 0x000314E7 File Offset: 0x0002F6E7
	// (set) Token: 0x06003D18 RID: 15640 RVA: 0x000314EF File Offset: 0x0002F6EF
	public List<SpriteRenderer> copyRenderers { get; set; }

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x06003D19 RID: 15641 RVA: 0x000314F8 File Offset: 0x0002F6F8
	// (set) Token: 0x06003D1A RID: 15642 RVA: 0x00031500 File Offset: 0x0002F700
	public bool looping { get; set; }

	// Token: 0x06003D1B RID: 15643 RVA: 0x00117E10 File Offset: 0x00116010
	public virtual void Start()
	{
		this.looping = true;
		this.copyRenderers = new List<SpriteRenderer>();
		this.direction = ((!this.negativeDirection) ? 1 : -1);
		SpriteRenderer component = base.transform.GetComponent<SpriteRenderer>();
		this.copyRenderers.Add(component);
		this.size = ((this.axis != ScrollingSprite.Axis.X) ? component.sprite.bounds.size.y : component.sprite.bounds.size.x) - this.offset;
		for (int i = 0; i < this.count; i++)
		{
			GameObject gameObject = new GameObject(base.gameObject.name + " Copy");
			gameObject.transform.parent = base.transform;
			gameObject.transform.ResetLocalTransforms();
			SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sortingLayerID = component.sortingLayerID;
			spriteRenderer.sortingOrder = component.sortingOrder;
			spriteRenderer.sprite = component.sprite;
			spriteRenderer.material = component.material;
			this.copyRenderers.Add(spriteRenderer);
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject);
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.ResetLocalTransforms();
			this.copyRenderers.Add(gameObject2.GetComponent<SpriteRenderer>());
			if (this.axis == ScrollingSprite.Axis.X)
			{
				gameObject.transform.SetLocalPosition(new float?((float)this.direction * (this.size + this.size * (float)i)), new float?(0f), new float?(0f));
				gameObject2.transform.SetLocalPosition(new float?((float)this.direction * -(this.size + this.size * (float)i)), new float?(0f), new float?(0f));
			}
			else
			{
				gameObject.transform.SetLocalPosition(new float?(0f), new float?(this.size + this.size * (float)i), new float?(0f));
				gameObject2.transform.SetLocalPosition(new float?(0f), new float?(-(this.size + this.size * (float)i)), new float?(0f));
			}
		}
		this.startY = base.transform.localPosition.y;
	}

	// Token: 0x06003D1C RID: 15644 RVA: 0x001180A0 File Offset: 0x001162A0
	public virtual void Update()
	{
		this.pos = base.transform.localPosition;
		if (this.axis == ScrollingSprite.Axis.X)
		{
			if (this.pos.x <= -this.size && this.looping)
			{
				this.pos.x = this.pos.x + this.size;
				if (this.isRotated)
				{
					this.pos.y = this.startY;
				}
				this.onLoop();
			}
			if (this.pos.x >= this.size && this.looping)
			{
				this.pos.x = this.pos.x - this.size;
				this.onLoop();
			}
			if (!this.isRotated)
			{
				this.pos.x = this.pos.x - (float)((!this.negativeDirection) ? 1 : -1) * this.speed * CupheadTime.Delta * this.playbackSpeed;
			}
		}
		else
		{
			if (this.pos.y <= -this.size && this.looping)
			{
				this.pos.y = this.pos.y + this.size;
				this.onLoop();
			}
			if (this.pos.y >= this.size && this.looping)
			{
				this.pos.y = this.pos.y - this.size;
				this.onLoop();
			}
			if (!this.isRotated)
			{
				this.pos.y = this.pos.y - (float)((!this.negativeDirection) ? 1 : -1) * this.speed * CupheadTime.Delta * this.playbackSpeed;
			}
		}
		if (this.isRotated)
		{
			this.pos -= base.transform.right * this.speed * CupheadTime.Delta;
		}
		base.transform.localPosition = this.pos;
	}

	// Token: 0x06003D1D RID: 15645 RVA: 0x00031509 File Offset: 0x0002F709
	public virtual void onLoop()
	{
	}

	// Token: 0x06003D1E RID: 15646 RVA: 0x0003150B File Offset: 0x0002F70B
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.copyRenderers = null;
	}

	// Token: 0x0400309D RID: 12445
	public ScrollingSprite.Axis axis;

	// Token: 0x0400309E RID: 12446
	[SerializeField]
	public bool negativeDirection;

	// Token: 0x0400309F RID: 12447
	[SerializeField]
	public bool onLeft;

	// Token: 0x040030A0 RID: 12448
	[SerializeField]
	public bool isRotated;

	// Token: 0x040030A1 RID: 12449
	[Range(0f, 4000f)]
	public float speed;

	// Token: 0x040030A2 RID: 12450
	[SerializeField]
	public float offset;

	// Token: 0x040030A3 RID: 12451
	[SerializeField]
	[Range(1f, 10f)]
	public int count = 1;

	// Token: 0x040030A4 RID: 12452
	[NonSerialized]
	public float playbackSpeed = 1f;

	// Token: 0x040030A5 RID: 12453
	public float size;

	// Token: 0x040030A6 RID: 12454
	public Vector3 pos;

	// Token: 0x040030A7 RID: 12455
	public float startY;

	// Token: 0x040030A8 RID: 12456
	public int direction;

	// Token: 0x0200123D RID: 4669
	public enum Axis
	{
		// Token: 0x04007E65 RID: 32357
		X,
		// Token: 0x04007E66 RID: 32358
		Y
	}
}
