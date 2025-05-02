using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000303 RID: 771
public class RetroArcadeCaterpillarBodyPart : RetroArcadeEnemy
{
	// Token: 0x0600223D RID: 8765 RVA: 0x000BCB28 File Offset: 0x000BAD28
	public RetroArcadeCaterpillarBodyPart Create(int index, RetroArcadeCaterpillarBodyPart.Direction direction, RetroArcadeCaterpillarManager manager, LevelProperties.RetroArcade.Caterpillar properties)
	{
		RetroArcadeCaterpillarBodyPart retroArcadeCaterpillarBodyPart = this.InstantiatePrefab<RetroArcadeCaterpillarBodyPart>();
		retroArcadeCaterpillarBodyPart.transform.SetPosition(new float?((direction != RetroArcadeCaterpillarBodyPart.Direction.Right) ? 320f : -320f), new float?(300f + (float)index * 50f), null);
		retroArcadeCaterpillarBodyPart.direction = direction;
		retroArcadeCaterpillarBodyPart.manager = manager;
		retroArcadeCaterpillarBodyPart.properties = properties;
		retroArcadeCaterpillarBodyPart.manager = manager;
		retroArcadeCaterpillarBodyPart.targetPos = new Vector2(retroArcadeCaterpillarBodyPart.transform.position.x, 230f);
		retroArcadeCaterpillarBodyPart.moveY = true;
		retroArcadeCaterpillarBodyPart.hp = properties.hp;
		return retroArcadeCaterpillarBodyPart;
	}

	// Token: 0x0600223E RID: 8766 RVA: 0x0001D355 File Offset: 0x0001B555
	public override void Start()
	{
		base.PointsWorth = this.properties.pointsGained;
		base.PointsBonus = this.properties.pointsBonus;
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x000BCBD4 File Offset: 0x000BADD4
	public override void FixedUpdate()
	{
		if (this.movingY)
		{
			return;
		}
		float num = this.manager.moveSpeed * CupheadTime.FixedDelta;
		float magnitude = (this.targetPos - base.transform.position).magnitude;
		if (magnitude > num)
		{
			this.move(num);
		}
		else
		{
			base.transform.position = this.targetPos;
			if (this.moveY)
			{
				this.moveY = false;
				this.targetPos = new Vector2((this.direction != RetroArcadeCaterpillarBodyPart.Direction.Left) ? 320f : -320f, base.transform.position.y);
				if (this.atBottom && this.isHead)
				{
					this.manager.OnReachBottom();
				}
			}
			else
			{
				this.moveY = true;
				this.direction = ((this.direction != RetroArcadeCaterpillarBodyPart.Direction.Left) ? RetroArcadeCaterpillarBodyPart.Direction.Left : RetroArcadeCaterpillarBodyPart.Direction.Right);
				if (this.atBottom)
				{
					this.targetPos = new Vector2(base.transform.position.x, 230f);
					this.atBottom = false;
				}
				else if (this.timesDropped >= this.properties.dropCount)
				{
					this.targetPos = new Vector2(base.transform.position.x, -120f);
					this.timesDropped = 0;
					this.atBottom = true;
				}
				else
				{
					this.targetPos = new Vector2(base.transform.position.x, base.transform.position.y - 50f);
					this.timesDropped++;
					if (this.bulletPrefab != null)
					{
						this.Shoot();
					}
				}
			}
			this.move(num - magnitude);
		}
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x000BCDCC File Offset: 0x000BAFCC
	public void move(float distance)
	{
		base.transform.position = base.transform.position + (this.targetPos - base.transform.position).normalized * distance;
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x0001D379 File Offset: 0x0001B579
	public override void Dead()
	{
		base.Dead();
		if (!this.isHead)
		{
			this.manager.OnBodyPartDie(this);
		}
	}

	// Token: 0x06002242 RID: 8770 RVA: 0x000BCE28 File Offset: 0x000BB028
	public void Shoot()
	{
		float rotation = MathUtils.DirectionToAngle(PlayerManager.GetNext().transform.position - this.bulletRoot.position);
		this.bulletPrefab.Create(this.bulletRoot.position, rotation, this.properties.shotSpeed);
	}

	// Token: 0x06002243 RID: 8771 RVA: 0x0001D398 File Offset: 0x0001B598
	public void OnWaveEnd()
	{
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x06002244 RID: 8772 RVA: 0x000BCE88 File Offset: 0x000BB088
	public IEnumerator moveOffscreen_cr()
	{
		base.MoveY(420f, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04001C30 RID: 7216
	public const float TOP_Y = 230f;

	// Token: 0x04001C31 RID: 7217
	public const float BOTTOM_Y = -120f;

	// Token: 0x04001C32 RID: 7218
	public const float SPACING = 50f;

	// Token: 0x04001C33 RID: 7219
	public const float OFFSCREEN_Y = 300f;

	// Token: 0x04001C34 RID: 7220
	public const float MOVE_OFFSCREEN_SPEED = 500f;

	// Token: 0x04001C35 RID: 7221
	public const float TURNAROUND_X = 320f;

	// Token: 0x04001C36 RID: 7222
	[SerializeField]
	public BasicProjectile bulletPrefab;

	// Token: 0x04001C37 RID: 7223
	[SerializeField]
	public Transform bulletRoot;

	// Token: 0x04001C38 RID: 7224
	[SerializeField]
	public bool isHead;

	// Token: 0x04001C39 RID: 7225
	public LevelProperties.RetroArcade.Caterpillar properties;

	// Token: 0x04001C3A RID: 7226
	public RetroArcadeCaterpillarManager manager;

	// Token: 0x04001C3B RID: 7227
	public RetroArcadeCaterpillarBodyPart.Direction direction;

	// Token: 0x04001C3C RID: 7228
	public Vector2 targetPos;

	// Token: 0x04001C3D RID: 7229
	public bool moveY;

	// Token: 0x04001C3E RID: 7230
	public int timesDropped;

	// Token: 0x04001C3F RID: 7231
	public bool atBottom;

	// Token: 0x02000E2B RID: 3627
	public enum Direction
	{
		// Token: 0x04006676 RID: 26230
		Left,
		// Token: 0x04006677 RID: 26231
		Right
	}
}
