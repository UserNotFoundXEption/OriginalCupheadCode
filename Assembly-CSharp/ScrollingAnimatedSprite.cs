using System;
using UnityEngine;

// Token: 0x020005AB RID: 1451
public class ScrollingAnimatedSprite : AbstractPausableComponent
{
	// Token: 0x06003D0D RID: 15629 RVA: 0x00117A0C File Offset: 0x00115C0C
	public override void Awake()
	{
		base.Awake();
		if (ScrollingAnimatedSprite.copying)
		{
			return;
		}
		ScrollingAnimatedSprite.copying = true;
		SpriteRenderer component = base.transform.GetComponent<SpriteRenderer>();
		this.size = ((this.axis != ScrollingAnimatedSprite.Axis.X) ? ((int)component.sprite.bounds.size.y) : ((int)component.sprite.bounds.size.x)) - this.offset;
		for (int i = 0; i < this.count; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject);
			GameObject gameObject2 = Object.Instantiate<GameObject>(base.gameObject);
			gameObject.GetComponent<ScrollingAnimatedSprite>().enabled = false;
			gameObject2.GetComponent<ScrollingAnimatedSprite>().enabled = false;
			gameObject.transform.SetParent(base.transform);
			gameObject2.transform.SetParent(base.transform);
			if (this.axis == ScrollingAnimatedSprite.Axis.X)
			{
				gameObject.transform.SetLocalPosition(new float?((float)(this.size + this.size * i)), new float?(0f), new float?(0f));
				gameObject2.transform.SetLocalPosition(new float?((float)(-(float)(this.size + this.size * i))), new float?(0f), new float?(0f));
			}
			else
			{
				gameObject.transform.SetLocalPosition(new float?(0f), new float?((float)(this.size + this.size * i)), new float?(0f));
				gameObject2.transform.SetLocalPosition(new float?(0f), new float?((float)(-(float)(this.size + this.size * i))), new float?(0f));
			}
		}
		ScrollingAnimatedSprite.copying = false;
	}

	// Token: 0x06003D0E RID: 15630 RVA: 0x00117BF8 File Offset: 0x00115DF8
	public void Update()
	{
		Vector3 localPosition = base.transform.localPosition;
		if (this.axis == ScrollingAnimatedSprite.Axis.X)
		{
			if (localPosition.x <= (float)(-(float)this.size))
			{
				localPosition.x += (float)this.size;
			}
			if (localPosition.x >= (float)this.size)
			{
				localPosition.x -= (float)this.size;
			}
			localPosition.x -= (float)((!this.negativeDirection) ? 1 : -1) * this.speed * CupheadTime.Delta * this.playbackSpeed;
		}
		else
		{
			if (localPosition.y <= (float)(-(float)this.size))
			{
				localPosition.y += (float)this.size;
			}
			if (localPosition.y >= (float)this.size)
			{
				localPosition.y -= (float)this.size;
			}
			localPosition.y -= (float)((!this.negativeDirection) ? 1 : -1) * this.speed * CupheadTime.Delta * this.playbackSpeed;
		}
		base.transform.localPosition = localPosition;
	}

	// Token: 0x0400308B RID: 12427
	public ScrollingAnimatedSprite.Axis axis;

	// Token: 0x0400308C RID: 12428
	[SerializeField]
	public bool negativeDirection;

	// Token: 0x0400308D RID: 12429
	[SerializeField]
	[Range(0f, 2000f)]
	public float speed;

	// Token: 0x0400308E RID: 12430
	[SerializeField]
	public int offset;

	// Token: 0x0400308F RID: 12431
	[SerializeField]
	[Range(1f, 10f)]
	public int count = 1;

	// Token: 0x04003090 RID: 12432
	[NonSerialized]
	public float playbackSpeed = 1f;

	// Token: 0x04003091 RID: 12433
	public int size;

	// Token: 0x04003092 RID: 12434
	public static bool copying;

	// Token: 0x0200123A RID: 4666
	public enum Axis
	{
		// Token: 0x04007E57 RID: 32343
		X,
		// Token: 0x04007E58 RID: 32344
		Y
	}
}
