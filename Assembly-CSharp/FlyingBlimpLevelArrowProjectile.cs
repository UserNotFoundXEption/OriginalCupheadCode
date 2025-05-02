using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200023B RID: 571
public class FlyingBlimpLevelArrowProjectile : HomingProjectile
{
	// Token: 0x06001A1E RID: 6686 RVA: 0x000A7D14 File Offset: 0x000A5F14
	public FlyingBlimpLevelArrowProjectile Create(Vector2 pos, float startRotation, float startSpeed, float speed, float rotation, float timeBeforeDeath, float timeBeforeHoming, AbstractPlayerController player, float hp)
	{
		FlyingBlimpLevelArrowProjectile flyingBlimpLevelArrowProjectile = base.Create(pos, startRotation, startSpeed, speed, rotation, timeBeforeDeath, timeBeforeHoming, player) as FlyingBlimpLevelArrowProjectile;
		flyingBlimpLevelArrowProjectile.CollisionDeath.OnlyPlayer();
		flyingBlimpLevelArrowProjectile.DamagesType.OnlyPlayer();
		flyingBlimpLevelArrowProjectile.health = hp;
		flyingBlimpLevelArrowProjectile.timeToDeath = timeBeforeDeath;
		flyingBlimpLevelArrowProjectile.speed = speed;
		return flyingBlimpLevelArrowProjectile;
	}

	// Token: 0x06001A1F RID: 6687 RVA: 0x00016323 File Offset: 0x00014523
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.trail_cr());
	}

	// Token: 0x06001A20 RID: 6688 RVA: 0x0001635B File Offset: 0x0001455B
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06001A21 RID: 6689 RVA: 0x000A7D6C File Offset: 0x000A5F6C
	public IEnumerator trail_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			Effect trail = Object.Instantiate<Effect>(this.trailPrefab);
			trail.transform.position = base.transform.position;
			trail.GetComponent<Animator>().SetInteger("PickAni", Random.Range(0, 3));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001A22 RID: 6690 RVA: 0x00016370 File Offset: 0x00014570
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06001A23 RID: 6691 RVA: 0x000A7D88 File Offset: 0x000A5F88
	public override void Die()
	{
		base.animator.SetTrigger("dead");
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(-90f));
		base.Die();
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x0001639B File Offset: 0x0001459B
	public void Destroy()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x000A7DE8 File Offset: 0x000A5FE8
	public IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.timeToDeath);
		YieldInstruction wait = new WaitForFixedUpdate();
		base.HomingEnabled = false;
		for (;;)
		{
			base.transform.position += base.transform.right * this.speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x040014EE RID: 5358
	[SerializeField]
	public Effect trailPrefab;

	// Token: 0x040014EF RID: 5359
	public float speed;

	// Token: 0x040014F0 RID: 5360
	public float health;

	// Token: 0x040014F1 RID: 5361
	public float timeToDeath;

	// Token: 0x040014F2 RID: 5362
	public DamageReceiver damageReceiver;
}
