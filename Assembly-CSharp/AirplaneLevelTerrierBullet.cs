using System;
using UnityEngine;

// Token: 0x0200058A RID: 1418
public class AirplaneLevelTerrierBullet : BasicProjectile
{
	// Token: 0x06003BF2 RID: 15346 RVA: 0x00114660 File Offset: 0x00112860
	public AirplaneLevelTerrierBullet Create(Vector2 position, float rotation, float speed, float acceleration)
	{
		AirplaneLevelTerrierBullet airplaneLevelTerrierBullet = this.Create(position, rotation) as AirplaneLevelTerrierBullet;
		airplaneLevelTerrierBullet.endVelocity = MathUtils.AngleToDirection(rotation) * speed;
		airplaneLevelTerrierBullet.startVelocity = airplaneLevelTerrierBullet.endVelocity.normalized * 0.1f;
		airplaneLevelTerrierBullet.accelT = 0f;
		airplaneLevelTerrierBullet.accel = acceleration;
		airplaneLevelTerrierBullet.transform.rotation = Quaternion.identity;
		float num = Vector3.SignedAngle(airplaneLevelTerrierBullet.velocity, Vector3.up, Vector3.forward);
		while (Mathf.Abs(num) > 45f)
		{
			num -= 90f * Mathf.Sign(num);
		}
		airplaneLevelTerrierBullet.transform.Rotate(new Vector3(0f, 0f, -num));
		return airplaneLevelTerrierBullet;
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x000308DA File Offset: 0x0002EADA
	public void PlayWow()
	{
		base.animator.Play("WowIntro");
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x00114728 File Offset: 0x00112928
	public override void Move()
	{
		this.accelT += CupheadTime.FixedDelta * this.accel * 1.6f;
		this.velocity = Vector3.Lerp(this.startVelocity, this.endVelocity, this.accelT);
		base.transform.position += this.velocity * CupheadTime.FixedDelta;
	}

	// Token: 0x04002F9B RID: 12187
	public const float BASE_ACCELERATION = 1.6f;

	// Token: 0x04002F9C RID: 12188
	public Vector3 velocity;

	// Token: 0x04002F9D RID: 12189
	public Vector3 startVelocity;

	// Token: 0x04002F9E RID: 12190
	public Vector3 endVelocity;

	// Token: 0x04002F9F RID: 12191
	public float accelT;

	// Token: 0x04002FA0 RID: 12192
	public float accel;
}
