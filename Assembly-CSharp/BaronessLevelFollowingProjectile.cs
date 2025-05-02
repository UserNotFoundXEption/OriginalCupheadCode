using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class BaronessLevelFollowingProjectile : AbstractProjectile
{
	// Token: 0x06000FA1 RID: 4001 RVA: 0x0000D40D File Offset: 0x0000B60D
	public override void Awake()
	{
		base.Awake();
		this.isActive = true;
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x0008DF64 File Offset: 0x0008C164
	public void Init(Vector2 pos, Vector3 target, LevelProperties.Baroness.BaronessVonBonbon properties, AbstractPlayerController player, BaronessLevelCastle parent)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.target = target;
		this.player = player;
		this.parent = parent;
		this.parent.OnDeathEvent += this.KillProjectile;
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x0000D41C File Offset: 0x0000B61C
	public void KillProjectile()
	{
		this.isActive = false;
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x0000D425 File Offset: 0x0000B625
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06000FA5 RID: 4005 RVA: 0x0000D43A File Offset: 0x0000B63A
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (!this.isActive)
		{
			this.Die();
		}
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x0000D469 File Offset: 0x0000B669
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x0008DFB8 File Offset: 0x0008C1B8
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float count = 0f;
		for (;;)
		{
			Vector2 start = base.transform.position;
			this.target = this.player.transform.position;
			float followTime = this.properties.finalProjectileMoveDuration;
			float t = 0f;
			while (t < followTime)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, this.target, this.properties.finalProjectileSpeed * CupheadTime.FixedDelta);
				t += CupheadTime.FixedDelta;
				yield return wait;
			}
			this.player = PlayerManager.GetNext();
			count += 1f;
			if (count > this.properties.finalProjectileRedirectCount)
			{
				break;
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.finalProjectileRedirectDelay);
		}
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		Vector3 dir = this.player.transform.position - base.transform.position;
		for (;;)
		{
			base.transform.position += dir.normalized * this.properties.finalProjectileSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x0000D487 File Offset: 0x0000B687
	public override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
		base.Die();
	}

	// Token: 0x04000CC2 RID: 3266
	public LevelProperties.Baroness.BaronessVonBonbon properties;

	// Token: 0x04000CC3 RID: 3267
	public AbstractPlayerController player;

	// Token: 0x04000CC4 RID: 3268
	public Vector3 target;

	// Token: 0x04000CC5 RID: 3269
	public BaronessLevelCastle parent;

	// Token: 0x04000CC6 RID: 3270
	public bool timesUp;

	// Token: 0x04000CC7 RID: 3271
	public bool isActive;
}
