using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class DevilLevelPitchforkRingProjectile : AbstractProjectile
{
	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06001536 RID: 5430 RVA: 0x0001206C File Offset: 0x0001026C
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06001537 RID: 5431 RVA: 0x0001206F File Offset: 0x0001026F
	public override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x0009BC28 File Offset: 0x00099E28
	public DevilLevelPitchforkRingProjectile Create(Vector2 pos, float speed, float groundDuration, DevilLevelSittingDevil parent, float waitTime)
	{
		DevilLevelPitchforkRingProjectile devilLevelPitchforkRingProjectile = this.InstantiatePrefab<DevilLevelPitchforkRingProjectile>();
		devilLevelPitchforkRingProjectile.transform.position = pos;
		devilLevelPitchforkRingProjectile.speed = speed;
		devilLevelPitchforkRingProjectile.state = DevilLevelPitchforkRingProjectile.State.Idle;
		devilLevelPitchforkRingProjectile.groundDuration = groundDuration;
		devilLevelPitchforkRingProjectile.parent = parent;
		devilLevelPitchforkRingProjectile.waitTime = waitTime;
		devilLevelPitchforkRingProjectile.StartCoroutine(devilLevelPitchforkRingProjectile.wait_cr());
		return devilLevelPitchforkRingProjectile;
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x00012076 File Offset: 0x00010276
	public override void Update()
	{
		base.Update();
		if (this.parent == null)
		{
			this.Die();
		}
	}

	// Token: 0x0600153A RID: 5434 RVA: 0x00012095 File Offset: 0x00010295
	public override void Start()
	{
		base.Start();
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x000120A9 File Offset: 0x000102A9
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x0009BC80 File Offset: 0x00099E80
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.waitTimeUp)
		{
			return;
		}
		if (!base.dead && this.state == DevilLevelPitchforkRingProjectile.State.Attacking)
		{
			if (!this.soundPlayed)
			{
				this.AttackSFX();
				this.soundPlayed = true;
			}
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			float radius = base.GetComponent<CircleCollider2D>().radius;
		}
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x0009BD0C File Offset: 0x00099F0C
	public IEnumerator wait_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.waitTime);
		this.waitTimeUp = true;
		base.GetComponent<Collider2D>().enabled = true;
		base.animator.SetTrigger("Continue");
		yield break;
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x0009BD28 File Offset: 0x00099F28
	public void Attack()
	{
		if (!base.dead)
		{
			this.state = DevilLevelPitchforkRingProjectile.State.Attacking;
			this.velocity = this.speed * (PlayerManager.GetNext().center - base.transform.position).normalized;
			base.StartCoroutine(this.main_cr());
		}
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x0009BD8C File Offset: 0x00099F8C
	public IEnumerator main_cr()
	{
		while (this.state == DevilLevelPitchforkRingProjectile.State.Attacking)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.groundDuration);
		this.Die();
		yield break;
	}

	// Token: 0x06001540 RID: 5440 RVA: 0x000120C7 File Offset: 0x000102C7
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001541 RID: 5441 RVA: 0x000120DA File Offset: 0x000102DA
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.animator.SetBool("IsPink", parryable);
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x000120F4 File Offset: 0x000102F4
	public void AttackSFX()
	{
		AudioManager.Play("devil_ring_projectile");
		this.emitAudioFromObject.Add("devil_ring_projectile");
	}

	// Token: 0x0400115F RID: 4447
	public DevilLevelPitchforkRingProjectile.State state;

	// Token: 0x04001160 RID: 4448
	public Vector2 velocity;

	// Token: 0x04001161 RID: 4449
	public float speed;

	// Token: 0x04001162 RID: 4450
	public float groundDuration;

	// Token: 0x04001163 RID: 4451
	public DevilLevelSittingDevil parent;

	// Token: 0x04001164 RID: 4452
	public float waitTime;

	// Token: 0x04001165 RID: 4453
	public bool waitTimeUp;

	// Token: 0x04001166 RID: 4454
	public bool soundPlayed;

	// Token: 0x02000B5A RID: 2906
	public enum State
	{
		// Token: 0x04005322 RID: 21282
		Idle,
		// Token: 0x04005323 RID: 21283
		Attacking,
		// Token: 0x04005324 RID: 21284
		OnGround
	}
}
