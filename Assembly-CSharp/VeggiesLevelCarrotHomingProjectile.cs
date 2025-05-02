using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003CB RID: 971
public class VeggiesLevelCarrotHomingProjectile : HomingProjectile
{
	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06002ABA RID: 10938 RVA: 0x00023E89 File Offset: 0x00022089
	public override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06002ABB RID: 10939 RVA: 0x00023E90 File Offset: 0x00022090
	// (set) Token: 0x06002ABC RID: 10940 RVA: 0x00023E98 File Offset: 0x00022098
	public VeggiesLevelCarrotHomingProjectile.State state { get; set; }

	// Token: 0x06002ABD RID: 10941 RVA: 0x000D4B38 File Offset: 0x000D2D38
	public VeggiesLevelCarrotHomingProjectile Create(AbstractPlayerController player, VeggiesLevelCarrot parent, Vector2 pos, float speed, float rotationSpeed, float health)
	{
		VeggiesLevelCarrotHomingProjectile veggiesLevelCarrotHomingProjectile = base.Create(pos, -90f, speed, speed, rotationSpeed, this.DestroyLifetime, 0f, player) as VeggiesLevelCarrotHomingProjectile;
		veggiesLevelCarrotHomingProjectile.CollisionDeath.OnlyPlayer();
		veggiesLevelCarrotHomingProjectile.DamagesType.OnlyPlayer();
		veggiesLevelCarrotHomingProjectile.Init(parent, health);
		return veggiesLevelCarrotHomingProjectile;
	}

	// Token: 0x06002ABE RID: 10942 RVA: 0x00023EA1 File Offset: 0x000220A1
	public void LateUpdate()
	{
		this.UpdateHitBox();
	}

	// Token: 0x06002ABF RID: 10943 RVA: 0x00023EA9 File Offset: 0x000220A9
	public void UpdateHitBox()
	{
		this.hitBox.position = base.transform.position;
	}

	// Token: 0x06002AC0 RID: 10944 RVA: 0x00023EC1 File Offset: 0x000220C1
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.parent.OnDeathEvent -= this.OnDeath;
	}

	// Token: 0x06002AC1 RID: 10945 RVA: 0x00023EE0 File Offset: 0x000220E0
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002AC2 RID: 10946 RVA: 0x00023EEA File Offset: 0x000220EA
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		this.Die();
	}

	// Token: 0x06002AC3 RID: 10947 RVA: 0x000D4B8C File Offset: 0x000D2D8C
	public override void Die()
	{
		if (!base.GetComponent<Collider2D>().enabled)
		{
			return;
		}
		base.Die();
		base.animator.SetTrigger("OnDeath");
		base.GetComponent<Collider2D>().enabled = false;
		this.hitBox.gameObject.SetActive(false);
		this.StopAllCoroutines();
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(-90f));
	}

	// Token: 0x06002AC4 RID: 10948 RVA: 0x000D4C0C File Offset: 0x000D2E0C
	public void Init(VeggiesLevelCarrot parent, float health)
	{
		this.hitBox.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.parent = parent;
		this.health = health;
		parent.OnDeathEvent += this.OnDeath;
		base.transform.localScale = Vector3.one * (1f + Random.Range(0.1f, -0.1f));
	}

	// Token: 0x06002AC5 RID: 10949 RVA: 0x000D4C80 File Offset: 0x000D2E80
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f && this.state != VeggiesLevelCarrotHomingProjectile.State.Dead)
		{
			this.state = VeggiesLevelCarrotHomingProjectile.State.Dead;
			AudioManager.Play("level_veggies_carrot_projectile_death");
			this.emitAudioFromObject.Add("level_veggies_carrot_projectile_death");
			base.StartCoroutine(this.dying_cr());
		}
	}

	// Token: 0x06002AC6 RID: 10950 RVA: 0x000D4CEC File Offset: 0x000D2EEC
	public IEnumerator dying_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06002AC7 RID: 10951 RVA: 0x00023EFA File Offset: 0x000220FA
	public void OnDeath()
	{
		this.hitBox.gameObject.SetActive(false);
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.parentDied_cr());
	}

	// Token: 0x06002AC8 RID: 10952 RVA: 0x00023F26 File Offset: 0x00022126
	public void End()
	{
		this.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x06002AC9 RID: 10953 RVA: 0x000D4D08 File Offset: 0x000D2F08
	public IEnumerator parentDied_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, Random.Range(0f, 0.5f));
		this.End();
		yield break;
	}

	// Token: 0x0400239F RID: 9119
	public const float SCALE_RAND = 0.1f;

	// Token: 0x040023A1 RID: 9121
	[SerializeField]
	public Transform hitBox;

	// Token: 0x040023A2 RID: 9122
	public VeggiesLevelCarrot parent;

	// Token: 0x040023A3 RID: 9123
	public float health;

	// Token: 0x02000FE4 RID: 4068
	public enum State
	{
		// Token: 0x0400720A RID: 29194
		In,
		// Token: 0x0400720B RID: 29195
		InComplete,
		// Token: 0x0400720C RID: 29196
		Firing,
		// Token: 0x0400720D RID: 29197
		Dead
	}
}
