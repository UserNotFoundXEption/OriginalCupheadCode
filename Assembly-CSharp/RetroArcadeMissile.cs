using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000308 RID: 776
public class RetroArcadeMissile : AbstractCollidableObject
{
	// Token: 0x0600225E RID: 8798 RVA: 0x000BD4CC File Offset: 0x000BB6CC
	public void Init(Vector2 pos, float rotation, LevelProperties.RetroArcade.Missile properties, Vector3 pivot)
	{
		base.transform.position = pos;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(rotation));
		this.properties = properties;
		this.pivotPoint = pivot;
	}

	// Token: 0x0600225F RID: 8799 RVA: 0x0001D43B File Offset: 0x0001B63B
	public void Start()
	{
		this.loopXSize = this.properties.loopXSize;
		this.loopYSize = this.properties.loopYSize;
		this.damageDealer = DamageDealer.NewEnemy();
		this.Deactivate();
	}

	// Token: 0x06002260 RID: 8800 RVA: 0x0001D470 File Offset: 0x0001B670
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002261 RID: 8801 RVA: 0x0001D488 File Offset: 0x0001B688
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002262 RID: 8802 RVA: 0x0001D4A6 File Offset: 0x0001B6A6
	public void StartCircle(bool onRight, Vector3 pivot)
	{
		this.circleAngle = 0f;
		this.pivotPoint = pivot;
		base.GetComponent<SpriteRenderer>().enabled = true;
		base.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.move_in_circle_cr(onRight));
	}

	// Token: 0x06002263 RID: 8803 RVA: 0x000BD520 File Offset: 0x000BB720
	public IEnumerator move_in_circle_cr(bool onRight)
	{
		Vector3 handleRotationX = Vector3.zero;
		float rotateInCir;
		if (onRight)
		{
			rotateInCir = -90f;
		}
		else
		{
			rotateInCir = 90f;
		}
		while (this.circleAngle < 6.108652f)
		{
			this.circleAngle += 5f * CupheadTime.Delta;
			if (onRight)
			{
				handleRotationX = new Vector3(-Mathf.Sin(this.circleAngle) * this.loopXSize, 0f, 0f);
			}
			else
			{
				handleRotationX = new Vector3(Mathf.Sin(this.circleAngle) * this.loopXSize, 0f, 0f);
			}
			Vector3 handleRotationY = new Vector3(0f, Mathf.Cos(this.circleAngle) * this.loopYSize, 0f);
			base.transform.position = this.pivotPoint;
			base.transform.position += handleRotationX + handleRotationY;
			Vector3 dir = this.pivotPoint - base.transform.position;
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(dir) + rotateInCir));
			yield return null;
		}
		this.Deactivate();
		yield return null;
		yield break;
	}

	// Token: 0x06002264 RID: 8804 RVA: 0x0001D4E0 File Offset: 0x0001B6E0
	public void Deactivate()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x04001C5E RID: 7262
	public LevelProperties.RetroArcade.Missile properties;

	// Token: 0x04001C5F RID: 7263
	public float loopYSize;

	// Token: 0x04001C60 RID: 7264
	public float loopXSize;

	// Token: 0x04001C61 RID: 7265
	public float circleAngle;

	// Token: 0x04001C62 RID: 7266
	public Vector3 pivotPoint;

	// Token: 0x04001C63 RID: 7267
	public DamageDealer damageDealer;
}
