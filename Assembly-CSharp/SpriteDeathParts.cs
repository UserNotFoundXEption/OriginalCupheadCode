using System;
using UnityEngine;

// Token: 0x020005AF RID: 1455
public class SpriteDeathParts : AbstractCollidableObject
{
	// Token: 0x06003D22 RID: 15650 RVA: 0x0011842C File Offset: 0x0011662C
	public SpriteDeathParts CreatePart(Vector3 position)
	{
		SpriteDeathParts spriteDeathParts = this.InstantiatePrefab<SpriteDeathParts>();
		spriteDeathParts.transform.position = position;
		return spriteDeathParts;
	}

	// Token: 0x06003D23 RID: 15651 RVA: 0x00118450 File Offset: 0x00116650
	public override void Awake()
	{
		base.Awake();
		this.velocity = new Vector2(Random.Range(this.VelocityXMin, this.VelocityXMax), Random.Range(this.VelocityYMin, this.VelocityYMax));
		if (this.rotate)
		{
			this.rotationSpeed = Random.Range(this.rotationSpeedRange.minimum, this.rotationSpeedRange.maximum) * (float)Rand.PosOrNeg();
		}
	}

	// Token: 0x06003D24 RID: 15652 RVA: 0x00031522 File Offset: 0x0002F722
	public virtual void Update()
	{
		this.Step(Time.fixedDeltaTime);
	}

	// Token: 0x06003D25 RID: 15653 RVA: 0x001184C4 File Offset: 0x001166C4
	public virtual void Step(float deltaTime)
	{
		if (this.clampFallVelocity)
		{
			this.velocity.y = Mathf.Clamp(this.velocity.y, -5000f, float.MaxValue);
		}
		base.transform.position += (this.velocity + new Vector2(0f, this.accumulatedGravity)) * deltaTime;
		this.accumulatedGravity += this.GRAVITY;
		if (this.rotate)
		{
			this.currentAngle += this.rotationSpeed * deltaTime;
			base.transform.rotation = Quaternion.Euler(0f, 0f, this.currentAngle);
		}
		if (base.transform.position.y < -360f - this.bottomOffset)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003D26 RID: 15654 RVA: 0x0003152F File Offset: 0x0002F72F
	public void SetVelocityX(float min, float max)
	{
		this.velocity.x = Random.Range(min, max);
	}

	// Token: 0x06003D27 RID: 15655 RVA: 0x00031543 File Offset: 0x0002F743
	public void SetVelocityY(float min, float max)
	{
		this.velocity.y = Random.Range(min, max);
	}

	// Token: 0x040030AE RID: 12462
	public float bottomOffset = 100f;

	// Token: 0x040030AF RID: 12463
	public float VelocityXMin = -500f;

	// Token: 0x040030B0 RID: 12464
	public float VelocityXMax = 500f;

	// Token: 0x040030B1 RID: 12465
	public float VelocityYMin = 500f;

	// Token: 0x040030B2 RID: 12466
	public float VelocityYMax = 1000f;

	// Token: 0x040030B3 RID: 12467
	public float GRAVITY = -100f;

	// Token: 0x040030B4 RID: 12468
	[SerializeField]
	public bool clampFallVelocity;

	// Token: 0x040030B5 RID: 12469
	[SerializeField]
	public bool rotate;

	// Token: 0x040030B6 RID: 12470
	[SerializeField]
	public Rangef rotationSpeedRange;

	// Token: 0x040030B7 RID: 12471
	public Vector2 velocity;

	// Token: 0x040030B8 RID: 12472
	public float accumulatedGravity;

	// Token: 0x040030B9 RID: 12473
	public float rotationSpeed;

	// Token: 0x040030BA RID: 12474
	public float currentAngle;
}
