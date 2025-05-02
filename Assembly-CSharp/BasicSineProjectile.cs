using System;
using UnityEngine;

// Token: 0x0200058E RID: 1422
public class BasicSineProjectile : BasicProjectile
{
	// Token: 0x06003C11 RID: 15377 RVA: 0x0011496C File Offset: 0x00112B6C
	public BasicSineProjectile Create(Vector2 pos, float rotation, float velocity, float sinVelocity, float sinSize)
	{
		BasicSineProjectile basicSineProjectile = this.Create(pos) as BasicSineProjectile;
		basicSineProjectile.velocity = velocity;
		basicSineProjectile.rotation = rotation;
		basicSineProjectile.sinSize = sinSize;
		basicSineProjectile.sinVelocity = sinVelocity;
		return basicSineProjectile;
	}

	// Token: 0x06003C12 RID: 15378 RVA: 0x00030A7F File Offset: 0x0002EC7F
	public override void Start()
	{
		base.Start();
		this.CalculateSin();
	}

	// Token: 0x06003C13 RID: 15379 RVA: 0x001149A8 File Offset: 0x00112BA8
	public void CalculateSin()
	{
		Vector2 zero = Vector2.zero;
		zero.x = (this.direction.x + base.transform.position.x) / 2f;
		zero.y = (this.direction.y + base.transform.position.y) / 2f;
		float num = -((this.direction.x - base.transform.position.x) / (this.direction.y - base.transform.position.y));
		float num2 = zero.y - num * zero.x;
		Vector2 zero2 = Vector2.zero;
		zero2.x = zero.x + 1f;
		zero2.y = num * zero2.x + num2;
		this.normalized = Vector3.zero;
		this.normalized = zero2 - zero;
		this.normalized.Normalize();
	}

	// Token: 0x06003C14 RID: 15380 RVA: 0x00114AC0 File Offset: 0x00112CC0
	public override void Move()
	{
		this.direction = MathUtils.AngleToDirection(this.rotation);
		Vector2 vector = base.transform.position;
		this.angle += this.sinVelocity * CupheadTime.Delta;
		vector += this.normalized * Mathf.Sin(this.angle) * this.sinSize;
		vector += this.direction * this.velocity * CupheadTime.Delta;
		base.transform.position = vector;
	}

	// Token: 0x04002FA8 RID: 12200
	public Vector2 direction;

	// Token: 0x04002FA9 RID: 12201
	public Vector2 normalized;

	// Token: 0x04002FAA RID: 12202
	public float velocity;

	// Token: 0x04002FAB RID: 12203
	public float sinVelocity;

	// Token: 0x04002FAC RID: 12204
	public float angle;

	// Token: 0x04002FAD RID: 12205
	public float rotation;

	// Token: 0x04002FAE RID: 12206
	public float sinSize;
}
