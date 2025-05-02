using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200028D RID: 653
public class FlyingMermaidLevelPufferfish : AbstractProjectile
{
	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06001DA7 RID: 7591 RVA: 0x000191C8 File Offset: 0x000173C8
	// (set) Token: 0x06001DA8 RID: 7592 RVA: 0x000191D0 File Offset: 0x000173D0
	public FlyingMermaidLevelPufferfish.State state { get; set; }

	// Token: 0x06001DA9 RID: 7593 RVA: 0x000B15C0 File Offset: 0x000AF7C0
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Vector2 vector = base.transform.position;
		vector.y = this.spawnY;
		base.transform.position = vector;
		this.SetParryable(this.parryable);
		base.animator.Play("Idle", 0, Random.Range(0f, 1f));
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x000191D9 File Offset: 0x000173D9
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001DAB RID: 7595 RVA: 0x000B1654 File Offset: 0x000AF854
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		AudioManager.Play("level_mermaid_merdusa_puffer_fish_hit");
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != FlyingMermaidLevelPufferfish.State.Dying)
		{
			this.state = FlyingMermaidLevelPufferfish.State.Dying;
			this.StartDeath();
		}
	}

	// Token: 0x06001DAC RID: 7596 RVA: 0x000191F7 File Offset: 0x000173F7
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.state != FlyingMermaidLevelPufferfish.State.Dying && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x00019221 File Offset: 0x00017421
	public void Init(LevelProperties.FlyingMermaid.Pufferfish properties)
	{
		this.properties = properties;
		this.hp = properties.hp;
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x000B16A8 File Offset: 0x000AF8A8
	public IEnumerator loop_cr()
	{
		float speed = this.properties.floatSpeed * Random.Range(0.9f, 1.1f);
		for (;;)
		{
			Vector2 position = base.transform.position;
			position.y += speed * CupheadTime.Delta;
			base.transform.position = position;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x00019243 File Offset: 0x00017443
	public void StartDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.dying_cr());
	}

	// Token: 0x06001DB0 RID: 7600 RVA: 0x000B16C4 File Offset: 0x000AF8C4
	public IEnumerator dying_cr()
	{
		base.gameObject.tag = "EnemyProjectile";
		this.deathFX.Create(base.transform.position);
		base.animator.Play("Death");
		float velocity = 100f;
		while (base.transform.position.y > -660f)
		{
			velocity += CupheadTime.Delta * 300f;
			base.transform.AddPosition(0f, (-velocity + this.accumulatedGravity) * CupheadTime.Delta, 0f);
			this.accumulatedGravity += -100f;
			yield return null;
		}
		this.Die();
		yield break;
	}

	// Token: 0x06001DB1 RID: 7601 RVA: 0x00019258 File Offset: 0x00017458
	public override void Die()
	{
		base.transform.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x06001DB2 RID: 7602 RVA: 0x00019271 File Offset: 0x00017471
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.deathFX = null;
	}

	// Token: 0x0400184C RID: 6220
	public const float GRAVITY = -100f;

	// Token: 0x0400184E RID: 6222
	[SerializeField]
	public Effect deathFX;

	// Token: 0x0400184F RID: 6223
	[SerializeField]
	public float spawnY;

	// Token: 0x04001850 RID: 6224
	[SerializeField]
	public bool parryable;

	// Token: 0x04001851 RID: 6225
	public DamageReceiver damageReceiver;

	// Token: 0x04001852 RID: 6226
	public LevelProperties.FlyingMermaid.Pufferfish properties;

	// Token: 0x04001853 RID: 6227
	public float hp;

	// Token: 0x04001854 RID: 6228
	public float accumulatedGravity;

	// Token: 0x02000D4C RID: 3404
	public enum State
	{
		// Token: 0x04006067 RID: 24679
		Idle,
		// Token: 0x04006068 RID: 24680
		Dying
	}
}
