using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000334 RID: 820
public class RobotLevelGemProjectile : AbstractProjectile
{
	// Token: 0x170002FC RID: 764
	// (get) Token: 0x060023D1 RID: 9169 RVA: 0x0001E35D File Offset: 0x0001C55D
	// (set) Token: 0x060023D2 RID: 9170 RVA: 0x0001E365 File Offset: 0x0001C565
	public float Speed { get; set; }

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x060023D3 RID: 9171 RVA: 0x0001E36E File Offset: 0x0001C56E
	public override float DestroyLifetime
	{
		get
		{
			return this.lifeTime;
		}
	}

	// Token: 0x060023D4 RID: 9172 RVA: 0x000C1F5C File Offset: 0x000C015C
	public virtual AbstractProjectile Init(MinMax speed, float acceleration, float waveLength, float waveSpeedMultiplier, float lifeTime, bool isBlue, bool isParryable)
	{
		base.ResetLifetime();
		base.ResetDistance();
		float num = Random.Range(0.92f, 1.08f);
		this.minSpeed = speed.min * num;
		this.maxSpeed = speed.max * num;
		this.Speed = this.minSpeed;
		this.acceleration = acceleration;
		this.waveLength = waveLength;
		this.waveSpeedMultiplier = waveSpeedMultiplier;
		this.lifeTime = lifeTime;
		base.animator.SetFloat("Gem", (float)((!isBlue) ? 0 : 1));
		this.time = 0f;
		this.originalPosition = base.transform.position;
		this.SetParryable(isParryable);
		if (isParryable)
		{
			base.animator.Play("GemParry", 0, Random.value);
		}
		else
		{
			base.animator.Play("Gem", 0, Random.value);
		}
		base.StartCoroutine(this.speed_cr());
		base.StartCoroutine(this.fadeIn_cr());
		return this;
	}

	// Token: 0x060023D5 RID: 9173 RVA: 0x000C2064 File Offset: 0x000C0264
	public override void Update()
	{
		base.Update();
		this.originalPosition += -base.transform.right * this.Speed * CupheadTime.Delta;
		base.transform.position = this.originalPosition + Mathf.Sin(this.time * this.waveSpeedMultiplier) * this.waveLength * base.transform.up;
		this.time += CupheadTime.Delta;
	}

	// Token: 0x060023D6 RID: 9174 RVA: 0x0001E376 File Offset: 0x0001C576
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060023D7 RID: 9175 RVA: 0x0001E394 File Offset: 0x0001C594
	public void SetCollider(bool c)
	{
		base.GetComponent<CircleCollider2D>().enabled = c;
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000C2108 File Offset: 0x000C0308
	public IEnumerator effect_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, Random.Range(0f, 0.3f));
		for (;;)
		{
			this.effectPrefab.Create(this.effectRoot.position);
			yield return CupheadTime.WaitForSeconds(this, 0.3f);
		}
		yield break;
	}

	// Token: 0x060023D9 RID: 9177 RVA: 0x000C2124 File Offset: 0x000C0324
	public IEnumerator speed_cr()
	{
		this.Speed = this.minSpeed;
		while (this.Speed < this.maxSpeed)
		{
			this.Speed += this.acceleration;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060023DA RID: 9178 RVA: 0x000C2140 File Offset: 0x000C0340
	public IEnumerator fadeIn_cr()
	{
		SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
		while (sprite.color.a < 1f)
		{
			Color c = sprite.color;
			c.a += 1f * CupheadTime.Delta;
			sprite.color = c;
			yield return null;
		}
		Color color = sprite.color;
		color.a = 1f;
		sprite.color = color;
		yield break;
	}

	// Token: 0x060023DB RID: 9179 RVA: 0x0001E3A2 File Offset: 0x0001C5A2
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.effectPrefab = null;
	}

	// Token: 0x060023DC RID: 9180 RVA: 0x0001E3B1 File Offset: 0x0001C5B1
	public override void OnParryDie()
	{
		this.Recycle<RobotLevelGemProjectile>();
	}

	// Token: 0x060023DD RID: 9181 RVA: 0x0001E3B9 File Offset: 0x0001C5B9
	public override void OnDieDistance()
	{
		this.Recycle<RobotLevelGemProjectile>();
	}

	// Token: 0x060023DE RID: 9182 RVA: 0x0001E3C1 File Offset: 0x0001C5C1
	public override void OnDieLifetime()
	{
		this.Recycle<RobotLevelGemProjectile>();
	}

	// Token: 0x060023DF RID: 9183 RVA: 0x0001E3C9 File Offset: 0x0001C5C9
	public override void OnDieAnimationComplete()
	{
		this.Recycle<RobotLevelGemProjectile>();
	}

	// Token: 0x04001DAD RID: 7597
	public const string GemParameterName = "Gem";

	// Token: 0x04001DAE RID: 7598
	public const string GemParryParameterName = "GemParry";

	// Token: 0x04001DAF RID: 7599
	public const float SpeedVariation = 0.08f;

	// Token: 0x04001DB0 RID: 7600
	public const float FadeTime = 0.3f;

	// Token: 0x04001DB1 RID: 7601
	public const float FadeRate = 0.3f;

	// Token: 0x04001DB3 RID: 7603
	[SerializeField]
	public Effect effectPrefab;

	// Token: 0x04001DB4 RID: 7604
	[SerializeField]
	public Transform effectRoot;

	// Token: 0x04001DB5 RID: 7605
	public Vector3 originalPosition;

	// Token: 0x04001DB6 RID: 7606
	public Vector3 originalScale;

	// Token: 0x04001DB7 RID: 7607
	public float minSpeed;

	// Token: 0x04001DB8 RID: 7608
	public float maxSpeed;

	// Token: 0x04001DB9 RID: 7609
	public float acceleration;

	// Token: 0x04001DBA RID: 7610
	public float waveLength;

	// Token: 0x04001DBB RID: 7611
	public float waveSpeedMultiplier;

	// Token: 0x04001DBC RID: 7612
	public float time;

	// Token: 0x04001DBD RID: 7613
	public float lifeTime;
}
