using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200016F RID: 367
public class BeeLevelTurbineBullet : AbstractProjectile
{
	// Token: 0x060011A7 RID: 4519 RVA: 0x00092C78 File Offset: 0x00090E78
	public BeeLevelTurbineBullet Create(Vector2 pos, float rotation, bool onRight, LevelProperties.Bee.TurbineBlasters properties)
	{
		BeeLevelTurbineBullet beeLevelTurbineBullet = base.Create() as BeeLevelTurbineBullet;
		beeLevelTurbineBullet.properties = properties;
		beeLevelTurbineBullet.transform.position = pos;
		beeLevelTurbineBullet.onRight = onRight;
		beeLevelTurbineBullet.direction = MathUtils.AngleToDirection(rotation);
		beeLevelTurbineBullet.velocity = properties.bulletSpeed;
		beeLevelTurbineBullet.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(rotation));
		beeLevelTurbineBullet.sprite.flipX = onRight;
		return beeLevelTurbineBullet;
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x0000EF7E File Offset: 0x0000D17E
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.trail_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060011A9 RID: 4521 RVA: 0x0000EFA0 File Offset: 0x0000D1A0
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060011AA RID: 4522 RVA: 0x0000EFBE File Offset: 0x0000D1BE
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060011AB RID: 4523 RVA: 0x00092D04 File Offset: 0x00090F04
	public IEnumerator move_cr()
	{
		while (base.transform.position.y < 360f - this.loopSizeY)
		{
			base.transform.position += this.direction * this.velocity * CupheadTime.Delta;
			yield return null;
		}
		base.StartCoroutine(this.move_in_circle_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060011AC RID: 4524 RVA: 0x00092D20 File Offset: 0x00090F20
	public IEnumerator move_in_circle_cr()
	{
		this.pivotPoint = base.transform.position + Vector3.right * ((!this.onRight) ? this.loopSizeX : (-this.loopSizeX));
		Vector3 handleRotationX = Vector3.zero;
		float offset = 100f;
		this.circleAngle -= 1.57079637f;
		float endPos;
		float endVelocity;
		float rotateInCir;
		if (this.onRight)
		{
			endPos = -640f - offset;
			endVelocity = -this.velocity;
			rotateInCir = -90f;
		}
		else
		{
			endPos = 640f + offset;
			endVelocity = this.velocity;
			rotateInCir = 90f;
		}
		while (this.circleAngle < 6.108652f)
		{
			this.circleAngle += this.properties.bulletCircleTime * CupheadTime.Delta;
			if (this.onRight)
			{
				handleRotationX = new Vector3(-Mathf.Sin(this.circleAngle) * this.loopSizeX, 0f, 0f);
			}
			else
			{
				handleRotationX = new Vector3(Mathf.Sin(this.circleAngle) * this.loopSizeX, 0f, 0f);
			}
			Vector3 handleRotationY = new Vector3(0f, Mathf.Cos(this.circleAngle) * this.loopSizeY, 0f);
			base.transform.position = this.pivotPoint;
			base.transform.position += handleRotationX + handleRotationY;
			Vector3 dir = this.pivotPoint - base.transform.position;
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(dir) + rotateInCir));
			yield return null;
		}
		while (base.transform.position.x != endPos)
		{
			base.transform.AddPosition(endVelocity * CupheadTime.Delta, 0f, 0f);
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060011AD RID: 4525 RVA: 0x00092D3C File Offset: 0x00090F3C
	public IEnumerator trail_cr()
	{
		for (;;)
		{
			this.trailPrefab.Create(base.transform.position);
			yield return CupheadTime.WaitForSeconds(this, 0.25f);
		}
		yield break;
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x0000EFDC File Offset: 0x0000D1DC
	public override void Die()
	{
		base.Die();
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
	}

	// Token: 0x04000E29 RID: 3625
	[SerializeField]
	public Effect trailPrefab;

	// Token: 0x04000E2A RID: 3626
	[SerializeField]
	public SpriteRenderer sprite;

	// Token: 0x04000E2B RID: 3627
	public LevelProperties.Bee.TurbineBlasters properties;

	// Token: 0x04000E2C RID: 3628
	public float velocity;

	// Token: 0x04000E2D RID: 3629
	public float circleAngle;

	// Token: 0x04000E2E RID: 3630
	public float loopSizeY = 200f;

	// Token: 0x04000E2F RID: 3631
	public float loopSizeX = 500f;

	// Token: 0x04000E30 RID: 3632
	public bool onRight;

	// Token: 0x04000E31 RID: 3633
	public Vector3 direction;

	// Token: 0x04000E32 RID: 3634
	public Vector3 pivotPoint;
}
