using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000315 RID: 789
public class RetroArcadeSheriff : RetroArcadeEnemy
{
	// Token: 0x060022BD RID: 8893 RVA: 0x000BE804 File Offset: 0x000BCA04
	public RetroArcadeSheriff Create(Vector3 pos, float speed, bool clockwise, float offset, LevelProperties.RetroArcade.Sheriff properties)
	{
		RetroArcadeSheriff retroArcadeSheriff = this.InstantiatePrefab<RetroArcadeSheriff>();
		retroArcadeSheriff.transform.position = pos;
		retroArcadeSheriff.properties = properties;
		retroArcadeSheriff.speed = speed;
		retroArcadeSheriff.clockwise = clockwise;
		retroArcadeSheriff.offset = offset;
		return retroArcadeSheriff;
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x0001D89F File Offset: 0x0001BA9F
	public override void Start()
	{
		this.side = RetroArcadeSheriff.Side.Right;
		this.SelectDirection();
	}

	// Token: 0x060022BF RID: 8895 RVA: 0x000BE844 File Offset: 0x000BCA44
	public void SelectDirection()
	{
		if (base.transform.position.y > 200f)
		{
			this.side = RetroArcadeSheriff.Side.Top;
			this.direction = ((!this.clockwise) ? Vector3.left : Vector3.right);
		}
		else if (base.transform.position.y < -100f)
		{
			this.side = RetroArcadeSheriff.Side.Bottom;
			this.direction = ((!this.clockwise) ? Vector3.right : Vector3.left);
		}
		else if (base.transform.position.x < 0f)
		{
			this.side = RetroArcadeSheriff.Side.Left;
			this.direction = ((!this.clockwise) ? Vector3.down : Vector3.up);
		}
		else
		{
			this.side = RetroArcadeSheriff.Side.Right;
			this.direction = ((!this.clockwise) ? Vector3.up : Vector3.down);
		}
	}

	// Token: 0x060022C0 RID: 8896 RVA: 0x0001D8AE File Offset: 0x0001BAAE
	public void StartMoving()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060022C1 RID: 8897 RVA: 0x000BE954 File Offset: 0x000BCB54
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			switch (this.side)
			{
			case RetroArcadeSheriff.Side.Top:
				if (this.clockwise && base.transform.position.x >= (float)Level.Current.Right - this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Right;
					this.direction = Vector3.down;
				}
				else if (!this.clockwise && base.transform.position.x <= (float)Level.Current.Left + this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Left;
					this.direction = Vector3.down;
				}
				break;
			case RetroArcadeSheriff.Side.Bottom:
				if (this.clockwise && base.transform.position.x <= (float)Level.Current.Left + this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Left;
					this.direction = Vector3.up;
				}
				else if (!this.clockwise && base.transform.position.x >= (float)Level.Current.Right - this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Right;
					this.direction = Vector3.up;
				}
				break;
			case RetroArcadeSheriff.Side.Left:
				if (this.clockwise && base.transform.position.y >= (float)Level.Current.Ceiling - this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Top;
					this.direction = Vector3.right;
				}
				else if (!this.clockwise && base.transform.position.y <= (float)Level.Current.Ground + this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Bottom;
					this.direction = Vector3.right;
				}
				break;
			case RetroArcadeSheriff.Side.Right:
				if (!this.clockwise && base.transform.position.y >= (float)Level.Current.Ceiling - this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Top;
					this.direction = Vector3.left;
				}
				else if (this.clockwise && base.transform.position.y <= (float)Level.Current.Ground + this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Bottom;
					this.direction = Vector3.left;
				}
				break;
			}
			Vector3 pos = base.transform.position;
			pos.x = Mathf.Clamp(base.transform.position.x, (float)Level.Current.Left + this.offset, (float)Level.Current.Right - this.offset);
			pos.y = Mathf.Clamp(base.transform.position.y, (float)Level.Current.Ground + this.offset, (float)Level.Current.Ceiling - this.offset);
			base.transform.position = pos;
			base.transform.position += this.direction * this.speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060022C2 RID: 8898 RVA: 0x000BE970 File Offset: 0x000BCB70
	public void Shoot(AbstractPlayerController player)
	{
		Vector3 vector = player.transform.position - base.transform.position;
		this.projectile.Create(base.transform.position, MathUtils.DirectionToAngle(vector), this.properties.shotSpeed);
	}

	// Token: 0x04001CB8 RID: 7352
	public float speed;

	// Token: 0x04001CB9 RID: 7353
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x04001CBA RID: 7354
	public const float Y_TOP_THRESHOLD = 200f;

	// Token: 0x04001CBB RID: 7355
	public const float Y_BOTTOM_THRESHOLD = -100f;

	// Token: 0x04001CBC RID: 7356
	public const float X_THRESHOLD = 290f;

	// Token: 0x04001CBD RID: 7357
	public RetroArcadeSheriff.Side side;

	// Token: 0x04001CBE RID: 7358
	public LevelProperties.RetroArcade.Sheriff properties;

	// Token: 0x04001CBF RID: 7359
	public float offset;

	// Token: 0x04001CC0 RID: 7360
	public bool clockwise;

	// Token: 0x04001CC1 RID: 7361
	public Vector3 direction;

	// Token: 0x04001CC2 RID: 7362
	public Vector2 targetPos;

	// Token: 0x02000E44 RID: 3652
	public enum Side
	{
		// Token: 0x04006712 RID: 26386
		Top,
		// Token: 0x04006713 RID: 26387
		Bottom,
		// Token: 0x04006714 RID: 26388
		Left,
		// Token: 0x04006715 RID: 26389
		Right
	}
}
