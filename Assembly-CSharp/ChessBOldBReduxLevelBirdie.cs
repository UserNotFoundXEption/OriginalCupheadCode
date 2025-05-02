using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000174 RID: 372
public class ChessBOldBReduxLevelBirdie : AbstractProjectile
{
	// Token: 0x17000245 RID: 581
	// (get) Token: 0x060011D9 RID: 4569 RVA: 0x0000F1E5 File Offset: 0x0000D3E5
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060011DA RID: 4570 RVA: 0x000935C8 File Offset: 0x000917C8
	public void Setup(Transform pivotPoint, float angle, LevelProperties.ChessBOldB.Birdie properties, float loopSize, bool chosenBall)
	{
		this.angle = angle;
		this.pivotPoint = pivotPoint;
		this.properties = properties;
		this.loopSize = loopSize;
		this.isMoving = false;
		this.chosenBall = chosenBall;
		this.RepositionBall();
		this.sprite.color = this.defaultColor;
		this.SetParryable(false);
	}

	// Token: 0x060011DB RID: 4571 RVA: 0x0000F1EC File Offset: 0x0000D3EC
	public override void Awake()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
		base.Awake();
	}

	// Token: 0x060011DC RID: 4572 RVA: 0x0000F200 File Offset: 0x0000D400
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060011DD RID: 4573 RVA: 0x0000F21E File Offset: 0x0000D41E
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.isMoving)
		{
			this.MoveBirdie();
		}
	}

	// Token: 0x060011DE RID: 4574 RVA: 0x0000F237 File Offset: 0x0000D437
	public void StopMoving()
	{
		this.isMoving = false;
	}

	// Token: 0x060011DF RID: 4575 RVA: 0x0000F240 File Offset: 0x0000D440
	public void HandleMovement(float rotationTime, bool goingClockwise)
	{
		this.rotationTime = ((!goingClockwise) ? (-rotationTime) : rotationTime);
		this.isMoving = true;
	}

	// Token: 0x060011E0 RID: 4576 RVA: 0x00093620 File Offset: 0x00091820
	public void RepositionBall()
	{
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		this.angle *= 0.0174532924f;
		zero..ctor(Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
		zero2..ctor(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
		base.transform.position = this.pivotPoint.position;
		base.transform.position += zero + zero2;
		this.offScreen = false;
		this.damageDealer.SetDamageFlags(false, false, false);
	}

	// Token: 0x060011E1 RID: 4577 RVA: 0x000936E4 File Offset: 0x000918E4
	public void MoveBirdie()
	{
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		this.angle += this.rotationTime * CupheadTime.FixedDelta;
		zero..ctor(Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
		zero2..ctor(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
		base.transform.position = this.pivotPoint.position;
		base.transform.position += zero + zero2;
	}

	// Token: 0x060011E2 RID: 4578 RVA: 0x0009379C File Offset: 0x0009199C
	public override void OnParry(AbstractPlayerController player)
	{
		if (this.ParryBirdie != null)
		{
			this.isMoving = false;
			this.ParryBirdie(this.chosenBall);
			base.StartCoroutine(this.turn_off_collider_cr());
			if (!this.chosenBall)
			{
				base.StartCoroutine(this.attack_cr());
			}
		}
	}

	// Token: 0x060011E3 RID: 4579 RVA: 0x0000F25D File Offset: 0x0000D45D
	public void TurnPink()
	{
		this.SetParryable(true);
		this.sprite.color = this.pinkColor;
	}

	// Token: 0x060011E4 RID: 4580 RVA: 0x000937F4 File Offset: 0x000919F4
	public IEnumerator turn_off_collider_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, this.properties.colliderOffTime);
		base.GetComponent<Collider2D>().enabled = true;
		this.offScreen = false;
		this.damageDealer.SetDamageFlags(true, false, false);
		yield break;
	}

	// Token: 0x060011E5 RID: 4581 RVA: 0x00093810 File Offset: 0x00091A10
	public IEnumerator attack_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		AbstractPlayerController player = PlayerManager.GetNext();
		Vector3 dir = player.transform.position - base.transform.position;
		float angle = MathUtils.DirectionToAngle(dir);
		bool changedDirection = false;
		float straightTime = 0f;
		float timeToStraight = this.properties.timeToStraight;
		float arcTime = 0f;
		float timeToArc = this.properties.timeToMaxSpeed;
		MinMax xSpeedMinMax = this.properties.xSpeed;
		MinMax ySpeedMinMax = this.properties.ySpeed;
		float xSpeed = 0f;
		float ySpeed = 0f;
		base.StartCoroutine(this.check_bounds_cr());
		while (!this.offScreen)
		{
			if (arcTime < timeToArc)
			{
				arcTime += CupheadTime.FixedDelta;
				xSpeed = xSpeedMinMax.GetFloatAt(arcTime / timeToArc);
				ySpeed = ySpeedMinMax.GetFloatAt(1f - arcTime / timeToArc);
			}
			if (xSpeed > 0f && !changedDirection)
			{
				if (straightTime < timeToStraight)
				{
					straightTime += CupheadTime.FixedDelta;
					dir = player.transform.position - base.transform.position;
					angle = MathUtils.DirectionToAngle(dir);
				}
				else
				{
					changedDirection = true;
				}
			}
			Vector3 speed = new Vector3(xSpeed, ySpeed);
			Quaternion rot = Quaternion.Euler(0f, 0f, angle);
			speed = rot * speed;
			base.transform.position += speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x0009382C File Offset: 0x00091A2C
	public IEnumerator check_bounds_cr()
	{
		float offset = 200f;
		while (base.transform.position.x < (float)Level.Current.Right + offset && base.transform.position.x > (float)Level.Current.Left - offset && base.transform.position.y < (float)Level.Current.Ceiling + offset && base.transform.position.y > (float)Level.Current.Ground - offset)
		{
			yield return null;
		}
		this.offScreen = true;
		yield return null;
		yield break;
	}

	// Token: 0x04000E55 RID: 3669
	public const float ONE = 1f;

	// Token: 0x04000E56 RID: 3670
	public ChessBOldBReduxLevelBirdie.OnParryBirdie ParryBirdie;

	// Token: 0x04000E57 RID: 3671
	[SerializeField]
	public Color defaultColor;

	// Token: 0x04000E58 RID: 3672
	[SerializeField]
	public Color pinkColor;

	// Token: 0x04000E59 RID: 3673
	public LevelProperties.ChessBOldB.Birdie properties;

	// Token: 0x04000E5A RID: 3674
	public SpriteRenderer sprite;

	// Token: 0x04000E5B RID: 3675
	public Transform pivotPoint;

	// Token: 0x04000E5C RID: 3676
	public int timesToChangeDir;

	// Token: 0x04000E5D RID: 3677
	public float angle;

	// Token: 0x04000E5E RID: 3678
	public float loopSize;

	// Token: 0x04000E5F RID: 3679
	public float rotationTime;

	// Token: 0x04000E60 RID: 3680
	public bool isMoving;

	// Token: 0x04000E61 RID: 3681
	public bool chosenBall;

	// Token: 0x04000E62 RID: 3682
	public bool offScreen;

	// Token: 0x02000A9B RID: 2715
	// (Invoke) Token: 0x06005AE5 RID: 23269
	public delegate void OnParryBirdie(bool correctBall);
}
