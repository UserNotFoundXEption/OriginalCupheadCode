using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class GroundHomingMovement : AbstractPausableComponent
{
	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000A16 RID: 2582 RVA: 0x00009352 File Offset: 0x00007552
	// (set) Token: 0x06000A17 RID: 2583 RVA: 0x0000935A File Offset: 0x0000755A
	public AbstractPlayerController TrackingPlayer { get; set; }

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06000A18 RID: 2584 RVA: 0x00009363 File Offset: 0x00007563
	// (set) Token: 0x06000A19 RID: 2585 RVA: 0x0000936B File Offset: 0x0000756B
	public bool EnableHoming { get; set; }

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000A1A RID: 2586 RVA: 0x00009374 File Offset: 0x00007574
	// (set) Token: 0x06000A1B RID: 2587 RVA: 0x0000937C File Offset: 0x0000757C
	public GroundHomingMovement.Direction MoveDirection { get; set; }

	// Token: 0x06000A1C RID: 2588 RVA: 0x00009385 File Offset: 0x00007585
	public override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.loop_cr());
		if (this.startOnAwake)
		{
			this.EnableHoming = true;
		}
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0007AD30 File Offset: 0x00078F30
	public float hitPauseCoefficient()
	{
		DamageReceiver component = base.GetComponent<DamageReceiver>();
		if (component == null)
		{
			return 1f;
		}
		return (!component.IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x0007AD70 File Offset: 0x00078F70
	public IEnumerator loop_cr()
	{
		Quaternion radishRot = base.transform.localRotation;
		while (this.TrackingPlayer == null)
		{
			yield return null;
		}
		for (;;)
		{
			if (!base.enabled)
			{
				yield return null;
			}
			else
			{
				if (this.TrackingPlayer == null || this.TrackingPlayer.IsDead)
				{
					this.TrackingPlayer = PlayerManager.GetNext();
				}
				if (this.EnableHoming)
				{
					if (this.TrackingPlayer.transform.position.x > base.transform.position.x)
					{
						this.MoveDirection = GroundHomingMovement.Direction.Right;
						if (radishRot.z < 0.05235988f)
						{
							radishRot.z += 0.01f;
						}
					}
					else
					{
						this.MoveDirection = GroundHomingMovement.Direction.Left;
						if (radishRot.z > -0.05235988f)
						{
							radishRot.z -= 0.01f;
						}
					}
				}
				if (this.MoveDirection == GroundHomingMovement.Direction.Right)
				{
					this.velocityX += this.acceleration * CupheadTime.Delta * this.hitPauseCoefficient();
				}
				else
				{
					this.velocityX -= this.acceleration * CupheadTime.Delta * this.hitPauseCoefficient();
				}
				this.velocityX = Mathf.Clamp(this.velocityX, -this.maxSpeed, this.maxSpeed);
				Vector2 position = base.transform.localPosition;
				position.x += this.velocityX * CupheadTime.Delta * this.hitPauseCoefficient();
				if (this.bounceEnabled)
				{
					if (position.x < (float)Level.Current.Left + this.leftPadding)
					{
						position.x = (float)Level.Current.Left + this.leftPadding;
						this.velocityX *= -this.bounceRatio;
					}
					if (position.x > (float)Level.Current.Right - this.rightPadding)
					{
						position.x = (float)Level.Current.Right - this.rightPadding;
						this.velocityX *= -this.bounceRatio;
					}
				}
				if (this.destroyOffScreen)
				{
					SpriteRenderer component = base.GetComponent<SpriteRenderer>();
					if (position.x < (float)Level.Current.Left - component.bounds.size.x / 2f || position.x > (float)Level.Current.Right + component.bounds.size.x / 2f)
					{
						Object.Destroy(base.gameObject);
					}
				}
				base.transform.localPosition = position;
				if (this.enableRadishRot)
				{
					base.transform.localRotation = radishRot;
				}
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0400080F RID: 2063
	public bool startOnAwake;

	// Token: 0x04000810 RID: 2064
	public float maxSpeed;

	// Token: 0x04000811 RID: 2065
	public float acceleration;

	// Token: 0x04000812 RID: 2066
	public float bounceRatio;

	// Token: 0x04000813 RID: 2067
	public bool bounceEnabled;

	// Token: 0x04000814 RID: 2068
	public float leftPadding;

	// Token: 0x04000815 RID: 2069
	public float rightPadding;

	// Token: 0x04000816 RID: 2070
	public bool destroyOffScreen;

	// Token: 0x04000817 RID: 2071
	public bool enableRadishRot;

	// Token: 0x04000818 RID: 2072
	public float velocityX;

	// Token: 0x02000947 RID: 2375
	public enum Direction
	{
		// Token: 0x040045DC RID: 17884
		Left,
		// Token: 0x040045DD RID: 17885
		Right
	}
}
