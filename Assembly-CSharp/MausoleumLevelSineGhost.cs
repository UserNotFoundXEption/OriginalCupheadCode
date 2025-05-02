using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002C0 RID: 704
public class MausoleumLevelSineGhost : MausoleumLevelGhostBase
{
	// Token: 0x06001F38 RID: 7992 RVA: 0x000B5990 File Offset: 0x000B3B90
	public MausoleumLevelSineGhost Create(Vector2 position, float rotation, float speed, LevelProperties.Mausoleum.SineGhost properties)
	{
		MausoleumLevelSineGhost mausoleumLevelSineGhost = base.Create(position, rotation, speed) as MausoleumLevelSineGhost;
		mausoleumLevelSineGhost.rotation = rotation;
		mausoleumLevelSineGhost.properties = properties;
		return mausoleumLevelSineGhost;
	}

	// Token: 0x06001F39 RID: 7993 RVA: 0x0001A4B6 File Offset: 0x000186B6
	public override void Start()
	{
		base.Start();
		this.CalculateDirection();
		this.CalculateSin();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001F3A RID: 7994 RVA: 0x000B59BC File Offset: 0x000B3BBC
	public void CalculateSin()
	{
		Vector2 vector = Vector2.zero;
		vector = MathUtils.AngleToDirection(this.rotation) / 2f;
		float num = -((vector.x - base.transform.position.x) / (vector.y - base.transform.position.y));
		float num2 = vector.y - num * vector.x;
		Vector2 zero = Vector2.zero;
		zero.x = vector.x + 1f;
		zero.y = num * zero.x + num2;
		this.normalized = Vector3.zero;
		this.normalized = zero - vector;
		this.normalized.Normalize();
	}

	// Token: 0x06001F3B RID: 7995 RVA: 0x000B5A88 File Offset: 0x000B3C88
	public void CalculateDirection()
	{
		Vector2 vector = Vector2.zero;
		vector = MathUtils.AngleToDirection(this.rotation);
		float value = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		this.pointAtTarget = MathUtils.AngleToDirection(value);
		base.transform.SetEulerAngles(null, null, new float?(value));
	}

	// Token: 0x06001F3C RID: 7996 RVA: 0x000B5AF8 File Offset: 0x000B3CF8
	public IEnumerator move_cr()
	{
		Vector3 pos = base.transform.position;
		for (;;)
		{
			this.angle += this.properties.waveSpeed * CupheadTime.Delta;
			if (CupheadTime.Delta != 0f)
			{
				pos += this.normalized * Mathf.Sin(this.angle + this.properties.waveAmount) * (this.properties.waveAmount / 2f);
			}
			pos += this.pointAtTarget * this.properties.ghostSpeed * CupheadTime.Delta;
			base.transform.position = pos;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001F3D RID: 7997 RVA: 0x000B5B14 File Offset: 0x000B3D14
	public override void Die()
	{
		if (!base.isDead)
		{
			SpriteDeathParts spriteDeathParts = this.hat.CreatePart(base.transform.position);
			spriteDeathParts.animator.SetBool("HatA", Rand.Bool());
		}
		base.Die();
	}

	// Token: 0x06001F3E RID: 7998 RVA: 0x0001A4D7 File Offset: 0x000186D7
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.hat = null;
	}

	// Token: 0x04001978 RID: 6520
	[SerializeField]
	public SpriteDeathParts hat;

	// Token: 0x04001979 RID: 6521
	public Vector3 pointAtTarget;

	// Token: 0x0400197A RID: 6522
	public Vector3 normalized;

	// Token: 0x0400197B RID: 6523
	public float rotation;

	// Token: 0x0400197C RID: 6524
	public float angle;

	// Token: 0x0400197D RID: 6525
	public LevelProperties.Mausoleum.SineGhost properties;
}
