using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001BB RID: 443
public class DevilLevelBombExplosion : Effect
{
	// Token: 0x06001507 RID: 5383 RVA: 0x00011E10 File Offset: 0x00010010
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001508 RID: 5384 RVA: 0x00011E23 File Offset: 0x00010023
	public void Start()
	{
		base.StartCoroutine(this.timer_cr());
		AudioManager.Play("bat_bomb_explo");
	}

	// Token: 0x06001509 RID: 5385 RVA: 0x00011E3C File Offset: 0x0001003C
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x00011E54 File Offset: 0x00010054
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x0009B0E4 File Offset: 0x000992E4
	public IEnumerator timer_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.loopTime);
		base.animator.SetTrigger("Continue");
		yield break;
	}

	// Token: 0x0400113A RID: 4410
	[SerializeField]
	public float loopTime;

	// Token: 0x0400113B RID: 4411
	public DamageDealer damageDealer;
}
