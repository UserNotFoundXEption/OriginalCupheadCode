using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001D0 RID: 464
public class DicePalaceBoozeLevelTumbler : DicePalaceBoozeLevelBossBase
{
	// Token: 0x060015B6 RID: 5558 RVA: 0x00012744 File Offset: 0x00010944
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x060015B7 RID: 5559 RVA: 0x0001277A File Offset: 0x0001097A
	public void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x0009CBD4 File Offset: 0x0009ADD4
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		float health = this.health;
		this.health -= info.damage;
		if (health > 0f)
		{
			Level.Current.timeline.DealDamage(Mathf.Clamp(health - this.health, 0f, health));
		}
		if (this.health < 0f && !base.isDead)
		{
			this.StartDying();
			this.TumblerDeathSFX();
		}
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x0009CC50 File Offset: 0x0009AE50
	public override void LevelInit(LevelProperties.DicePalaceBooze properties)
	{
		this.attackDelayIndex = Random.Range(0, properties.CurrentState.tumbler.beamDelayString.Split(new char[]
		{
			','
		}).Length);
		Level.Current.OnIntroEvent += this.OnIntroEnd;
		Level.Current.OnWinEvent += this.HandleDead;
		this.health = properties.CurrentState.tumbler.tumblerHP;
		AudioManager.Play("booze_tumbler_intro");
		this.emitAudioFromObject.Add("booze_tumbler_intro");
		base.LevelInit(properties);
	}

	// Token: 0x060015BA RID: 5562 RVA: 0x00012787 File Offset: 0x00010987
	public void OnIntroEnd()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x060015BB RID: 5563 RVA: 0x0009CCF0 File Offset: 0x0009AEF0
	public IEnumerator attack_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.properties.CurrentState.tumbler.beamDelayString.Split(new char[]
			{
				','
			})[this.attackDelayIndex]) - DicePalaceBoozeLevelBossBase.ATTACK_DELAY);
			base.animator.SetTrigger("OnAttack");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_Start", false, true);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.tumbler.beamWarningDuration);
			base.animator.SetTrigger("Continue");
			yield return base.animator.WaitForAnimationToStart(this, "Attack", false);
			AudioManager.Play("booze_tumbler_attack");
			this.emitAudioFromObject.Add("booze_tumbler_attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
			this.attackDelayIndex = (this.attackDelayIndex + 1) % base.properties.CurrentState.tumbler.beamDelayString.Split(new char[]
			{
				','
			}).Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060015BC RID: 5564 RVA: 0x00012796 File Offset: 0x00010996
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060015BD RID: 5565 RVA: 0x000127B4 File Offset: 0x000109B4
	public void EnableSpray()
	{
		AudioManager.Play("booze_tumbler_attack_spray");
		this.emitAudioFromObject.Add("booze_tumbler_attack_spray");
		base.animator.Play("Attack_Spray");
	}

	// Token: 0x060015BE RID: 5566 RVA: 0x000127E0 File Offset: 0x000109E0
	public void TumblerDeathSFX()
	{
		AudioManager.Play("tumbler_death_vox");
		this.emitAudioFromObject.Add("tumbler_death_vox");
	}

	// Token: 0x040011AA RID: 4522
	[SerializeField]
	public BoxCollider2D sprayCollider;

	// Token: 0x040011AB RID: 4523
	public int attackDelayIndex;

	// Token: 0x040011AC RID: 4524
	public DamageDealer damageDealer;

	// Token: 0x040011AD RID: 4525
	public DamageReceiver damageReceiver;
}
