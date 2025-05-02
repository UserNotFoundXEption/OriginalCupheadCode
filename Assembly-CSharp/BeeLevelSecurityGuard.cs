using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200016D RID: 365
public class BeeLevelSecurityGuard : LevelProperties.Bee.Entity
{
	// Token: 0x17000242 RID: 578
	// (get) Token: 0x0600118A RID: 4490 RVA: 0x0000EE1C File Offset: 0x0000D01C
	// (set) Token: 0x0600118B RID: 4491 RVA: 0x0000EE24 File Offset: 0x0000D024
	public BeeLevelSecurityGuard.State state { get; set; }

	// Token: 0x0600118C RID: 4492 RVA: 0x000929B0 File Offset: 0x00090BB0
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnTakeDamage;
		this.damageDealer = DamageDealer.NewEnemy();
		this.circleCollider = base.GetComponent<CircleCollider2D>();
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x0000EE2D File Offset: 0x0000D02D
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600118E RID: 4494 RVA: 0x0000EE45 File Offset: 0x0000D045
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600118F RID: 4495 RVA: 0x0000EE6E File Offset: 0x0000D06E
	public void OnTakeDamage(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x00092A00 File Offset: 0x00090C00
	public void StartSecurityGuard()
	{
		this.ResetGuard();
		this.p = base.properties.CurrentState.securityGuard;
		base.properties.OnStateChange += this.OnStateChange;
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x0000EE81 File Offset: 0x0000D081
	public void OnStateChange()
	{
		base.properties.OnStateChange -= this.OnStateChange;
		this.Die();
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
	public void Die()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.leave_cr());
	}

	// Token: 0x06001193 RID: 4499 RVA: 0x0000EEB5 File Offset: 0x0000D0B5
	public void ResetGuard()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x06001194 RID: 4500 RVA: 0x0000EEBD File Offset: 0x0000D0BD
	public void SfxThrow()
	{
		AudioManager.Play("bee_guard_attack");
		this.emitAudioFromObject.Add("bee_guard_attack");
	}

	// Token: 0x06001195 RID: 4501 RVA: 0x00092A50 File Offset: 0x00090C50
	public void Attack()
	{
		this.bombPrefab.Create(this.bombRoot.position, -(int)base.transform.localScale.x, this.p.idleTime, this.p.warningTime, this.p.childSpeed, this.p.childCount);
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x0000EED9 File Offset: 0x0000D0D9
	public void AttackComplete()
	{
	}

	// Token: 0x06001197 RID: 4503 RVA: 0x00092ABC File Offset: 0x00090CBC
	public void FlipX()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x0000EEDB File Offset: 0x0000D0DB
	public float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06001199 RID: 4505 RVA: 0x00092B04 File Offset: 0x00090D04
	public IEnumerator go_cr()
	{
		AudioManager.Play("bee_guard_spawn");
		this.emitAudioFromObject.Add("bee_guard_spawn");
		AudioManager.PlayLoop("bee_guard_flying_loop");
		this.emitAudioFromObject.Add("bee_guard_flying_loop");
		for (;;)
		{
			yield return base.StartCoroutine(this.move_cr());
			yield return base.StartCoroutine(this.attack_cr());
		}
		yield break;
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x00092B20 File Offset: 0x00090D20
	public IEnumerator move_cr()
	{
		this.state = BeeLevelSecurityGuard.State.Move;
		float t = 0f;
		float time = this.p.attackDelay.RandomFloat();
		while (t < time)
		{
			base.transform.AddPositionForward2D(-this.p.speed * CupheadTime.Delta * base.transform.localScale.x * this.hitPauseCoefficient());
			if ((base.transform.localScale.x > 0f && base.transform.position.x <= -490f) || (base.transform.localScale.x < 0f && base.transform.position.x >= 490f))
			{
				yield return base.StartCoroutine(this.turn_cr());
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x00092B3C File Offset: 0x00090D3C
	public IEnumerator attack_cr()
	{
		this.state = BeeLevelSecurityGuard.State.Attack;
		base.animator.SetTrigger("OnAttack");
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
		yield break;
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x00092B58 File Offset: 0x00090D58
	public IEnumerator leave_cr()
	{
		LevelBossDeathExploder exploder = base.GetComponent<LevelBossDeathExploder>();
		this.state = BeeLevelSecurityGuard.State.Leaving;
		exploder.StartExplosion();
		if (base.transform.localScale.x < 0f && base.transform.position.x < 0f)
		{
			base.transform.SetScale(new float?(-1f), new float?(1f), new float?(1f));
		}
		if (base.transform.localScale.x > 0f && base.transform.position.x > 0f)
		{
			base.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		}
		base.animator.Play("Leave");
		AudioManager.Stop("bee_guard_flying_loop");
		AudioManager.Play("bee_guard_leave");
		this.emitAudioFromObject.Add("bee_guard_leave");
		this.circleCollider.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 2f);
		exploder.StopExplosions();
		AudioManager.Play("bee_guard_death");
		this.emitAudioFromObject.Add("bee_guard_death");
		bool leave = true;
		while (leave)
		{
			base.transform.AddPositionForward2D(-this.p.speed * CupheadTime.Delta * base.transform.localScale.x * this.hitPauseCoefficient());
			yield return null;
			if (base.transform.position.x > 1280f || base.transform.position.x < -1280f)
			{
				leave = false;
			}
		}
		this.state = BeeLevelSecurityGuard.State.Ready;
		yield break;
	}

	// Token: 0x0600119D RID: 4509 RVA: 0x00092B74 File Offset: 0x00090D74
	public IEnumerator turn_cr()
	{
		base.animator.Play("Turn");
		yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
		yield break;
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x0000EEFC File Offset: 0x0000D0FC
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.bombPrefab = null;
	}

	// Token: 0x04000E1D RID: 3613
	[SerializeField]
	public Transform bombRoot;

	// Token: 0x04000E1E RID: 3614
	[SerializeField]
	public BeeLevelSecurityGuardBomb bombPrefab;

	// Token: 0x04000E1F RID: 3615
	public LevelProperties.Bee.SecurityGuard p;

	// Token: 0x04000E20 RID: 3616
	public DamageReceiver damageReceiver;

	// Token: 0x04000E21 RID: 3617
	public DamageDealer damageDealer;

	// Token: 0x04000E22 RID: 3618
	public CircleCollider2D circleCollider;

	// Token: 0x02000A87 RID: 2695
	public enum State
	{
		// Token: 0x04004D65 RID: 19813
		Ready,
		// Token: 0x04004D66 RID: 19814
		Move,
		// Token: 0x04004D67 RID: 19815
		Attack,
		// Token: 0x04004D68 RID: 19816
		Leaving
	}
}
