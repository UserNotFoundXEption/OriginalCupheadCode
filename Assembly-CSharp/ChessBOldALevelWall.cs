using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000173 RID: 371
public class ChessBOldALevelWall : AbstractProjectile
{
	// Token: 0x17000244 RID: 580
	// (get) Token: 0x060011D3 RID: 4563 RVA: 0x0000F1B0 File Offset: 0x0000D3B0
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060011D4 RID: 4564 RVA: 0x0009353C File Offset: 0x0009173C
	public void StartRotate(float angle, ChessBOldALevelBishop parent, float loopSize, float speed, bool isClockwise, float scale)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.angle = angle;
		this.parent = parent;
		this.speed = speed;
		this.loopSize = loopSize;
		this.isClockwise = isClockwise;
		base.transform.SetScale(new float?(scale), null, null);
		base.StartCoroutine(this.move_wall_cr());
	}

	// Token: 0x060011D5 RID: 4565 RVA: 0x0000F1B7 File Offset: 0x0000D3B7
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x000935AC File Offset: 0x000917AC
	public IEnumerator move_wall_cr()
	{
		Vector3 handleRotation = Vector3.zero;
		if (this.angle == 0f || this.angle == 180f)
		{
			base.transform.position = this.parent.transform.position + MathUtils.AngleToDirection(this.angle) * this.loopSize;
		}
		else
		{
			base.transform.position = this.parent.transform.position + MathUtils.AngleToDirection(this.angle) * this.loopSize;
		}
		this.angle *= 0.0174532924f;
		for (;;)
		{
			if (this.isClockwise)
			{
				this.angle += this.speed * CupheadTime.FixedDelta;
			}
			else
			{
				this.angle -= this.speed * CupheadTime.FixedDelta;
			}
			handleRotation = new Vector3(Mathf.Sin(this.angle) * this.loopSize, Mathf.Cos(this.angle) * this.loopSize, 0f);
			base.transform.position = this.parent.transform.position;
			base.transform.position += handleRotation;
			Vector3 diff = this.parent.transform.position - base.transform.position;
			diff.Normalize();
			base.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(diff.y, diff.x) * 57.29578f + 90f);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x060011D7 RID: 4567 RVA: 0x0000F1D5 File Offset: 0x0000D3D5
	public void Dead()
	{
		this.Recycle<ChessBOldALevelWall>();
	}

	// Token: 0x04000E50 RID: 3664
	public ChessBOldALevelBishop parent;

	// Token: 0x04000E51 RID: 3665
	public bool isClockwise;

	// Token: 0x04000E52 RID: 3666
	public float angle;

	// Token: 0x04000E53 RID: 3667
	public float loopSize;

	// Token: 0x04000E54 RID: 3668
	public float speed;
}
