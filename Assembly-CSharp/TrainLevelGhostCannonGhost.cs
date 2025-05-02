using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003AF RID: 943
public class TrainLevelGhostCannonGhost : HomingProjectile
{
	// Token: 0x17000331 RID: 817
	// (get) Token: 0x060029D4 RID: 10708 RVA: 0x00023389 File Offset: 0x00021589
	public override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x000D2BF0 File Offset: 0x000D0DF0
	public TrainLevelGhostCannonGhost Create(Vector3 pos, float delay, float speed, float aimSpeed, float health, float skullSpeed)
	{
		TrainLevelGhostCannonGhost trainLevelGhostCannonGhost = base.Create(pos, -90f, speed, speed, aimSpeed, float.MaxValue, 2f, PlayerManager.GetNext()) as TrainLevelGhostCannonGhost;
		trainLevelGhostCannonGhost.HomingEnabled = false;
		trainLevelGhostCannonGhost.transform.position = pos;
		trainLevelGhostCannonGhost.delay = delay;
		trainLevelGhostCannonGhost.health = health;
		trainLevelGhostCannonGhost.skullSpeed = skullSpeed;
		trainLevelGhostCannonGhost.GetComponent<Collider2D>().enabled = false;
		return trainLevelGhostCannonGhost;
	}

	// Token: 0x060029D6 RID: 10710 RVA: 0x00023390 File Offset: 0x00021590
	public override void Start()
	{
		base.Start();
		this.damageable = false;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x060029D7 RID: 10711 RVA: 0x000233CF File Offset: 0x000215CF
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060029D8 RID: 10712 RVA: 0x000233ED File Offset: 0x000215ED
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.Die();
		}
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x000D2C60 File Offset: 0x000D0E60
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (!this.damageable || this.health <= 0f)
		{
			return;
		}
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060029DA RID: 10714 RVA: 0x000D2CB4 File Offset: 0x000D0EB4
	public override void Die()
	{
		AudioManager.Play("train_lollipop_cannon_ghost_death");
		this.emitAudioFromObject.Add("train_lollipop_cannon_ghost_death");
		this.StopAllCoroutines();
		this.health = -1f;
		this.damageable = false;
		base.animator.Play("Die");
	}

	// Token: 0x060029DB RID: 10715 RVA: 0x00023404 File Offset: 0x00021604
	public void DropSkull()
	{
		this.skullPrefab.Create(base.transform.position, this.skullSpeed);
	}

	// Token: 0x060029DC RID: 10716 RVA: 0x00023423 File Offset: 0x00021623
	public void OnDieAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060029DD RID: 10717 RVA: 0x000D2D04 File Offset: 0x000D0F04
	public IEnumerator start_cr()
	{
		yield return base.StartCoroutine(this.up_cr());
		yield return CupheadTime.WaitForSeconds(this, this.delay);
		base.animator.Play("Attack");
		this.damageable = true;
		base.HomingEnabled = true;
		base.GetComponent<Collider2D>().enabled = true;
		yield break;
	}

	// Token: 0x060029DE RID: 10718 RVA: 0x000D2D20 File Offset: 0x000D0F20
	public IEnumerator up_cr()
	{
		yield return base.TweenPositionY(base.transform.position.y, 500f, 0.4f, EaseUtils.EaseType.linear);
		yield break;
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x00023430 File Offset: 0x00021630
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.skullPrefab = null;
	}

	// Token: 0x04002304 RID: 8964
	[SerializeField]
	public TrainLevelGhostCannonGhostSkull skullPrefab;

	// Token: 0x04002305 RID: 8965
	public float delay;

	// Token: 0x04002306 RID: 8966
	public float health;

	// Token: 0x04002307 RID: 8967
	public float skullSpeed;

	// Token: 0x04002308 RID: 8968
	public bool damageable;

	// Token: 0x04002309 RID: 8969
	public DamageReceiver damageReceiver;
}
