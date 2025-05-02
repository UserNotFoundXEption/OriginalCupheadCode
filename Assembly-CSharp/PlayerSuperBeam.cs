using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000525 RID: 1317
public class PlayerSuperBeam : AbstractPlayerSuper
{
	// Token: 0x06003795 RID: 14229 RVA: 0x0002D658 File Offset: 0x0002B858
	public override void Awake()
	{
		base.Awake();
		this.damageReceivers = new List<DamageReceiver>();
	}

	// Token: 0x06003796 RID: 14230 RVA: 0x0002D66B File Offset: 0x0002B86B
	public override void StartSuper()
	{
		base.StartSuper();
		AudioManager.Play("player_super_beam_start");
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x06003797 RID: 14231 RVA: 0x00103BA0 File Offset: 0x00101DA0
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		DamageReceiver component = hit.GetComponent<DamageReceiver>();
		if (component != null)
		{
			if (this.damageReceivers.Contains(component))
			{
				return;
			}
			this.damageReceivers.Add(component);
		}
	}

	// Token: 0x06003798 RID: 14232 RVA: 0x00103BE0 File Offset: 0x00101DE0
	public void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer dealer)
	{
		Collider2D componentInChildren = receiver.GetComponentInChildren<Collider2D>();
		Vector2 vector = Vector2.zero;
		Vector2 zero = Vector2.zero;
		if (componentInChildren.GetType() == typeof(BoxCollider2D))
		{
			vector = (componentInChildren as BoxCollider2D).size;
		}
		else
		{
			if (componentInChildren.GetType() != typeof(CircleCollider2D))
			{
				return;
			}
			vector = Vector2.one * (componentInChildren as CircleCollider2D).radius;
		}
		float num = receiver.transform.position.x + Random.Range(-vector.x / 2f, vector.x / 2f);
		zero..ctor(num, base.transform.position.y + (float)Random.Range(-100, 100));
		this.hitPrefab.Create(zero);
	}

	// Token: 0x06003799 RID: 14233 RVA: 0x00103CC8 File Offset: 0x00101EC8
	public IEnumerator super_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Start", false, true);
		this.Fire();
		yield return CupheadTime.WaitForSeconds(this, WeaponProperties.LevelSuperBeam.time);
		base.animator.SetTrigger("OnEnd");
		AudioManager.Play("player_super_beam_end_ground");
		AudioManager.Stop("player_superbeam_firing_loop");
		this.EndSuper(true);
		yield break;
	}

	// Token: 0x0600379A RID: 14234 RVA: 0x00103CE4 File Offset: 0x00101EE4
	public override void Fire()
	{
		base.Fire();
		AudioManager.Play("player_superbeam_firing_loop");
		AudioManager.Play("player_superbeam_milk_explosion");
		this.damageDealer = new DamageDealer(WeaponProperties.LevelSuperBeam.damage, WeaponProperties.LevelSuperBeam.damageRate, DamageDealer.DamageSource.Super, false, true, true);
		this.damageDealer.OnDealDamage += this.OnDealDamage;
		this.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
		this.damageDealer.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
		meterScoreTracker.Add(this.damageDealer);
	}

	// Token: 0x0600379B RID: 14235 RVA: 0x0002D68A File Offset: 0x0002B88A
	public void OnEndAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600379C RID: 14236 RVA: 0x0002D697 File Offset: 0x0002B897
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.hitPrefab = null;
		this.damageReceivers.Clear();
		this.damageReceivers = null;
	}

	// Token: 0x04002CAC RID: 11436
	[Header("Effects")]
	[SerializeField]
	public Effect hitPrefab;

	// Token: 0x04002CAD RID: 11437
	public List<DamageReceiver> damageReceivers;
}
