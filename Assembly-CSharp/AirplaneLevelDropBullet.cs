using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class AirplaneLevelDropBullet : AbstractProjectile
{
	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06000E08 RID: 3592 RVA: 0x0000BF6E File Offset: 0x0000A16E
	// (set) Token: 0x06000E09 RID: 3593 RVA: 0x0000BF76 File Offset: 0x0000A176
	public bool isMoving { get; set; }

	// Token: 0x06000E0A RID: 3594 RVA: 0x00089038 File Offset: 0x00087238
	public virtual AirplaneLevelDropBullet Init(Vector3 targetPos, Vector3 startPos, float dropSpeed, float shootSpeed, bool onLeft, bool camHorizontal)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = startPos;
		this.YtoSwitch = targetPos.y;
		this.shootSpeed = shootSpeed;
		this.onLeft = onLeft;
		base.transform.SetScale(new float?((float)((!onLeft) ? -1 : 1)), null, null);
		this.rend.sortingOrder = ((!onLeft) ? 501 : 500);
		this.targetPos = targetPos;
		this.bounds = ((!camHorizontal) ? CupheadLevelCamera.Current.Bounds.xMax : CupheadLevelCamera.Current.Bounds.yMax);
		this.dropSpeed = dropSpeed;
		this.moveDir = ((!onLeft) ? Vector3.right : Vector3.left);
		this.goingDown = true;
		this.isMoving = true;
		this.boxColl.enabled = false;
		this.circColl.enabled = true;
		this.t = 0.7853982f;
		this.startPos = startPos + Vector3.down * (Mathf.Sin(this.t) * 600f);
		return this;
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x0000BF7F File Offset: 0x0000A17F
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.rotate_cr());
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x00089188 File Offset: 0x00087388
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.goingDown)
		{
			this.t += CupheadTime.FixedDelta * this.dropSpeed;
			if (this.t < 3.14159274f)
			{
				base.transform.position = new Vector3(EaseUtils.EaseOutSine(this.startPos.x, this.targetPos.x, Mathf.InverseLerp(0.7853982f, 3.14159274f, this.t)), this.startPos.y + Mathf.Sin(this.t) * 600f);
			}
			else
			{
				base.transform.position += Vector3.up * (Mathf.Sin(3.14159274f) - Mathf.Sin(3.14159274f - CupheadTime.FixedDelta * this.dropSpeed)) * 600f;
			}
			if (base.transform.position.y < this.YtoSwitch)
			{
				base.transform.position = new Vector3(base.transform.position.x, this.YtoSwitch);
				this.moveDir = ((!this.onLeft) ? Vector3.left : Vector3.right);
				this.dropSpeed = this.shootSpeed;
				this.goingDown = false;
				base.animator.SetTrigger("ToShoot");
				this.boxColl.enabled = true;
				this.circColl.enabled = false;
				this.shootFX.Create(base.transform.position, base.transform.localScale);
				this.t = 0f;
			}
		}
		else
		{
			this.t += CupheadTime.FixedDelta;
			if (this.t > 0.333333343f)
			{
				this.speedLines.enabled = false;
			}
			base.transform.position += this.moveDir * this.dropSpeed * CupheadTime.FixedDelta;
			if (base.transform.position.x < -this.bounds - 100f || base.transform.position.x > this.bounds + 100f)
			{
				this.isMoving = false;
				this.Recycle<AirplaneLevelDropBullet>();
			}
		}
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x0000BF94 File Offset: 0x0000A194
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x00089408 File Offset: 0x00087608
	public IEnumerator rotate_cr()
	{
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.0416666679f, false);
		float startRotation = 360f;
		float rotateSpeed = 1f;
		float rotateTime = 0f;
		while (this.goingDown)
		{
			base.transform.SetEulerAngles(null, null, new float?(startRotation * rotateTime));
			rotateTime += CupheadTime.FixedDelta * rotateSpeed;
			yield return wait;
		}
		base.transform.SetEulerAngles(null, null, new float?(startRotation));
		yield return null;
		yield break;
	}

	// Token: 0x04000B16 RID: 2838
	public const float SPAWN_OFFSET = 100f;

	// Token: 0x04000B17 RID: 2839
	public const float ARC_HEIGHT = 600f;

	// Token: 0x04000B19 RID: 2841
	public Vector3 moveDir;

	// Token: 0x04000B1A RID: 2842
	public Vector3 startPos;

	// Token: 0x04000B1B RID: 2843
	public Vector3 targetPos;

	// Token: 0x04000B1C RID: 2844
	public float shootSpeed;

	// Token: 0x04000B1D RID: 2845
	public float dropSpeed;

	// Token: 0x04000B1E RID: 2846
	public float YtoSwitch;

	// Token: 0x04000B1F RID: 2847
	public float bounds;

	// Token: 0x04000B20 RID: 2848
	public bool goingDown;

	// Token: 0x04000B21 RID: 2849
	public bool onLeft;

	// Token: 0x04000B22 RID: 2850
	[SerializeField]
	public CircleCollider2D circColl;

	// Token: 0x04000B23 RID: 2851
	[SerializeField]
	public BoxCollider2D boxColl;

	// Token: 0x04000B24 RID: 2852
	[SerializeField]
	public Effect shootFX;

	// Token: 0x04000B25 RID: 2853
	[SerializeField]
	public SpriteRenderer speedLines;

	// Token: 0x04000B26 RID: 2854
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04000B27 RID: 2855
	public float t;
}
