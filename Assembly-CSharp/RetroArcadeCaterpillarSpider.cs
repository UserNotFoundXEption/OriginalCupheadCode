using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000305 RID: 773
public class RetroArcadeCaterpillarSpider : RetroArcadeEnemy
{
	// Token: 0x0600224C RID: 8780 RVA: 0x000BD078 File Offset: 0x000BB278
	public RetroArcadeCaterpillarSpider Create(RetroArcadeCaterpillarSpider.Direction direction, LevelProperties.RetroArcade.Caterpillar properties)
	{
		RetroArcadeCaterpillarSpider retroArcadeCaterpillarSpider = this.InstantiatePrefab<RetroArcadeCaterpillarSpider>();
		retroArcadeCaterpillarSpider.transform.SetPosition(new float?((direction != RetroArcadeCaterpillarSpider.Direction.Right) ? 320f : -320f), new float?(300f), null);
		retroArcadeCaterpillarSpider.direction = direction;
		retroArcadeCaterpillarSpider.properties = properties;
		retroArcadeCaterpillarSpider.targetPos = new Vector2(retroArcadeCaterpillarSpider.transform.position.x, properties.spiderPathY.max);
		retroArcadeCaterpillarSpider.state = RetroArcadeCaterpillarSpider.State.Entering;
		retroArcadeCaterpillarSpider.hp = 1f;
		return retroArcadeCaterpillarSpider;
	}

	// Token: 0x0600224D RID: 8781 RVA: 0x000BD110 File Offset: 0x000BB310
	public override void FixedUpdate()
	{
		if (base.IsDead)
		{
			return;
		}
		float num = this.properties.spiderSpeed * CupheadTime.FixedDelta;
		float magnitude = (this.targetPos - base.transform.position).magnitude;
		if (magnitude > num)
		{
			this.move(num);
		}
		else
		{
			base.transform.position = this.targetPos;
			switch (this.state)
			{
			case RetroArcadeCaterpillarSpider.State.Entering:
				this.state = RetroArcadeCaterpillarSpider.State.ZigZagDown;
				break;
			case RetroArcadeCaterpillarSpider.State.ZigZagDown:
			case RetroArcadeCaterpillarSpider.State.ZigZagUp:
				if (this.numZigZags >= this.properties.spiderNumZigZags)
				{
					this.state = RetroArcadeCaterpillarSpider.State.Leaving;
				}
				else
				{
					this.state = ((this.state != RetroArcadeCaterpillarSpider.State.ZigZagUp) ? RetroArcadeCaterpillarSpider.State.ZigZagUp : RetroArcadeCaterpillarSpider.State.ZigZagDown);
					this.numZigZags++;
				}
				break;
			case RetroArcadeCaterpillarSpider.State.Leaving:
				Object.Destroy(base.gameObject);
				return;
			}
			RetroArcadeCaterpillarSpider.State state = this.state;
			if (state != RetroArcadeCaterpillarSpider.State.ZigZagUp && state != RetroArcadeCaterpillarSpider.State.ZigZagDown)
			{
				if (state == RetroArcadeCaterpillarSpider.State.Leaving)
				{
					this.targetPos.y = 300f;
				}
			}
			else
			{
				this.targetPos.x = (float)((this.direction != RetroArcadeCaterpillarSpider.Direction.Right) ? -1 : 1) * Mathf.Lerp(-320f, 320f, (float)this.numZigZags / (float)this.properties.spiderNumZigZags);
				this.targetPos.y = ((this.state != RetroArcadeCaterpillarSpider.State.ZigZagUp) ? this.properties.spiderPathY.min : this.properties.spiderPathY.max);
			}
			this.move(num - magnitude);
		}
	}

	// Token: 0x0600224E RID: 8782 RVA: 0x000BD2D8 File Offset: 0x000BB4D8
	public void move(float distance)
	{
		base.transform.position = base.transform.position + (this.targetPos - base.transform.position).normalized * distance;
	}

	// Token: 0x0600224F RID: 8783 RVA: 0x0001D3C8 File Offset: 0x0001B5C8
	public override void Dead()
	{
		base.Dead();
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x06002250 RID: 8784 RVA: 0x000BD334 File Offset: 0x000BB534
	public IEnumerator moveOffscreen_cr()
	{
		base.MoveY(300f - base.transform.position.y, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04001C47 RID: 7239
	public const float OFFSCREEN_Y = 300f;

	// Token: 0x04001C48 RID: 7240
	public const float MOVE_OFFSCREEN_SPEED = 500f;

	// Token: 0x04001C49 RID: 7241
	public const float MAX_X = 320f;

	// Token: 0x04001C4A RID: 7242
	public LevelProperties.RetroArcade.Caterpillar properties;

	// Token: 0x04001C4B RID: 7243
	public RetroArcadeCaterpillarSpider.Direction direction;

	// Token: 0x04001C4C RID: 7244
	public Vector2 targetPos;

	// Token: 0x04001C4D RID: 7245
	public RetroArcadeCaterpillarSpider.State state;

	// Token: 0x04001C4E RID: 7246
	public int numZigZags;

	// Token: 0x02000E2D RID: 3629
	public enum Direction
	{
		// Token: 0x0400667D RID: 26237
		Left,
		// Token: 0x0400667E RID: 26238
		Right
	}

	// Token: 0x02000E2E RID: 3630
	public enum State
	{
		// Token: 0x04006680 RID: 26240
		Entering,
		// Token: 0x04006681 RID: 26241
		ZigZagDown,
		// Token: 0x04006682 RID: 26242
		ZigZagUp,
		// Token: 0x04006683 RID: 26243
		Leaving
	}
}
