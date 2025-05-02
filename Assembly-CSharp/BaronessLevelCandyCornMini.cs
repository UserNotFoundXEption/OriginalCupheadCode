using System;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class BaronessLevelCandyCornMini : AbstractProjectile
{
	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x0000D697 File Offset: 0x0000B897
	// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x0000D69F File Offset: 0x0000B89F
	public BaronessLevelCandyCornMini.State state { get; set; }

	// Token: 0x06000FD6 RID: 4054 RVA: 0x0000D6A8 File Offset: 0x0000B8A8
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06000FD7 RID: 4055 RVA: 0x0000D6D3 File Offset: 0x0000B8D3
	public override void Start()
	{
		base.Start();
		base.GetComponent<SpriteRenderer>().flipX = Rand.Bool();
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x0000D6EB File Offset: 0x0000B8EB
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x0000D709 File Offset: 0x0000B909
	public void Init(Vector2 pos, float speed, float health)
	{
		this.speed = speed;
		base.transform.position = pos;
		this.health = health;
		this.state = BaronessLevelCandyCornMini.State.Spawned;
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x0000D731 File Offset: 0x0000B931
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x0008E6DC File Offset: 0x0008C8DC
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		float num = 2f;
		Vector3 position = base.transform.position;
		position.y = Mathf.MoveTowards(base.transform.position.y, 720f + num, this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
		base.transform.position = position;
		if (base.transform.position.y == 720f + num)
		{
			this.Die();
		}
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x0000D74F File Offset: 0x0000B94F
	public float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x0008E76C File Offset: 0x0008C96C
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			this.deathEffect.Create(base.transform.position);
			this.Die();
		}
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x0000D770 File Offset: 0x0000B970
	public override void Die()
	{
		base.Die();
		this.state = BaronessLevelCandyCornMini.State.Unspawned;
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x0000D790 File Offset: 0x0000B990
	public void SoundCandyCornMiniBite()
	{
		AudioManager.Play("level_baroness_candycorn_mini_bite");
		this.emitAudioFromObject.Add("level_baroness_candycorn_mini_bite");
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x0000D7AC File Offset: 0x0000B9AC
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.deathEffect = null;
	}

	// Token: 0x04000CE4 RID: 3300
	[SerializeField]
	public Effect deathEffect;

	// Token: 0x04000CE6 RID: 3302
	public float speed;

	// Token: 0x04000CE7 RID: 3303
	public float health;

	// Token: 0x04000CE8 RID: 3304
	public Vector3 lastPos;

	// Token: 0x04000CE9 RID: 3305
	public Vector3 distFromLeaderX;

	// Token: 0x04000CEA RID: 3306
	public DamageReceiver damageReceiver;

	// Token: 0x02000A15 RID: 2581
	public enum State
	{
		// Token: 0x04004AA8 RID: 19112
		Unspawned,
		// Token: 0x04004AA9 RID: 19113
		Spawned,
		// Token: 0x04004AAA RID: 19114
		Dying
	}
}
