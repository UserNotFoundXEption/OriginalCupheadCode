using System;
using UnityEngine;

// Token: 0x02000155 RID: 341
public class BaronessLevelBackgroundChange : AbstractPausableComponent
{
	// Token: 0x0600106E RID: 4206 RVA: 0x000901FC File Offset: 0x0008E3FC
	public override void Awake()
	{
		base.Awake();
		SpriteRenderer component = base.transform.GetComponent<SpriteRenderer>();
		this.size = ((this.b_axis != BaronessLevelBackgroundChange.B_Axis.X) ? ((int)component.sprite.bounds.size.y) : ((int)component.sprite.bounds.size.x)) - this.b_offset;
		this.getOffset.x = base.transform.position.x;
		for (int i = 0; i < this.b_count; i++)
		{
			this.copy = new GameObject(base.gameObject.name + " Copy");
			this.copy.transform.parent = base.transform;
			this.copy.transform.ResetLocalTransforms();
			SpriteRenderer spriteRenderer = this.copy.AddComponent<SpriteRenderer>();
			spriteRenderer.sortingLayerID = component.sortingLayerID;
			spriteRenderer.sortingOrder = component.sortingOrder;
			spriteRenderer.sprite = component.sprite;
			spriteRenderer.material = component.material;
			GameObject gameObject = Object.Instantiate<GameObject>(this.copy);
			gameObject.transform.parent = base.transform;
			this.copy.transform.SetLocalPosition(new float?((float)(-(float)(this.size + this.size * i))), new float?(0f), new float?(0f));
			gameObject.transform.SetLocalPosition(new float?((float)(-(float)(this.size * 2 + this.size * i))), new float?(0f), new float?(0f));
		}
	}

	// Token: 0x0600106F RID: 4207 RVA: 0x0000DDA6 File Offset: 0x0000BFA6
	public void OnEnable()
	{
		if (!this.isClouds && this.sprite != null)
		{
			this.sprite.GetComponent<OneTimeScrollingSprite>().OutCondition = (() => this.baroness.state == BaronessLevelCastle.State.Dead);
		}
	}

	// Token: 0x06001070 RID: 4208 RVA: 0x0000DDE0 File Offset: 0x0000BFE0
	public void OnDisable()
	{
		if (this.sprite != null)
		{
			this.sprite.GetComponent<OneTimeScrollingSprite>().OutCondition = null;
		}
	}

	// Token: 0x06001071 RID: 4209 RVA: 0x000903C8 File Offset: 0x0008E5C8
	public void Update()
	{
		if (!this.isClouds && this.baroness.state != BaronessLevelCastle.State.Chase)
		{
			return;
		}
		if (!this.baroness.pauseScrolling)
		{
			if (base.GetComponent<ParallaxLayer>() != null)
			{
				base.GetComponent<ParallaxLayer>().enabled = false;
			}
			if (this.sprite != null)
			{
				this.sprite.speed = -this.speed;
			}
			Vector3 localPosition = base.transform.localPosition;
			if (localPosition.x >= -((float)this.size - this.getOffset.x))
			{
				localPosition.x -= (float)this.size;
			}
			if (localPosition.x <= (float)this.size - this.getOffset.x)
			{
				localPosition.x += (float)this.size;
			}
			localPosition.x -= (float)((!this.b_negativeDirection) ? 1 : -1) * this.speed * CupheadTime.Delta * this.b_playbackSpeed;
			base.transform.localPosition = localPosition;
		}
		else if (this.sprite != null)
		{
			this.sprite.GetComponent<OneTimeScrollingSprite>().speed = 0f;
		}
	}

	// Token: 0x04000D5B RID: 3419
	public BaronessLevelBackgroundChange.B_Axis b_axis;

	// Token: 0x04000D5C RID: 3420
	public int size;

	// Token: 0x04000D5D RID: 3421
	public const float X_OUT = -1280f;

	// Token: 0x04000D5E RID: 3422
	[Range(0f, 2000f)]
	public float speed;

	// Token: 0x04000D5F RID: 3423
	[SerializeField]
	public bool isClouds;

	// Token: 0x04000D60 RID: 3424
	[SerializeField]
	public bool b_negativeDirection;

	// Token: 0x04000D61 RID: 3425
	[SerializeField]
	public int b_offset;

	// Token: 0x04000D62 RID: 3426
	[SerializeField]
	[Range(1f, 10f)]
	public int b_count = 1;

	// Token: 0x04000D63 RID: 3427
	[SerializeField]
	public BaronessLevelCastle baroness;

	// Token: 0x04000D64 RID: 3428
	[SerializeField]
	public OneTimeScrollingSprite sprite;

	// Token: 0x04000D65 RID: 3429
	[NonSerialized]
	public float b_playbackSpeed = 1f;

	// Token: 0x04000D66 RID: 3430
	public GameObject copy;

	// Token: 0x04000D67 RID: 3431
	public Vector3 getOffset;

	// Token: 0x02000A42 RID: 2626
	public enum B_Axis
	{
		// Token: 0x04004B95 RID: 19349
		X,
		// Token: 0x04004B96 RID: 19350
		Y
	}
}
